using Cooperative.DBConnector;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace Cooperative.Models
{
    public class CoopModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public CoopModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public CoopModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        #region Added new API for returning UrlFilePath (29-04-2026) by Narpat 
        /// <summary>
        /// GetUrlByCoopId 
        /// </summary>
        /// <param name="pacsId"></param>
        /// <param name="coopId"></param>
        /// <param name="fileUploader"></param>
        /// <returns></returns>
        public async Task<string> GetUrlByCoopId(string pacsId, string coopId, HttpPostedFileBase fileUploader)
        {
            string returnPath = string.Empty;
            string fullPath = string.Empty;
            string baseUrl = string.Empty;
            try
            {
                if (fileUploader == null || string.IsNullOrEmpty(fileUploader.FileName))
                    return null;

                // Clean file name
                string fileName = Path.GetFileNameWithoutExtension(fileUploader.FileName);
                string extension = Path.GetExtension(fileUploader.FileName);

                fileName = Regex.Replace(fileName, @"[^a-zA-Z0-9]", "");

                string uniqFileName = "_" + Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
                //string finalFileName = fileName + coopId + uniqFileName + extension;

                string finalFileName = fileName + coopId + extension; //old code

                // Folder path
                string basePath = HttpContext.Current.Server.MapPath("~/Content/Uploader/" + coopId);

                // Create directory if not exists
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                // Full file path
                fullPath = Path.Combine(basePath, finalFileName);

                // Save file
                fileUploader.SaveAs(fullPath);
                string folderPath = "Content/Uploader";
                // Return URL
                if (HttpContext.Current.Request.Url.Host.Contains("localhost")) { baseUrl = "http://localhost:27466"; }
                else { baseUrl = "https://rajsahakarapp.rajasthan.gov.in"; }
                returnPath = $"{baseUrl}/{folderPath}/{coopId}/{finalFileName}";

                return await Task.FromResult(returnPath);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CoopModel",
                    FunctionName = "GetUrlByCoopId(string pacsId : '" + pacsId + "', string coopId: '" + coopId + "', HttpPostedFileBase fileUploader:'" + fileUploader + "')"
                }, ex);

                return null;
            }
        }


        public class ResponseModel
        {
            public string url { get; set; }
            public string status { get; set; }
            public string message { get; set; }

        }

        #endregion
    }
}
