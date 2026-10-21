using Cooperative.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class EsanchaarController : Controller
    {
        public string orcl = string.Empty;
        public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        public const SslProtocols _Tls11 = (SslProtocols)0x00000300;

        public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        //public const SecurityProtocolType Tls11 = (SecurityProtocolType)_Tls11;
        public const SecurityProtocolType SystemDefault = (SecurityProtocolType)0;

        // GET: Esanchaar
        public ActionResult Index()
        {
            return View();
        }


        public string SendSMS(string language, string Message, List<string> Moblist, int Id)
        {
            try
            {
                string WhereClause = "";
                if (Id > 0)
                {
                    WhereClause = "and TemplateId=" + Id;
                    EsanchaarModel objEsanchaarModel = new EsanchaarModel(new DBLogger());

                    DataTable DtTemplate = objEsanchaarModel.GetSMSTempleteList(WhereClause);
                    Message = DtTemplate.Rows[0]["TemplateHeadingEN"].ToString().Replace("@", Message);
                }


                language = "ENG";

                DataTable Dt = null;
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("1100", false);
                string username = Dt.Rows[0]["MERCHANTCODE"].ToString();
                string password = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                string esanchaaruniqueid = Dt.Rows[0]["OFFICECODE"].ToString();
                string esanchaarserviceName = Dt.Rows[0]["SERVICENAME"].ToString();
                string esanchaarurl = Dt.Rows[0]["REDIRECTURL"].ToString();
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                Object obj, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors errors)
                { return (true); };

                string mobilelist = "";
                for (int i = 0; i < Moblist.Count; i++)
                {
                    mobilelist += "\"" + Moblist[i].ToString() + "" + "\",";
                }

                string inputparams = "{\"UniqueID\":\"" + esanchaaruniqueid + "\",\"serviceName\":\"" + esanchaarserviceName + "\",\"language\":\"" + language + "\",\"message\":\"" + Message + "\",\"mobileNo\": [" + mobilelist.Substring(0, mobilelist.Length - 1) + "]}";
                string status = WebRequestinJson(esanchaarurl, inputparams, "application/json", username, password);

                JObject root = (JObject)JObject.Parse(status);
                foreach (var item in root)
                {
                    if (item.Key == "responseCode")
                        status = item.Value.ToString().Replace("\"", "");
                }
                return status;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        public string SendSocietySMS(string Message, string MobNo, string Language = "",string templateID="")
        {
            try
            {
                string language = Language == "" ? "ENG" : "HIN";

                if (!string.IsNullOrEmpty(Language))
                    language = Language;


                DataTable Dt = null;
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                
                Dt = objEmitraModel.GetApplicationKey("1100", false);
                string username = Dt.Rows[0]["MERCHANTCODE"].ToString();
                string password = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                string esanchaaruniqueid = Dt.Rows[0]["OFFICECODE"].ToString();
                string esanchaarserviceName = Dt.Rows[0]["SERVICENAME"].ToString();
                string esanchaarurl = Dt.Rows[0]["REDIRECTURL"].ToString();
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                Object obj, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors errors)
                { return (true); };

                string mobilelist = "";
                //for (int i = 0; i < Moblist.Count; i++)
                //{
                //    mobilelist += "\"" + Moblist[i].ToString() + "" + "\",";
                //}

                //mobilelist = MobNo.Replace(",","\",\"");
                mobilelist += "\"" + MobNo.Replace(",", "\",\"") + "" + "\",";


                string inputparams = "{\"UniqueID\":\"" + esanchaaruniqueid + "\",\"serviceName\":\"" + esanchaarserviceName + "\",\"language\":\"" + language + "\",\"templateID\":\"" + templateID + "\",\"message\":\"" + Message + "\",\"mobileNo\": [" + mobilelist.Substring(0, mobilelist.Length - 1) + "]}";
                string status = WebRequestinJson(esanchaarurl, inputparams, "application/json", username, password);

                JObject root = (JObject)JObject.Parse(status);
                foreach (var item in root)
                {
                    if (item.Key == "responseCode")
                        status = item.Value.ToString().Replace("\"", "");
                }
                return status;
            }
            catch (Exception ex)
            {
                EsanchaarModel objEsanchaarModel = new EsanchaarModel(new DBLogger());
                objEsanchaarModel.InsertFailedSMSTrail(MobNo, Message);
                return ex.ToString();
            }
        }


        public string WebRequestinJson(string url, string postData, string contenttype, string username, string password)
        {
            try
            {
                string ret = string.Empty;
                StreamWriter requestWriter;

                var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.ServicePoint.Expect100Continue = false;
                    webRequest.Timeout = 40000;
                    webRequest.Headers.Add("username", username);
                    webRequest.Headers.Add("password", password);
                    webRequest.ContentType = contenttype;
                    //POST the data.
                    using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(postData);
                    }
                }

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream resStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                ret = reader.ReadToEnd();
                return ret;
            }
            catch (WebException wex)
            {
                throw wex;
            }
        }
    }
}