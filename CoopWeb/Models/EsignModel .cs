using Cooperative.DBConnector;
using System;
using System.Data;
using System.Data.SqlClient;
using Cooperative.Controllers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static javax.net.ssl.SSLEngineResult;
using static iTextSharp.text.pdf.PdfDocument;
using iTextSharp.text.pdf.parser;

namespace Cooperative.Models
{
    public class EsignModel
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public EsignModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        #region CONFIG (UAT / PROD)
        private readonly string TokenApiUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_TokenApiUrl : GlobalConstant.Prod_TokenApiUrl;
        private readonly string SignedXmlApiUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_SignedXmlApiUrl : GlobalConstant.Prod_SignedXmlApiUrl;
        private readonly string SignPdfApiUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_SignPdfApiUrl : GlobalConstant.Prod_SignPdfApiUrl;
        private readonly string SignedXmlStatusApiUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_SignedXmlStatusApiUrl : GlobalConstant.Prod_SignedXmlStatusApiUrl;

        private readonly string SSOID = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_SSOID : GlobalConstant.Prod_SSOID;
        private readonly string SecretKey = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_SecretKey : GlobalConstant.Prod_SecretKey;
        private readonly string ApplicationCode = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_ApplicationCode : GlobalConstant.Prod_ApplicationCode;
        private readonly string AspId = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.Uat_AspId : GlobalConstant.Prod_AspId;
        #endregion



