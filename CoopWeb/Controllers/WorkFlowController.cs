using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Web;
using System.Web.Mvc;


namespace Cooperative.Controllers
{
    [Error]
    //[LogActionFilter]
    //[CustomAuthentication]
    public class WorkFlowController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        public ActionResult WorkFlow()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.WorkFlow, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        [ActionName("Getworkflow")]
        public string Getworkflow()
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(_LoggedInUser.RoleId, "/WorkFlow/WorkFlow"));
        }

        [HttpPost]
        [ActionName("WorkFlowConfiguration")]
        public string SaveWorkFlowConfiguration(WorkFlowModel _Model)
        {
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            _Model.CreatedBy = Convert.ToInt32(_LoggedInUser.UserId);
            return _Model.SaveWorkFlowConfiguration().ToString();
        }

        [HttpGet]
        [ActionName("WorkFlowAction")]
        public string WorkFlowAction(string WhereClause = "")
        {
            WorkFlowModel objWorkFl = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFl.WorkFlowExecutionAction(WhereClause));
        }

        [HttpGet]
        [ActionName("WorkFlowList")]
        public string GetWorkFlowList()
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetWorkFlowList());
        }

        [HttpPost]
        [ActionName("ChangeWorkFlowStatus")]
        public string ChangeWorkFlowStatus(string Id, string Action)
        {
            int _id = 0, _ActionId = 0;
            if (!(int.TryParse(Id, out _id) && int.TryParse(Action, out _ActionId)))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return objWorkFlowModel.ChangeWorkFlowStatus(_id, _ActionId).ToString();
        }


        public string GetAllWorkflow(int RoleId, string url)
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(RoleId, url));
        }

        [HttpPost]
        public string WorkFlowActionList(int TypeId=0, string RequestId="",string MenuURL="")
        {
            int _RequestId = 0;
            //string url = Request.Url.LocalPath;
            int.TryParse(RequestId, out _RequestId);
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.WorkFlowActionList(TypeId, _RequestId, _LoggedInUser.RoleId, MenuURL));
        }

        [HttpPost]
        public string WorkFlowNextRoleList(int TypeId, int ActionId, string MenuURL = "")
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.WorkFlowNextRoleList(TypeId, ActionId, _LoggedInUser.RoleId, MenuURL));
        }

        [HttpPost]
        public string WorkFlowNextUserList(int TypeId, int RoleId)
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.WorkFlowNextUserList(TypeId, RoleId, _LoggedInUser.RoleId));
        }
    }
}