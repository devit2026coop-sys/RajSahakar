using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using CoopDAL;

namespace Cooperative.Controllers
{
    //[Error]
    //[CustomAuthentication]

    public class MasterController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        [ActionName("GetMasterData")]
        public string GetMasterDataDetails(string MasterType = "", string MasterCode = "", string MasterCode2 = "")
        {

            string _MasterData = "{}";
            int Code = 0;

            if (string.IsNullOrEmpty(MasterType) || !int.TryParse(MasterCode, out Code))
            {
                // Response.Redirect("~/Views/Error/General.cshtml");
            }

            MasterModel master = new MasterModel(new DBLogger());


            switch (MasterType.ToUpper())
            {
                case "TITLE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TitleList()); break; }
                case "DEPARTMENT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DepartmentList()); break; }
                case "DESIGNATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DesignationtList(Code)); break; }
                case "POST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PostList(Code)); break; }
                case "GENDER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GenderList()); break; }
                case "RELIGION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ReligionList()); break; }
                case "CAST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CastList()); break; }
                case "CATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CategoryList()); break; }
                case "STATE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.StateList(Code)); break; }
                case "DIVISION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DivisionList()); break; }
                case "DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DistrictList(Code)); break; }
                case "TEHSIL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TehsilList(Code)); break; }
                case "BLOCK": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList(Code)); break; }
                case "PANCHAYATSAMITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList(Code)); break; }
                case "ASSEMBLY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.AssemblyList(Code)); break; }
                case "GRAMPANCHAYAT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GramPanchayatList(Code)); break; }
                case "VILLAGE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.VillageList(Code)); break; }
                case "CITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityList(Code, MasterCode2)); break; }
                case "CITYLISTBY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityByCityTypeList(Code, MasterCode2)); break; }