        #region 1️D GENERATE JWT TOKEN
        public async Task<TokenResponse> GenerateTokenAsync()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string auth = SSOID + ":" + SecretKey;
                string authBase = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase);
                    HttpResponseMessage response = await client.PostAsync(TokenApiUrl, null);
                    string jsonData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenResponse>(jsonData);
                }
            }
            catch (Exception ex)
            {
                return new TokenResponse
                {
                    status = "FAILED",
                    message = "Token generation failed",
                    exception = ex.Message
                };
            }
        }
        #endregion

        #region 2D GENERATE SIGNED XML
        public async Task<SignedXmlResponse> GenerateSignedXmlAsync(string pdfPath, string prn, string responseUrl, bool isSecondTime = false, string DocType = "", string location = "", string designation = "", string personName = "", bool isThirdTime = false)
        {
            string jsonResponse = string.Empty;
            try
            {
                TokenResponse token = await GenerateTokenAsync();
                if (token.status != "SUCCESS")
                    throw new Exception("JWT Token generation failed");

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.data.encryptedToken);
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    byte[] pdfBytes = File.ReadAllBytes(pdfPath);

                    form.Add(new ByteArrayContent(pdfBytes), "pdfFile", System.IO.Path.GetFileName(pdfPath));
                    form.Add(new StringContent(ApplicationCode), "applicationCode");
                    form.Add(new StringContent(AspId), "aspId");
                    //form.Add(new StringContent(isSecondTime ? "180" : "120"), "xcord");
                    //form.Add(new StringContent(isSecondTime ? "100" : "60"), "ycord");
                    if (isThirdTime)
                    {
                        form.Add(new StringContent("415"), "xcord");
                        form.Add(new StringContent("50"), "ycord");
                    }
                    if (isSecondTime)
                    {
                        form.Add(new StringContent("230"), "xcord");
                        form.Add(new StringContent("50"), "ycord");
                    }
                    else
                    {
                        form.Add(new StringContent("45"), "xcord");
                        form.Add(new StringContent("50"), "ycord");
                    }
                    form.Add(new StringContent(prn), "prn");
                    form.Add(new StringContent("ALL"), "signatureOnPageNumber");
                    
                    //if (DocType == "100MTFSGenerate Certificate" || DocType == "500MTFSGenerate Certificate")
                    //{
                    //    form.Add(new StringContent("Last"), "signatureOnPageNumber");
                    //}
                    //else
                    //{
                    //    form.Add(new StringContent("ALL"), "signatureOnPageNumber");
                    //}

                    if (DocType == "Citizen Certificate" || DocType == "Guarantor Certificate" || DocType == "Guarantor1 Certificate" || DocType == "Godown President")
                    { form.Add(new StringContent(personName), "personName"); }
                    else
                    { form.Add(new StringContent(_LoggedInUser.OfficerNameEn), "personName"); }
                    form.Add(new StringContent(string.IsNullOrWhiteSpace(_LoggedInUser.DesignationEn) ? designation : _LoggedInUser.DesignationEn), "personDesignation");
                    form.Add(new StringContent(location), "personLocation");
                    form.Add(new StringContent("M"), "signatureSize");
                    form.Add(new StringContent(responseUrl), "responseUrl");
                    HttpResponseMessage response = await client.PostAsync(SignedXmlApiUrl, form);
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SignedXmlResponse>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "EsignModel",
                    FunctionName = "public async Task<SignedXmlResponse> GenerateSignedXmlAsync(string pdfPath, string prn, string responseUrl, bool isSecondTime = false, string DocType = \"\")"
                }, ex);

                return null;
            }
        }
        #endregion

        #region 3️D SIGN PDF
        public async Task<SignPdfResponse> SignPdfAsync(string esignResponseBase64, string prn, string txnId)
        {
            DBLogger objDBLogger = new DBLogger();
            try
            {
                TokenResponse token = await GenerateTokenAsync();

                if (token.status != "SUCCESS")
                    throw new Exception("JWT Token generation failed");

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.data.encryptedToken); MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new StringContent(esignResponseBase64), "esignResponse");
                    form.Add(new StringContent(prn), "prn");
                    form.Add(new StringContent(txnId), "txnId");
                    form.Add(new StringContent(ApplicationCode), "applicationCode");
                    form.Add(new StringContent(AspId), "aspId");
                    HttpResponseMessage response = await client.PostAsync(SignPdfApiUrl, form);
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SignPdfResponse>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "EsignModel",
                    FunctionName = "public async Task<SignedXmlResponse> GenerateSignedXmlAsync(string pdfPath, string prn, string responseUrl, bool isSecondTime = false, string DocType = \"\")"
                }, ex);

                return null;
            }
        }
        #endregion

        #region 4️D GET SIGNED XML STATUS
        public async Task<SignedXmlResponse> GetSignedXmlStatusAsync(string prn, string txnId)
        {
            TokenResponse token = await GenerateTokenAsync();
            if (token.status != "SUCCESS")
                throw new Exception("JWT Token generation failed");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.data.encryptedToken);
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(ApplicationCode), "applicationCode");
                form.Add(new StringContent(prn), "prn");
                form.Add(new StringContent(txnId), "txnId");
                HttpResponseMessage response = await client.PostAsync(SignedXmlStatusApiUrl, form);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SignedXmlResponse>(jsonResponse);
            }
        }
        #endregion

        public DataTable GetTransactionByPRN(string prn)
        {
            dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetTransactionByPRN", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@prn", DBFactoryParameterType.NVarchar, prn);
                dt = objDBAccess.GetTable();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EsignModel", FunctionName = "GetTransactionByPRN(string prn)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int InsertEsignTransaction(string PRN, string PdfPath, string SSOID, long UserId, int RoleId, string Status, string DocType, string ReturnPath = "", string DocId = "", string SocietyIDEnc = "", string Remark = "", string documentName = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", string signedXMLData = "", string AuditBy = "", string AssignedIds = "", string IsFirm = "", int DistrictId = 0, string FinancialYearId = "", string FinancialYearName = "", string AuditTillDate = "", string IsForced = "", string AuditByDepartmentorCA = "", string DeleteResions = "", int UserRoleDeptMappingId = 0, string PurposeOfLoanOther = "", string LoanAmountRecommendedByDCS = "", string PaySummaryId = "", string DocIds = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertEsignTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PRN", DBFactoryParameterType.NVarchar, PRN);
                objDBAccess.AddParameterToCommand("@PdfPath", DBFactoryParameterType.NVarchar, PdfPath);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@DocType", DBFactoryParameterType.NVarchar, DocType);
                objDBAccess.AddParameterToCommand("@ReturnPath", DBFactoryParameterType.NVarchar, ReturnPath);
                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.NVarchar, DocId);
                objDBAccess.AddParameterToCommand("@DocIds", DBFactoryParameterType.NVarchar, DocIds);
                objDBAccess.AddParameterToCommand("@PVStage", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@signedXMLData", DBFactoryParameterType.NVarchar, signedXMLData);
                objDBAccess.AddParameterToCommand("@fileName", DBFactoryParameterType.NVarchar, documentName);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmendAmount", DBFactoryParameterType.NVarchar, AmendmendAmount);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, ReceiptNumber);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.NVarchar, AuditBy);
                objDBAccess.AddParameterToCommand("@AssignedIds", DBFactoryParameterType.NVarchar, AssignedIds);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.NVarchar, IsFirm);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.NVarchar, FinancialYearId);
                objDBAccess.AddParameterToCommand("@FinancialYearName", DBFactoryParameterType.NVarchar, FinancialYearName);
                objDBAccess.AddParameterToCommand("@AuditTillDate", DBFactoryParameterType.NVarchar, AuditTillDate);
                objDBAccess.AddParameterToCommand("@IsForced", DBFactoryParameterType.NVarchar, IsForced);
                objDBAccess.AddParameterToCommand("@AuditByDepartmentorCA", DBFactoryParameterType.NVarchar, AuditByDepartmentorCA);
                objDBAccess.AddParameterToCommand("@DeleteResions", DBFactoryParameterType.NVarchar, DeleteResions);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PurposeOfLoanOther", DBFactoryParameterType.NVarchar, PurposeOfLoanOther);
                objDBAccess.AddParameterToCommand("@LoanAmountRecommendedByDCS", DBFactoryParameterType.NVarchar, LoanAmountRecommendedByDCS);
                objDBAccess.AddParameterToCommand("@PaySummaryId", DBFactoryParameterType.VARCHAR, PaySummaryId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "InsertEsignTransaction(...)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int UpdateEsignStatus(string PRN, string Status, string TxnId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateEsignStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PRN", DBFactoryParameterType.NVarchar, PRN);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@TxnId", DBFactoryParameterType.NVarchar, TxnId);
                Result = Convert.ToInt32(objDBAccess.ExecuteCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "UpdateEsignStatus(string PRN=" + PRN + ",string Status=" + Status + ",string TxnId=" + TxnId + ")"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        public int InsertEsignApiLog(string PRN, string ApiType, string RequestData, string ResponseData, string ResponseCode, string esponseMessage, string IsSuccess)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertEsignApiLog", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PRN", DBFactoryParameterType.NVarchar, PRN);
                objDBAccess.AddParameterToCommand("@ApiType", DBFactoryParameterType.NVarchar, ApiType);
                objDBAccess.AddParameterToCommand("@RequestData", DBFactoryParameterType.NVarchar, RequestData);
                objDBAccess.AddParameterToCommand("@ResponseData", DBFactoryParameterType.NVarchar, ResponseData);
                objDBAccess.AddParameterToCommand("@ResponseCode", DBFactoryParameterType.NVarchar, ResponseCode);
                objDBAccess.AddParameterToCommand("@ResponseMessage", DBFactoryParameterType.NVarchar, esponseMessage);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, IsSuccess);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "InsertEsignApiLog"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int UpdateSignedPdfPath(string PRN, string SignedPdfPath)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateSignedPdfPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PRN", DBFactoryParameterType.NVarchar, PRN);
                objDBAccess.AddParameterToCommand("@SignedPdfPath", DBFactoryParameterType.NVarchar, SignedPdfPath);
                Result = Convert.ToInt32(objDBAccess.ExecuteCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "UpdateSignedPdfPath(string PRN,string SignedPdfPath)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        /// <summary>
        /// GetLatestSignedTransaction 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EsignTransactionEntity GetLatestSignedTransaction(long userId)
        {
            EsignTransactionEntity result = null;
            DataTable dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetLatestSignedTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, userId);
                dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    result = new EsignTransactionEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        PRN = row["PRN"].ToString(),
                        TxnId = row["TxnId"].ToString(),
                        PdfPath = row["PdfPath"].ToString(),
                        SignedPdfPath = row["SignedPdfPath"].ToString(),
                        SSOID = row["SSOID"].ToString(),
                        UserId = Convert.ToInt64(row["UserId"]),
                        RoleId = Convert.ToInt32(row["RoleId"]),
                        Status = row["Status"].ToString(),
                        CreatedOn = row["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["CreatedOn"]),
                        UpdatedOn = row["UpdatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["UpdatedOn"])
                    };
                }

                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "GetLatestSignedTransaction(long userId)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        /// <summary>
        /// GetLatestFailedTransaction
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EsignTransactionEntity GetLatestFailedTransaction(long userId)
        {
            EsignTransactionEntity result = null;
            DataTable dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetLatestFailedTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, userId);
                dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    result = new EsignTransactionEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        PRN = row["PRN"].ToString(),
                        TxnId = row["TxnId"].ToString(),
                        PdfPath = row["PdfPath"].ToString(),
                        SignedPdfPath = row["SignedPdfPath"].ToString(),
                        SSOID = row["SSOID"].ToString(),
                        UserId = Convert.ToInt64(row["UserId"]),
                        RoleId = Convert.ToInt32(row["RoleId"]),
                        Status = row["Status"].ToString(),
                        DocType = row["DocType"].ToString(),
                        ReturnPath = row["ReturnPath"].ToString(),
                        CreatedOn = row["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["CreatedOn"]),
                        UpdatedOn = row["UpdatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["UpdatedOn"])
                    };
                }

                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.EsignModel",
                    FunctionName = "GetLatestFailedTransaction(long userId : '" + userId + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public DataSet GetTransactionByPRNNo(string prn)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetTransactionByPRNNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@prn", DBFactoryParameterType.NVarchar, prn);
                ds = objDBAccess.GetDataSet();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EsignModel", FunctionName = "GetTransactionByPRN(string prn)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataTable GetXmlResponseByPRN(string prn)
        {
            dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetXmlResponseByPRN", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@prn", DBFactoryParameterType.NVarchar, prn);
                dt = objDBAccess.GetTable();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EsignModel", FunctionName = "GetTransactionByPRN(string prn)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public string MergeMultiplePdfFiles(string pdfPath, string dirName)
        {
            string FinalUrl = string.Empty;

            try
            {
                var filePathParts = pdfPath.Split('#');

                List<string> pdfFiles = filePathParts.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + x)).ToList();

                string mergedFolder = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + dirName);
                if (!Directory.Exists(mergedFolder))
                {
                    Directory.CreateDirectory(mergedFolder);
                }

                string mergedFileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string mergedPhysicalPath = System.IO.Path.Combine(mergedFolder, mergedFileName);

                using (FileStream stream = new FileStream(mergedPhysicalPath, FileMode.Create))
                {
                    Document document = new Document();
                    PdfCopy pdfCopy = new PdfCopy(document, stream);
                    document.Open();

                    foreach (string file in pdfFiles)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            using (PdfReader reader = new PdfReader(file))
                            {
                                for (int page = 1; page <= reader.NumberOfPages; page++)
                                {
                                    pdfCopy.AddPage(pdfCopy.GetImportedPage(reader, page));
                                }
                            }
                        }
                    }

                    document.Close();
                }

                FinalUrl = mergedPhysicalPath;


                return FinalUrl;
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }
    }



    public class TokenResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string exception { get; set; }
        public TokenData data { get; set; }
    }

    public class TokenData
    {
        public string ssoId { get; set; }
        public string jwtId { get; set; }
        public string encryptedToken { get; set; }
    }

    public class SignedXmlResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string DecodedSignedXml { get; set; }
        public SignedXmlData data { get; set; }
    }

    public class SignedXmlData
    {
        public string responseCode { get; set; }
        public string responseMsg { get; set; }
        public string signedXMLData { get; set; }
        public string prn { get; set; }
        public string txnId { get; set; }
    }

    public class SignPdfResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public SignPdfData data { get; set; }
    }

    public class SignPdfData
    {
        public string responseCode { get; set; }
        public string responseMsg { get; set; }
        public string signedPDFBase64 { get; set; }
        public string prn { get; set; }
        public string txnId { get; set; }
    }

    public class EsignTransactionEntity
    {
        public int Id { get; set; }
        public string PRN { get; set; }
        public string TxnId { get; set; }
        public string PdfPath { get; set; }
        public string SignedPdfPath { get; set; }
        public string SSOID { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string DocType { get; set; }
        public string ReturnPath { get; set; }
    }
    public class ResponseDataModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string RetrunValue { get; set; }
    }
    public class UserRequestModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public bool Status { get; set; }
    }

    public class EsignTransactionResponseModel
    {
        public string DocType { get; set; } = string.Empty;
        public string ResStatus { get; set; } = string.Empty;
        public string SocietyIDEnc { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool IsAmendment { get; set; } = false;
        public string AmendmendAmount { get; set; } = string.Empty;
        public string ReceiptNumber { get; set; } = string.Empty;
        public int StatusId { get; set; } = 0;
        public string Response { get; set; } = string.Empty;
        public string JsonData { get; set; } = string.Empty;
        public long UserId { get; set; } = 0;
        public string AuditBy { get; set; } = string.Empty;
        public string AssignedIds { get; set; } = string.Empty;
        public string IsFirm { get; set; } = string.Empty;
        public string DistrictId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public string FinancialYearName { get; set; } = string.Empty;
        public string AuditTillDate { get; set; } = string.Empty;
        public string IsForced { get; set; } = string.Empty;
        public string AuditByDepartmentorCA { get; set; } = string.Empty;
        public string DeleteResions { get; set; } = string.Empty;
        public string TxnId { get; set; } = "";
        public string PdfPath { get; set; } = "";
        public int RoleId { get; set; } = 0;
        public string SSOID { get; set; } = "";
        public string DocId { get; set; } = "";
        public string Remark { get; set; } = "";
        public string ReturnPath { get; set; } = "";
        public string ResponseMsg { get; set; } = "";
        public string UserRoleDeptMappingId { get; set; }
        public string PRN { get; set; }
        public string PurposeOfLoanOther { get; set; }
        public string LoanAmountRecommendedByDCS { get; set; }
        public string PaySummaryId { get; set; }
        public string DocIds { get; set; }

    }
}