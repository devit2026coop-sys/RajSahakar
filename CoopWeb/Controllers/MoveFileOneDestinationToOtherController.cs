using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;
using System.Runtime.Remoting;
using System.Security.Policy;
using static iTextSharp.text.pdf.PdfDocument;
using sun.misc;
using java.security;
using System.Deployment.Internal;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using Ionic.Zip;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Ajax.Utilities;

namespace Cooperative.Controllers
{
    // [LogActionFilter]
    public class MoveFileOneDestinationToOtherController : Controller
    {
        public ActionResult MoveFile()
        {
            if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("10.70.234.208") && "tailorravi68" != _LoggedInUser.SSOID.ToLower().Trim())
            {
                return new ViewResult { ViewName = "~/Views/Error/UnAuthorizeAccess401" };
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
        [ActionName("CopyandMoveDirectory")]
        public string CopyandMoveDirectory(string sourceDirectory, string targetDirectory, string act)//Move and copy Directory One to Other
        {
            if (sourceDirectory != "" && targetDirectory != "")
            {
                sourceDirectory = Server.MapPath("~/" + sourceDirectory);
                targetDirectory = Server.MapPath("~/" + targetDirectory);
            }

            if (sourceDirectory.Contains("."))
            {
                string fileName = Path.GetFileName(sourceDirectory);
                string destinationFilePath = Path.Combine(targetDirectory, fileName);


                if (act == "Copy")
                {
                    System.IO.File.Copy(sourceDirectory, destinationFilePath, false);
                    return "Success";
                }
                else
                {
                    string a = CompressPdf(sourceDirectory);
                    if (a == "SUCCESS")
                        return "Success";
                    else
                        return "Fail";

                }

            }


            string currentPath = Environment.CurrentDirectory;
            try
            {
                if (act == "Copy")
                {
                    CopyDirectory(sourceDirectory, targetDirectory);
                }
                else if (act == "Move")
                {
                    MoveDirectory(sourceDirectory, targetDirectory);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            };
            return "Success";
        }

        static long GetDirectorySize(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;

            return Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories)
                            .Sum(file => new FileInfo(file).Length);
        }

        static string FormatSize(long bytes)
        {
            const double KB = 1024;
            const double MB = KB * 1024;
            const double GB = MB * 1024;
            const double TB = GB * 1024;

            if (bytes >= TB)
                return $"{bytes / TB:F2} TB";
            else if (bytes >= GB)
                return $"{bytes / GB:F2} GB";
            else if (bytes >= MB)
                return $"{bytes / MB:F2} MB";
            else if (bytes >= KB)
                return $"{bytes / KB:F2} KB";
            else
                return $"{bytes} Bytes";
        }

        [HttpPost]
        [ActionName("AllFilesInDirectory")]
        public string AllFilesInDirectory(string sourceDirectory, bool isSize = false)//Move and copy Directory One to Other
        {
            string DirectoryPath = sourceDirectory;
            ////var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            if (sourceDirectory == "")
            {
                sourceDirectory = Server.MapPath("../");
            }
            else
            {
                sourceDirectory = Server.MapPath("~/" + sourceDirectory);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileSize");
            dt.Columns.Add("LastModifiedDate");
            dt.Columns.Add("FullPath");

            //if (dt == null)
            //{
            //    dt = new DataTable();
            //    dt.Columns.Add("FileName", typeof(string));                
            //    dt.Columns.Add("FileSize", typeof(string));
            //    dt.Columns.Add("LastModifiedDate", typeof(DateTime));
            //    dt.Columns.Add("FullPath", typeof(string));
            //}
            if (!isSize)
            {
                try
                {
                    // Check if the source directory exists
                    if (Directory.Exists(sourceDirectory))
                    {
                        // Get all files in the source directory
                        string[] files = Directory.GetFiles(sourceDirectory);
                        // Read each file to the target directory
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            int lastSlashIndex = file.LastIndexOf('/');
                            FileInfo fileInfo = new FileInfo(file);
                            long fileSizeInBytes = fileInfo.Length;
                            DateTime lastModified = fileInfo.LastWriteTime;
                            dt.Rows.Add(fileName,/* FormatSize(fileSizeInBytes) */"not count", lastModified, DirectoryPath + "/" + fileName);
                        }
                        string[] subDirs = Directory.GetDirectories(sourceDirectory);
                        foreach (string subDir in subDirs)
                        {
                            string dirName = Path.GetFileName(subDir);
                            long size = GetDirectorySize(subDir);
                            DirectoryInfo dirInfo = new DirectoryInfo(subDir);
                            DateTime lastModified = dirInfo.LastWriteTime;
                            dt.Rows.Add(dirName, /*FormatSize(size)*/"not count", lastModified, DirectoryPath + "/" + dirName);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Source directory '{sourceDirectory}' does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    dt = null;
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MoveFileOneDestinationtoOther", FunctionName = " AllFilesInDirectory(string sourceDirectory)1 " }, ex);

                }
            }
            else
            {
                List<object> fileList = new List<object>();
                try
                {
                    var fileBag = new ConcurrentBag<DataRow>();
                    int fileLimit = 5; // Limit the number of files processed
                    int fileCount = 0;
                    int dirCount = 0;
                    object lockObj = new object();
                    // Process Files in Parallel
                    Parallel.ForEach(Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.TopDirectoryOnly), (file, state) =>
                    {
                        //if (fileCount >= fileLimit)
                        //{
                        //    state.Stop(); // Stop processing further files
                        //    return;
                        //}
                        FileInfo fileInfo = new FileInfo(file);
                        // DataRow row = dt.NewRow();
                        //row["FileName"] = fileInfo.Name;
                        //row["LastModifiedDate"] = fileInfo.LastWriteTime;
                        //row["FullPath"] = DirectoryPath + "/" + fileInfo.Name;

                        DataRow row;


                        lock (lockObj)
                        {
                            row = dt.NewRow(); // Ensure DataRow is created inside a lock
                            row["FileName"] = fileInfo.Name;
                            row["LastModifiedDate"] = fileInfo.LastWriteTime;
                            row["FullPath"] = DirectoryPath + "/" + fileInfo.Name;
                        }



                        if (fileCount <= fileLimit) //if (Interlocked.Increment(ref fileCount) <= fileLimit)
                        {
                            row["FileSize"] = FormatSize(fileInfo.Length);
                        }
                        else
                        {
                            row["FileSize"] = "Not Calculated";
                        }

                        Interlocked.Increment(ref fileCount);
                        fileBag.Add(row);

                    });

                    // Process Directories in Parallel
                    Parallel.ForEach(Directory.EnumerateDirectories(sourceDirectory), (dir, state) =>
                    {
                        //if (fileCount >= fileLimit)
                        //{
                        //    state.Stop(); // Stop processing further directories
                        //    return;
                        //}
                        DirectoryInfo dirInfo = new DirectoryInfo(dir);
                        DataRow row = dt.NewRow();
                        lock (lockObj)
                        {
                            row["FileName"] = dirInfo.Name;
                            // row["FileSize"] = FormatSize(GetDirectorySize(dir));
                            row["LastModifiedDate"] = dirInfo.LastWriteTime;
                            row["FullPath"] = DirectoryPath + "/" + dirInfo.Name;
                        }
                        if (dirCount <= fileLimit) // if (Interlocked.Increment(ref dirCount) <= fileLimit)
                        {
                            row["FileSize"] = FormatSize(GetDirectorySize(dir));
                        }
                        else
                        {
                            row["FileSize"] = "Not Calculated";
                        }
                        fileBag.Add(row);
                        Interlocked.Increment(ref dirCount);
                    });

                    // Add collected rows to DataTable
                    foreach (var row in fileBag)
                    {
                        dt.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MoveFileOneDestinationtoOther", FunctionName = " AllFilesInDirectory(string sourceDirectory) " }, ex);
                    // dt = null;
                }
            }

            string a = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
        }



        static void CopyDirectory(string sourceDir, string targetDir)
        {
            // Create the target directory if it does not exist
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copy all files in the current directory
            string[] files = Directory.GetFiles(sourceDir);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDir, fileName);
                System.IO.File.Copy(file, destFile, true); // Overwrite existing files if necessary
            }

            // Copy all subdirectories and their contents
            string[] subDirs = Directory.GetDirectories(sourceDir);
            foreach (string subDir in subDirs)
            {
                string dirName = Path.GetFileName(subDir);
                string destDir = Path.Combine(targetDir, dirName);
                CopyDirectory(subDir, destDir); // Recursively copy subdirectories
            }
        }
        static void MoveDirectory(string sourceDir, string targetDir)
        {
            // Create the target directory if it does not exist
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copy all files in the current directory
            string[] files = Directory.GetFiles(sourceDir);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDir, fileName);
                System.IO.File.Move(file, destFile); // Overwrite existing files if necessary
            }

