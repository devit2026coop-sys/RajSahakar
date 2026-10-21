namespace Cooperative.Controllers
{
    public class CommonMessageFactory
    {
        public static string Message(int MessageCode, string Language = "en-US")
        {
            switch (Language.ToLower())
            {
                case "en-us":
                    {
                        switch (MessageCode)
                        {
                            case 0: return "Data Already Exists.";
                            case 1: return "Data Saved Successfully.";
                            case 2: return "Data Updated Successfully.";
                            case 3: return "Status change Successfully.";
                            case 4: return "Data deleted Successfully.";
                            case 401: return "Provided input data is not in correct format.";
                            default: return "Error code response not found.";
                        }
                    }
                case "hi-in":
                    {
                        switch (MessageCode)
                        {
                            case 0: return "डेटा पहले से मौजूद है ।";
                            case 1: return "डेटा सफलतापूर्वक सहेजा गया ।";
                            case 2: return "डेटा सफलतापूर्वक अपडेट किया गया.";
                            case 3: return "स्थिति परिवर्तन सफलतापूर्वक.";
                            case 4: return "डेटा को सफलतापूर्वक हटाया गया.";
                            case 401: return "प्रदान इनपुट डेटा सही प्रारूप में नहीं है |";
                            default: return "त्रुटि कोड प्रतिक्रिया नहीं मिली ।";
                        }
                    }
                default:
                    return string.Empty;
            }
        }

        public static string Response(string _Message)
        {
            if (string.IsNullOrEmpty(_Message))
                return "{}";

            return "{\"Status\":true,\"data\":[\"" + _Message + "\"]}";
        }

        public static string ResponseByCulture(int dbResponseCode, int OperationCode, string Language = "en-us")
        {
            string Messsage = string.Empty;
            Language = (Language.ToLower() == "en-us" ? Language.ToLower() : "hi-in");

            if (dbResponseCode == -1 || dbResponseCode == 0)
                return (Language == "en-us" ? "Server side error occurred." : "सर्वर साइड त्रुटि हुई।");

            if (OperationCode == 1) // In case of save data
            {
                if (dbResponseCode > 0)
                    Messsage = (Language == "en-us" ? "Record saved successfully." : "रिकॉर्ड सफलतापूर्वक सहेजा गया।");
                else if (dbResponseCode == -2)
                    Messsage = (Language == "en-us" ? "Duplicate data found." : "डुप्लिकेट डेटा मिला।");
            }
            else if (OperationCode == 2) // In case of edit data
            {
                if (dbResponseCode > 0)
                    Messsage = (Language == "en-us" ? "Record updated successfully." : "रिकॉर्ड सफलतापूर्वक अपडेट किया गया।");
                else if (dbResponseCode == -2)
                    Messsage = (Language == "en-us" ? "Duplicate data found." : "डुप्लिकेट डेटा मिला।");
            }

            return Messsage;
        }

        public static string GetJsonStringResponse(string Status,string Message)
        {
            ResultMessage obj = new ResultMessage() { status = Status, data = Message };

            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);

            return json;
        }

        public static ResultMessage GetJsonResponse(string Status, string Message)
        {
            ResultMessage obj = new ResultMessage() { status = Status, data = Message };

            //var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);

            return obj;
        }
    }
    public class ResultMessage
    {
        public string status { get; set; }
        public string data { get; set; }
        public string responce { get; set; }
    }



}

