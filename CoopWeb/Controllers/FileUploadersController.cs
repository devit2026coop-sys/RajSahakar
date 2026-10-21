using com.sun.istack.@internal.logging;
using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [LogActionFilter]
    public class FileUploadersController : Controller
    {
        // GET: FileUploaders
        public ActionResult FileUploaders()
        {
            if ("tailorravi68" != _LoggedInUser?.SSOID.ToLower().Trim())
            {
                return RedirectToAction("UnAuthorizeAccess401", "Error");
            }
            return View();
        }

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        [ActionName("GetDirectoryData")]
        public string GetDirectoryData(string ID = "")
        {
            string ReturnVal = "";
            FileUploadersModel objFileUploadersModel = new FileUploadersModel(new FileLogger());
            if (ID.Length > 0)
            {
                return COMMON_FUNCTION.DataTableToJSONString(objFileUploadersModel.GetAllDirectory(" and Flag=" + ID));
            }
            else
                return COMMON_FUNCTION.DataTableToJSONString(objFileUploadersModel.GetAllDirectory(" and Flag=0"));

        }

        [HttpPost]
        [ActionName("SaveDirectory")]
        public string SaveDirectory(FileUploaderEntity _Entity)//FileUploadersModel _Entity
        {
            _Entity.Message = "-1";
            try
            {
                string fileName = string.Empty;
                string pathString = "";
                if (_Entity.IsWeb)
                {
                    _Entity.WebConfigText = System.IO.File.ReadAllText(Server.MapPath("~/Web.config"));
                    _Entity.Message = _Entity.WebConfigText;
                }
                else if (!_Entity.File.Contains("."))
                {
                    if (_Entity.IsNewDirectory == 1)
                    {
                        pathString = "../" + _Entity.NewDirectory;
                        if (!Directory.Exists(pathString))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }
                    }
                    else if (_Entity.DirectoryName == "ROOT" && _Entity.SubDirectoryName == "undefined")
                    {
                        if (!Directory.Exists(pathString))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }
                    }
                    else if (_Entity.DirectoryName != null && _Entity.SubDirectoryName != null)
                    {
                        pathString = "~/" + _Entity.DirectoryName + "/" + _Entity.SubDirectoryName + "/";
                        if (!Directory.Exists(pathString))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }
                    }
                    //INSERT DATA IN TABLE
                    FileUploadersModel objFileUploadersModel = new FileUploadersModel(new FileLogger());
                    _Entity.Message = (objFileUploadersModel.InsertDirectory(_Entity)).ToString();
                    //END
                }
                else if (_Entity.DirectoryName != null && _Entity.SubDirectoryName == "ROOT")
                {
                    pathString = "../" + _Entity.DirectoryName + "/";
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }
                }
                else if (_Entity.DirectoryName != null && _Entity.SubDirectoryName != null)
                {
                    pathString = "../" + _Entity.DirectoryName + "/" + _Entity.SubDirectoryName + "/";
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploaders", FunctionName = "GetAllDirectory" }, ex);
            }

            return _Entity.Message;

        }



        [HttpGet]
        public ActionResult SaveSocietyFile()
        {
            //UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            //    ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorData, (Request.Cookies[SessionIdentity.Culture]).Value);
            //}
            return View();
        }

        [HttpPost]
        public string SaveSocietyAllFile()
        {
            string ReturnVal = "";



            DataTable dt = new DataTable();

            SocietyModel objSocietyModelModel = new SocietyModel(new FileLogger());
            //  DataTable dt = objSocietyModelModel.GetAllActiveSocietyList();
            string WorkPlanEcoViabilityFile;
            string GeneralMeetingProcedureFile;
            string BalanceSheetExcelFile;
            string ObjectsFile;
            string SOCIETIESAUTHORIZEDPERSONS;
            string SLSAFileName;
            string MemberListFileName;
            string DLSCFileName;
            string SOCFileName;
            string RSSCFileName;
            string RulesRegulationFile;
            string BylawsFile;
            string RegistrationCertificateFile;
            string BankOpeningCertificate;
            string BankPassBook;
            string SocietyID = "", RegistrationNo = "", SocietyName = "";
            string QueryForUpdateSociety = "";
            foreach (DataRow row in dt.Rows)
            {
                if (row["SocietyID"] != null)
                {
                    SocietyID = row["SocietyID"].ToString();
                }
                if (row["RegistrationNo"] != null)
                {
                    RegistrationNo = row["RegistrationNo"].ToString();
                }
                if (row["SocietyName"] != null)
                {
                    SocietyName = row["SocietyName"].ToString();
                }
                string pathString = "~/Content/SocietyProfiling/";

                pathString += Regex.Replace(RegistrationNo, "[^a-zA-Z0-9_]+", "");
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }

                if (row["WorkPlanEcoViabilityFile"] != null)
                {
                    WorkPlanEcoViabilityFile = row["WorkPlanEcoViabilityFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + WorkPlanEcoViabilityFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = OldfileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(WorkPlanEcoViabilityFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        WorkPlanEcoViabilityFile = "";
                    }
                }
                else
                {
                    WorkPlanEcoViabilityFile = "";
                }


                if (row["GeneralMeetingProcedureFile"] != null)
                {
                    GeneralMeetingProcedureFile = row["GeneralMeetingProcedureFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + GeneralMeetingProcedureFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = GeneralMeetingProcedureFile = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(GeneralMeetingProcedureFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        GeneralMeetingProcedureFile = "";
                    }
                }
                else
                {
                    GeneralMeetingProcedureFile = "";
                }

                if (row["BalanceSheetExcelFile"] != null)
                {
                    BalanceSheetExcelFile = row["BalanceSheetExcelFile"].ToString();
                    string OldfileName = BalanceSheetExcelFile = "~/OldSocietyProfiling/" + BalanceSheetExcelFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(BalanceSheetExcelFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        BalanceSheetExcelFile = "";
                    }
                }
                else
                {
                    BalanceSheetExcelFile = "";
                }

                if (row["ObjectsFile"] != null)
                {
                    ObjectsFile = row["ObjectsFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + ObjectsFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = ObjectsFile = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(ObjectsFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        ObjectsFile = "";
                    }
                }
                else
                {
                    ObjectsFile = "";
                }

                if (row["SOCIETIESAUTHORIZEDPERSONS"] != null)
                {
                    SOCIETIESAUTHORIZEDPERSONS = row["SOCIETIESAUTHORIZEDPERSONS"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + SOCIETIESAUTHORIZEDPERSONS;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = SOCIETIESAUTHORIZEDPERSONS = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(SOCIETIESAUTHORIZEDPERSONS);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        SOCIETIESAUTHORIZEDPERSONS = "";
                    }
                }
                else
                {
                    SOCIETIESAUTHORIZEDPERSONS = "";
                }
                if (row["SLSAFileName"] != null)
                {
                    SLSAFileName = row["SLSAFileName"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + SLSAFileName;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = SLSAFileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(SLSAFileName);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        SLSAFileName = "";
                    }
                }
                else
                {
                    SLSAFileName = "";
                }
                if (row["MemberListFileName"] != null)
                {
                    MemberListFileName = row["MemberListFileName"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + MemberListFileName;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = MemberListFileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(MemberListFileName);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        MemberListFileName = "";
                    }
                }
                else
                {
                    MemberListFileName = "";
                }

                if (row["DLSCFileName"] != null)
                {
                    DLSCFileName = row["DLSCFileName"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + DLSCFileName;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = DLSCFileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(DLSCFileName);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        DLSCFileName = "";
                    }
                }
                else
                {
                    DLSCFileName = "";
                }

                if (row["SOCFileName"] != null)
                {
                    SOCFileName = row["SOCFileName"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + SOCFileName;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = SOCFileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(SOCFileName);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        SOCFileName = "";
                    }
                }
                else
                {
                    SOCFileName = "";
                }
                if (row["RSSCFileName"] != null)
                {
                    RSSCFileName = row["RSSCFileName"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + RSSCFileName;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = RSSCFileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(RSSCFileName);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        RSSCFileName = "";
                    }
                }
                else
                {
                    RSSCFileName = "";
                }

                if (row["RulesRegulationFile"] != null)
                {
                    RulesRegulationFile = row["RulesRegulationFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + RulesRegulationFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = RulesRegulationFile = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(RulesRegulationFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        RulesRegulationFile = "";
                    }
                }
                else
                {
                    RulesRegulationFile = "";
                }

                if (row["BylawsFile"] != null)
                {
                    BylawsFile = row["BylawsFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + BylawsFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = BylawsFile = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(BylawsFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        BylawsFile = "";
                    }
                }
                else
                {
                    BylawsFile = "";
                }
                if (row["RegistrationCertificateFile"] != null)
                {
                    RegistrationCertificateFile = row["RegistrationCertificateFile"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + RegistrationCertificateFile;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = RegistrationCertificateFile = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(RegistrationCertificateFile);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        RegistrationCertificateFile = "";
                    }
                }
                else
                {
                    RegistrationCertificateFile = "";
                }
                if (row["BankOpeningCertificate"] != null)
                {
                    BankOpeningCertificate = row["BankOpeningCertificate"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + BankOpeningCertificate;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = BankOpeningCertificate = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(BankOpeningCertificate);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        BankOpeningCertificate = "";
                    }
                }
                else
                {
                    BankOpeningCertificate = "";
                }
                if (row["BankPassBook"] != null)
                {
                    BankPassBook = row["BankPassBook"].ToString();
                    string OldfileName = "~/OldSocietyProfiling/" + BankPassBook;
                    FileInfo fi = new FileInfo(OldfileName);
                    if (fi.Exists)
                    {
                        var newfileName = BankPassBook == Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(BankPassBook);
                        System.IO.File.Copy(OldfileName, pathString + "/" + newfileName);
                    }
                    else
                    {
                        BankPassBook = "N A";
                    }
                }
                else
                {
                    BankPassBook = "";
                }
                QueryForUpdateSociety += " update Societies set " +
                "   WorkPlanEcoViabilityFile	 ='" + WorkPlanEcoViabilityFile + "'" +
                " , GeneralMeetingProcedureFile ='" + GeneralMeetingProcedureFile + "'" +
                " , BalanceSheetExcelFile ='" + BalanceSheetExcelFile + "'" +
                " , ObjectsFile ='" + ObjectsFile + "'" +
                " , SOCIETIESAUTHORIZEDPERSONS ='" + SOCIETIESAUTHORIZEDPERSONS + "'" +
                " , SLSAFileName ='" + SLSAFileName + "'" +
                " , MemberListFileName ='" + MemberListFileName + "'" +
                " , DLSCFileName ='" + DLSCFileName + "'" +
                " , SOCFileName ='" + SOCFileName + "'" +
                " , RSSCFileName ='" + RSSCFileName + "'" +
                " , RulesRegulationFile ='" + RulesRegulationFile + "'" +
                " , BylawsFile ='" + BylawsFile + "'" +
                " , RegistrationCertificateFile ='" + RegistrationCertificateFile + "'" +
                " , BankOpeningCertificate ='" + BankOpeningCertificate + "'" +
                " , BankPassBook ='" + BankPassBook + "'" +
                "  where SocietyID=" + SocietyID + " ; ";
            }
            FileUploadersModel objFileUploadersModel = new FileUploadersModel(new FileLogger());
            //  objFileUploadersModel.SaveSocietyAllFile(QueryForUpdateSociety).ToString();


            dt.Clear();
            dt = objSocietyModelModel.GetAllActiveSocietyAminDetailList();
            string PhotoOld = "";
            string AdministrativeId = "";
            QueryForUpdateSociety = " ";
            foreach (DataRow row in dt.Rows)
            {
                if (row["SocietyID"] != null)
                {
                    SocietyID = row["SocietyID"].ToString();
                }
                if (row["RegistrationNo"] != null)
                {
                    RegistrationNo = row["RegistrationNo"].ToString();
                }
                if (row["SocietyName"] != null)
                {
                    SocietyName = row["SocietyName"].ToString();
                }
                if (row["AdministrativeId"] != null)
                {
                    AdministrativeId = row["AdministrativeId"].ToString();
                }
                string pathString = "~/Content/SocietyProfiling/";

                pathString += Regex.Replace(RegistrationNo, "[^a-zA-Z0-9_]+", "");
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }

                if (row["PhotoOld"] != null)
                {
                    PhotoOld = row["PhotoOld"].ToString();
                    byte[] bytes = Convert.FromBase64String(PhotoOld);
                    var newfileName = PhotoOld = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".jpg";

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        FileStream fs = new FileStream(Server.MapPath(pathString + "/" + newfileName), FileMode.CreateNew, FileAccess.Write);
                        ms.WriteTo(fs);
                        fs.Close();
                        fs.Dispose();
                        ms.Flush();
                    }
                }
                else
                {
                    PhotoOld = "N A";
                }
                QueryForUpdateSociety += " update AdministrativeDetails set " +
                " Photo='" + PhotoOld + "'" +
                " where AdministrativeId=" + AdministrativeId + " ; ";
            }
            //   FileUploadersModel objFileUploadersModel = new FileUploadersModel(new FileLogger());
            return objFileUploadersModel.SaveSocietyAllFile(QueryForUpdateSociety).ToString();


        }

        [HttpPost]
        [ActionName("SaveFileAfterResize")]
        public string SaveFileAfterResize(FileUploaderEntity _Entity)//FileUploadersModel _Entity
        {
            _Entity.Message = "-1";
            try
            {
                string fileName = string.Empty;
                string pathString = "";
                if (_Entity.DirectoryName != null)
                {
                    pathString = "../Content/SocietyProfiling/" + _Entity.DirectoryName + "/";
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = file.FileName;
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        return _Entity.Message = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploaders", FunctionName = "GetAllDirectory" }, ex);
            }

            return _Entity.Message;

        }

        [HttpPost]
        [ActionName("CommonFileUploader")]
        public JsonResult CommonFileUploader()
        {
            try
            {
                string fileName = string.Empty;
                string pathString = "~/TempFiles/";
                int randomNum = new Random().Next(1000, 10000);
                if (!Directory.Exists(Server.MapPath(pathString)))
                    Directory.CreateDirectory(Server.MapPath(pathString));
                string[] filesArr = Directory.GetFiles(Server.MapPath(pathString));
                if (filesArr.Length > 0)
                {
                    foreach (string file in filesArr)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            FileInfo info = new FileInfo(file);
                            if (info.LastWriteTime < DateTime.Now.AddDays(-1)) //Remove one or more day old temp files
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                }
                var httpPostedFile = HttpContext.Request.Files["file"];
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    fileName = file.FileName.Replace(" ", "-");
                    string filePath = Path.Combine(Server.MapPath(pathString), DateTime.Now.ToString($"yyyyMMddHHmmssffff{randomNum.ToString()}-") + fileName);
                    string tempFilePath = Path.Combine(Server.MapPath(pathString), DateTime.Now.ToString($"yyyyMMddHHmmssffff{randomNum.ToString()}-") + "temp" + fileName);
                    if (Path.GetExtension(tempFilePath) == ".pdf")
                    {
                        file.SaveAs(tempFilePath);
                        filePath = WatermarkService.AddWatermarkToPdf(tempFilePath, filePath, "rajsahakarapp.rajasthan.gov.in");
                        if (System.IO.File.Exists(tempFilePath))
                        {
                            System.IO.File.Delete(tempFilePath);
                        }
                    }
                    else if (".wav,.ogg,.aac,.m4a,.mp3,.mp4,.avi,.flv,.wmv,.mov,.mkv,.mpeg,.3gp".Contains(Path.GetExtension(tempFilePath)))
                    {
                        file.SaveAs(filePath);
                    }
                    else if (".jpg,.jpeg,.png,.bmp".Contains(Path.GetExtension(tempFilePath)))
                        using (var image = Image.FromStream(httpPostedFile.InputStream))
                        {
                            filePath = WatermarkService.AddWatermarkToImage(image, filePath, "rajsahakarapp.rajasthan.gov.in");
                        }

                    return Json(new { Msg = "File uploaded successfully.", filePath = filePath });
                }
                else
                    return Json(new { Msg = "Please choose file to upload.", filePath = "" });

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploaders", FunctionName = "CommonFileUploader" }, ex);
            }
            return Json(new { Msg = "Error", filePath = "" });
        }

    }
}