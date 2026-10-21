using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Cooperative.DBConnector;
using Cooperative.Models;

namespace Cooperative.Controllers
{
    public class FileController : Controller
    {
        //private readonly HttpClient _httpClient;

        //public FileController(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}
        //private ILogger objILogger;
        //public FileController(ILogger objErrorLogger)
        //{
        //    objILogger = objErrorLogger;           
        //}

        // Download file from URL
        public async Task<ActionResult> DownloadFromUrl(string url)
        {
            try
            {
                var _httpClient = new HttpClient();

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var contentStream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType.ToString();
                var fileName = Path.GetFileName(url);

                return File(contentStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}