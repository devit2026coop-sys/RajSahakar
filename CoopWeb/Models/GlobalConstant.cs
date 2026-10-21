using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Cooperative.Models
{
    public static class GlobalConstant
    {
        //OLD KEY = "MCTqHSggGVHsyqgJu5fUpWZDk5Z8DVIRKgZOnfWUr9jY0dKbBdE4Z90"   01/04/2022
        //Old24Mar2023 public static readonly string AadharAuthLicenseKey = "MDAW2xSJCuC-htWkTHcw9LTYXs1rl8kyfygbLztYldZocGJR6DDtE5A";//"MKmyGwbThaYG35Ahinwx35nLtBYXrNMP4ejWD7A9-x6InP7y4xLROXU";

        public static readonly string AadharAuthClientId = "0c8fe990-3b61-406d-b185-0fafc80e73d7";

        //public static readonly string AadharAuthLicenseKey = "MJSazxO49Eh5vQ2BlcbUG--uNQ4tCpqKPF-OoFa0BZo0CE4CDBBtXVA";   //2022 old
        //public static readonly string AadharAuthSUBAUS = "PRCIS22869"; //"PNSCL22866"; //2022 old

        public static readonly string AadharAuthLicenseKey = "MLOfbnSLo_yauICOOpafflxx0ahYh5luDfkeB1IpxgF_B2lfpW87vbN";   //change by 2nov2023  NEW EXPIREY DATE: 2025-03-31
        public static readonly string AadharAuthSUBAUS = "PCD8231094"; //"PNSCL22866";   //change by 2nov2023  NEW EXPIREY DATE: 2025-03-31



        //public static readonly string AadharAuthLicenseKey = "MDAW2xSJCuC-htWkTHcw9LTYXs1rl8kyfygbLztYldZocGJR6DDtE5A";   //change by 2nov2023  NEW EXPIREY DATE: 2025-03-31
        //public static readonly string AadharAuthSUBAUS = "PRCIS22869"; //"PNSCL22866";   //change by 2nov2023  NEW EXPIREY DATE: 2025-03-31



        public static readonly string AadhaarDSMDETOKANIZEV2URL = "https://api.sewadwaar.rajasthan.gov.in/app/live/Aadhaar/Prod/detokenizeV2/doitAadhaar/encDec/demo/hsm/auth";
        public static readonly string SendOTPForAadharAuthURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/otp/request/prod";

        public static readonly string VerifyOTPForAadharAuthURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/kyc/otp/prod";

        public static readonly string AadharAuthEsignClientId = "02a79073-c2db-4c84-b810-885fdaa8c54a";
        public static readonly string SendOTPForAadharAuthEsignURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/RajeSign/Prod/all/webresources/generic/sendOTP/Aadhar";

        public static readonly string VerifyOTPForAadharAuthEsignURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/RajeSign/Prod/all/webresources/generic/authOTP/Aadhar";

        public static readonly string EsignPDFURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/rajesign/Prod/Service/all/webresources/generic/esign/Doc";

        //15oct2024 madhuri
        public static readonly string SendOTPForAadharAuthEsignURL_Test = "https://aadhaarauthtest.rajasthan.gov.in/AadhaarAuth";

        //    public static readonly string AadharAuthEsigEncKey_Test = "O0HP3OOR1DP2KZY752TJDO9DRHURSOX1";
        public static readonly string AadharAuthEsigEncKey_Test = "4MKHPUYXLXGQK0IBDBI33ICD7YQMC53Y";


        public static readonly string AadharAuthEsigAppCode_Test = "PRAVL22868ABFAP012";

        public static readonly string AadharAuthEsigEncKey_Test_stagging = "AWTIO8BOW66FN72ENACW4CNNYTW4QEX5";
        public static readonly string AadharAuthEsigAppCode_Test_stagging = "PRAVL22868ABFAP021";




        public static readonly string EsignPDFURL_Test = "https://apitest.sewadwaar.rajasthan.gov.in/app/live/rajesign/Staging/Service/all/webresources/generic/esign/Doc";//2.1

        //2024-07-24 #Pradeep
        #region RAJNIVESH
        //Staging
        //public const string RAJNIVESH_CODE = "43";
        //public const string RAJNIVESH_APPNAME = "COOP";
        //public const string RAJNIVESH_PASSWORD = "APSA48F4";
        //public const string RAJNIVESH_XAPIKey = "D5A29E0D-E72D-45E3-937B-E7584E2BAEFC";
        //public const string RAJNIVESH_URL = "https://rajniveshtest.rajasthan.gov.in/sso/";
        //public const string RAJNIVESH_API_URL = "https://rajniveshtest.rajasthan.gov.in/API/API/V1/";

        //Production
        public const string RAJNIVESH_CODE = "43";
        public const string RAJNIVESH_APPNAME = "COOP";
        public const string RAJNIVESH_PASSWORD = "BAA731EW";
        public const string RAJNIVESH_XAPIKey = "9160813D-C1D9-4257-99BC-25CD53159870";
        public const string RAJNIVESH_URL = "https://rajnivesh.rajasthan.gov.in/sso/";
        public const string RAJNIVESH_API_URL = "https://rajnivesh.rajasthan.gov.in/API/API/V1/";

        #endregion

        #region PM KISAN

        public static readonly string PMK_Token = "FHGBHFYBT268Gpf37hmJ6RY";
        public static readonly string AesGcm256EncryptionKey1 = "F1BC8654828649C2FE105D080B9A63A286B0C70A3909D2DAC8D36147A70735D6";
        public static readonly string AesGcm256EncryptionKey2 = "EE14FACFD8EBFE23800885DE3E257AC9";

        //2025-06-27 #Pradeep
        public static readonly string JanaadhaarCMKisanAesKey = "sPI7klpi3hQS5iY7";
        public static readonly string JanaadhaarCMKisanApiId = "232";
        public static readonly string JanaadhaarCMKisanUrl = "https://janapp.rajasthan.gov.in/Service/action/encrptyApiInputGenric";
        #endregion


        #region PDCS RCDF

        public static readonly string PDCS_API_KEY = "A1F9B3D47E68CC119FE6A2D4CBA9371D7F54A3C5D9B7E8F04D2A3B8E1F923C5A";
        public static readonly string PDCS_PROFILE_URL = "https://pdcsrcdf.rajasthan.gov.in/PDCSAPI/PdcsProfile";
        public static readonly string UPDATE_PDCS_PROFILE_URL = "https://pdcsrcdf.rajasthan.gov.in/PDCSAPI/UpdatePdcsProfile";

        #endregion

        #region MMPBY Farmer details
        public static readonly string MMPBY_UAT_API_URL = "https://apitest.sewadwaar.rajasthan.gov.in/app/live/MMPBY/api/getRegDetails";
        public static readonly string MMPBY_PROD_API_URL = "https://api.sewadwaar.rajasthan.gov.in/app/live/MMPBY/api/getRegDetails";
        #endregion

        #region ESIGN CONFIG (UAT/PROD)
        public static readonly string Uat_TokenApiUrl = "https://rajesignapitest.rajasthan.gov.in/caEsign/auth/generateToken";
        public static readonly string Uat_SignedXmlApiUrl = "https://rajesignapitest.rajasthan.gov.in/caEsign/v2/generateSignedXmlV2_1";
        public static readonly string Uat_SignPdfApiUrl = "https://rajesignapitest.rajasthan.gov.in/caEsign/v2/signingPdfV2_1";
        public static readonly string Uat_SignedXmlStatusApiUrl = "https://rajesignapitest.rajasthan.gov.in/caEsign/v2/getSignedXmlV2_1";

        public static readonly string Uat_SSOID = "RITIKAPANDYA.DOIT";
        public static readonly string Uat_SecretKey = "wv8bFlukbdwHrnclxt61Co1Yu/mG+C8Wtm4n5+Qgyo4=";
        public static readonly string Uat_ApplicationCode = "RAJSAHKARUAT";
        public static readonly string Uat_AspId = "RISL";

        // Production

        public static readonly string Prod_TokenApiUrl = "https://rajesignapi.rajasthan.gov.in/caEsign/auth/generateToken";
        public static readonly string Prod_SignedXmlApiUrl = "https://rajesignapi.rajasthan.gov.in/caEsign/v2/generateSignedXmlV2_1";
        public static readonly string Prod_SignPdfApiUrl = "https://rajesignapi.rajasthan.gov.in/caEsign/v2/signingPdfV2_1";
        public static readonly string Prod_SignedXmlStatusApiUrl = "https://rajesignapi.rajasthan.gov.in/caEsign/v2/getSignedXmlV2_1";

        public static readonly string Prod_SSOID = "RJTO201136028433";
        public static readonly string Prod_SecretKey = "MV5YGTz07zfyW3e9+uDK3kvgKAbpLPz+w9hBV73mBdg=";
        public static readonly string Prod_ApplicationCode = "RAJSAHAKARPROD";
        public static readonly string Prod_AspId = "RISL";

        #endregion






    }
}