using CoopDAL;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.Models;
using FactoryAndBoiler.Payment;
using iTextSharp.text.pdf.codec;
using java.lang;
using javax.swing.plaf;
using javax.xml.transform;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using sun.security.x509;
using sun.swing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml;
using static Cooperative.Models.CoopModel;
using static Cooperative.Models.GopalCreditCardModel;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Controllers
{
    public class eSignController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        EsignModel eSignModel = new EsignModel(new DBLogger());
        DBFactoryMSSQL objDBAccess = new DBFactoryMSSQL();
        DBLogger objDBLogger = new DBLogger();

        //Redirect URL 

        private readonly string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
        private readonly string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RedirectESPportal(string prnNo)
        {
            string xmlResponse = string.Empty;
            if (!string.IsNullOrEmpty(prnNo))
            {
                DataTable dt = eSignModel.GetXmlResponseByPRN(prnNo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    xmlResponse = dt.Rows[0]["signedXMLData"]?.ToString();
                }
            }
            ViewBag.response = xmlResponse;
            return View();
        }
        public ActionResult EsignSuccess(string prnNo)
        {
            DataTable prnTbl = eSignModel.GetTransactionByPRN(prnNo);

            long UserId = Convert.ToInt64(prnTbl.Rows[0]["UserId"]);
            int RoleId = Convert.ToInt32(prnTbl.Rows[0]["RoleId"]);
            string SSO_ID = prnTbl.Rows[0]["SSOID"].ToString();
            string PRN = prnTbl.Rows[0]["PRN"].ToString();
            string UserRoleDeptMappingId = prnTbl.Rows[0]["UserRoleDeptMappingId"].ToString();

            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            UserEntity _User = _db.LoginBySsoRole(SSO_ID, UserRoleDeptMappingId);
            if (_User == null)
            {
                return Redirect("https://sso.rajasthan.gov.in/");
            }

            string SSOID = _User.SSOID;
            string IMGUrl = _User.ImageUrl;
            string AadharNo = _User.AadharNo;
            string AadharRefId = _User.AadharRefId;
            if (_User != null)
            {
                //Session.Abandon();
                _User.KIOSKCODE = "";
                _User.UserType = "D";
                _User.ImageUrl = IMGUrl;
                _User.AadharNo = AadharNo;
                _User.AadharRefId = AadharRefId;
                Session[SessionIdentity.Admin] = _User;
                if (Request.Cookies[SessionIdentity.Culture] == null)
                {
                    Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                    {
                        Value = "en-us",
                        HttpOnly = true
                    });
                }
                regenerateId();
            }
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }
            return View();
        }

        public ActionResult EsignFailed(string prnNo)
        {
            DataTable prnTbl = eSignModel.GetTransactionByPRN(prnNo);

            long UserId = Convert.ToInt64(prnTbl.Rows[0]["UserId"]);
            int RoleId = Convert.ToInt32(prnTbl.Rows[0]["RoleId"]);
            string SSO_ID = prnTbl.Rows[0]["SSOID"].ToString();
            string PRN = prnTbl.Rows[0]["PRN"].ToString();
            string UserRoleDeptMappingId = prnTbl.Rows[0]["UserRoleDeptMappingId"].ToString();

            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            UserEntity _User = _db.LoginBySsoRole(SSO_ID, UserRoleDeptMappingId);
            if (_User == null)
            {
                return Redirect("https://sso.rajasthan.gov.in/");
            }

            string SSOID = _User.SSOID;
            string IMGUrl = _User.ImageUrl;
            string AadharNo = _User.AadharNo;
            string AadharRefId = _User.AadharRefId;
            if (_User != null)
            {
                //Session.Abandon();
                _User.KIOSKCODE = "";
                _User.UserType = "D";
                _User.ImageUrl = IMGUrl;
                _User.AadharNo = AadharNo;
                _User.AadharRefId = AadharRefId;
                Session[SessionIdentity.Admin] = _User;
                if (Request.Cookies[SessionIdentity.Culture] == null)
                {
                    Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                    {
                        Value = "en-us",
                        HttpOnly = true
                    });
                }
                regenerateId();
            }
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }

            return View();
        }

        public static string GeneratePRN()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        public JsonResult ValidateEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null)
        {
            try
            {
                if ((DocType == "Citizen Certificate" || DocType == "Objective Certificate" || DocType == "Amendment Member Certificate" || DocType == "OTS Certificate" || DocType == "BankOpening Certificate"
                    || DocType == "WRSummary" || DocType == "Ware House" || DocType == "Member Certificate" || DocType == "Amendment Bylaws" || DocType == "Bylaws Certificate" || DocType == "GCC Certificate"
                    || DocType == "CMKisan" || DocType == "NOC Certificate" || DocType == "MPacs" || DocType == "Audit Order" || DocType == "Certificate Issue" || DocType == "Amendment Certificate"
                    || DocType == "MokaReport Certificate" || DocType == "GCCPACS Certificate" || DocType == "Guarantor Certificate" || DocType == "Guarantor1 Certificate" || DocType == "CCS Complainant" || DocType == "Godown Document" || DocType == "Trainning Certificate" || DocType == "ASGenerate Certificate" || DocType == "AGM Certificate" || DocType == "500 FSGenerate Certificate" || DocType == "250 FSGenerate Certificate" || DocType == "100 FSGenerate Certificate" || DocType == "101 FSGenerate Certificate") && _LoggedInUser == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session timeout, login again."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(DocumentPath) || string.IsNullOrWhiteSpace(DocumentName))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid document."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(DocumentPath))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Document not found."
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region STEP 1 - START ESIGN
        [HttpPost]
        public async Task<JsonResult> StartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", string Location = "", string Desination = "", string PurposeOfLoanOther = "")
        {
            DBLogger objDBLogger = new DBLogger();
            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = string.Empty;
                    switch (DocType)
                    {
                        case "Ware House":
                            pdfPath = Server.MapPath(DocumentPath) + "/" + DocumentName;
                            break;
                        case "OTS Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "CMKisan":
                            pdfPath = DocumentPath;
                            break;
                        case "Certificate Issue":
                            pdfPath = DocumentPath;
                            break;
                        case "Member Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Objective Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Bylaws Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Amendment Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Amendment Member Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Amendment Bylaws":
                            pdfPath = DocumentPath;
                            break;
                        case "NOC Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "MokaReport Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "GCCPACS Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Guarantor Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Guarantor1 Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Godown Document":
                            pdfPath = DocumentPath;
                            break;
                        case "Godown BranchManager":
                            pdfPath = DocumentPath;
                            break;
                        case "Godown President":
                            pdfPath = DocumentPath;
                            break;
                        case "Trainning Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "ASGenerate Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "Trainning Invoice":
                            pdfPath = DocumentPath;
                            break;
                        case "CCS NoticeIssue":
                            pdfPath = DocumentPath;
                            break;
                        case "AGM Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "250 FSGenerate Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "500 FSGenerate Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "100 FSGenerate Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "101 FSGenerate Certificate":
                            pdfPath = DocumentPath;
                            break;
                        default:
                            pdfPath = Server.MapPath(DocumentPath);
                            break;
                    }
                    string prn = GeneratePRN();
                    string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/" : "https://rajsahakarapp.rajasthan.gov.in/";
                    ReturnPath = baseUrl + ReturnPath;
                    var existing = eSignModel.GetTransactionByPRN(prn);
                    if (existing.Rows.Count > 0)
                    {
                        return Json(new { success = false, message = "Transaction already exists." });
                    }

                    // INSERT TRANSACTION
                    eSignModel.InsertEsignTransaction(prn, pdfPath, _LoggedInUser.SSOID, _LoggedInUser.UserId, _LoggedInUser.RoleId, "INITIATED", DocType, ReturnPath, docid, SocietyIDEnc, Remark, DocumentName, StatusId, IsAmendment, AmendmendAmount, ReceiptNumber, "", "", "", "", _LoggedInUser.DistrictId, "", "", "", "", "", "", _LoggedInUser.UserRoleDeptMappingId, PurposeOfLoanOther);

                    SignedXmlResponse signedXmlResp;
                    bool isSecond = (DocType == "Guarantor1 Certificate" || DocType == "Godown BranchManager") ? true : false;
                    bool isThiredTime = DocType == "Godown President" ? true : false;

                    signedXmlResp = await eSignModel.GenerateSignedXmlAsync(pdfPath, prn, responseUrl, isSecond, DocType, Location, Desination, Remark, isThiredTime);
                    // LOG XML GENERATION API
                    eSignModel.InsertEsignApiLog(prn, "XML_GENERATE", "PDFPath: " + pdfPath, JsonConvert.SerializeObject(signedXmlResp), signedXmlResp?.data?.responseCode, signedXmlResp?.data?.responseMsg, signedXmlResp?.status);

                    if (signedXmlResp != null && signedXmlResp.status == "SUCCESS")
                    {
                        // UPDATE STATUS
                        eSignModel.UpdateEsignStatus(prn, "XML_GENERATED", signedXmlResp.data.txnId);

                        return Json(new
                        {
                            success = true,
                            xml = Encoding.UTF8.GetString(Convert.FromBase64String(signedXmlResp.data.signedXMLData))
                        });
                    }
                    else
                    {
                        eSignModel.UpdateEsignStatus(prn, "FAILED", null);
                        return Json(new
                        {
                            success = false,
                            //message = "XML Generation Failed"
                            message = "File generation failed due to a server error. Please try again later.",
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "Session out !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region STEP 2 - ESP CALLBACK
        [ValidateInput(false)]
        public async Task<ActionResult> EspResponse()
        {
            EsignTransactionResponseModel responseModel = new EsignTransactionResponseModel();
            try
            {
                string esignData = Request.Unvalidated.Form["esignData"];

                if (string.IsNullOrEmpty(esignData))
                    return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });

                string decodedXml = esignData;

                System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(decodedXml);
                string prn = doc.Root?.Attribute("txn")?.Value ?? "";

                if (string.IsNullOrEmpty(prn))
                    return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });


                DataTable prnTbl = eSignModel.GetTransactionByPRN(prn);

                if (prnTbl.Rows.Count == 0)
                    return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });

                responseModel.TxnId = prnTbl.Rows[0]["TxnId"].ToString();
                responseModel.PdfPath = prnTbl.Rows[0]["PdfPath"].ToString();
                responseModel.UserId = Convert.ToInt64(prnTbl.Rows[0]["UserId"]);
                responseModel.RoleId = Convert.ToInt32(prnTbl.Rows[0]["RoleId"]);
                responseModel.SSOID = prnTbl.Rows[0]["SSOID"].ToString();
                responseModel.DocId = prnTbl.Rows[0]["DocId"].ToString();
                responseModel.DocIds = prnTbl.Rows[0]["DocIds"].ToString();
                responseModel.DocType = prnTbl.Rows[0]["DocType"].ToString();
                responseModel.Remark = prnTbl.Rows[0]["PVStage"].ToString();
                responseModel.ReturnPath = prnTbl.Rows[0]["ReturnPath"].ToString();
                responseModel.ResponseMsg = prnTbl.Rows[0]["ResponseMessage"].ToString();
                responseModel.ResStatus = prnTbl.Rows[0]["Status"].ToString();
                responseModel.FileName = prnTbl.Rows[0]["fileName"].ToString();
                responseModel.IsAmendment = Convert.ToBoolean(prnTbl.Rows[0]["IsAmendment"]);
                responseModel.AmendmendAmount = prnTbl.Rows[0]["AmendmendAmount"].ToString();
                responseModel.ReceiptNumber = prnTbl.Rows[0]["ReceiptNumber"].ToString();
                responseModel.StatusId = Convert.ToInt32(prnTbl.Rows[0]["StatusId"]);
                responseModel.SocietyIDEnc = prnTbl.Rows[0]["SocietyIDEnc"].ToString();
                responseModel.JsonData = prnTbl.Rows[0]["signedXMLData"].ToString();
                responseModel.AuditBy = prnTbl.Rows[0]["AuditBy"].ToString();
                responseModel.AssignedIds = prnTbl.Rows[0]["AssignedIds"].ToString();
                responseModel.IsFirm = prnTbl.Rows[0]["IsFirm"].ToString();
                responseModel.DistrictId = prnTbl.Rows[0]["DistrictId"].ToString();
                responseModel.FinancialYearId = prnTbl.Rows[0]["FinancialYearId"].ToString();
                responseModel.FinancialYearName = prnTbl.Rows[0]["FinancialYearName"].ToString();
                responseModel.AuditTillDate = prnTbl.Rows[0]["AuditTillDate"].ToString();
                responseModel.IsForced = prnTbl.Rows[0]["IsForced"].ToString();
                responseModel.AuditByDepartmentorCA = prnTbl.Rows[0]["AuditByDepartmentorCA"].ToString();
                responseModel.DeleteResions = prnTbl.Rows[0]["DeleteResions"].ToString();
                responseModel.UserRoleDeptMappingId = prnTbl.Rows[0]["UserRoleDeptMappingId"].ToString();
                responseModel.PRN = prnTbl.Rows[0]["PRN"].ToString();
                responseModel.PurposeOfLoanOther = prnTbl.Rows[0]["PurposeOfLoanOther"].ToString();
                responseModel.LoanAmountRecommendedByDCS = prnTbl.Rows[0]["LoanAmountRecommendedByDCS"].ToString();
                responseModel.PaySummaryId = prnTbl.Rows[0]["PaySummaryId"].ToString();



                UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
                UserEntity _User = _db.LoginBySsoRole(responseModel.SSOID, responseModel.UserRoleDeptMappingId);
                string SSOID = _User.SSOID;
                string IMGUrl = _User.ImageUrl;
                string AadharNo = _User.AadharNo;
                string AadharRefId = _User.AadharRefId;
                if (_User != null)
                {
                    //Session.Abandon();
                    _User.KIOSKCODE = "";
                    _User.UserType = "D";
                    _User.ImageUrl = IMGUrl;
                    _User.AadharNo = AadharNo;
                    _User.AadharRefId = AadharRefId;
                    Session[SessionIdentity.Admin] = _User;
                    if (Request.Cookies[SessionIdentity.Culture] == null)
                    {
                        Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                        {
                            Value = "en-us",
                            HttpOnly = true
                        });
                    }
                    regenerateId();
                }

                eSignModel.UpdateEsignStatus(prn, "ESP_RETURNED", responseModel.TxnId);

                string encodedXml = Convert.ToBase64String(Encoding.UTF8.GetBytes(esignData));
                // Call Sign PDF API
                SignPdfResponse signPdfResp = await eSignModel.SignPdfAsync(encodedXml, prn, responseModel.TxnId);

                // LOG PDF SIGN API
                eSignModel.InsertEsignApiLog(prn, "PDF_SIGN", "Base64 XML received", JsonConvert.SerializeObject(signPdfResp), signPdfResp?.data?.responseCode, signPdfResp?.data?.responseMsg, signPdfResp?.status);

                if (signPdfResp != null && signPdfResp.status == "SUCCESS" && signPdfResp.data.responseCode == "E_0001")
                {
                    byte[] signedPdf = Convert.FromBase64String(signPdfResp.data.signedPDFBase64);
                    System.IO.File.WriteAllBytes(responseModel.PdfPath, signedPdf);
                    eSignModel.UpdateSignedPdfPath(prn, responseModel.PdfPath);
                    try
                    {
                        if (responseModel.DocType == "BankOpening Certificate")
                        {
                            responseModel.Response = BankOpeningDetailUpdate(responseModel.SocietyIDEnc, responseModel.FileName, responseModel.StatusId, responseModel.Remark, responseModel.UserId, responseModel.IsAmendment, responseModel.AmendmendAmount, responseModel.ReceiptNumber);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Certificate Issue")
                        {
                            var ds = UpdateEsignStage(responseModel.SocietyIDEnc, 2, responseModel.FileName);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Member Certificate")
                        {
                            var ds = UpdateEsignStage(responseModel.SocietyIDEnc, 3, responseModel.FileName);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }

                        else if (responseModel.DocType == "Bylaws Certificate")
                        {
                            DataSet ds = UpdateEsignStage(responseModel.SocietyIDEnc, 4, responseModel.FileName);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                if ((Convert.ToInt32(dr["ApplicationStatus"]) == 1058 || Convert.ToInt32(dr["ApplicationStatus"]) == 40) && Convert.ToInt32(dr["ActId"]) == 3)
                                {
                                    if (Convert.ToInt32(dr["IsEsignStage"]) == 4 && !string.IsNullOrWhiteSpace(dr["RegistrationCertificateFile"].ToString()) && !string.IsNullOrWhiteSpace(dr["SOCIETIESAUTHORIZEDPERSONS"].ToString()))
                                    {
                                        responseModel.Response = CertificateIssuesWorkFlow(responseModel.SocietyIDEnc, dr["RegistrationCertificateFile"].ToString(), dr["SOCIETIESAUTHORIZEDPERSONS"].ToString(),
                                            responseModel.StatusId, responseModel.Remark, responseModel.UserRoleDeptMappingId, responseModel.IsAmendment, responseModel.AmendmendAmount, responseModel.ReceiptNumber, responseModel.SSOID);
                                        return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                                    }
                                }
                                //else if (Convert.ToInt32(dr["IsEsignStage"]) == 4 && !string.IsNullOrWhiteSpace(dr["RegistrationCertificateFile"].ToString()) && !string.IsNullOrWhiteSpace(dr["SOCIETIESAUTHORIZEDPERSONS"].ToString()) && Convert.ToInt32(dr["ActId"]) == 7)
                                //{
                                //    responseModel.Response = CertificateIssuesWorkFlow(responseModel.SocietyIDEnc, dr["RegistrationCertificateFile"].ToString(), dr["SOCIETIESAUTHORIZEDPERSONS"].ToString(),
                                //        responseModel.StatusId, responseModel.Remark, responseModel.UserRoleDeptMappingId, responseModel.IsAmendment, responseModel.AmendmendAmount, responseModel.ReceiptNumber, responseModel.SSOID);
                                //    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                                //}
                            }
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Objective Certificate")
                        {
                            DataSet ds = UpdateEsignStage(responseModel.SocietyIDEnc, 5, responseModel.FileName);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                if (Convert.ToInt32(dr["IsEsignStage"]) == 5 && !string.IsNullOrWhiteSpace(dr["RegistrationCertificateFile"].ToString()) && !string.IsNullOrWhiteSpace(dr["SOCIETIESAUTHORIZEDPERSONS"].ToString()) &&
                                    !string.IsNullOrWhiteSpace(dr["RulesRegulationFile"].ToString()) && !string.IsNullOrWhiteSpace(dr["ObjectsFile"].ToString()))
                                {
                                    responseModel.Response = CertificateIssuesWorkFlow(responseModel.SocietyIDEnc, dr["RegistrationCertificateFile"].ToString(), dr["SOCIETIESAUTHORIZEDPERSONS"].ToString(),
                                        responseModel.StatusId, responseModel.Remark, responseModel.UserRoleDeptMappingId, responseModel.IsAmendment, responseModel.AmendmendAmount, responseModel.ReceiptNumber, responseModel.SSOID);
                                    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                                }
                            }
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Ware House")
                        {
                            string nameWithoutExtension = responseModel.FileName.Split('.')[0];
                            responseModel.Response = WareHouseeSignWorkFlow(nameWithoutExtension, responseModel.JsonData);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "WRSummary")
                        {
                            responseModel.Response = WRSummaryeSignWorkFlow(responseModel.FileName, responseModel.JsonData, responseModel.Remark, responseModel.SocietyIDEnc);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "OTS Certificate")
                        {
                            responseModel.Response = OTSeSignWorkFlow(responseModel.JsonData, responseModel.FileName);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "CCS Complainant")
                        {
                            responseModel.Response = CCSComplainteSignWorkFlow(responseModel.JsonData, responseModel.FileName, responseModel.SocietyIDEnc);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "CCS NoticeIssue")
                        {
                            string response = CCSNoticeIssueWorkFlow(responseModel.JsonData, responseModel.FileName, responseModel.SocietyIDEnc);
                            if (response == "Success")
                            { return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN }); }
                            else
                            { return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN }); }
                        }
                        else if (responseModel.DocType == "CMKisan")
                        {
                            responseModel.Response = CMKishaneSignWorkFlow(responseModel.SocietyIDEnc, responseModel.FileName, responseModel.UserId);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Audit Order" && responseModel.AmendmendAmount == "0")
                        {

                            responseModel.Response = AuditDepartmentWorkFlow(responseModel);

                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Audit Order" && responseModel.AmendmendAmount == "1")
                        {
                            responseModel.Response = AuditAssignByDeptWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });

                        }
                        else if (responseModel.DocType == "Auditor Department" && responseModel.AmendmendAmount == "0")
                        {
                            responseModel.Response = AuditAssignByDepartmentNotRequest(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Auditor Department" && responseModel.AmendmendAmount == "1")
                        {
                            responseModel.Response = AuditAssignByDepartment(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "MPacs")
                        {
                            responseModel.Response = MPacsWorkFlow(responseModel.JsonData, responseModel.Remark, responseModel.FileName, responseModel.StatusId.ToString());
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Amendment Certificate")
                        {
                            var ds = UpdateAmendmentEsignStage(responseModel.SocietyIDEnc, 2, responseModel.FileName, Convert.ToInt32(responseModel.DocId));
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        //else if (responseModel.DocType == "Amendment Member Certificate")
                        //{
                        //    var ds = UpdateAmendmentEsignStage(responseModel.SocietyIDEnc, 3, responseModel.FileName, Convert.ToInt32(responseModel.DocId));
                        //    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        //}

                        //else if (responseModel.DocType == "Amendment Bylaws")
                        //{
                        //    var ds = UpdateAmendmentEsignStage(responseModel.SocietyIDEnc, 4, responseModel.FileName, Convert.ToInt32(responseModel.DocId));
                        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        DataRow dr = ds.Tables[0].Rows[0];
                        //        if (Convert.ToInt32(dr["IsEsignStage"]) == 4 && !string.IsNullOrWhiteSpace(dr["RegistrationCertificateFile"].ToString()) && !string.IsNullOrWhiteSpace(dr["RulesRegulationFile"].ToString()) &&
                        //            !string.IsNullOrWhiteSpace(dr["MemberCertificate"].ToString()))
                        //        {
                        //            responseModel.Response = AmendmentWorkFlow(responseModel.SocietyIDEnc, dr["RegistrationCertificateFile"].ToString(), dr["MemberCertificate"].ToString(), dr["RulesRegulationFile"].ToString(),
                        //                responseModel.StatusId, responseModel.Remark, responseModel.UserId, Convert.ToInt32(responseModel.DocId), responseModel.AmendmendAmount, responseModel.ReceiptNumber);
                        //            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        //        }
                        //    }
                        //    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        //}
                        else if (responseModel.DocType == "Amendment Member Certificate")
                        {
                            var ds = UpdateAmendmentEsignStage(responseModel.SocietyIDEnc, 3, responseModel.FileName, Convert.ToInt32(responseModel.DocId));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                if (Convert.ToInt32(dr["IsEsignStage"]) == 3 && !string.IsNullOrWhiteSpace(dr["MemberCertificate"].ToString()))
                                {
                                    responseModel.Response = AmendmentWorkFlow(responseModel.SocietyIDEnc, "", dr["MemberCertificate"].ToString(), "",
                                        responseModel.StatusId, responseModel.Remark, responseModel.UserId, Convert.ToInt32(responseModel.DocId), responseModel.AmendmendAmount, responseModel.ReceiptNumber);
                                    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                                }
                            }
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }

                        else if (responseModel.DocType == "Amendment Bylaws")
                        {
                            var ds = UpdateAmendmentEsignStage(responseModel.SocietyIDEnc, 4, responseModel.FileName, Convert.ToInt32(responseModel.DocId));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                if (Convert.ToInt32(dr["IsEsignStage"]) == 4 && !string.IsNullOrWhiteSpace(dr["RulesRegulationFile"].ToString()))
                                {
                                    responseModel.Response = AmendmentWorkFlow(responseModel.SocietyIDEnc, dr["RegistrationCertificateFile"].ToString(), "", dr["RulesRegulationFile"].ToString(),
                                        responseModel.StatusId, responseModel.Remark, responseModel.UserId, Convert.ToInt32(responseModel.DocId), responseModel.AmendmendAmount, responseModel.ReceiptNumber);
                                    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                                }
                            }
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "NOC Certificate")
                        {
                            responseModel.Response = NOCCertificateWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }

                        else if (responseModel.DocType == "MokaReport Certificate")
                        {
                            responseModel.Response = GCCMokaReportFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "GCC Certificate" || responseModel.DocType == "ReSanction Certificate")
                        {
                            responseModel.Response = GCCCertificateWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Citizen Certificate" || responseModel.DocType == "GCCPACS Certificate" || responseModel.DocType == "Guarantor Certificate" || responseModel.DocType == "Guarantor1 Certificate")
                        {
                            responseModel.Response = GCCWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Godown Document" || responseModel.DocType == "Godown BranchManager" || responseModel.DocType == "Godown President" || responseModel.DocType == "ASGenerate Certificate" || responseModel.DocType == "250 FSGenerate Certificate" || responseModel.DocType == "500 FSGenerate Certificate" || responseModel.DocType == "100 FSGenerate Certificate" || responseModel.DocType == "101 FSGenerate Certificate")
                        {
                            string resultMessage = GodownWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "Trainning Certificate" || responseModel.DocType == "Trainning Invoice")
                        {
                            string resultMessage = TrainingWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }
                        else if (responseModel.DocType == "AGM Certificate")
                        {
                            string resultMessage = AGMWorkFlow(responseModel);
                            return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                        }

                    }
                    catch (System.Exception ex)
                    {
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "eSignController", FunctionName = "public async Task<ActionResult> EspResponse() catch" }, ex);
                        return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });
                    }

                    return RedirectToAction("EsignSuccess", new { prnNo = responseModel.PRN });
                }
                else
                {
                    eSignModel.UpdateEsignStatus(prn, "FAILED", responseModel.TxnId);
                    return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });
                }
            }
            catch (System.Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "eSignController", FunctionName = "public async Task<ActionResult> EspResponse() ESP Response catch" }, ex);
                return RedirectToAction("EsignFailed", new { prnNo = responseModel.PRN });

            }
        }

        #endregion

        void regenerateId()
        {
            SessionIDManager manager = new SessionIDManager();
            string oldId = manager.GetSessionID(System.Web.HttpContext.Current);
            string newId = manager.CreateSessionID(System.Web.HttpContext.Current);
            bool isAdd = false, isRedir = false;
            manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedir, out isAdd);
            HttpApplication ctx = (HttpApplication)System.Web.HttpContext.Current.ApplicationInstance;
            HttpModuleCollection mods = ctx.Modules;
            SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
            FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase store = null;
            FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
            foreach (FieldInfo field in fields)
            {
                if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
                if (field.Name.Equals("_rqId")) rqIdField = field;
                if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
                if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
            }
            object lockId = rqLockIdField.GetValue(ssm);
            if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(System.Web.HttpContext.Current, oldId, lockId);
            rqStateNotFoundField.SetValue(ssm, true);
            rqIdField.SetValue(ssm, newId);
        }

        public JsonResult GetLatestEsignStatus()
        {
            long _userid = 0;
            if (_LoggedInUser != null)
            {
                _userid = _LoggedInUser.UserId;
            }
            var data = eSignModel.GetLatestSignedTransaction(_userid);
            if (data == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new { PRN = data.PRN, TxnId = data.TxnId, Status = data.Status, DocPath = data.PdfPath, ReturnPath = data.ReturnPath }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLatestFailedTransaction()
        {
            long _userid = 0;
            if (_LoggedInUser != null)
            {
                _userid = _LoggedInUser.UserId;
            }
            var data = eSignModel.GetLatestFailedTransaction(_userid);

            if (data == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                PRN = data.PRN,
                Status = data.Status,
                DocType = data.DocType,
                ReturnPath = data.ReturnPath,
                Message = "Digital signing was not completed."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLatestTransactionByPRN(string prnNo = "")
        {
            var responce = new APIResponce();
            try
            {
                var ds = eSignModel.GetTransactionByPRNNo(prnNo);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        result = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Transaction not found.";
                }
            }
            catch (System.Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error." + ex.Message;
                // Log error
            }

            return Json(new { result = responce }, JsonRequestBehavior.AllowGet);
        }

        public string[] StringSplit(string StringToSplit, string Delimitator)
        {
            return StringToSplit.Split(new[] { Delimitator }, StringSplitOptions.None);
        }

        #region WorkFlow Start 
        private string BankOpeningDetailUpdate(string SocietyIDEnc, string fileName, int StatusId, string Remark, long UserId, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            ApplicationFlowModel _Modal = new ApplicationFlowModel();
            int result = 0;
            SocietyModel SM = new SocietyModel();
            SocietyRegistrationEntity _EntitySRE = new SocietyRegistrationEntity();
            _EntitySRE.SocietyIDEnc = SocietyIDEnc;
            _EntitySRE.flag = 2;
            _EntitySRE.BankOpeningCertificate = fileName;
            result = Convert.ToInt32(SM.SaveBankDetailmodel(_EntitySRE));
            if (result > 0)
            {
                var ds = UpdateEsignStage(SocietyIDEnc, 1, "");
            }

            if (result > 0)
            {
                result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment);

                if (result == 1)
                {
                    RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                    DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIDEnc);

                    if (dtr != null && dtr.Rows.Count > 0)
                    {
                        RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                        rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                        rajNiveshStatus.STATUS_CODE = Convert.ToInt32(dtr.Rows[0]["STATUS_CODE"].ToString());
                        rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                        rajNiveshStatus.REMARKS = dtr.Rows[0]["REMARKS"].ToString();
                        rajNivesh.UpdateStatus(rajNiveshStatus);
                    }
                }
                return "Success";
            }
            else
            { return result.ToString(); }

        }

        private string CertificateIssuesWorkFlow(string SocietyIDEnc, string certificateFile, string memberCertificateFile, int StatusId, string Remark, string UserId, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", string SSOId = "")
        {
            ApplicationFlowModel modal = new ApplicationFlowModel();
            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            UserEntity _User = _db.LoginBySsoRole(SSOId, UserId);
            string SSOID = _User.SSOID;
            string IMGUrl = _User.ImageUrl;
            string AadharNo = _User.AadharNo;
            string AadharRefId = _User.AadharRefId;
            if (_User != null)
            {
                //Session.Abandon();
                _User.KIOSKCODE = "";
                _User.UserType = "D";
                _User.ImageUrl = IMGUrl;
                _User.AadharNo = AadharNo;
                _User.AadharRefId = AadharRefId;
                Session[SessionIdentity.Admin] = _User;
                if (Request.Cookies[SessionIdentity.Culture] == null)
                {
                    Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                    {
                        Value = "en-us",
                        HttpOnly = true
                    });
                }
                regenerateId();
            }

            int result = modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, 0, IsAmendment, AmendmendAmount, ReceiptNumber);

            if (result == 0)
            {
                return "Fail";
            }

            ApplicationFlowController appFlow = new ApplicationFlowController();
            string response = appFlow.SaveCertificate(SocietyIDEnc, certificateFile, memberCertificateFile, true);
            if (string.IsNullOrEmpty(response) || response == "Fail" || response == "-1")
            {
                return "Fail";
            }
            if (result == 1)
            {
                RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIDEnc);

                if (dtr != null && dtr.Rows.Count > 0)
                {
                    RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                    rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                    rajNiveshStatus.STATUS_CODE = Convert.ToInt32(dtr.Rows[0]["STATUS_CODE"].ToString());
                    rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                    rajNiveshStatus.REMARKS = dtr.Rows[0]["REMARKS"].ToString();
                    rajNivesh.UpdateStatus(rajNiveshStatus);
                }
            }

            return "Success";
        }
        private string WareHouseeSignWorkFlow(string FileName = "", string details = "")
        {
            string filepath = Server.MapPath("~/Content/RSWC");
            List<WarehouseRSWCModel> DetailsList = JsonConvert.DeserializeObject<List<WarehouseRSWCModel>>(details);
            string result = "";
            string OrderNo = "";
            string Receipt_No = "";

            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            if (DetailsList[0].Id == "0")
            {
                result = objNew.InsertUpdateRSWC(DetailsList).ToString();
            }
            else
            {
                result = objNew.UpdateRSWC(DetailsList).ToString();
            }
            Receipt_No = result;
            if (System.IO.File.Exists(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf"))
            {
                System.IO.File.Delete(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
            }
            System.IO.File.Move(filepath + "/" + FileName + ".pdf", filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
            if (result == "-1" || result == "0")
            {
                return result;
            }
            else
            {
                result = "Success";
            }
            FileName = Receipt_No.Replace("/", "_");

            string json = JsonConvert.SerializeObject(DetailsList);
            DetailsList.ForEach(x =>
            {
                x.Id = "1";
                x.EntryFile = FileName + ".pdf";
                x.Receipt_No = Receipt_No;
            });
            objNew.InsertUpdateRSWC(DetailsList).ToString();
            return result;
        }

        private string OTSeSignWorkFlow(string Data, string fileName)
        {
            string message = "";
            var pLDBLoan = new PLDBLoan();
            var _model = JsonConvert.DeserializeObject<PLDB_OTSSchemeData>(Data);
            _model.CertificateGenerated = fileName;
            var result = 0;
            result = pLDBLoan.SaveFinalReceipt(_model);
            if (result > 0)
            {
                message = "Document Signed Successfully";
            }
            else if (result == -2)
            {
                message = "Duplicate";
            }
            else
            {
                message = "Some Error Occurred.";
            }
            return message;
        }

        private string CCSComplainteSignWorkFlow(string data, string FileNameComplainant, string DisposeFile = "")
        {
            SocietyModel objNew = new SocietyModel(new DBLogger());
            var model = JsonConvert.DeserializeObject<SocietyDetailsforAction>(data);
            string result = "-1";
            string link = "";
            string status = "";
            model.ComplaintId = model.complainerId.ToString();
            model.complainant_letter = FileNameComplainant;
            result = objNew.InsertUpdateCCSComplaintLetter(model);
            if (result != "-1")
            {
                //send sms to Complainant
                link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameComplainant + ".pdf";
                status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", model.MobileNo);

                string email = Request.Url.Host.Contains("localhost") ? "balwant.chaudhary@devitpl.com" : model.Email;
                COMMON_FUNCTION.SendEmail(email, "Rajsahakar:- Notice Issued Against Complaint No-" + model.ReceiptNo, model.Notice);
                if (result != "" && result != "-1")
                {
                    int DistrictId = _LoggedInUser.DistrictId;
                    model.ComplaintId = model.complainerId.ToString();
                    model.complainant_letter = DisposeFile;
                    result = objNew.InsertUpdateCCSComplaintLetter(model);
                    if (result != "-1")
                    {
                        if (Request.Url.Host.Contains("localhost"))
                        {

                        }
                        else
                        {
                            link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + model.complainant_letter + ".pdf";
                            status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", model.MobileNo);
                        }
                    }
                }
                result = objNew.saveActiondetail(model);
            }
            return result;
        }

        private string CMKishaneSignWorkFlow(string enc_id, string fileName, long userId)
        {
            string result = "";
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            bool status = cMK_BeneficaryRFT.UpdateRFT_BILL_eSignInfo(enc_id, fileName, userId);
            if (status) { result = "File Esigned Successfully"; } else { result = "Fail"; }
            return result;
        }

        private string WRSummaryeSignWorkFlow(string FileName = "", string details = "", string Receipt_No = "", string uniqeId = "")
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            List<WarehouseRSWCModel> DetailsList = JsonConvert.DeserializeObject<List<WarehouseRSWCModel>>(details);
            string request_No = string.Empty;
            DetailsList.ForEach(x =>
            {
                x.Id = "1";
                x.EntryFile = FileName;
            });
            return request_No = objNew.InsertUpdateWRSummary(DetailsList, Receipt_No, uniqeId).ToString();
        }


        //Audit WorkFlow 18-06-2026
        private string AuditDepartmentWorkFlow(EsignTransactionResponseModel model)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            string result = "";
            string saveIDs = "";
            string Ids = "";
            string fullPath = "";


            //Ids = model.DocId;
            //if (Ids.Length > 0)
            //    saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);
            //Ids = model.DocId?.TrimEnd(',');
            Ids = model.PurposeOfLoanOther?.TrimEnd(',');
            fullPath = "../Content/AuditReport/" + model.FileName;

            result = objSocietyModel.saveAuditAssignbydeprtNotRequestReOrder(model.Remark, model.AuditBy, Ids, model.AssignedIds, Convert.ToString(model.StatusId), model.IsFirm, model.UserId, fullPath, Convert.ToInt32(model.UserRoleDeptMappingId), model.AuditTillDate, model.IsForced, model.SSOID, Convert.ToInt32(model.FinancialYearId), model.ReceiptNumber, model.DeleteResions).ToString();
            objAE.OrderNo = model.ReceiptNumber;
            if (result != "-1")
            {
                result = "Success";
            }
            else
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
            }
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSignController with Audit",
                FunctionName = "AuditDepartmentWorkFlow(responseModel :-) Step 4 start responseModel result " + result
            }, new System.Exception(model.PRN));
            return result;
        }

        private string AuditAssignByDeptWorkFlow(EsignTransactionResponseModel model)
        {
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            string result = "";
            string saveIDs = "";
            string Ids = "";
            string fullPath = "";
            //Ids = model.DocId;

            //if (Ids.Length > 0)
            //    saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);
            Ids = model.PurposeOfLoanOther;
            saveIDs = Ids;

            if (saveIDs.Length > 0)
                saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

            fullPath = "../Content/AuditReport/" + model.FileName;
            result = objSocietyModel.saveAuditAssignbydeprt(model.Remark, model.AuditBy, saveIDs, Convert.ToString(model.StatusId), model.IsFirm, fullPath, Convert.ToInt32(model.UserRoleDeptMappingId), model.ReceiptNumber).ToString();
            if (result != "-1")
                result = "Success";
            else
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
                result = "Error";
            }
            return result;
        }
        private string AuditAssignByDepartmentNotRequest(EsignTransactionResponseModel model)
        {
            DBLogger objDBLogger = new DBLogger();
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSign Controller start setup : 1",
                FunctionName = "AuditAssignByDepartmentNotRequest(EsignTransactionResponseModel " + JsonConvert.SerializeObject(model) + ")"
            }, new System.Exception(model.PRN));

            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            string result = "";
            string saveIDs = "";
            if (model.DocIds.Length > 0)
                saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

            string fullPath = "../Content/AuditReport/" + model.FileName;
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSign Controller start setup : 2,saveIDs" + saveIDs,
                FunctionName = "AuditAssignByDepartmentNotRequest(EsignTransactionResponseModel " + JsonConvert.SerializeObject(model) + ")"
            }, new System.Exception(model.DocIds));
            result = objSocietyModel.saveAuditAssignbydeprtNotRequest(model.Remark, model.AuditBy, model.DocId, Convert.ToString(model.StatusId), model.IsFirm, model.UserId, fullPath, Convert.ToInt32(model.UserRoleDeptMappingId), model.AuditTillDate, model.IsForced, model.SSOID, Convert.ToInt32(model.FinancialYearId), model.ReceiptNumber).ToString();
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSign Controller start setup : 3,Return Result" + result,
                FunctionName = "saveAuditAssignbydeprtNotRequest(" + "Remark : " + model.Remark + ", AuditBy : " + model.AuditBy + ", DocId : " + model.DocId + ", StausId : " + Convert.ToString(model.StatusId) + ", IsFirm : " + model.IsFirm + ", UserId : " + model.UserId + ", fullPath : " + fullPath + ", UserRoleDeptMappingId : " + Convert.ToInt32(model.UserRoleDeptMappingId) + ", AuditTillDate : " + model.AuditTillDate + ", IsForced : " + model.IsForced + ", SSOID : " + model.SSOID +
                               ", FinancialYearId : " + Convert.ToInt32(model.FinancialYearId) + ", ReceiptNumber : " + model.ReceiptNumber + ")"
            }, new System.Exception("Return Result :- " + result));

            if (result != "-1" && result != "-2")
                result = "Success";
            else
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
                result = "Error";
            }
            return result;
        }
        private string AuditAssignByDepartment(EsignTransactionResponseModel model)
        {
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            string result = "";
            string saveIDs = "";
            string fullPath = "";
            DBLogger objDBLogger = new DBLogger();

            if (model.DocIds.Length > 0)
                saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

            fullPath = "../Content/AuditReport/" + model.FileName;
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSign Controller start setup : 1",
                FunctionName = "AuditAssignByDepartment(EsignTransactionResponseModel " + JsonConvert.SerializeObject(model) + ")"
            }, new System.Exception(model.PRN));

            result = objSocietyModel.saveAuditAssignbydeprt(model.Remark, model.AuditBy, saveIDs, Convert.ToString(model.StatusId), model.IsFirm, fullPath, Convert.ToInt32(model.UserRoleDeptMappingId), model.ReceiptNumber).ToString();
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "eSign Controller start setup : 3,Return Result" + result,
                FunctionName = "saveAuditAssignbydeprt(" + "Remark : " + model.Remark + ", AuditBy : " + model.AuditBy + ", DocId : " + model.DocId + ", StausId : " + Convert.ToString(model.StatusId) + ", IsFirm : " + model.IsFirm + ", UserId : " + model.UserId + ", fullPath : " + fullPath + ", UserRoleDeptMappingId : " + Convert.ToInt32(model.UserRoleDeptMappingId) + ", AuditTillDate : " + model.AuditTillDate + ", IsForced : " + model.IsForced + ", SSOID : " + model.SSOID +
                               ", FinancialYearId : " + Convert.ToInt32(model.FinancialYearId) + ", ReceiptNumber : " + model.ReceiptNumber + ")"
            }, new System.Exception("Return Result :- " + result));

            if (result != "-1")
                result = "Success";
            else
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
                result = "Error";
            }
            return result;
        }
        private string GCCWorkFlow(EsignTransactionResponseModel responseModel = null)
        {
            string message = "";
            string gccFlow = "";
            string gccFormData = "";
            DataTable data = null;

            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            GopalCreditCardFormModel creditCardFormModel = new GopalCreditCardFormModel();
            GccApplicationFlowModel _AppFlowModel = new GccApplicationFlowModel();
            if (responseModel.StatusId != 4 && responseModel.DocType != "Citizen Certificate")
            {
                data = gccModel.GetApplicationFlowByGCCId(Convert.ToInt32(responseModel.DocId), Convert.ToInt32(responseModel.UserId));
                gccFlow = data.Rows[0]["GccApplicationFlow"].ToString();
                gccFormData = data.Rows[0]["GopalCreditCardForm"].ToString();

                _AppFlowModel = JsonConvert.DeserializeObject<GccApplicationFlowModel>(gccFlow);
                _AppFlowModel.StatusId = 11;
                _AppFlowModel.UserId = 0;
            }


            creditCardFormModel.gcc_id = Convert.ToInt32(responseModel.DocId);
            if (responseModel.StatusId == 0 || responseModel.StatusId == 1 && responseModel.DocType == "GCCPACS Certificate")
                creditCardFormModel.IsPacsManagerSigned = true;
            else if (responseModel.StatusId == 2 && responseModel.DocType == "Guarantor Certificate")
                creditCardFormModel.IsGurantor1Signed = true;
            else if (responseModel.StatusId == 3 && responseModel.DocType == "Guarantor1 Certificate")
                creditCardFormModel.IsGurantor2Signed = true;
            else if (responseModel.StatusId == 4 && responseModel.DocType == "Citizen Certificate")
                creditCardFormModel.IsApplicantSigned = true;
            int r = gccModel.UpdateEsingInfoOfGCC(creditCardFormModel);
            int result = 0;
            if (r > 0)
            {
                DataTable dt = gccModel.GetEsignStatusByGCC(creditCardFormModel.gcc_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    creditCardFormModel.IsApplicantSigned = Convert.ToBoolean(dt.Rows[0]["IsApplicantSigned"]);
                    creditCardFormModel.IsPacsManagerSigned = Convert.ToBoolean(dt.Rows[0]["IsPacsManagerSigned"]);
                    creditCardFormModel.IsGurantor1Signed = Convert.ToBoolean(dt.Rows[0]["IsGurantor1Signed"]);
                    creditCardFormModel.IsGurantor2Signed = Convert.ToBoolean(dt.Rows[0]["IsGurantor2Signed"]);
                    if (creditCardFormModel.IsApplicantSigned == true && creditCardFormModel.IsPacsManagerSigned == true && creditCardFormModel.IsGurantor1Signed == true && creditCardFormModel.IsGurantor2Signed == true)
                    {
                        result = gccModel.AddApplicationFlow_V1(_AppFlowModel);
                    }
                    if (_AppFlowModel.StatusId == 19)
                    {
                        //var status = gccModel.PushGCCTransactionToJanaadhar(creditCardFormModel, _AppFlowModel, result);
                    }
                }

                message = "Success";
            }
            else
            {
                message = "Fail";
            }
            return message;
        }
        private string MPacsWorkFlow(string jsonData, string Remark, string fileName, string Status)
        {
            int Result = 0;
            try
            {
                string IDs = "";

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                List<InspactorStatusDetails> InspactorStatusDetails = JsonConvert.DeserializeObject<List<InspactorStatusDetails>>(jsonData);

                if (InspactorStatusDetails != null && InspactorStatusDetails.Count > 1)
                {
                    foreach (var item in InspactorStatusDetails)
                    {
                        IDs += item.InspactorTargetsId + ",";
                    }

                    IDs = IDs.TrimEnd(',');
                }
                Result = objSocietyModel.SaveInspactorStatusDRForNewEsign(InspactorStatusDetails, Remark, fileName, Status, IDs);
                return Result.ToString();
            }
            catch (System.Exception)
            {
                return Result.ToString();
                throw;
            }
        }
        private string AmendmentWorkFlow(string SocietyIDEnc, string CirtFile, string MemberFile, string BylawsFile, int StatusId, string Remark, long UserId, int AmendmentId, string AmendmendAmount, string ReceiptNumber)
        {
            ApplicationFlowModel flowModel = new ApplicationFlowModel();
            int result = flowModel.AddApplicationFlowAmendment1958(SocietyIDEnc, StatusId, Remark, UserId, AmendmentId, AmendmendAmount, ReceiptNumber);
            if (result == 0)
            {
                return "Fail";
            }
            else
            {
                string response = flowModel.SaveSocietyAmendmendData(SocietyIDEnc, CirtFile, MemberFile, BylawsFile, true, StatusId, AmendmentId);
                if (response == "Fail" || response == "-1")
                {
                    return "Fail";
                }
                return "Success";
            }
        }

        private DataSet UpdateEsignStage(string SocietyIDEnc, int StatusId, string certificateFile)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateEsignStage", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@CertificateFile", DBFactoryParameterType.VARCHAR, certificateFile);
                objDBAccess.AddParameterToCommand("@IsEsignStage", DBFactoryParameterType.Int, StatusId);
                ds = objDBAccess.GetDataSet();
            }
            catch (System.Exception ex)
            {
                ds = null;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.esignController.UpdateEsignStage", FunctionName = "UpdateEsignStage(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            return ds;
        }

        private DataSet UpdateAmendmentEsignStage(string SocietyIDEnc, int StatusId, string certificateFile, int AmendmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AmendmentUpdateEsignStage", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@CertificateFile", DBFactoryParameterType.VARCHAR, certificateFile);
                objDBAccess.AddParameterToCommand("@IsEsignStage", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                ds = objDBAccess.GetDataSet();
            }
            catch (System.Exception ex)
            {
                ds = null;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.esignController.UpdateAmendmentEsignStage", FunctionName = "UpdateAmendmentEsignStage(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            return ds;
        }

        private string NOCCertificateWorkFlow(EsignTransactionResponseModel responseModel)
        {
            int result = 0;
            string message = "";
            GccApplicationFlowModel _AppFlowModel = new GccApplicationFlowModel();
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            GopalCreditCardFormModel _Model = new GopalCreditCardFormModel();
            _AppFlowModel = JsonConvert.DeserializeObject<GccApplicationFlowModel>(responseModel.PurposeOfLoanOther);
            _Model = JsonConvert.DeserializeObject<GopalCreditCardFormModel>(responseModel.JsonData);

            _AppFlowModel.StatusId = responseModel.StatusId;
            _AppFlowModel.DocumentURL = responseModel.FileName;
            _AppFlowModel.GccId = Convert.ToInt32(responseModel.DocId);
            _AppFlowModel.Remark = responseModel.Remark;
            result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);
            GopalCreditCardController gccController = new GopalCreditCardController();
            if (_AppFlowModel.StatusId == 19)
            {
                // var status = _Modal.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);
            }
            if (result > 0)
            {
                gccController.SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                message = "Success";
            }
            else
            {
                message = "Fail";
            }
            return message;
        }

        private string GCCMokaReportFlow(EsignTransactionResponseModel responseModel)
        {
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            GopalCreditCardFormModel _Model = new GopalCreditCardFormModel();
            GccApplicationFlowModel _AppFlowModel = new GccApplicationFlowModel();

            _Model = JsonConvert.DeserializeObject<GopalCreditCardFormModel>(responseModel.JsonData);
            _AppFlowModel = JsonConvert.DeserializeObject<GccApplicationFlowModel>(responseModel.PurposeOfLoanOther);


            int result = 0;
            GccDocumentsModel docMoka = new GccDocumentsModel();
            docMoka.DocumentURL = responseModel.FileName;
            docMoka.DocId = 1;
            docMoka.GccId = Convert.ToInt32(responseModel.DocId);
            docMoka.GccId = _AppFlowModel.GccId;
            _AppFlowModel.DocId = _Modal.InsertGCCReportFile(docMoka);

            _AppFlowModel.StatusId = responseModel.StatusId;
            _AppFlowModel.Remark = responseModel.Remark;
            _AppFlowModel.DocumentURL = responseModel.FileName;
            result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
            if (result > 0)
            {
                _Modal.UpdateMokaReportData(_Model);
                return "Success";
            }
            else
            {
                return "Fail";
            }
        }

        private string GCCCertificateWorkFlow(EsignTransactionResponseModel responseModel)
        {
            int result = 0;
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            GopalCreditCardFormModel _Model = new GopalCreditCardFormModel();
            GccApplicationFlowModel _AppFlowModel = new GccApplicationFlowModel();

            _Model = JsonConvert.DeserializeObject<GopalCreditCardFormModel>(responseModel.JsonData);
            _AppFlowModel = JsonConvert.DeserializeObject<GccApplicationFlowModel>(responseModel.PurposeOfLoanOther);

            GccDocumentsModel _Entity = new GccDocumentsModel { DocId = 3, DocumentURL = responseModel.FileName, GccId = _AppFlowModel.GccId };
            var ReturnRegiNo = _Modal.InsertGCCReportFile(_Entity);

            result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
            if (result > 0)
            {
                var gccController = new GopalCreditCardController();
                gccController.SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                return "Success";
            }
            else { return "Fail"; }
        }

        private string GodownWorkFlow(EsignTransactionResponseModel responseModel)
        {
            string message = string.Empty;
            int result = 0;
            int isEsignStage = 0;

            Cooperative.Models.GodownModel objGoDownModel = new Cooperative.Models.GodownModel(new DBLogger());
            switch (responseModel.DocType)
            {
                case "Godown Document":
                    isEsignStage = 1;
                    break;
                case "Godown President":
                    isEsignStage = 2;
                    break;
                case "Godown BranchManager":
                    isEsignStage = 3;
                    break;
                case "ASGenerate Certificate":
                    isEsignStage = 4;
                    break;
                case "100 FSGenerate Certificate":
                    isEsignStage = 5;
                    break;
                case "101 FSGenerate Certificate":
                    isEsignStage = 5;
                    break;
                case "500 FSGenerate Certificate":
                    isEsignStage = 5;
                    break;
                case "250 FSGenerate Certificate":
                    isEsignStage = 5;
                    break;
                default:
                    isEsignStage = 0;
                    break;
            }

            result = objGoDownModel.UpdateEsignStage(responseModel.DocId, isEsignStage);
            if (result > 0)
            {
                if (responseModel.DocType == "ASGenerate Certificate")
                {
                    objGoDownModel.UpdateASGeneratePath(Convert.ToInt32(responseModel.DocId), responseModel.FileName);
                    objGoDownModel.UpdateAsEsignProcess(Convert.ToInt32(responseModel.DocId));
                }
                else if (responseModel.DocType == "100 FSGenerate Certificate" || responseModel.DocType == "101 FSGenerate Certificate" || responseModel.DocType == "250 FSGenerate Certificate" || responseModel.DocType == "500 FSGenerate Certificate")
                {
                    objGoDownModel.UpdateFsGeneratePath(Convert.ToInt32(responseModel.DocId), responseModel.FileName);
                    objGoDownModel.UpdateFSEsignProcess(Convert.ToInt32(responseModel.DocId));
                }

                result = objGoDownModel.UpdateAction(responseModel.DocId, responseModel.ReceiptNumber);
                if (result > 0)
                    message = "Success";
                else
                    message = "Fail";
                return message;

            }
            return message;
        }

        private string TrainingWorkFlow(EsignTransactionResponseModel responseModel)
        {
            string message = string.Empty;

            int result = 0;
            int isEsign = 0;
            var trainingModel = new TrainningModel();
            switch (responseModel.DocType)
            {
                case "Trainning Certificate":
                    isEsign = 1;
                    break;
                case "Trainning Invoice":
                    isEsign = 2;
                    break;
                default:
                    isEsign = 0;
                    break;
            }
            string fullPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/Content/Training/" + responseModel.FileName : "https://rajsahakarapp.rajasthan.gov.in/Content/Training/" + responseModel.FileName;
            result = trainingModel.RICEMTrainingAttendance(Convert.ToInt32(responseModel.DocId), fullPath, isEsign);
            if (result > 0)
            {
                message = "Success";
            }
            else { message = "Fail"; }
            return message;
        }
        private string CCSNoticeIssueWorkFlow(string data, string FileNameComplainant, string DisposeFile = "")
        {
            string message = string.Empty;
            string result = "0";

            SocietyModel objNew = new SocietyModel(new DBLogger());
            if (data != null)
            {
                var model = JsonConvert.DeserializeObject<SocietyDetailsforAction>(data);
                model.complainant_letter = FileNameComplainant;
                if (!string.IsNullOrWhiteSpace(model.complainant_letter))
                {
                    string Doc = model.complainant_letter;
                    model.ComplaintId = model.complainerId.ToString();
                    model.complainant_letter = Doc;
                    result = objNew.InsertUpdateCCSComplaintLetter(model);
                    if (result != "-1")
                    {
                        // Send SMS to Complainant
                        string MobileNo = Request.Url.Host.Contains("localhost") ? "9460934860" : model.MobileNo;
                        string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + model.complainant_letter + ".pdf";

                        string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", MobileNo);

                        string email = Request.Url.Host.Contains("localhost") ? "balwant.chaudhary@devitpl.com" : model.Email;
                        Cooperative.Controllers.COMMON_FUNCTION.SendEmail(email, "Rajsahakar:- Notice Issued Against Complaint No-" + model.ReceiptNo, model.Notice);
                        SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                        result = objSocietyModel.saveActiondetail(model);
                        result = "1";
                        message = "Success";
                    }
                }
            }
            else
            {
                message = "Error";
            }

            return message;
        }

        private string AGMWorkFlow(EsignTransactionResponseModel responseModel)
        {
            int result = 0;
            string response = string.Empty;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            List<InspactorStatusDetails> inspactorStatusDetails = new List<InspactorStatusDetails>();
            if (!string.IsNullOrEmpty(responseModel.PurposeOfLoanOther))
            {
                inspactorStatusDetails = JsonConvert.DeserializeObject<List<InspactorStatusDetails>>(responseModel.PurposeOfLoanOther);
            }

            result = objSocietyModel.SaveGNStatus(inspactorStatusDetails, responseModel.Remark, responseModel.SocietyIDEnc, Convert.ToString(responseModel.StatusId), responseModel.AmendmendAmount, responseModel.ReceiptNumber, responseModel.FileName);
            if (result == 0)
            {
                response = "Fail";
            }
            else
            {
                response = "Success";
            }

            return response;
        }

        #endregion
    }
}


