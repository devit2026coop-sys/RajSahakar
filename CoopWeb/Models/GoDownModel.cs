using Antlr.Runtime.Tree;
using com.sun.security.ntlm;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using sun.swing;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;
using static Cooperative.Controllers.InspectionController;
using static Cooperative.Models.CoopModel;
using static iTextSharp.text.pdf.AcroFields;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Models
{
    public class GodownModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public GodownModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public GodownModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        public int GodownDetailsInsert(GodownProposalEntity obj)
        {
            int Result = -1;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("insertGodown_Proposal", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicantName", DBFactoryParameterType.NVarchar, obj.ApplicantName);
                objDBAccess.AddParameterToCommand("@SocietyAvailabilityRemark", DBFactoryParameterType.NVarchar, obj.SocietyAvailabilityRemark);
                objDBAccess.AddParameterToCommand("@SanctionAmt", DBFactoryParameterType.Int, obj.SanctionAmt);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, obj.BLOCK_ID);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, obj.GP_ID);
                objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, obj.VILLAGE_ID);
                objDBAccess.AddParameterToCommand("@CCB", DBFactoryParameterType.NVarchar, obj.CCB);
                objDBAccess.AddParameterToCommand("@CCBBranchId", DBFactoryParameterType.NVarchar, obj.CCBBranchId);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.NVarchar, obj.SocietyID);
                objDBAccess.AddParameterToCommand("@MTCapacityId", DBFactoryParameterType.NVarchar, obj.MTCapacityId);
                objDBAccess.AddParameterToCommand("@SocietyAvailabilityId", DBFactoryParameterType.NVarchar, obj.SocietyAvailabilityId);
                objDBAccess.AddParameterToCommand("@FormationPath", DBFactoryParameterType.NVarchar, obj.FormationPath);
                objDBAccess.AddParameterToCommand("@IFSCCode", DBFactoryParameterType.NVarchar, obj.IFSCCode);
                objDBAccess.AddParameterToCommand("@AccountNumber", DBFactoryParameterType.NVarchar, obj.AccountNumber);
                objDBAccess.AddParameterToCommand("@LandSize1", DBFactoryParameterType.Float, obj.LandSize1);
                objDBAccess.AddParameterToCommand("@LandSize2", DBFactoryParameterType.Float, obj.LandSize2);
                objDBAccess.AddParameterToCommand("@TotalLandSize1", DBFactoryParameterType.Float, obj.TotalLandSize1);
                objDBAccess.AddParameterToCommand("@TotalLandSize2", DBFactoryParameterType.Float, obj.TotalLandSize2);
                objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, obj.GodownId);
                objDBAccess.AddParameterToCommand("@NoOfGodown", DBFactoryParameterType.Int, obj.NoOfGodown);
                objDBAccess.AddParameterToCommand("@GodownDocuments", DBFactoryParameterType.NVarchar, obj.GodownDocuments);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId ", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@AssemblyId", DBFactoryParameterType.Int, obj.AssemblyId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && obj.TblGodownTimeBoundDetails != null)
                {

                    int FlagFirstRow = 1;
                    int InnerResult = 0;

                    foreach (TblTimeBoundDetails obj1 in obj.TblGodownTimeBoundDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertGodownTimeBoundDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, obj1.SocietyID);
                        objDBAccess.AddParameterToCommand("@MTCapacityId", DBFactoryParameterType.Int, obj1.MTCapacityId);
                        objDBAccess.AddParameterToCommand("@SchemeId", DBFactoryParameterType.Int, obj1.SchemeId);
                        objDBAccess.AddParameterToCommand("@FinancialYearID", DBFactoryParameterType.Int, obj1.FinancialYearID);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, obj1.ID);
                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GodownDetailsInsert(GodownProposalEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetGodownProposalList(ref DataPagingModel Details, Int64 MappingId, int Status = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "";
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
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
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {
                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        if (x.Value.IndexOf("? string:0 ?") < 0)
                        {
                            whereCase += " and " + x.Key + "='" + x.Value + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault().IndexOf("? string:0 ?") < 0)
                        {
                            CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        }
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("getGodownlist_Proposal", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, MappingId);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("getGodownlist_Proposal", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, MappingId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = " public DataTable GetGodownProposalList(ref DataPagingModel Details, Int64 MappingId, int Status = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

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
                SqlCommand cmd = objDBAccess.CreateCommand("Godown_SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, Obj.SSOID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "SocietyListWithDepartmentUser(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable GetNewLovValuesByListId(string ListCode)
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GetNewLovValuesByListId(int ListId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetNewLovValuesDocByListId(string ListCode)
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GetNewLovValuesByListId(int ListId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetGodownDetailsById(int godownId)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetGodownDetailsById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GodownModel", FunctionName = "GetGodownDetailsById" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        [HttpGet]
        public string GodownPacsLevelLetter(GodownProposalEntity _Entity)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return "Fail";
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/GodownPacsLevel.html");

                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));

                // Get Godown Details
                DataTable dt = objGodownModel.GetGodownDetailsById(_Entity.GodownId);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                    strbld.Replace("##District##", Convert.ToString(dr["DISTRICT_ENG"]));
                    strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                    strbld.Replace("##SanctionAmt##", Convert.ToString(dr["SanctionAmt"]));
                    strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                    strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                    strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                    strbld.Replace("##LandSize1##", Convert.ToString(dr["LandSize1"]));
                    strbld.Replace("##LandSize2##", Convert.ToString(dr["LandSize2"]));
                    strbld.Replace("##TotalLandSize1##", Convert.ToString(dr["TotalLandSize1"]));
                    strbld.Replace("##TotalLandSize2##", Convert.ToString(dr["TotalLandSize2"]));
                    strbld.Replace("##CurrentDate##", DateTime.Now.ToString("dd/MM/yyyy"));
                    strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                    strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                    strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                    strbld.Replace("##MTCapacityName##", Convert.ToString(dr["MTCapacityName"]));
                    strbld.Replace("##AvailabilityOfSocietyName##", Convert.ToString(dr["AvailabilityOfSocietyName"]));
                }

                GodownModel godownModel = new GodownModel();

                string fileFullPath = godownModel.CommonPdfGenerate(strbld);
                return fileFullPath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GodownController", FunctionName = "GodownPacsLevelLetter" }, ex);
                return "Fail";
            }
        }

        public string CommonPdfGenerate(StringBuilder sb)
        {
            string wkhtmlPath = HostingEnvironment.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(HostingEnvironment.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;

            try
            {
                string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {sb}
                    </body>
                   </html>";

                var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                string fileName = "GodownPacsLevel_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";

                string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdf);
                return filePath;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int UpdateFormationPath(int godownId, string filePath, string docName)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_UpdateGodownFormationPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                objDBAccess.AddParameterToCommand("@FormationPath", DBFactoryParameterType.NVarchar, filePath);
                objDBAccess.AddParameterToCommand("@DocName", DBFactoryParameterType.NVarchar, docName);

                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateFormationPath" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }


        public DataTable GetNewLovValuesForLandDetailByListId(string ListCode)
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


        public DataTable GETGodownFormationList(string SearchText = "")
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_GETGodownFormationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId ", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GETGodownFormationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GETActionsList()
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_GETGodownActionsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId ", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GETActionsList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataSet GetGodownListApplicationFormViewData(string Ids, string Action)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_GodownApplicationFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GetGodownListApplicationFormViewData" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public int Delete_GodownApplication(int id)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_DeleteGodownApplication", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Id", id);
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }



        public DataSet GetDocumentsByApplicationId(int ApplicationId)
        {
            DataSet dt = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetDocumentsByApplicationId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationId", DBFactoryParameterType.Int, ApplicationId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "GetDocumentsByApplicationId" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetSocietyDetails(string Type, int DistrictId, int BlockId, int CityId, int GPId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetGodownSocietyDetailsByType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.NVarchar, BlockId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.NVarchar, CityId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.NVarchar, GPId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = " public DataTable GetSocietyuDetails(string Type, int DistrictId, int BlockId, int CityId, int GPId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetGodownProposalSchemeId(string ListCode)
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


        public int UpdateAction(string selectedApplications, string Status)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateGodownAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Applications", DBFactoryParameterType.NVarchar, selectedApplications);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);


                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateAction(string selectedApplications, string Status)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        /// <summary>
        /// UpdateEsignStage
        /// </summary>
        /// <param name="selectedApplications"></param>
        /// <param name="isEsign"></param>
        /// <returns></returns>
        public int UpdateEsignStage(string selectedApplications, int isEsign)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateGodownEsignStage", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Applications", DBFactoryParameterType.NVarchar, selectedApplications);
                objDBAccess.AddParameterToCommand("@IsEsignStage", DBFactoryParameterType.NVarchar, isEsign);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateEsignStage(string selectedApplications, int isEsign)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string CommonASGenerate(StringBuilder sb)
        {
            string wkhtmlPath = HostingEnvironment.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(HostingEnvironment.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;

            try
            {
                string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {sb}
                    </body>
                   </html>";

                var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                string fileName = "AS_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdf);
                return filePath;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public string CommonFSGenerate(StringBuilder sb)
        {
            string wkhtmlPath = HostingEnvironment.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(HostingEnvironment.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
            try
            {
                string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}

                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {sb}
                    </body>
                   </html>";

                var pdf = PDFgenerator.GenerateFSPdfFromHtml(finalHtml, wkhtmlPath);
                string fileName = "FS_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdf);
                return filePath;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int UpdateASGeneratePath(int godownId, string formationPath)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_UpdateASGenerate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                objDBAccess.AddParameterToCommand("@ASGeneratePath", DBFactoryParameterType.NVarchar, formationPath);

                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateFormationPath" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        public int UpdateAsEsignProcess(int godownId)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_UpdateASGenerate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateAsEsignProcess(godownId:-" + godownId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public DataTable GetNewLovValuesForVacantDetailByListId(string ListCode)
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

        public string GenerateASPdf(int ApplicationId, int nextStatusId = 0)
        {
            string docName = string.Empty;
            string pdfPath = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/GodownASGenerate.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found."
                    });
                }

                DataRow dr = dt.Rows[0];
                string asGeneratePath = Convert.ToString(dr["ASGeneratePath"]);
                int statusId = Convert.ToInt32(dr["StatusId"]);

                // If AS already generated and status is 20
                if (!string.IsNullOrWhiteSpace(asGeneratePath) && statusId == 19 && nextStatusId == 20)
                {
                    string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");
                    docName = asGeneratePath;
                    string fullPath = Path.Combine(folderPath, docName);
                    pdfPath = fullPath;
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "ASGenerate Certificate",
                        DocName = docName,
                        ReturnPath = "/Godown/GodownFormationAction",
                        Location = "",
                        Designation = ""
                    });
                }

                // Replace Template Values
                strbld.Replace("##ASNo##", Convert.ToString(dr["ASNumber"]));
                strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                strbld.Replace("##District##", Convert.ToString(dr["DISTRICT_ENG"]));
                strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##SanctionAmt##", Convert.ToString(dr["SanctionAmt"]));
                strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                strbld.Replace("##LandSize1##", Convert.ToString(dr["LandSize1"]));
                strbld.Replace("##LandSize2##", Convert.ToString(dr["LandSize2"]));
                strbld.Replace("##TotalLandSize1##", Convert.ToString(dr["TotalLandSize1"]));
                strbld.Replace("##TotalLandSize2##", Convert.ToString(dr["TotalLandSize2"]));
                strbld.Replace("##Date##", DateTime.Now.ToString("dd/MM/yyyy"));
                strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                strbld.Replace("##MTCapacityName##", Convert.ToString(dr["MTCapacityName"]));
                strbld.Replace("##AvailabilityOfSocietyName##", Convert.ToString(dr["AvailabilityOfSocietyName"]));
                strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ApplicationNo"]));
                // Generate PDF
                pdfPath = objGodownModel.CommonASGenerate(strbld);
                docName = Path.GetFileName(pdfPath);

                // Update path & action only when next status is 19
                if (nextStatusId == 19)
                {
                    objGodownModel.UpdateASGeneratePath(ApplicationId, docName);
                    objGodownModel.UpdateAction(Convert.ToString(ApplicationId), Convert.ToString(nextStatusId));
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "ASGenerate Certificate",
                    DocName = docName,
                    ReturnPath = "/Godown/GodownFormationAction",
                    Location = "",
                    Designation = ""
                });
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateASPDF"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }

        #region Fs Generate and eSign for fs on 06-10-2026
        public int UpdateFsGeneratePath(int godownId, string formationPath)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_UpdateFSGenerate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                objDBAccess.AddParameterToCommand("@FsGeneratePath", DBFactoryParameterType.NVarchar, formationPath);

                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateFormationPath" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        public int UpdateFSEsignProcess(int godownId)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_FSEsignProcess", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, godownId);
                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "UpdateFSEsignProcess(godownId:-" + godownId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        public string GenerateCommonFSPdf(int ApplicationId, int nextStatusId = 0, string lovCode = "")
        {
            string responseData = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new { Success = false, Message = "Session expired." });
                }

                switch (lovCode)
                {
                    case "100":
                        responseData = GenerateFSBudgetGhosna100MTNewGss(ApplicationId, nextStatusId);
                        break;
                    case "500":
                        responseData = GenerateFSSwakritiAdhesh500MTDraft(ApplicationId, nextStatusId);
                        break;
                    case "250":
                        responseData = GenerateFSBudgetGhoshana250MT2Order(ApplicationId, nextStatusId);
                        break;
                    case "101":
                        responseData = GenerateFSBudgetGhosna100MTJinShiranFirstOrder(ApplicationId, nextStatusId);
                        break;
                    default:
                        responseData = null;
                        break;
                }
                return responseData;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateCommonFSPdf"
                }, ex);

                return JsonConvert.SerializeObject(new { Success = false, Message = "Error while generating FS PDF." });
            }
        }
        public string GenerateFSBudgetGhosna100MTNewGss(int ApplicationId, int nextStatusId = 0)
        {
            string docName = string.Empty;
            string pdfPath = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/BudgetGhosna100MTGssApprovaOrder.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found."
                    });
                }

                DataRow dr = dt.Rows[0];
                string fsGeneratePath = Convert.ToString(dr["FsGeneratePath"]);
                int statusId = Convert.ToInt32(dr["StatusId"]);

                // If AS already generated and status is 20
                if (!string.IsNullOrWhiteSpace(fsGeneratePath) && statusId == 21 && nextStatusId == 22)
                {
                    string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");
                    docName = fsGeneratePath;
                    string fullPath = Path.Combine(folderPath, docName);
                    pdfPath = fullPath;
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "100 FSGenerate Certificate",
                        DocName = docName,
                        ReturnPath = "/Godown/GodownFormationAction",
                        Location = "Jaipur",
                        Designation = "Register"
                    });
                }

                // Replace Template Values
                strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                strbld.Replace("##DistrictName##", Convert.ToString(dr["DISTRICT_ENG"]));
                strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ASNumber"]));
                strbld.Replace("##PanchayatSamiti##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##Vidhansabha##", Convert.ToString(dr["AssemblyName"]));
                strbld.Replace("##DivisionName##", Convert.ToString(dr["DivisionName"]));
                strbld.Replace("##Date##", DateTime.Now.ToString("dd-MM-yyyy"));
                strbld.Replace("##Logo##", HttpContext.Current.Server.MapPath("~/Content/Images/iyc_logo.png"));

                // Generate PDF
                pdfPath = objGodownModel.CommonFSGenerate(strbld);
                docName = Path.GetFileName(pdfPath);

                if (nextStatusId == 21)
                {
                    objGodownModel.UpdateFsGeneratePath(ApplicationId, docName);
                    objGodownModel.UpdateAction(Convert.ToString(ApplicationId), Convert.ToString(nextStatusId));
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "100 FSGenerate Certificate",
                    DocName = docName,
                    ReturnPath = "/Godown/GodownFormationAction",
                    Location = "Jaipur",
                    Designation = "Register"
                });
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateFS100MTFirstOrder"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }
        public string GenerateFSSwakritiAdhesh500MTDraft(int ApplicationId, int nextStatusId = 0)
        {
            string docName = string.Empty;
            string pdfPath = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/GodownFS_500MT_02_ApprovalOrder_Draft.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found."
                    });
                }

                DataRow dr = dt.Rows[0];
                string fsGeneratePath = Convert.ToString(dr["FsGeneratePath"]);
                int statusId = Convert.ToInt32(dr["StatusId"]);

                // If AS already generated and status is 20
                if (!string.IsNullOrWhiteSpace(fsGeneratePath) && statusId == 21 && nextStatusId == 22)
                {
                    string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");
                    docName = fsGeneratePath;
                    string fullPath = Path.Combine(folderPath, docName);
                    pdfPath = fullPath;
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "500 FSGenerate Certificate",
                        DocName = docName,
                        ReturnPath = "/Godown/GodownFormationAction",
                        Location = "Jaipur",
                        Designation = "Register"
                    });
                }

                // Replace Template Values
                strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                strbld.Replace("##DistrictName##", Convert.ToString(dr["DISTRICT_ENG"]));
                strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##SanctionAmt##", Convert.ToString(dr["SanctionAmt"]));
                strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ASNumber"]));
                strbld.Replace("##PanchayatSamiti##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##Vidhansabha##", Convert.ToString(dr["AssemblyName"]));
                strbld.Replace("##DivisionName##", Convert.ToString(dr["DivisionName"]));
                strbld.Replace("##Date##", DateTime.Now.ToString("dd-MM-yyyy"));
                strbld.Replace("##Logo##", HttpContext.Current.Server.MapPath("~/Content/Images/iyc_logo.png"));

                // Generate PDF
                pdfPath = objGodownModel.CommonFSGenerate(strbld);
                docName = Path.GetFileName(pdfPath);

                // Update path & action only when next status is 21
                if (nextStatusId == 21)
                {
                    objGodownModel.UpdateFsGeneratePath(ApplicationId, docName);
                    objGodownModel.UpdateAction(Convert.ToString(ApplicationId), Convert.ToString(nextStatusId));
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "500 FSGenerate Certificate",
                    DocName = docName,
                    ReturnPath = "/Godown/GodownFormationAction",
                    Location = "Jaipur",
                    Designation = "Register"
                });
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateFS500MTApprovalOrderDraft"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }
        public string GenerateFSBudgetGhoshana250MT2Order(int ApplicationId, int nextStatusId = 0)
        {
            string docName = string.Empty;
            string pdfPath = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/BudgetGhosna250MT2OrderApprovaOrder.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found."
                    });
                }

                DataRow dr = dt.Rows[0];
                string fsGeneratePath = Convert.ToString(dr["FsGeneratePath"]);
                int statusId = Convert.ToInt32(dr["StatusId"]);

                // If AS already generated and status is 20
                if (!string.IsNullOrWhiteSpace(fsGeneratePath) && statusId == 21 && nextStatusId == 22)
                {
                    string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");
                    docName = fsGeneratePath;
                    string fullPath = Path.Combine(folderPath, docName);
                    pdfPath = fullPath;
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "250 FSGenerate Certificate",
                        DocName = docName,
                        ReturnPath = "/Godown/GodownFormationAction",
                        Location = "Jaipur",
                        Designation = "Register"
                    });
                }

                // Replace Template Values
                strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                strbld.Replace("##DistrictName##", Convert.ToString(dr["DISTRICT_ENG"]));
                strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##SanctionAmt##", Convert.ToString(dr["SanctionAmt"]));
                strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ASNumber"]));
                strbld.Replace("##PanchayatSamiti##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##Vidhansabha##", Convert.ToString(dr["AssemblyName"]));
                strbld.Replace("##DivisionName##", Convert.ToString(dr["DivisionName"]));
                strbld.Replace("##Date##", DateTime.Now.ToString("dd-MM-yyyy"));
                strbld.Replace("##Logo##", HttpContext.Current.Server.MapPath("~/Content/Images/iyc_logo.png"));

                // Generate PDF
                pdfPath = objGodownModel.CommonFSGenerate(strbld);
                docName = Path.GetFileName(pdfPath);

                if (nextStatusId == 21)
                {
                    objGodownModel.UpdateFsGeneratePath(ApplicationId, docName);
                    objGodownModel.UpdateAction(Convert.ToString(ApplicationId), Convert.ToString(nextStatusId));
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "250 FSGenerate Certificate",
                    DocName = docName,
                    ReturnPath = "/Godown/GodownFormationAction",
                    Location = "Jaipur",
                    Designation = "Register"
                });
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateFS100MTFirstOrder"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }
        public string GenerateFSBudgetGhosna100MTJinShiranFirstOrder(int ApplicationId, int nextStatusId = 0)
        {
            string docName = string.Empty;
            string pdfPath = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/BudgetGhoshna_GodownFS_100MT_FirstOrder.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found."
                    });
                }

                DataRow dr = dt.Rows[0];
                string fsGeneratePath = Convert.ToString(dr["FsGeneratePath"]);
                int statusId = Convert.ToInt32(dr["StatusId"]);

                //If AS already generated and status is 20
                if (!string.IsNullOrWhiteSpace(fsGeneratePath) && statusId == 21 && nextStatusId == 22)
                {
                    string folderPath = HostingEnvironment.MapPath("~/Content/GodownDocument/");
                    docName = fsGeneratePath;
                    string fullPath = Path.Combine(folderPath, docName);
                    pdfPath = fullPath;
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "101 FSGenerate Certificate",
                        DocName = docName,
                        ReturnPath = "/Godown/GodownFormationAction",
                        Location = "Jaipur",
                        Designation = "Register"
                    });
                }

                // Replace Template Values
                strbld.Replace("##Address##", Convert.ToString(dr["Address"]));
                strbld.Replace("##DistrictName##", Convert.ToString(dr["DISTRICT_ENG"]));
                strbld.Replace("##Village##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##SanctionAmt##", Convert.ToString(dr["SanctionAmt"]));
                strbld.Replace("##PresidentName##", Convert.ToString(dr["PresidentName"]));
                strbld.Replace("##SecretaryName##", Convert.ToString(dr["SecretaryName"]));
                strbld.Replace("##CCBName##", Convert.ToString(dr["CCBName"]));
                strbld.Replace("##SocietyName##", Convert.ToString(dr["SocietyName"]));
                strbld.Replace("##GP_ENG##", Convert.ToString(dr["GP_ENG"]));
                strbld.Replace("##BLOCK_ENG##", Convert.ToString(dr["BLOCK_ENG"]));
                strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ASNumber"]));
                strbld.Replace("##PanchayatSamiti##", Convert.ToString(dr["VILLAGE_ENG"]));
                strbld.Replace("##Vidhansabha##", Convert.ToString(dr["AssemblyName"]));
                strbld.Replace("##DivisionName##", Convert.ToString(dr["DivisionName"]));
                strbld.Replace("##Date##", DateTime.Now.ToString("dd-MM-yyyy"));
                strbld.Replace("##Logo##", HttpContext.Current.Server.MapPath("~/Content/Images/iyc_logo.png"));
                // Generate PDF
                pdfPath = objGodownModel.CommonFSGenerate(strbld);
                docName = Path.GetFileName(pdfPath);

                // Update path & action only when next status is 21
                if (nextStatusId == 21)
                {
                    objGodownModel.UpdateFsGeneratePath(ApplicationId, docName);
                    objGodownModel.UpdateAction(Convert.ToString(ApplicationId), Convert.ToString(nextStatusId));
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "101 FSGenerate Certificate",
                    DocName = docName,
                    ReturnPath = "/Godown/GodownFormationAction",
                    Location = "Jaipur",
                    Designation = "Register"
                });
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "GodownController",
                    FunctionName = "GenerateFS500MTApprovalOrderDraft"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }

        #endregion
    }
}