                case "WARD": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WardList(Code)); break; }
                case "FINANCIALYEAR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.FinancialYearList()); break; }
                case "SOCIETYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietyTypeList(Code)); break; }
                case "BUSINESS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.BusinessList(MasterCode)); break; }
                case "LIABILITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LiablityList()); break; }
                case "SOCIETYCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietyClassList(Code)); break; }
                case "SOCIETYSUBCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietySubClassList(Code, MasterCode2)); break; }

                case "UNITS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.UnitListByDeptId(Code)); break; }
                case "LEVELS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LevelList()); break; }
                case "OCCUPATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OccupationList()); break; }
                case "MEMBERTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MemberTypeList()); break; }
                case "ACT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ACTList()); break; }
                case "CLASSES": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ClassesList()); break; }
                case "SubClasses": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ClassesList()); break; }
                case "Buissness": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.BusinessList(MasterCode)); break; }
                // case "Unit": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetAllSubUnitList('')); break; }
                case "Organisation": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OrganisationsList()); break; }
                // case "PurchaseCenter": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PurchaseCenterList()); break; }                 
                case "Office": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OfficesList()); break; }
                case "RESIONMAPPING": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ResionMappingList(MasterCode)); break; }
                case "CHANNEL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ChannelList()); break; }
                case "TARGET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TargetList()); break; }
                case "USERS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.UsersList()); break; }
                case "MAJORACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMajorActivityList("0")); break; }
                case "ACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetActivityList(MasterCode)); break; }
                case "MONTH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterMonthList()); break; }
                case "RELATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterRelationList()); break; }
                case "ECONOMICGROUP": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterEconomicList()); break; }
                case "PLDB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterPLDBList(MasterCode)); break; }
                case "PLDBBRANCH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterPLDBBranchList(MasterCode)); break; }
                case "EDUCATIONQUALIFICATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetEducationQualificationList()); break; }
                case "FERTILIZER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetFertilizerList()); break; }
                case "MENUCONFIGURATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MenuListForConfiguration()); break; }
                case "CONFIGURATIONACTION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ActionListForConfiguration()); break; }
                case "DEALERNATURE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DealerNature()); break; }
                case "LIABILITYASSET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LiabilityAssetMasterList(Convert.ToInt32(MasterCode))); break; }
                case "CITYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityTypeList(Convert.ToInt32(MasterCode))); break; }
                //case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }
                case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }

                case "PLANT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PlantList(Convert.ToInt32(MasterCode))); break; }
                case "DEPORT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DepotList(Convert.ToInt32(MasterCode))); break; }
                case "WAREHOUSE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WarehouseList(Convert.ToInt32(MasterCode))); break; }

                case "CCSCOMPLAINTDOCTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CCSComplaintDocumentTypeList()); break; }
                case "RAJFEDRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.RajfedROList()); break; }
                case "DISTRICTBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetAllDistrictListByRO(Code)); break; }
                case "DISTRICTBYRODISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetDistrictListByRODistrict(Code)); break; }
                case "WAREHOUSEBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetWarehousetByRODistrict(Code)); break; }
                case "BANKLIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetBankMasterList()); break; }
                case "CCB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetCCBData(Code)); break; }
                case "GRIEVANCECATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GrievanceCategoryList()); break; }//bhupendra
                case "GSS_DAIRY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetGSS_DAIRYList(Code, _LoggedInUser.SSOID)); break; }//bhupendra
                case "INSPECTOR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetInspector(_LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.DistrictId)); break; }//bhupendra
                case "FLOWPROCESSSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetSocietyFlowProcessStatus(_LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.DistrictId)); break; }
                case "CMINSTALLMENTTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CMInstallmentType()); break; }
                case "CMKISAN": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetCMK_MasterData(MasterCode)); break; }

                // Pankaj 26-06-2024 Start
                case "PANCHAYATSAMITY_DISTRICTCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList_DistrictCode(MasterCode)); break; }
                case "GRAMPANCHAYAT_BLOCKCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GramPanchayatList_BLOCKCODE(MasterCode)); break; }
                case "VILLAGE_GPCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.VillageList_GPCODE(MasterCode)); break; }
                case "CITY_DISTRICTCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityList_DISTRICTCODE(MasterCode, MasterCode2)); break; }
                case "WARD_CITYCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WardList_CITYCODE(MasterCode)); break; }
                // Pankaj 26-06-2024 End

                case "CM_SUB_DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.Cm_SubDistrict(MasterCode)); break; }
                case "CM_VILLAGE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.Cm_Village(MasterCode, MasterCode2)); break; }
                //23-08-2024 # Pradeep
                case "LOANPURPOSE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PurposeOfLoan()); break; }
                case "MN_DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MN_DistrictList(Code)); break; }
                case "MN_BLOCK": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MN_BlockList(Code)); break; }
                case "DISTRICT_WR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DISTRICT_WR(Code)); break; }
                case "PAYMENTMODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetPaymentModeList()); break; } //21-04-2025 #Pradeep
                case "OTSPROCESSSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetOtsProcessStatus()); break; } //29-04-2025 #Pradeep
                case "OTSPENDINGLEVEL": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetOtsPendingLevel(Code)); break; } //18-06-2025 #Pradeep
                case "FUNCTIONALSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.FunctionalStatusList()); break; } //18-06-2025 #Pradeep

                case "CCB_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "CCB_BANK_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbBranchData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "PACS_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetPacsData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "MAPPING_GCC_ID": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbIdsByMappingGccId()); break; } //07-10-2025 #Pradeep

                case "PLDBSECTORODNEW": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetPLDBSectorODNew()); break; } //16-12-2025 #Jaya
                case "KPIFORPACS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.KPIforPacs()); break; }
                case "MEMBERPOST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MemberPostList()); break; }
                case "PRODUCTTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetProductTypeList()); break; }
                case "COMPANY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetCompanyList()); break; }
                case "PRODUCT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMstProductList(Code)); break; }
                default: break;

            }
            return _MasterData;
        }

        [HttpGet]
        public ActionResult Division()
        {

            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Division, (Request.Cookies[SessionIdentity.Culture]).Value);

            }
            return View();
        }

        [HttpGet]
        [ActionName("SelectRole")]
        public string SelectRoleList(int LevelId = 0)
        {

            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectRoleList(LevelId));
        }

        [HttpGet]
        [ActionName("SelectCCBSocietyForCCBUser")]
        public string GetSocietyForCCBUser(int Code = 0)
        {
            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetCCBData(Code, _LoggedInUser.SSOID));
        }


        [HttpGet]
        [ActionName("SocietyCategoryList")]
        public string GetAllSocietyCategory(string ClassId, string SubClassId, string SocietyTypeId)
        {
            string WhereClause = "";
            if (ClassId != null && ClassId.Length > 0)
            {
                WhereClause = " and SocietyClassId=" + ClassId;
            }
            if (SubClassId != null && SubClassId.Length > 0)
            {
                if (SubClassId != "3030" && SubClassId != "3031" && SubClassId != "3032")
                    WhereClause += " and SubClassId=" + SubClassId;
            }
            if (SocietyTypeId != null && SocietyTypeId.Length > 0)
            {
                WhereClause += " and SocietyTypeId=" + SocietyTypeId;
            }
            SubClassesModel objsubclass = new SubClassesModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objsubclass.GetAllSocietyCategory(WhereClause));
        }

        [HttpGet]
        public ActionResult District()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.District, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();

        }

        [HttpGet]
        public ActionResult FinancialYear()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FinancialYear, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpGet]
        public ActionResult Classes()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Classes, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpGet]
        public ActionResult SportAssociation()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SportAssociation, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveClasses")]
        public JsonResult MenuSmsActionInsert(ClassesModel _Model)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddClasses());
        }


        [HttpGet]
        [ActionName("ClassList")]
        public string GetAllClass(string WhereClause)
        {

            ClassesModel objclassModel = new ClassesModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objclassModel.GetAllClasses());
        }

        [HttpPost]
        [ActionName("DeleteandstatusClass")]
        public JsonResult delete_record(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            ClassesModel objclassModel = new ClassesModel(new FileLogger());
            return Json(objclassModel.DeleteChangeStatusClasses(id, Action), JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public ActionResult Department()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Department, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        public ActionResult Designation()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Designation, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Post()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Post, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Gender()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Gender, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Religion()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Religion, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Cast()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Cast, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Category()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Category, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult State()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.State, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Tehsil()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Tehsil, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Block()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Block, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpGet]
        public ActionResult PACSBranch()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GP()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GP, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Village()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Village, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Ward()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Ward, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult City()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.City, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Widget()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Widget, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        public ActionResult FileUpload()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FileUpload, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpPost]
        [ActionName("SaveWidget")]
        public JsonResult WidgetInsert(WidgetModel _Model)//UserRoleEntity _Model
        {
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            //WidgetModel objWidgetModel = new WidgetModel(new FileLogger());
            return Json(_Model.AddWidgetItem());
        }


        [HttpPost]
        [ActionName("SaveBuissnessRegister")]
        public JsonResult BuissnessForm(BuissnessRegistrationModel _Model)
        {

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            _Model.CreatedBy = _Model.UpdatedBy = Convert.ToInt32(_LoggedInUser.UserId);
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.InsertBuissness(_Model));
        }


        public ActionResult BuissnessForm()
        {
            return View();
        }

        public ActionResult LiablitiesForm()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SaveLiablitiesRegistrations")]
        public int SaveLiablitiesRegistration(LiablitiesRegistrationModel _Model)
        {
            var result = _Model.InsertLiabilities(_Model);
            return result;
        }




        [HttpGet]
        public ActionResult SubClasses()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SubClasses, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveSubClasses")]
        public JsonResult MenuSubActionInsert(SubClassesModel _Model)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("CreationDate");
            //if (!ModelState.IsValid)
            //    return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            return Json(_Model.AddSubClasses(_Model));

        }

        [HttpPost]
        public string ChangeStatusSubClass(string Id, string Action)
        {
            int SubClassId = 0, ActionId = 0;
            if (!int.TryParse(Id, out SubClassId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SubClassesModel objsubclass = new SubClassesModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objsubclass.ChangeStatusSubClass(SubClassId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }
        [HttpGet]
        [ActionName("SubClassList")]
        public string GetAllSubClass(string ClassId, string SocietyTypeId)
        {
            string WhereClause = "";
            if (ClassId != null && ClassId.Length > 0)
            {

                WhereClause = " and SubClasses.ClassId=" + ClassId;
            }
            if (SocietyTypeId != null && SocietyTypeId.Length > 0)
            {
                WhereClause += " and SocietyTypeId=" + SocietyTypeId;
            }
            SubClassesModel objsubclass = new SubClassesModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objsubclass.GetAllSubClasses(WhereClause));
        }

        [HttpGet]
        public ActionResult Buissness()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Buissness, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpGet]
        [ActionName("BusinessList")]
        public string GetAllBuisness(string WhereClause)

        {
            BuissnessRegistrationModel objbuisnessregistration = new BuissnessRegistrationModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objbuisnessregistration.GetAllBuisness());
        }
        [HttpPost]
        public string ChangeStatusBusiness(string Id, string Action)
        {
            int BusinessId = 0, ActionId = 0;
            if (!int.TryParse(Id, out BusinessId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            BuissnessRegistrationModel objbuisnessregistration = new BuissnessRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objbuisnessregistration.ChangeStatusBusiness(BusinessId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        [HttpGet]
        public ActionResult SocietyType()
        {
            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyType, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        [HttpPost]
        [ActionName("SaveSocietyType")]
        public JsonResult SocietyTypeForm(SocietyTypeModal _Model)
        {

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            _Model.CreatedBy = _Model.UpdatedBy = Convert.ToInt32(_LoggedInUser.UserId);
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();


            return Json(_Model.InsertSocietyType(_Model));
        }
        [HttpGet]
        [ActionName("SocietyTypeList")]
        public string GetAllSocietyType(string WhereClause)
        {
            SocietyTypeModal objSocityclass = new SocietyTypeModal(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objSocityclass.GetAllSocietyType());
        }

        [HttpPost]
        public string ChangeStatusSocietyType(string Id, string Action)
        {
            int SocietyTypeId = 0, ActionId = 0;
            if (!int.TryParse(Id, out SocietyTypeId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SocietyTypeModal objSocityclass = new SocietyTypeModal(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSocityclass.ChangeStatusSocietyType(SocietyTypeId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }
        [HttpGet]
        [ActionName("DepartmentList")]
        public string GetAllDepartment(string WhereClause)
        {
            MasterModel objclassModel = new MasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objclassModel.DepartmentList());
        }

        [HttpGet]
        public ActionResult Unit()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Unit, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveUnitForm")]
        public JsonResult unitinsert(UnitMasterModel _Modal)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddUnit(_Modal));

        }

        [HttpGet]
        [ActionName("UnitsList")]
        public string GetAllSubUnitList(string WhereClause = "")
        {
            UnitMasterModel objUnitmaster = new UnitMasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objUnitmaster.GetAllUnits(WhereClause));
        }


        [HttpGet]
        [ActionName("OrganisationsList")]
        public string GetAllSubOrganisationList(int DepartmentId = 0, int OrgId = 0)
        {
            string WhereClause = "";
            if (OrgId > 0)
                WhereClause = " and o.isactive in (0,1) and OrganisationId= " + OrgId;
            else if (DepartmentId > 0)
                WhereClause = " and o.isactive =1  And  DepartmentId=" + DepartmentId;
            else
                WhereClause = " and o.isactive in (0,1) ";

            OrganisationMasterModel objOrganisationmaster = new OrganisationMasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objOrganisationmaster.GetAllOrganisations(WhereClause));
        }
        [HttpGet]
        public ActionResult Organisation()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Organisation, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveOrganisationForm")]
        public JsonResult Organisationinsert(string OrganisationNameEn, string OrganisationNameHi, string IsActive, int OrganisationId, int DepartmentId)
        {
            OrganisationMasterModel _Modal = new OrganisationMasterModel();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddOrganisation(OrganisationNameEn, OrganisationNameHi, IsActive, OrganisationId, DepartmentId));

        }
        [HttpPost]
        [ActionName("ChangedeleteStatusOrganisation")]
        public JsonResult delete_recordOrganisation(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            OrganisationMasterModel objOrganisationModel = new OrganisationMasterModel(new DBLogger());
            return Json(objOrganisationModel.DeleteOrganisationfun(id, Action), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("ChangedeleteStatusunit")]
        public JsonResult delete_recordunit(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            UnitMasterModel objUnitModel = new UnitMasterModel(new DBLogger());
            return Json(objUnitModel.Deleteunitfun(id, Action), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int ChangeDesignationStatus(string DesignationId, string ActiveStatus, string ActionId)
        {
            if (string.IsNullOrEmpty(ActiveStatus))
                ActiveStatus = "false";

            int _designationId = 0, _ActionId = 0;
            bool _IsActive = false;

            if (!(int.TryParse(DesignationId, out _designationId) && int.TryParse(ActionId, out _ActionId) && bool.TryParse(ActiveStatus, out _IsActive)))
                return -2;

            MasterModel objMasterModel = new MasterModel(new FileLogger());

            return objMasterModel.ChangeDesignationStatus(_designationId, (_IsActive ? false : true), _ActionId);
        }


        [HttpPost]
        [ActionName("SaveDesignationFormdata")]
        public JsonResult SaveDesignation(DesignationMasterModel _Modal)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddDesignation(_Modal));

        }

        [HttpPost]
        [ActionName("ChangedeleteStatusDesignation")]
        public JsonResult delete_recorddesignation(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            DesignationMasterModel objDesignationMasterModel = new DesignationMasterModel(new DBLogger());
            return Json(objDesignationMasterModel.DeleteDesignationfun(id, Action), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ActionName("SaveDepartmentForm")]
        public JsonResult Departmentinsert(string DEPTNAMEENGLISH, string DEPTNAMEHINDI, string IsActive, int ID)
        {
            DepartmentMasterModel _Modal = new DepartmentMasterModel();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddDepartment(DEPTNAMEENGLISH, DEPTNAMEHINDI, IsActive, ID));

        }
        [HttpPost]
        [ActionName("ChangeStatusDepartment")]
        public JsonResult ChangeStatusDepartment(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            DepartmentMasterModel objDepartmentModel = new DepartmentMasterModel(new DBLogger());
            return Json(objDepartmentModel.ChangeStatusDepartment(id, Action), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [ActionName("GetDepartmentList")]
        public string GetAllDepartments(string WhereClause = "")
        {
            DepartmentMasterModel objDepartmentModel = new DepartmentMasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentModel.GetAllDepartments(WhereClause));
        }



        [HttpGet]
        [ActionName("OfficesList")]
        public string GetAllSubOfficeList(string WhereClause)
        {
            OfficeMasterModel objOfficemaster = new OfficeMasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objOfficemaster.GetAllOffices(WhereClause));
        }

        [HttpGet]
        public ActionResult Office()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Office, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveOfficeForm")]
        public JsonResult Officeinsert(string OfficeNameEn, string OfficeNameHi, string IsActive, int OfficeId, int OrganizationLevelsMappingID)
        {
            OfficeMasterModel _Modal = new OfficeMasterModel();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddOffice(OfficeNameEn, OfficeNameHi, IsActive, OfficeId, OrganizationLevelsMappingID));

        }
        [HttpPost]
        [ActionName("ChangeStatusOffice")]
        public JsonResult delete_recordOffice(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            OfficeMasterModel objOfficeModel = new OfficeMasterModel(new DBLogger());
            return Json(objOfficeModel.ChangeStatusOffice(id, Action), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UnAuthorizeAccess()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UnAuthorizeAccess, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }




        [HttpGet]
        public ActionResult ActionMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveSocietyProcessStatus(SocietyProcessStatus model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = model.SocietyProceesStatusId > 0 ? 2 : 1;
            return Json(objMasterModel.InsertSocietyProcessStatus(model));
        }

        [HttpPost]
        public string GetSocietyProcessStatus(string clause)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.GetSocietyProcessStatus(clause));
        }

        [HttpPost]
        public JsonResult ChangeSocietyProcessStatus(SocietyProcessStatus model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = 3;
            return Json(objMasterModel.InsertSocietyProcessStatus(model));
        }

        [HttpPost]
        public JsonResult DeleteSocietyProcessStatus(SocietyProcessStatus model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = 4;
            return Json(objMasterModel.InsertSocietyProcessStatus(model));
        }

        [HttpGet]
        public ActionResult WorkFlowConfiguration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveWorkFlowConfiguration(ApplicationNextAction model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = model.ApplicationNextActionId > 0 ? 2 : 1;
            return Json(objMasterModel.SaveWorkFlowConfiguration(model));
        }

        [HttpPost]
        public string GetWorkFlowConfiguration(string clause)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.GetWorkFlowConfiguration(clause));
        }

        [HttpPost]
        public JsonResult ChangeWorkFlowConfiguration(ApplicationNextAction model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = 3;
            return Json(objMasterModel.SaveWorkFlowConfiguration(model));
        }

        [HttpPost]
        public JsonResult DeleteWorkFlowConfiguration(ApplicationNextAction model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.ActionId = 4;
            return Json(objMasterModel.SaveWorkFlowConfiguration(model));
        }
        [HttpGet]
        [ActionName("PACSBranchList")]
        public string PACSBranchList(string DISTRICT_ID)
        {
            MasterModel objclassModel = new MasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objclassModel.PACSBranchList(DISTRICT_ID));
        }
        [HttpGet]
        [ActionName("ResionMappingByDistIdOrganizationLevelsMappingID")]
        public string ResionMappingByDistIdOrganizationLevelsMappingID(string DistrictId, string OrganizationLevelsMappingID, string MappingID = "0")
        {
            MasterModel objclassModel = new MasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objclassModel.ResionMappingByDistIdOrganizationLevelsMappingID(DistrictId, OrganizationLevelsMappingID, MappingID));
        }


        public ActionResult LiabilityAsset()
        {
            return View();
        }


        [HttpPost]
        public int InsertUpdateLiabilityAsset(LiabilityAssetMasterEntity model)
        {
            model.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
            model.ActionId = model.LiabilityAssetMasterId > 0 ? 2 : 1;
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            int Result = objMasterModel.InsertUpdateLiabilityAsset(model);
            return Result;
        }

        [HttpPost]
        public int LiabilityAssetMasterSetStatus(int LiabilityAssetMasterId, int ActionId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.LiabilityAssetMasterSetStatus(LiabilityAssetMasterId, ActionId);
        }


        [HttpGet]
        public ActionResult GoDown()
        {
            ViewBag.DistrictId = _LoggedInUser.DistrictId;
            ViewBag.UserLevel = _LoggedInUser.LevelId;
            ViewBag.WarehouseId = _LoggedInUser.WarehouseId;
            return View();
        }

        [HttpPost]
        public int RajfedGoDownInsertUpdate(GoDownMasterEntity model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
            model.ActionId = model.GoDownId > 0 ? 2 : 1;
            return objMasterModel.RajfedGoDownInsertUpdate(model);
        }

        [HttpPost]
        public int RajfedGoDownSetStatus(int GoDownId, int ActionId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.RajfedGoDownSetStatus(GoDownId, ActionId);
        }

        [HttpPost]
        public string RajfedGoDownList(int GoDownId, int WarehouseId = 0)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMasterModel.RajfedGoDownList(GoDownId, WarehouseId));
        }


        [HttpGet]
        public ActionResult Warehouse()
        {
            ViewBag.DistrictId = _LoggedInUser.DistrictId;
            ViewBag.UserLevel = _LoggedInUser.LevelId;
            ViewBag.WarehouseId = _LoggedInUser.WarehouseId;
            return View();
        }

        [HttpPost]
        public int RajfedWarehouseInsertUpdate(RajfedWarehouseMasterEntity model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.LicenceNo = string.IsNullOrEmpty(model.LicenceNo) ? "Exempted" : model.LicenceNo;
            model.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
            model.ActionId = model.WarehousId > 0 ? 2 : 1;
            return objMasterModel.RajfedWarehouseInsertUpdate(model);
        }

        [HttpPost]
        public int RajfedWarehouseSetStatus(int WarehouseId, int ActionId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.RajfedWarehouseSetStatus(WarehouseId, ActionId);
        }

        [HttpPost]
        public string RajfedWarehouseList(int WarehouseId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            string WhereClause = string.Empty;
            int UserCityId = _LoggedInUser.CityId, UserBlockId = _LoggedInUser.BlockId;
            if (_LoggedInUser.LevelId == 1)//For Stat User
            {
                WhereClause = " AND 1=1 ";
            }
            else if (WarehouseId > 0)
            {
                WhereClause += WarehouseId == 0 ? " AND RWM.WarehousId= RWM.WarehousId " : " AND RWM.WarehousId = " + WarehouseId.ToString();
            }
            else
            {
                if (UserCityId > 0)
                {
                    WhereClause += " AND RWM.ISCITY= 1  AND RWM.BlockCityId = " + UserCityId.ToString();
                }
                else
                {
                    WhereClause += " AND RWM.ISCITY= 0  AND RWM.BlockCityId = " + UserBlockId.ToString();
                }
            }


            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.RajfedWarehouseList(WhereClause));
        }

        [HttpPost]
        public string RajfedWarehouseList1(List<Params> parameters)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.RajfedWarehouseList(COMMON_FUNCTION.WhereClauseByParams(parameters)));
        }


        [HttpGet]
        public ActionResult Commodity()
        {
            return View();
        }

        [HttpPost]
        public int CommodityInsertUpdate(CommodityEntity model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
            model.ActionId = model.CommodityId > 0 ? 2 : 1;
            return objMasterModel.CommodityInsertUpdate(model);
        }

        [HttpPost]
        public int CommoditySetStatus(int CommodityId, int ActionId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.CommoditySetStatus(CommodityId, ActionId);
        }

        [HttpPost]
        public string CommodityList(int CommodityId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.RajfedCommodityMasterList(CommodityId));
        }

        [HttpGet]
        public ActionResult CommodityMSP()
        {
            return View();
        }

        [HttpPost]
        public int CommodityMSPInsertUpdate(CommodityMSPEntity model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            model.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
            model.ActionId = model.CommodityId > 0 ? 2 : 1;
            return objMasterModel.CommodityMSPInsertUpdate(model);
        }

        [HttpPost]
        public int CommodityMSPSetStatus(int CommodityMSPId, int ActionId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.CommodityMspSetStatus(CommodityMSPId, ActionId);
        }

        [HttpPost]
        public string CommodityMSPList(int CommodityMSPId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.CommodityMSPList(CommodityMSPId));
        }


        public string RajfedWarehouseDetails(int WarehouseId, bool IsCWC)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            string WhereClause = string.Empty;
            WhereClause += WarehouseId == 0 ? " AND RWM.WarehousId= RWM.WarehousId " : " AND RWM.WarehousId = " + WarehouseId.ToString();
            if (IsCWC)
            {
                WhereClause += " AND RWM.IsCentralWarehouse=1";
            }
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.RajfedWarehouseList(WhereClause));
        }

        public ActionResult Holiday()
        {
            return View();
        }

        [HttpGet]
        public ActionResult WrHoliday()
        {
            //UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.WrHoliday, (Request.Cookies[SessionIdentity.Culture]).Value);
            // }
            return View();
        }


        [HttpGet]
        [ActionName("PurchaseCenterList")]
        public string GetAllRajfed_PurchaseCenterList()
        {
            PurchaseCenterMasterModel objPurchaseCenterMasterModel = new PurchaseCenterMasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objPurchaseCenterMasterModel.GetAllPurchaseCenterList());
        }

        [HttpGet]
        public ActionResult Rajfed_PurchaseCenterList()
        {
            UserModel objModel = new UserModel(new FileLogger());
            bool result = objModel.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.PurchaseCenter, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SavePurchaseCenterForm")]
        public JsonResult Rajfed_PurchaseCenterInsert(int PurchaseCenterId, int DistrictId, string PurchaseCenterName_En, string PurchaseCenterName_Hi, int CommodityId, int IsActive)
        {
            PurchaseCenterMasterModel objModel = new PurchaseCenterMasterModel(new FileLogger());
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(objModel.AddPurchaseCenterMaster(PurchaseCenterId, DistrictId, PurchaseCenterName_En, PurchaseCenterName_Hi, CommodityId, IsActive));
        }

        [HttpPost]
        [ActionName("ChangedeleteStatusPurchaseCenter")]
        public JsonResult DeleteRajFed_PurchaseCenter(int Id, int ActionId)
        {
            ActionId = (ActionId == 0 ? 1 : (ActionId == 1 ? 0 : ActionId));

            PurchaseCenterMasterModel purchaseCenterMasterModel = new PurchaseCenterMasterModel(new FileLogger());
            return Json(purchaseCenterMasterModel.DeletePurchageCenter(Id, ActionId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetRoleByOrganizationLevelsMappingID")]
        public string GetRoleByOrganizationLevelsMappingID(int LevelId = 0)
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectRoleList(LevelId));
        }

        #region Bank Details


        [HttpGet]
        [ActionName("GetBankDetails")]
        public ActionResult GetBankDetails(string WhereSearchVal)
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ////Bank Master List
            //BankMasterModel bankMaster = new BankMasterModel();
            //List<BankMasterModel> bankMasterList = new List<BankMasterModel>();
            //bankMasterList = bankMaster.GetListofBankMaster();
            //ViewBag.BankMasterList = bankMasterList;

            ////Socienty Dropdown


            //BankDetailsMasterModel societyModel = new BankDetailsMasterModel();
            //List<BankDetailsMasterModel> societyList = new List<BankDetailsMasterModel>();
            //societyList = societyModel.GetSocietyListByPLDBID(_LoggedInUser.PLDBId);

            //List<BankDetailsMasterModel> PLDBBranch = new List<BankDetailsMasterModel>();


            ////List<BankDetailsMasterModel> societyList = new List<BankDetailsMasterModel>();
            //PLDBBranch= societyModel.GetBranchListByPLDBID(_LoggedInUser.PLDBId);
            //ViewBag.PLDBBranch = PLDBBranch;



            //ViewBag.SocietyMasterList = societyList;

            //string WhereClause = "";
            //BankDetailsMasterModel objBankDetailsMasterModel = new BankDetailsMasterModel(new FileLogger());
            //List<BankDetailsMasterModel> objBankDetailsList = new List<BankDetailsMasterModel>();
            //BankDetailsViewModel bankDetailsVM = new BankDetailsViewModel();

            //if (WhereSearchVal != "" && WhereSearchVal != null)
            //{
            //    WhereClause = " and ApplicationNo like '%" + WhereSearchVal + "%'" + "or bd.BankName like '%" + WhereSearchVal + "%'" + "or BankBranch like'%" + WhereSearchVal + "%'" + "or IFSCCode like'%"
            //        + WhereSearchVal + "%'" + "or BankAccountNo like'%" + WhereSearchVal + "%'" + "or AccHolderName like'%" + WhereSearchVal + "%'" + "or  bm.BankName like'%" + WhereSearchVal + "%'";
            //}
            //DataPagingModel Details = new DataPagingModel();
            //Details.PageNumber = 1;
            //Details.PageSize = 10;

            //bankDetailsVM.BankDetailsList = objBankDetailsMasterModel.GetMst_BankDetails(ref Details, WhereClause);
            //return View(bankDetailsVM);

            // ViewBag.DataPagingModel = Details;
            return View();//View(bankDetailsVM);
        }

        public ActionResult InsertUpdateBankDetails()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SaveBankDetails")]
        public string InsertUpdateBankDetails(BankDetailsViewModel objBankDetailsVM)
        {
            BankDetailsMasterModel objBankDetailsMasterModel = new BankDetailsMasterModel(new FileLogger());
            List<BankDetailsMasterModel> objBankDetailsList = new List<BankDetailsMasterModel>();
            BankDetailsViewModel BankDetailsVM = new BankDetailsViewModel();

            return objBankDetailsMasterModel.AddBankDetails(objBankDetailsVM).ToString();
            //if (result == 1)
            //{
            //    ViewBag.Message = "Record save successfully";
            //    return RedirectToAction("GetBankDetails");
            //}
            //else if (result == 2)
            //{
            //    ViewBag.Message = "Record update successfully";
            //    return RedirectToAction("GetBankDetails");
            //}
            //else
            //{
            //    return View();
            //}
        }


        [HttpPost]
        public JsonResult DeleteBankDetails(int Id, int ActionId, string Type)
        {
            ActionId = (ActionId == 0 ? 1 : (ActionId == 1 ? 0 : ActionId));

            BankDetailsMasterModel objBankDetailsMasterModel = new BankDetailsMasterModel(new FileLogger());
            return Json(objBankDetailsMasterModel.DeleteBankDetails(Id, ActionId, Type), JsonRequestBehavior.AllowGet);
        }

        public JsonResult _BankDetails(int id)
        {

            BankMasterModel bankMaster = new BankMasterModel();
            List<BankMasterModel> bankMasterList = new List<BankMasterModel>();
            bankMasterList = bankMaster.GetListofBankMaster();
            ViewBag.BankMasterList = bankMasterList;


            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            List<CitizenModel> SocietyList = new List<CitizenModel>();
            //SocietyList = objCitizenModel.GetSocietyList(objSocietyRegistrationEntity);
            ViewBag.SocietyMasterList = SocietyList;

            string WhereClause = "";
            BankDetailsMasterModel objBankDetailsMasterModel = new BankDetailsMasterModel(new FileLogger());
            List<BankDetailsMasterModel> objBankDetailsList = new List<BankDetailsMasterModel>();
            BankDetailsViewModel BankDetailsVM = new BankDetailsViewModel();

            DataPagingModel Details = new DataPagingModel();

            if (id > 0)
            {
                WhereClause = "and Id =" + id;
                Details.SearchText = WhereClause.ToString();

            }

            var dt = objBankDetailsMasterModel.GetMst_BankDetails(ref Details, WhereClause, 1);
            //BankDetailsVM.BankDetails = BankDetailsVM.BankDetailsList[0];
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);
            // return View("_BankDetails", BankDetailsVM);
        }

        [HttpPost]
        public JsonResult _BankDetailList(DataPagingModel Details, int SocietyType)
        {
            //MISModel objModel = new MISModel(new DBLogger());
            //DataTable DT = objModel.GetMonthlySummary(ref Details);


            string WhereClause = "";
            BankDetailsMasterModel objBankDetailsMasterModel = new BankDetailsMasterModel(new FileLogger());
            List<BankDetailsMasterModel> objBankDetailsList = new List<BankDetailsMasterModel>();
            BankDetailsViewModel bankDetailsVM = new BankDetailsViewModel();
            string WhereSearchVal = string.Empty;
            if (WhereSearchVal != "" && WhereSearchVal != null)
            {
                WhereClause = " and ApplicationNo like '%" + WhereSearchVal + "%'" + "or bd.BankName like '%" + WhereSearchVal + "%'" + "or BankBranch like'%" + WhereSearchVal + "%'" + "or IFSCCode like'%"
                    + WhereSearchVal + "%'" + "or BankAccountNo like'%" + WhereSearchVal + "%'" + "or AccHolderName like'%" + WhereSearchVal + "%'" + "or  bm.BankName like'%" + WhereSearchVal + "%'";
            }
            DataTable DT = objBankDetailsMasterModel.GetMst_BankDetails(ref Details, WhereClause, SocietyType);
            //return View(bankDetailsVM);

            //ViewBag.DataPagingModel = Details;
            //return PartialView("_BankDetailList", bankDetailsVM);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BankDetails()
        {
            return View();
        }


        public JsonResult GetDistrictDivisionReasion()
        {
            Masters obj = new Masters();
            var data = obj.GetDistrictDivisionReasion();
            return Json(new { List = data }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        public ActionResult AddPurchaseCenter()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SavePurchaseCenter(PurchaseCenterModel Data)
        {
            return Json((new ServicesController()).AddPurchaseCenter(Data), JsonRequestBehavior.AllowGet);
        }


        #region AuditDistrictId

        [HttpGet]
        [ActionName("GeAuditDistrictID")]
        public string GeAuditDistrictID(int DivisionId = 0)
        {
            MasterModel objAuditDistrict = new MasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objAuditDistrict.AuditDistrictList(DivisionId));
        }
        #endregion


        [HttpGet]
        public ActionResult Pacs()
        {
            //UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            //    ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            //    ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.District, (Request.Cookies[SessionIdentity.Culture]).Value);
            //}
            return View();
        }

        [HttpPost]
        public int SavePacsForm(Pacs _model)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return objMasterModel.InsertPacsData(_model);

        }

        [HttpPost]
        [ActionName("PacsList")]
        public string GetPacsList()
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.GetPacsList());
        }


        [HttpPost]
        [ActionName("InspectorList")]
        public string GetInspectorList(int DistrictId, int DivisionId, int OfficeId)
        {
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMasterModel.GetInspector(0, DistrictId, DivisionId, OfficeId));

        }

    }
}