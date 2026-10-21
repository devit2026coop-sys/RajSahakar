using Cooperative.DBConnector;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Cooperative.Controllers;
using java.util;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using static Cooperative.Models.GopalCreditCardModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using sun.swing;
using System.Web;

namespace Cooperative.Models
{
    public class BeneficiaryModel
    {
        public int Action { get; set; }
        public string BeneficiaryDataId { get; set; }
        public string RegNo { get; set; }
        public string FarmerName { get; set; }
        public string DateOfBirth { get; set; }
        public string FatherName { get; set; }
        public string AadhaarHash { get; set; }
        public string MobileNo { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string TownCode { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string LandSeedingStatus { get; set; }
        public string HomeAddress { get; set; }
        public string PinCode { get; set; }
        public string Gender { get; set; }
        public string FarmerCategory { get; set; }
        public string GID { get; set; }
        public string TownName { get; set; }
        public string Religion { get; set; }
        public string Email { get; set; }
        public string IsDiscontinued { get; set; }
        public string RegDate { get; set; }
        public string Eligiblity { get; set; }
        public string PMKisanInstallmentNo { get; set; }
        public string Quadrimester { get; set; }
        public string FinancialYear { get; set; }
        public string MultiplePaymentFlag { get; set; }
        public string DistrictId { get; set; }
        public string RegNoEnc
        {
            set
            {
                value = COMMON_FUNCTION.Encrypt(this.RegNo);
            }
        }
        public string PFMSBeneficiaryCode { get; set; }
    }

    public class BeneficiaryInfoModel
    {
        public string FarmerName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string RegNo { get; set; }
        public string DistrictName { get; set; }
        public string AadhaarNo { get; set; }
        public string SubDistrictName { get; set; }
        public string VillageName { get; set; }
        public int LandSeedingStatus { get; set; }
        public string HomeAddress { get; set; }
        public bool IsDiscontinued { get; set; }
        public string MobileNo { get; set; }
        public string StateName { get; set; }
        public string WardName { get; set; }
        public bool Eligiblity { get; set; }
        public string RegDate { get; set; }
    }

    public class BeneficiaryTransactionInfo
    {
        public string AadharNo { get; set; }
        public string JanAadharNo { get; set; }
        public string JanAadharMID { get; set; }
        public string RegistrationNo { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSCCode { get; set; }
        public Int32 Amount { get; set; }
        public Int64 TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string CreditStatus { get; set; }
    }

    public class DbtTransModel
    {
        public string CreditAmount { get; set; }
        public string CreditBankCode { get; set; }
        public string CreditRemarks { get; set; }
        public string CreditStatus { get; set; }
        public string CreditName { get; set; }
        public string IFSC { get; set; }
        public string InstallmentType { get; set; }
        public string LotOpen { get; set; }
        public string PaymentStatus { get; set; }
        public string BankName { get; set; }
        public string CreditedAccount { get; set; }
        public string CreditDate { get; set; }
        public string UtrNo { get; set; }
        public string TransactionFailedReason { get; set; }
        public string FtoPendingReason { get; set; }
    }
    public class CMK_StatusTrackerSummaryModel
    {
        public string W_Today { get; set; }
        public string W_Yesterday { get; set; }
        public string W_ThisMonth { get; set; }
        public string W_LastMonth { get; set; }
        public string W_ThisYear { get; set; }
        public string W_LastYear { get; set; }
        public string W_AllTime { get; set; }
        public string E_Today { get; set; }
        public string E_Yesterday { get; set; }
        public string E_ThisMonth { get; set; }
        public string E_LastMonth { get; set; }
        public string E_ThisYear { get; set; }
        public string E_LastYear { get; set; }
        public string E_AllTime { get; set; }
    }
    public class PMKisanDashboardModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public int FinancialYearId { get; set; }
        public float TotalBeneficiary { get; set; }
        public float TotalEligibleBeneficiary { get; set; }
        public float TotalMoneyGiven { get; set; }
        public List<PMKisanQuadrimesterModel> QuadrimesterList { get; set; }

        public PMKisanDashboardModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PMKisanDashboardModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }


        public DataSet GetPMK_DashboardData(int financialYearId = 0, int installmentType = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@InstallmentType", DBFactoryParameterType.Int, installmentType);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "dashboard");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetPMK_DashboardData(int financialYearId = 0, int installmentType = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetPMK_QuadrimesterData()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "QuadrimesterData");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GetPMK_QuadrimesterData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }



        public DataSet GetBeneficiaryList(int financialYearId = 0, int districtId = 0, string subDistrictName = "", string villageName = "", int installmentType = 0, string locationType = "district")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                // Action
                //string actionName = "GetCMK_BeneficiaryList";
                string actionName = "";

                if (locationType == "district" || locationType == "subdistrict" || locationType == "Village")
                {
                    actionName = "GetCMK_BeneficiaryList_ByLocationType";
                }
               

                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, subDistrictName);
                objDBAccess.AddParameterToCommand("@InstallmentType", DBFactoryParameterType.Int, installmentType);             
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionName);
                // flag
                objDBAccess.AddParameterToCommand("@LocationType", DBFactoryParameterType.VARCHAR, locationType);
                objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.VARCHAR, villageName); 

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