            // Copy all subdirectories and their contents
            string[] subDirs = Directory.GetDirectories(sourceDir);
            foreach (string subDir in subDirs)
            {
                string dirName = Path.GetFileName(subDir);
                string destDir = Path.Combine(targetDir, dirName);
                MoveDirectory(subDir, destDir); // Recursively copy subdirectories
            }
        }
        [HttpPost]
        public string UploadMultipleFile(string dirName)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    //string fileDirectory = Server.MapPath("~/Content/SocietyProfiling/" + dirName);
                    string fileDirectory = "";
                    if (dirName.Contains("../"))
                        fileDirectory = Server.MapPath("~/" + dirName.Replace("../",""));
                    else
                        fileDirectory = Server.MapPath("~/Content/" + dirName);

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = file.FileName;

                        file.SaveAs(Path.Combine(fileDirectory, fileName));
                    }
                }
                catch (System.Exception ex)
                {
                    return "";
                }
            }

            return fileName;
        }



        [HttpPost]
        [ActionName("ReadDirectoryData")]
        public string ReadDirectoryData(string sourceDirectory)//Move and copy Directory One to Other
        {
            var Message = "";
            try
            {
                Message = System.IO.File.ReadAllText(Server.MapPath("~/" + sourceDirectory));
            }
            catch (Exception)
            {

            }
            return Message;
        }


        [HttpPost]
        [ActionName("UpdateNewRegNo")]
        public void UpdateNewRegNo()
        {
            // CopyDirectory(,)
        }


        #region Get All Societies Which RegNO Change (remove district like anoopgarh) 23jan2025
        public string GetAllUpdatedRegNoSocieties()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable dt = new DataTable();
            dt = objSocietyModel.GetAllUpdatedRegNoSocieties();

            int count = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    try
                    {
                        if (dr["oldRegNo"].ToString() != "" && dr["NewRegNo"].ToString() != "")
                        {
                            string sourceDir = "Content/SocietyProfiling/" + dr["oldRegNo"].ToString().Replace("/", "").Replace("-", "").Replace(" ", "");
                            string targetDir = "Content/SocietyProfiling/" + dr["NewRegNo"].ToString().Replace("/", "").Replace("-", "").Replace(" ", "");
                            int SocietyId = Convert.ToInt32(dr["SocietyId"]);


                            string sourceDirPath = Server.MapPath("~/" + sourceDir);
                            string targetDirPath = Server.MapPath("~/" + targetDir);

                            if (Directory.Exists(sourceDirPath))
                            {
                                string[] Sourcefiles = Directory.GetFiles(sourceDirPath);
                                Directory.Move(sourceDirPath, targetDirPath);
                               
                                if (Sourcefiles.Length > 0)
                                {
                                    //  CopyDirectory(sourceDirPath, targetDirPath);
                                    string[] targetfiles = Directory.GetFiles(targetDirPath);
                                    if (Sourcefiles.Length == targetfiles.Length)
                                    {
                                        var dt1 = objSocietyModel.RegNoChangeStatus(SocietyId);
                                        count += 1;
                                        // return Json(new { Result = true, Message = "", data = dt });
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MoveFileOneDestinationToOther", FunctionName = "GetAllUpdatedRegNoSocieties()" }, ex);
                    }

                }
                // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
            }
            return "Success";

        }

        #endregion

        #region CompressPdf     

        public static string CompressPdf(string file)
        {
            string backupPdf = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_backup" + Path.GetExtension(file));
            string compressedPdf = file; // Overwrite the original file
            System.IO.File.Copy(file, backupPdf, true); // Take a backup  
            try
            {
                //CompressPdf(backupPdf, file);

                using (PdfReader reader = new PdfReader(backupPdf))
                using (FileStream outputStream = new FileStream(file, FileMode.Create))
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                    writer.SetFullCompression(); // ✅ Correctly applied before opening document

                    document.Open();
                    PdfContentByte content = writer.DirectContent;

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        content.AddTemplate(page, 0, 0);
                    }

                    document.Close();
                    writer.Close();
                    reader.Close();
                }
                return "SUCCESS";
                //  Console.WriteLine("PDF compressed successfully.");
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MoveFileOneDestinationToOther", FunctionName = "CompressPdf(string file)" + file }, ex);

            }
            return "";
        }

        //public  void DownloadFile(string File)
        //{
        //     File = Server.MapPath("~/" + File);
        //    File=File.Replace("\\", "/");           
        //    string localFilePath = @"D:/Downloads/";    

        //        // response.StatusCode == HttpStatusCode.OK; // Returns true if file exists
        //        if (!Directory.Exists(localFilePath))
        //        {
        //            Directory.CreateDirectory(localFilePath);
        //        }
        //        if (!System.IO.File.Exists(File))
        //        {
        //            Console.WriteLine("Error: Source file does not exist!");
        //            return;
        //        }
        //        if (!Directory.Exists(localFilePath))
        //        {
        //            Directory.CreateDirectory(localFilePath);
        //        }          

        //        using (WebClient client = new WebClient())
        //        {
        //            client.DownloadFile(File, localFilePath);
        //            Console.WriteLine("File downloaded successfully!");
        //        }      

        //}

        public ActionResult DownloadFile(string filePath)
        {
            string fullPath = Server.MapPath("~/" + filePath);
            if (System.IO.File.Exists(fullPath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes, "text/html", Path.GetFileName(fullPath));
            }
            else
            {
                return HttpNotFound("File not found.");
            }
        }

        [HttpPost]
        public JsonResult DeleteFile(string filePath)
        {
            try
            {
                string fullPath = Server.MapPath("~/" + filePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Json(new { success = true, message = "File deleted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "File not found!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult RemoveUnnecessaryGuarantors()
        {
            DataTable dt = new DataTable();
            GopalCreditCardDBModel model = new GopalCreditCardDBModel();
            dt = model.GetUnnecessaryGuarantors();
            int Result = -1;

            if (dt != null)
            {

                foreach (DataRow row in dt.Rows)
                {
                    string PhotoPath = row["Photo"].ToString();  // Example column
                    string SignaturePath = row["Signature"].ToString();  // Example column
                    string PhotoPath1 = row["Photo1"].ToString();  // Example column
                    string SignaturePath1 = row["Signature1"].ToString();  // Example column        
                    try
                    {
                        string fullPhotoPath = Server.MapPath("~/" + PhotoPath);
                        string fullSignaturePath = Server.MapPath("~/" + SignaturePath);

                        string fullPhotoPath1 = Server.MapPath("~/" + PhotoPath1);
                        string fullSignaturePath1 = Server.MapPath("~/" + SignaturePath1);

                        if ((System.IO.File.Exists(fullPhotoPath) || System.IO.File.Exists(fullSignaturePath)) || (System.IO.File.Exists(fullPhotoPath1) || System.IO.File.Exists(fullSignaturePath1)))
                        {
                            if (System.IO.File.Exists(fullPhotoPath)) System.IO.File.Delete(fullPhotoPath);
                            if (System.IO.File.Exists(fullSignaturePath)) System.IO.File.Delete(fullSignaturePath);

                            if (System.IO.File.Exists(fullPhotoPath1)) System.IO.File.Delete(fullPhotoPath1);
                            if (System.IO.File.Exists(fullSignaturePath1)) System.IO.File.Delete(fullSignaturePath1);

                            Result = model.RemoveFileOfUnnecessaryGuarantors(Convert.ToInt32(row["Id"]));
                            //return Json(new { success = true, message = "File deleted successfully!" });
                        }
                        else
                        {
                            // return Json(new { success = false, message = "File not found!" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Error: " + ex.Message });
                    }

                }

            }
            if (Result > 0)
            { return Json(new { success = true, message = "File deleted successfully!" }); }
            return Json(new { success = false, message = "Data Not Found! " });
        }


        #endregion

    }

}