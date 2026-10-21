using System;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Cooperative.Models;

namespace Cooperative.Controllers
{
    public class DBMigrationController : Controller

    {
        // GET: CodeMigration
        public ActionResult TableList()
        {
            return View();
        }

        //[Route("API/TableSchema/GetTableList")]
        [HttpPost]
        public string GetTableList()
        {
            DataMigration objRepo = new DataMigration();
            return COMMON_FUNCTION.DataTableToJSONString(objRepo.GetTableList());
        }

        //[Route("API/TableSchema/PushTableValue")]
        [HttpPost]
        public string PushTableDataLocal(string TableName)
        {
            string k = "!Faild";
            try
            {
                if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("103.203.138.116"))
                {
                    string apiUrlLive = "https://rajsahakar.rajasthan.gov.in/DBMigration/PushTableDataLive?TableName=" + TableName + "";                   
                    object input = new
                    {
                        Name = TableName,
                    };
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string data = client.UploadString(apiUrlLive, inputJson);                    
                    List<InsertData> InsertDatalst = JsonConvert.DeserializeObject<List<InsertData>>(data);

                    if (InsertDatalst.Count > 1)
                    {
                        k= new DataMigration().PushDataToLocalDB(InsertDatalst, TableName);
                    }
                }


                return k;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        //[Route("API/TableSchema/FromLive/PushTableValue")]
        [HttpPost]
        public string PushTableDataLive(string TableName)
        {
            DataTable DT = null;
            try
            {
                DataMigration objRepo = new DataMigration();
                DT = objRepo.GetTableData(TableName);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        //[HttpPost]
        //public JsonResult TurncateTable(string TableName)
        //{
        //    Repository obj = new Repository();
        //    bool output = obj.TurncateTable(TableName);
        //    return Json(new { success = output }, JsonRequestBehavior.AllowGet);
        //}
    }
}
