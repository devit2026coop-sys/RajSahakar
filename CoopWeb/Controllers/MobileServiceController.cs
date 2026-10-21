using CoopDAL;
using Cooperative.Filters;
using Cooperative.JanadhaarClasses;
using Cooperative.Models;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Cooperative.Models.GopalCreditCardModel;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Controllers
{
    public class MobileServiceController : Controller
    {


        [HttpPost]
        public string MobileAppService(CallService obj)
        {

            var urldt = new System.Data.DataTable("tableName");

            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            string json = "";
            try
            {

                CoopMobileService objcall = new CoopMobileService();
                MobileServiceEntity objMSEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<MobileServiceEntity>(obj.srvparam);
                if (obj.usrnm == "cooperative" && (obj.psw == "cooperative@123"))
                {
                    try
                    {
                        switch (obj.srvnm)
                        {
                            case "SoceityAudit":


                                switch (obj.srvmethodnm)
                                {
                                    case "auditcounts":
                                        json = objcall.GetAuditCounts(objMSEntity);
                                        break;
                                    case "auditdetail":
                                        json = objcall.GetAuditdetail(objMSEntity);
                                        break;

                                    default:
                                        urldt.Rows.Add(new Object[]{
                                        "Service method name not valid!!!",1,
                                        ""
                                        });
                                        json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                                        break;


                                }
                                break;

                            case "SocietyInfo":
                                switch (obj.srvmethodnm)
                                {
                                    case "getMemberInfo":
                                        json = objcall.getMemberInfo(objMSEntity);
                                        break;
                                    case "savegeotagInfo":
                                        json = objcall.saveGeotagInfo(objMSEntity);
                                        break;
                                    case "blockbyDistrictId":
                                        json = objcall.blockbyDistrictId(objMSEntity);
                                        break;
                                    case "SocietiesbyBlockid":
                                        json = objcall.SocietiesbyBlockid(objMSEntity);
                                        break;
                                    case "GetAppversionByName":
                                        json = objcall.GetAppversionByName(objMSEntity);
                                        break;

                                    default:
                                        urldt.Rows.Add(new Object[]{
                                        "Service method name not valid!!!",1,
                                        ""
                                        });
                                        json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                                        break;


                                }

                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandleMobile(new ErrorLogEntity() { PageName = "MobileApp_Service", FunctionName = "MobileAppService(" + ex + ")" }, ex);
                        urldt.Rows.Add(new Object[]{
                                "Please try Again",1,
                              "Please try Again"
                                });

                        json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                        json = json.Replace("\"[", "[").Replace("]\"", "]");
                    }
                }

                else
                {
                    //json = "Service Authentication Failed!!!";
                    urldt.Rows.Add(new Object[]{
                                "Service Authentication Failed!!!",1,
                                ""

                                });
                    json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                }



                return json;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandleMobile(new ErrorLogEntity() { PageName = "MobileApp_Service", FunctionName = "MobileAppService(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                 "Please try Again",1,
                                "Please try Again"
                                });
                json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                return json;
            }


        }



        [HttpPost]
        public string ImageUploaderAPI(fileuploadEntity objfile)
        {
            string json = "";
            string path = "";
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));

            try
            {
                if (objfile.files.ContentLength > 0)
                {
                    string FileName = Path.GetFileNameWithoutExtension(objfile.files.FileName);
                    string FileExtension = Path.GetExtension(objfile.files.FileName);

                    FileName = FileName.Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + FileExtension;
                    if (objfile.IsDirectUpload)
                    {
                        objfile.ImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Commonfiles");
                        if (!Directory.Exists(objfile.ImagePath))
                        {
                            Directory.CreateDirectory(objfile.ImagePath);

                        }
                    }

                    objfile.files.SaveAs(Path.Combine(objfile.ImagePath, FileName));

                    path = objfile.ImagePath + "/" + FileName;


                    if (System.IO.File.Exists(path))
                    {
                        if (objfile.IsDirectUpload)
                        {
                            string url;
                            // new csCommon().GetURLByKey("ApplicationURL");
                            url = "https://" + Request.Url.Authority.ToString().Replace(":" + Request.Url.Port, string.Empty);
                            url = url + "/Content/" + objfile.AppName + "/Commonfiles/" + FileName;
                            urldt.Rows.Add(new Object[]{
                                "success",0,
                               url
                                });

                            json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                            json = json.Replace("\"[", "[").Replace("]\"", "]");


                        }
                        else
                        {
                            // create table
                            urldt.Rows.Add(new Object[]{
                                "Please try again",1,
                               "data not accurate"
                                });

                            json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                            json = json.Replace("\"[", "[").Replace("]\"", "]");
                            //json = "record not exists";
                        }

                    }


                }
                return json;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandleMobile(new ErrorLogEntity() { PageName = "Service", FunctionName = "ImageUploader(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "Failed !!!",1,
                                ""
                                });
                json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                return json;
            }


        }
        //


        public class CallService
        {
            public string usrnm { get; set; }
            public string psw { get; set; }
            public int userid { get; set; }
            public string srvnm { get; set; }
            public string srvmethodnm { get; set; }
            public string srvparam { get; set; }
            public string srvresponce { get; set; }
            public List<object> data = new List<object>();
            public string AppCode { get; set; }
            public string EncKey { get; set; }
        }


        public class fileuploadEntity
        {
            public int id { get; set; }

            public HttpPostedFileBase files { get; set; }
            public int ImageId { get; set; }
            public string AppName { get; set; }
            public string tablename { get; set; }
            public string columnname { get; set; }
            public string ImagePath { get; set; }
            public string uniqueidcolumnname { get; set; }
            public bool IsDirectUpload { get; set; }
            public int DocumentMasterId { get; set; }
            public int SubsidyId { get; set; }
            public long ApplicationId { get; set; }
            public long CreatedBy { get; set; }

            public string latitude { get; set; }
            public string longitude { get; set; }
        }




    }
}
