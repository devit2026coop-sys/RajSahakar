using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperative.Filters;

namespace Cooperative.Controllers
{
    public class WorkFlowMasterController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: WorkFlowMaster
        public ActionResult Index()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.WorkFlowConfiguration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        public ActionResult WorkFlowMaster()
        {
            return View();
        }

        public ActionResult SeprateModuleWorkFlowMaster()
        {
            return View();
        }

        [HttpGet]
        [ActionName("WorkFlowConfigList")]
        public string WorkFlowConfigList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.WorkFlowConfigList());
        }

        [HttpGet]
        [ActionName("SpetModuleWorkFlowConfigList")]
        public string SpetModuleWorkFlowConfigList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.SpetModuleWorkFlowConfigList());
        }


        [HttpGet]
        [ActionName("WorkFlowDetails")]
        public string WorkFlowDetails(int workflowmasterid)
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.WorkFlowDetails(workflowmasterid));
        }

        [HttpPost]
        [ActionName("ChangeStatusWorkflow")]
        public JsonResult ChangeStatusWorkflow(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

            WorkflowmasterModel objWorkFlowModel = new WorkflowmasterModel(new DBLogger());
            return Json(objWorkFlowModel.ChangeStatusWorkflow(id, Action), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ActionName("ModuleList")]
        //public string ModuleList(int  SchemeId )
        //{
        //    WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

        //    return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.ModuleListBySchemeId(SchemeId));
        //}
        [HttpGet]
        [ActionName("ModuleList")]
        public string ModuleList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.ModuleList());
        }


        [HttpPost]
        [ActionName("SubModuleList")]
        public string SubModuleList(string WhereClause)
        {
            WhereClause = WhereClause.Replace("object:", "");
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.SubModuleList(WhereClause));
        }

        [HttpPost]
        [ActionName("SeptSubModuleList")]
        public string SeptSubModuleList(int DepartmentId = 0)
        {          
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.SeptSubModuleList(DepartmentId));
        }

        //[HttpPost]
        //[ActionName("SubModuleList")]
        //public string SubModuleList(int SchemeId, int ModuleId)
        //{
        //    //WhereClause = WhereClause.Replace("object:", "");
        //    WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
        //    return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.CommonSubModuleList(SchemeId, ModuleId));
        //}

        [HttpGet]
        [ActionName("LevelList")]
        public string LevelList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.LevelList());
        }

       

        [HttpGet]
        [ActionName("ActionList")]
        public string ActionList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.ActionList());
        }

        [HttpGet]
        [ActionName("GodownActionList")]
        public string GodownActionList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.GodownActionList());
        }

        [HttpGet]
        [ActionName("ActionHeadList")]
        public string ActionHeadList()
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objModulemaster.ActionHeadList());
        }

        [HttpPost]
        [ActionName("SaveWorkFlow")]
        public JsonResult InsertCaseRegistraion(WorkflowmasterModel _Modal)
        {
            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return Json(objModulemaster.SaveWorkFlowConfiguration(_Modal));

        }


        [HttpPost]
        [ActionName("SaveWorkFlowDetails")]
        public JsonResult InsertAdvocateDetails(WorkFlowDetails _Modal)
        {

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Modal.lstWorkFlowScope = Request.Form["lstWorkFlowScope"].ToString().Equals("[]") ? new List<WorkFlowScopeEntity>() : (List<WorkFlowScopeEntity>)s.Deserialize(Request.Form["lstWorkFlowScope"].ToString(), typeof(List<WorkFlowScopeEntity>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            WorkflowmasterModel objModulemaster = new WorkflowmasterModel(new FileLogger());
            return Json(objModulemaster.SaveWorkFlowDetailsConfiguration(_Modal));

        }

    }
}