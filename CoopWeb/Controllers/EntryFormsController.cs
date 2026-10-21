using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [LogActionFilter]
    public class EntryFormsController : Controller
    {
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        public ActionResult BhandarEntryForm()
        {
            return View();
        }

        public ActionResult BhandarDashboard()
        {
            return View();
        }

        public ActionResult ExlUploadForSTLoan()
        {
            return View();
        }
        public ActionResult PurchaseSalesSociety()
        {
            return View();
        }
        public ActionResult FertiMngEntry()
        {
            return View();
        }
        [HttpPost]
        [ActionName("SaveBhandarForms")]
        public JsonResult SaveBhandarFormsData(BhandarFormDataEntity _Model)
        {
            int result = -1;
            if (_Model != null)
            {
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));
                _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                result = objEntryForms.InsertBhandarData(_Model);
            }
            return Json(result);
        }

        [HttpPost]
        public string GetYearTargetData(BhandarFormDataEntity _Model)
        {
            DataTable DT = null;
            //if (_Model.BhandarId == _loggedInUSerDetails.DistrictId)
            //{
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.BhandarMprDataList(_Model);
            //}
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        public string BhandarMprDataForDashboard()

        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();
            string WhereClause = " Where 1=1";

            if (DateTime.Now.Month < 4)
            {
                WhereClause += " FinancialYear.FIN_YEAR_END = " + DateTime.Now.Year;
            }
            if (DateTime.Now.Month > 3)
            {
                WhereClause += " FinancialYear.FIN_YEAR_START = " + DateTime.Now.Year;
            }
            DataTable DT = null;
            //if (_Model.BhandarId == _loggedInUSerDetails.DistrictId)
            //{
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.BhandarMprDataForDashboard(_Model);
            //}
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }


        [HttpPost]
        public string BhandarDashboardData()
        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();

            int DistrictId = 0;
            int.TryParse(_loggedInUSerDetails.DistrictId.ToString(), out DistrictId);
            DataTable DT = null;
            //if (_Model.BhandarId == _loggedInUSerDetails.DistrictId)
            //{
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.BhandarDashboardData(DistrictId);
            //}
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        public string KVSSociety(int KVSSID = 0, int DistrictId = 0)
        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();

            DataTable DT = null;
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.KVSSociety(KVSSID, DistrictId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        public string GetGSSocietyList(int BlockId = 0)
        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();

            DataTable DT = null;
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            string WhereClause = "";
            if (BlockId > 0)
            {
                WhereClause = " and GSS.BlockId=" + BlockId.ToString();
            }
            DT = objEntryForms.GetGSSocietyList(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        public string KVSSData(KVSSDataEntity KVSSEntity)
        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();

            DataTable DT = null;
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.KVSSData(KVSSEntity);
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        [ActionName("SaveKVSSForms")]
        public JsonResult SaveKVSSFormsData(KVSSDataEntity _Model)
        {
            int result = -1;
            if (_Model != null)
            {
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                ModelState.Remove("UpdateBy"); ModelState.Remove("UpdateIP");
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));
                _Model.InsertBy = _loggedInUSerDetails.UserId;
                _Model.InsertIP = COMMON_FUNCTION.GetIpAddress();
                result = objEntryForms.SaveKVSSFormsData(_Model);
            }
            return Json(result);
        }

        [HttpPost]
        [ActionName("SaveStLoanForms")]
        public JsonResult SaveStLoanFormsData(StLoanFormDataEntity _Model)
        {
            int result = -1;
            if (_Model != null)
            {
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));
                _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                result = objEntryForms.SaveStLoanFormsData(_Model);
            }
            return Json(result);
        }

        [HttpPost]
        [ActionName("SaveStLoanFormsExl")]
        public JsonResult SaveStLoanFormsExl(StLoanFormDataEntity _Model)
        {
            string result = "";
            string Msg = "";
            if (_Model != null)
            {
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));
                string fileName = string.Empty;
                _Model.ExcelUrl = "";
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    try
                    {
                        var file = Request.Files[0];
                        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        _Model.ExcelUrl = fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/EntryForms"), fileName));
                    }
                    catch { }

                    string connString = "";
                    List<StLoanFormDataTranEntity> StLoanFormDataTranlstLocal = new List<StLoanFormDataTranEntity>();
                    string strFileType = System.IO.Path.GetExtension(_Model.ExcelUrl).ToLower();
                    if (1 > 0)
                    {
                        string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyExcel/" + _Model.ExcelUrl));

                        if (strFileType.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (strFileType.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        OleDbConnection conn = new OleDbConnection(connString);

                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        foreach (DataRow SheetRow in dtSheet.Rows)
                        {
                            string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                            OleDbCommand cmd2 = new OleDbCommand(query, conn);
                            OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            //  dt.Columns.Add("CEOAadharEnc", typeof(System.String));
                            //  dt.Columns.Add("ExcelUploadId", typeof(System.String));
                            try
                            {
                                int i = 0;

                                foreach (DataRow Dr in dt.Rows)
                                {
                                    i++;
                                    string DCCB_NAME = Dr["DCCB_NAME"].ToString().Trim();
                                    string Total_Application_Received = Dr["Total_Application_Received"].ToString().Trim();
                                    string Approved_By_Pacs = Dr["Approved_By_Pacs"].ToString().Trim();
                                    string Approved_By_Branch = Dr["Approved_By_Branch"].ToString().Trim();
                                    string DMR_Credited_And_Loan_Disbursement = Dr["DMR_Credited_And_Loan_Disbursement"].ToString().Trim();
                                    if (Total_Application_Received.Length > 0)
                                    {
                                        double myNum = 0;
                                        if (Double.TryParse(Total_Application_Received.ToString(), out myNum))
                                        {
                                            // it is a number
                                        }
                                        else
                                        {
                                            Msg = "Please enter only digit in Total_Application_Received of " + DCCB_NAME + " on line No " + i.ToString();
                                            // it is not a number
                                        }
                                    }
                                    if (Approved_By_Pacs.Length > 0)
                                    {
                                        double myNum = 0;
                                        if (Double.TryParse(Approved_By_Pacs.ToString(), out myNum))
                                        {
                                            // it is a number
                                        }
                                        else
                                        {
                                            Msg = "Please enter only digit in Approved_By_Pacs of " + DCCB_NAME + " on line No " + i.ToString();
                                            // it is not a number
                                        }
                                    }
                                    if (Approved_By_Branch.Length > 0)
                                    {
                                        double myNum = 0;
                                        if (Double.TryParse(Approved_By_Branch.ToString(), out myNum))
                                        {
                                            // it is a number
                                        }
                                        else
                                        {
                                            Msg = "Please enter only digit in Approved_By_Branch of " + DCCB_NAME + " on line No " + i.ToString();
                                            // it is not a number
                                        }
                                    }
                                    if (DMR_Credited_And_Loan_Disbursement.Length > 0)
                                    {
                                        double myNum = 0;
                                        if (Double.TryParse(DMR_Credited_And_Loan_Disbursement.ToString(), out myNum))
                                        {
                                            // it is a number
                                        }
                                        else
                                        {
                                            Msg = "Please enter only digit in DMR_Credited_And_Loan_Disbursement of " + DCCB_NAME + " on line No " + i.ToString();
                                            // it is not a number
                                        }
                                    }

                                }

                                string ResultInner = "";
                                if (Msg == "")
                                {
                                    DataTable dtCCB = objEntryForms.CCBData();
                                    IDictionary<string, int> dict = new Dictionary<string, int>();
                                    foreach (DataRow dr in dtCCB.Rows)
                                    {
                                        int CCBId = 0;
                                        int.TryParse(dr["CCBId"].ToString(), out CCBId);
                                        string CCBNameEn = "";
                                        CCBNameEn = dr["CCBNameEn"].ToString();
                                        dict.Add(CCBNameEn, CCBId);
                                    }
                                    foreach (DataRow Dr in dt.Rows)
                                    {
                                        i++;
                                        string DCCB_NAME = Dr["DCCB_NAME"].ToString().Trim();
                                        string Total_Application_Received = Dr["Total_Application_Received"].ToString().Trim();
                                        string Approved_By_Pacs = Dr["Approved_By_Pacs"].ToString().Trim();
                                        string Approved_By_Branch = Dr["Approved_By_Branch"].ToString().Trim();
                                        string DMR_Credited_And_Loan_Disbursement = Dr["DMR_Credited_And_Loan_Disbursement"].ToString().Trim();
                                        if (Total_Application_Received.Length == 0)
                                        {
                                            Total_Application_Received = "0";
                                        }
                                        if (Approved_By_Pacs.Length == 0)
                                        {
                                            Approved_By_Pacs = "0";
                                        }
                                        if (Approved_By_Branch.Length == 0)
                                        {
                                            Approved_By_Branch = "0";
                                        }
                                        if (DMR_Credited_And_Loan_Disbursement.Length == 0)
                                        {
                                            DMR_Credited_And_Loan_Disbursement = "0";
                                        }
                                        StLoanFormDataTranEntity StLoanFormDataTranlstItemLocal = new StLoanFormDataTranEntity();
                                        if (dict.ContainsKey(DCCB_NAME))
                                        {
                                            StLoanFormDataTranlstItemLocal.CCBId = dict[DCCB_NAME];
                                            StLoanFormDataTranlstItemLocal.Total_Application_Received = Convert.ToInt32(Total_Application_Received);
                                            StLoanFormDataTranlstItemLocal.Approved_By_Pacs = Convert.ToInt32(Approved_By_Pacs);
                                            StLoanFormDataTranlstItemLocal.Approved_By_Branch = Convert.ToInt32(Approved_By_Branch);
                                            StLoanFormDataTranlstItemLocal.DMR_Credited_And_Loan_Disbursement = Convert.ToInt32(DMR_Credited_And_Loan_Disbursement);

                                            StLoanFormDataTranlstItemLocal.IsActive = 1;
                                            StLoanFormDataTranlstItemLocal.CreatedBy = int.Parse(_loggedInUSerDetails.UserId.ToString());
                                            StLoanFormDataTranlstItemLocal.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                                            StLoanFormDataTranlstLocal.Add(StLoanFormDataTranlstItemLocal);
                                        }

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Msg = ex.Message;
                            }
                            break;
                        }
                    }
                    _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                    _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                    if (Msg == "")
                    {
                        _Model.StLoanFormDataTranlst = StLoanFormDataTranlstLocal;
                        result = objEntryForms.SaveStLoanFormsData(_Model).ToString();
                    }
                    else
                        result = Msg;
                }
            }
            return Json(result);
        }

        #region FertiMNGEntry

        [HttpPost]
        public string SaveFertiMNGReport(FertiMngReport FertiEntity)
        {
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            FertiEntity.CreatedBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
            FertiEntity.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            FertiEntity.IsActive = 1;
            return objEntryForms.SaveFertiMNGReport(FertiEntity).ToString();
        }
        [HttpPost]
        public string GetStockVal(FertiMngReport FertiEntity)
        {
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            FertiEntity.CreatedBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
            FertiEntity.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            FertiEntity.IsActive = 1;
            return objEntryForms.GetStockVal(FertiEntity).ToString();
        }
        [HttpPost]
        public string GetFertiMNGReportList(FertiMngReport FertiEntity)
        {
            DataTable DT = null;
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.GetFertiMNGReportList(FertiEntity);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public string DeleteFertiMNGReport(int FertiMNGReportId)
        {
            int DT = 0;
            if (FertiMNGReportId > 0)
            {
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                DT = objEntryForms.DeleteFertiMNGReport(FertiMNGReportId); 
            }
            return (DT.ToString());
        }
        #endregion



        public ActionResult UploadFertilizerStock()
        {
            return View();
        }
        [HttpPost]
        [ActionName("SaveFertilizerStock")]
        public JsonResult SaveFertilizerStock(string Date)
        {
            string result = "";
            string Msg = "";
            
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));
                string fileName = string.Empty;
             
                if (Request.Files != null && Request.Files.Count > 0)
                {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/EntryForms"), fileName));

                    DataTable memberData = new DataTable();

                    result = ValidateExcel(ref memberData, Path.Combine(Server.MapPath("~/Content/EntryForms"), fileName));

                    if (result != "OK")
                    {
                        return Json(CommonMessageFactory.GetJsonResponse("false", result));
                    }
                    else
                    {
                        return Json(objEntryForms.UploadFertilizerStock(memberData, Date));
                    }

                }
                catch { }

                    
                }
            
            return Json(result);
        }

        public string ValidateExcel(ref DataTable Data, string excelPath)
        {
            string result = "";
            try
            {
                string extension = Path.GetExtension(excelPath).ToLower();
                if (extension != ".xls")
                {
                    result = "Please upload xlsx formate only";
                }
                else
                if (System.IO.File.Exists(excelPath))
                {
                    Data = COMMON_FUNCTION.ReadExcelData(excelPath, "StockData");
                    string str = "";
                    foreach (DataColumn column in Data.Columns)
                    {
                        str += (str != "" ? "," : "") + column.ColumnName.Replace(" ", "");
                    }

                    string[] ckclm = str.Split(',');
                    bool isAllColomns = true;
                    string colomn = "";

                    string[] excelColumns = new string[] { "RetIdStr", "NatureName", "Fertilizer", "OpeningBalance" };                 

                    foreach (string clm in excelColumns)
                    {
                        if (!ckclm.Contains(clm))
                        {
                            isAllColomns = false;
                            colomn += (colomn != "" ? ", " : "") + clm;
                        }
                    }



                    if (!isAllColomns)
                    {
                        result += "Columns ''" + colomn + "'' is missing or incorrect. Please check and upload again.<br/>";
                    }
                    string aadharck = "";
                    for (int i = 0; i < Data.Rows.Count; i++)
                    {
                        foreach (DataColumn column in Data.Columns)
                        {
                            if (Data.Rows[i][column.ColumnName] == null || Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") == "")
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                            }
                            int n; float b;
                           
                            if (column.ColumnName == "OpeningBalance" && Data.Rows[i][column.ColumnName].ToString() != null && Data.Rows[i][column.ColumnName].ToString() != "" && !float.TryParse(Data.Rows[i][column.ColumnName].ToString(), out b))
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "''. Please fill numeric or decimal value only.<br/>";
                            }   
                        }
                    }

                    if (result == "")
                        result = "OK";

                }
                else
                {
                    result = "Execl File is not uploaded on server";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }



    }
}