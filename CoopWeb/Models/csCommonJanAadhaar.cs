using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Linq;
using System.Data.OleDb;
using Newtonsoft.Json;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.ComponentModel;
using static java.net.Authenticator;
using System.Web.Routing;
using static EsignAesEncryption;
using iTextSharp.text.pdf.qrcode;

namespace Cooperative.Models
{
    public class csCommonJanAadhaar
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private string KeyValue;
        private DataTable dt;

        public csCommonJanAadhaar(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public csCommonJanAadhaar()
        {
            objDBAccess = new DBFactoryMSSQL();
            KeyValue = string.Empty;
        }
   
 
    }
}