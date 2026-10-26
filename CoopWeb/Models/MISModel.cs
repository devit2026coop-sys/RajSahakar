using Antlr.Runtime.Tree;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Cooperative.Controllers.InspectionController;

namespace Cooperative.Models
{
    public class MISModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public MISModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MISModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        public DataTable GetAnswerofQuestionForGraph(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetChartData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetAnswerofQuestionForGraph(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetComplaintReport(ref DataPagingModel Details, ref string TotalAmount, ref string TotalCSS, int DistrictId, string SocietyId = "0", int SearchType = 1)
        {
            dt.Clear();
            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();
            try
            {
                string searchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetComplaintReportNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetComplaintReportNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                dt = objDBAccess.GetTable();
                if (SocietyId == "0")
                {
                    TotalAmount = dt.Compute("Sum(toamt)", string.Empty).ToString(); ;
                    TotalCSS = dt.Compute("Sum(Total)", string.Empty).ToString(); ;
                }
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
            return dt;
        }

        public DataTable GetOICDashbordData(int DistrictId, int ActId)
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OICDashbordData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = " GetOICDashbordData(int DistrictId, int ActId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataTable GetInspectionReportData(int FinancialYearId, int ActId, int DistrictId, int FilterId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditInspectionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, FinancialYearId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FilterId", DBFactoryParameterType.Int, FilterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetInspectionReportData(int FinancialYearId, int ActId,int DistrictId,int FilterId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        #region Get CCSComplaint Report


        public DataTable GetActionIdCCSComplaintReportCount(int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintReportCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }

        public DataTable GetCCSComplaintListSocieties(int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintListSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetCCSComplaintListSocieties(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }

        public DataTable GetActionIdCCSComplaintReport(ref DataPagingModel Details, int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {


                var CCSId = Details.SearchParameter.Where(x => x.Key == "CCSId").Select(x => x.Value).FirstOrDefault();
                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();

                if (CCSId != null)
                {
                    WhereClause += " and CCSId=" + CCSId;
                }

                if (SearchText != null)
                {
                    WhereClause += " and ComplainerName like '%" + SearchText + "%'" + "or Address like '%" + SearchText + "%'" + "or MobileNo like'%" + SearchText + "%'" + "or AmountDeposited like'%" + SearchText + "%'";
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaintList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaintList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }


        #endregion


        public DataTable GetCCSComplaintStatusReportDetails(ref DataPagingModel Details, int ActionId, string WhereClause, int MultiState, int DistrictId, int FinYear)
        {
            DataTable ds = new DataTable();
            try
            {

                WhereClause = "";
                var CCSId = Details.SearchParameter.Where(x => x.Key == "CCSId").Select(x => x.Value).FirstOrDefault();
                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();

                if (CCSId != null)
                {
                    WhereClause += " and CCSId=" + CCSId;
                }

                //if (SearchText != null)
                //{
                //    WhereClause += " and ComplainerName like '%" + SearchText + "%'" + "or Address like '%" + SearchText + "%'" + "or MobileNo like'%" + SearchText + "%'" + "or AmountDeposited like'%" + SearchText + "%'";
                //}

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaintStatusReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@MultiState", DBFactoryParameterType.Int, MultiState);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinYear);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaintStatusReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@MultiState", DBFactoryParameterType.Int, MultiState);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinYear);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetCCSComplaintStatusReportDetails(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }


        public DataSet GetComplaintReportData(int DistrictId, string SocietyId, int SearchType)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetComplaintReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
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




        #region Code Written By Jitendra Start Date 18-July-2019

        public string InsertAuditReportDashboardByExcel(AuditReportDashboardEntity obj)
        {
            string Result = "-1";
            string ExcelUploadId = "";
            string connString = "";
            try
            {
                string conString = string.Empty;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditReportDashboardExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = ExcelUploadId = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/AuditReportExcelForDashboard/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int ResultInner = 0; int Suc = 0, Unsuc = 0, Dupli = 0;
                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));

                        try
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                int YearId = obj.YearId;
                                int MonthId = obj.MonthId;
                                int DistrictId = obj.DistrictId;
                                string DivisionName = "";// Dr["संभाग"].ToString().Trim();
                                string DistrictName = Dr["नाम इकाई"].ToString().Trim();
                                string AuditableSocieties = Dr["आडिट योग्य समितियाँ"].ToString().Trim();
                                string AuditedSocietiesTillLastMonth = Dr["गत माह तक आडिटेड समितियाँ"].ToString().Trim();
                                string AuditedSocietiesCurrentMonth = Dr["माह में आडिटेड समितियाँ"].ToString().Trim();

                                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertAuditReportForDashboardByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);

                                cmd1.Parameters.AddWithValue("@YearId", YearId);
                                cmd1.Parameters.AddWithValue("@MonthId", MonthId);
                                cmd1.Parameters.AddWithValue("@DivisionName", DivisionName);
                                cmd1.Parameters.AddWithValue("@DistrictId", DistrictId);
                                cmd1.Parameters.AddWithValue("@DistrictName", DistrictName);
                                cmd1.Parameters.AddWithValue("@AuditableSocieties", AuditableSocieties);
                                cmd1.Parameters.AddWithValue("@AuditedSocietiesTillLastMonth", AuditedSocietiesTillLastMonth);
                                cmd1.Parameters.AddWithValue("@AuditedSocietiesCurrentMonth", AuditedSocietiesCurrentMonth);
                                cmd1.Parameters.AddWithValue("@ExcelUploadId", ExcelUploadId);
                                cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                cmd1.Parameters.AddWithValue("@SSOID", obj.SSOID);
                                ResultInner = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());

                                if (ResultInner > 0)
                                    Suc = Suc + 1;
                                else if (ResultInner == -2)
                                    Dupli = Dupli + 1;
                                else if (ResultInner == -1)
                                    Unsuc = Unsuc + 1;
                            }

                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAuditReportDashboardExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportDashboardExcelDataDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportDashboardExcelDetails(long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetExlAuditReportDashboardDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelAuditReportDashboardDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetExlAuditReportDashboardDetails(int ExlId, int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)
        {
            string Result = "-1";
            string ExcelUploadId = "";
            string connString = "";
            try
            {
                string conString = string.Empty;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditReportDashboardHeadWiseExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = ExcelUploadId = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/AuditReportExcelForDashboard/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int ResultInner = 0; int Suc = 0, Unsuc = 0, Dupli = 0;
                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));

                        //  objDBAccess.OpenConnection();
                        try
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                int YearId = obj.YearId;
                                int MonthId = obj.MonthId;
                                int DistrictId = obj.DistrictId;

                                string DistrictName = Dr["नाम खण्ड/इकाई"].ToString().Trim();
                                string TypeName = Dr["प्रकार"].ToString().Trim();// Dr["नाम खण्ड/इकाई"].ToString().Trim();
                                string Apex = Dr["अपेक्स"].ToString().Trim();
                                string CCB = Dr["सीसीबी"].ToString().Trim();
                                string MilkFederatrion = Dr["दुग्ध संघ"].ToString().Trim();

                                string PLDB = Dr["पीएलडीबी"].ToString().Trim();
                                string UrbanBank = Dr["अरबन बैंक"].ToString().Trim();
                                string WholeSale = Dr["होलसेल भंडार"].ToString().Trim();
                                string BuySell = Dr["क्रय विक्रय"].ToString().Trim();


                                string DistrictUnion = Dr["जिला स संघ"].ToString().Trim();
                                string HouseDevelopment = Dr["गृह निर्माण"].ToString().Trim();
                                string Village = Dr["ग्राम सेवा"].ToString().Trim();
                                string PrimaryDairy = Dr["प्राथ डेयरी"].ToString().Trim();


                                string OilSeed = Dr["तिलहन"].ToString().Trim();
                                string Salary = Dr["सैलरी"].ToString().Trim();
                                string Credit = Dr["क्रेडिट"].ToString().Trim();
                                string Women = Dr["महिला"].ToString().Trim();


                                string PrimaryStorage = Dr["प्राथ भंडार"].ToString().Trim();
                                string Industrial = Dr["औधोगिक"].ToString().Trim();
                                string Khadi = Dr["खादी"].ToString().Trim();
                                string Bunkar = Dr["बुनकर"].ToString().Trim();


                                string Agriculture = Dr["कृषि"].ToString().Trim();
                                string MultiplePurpose = Dr["बहु उद्देशीय"].ToString().Trim();
                                string Other = Dr["अन्य"].ToString().Trim();
                                string GrandTotal = "0"; //Dr["कुल योग"].ToString().Trim();

                                int _typeId = TypeName == "कुल समितियाँ" ? 1 : (TypeName == "माह तक आडिटेड समितियाँ" ? 2 : (TypeName == "माह के पश्चात आडिट से शेष समितियाँ" ? 3 : -1));

                                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertAuditReportForDashboardHeadWiseByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);

                                cmd1.Parameters.AddWithValue("@YearId", YearId);
                                cmd1.Parameters.AddWithValue("@MonthId", MonthId);
                                cmd1.Parameters.AddWithValue("@DistrictId", DistrictId);
                                cmd1.Parameters.AddWithValue("@DistrictName", DistrictName);
                                cmd1.Parameters.AddWithValue("@Apex", Apex);
                                cmd1.Parameters.AddWithValue("@CCB", CCB);
                                cmd1.Parameters.AddWithValue("@MilkFederation", MilkFederatrion);
                                cmd1.Parameters.AddWithValue("@PLDB", PLDB);
                                cmd1.Parameters.AddWithValue("@Urban", UrbanBank);
                                cmd1.Parameters.AddWithValue("@WholeSale", WholeSale);
                                cmd1.Parameters.AddWithValue("@BuySell", BuySell);
                                cmd1.Parameters.AddWithValue("@DistrictUnion", DistrictUnion);
                                cmd1.Parameters.AddWithValue("@HouseDevelopment", HouseDevelopment);
                                cmd1.Parameters.AddWithValue("@Village", Village);
                                cmd1.Parameters.AddWithValue("@PrimaryDairy", PrimaryDairy);
                                cmd1.Parameters.AddWithValue("@OilSeed", OilSeed);
                                cmd1.Parameters.AddWithValue("@Salary", Salary);
                                cmd1.Parameters.AddWithValue("@Credit", Credit);
                                cmd1.Parameters.AddWithValue("@Women", Women);
                                cmd1.Parameters.AddWithValue("@PrimaryStorage", PrimaryStorage);
                                cmd1.Parameters.AddWithValue("@Industrial", Industrial);
                                cmd1.Parameters.AddWithValue("@Khadi", Khadi);
                                cmd1.Parameters.AddWithValue("@Bunkar", Bunkar);
                                cmd1.Parameters.AddWithValue("@Agriculture", Agriculture);
                                cmd1.Parameters.AddWithValue("@Multipurpose", MultiplePurpose);
                                cmd1.Parameters.AddWithValue("@Others", Other);
                                cmd1.Parameters.AddWithValue("@ExcelUploadId", ExcelUploadId);
                                cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                cmd1.Parameters.AddWithValue("@SSOID", obj.SSOID);
                                cmd1.Parameters.AddWithValue("@Total", GrandTotal);
                                cmd1.Parameters.AddWithValue("@TypeId", _typeId);
                                ResultInner = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());

                                if (ResultInner > 0)
                                    Suc = Suc + 1;
                                else if (ResultInner == -2)
                                    Dupli = Dupli + 1;
                                else if (ResultInner == -1)
                                    Unsuc = Unsuc + 1;

                            }

                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAuditReportHeadWiseDashboardExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportDashboardHeadWiseExcelDataDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportHeadWiseDashboardExcelDetails(long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetExlAuditReportHeadWiseDashboardDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelAuditReportDashboardHeadWiseDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetExlAuditReportHeadWiseDashboardDetails(int ExlId, int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAuditReportRegionWise(int YearId, int MonthId, int Type)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportRegionWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, YearId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, MonthId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportRegionWise(int YearId, int MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable PACSReport(int LevelId, int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PACSReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable PACSReport(int LevelId, int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable FertilizerReportData(int DistrictId, int FertilizerType, string dateOn, int RetailerNatureId, int ReportType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerOpeningStockReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.VARCHAR, dateOn);
                objDBAccess.AddParameterToCommand("@IsReport", DBFactoryParameterType.Int, ReportType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FertilizerType", DBFactoryParameterType.Int, FertilizerType);
                objDBAccess.AddParameterToCommand("@NatureId", DBFactoryParameterType.Int, RetailerNatureId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        /*getReport*/
        public DataTable GodownFilterData(string key, int DistrictId, int StatusId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("districtwiseGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@key", DBFactoryParameterType.NVarchar, key);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GodownFilterDistrictData(string key, int DistrictId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("districtwiseGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@key", DBFactoryParameterType.NVarchar, key);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable RetailerTransactionYearMonthList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RetailerTransactionYearMonthList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable RetailerTransactionYearMonthList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable FertilizerRetailerTransactionReportData(int DistrictId, string YearId, int Month, int NatureId, int ReportType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerRetailerTransactionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.Int, ReportType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.VARCHAR, YearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@NatureId", DBFactoryParameterType.Int, NatureId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerRetailerTransactionReportData(int DistrictId, string YearId, int Month,int NatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetRSWC_MISReportData123(string WhereClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, "0");
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetRSWC_MISReportData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        #endregion

        public DataSet GetWarehouseManagerDashboardData(int WarehouseId)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WR_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.Int, WarehouseId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, _LoggedInUser.DivisionId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataSet GetWarehouseManagerDashboardData(int WarehouseId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataTable GetWarehouseManagerDashboardData(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WR_CommodityWiseDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetWarehouseManagerDashboardData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        // public DataSet GetSocietyDistWiseData(string ActId, int SocietyTypeId, string CreatetionDate, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)

        public DataSet GetSocietyDistWiseData(int ActId, int SocietyTypeId, long UserRoleDeptMappingId, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1, string FromDate = "", string ToDate = "", string FinancialYear = "")
        {
            //DataSet ds = new DataSet();
            //try
            //{
            //    objDBAccess.OpenConnection();
            //    SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDistWiseData", DBFactoryCommandType.PROCEDURE_COMMAND);
            //    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.VARCHAR, ActId);
            //    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
            //    objDBAccess.AddParameterToCommand("@CreatetionDate", DBFactoryParameterType.VARCHAR, CreatetionDate);
            //    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
            //    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
            //    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

            //    ds = objDBAccess.GetDataSet();
            //}
            //catch (Exception ex)
            //{
            //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId)" }, ex);
            //}
            //finally
            //{
            //    objDBAccess.CloseConnection();
            //    dt.Dispose();
            //}
            //return ds;

            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDistWiseData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, ToDate);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.VARCHAR, FinancialYear);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId,,int LevelId = 1,  int DistrictId =0, int DivisionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;

        }


        public DataTable GetDistrictWiseSocietyReport(int ActId, int SocietyTypeId, long UserRoleDeptMappingId, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)
        {

            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDistrictWiseSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                dt = objDBAccess.GetTable();


                //TotalAmount = dt.Compute("Sum(toamt)", string.Empty).ToString(); ;
                //TotalCSS = dt.Compute("Sum(Total)", string.Empty).ToString(); ;


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId,,int LevelId = 1,  int DistrictId =0, int DivisionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;

        }

        //    public DataTable GetSocietyReportData(
        //ref DataPagingModel Details,
        //int StatusId,
        //long UserRoleDeptMappingId,
        //int ActId,
        //int SocietyTypeId,
        //int DistrictId = 0,
        //int IsOnline = -1,
        //int Status_Id = 0,
        //int IsExport = 0)
        //    {
        //        DataTable ds = new DataTable();
        //        try
        //        {
        //            string whereCase = "";

        //            var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
        //            var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();

        //            string whereDate = @" and convert(date,A.ActionDate,111) ";

        //            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
        //            {
        //                string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
        //                string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
        //                whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
        //            }

        //            if (DistrictId > 0)
        //                whereCase += " and ROD.DISTRICT_ID=" + DistrictId;

        //            if (ActId > 0)
        //                whereCase += " and so.ActId=" + ActId;

        //            if (SocietyTypeId > 0)
        //                whereCase += " and so.SocietyTypeId=" + SocietyTypeId;

        //            if (Status_Id > 0)
        //                whereCase += " and A.StatusId=" + Status_Id;

        //            if (Details.SearchParameter.Any(x => x.Key == "SearchText"))
        //            {
        //                string searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
        //                whereCase += " and ( so.RegistrationNo like '%" + searchText + "%' or so.SocietyName like '%" + searchText + "%')";
        //            }

        //            objDBAccess.OpenConnection();

        //            // ✅ EXPORT MODE FIX
        //            if (IsExport == 1)
        //            {
        //                Details.PageNumber = 1;
        //                Details.PageSize = 100000;   // or any large number
        //            }
        //            else
        //            {
        //                // normal count logic
        //                if (Details.PageNumber == 1)
        //                {
        //                    SqlCommand countCmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);

        //                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
        //                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
        //                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
        //                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
        //                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
        //                    objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
        //                    objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);

        //                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //                }
        //            }

        //            // ✅ MAIN DATA CALL
        //            SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);

        //            objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
        //            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
        //            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
        //            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
        //            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
        //            objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
        //            objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);

        //            // ✅ PASS EXPORT FLAG TO SQL
        //            objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Int, IsExport);

        //            ds = objDBAccess.GetTable();
        //        }
        //        catch (Exception ex)
        //        {
        //            objILogger.ErrorHandle(new ErrorLogEntity()
        //            {
        //                PageName = "Cooperative.Models.MIS",
        //                FunctionName = "GetSocietyReportData"
        //            }, ex);
        //        }
        //        finally
        //        {
        //            objDBAccess.CloseConnection();
        //            ds.Dispose(); // ✅ FIX (you had dt.Dispose wrong)
        //        }
        //        return ds;
        //    }

        public DataTable GetSocietyReportData(ref DataPagingModel Details, int StatusId, long UserRoleDeptMappingId, int ActId, int SocietyTypeId, int DistrictId = 0, int IsOnline = -1, int Status_Id = 0, int IsExport = 0, string FinancialYear = "")
        {
            DataTable ds = new DataTable();
            try
            {
                string whereCase = "";
                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                string whereDate = @" and convert(date,A.ActionDate) ";

                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    whereCase += whereDate + " between '" + fromDate + "' and '" + toDate + "'";
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    whereCase += whereDate + " = '" + fromDate + "'";
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    whereCase += whereDate + " = '" + toDate + "'";
                }

                //if (IsOnline > 0)
                //{
                //    whereCase += " and isnull(so.IsNewRegistration,0)=" + IsOnline; 
                //}

                if (DistrictId > 0)
                {
                    whereCase += " and ROD.DISTRICT_ID=" + DistrictId;

                }
                if (ActId > 0)
                {
                    whereCase += " and so.ActId=" + ActId;

                }
                if (SocietyTypeId > 0)
                {
                    whereCase += " and so.SocietyTypeId=" + SocietyTypeId;

                }

                if (Status_Id > 0)
                {
                    whereCase += " and A.StatusId=" + Status_Id;
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                    whereCase += " and ( so.RegistrationNo like '%" + searchText + "%' or so.SocietyName like '%" + searchText + "%' or  ProceesStatusTextEn like '%" + searchText + "%' or ProceesStatusTextHi like '%" + searchText + "%' or act.ActNameEn_Short like '%" + searchText + "%' or act.ActNameHi_Short like '%" + searchText + "%'  or SocietyType.TypeameEng like '%" + searchText + "%' or SocietyType.TypeNameHin like '%" + searchText + "%' or District.DISTRICT_ENG like '%" + searchText + "%' or District.DISTRICT_MANGAL like '%" + searchText + "%')";
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
                    objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Int, IsExport);
                    objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.VARCHAR, FinancialYear);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Int, IsExport);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.VARCHAR, FinancialYear);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataSet GetWarehouseList(string WhereClause)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWarehouseList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataSet GetWarehouseList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataTable GetWarehouseRSWCList(string WhereClause)
        {
            dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, 100000);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWarehouseRSWCList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable GetWRROReport(ref DataPagingModel Details, string ReportType, int DivisionId)
        {
            string qry = "";
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

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "case when cast(rs.valid_from_date as date)>cast(cast(year(rs.valid_from_date) as varchar(5))+'-03-31' as date) and cast(rs.valid_from_date as date)<cast(cast(year(rs.valid_from_date)+1 as varchar(5))+'-04-01' as date)		then cast( year(rs.valid_from_date) as varchar(5))+'-'+right(cast( year(rs.valid_from_date)+1 as varchar(5)),2) else cast(year(rs.valid_from_date)-1 as varchar(5))+'-'+right(cast(year(rs.valid_from_date) as varchar(5)),2) end";
                        }
                        if (x.Key == "DISTRICT_ID")
                        {

                            whereCase += " and ( dst.DISTRICT_ID='" + x.Value + "'" + " or dst1.DISTRICT_ID='" + x.Value + "') ";
                        }

                        if (x.Key != "DISTRICT_ID")
                            whereCase += " and " + key + "='" + x.Value.ToString() + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRROReport(ref DataPagingModel Details, string ReportType)" + qry }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";



                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "commoditytype").Any())
                    {
                        string val = Details.SearchParameter.Where(x => x.Key == "commoditytype").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and IsKharif=" + val;
                    }

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

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "commoditytype"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "cast(FIN_YEAR_START as varchar(5))+'-'+cast(right(FIN_YEAR_END,2) as varchar(5))";
                        }

                        whereCase += " and " + key + "='" + x.Value + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetWarehouseWRReport(ref DataPagingModel Details, string ReportType, int WarehouseId)
        {

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " and wharehouse_id=" + WarehouseId;

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

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "case when cast(rs.valid_from_date as date)>cast(cast(year(rs.valid_from_date) as varchar(5))+'-03-31' as date) and cast(rs.valid_from_date as date)<cast(cast(year(rs.valid_from_date)+1 as varchar(5))+'-04-01' as date)		then cast( year(rs.valid_from_date) as varchar(5))+'-'+right(cast( year(rs.valid_from_date)+1 as varchar(5)),2) else cast(year(rs.valid_from_date)-1 as varchar(5))+'-'+right(cast(year(rs.valid_from_date) as varchar(5)),2) end";
                        }


                        whereCase += " and " + key + "='" + x.Value + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
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

                cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWRDateWiseReport(ref DataPagingModel Details)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable resultDt = new DataTable();
            try
            {
                string FromDate = "";
                string ToDate = "";
                string CommodityId = "0";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();
                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    CommodityId = Details.SearchParameter.Where(x => x.Key == "item_kind").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = FromDate;
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        FromDate = ToDate;
                    }
                    else
                    {
                        FromDate = DateTime.Now.Year + "/04/01";
                        ToDate = (DateTime.Now.Year + 1) + "/03/31";

                    }

                    //convert(date, created_date,111) between   convert(date, @FromDate,111)  and  convert(date, @ToDateTime,111) 
                }



                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRCountReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                    objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRCountReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetWRDateWiseROReport(ref DataPagingModel Details, int DivisionId)
        {

            DataTable resultDt = new DataTable();
            string qry = "";
            try
            {
                string whereClause = "";
                string FromDate = "";
                string ToDate = "";
                string CommodityId = "0";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    CommodityId = Details.SearchParameter.Where(x => x.Key == "item_kind").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = FromDate;
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        FromDate = ToDate;
                    }
                    else
                    {
                        FromDate = DateTime.Now.Year + "/04/01";
                        ToDate = (DateTime.Now.Year + 1) + "/03/31";

                    }

                    //convert(date, created_date,111) between   convert(date, @FromDate,111)  and  convert(date, @ToDateTime,111) 
                }
                else
                {
                    FromDate = DateTime.Now.Year + "/04/01";
                    ToDate = (DateTime.Now.Year + 1) + "/03/31";
                }


                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRCountROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.DateTime, FromDate);
                    objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRCountROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.DateTime, FromDate);
                objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                qry += objDBAccess.getSqlCommandText();
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRDateWiseROReport(ref DataPagingModel Details,int DistrictId)" + qry }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetWRCount(ref DataPagingModel Details, string Level)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable resultDt = new DataTable();
            try
            {

                string whereClause = "";
                string FromDate = "";
                string ToDate = "";
                string CommodityId = "0";

                if (Level == "2" || Level == "3")
                {
                    return GetWRReport(ref Details, Level == "2" ? "Warehouse" : "WR");
                }
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    CommodityId = Details.SearchParameter.Where(x => x.Key == "item_kind").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = FromDate;
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        FromDate = ToDate;
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        whereClause += " and " + key + "='" + x.Value + "'";
                    }
                    //convert(date, created_date,111) between   convert(date, @FromDate,111)  and  convert(date, @ToDateTime,111) 
                }



                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                //if (Details.PageNumber == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("RajfedGetWRCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //    //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //    //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                //    //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                //    //objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                //    objDBAccess.AddParameterToCommand("@WareCase", DBFactoryParameterType.Int, CommodityId);
                //    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //}

                //if (Details.IsAllRecords)
                //{
                //    Details.PageSize = Details.TotalRecords;
                //}

                cmd = objDBAccess.CreateCommand("RajfedGetWRCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                //objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                objDBAccess.AddParameterToCommand("@WareCase", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetWRCountRO(ref DataPagingModel Details, string Level, int DivisionId)
        {

            DataTable resultDt = new DataTable();
            string qry = "";
            try
            {
                string whereClause = "";
                string FromDate = "";
                string ToDate = "";
                string CommodityId = "0";

                if (Level == "2" || Level == "3")
                {
                    return GetWRROReport(ref Details, Level == "2" ? "Warehouse" : "WR", DivisionId);
                }
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    CommodityId = Details.SearchParameter.Where(x => x.Key == "item_kind").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = FromDate;
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        FromDate = ToDate;
                        whereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "cast(FIN_YEAR_START as varchar(5))+'-'+cast(right(FIN_YEAR_END,2) as varchar(5))";
                        }
                        if (x.Key == "DISTRICT_ID")
                        {

                            whereClause += " and ( dst.DISTRICT_ID='" + x.Value + "'" + " or dst1.DISTRICT_ID='" + x.Value + "') ";
                        }

                        if (x.Key != "DISTRICT_ID")
                            whereClause += " and " + key + "='" + x.Value + "'";
                    }
                    //convert(date, created_date,111) between   convert(date, @FromDate,111)  and  convert(date, @ToDateTime,111) 
                }



                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                //if (Details.PageNumber == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("RajfedGetWRCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //    //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //    //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                //    //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                //    //objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.DateTime, ToDate);
                //    objDBAccess.AddParameterToCommand("@WareCase", DBFactoryParameterType.Int, CommodityId);
                //    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //}

                //if (Details.IsAllRecords)
                //{
                //    Details.PageSize = Details.TotalRecords;
                //}

                cmd = objDBAccess.CreateCommand("RajfedGetWRCountRO", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@WareCase", DBFactoryParameterType.NVarchar, whereClause);

                qry = objDBAccess.getSqlCommandText();
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = " public DataTable GetWRCountRO(ref DataPagingModel Details, string Level, int DistrictId)" + qry }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetCCBInvestmentMISReport(int DistrictId, int DivisionId)
        {

            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCCBInvestmentMISReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetCCBInvestmentMISReport(int DistrictId,int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetCertificateforVerify(string RegistrationNo, string Type = "")
        {

            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCertificateforVerify", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCertificateforVerify(" + RegistrationNo + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetAdministrativeOfficers(string SocietyId)
        {

            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetAdministrativeOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDInc", DBFactoryParameterType.NVarchar, SocietyId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetAdministrativeOfficers(" + SocietyId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetGeneratedCertificate()
        {

            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetGeneratedCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCertificateforVerify()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        #region GetActionIdCCSComplaintReport
        public DataSet GetActionIdCCSComplaintReport(int ActionId)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }

        public DataSet GetPropertyReport(int SID)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("[GetAllPropertyDetails]", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, SID);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }

        #endregion


        #region Digital Register
        public DataTable GetDigitalRegister(ref DataPagingModel Details)
        {
            DataTable ds = new DataTable();
            try
            {
                string WhereClause = "";


                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                string FromDate = "";
                string ToDate = "";
                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();

                string whereDate = @" and   convert(date,ActionDate,111) ";
                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    ToDate = FromDate;
                    WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    FromDate = ToDate;
                    WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                }

                if (SearchText != null)
                {
                    WhereClause += " and (D.RegistrationNo like '%" + SearchText + "%' or ssoid like '%" + SearchText + "%' or D.SocietyName like '%" + SearchText + "%' or TypeameEng like '%" + SearchText + "%' or TypeNameHin like '%" + SearchText + "%')";
                }

                string month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                string year = Details.SearchParameter.Where(x => x.Key == "Year").Select(x => x.Value).FirstOrDefault();
                string actId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                string actTypeid = Details.SearchParameter.Where(x => x.Key == "SocietyType").Select(x => x.Value).FirstOrDefault();
                string district = Details.SearchParameter.Where(x => x.Key == "district_id").Select(x => x.Value).FirstOrDefault();
                //if (!string.IsNullOrWhiteSpace(month))
                //{
                //    WhereClause += " and DATEPART(MONTH,P.ActionDate) in(" + month + ") ";
                //}
                //if (!string.IsNullOrWhiteSpace(year))
                //{
                //    WhereClause += " and DATEPART(YEAR,P.ActionDate) in(" + year + ") ";
                //}
                if (!string.IsNullOrWhiteSpace(actId))
                {
                    WhereClause += " and  D.ActId in(" + actId + ")";
                }
                if (!string.IsNullOrWhiteSpace(actTypeid))
                {
                    WhereClause += " and D.SocietyTypeId in(" + actTypeid + ")";
                }
                if (!string.IsNullOrWhiteSpace(district))
                {
                    WhereClause += " and MD.DISTRICT_Id in(" + district + ")";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyRegister", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyRegister", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetDigitalRegister(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }

        #endregion


        #region Monthly Summary
        public DataTable GetMonthlySummary(ref DataPagingModel Details)
        {
            DataTable ds = new DataTable();
            try
            {
                string whereClause = " ";
                string month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                string year = Details.SearchParameter.Where(x => x.Key == "Year").Select(x => x.Value).FirstOrDefault();
                string actId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                string actTypeid = Details.SearchParameter.Where(x => x.Key == "ActType").Select(x => x.Value).FirstOrDefault();
                string district = Details.SearchParameter.Where(x => x.Key == "district_id").Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(month))
                {
                    whereClause += " and DATEPART(MONTH,P.ActionDate) in(" + month + ") ";
                }
                if (!string.IsNullOrWhiteSpace(year))
                {
                    whereClause += " and DATEPART(YEAR,P.ActionDate) in(" + year + ") ";
                }
                if (!string.IsNullOrWhiteSpace(actId))
                {
                    whereClause += " and  D.ActId in(" + actId + ")";
                }
                if (!string.IsNullOrWhiteSpace(actTypeid))
                {
                    whereClause += " and D.SocietyTypeId in(" + actTypeid + ")";
                }
                if (!string.IsNullOrWhiteSpace(district))
                {
                    whereClause += " and MD.DISTRICT_Id in(" + district + ")";
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetMonthlySummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereClause);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetMonthlySummary(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }


        #endregion

        #region Society Analytics Report

        public DataSet GetSocietySatausAnalytics(ref DataPagingModel Details, int ActId = 0, int StatusId = 0, int DistrictId = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                string whereClause = " ";
                string month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                string year = Details.SearchParameter.Where(x => x.Key == "Year").Select(x => x.Value).FirstOrDefault();
                string actId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                string actTypeid = Details.SearchParameter.Where(x => x.Key == "ActType").Select(x => x.Value).FirstOrDefault();
                string district = Details.SearchParameter.Where(x => x.Key == "district_id").Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(month))
                {
                    whereClause += " and DATEPART(MONTH,P.ActionDate) in(" + month + ") ";
                }
                if (!string.IsNullOrWhiteSpace(year))
                {
                    whereClause += " and DATEPART(YEAR,P.ActionDate) in(" + year + ") ";
                }
                if (!string.IsNullOrWhiteSpace(actId))
                {
                    whereClause += " and  D.ActId in(" + actId + ")";
                }
                if (!string.IsNullOrWhiteSpace(actTypeid))
                {
                    whereClause += " and D.SocietyTypeId in(" + actTypeid + ")";
                }
                if (!string.IsNullOrWhiteSpace(district))
                {
                    whereClause += " and MD.DISTRICT_Id in(" + district + ")";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetSocietySatausAnalytics", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@whereClause", DBFactoryParameterType.NVarchar, whereClause);


                //objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereClause);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                ds = objDBAccess.GetDataSet();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetSocietySatausAnalytics(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }


        #endregion

        #region Bulk Insert By Excel Report

        public DataTable GetSocityExcelDetails(string fromDate, string toDate)
        {
            DataTable ds = new DataTable();
            try
            {
                string FromDate = "";
                string ToDate = "";
                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    ToDate = FromDate;
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    FromDate = ToDate;
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetSocityExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, FromDate);
                objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.VARCHAR, ToDate);
                //objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereClause);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetSocietySatausAnalytics(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }

        #endregion

        #region Society Audit District Wise Report

        public DataTable GetSocietyAuditDistrictWise(string FinancialYear, int ActId)
        {
            DataTable ds = new DataTable();
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetSocietyAuditDistrictWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.VARCHAR, FinancialYear);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                //objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereClause);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetSocietyAuditDistrictWise(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }

        #endregion

        #region Proffiling Audit Status Reprt

        public DataTable GetProffilingAndauditStatus(ref DataPagingModel Details)
        {
            DataTable ds = new DataTable();
            try
            {
                string WhereClause = "";

                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();

                if (SearchText != null)
                {
                    WhereClause += " and (SS.RegistrationNo like '%" + SearchText + "%' or ssoid like '%" + SearchText + "%' or SS.SocietyName like '%" + SearchText + "%' or TypeameEng like '%" + SearchText + "%' or TypeNameHin like '%" + SearchText + "%')";
                }

                string SocietyID = Details.SearchParameter.Where(x => x.Key == "SocietyID").Select(x => x.Value).FirstOrDefault();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetProffilingAndauditStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, SocietyID);
                    objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@OrgId", DBFactoryParameterType.Int, _LoggedInUser.OrganizationId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetProffilingAndauditStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, SocietyID);
                objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@OrgId", DBFactoryParameterType.Int, _LoggedInUser.OrganizationId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetProffilingAndauditStatus(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }
        #endregion


        #region Get Complain Under Section Count Report

        public DataTable GetComplainUnderSection(int DistrictId)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetComplainUnderSection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetComplainUnderSection(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }
        #endregion

        public DataSet GetNewDashboard(int DivisionId, int DistrictId, int FYID)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetNewDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, FYID);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetNewDashboard(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }




        #region  BALWANT 
        #region PLDB Reports
        public DataTable GetLoanApplicationListReport(int PldbId, int BranchId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBLoanAppReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, PldbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public DataTable GetUploadFileType(string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMemberAmountReport(int PldbId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PLDBMemberAmmountReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, PldbId);
                // objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public DataTable GetUploadFileType(string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable PLDBLoanAmmountReportdata(int PldbId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetLoanAmtData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, PldbId);
                // objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public DataTable GetUploadFileType(string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion
        #region wramtreport
        public DataTable wramtreport(string Fy, string CommodityType, int WharehousId, string fromdatId, string TodatId)
        {
            DataTable dt = new DataTable();
            try
            {
                string date = "";
                if (!string.IsNullOrWhiteSpace(fromdatId))
                {
                    date = fromdatId.Split('/')[2] + "/" + fromdatId.Split('/')[1] + "/" + fromdatId.Split('/')[0];
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllWrAmtDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinacialYr", DBFactoryParameterType.NVarchar, Fy);
                objDBAccess.AddParameterToCommand("@WharehouseId", DBFactoryParameterType.Int, WharehousId);
                objDBAccess.AddParameterToCommand("@CommodityType", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(CommodityType) ? "" : CommodityType);
                objDBAccess.AddParameterToCommand("@Fromdate", DBFactoryParameterType.NVarchar, date);
                objDBAccess.AddParameterToCommand("@todate", DBFactoryParameterType.NVarchar, TodatId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion
        #region WR Physical Quantity Report
        public DataTable GetWRPhysicalQuantityReport(string Fy, string CommodityType, int WharehousId, string fromdatId, string TodatId)
        {
            DataTable dt = new DataTable();
            try
            {
                string date = "";
                if (!string.IsNullOrWhiteSpace(fromdatId))
                {
                    date = fromdatId.Split('/')[2] + "/" + fromdatId.Split('/')[1] + "/" + fromdatId.Split('/')[0];
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWRPhysicalQuantityReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinacialYr", DBFactoryParameterType.NVarchar, Fy);
                objDBAccess.AddParameterToCommand("@WharehouseId", DBFactoryParameterType.Int, WharehousId);
                objDBAccess.AddParameterToCommand("@CommodityType", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(CommodityType) ? "" : CommodityType);
                objDBAccess.AddParameterToCommand("@Fromdate", DBFactoryParameterType.NVarchar, date);
                objDBAccess.AddParameterToCommand("@todate", DBFactoryParameterType.NVarchar, TodatId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion
        #endregion

        //GetCooperativeReport BHOOPENDRA

        public DataSet GetCooperativeReport(int ActId, long UserRoleDeptMappingId, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)
        {
            DataSet resultDt = new DataSet();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCooperativeSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                resultDt = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }
        public DataTable GetSocietyRegistrationTimeSheet(ref DataPagingModel Details, int Type)
        {
            DataSet ds = new DataSet();
            DataTable resultDt = new DataTable();
            try
            {
                string whereClause = "";
                string SearchText = "";
                string ToDate = "";
                string CommodityId = "0";
                if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                {
                    whereClause = " and DISTRICT_ID=" + Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyRegistrationTimeSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyRegistrationTimeSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetSocietyRegistrationTimeSheet(ref DataPagingModel Details,int DistrictId)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public DataTable GetAuditMPRMonthWise(ref DataPagingModel Details, int Type)
        {
            DataSet ds = new DataSet();
            DataTable resultDt = new DataTable();
            try
            {
                string whereClause = "";
                string Year = "";
                string Month = "";
                string DistrictId = "0";
                string GroupBy = "1";
                string DivisionId = "0";
                if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                {
                    DistrictId = Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "DIVISION_ID").Any())
                {
                    DivisionId = Details.SearchParameter.Where(x => x.Key == "DIVISION_ID").Select(x => x.Value).FirstOrDefault();
                }


                if (Details.SearchParameter.Where(x => x.Key == "GroupBy").Any())
                {
                    GroupBy = Details.SearchParameter.Where(x => x.Key == "GroupBy").Select(x => x.Value).FirstOrDefault();
                }


                if (Details.SearchParameter.Where(x => x.Key == "Year").Any())
                {
                    Year = Details.SearchParameter.Where(x => x.Key == "Year").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "Month").Any())
                {
                    Month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetAuditMPRMonthWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                    objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, Year);
                    objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                    objDBAccess.AddParameterToCommand("@GroupBy", DBFactoryParameterType.Int, GroupBy);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetAuditMPRMonthWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, Year);
                objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@GroupBy", DBFactoryParameterType.Int, GroupBy);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetSocietyRegistrationTimeSheet(ref DataPagingModel Details,int DistrictId)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }
        public DataTable GetActionIdCCSComplaintReportCountByDistrict()
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GETCCSCompaintDataReportDistrictWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }
        public DataTable GetAuditWorkSheetReport(ref DataPagingModel Details, int Type)
        {
            DataSet ds = new DataSet();
            DataTable resultDt = new DataTable();
            try
            {
                string whereClause = "";
                string Year = "";
                string Month = "";
                string DistrictId = "0";
                string GroupBy = "1";
                string DivisionId = "0";
                if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                {
                    DistrictId = Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "DIVISION_ID").Any())
                {
                    DivisionId = Details.SearchParameter.Where(x => x.Key == "DIVISION_ID").Select(x => x.Value).FirstOrDefault();
                }


                if (Details.SearchParameter.Where(x => x.Key == "GroupBy").Any())
                {
                    GroupBy = Details.SearchParameter.Where(x => x.Key == "GroupBy").Select(x => x.Value).FirstOrDefault();
                }


                if (Details.SearchParameter.Where(x => x.Key == "Year").Any())
                {
                    Year = Details.SearchParameter.Where(x => x.Key == "Year").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "Month").Any())
                {
                    Month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetAuditWorkSheetReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                    objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    //objDBAccess.AddParameterToCommand("@GroupBy", DBFactoryParameterType.Int, GroupBy);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetAuditWorkSheetReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                //objDBAccess.AddParameterToCommand("@GroupBy", DBFactoryParameterType.Int, GroupBy);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetSocietyRegistrationTimeSheet(ref DataPagingModel Details,int DistrictId)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }


        public DataTable GetPLDBMemberShipReport(ref DataPagingModel Details, string ReportType)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            if (x.Value != "" && x.Value != "0")
                            {
                                whereCase += " and  DATEPART(year,CreatedDate)='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "ddlPldbId")
                        {
                            if (x.Value != "" && x.Value != "0")
                            {
                                whereCase += " and  PLDBData.PldbId='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "MY")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  DATEPART(month,CreatedDate)='" + x.Value + "'";
                            }

                        }
                        if (x.Key == "Districtdll")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  PLDBData.DistrictId='" + x.Value + "'";
                            }

                        }

                        if (x.Key == "ddlCategory")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  SMR.CategoryId='" + x.Value + "'";
                            }

                        }

                        if (x.Key == "ddlAge")
                        {

                            if (x.Value != "0")
                            {
                                whereCase += " and  SMR.Age between " + x.Value.Split('-')[0] + " and " + x.Value.Split('-')[1] + "";
                            }

                        }
                        if (x.Key == "ddlbranch")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  SMR.BranchId='" + x.Value + "'";
                            }

                        }
                        if (x.Key == "blockdll")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  SMR.BLOCK_ID='" + x.Value + "'";
                            }

                        }


                    }
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBMemberShipReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
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

                cmd = objDBAccess.CreateCommand("GetPLDBMemberShipReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetPLDBMemberShipReportbycount(ref DataPagingModel Details, string ReportType, string Value)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();



                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;

                        if (x.Key == "ddlPldbId")
                        {
                            if (x.Value != "" && x.Value != "0")
                            {
                                whereCase += " and  PLDBData.PldbId='" + x.Value + "'";
                            }
                        }

                        if (ReportType == "FY")
                        {
                            whereCase += " and  DATEPART(year,CreatedDate)='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "fy")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  DATEPART(year,CreatedDate)='" + x.Value + "'";
                                }
                            }

                        }


                        if (ReportType == "MW")
                        {
                            whereCase += " and  DATEPART(month,CreatedDate)='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "MY")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  DATEPART(month,CreatedDate)='" + x.Value + "'";
                                }
                            }

                        }

                        if (ReportType == "DW")
                        {
                            whereCase += " and  PLDBData.DistrictId='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "Districtdll")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  PLDBData.DistrictId='" + x.Value + "'";
                                }
                            }

                        }

                        if (ReportType == "BW")
                        {
                            whereCase += " and  SMR.BLOCK_ID='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "blockdll")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  SMR.BLOCK_ID='" + x.Value + "'";
                                }
                            }

                        }

                        if (ReportType == "CW")
                        {
                            whereCase += " and  SMR.CategoryId='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "ddlCategory")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  SMR.CategoryId='" + x.Value + "'";
                                }
                            }

                        }
                        if (ReportType == "AW")
                        {
                            whereCase += " and  SMR.Age between " + Value.Split('-')[0] + " and " + Value.Split('-')[1] + " ";
                        }
                        else
                        {
                            if (x.Key == "ddlAge")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  SMR.Age between " + x.Value.Split('-')[0] + " and " + x.Value.Split('-')[1] + "";
                                }
                            }

                        }

                        if (ReportType == "BR")
                        {
                            whereCase += " and  SMR.BranchId='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "ddlbranch")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  SMR.BranchId='" + x.Value + "'";
                                }
                            }

                        }
                        //else
                        //{
                        //    if (x.Value!="")
                        //    {
                        //        whereCase += " and  SMR.Age='" + x.Value + "'";
                        //    }
                        //}
                    }
                    if (ReportType == "TMFW")
                    {
                        whereCase += " and  SMR.MembershipFee='" + Value + "'";
                    }


                    if (ReportType == "STW")
                    {
                        whereCase += " and  SMR.ActionStatus='" + Value + "'";
                    }
                }
                else
                {
                    if (ReportType == "FY")
                    {
                        string currentYear = DateTime.Now.Year.ToString();
                        whereCase += " and  DATEPART(year,SMR.CreatedDate)='" + Value + "'";
                    }
                    if (ReportType == "MW")
                    {
                        string currentYear = DateTime.Now.Year.ToString();
                        whereCase += " and  DATEPART(year,CreatedDate)='" + currentYear + "' and DATEPART(month,CreatedDate)='" + Value + "'";
                    }
                    if (ReportType == "BW")
                    {
                        whereCase += " and  SMR.BLOCK_ID='" + Value + "'";
                    }
                    if (ReportType == "DW")
                    {
                        whereCase += " and  PLDBData.DistrictId='" + Value + "'";
                    }
                    if (ReportType == "CW")
                    {
                        whereCase += " and  SMR.categoryId='" + Value + "'";
                    }
                    if (ReportType == "AW")
                    {
                        whereCase += " and  SMR.Age between " + Value.Split('-')[0] + " and " + Value.Split('-')[1] + "";
                    }
                    if (ReportType == "BR")
                    {
                        whereCase += " and  SMR.BranchId='" + Value + "'";
                    }
                    if (ReportType == "TMFW")
                    {
                        whereCase += " and  SMR.MembershipFee='" + Value + "'";
                    }
                    if (ReportType == "STW")
                    {
                        whereCase += " and  SMR.ActionStatus='" + Value + "'";
                    }

                }

                //string currentYear = DateTime.Now.Year.ToString();
                //whereCase += " and  DATEPART(year,CreatedDate)='" + currentYear + "'";
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBMemberShipReportbycount", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
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

                cmd = objDBAccess.CreateCommand("GetPLDBMemberShipReportbycount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetPLDBappLoanReportList(ref DataPagingModel Details, string ReportType)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {
                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            if (x.Value != "")
                            {
                                whereCase += " and  DATEPART(year,ap.created_date)='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "MY")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  DATEPART(month,ap.created_date)='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "Districtdll")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  pd.DistrictId='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "blockdll")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  SR.BLOCK_ID='" + x.Value + "'";
                            }
                        }

                        if (x.Key == "ddlPldbId")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  pd.PldbId='" + x.Value + "'";
                            }
                        }
                        if (x.Key == "ddlbranch")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  th.BranchId='" + x.Value + "'";
                            }

                        }
                        if (x.Key == "ddlCategory")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and  (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.CategoryId ='" + x.Value + "')>0";
                            }

                        }
                        if (x.Key == "ddlAge")
                        {
                            if (x.Value != "0")
                            {
                                whereCase += " and (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.Age between " + x.Value.Split('-')[0] + " and " + x.Value.Split('-')[1] + " )>0 ";
                            }

                        }
                    }
                }
                else
                {
                    if (ReportType == "MW")
                    {
                        string currentYear = DateTime.Now.Year.ToString();
                        whereCase += " and  DatePart(year,ap.created_date)='" + currentYear + "'";
                    }
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
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

                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetPLDBLoanListbycount(ref DataPagingModel Details, string ReportType, string Value)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;


                        if (ReportType == "FY")
                        {
                            whereCase += " and  DATEPART(year,ap.created_date)='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "fy")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  DATEPART(year,ap.created_date)='" + x.Value + "'";
                                }
                            }

                        }

                        if (ReportType == "MW")
                        {
                            whereCase += " and  DATEPART(month,ap.created_date)='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "MY")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  DATEPART(month,ap.created_date)='" + x.Value + "'";
                                }
                            }

                        }

                        if (ReportType == "PB")
                        {
                            whereCase += " and  th.BranchId='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "ddlbranch")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += " and  th.BranchId='" + x.Value + "'";
                                }
                            }

                        }
                        if (ReportType == "PW")
                        {
                            whereCase += " and  pd.PLDBId='" + Value + "'";
                        }
                        else
                        {
                            if (x.Key == "ddlPldbId")
                            {
                                if (x.Value != "")
                                {
                                    whereCase += "and  pd.PldbId='" + x.Value + "'";
                                }
                            }

                        }

                        if (x.Key == "Districtdll")
                        {
                            if (x.Value != "")
                            {
                                whereCase += "and  pd.DistrictId='" + x.Value + "'";
                            }
                        }

                        if (x.Key == "blockdll")
                        {
                            if (x.Value != "")
                            {
                                whereCase += "and  SR.BLOCK_ID='" + x.Value + "'";
                            }
                        }


                        if (x.Key == "ddlAge")
                        {
                            if (x.Value != "")
                            {
                                whereCase += " and (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.Age between " + x.Value.Split('-')[0] + " and " + x.Value.Split('-')[1] + " )>0 ";

                                //whereCase += "and  SR.Age='" + x.Value + "'";
                            }
                        }

                        if (x.Key == "ddlCategory")
                        {
                            if (x.Value != "")
                            {
                                whereCase += " and  (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.CategoryId ='" + x.Value + "')>0";

                                // whereCase += "and  SR.CategoryId='" + x.Value + "'";
                            }
                        }

                    }


                    if (ReportType == "QA")
                    {
                        whereCase += " and  DATEPART(QUARTER,ap.created_date)='" + Value + "'";
                    }

                    if (ReportType == "PUW")
                    {
                        whereCase += " and (select  count(*) from PLDBLoanAmount a where a.loan_id=ap.loan_id and loan_purpose_id='" + Value + "')>0";
                        //whereCase += " and  ap.purpose_id='" + Value + "'";
                    }
                    if (ReportType == "SW")
                    {
                        whereCase += " and (select count(*) from PLDBLoanSector pls where pls.sector_id=(select ps.sector_id from PLDBLoanScheme ps where ps.scheme_id=(select top 1 p.scheme_id from PLDBLoanPurpose p where p.purpose_id in(select top 1 a.loan_purpose_id from PLDBLoanAmount a where a.loan_id=ap.loan_id order by loan_amount_id desc)) and  sector_id='" + Value + "'))>0";

                    }

                    if (ReportType == "SEMW")
                    {
                        whereCase += " and (select count(*) from PLDBLoanScheme ps where ps.scheme_id=(select top 1p.scheme_id from PLDBLoanPurpose p where p.purpose_id in(select top 1 a.loan_purpose_id from PLDBLoanAmount a where a.loan_id=ap.loan_id order by loan_amount_id desc) and  p.scheme_id='" + Value + "'))>0 ";

                        //whereCase += " and  m.scheme_id='" + Value + "'";
                    }

                    if (ReportType == "STW")
                    {
                        whereCase += " and  SR.ActionStatus='" + Value + "'";
                    }



                }
                else
                {
                    if (ReportType == "FY")
                    {
                        whereCase += " and  DATEPART(year,ap.created_date)='" + Value + "'";
                    }
                    if (ReportType == "MW")
                    {
                        string currentYear = DateTime.Now.Year.ToString();
                        whereCase += " and  DATEPART(year,ap.created_date)='" + currentYear + "' and DATEPART(month,ap.created_date)='" + Value + "'";
                    }



                    if (ReportType == "QA")
                    {
                        whereCase += " and  DATEPART(QUARTER,ap.created_date)='" + Value + "'";
                    }
                    if (ReportType == "SW")//sector
                    {
                        whereCase += " and (select count(*) from PLDBLoanSector pls where pls.sector_id=(select ps.sector_id from PLDBLoanScheme ps where ps.scheme_id=(select top 1 p.scheme_id from PLDBLoanPurpose p where p.purpose_id in(select top 1 a.loan_purpose_id from PLDBLoanAmount a where a.loan_id=ap.loan_id order by loan_amount_id desc)) and  sector_id='" + Value + "'))>0";

                        //whereCase += " and  se.sector_id='" + Value + "'";
                    }
                    if (ReportType == "SEMW")//secheme
                    {
                        whereCase += " and (select count(*) from PLDBLoanScheme ps where ps.scheme_id=(select top 1p.scheme_id from PLDBLoanPurpose p where p.purpose_id in(select top 1 a.loan_purpose_id from PLDBLoanAmount a where a.loan_id=ap.loan_id order by loan_amount_id desc) and  p.scheme_id='" + Value + "'))>0 ";
                    }
                    if (ReportType == "PUW")
                    {

                        whereCase += " and (select  count(*) from PLDBLoanAmount a where a.loan_id=ap.loan_id and loan_purpose_id='" + Value + "')>0";
                    }


                    if (ReportType == "BW")
                    {
                        whereCase += " and  SR.BLOCK_ID='" + Value + "'";
                    }
                    if (ReportType == "DW")
                    {
                        whereCase += " and  pd.DistrictId='" + Value + "'";
                    }
                    if (ReportType == "CW")
                    {
                        whereCase += " and  (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.CategoryId ='" + Value + "')>0";
                        //whereCase += " and  SR.categoryId='" + Value + "'";
                    }
                    if (ReportType == "AW")
                    {
                        whereCase += " and (select count(*)  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id and s.Age between " + Value.Split('-')[0] + " and " + Value.Split('-')[1] + " )>0 ";

                        //whereCase += " and  SR.Age='" + Value + "'";
                    }
                    if (ReportType == "PB")
                    {
                        whereCase += " and  ap.branch_id='" + Value + "'";
                    }
                    if (ReportType == "PW")
                    {
                        whereCase += " and  ap.pldb_id='" + Value + "'";
                    }
                    if (ReportType == "STW")
                    {
                        whereCase += " and  SR.ActionStatus='" + Value + "'";
                    }

                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBLoanAppCountbylist", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
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

                cmd = objDBAccess.CreateCommand("GetPLDBLoanAppCountbylist", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetRSWC_MISReportData(string WhereClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, "0");
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetRSWC_MISReportData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

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



        public DataTable GetCCSComplaintStatusReport(int DistrictId, int FinYear)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintStatusReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinYear);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetCCSComplaintStatusReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }



        public DataTable GetHousingSociety(ref DataPagingModel Details)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();



                string whereCase = "";
                string ApplicationStatus = "";
                string DistrictId = "0";

                if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                {
                    DistrictId = " and ROD.DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                }



                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    whereCase = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    // whereCase += " and (replace(S.SocietyName,'',' ') like '%" + Value + "%' or replace(S.RegistrationNo,' ','')  like '%" + Value + "%' or replace(F.Remark,' ','')  like '%" + Value + "%' or replace(P.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(P.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }




                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetHousingSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());



                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }



                cmd = objDBAccess.CreateCommand("GetHousingSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
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

        public DataTable GetHousingSocietyAudtiReport(string Id)
        {
            dt.Clear();
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetHousingSocietyAudtiReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@MappginId", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, Id);
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

        public DataTable GetSocietyDistrictWise()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDistrictWiseData", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyReport(ref DataPagingModel Details)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();



                string whereCase = "";


                string SocietyTypeId = "";
                string ClassId = "";
                string SubClassId = "";
                string SocietyCategoryId = "";
                string IsActive = "";
                string ActId = "";
                string IsNewRegistration = "";
                string IsAmendment = "";
                string ApplicationStatus = "";
                string IsRural = "";
                string HouseNo = "";
                string LaneStreetName = "";
                string Locality = "";
                string STATE_ID = "";
                string DISTRICT_ID = "";
                string CITY_ID = "";
                string BLOCK_ID = "";
                string WARD_ID = "";
                string GP_ID = "";
                string VILLAGE_ID = "";
                string PinCode = "";


                if (Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").Any())
                {
                    SocietyTypeId = Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").Select(x => x.Value).FirstOrDefault();

                }
                if (Details.SearchParameter.Where(x => x.Key == "ClassId").Any())
                {
                    ClassId = Details.SearchParameter.Where(x => x.Key == "ClassId").Select(x => x.Value).FirstOrDefault();

                }

                if (Details.SearchParameter.Where(x => x.Key == "SubClassId").Any())
                {
                    SubClassId = Details.SearchParameter.Where(x => x.Key == "SubClassId").Select(x => x.Value).FirstOrDefault();

                }

                if (Details.SearchParameter.Where(x => x.Key == "SocietyCategoryId").Any())
                {
                    SocietyCategoryId = Details.SearchParameter.Where(x => x.Key == "SocietyCategoryId").Select(x => x.Value).FirstOrDefault();

                }

                if (Details.SearchParameter.Where(x => x.Key == "IsActive").Any())
                {
                    IsActive = "1";// Details.SearchParameter.Where(x => x.Key == "IsActive").Select(x => x.Value).FirstOrDefault();

                }
                if (Details.SearchParameter.Where(x => x.Key == "ActId").Any())
                {
                    ActId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();

                }
                if (Details.SearchParameter.Where(x => x.Key.Contains("IsNewRegistration")).Any())
                {
                    //IsNewRegistration = Details.SearchParameter.Where(x => x.Key.Contains("IsNewRegistration")).Select(x => x.Value).FirstOrDefault();
                    IsNewRegistration = "1";

                }

                if (Details.SearchParameter.Where(x => x.Key.Contains("IsAmendment")).Any())
                {
                    IsAmendment = "1";// Details.SearchParameter.Where(x => x.Key.Contains("IsAmendment")).Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "IsRural").Any())
                {
                    IsRural = Details.SearchParameter.Where(x => x.Key == "IsRural").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "HouseNo").Any())
                {
                    HouseNo = Details.SearchParameter.Where(x => x.Key == "HouseNo").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "LaneStreetName").Any())
                {
                    LaneStreetName = Details.SearchParameter.Where(x => x.Key == "LaneStreetName").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "Locality").Any())
                {
                    Locality = Details.SearchParameter.Where(x => x.Key == "Locality").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "STATE_ID").Any())
                {
                    STATE_ID = Details.SearchParameter.Where(x => x.Key == "STATE_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    DISTRICT_ID = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "CITY_ID").Any())
                {
                    CITY_ID = Details.SearchParameter.Where(x => x.Key == "CITY_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "BLOCK_ID").Any())
                {
                    BLOCK_ID = Details.SearchParameter.Where(x => x.Key == "BLOCK_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "WARD_ID").Any())
                {
                    WARD_ID = Details.SearchParameter.Where(x => x.Key == "WARD_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "GP_ID").Any())
                {
                    GP_ID = Details.SearchParameter.Where(x => x.Key == "GP_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "VILLAGE_ID").Any())
                {
                    VILLAGE_ID = Details.SearchParameter.Where(x => x.Key == "VILLAGE_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "PinCode").Any())
                {
                    PinCode = Details.SearchParameter.Where(x => x.Key == "PinCode").Select(x => x.Value).FirstOrDefault();
                }









                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(S.SocietyName,'',' ') like '%" + Value + "%' or replace(S.RegistrationNo,' ','')  like '%" + Value + "%' or replace(F.Remark,' ','')  like '%" + Value + "%' or replace(P.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(P.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }




                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.NVarchar, ClassId);
                    objDBAccess.AddParameterToCommand("@SubClassId", DBFactoryParameterType.NVarchar, SubClassId);
                    objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.NVarchar, SocietyCategoryId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                    objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.NVarchar, IsNewRegistration);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
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
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("GetSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.NVarchar, ClassId);
                objDBAccess.AddParameterToCommand("@SubClassId", DBFactoryParameterType.NVarchar, SubClassId);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.NVarchar, SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.NVarchar, IsNewRegistration);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
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

        public DataTable GetAmendmentReport(ref DataPagingModel Details, int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("GetAmendmentReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAmendmentReport()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetAmendmentDetailsReport(ref DataPagingModel Details, int DistrictId, int ListType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                string SearchText = "";

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetAmendmentDetailsReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.NVarchar, ListType);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("GetAmendmentDetailsReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.NVarchar, ListType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAmendmentDetailsReport()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        #region Amit Kumar Cooperative management tabulator Report
        public DataTable GetSocietydivisionwiseMPR(string Dateval, int DIVISION_ID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativeDivisionwiseMPR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "DivisionwiseMPR");
                objDBAccess.AddParameterToCommand("@Dateval", DBFactoryParameterType.NVarchar, Dateval);
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.NVarchar, DIVISION_ID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietydivisionwiseMPR()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetSocietyDistrictwiseMPR(string Dateval, int District)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativeDistrictwiseMPR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "DistrictwiseMPR");
                objDBAccess.AddParameterToCommand("@Dateval", DBFactoryParameterType.NVarchar, Dateval);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, District);
                //objDBAccess.AddParameterToCommand("@DivisonId", DBFactoryParameterType.NVarchar, Dateval);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyDistrictwiseMPR()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetSocietyDivisionDistrictwiseMPR(int DIVISION_ID = 0, int DistrictId = 0, string Dateval = "", string Type = "")
        {
            dt.Clear();
            try
            {
                //CooperativeDivisionDistrictwiseMPR

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativeDivision_DistrictwiseMPR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Dateval", DBFactoryParameterType.NVarchar, Dateval);
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, DIVISION_ID);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyDivisionDistrictwiseMPR()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetDIVISIONList(string Dateval = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativeDistrictwiseMPR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "DivisionList");
                objDBAccess.AddParameterToCommand("@Dateval", DBFactoryParameterType.NVarchar, Dateval);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyDivisionDistrictwiseMPR()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetCooperativeMovement()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Cooperativemovement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "Cooperativemovement");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCooperativeMovement()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }



        public class CooperativeMovement
        {
            public int CoopMovementId { get; set; }
            public int ItemId { get; set; }
            public decimal ItemValue { get; set; }
            public int IsActive { get; set; }
            public List<CooperativeMovementRJ> objcooprativemovement { get; set; }
        }
        public class CooperativeMovementRJ
        {
            public int CoopMovementId { get; set; }
            public int ItemId { get; set; }
            public decimal ItemValue { get; set; }
            public int IsActive { get; set; }



        }



        public int InsertCoop(List<CooperativeMovementRJ> lstData)
        {
            int Result = 0;
            if (lstData != null && lstData.Count() > 0)
            {
                try
                {
                    foreach (var obj in lstData)
                    {
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertCooperativeMovement", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "Insert");
                        objDBAccess.AddParameterToCommand("@CoopMovementId", DBFactoryParameterType.Int, obj.CoopMovementId);
                        objDBAccess.AddParameterToCommand("@ItemId", DBFactoryParameterType.Int, obj.ItemId);
                        objDBAccess.AddParameterToCommand("@ItemValue", DBFactoryParameterType.Int, obj.ItemValue);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "InsertCoop(CooperativeMovement obj)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }



            }


            return Result;
        }



        #endregion


        #region Mukesh

        public DataTable GetWomanSavingCreditSociety(ref DataPagingModel Details, int DistrictId, string Month)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                string SearchText = "";

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("WomanSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("WomanSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAmendmentDetailsReport()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }




        #endregion

        public DataTable GetLokGuarenteeService(string Month = "", string FromMonth = "", string ToMonth = "")
        {
            DataTable ds = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetLokGuaranteeServiceWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.VARCHAR, Month);
                objDBAccess.AddParameterToCommand("@FromMonth", DBFactoryParameterType.VARCHAR, FromMonth);
                objDBAccess.AddParameterToCommand("@ToMonth", DBFactoryParameterType.VARCHAR, ToMonth);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetLokGuarenteeService(ref DataPagingModel Details)" }, ex);
            }
            return ds;
        }

        public DataTable GetAuditProgressBlock()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("Proc_GetAuditProgressBlock", DBFactoryCommandType.PROCEDURE_COMMAND);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "GetAuditProgressBlock()" }, ex);
            }

            return ds;
        }
        #region

        public DataTable GetWomenInformationSociety(int DistrictId, string Month)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("WomanInformationSocietyRPT", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetWomenInformationSociety()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetWomenInformationSocietyDetails(int ClassId, string Whereclouse = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("WomanInformationSocietyDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.NVarchar, ClassId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetWomenInformationSocietyDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        #endregion

        public DataTable CoopBankMprDataList(int DistrictId, string Month)
        {
            dt.Clear();
            try
            {



                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CoopBankMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "UploadAuditByAuditorMod(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetMovementZoneWiseMPRRpt(string year = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCoopMovmentZoneWiseRpt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year);
                // objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year.Split('-')[0]);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetCoopMovmentZoneWiseRpt(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        //public int LoanRegistration(LoanFormDataEntity _Model)
        //{
        //    int Result = 0;
        //    UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("LoanRegistrations", DBFactoryCommandType.PROCEDURE_COMMAND); objDBAccess.AddParameterToCommand("@LoanDataId", DBFactoryParameterType.Int, _Model.LoanDataId);
        //        objDBAccess.AddParameterToCommand("@AprStamt", DBFactoryParameterType.NVarchar, _Model.AprStamt);
        //        objDBAccess.AddParameterToCommand("@AprMtamt", DBFactoryParameterType.NVarchar, _Model.AprMtamt);
        //        objDBAccess.AddParameterToCommand("@Aprltamt", DBFactoryParameterType.Int, _Model.Aprltamt);
        //        objDBAccess.AddParameterToCommand("@RecStamt", DBFactoryParameterType.Int, _Model.RecStamt);
        //        objDBAccess.AddParameterToCommand("@RecMtamt", DBFactoryParameterType.Int, _Model.RecMtamt);
        //        objDBAccess.AddParameterToCommand("@Recltamt", DBFactoryParameterType.Int, _Model.Recltamt);
        //        // objDBAccess.AddParameterToCommand("@FdAmount", DBFactoryParameterType.Int, _Model.FdAmount);
        //        // objDBAccess.AddParameterToCommand("@FundSavingAccount", DBFactoryParameterType.Int, _Model.FundSavingAccount);
        //        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
        //        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
        //        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);
        //        objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, _loggedInUSerDetails.PLDBBrancchId);
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "LoanRegistrations(SocietyRegistrationEntity _Model)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}
        //public DataTable LoanMprDataList(string year)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("LoanMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, year);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "LoanMprDataList(string SocietyIdEnc)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        //Madhuri

        public int LoanRegistration(LoanFormDataEntity _Model)
        {
            int Result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoanRegistrations", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@AprStamt", DBFactoryParameterType.BigInt, _Model.AprStamt);
                objDBAccess.AddParameterToCommand("@AprMtamt", DBFactoryParameterType.BigInt, _Model.AprMtamt);
                objDBAccess.AddParameterToCommand("@Aprltamt", DBFactoryParameterType.BigInt, _Model.Aprltamt);

                objDBAccess.AddParameterToCommand("@RecStamt", DBFactoryParameterType.BigInt, _Model.RecStamt);
                objDBAccess.AddParameterToCommand("@RecMtamt", DBFactoryParameterType.BigInt, _Model.RecMtamt);
                objDBAccess.AddParameterToCommand("@Recltamt", DBFactoryParameterType.BigInt, _Model.Recltamt);

                objDBAccess.AddParameterToCommand("@loastamt", DBFactoryParameterType.BigInt, _Model.loastamt);
                objDBAccess.AddParameterToCommand("@loamtamt", DBFactoryParameterType.BigInt, _Model.loamtamt);
                objDBAccess.AddParameterToCommand("@loaltamt", DBFactoryParameterType.BigInt, _Model.loaltamt);

                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, _Model.YearName);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "LoanRegistrations(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }




        //loanMprList


        public DataTable LoanMprDataList()
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();

                if (_loggedInUSerDetails.RoleId == 10222)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("LoanMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 2);
                }
                else
                {

                    SqlCommand cmd = objDBAccess.CreateCommand("LoanMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.DistrictId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 1);

                }


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "LoanMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable AuditProgressReportBySociety(string year, string date)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditProgressReportBySociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year);
                objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.NVarchar, date);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "AuditProgressReportBySociety(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable AuditProgressReportByDepartment(string year, string date)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditProgressReportByDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year);
                objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.NVarchar, date);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "AuditProgressReportByDepartment(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable AuditFeesReportPrimaryDairy(string year, string date, int CategoryId)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditFeesReportPrimaryDairy", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year);
                objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.NVarchar, date);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.NVarchar, CategoryId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "AuditProgressReportByDepartment(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyCategory()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyCategory", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetSocietyCategory(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataSet GetCateWiseReportauditMPR(int Id, int year = 0, int month = 0)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCategoryWiseBlockAuditRpt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@year", DBFactoryParameterType.Int, year);
                objDBAccess.AddParameterToCommand("@month", DBFactoryParameterType.Int, month);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "UploadAuditByAuditorMod(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }


        //AuditProgressBlockWise
        public DataTable AuditDataList(int Id, string Month)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditProgressBlockRpt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
                //  objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "AuditProgressBlockRpt(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable getGodownlist()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWarehouseRI", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "DataTable GetSocietyOfficeProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        //Madhuri





        public DataTable GetDistrictByDivison(int DivisonId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DemoDistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);





                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisonId);





                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = " GetDistrictByDivison(int DivisonId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }






        public DataTable SocietiesDetails(int DivisonId, int DistrictId, int blockId, int CityId, int Gp, int villageId, int ActId, int RuralUrban)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietiesDetails_div_dis", DBFactoryCommandType.PROCEDURE_COMMAND);





                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisonId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@blockId", DBFactoryParameterType.Int, blockId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@Gp", DBFactoryParameterType.Int, Gp);
                objDBAccess.AddParameterToCommand("@villageId", DBFactoryParameterType.Int, villageId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@RuralUrban", DBFactoryParameterType.Int, RuralUrban);





                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = " SocietiesDetails_div_dis(int DivisonId, int DistrictId, int blockId, int CityId, int Gp,int villageId, int ActId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }







        //public DataTable AuditDataList(int Id, string Month)
        //{
        // dt.Clear();
        // try
        // {





        // objDBAccess.OpenConnection();
        // SqlCommand cmd = objDBAccess.CreateCommand("AuditProgressBlockRpt", DBFactoryCommandType.PROCEDURE_COMMAND);



        // objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
        // objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
        // dt = objDBAccess.GetTable();
        // }
        // catch (Exception ex)
        // {
        // objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "AuditProgressBlockRpt(string SocietyIdEnc)" }, ex);
        // }
        // finally
        // {
        // objDBAccess.CloseConnection();
        // }
        // return dt;
        //}


        //Madhuri 30-03-2022


        public int SaveCoopMovFormsData(CoopMovRj _Model)
        {
            int Result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CoopMovRjEntryForm", DBFactoryCommandType.PROCEDURE_COMMAND);

                //  objDBAccess.AddParameterToCommand("@RJCoopMovId"       ,    DBFactoryParameterType.Int, _Model.RJCoopMovId);
                // objDBAccess.AddParameterToCommand("@DistrictId"        ,     DBFactoryParameterType.Int,_Model. DistrictId);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, _Model.Year);
                objDBAccess.AddParameterToCommand("@CooperativeSociety", DBFactoryParameterType.Int, _Model.CooperativeSociety);
                objDBAccess.AddParameterToCommand("@Membership", DBFactoryParameterType.Int, _Model.Membership);
                objDBAccess.AddParameterToCommand("@ShareCapital", DBFactoryParameterType.Double, _Model.ShareCapital);
                objDBAccess.AddParameterToCommand("@WorkingCapital", DBFactoryParameterType.Double, _Model.WorkingCapital);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement", DBFactoryParameterType.Double, _Model.St_Loan_Advancement);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered", DBFactoryParameterType.Double, _Model.St_Loan_Recovered);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement", DBFactoryParameterType.Double, _Model.Mt_Loan_Advancement);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered", DBFactoryParameterType.Double, _Model.Mt_Loan_Recovered);
                objDBAccess.AddParameterToCommand("@Lt_Loan_Advancement", DBFactoryParameterType.Double, _Model.Lt_Loan_Advancement);
                objDBAccess.AddParameterToCommand("@Lt_Loan_Recovered", DBFactoryParameterType.Double, _Model.Lt_Loan_Recovered);
                objDBAccess.AddParameterToCommand("@Purchase", DBFactoryParameterType.Double, _Model.Purchase);
                objDBAccess.AddParameterToCommand("@Sale", DBFactoryParameterType.Double, _Model.Sale);



                // objDBAccess.AddParameterToCommand("@FdAmount", DBFactoryParameterType.Int, _Model.FdAmount);
                // objDBAccess.AddParameterToCommand("@FundSavingAccount", DBFactoryParameterType.Int, _Model.FundSavingAccount);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);




                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SaveCoopMovFormsData(CoopMovRj _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetCoopMovFormsData(int DistrictId, string Year)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDetailsCoopMovRj", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, Year);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetCoopMovFormsData(int DistrictId, string Month)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int SaveCoopState(CoopASt _Model)
        {
            int Result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StCoopBankEntryForm", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, _Model.Year);
                objDBAccess.AddParameterToCommand("@Number", DBFactoryParameterType.Int, _Model.Number);
                objDBAccess.AddParameterToCommand("@Membership_Gov", DBFactoryParameterType.Int, _Model.Membership_Gov);
                objDBAccess.AddParameterToCommand("@Membership_Soc", DBFactoryParameterType.Int, _Model.Membership_Soc);
                objDBAccess.AddParameterToCommand("@ShareCapital_Gov", DBFactoryParameterType.Double, _Model.ShareCapital_Gov);
                objDBAccess.AddParameterToCommand("@ShareCapital_Soc", DBFactoryParameterType.Double, _Model.ShareCapital_Soc);
                objDBAccess.AddParameterToCommand("@Deposit", DBFactoryParameterType.Double, _Model.Deposit);
                objDBAccess.AddParameterToCommand("@Reserve", DBFactoryParameterType.Double, _Model.Reserve);
                objDBAccess.AddParameterToCommand("@OwnedFunds", DBFactoryParameterType.Double, _Model.OwnedFunds);
                objDBAccess.AddParameterToCommand("@Borrowing", DBFactoryParameterType.Double, _Model.Borrowing);
                objDBAccess.AddParameterToCommand("@WorkingCapital", DBFactoryParameterType.Double, _Model.WorkingCapital);
                objDBAccess.AddParameterToCommand("@LoanAdvances", DBFactoryParameterType.Double, _Model.LoanAdvances);
                objDBAccess.AddParameterToCommand("@LoanRecoveries", DBFactoryParameterType.Double, _Model.LoanRecoveries);
                objDBAccess.AddParameterToCommand("@ProfitDuringTheYear", DBFactoryParameterType.Double, _Model.ProfitDuringTheYear);






                // objDBAccess.AddParameterToCommand("@FdAmount", DBFactoryParameterType.Int, _Model.FdAmount);
                // objDBAccess.AddParameterToCommand("@FundSavingAccount", DBFactoryParameterType.Int, _Model.FundSavingAccount);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);




                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SaveCoopMovFormsData(CoopMovRj _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetCoopStateData(int DistrictId, string Year)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDetailsMainCooperativeSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, Year);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetCoopMovFormsData(int DistrictId, string Month)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //MainCooperative Societies(7.3)

        public int SaveMainCoopSocieties(MainCooperativeSocieties _Model)
        {
            int Result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MainCooperativeSocietiesEntryForm", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, _Model.Year);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_Apex", DBFactoryParameterType.BigInt, _Model.NoOfSocieties_Apex);
                objDBAccess.AddParameterToCommand("@Membership_Apex", DBFactoryParameterType.BigInt, _Model.Membership_Apex);
                objDBAccess.AddParameterToCommand("@ShareCapital_Apex", DBFactoryParameterType.Double, _Model.ShareCapital_Apex);
                objDBAccess.AddParameterToCommand("@Reserve_Apex", DBFactoryParameterType.Double, _Model.Reserve_Apex);
                objDBAccess.AddParameterToCommand("@Deposits_Apex", DBFactoryParameterType.Double, _Model.Deposits_Apex);
                objDBAccess.AddParameterToCommand("@Borrowing_Apex", DBFactoryParameterType.Double, _Model.Borrowing_Apex);
                objDBAccess.AddParameterToCommand("@WorkingCapital_Apex", DBFactoryParameterType.Double, _Model.WorkingCapital_Apex);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_CCBs", DBFactoryParameterType.BigInt, _Model.NoOfSocieties_CCBs);
                objDBAccess.AddParameterToCommand("@Membership_CCBs", DBFactoryParameterType.BigInt, _Model.Membership_CCBs);
                objDBAccess.AddParameterToCommand("@ShareCapital_CCBs", DBFactoryParameterType.Double, _Model.ShareCapital_CCBs);
                objDBAccess.AddParameterToCommand("@Reserve_CCBs", DBFactoryParameterType.Double, _Model.Reserve_CCBs);
                objDBAccess.AddParameterToCommand("@Deposits_CCBs", DBFactoryParameterType.Double, _Model.Deposits_CCBs);
                objDBAccess.AddParameterToCommand("@Borrowing_CCBs", DBFactoryParameterType.Double, _Model.Borrowing_CCBs);
                objDBAccess.AddParameterToCommand("@WorkingCapital_CCBs", DBFactoryParameterType.Double, _Model.WorkingCapital_CCBs);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_UB", DBFactoryParameterType.BigInt, _Model.NoOfSocieties_UB);
                objDBAccess.AddParameterToCommand("@Membership_UB", DBFactoryParameterType.BigInt, _Model.Membership_UB);
                objDBAccess.AddParameterToCommand("@ShareCapital_UB", DBFactoryParameterType.Double, _Model.ShareCapital_UB);
                objDBAccess.AddParameterToCommand("@Reserve_UB", DBFactoryParameterType.Double, _Model.Reserve_UB);
                objDBAccess.AddParameterToCommand("@Deposits_UB", DBFactoryParameterType.Double, _Model.Deposits_UB);
                objDBAccess.AddParameterToCommand("@Borrowing_UB", DBFactoryParameterType.Double, _Model.Borrowing_UB);
                objDBAccess.AddParameterToCommand("@WorkingCapital_UB", DBFactoryParameterType.Double, _Model.WorkingCapital_UB);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_Agri", DBFactoryParameterType.BigInt, _Model.NoOfSocieties_Agri);
                objDBAccess.AddParameterToCommand("@Membership_Agri", DBFactoryParameterType.BigInt, _Model.Membership_Agri);
                objDBAccess.AddParameterToCommand("@ShareCapital_Agri", DBFactoryParameterType.Double, _Model.ShareCapital_Agri);
                objDBAccess.AddParameterToCommand("@Reserve_Agri", DBFactoryParameterType.Double, _Model.Reserve_Agri);


                objDBAccess.AddParameterToCommand("@NoOfSocieties_House", DBFactoryParameterType.Double, _Model.NoOfSocieties_House);
                objDBAccess.AddParameterToCommand("@Membership_House", DBFactoryParameterType.Double, _Model.Membership_House);
                objDBAccess.AddParameterToCommand("@ShareCapital_House", DBFactoryParameterType.Double, _Model.ShareCapital_House);
                objDBAccess.AddParameterToCommand("@Reserve_House", DBFactoryParameterType.Double, _Model.Reserve_House);
                objDBAccess.AddParameterToCommand("@Deposits_House", DBFactoryParameterType.Double, _Model.Deposits_House);
                objDBAccess.AddParameterToCommand("@Borrowing_House", DBFactoryParameterType.Double, _Model.Borrowing_House);
                objDBAccess.AddParameterToCommand("@WorkingCapital_House", DBFactoryParameterType.Double, _Model.WorkingCapital_House);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_indsociety", DBFactoryParameterType.Double, _Model.NoOfSocieties_indsociety);
                objDBAccess.AddParameterToCommand("@Membership_indsociety", DBFactoryParameterType.Double, _Model.Membership_indsociety);
                objDBAccess.AddParameterToCommand("@ShareCapital_indsociety", DBFactoryParameterType.Double, _Model.ShareCapital_indsociety);
                objDBAccess.AddParameterToCommand("@Reserve_indsociety", DBFactoryParameterType.Double, _Model.Reserve_indsociety);
                objDBAccess.AddParameterToCommand("@Deposits_indsociety", DBFactoryParameterType.Double, _Model.Deposits_indsociety);
                objDBAccess.AddParameterToCommand("@Borrowing_indsociety", DBFactoryParameterType.Double, _Model.Borrowing_indsociety);
                objDBAccess.AddParameterToCommand("@WorkingCapital_indsociety", DBFactoryParameterType.Double, _Model.WorkingCapital_indsociety);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_labourcontract", DBFactoryParameterType.Double, _Model.NoOfSocieties_labourcontract);
                objDBAccess.AddParameterToCommand("@Membership_labourcontract", DBFactoryParameterType.Double, _Model.Membership_labourcontract);
                objDBAccess.AddParameterToCommand("@ShareCapital_labourcontract", DBFactoryParameterType.Double, _Model.ShareCapital_labourcontract);
                objDBAccess.AddParameterToCommand("@Reserve_labourcontract", DBFactoryParameterType.Double, _Model.Reserve_labourcontract);
                objDBAccess.AddParameterToCommand("@Deposits_labourcontract", DBFactoryParameterType.Double, _Model.Deposits_labourcontract);
                objDBAccess.AddParameterToCommand("@Borrowing_labourcontract", DBFactoryParameterType.Double, _Model.Borrowing_labourcontract);
                objDBAccess.AddParameterToCommand("@WorkingCapital_labourcontract", DBFactoryParameterType.Double, _Model.WorkingCapital_labourcontract);


                objDBAccess.AddParameterToCommand("@NoOfSocieties_Marketing", DBFactoryParameterType.Double, _Model.NoOfSocieties_Marketing);
                objDBAccess.AddParameterToCommand("@Membership_Marketing", DBFactoryParameterType.Double, _Model.Membership_Marketing);
                objDBAccess.AddParameterToCommand("@Reserve_Marketing", DBFactoryParameterType.Double, _Model.Reserve_Marketing);
                objDBAccess.AddParameterToCommand("@Deposits_Marketing", DBFactoryParameterType.Double, _Model.Deposits_Marketing);
                objDBAccess.AddParameterToCommand("@ShareCapital_Marketing", DBFactoryParameterType.Double, _Model.ShareCapital_Marketing);
                objDBAccess.AddParameterToCommand("@Borrowing_Marketing", DBFactoryParameterType.Double, _Model.Borrowing_Marketing);
                objDBAccess.AddParameterToCommand("@WorkingCapital_Marketing", DBFactoryParameterType.Double, _Model.WorkingCapital_Marketing);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_MilkSupply", DBFactoryParameterType.Double, _Model.NoOfSocieties_MilkSupply);
                objDBAccess.AddParameterToCommand("@Membership_MilkSupply", DBFactoryParameterType.Double, _Model.Membership_MilkSupply);
                objDBAccess.AddParameterToCommand("@ShareCapital_MilkSupply", DBFactoryParameterType.Double, _Model.ShareCapital_MilkSupply);
                objDBAccess.AddParameterToCommand("@Reserve_MilkSupply", DBFactoryParameterType.Double, _Model.Reserve_MilkSupply);
                objDBAccess.AddParameterToCommand("@Deposits_MilkSupply", DBFactoryParameterType.Double, _Model.Deposits_MilkSupply);
                objDBAccess.AddParameterToCommand("@Borrowing_MilkSupply", DBFactoryParameterType.Double, _Model.Borrowing_MilkSupply);
                objDBAccess.AddParameterToCommand("@WorkingCapital_MilkSupply", DBFactoryParameterType.Double, _Model.WorkingCapital_MilkSupply);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_MilkUnion", DBFactoryParameterType.Double, _Model.NoOfSocieties_MilkUnion);
                objDBAccess.AddParameterToCommand("@Membership_MilkUnion", DBFactoryParameterType.Double, _Model.Membership_MilkUnion);
                objDBAccess.AddParameterToCommand("@ShareCapital_MilkUnion", DBFactoryParameterType.Double, _Model.ShareCapital_MilkUnion);
                objDBAccess.AddParameterToCommand("@Reserve_MilkUnion", DBFactoryParameterType.Double, _Model.Reserve_MilkUnion);
                objDBAccess.AddParameterToCommand("@Deposits_MilkUnion", DBFactoryParameterType.Double, _Model.Deposits_MilkUnion);
                objDBAccess.AddParameterToCommand("@Borrowing_MilkUnion", DBFactoryParameterType.Double, _Model.Borrowing_MilkUnion);
                objDBAccess.AddParameterToCommand("@WorkingCapital_MilkUnion", DBFactoryParameterType.Double, _Model.WorkingCapital_MilkUnion);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_Othersociety", DBFactoryParameterType.Double, _Model.NoOfSocieties_Othersociety);
                objDBAccess.AddParameterToCommand("@Membership_Othersociety", DBFactoryParameterType.Double, _Model.Membership_Othersociety);
                objDBAccess.AddParameterToCommand("@ShareCapital_Othersociety", DBFactoryParameterType.Double, _Model.ShareCapital_Othersociety);
                objDBAccess.AddParameterToCommand("@Reserve_Othersociety", DBFactoryParameterType.Double, _Model.Reserve_Othersociety);
                objDBAccess.AddParameterToCommand("@Deposits_Othersociety", DBFactoryParameterType.Double, _Model.Deposits_Othersociety);
                objDBAccess.AddParameterToCommand("@Borrowing_Othersociety", DBFactoryParameterType.Double, _Model.Borrowing_Othersociety);
                objDBAccess.AddParameterToCommand("@WorkingCapital_Othersociety", DBFactoryParameterType.Double, _Model.WorkingCapital_Othersociety);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_PrimaryBhander", DBFactoryParameterType.Double, _Model.NoOfSocieties_PrimaryBhander);
                objDBAccess.AddParameterToCommand("@MembershipPrimaryBhandery", DBFactoryParameterType.Double, _Model.MembershipPrimaryBhandery);
                objDBAccess.AddParameterToCommand("@ShareCapital_PrimaryBhander", DBFactoryParameterType.Double, _Model.ShareCapital_PrimaryBhander);
                objDBAccess.AddParameterToCommand("@Reserve_PrimaryBhander", DBFactoryParameterType.Double, _Model.Reserve_PrimaryBhander);
                objDBAccess.AddParameterToCommand("@Deposits_PrimaryBhander", DBFactoryParameterType.Double, _Model.Deposits_PrimaryBhander);
                objDBAccess.AddParameterToCommand("@Borrowing_PrimaryBhander", DBFactoryParameterType.Double, _Model.Borrowing_PrimaryBhander);
                objDBAccess.AddParameterToCommand("@WorkingCapital_PrimaryBhander", DBFactoryParameterType.Double, _Model.WorkingCapital_PrimaryBhander);

                objDBAccess.AddParameterToCommand("@NoOfSocieties_WholesaleBhander", DBFactoryParameterType.Double, _Model.NoOfSocieties_WholesaleBhander);
                objDBAccess.AddParameterToCommand("@MembershipPWholesaleBhander", DBFactoryParameterType.Double, _Model.MembershipPWholesaleBhander);
                objDBAccess.AddParameterToCommand("@ShareCapital_WholesaleBhander", DBFactoryParameterType.Double, _Model.ShareCapital_WholesaleBhander);
                objDBAccess.AddParameterToCommand("@Reserve_WholesaleBhander", DBFactoryParameterType.Double, _Model.Reserve_WholesaleBhander);
                objDBAccess.AddParameterToCommand("@Deposits_WholesaleBhander", DBFactoryParameterType.Double, _Model.Deposits_WholesaleBhander);
                objDBAccess.AddParameterToCommand("@Borrowing_WholesaleBhander", DBFactoryParameterType.Double, _Model.Borrowing_WholesaleBhander);
                objDBAccess.AddParameterToCommand("@WorkingCapital_WholesaleBhander", DBFactoryParameterType.Double, _Model.WorkingCapital_WholesaleBhander);

                objDBAccess.AddParameterToCommand("@Deposits_Agri", DBFactoryParameterType.Double, _Model.Deposits_Agri);
                objDBAccess.AddParameterToCommand("@Borrowing_Agri", DBFactoryParameterType.Double, _Model.Borrowing_Agri);
                objDBAccess.AddParameterToCommand("@WorkingCapital_Agri", DBFactoryParameterType.Double, _Model.WorkingCapital_Agri);
                objDBAccess.AddParameterToCommand("@NoOfSocieties_PLDB", DBFactoryParameterType.BigInt, _Model.NoOfSocieties_PLDB);
                objDBAccess.AddParameterToCommand("@Membership_PLDB", DBFactoryParameterType.BigInt, _Model.Membership_PLDB);
                objDBAccess.AddParameterToCommand("@ShareCapital_PLDB", DBFactoryParameterType.Double, _Model.ShareCapital_PLDB);
                objDBAccess.AddParameterToCommand("@Reserve_PLDB", DBFactoryParameterType.Double, _Model.Reserve_PLDB);
                objDBAccess.AddParameterToCommand("@Deposits_PLDB", DBFactoryParameterType.Double, _Model.Deposits_PLDB);
                objDBAccess.AddParameterToCommand("@Borrowing_PLDB", DBFactoryParameterType.Double, _Model.Borrowing_PLDB);
                objDBAccess.AddParameterToCommand("@WorkingCapital_PLDB", DBFactoryParameterType.Double, _Model.WorkingCapital_PLDB);



                // objDBAccess.AddParameterToCommand("@FdAmount", DBFactoryParameterType.Int, _Model.FdAmount);
                // objDBAccess.AddParameterToCommand("@FundSavingAccount", DBFactoryParameterType.Int, _Model.FundSavingAccount);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);




                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SaveMainCoopSocieties(MainCooperativeSocieties _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetMainCoopSocietyData(int DistrictId, string Year)
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                objDBAccess.OpenConnection();

                if (_loggedInUSerDetails.RoleId == 10222)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("MainCooperativeSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 2);
                }

                else
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("MainCooperativeSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);
                    objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, Year);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 1);
                }
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetCoopMovFormsData(int DistrictId, string Month)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //primary Agriculture Credit Societies 



        public int SavePrimaryAgriSocieties(PrimaryAgricultureCreditSocieties _Model)
        {
            int Result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PrimaryAgriCreditSocietiesForm", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Membership_SocietyNo", DBFactoryParameterType.BigInt, _Model.Membership_SocietyNo);
                objDBAccess.AddParameterToCommand("@Membership_TotalAm", DBFactoryParameterType.BigInt, _Model.Membership_TotalAm);
                objDBAccess.AddParameterToCommand("@Membership_ScNO", DBFactoryParameterType.BigInt, _Model.Membership_ScNO);
                objDBAccess.AddParameterToCommand("@Membership_ScAmount", DBFactoryParameterType.BigInt, _Model.Membership_ScAmount);
                objDBAccess.AddParameterToCommand("@Membership_StNo", DBFactoryParameterType.BigInt, _Model.Membership_StNo);
                objDBAccess.AddParameterToCommand("@Membership_StAmount", DBFactoryParameterType.BigInt, _Model.Membership_StAmount);

                objDBAccess.AddParameterToCommand("@BorrowingMember_SocietyNo", DBFactoryParameterType.BigInt, _Model.BorrowingMember_SocietyNo);
                objDBAccess.AddParameterToCommand("@BorrowingMember_TotalAm", DBFactoryParameterType.BigInt, _Model.BorrowingMember_TotalAm);
                objDBAccess.AddParameterToCommand("@BorrowingMember_ScNO", DBFactoryParameterType.BigInt, _Model.BorrowingMember_ScNO);
                objDBAccess.AddParameterToCommand("@BorrowingMember_ScAmount", DBFactoryParameterType.BigInt, _Model.BorrowingMember_ScAmount);
                objDBAccess.AddParameterToCommand("@BorrowingMember_StNo", DBFactoryParameterType.BigInt, _Model.BorrowingMember_StNo);
                objDBAccess.AddParameterToCommand("@BorrowingMember_StAmount", DBFactoryParameterType.BigInt, _Model.BorrowingMember_StAmount);

                objDBAccess.AddParameterToCommand("@MemberaIndebted_SocietyNo", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_SocietyNo);
                objDBAccess.AddParameterToCommand("@MemberaIndebted_TotalAm", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_TotalAm);
                objDBAccess.AddParameterToCommand("@MemberaIndebted_ScNO", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_ScNO);
                objDBAccess.AddParameterToCommand("@MemberaIndebted_ScAmount", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_ScAmount);
                objDBAccess.AddParameterToCommand("@MemberaIndebted_StNo", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_StNo);
                objDBAccess.AddParameterToCommand("@MemberaIndebted_StAmount", DBFactoryParameterType.BigInt, _Model.MemberaIndebted_StAmount);

                objDBAccess.AddParameterToCommand("@MemberaDefaulting_SocietyNo", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_SocietyNo);
                objDBAccess.AddParameterToCommand("@MemberaDefaulting_TotalAm", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_TotalAm);
                objDBAccess.AddParameterToCommand("@MemberaDefaulting_ScNO", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_ScNO);

                objDBAccess.AddParameterToCommand("@Fyear", DBFactoryParameterType.NVarchar, _Model.Fyear);

                objDBAccess.AddParameterToCommand("@MemberaDefaulting_ScAmount", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_ScAmount);
                objDBAccess.AddParameterToCommand("@MemberaDefaulting_StNo", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_StNo);
                objDBAccess.AddParameterToCommand("@MemberaDefaulting_StAmount", DBFactoryParameterType.BigInt, _Model.MemberaDefaulting_StAmount);

                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_SocietyNo", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_SocietyNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_TotalAm", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_TotalAm);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_ScNO", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_ScNO);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_ScAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_ScAmount);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_StNo ", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_StNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Advancement_StAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_Advancement_StAmount);

                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_SocietyNo", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_SocietyNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_TotalAm", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_TotalAm);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_ScNO", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_ScNO);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_ScAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_ScAmount);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_StNo ", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_StNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Advancement_StAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Advancement_StAmount);

                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_SocietyNo", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_SocietyNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_TotalAm", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_TotalAm);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_ScNO", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_ScNO);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_ScAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_ScAmount);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_StNo", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_StNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Recovered_StAmount ", DBFactoryParameterType.BigInt, _Model.St_Loan_Recovered_StAmount);

                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_SocietyNo", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_SocietyNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_TotalAm", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_TotalAm);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_ScNO", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_ScNO);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_ScAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_ScAmount);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_StNo", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_StNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_Recovered_StAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_Recovered_StAmount);

                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_SocietyNo", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_SocietyNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_TotalAm ", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_TotalAm);
                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_ScNO", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_ScNO);
                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_ScAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_ScAmount);
                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_StNo", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_StNo);
                objDBAccess.AddParameterToCommand("@St_Loan_Outstanding_StAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_Outstanding_StAmount);

                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_SocietyNo", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_SocietyNo);
                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_TotalAm ", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_TotalAm);
                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_ScNO", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_ScNO);
                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_ScAmount", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_ScAmount);
                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_StNo", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_StNo);
                objDBAccess.AddParameterToCommand("@MT_Loan_Outstanding_StAmount", DBFactoryParameterType.BigInt, _Model.MT_Loan_Outstanding_StAmount);


                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_SocietyNo", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_SocietyNo);
                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_TotalAm", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_TotalAm);
                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_ScNO ", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_ScNO);
                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_ScAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_ScAmount);
                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_StNo", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_StNo);
                objDBAccess.AddParameterToCommand("@St_Loan_OverDue_StAmount", DBFactoryParameterType.BigInt, _Model.St_Loan_OverDue_StAmount);

                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_SocietyNo ", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_SocietyNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_TotalAm", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_TotalAm);
                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_ScNO", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_ScNO);
                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_ScAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_ScAmount);
                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_StNo", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_StNo);
                objDBAccess.AddParameterToCommand("@Mt_Loan_OverDue_StAmount", DBFactoryParameterType.BigInt, _Model.Mt_Loan_OverDue_StAmount);

                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SavePrimaryAgriSocieties(MainCooperativeSocieties _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetPrimaryAgriSocieties(int DistrictId, string Year)
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {

                objDBAccess.OpenConnection();
                if (_loggedInUSerDetails.RoleId == 10222)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("GetPrimaryAgriCreditSoc", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 2);
                }
                else
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("[GetPrimaryAgriCreditSoc]", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _loggedInUSerDetails.DistrictId);
                    objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, Year);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, 1);
                }
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "GetCoopMovFormsData(int DistrictId, string Month)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }




        public DataTable CooperativeMovementRpt(string FYear)
        {
            dt.Clear();
            try
            {



                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativeMovementAdvanceRpt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, FYear);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "CooperativeMovementAdvanceRpt(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //Bhavesh Singh 25/03/2026
        #region 'Gcc CCP Report'
        public DataSet GetCCPGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0, DateTime? FromDate = null, DateTime? ToDate = null, string action = "", int Type = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_GccMisReportCCP", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                string finalAction = "";

                if (Type == 1)
                {
                    finalAction = "GccRenewalSummary";   // TRUE = Renew
                }
                else
                {
                    finalAction = "GccNewSummarynn";     // FALSE = New
                }

                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, finalAction);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.PMKisanModel",
                        FunctionName = "GetCCPGccSummary"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }
        //public DataSet GetCCPGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0, string FromDate = "", string ToDate = "", string action = "", bool Type = false )
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        //cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        cmd = objDBAccess.CreateCommand("P_GccMisReportCCP", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        //cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
        //        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
        //        objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
        //        objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
        //        objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
        //        objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
        //        objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(FromDate) ? (object)DBNull.Value : FromDate);

        //        objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(ToDate) ? (object)DBNull.Value : ToDate);
        //        //objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccSummaryExport"); 


        //        if (!string.IsNullOrEmpty(action))
        //        {
        //            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, action);
        //        }
        //        else
        //        {
        //            if (DistrictWise)
        //            {
        //                if (!IsOverall)
        //                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseSummary");
        //                else
        //                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseSummary");
        //            }
        //            else
        //            {
        //                if (!IsOverall)
        //                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccSummary");
        //                else
        //                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallSummary");
        //            }
        //        }

        //        // Handle the new action case
        //        //if (!string.IsNullOrEmpty(action) && action.Equals("GccNewSummary", StringComparison.OrdinalIgnoreCase))
        //        if (Type)
        //        {
        //            // New GCC Summary
        //            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccNewSummarynn");
        //            //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(FromDate) ? (object)DBNull.Value : FromDate);
        //            //objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(ToDate) ? (object)DBNull.Value : ToDate);
        //        }
        //        else
        //        {
        //            // Renewal GCC Summary
        //            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccRenewalSummary");
        //            //objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(FromDate) ? (object)DBNull.Value : FromDate);
        //            //objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, string.IsNullOrEmpty(ToDate) ? (object)DBNull.Value : ToDate);
        //        }

        //        return objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0)" }, ex);
        //    }

        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return null;
        //}

        #endregion


        #region 'Gcc Report'
        public DataSet GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                //cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //cmd = objDBAccess.CreateCommand("P_GccMisReportCCP", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);


                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseSummary");

                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallSummary");
                }


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetGccSummaryList(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0, int ApplicationType = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                //cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //cmd = objDBAccess.CreateCommand("P_GccMisReportCCP", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd = objDBAccess.CreateCommand("P_GccMisReportList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@ApplicationType", DBFactoryParameterType.Int, ApplicationType);


                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseSummary");

                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallSummary");
                }


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }



        #region 'Gcc Re Apply Report'
        public DataSet GetGccRenewSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_GccRenewMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);

                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseSummary");

                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccRenewSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallSummary");
                }

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        #endregion

        public DataSet GetGccApplicationList(ref DataPagingModel Details, int financialYearId = 0, int districtId = 0, int ccbId = 0, int branchId = 0, int pacsId = 0, int statusType = 0, bool isExport = false, bool IsOverall = false, int blockId = 0, int gpId = 0, bool DistrictWise = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_GccMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CcbId", DBFactoryParameterType.VARCHAR, ccbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.VARCHAR, branchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.VARCHAR, pacsId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.Int, statusType);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.VARCHAR, blockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.VARCHAR, gpId);
                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseDetails");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseDetails");
                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDetails");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDetails");
                }
                //dt= objDBAccess.GetDataTable();
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "public DataSet GetGccApplicationList(ref DataPagingModel Details, int financialYearId = 0,  int districtId = 0, int ccbId = 0, int branchId = 0, int pacsid = 0, bool isExport = false)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetGccMahilaNidhiSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool IsOverall = false, bool DistrictWise = false, int BlockId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_GccMahilaNidhiMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, PacsId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);

                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseSummary");

                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccSummary");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallSummary");
                }



                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetGccMahilaNidhiApplicationList(ref DataPagingModel Details, int financialYearId = 0, int districtId = 0, int ccbId = 0, int branchId = 0, int pacsId = 0, int statusType = 0, bool isExport = false, bool IsOverall = false, int blockId = 0, int gpId = 0, bool DistrictWise = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("P_GccMahilaNidhiMisReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CcbId", DBFactoryParameterType.VARCHAR, ccbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.VARCHAR, branchId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.VARCHAR, pacsId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.Int, statusType);

                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.VARCHAR, blockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.VARCHAR, gpId);


                if (DistrictWise)
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDistrictWiseDetails");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDistrictWiseDetails");
                }
                else
                {
                    if (!IsOverall)
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccDetails");
                    else
                        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccOverallDetails");
                }
                //dt= objDBAccess.GetDataTable();
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "public DataSet GetGccApplicationList(ref DataPagingModel Details, int financialYearId = 0,  int districtId = 0, int ccbId = 0, int branchId = 0, int pacsid = 0, bool isExport = false)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        #endregion

        #region BeneficiaryList
        public DataSet GetBeneficiaryList(int districtId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCMK_BeneficiarySummaryList");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetBeneficiaryList(int financialYearId = 0, int districtId = 0, string subDistrictName = \"\", int installmentType = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetBeneficiaryDetailList(ref DataPagingModel Details, int districtId = 0, bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCMK_BeneficiarySummaryDetailList");
                //dt= objDBAccess.GetDataTable();
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "public DataTable GetBeneficiaryDetailList(int financialYearId = 0, int installmentType = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        #endregion


        #region PLDB 
        public DataSet GetPldbDistrictWiseSummary(string Action = "", int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string StatusType = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                string spname = "P_PldbMisReport";

                if (Action == "PldbBranchWiseTaamilSummary")
                {
                    spname = "P_PldbBankMisReport";
                }


                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, PLDB_ID);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, PLDB_Branch_ID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, StatusType);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbDistrictWiseSummary(string Action = \"\",int financialYearId = 0, int districtId=0, int PLDB_ID = 0, int PLDB_Branch_ID = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }



        public DataSet GetPldbCallSummary(DataPagingModel Details, string Action = "", int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string StatusType = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                string spname = "P_PldbMisReport";
                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, PLDB_ID);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, PLDB_Branch_ID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, StatusType);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbCallSummary(string Action = \"\",int financialYearId = 0, int districtId=0, int PLDB_ID = 0, int PLDB_Branch_ID = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetPldbTamilSelfieSummary(string Action = "", int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string StatusType = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                string spname = "P_PldbTamilSelfieMisReport";

                //if (Action == "PldbBranchWiseTaamilSummary")
                //{
                //    spname = "P_PldbBankMisReport";
                //}


                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, PLDB_ID);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, PLDB_Branch_ID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, StatusType);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbDistrictWiseSummary(string Action = \"\",int financialYearId = 0, int districtId=0, int PLDB_ID = 0, int PLDB_Branch_ID = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        #endregion

        #region PLDBOTSSummary Added By Narpat Kumawat on 16-05-2025
        /// <summary>
        /// GetPldbOtsSummary Details
        /// </summary>
        /// <returns></returns>
        public DataSet GetPldbOtsSummary(int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "")
        {
            string action = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                string spname = "P_PldbOtsMisReport";

                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, PLDB_ID);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, PLDB_Branch_ID);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbOtsSummary(string Action = '" + action + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        /// <summary>
        /// GetPldbOtsSummaryDetails by OtsApplicationId
        /// </summary>
        /// <param name="PLDB_OTS_SchemeID"></param>
        /// <returns></returns>
        public DataSet GetPldbOtsDetailsByOtsId(int PLDB_OTS_SchemeID)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                string Action = "PldbOtsDetailsbyOtsId";

                string spname = "P_PldbOtsMisReport";

                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.Int, PLDB_OTS_SchemeID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbOtsDetailsByOtsId(string PLDB_OTS_SchemeID = '" + PLDB_OTS_SchemeID + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetPldbOtsSchemaDetails(int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "")
        {
            string action = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                string spname = "P_PldbOtsSchemaReport";

                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, PLDB_ID);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, PLDB_Branch_ID);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbOtsSummary(string Action = '" + action + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetLoaneePRReceiptDetails(string FilterDate = "", string action = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                string spname = "P_PldbLoaneeReceiptReport";
                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, action);
                objDBAccess.AddParameterToCommand("@FilterDate", DBFactoryParameterType.NVarchar, FilterDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.NVarchar, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.NVarchar, pldbBranchId);


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetLoaneePRReceiptDetails(string FilterDate = '" + FilterDate + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetLoaneeFRReceiptDetails(string FilterDate = "", string action = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                string spname = "P_PldbLoaneeFinalReceiptReport";
                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, action);
                objDBAccess.AddParameterToCommand("@FilterDate", DBFactoryParameterType.NVarchar, FilterDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.NVarchar, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.NVarchar, pldbBranchId);


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetLoaneeFRReceiptDetails(string FilterDate = '" + FilterDate + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNIActReportDetail(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("PldbNIActSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.NVarchar, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.NVarchar, pldbBranchId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetNIActReportDetail(string actionType = '" + actionType + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetMonthWiseOTSRecoveryDetail(ref DataPagingModel Details, string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0, int ConstituencyId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("NIActDetailsSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, pldbBranchId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@ConstituencyId", DBFactoryParameterType.Int, ConstituencyId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetMonthWiseOTSRecoveryDetail(ref DataPagingModel Details, string actionType = \"\", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetOTSLandMortgagedDetails(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("OTSLandMortgagedReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, pldbBranchId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetNiActDetails(string otsApplicationId, int summaryId" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetMisSectionFor138NiActDetails(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("MisSectionFor138NiActSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, pldbBranchId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetMisSectionFor138NiActDetails(string otsApplicationId, int summaryId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetOtsDailyProgressReport(OtsDprRequestModel otsDprRequest) // Added by Pradeep @2025-07-22
        {
            try
            {
                string action = "";
                if (otsDprRequest.reportNo == 1)
                    action = "Report1";
                else if (otsDprRequest.reportNo == 2)
                    action = "Report2";
                else if (otsDprRequest.reportNo == 3)
                    action = "DistrictWiseReport";
                else if (otsDprRequest.reportNo == 4)
                    action = "DistrictWisePossibleReport";
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OtsProgressReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, action);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.Int, otsDprRequest.reportType);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, otsDprRequest.pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, otsDprRequest.pldbBranchId);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, otsDprRequest.districtId);
                objDBAccess.AddParameterToCommand("@VidhanSabha_ID", DBFactoryParameterType.Int, otsDprRequest.vidhanSabhaID);
                objDBAccess.AddParameterToCommand("@VidhanSabha", DBFactoryParameterType.VARCHAR, otsDprRequest.vidhanSabha);
                objDBAccess.AddParameterToCommand("@ReportDate", DBFactoryParameterType.VARCHAR, otsDprRequest.reportDate);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetOtsDailyProgressReport(OtsDprRequestModel otsDprRequest)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetOtsDailyProgressViewDetails(OtsDprRequestModel otsDprRequest) // Added by Pradeep @2025-08-07
        {
            try
            {
                string action = "";
                if (otsDprRequest.reportNo == 1)
                    action = "Report1";
                else if (otsDprRequest.reportNo == 2)
                    action = "Report2";
                else if (otsDprRequest.reportNo == 3)
                    action = "DistrictWiseReport";
                else if (otsDprRequest.reportNo == 4)
                    action = "DistrictWisePossibleReport";
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OtsProgressReportView", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, action);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.Int, otsDprRequest.reportType);
                objDBAccess.AddParameterToCommand("@ActionType", DBFactoryParameterType.Int, otsDprRequest.actionType);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, otsDprRequest.pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, otsDprRequest.pldbBranchId);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, otsDprRequest.districtId);
                objDBAccess.AddParameterToCommand("@VidhanSabha_ID", DBFactoryParameterType.Int, otsDprRequest.vidhanSabhaID);
                objDBAccess.AddParameterToCommand("@VidhanSabha", DBFactoryParameterType.VARCHAR, otsDprRequest.vidhanSabha);
                objDBAccess.AddParameterToCommand("@ReportDate", DBFactoryParameterType.VARCHAR, ((otsDprRequest.reportDate == "" || otsDprRequest.reportDate == "null") ? null : otsDprRequest.reportDate));
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetOtsDailyProgressViewDetails(OtsDprRequestModel otsDprRequest)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public class OtsDprRequestModel
        {
            public int? reportType { get; set; } = 0;
            public int? pldbId { get; set; } = 0;
            public int? pldbBranchId { get; set; } = 0;
            public string reportDate { get; set; }
            public int? reportNo { get; set; } = 0;
            public int? actionType { get; set; } = 0;
            public int? districtId { get; set; } = 0;
            public int? vidhanSabhaID { get; set; } = 0;
            public string vidhanSabha { get; set; }
        }
        #endregion

        #region GccLoanRpt

        public DataTable GccLoanRpt(ref DataPagingModel Details, int LevelId, bool IsExport = false)
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                //string whereCase = " ";
                string SearchParamsJson = JsonConvert.SerializeObject(Details.SearchParameter);
                Console.WriteLine(SearchParamsJson);

                cmd = objDBAccess.CreateCommand("DueDateDiaryMisRpt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchParameterJson", DBFactoryParameterType.NVarchar, SearchParamsJson);
                objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.NVarchar, IsExport);

                //Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                dt = objDBAccess.GetTable();
                Details.TotalRecords = dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalRecords"]) : 0;


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyBasicDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GccLoanReports(ref DataPagingModel Details, int LevelId, bool IsExport = false, string SpName = "")
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                //string whereCase = " ";
                string SearchParamsJson = JsonConvert.SerializeObject(Details.SearchParameter);
                Console.WriteLine(SearchParamsJson);

                cmd = objDBAccess.CreateCommand(SpName, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchParameterJson", DBFactoryParameterType.NVarchar, SearchParamsJson);
                objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.NVarchar, IsExport);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.UserRoleDeptMappingId);

                //Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                dt = objDBAccess.GetTable();
                Details.TotalRecords = dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalRecords"]) : 0;

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GccLoanReports(ref DataPagingModel Details, int LevelId, bool IsExport = false, string SpName=\"\")" + SpName }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion


        #region GetGccLoanApplicationList
        public DataTable GetGccLoanApplicationList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                //string SelectedStatus = "";

                //ApplicationStatus Filter
                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }

                ////Status
                //if (Details.SearchParameter.Where(x => x.Key == "SelectedStatus").Any())
                //{
                //    SelectedStatus = Details.SearchParameter.Where(x => x.Key == "SelectedStatus").Select(x => x.Value).FirstOrDefault();
                //}

                //District Filter
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and a.LoanDistrictId = " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }
                // Block Filter 
                if (Details.SearchParameter.Any(x => x.Key == "CCBBANK_ID"))
                {
                    whereCase += " AND a.LoanBankID = " + Details.SearchParameter.FirstOrDefault(x => x.Key == "CCBBANK_ID").Value;
                }

                // Branch Filter 
                if (Details.SearchParameter.Any(x => x.Key == "PLDB_Branch_ID"))
                {
                    whereCase += " AND a.LoanBranchId = " + Details.SearchParameter.FirstOrDefault(x => x.Key == "PLDB_Branch_ID").Value;
                }

                // Pacs Filter
                if (Details.SearchParameter.Any(x => x.Key == "Pacs_Id"))
                {
                    var pacsId = Details.SearchParameter.FirstOrDefault(x => x.Key == "Pacs_Id").Value;

                    if (string.IsNullOrEmpty(pacsId) || pacsId == "0")
                    {
                        whereCase += " AND (a.PACS_ID IS NULL OR a.PACS_ID = 0)";
                    }
                    else
                    {
                        whereCase += " AND a.PACS_ID = " + pacsId;
                    }
                }

                //SearchText Filter
                if (Details.SearchParameter.Any(x => x.Key == "SearchText"))
                {
                    string value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " AND ( " + " REPLACE(a.Name_ENG, ' ', '') LIKE '%" + value + "%' OR " + " REPLACE(a.MobileNo, ' ', '') LIKE '%" + value + "%' OR " + " REPLACE(a.RegistrationNo, ' ', '') LIKE '%" + value + "%' " + " )";
                }

                //FromDate Filter
                if (Details.SearchParameter.Any(x => x.Key == "FromDate"))
                {
                    string fromDate = Details.SearchParameter.FirstOrDefault(x => x.Key == "FromDate").Value;
                    whereCase += " AND CAST(b.PaymentDate AS DATE) >= '" + fromDate + "'";
                }

                //ToDate Filter
                if (Details.SearchParameter.Any(x => x.Key == "ToDate"))
                {
                    string toDate = Details.SearchParameter.FirstOrDefault(x => x.Key == "ToDate").Value;
                    whereCase += " AND CAST(b.PaymentDate AS DATE) <= '" + toDate + "'";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GccLoanApplicationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    //objDBAccess.AddParameterToCommand("@FilterStatusId", DBFactoryParameterType.Int, SelectedStatus);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@MappedId", DBFactoryParameterType.Int, _LoggedInUser.GccId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GccLoanApplicationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@FilterStatusId", DBFactoryParameterType.Int, SelectedStatus);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@MappedId", DBFactoryParameterType.NVarchar, _LoggedInUser.GccId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Get_GopalApplicationFormViewData(string RNO, string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion GetGccLoanApplicationList



        #region GetGCCSummaryDistrictWise
        public List<Dictionary<string, object>> GetGCCSummaryDistrictWise(ref DataPagingModel Details)
        {

            DataTable dt = new DataTable();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and d.DISTRICT_ID = " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int DistrictID = _LoggedInUser.DistrictId;

                SqlCommand cmd = objDBAccess.CreateCommand("GCCSummaryDistrictWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictID);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();

                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var dict = new Dictionary<string, object>();

                        if (dt.Columns != null && dt.Columns.Count > 0)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        list.Add(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetGCCSummaryDistrictWise"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return list;
        }

        #endregion GetGCCSummaryDistrictWise


        #region GetPLBDLoanApplicationPldbWiseList
        public List<Dictionary<string, object>> GetPLBDLoanApplicationPldbWiseList(ref DataPagingModel Details)
        {

            DataTable dt = new DataTable();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "FinancialYearId").Any())
                {
                    var financialYearId = Details.SearchParameter.Where(x => x.Key == "FinancialYearId").Select(x => x.Value).FirstOrDefault();

                    if (financialYearId != "0")
                    {
                        whereCase += " and a.FinancialYearId = " + financialYearId;
                    }
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and a.PldbId = " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "BRANCH_ID").Any())
                {
                    whereCase += " and a.BranchId = " + Details.SearchParameter.Where(x => x.Key == "BRANCH_ID").Select(x => x.Value).FirstOrDefault();
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPldbWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var dict = new Dictionary<string, object>();

                        if (dt.Columns != null && dt.Columns.Count > 0)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        list.Add(dict);
                    }
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetPLBDLoanApplicationPldbWiseList"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return list;
        }

        #endregion GetPLBDLoanApplicationPldbWiseList


        #region GetPLBDLoanApplicationSectorWiseList
        public Dictionary<string, object> GetPLBDLoanApplicationSectorWiseList(ref DataPagingModel Details)
        {
            DataSet ds = new DataSet();

            List<Dictionary<string, object>> headerList = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Any(x => x.Key == "PLDB_ID"))
                {
                    whereCase += " AND a.PldbId = " + Details.SearchParameter.First(x => x.Key == "PLDB_ID").Value;
                }

                if (Details.SearchParameter.Any(x => x.Key == "BRANCH_ID"))
                {
                    whereCase += " AND a.BranchId = " + Details.SearchParameter.First(x => x.Key == "BRANCH_ID").Value;
                }

                UserEntity loggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationSectorWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, loggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, loggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, loggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, loggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, loggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, loggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                ds = objDBAccess.GetDataSet();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        headerList.Add(new Dictionary<string, object>
                        {
                            { "SectorId", row["SectorId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["SectorId"])  },
                            { "HeaderName", row["HeaderName"].ToString() },
                            { "SortOrder", Convert.ToInt32(row["SortOrder"]) }
                        });
                    }
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];

                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> record = new Dictionary<string, object>();

                        foreach (DataColumn col in dt.Columns)
                        {
                            record[col.ColumnName] = row[col];
                        }

                        dataList.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetPLBDLoanApplicationSectorWiseList"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return new Dictionary<string, object> { { "headers", headerList }, { "data", dataList } };
        }
        #endregion

        public class PurposeComparer : IEqualityComparer<Dictionary<string, object>>
        {
            public bool Equals(Dictionary<string, object> x, Dictionary<string, object> y)
            {
                return (int)x["PurposeId"] == (int)y["PurposeId"];
            }

            public int GetHashCode(Dictionary<string, object> obj)
            {
                return obj["PurposeId"].GetHashCode();
            }
        }


        #region GetPLBDLoanApplicationPurposeWiseList
        public Dictionary<string, object> GetPLBDLoanApplicationPurposeWiseList(ref DataPagingModel Details)
        {
            DataSet ds;
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            objDBAccess.OpenConnection();

            string whereCase = "";

            if (Details.SearchParameter.Any(x => x.Key == "PLDB_ID"))
            {
                whereCase += " AND la.PldbId=" + Details.SearchParameter.First(x => x.Key == "PLDB_ID").Value;
            }

            if (Details.SearchParameter.Any(x => x.Key == "BRANCH_ID"))
            {
                whereCase += " AND la.BranchId=" + Details.SearchParameter.First(x => x.Key == "BRANCH_ID").Value;
            }

            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPurposeWise", DBFactoryCommandType.PROCEDURE_COMMAND);

            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, user.UserId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, user.PLDBId);
            objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, user.PLDBBrancchId);
            objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

            ds = objDBAccess.GetDataSet();

            DataTable headerTable = ds.Tables[0];

            var sectorHeaders = headerTable.AsEnumerable().GroupBy(r => new { SectorId = r.Field<int>("SectorId"), SectorName = r.Field<string>("SectorShortName") }).Select(g => new Dictionary<string, object>{
            { "SectorId", g.Key.SectorId },
            { "SectorShortName", g.Key.SectorName },
            { "Purposes", g.Where(x => !x.IsNull("PurposeId")).Select(x => new Dictionary<string, object> {
                    { "PurposeId", x.Field<int>("PurposeId") },
                    { "PurposeName", x.Field<string>("PurposeName") } }).Distinct(new PurposeComparer()).ToList()
            }}).ToList();

            DataTable dt = ds.Tables[1];
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                    dict[col.ColumnName] = row[col];

                dataList.Add(dict);
            }

            objDBAccess.CloseConnection();

            return new Dictionary<string, object> { { "Headers", sectorHeaders }, { "Data", dataList } };
        }
        #endregion




        #region GetPLBDLoanApplicationPurposeWiseSectorList 

        public Dictionary<string, object> GetPLBDLoanApplicationPurposeWiseSectorList(ref DataPagingModel Details)
        {
            DataSet ds = null;
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationBySector", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, user.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, user.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, user.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                if (Details.SearchParameter.Any(x => x.Key == "SECTOR_ID"))
                {
                    objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, Convert.ToInt32(Details.SearchParameter.First(x => x.Key == "SECTOR_ID").Value));
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, 0);
                }

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object> { { "Headers", new List<object>() }, { "Data", new List<object>() }, { "Error", ex.Message } };
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                return new Dictionary<string, object> { { "Headers", new List<object>() }, { "Data", new List<object>() } };
            }

            DataTable headerTable = ds.Tables[0];

            var sectorHeaders = headerTable.AsEnumerable().GroupBy(r => r.Field<string>("SectorName")).Select(g => new Dictionary<string, object> { { "SectorName", g.Key }, { "Purposes", g.Select(x => x.Field<string>("PurposeName")).Distinct().ToList() } }).ToList();

            DataTable dt = ds.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }

                dataList.Add(dict);
            }

            return new Dictionary<string, object> { { "Headers", sectorHeaders }, { "Data", dataList } };
        }

        #endregion GetPLBDLoanApplicationPurposeWiseSectorList



        //#region GetLoanApplicationDetailBySector
        //public Dictionary<string, object> GetLoanApplicationDetailBySector(int pldbId = 0, int sectorId = 0, int purposeId = 0 , int branchid = 0)
        //{
        //    DataSet ds;
        //    List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        //    UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
        //    objDBAccess.OpenConnection();

        //    SqlCommand cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationDetailBySector", DBFactoryCommandType.PROCEDURE_COMMAND);
        //    objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
        //    objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
        //    objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, purposeId);
        //    objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchid);
        //    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
        //    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);

        //    ds = objDBAccess.GetDataSet();

        //    DataTable dt = ds.Tables[0];

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        Dictionary<string, object> dict = new Dictionary<string, object>();

        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            dict[col.ColumnName] = row[col];
        //        }

        //        dataList.Add(dict);
        //    }

        //    objDBAccess.CloseConnection();

        //    return new Dictionary<string, object> { { "Data", dataList } };
        //}

        //#endregion GetLoanApplicationDetailBySector



        #region GetLoanApplicationDetailBySector
        public DataTable GetLoanApplicationDetailBySector(DataPagingModel Details, int pldbId = 0, int sectorId = 0, int purposeId = 0, int branchid = 0)
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
            objDBAccess.OpenConnection();

            SqlCommand cmd = new SqlCommand();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationDetailBySector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
                objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, purposeId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchid);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }

            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationDetailBySector", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
            objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, purposeId);
            objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchid);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

            ds = objDBAccess.GetTable();

            return ds;
        }

        #endregion GetLoanApplicationDetailBySector



        #region GetLoanApplicationSectorCountDetail
        public DataTable GetLoanApplicationSectorCountDetail(DataPagingModel Details, int pldbId = 0, int sectorId = 0, int branchid = 0)
        {
            DataTable ds;
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
            objDBAccess.OpenConnection();

            SqlCommand cmd = new SqlCommand();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationSectorCountDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchid);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }

            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationSectorCountDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchid);
            objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;
        }

        #endregion GetLoanApplicationSectorCountDetail


        #region GetLoanApplicationPLDBCountDetail
        public DataTable GetLoanApplicationPLDBCountDetail(DataPagingModel Details, int pldbId = 0, int branchId = 0, int FinancialYearId = 0)
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPldbWiseDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, FinancialYearId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPldbWiseDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, FinancialYearId);
            objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchId);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetLoanApplicationPLDBCountDetail


        public DataSet GetPacsSummaryDetails(ref DataPagingModel Details, string FromDate = "", string ToDate = "", string actionType = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetPacsSummaryDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, actionType);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, ToDate);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPacsSummaryDetails(string FromDate = '" + FromDate + "', string ToDate= '" + ToDate + "', string actionName='" + actionType + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        #region GetGCCSummaryBankWise
        public List<Dictionary<string, object>> GetGCCSummaryBankWise(ref DataPagingModel Details)
        {

            DataTable dt = new DataTable();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                if (Details.SearchParameter.Where(x => x.Key == "CCBBANK_ID").Any())
                {
                    whereCase += " and b.CCBId = " + Convert.ToString(Details.SearchParameter.Where(x => x.Key == "CCBBANK_ID").Select(x => x.Value).FirstOrDefault());
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GCCSummaryBankWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();

                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var dict = new Dictionary<string, object>();

                        if (dt.Columns != null && dt.Columns.Count > 0)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        list.Add(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetGCCSummaryBankWise" }, ex);
                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return list;
        }


        //Bhavesh Singh 

        #region GetPLBDLoanApplicationSUMMERYWISEList
        public Dictionary<string, object> GetPLBDLoanApplicationSUMMERYWISEList(ref DataPagingModel Details)
        {
            DataSet ds = new DataSet();

            List<string> headerList = new List<string>();
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            // Sector Based Count
            List<Dictionary<string, object>> summaryList = new List<Dictionary<string, object>>();


            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Any(x => x.Key == "PLDB_ID"))
                {
                    whereCase += " AND a.PldbId = " + Details.SearchParameter.First(x => x.Key == "PLDB_ID").Value;
                }

                if (Details.SearchParameter.Any(x => x.Key == "BRANCH_ID"))
                {
                    whereCase += " AND a.BranchId = " + Details.SearchParameter.First(x => x.Key == "BRANCH_ID").Value;
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("SP_SectorPurpose_Count_Amount", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                ds = objDBAccess.GetDataSet();

                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        headerList.Add(row["HeaderName"].ToString());
                    }
                }
                //TABLE 1(DATA)

                if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];

                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();

                        foreach (DataColumn col in dt.Columns)
                        {
                            dict[col.ColumnName] = row[col];
                        }

                        dataList.Add(dict);
                    }
                }

                //TABLE 2(SECTOR TOTALS / GRAND TOTAL)
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt2 = ds.Tables[0];

                    foreach (DataRow row in dt2.Rows)
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();

                        foreach (DataColumn col in dt2.Columns)
                        {
                            dict[col.ColumnName] = row[col];
                        }

                        summaryList.Add(dict);
                    }
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetPLBDLoanApplicationSUMMERYWISEList"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return new Dictionary<string, object> { { "Headers", headerList }, { "Data", dataList }, { "Summary", summaryList } };
        }

        #endregion GetPLBDLoanApplicationSUMMERYWISEList

        //new APIPagingResponce Summary
        public Dictionary<string, object> GetPLBDLoanApplicationReportSUMMERYWISE(ref DataPagingModel Details)
        {
            DataSet ds = new DataSet();
            List<string> headerList = new List<string>();
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Any(x => x.Key == "PLDB_ID"))
                    whereCase += " AND a.PldbId = " + Details.SearchParameter.First(x => x.Key == "PLDB_ID").Value;

                if (Details.SearchParameter.Any(x => x.Key == "BRANCH_ID"))
                    whereCase += " AND a.BranchId = " + Details.SearchParameter.First(x => x.Key == "BRANCH_ID").Value;

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationSummaryWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                ds = objDBAccess.GetDataSet();
                // ===== SINGLE TABLE HANDLING =====
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    // Headers (Dynamic)
                    foreach (DataColumn col in dt.Columns)
                    {
                        headerList.Add(col.ColumnName);
                    }

                    // Data
                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            dict[col.ColumnName] = row[col];
                        }
                        dataList.Add(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetPLBDLoanApplicationSUMMERYWISEList"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return new Dictionary<string, object>
            {
                { "Headers", headerList },
                { "Data", dataList }
            };
        }
        #endregion GetGCCSummaryDistrictWise

        //Bhavesh Count Dtails of SummerWise 08-01-2026
        #region GetLoanApplicationSummerWIseCountDetail
        public Dictionary<string, object> GetLoanApplicationSummerWIseCountDetail(int pldbId = 0, int PurposeId = 0)
        {
            DataSet ds;
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
            objDBAccess.OpenConnection();

            SqlCommand cmd = objDBAccess.CreateCommand("sp_GetLoanApplicationSummeryCountDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, PurposeId);
            //objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);

            ds = objDBAccess.GetDataSet();

            DataTable dt = ds.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }

                dataList.Add(dict);
            }

            objDBAccess.CloseConnection();

            return new Dictionary<string, object> { { "Data", dataList } };
        }

        #endregion GetLoanApplicationSummerWIseCountDetail


        #region GetPLDBSubventionODApplicationList
        public List<Dictionary<string, object>> GetPLDBSubventionODApplicationList(ref DataPagingModel Details)
        {

            DataTable dt = new DataTable();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and sd.PLDB_Id = " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "BRANCH_ID").Any())
                {
                    whereCase += " and sd.PLDB_BranchId = " + Details.SearchParameter.Where(x => x.Key == "BRANCH_ID").Select(x => x.Value).FirstOrDefault();
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("PLDBSubventionODApplicationListData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var dict = new Dictionary<string, object>();

                        if (dt.Columns != null && dt.Columns.Count > 0)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        list.Add(dict);
                    }
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MisModel",
                    FunctionName = "GetPLDBSubventionODApplicationList"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return list;
        }

        #endregion GetPLDBSubventionODApplicationList



        #region GetPLDBSubventionODDetailList
        public DataTable GetPLDBSubventionODDetailList(DataPagingModel Details, int pldbId = 0)
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("PLDBSubventionODDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("PLDBSubventionODDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetPLDBSubventionODDetailList


        //Bhavesh Singh 17/03/25

        #region GetMPacsMembershipDistrictWiseCount
        public DataTable GetMPacsMembershipDistrictWiseCount(DataPagingModel Details, int pldbId = 0)
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                //cmd = objDBAccess.CreateCommand("GetMPacsMembershipDistrictWiseCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd = objDBAccess.CreateCommand("usp_GetMPacsMembershipDistrictWiseCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
                //Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("usp_GetMPacsMembershipDistrictWiseCount", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetMPacsMembershipDistrictWiseCount



        //Bhavesh Singh 09/03/26
        public class AadhaarServiceLogRequest
        {
            public string Aadhaar { get; set; }
            public string ServiceType { get; set; }
            //public DateTime? FromDate { get; set; }
            //public DateTime? ToDate { get; set; }
            //public int PageNo { get; set; } = 1;
            //public int PageSize { get; set; } = 20;
        }

        //Bhavesh Singh 09/03/26
        public class AadhaarServiceLogModel
        {
            public string Aadhaar { get; set; }
            public string ServiceType { get; set; }
            public string RequestXML { get; set; }
            public string ResponceXML { get; set; }
            public string Status { get; set; }
            public string TxnId { get; set; }
            public DateTime CreateDate { get; set; }
            public string ErrorCode { get; set; }
            public string ModuleName { get; set; }
            public int TotalCount { get; set; }
        }
        public DataTable GetAadhaarServiceLogReport(DataPagingModel Details, string Aadhaar = null, string ServiceType = null)
        {
            DataTable ds = new DataTable();

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("GetAadhaarServiceLogReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Aadhaar", DBFactoryParameterType.NVarchar, Aadhaar);
                objDBAccess.AddParameterToCommand("@ServiceType", DBFactoryParameterType.NVarchar, ServiceType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }

            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("GetAadhaarServiceLogReport", DBFactoryCommandType.PROCEDURE_COMMAND);

            objDBAccess.AddParameterToCommand("@Aadhaar", DBFactoryParameterType.NVarchar, Aadhaar);
            objDBAccess.AddParameterToCommand("@ServiceType", DBFactoryParameterType.NVarchar, ServiceType); objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;


        }

        public DataTable GetMMPBYFarmerDetails(DataPagingModel Details, int DistrictId = 0)
        {
            DataTable ds = new DataTable();

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            string whereCase = "";
            string searchText = "";

            if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
            {
                string Value = Details.SearchParameter
                    .Where(x => x.Key == "SearchText")
                    .Select(x => x.Value)
                    .FirstOrDefault()
                    .Replace(" ", "");

                whereCase += " and (REPLACE(md.jan_aadhaar_no,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.full_name,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.jan_aadhaar_member_id,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.address,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.mobile_number,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.city_block_name,' ','') like '%" + Value + "%' " +
                             "or REPLACE(md.policy_no,' ','') like '%" + Value + "%')";
            }

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("GetMMPBYFarmerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@searchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@DistrictId ", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@where", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }

            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("GetMMPBYFarmerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);

            objDBAccess.AddParameterToCommand("@searchText", DBFactoryParameterType.NVarchar, searchText);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            //objDBAccess.AddParameterToCommand("@DistrictId ", DBFactoryParameterType.Int, DistrictId);
            objDBAccess.AddParameterToCommand("@DistrictId ", DBFactoryParameterType.Int, DistrictId);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@where", DBFactoryParameterType.NVarchar, whereCase);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;
        }

        //Addde By Bhavesh 23/03/2026

        public DataTable GetSocietyStatusList()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyStatusList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.SocietyModel",
                    FunctionName = "DataTable GetSocietyStatusList()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        #region BUDS_Status_Report

        public DataTable GetBUDSStatusReportDistWise(int DistrictId, string SearchingText = "", string FromDate = "", string ToDate = "")
        {
            DataTable ds = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Get_BUDS_StatusReport_DistWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SearchingText", DBFactoryParameterType.VARCHAR, SearchingText);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, ToDate);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable Get_BUDS_StatusReport_DistWise(int UserId, int DistrictId, string SearchingText = '')" }, ex);
            }
            return ds;
        }
        #endregion 'BUDS_Status_Report'

        #region 'GCCDataYearWiseInformation'
        public DataSet GCCDataYearWiseInformation(int financialYearId = 0, int districtId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetGCCDataYearWiseInformation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GCCDataYearWiseInformation");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetGCCDataYearWiseInformation(int financialYearId = 0, int districtId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        #endregion 'GetGCCDataYearWiseInformation'


        public DataSet GetTotalLoaneeDetails(string year, string type)
        {

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetTotalLoaneeDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.NVarchar, year);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, type);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetTotalLoaneeDetails(string year , string type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;

        }

        #region GetLoanApplicationPLDBStatusCountDetail
        public DataTable GetLoanApplicationPLDBStatusCountDetail(DataPagingModel Details, int pldbId = 0, string statusType = "", int financialYearId = 0, int branchId = 0)
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPldbStatusWiseDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchId);
                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, statusType);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("sp_PLBDLoanApplicationPldbStatusWiseDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
            objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, branchId);
            objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, statusType);
            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, user.UserRoleDeptMappingId);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
            objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, user.LevelId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, user.RoleId);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetLoanApplicationPLDBStatusCountDetail

        #region 'GccNewBankWiseSanctionList'

        public DataSet GccBankWiseSanctionList(ref DataPagingModel Details)
        {
            string qry = "";

            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                string ccbId = string.Empty;
                string ccbbranchId = string.Empty;
                //string financialYearId = string.Empty;
                string isRenew = string.Empty;
                string type = string.Empty;
                string ReportType = string.Empty;
                string fromDate = string.Empty;
                string toDate = string.Empty;

                fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").FirstOrDefault().Value;
                toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").FirstOrDefault().Value;

                string nodOdStatus = string.Empty;
                nodOdStatus = Details.SearchParameter.Where(x => x.Key == "NODODStatus").FirstOrDefault().Value;

                //financialYearId = Details.SearchParameter.Where(x => x.Key == "FinancialYearId").FirstOrDefault().Value;
                ccbId = Details.SearchParameter.Where(x => x.Key == "LoanBankID").FirstOrDefault().Value;
                ccbbranchId = Details.SearchParameter.Where(x => x.Key == "LoanBranchId").FirstOrDefault().Value;
                isRenew = Details.SearchParameter.Where(x => x.Key == "IsOverall").FirstOrDefault().Value;
                type = Details.SearchParameter.Where(x => x.Key == "Type").FirstOrDefault().Value;
                ReportType = Details.SearchParameter.Where(x => x.Key == "ReportType").FirstOrDefault().Value;

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();

                if (Details.IsAllRecords)
                {
                    Details.PageSize = 500000;
                }

                cmd = objDBAccess.CreateCommand("sp_GetGCCReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //cmd = objDBAccess.CreateCommand("sp_GetGCCReport_test14072026", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CCB_Id", DBFactoryParameterType.Int, ccbId);
                objDBAccess.AddParameterToCommand("@CCBBranch_Id", DBFactoryParameterType.Int, ccbbranchId);
                //objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.VARCHAR, financialYearId);
                objDBAccess.AddParameterToCommand("@IsRenew", DBFactoryParameterType.Int, isRenew);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, type);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@NODODStatus", DBFactoryParameterType.VARCHAR, nodOdStatus);

                qry += objDBAccess.getSqlCommandText();

                dt = objDBAccess.GetTable();

                if (dt != null)
                {
                    ds.Tables.Add(dt.Copy());

                    if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalRecords"))
                    {
                        Details.TotalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);
                    }
                    else
                    {
                        Details.TotalRecords = 0;
                    }
                }
                else
                {
                    Details.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GccBankWiseSanctionList" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion
        //#region 'GetCSSComplaintAgeingData'
        //public DataSet GetCSSComplaintAgeingData(DataPagingModel Details)
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = objDBAccess.CreateCommand("GetCSSComplaintAgeingData", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
        //        return objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCSSComplaintAgeingData" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return null;
        //}
        //#endregion



        #region GetCSSComplaintAgeingData
        public DataTable GetCSSComplaintAgeingDataOld(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and c.DistrictId = " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCSSComplaintAgeingData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCSSComplaintAgeingData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetSubsidySummaryList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetCSSComplaintAgeingData(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                string districtId = string.Empty;
                string cssId = string.Empty;
                string selectStates = string.Empty;
                districtId = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").FirstOrDefault().Value;
                cssId = Details.SearchParameter.Where(x => x.Key == "CCSId").FirstOrDefault().Value;
                selectStates = Details.SearchParameter.Where(x => x.Key == "rdoSelect").FirstOrDefault().Value;

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.IsAllRecords) { Details.PageSize = 500000; }
                cmd = objDBAccess.CreateCommand("GetCSSComplaintAgeingData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, Convert.ToInt32(districtId));
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, selectStates);
                objDBAccess.AddParameterToCommand("@cssId", DBFactoryParameterType.NVarchar, Convert.ToInt32(cssId));
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    Details.TotalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);
                }
                else
                {
                    Details.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.PLDPLoanApplication",
                        FunctionName = "GetCSSComplaintAgeingData(ref DataPagingModel Details)" + qry
                    },
                    ex
                );
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        #endregion  GetCSSComplaintAgeingData


        #region 'GetSocietyMappingList'

        public DataSet GetSocietyMappingList(ref DataPagingModel Details)
        {
            string qry = "";

            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();
                string districtid = string.Empty;
                string IsRural = string.Empty;
                string IsOnline = string.Empty;
                string ReportType = string.Empty;
                string StatusId = string.Empty;

                districtid = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").FirstOrDefault().Value;
                IsRural = Details.SearchParameter.Where(x => x.Key == "IsRural").FirstOrDefault().Value;

                IsOnline = Details.SearchParameter.Where(x => x.Key == "IsOnline").FirstOrDefault().Value;
                ReportType = Details.SearchParameter.Where(x => x.Key == "ReportType").FirstOrDefault().Value;
                StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").FirstOrDefault().Value ?? "0";


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();

                if (Details.IsAllRecords)
                {
                    Details.PageSize = 500000;
                }

                cmd = objDBAccess.CreateCommand("USP_GetSocietiesJurisdictionData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtid);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Int, IsRural);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, Convert.ToInt32(StatusId));
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.VARCHAR, IsOnline);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);

                dt = objDBAccess.GetTable();

                if (dt != null)
                {
                    ds.Tables.Add(dt.Copy());
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalRecords"))
                    {
                        Details.TotalRecords = Convert.ToInt32(dt.Rows[0]["TotalRecords"]);
                    }
                    else
                    {
                        Details.TotalRecords = 0;
                    }
                }
                else
                {
                    Details.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyMappingList" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion


        #region 'GetPldbCMSOtsList'

        public DataSet GetPldbCMSOtsList(int pldbId = 0, int pldbbranchId = 0)
        {
            string qry = "";

            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetPldbCMSOtsApplicationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.NVarchar, pldbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, pldbbranchId);

                qry += objDBAccess.getSqlCommandText();
                ds = objDBAccess.GetDataSet();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GccBankWiseSanctionList" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion


        #region GetLoanApplicationOTSCountDetail
        public DataTable GetLoanApplicationOTSCountDetail(DataPagingModel Details, int pldbId = 0, string docType = "")
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("GetPldbCMSOtsApplicationDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, docType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("GetPldbCMSOtsApplicationDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, pldbId);
            objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, docType);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetLoanApplicationOTSCountDetail


        public DataSet GetPacsSummaryNewDetails(ref DataPagingModel Details, string FromDate = "", string ToDate = "", string actionType = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetPacsSummaryDetails_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, actionType);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, ToDate);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPacsSummaryDetails(string FromDate = '" + FromDate + "', string ToDate= '" + ToDate + "', string actionName='" + actionType + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        #region 'GetGSSMpacsTargetCount'

        public DataSet GetGSSMpacsTargetCount(int DistrictId = 0)
        {
            string qry = "";

            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetGSSMpacsTargetCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                qry += objDBAccess.getSqlCommandText();
                ds = objDBAccess.GetDataSet();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetGSSMpacsTargetCount" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion


        #region GetGSSMpacsTargetCountDetail
        public DataTable GetGSSMpacsTargetCountDetail(DataPagingModel Details, int DistrictId = 0, string Type = "")
        {
            DataTable ds = new DataTable();
            UserEntity user = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();

            if (Details.PageNumber == 1)
            {
                cmd = objDBAccess.CreateCommand("USP_GetGSSMpacsTargetDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            if (Details.IsAllRecords)
            {
                Details.PageSize = Details.TotalRecords;
            }

            cmd = objDBAccess.CreateCommand("USP_GetGSSMpacsTargetDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
            objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

            ds = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return ds;

        }

        #endregion GetGSSMpacsTargetCountDetail



        #region 'GetSocietyMergeReport'

        public DataSet GetSocietyMergeReport(string Action = "", int DistrictId = 0)
        {
            string qry = "";

            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyMergeReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                qry += objDBAccess.getSqlCommandText();
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyMergeReport" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion


        #region 'GetActiveSocietyDetail'

        public DataTable GetActiveSocietyDetail(DataPagingModel Details, int DistrictId = 0, int FlagId = 0)
        {
            string qry = "";

            DataTable ds = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = new SqlCommand();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyMergeReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "getActiveSocietyDetail");
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@FlagId", DBFactoryParameterType.Int, FlagId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyMergeReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "getActiveSocietyDetail");
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FlagId", DBFactoryParameterType.Int, FlagId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

                ds = objDBAccess.GetTable();

                objDBAccess.CloseConnection();

                return ds;

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetActiveSocietyDetail" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        #endregion



        #region GetManageSocietyDetailsList
        public DataTable GetManageSocietyDetailsList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            //string whereCase = "";
            string districtId = string.Empty;
            try
            {
                objDBAccess.OpenConnection();

                districtId = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").FirstOrDefault().Value;

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetEditnDelete_SocietyDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetEditnDelete_SocietyDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetManageSocietyDetailsList(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion GetManageSocietyDetailsList


        #region GetElectionReportList
        public DataTable GetElectionDetailReportList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " AND ( " + " REPLACE(S.SocietyName, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(E.RegistrationNo, ' ', '') LIKE '%" + Value + "%' OR " +
                    " REPLACE(A.Name, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(E.PostName, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(LV.Lovdescription, ' ', '') LIKE '%" + Value + "%' " + " )";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetElectionListReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetElectionListReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetElectionDetailReportList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #endregion  GetElectionReportList

        #region GetElectionFlowReportList
        public DataTable GetElectionFlowDetailReportList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " AND ( " + " REPLACE(S.SocietyName, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(E.RegistrationNo, ' ', '') LIKE '%" + Value + "%' OR " +
                    " REPLACE(A.Name, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(E.PostName, ' ', '') LIKE '%" + Value + "%' OR " + " REPLACE(LV.Lovdescription, ' ', '') LIKE '%" + Value + "%' " + " )";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetElectionFlowListReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetElectionFlowListReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetElectionDetailReportList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #endregion  GetElectionFlowReportList

        public DataTable GetNewLovValuesForStatusListId(string ListCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GetNewLovValuesForLandDetailByListId(int ListCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveWorkflowAction(WorkflowActionModel workflowActionModel)
        {
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SaveWorkflowAction", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@ElectionId", DBFactoryParameterType.Int, workflowActionModel.ElectionId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, workflowActionModel.StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, workflowActionModel.Remark);
                objDBAccess.AddParameterToCommand("@ActionBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "SaveWorkflowAction" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable GetKPIReport(string KPIFor, int Month, string FY, int OfficeId, int DistrictId, int ResionId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, KPIFor);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.VARCHAR, FY);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, OfficeId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, ResionId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MISModel", FunctionName = "GetKPIReport" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetKPIOffices(string KPIFor, string Month, string FinancialYearId)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetKPIOffices", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, KPIFor);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.VARCHAR, Month);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.VARCHAR, FinancialYearId);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MISModel", FunctionName = "GetKPIOffices" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetKPIOverallRating(string KPIFor, int Month, string FY, int OfficeId, int DistrictId = 0, int DivisionId = 0, int ResionId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIOverallRating", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, KPIFor);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.VARCHAR, FY);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, OfficeId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, ResionId);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MISModel", FunctionName = "GetKPIOverallRating" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        #region GetSocietyLocationByDistrictReportList
        public DataTable GetSocietyLocationByDistrictReportList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                if (Details.SearchParameter.Any(x => x.Key == "DISTRICT_ID"))
                {
                    string districtId = Details.SearchParameter.First(x => x.Key == "DISTRICT_ID").Value;

                    if (!string.IsNullOrWhiteSpace(districtId) && districtId != "0")
                    {
                        whereCase += " AND S.DISTRICT_ID = " + districtId;
                    }
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("USP_GetSocietyLocationByDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("USP_GetSocietyLocationByDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetSocietyLocationByDistrictReportList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #endregion  GetSocietyLocationByDistrictReportList


        public DataSet GetSocietyGeoTaggingReport()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetSocietyGeoTaggingReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDetail");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetSocietyGeoTaggingReport()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetFertilizerDemandDetail(ref DataPagingModel Details, bool isExport = false, string actionType = "", int type = 0, int DemandRequestId = 0)
        {
            var ds = new DataSet();
            dt.Clear();
            string qry = "";
            try
            {
                string DistrictId = "0";
                string SocietyType = "";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    DistrictId = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "SocietyType").Any())
                {
                    SocietyType = Details.SearchParameter.Where(x => x.Key == "SocietyType").Select(x => x.Value).FirstOrDefault();
                }

                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("USP_GetFertilizerDemandReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, type);
                objDBAccess.AddParameterToCommand("@DemandRequestId", DBFactoryParameterType.BigInt, DemandRequestId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, SocietyType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetFertilizerDemandDetail(ref DataPagingModel Details, bool isExport = false, string actionType = \"\", int type = 0, int DemandRequestId = 0)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                ds.Dispose();
            }

            return ds;
        }

        public DataSet GetFertilizerDemandMultiDetail(ref DataPagingModel Details, bool isExport = false, string actionType = "", int type = 0, int DemandRequestId = 0)
        {
            var ds = new DataSet();
            dt.Clear();
            string qry = "";
            try
            {
                string DistrictId = "0";
                string SocietyType = "";
                string ReportType = "1";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    DistrictId = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "SocietyType").Any())
                {
                    SocietyType = Details.SearchParameter.Where(x => x.Key == "SocietyType").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "ReportType").Any())
                {
                    ReportType = Details.SearchParameter.Where(x => x.Key == "ReportType").Select(x => x.Value).FirstOrDefault();
                }

                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("USP_GetFertilizerDemandReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, type);
                objDBAccess.AddParameterToCommand("@DemandRequestId", DBFactoryParameterType.BigInt, DemandRequestId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, SocietyType);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetFertilizerDemandDetail(ref DataPagingModel Details, bool isExport = false, string actionType = \"\", int type = 0, int DemandRequestId = 0)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                ds.Dispose();
            }

            return ds;
        }

        public DataSet GetTaggedItemDetail(ref DataPagingModel Details, bool isExport = false)
        {
            var ds = new DataSet();
            dt.Clear();
            string qry = "";
            try
            {
                string DistrictId = "0";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    DistrictId = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("USP_GetTaggingItemReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetTaggedItemDetail(ref DataPagingModel Details, bool isExport = false)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                ds.Dispose();
            }

            return ds;
        }

        public DataTable GetKPI_PerformanceCriteria(string criteriaForKPI)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPI_PerformanceCriteria", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CriteriaForKPI", DBFactoryParameterType.NVarchar, criteriaForKPI);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetKPI_PerformanceCriteria(string criteriaForKPI)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataSet GetFertilizerDetailByUser(string actionType = "", int DemandRequestId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetFertilizerReportByUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@DemandRequestId", DBFactoryParameterType.BigInt, DemandRequestId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetFertilizerDetailByUser()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataTable GetDRSocietyList()
        {
            DataTable dt = new DataTable();

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetSocietyListForDR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetDRSocietyList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable UpdateSocietyLocationApproval(int societyLocationDetailsID, int societyId, int isApproved, string remark)
        {

            DataTable dt = new DataTable();

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_UpdateSocietyLocationApproval", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyLocationDetailsID", DBFactoryParameterType.Int, societyLocationDetailsID);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, societyId);
                objDBAccess.AddParameterToCommand("@IsApproved", DBFactoryParameterType.Int, isApproved);
                objDBAccess.AddParameterToCommand("@ApprovedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, remark);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "UpdateSocietyLocationApproval(int societyLocationDetailsID, int societyId, int isApproved)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetKPIpldbOverallRating(string KPIFor, int Month, string FY, int PldbId)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPLDBKPIOverallRating", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, KPIFor);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.VARCHAR, FY);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, PldbId);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MISModel", FunctionName = "GetKPIOverallRating" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



    }
}

public class WorkflowActionModel
{
    public int ElectionId { get; set; }
    public int StatusId { get; set; }
    public string Remark { get; set; }
    public int ActionBy { get; set; }
}