using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class MasterSettingsController : Controller
    {
        // GET: MasterSettings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrganizationLevelsMappings()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SaveOrgLevelMapping")]
        public string SaveOrganizationLevelMapping(List<OrganizationLevelsMapping> lst)
        {
            OrganizationLevelsMapping obj = new OrganizationLevelsMapping();
            return CommonMessageFactory.Response(CommonMessageFactory.Message(obj.AddOrganizationLevelMappings(lst), (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        [HttpGet]
        [ActionName("OrganizationLevelsList")]
        public string GetAllOrganizationLevels(string WhereClause = "")
        {
            WhereClause = (WhereClause).ToString();
            WhereClause = (WhereClause == "undefined" ? "" : WhereClause);
            OrganizationLevelsMapping obj = new OrganizationLevelsMapping();
            return COMMON_FUNCTION.DataTableToJSONString(obj.GetAllOrganizationLevels(WhereClause));
        }

        [HttpPost]
        public string ChangeMappingStatus(string Id, string Action)
        {
            int MappingID = 0, ActionId = 0;
            if (!int.TryParse(Id, out MappingID) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            OrganizationLevelsMapping objModel = new OrganizationLevelsMapping(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objModel.ChangeStatus(MappingID, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        // Start DesignationLevelMappings
        [HttpGet]
        [ActionName("DesignationLevelMappingsList")]
        public string GetAllSubDesignationLevelMappingList(string WhereClause)
        {
            DesignationLevelMappingModel objDesignationLevelMapping = new DesignationLevelMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDesignationLevelMapping.GetAllDesignationLevelMappings(WhereClause));
        }

        [HttpGet]
        public ActionResult DesignationLevelMapping()
        {
          //  ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
           // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.DesignationLevelMapping, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpPost]
        [ActionName("SaveDesignationLevelMappingForm")]
        public JsonResult DesignationLevelMappinginsert( int DESIGNATIONCODE, string IsHOD, string IsActive, int DesignationLevelMappingId, int OrganizationLevelsMappingID,int OfficeId)
        {
            DesignationLevelMappingModel _Modal = new DesignationLevelMappingModel();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            return Json(_Modal.AddDesignationLevelMapping(DESIGNATIONCODE,  IsHOD, IsActive, DesignationLevelMappingId, OrganizationLevelsMappingID, OfficeId));

        }

        [HttpPost]
        [ActionName("ChangeStatusDesignationLevelMapping")]
        public JsonResult delete_recordDesignationLevelMapping(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));
            DesignationLevelMappingModel objDesignationLevelMappingModel = new DesignationLevelMappingModel(new DBLogger());
            return Json(objDesignationLevelMappingModel.ChangeStatusDesignationLevelMapping(id, Action), JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        [ActionName("GetAllDesignationByOrganizationLevelsMappingID")]
        public string GetAllDesignationByOrganizationLevelsMappingID(string OrganizationLevelsMappingID,string OfficeId)
        {
            DesignationLevelMappingModel objDesignationLevelMapping = new DesignationLevelMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDesignationLevelMapping.GetAllDesignationByOrganizationLevelsMappingID(OrganizationLevelsMappingID, OfficeId));
        }
        // End DesignationLevelMappings
    }
}