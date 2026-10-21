using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AcceptVerbs("Post")]
        public void Save()
        {
            var file = HttpContext.Request.Form["File"];

            try
            {
                
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadFiles"];

                if (httpPostedFile != null)
                {
                    var fileSave = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles"); 
                    var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName); // get the file path
                    if (!System.IO.File.Exists(fileSavePath))
                    {
                        httpPostedFile.SaveAs(fileSavePath); // save the file in the provided path
                        HttpResponse Response = System.Web.HttpContext.Current.Response;
                        Response.Clear();
                        Response.ContentType = "application/json; charset=utf-8";
                        Response.StatusDescription = "File uploaded succesfully";
                        Response.End();
                    }
                    else
                    {
                        HttpResponse Response = System.Web.HttpContext.Current.Response;
                        Response.Clear();
                        Response.Status = "204 File already exists";
                        Response.StatusCode = 204;
                        Response.StatusDescription = "File already exists";
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.Status = "204 No Content";
                Response.StatusDescription = e.Message;
                Response.End();
            }
        }
        [AcceptVerbs("Post")]
        public void Remove()
        {
            try
            {
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadFiles"];
                var fileSave = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
                var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath); // removed the uploaded file from the saved area
                }
                
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.Status = "200 OK";
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusDescription = "File removed succesfully";
                Response.End();
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.Status = "200 OK";
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusDescription = "File removed succesfully";
                Response.End();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}