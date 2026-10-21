using Cooperative.Filters;
using Cooperative.MasterDataSoap;
using Cooperative.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    public class RajSewadwaarController : BaseController
    {      

        [HttpGet]
        [ActionName("GeographicalDetails")]
        //[OutputCache(Duration = 10, VaryByParam = "none")]
        public JsonResult GetGeographicalDetails(string GeographyType="", string GeographyCode="")
        {
            string GeographicalDetails = "{}";
            int Code = 0;

            if (string.IsNullOrEmpty(GeographyType) || !int.TryParse(GeographyCode, out Code))
                return Json(GeographicalDetails, JsonRequestBehavior.AllowGet);

            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());

            switch (GeographyType.ToUpper())
            {
                case "DIVISION": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DivisionList()); break; }
                case "DISTRICT": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DistrictList(Code)); break; }
                case "TEHSIL": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.TehsilList(Code)); break; }
                case "PANCHAYATSAMITY": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PanchayatSamitiBlockList(Code)); break; }
                case "GRAMPANCHAYAT": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GramPanchayatList(Code)); break; }
                case "VILLAGE": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.VillageList(Code)); break; }
                case "CITY": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CityList(Code)); break; }
                case "WARD": { GeographicalDetails = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.WardList(Code)); break; }
                default: break;
            }

            return Json(GeographicalDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("CountryList")]
        //[OutputCache(Duration = 10, VaryByParam = "none")]
        public JsonResult GetCountryList()
        {
            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(3);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetMstDataList")]
        //[OutputCache(Duration = 10, VaryByParam = "none")]
        public JsonResult GetMstDataList(string req)
        {
            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(Convert.ToInt32(req));
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("StateList")]
        [OutputCache(Duration = 10, VaryByParam = "none")]
        public JsonResult GetStateList(string CountryId = "17")
        {
            int _TempCountryId = 0;
            if (!int.TryParse(CountryId, out _TempCountryId))
                return Json("{}", JsonRequestBehavior.AllowGet);

            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(_TempCountryId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionName("DivisionList")]
        // [OutputCache(Duration = 10, VaryByParam = "none")]
        public JsonResult GetDivisionList(string StateId = "57")
        {
            int _TempStateId = 0;
            if (!int.TryParse(StateId, out _TempStateId))
                return Json("{}", JsonRequestBehavior.AllowGet);

            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(_TempStateId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("DistrictList")]
        public JsonResult GetDistrictList(string DivisionId = "")
        {
            int _TempDivisionId = 0;
            if (!int.TryParse(DivisionId, out _TempDivisionId))
                return Json("{}", JsonRequestBehavior.AllowGet);

            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(_TempDivisionId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionName("TehsilList")]
        public JsonResult GetTehsilList(string DistrictId = "")
        {
            int _TempDistrictId = 0;
            if (!int.TryParse(DistrictId, out _TempDistrictId))
                return Json("{}", JsonRequestBehavior.AllowGet);

            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(_TempDistrictId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("PanchayatSamityList")]
        public JsonResult GetPanchayatSamitiList(string TehsilIdId = "")
        {
            int _TempTehsilIdId = 0;
            if (!int.TryParse(TehsilIdId, out _TempTehsilIdId))
                return Json("{}", JsonRequestBehavior.AllowGet);

            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(_TempTehsilIdId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionName("VillageList")]
        public JsonResult GetVillageList()
        {
            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(59);

            List<VilageData> _list = (List<VilageData>)Newtonsoft.Json.JsonConvert.DeserializeObject(Result, typeof(List<VilageData>));

            _list = _list.FindAll(X => X.GP_ID == "21");

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(_list), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("FinancialYearList")]
        public JsonResult GetFinancialYearList()
        {
            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());
            var Result = objRajSewadwaarMaster.GetDataByService(7);

            List<VilageData> _list = (List<VilageData>)Newtonsoft.Json.JsonConvert.DeserializeObject(Result, typeof(List<VilageData>));

            _list = _list.FindAll(X => X.GP_ID == "21");

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(_list), JsonRequestBehavior.AllowGet);
        }
    }

    class VilageData
    {
        public string VILLAGE_ID { get; set; }
        public string VILLAGE_CODE { get; set; }
        public string VILLAGE_ENG { get; set; }
        public string VILLAGE_MANGAL { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFICATION_DATE { get; set; }
        public string VERSION { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string GP_ID { get; set; }
        public string PATWAR_ID { get; set; }
        public string AC_ID { get; set; }
        public string REMARKS { get; set; }
        public string PS_ID { get; set; }
    }

    public class RajSewadwaarMaster
    {
        private ILogger objILogger;
        //private DBFactoryOracle objDBAccess;
        private string _Response = "{}";

        public RajSewadwaarMaster(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            // objDBAccess = new DBFactoryOracle();
        }

        /// <summary>
        /// Service Code =>
        ///1	    BDH_BANK Bank Master
        ///2	    BDH_BANK_BRANCH Bank Branch Master
        ///3	    BDH_CITY City Master
        ///4	    BDH_WARD Ward Master
        ///5	    EMITRA_ACCOUNT_TYPE Account Type Master
        ///6	    EMITRA_COUNTRY Country Master
        ///7	    EMITRA_FIN_YEAR Financial Year Master
        ///8	    EMITRA_GENDER Gender Master
        ///9	    EMITRA_HOLIDAY Holiday Master
        ///10	EMITRA_JURISDICTION Jurisdiction Master
        ///11	EMITRA_JURISDICTION_LEVEL Jurisdiction Level Master
        ///12	EMITRA_JURISDICTION_TYPE Jurisdiction Type Master
        ///13	EMITRA_MOTHER_TONGUE Mother Tongue Master
        ///14	EMITRA_MUNICIPALITY_TYPE Municipality Master
        ///15	EMITRA_OCCUPATION Municipality Type Master
        ///16	EMITRA_OFFICE_TYPE Office Type Master
        ///17	EMITRA_STATE State Master
        ///18	IHRMS_ADMIN_DEPT Admin Department Master
        ///19	IHRMS_AWARDLEVEL Award Level Master
        ///20	IHRMS_CADRE Cadre Master
        ///21	IHRMS_CASTE Caste Master
        ///22	IHRMS_CATEGORY Category Master
        ///23	IHRMS_COURT Court  Master
        ///24	IHRMS_CRITICAL_DISABILITY Critical Disability Master
        ///25	IHRMS_CRITICAL_ILLNESS Critical Illness Master
        ///26	IHRMS_DDO_INFO DDO Info Master
        ///27	IHRMS_DEGREE_TYPE Degree Type Master
        ///28	IHRMS_DEPARTMENT Department Master
        ///29	IHRMS_DESIGNATION Designation Master
        ///30	IHRMS_LANGUAGE Language Master
        ///31	IHRMS_LEAVE Leave Master
        ///32	IHRMS_MARITAL_STATUS Marital Status Master
        ///33	IHRMS_OFFICE Office Master
        ///34	IHRMS_OFFICE_HIERARCHY Office Hierarchy Master
        ///35	IHRMS_OFFICE_LEVEL Office Level Master
        ///36	IHRMS_PAYCOMMISION Pay Commission Master
        ///37	IHRMS_PAYSCALE Pay Scale Master
        ///38	IHRMS_POST Post Master
        ///39	IHRMS_PROFESSION Profession Master
        ///40	IHRMS_QUALIFICATION_DEGREE Qualification Degree Master
        ///41	IHRMS_QUALIFICATION_LEVEL Qualification Level Master
        ///42	IHRMS_QUALIFICATION_SUBJECT Qualification Subject Master
        ///43	IHRMS_QUALIFICATION_UNIVERSITY Qualification University Master
        ///44	IHRMS_QUOTA Quota Master
        ///45	IHRMS_RELATION Relation Master
        ///46	IHRMS_RELIGION_NAME Religion Master
        ///47	IHRMS_SERVICE Service Master
        ///48	IHRMS_SERVICE_SELECTION Service Selection Master
        ///49	IHRMS_SERVICE_TYPE Service Type Master
        ///50	IHRMS_STATUS_TYPE Status Type Master
        ///51	IHRMS_TITLE Title Master
        ///52	IHRMS_TRANSFER_TYPE Transfer Type Master
        ///53	IHRMS_TREASURY Treasury Master
        ///54	IHRMS_TYPE_OF_SERVICE Type of Service Master
        ///55	PRAJ_BLOCK Block Master
        ///56	PRAJ_DISTRICT District Master
        ///57	PRAJ_DIVISION Division Master
        ///58	PRAJ_GP GP Master
        ///59	PRAJ_VILLAGE Village Master
        ///60	RVNU_ILR ILR Master
        ///61	RVNU_PATWAR Patwar Master
        ///62	RVNU_TEHSIL Tehsil Master
        ///63	CITY_TYPE City Type Master
        ///64	ELECTION_AC Assembly Constituency
        ///65	ELECTION_DISTRICT_AC_PC District Assembly Constituency / Parliament Constituency Mapping
        ///66	ELECTION_PC Parliament Constituency Master
        ///67	POPULATION_BLOCK Block Population Master
        ///68	POPULATION_CITY City Population Master
        ///69	POPULATION_VILLAGE Village Population Master
        ///70	SD_TEHSIL Sub Division Tehsil Mapping
        ///71	SUB_DIVISION Sub Division Master
        ///72	SUB_TEHSIL Sub Tehsil Master
        /// </summary>
        /// <param name="ServiceCode">Service reference code provided by Raj Sewa Dwaar Application</param>
        /// <returns></returns>
        public string GetDataByService(int ServiceCode)
        {
            try
            {
                MasterDataSoapClient client = new MasterDataSoapClient();
                AuthHeader authentication = new AuthHeader();
                authentication.Username = "Doit";
                authentication.Password = "Doit@123";

                _Response = client.GetMasterData(authentication, "EV7TGZ8MZZ", ServiceCode, true, true); //JsonConvert.DeserializeObject(
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.RajSewadwaarMaster", FunctionName = "public string GetDataByService(int ServiceCode)" }, ex);

                //_Response = Fetch data from db here;
            }
            finally // If Service request fails at Raj Sewa dwaar then fetch data from our database
            {
                //objDBAccess.CloseConnection();
            }
            return _Response;
        }
    }

    public class MasterEntryIntoSql
    {
        public string Insert(string MasterTableNameId)
        {
            if (string.IsNullOrEmpty(MasterTableNameId))
                return "Master table name not defined.";

            string[] TableIdCollection = MasterTableNameId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());
            RajSewadwaarMaster objRajSewadwaarMaster = new RajSewadwaarMaster(new DBLogger());

            foreach (string TableId in TableIdCollection)
            {
                String _TableName = GetTableName(TableId);
                if (!string.IsNullOrEmpty(_TableName))
                {
                    DataTable ServiceId = objRajSewaDwaarModel.IsTableExists(_TableName);

                    if (ServiceId != null && ServiceId.Rows.Count > 0 && Convert.ToString(ServiceId.Rows[0][0]) == "-1") // Table does not exists in db
                    {
                        string data = objRajSewadwaarMaster.GetDataByService(GetTableIdAsPerEmitraService(TableId));

                        if (data == "{}")
                            continue;

                        string strCreateTable = SetTableStructure(TableId); // create table structure
                        if (!string.IsNullOrEmpty(strCreateTable))
                        {
                            int _Result = objRajSewaDwaarModel.CreateTable(strCreateTable);

                            DataTable dt = GetDataTableFromJSONstring(data, TableId);  // Insert Data Now

                            if (dt != null && dt.Rows.Count > 0)
                                objRajSewaDwaarModel.BulkInsertToDb(dt, TableId);


                        }
                    }
                    else
                    {

                    }
                }
            }

            return string.Empty;
        }

        private string GetTableName(string Id)
        {
            switch (Id)
            {
                case "BDH_CITY": return "City";
                case "BDH_WARD": return "Ward";
                case "EMITRA_COUNTRY": return "Country";
                case "EMITRA_GENDER": return "Gender";
                case "EMITRA_HOLIDAY": return "Holiday";
                case "EMITRA_MUNICIPALITY_TYPE": return "MunicipalityType";
                case "EMITRA_STATE": return "State";
                case "IHRMS_CASTE": return "Cast";
                case "IHRMS_CATEGORY": return "Category";
                case "IHRMS_DESIGNATION": return "Designation";
                case "IHRMS_LANGUAGE": return "Language";
                case "IHRMS_OFFICE": return "Office";
                case "IHRMS_POST": return "Post";
                case "IHRMS_RELIGION_NAME": return "Religion";
                case "IHRMS_TITLE": return "Title";
                case "PRAJ_BLOCK": return "Block";
                case "PRAJ_DISTRICT": return "District";
                case "PRAJ_DIVISION": return "Division";
                case "PRAJ_GP": return "GramPanchayat";
                case "PRAJ_VILLAGE": return "Village";
                case "RVNU_TEHSIL": return "Tehsil";

                default: return string.Empty;
            }
        }

        private Int32 GetTableIdAsPerEmitraService(string Id)
        {
            switch (Id)
            {
                case "BDH_CITY": return 3;
                case "BDH_WARD": return 4;
                case "EMITRA_COUNTRY": return 6;
                case "EMITRA_GENDER": return 8;
                case "EMITRA_HOLIDAY": return 9;
                case "EMITRA_MUNICIPALITY_TYPE": return 14;
                case "EMITRA_STATE": return 17;
                case "IHRMS_CASTE": return 21;
                case "IHRMS_CATEGORY": return 22;
                case "IHRMS_DESIGNATION": return 29;
                case "IHRMS_LANGUAGE": return 30;
                case "IHRMS_OFFICE": return 33;
                case "IHRMS_POST": return 38;
                case "IHRMS_RELIGION_NAME": return 46;
                case "IHRMS_TITLE": return 51;
                case "PRAJ_BLOCK": return 55;
                case "PRAJ_DISTRICT": return 56;
                case "PRAJ_DIVISION": return 57;
                case "PRAJ_GP": return 58;
                case "PRAJ_VILLAGE": return 59;
                case "RVNU_TEHSIL": return 62;
                default: return default(Int32);
            }
        }

        private string SetTableStructure(string Id)
        {
            string TableScheme = string.Empty;
            switch (Id)
            {
                case "BDH_CITY": { TableScheme = "CREATE TABLE City (CITY_ID INT,DISTRICT_ID int,CITY_CODE NVARCHAR(20),CITY_ENG VARCHAR(100),CITY_MANGAL NVARCHAR(100),CITY_TYPE INT,CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, REMARKS NVARCHAR(500))"; break; }
                case "BDH_WARD": { TableScheme = "CREATE TABLE Ward (WARD_ID INT,WARD_CODE NVARCHAR(20),WARD_ENG VARCHAR(100),WARD_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,CITY_ID INT,REMARKS NVARCHAR(500))"; break; }
                case "EMITRA_COUNTRY": { TableScheme = ""; break; }
                case "EMITRA_GENDER": { TableScheme = ""; break; }
                case "EMITRA_HOLIDAY": { TableScheme = ""; break; }
                case "EMITRA_MUNICIPALITY_TYPE": { TableScheme = ""; break; }
                case "EMITRA_STATE": { TableScheme = "CREATE TABLE State (STATE_ID INT,STATE_CODE NVARCHAR(20),STATE_ENG VARCHAR(100),STATE_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,CAPITAL NVARCHAR(100),REMARKS NVARCHAR(500))"; break; }
                case "IHRMS_CASTE": { TableScheme = ""; break; }
                case "IHRMS_CATEGORY": { TableScheme = ""; break; }
                case "IHRMS_DESIGNATION": { TableScheme = ""; break; }
                case "IHRMS_LANGUAGE": { TableScheme = ""; break; }
                case "IHRMS_OFFICE": { TableScheme = ""; break; }
                case "IHRMS_POST": { TableScheme = ""; break; }
                case "IHRMS_RELIGION_NAME": { TableScheme = ""; break; }
                case "IHRMS_TITLE": { TableScheme = ""; break; }
                case "PRAJ_BLOCK": { TableScheme = "CREATE TABLE Block (BLOCK_ID INT,BLOCK_CODE NVARCHAR(20),BLOCK_ENG VARCHAR(100),BLOCK_MANGAL NVARCHAR(100),BLOCK_STATUS BIT,CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,DISTRICT_ID  INT,REMARKS NVARCHAR(500))"; break; }
                case "PRAJ_DISTRICT": { TableScheme = "CREATE TABLE District (DISTRICT_ID INT,DISTRICT_CODE NVARCHAR(20),DISTRICT_ENG VARCHAR(100),DISTRICT_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,DIVISION_ID INT,REMARKS NVARCHAR(500),IS_TSP BIT)"; break; }
                case "PRAJ_DIVISION": { TableScheme = "CREATE TABLE Division (DIVISION_ID INT,DIVISION_CODE NVARCHAR(20),DIVISION_ENG VARCHAR(100),DIVISION_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,REMARKS NVARCHAR(500))"; break; }
                case "PRAJ_GP": { TableScheme = "CREATE TABLE GramPanchayat (GP_ID INT,GP_CODE NVARCHAR(20),GP_ENG VARCHAR(100),GP_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,BLOCK_ID INT,REMARKS NVARCHAR(500))"; break; }
                case "PRAJ_VILLAGE": { TableScheme = "CREATE TABLE Village (VILLAGE_ID INT,VILLAGE_CODE NVARCHAR(20),VILLAGE_ENG VARCHAR(100),VILLAGE_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,GP_ID INT,PATWAR_ID INT,AC_ID INT,REMARKS NVARCHAR(500),PS_ID INT)"; break; }
                case "RVNU_TEHSIL": { TableScheme = "CREATE TABLE Tehsil (TEHSIL_ID INT,TEHSIL_CODE NVARCHAR(20),TEHSIL_ENG VARCHAR(100),TEHSIL_MANGAL NVARCHAR(100),CREATION_DATE DATETIME, MODIFICATION_DATE DATETIME,[VERSION] varchar(20),IS_ACTIVE BIT, IS_DELETED BIT,DISTRICT_ID INT,REMARKS NVARCHAR(500))"; break; }
                default: { TableScheme = ""; break; }
            }

            return TableScheme;
        }

        private DataTable GetDataTableFromJSONstring(string jsonServiceData, string TableToCast)
        {
            DataTable dt = new DataTable();
            JArray json = JArray.Parse(jsonServiceData);
            switch (TableToCast)
            {
                case "BDH_CITY":
                    {
                        dt.Columns.Add("CITY_ID", typeof(Int32));
                        dt.Columns.Add("DISTRICT_ID", typeof(Int32));
                        dt.Columns.Add("CITY_CODE", typeof(string));
                        dt.Columns.Add("CITY_ENG", typeof(string));
                        dt.Columns.Add("CITY_MANGAL", typeof(string));
                        dt.Columns.Add("CITY_TYPE", typeof(Int32));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("CITY_ID")),
                                Convert.ToInt32(string.IsNullOrEmpty(P.SelectToken("DISTRICT_ID").ToString()) ? 0 : P.SelectToken("DISTRICT_ID")),
                                Convert.ToString(P.SelectToken("CITY_CODE")),
                                Convert.ToString(P.SelectToken("CITY_ENG")),
                                Convert.ToString(P.SelectToken("CITY_MANGAL")),
                                Convert.ToInt32(P.SelectToken("CITY_TYPE")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });

                        }

                        break;
                    }
                case "BDH_WARD":
                    {
                        dt.Columns.Add("WARD_ID", typeof(Int32));
                        dt.Columns.Add("WARD_CODE", typeof(string));
                        dt.Columns.Add("WARD_ENG", typeof(string));
                        dt.Columns.Add("WARD_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("CITY_ID", typeof(Int32));
                        dt.Columns.Add("REMARKS", typeof(string));

                        int Counter = 0;
                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("WARD_ID")),
                                Convert.ToString(P.SelectToken("WARD_CODE")),
                                Convert.ToString(P.SelectToken("WARD_ENG")),
                                Convert.ToString(P.SelectToken("WARD_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToInt32(P.SelectToken("CITY_ID")),
                               Convert.ToString(P.SelectToken("REMARKS"))
                            });
                            }
                            catch { Counter++; }


                        }

                        break;
                    }
                case "EMITRA_COUNTRY":
                    {
                        break;
                    }
                case "EMITRA_GENDER":
                    {
                        break;
                    }
                case "EMITRA_HOLIDAY":
                    {
                        break;
                    }
                case "EMITRA_MUNICIPALITY_TYPE":
                    {
                        break;
                    }
                case "EMITRA_STATE":
                    {
                        dt.Columns.Add("STATE_ID", typeof(Int32));
                        dt.Columns.Add("STATE_CODE", typeof(string));
                        dt.Columns.Add("STATE_ENG", typeof(string));
                        dt.Columns.Add("STATE_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("CAPITAL", typeof(string));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("STATE_ID")),
                                Convert.ToString(P.SelectToken("STATE_CODE")),
                                Convert.ToString(P.SelectToken("STATE_ENG")),
                                Convert.ToString(P.SelectToken("STATE_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                 Convert.ToString(P.SelectToken("CAPITAL")),
                               Convert.ToString(P.SelectToken("REMARKS"))
                            });

                        }

                        break;
                    }
                case "IHRMS_CASTE":
                    {
                        break;
                    }
                case "IHRMS_CATEGORY":
                    {
                        break;
                    }
                case "IHRMS_DESIGNATION":
                    {
                        break;
                    }
                case "IHRMS_LANGUAGE":
                    {
                        break;
                    }
                case "IHRMS_OFFICE":
                    {
                        break;
                    }
                case "IHRMS_POST":
                    {
                        break;
                    }
                case "IHRMS_RELIGION_NAME":
                    {
                        break;
                    }
                case "IHRMS_TITLE":
                    {
                        break;
                    }
                case "PRAJ_BLOCK":
                    {
                        dt.Columns.Add("BLOCK_ID", typeof(Int32));
                        dt.Columns.Add("BLOCK_CODE", typeof(string));
                        dt.Columns.Add("BLOCK_ENG", typeof(string));
                        dt.Columns.Add("BLOCK_MANGAL", typeof(string));
                        dt.Columns.Add("BLOCK_STATUS", typeof(bool));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("DISTRICT_ID", typeof(int));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("BLOCK_ID")),
                                Convert.ToString(P.SelectToken("BLOCK_CODE")),
                                Convert.ToString(P.SelectToken("BLOCK_ENG")),
                                Convert.ToString(P.SelectToken("BLOCK_MANGAL")),
                                Convert.ToBoolean(P.SelectToken("BLOCK_STATUS").ToString()=="1" ? true : false),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToInt32(P.SelectToken("DISTRICT_ID")),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });
                            }
                            catch { }


                        }

                        break;
                    }
                case "PRAJ_DISTRICT":
                    {
                        dt.Columns.Add("DISTRICT_ID", typeof(Int32));
                        dt.Columns.Add("DISTRICT_CODE", typeof(string));
                        dt.Columns.Add("DISTRICT_ENG", typeof(string));
                        dt.Columns.Add("DISTRICT_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("DIVISION_ID", typeof(int));
                        dt.Columns.Add("REMARKS", typeof(string));
                        dt.Columns.Add("IS_TSP", typeof(bool));

                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("DISTRICT_ID")),
                                Convert.ToString(P.SelectToken("DISTRICT_CODE")),
                                Convert.ToString(P.SelectToken("DISTRICT_ENG")),
                                Convert.ToString(P.SelectToken("DISTRICT_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString().Length ==0 ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToInt32(P.SelectToken("DIVISION_ID")),
                                 Convert.ToString(P.SelectToken("REMARKS")),
                                 Convert.ToBoolean(P.SelectToken("IS_TSP").ToString() =="N" ? false : true)

                            });
                            }
                            catch { }


                        }

                        break;
                    }
                case "PRAJ_DIVISION":
                    {
                        try
                        {
                            dt.Columns.Add("DIVISION_ID", typeof(Int32));
                            dt.Columns.Add("DIVISION_CODE", typeof(string));
                            dt.Columns.Add("DIVISION_ENG", typeof(string));
                            dt.Columns.Add("DIVISION_MANGAL", typeof(string));
                            dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                            dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                            dt.Columns.Add("VERSION", typeof(string));
                            dt.Columns.Add("IS_ACTIVE", typeof(bool));
                            dt.Columns.Add("IS_DELETED", typeof(bool));
                            dt.Columns.Add("REMARKS", typeof(string));

                            foreach (var P in json)
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("DIVISION_ID")),
                                Convert.ToString(P.SelectToken("DIVISION_CODE")),
                                Convert.ToString(P.SelectToken("DIVISION_ENG")),
                                Convert.ToString(P.SelectToken("DIVISION_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });

                            }
                        }
                        catch { }


                        break;
                    }
                case "PRAJ_GP":
                    {
                        dt.Columns.Add("GP_ID", typeof(Int32));
                        dt.Columns.Add("GP_CODE", typeof(string));
                        dt.Columns.Add("GP_ENG", typeof(string));
                        dt.Columns.Add("GP_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("BLOCK_ID", typeof(Int32));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("GP_ID")),
                                Convert.ToString(P.SelectToken("GP_CODE")),
                                Convert.ToString(P.SelectToken("GP_ENG")),
                                Convert.ToString(P.SelectToken("GP_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToString(P.SelectToken("BLOCK_ID")),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });
                            }
                            catch { }


                        }

                        break;
                    }
                case "PRAJ_VILLAGE":
                    {
                        dt.Columns.Add("VILLAGE_ID", typeof(Int32));
                        dt.Columns.Add("VILLAGE_CODE", typeof(string));
                        dt.Columns.Add("VILLAGE_ENG", typeof(string));
                        dt.Columns.Add("VILLAGE_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("GP_ID", typeof(Int32));
                        dt.Columns.Add("PATWAR_ID", typeof(Int32));
                        dt.Columns.Add("AC_ID", typeof(Int32));
                        dt.Columns.Add("PS_ID", typeof(Int32));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("VILLAGE_ID")),
                                Convert.ToString(P.SelectToken("VILLAGE_CODE")),
                                Convert.ToString(P.SelectToken("VILLAGE_ENG")),
                                Convert.ToString(P.SelectToken("VILLAGE_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToInt32(P.SelectToken("GP_ID")),
                                Convert.ToInt32(P.SelectToken("PATWAR_ID")),
                                Convert.ToInt32(P.SelectToken("AC_ID")),
                                Convert.ToInt32(P.SelectToken("PS_ID")),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });
                            }
                            catch { }


                        }

                        break;
                    }
                case "RVNU_TEHSIL":
                    {

                        dt.Columns.Add("TEHSIL_ID", typeof(Int32));
                        dt.Columns.Add("TEHSIL_CODE", typeof(string));
                        dt.Columns.Add("TEHSIL_ENG", typeof(string));
                        dt.Columns.Add("TEHSIL_MANGAL", typeof(string));
                        dt.Columns.Add("CREATION_DATE", typeof(DateTime));
                        dt.Columns.Add("MODIFICATION_DATE", typeof(DateTime));
                        dt.Columns.Add("VERSION", typeof(string));
                        dt.Columns.Add("IS_ACTIVE", typeof(bool));
                        dt.Columns.Add("IS_DELETED", typeof(bool));
                        dt.Columns.Add("DISTRICT_ID", typeof(Int32));
                        dt.Columns.Add("REMARKS", typeof(string));

                        foreach (var P in json)
                        {
                            try
                            {
                                dt.Rows.Add(new object[] { Convert.ToInt32(P.SelectToken("TEHSIL_ID")),
                                Convert.ToString(P.SelectToken("TEHSIL_CODE")),
                                Convert.ToString(P.SelectToken("TEHSIL_ENG")),
                                Convert.ToString(P.SelectToken("TEHSIL_MANGAL")),
                                Convert.ToDateTime(P.SelectToken("CREATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("CREATION_DATE")),
                                Convert.ToDateTime(P.SelectToken("MODIFICATION_DATE").ToString() == "" ? DateTime.Now : P.SelectToken("MODIFICATION_DATE")),
                                Convert.ToString(P.SelectToken("VERSION")),
                                Convert.ToBoolean(P.SelectToken("IS_ACTIVE").ToString() =="1" ? true : false),
                                Convert.ToBoolean(P.SelectToken("IS_DELETED").ToString() =="1" ? true : false),
                                Convert.ToInt32(P.SelectToken("DISTRICT_ID")),
                                Convert.ToString(P.SelectToken("REMARKS"))
                            });
                            }
                            catch { }


                        }

                        break;
                    }
                default: { break; }
            }

            return dt;
        }
    }

}