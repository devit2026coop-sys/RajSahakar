using System.Web;

namespace Cooperative.JanadhaarClasses
{
    public static class JanAadhaarConfig
    {
        public static readonly string BaseUrl =
            ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208"))
            ? "https://apitest.sewadwaar.rajasthan.gov.in/app/live/apiservice/janAadhaar/v1/"
            : "https://api.sewadwaar.rajasthan.gov.in/app/live/apiservice/janAadhaar/v1/";
        
        public static readonly string JanaadharProdValidateOtpUrl = "https://api.sewadwaar.rajasthan.gov.in/app/live/janAadhaar/v1/";

        //public static readonly string ClientId = "254eced2ee1bfc019d3a09dc4ef8e8ac";
        //public static readonly string ClientId = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
        public static readonly string ClientId =
            ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208"))
            ? "16a271d908683839665332a7f58331c4"
            : "c24fa5a5db5a42fc1370dfc1a57c8eaf";


        public static readonly string PrivateCertPath = HttpContext.Current.Server.MapPath("~/Content/JanaadhaarKey/COOP_RAJ.pfx");
        public static readonly string PrivateCertKey = HttpContext.Current.Server.MapPath("~/Content/JanaadhaarKey/privateKey.key");
        public static readonly string PrivateCertPassword = "Cooperative@2026";
        public static readonly string PublicCertPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208"))
            ? HttpContext.Current.Server.MapPath("~/Content/JanaadhaarKey/RJAAPublicKey.cer")
            : HttpContext.Current.Server.MapPath("~/Content/JanaadhaarKey/THE_JAN_AADHAAR_AUTHORITY.cer");


        //public static readonly string appCode = "appCode";
        //public static readonly string schemShortCode = "schemShortCode";


    }
}
