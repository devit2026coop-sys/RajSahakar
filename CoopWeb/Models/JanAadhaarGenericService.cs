using Cooperative.Controllers;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static Cooperative.Models.GopalCreditCardModel;

namespace Cooperative.JanadhaarClasses
{
    public class JanAadhaarGenericService
    {
        private readonly HttpClient _httpClient;
        private readonly CryptoHelper _crypto;

        public JanAadhaarGenericService()
        {
            var privateCert = new X509Certificate2(
                                    JanAadhaarConfig.PrivateCertPath,
                                    JanAadhaarConfig.PrivateCertPassword,
                                    X509KeyStorageFlags.MachineKeySet |
                                    X509KeyStorageFlags.PersistKeySet |
                                    X509KeyStorageFlags.Exportable
                                );
            var publicCert = CryptoHelper.LoadCertificate(JanAadhaarConfig.PublicCertPath);

            _crypto = new CryptoHelper(publicCert.GetRSAPublicKey(), privateCert.GetRSAPrivateKey());
            _httpClient = new HttpClient();
        }

        public async Task<string> CallApiAsync(string endpoint, object dataPayload)
        {
            try
            {
                var payload = new { data = dataPayload };
                string json = JsonConvert.SerializeObject(payload);
                string signature = _crypto.SignData(json);

                var finalPayload = new
                {
                    data = dataPayload,
                    signature = signature
                };

                COMMON_FUNCTION.Sp_callTrackingInsert("Janaadhaar Payload=", JsonConvert.SerializeObject(finalPayload));
                string encryptedData = _crypto.EncryptDataWithAES(JsonConvert.SerializeObject(finalPayload));

                var request = new { data = encryptedData };
                string requestBody = JsonConvert.SerializeObject(request);
                ServicePointManager.SecurityProtocol =
                        SecurityProtocolType.Tls13 |
                        SecurityProtocolType.Tls12 |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls;

                _httpClient.DefaultRequestHeaders.Clear();

                string apiUrl = $"{JanAadhaarConfig.BaseUrl}{endpoint}?client_id={JanAadhaarConfig.ClientId}";
                if (!(System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost")
                    && !(System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")
                    && endpoint == "validate-otp")
                    apiUrl = $"{JanAadhaarConfig.JanaadharProdValidateOtpUrl}{endpoint}?client_id={JanAadhaarConfig.ClientId}";

                var response = await _httpClient.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                string encryptedResponse = await response.Content.ReadAsStringAsync();

                string encryptedRespData = JObject.Parse(encryptedResponse)["data"]?.ToString();
                string decryptedResponse = CryptoHelper.DecryptDataWithAES(encryptedRespData);
                COMMON_FUNCTION.Sp_callTrackingInsert("Janaadhaar decryptedResponse=", decryptedResponse);

                return decryptedResponse;
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = false,
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }

        public async Task<string> getResponseData(string SchemeName, string Action, string JanaadhaarNo, string memberId = "", string tid = "", string otp = "", DBT_Transaction dbt = null)
        {
            ///*
            string schemShortCode = "RAJSAHAKAR";
            string appCode = "PJAN6268201";
            if (((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")))
            {
                appCode = "JAN6268201";
            }
            string jsonString = "";

            try
            {

                if (SchemeName == "coop") { schemShortCode = "coop"; }
                else if (SchemeName == "GCC") { schemShortCode = "gcc"; }
                else if (SchemeName == "pmk") { schemShortCode = "pmk"; }
                schemShortCode = "RAJSAHAKAR";
                Random rnd = new Random();
                int sixDigitNumber = rnd.Next(100000, 1000000);
                string transactionId = SchemeName + DateTime.Now.ToString("yyyyMMdd") + sixDigitNumber;

                dynamic requestObj = null;
                string UrlDataType = "";

                if (Action == "FetchMemberList")
                {
                    requestObj = new
                    {
                        data = new
                        {
                            appCode = appCode,
                            schemShortCode = schemShortCode,
                            transactionId = transactionId,
                            janId = JanaadhaarNo
                        }
                    };
                    UrlDataType = "member-list";
                }
                else if (Action == "GenerateOTP")
                {
                    requestObj = new
                    {
                        data = new
                        {
                            appCode = appCode,
                            schemShortCode = schemShortCode,
                            transactionId = transactionId,
                            memberId = memberId
                        }
                    };

                    UrlDataType = "generate-otp";
                }
                else if (Action == "ValidateOTP_FetchRequestedData")
                {
                    requestObj = new
                    {
                        data = new
                        {
                            appCode = appCode,
                            schemShortCode = schemShortCode,
                            transactionId = transactionId,
                            memberId = memberId,
                            tid = tid,
                            otp = otp
                        }
                    };
                    UrlDataType = "validate-otp";
                }
                else if (Action == "JanAadaarDetailByAadhaar")
                {
                    requestObj = new
                    {
                        data = new
                        {
                            appCode = appCode,
                            schemShortCode = schemShortCode,
                            transactionId = transactionId,
                            janId = JanaadhaarNo
                        }
                    };
                    UrlDataType = "jandtl-by-refid";
                }
                else if (Action == "pushDBTCashTransaction")
                {
                    requestObj = new
                    {
                        data = new
                        {
                            transactionId = dbt.transactionId,
                            schemeCode = dbt.schemeCode,
                            entitlementId = dbt.entitlementId,
                            entitlementMemId = dbt.entitlementMemId,
                            janaadhaarId = dbt.janaadhaarId,
                            janaadhaarMemId = dbt.janaadhaarMemId,
                            transactionMode = dbt.transactionMode,
                            aadharNo = dbt.aadharNo,
                            bankAccNo = dbt.bankAccNo,
                            ifsc = dbt.ifsc,
                            micr = dbt.micr,
                            paymentAmount = dbt.paymentAmount,
                            paymentDate = dbt.paymentDate

                        }
                    };
                    UrlDataType = "pushCashTransation";
                }

                string DecResponseString = await CallApiAsync(UrlDataType, requestObj.data);


                if (1 == 2 && Action == "pushDBTCashTransaction")
                {
                    DecResponseString = @"
                        {
                          ""response"": {
                            ""status"": true,
                            ""message"": ""Transaction successful"",
                            ""responseCode"": ""JAN_200"",
                            ""transactionId"": ""TXN000123"",
                            ""schemeCode"": ""SCM001""
                          },
                          ""signature"": ""<RSA_Signature>""
                        }";

                }

                return DecResponseString;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.JanAadhaarGenericService", FunctionName = "getResponseData " + jsonString }, ex);
                return ""; ;
            }

        }

        public async Task<string> GenerateOTP(string memberId, string SchemeName)
        {
            try
            {
                string DecResponseString = await getResponseData(SchemeName, "GenerateOTP", "", memberId);
                RootResponse parsedResponse = JsonConvert.DeserializeObject<RootResponse>(DecResponseString);
                string resultjson = new JavaScriptSerializer().Serialize(parsedResponse.response);
                return resultjson;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.JanAadhaarGenericService",
                    FunctionName = "GenerateOTP"
                }, ex);
                return ex.Message;
            }

        }
        public async Task<OTPResponse> ValidateOTP_FetchRequestedData(string memberId, string SchemeName, string tid, string otp, string SocietyDirectory = "", string janAadharNo = "")
        {
            try
            {
                string DecResponseString = await getResponseData(SchemeName, "ValidateOTP_FetchRequestedData", "", memberId, tid, otp);
                OTPResponse parsedResponse = JsonConvert.DeserializeObject<OTPResponse>(DecResponseString);
                return parsedResponse;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CommonFunctionJanaadhaar",
                    FunctionName = "ValidateOTP_FetchRequestedData"
                }, ex);
                return null;
            }
        }

        public async Task<string> PushGCCTransactionToJanaadhar(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int result = 0)
        {
            string responseString = "";

            string dateTime = DateTime.Now.ToString("dd/MM/yyyy");
            _Model.JanAadharNo = csCommon.Decrypt_SA(_Model.JanAadharNo);
            _Model.AadharNo = COMMON_FUNCTION.Decrypt(_Model.AadharNoEnc);
            DBT_Transaction dBTTransaction = new DBT_Transaction();
            dBTTransaction.transactionId = result.ToString(); ;
            dBTTransaction.schemeCode = "CO_GKCC";
            dBTTransaction.entitlementId = _Model.RegistrationNo;
            dBTTransaction.entitlementMemId = _Model.JanAadharMID;
            dBTTransaction.janaadhaarId = _Model.JanAadharNo;
            dBTTransaction.janaadhaarMemId = _Model.JanAadharMID;
            dBTTransaction.transactionMode = "AD/AC";
            dBTTransaction.aadharNo = _Model.AadharNo;
            dBTTransaction.bankAccNo = _Model.BankAccountNo;
            dBTTransaction.ifsc = _Model.CurrentBankIFSCCode;
            dBTTransaction.micr = "";
            dBTTransaction.paymentAmount = _AppFlowModel.LoanAmountRecommendedByDCS.ToString();
            dBTTransaction.paymentDate = dateTime;

            responseString = await dbtTransactions(dBTTransaction, "GCC");
            return responseString;
        }


        public async Task<string> PushPMKisanTransactionToJanaadhar(BeneficiaryTransactionInfo _Model = null)
        {
            string responseString = "";
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy");
            DBT_Transaction dBTTransaction = new DBT_Transaction();
            dBTTransaction.transactionId = _Model.TransactionId.ToString(); ;
            dBTTransaction.schemeCode = "CMKISAN_DBT";
            dBTTransaction.entitlementId = _Model.RegistrationNo;
            dBTTransaction.entitlementMemId = _Model.JanAadharMID;
            dBTTransaction.janaadhaarId = _Model.JanAadharNo;
            dBTTransaction.janaadhaarMemId = _Model.JanAadharMID;
            dBTTransaction.transactionMode = "AD/AC";
            dBTTransaction.aadharNo = _Model.AadharNo;
            dBTTransaction.bankAccNo = _Model.BankAccountNo;
            dBTTransaction.ifsc = _Model.BankIFSCCode;
            dBTTransaction.micr = "";
            dBTTransaction.paymentAmount = _Model.Amount.ToString();
            dBTTransaction.paymentDate = _Model.TransactionDate;

            responseString = await dbtTransactions(dBTTransaction, "PMK");
            return responseString;

        }


        public async Task<string> dbtTransactions(DBT_Transaction dBTTransaction, string scheme = "")
        {
            string DecResponseString = "", jsonStr = "";
            try
            {
                jsonStr = JsonConvert.SerializeObject(dBTTransaction);
                if (scheme == "GCC")
                {
                    DecResponseString = await getResponseData("GCC", "pushDBTCashTransaction", "", "", "", "", dBTTransaction);
                    GopalCreditCardDBModel gopalCreditCardDBModel = new GopalCreditCardDBModel();
                    var a = gopalCreditCardDBModel.InsertPushDataStatus(dBTTransaction.Id, jsonStr, DecResponseString);
                }
                else if (scheme == "PMK")
                {
                    DecResponseString = await getResponseData("PMK", "pushDBTCashTransaction", "", "", "", "", dBTTransaction);
                    PMKisanDashboardModel pmkisanmodel = new PMKisanDashboardModel(new FileLogger());
                    var a1 = pmkisanmodel.InsertJanaadharPushDataStatus(Convert.ToInt32(dBTTransaction.transactionId), jsonStr, DecResponseString, (DecResponseString == null && DecResponseString.Contains("isSaved>Y") ? 1 : 0));

                }
                return DecResponseString;
            }

            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.commonfunctionJanaadhaar",
                    FunctionName = "dbtTransactions"
                }, ex);
            }
            return DecResponseString;

        }
    }
}
