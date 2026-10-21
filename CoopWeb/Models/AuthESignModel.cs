using System.Data;
using Cooperative.DBConnector;


namespace Cooperative.Models
{
    public class AuthESignModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public AuthESignModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }








        public class OTPResponse
        {
            public string Status { get; set; }
            public string UidaiErrorCode { get; set; }
            public string ResponseCode { get; set; }
            public string UidaiStatus { get; set; }
            public string UidaiErrorMsg { get; set; }
            public string ErrorMessage { get; set; }
            public string TransactionId { get; set; }
            public string SuccessMessage { get; set; }
            public string Document { get; set; }
        }
    }
}
