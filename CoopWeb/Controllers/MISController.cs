using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using FactoryAndBoiler.Payment;
using java.io;
using java.util;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using sun.reflect.generics.scope;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Cooperative.Models.MISModel;


namespace Cooperative.Controllers
{
    [Error]
    //[LogActionFilter]
    //[CustomAuthentication]
    public class
        MISController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        [HttpGet]
        [ActionName("GetChartData")]
        public string GetChartData()
        {

            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModel.GetAnswerofQuestionForGraph());
        }

        [HttpGet]
        public ActionResult GccDistrictWiseSummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBDistrictWiseReport()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TamilSelfieReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBCallCenterReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBBranchWiseTaamilReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBLoanWiseReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBSummaryReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GccMahilaNidhiDistrictWiseSummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageSocietyDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AuditProgressBlockRpt()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyElectionList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyLocationReportList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GccBankWiseSummaryList() { return View(); }

        [HttpGet]
        public ActionResult GccBankWiseSummary() { return View(); }

        [HttpGet]
        public ActionResult GccBankWiseSanctionDetails() { return View(); }

        //Bhavesh Singh 25/03/2026 for CCP Report
        [HttpGet]
        public ActionResult GccReportCCP() { return View(); }

        [HttpGet]
        public ActionResult GccBankWiseReApplySummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GccApplicationList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GccRecoveryReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GCCRecoveryLoanListDetail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyMergeReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GCCSummaryDistrictList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GCCRecoverySummaryBankList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyMappingReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PldbCMSOtsApplicationReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GCCDataYearWiseInformation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GCCDetailsYearWise()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationPldbWiseList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationSectorWiseList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetGSSMpacsTargetDetailReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationPurposeWiseList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationPurposeWiseSectorList()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GSSMpacsTargetList()
        {
            return View();
        }


        [HttpGet]
        public ActionResult PLBDLoanApplicationCountDetailList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationSectorDetailList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBSubventionODApplicationList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLBDLoanApplicationDetailList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PldbCMSOtsApplicationDetailReport()
        {
            return View();
        }


        [HttpGet]
        public ActionResult PLDBSubventionODApplicationDetailsList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GccMahilaNidhiApplicationList() { return View(); }


        [HttpGet]
        public ActionResult TrainingReport()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TrainningReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult CaseReport()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TrainningReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult CCSComplaintSoceityReport()

        {
            return View();
        }

        [HttpGet]
        public ActionResult DairySocietyRegisterDistWise()
        {
            return View();
        }
        public ActionResult PLDBLoanApplicationMisReport()
        {
            return View();
        }

        public ActionResult MPacs_InspectorList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyElectionWorkFlow()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetComplaintReport(DataPagingModel Details, int DistrictId = 0, string SocietyId = "0", int SearchType = 1)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            string TotalAmount = "";
            string TotalCSS = "";
            DataTable DT = objMISModel.GetComplaintReport(ref Details, ref TotalAmount, ref TotalCSS, DistrictId, SocietyId, SearchType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details, TotalAmount = TotalAmount, TotalCSS = TotalCSS }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult ElectionReport()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TrainningReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult SocietyRegisterReport()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyRegisterReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }


        [HttpGet]
        public ActionResult OICDashbord()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyRegisterReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult AuditReportAll()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CCSPropertyReport()
        {
            return View();
        }


        [HttpGet]
        public ActionResult BeneficiaryListSummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DueDateDiary()
        {
            return View();
        }



        [HttpGet]
        public ActionResult BeneficiaryListSummaryDetails()
        {
            return View();
        }


        [HttpPost]
        [ActionName("GetOICDashbordData")]
        public string GetOICDashbordData(int DistrictId, int ActId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel _Modal = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_Modal.GetOICDashbordData(DistrictId, ActId));

        }

        [HttpGet]
        public ActionResult InspectionReport()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.InspectionReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpPost]
        [ActionName("GetInspectionReport")]
        public string GetInspectionReport(int FinancialYearId, int ActId, int DistrictId, int FilterId)
        {
            MISModel _Modal = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_Modal.GetInspectionReportData(FinancialYearId, ActId, DistrictId, FilterId));

        }


        #region Code By Jitendra Date From 18-July-2019

        [HttpGet]
        public ActionResult AuditReportDistrictWise()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AuditReportDivisionWise()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AuditReportShow_Admin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PACSReport_Admin()
        {
            return View();
        }



        [HttpGet]
        public ActionResult ExcelUploadForAuditReportDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GodownReport()
        {
            return View();
        }

        //Bhavesh
        [HttpGet]
        public ActionResult PLBDLoanApplicationSummeryWiseList()
        {
            return View();
        }

        //Bhavesh 08-01-2026 for Count Summer Wise
        [HttpGet]
        public ActionResult PLBDLoanApplicationSummeryWiseDetailList()
        {
            return View();
        }


        [HttpGet]
        public ActionResult MMPBYFarmerDetailsReport()
        {
            return View();
        }

        // Bhavesh Singh 17/03/2026

        [HttpGet]
        public ActionResult PacsMembershipDistrictWiseCountList()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InsertAuditReportDashboardByExcel")]
        public string InsertAuditReportDashboardByExcel(AuditReportDashboardEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            _Model.SSOID = _LoggedInUser.SSOID;
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/AuditReportExcelForDashboard"), fileName));
                }
                catch { }

                _Model.DistrictId = _LoggedInUser.DistrictId;
                _Model.CreatedBy = int.Parse(_LoggedInUser.UserId.ToString());
                _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                MISModel objMISModel = new MISModel(new DBLogger());
                return objMISModel.InsertAuditReportDashboardByExcel(_Model).ToString();
            }
            return "File Not Uploaded";
        }


        [HttpPost]
        [ActionName("GetAuditReportForDashboardDetails")]
        public string GetAuditReportForDashboardDetails()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetAuditReportDashboardExcelDetails(_LoggedInUser.UserId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("GetExlAuditReportDashboardDetails")]
        public string GetExlAuditReportDashboardDetails(int ExlId, int StatusId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetExlAuditReportDashboardDetails(ExlId, StatusId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }



        [HttpPost]
        [ActionName("InsertAuditReportDashboardHeadWiseByExcel")]
        public string InsertAuditReportDashboardHeadWiseByExcel(AuditReportDashboardEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            _Model.SSOID = _LoggedInUser.SSOID;
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/AuditReportExcelForDashboard"), fileName));
                }
                catch { }

                _Model.DistrictId = _LoggedInUser.DistrictId;
                _Model.CreatedBy = int.Parse(_LoggedInUser.UserId.ToString());
                _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
                MISModel objMISModel = new MISModel(new DBLogger());
                return objMISModel.InsertAuditReportHeadWiseDashboardByExcel(_Model).ToString();
            }
            return "File Not Uploaded";
        }


        [HttpPost]
        [ActionName("GetAuditReportHeadWiseForDashboardDetails")]
        public string GetAuditReportHeadWiseForDashboardDetails()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetAuditReportHeadWiseDashboardExcelDetails(_LoggedInUser.UserId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("GetExlAuditReportHeadWiseDashboardDetails")]
        public string GetExlAuditReportHeadWiseDashboardDetails(int ExlId, int StatusId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetExlAuditReportHeadWiseDashboardDetails(ExlId, StatusId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult AuditReportShow()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetAuditReportRegionWise")]
        public string GetAuditReportRegionWise(string Year, string Month, string Type)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetAuditReportRegionWise(int.Parse(Year), int.Parse(Month), int.Parse(Type));
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult PACSReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CCSComplaintReport()
        {
            return View();
        }

        [HttpPost]
        public string GetPACSReportData(int LevelId, int DistrictId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.PACSReport(LevelId, DistrictId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult FertilizerReport()
        {
            return View();
        }

        [HttpPost]
        public string GetFertilizerReportData(string DistrictId, string NatureId, string FilterType, string dateOn, string Year = "", string Month = "", int param1 = 0)
        {
            MISModel objMISModel = new MISModel(new DBLogger());

            int _MonthId = 0, _DistictId = 0, _NatureId = 0, _FilterType = 0;
            int.TryParse(Month, out _MonthId);
            int.TryParse(DistrictId, out _DistictId);
            int.TryParse(NatureId, out _NatureId);
            int.TryParse(FilterType, out _FilterType);


            if (FilterType == "1")
            {
                if (param1 == 2)
                    return ReportCommonFunction.FertilizerreportDistrictWise(objMISModel.FertilizerReportData(_DistictId, 0, dateOn, _NatureId, param1));

                else if (param1 == 3)
                    return ReportCommonFunction.FertilizerRetailerDetailer(objMISModel.FertilizerReportData(_DistictId, 0, dateOn, _NatureId, param1));
                else
                    return "[]";

            }
            else
            {
                return COMMON_FUNCTION.DataTableToJSONString(objMISModel.FertilizerRetailerTransactionReportData(_DistictId, Year, _MonthId, _NatureId, param1));
            }
        }


        [HttpPost]
        public JsonResult GetGodownReportData()
        {
            MISModel objMISModel = new MISModel(new DBLogger());

            DataTable DT = objMISModel.GodownFilterData("COUNT_DISTRICTWISE_GODOWN", _LoggedInUser.DistrictId);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));

        }

        [HttpPost]
        public JsonResult GetGodownReporDistricttData(int DistrictId, int StatusId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GodownFilterDistrictData("GODOWN_DITRICTWISE", DistrictId, StatusId);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }

        [HttpGet]
        public string RetailerTransactionYearMonthList()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMISModel.RetailerTransactionYearMonthList());
        }

        [HttpGet]
        public ActionResult RSWC_MIS_Report()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RSWCMIS123(List<Params> parameters)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GetRSWC_MISReportData(COMMON_FUNCTION.WhereClauseByParams(parameters));
            return Json(DT);
        }
        [HttpPost]
        public string GetComplaintReportData(int DistrictId, string SocietyId, int SearchType)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataSet DT = objMISModel.GetComplaintReportData(DistrictId, SocietyId, SearchType);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);

        }

        [CustomAuthentication]
        [HttpPost]
        public string GetWarehouseManagerDashboardData()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataSet DT = objMISModel.GetWarehouseManagerDashboardData(_LoggedInUser.WarehouseId);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }
        [HttpGet]
        public ActionResult CommodityWiseWRDetail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCommodityWiseWRDetails()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            return Json(objMISModel.GetWarehouseManagerDashboardData());
        }




        [CustomAuthentication]
        [HttpPost]
        public string GetSocietyDistWiseData1(string ActId, int SocietyTypeId, int DIVISION_ID = 0, int IsOnline = -1, int DistrictId = 0)
        {
            //int LevelId = 0;
            //if (DIVISION_ID==0)
            //    DIVISION_ID = _LoggedInUser.DivisionId;

            //if(DIVISION_ID != 0)
            //    LevelId = 2;
            //else
            //    LevelId = _LoggedInUser.LevelId;
            MISModel objMISModel = new MISModel(new DBLogger());
            //DataSet DT = objMISModel.GetSocietyDistWiseData(ActId, SocietyTypeId, CreatetionDate, LevelId, _LoggedInUser.DistrictId, DIVISION_ID, IsOnline);
            if (ActId.Length > 1)
            {
                ActId = "0";
            }
            DataSet DT = objMISModel.GetSocietyDistWiseData(int.Parse(ActId), SocietyTypeId, _LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.LevelId, DistrictId == 0 ? _LoggedInUser.DistrictId : DistrictId, _LoggedInUser.DivisionId, IsOnline);

            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);

        }
        //[HttpPost]
        //public string GetSocietyReportData(int StatusId, int ActId, int SocietyTypeId, int DistrictId)
        //{
        //    MISModel objMISModel = new MISModel(new DBLogger());
        //    DataSet DT = objMISModel.GetSocietyReportData(StatusId, ActId, SocietyTypeId, DistrictId);
        //    return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        //}

        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetSocietyReportData(DataPagingModel Details, int StatusId, int ActId, int SocietyTypeId, int DistrictId, int IsOnline = -1, int Status_Id = 0, int IsExport = 0, string FinancialYear = "")
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GetSocietyReportData(ref Details, StatusId, _LoggedInUser.UserRoleDeptMappingId, ActId, SocietyTypeId, DistrictId, IsOnline, Status_Id, IsExport, FinancialYear);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            //return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }

        [HttpGet]
        public ActionResult SocietyRegisterDistWise()
        {
            return View();
        }

        [HttpPost]
        public string GetWarehouseList(int DistrictId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            string whereClause = string.Empty;

            int _DistrictId = 0;

            if (int.TryParse(DistrictId.ToString(), out _DistrictId) && DistrictId != 0)
                whereClause = " AND A.DistrictId=" + _DistrictId;
            else
                whereClause = " AND 1=1";
            DataSet DT = objMISModel.GetWarehouseList(whereClause);

            return JsonConvert.SerializeObject(DT, Formatting.Indented);
        }

        [HttpPost]
        public JsonResult GetWRDetails(int CommodityId, int DistrictId, int WarehouseId, int KVSSId, int PurchaseCenterId, string Search)
        {
            string WhereClause = string.Empty;

            WhereClause += CommodityId > 0 ? " AND cm.CommodityId=" + CommodityId : " ";
            WhereClause += DistrictId > 0 ? " AND A.received_from_district=" + DistrictId : " ";
            WhereClause += WarehouseId > 0 ? " AND A.wharehouse_id=" + WarehouseId : " ";
            WhereClause += KVSSId > 0 ? " AND A.received_from_kvss=" + KVSSId : " ";
            WhereClause += PurchaseCenterId > 0 ? " AND A.purchase_center_id=" + PurchaseCenterId : " ";
            WhereClause += !string.IsNullOrEmpty(Search) ? " AND 1=1 " : " ";

            //string  whereCase = " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";


            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel objNew = new MISModel(new DBLogger());

            DataTable DT = objNew.GetWarehouseRSWCList(WhereClause);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }



        [HttpGet]
        public ActionResult WRReport()
        {
            return View();
        }





        [HttpPost]
        public JsonResult GetWRReport(DataPagingModel Details, string ReportType)
        {
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRReport(ref Details, ReportType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpPost]
        public string GetWRReportExportEn(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRReport(ref Details, ReportType);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>Warehouse </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    //sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");

                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    //sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : ReportType == "RO" ? "Regional Office" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Total Lots</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                decimal TotalWR = 0;
                int packages_lots = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WarehousNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");

                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "10" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (ReportType != "WR")
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalWR.ToString() + "</b> </td>");

                }
                sb.Append("<td style='text-align:right'> <b>" + packages_lots.ToString() + "</b> </td>");

                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                if (ReportType == "WR")
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                else
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " ElectionPDF(DataTable List, string FileName, string Titel)" }, ex);
                result = "Error";
            }
            return null;
        }
        [HttpPost]
        public string GetWRReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRReport(ref Details, ReportType);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>Warehouse </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    //sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");

                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    //sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : ReportType == "RO" ? "Regional Office" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Total Lots</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                decimal TotalWR = 0;
                int packages_lots = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WarehousNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");

                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "10" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (ReportType != "WR")
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalWR.ToString() + "</b> </td>");

                }
                sb.Append("<td style='text-align:right'> <b>" + packages_lots.ToString() + "</b> </td>");

                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                if (ReportType == "WR")
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                else
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }


        #region Warehouse Report


        [HttpGet]
        public ActionResult WarehouseWRReport()
        {
            return View();
        }

        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetWarehouseWRReport(DataPagingModel Details, string ReportType)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;

            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWarehouseWRReport(ref Details, ReportType, WarehouseId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetWarehouseWRReportExportEn(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWarehouseWRReport(ref Details, ReportType, WarehouseId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");
                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "12" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, (ReportType == "WR" ? "15px;" : "")));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWarehouseWRReportExportEn(DataPagingModel Details, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }
        [HttpPost]
        public string GetWarehouseWRReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWarehouseWRReport(ref Details, ReportType, WarehouseId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");
                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "12" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, (ReportType == "WR" ? "15px;" : "")));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWarehouseWRReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpGet]
        public ActionResult WRCountReport()
        {
            return View();
        }


        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetWRCountReport(DataPagingModel Details)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;

            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRDateWiseReport(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        public JsonResult GetWRCountReportExportEn(DataPagingModel Details)
        {

            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRDateWiseReport(ref Details);

                var FromDate = "";
                var ToDate = "";


                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate;
                    ToDate = toDate;
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate;
                    ToDate = FromDate;
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    ToDate = toDate;
                    FromDate = ToDate;
                }
                else
                {
                    FromDate = "01/04/" + DateTime.Now.Year;
                    ToDate = "31/03/" + (DateTime.Now.Year + 1);

                }









                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table id='content' border='1' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='" + (List.Columns.Count + 1) + "' style='text-align:left;font-size:20px;'>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD. JAIPUR");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:left;'>Commodity:</th><td  colspan='" + (List.Columns.Count) + "' style='text-align:left;'>" + List.Rows[0]["dt"] + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:left;'>Date:</th><td  colspan='" + (List.Columns.Count) + "' style='text-align:left;'>" + FromDate + " <b>To</b> " + ToDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:center;' rowspan='2' > Sr. No. </th>");
                sb.Append("<th style='text-align:center' rowspan='2'> Date  </th>   <th  colspan='" + (List.Columns.Count - 2) + "'></th>   ");
                sb.Append("<th  rowspan='2' style='text-align:center'> Total </th>");
                sb.Append("</tr>");
                sb.Append("<tr>");

                int[] columnTotal = new int[List.Rows.Count - 1];
                int[] rowTotal = new int[List.Columns.Count - 2];


                if (List.Rows.Count > 1)
                {
                    for (int j = 1; j < List.Columns.Count - 1; j++)
                    {
                        sb.Append("<th style='text-align:center'> " + List.Rows[0]["name" + j] + " </th>");
                    }
                }
                //sb.Append("</tr>");
                //sb.Append("<tr>");



                sb.Append("</tr>");
                for (int i = 1; i < List.Rows.Count; i++)
                {
                    sb.Append("<tr><td style='text-align:center'> " + (i) + " </td>");
                    sb.Append("<td  style='text-align:center'> " + List.Rows[i]["dt"].ToString() + " </td>");
                    for (int j = 1; j < List.Columns.Count - 1; j++)
                    {
                        sb.Append("<td style='text-align:center'> " + List.Rows[i]["name" + j].ToString() + " </td>");
                        rowTotal[j - 1] = rowTotal[j - 1] + Convert.ToInt32(List.Rows[i]["name" + j].ToString());
                        columnTotal[i - 1] = columnTotal[i - 1] + Convert.ToInt32(List.Rows[i]["name" + j].ToString());
                    }
                    //sb.Append("</tr>");
                    // sb.Append("<tr>");
                    sb.Append("<td style='text-align:center'> " + columnTotal[i - 1].ToString() + " </td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr>");
                sb.Append("<th colspan=2 style='text-align:center'> Total </th>");
                if (List.Rows.Count > 1)
                {
                    for (int i = 0; i < rowTotal.Length; i++)
                    {
                        sb.Append("<th style='text-align:center'> " + rowTotal[i] + " </th>");
                    }
                }
                sb.Append("</tr>");

                sb.Append("</table>");
                return Json(new
                {
                    DataList = sb.ToString()
                }, JsonRequestBehavior.AllowGet);


            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRCountReportExportEn(DataPagingModel Details)" }, ex);
                // result = "Error";
            }
            return null;
        }





        [HttpGet]
        public ActionResult WRCountReportWarehouseWise()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetWRCountReportWarehouseWise(DataPagingModel Details, string Level)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;

            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRCount(ref Details, Level);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetWRCountReportWarehouseWiseExportEn(DataPagingModel Details, string ReportHead, string Level)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRCount(ref Details, Level);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (Level == "3" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (Level == "3")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>Warehouse</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                int TotalWR = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (Level == "3")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td></tr>");

                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (Level == "3" ? "12" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (Level != "3")
                {
                    sb.Append("<td  style='text-align:right'><b>" + TotalWR + "</b>  </td>");
                    sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td></tr>");
                }
                else
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                    sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                }


                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, (Level == "3" ? "15px;" : "")));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRCountReportWarehouseWiseExportEn(DataPagingModel Details, string ReportHead, string Level)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpGet]
        public ActionResult WRSummaryReport()
        {
            return View();
        }

        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetWRSummaryList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryList(ref Details, "2");
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetWRSummaryDetailsList(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryDetailsList(RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string GetWRSummaryReportExportEn(DataPagingModel Details, string ReportHead)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                DataTable List = objNew.GetWRSummaryList(ref Details, "2");
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:10px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:10px;'>WR Summary Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:10px;'>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:center;font-size:10px;'>Sr.No. </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'>Summary No. </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'>Commodity Name </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'>Summary Date </th>");
                sb.Append("<th style='text-align:center;font-size:10px;'>Total WR </th>");
                sb.Append("<th style='text-align:center;font-size:10px;'>Total Lots </th>");
                sb.Append("<th style='text-align:center;font-size:10px;'>Net Quantity (Quintal) </th>");
                sb.Append("<th style='text-align:center;font-size:10px;'>Total Value </th>");
                sb.Append("</tr>");



                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalLots = 0;
                int TotalWR = 0;
                decimal TotalValue = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    sb.Append("<tr><td style='text-align:center;font-size:10px;'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["request_no"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["CommodityNameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["created_date"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right;font-size:10px;'> " + dr["TotalWR"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right;font-size:10px;'> " + dr["packages_lots"].ToString() + " </td>");
                    sb.Append("<td style='text-align:right';font-size:10px;> " + dr["net_quantity"].ToString() + " </td>");
                    sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["TotalValue"].ToString() + " </td>");
                    sb.Append("</tr>");
                    TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                    TotalLots += Convert.ToDecimal(dr["packages_lots"].ToString());
                    TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                    TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                }
                sb.Append("<tr><td colspan='4'  style='text-align:right;font-size:10px;'><b>Total</b></td>");
                sb.Append("<td style='text-align:right;font-size:10px;'> <b>" + TotalWR + "</b> </td>");
                sb.Append("<td style='text-align:right;font-size:10px;'> <b>" + TotalLots + "</b> </td>");
                sb.Append("<td  style='text-align:right;font-size:10px;'><b>" + TotalNetQuantity.ToString() + "</b>  </td>");
                sb.Append("<td  style='text-align:right;font-size:10px;'><b>" + TotalValue.ToString() + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRSummaryReportExportEn(DataPagingModel Details, string ReportHead)" }, ex);
                result = "Error";
            }
            return null;
        }

        [HttpPost]
        public string GetWRSummaryDetailsReportExportEn(string RequestNo, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;



                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                DataTable List = objNew.GetWRSummaryDetailsList(RequestNo);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Summary Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                ReportHead += ",Commodity#" + List.Rows[0]["CommodityNameEn"];

                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:10px;'>");


                sb.Append("<tr>");
                sb.Append("<th style='text-align:center'>Sr.No. </th>");
                sb.Append("<th style='text-align:center'>WR No. </th>");
                //sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                //sb.Append("<th style='text-align:center'>District </th>");
                sb.Append("<th style='text-align:center'>KVSS </th>");
                //sb.Append("<th style='text-align:center'>Commodity </th>");
                sb.Append("<th style='text-align:center'>Purchase Center </th>");
                sb.Append("<th style='text-align:center'>WR DATE </th>");
                //sb.Append("<th style='text-align:center'>FROM DATE </th>");
                //sb.Append("<th style='text-align:center'>TO DATE </th>");
                sb.Append("<th style='text-align:center'>Price </th>");
                sb.Append("<th style='text-align:center'>Total Packages </th>");
                sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                sb.Append("<th style='text-align:center'>Total Value </th>");
                sb.Append("</tr>");



                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                int TotalPackages = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                    sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["TotalProce"].ToString() + " </td></tr>");
                    TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                    TotalValue += Convert.ToDecimal(dr["TotalProce"].ToString());
                    TotalPackages += Convert.ToInt32(dr["packages_lots"].ToString());


                }
                sb.Append("<tr><td colspan='6'  style='text-align:right'><b>Total</b></td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalPackages.ToString() + "</b> </td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRSummaryDetailsReportExportEn(string RequestNo, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpGet]
        public void GetWRSummaryDetailsReportExportExcelEn(string RequestNo)
        {
            string result = "";
            string OrderNo = "";
            try
            {


                int WarehouseId = _LoggedInUser.WarehouseId;



                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                DataTable List = objNew.GetWRSummaryDetailsList(RequestNo);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>WR Details</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:15px;'>");
                sb.Append("<tr>");
                sb.Append("<th colspan='13' style='text-align:left:font-size:25px;' >WR Summary Report ");
                sb.Append("</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th   style='text-align:left;'> ");
                sb.Append("Summary No");
                sb.Append("</th>");
                sb.Append("<td  colspan='12' style='text-align:left;'> ");
                sb.Append(RequestNo);
                sb.Append("  </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th   style='text-align:left;'> ");
                sb.Append("Summary Date");
                sb.Append("</th>");
                sb.Append("<td  colspan='12' style='text-align:left;'> ");
                sb.Append(List.Rows[0]["summary_date"].ToString());
                sb.Append("  </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th   style='text-align:left;'> ");
                sb.Append("Commodity");
                sb.Append("</th>");
                sb.Append("<td  colspan='12' style='text-align:left;'> ");
                sb.Append(List.Rows[0]["CommodityNameEn"].ToString());
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th colspan='13'>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:15px;'>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:center'>Sr.No. </th>");
                sb.Append("<th style='text-align:left'>WR No. </th>");
                sb.Append("<th style='text-align:left'>Sr.No. Of Physical Receipt </th>");
                sb.Append("<th style='text-align:left'>Regional Office </th>");
                sb.Append("<th style='text-align:left'>KVSS </th>");
                //sb.Append("<th style='text-align:center'>Commodity </th>");
                sb.Append("<th style='text-align:left'>Purchase Center </th>");
                sb.Append("<th style='text-align:left'>WR DATE </th>");
                sb.Append("<th style='text-align:left'>FROM DATE </th>");
                sb.Append("<th style='text-align:left'>TO DATE </th>");
                sb.Append("<th style='text-align:right'>Price </th>");
                sb.Append("<th style='text-align:right'>Total Packages </th>");
                sb.Append("<th style='text-align:right'>Net Quantity (Quintal) </th>");
                sb.Append("<th style='text-align:right'>Total Value </th>");
                sb.Append("</tr>");
                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                int TotalPackages = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["RONameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["Price"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                    sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                    TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                    TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    TotalPackages += Convert.ToInt32(dr["packages_lots"].ToString());
                }
                sb.Append("<tr><td colspan='10'  style='text-align:right'><b>Total</b></td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalPackages.ToString() + "</b> </td>");
                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");


                sb.Append("</table></body></html>");




                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=WRSummaryReport_" + RequestNo + ".xls");

                Response.Output.Write(sb.ToString());
                Response.End();






                //ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //DataTable DT = new DataTable();
                //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRSummaryDetailsReportExportExcelEn(string RequestNo)" }, ex);
                result = "Error";
            }
            //return null;
        }

        [HttpGet]
        [CustomAuthentication]
        public ActionResult WRRoReport()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.DivisionId = _LoggedInUser.DivisionId;
            return View();
        }

        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetWRRoReport(DataPagingModel Details, string ReportType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRROReport(ref Details, ReportType, _LoggedInUser.DivisionId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpPost]
        public string GetWRRoReportExportEn(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRROReport(ref Details, ReportType, _LoggedInUser.DivisionId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>Warehouse </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    //sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");

                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    //sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : ReportType == "RO" ? "Regional Office" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Total Lots</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                decimal TotalWR = 0;
                int packages_lots = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WarehousNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");

                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "10" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (ReportType != "WR")
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalWR.ToString() + "</b> </td>");

                }
                sb.Append("<td style='text-align:right'> <b>" + packages_lots.ToString() + "</b> </td>");

                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                if (ReportType == "WR")
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                else
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = "  GetWRRoReportExportEn(DataPagingModel Details, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }
        [HttpPost]
        public string GetWRRoReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRROReport(ref Details, ReportType, _LoggedInUser.DivisionId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (ReportType == "WR")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>Warehouse </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    //sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");

                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    //sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>" + (ReportType == "FY" ? "Financial Year" : ReportType == "Warehouse" ? "Warehouse" : ReportType == "Commodity" ? "Commodity" : ReportType == "KVSS" ? "KVSS" : ReportType == "PurchaseCenter" ? "Purchase Center" : ReportType == "RO" ? "Regional Office" : "") + "</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Total Lots</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");
                    sb.Append("<th style='text-align:right'>Total Value</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                decimal TotalWR = 0;
                int packages_lots = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (ReportType == "WR")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WarehousNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");

                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        //sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                        packages_lots += Convert.ToInt32(dr["packages_lots"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (ReportType == "WR" ? "10" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (ReportType != "WR")
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalWR.ToString() + "</b> </td>");

                }
                sb.Append("<td style='text-align:right'> <b>" + packages_lots.ToString() + "</b> </td>");

                sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                if (ReportType == "WR")
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px;"));
                else
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRRoReportExportHi(DataPagingModel Details, string ReportHead, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }






        [HttpGet]
        public ActionResult WRCountROReport()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.DivisionId = _LoggedInUser.DivisionId;
            return View();
        }
        [HttpPost]
        public JsonResult GetWRCountROReport(DataPagingModel Details)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int DivisionId = _LoggedInUser.DivisionId;

            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRDateWiseROReport(ref Details, DivisionId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWRCountROReportExportEn(DataPagingModel Details)
        {

            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int DivisionId = _LoggedInUser.DivisionId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRDateWiseROReport(ref Details, DivisionId);

                var FromDate = "";
                var ToDate = "";


                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate;
                    ToDate = toDate;
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    FromDate = fromDate;
                    ToDate = FromDate;
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    ToDate = toDate;
                    FromDate = ToDate;
                }
                else
                {
                    FromDate = "01/04/" + DateTime.Now.Year;
                    ToDate = "31/03/" + (DateTime.Now.Year + 1);

                }









                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table id='content' border='1' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='" + (List.Columns.Count + 1) + "' style='text-align:left;font-size:20px;'>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD. JAIPUR");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:left;'>Commodity:</th><td  colspan='" + (List.Columns.Count) + "' style='text-align:left;'>" + List.Rows[0]["dt"] + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:left;'>Date:</th><td  colspan='" + (List.Columns.Count) + "' style='text-align:left;'>" + FromDate + " <b>To</b> " + ToDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:center;' rowspan='2' > Sr. No. </th>");
                sb.Append("<th style='text-align:center' rowspan='2'> Date  </th>   <th  colspan='" + (List.Columns.Count - 2) + "'></th>   ");
                sb.Append("<th  rowspan='2' style='text-align:center'> Total </th>");
                sb.Append("</tr>");
                sb.Append("<tr>");

                int[] columnTotal = new int[List.Rows.Count - 1];
                int[] rowTotal = new int[List.Columns.Count - 2];


                if (List.Rows.Count > 1)
                {
                    for (int j = 1; j < List.Columns.Count - 1; j++)
                    {
                        sb.Append("<th style='text-align:center'> " + List.Rows[0]["name" + j] + " </th>");
                    }
                }
                //sb.Append("</tr>");
                //sb.Append("<tr>");



                sb.Append("</tr>");
                for (int i = 1; i < List.Rows.Count; i++)
                {
                    sb.Append("<tr><td style='text-align:center'> " + (i) + " </td>");
                    sb.Append("<td  style='text-align:center'> " + List.Rows[i]["dt"].ToString() + " </td>");
                    for (int j = 1; j < List.Columns.Count - 1; j++)
                    {
                        sb.Append("<td style='text-align:center'> " + List.Rows[i]["name" + j].ToString() + " </td>");
                        rowTotal[j - 1] = rowTotal[j - 1] + Convert.ToInt32(List.Rows[i]["name" + j].ToString());
                        columnTotal[i - 1] = columnTotal[i - 1] + Convert.ToInt32(List.Rows[i]["name" + j].ToString());
                    }
                    //sb.Append("</tr>");
                    // sb.Append("<tr>");
                    sb.Append("<td style='text-align:center'> " + columnTotal[i - 1].ToString() + " </td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr>");
                sb.Append("<th colspan=2 style='text-align:center'> Total </th>");
                if (List.Rows.Count > 1)
                {
                    for (int i = 0; i < rowTotal.Length; i++)
                    {
                        sb.Append("<th style='text-align:center'> " + rowTotal[i] + " </th>");
                    }
                }
                sb.Append("</tr>");

                sb.Append("</table>");
                return Json(new
                {
                    DataList = sb.ToString()
                }, JsonRequestBehavior.AllowGet);


            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetWRCountROReportExportEn(DataPagingModel Details)" }, ex);
                // result = "Error";
            }
            return null;
        }






        [HttpGet]
        public ActionResult WRCountReportWarehouseWiseRO()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.DivisionId = _LoggedInUser.DivisionId;
            return View();
        }


        [HttpPost]
        public JsonResult GetWRCountReportWarehouseWiseRO(DataPagingModel Details, string Level)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int DivisionId = _LoggedInUser.DivisionId;

            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetWRCountRO(ref Details, Level, DivisionId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetWRCountReportWarehouseWiseROExportEn(DataPagingModel Details, string ReportHead, string Level)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }

                int WarehouseId = _LoggedInUser.WarehouseId;

                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetWRCountRO(ref Details, Level, _LoggedInUser.DivisionId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:10px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>WR Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (Level == "3" ? "font-size:10px;" : "font-size:11px;") + "'>");

                if (Level == "3")
                {
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:center'>WR No. </th>");
                    sb.Append("<th style='text-align:center'>Sr.No. Of Physical Receipt </th>");
                    sb.Append("<th style='text-align:center'>District </th>");
                    sb.Append("<th style='text-align:center'>KVSS </th>");
                    sb.Append("<th style='text-align:center'>Commodity </th>");
                    sb.Append("<th style='text-align:center'>Purchase Center </th>");
                    sb.Append("<th style='text-align:center'>WR DATE </th>");
                    sb.Append("<th style='text-align:center'>FROM DATE </th>");
                    sb.Append("<th style='text-align:center'>TO DATE </th>");
                    sb.Append("<th style='text-align:center'>Price </th>");
                    sb.Append("<th style='text-align:center'>No. of Packages or Lots </th>");
                    sb.Append("<th style='text-align:center'>Net Quantity (Quintal) </th>");
                    sb.Append("<th style='text-align:center'>Total Value </th>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr><th style='text-align:center'>Sr.No. </th>");
                    sb.Append("<th style='text-align:left'>Warehouse</th>");
                    sb.Append("<th style='text-align:right'>Total WR</th>");
                    sb.Append("<th style='text-align:right'>Net Quantity(Quintal)</th>");

                    sb.Append("</tr>");
                }


                int index = 1;
                decimal TotalNetQuantity = 0;
                decimal TotalValue = 0;
                int TotalWR = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    if (Level == "3")
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["receipt_serial_no"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["CommodityNameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["PurchaseCenterEng"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["from_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["to_date"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["Price"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["packages_lots"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalValue"].ToString() + " </td></tr>");
                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                    }
                    else
                    {
                        sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                        sb.Append("<td  style='text-align:left'> " + dr["NameEn"].ToString() + " </td>");
                        sb.Append("<td  style='text-align:right'> " + dr["TotalWR"].ToString() + " </td>");
                        sb.Append("<td style='text-align:right'> " + dr["net_quantity"].ToString() + " </td></tr>");

                        TotalNetQuantity += Convert.ToDecimal(dr["net_quantity"].ToString());
                        TotalValue += Convert.ToDecimal(dr["TotalValue"].ToString());
                        TotalWR += Convert.ToInt32(dr["TotalWR"].ToString());
                    }
                }
                sb.Append("<tr><td colspan='" + (Level == "3" ? "12" : "2") + "'  style='text-align:right'><b>Total</b></td>");
                if (Level != "3")
                {
                    sb.Append("<td  style='text-align:right'><b>" + TotalWR + "</b>  </td>");
                    sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td></tr>");
                }
                else
                {
                    sb.Append("<td style='text-align:right'> <b>" + TotalNetQuantity.ToString() + "</b> </td>");
                    sb.Append("<td  style='text-align:right'><b>" + TotalValue + "</b>  </td></tr>");
                }


                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, (Level == "3" ? "15px;" : "")));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = "GetWRCountReportWarehouseWiseROExportEn(DataPagingModel Details, string ReportHead, string Level)" }, ex);
                result = "Error";
            }
            return null;
        }





        #endregion


        [HttpGet]
        public ActionResult CCBInvestmentDetail()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetCCBInvestmentMISReport(int DistrictId, int DivisionId)
        {
            DataTable data = new MISModel(new DBLogger()).GetCCBInvestmentMISReport(DistrictId, DivisionId);
            return Json(COMMON_FUNCTION.DataTableToJSONString(data));
        }





        #endregion

        [HttpGet]
        public ActionResult SocietyListAssignedToAuditorReport()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetSocietyListAssignedToAuditor(DataPagingModel Details, int? Id, string FIN_YEAR_NAME, string DistrictId, string GroupByDivisionId, string GroupByDistrictId, string GroupBySocietyTypeId, bool isExport = false, int Month = 0, string IsEmpanelledCA = "", string AuditBy = "", int SocietyTypeId = 0)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();

            if (GroupByDivisionId != "0" && GroupByDivisionId != null && GroupByDivisionId != "" && GroupByDivisionId != "undefined")
            {
                Details.SearchParameter.Remove("A.Division_Id");
                Details.SearchParameter.Remove("GroupBy");
                Details.SearchParameter.Remove("FIN_YEAR_NAME");
                Details.SearchParameter.Remove("A.DISTRICT_ID");
                Details.SearchParameter.Remove("Societies.SocietyTypeId");
                Details.SearchParameter.Add("A.Division_Id", GroupByDivisionId);

            }
            if (GroupByDistrictId != "0" && GroupByDistrictId != null && GroupByDistrictId != "" && GroupByDistrictId != "undefined")
            {
                Details.SearchParameter.Remove("A.DISTRICT_ID");
                Details.SearchParameter.Remove("A.Division_Id");
                Details.SearchParameter.Remove("GroupBy");
                Details.SearchParameter.Remove("FIN_YEAR_NAME");
                Details.SearchParameter.Remove("Societies.SocietyTypeId");
                Details.SearchParameter.Add("A.DISTRICT_ID", GroupByDistrictId);
            }

            if (GroupBySocietyTypeId != "0" && GroupBySocietyTypeId != null && GroupBySocietyTypeId != "" && GroupBySocietyTypeId != "undefined")
            {
                Details.SearchParameter.Remove("A.DISTRICT_ID");
                Details.SearchParameter.Remove("A.Division_Id");
                Details.SearchParameter.Remove("GroupBy");
                Details.SearchParameter.Remove("FIN_YEAR_NAME");
                Details.SearchParameter.Remove("Societies.SocietyTypeId");
                Details.SearchParameter.Add("Societies.SocietyTypeId", GroupBySocietyTypeId);
            }
            if (DistrictId != "0" && DistrictId != null && DistrictId != "" && DistrictId != "undefined")
            {
                Details.SearchParameter.Remove("A.DISTRICT_ID");
                Details.SearchParameter.Remove("A.Division_Id");
                Details.SearchParameter.Remove("FIN_YEAR_NAME");
            }

            DataTable DT = objAuditormaster.SocietyListAssignedToAuditorReport(ref Details, Id, FIN_YEAR_NAME, DistrictId, isExport, Month, IsEmpanelledCA, AuditBy, SocietyTypeId);

            if (isExport && DT != null)
            {
                if (DT.Columns.Contains("FIN_YEAR_NAME"))
                    DT.Columns.Remove("FIN_YEAR_NAME");

                if (DT.Columns.Contains("Id"))
                    DT.Columns.Remove("Id");

                if (DT.Columns.Contains("TotalRecords"))
                    DT.Columns.Remove("TotalRecords");

                if (DT.Columns.Contains("TotalRecords1"))
                    DT.Columns.Remove("TotalRecords1");

                if (DT.Columns.Contains("RequestStatus"))
                    DT.Columns.Remove("RequestStatus");


                if (DT.Columns.Contains("AuditorAssignFilePath"))
                    DT.Columns.Remove("AuditorAssignFilePath");

                // DT.Columns.Remove("FIN_YEAR_NAME");




                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(DT, "Society-Audit-Upload-Report.xlsx");

                return Json(new { DataList = filename, DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSocietyListAssignedToAuditorById(string SocietyIdEnc = "")
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            DataTable DT = objAuditormaster.SocietyListAssignedToAuditorById(SocietyIdEnc);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSocietyListAssignedToAuditorReport()
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(objAuditormaster.GetSocietyListAssignedToAuditorReport()) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult SocietyAuditDocumentUploadReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyAuditDocumentUploadReport(DataPagingModel Details)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            DataTable DT = objAuditormaster.SocietyAuditDocumentUploadReport(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSocietyListAuditDocumentUploadReport()
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(objAuditormaster.GetSocietyLisSocietyAuditDocumentUploadReport()) }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionName("GetUserLevelId")]
        public string GetUserLevelId()
        {
            if (_LoggedInUser != null)
                return (_LoggedInUser.LevelId.ToString());
            else
                return "0";
        }


        [HttpGet]
        public ActionResult RegistrationAmountReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRegistrationAmountReport(DataPagingModel Details)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.UserRoleDeptMappingId.ToString();
            DataTable DT = objAuditormaster.GetRegistartionAmountReport(ref Details, _LoggedInUser.UserRoleDeptMappingId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRegistrationAmountSocietyDetails(DataPagingModel Details)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            DataTable DT = objAuditormaster.GetRegistrationAmountSocietyDetails(ref Details, _LoggedInUser.UserRoleDeptMappingId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        #region CCS Complaint Report

        [HttpPost]
        public JsonResult GetActionIdCCSComplaintReport(DataPagingModel Details, int ActionId, int DistrictId, bool IsRowCount = false, int MultiState = 0, int FinYear = 0)
        {
            string WhereClause = "";
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;

            MISModel objModel = new MISModel(new DBLogger());
            DataSet ds = new DataSet();
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;

            _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (objSocietyRegistrationEntity.LevelId == 1)
            {
                if (DistrictId != 0)
                {
                    WhereClause = " and a.DISTRICT_ID =" + DistrictId;
                }
            }
            else if (objSocietyRegistrationEntity.LevelId != 1)
                WhereClause = " and a.DISTRICT_ID = " + _LoggedInUser.DistrictId;


            DataTable DT = new DataTable();
            if (IsRowCount)
            {
                DT = objModel.GetActionIdCCSComplaintReportCount(ActionId, WhereClause);
            }
            else
            {
                DT = objModel.GetCCSComplaintStatusReportDetails(ref Details, ActionId, WhereClause, MultiState, DistrictId, FinYear);
            }
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCCSComplaintListSocieties(int ActionId, int DistrictId)
        {
            MISModel objModel = new MISModel(new DBLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            string WhereClause = "";
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;

            _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (objSocietyRegistrationEntity.LevelId == 1)
            {
                if (DistrictId != 0)
                {
                    WhereClause = " and a.DISTRICT_ID =" + DistrictId;
                }
            }
            else if (objSocietyRegistrationEntity.LevelId != 1)
                WhereClause = " and a.DISTRICT_ID = " + _LoggedInUser.DistrictId;
            DataTable DT = objModel.GetCCSComplaintListSocieties(ActionId, WhereClause);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string GetCCSData(string DistrictId, int ActionId)
        {
            SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
            // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objCitizenModel.GetCCSData(DistrictId, 0, ActionId));
        }

        public ActionResult GetCCSComplaintReport()
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            ViewBag.LoggedInUserLevelId = _LoggedInUser.LevelId;
            return View();
        }


        #endregion


        [HttpGet]
        public string GetPropertyDetailsReport(int SID = 0)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataSet ds = objModel.GetPropertyReport(SID);
            return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
        }


        #region Digital Register
        public ActionResult DigitalRegister()
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            ViewBag.LoggedInUserLevelId = _LoggedInUser.LevelId;
            return View();
        }

        [HttpPost]
        public JsonResult GetDigitalRegister(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetDigitalRegister(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Monthly Summary
        public ActionResult MonthlySummary()
        {
            return View();
        }


        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetMonthlySummary(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetMonthlySummary(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpPost]
        public JsonResult Get(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetMonthlySummary(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult MonthlySummaryExport(DataPagingModel Details)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable List = objNew.GetMonthlySummary(ref Details);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'> Monthly Summary ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                //sb.Append(ReportHead.Replace("#", " :- ").Replace(",", ", "));
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                sb.Append("<tr>");
                sb.Append("<th style='text-align:center'>Sr.No. </th>");
                sb.Append("<th style='text-align:center'>Year </th>");
                sb.Append("<th style='text-align:center'>Month  </th>");
                sb.Append("<th style='text-align:center'>District </th>");
                sb.Append("<th style='text-align:center'>Act  </th>");
                sb.Append("<th style='text-align:center'>Types of Society </th>");
                //sb.Append("<th style='text-align:center'>Count of APP NO </th>");
                sb.Append("<th style='text-align:center'>No of Application Received </th>");
                sb.Append("<th style='text-align:center'>No of Application Registered </th>");
                sb.Append("<th style='text-align:center'>No of Application Pending at Citizen </th>");
                sb.Append("<th style='text-align:center'>No of Application Pending at Department </th>");
                sb.Append("<th style='text-align:center'>No of Application Rejected </th>");
                sb.Append("<th style='text-align:center'>No of Application Auto Close </th>");
                sb.Append("<th style='text-align:center'>Fee Deposited In the Month </th>");
                sb.Append("</tr>");


                int index = 1;
                foreach (DataRow dr in List.Rows) // search whole table
                {

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Year"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Month"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["District"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Act"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["TypesofSociety"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["NoOfApplicationReceived"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left'> " + dr["WRGenerateDate"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["NoOfApplicationRegistered"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["NoOfApplicationPendingAtCitizen"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["NoOfApplicationPendingAtDepartment"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["NoOfApplicationRejected"].ToString() + " </td>");
                    sb.Append("<td style='text-align:right'> " + dr["NoOfApplicationAutoClose"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:right'> " + dr["FeeDepositedInTheMonth"].ToString() + " </td></tr>");
                    sb.Append("</tr>");

                }
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");

                return Json(new
                {
                    DataList = sb.ToString()
                }, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = "MonthlySummaryExport(DataPagingModel Details)" }, ex);
                result = "Error";
            }
            return null;
        }


        #endregion






        #region Society Analytics Report
        public ActionResult SocietyAnalytics()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietySatausAnalytics(DataPagingModel Details, int ActId = 0, int StatusId = 0, int DistrictId = 0)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataSet DT = objModel.GetSocietySatausAnalytics(ref Details, ActId, StatusId, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Bulk Insert By Excel Report
        public ActionResult BulkInsertByExcelReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBulkInsertByExcelReport(string FromDate, string ToDate)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocityExcelDetails(FromDate, ToDate);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Society Audit District Wise Report

        public ActionResult SocietyAuditDistrictWiseReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyAuditDistrictWiseReport(string FinancialYear, int ActId)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocietyAuditDistrictWise(FinancialYear, ActId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        [HttpGet]
        [Obsolete] //Pradeep Saini 23-05-2024
        public JsonResult GetCountSocietyAssignedToAuditor(string FIN_YEAR_NAME, string DistrictId, string SocietyTypeId, string GroupById, string Division_Id)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();

            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());

            string WhareClause = "";
            if (SocietyTypeId != "0" && SocietyTypeId != null && SocietyTypeId != "" && SocietyTypeId != "undefined")
            {
                WhareClause = "and Societies.SocietyTypeId =" + SocietyTypeId;
            }


            if (Division_Id != "0" && Division_Id != null && Division_Id != "" && Division_Id != "undefined")
            {
                WhareClause += "and A.DIVISION_ID=" + Division_Id;
            }

            if (GroupById != "0" && GroupById != null && GroupById != "" && GroupById != "undefined")
            {
                DataSet ds = new DataSet();
                if (GroupById == "1")
                {
                    DataTable dt = objRajSewaDwaarModel.DivisionList();
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    foreach (DataRow dr in dt.Rows)
                    {
                        int DIVISION_ID = Convert.ToInt32(dr["DIVISION_ID"]);
                        string DIVISION_ENG = Convert.ToString(dr["DIVISION_ENG"]);
                        WhareClause = "and A.DIVISION_ID=" + DIVISION_ID;
                        DataTable DT1 = objAuditormaster.GetCountSocietyAssignedToAuditor(FIN_YEAR_NAME, DistrictId, WhareClause);
                        DataColumn col = DT1.Columns.Add("DIVISION_ID", typeof(Int32));
                        DataColumn col2 = DT1.Columns.Add("DIVISION_ENG", typeof(string));

                        int TotalCount = 0;
                        foreach (DataRow row in DT1.Rows)
                        {
                            TotalCount = Convert.ToInt32(row["TOTAL"]);
                            row["DIVISION_ID"] = DIVISION_ID;
                            row["DIVISION_ENG"] = DIVISION_ENG;
                        }
                        if (TotalCount > 0)
                        {
                            ds.Tables.Add(DT1);
                        }
                    }
                    ;
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
                    //}
                }
                else if (GroupById == "2")
                {
                    DataTable dt = objRajSewaDwaarModel.DistrictList(0);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int DISTRICT_ID = Convert.ToInt32(dr["DISTRICT_ID"]);
                        string DISTRICT_ENG = Convert.ToString(dr["DISTRICT_ENG"]);
                        WhareClause = "and A.DISTRICT_ID=" + DISTRICT_ID;
                        DataTable DT1 = objAuditormaster.GetCountSocietyAssignedToAuditor(FIN_YEAR_NAME, DistrictId, WhareClause);
                        DataColumn col = DT1.Columns.Add("DISTRICT_ID", typeof(Int32));
                        DataColumn col2 = DT1.Columns.Add("DISTRICT_ENG", typeof(string));

                        int TotalCount = 0;
                        foreach (DataRow row in DT1.Rows)
                        {
                            TotalCount = Convert.ToInt32(row["TOTAL"]);
                            row["DISTRICT_ID"] = DISTRICT_ID;
                            row["DISTRICT_ENG"] = DISTRICT_ENG;
                        }

                        if (TotalCount > 0)
                        {
                            ds.Tables.Add(DT1);
                        }
                    }
                    ;
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
                }
                else if (GroupById == "3")
                {
                    DataTable dt = objRajSewaDwaarModel.SocietyTypeList(3);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int SocietyType_Id = Convert.ToInt32(dr["SocietyTypeId"]);
                        string TypeameEng = Convert.ToString(dr["TypeameEng"]);
                        WhareClause = "and Societies.SocietyTypeId=" + SocietyType_Id;
                        DataTable DT1 = objAuditormaster.GetCountSocietyAssignedToAuditor(FIN_YEAR_NAME, DistrictId, WhareClause);
                        DataColumn col = DT1.Columns.Add("SocietyTypeId", typeof(Int32));
                        DataColumn col2 = DT1.Columns.Add("TypeameEng", typeof(string));

                        int TotalCount = 0;
                        foreach (DataRow row in DT1.Rows)
                        {
                            TotalCount = Convert.ToInt32(row["TOTAL"]);
                            row["SocietyTypeId"] = SocietyType_Id;
                            row["TypeameEng"] = TypeameEng;
                        }
                        if (TotalCount > 0)
                        {
                            ds.Tables.Add(DT1);
                        }
                    }
                    ;
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
                }
            }


            DataTable DT = objAuditormaster.GetCountSocietyAssignedToAuditor(FIN_YEAR_NAME, DistrictId, WhareClause);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountSocietyAssignedToAuditor_v1(string FIN_YEAR_NAME, string DistrictId, string Division_Id = "", int Month = 0, string IsEmpanelledCA = "")
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());

            DataTable DT = objAuditormaster.GetCountSocietyAssignedToAuditor_v1(FIN_YEAR_NAME, DistrictId, Division_Id, Month, IsEmpanelledCA);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        #region Proffiling Audit Status Reprt
        public ActionResult ProffilingAuditStatus()
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            ViewBag.LoggedInUserLevelId = _LoggedInUser.LevelId;
            return View();
        }


        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetProffilingAndauditStatus(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetProffilingAndauditStatus(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 04/062020  BALWANT
        #region PLDB Reports


        public ActionResult PLDBMembershipAmmountReport()
        {
            return View();
        }
        public ActionResult PLDBLoanAmmountReport()
        {
            return View();
        }


        [HttpGet]
        public string GetLoanAppReport(int PldbId, int BranchId)
        {
            MISModel objLoan = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objLoan.GetLoanApplicationListReport(PldbId, BranchId));
        }

        [HttpGet]
        public string MembershipAmmountReportdata(int PldbId = 0)
        {
            MISModel objLoan = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objLoan.GetMemberAmountReport(PldbId));
        }



        [HttpGet]
        public string PLDBLoanammountReports(int PldbId = 0)
        {
            MISModel objLoan = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objLoan.PLDBLoanAmmountReportdata(PldbId));
        }
        #endregion

        #region WR Amount Report
        [HttpGet]
        public ActionResult WRAmmountReport()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetWRAmmountReport(string Fy, string commodityid, int WharehousId, string fromdatId, string TodatId)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable dt = objModel.wramtreport(Fy, commodityid, WharehousId, fromdatId, TodatId);
            //return COMMON_FUNCTION.DataTableToJSONString(dt);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region WR Amount Report
        [HttpGet]
        public ActionResult WRReportPhysicalQuantity()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetWRPhysicalQuantityReport(string Fy, string commodityid, int WharehousId = 0, string fromdatId = "", string TodatId = "")
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable dt = objModel.GetWRPhysicalQuantityReport(Fy, commodityid, WharehousId, fromdatId, TodatId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion

        [HttpGet]
        public ActionResult AuditOrderListReport()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.LevelId = _LoggedInUser.LevelId;
                ViewBag.DivisionId = _LoggedInUser.DivisionId;
                ViewBag.DistrictId = _LoggedInUser.DistrictId;
                ViewBag.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;

                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }





            return View();
        }
        //END

        public ActionResult CooperativeSocietyReport()
        {
            return View();
        }

        [HttpPost]
        public string GetCooperativeSocietyReport(string ActId, int DIVISION_ID = 0, int IsOnline = -1, int DistrictId = 0)
        {

            MISModel objMISModel = new MISModel(new DBLogger());
            if (ActId.Length > 1)
            {
                ActId = "0";
            }
            DataSet DT = objMISModel.GetCooperativeReport(int.Parse(ActId), _LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.LevelId, DistrictId == 0 ? _LoggedInUser.DistrictId : DistrictId, _LoggedInUser.DivisionId, IsOnline);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }


        [HttpGet]
        public ActionResult SocietyAuditprogress()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.InspectionReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public JsonResult GetCountSocietyAuditprogress123(string FIN_YEAR_NAME, string DistrictId, string SocietyTypeId, string GroupById)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();

            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());
            string WhareClause = "";

            DataTable dt = objRajSewaDwaarModel.DistrictList(0);
            DataSet ds = new DataSet();

            int totalCount = 0;
            int totalAppointedCount = 0;
            int totalPartialSubmitCount = 0;
            int totalCompletedCount = 0;
            if (DistrictId != "undefined" || FIN_YEAR_NAME != "undefined")
            {
                if (DistrictId != "undefined" && DistrictId != "")
                {
                    DataTable filteredDt = new DataTable();
                    var dr = dt.AsEnumerable();
                    dr = dr.Where(row => row.Field<Int32>("DISTRICT_ID") == int.Parse(DistrictId));
                    if (dr.Any())
                    {
                        filteredDt = dr.CopyToDataTable();
                    }
                    string DISTRICT_ENG = Convert.ToString(filteredDt.Rows[0]["DISTRICT_ENG"]);
                    WhareClause = "and A.DISTRICT_ID=" + DistrictId;
                    DataTable DT = objAuditormaster.GetCountSocietyauditprogesslist(FIN_YEAR_NAME, DistrictId, WhareClause);
                    DataColumn col2 = DT.Columns.Add("DISTRICT_ENG", typeof(string));
                    DT.Rows[0]["DISTRICT_ENG"] = DISTRICT_ENG;
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
                }
                else if (FIN_YEAR_NAME != "undefined" && FIN_YEAR_NAME != "")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int DISTRICT_ID = Convert.ToInt32(dr["DISTRICT_ID"]);
                        string DISTRICT_ENG = Convert.ToString(dr["DISTRICT_ENG"]);
                        WhareClause = "and A.DISTRICT_ID=" + DISTRICT_ID;
                        DataTable DT1 = objAuditormaster.GetCountSocietyauditprogesslist(FIN_YEAR_NAME, DistrictId, WhareClause);
                        DataColumn col = DT1.Columns.Add("DISTRICT_ID", typeof(Int32));
                        DataColumn col2 = DT1.Columns.Add("DISTRICT_ENG", typeof(string));

                        DataColumn total = DT1.Columns.Add("TotalCount", typeof(string));
                        DataColumn totalAppointed = DT1.Columns.Add("TotalAppointed", typeof(Int32));
                        DataColumn totalPartialSubmit = DT1.Columns.Add("TotalPartialSubmit", typeof(Int32));
                        DataColumn totalCompleted = DT1.Columns.Add("TotalCompleted", typeof(Int32));
                        foreach (DataRow row in DT1.Rows)
                        {
                            row["DISTRICT_ID"] = DISTRICT_ID;
                            row["DISTRICT_ENG"] = DISTRICT_ENG;
                            int total_Count = Convert.ToInt32(row["TOTAL"]);
                            int totalAppointed_Count = Convert.ToInt32(row["Appointed"]);
                            int totalPartialSubmit_Count = Convert.ToInt32(row["PARTIALSUBMIT"]);
                            int totalCompleted_Count = Convert.ToInt32(row["Completed"]);

                            totalCount += total_Count;
                            totalAppointedCount += totalAppointed_Count;
                            totalPartialSubmitCount += totalPartialSubmit_Count;
                            totalCompletedCount += totalCompleted_Count;

                            row["TotalCount"] = totalCount;
                            row["TotalAppointed"] = totalAppointedCount;
                            row["TotalPartialSubmit"] = totalPartialSubmitCount;
                            row["TotalCompleted"] = totalCompletedCount;

                        }
                        ds.Tables.Add(DT1);
                    }
                    ;

                }
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //int totalCount = 0;
                //int totalAppointedCount = 0;
                //int totalPartialSubmitCount = 0;
                //int totalCompletedCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int DISTRICT_ID = Convert.ToInt32(dr["DISTRICT_ID"]);
                    string DISTRICT_ENG = Convert.ToString(dr["DISTRICT_ENG"]);
                    WhareClause = "and A.DISTRICT_ID=" + DISTRICT_ID;
                    DataTable DT1 = objAuditormaster.GetCountSocietyauditprogesslist(FIN_YEAR_NAME, DistrictId, WhareClause);
                    DataColumn col = DT1.Columns.Add("DISTRICT_ID", typeof(Int32));
                    DataColumn col2 = DT1.Columns.Add("DISTRICT_ENG", typeof(string));

                    DataColumn total = DT1.Columns.Add("TotalCount", typeof(string));
                    DataColumn totalAppointed = DT1.Columns.Add("TotalAppointed", typeof(Int32));
                    DataColumn totalPartialSubmit = DT1.Columns.Add("TotalPartialSubmit", typeof(Int32));
                    DataColumn totalCompleted = DT1.Columns.Add("TotalCompleted", typeof(Int32));
                    foreach (DataRow row in DT1.Rows)
                    {
                        row["DISTRICT_ID"] = DISTRICT_ID;
                        row["DISTRICT_ENG"] = DISTRICT_ENG;
                        int total_Count = Convert.ToInt32(row["TOTAL"]);
                        int totalAppointed_Count = Convert.ToInt32(row["Appointed"]);
                        int totalPartialSubmit_Count = Convert.ToInt32(row["PARTIALSUBMIT"]);
                        int totalCompleted_Count = Convert.ToInt32(row["Completed"]);

                        totalCount += total_Count;
                        totalAppointedCount += totalAppointed_Count;
                        totalPartialSubmitCount += totalPartialSubmit_Count;
                        totalCompletedCount += totalCompleted_Count;

                        row["TotalCount"] = totalCount;
                        row["TotalAppointed"] = totalAppointedCount;
                        row["TotalPartialSubmit"] = totalPartialSubmitCount;
                        row["TotalCompleted"] = totalCompletedCount;

                    }

                    ds.Tables.Add(DT1);
                }
                ;

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SocietyRegistrationTimeSheet()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyRegistrationTimeSheet(DataPagingModel Details, int type)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocietyRegistrationTimeSheet(ref Details, type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SocietyAuditMPR()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAuditMPRMonthWise(DataPagingModel Details, int type)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetAuditMPRMonthWise(ref Details, type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetActionIdCCSComplaintBydisteict")]
        public string GetCCSComplaintBydisteict()
        {
            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModel.GetActionIdCCSComplaintReportCountByDistrict());
        }


        public ActionResult SocietyAuditWorkSheetReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAuditWorkSheetReport(DataPagingModel Details, int type)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetAuditWorkSheetReport(ref Details, type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PLDBLoanApplicationReport()
        //{
        //    return View();
        //}


        public ActionResult PLDBLoanApplicationReport()
        {

            if (_LoggedInUser.PLDBId != 0)
            {
                string PLDBID = _LoggedInUser.PLDBId.ToString();
                ViewBag.PLDBID = PLDBID;
            }
            if (_LoggedInUser.PLDBBrancchId != 0)
            {
                string BranchPLDBID = _LoggedInUser.PLDBBrancchId.ToString();
                ViewBag.BranchPLDBID = BranchPLDBID;
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetPLDBappLoanReport(DataPagingModel Details, string ReportType)
        {

            string SSO_ID = _LoggedInUser.SSOID.ToString();
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetPLDBappLoanReportList(ref Details, ReportType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetPLDBLoanListbycount(DataPagingModel Details, string ReportType, string Value)
        {
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetPLDBLoanListbycount(ref Details, ReportType, Value);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PLDBMemberShipReport()
        {
            if (_LoggedInUser.PLDBId != 0)
            {
                string PLDBID = _LoggedInUser.PLDBId.ToString();
                ViewBag.PLDBID = PLDBID;
            }
            if (_LoggedInUser.PLDBBrancchId != 0)
            {
                string BranchPLDBID = _LoggedInUser.PLDBBrancchId.ToString();
                ViewBag.BranchPLDBID = BranchPLDBID;
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetPLDBMemberShipReport(DataPagingModel Details, string ReportType)
        {
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetPLDBMemberShipReport(ref Details, ReportType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetPLDBMemberShipReportbycount(DataPagingModel Details, string ReportType, string Value)
        {
            MISModel objNew = new MISModel(new DBLogger());
            DataTable DT = objNew.GetPLDBMemberShipReportbycount(ref Details, ReportType, Value);

            foreach (var item in DT.AsEnumerable())
            {
                item.SetField("RequestIdEnc", COMMON_FUNCTION.Encrypt(Convert.ToString(item["RequestId"])));
                item.SetField("RequestId", -1);
            }

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        //--Export Pdf Generated--start


        [HttpPost]
        public JsonResult GetPLDBMembershipReportExportEn(DataPagingModel Details, string ReportType, string Value)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                DataTable DT = objNew.GetPLDBMemberShipReportbycount(ref Details, ReportType, Value);
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
                //OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<html><body>");
                //sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                //sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                //sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>PLDB Membership Report ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'> ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''>");
                //sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");


                //sb.Append("<tr>");
                //sb.Append("<th style='text-align:center'>Sr.No. </th>");
                //sb.Append("<th style='text-align:center'>District </th>");
                //sb.Append("<th style='text-align:center'>Name </th>");
                //sb.Append("<th style='text-align:center'>Mobile No. </th>");
                //sb.Append("<th style='text-align:center'>Aadhar No. </th>");
                //sb.Append("<th style='text-align:center'>MembershipNo</th>");
                //sb.Append("</tr>");
                //int index = 1;

                //foreach (DataRow dr in List.Rows)
                //{

                //    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["Name"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MobileNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["AADHARNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MembershipNo"].ToString() + " </td></tr>");


                //}
                //sb.Append("</table>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("</table>");
                //sb.Append("</div>");
                //sb.Append("</div>");
                //sb.Append("</body></html>");
                //ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //DataTable DT = new DataTable();
                //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetPLDBMembershipReportExportEn(DataPagingModel Details, string ReportType, string Value)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpPost]
        public JsonResult GetPLDBMembershipReportExportHi(DataPagingModel Details, string ReportType, string Value)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                // DataTable List = objNew.GetWRReport(ref Details, ReportType);
                DataTable DT = objNew.GetPLDBMemberShipReportbycount(ref Details, ReportType, Value);
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
                //OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<html><body>");
                //sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                //sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                //sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>PLDB Membership Report ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'> ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''>");
                //sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");


                //sb.Append("<tr>");
                //sb.Append("<th style='text-align:center'>Sr.No. </th>");
                //sb.Append("<th style='text-align:center'>District </th>");
                //sb.Append("<th style='text-align:center'>Name </th>");
                //sb.Append("<th style='text-align:center'>Mobile No. </th>");
                //sb.Append("<th style='text-align:center'>Aadhar No. </th>");
                //sb.Append("<th style='text-align:center'>MembershipNo</th>");
                //sb.Append("</tr>");
                //int index = 1;

                //foreach (DataRow dr in List.Rows)
                //{

                //    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["Name"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MobileNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["AADHARNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MembershipNo"].ToString() + " </td></tr>");


                //}
                //sb.Append("</table>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("</table>");
                //sb.Append("</div>");
                //sb.Append("</div>");
                //sb.Append("</body></html>");
                //ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //DataTable DT = new DataTable();
                //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = "GetPLDBMembershipReportExportHi(DataPagingModel Details, string ReportType, string Value)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpPost]
        public JsonResult GetPLDBLoanAppReportExportEn(DataPagingModel Details, string ReportType, string Value)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());

                // DataTable List = objNew.GetWRReport(ref Details, ReportType);
                DataTable DT = objNew.GetPLDBLoanListbycount(ref Details, ReportType, Value);
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);



                //// DataTable List = objNew.GetWRReport(ref Details, ReportType);
                //DataTable List = objNew.GetPLDBLoanListbycount(ref Details, ReportType, Value);
                //OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<html><body>");
                //sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                //sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                //sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>PLDB Loan Application Report ");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'> ");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''>");
                //sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");


                //sb.Append("<tr>");
                //sb.Append("<th style='text-align:center'>Sr.No. </th>");
                //sb.Append("<th style='text-align:center'>Application No. </th>");
                //sb.Append("<th style='text-align:center'>Name </th>");
                //sb.Append("<th style='text-align:center'>Mobile No. </th>");
                //sb.Append("<th style='text-align:center'>SSOID </th>");
                //sb.Append("<th style='text-align:center'>MembershipNo</th>");
                //sb.Append("<th style='text-align:center'>DateTime</th>");
                //sb.Append("</tr>");
                //int index = 1;

                //foreach (DataRow dr in List.Rows)
                //{

                //    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["application_no"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["OfficerNameEn"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MobileNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["SSO_ID"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MembershipNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["created_date"].ToString() + " </td></tr>");


                //}
                //sb.Append("</table>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("</table>");
                //sb.Append("</div>");
                //sb.Append("</div>");
                //sb.Append("</body></html>");
                //ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //DataTable DT = new DataTable();
                //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = " GetPLDBLoanAppReportExportEn(DataPagingModel Details, string ReportType, string Value)" }, ex);
                result = "Error";
            }
            return null;
        }



        [HttpPost]
        public JsonResult GetPLDBLoanAppReportExportHi(DataPagingModel Details, string ReportType, string Value)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;
                MISModel objNew = new MISModel(new DBLogger());

                // DataTable List = objNew.GetWRReport(ref Details, ReportType);
                DataTable DT = objNew.GetPLDBLoanListbycount(ref Details, ReportType, Value);
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

                //MISModel objNew = new MISModel(new DBLogger());
                //// DataTable List = objNew.GetWRReport(ref Details, ReportType);
                //DataTable List = objNew.GetPLDBLoanListbycount(ref Details, ReportType, Value);
                //OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<html><body>");
                //sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                //sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                //sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>PLDB Membership Report ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'> ");
                //sb.Append("  </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''>");
                //sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                //sb.Append("<tr>");
                //sb.Append("<th style='text-align:center'>Sr.No. </th>");
                //sb.Append("<th style='text-align:center'>Application No. </th>");
                //sb.Append("<th style='text-align:center'>Name </th>");
                //sb.Append("<th style='text-align:center'>Mobile No. </th>");
                //sb.Append("<th style='text-align:center'>Created By </th>");
                //sb.Append("<th style='text-align:center'>SSOID </th>");
                //sb.Append("<th style='text-align:center'>MembershipNo</th>");
                //sb.Append("<th style='text-align:center'>DateTime</th>");
                //sb.Append("</tr>");
                //int index = 1;

                //foreach (DataRow dr in List.Rows)
                //{

                //    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["application_no"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["Name"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MobileNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["OfficerNameEn"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["SSO_ID"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["MembershipNo"].ToString() + " </td>");
                //    sb.Append("<td  style='text-align:left'> " + dr["created_date"].ToString() + " </td></tr>");


                //}
                //sb.Append("</table>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("</table>");
                //sb.Append("</div>");
                //sb.Append("</div>");
                //sb.Append("</body></html>");
                //ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //DataTable DT = new DataTable();
                //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.MIS", FunctionName = "GetPLDBLoanAppReportExportHi(DataPagingModel Details, string ReportType, string Value)" }, ex);
                result = "Error";
            }
            return null;
        }


        [HttpPost]
        public JsonResult RSWCMIS(List<Params> parameters)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GetRSWC_MISReportData(COMMON_FUNCTION.WhereClauseByParams(parameters));
            //return Json(DT);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }



        [HttpGet]
        public JsonResult GetCountSocietyAuditprogress(string FIN_YEAR_NAME, string DistrictId, string SocietyTypeId, string GroupById)
        {

            string WhareClause = "";
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            DataTable DT1 = objAuditormaster.GetCountSocietyauditprogesslist(FIN_YEAR_NAME, DistrictId, WhareClause);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT1) }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public string GetSocietyDistWiseData(string ActId, int SocietyTypeId, int DIVISION_ID = 0, int IsOnline = -1, int DistrictId = 0, string Status = "", string FromDate = "", string ToDate = "", string FinancialYear = "")
        {

            MISModel objMISModel = new MISModel(new DBLogger());
            //DataSet DT = objMISModel.GetSocietyDistWiseData(ActId, SocietyTypeId, CreatetionDate, LevelId, _LoggedInUser.DistrictId, DIVISION_ID, IsOnline);
            if (ActId.Length > 1)
            {
                ActId = "0";
            }
            if (Status.Contains("1958"))
            {
                ActId = "1";
            }
            if (Status.Contains("2001"))
            {
                ActId = "3";
            }
            if (Status.Contains("2005"))
            {
                ActId = "7";
            }
            if (Status.Contains("Others"))
            {
                ActId = "6";
            }

            if (Status.Contains("APEX"))
            {
                SocietyTypeId = 9;
            }

            if (Status.Contains("CENTRAL"))
            {
                SocietyTypeId = 8;
            }

            if (Status.Contains("GAU"))
            {
                SocietyTypeId = 1;
            }
            if (Status.Contains("DAIRY") && Status.Contains("PRIMARY"))
            {
                SocietyTypeId = 10;
            }
            if (Status.Contains("PRIMARY") && !Status.Contains("DAIRY"))
            {
                SocietyTypeId = 7;
            }

            if (Status.Contains("SCHOOL"))
            {
                SocietyTypeId = 5;
            }
            if (Status == "SOCIETIES UNDER RAJIV GANDHI PEYAJAL MISSION")
            {
                SocietyTypeId = 4;
            }

            DataSet DT = objMISModel.GetSocietyDistWiseData(int.Parse(ActId), SocietyTypeId, _LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.LevelId, DistrictId == 0 ? _LoggedInUser.DistrictId : DistrictId, _LoggedInUser.DivisionId, IsOnline, FromDate, ToDate, FinancialYear);

            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);

        }
        [HttpGet]
        public ActionResult OldSocietyProflingReport()
        {
            return View();
        }

        public ActionResult SocietyApplicationTransactionList()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetSocietyPaymentTransectin(DataPagingModel Details, string SocietyIdENC = "")
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocietyPaymentTransectin(ref Details, SocietyIdENC);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CCSComplaintStatusReport()
        {
            return View();
        }

        [HttpGet]
        public string GetCCSComplaintStatusReport(int DistrictId, int FinYear = 0)
        {

            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModel.GetCCSComplaintStatusReport(DistrictId, FinYear));
        }

        public ActionResult housingsociety()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetHousingSociety(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetHousingSociety(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetHousingSocietyAudtiReport(string Id)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetHousingSocietyAudtiReport(Id);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult UpdateRegiDistrictWise()
        {
            return View();
        }
        [HttpGet]
        public string GetDistrictWiseRegis()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetSocietyDistrictWise();
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }


        [HttpGet]
        public ActionResult SocietyReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyReport(DataPagingModel Details)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocietyReport(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AmendmentReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAmendmentReport(DataPagingModel Details, int DistrictId)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetAmendmentReport(ref Details, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAmendmentDetailsReport(DataPagingModel Details, int DistrictId, int ListType)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetAmendmentDetailsReport(ref Details, DistrictId, ListType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GodownReportStatusWise()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetGodownReportStatusWise()
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GodownFilterData("GODOWN_STATUSWISE", _LoggedInUser.DistrictId);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }

        [HttpPost]
        public JsonResult GetGodownReportStatusDetails(int StatusId)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GodownFilterData("GODOWN_DETAILSSTATUSWISE", _LoggedInUser.DistrictId, StatusId);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }
        #region Mukesh

        [HttpGet]
        public ActionResult WomanSavingCreditSocietyRPT()
        {
            return View();
        }

        [HttpGet]
        public ActionResult WomanInformationSocietyRPT()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CoopDairyFederationMPRReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CoopPressMPRReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult YearTradeSchemeMPRReport()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CoopConsumerMPRReport()
        {
            return View();
        }
        #endregion


        #region Amit Kumar Cooperative management tabulator Report
        public ActionResult SocietyDivisionwiseMPR()
        {
            return View();
        }



        [HttpPost]
        [ActionName("GetSocietydivisionwiseMPR")]
        public JsonResult GetSocietydivisionwiseMPR(string Dateval, int DIVISION_ID)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GetSocietydivisionwiseMPR(Dateval, DIVISION_ID);
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMISModel.GetSocietydivisionwiseMPR(Dateval, DIVISION_ID));
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SocietyDistrictWiseMPR()
        {
            return View();
        }
        [HttpPost]
        [ActionName("GetSocietyDistrictwiseMPR")]
        public JsonResult GetSocietyDistrictwiseMPR(string Dateval, int District)
        {


            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = objMISModel.GetSocietyDistrictwiseMPR(Dateval, District);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMISModel.GetSocietyDistrictwiseMPR(Dateval, District));
        }
        public ActionResult SocietyDivisionDistrictWiseMPR()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyDivisionDistrictwiseMPR(string Type, int DIVISION_ID = 0, int DistrictId = 0, string Dateval = "")
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetSocietyDivisionDistrictwiseMPR(DIVISION_ID, DistrictId, Dateval, Type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetDIVISIONList")]
        public string GetDIVISIONList(string Dateval = "")
        {
            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objModel.GetDIVISIONList(Dateval));
        }
        public ActionResult CooperativeMovementInRJ()
        {
            return View();
        }
        [HttpPost]
        [ActionName("GetCooperativeMovement")]
        public string GetCooperativeMovement()
        {
            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objModel.GetCooperativeMovement());
        }
        [HttpPost]
        [ActionName("InsertCoop")]
        public JsonResult InsertCoop(List<CooperativeMovementRJ> obj)
        {
            MISModel objModel = new MISModel(new DBLogger());
            return Json(objModel.InsertCoop(obj));
            //return objModel.InsertCoop(obj).ToString();
        }
        #endregion
        public ActionResult SocietyLiquidationReport()
        {
            return View();
        }
        [HttpPost]
        [ActionName("GetLiquidationData1")]
        public JsonResult GetLiquidationData(int DistrictId, string Month)
        {
            DataTable DT = null;
            SocietyModel objEntryForms = new SocietyModel(new DBLogger());
            DT = objEntryForms.LiquidationMprDataList(DistrictId, Month);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ActionName("GetWomanSavingCreditSociety")]
        public JsonResult GetWomanSavingCreditSociety(DataPagingModel Details, int DistrictId, string Month)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataTable DT = objModel.GetWomanSavingCreditSociety(ref Details, DistrictId, Month);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("GetWomenInformationSociety")]
        public string GetWomenInformationSociety(int DistrictId, string Month)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMISModel.GetWomenInformationSociety(DistrictId, Month));
        }
        [HttpPost]
        [ActionName("GetWomenInformationSocietyDetails")]
        public string GetWomenInformationSocietyDetails(int ClassId, string Whereclouse = "")
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMISModel.GetWomenInformationSocietyDetails(ClassId, Whereclouse));
        }

        public ActionResult LokGuaranteeServiceP2()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetLokGuaranteeService")]
        public JsonResult GetLokGuaranteeService(string Month = "", string FromMonth = "", string ToMonth = "")
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetLokGuarenteeService(Month, FromMonth, ToMonth);
            // return COMMON_FUNCTION.DataTableToJSONString(DT);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LokGuaranteeServiceWiseRpt()
        {
            return View();
        }

        public ActionResult CooperativeBankMprRpt()
        {
            return View();
        }



        [HttpPost]
        [ActionName("GetCoopBankMprData1")]
        public string GetCoopBankMprData(int DistrictId, string Month)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.CoopBankMprDataList(DistrictId, Month);
            return COMMON_FUNCTION.DataTableToJSONString(DT);



        }
        [HttpGet]
        public ActionResult CooperateMovementZoneWiseRpt1()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DistrictwiseAdvancementLoanRpt()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DistrictwiseAdvancementLoanMprRpt()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.RoleId = _LoggedInUser.RoleId;
            return View();
        }

        //[HttpPost]
        //[ActionName("GetLoanData1")]
        //public string GetLoanData(string year)
        //{
        //    DataTable DT = null;
        //    MISModel objEntryForms = new MISModel(new DBLogger());
        //    DT = objEntryForms.LoanMprDataList(year);
        //    return COMMON_FUNCTION.DataTableToJSONString(DT);



        //}
        [HttpPost]
        [ActionName("GetLoanData1")]
        public string GetLoanData(string FYear)
        {

            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.LoanMprDataList();
            return COMMON_FUNCTION.DataTableToJSONString(DT);



        }

        [HttpPost]
        [ActionName("SaveLoanFormsData")]
        public JsonResult SaveLoanFormsData(LoanFormDataEntity _Model)
        {
            int result = -1;
            if (_Model != null)
            {



                MISModel obj = new MISModel();
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                //_Model.CreatedBy = _loggedInUSerDetails.UserId;
                // _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();



                result = obj.LoanRegistration(_Model);



            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetCoopMovmentZoneWiseRptData1")]
        public string GetCoopMovmentZoneWiseRptData(string year = "")
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.GetMovementZoneWiseMPRRpt(year);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult AuditProgressReportBySociety()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AuditProgressReportBySociety")]
        public string AuditProgressReportBySociety(string year, string date)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.AuditProgressReportBySociety(year, date);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        public ActionResult AuditProgressReportByDepartment()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AuditProgressReportByDepartment")]
        public string AuditProgressReportByDepartment(string year, string date)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.AuditProgressReportByDepartment(year, date);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public ActionResult AuditFeesReportPrimaryDairy()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AuditFeesReportPrimaryDairy")]
        public string AuditFeesReportPrimaryDairy(string year, string date, int CategoryId)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.AuditFeesReportPrimaryDairy(year, date, CategoryId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("GetSocietyCategory")]
        public string GetSocietyCategory()
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.GetSocietyCategory();
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
        [HttpPost]
        [ActionName("GetCateWiseReportaudit")]
        public string GetCateWiseReportaudit(int Id = 0, string YearMonth = "")
        {
            DataSet DS = null; int year = 0; int month = 0;
            if (YearMonth != "")
            {
                year = Convert.ToInt32(YearMonth.Split('-')[0]);
                month = Convert.ToInt32(YearMonth.Split('-')[1]);
            }
            MISModel objEntryForms = new MISModel(new DBLogger());
            DS = objEntryForms.GetCateWiseReportauditMPR(Id, year, month);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS);
        }
        public ActionResult CategoryWiseBlockAuditRpt()
        {
            return View();
        }

        public ActionResult CategoryWiseBlockAuditRptbacklog()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AuditProgressBlockWiseRpt()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AuditProgressBlockWiseBackLogRpt()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetAuditData1")]
        public string GetAuditData(int Id = 0, string Month = "")
        {
            DataTable DT = null;
            //int year = 0; int month = 0;

            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.AuditDataList(Id, Month);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpGet]
        public ActionResult GodownProgressiveReport()
        {

            return View();
        }

        [HttpPost]
        [ActionName("Godownlists")]
        public string Godownlists()
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.getGodownlist();
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        public ActionResult SocietyDetails_MPR_RPT()
        {
            return View();
        }

        // Bhavesh Singh 09/03/2026
        public ActionResult AadhaarServiceLogReport()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetDistrictByDivison")]
        public string GetDistrictByDivison(int DivisonId)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetDistrictByDivison(DivisonId);
            // return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        [ActionName("GetSocietiesDetails")]
        public string GetSocietiesDetails(int DivisonId, int DistrictId, int blockId, int CityId, int Gp, int villageId, int ActId, int RuralUrban)
        {
            DataTable DT = null;
            MISModel objSocietyDetails = new MISModel(new DBLogger());
            DT = objSocietyDetails.SocietiesDetails(DivisonId, DistrictId, blockId, CityId, Gp, villageId, ActId, RuralUrban);
            // return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        public ActionResult MPRCoopMovementRaj()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MPRStateCoopBank()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MPRMainCoopSocieties()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.RoleId = _LoggedInUser.RoleId;
            return View();
        }
        [HttpGet]
        public ActionResult MPRPrimaryAgriCreditSocieties()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.RoleId = _LoggedInUser.RoleId;
            return View();
        }



        #region Madhuri

        [HttpPost]
        [ActionName("SaveCoopMovFormsData")]
        public JsonResult SaveCoopMovFormsData(CoopMovRj _Model)
        {
            int result = -1;
            if (_Model != null)
            {

                MISModel objEntryForms = new MISModel(new DBLogger());
                // ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                //  _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                //  _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();

                result = objEntryForms.SaveCoopMovFormsData(_Model);


            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetCoopMovFormsData")]
        public string GetCoopMovFormsData(int DistrictId, string Year)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetCoopMovFormsData(DistrictId, Year);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }



        [HttpPost]
        [ActionName("SaveCoopState")]
        public JsonResult SaveCoopState(CoopASt _Model)
        {
            int result = -1;
            if (_Model != null)
            {

                MISModel objEntryForms = new MISModel(new DBLogger());
                // ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                //  _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                //  _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();

                result = objEntryForms.SaveCoopState(_Model);


            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetCoopStateData")]
        public string GetCoopStateData(int DistrictId, string Year)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetCoopStateData(DistrictId, Year);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        //MainMprSocieties(7.3)


        [HttpPost]
        [ActionName("SaveMainCoopSocieties")]
        public JsonResult SaveMainCoopSocieties(MainCooperativeSocieties _Model)
        {
            int result = -1;
            if (_Model != null)
            {

                MISModel objEntryForms = new MISModel(new DBLogger());
                // ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                //  _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                //  _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();

                result = objEntryForms.SaveMainCoopSocieties(_Model);


            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetMainCoopSocietyData")]
        public string GetMainCoopSocietyData(int DistrictId, string Year)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetMainCoopSocietyData(DistrictId, Year);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }



        //Primary Agriculture Credit Societies(7.4)


        [HttpPost]
        [ActionName("SavePrimaryAgriSocieties")]
        public JsonResult SavePrimaryAgriSocieties(PrimaryAgricultureCreditSocieties _Model)
        {
            int result = -1;
            if (_Model != null)
            {

                MISModel objEntryForms = new MISModel(new DBLogger());
                // ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                //  _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                //  _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();

                result = objEntryForms.SavePrimaryAgriSocieties(_Model);


            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetPrimaryAgriSocieties")]
        public string GetPrimaryAgriSocieties(int DistrictId, string Year)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            DT = objEntryForms.GetPrimaryAgriSocieties(DistrictId, Year);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }





        [HttpGet]
        public ActionResult CooperativeMovementAdvancementRpt()
        {
            return View();
        }



        [HttpPost]
        [ActionName("CooperativeMovementRpt")]
        public string CooperativeMovementRpt(string FYear)
        {
            DataTable DT = null;
            MISModel objEntryForms = new MISModel(new DBLogger());
            // DT = objEntryForms.LiquidationMprDataList();
            DT = objEntryForms.CooperativeMovementRpt(FYear);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }






        #endregion

        //Bhavesh Singh 25/03/2026
        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GetCCPGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool Overall = false, bool DistrictWise = false, int blockId = 0, DateTime? FromDate = null, DateTime? ToDate = null, int Type = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetCCPGccSummary(financialYearId, districtId, CCBId, BranchId, PacsId, Overall, DistrictWise, blockId, FromDate, ToDate, "", Type);
            if (ds != null)
            {
                responce.Data = new
                {
                    GccSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        #region 'Gcc Report'
        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GetGccSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool Overall = false, bool DistrictWise = false, int blockId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetGccSummary(financialYearId, districtId, CCBId, BranchId, PacsId, Overall, DistrictWise, blockId);
            if (ds != null)
            {
                responce.Data = new
                {
                    GccSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        //Replica of GetGccSummary
        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GetGccSummaryList(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool Overall = false, bool DistrictWise = false, int blockId = 0, int ApplicationType = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());
            //var adhar =  COMMON_FUNCTION.Encrypt("776235936169");
            var responce = new APIResponce();

            var ds = mISModel.GetGccSummaryList(financialYearId, districtId, CCBId, BranchId, PacsId, Overall, DistrictWise, blockId, ApplicationType);
            if (ds != null)
            {
                responce.Data = new
                {
                    GccSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        #region 'Gcc Re Apply Report'
        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GetGccRenewSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool Overall = false, bool DistrictWise = false, int blockId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetGccRenewSummary(financialYearId, districtId, CCBId, BranchId, PacsId, Overall, DistrictWise, blockId);
            if (ds != null)
            {
                responce.Data = new
                {
                    GccRenewSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccRenewSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        //[OutputCache(Duration = 3600]
        public string GetGccApplicationList(DataPagingModel Details, int financialYearId = 0, int districtId = 0, int ccbId = 0, int branchId = 0, int pacsId = 0, int statusType = 0, bool isExport = false, bool IsOverall = false, int blockId = 0, int gpId = 0, bool DistrictWise = false)
        {
            var responce = new APIResponce();
            var mis = new MISModel(new DBLogger());
            var ds = mis.GetGccApplicationList(ref Details, financialYearId, districtId, ccbId, branchId, pacsId, statusType, isExport, IsOverall, blockId, gpId, DistrictWise);
            DataTable DT = ds.Tables[0];
            if (isExport)
            {
                if (DT.Columns.Contains("TotalRecords"))
                    DT.Columns.Remove("TotalRecords");
                if (DT.Columns.Contains("GopalCreditCardId"))
                    DT.Columns.Remove("GopalCreditCardId");
                if (DT.Columns.Contains("Gender"))
                    DT.Columns.Remove("Gender");
                if (DT.Columns.Contains("MobileNo"))
                    DT.Columns.Remove("MobileNo");
                if (DT.Columns.Contains("FatherName_HI"))
                    DT.Columns.Remove("FatherName_HI");
                if (DT.Columns.Contains("FatherName_ENG"))
                    DT.Columns.Remove("FatherName_ENG");
                if (DT.Columns.Contains("encGccid"))
                    DT.Columns.Remove("encGccid");
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(DT, "Gopal-Credit-Card-Application-List.xlsx");
                return filename;
            }


            if (DT != null)
            {
                if (DT != null && DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Encrypted_RegistrationNo");
                    DT.Columns.Add("Encrypted_GopalCreditCardId");
                }
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["Encrypted_RegistrationNo"] = !String.IsNullOrEmpty(DT.Rows[i]["RegistrationNo"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["RegistrationNo"].ToString()) : "";
                    DT.Rows[i]["Encrypted_GopalCreditCardId"] = !String.IsNullOrEmpty(DT.Rows[i]["GopalCreditCardId"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["GopalCreditCardId"].ToString()) : "";
                }

            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GetGccMahilaNidhiSummary(int financialYearId = 0, int districtId = 0, int CCBId = 0, int BranchId = 0, int PacsId = 0, bool Overall = false, bool DistrictWise = false, int blockId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetGccMahilaNidhiSummary(financialYearId, districtId, CCBId, BranchId, PacsId, Overall, DistrictWise, blockId);
            if (ds != null)
            {
                responce.Data = new
                {
                    GccSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        //[OutputCache(Duration = 3600]
        public string GetGccMahilaNidhiApplicationList(DataPagingModel Details, int financialYearId = 0, int districtId = 0, int ccbId = 0, int branchId = 0, int pacsId = 0, int statusType = 0, bool isExport = false, bool IsOverall = false, int blockId = 0, int gpId = 0, bool DistrictWise = false)
        {
            var responce = new APIResponce();
            var mis = new MISModel(new DBLogger());
            var ds = mis.GetGccMahilaNidhiApplicationList(ref Details, financialYearId, districtId, ccbId, branchId, pacsId, statusType, isExport, IsOverall, blockId, gpId, DistrictWise);
            DataTable DT = ds.Tables[0];
            if (isExport)
            {
                if (DT.Columns.Contains("TotalRecords"))
                    DT.Columns.Remove("TotalRecords");
                if (DT.Columns.Contains("GopalCreditCardId"))
                    DT.Columns.Remove("GopalCreditCardId");
                if (DT.Columns.Contains("Gender"))
                    DT.Columns.Remove("Gender");
                if (DT.Columns.Contains("MobileNo"))
                    DT.Columns.Remove("MobileNo");
                if (DT.Columns.Contains("FatherName_HI"))
                    DT.Columns.Remove("FatherName_HI");
                if (DT.Columns.Contains("FatherName_ENG"))
                    DT.Columns.Remove("FatherName_ENG");
                if (DT.Columns.Contains("encGccid"))
                    DT.Columns.Remove("encGccid");
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(DT, "Gopal-Credit-Card-Application-List.xlsx");
                return filename;
            }


            if (DT != null)
            {
                if (DT != null && DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Encrypted_RegistrationNo");
                    DT.Columns.Add("Encrypted_GopalCreditCardId");
                }
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["Encrypted_RegistrationNo"] = !String.IsNullOrEmpty(DT.Rows[i]["RegistrationNo"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["RegistrationNo"].ToString()) : "";
                    DT.Rows[i]["Encrypted_GopalCreditCardId"] = !String.IsNullOrEmpty(DT.Rows[i]["GopalCreditCardId"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["GopalCreditCardId"].ToString()) : "";
                }

            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        #endregion

        #region BeneficiaryList


        [HttpGet]
        [OutputCache(Duration = 3600 * 24)]
        public JsonResult GetBeneficiaryList(int districtId = 0)
        {
            var responce = new APIResponce();
            var BeneficiaryList = new MISModel(new FileLogger());
            var ds = BeneficiaryList.GetBeneficiaryList(districtId);
            if (ds != null)
            {
                responce.Data = new
                {
                    BeneficiaryList = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalBeneficiaryList = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                    // Quadrimester = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [OutputCache(Duration = 3600 * 24)]
        public string GetBeneficiaryDetailList(DataPagingModel Details, int districtId = 0, bool isExport = false)
        {
            var responce = new APIResponce();
            var BeneficiaryList = new MISModel(new FileLogger());
            var ds = BeneficiaryList.GetBeneficiaryDetailList(ref Details, districtId, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");

                if (dt.Columns.Contains("DistrictName"))
                    dt.Columns.Remove("DistrictName");



                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "BeneficiaryDetailList.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }
        #endregion

        #region Monthly Activity
        [HttpGet]
        public ActionResult MonthlyActivityForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MonthlyActivityReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveMonthlyActivity(EventActivityHistoryModel eventActivity)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var b = new EventActivityHistoryModel(new FileLogger());
            eventActivity.CreatedBy = _LoggedInUser.UserId;
            var result = b.SaveEventActivityHistory(eventActivity);
            if (result)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMonthlyActivityReport(DataPagingModel Details)
        {
            var responce = new APIResponce();
            var eventActivity = new EventActivityHistoryModel(new FileLogger());
            var result = eventActivity.GetEventActivityHistory(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(result.Tables[0]), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region pldb
        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetPldbDistrictWiseSummary(int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "", string StatusType = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetPldbDistrictWiseSummary(Type, financialYearId, districtId, PLDB_ID, PLDB_Branch_ID, StatusType);
            if (ds != null)
            {
                responce.Data = new
                {
                    GetPldbSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGetPldbSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpPost]
        public JsonResult GetPldbCallSummary(DataPagingModel Details, int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "", string StatusType = "", bool isExport = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetPldbCallSummary(Details, Type, financialYearId, districtId, PLDB_ID, PLDB_Branch_ID, StatusType, isExport);
            DataTable dt = new DataTable();
            dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            if (isExport)
            {
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");
                if (dt.Columns.Contains("BranchName_En"))
                    dt.Columns["BranchName_En"].ColumnName = "Branch Name";
                if (dt.Columns.Contains("LoaneePart"))
                    dt.Columns["LoaneePart"].ColumnName = "25% of LoaneePart";

                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(dt, "pldbCallCenterReport.xlsx");
                return Json(new { filename = filename });
            }


            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetPldbTamilSelfieSummary(int financialYearId = 0, int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "", string StatusType = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GetPldbTamilSelfieSummary(Type, financialYearId, districtId, PLDB_ID, PLDB_Branch_ID, StatusType);
            if (ds != null)
            {
                responce.Data = new
                {
                    GetPldbSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGetPldbSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region OTS Repayment Report Added By Narpat Kumawat on 16-05-2025
        /// <summary>
        /// OTS Repayment Report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OTS_Repayment_Summary()
        {
            return View();
        }

        /// <summary>
        /// GetPldbotsSummary
        /// </summary>
        /// <returns></returns>
        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetPldbOtsSummary(int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();
            var ds = mISModel.GetPldbOtsSummary(districtId, PLDB_ID, PLDB_Branch_ID, Type);
            if (ds != null)
            {
                responce.Data = new
                {
                    GetPldbOtsSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    GetPldbTotalAmount = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPldbOtsSummaryDetails by OtsApplicationId
        /// </summary>
        /// <param name="OtsApplicationId"></param>
        /// <returns></returns>
        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetPldbOtsSummaryDetail(int OtsId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();
            var ds = mISModel.GetPldbOtsDetailsByOtsId(OtsId);
            if (ds != null)
            {
                responce.Data = new
                {
                    GetPldbOtsSummaryDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    OtsTotalAmountSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OTS_Flow_Statistics()
        {
            return View();
        }

        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetPldbOtsSchemaSummaryDetails(int districtId = 0, int PLDB_ID = 0, int PLDB_Branch_ID = 0, string Type = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();
            var ds = mISModel.GetPldbOtsSchemaDetails(districtId, PLDB_ID, PLDB_Branch_ID, Type);
            if (ds != null)
            {
                responce.Data = new
                {
                    GetPldbOtsSchemaSummary = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region LoaneePRDetails on 12-06-2025
        [HttpGet]
        public ActionResult LoaneePRDetails()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoaneeFRDetails()
        {
            return View();
        }

        public ActionResult Loanee_PR_Receipt()
        {
            return View();
        }

        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetLoaneeReceiptDetails(string FilterDate = "", string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            if (_LoggedInUser.PLDBId > 0)
                pldbId = _LoggedInUser.PLDBId;

            MISModel mISModel = new MISModel(new DBLogger());
            var responce = new APIResponce();
            var ds = mISModel.GetLoaneePRReceiptDetails(FilterDate, actionType, districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    LoaneeReceiptDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetLoaneeFinalReceiptDetails(string FilterDate = "", string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            if (_LoggedInUser.PLDBId > 0)
                pldbId = _LoggedInUser.PLDBId;

            MISModel mISModel = new MISModel(new DBLogger());
            var responce = new APIResponce();
            var ds = mISModel.GetLoaneeFRReceiptDetails(FilterDate, actionType, districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    LoaneeReceiptDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        #endregion#endif

        #region PLDBNIActReport on 19-06-2025
        public ActionResult NiActReport()
        {
            return View();
        }

        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetNIActDetails(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            if (_LoggedInUser.PLDBId > 0)
                pldbId = _LoggedInUser.PLDBId;


            MISModel mISModel = new MISModel(new DBLogger());
            var responce = new APIResponce();
            var ds = mISModel.GetNIActReportDetail(actionType, districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    NIActSummaryDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MonthWiseOTSRecovery()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMonthWiseOTSRecovery(DataPagingModel Details, string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0, int ConstituencyId = 0)
        {
            var responce = new APIResponce();
            var mISModel = new MISModel(new FileLogger());
            var ds = mISModel.GetMonthWiseOTSRecoveryDetail(ref Details, actionType, districtId, pldbId, pldbBranchId, ConstituencyId);
            if (ds != null)
            {
                responce.Data = new
                {
                    monthWiseOtsData = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };

                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return new JsonResult
            {
                Data = responce,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
            //return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OTS_Land_Mortgaged_Report()
        {
            return View();
        }


        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetOTSLandMortgagedDetails(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            var responce = new APIResponce();
            var mISModel = new MISModel(new FileLogger());
            var ds = mISModel.GetOTSLandMortgagedDetails(actionType, districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    otsLandMortgagedNormalData = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Mis_Section_For_138_NiAct()
        {
            return View();
        }


        [CustomAuthentication]
        [HttpGet]
        public JsonResult GetMisSectionFor138NiActDetail(string actionType = "", int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            var responce = new APIResponce();
            var mISModel = new MISModel(new FileLogger());
            var ds = mISModel.GetMisSectionFor138NiActDetails(actionType, districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    misSectionForNiActData = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Daily Progress Report #Pradeep 2025-07-22

        public ActionResult OtsDailyProgressReport()
        {
            return View();
        }
        public ActionResult OtsDailyProgressViewDetails()
        {
            return View();
        }
        public ActionResult OtsDailyProgressReport2()
        {
            return View();
        }
        public ActionResult OtsDailyProgressViewDetails2()
        {
            return View();
        }

        public ActionResult OtsDailyProgressDistrictReport()
        {
            return View();
        }
        public ActionResult OtsDailyProgressViewDistrictDetails()
        {
            return View();
        }
        public ActionResult OtsDailyProgressDistrictReport2()
        {
            return View();
        }
        public ActionResult OtsDailyProgressViewDistrictDetails2()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOtsDailyProgressReport(OtsDprRequestModel otsDprRequest)
        {
            var responce = new APIResponce();
            var mISModel = new MISModel(new FileLogger());
            var ds = mISModel.GetOtsDailyProgressReport(otsDprRequest);
            if (ds != null)
            {
                responce.Data = new
                {
                    otsDPR = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOtsDailyProgressViewDetails(OtsDprRequestModel otsDprRequest)
        {
            var responce = new APIResponce();
            var mISModel = new MISModel(new FileLogger());
            var ds = mISModel.GetOtsDailyProgressViewDetails(otsDprRequest);
            if (ds != null)
            {
                responce.Data = new
                {
                    otsDPR = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region gcc Loan 
        [HttpPost]
        [ActionName("GccLoanRpt")]
        public JsonResult GccLoanRpt(DataPagingModel Details, ExportColumn Exportjson = null)
        {
            int Division_District_Id = 0;

            if (_LoggedInUser.LevelId == 2)
            {
                Division_District_Id = _LoggedInUser.DivisionId;
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                Division_District_Id = _LoggedInUser.DistrictId;
            }


            bool IsExport = Exportjson == null ? false : true;

            MISModel objMisModel = new MISModel(new DBLogger());
            DataTable dt = objMisModel.GccLoanRpt(ref Details, _LoggedInUser.LevelId, IsExport);

            if (Exportjson != null)
            {
                dt = RemoveAndRnameColumnsDt(dt, Exportjson);
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(dt, "DueDateDiary.xlsx");
                return Json(new { filename = filename });
            }

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
        public DataTable RemoveAndRnameColumnsDt(DataTable dt, ExportColumn ExportColumn)
        {
            // 1. Remove Columns
            if (ExportColumn.RemoveColumns != null)
            {
                foreach (var col in ExportColumn.RemoveColumns)
                {
                    if (dt.Columns.Contains(col))
                        dt.Columns.Remove(col);
                }
            }

            // 2. Rename Columns
            if (ExportColumn.RenameColumns != null)
            {
                foreach (var kvp in ExportColumn.RenameColumns)
                {
                    if (dt.Columns.Contains(kvp.Key))
                        dt.Columns[kvp.Key].ColumnName = kvp.Value;
                }
            }
            return dt;
        }
        public JsonResult GccLoanReports(DataPagingModel Details, ExportColumn Exportjson = null, string Action = "")
        {
            string PageName = Action, SpName = "";
            switch (PageName)
            {
                case "DueDateDiary": SpName = "GccMisLoanRpt"; break;
                case "GccRecoveryReport": SpName = "GccMisRecoveryReport"; break;
                case "CMBeneficiaryList": SpName = "CM_KisanMasterList"; break;
                case "MPacsData": SpName = "GetMpacsSocietyData"; break;
                case "GSSTargetData": SpName = "GetGSSTargetData"; break;
                case "RecoveryList": SpName = "GccMisLoanRpt"; break;
                case "MPacs_InspectorList": SpName = "GetMPacs_InspectorList"; break;

                default: SpName = ""; break;
            }
            if (Action == "" || SpName == "")
            {
                return Json(new { DataList = (object)null, DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }

            int Division_District_Id = 0;

            if (_LoggedInUser.LevelId == 2)
            {
                Division_District_Id = _LoggedInUser.DivisionId;
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                Division_District_Id = _LoggedInUser.DistrictId;
            }

            bool IsExport = Exportjson == null ? false : true;
            MISModel objMisModel = new MISModel(new DBLogger());
            DataTable dt = objMisModel.GccLoanReports(ref Details, _LoggedInUser.LevelId, IsExport, SpName);

            if (Exportjson != null)
            {
                dt = RemoveAndRnameColumnsDt(dt, Exportjson);
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(dt, Exportjson.ReportName + ".xlsx");
                return Json(new { filename = filename });
            }
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GccLoanApplicationDataList

        [HttpPost]
        [ActionName("GetGccLoanApplicationList")]
        public JsonResult GetGccLoanApplicationList(DataPagingModel Details)
        {
            MISModel objMisModel = new MISModel(new DBLogger());
            DataTable DT = objMisModel.GetGccLoanApplicationList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Get_CCBList(string id)
        {
            try
            {
                int DistrictId = 0;
                if (!String.IsNullOrEmpty(id))
                {
                    DistrictId = Convert.ToInt32(id);
                }
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                DataTable dtCCB = objEntryForms.CCBData_GCC(0, DistrictId);
                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_CCBList(string id)" }, ex);
                return null;
            }
        }


        [HttpPost]
        public string Get_LoanBankBranch(string LoanDistrictId, string LoanBankId)
        {
            try
            {
                int DistrictId = 0, BankId = 0;
                if (!String.IsNullOrEmpty(LoanDistrictId))
                {
                    DistrictId = Convert.ToInt32(LoanDistrictId);
                }
                if (!String.IsNullOrEmpty(LoanBankId))
                {
                    BankId = Convert.ToInt32(LoanBankId);
                }

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DataTable dtCCB = objGopalCreditCardDBModel.Get_LoanBankBranch(DistrictId, BankId);

                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_LoanBankBranch(string LoanDistrictId, string LoanBankId)" }, ex);
                return null;
            }
        }
        [HttpPost]
        public string Get_LoanBranch_PACS(string LoanBranchId)
        {
            try
            {
                int BranchId = 0;
                if (!String.IsNullOrEmpty(LoanBranchId))
                {
                    BranchId = Convert.ToInt32(LoanBranchId);
                }

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DataTable dtCCB = objGopalCreditCardDBModel.Get_LoanBranch_PACS(BranchId);

                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_LoanBranch_PACS(string LoanBranchId)" }, ex);
                return null;
            }
        }


        #endregion GccLoanApplicationDataList

        #region GetGCCSummaryDistrictWise

        [HttpPost]
        [ActionName("GetGCCSummaryDistrictWise")]
        public JsonResult GetGCCSummaryDistrictWise(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());
                var result = objMisModel.GetGCCSummaryDistrictWise(ref Details);
                return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetGCCSummaryDistrictWise

        #region GetPLBDLoanApplicationPldbWiseList

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationPldbWise")]
        public JsonResult GetPLBDLoanApplicationPldbWise(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());
                var result = objMisModel.GetPLBDLoanApplicationPldbWiseList(ref Details);
                return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetPLBDLoanApplicationPldbWiseList


        #region GetPLBDLoanApplicationSectorWiseList

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationSectorWise")]
        public JsonResult GetPLBDLoanApplicationSectorWise(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetPLBDLoanApplicationSectorWiseList(ref Details);

                return Json(new { status = true, headers = result["headers"], data = result["data"] }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion GetPLBDLoanApplicationSectorWiseList

        //Bhavesh Singh
        #region GetPLBDLoanApplicationSectorWiseList

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationSUMMERYWISE")]
        public JsonResult GetPLBDLoanApplicationSUMMERYWISE(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetPLBDLoanApplicationSUMMERYWISEList(ref Details);

                return Json(new { status = true, headers = result["Headers"], data = result["Data"], Summary = result["Summary"] }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion GetPLBDLoanApplicationSectorWiseList

        // API Summary

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationReportSUMMERYWISE")]
        public JsonResult GetPLBDLoanApplicationReportSUMMERYWISE(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetPLBDLoanApplicationReportSUMMERYWISE(ref Details);

                return Json(new
                {
                    status = true,
                    headers = result["Headers"],
                    data = result["Data"]
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }



        #region PACS Summary on 23-12-2025
        public ActionResult PACSSummary()
        {
            return View();
        }

        [HttpPost]
        public string GetPacsSummaryDetails(DataPagingModel Details, string FromDate = "", string ToDate = "", string actionType = "", bool isExport = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel mISModel = new MISModel(new DBLogger());
            var responce = new APIResponce();
            var ds = mISModel.GetPacsSummaryDetails(ref Details, FromDate, ToDate, actionType, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "PacsSummary.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        #endregion

        #region GetPLBDLoanApplicationPurposeWiseList

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationPurposeWise")]
        public JsonResult GetPLBDLoanApplicationPurposeWise(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetPLBDLoanApplicationPurposeWiseList(ref Details);

                return Json(new { status = true, headers = result["Headers"], data = result["Data"] }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetPLBDLoanApplicationPurposeWiseList


        #region GetPLBDLoanApplicationPurposeWiseSectorList

        [HttpPost]
        [ActionName("GetPLBDLoanApplicationPurposeWiseSector")]
        public JsonResult GetPLBDLoanApplicationPurposeWiseSector(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetPLBDLoanApplicationPurposeWiseSectorList(ref Details);

                return Json(new { status = true, headers = result["Headers"], data = result["Data"] }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion GetPLBDLoanApplicationPurposeWiseSectorList

        #region GetLoanApplicationDetailBySector

        [HttpPost]
        [ActionName("GetLoanApplicationDetailBySector")]
        public JsonResult GetLoanApplicationDetailBySector(DataPagingModel Details, int pldbId, int sectorId, int purposeId, int branchid)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetLoanApplicationDetailBySector(Details, pldbId, sectorId, purposeId, branchid);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationDetailBySector



        #region GetLoanApplicationSectorCountDetail

        [HttpPost]
        [ActionName("GetLoanApplicationSectorCountDetail")]
        public JsonResult GetLoanApplicationSectorCountDetail(DataPagingModel Details, int pldbId, int sectorId, int branchId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetLoanApplicationSectorCountDetail(Details, pldbId, sectorId, branchId);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationDetailBySector


        #region GetLoanApplicationPLDBCountDetail

        [HttpPost]
        [ActionName("GetLoanApplicationPLDBCountDetail")]
        public JsonResult GetLoanApplicationPLDBCountDetail(DataPagingModel Details, int pldbId, int branchId, int financialYearId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetLoanApplicationPLDBCountDetail(Details, pldbId, branchId, financialYearId);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationPLDBCountDetail


        #region GetGCCSummaryBankWise

        [HttpPost]
        [ActionName("GetGCCSummaryBankWise")]
        public JsonResult GetGCCSummaryBankWise(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());
                var result = objMisModel.GetGCCSummaryBankWise(ref Details);
                return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetGCCSummaryBankWise


        //Bhavesh clickable

        #region GetLoanApplicationSummerWIseCountDetail

        [HttpPost]
        [ActionName("GetLoanApplicationSummerWIseCountDetail")]
        public JsonResult GetLoanApplicationSummerWIseCountDetail(int pldbId, int PurposeId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                var result = objMisModel.GetLoanApplicationSummerWIseCountDetail(pldbId, PurposeId);

                return Json(new { status = true, data = result["Data"] }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationSummerWIseCountDetail



        #region GetPLDBSubventionODApplicationList

        [HttpPost]
        [ActionName("GetPLDBSubventionODApplicationList")]
        public JsonResult GetPLDBSubventionODApplicationList(DataPagingModel Details)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());
                var result = objMisModel.GetPLDBSubventionODApplicationList(ref Details);
                return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion GetPLDBSubventionODApplicationList


        #region PLDBSubventionODApplicationDetailsList

        [HttpPost]
        [ActionName("PLDBSubventionODApplicationDetailsList")]
        public JsonResult PLDBSubventionODApplicationDetailsList(DataPagingModel Details, int pldbId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetPLDBSubventionODDetailList(Details, pldbId);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion PLDBSubventionODApplicationDetailsList

        //Bhavesh Singh Kushwah

        #region GetMPacsMembershipDistrictWiseCount

        [HttpPost]
        [ActionName("GetMPacsMembershipDistrictWiseCount")]
        public JsonResult GetMPacsMembershipDistrictWiseCount(DataPagingModel Details, int pldbId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetMPacsMembershipDistrictWiseCount(Details, pldbId);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetMPacsMembershipDistrictWiseCount

        //Bhavesh Singh

        //[HttpGet]
        //public JsonResult GetAadhaarServiceLogReport(string Aadhaar = null,string ServiceType = null,DateTime? FromDate = null,DateTime? ToDate = null,int PageNumber = 1,int PageSize = 20)
        //{
        //    var responce = new APIResponce();

        //    DataPagingModel paging = new DataPagingModel();
        //    paging.PageNumber = PageNumber;
        //    paging.PageSize = PageSize;

        //    //CommonServicesModel commonServices = new CommonServicesModel(new FileLogger());
        //    MISModel objMisModel = new MISModel(new DBLogger());

        //    var result = objMisModel.GetAadhaarServiceLogReport(paging, Aadhaar, ServiceType);

        //    responce.Data = result;

        //    return Json(new { Result = responce, TotalRecords = paging.TotalRecords }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetMMPBYFarmerDetails(DataPagingModel Details)
        {
            APIResponce responce = new APIResponce();

            MISModel objMisModel = new MISModel(new DBLogger());

            int districtId = Details.DistrictId;

            var result = objMisModel.GetMMPBYFarmerDetails(Details, districtId);

            responce.Status = "Success"; responce.Message = "Data Loaded";

            responce.Data = new
            {
                DataList = JsonConvert.SerializeObject(result),
                DataPaging = Details
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAadhaarServiceLogReport(DataPagingModel Details)
        {
            APIResponce responce = new APIResponce();

            MISModel objMisModel = new MISModel(new DBLogger());

            var result = objMisModel.GetAadhaarServiceLogReport(Details, null, null);

            responce.Status = "Success";
            responce.Message = "Data Loaded";

            responce.Data = new
            {
                DataList = JsonConvert.SerializeObject(result),
                DataPaging = Details
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetSocietyStatusList")]


        public JsonResult GetSocietyStatusList()
        {
            MISModel objSocietyModel = new MISModel();

            var dt = objSocietyModel.GetSocietyStatusList();

            var list = dt.AsEnumerable().Select(x => new
            {
                SocietyProceesStatusId = x["SocietyProceesStatusId"],
                ProceesStatusTextEn = x["ProceesStatusTextEn"],
                ProceesStatusTextHi = x["ProceesStatusTextHi"]
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //Added by bhavesh
        public ActionResult ExportSocietyReport(DataPagingModel Details, int StatusId, int ActId, int SocietyTypeId, int DistrictId, int IsOnline = -1)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            int IsExport = 1;
            DataTable dt = objMISModel.GetSocietyReportData(ref Details, StatusId, _LoggedInUser.UserRoleDeptMappingId, ActId, SocietyTypeId, DistrictId, IsOnline, 0, IsExport);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Society Report");
                ws.Columns().AdjustToContents();
                ws.Row(1).Style.Font.Bold = true;
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;  // Important!
                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Society_Report.xlsx"
                    );
                }
            }
        }

        public JsonResult GetCCPGccSummary_Export(int financialYearId = 0, int districtId = 0,
                                                  int CCBId = 0, int BranchId = 0, int PacsId = 0,
                                                  bool Overall = false, bool DistrictWise = false,
                                                  int blockId = 0, DateTime? FromDate = null, DateTime? ToDate = null, int Type = 0)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            var ds = mISModel.GetCCPGccSummary(financialYearId, districtId, CCBId,
                BranchId, PacsId, Overall, DistrictWise, blockId, FromDate, ToDate, "GccSummaryExport", Type);

            if (ds != null && ds.Tables.Count > 0)
            {
                var result = ds.Tables[0].AsEnumerable()
                    .Select(row => ds.Tables[0].Columns.Cast<DataColumn>()
                        .ToDictionary(col => col.ColumnName, col => row[col]))
                    .ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<object>(), JsonRequestBehavior.AllowGet);
        }
        //public string GetSocietyStatusList()
        //{
        //    MISModel objSocietyModel = new MISModel();

        //    return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyStatusList());
        //}

        //// Bhavesh 11/03/2026

        //[HttpPost]
        //public JsonResult GetMMPBYFarmerDetails(DataPagingModel Details)
        //{
        //    APIResponce responce = new APIResponce();

        //    MISModel objMisModel = new MISModel(new DBLogger());

        //    var result = objMisModel.GetMMPBYFarmerDetails(Details);

        //    responce.Status = "Success";  responce.Message = "Data Loaded";

        //    responce.Data = new
        //    {
        //        DataList = JsonConvert.SerializeObject(result),
        //        DataPaging = Details
        //    };

        //    return Json(responce, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetDistrictList()
        //{
        //    APIResponce responce = new APIResponce();

        //    MISModel objMisModel = new MISModel(new DBLogger());

        //    var result = objMisModel.GetDistrictList();

        //    responce.Status = "Success";
        //    responce.Message = "Data Loaded";

        //    responce.Data = JsonConvert.SerializeObject(result);

        //    return Json(responce, JsonRequestBehavior.AllowGet);
        //}



        [CustomAuthentication]
        [HttpGet]
        //[OutputCache(Duration = 3600 )]
        public JsonResult GCCDataYearWiseInformationList(int financialYearId = 0, int districtId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            MISModel mISModel = new MISModel(new DBLogger());

            var responce = new APIResponce();

            var ds = mISModel.GCCDataYearWiseInformation(financialYearId, districtId);
            if (ds != null)
            {
                responce.Data = new
                {
                    GCCDataYearWiseInformation = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    TotalGccSummary = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTotalLoaneeDetails(string year, string type)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            var ds = mISModel.GetTotalLoaneeDetails(year, type);

            return Json(new
            {
                Result = new
                {
                    Data = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public class ExportColumn
        {
            public List<string> RemoveColumns { get; set; }
            public Dictionary<string, string> RenameColumns { get; set; }
            public string ReportName { get; set; }
        }

        #region BUDS_Status_Report
        public ActionResult BUDSStatusReport()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetBUDSStatusReport")]
        public string GetBUDSStatusReport(int DistrictId, string SearchingText = "", string FromDate = "", string ToDate = "")
        {
            MISModel objModel = new MISModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModel.GetBUDSStatusReportDistWise(DistrictId, SearchingText, FromDate, ToDate));
        }
        #endregion
        [HttpGet]
        public ActionResult PLBDLoanApplicationStatusDetailList()
        {
            return View();
        }
        #region GetLoanApplicationStatusCountDetail

        [HttpPost]
        [ActionName("GetLoanApplicationStatusCountDetail")]
        public JsonResult GetLoanApplicationStatusCountDetail(DataPagingModel Details, int pldbId, string statusType, int financialYearId, int branchId)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetLoanApplicationPLDBStatusCountDetail(Details, pldbId, statusType, financialYearId, branchId);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationStatusCountDetail


        public ActionResult CCSComplaintAgeingReport()
        {
            return View();
        }


        #region GccNewBankWiseSanctionList

        [HttpPost]
        [ActionName("GetGccBankWiseSanctionList")]
        public JsonResult GccBankWiseSanctionList(DataPagingModel Details)
        {
            MISModel mISModel = new MISModel(new DBLogger());
            DataSet ds = mISModel.GccBankWiseSanctionList(ref Details);
            var result = new
            {
                GccSummary = (ds != null && ds.Tables.Count > 0) ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]) : null,
                DataPaging = Details
            };
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #endregion

        //#region 'GetCSSComplaintAgeingData'
        //[HttpGet]
        //[ActionName("GetCSSComplaintAgeingData")]
        //public JsonResult GetCSSComplaintAgeingData( int districtId = 0)
        //{
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        //    MISModel mISModel = new MISModel(new DBLogger());
        //    var responce = new APIResponce();

        //    var ds = mISModel.GetCSSComplaintAgeingData(districtId);

        //    if (ds != null)
        //    {
        //        responce.Data = new
        //        {
        //            ComplaintAgeingData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
        //        };

        //        responce.Status = "200";
        //        responce.Message = "Success";
        //    }
        //    else
        //    {
        //        responce.Status = "500";
        //        responce.Message = "Internal Server Error.";
        //    }

        //    return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        //}

        //#endregion


        #region GetCSSComplaintAgeingData

        [HttpPost]
        [ActionName("GetCSSComplaintAgeingData")]
        public JsonResult GetCSSComplaintAgeingData(DataPagingModel Details)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            DataTable DT = mISModel.GetCSSComplaintAgeingData(ref Details);

            var result = new
            {
                ComplaintAgeingData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT),
                DataPaging = Details
            };

            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion GetCSSComplaintAgeingData



        #region GccNewBankWiseSanctionList

        [HttpPost]
        [ActionName("GetSocietyMappingList")]
        public JsonResult GetSocietyMappingList(DataPagingModel Details)
        {
            MISModel mISModel = new MISModel(new DBLogger());
            DataSet ds = mISModel.GetSocietyMappingList(ref Details);
            var result = new
            {
                SocietyMapping = (ds != null && ds.Tables.Count > 0) ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]) : null,
                DataPaging = Details
            };
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #endregion


        [HttpPost]
        [ActionName("PldbCMSOtsApplicationList")]
        public JsonResult GetPldbCMSOtsApplicationList(int pldbid = 0, int pldbbranch = 0)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            DataSet ds = mISModel.GetPldbCMSOtsList(pldbid, pldbbranch);

            var result = new
            {
                OTSSummary = (ds != null && ds.Tables.Count > 0) ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]) : "[]"
            };

            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }



        #region GetLoanApplicationOTSCountDetail

        [HttpPost]
        [ActionName("GetLoanApplicationOTSCountDetail")]
        public JsonResult GetLoanApplicationOTSCountDetail(DataPagingModel Details, int pldbId, string docType)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetLoanApplicationOTSCountDetail(Details, pldbId, docType);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetLoanApplicationStatusCountDetail


        #region NCD Report Sector Wise or District Wise 21-05-2026
        [HttpGet]
        public ActionResult NCDSectorReport()
        {
            return View();
        }

        #endregion


        #region PACS Summary New on 25-05-2026
        public ActionResult PacsSummaryNew()
        {
            return View();
        }
        #endregion

        #region GetPacsSummaryNewDetails
        [HttpPost]
        public string GetPacsSummaryNewDetails(DataPagingModel Details, string FromDate = "", string ToDate = "", string actionType = "", bool isExport = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            MISModel mISModel = new MISModel(new DBLogger());
            var responce = new APIResponce();
            var ds = mISModel.GetPacsSummaryNewDetails(ref Details, FromDate, ToDate, actionType, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "PacsNewSummary.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }
        #endregion


        [HttpPost]
        [ActionName("GetGSSMpacsTargetCountList")]
        public JsonResult GetGSSMpacsTargetCountList(int DistrictId = 0)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            DataSet ds = mISModel.GetGSSMpacsTargetCount(DistrictId);

            var result = new
            {
                GSSMpacsTargetSummary = (ds != null && ds.Tables.Count > 0) ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]) : "[]"
            };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }



        #region GetGSSMpacsTargetCountDetail

        [HttpPost]
        [ActionName("GetGSSMpacsTargetCountDetail")]
        public JsonResult GetGSSMpacsTargetCountDetail(DataPagingModel Details, int DistrictId, string Type)
        {
            try
            {
                MISModel objMisModel = new MISModel(new DBLogger());

                DataTable DT = objMisModel.GetGSSMpacsTargetCountDetail(Details, DistrictId, Type);

                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetGSSMpacsTargetCountDetail


        [HttpPost]
        [ActionName("GetAllMergeSociety")]
        public JsonResult GetAllMergeSociety(string Action = "", int DistrictId = 0)
        {
            MISModel mISModel = new MISModel(new DBLogger());

            DataSet ds = mISModel.GetSocietyMergeReport("getAllMergeSociety", DistrictId);

            var result = new
            {
                SocietySummary = (ds != null && ds.Tables.Count > 0) ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]) : "[]"
            };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }


        #region GetActiveSocietyDetail

        [HttpPost]
        [ActionName("GetActiveSocietyDetail")]
        public JsonResult GetActiveSocietyDetails(DataPagingModel Details, int DistrictId, int FlagId)
        {

            try
            {
                MISModel mISModel = new MISModel(new DBLogger());
                DataTable DT = mISModel.GetActiveSocietyDetail(Details, DistrictId, FlagId);
                return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion GetActiveSocietyDetail      


        [HttpPost]
        [ActionName("GetManageSocietyDetailList")]
        public JsonResult GetManageSocietyDetailList(DataPagingModel Details)
        {
            MISModel objMisModel = new MISModel(new DBLogger());
            DataTable DT = objMisModel.GetManageSocietyDetailsList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        #region SocietyElectionReportList

        [HttpPost]
        [ActionName("SocietyElectionReportList")]
        public JsonResult SocietyElectionReportList(DataPagingModel Details)
        {
            MISModel objLoan = new MISModel(new DBLogger());

            DataTable DT = objLoan.GetElectionDetailReportList(ref Details);

            var result = new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #endregion SocietyElectionReportList

        #region SocietyElectionFlowReportList

        [HttpPost]
        [ActionName("SocietyElectionFlowReportList")]
        public JsonResult SocietyElectionFlowList(DataPagingModel Details)
        {
            MISModel objLoan = new MISModel(new DBLogger());

            DataTable DT = objLoan.GetElectionFlowDetailReportList(ref Details);

            var result = new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #endregion SocietyElectionReportList


        [HttpGet]
        [ActionName("GetNewLovValuesForStatusListId")]
        public string GetNewLovValuesForStatusListId(string ListCode)
        {
            DataTable DT = new MISModel().GetNewLovValuesForStatusListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        public JsonResult SaveWorkflowAction(WorkflowActionModel workflowActionModel)
        {
            MISModel objLoan = new MISModel(new DBLogger());
            int result = objLoan.SaveWorkflowAction(workflowActionModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KPIReports()
        {
            return View();
        }

        [HttpGet]
        public ActionResult KPIPLDBReports()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetKPIReport")]
        public string GetKPIReport(string KPIFor, int Month, string FY, int OfficeId, int DistrictId , int ResionId = 0)
        {
            DataTable dt = new MISModel().GetKPIReport(KPIFor, Month, FY, OfficeId, DistrictId , ResionId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetKPIOffices")]
        public string GetKPIOffices(string KPIFor, string Month, string FinancialYearId)
        {
            DataTable dt = new MISModel().GetKPIOffices(KPIFor, Month, FinancialYearId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetKPIOverallRating")]
        public string GetKPIOverallRating(string KPIFor, int Month, string FY, int OfficeId = 0, int DistrictId = 0, int DivisionId = 0, int ResionId = 0)
        {
            DataTable dt = new MISModel().GetKPIOverallRating(KPIFor, Month, FY, OfficeId, DistrictId, DivisionId, ResionId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        #region SocietyLocationReportList

        [HttpPost]
        [ActionName("SocietyLocationReportList")]
        public JsonResult SocietyLocationReportList(DataPagingModel Details)
        {
            MISModel objLoan = new MISModel(new DBLogger());

            DataTable DT = objLoan.GetSocietyLocationByDistrictReportList(ref Details);

            var result = new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #endregion SocietyLocationReportList


        #region GetSocietyGeoTaggingReport on 02-09-2026

        public ActionResult SocietyGeoTaggingReport()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetSocietyGeoTaggingDetail()
        {
            var responce = new APIResponce();
            MISModel misModel = new MISModel(new DBLogger());
            var ds = misModel.GetSocietyGeoTaggingReport();
            if (ds != null)
            {
                responce.Data = new
                {
                    getTaggingCount = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    geoTaggingData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region GetFertilizerDemandReport on 08-09-2026

        public ActionResult FertilizerDemandReport()
        {
            return View();
        }

        public ActionResult FertilizerDemandDetailsReport()
        {
            return View();
        }

        public string GetFertilizerDemandDetail(DataPagingModel Details, bool isExport = false, string actionType = "", int type = 0, int DemandRequestId = 0)
        {
            MISModel misModel = new MISModel(new DBLogger());
            var ds = misModel.GetFertilizerDemandDetail(ref Details, isExport, actionType, type, DemandRequestId);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");
                dt.Columns.Remove("FDRId");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FertilizerDemandReport.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);

        }

        public string GetFertilizerDemandMultiDetail(DataPagingModel Details, bool isExport = false, string actionType = "", int type = 0, int DemandRequestId = 0)
        {
            MISModel misModel = new MISModel(new DBLogger());
            string ReportType = "1";
            if (Details.SearchParameter.Where(x => x.Key == "ReportType").Any())
            {
                ReportType = Details.SearchParameter.Where(x => x.Key == "ReportType").Select(x => x.Value).FirstOrDefault();
            }
            var ds = misModel.GetFertilizerDemandMultiDetail(ref Details, isExport, actionType, type, DemandRequestId);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");
                dt.Columns.Remove("FDRId");

                var filename = "";

                if (ReportType == "1")
                {
                    filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FertilizerDemandDetails.xlsx");
                }
                else if (ReportType == "2")
                {
                    filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FertilizerCreditFacilityDetails.xlsx");
                }
                else
                {
                    filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FertilizerPurchaseDetails.xlsx");
                }
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);

        }


       


        public ActionResult TaggedItemDetailsReport()
        {
            //string encAadhaarNo = COMMON_FUNCTION.Encrypt("200902504760");
            //Ea5mTAPB9rlxfUe6TPKknA==
            return View();
        }

        public string GetTaggedItemDetail(DataPagingModel Details, bool isExport = false)
        {
            MISModel misModel = new MISModel(new DBLogger());
            var ds = misModel.GetTaggedItemDetail(ref Details, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");
                dt.Columns.Remove("TagId");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "TaggedItemDetailReport.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);

        }

       



        #endregion


        [HttpGet]
        [ActionName("GetKPI_PerformanceCriteria")]
        public string GetKPI_PerformanceCriteria(string criteriaForKPI)
        {
            DataTable dt = new MISModel().GetKPI_PerformanceCriteria(criteriaForKPI);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        public JsonResult GetFertilizerDetailByUser(string actionType = "", int DemandRequestId = 0)
        {
            var responce = new APIResponce();
            MISModel misModel = new MISModel(new DBLogger());
            var ds = misModel.GetFertilizerDetailByUser(actionType, DemandRequestId);
            if (ds != null)
            {
                responce.Data = new
                {
                    FertilizerDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult SocietyLocationStatusApprovedList()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetDRSocietyList")]
        public string GetDRSocietyList()
        {
            DataTable dt = new MISModel().GetDRSocietyList();

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        [ActionName("UpdateSocietyLocationApproval")]
        public string UpdateSocietyLocationApproval(int societyLocationDetailsID, int societyId, int isApproved, string remark)
        {
            DataTable dt = new MISModel().UpdateSocietyLocationApproval(societyLocationDetailsID, societyId, isApproved, remark);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpGet]
        [ActionName("GetKPIPLDBOverallRating")]
        public string GetKPIPLDBOverallRating(string KPIFor, int Month, string FY, int PldbId = 0)
        {
            DataTable dt = new MISModel().GetKPIpldbOverallRating(KPIFor, Month, FY, PldbId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        public ActionResult KPIUnitOfficeReports()
        {
            return View();
        }

    }
}