        public DataSet GetBeneficiaryDetailList(ref DataPagingModel Details, int financialYearId = 0, int installmentType = 0, int districtId = 0, string subDistrictName = "", string villageName = "", int type = 1, bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, financialYearId);
                objDBAccess.AddParameterToCommand("@InstallmentType", DBFactoryParameterType.Int, installmentType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, subDistrictName);
                objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.VARCHAR, villageName);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, type);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCMK_BeneficiaryDetailList");
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
        public DataSet GetBeneficiaryListForAudit(ref DataPagingModel Details, int installmentType = 0, int districtId = 0, string subDistrictName = "",  string searchText = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetCMK_BeneficiaryDataForAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InstallmentType", DBFactoryParameterType.Int, installmentType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, subDistrictName);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.VARCHAR, searchText);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "public DataTable GetBeneficiaryListForAudit(ref DataPagingModel Details, int installmentType = 0, int districtId = 0, string subDistrictName = \"\",  string searchText = \"\", bool isExport = false)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataTable Get_CMK_AadharInfo(string GID)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_BeneficiaryJanaadharUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ACTION", DBFactoryParameterType.VARCHAR, "_Get");
                objDBAccess.AddParameterToCommand("@GID", DBFactoryParameterType.VARCHAR, GID);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanDashboardModel", FunctionName = "Get_CMK_AadharInfo()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataTable Get_CMK_DbtTransactionByInstallmentId(int installmentId = 0, int pageNumber = 1, int pageSize = 200000)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_GetDBTTransactionByInstallmentId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_Get");
                objDBAccess.AddParameterToCommand("@InstallmentId", DBFactoryParameterType.Int, installmentId);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanDashboardModel", FunctionName = "Get_CMK_DbtTransactionByInstallmentId(int installmentId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public bool Update_CMK_JanaadharInfo(CMK_JanaadharModel cMK_Janaadhar)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_BeneficiaryJanaadharUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ACTION", DBFactoryParameterType.VARCHAR, "_Update");
                objDBAccess.AddParameterToCommand("@BeneficiaryDataId", DBFactoryParameterType.VARCHAR, cMK_Janaadhar.BeneficiaryDataId);
                objDBAccess.AddParameterToCommand("@AADHAAR_NO", DBFactoryParameterType.VARCHAR, cMK_Janaadhar.AADHAAR_NO);
                objDBAccess.AddParameterToCommand("@MEMBER_ID", DBFactoryParameterType.VARCHAR, cMK_Janaadhar.MEMBER_ID);
                objDBAccess.AddParameterToCommand("@JAN_AADHAR", DBFactoryParameterType.VARCHAR, cMK_Janaadhar.JAN_AADHAR);
                var result = objDBAccess.ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "Update_CMK_JanaadharInfo(CMK_JanaadharModel cMK_Janaadhar)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }

        public string PushPMKisanTransactionToJanaadhar_Staging(BeneficiaryTransactionInfo _Model = null)
        {
            string response = "";
            try
            {
                string scheme = "CMKISAN_DBT";
                string Url = "http://172.21.245.45/Service/action/transaction";


                StringBuilder requestXml = new StringBuilder();
                requestXml.Append("<root>");
                requestXml.Append("<Transaction>");
                requestXml.Append($"<entitlementId>{_Model.RegistrationNo}</entitlementId>");
                requestXml.Append($"<entitlementMemId>{_Model.JanAadharMID}</entitlementMemId>");
                requestXml.Append($"<janaadhaarId>{_Model.JanAadharNo}</janaadhaarId>");
                requestXml.Append($"<janaadhaarMemId>{_Model.JanAadharMID}</janaadhaarMemId>");
                requestXml.Append($"<transactionId>{_Model.TransactionId}</transactionId>");
                requestXml.Append($"<dueTransactionId>{_Model.TransactionId}</dueTransactionId>");
                requestXml.Append($"<aadharNo>{_Model.AadharNo}</aadharNo>");
                requestXml.Append("<eid></eid>");
                requestXml.Append($"<bankAccNo>{_Model.BankAccountNo}</bankAccNo>");
                requestXml.Append($"<ifsc>{_Model.BankIFSCCode}</ifsc>");
                requestXml.Append("<micr></micr>");
                requestXml.Append($"<paymentAmount>{_Model.Amount}</paymentAmount>");
                requestXml.Append($"<paymentDate>{_Model.TransactionDate}</paymentDate>");
                requestXml.Append($"<status>{_Model.CreditStatus}</status>");
                requestXml.Append("</Transaction>");
                requestXml.Append("</root>");

                // URL-encode the XML
                string encodedXml = Uri.EscapeDataString(requestXml.ToString());

                string postData = $"scheme={scheme}&reqXml={encodedXml}";
                byte[] dataBytes = Encoding.UTF8.GetBytes(postData);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = dataBytes.Length;

                using (Stream os = req.GetRequestStream())
                {
                    os.Write(dataBytes, 0, dataBytes.Length);
                }

                using (WebResponse resp = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        response = sr.ReadToEnd().Trim();
                    }
                }

                var a = InsertJanaadharPushDataStatus(_Model.TransactionId, requestXml.ToString(), response);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.PMKisanModel",
                    FunctionName = "PushPMKisanTransactionToJanaadhar_Staging"
                }, ex);
            }

            return response;
        }

        public async Task<string> PushPMKisanTransactionToJanaadhar(BeneficiaryTransactionInfo _Model = null)
        {
            string responseString = "";
            try
            {
                string scheme = "CMKISAN_DBT";
                string JanAdhaarClientID = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
                string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Transaction/Detail?client_id=" + JanAdhaarClientID;

                StringBuilder requestXml = new StringBuilder();
                requestXml.Append("<root>");
                requestXml.Append($"<scheme>{scheme}</scheme>");
                requestXml.Append("<Transaction>");
                requestXml.Append($"<entitlementId>{_Model.RegistrationNo}</entitlementId>");
                requestXml.Append($"<entitlementMemId>{_Model.JanAadharMID}</entitlementMemId>");
                requestXml.Append($"<janaadhaarId>{_Model.JanAadharNo}</janaadhaarId>");
                requestXml.Append($"<janaadhaarMemId>{_Model.JanAadharMID}</janaadhaarMemId>");
                requestXml.Append($"<transactionId>{_Model.TransactionId}</transactionId>");
                requestXml.Append($"<dueTransactionId>{_Model.TransactionId}</dueTransactionId>");
                requestXml.Append($"<aadharNo>{_Model.AadharNo}</aadharNo>");
                requestXml.Append("<eid></eid>");
                requestXml.Append($"<bankAccNo>{_Model.BankAccountNo}</bankAccNo>");
                requestXml.Append($"<ifsc>{_Model.BankIFSCCode}</ifsc>");
                requestXml.Append("<micr></micr>");
                requestXml.Append($"<paymentAmount>{_Model.Amount}</paymentAmount>");
                requestXml.Append($"<paymentDate>{_Model.TransactionDate}</paymentDate>");
                requestXml.Append($"<status>{_Model.CreditStatus}</status>");
                requestXml.Append("</Transaction>");
                requestXml.Append("</root>");
                var content = new StringContent(requestXml.ToString(), Encoding.UTF8, "application/xml");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    responseString = await response.Content.ReadAsStringAsync();

                    var a = InsertJanaadharPushDataStatus(_Model.TransactionId, requestXml.ToString(), responseString, (responseString == null && responseString.Contains("isSaved>Y") ? 1 : 0));

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.PMKisanModel",
                    FunctionName = "PushPMKisanTransactionToJanaadhar"
                }, ex);
            }
            return responseString;
        }

        public int InsertJanaadharPushDataStatus(Int64 TransactionId, string Request = "", string Response = "", int status = 0)
        {
            int Result = 0;
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_InsertPMK_JanaadharTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TransactionId", DBFactoryParameterType.BigInt, TransactionId);
                objDBAccess.AddParameterToCommand("@Response", DBFactoryParameterType.NVarchar, Response);
                objDBAccess.AddParameterToCommand("@Request", DBFactoryParameterType.NVarchar, Request);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, status);


                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = " InsertJanaadharPushDataStatus(Int64TransactionId,string Request=\"\", string Response =\"\"" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable Get_CMK_JanAadhaarCsvDataByInstallmentId(int installmentId = 0, int pageNumber = 1, int pageSize = 200000)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_JanAadhaarCsvData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InstallmentId", DBFactoryParameterType.Int, installmentId);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "Get_CMK_JanAadhaarCsvDataByInstallmentId(int installmentId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public bool CMK_GenerateCsv(DataTable dtCmk)
        {
            try
            {
                string filePath = "", filePathMapped = "";
                string pathDir = HttpContext.Current.Server.MapPath($"~/Content/PMKisan/BeneficiaryData/");
                if (!Directory.Exists(pathDir))
                    Directory.CreateDirectory(pathDir);
                filePath = $"CMKISAN_TRANSACTION_DATA_{DateTime.Now.ToString("yyyyMMdd_hhmm")}.csv";
                filePathMapped = Path.Combine(pathDir, filePath);
                return GeneratePipeSeparatedCsv(dtCmk, filePathMapped);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "CMK_GenerateCsv(DataTable dtCmk)" }, ex);
                return false;
            }
        }
        public bool GeneratePipeSeparatedCsv(DataTable dtCmk, string filePath)
        {
            try
            {
                var sb = new StringBuilder();
                var columnNames = dtCmk.Columns.Cast<DataColumn>()
                                            .Select(col => col.ColumnName);
                sb.AppendLine(string.Join("|", columnNames));

                foreach (DataRow row in dtCmk.Rows)
                {
                    var fields = row.ItemArray.Select(field =>
                    {
                        if (field == null || field == DBNull.Value)
                            return "";

                        string value = field.ToString();

                        if (value.Contains("|"))
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";

                        return value;
                    });

                    sb.AppendLine(string.Join("|", fields));
                }

                File.WriteAllText(filePath, sb.ToString());
                return true;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PMKisanModel", FunctionName = "GeneratePipeSeparatedCsv(DataTable dtCmk, string filePath)" }, ex);
                return false;
            }

        }


    }
    public class PMKisanQuadrimesterModel
    {
        public int FinancialYearId { get; set; }
        public int QuadrimesterId { get; set; }
        public string Quadrimester { get; set; }
        public float TotalMoneyGiven { get; set; }
    }
    public class ResponseFormatModel
    {
        public DataFormatModel d { get; set; }
    }

    public class DataFormatModel
    {
        public string _type { get; set; }
        public string output { get; set; }
    }
    public class CMK_JanaadharModel
    {
        public string AADHAAR_NO { get; set; }
        public string JAN_AADHAR { get; set; }
        public string MEMBER_ID { get; set; }
        public string BeneficiaryDataId { get; set; }
    }

}