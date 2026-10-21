using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using static Cooperative.Models.CoopModel;


namespace Cooperative.Controllers
{
    [Error]
    public class GodownController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        public ActionResult AddGodownForm()
        {
            return View();
        }

        [HttpGet]
        public string GodownLists(int Status = 0)
        {
            GodownModel objSocietyModel = new GodownModel(new DBLogger());

            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetGodownProposalList(ref Details, _LoggedInUser.UserRoleDeptMappingId, Status));
        }


        [HttpPost]
        public int SaveAddGodownForm(GodownProposalEntity _Entity)
        {
            GodownModel objGodownModel = new GodownModel(new DBLogger());

            _Entity.GodownDocuments = Request.Form["GodownDocuments"];
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();

            int godownId = objGodownModel.GodownDetailsInsert(_Entity);

            if (godownId > 0)
            {
                _Entity.GodownId = godownId;
                string pdfPath = objGodownModel.GodownPacsLevelLetter(_Entity);
                string docName = Path.GetFileName(pdfPath);
                objGodownModel.UpdateFormationPath(godownId, pdfPath, docName);
            }

            return godownId;
        }


        [HttpPost]
        public string CitizenDashboardDataWithDepartmentUser(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            GodownModel objMenuActionModel = new GodownModel(new FileLogger());
            DataTable DT = null;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                // objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
                DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                if (DT != null && DT.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
                }
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (_LoggedInUser.UserType == "D")
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                    objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
                    objSocietyRegistrationEntity.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;

                    DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                else
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.OrganizationId = _LoggedInUser.UserRoleDeptMappingId;
                    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                    if (objSocietyRegistrationEntity.UserType != "K")
                        DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                // }
            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public JsonResult UploadGodownDocumentOld(HttpPostedFileBase DocumentPath, int DocId)
        {
            try
            {
                if (DocumentPath == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Godown/Documents/");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString("N").ToUpper() + Path.GetExtension(DocumentPath.FileName);

                string filePath = Path.Combine(folder, fileName);

                DocumentPath.SaveAs(filePath);

                return Json(new { success = true, docId = DocId, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult UploadGodownDocument(int DocId)
        {
            try
            {
                if (Request.Files.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No file uploaded."
                    });
                }
                var files = Request.Files[0];
                if (files == null)
                    return Json(new { success = false, message = "File not received" });

                string extension = Path.GetExtension(files.FileName).ToLowerInvariant();
                if (extension != ".pdf")
                    return Json(new { success = false, message = "Only PDF files are allowed" });

                string folder = Server.MapPath("~/Content/Godowns/Doc/");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string fileName = Guid.NewGuid().ToString("N").ToUpper() + extension;

                string filePath = Path.Combine(folder, fileName);

                files.SaveAs(filePath);

                return Json(new { success = true, docId = DocId, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message + "\nstack trace: " + ex.StackTrace });
            }
        }


        [HttpPost]
        public JsonResult UploadPWDDocument(HttpPostedFileBase PWDCertificateFilePath)
        {
            try
            {
                if (PWDCertificateFilePath == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Godown/Documents/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(PWDCertificateFilePath.FileName);
                string filePath = Path.Combine(folder, fileName);

                PWDCertificateFilePath.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UploadRelatedGodownDocument(HttpPostedFileBase RelatedGodownDocumentFilePath)
        {
            try
            {
                if (RelatedGodownDocumentFilePath == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Godown/Documents/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(RelatedGodownDocumentFilePath.FileName);
                string filePath = Path.Combine(folder, fileName);

                RelatedGodownDocumentFilePath.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [ActionName("GetNewLovValuesByListId")]
        public string GetNewLovValuesByListId(string ListCode)
        {
            DataTable DT = new GodownModel().GetNewLovValuesByListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        [ActionName("GetNewLovValuesDocByListId")]
        public string GetNewLovValuesDocsByListId(string ListCode)
        {
            DataTable DT = new GodownModel().GetNewLovValuesDocByListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        [ActionName("GetNewLovValuesForLandByListId")]
        public string GetNewLovValuesForLandDetailsByListId(string ListCode)
        {
            DataTable DT = new GodownModel().GetNewLovValuesForLandDetailByListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult GodownFormationAction()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GETGodownFormationList")]
        public string GETGodownFormationList(string SearchText = "")
        {
            GodownModel objGoDownModel = new GodownModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objGoDownModel.GETGodownFormationList(SearchText));
        }

        [HttpPost]
        [ActionName("GETActionsList")]
        public string GETActionsList()
        {
            GodownModel objGoDownModel = new GodownModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objGoDownModel.GETActionsList());
        }


        [HttpPost]
        public JsonResult GetGodownListApplicationFormViewData(string GodownApplicationEncId)
        {
            try
            {
                string Ids = "";

                if (!String.IsNullOrEmpty(GodownApplicationEncId))
                {
                    CoopEntities DB = new CoopEntities();

                    Ids = DB.GetDecrypt(GodownApplicationEncId).FirstOrDefault().ToString();

                    GodownModel objLoan = new GodownModel(new DBLogger());

                    DataSet ds = objLoan.GetGodownListApplicationFormViewData(Ids, "GodownData");

                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]), DocumentList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[1]), GodownTimeBoundList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[2]) }, JsonRequestBehavior.AllowGet);
                }

                return null;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GodownController", FunctionName = "GetGodownListApplicationFormViewData" }, ex);

                return null;
            }
        }


        [HttpPost]
        [CustomAuthentication]
        public int DeleteGodownApplication(int id)
        {
            int result = 0;

            try
            {
                GodownModel objLoan = new GodownModel(new DBLogger());
                result = objLoan.Delete_GodownApplication(id);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GodownController", FunctionName = "DeleteGodownApplication(int id)" }, ex);
            }

            return result;
        }

        [HttpPost]
        public string GetDocumentsByApplicationId(int ApplicationId)
        {
            GodownModel objModel = new GodownModel();
            DataSet dt = objModel.GetDocumentsByApplicationId(ApplicationId);
            return COMMON_FUNCTION.DataTableToJSONString(dt.Tables[0]);
        }


        [HttpGet]
        public string GetSocietyData(string Type, int DistrictId = 0, int BlockId = 0, int CityId = 0, int GPId = 0)
        {
            GodownModel objNew = new GodownModel(new DBLogger());
            DataTable DT = objNew.GetSocietyDetails(Type, DistrictId, BlockId, CityId, GPId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }


        [HttpGet]
        [ActionName("GetGodownPropsalSchemeByListId")]
        public string GetGodownPropsalSchemeByListId(string ListCode)
        {
            DataTable DT = new GodownModel().GetGodownProposalSchemeId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("UpdateAction")]
        public JsonResult UpdateAction(string selectedApplications, string Status)
        {
            GodownModel objNew = new GodownModel(new DBLogger());
            return Json(objNew.UpdateAction(selectedApplications, Status));
        }



        [HttpGet]
        public string GenerateASPDFOld(int ApplicationId)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    return "Fail";
                }

                GodownModel objGodownModel = new GodownModel(new DBLogger());
                string file = HostingEnvironment.MapPath("~/Template/Godown/GodownASGenerate.html");
                StringBuilder strbld = new StringBuilder();
                strbld.Append(System.IO.File.ReadAllText(file));
                DataTable dt = objGodownModel.GetGodownDetailsById(ApplicationId);
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
                    strbld.Replace("##ApplicationNo##", Convert.ToString(dr["ApplicationNo"]));
                }

                string pdfPath = objGodownModel.CommonASGenerate(strbld);
                pdfPath = Path.GetFileName(pdfPath);
                objGodownModel.UpdateASGeneratePath(ApplicationId, pdfPath);

                return pdfPath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GodownController", FunctionName = "GenerateASPDF" }, ex);
                return "Fail";
            }
        }



        [HttpGet]
        [ActionName("GetNewLovValuesForVacantPostListId")]
        public string GetNewLovValuesForVacantDetailsByListId(string ListCode)
        {
            DataTable DT = new GodownModel().GetNewLovValuesForVacantDetailByListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        public JsonResult GenerateASPDF(int ApplicationId, int NextStatusId)
        {
            GodownModel objGodownModel = new GodownModel(new DBLogger());
            var responseModel = new PacsResponseModel();
            var resultData = objGodownModel.GenerateASPdf(ApplicationId, NextStatusId);
            if (!string.IsNullOrWhiteSpace(resultData))
            {
                responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
            }

            if (responseModel == null)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "No response received."
                }, JsonRequestBehavior.AllowGet);
            }

            if (!responseModel.Success)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = responseModel.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Success = true,
                Result = 1,
                DocPath = responseModel.DocPath ?? "",
                DocType = responseModel.DocType ?? "",
                DocName = responseModel.DocName ?? "",
                ReturnPath = responseModel.ReturnPath ?? "",
                Location = responseModel.Location ?? "",
                Designation = responseModel.Designation ?? "",
                ViewPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/Content/GodownDocument/" + responseModel.DocName : "https://rajsahakarapp.rajasthan.gov.in/Content/GodownDocument/" + responseModel.DocName
            }, JsonRequestBehavior.AllowGet);
        }


        #region Generated FS and eSign Process on 06-10-2026
        [HttpGet]
        public JsonResult GenerateFSPdfByTemplate(int ApplicationId, int NextStatusId, string fsType)
        {
            GodownModel objGodownModel = new GodownModel(new DBLogger());
            var responseModel = new PacsResponseModel();
            var resultData = objGodownModel.GenerateCommonFSPdf(ApplicationId, NextStatusId, fsType);
            if (!string.IsNullOrWhiteSpace(resultData))
            {
                responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
            }

            if (responseModel == null)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "No response received."
                }, JsonRequestBehavior.AllowGet);
            }

            if (!responseModel.Success)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = responseModel.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Success = true,
                Result = 1,
                DocPath = responseModel.DocPath ?? "",
                DocType = responseModel.DocType ?? "",
                DocName = responseModel.DocName ?? "",
                ReturnPath = responseModel.ReturnPath ?? "",
                Location = responseModel.Location ?? "",
                Designation = responseModel.Designation ?? "",
                ViewPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/Content/GodownDocument/" + responseModel.DocName : "https://rajsahakarapp.rajasthan.gov.in/Content/GodownDocument/" + responseModel.DocName
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


}