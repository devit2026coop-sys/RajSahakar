using Cooperative.Filters;
using Cooperative.Models;
using FactoryAndBoiler.Payment;
using Ionic.Zip;
using java.nio.file;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebGrease.Activities;

namespace Cooperative.Controllers
{
    [CustomAuthentication]
    public class PMKisanController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult CMBeneficiaryList()
        {
            return View();
        }
        public ActionResult BeneficiaryList()
        {
            return View();
        }
        public ActionResult GenerateRFT()
        {
            return View();
        }
        public ActionResult GenerateBeneficiaryListForDBT()
        {
            return View();
        }
        public ActionResult GenerateBeneficiaryDBTFiles()
        {
            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;
            return View();
        }
        public ActionResult BeneficiaryListDetails()
        {
            return View();
        }
        public ActionResult KnowYourStatus()
        {
            return View();
        }
        public ActionResult DataPullUtility()
        {
            return View();
        }
        public ActionResult BeneficiaryListForAudit()
        {
            return View();
        }

        [HttpGet]
        //[OutputCache(Duration = 3600 * 24)]
        public JsonResult GetPMK_DashboardData(int financialYearId, int installmentType = 0)
        {
            var responce = new APIResponce();
            var dashboard = new PMKisanDashboardModel(new FileLogger());
            var ds = dashboard.GetPMK_DashboardData(financialYearId, installmentType);
            if (ds != null)
            {
                responce.Data = new
                {
                    Dashboard = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 3600 * 24)]
        public JsonResult GetQuadrimesterData()
        {
            var responce = new APIResponce();
            var dashboard = new PMKisanDashboardModel(new FileLogger());
            var ds = dashboard.GetPMK_QuadrimesterData();
            if (ds != null)
            {
                responce.Data = new
                {
                    Quadrimester = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        //[OutputCache(Duration = 3600 * 24)]
        public JsonResult GetBeneficiaryList(int financialYearId = 0, int districtId = 0, string subDistrictName = "", string villageName = "", int installmentType = 0, string locationType = "district")
        {
            var responce = new APIResponce();
            var BeneficiaryList = new PMKisanDashboardModel(new FileLogger());
            var ds = BeneficiaryList.GetBeneficiaryList(financialYearId, districtId, subDistrictName, villageName, installmentType, locationType);
            if (ds != null)
            {
                responce.Data = new
                {
                    BeneficiaryList = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalBeneficiaryList = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                    // Quadrimester = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[OutputCache(Duration = 3600 * 24)]
        public string GetBeneficiaryDetailList(DataPagingModel Details, int financialYearId = 0, int installmentType = 0, int districtId = 0, string subDistrictName = "", string villageName = "", int type = 1, bool isExport = false)
        {
            var responce = new APIResponce();
            var BeneficiaryList = new PMKisanDashboardModel(new FileLogger());
            var ds = BeneficiaryList.GetBeneficiaryDetailList(ref Details, financialYearId, installmentType, districtId, subDistrictName, villageName, type, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");
                if (dt.Columns.Contains("RegNo"))
                    dt.Columns.Remove("RegNo");
                if (dt.Columns.Contains("RegDate"))
                    dt.Columns.Remove("RegDate");
                if (dt.Columns.Contains("HomeAddress"))
                    dt.Columns.Remove("HomeAddress");
                if (dt.Columns.Contains("DistrictName"))
                    dt.Columns.Remove("DistrictName");
                if (dt.Columns.Contains("SubDistrictName"))
                    dt.Columns.Remove("SubDistrictName");
                if (dt.Columns.Contains("VillageName"))
                    dt.Columns.Remove("VillageName");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "BeneficiaryDetailList.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }

        [HttpPost]
        public string GetBeneficiaryListForAudit(DataPagingModel Details, int installmentType = 0, int districtId = 0, string subDistrictName = "", string searchtext = "", bool isExport = false)
        {
            var responce = new APIResponce();
            var BeneficiaryList = new PMKisanDashboardModel(new FileLogger());
            var ds = BeneficiaryList.GetBeneficiaryListForAudit(ref Details, installmentType, districtId, subDistrictName, searchtext, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "CMK_BeneficiaryData.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }


        [HttpPost]
        public JsonResult GenerateBeneficiaryListDBT(CMK_BeneficaryRFTModel cMK_BeneficaryRFT)
        {
            var responce = new APIResponce();
            var b = new CMK_BeneficaryRFTModel(new FileLogger());
            cMK_BeneficaryRFT.CREATED_BY = _loggedInUSerDetails.UserId;
            var result = b.GenerateBeneficiaryListDBT(cMK_BeneficaryRFT);
            if (result)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_RFTDBT_List()
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.Get_RFTDBT_List();
            if (result != null && result.Tables.Count > 0)
            {
                responce.Data = COMMON_FUNCTION.DataTableToJSONString(result.Tables[0]);
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_RFTDBT_Details(string id_enc)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.Get_RFTDBT_Details(id_enc);
            if (result != null && result.Tables.Count > 0)
            {
                responce.Data = COMMON_FUNCTION.DataTableToJSONString(result.Tables[0]);
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GenerateBeneficiaryDBTFiles(string id_enc, string dbt_date)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.GenerateBeneficiaryDBTFiles(id_enc, dbt_date);
            if (result)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBeneficiaryDBTFiles(string id_enc)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.GetBeneficiaryDBTFiles(id_enc);
            if (result != null && result.Tables.Count > 0)
            {
                responce.Data = COMMON_FUNCTION.DataTableToJSONString(result.Tables[0]);
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateDBT_XML(string id_enc)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.GenerateDBT_XML(id_enc, _loggedInUSerDetails.UserId);
            if (result)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EsignXML(string id_enc, string password, int type = 1)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.EsignXML(id_enc, password, _loggedInUSerDetails.UserId);
            if (result == 1)
            {
                responce.Data = result;
                responce.Status = "200";
                responce.Message = "Success";
            }
            else if (result == 2)
            {
                responce.Data = result;
                responce.Status = "400";
                responce.Message = "The specified network password is not correct.";
            }
            else
            {
                responce.Data = result;
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EncryptXML(string id_enc)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.EncryptXML(id_enc);
            if (result)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_enc"></param>
        /// <param name="fileType">FileType => 1 = RFTBILL, 2 = XML FILE, 3 = ENCRYPTED XML FILE</param>
        /// <param name="isEsigned"></param>
        /// <returns></returns>
        public ActionResult DownloadAllBill(string id_enc, int fileType = 1, int isEsigned = 0)
        {

            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            DataSet ds = cMK_BeneficaryRFT.GetDBT_HeadDetailsBY_DH_ID(id_enc);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                string sourceDirectory = "";
                if (fileType == 1)
                    sourceDirectory = Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO_PATH"]}/RFT_BILL/{(isEsigned == 1 ? "eSigned/" : "")}");
                else if (fileType == 2)
                    sourceDirectory = Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO_PATH"]}/{(isEsigned == 1 ? "eSigned/" : "")}");
                else
                    sourceDirectory = Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO_PATH"]}/Encrypted/");
                // Create the zip file
                using (var memoryStream = new MemoryStream())
                {
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        string[] files = Directory.GetFiles(sourceDirectory);
                        // Add all files in the source directory to the zip
                        foreach (string file in files)
                        {
                            zip.AddFile(file, "");
                        }

                        // Save the zip file to the specified path
                        zip.Save(memoryStream);
                    }
                    memoryStream.Position = 0;
                    return File(memoryStream.ToArray(), "application/zip", "downloadedFiles.zip");
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult GenerateRFT_BILL(string id_enc)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.GenerateRFT_BILL(id_enc);
            if (result.Enabled)
            {
                if (cMK_BeneficaryRFT.UpdateRFT_BILL_Info(id_enc, result.Text))
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EsignRFT_BILL(string id_enc, string TXN, string OTP = "")
        {
            string pdfPath = "";
            string succmsg = string.Empty;
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = 0;// cMK_BeneficaryRFT.GenerateRFT_BILL(id_enc);

            ResultMessage otpResult = new ResultMessage();

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            try
            {
                if (TXN != "" || Request.Url.Host.Contains("localhost"))
                {
                    if (!Request.Url.Host.Contains("localhost")) { otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN); }
                    else { otpResult.status = "1"; otpResult.data = TXN; }


                    if (otpResult.status == "1")
                    {
                        DataSet ds = cMK_BeneficaryRFT.GetDebitHeadDetails(id_enc);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];


                            string pathDir = Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO"]}/RFT_BILL/");
                            string dpathDir = Server.MapPath($"~/Content/CMK_DBT/{dt.Rows[0]["RFT_NO"]}/RFT_BILL/eSigned/");
                            if (!Directory.Exists(dpathDir))
                                Directory.CreateDirectory(dpathDir);


                            string FilePath = System.IO.Path.Combine(pathDir, dt.Rows[0]["BILL_PATH"].ToString());
                            string destinationPath = System.IO.Path.Combine(dpathDir, dt.Rows[0]["BILL_PATH"].ToString());

                            System.IO.File.Copy(FilePath, destinationPath, true); // Overwrite if exists

                            //  var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, destinationPath, "JAIPUR (RAJASTHAN)", "JOINT REGISTRAR (ADMN.)");
                            //var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, destinationPath, "JAIPUR (RAJASTHAN)", "JOINT REGISTRAR (ADMN.)");
                            var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, destinationPath, "JAIPUR (RAJASTHAN)", "JOINT REGISTRAR (ADMN.)");

                            if (result1 != null) succmsg = result1.status;
                            if (succmsg == "1")
                            {
                                bool status = cMK_BeneficaryRFT.UpdateRFT_BILL_eSignInfo(id_enc, dt.Rows[0]["BILL_PATH"].ToString(), _LoggedInUser.UserId);

                                result = 1;
                            }
                            else { result = 0; }
                        }
                        else { result = 0; }
                    }
                    else
                    {
                        result = -2;
                    }
                }
                else
                {
                    result = 0;
                }


                if (result == 1)
                {
                    responce.Status = "200";
                    responce.Message = "File Esigned Successfully";
                }
                else if (result == -2)
                {
                    responce.Status = "100";
                    responce.Message = "Invalid OTP.";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);



            }
            catch (System.Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.PMKisan", FunctionName = "EsignRFT_BILL(string id_enc, string TXN,string OTP)" }, ex);
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GenerateRFTBillLatter(string id_enc)
        {
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                {
                    return Json("LOGOUT");
                }

                var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
                DataSet ds = cMK_BeneficaryRFT.GetDebitHeadDetails(id_enc);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    string rftNo = Convert.ToString(dt.Rows[0]["RFT_NO"]);
                    string documentName = Convert.ToString(dt.Rows[0]["BILL_PATH"]);

                    if (string.IsNullOrWhiteSpace(rftNo) || string.IsNullOrWhiteSpace(documentName))
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "Invalid file information.",
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = ""
                        }, JsonRequestBehavior.AllowGet);
                    }

                    string pathDir = Server.MapPath($"~/Content/CMK_DBT/{rftNo}/RFT_BILL/");
                    string dpathDir = Server.MapPath($"~/Content/CMK_DBT/{rftNo}/RFT_BILL/eSigned/");

                    if (!Directory.Exists(dpathDir))
                    {
                        Directory.CreateDirectory(dpathDir);
                    }

                    string filePath = System.IO.Path.Combine(pathDir, documentName);
                    string destinationPath = System.IO.Path.Combine(dpathDir, documentName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = "Source file not found.",
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = ""
                        }, JsonRequestBehavior.AllowGet);
                    }

                    System.IO.File.Copy(filePath, destinationPath, true);

                    return Json(new
                    {
                        Success = true,
                        DocPath = destinationPath,
                        DocType = "CMKisan",
                        DocName = documentName,
                        //ReturnPath = Url.Action("GenerateBeneficiaryDBTFiles", "PMKisan"),
                        ReturnPath = Url.Action("GenerateBeneficiaryDBTFiles", "PMKisan", new { id_enc = id_enc }),
                        Location = "JAIPUR (RAJASTHAN)",
                        Designation = "JOINT REGISTRAR (ADMN.)"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    Success = false,
                    Message = "PDF generation failed.",
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Controller.PMKisan",
                    FunctionName = "GenerateRFTBillLatter(string id_enc)"
                }, ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}