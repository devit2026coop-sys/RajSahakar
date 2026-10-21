using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Data;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class LovController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        [HttpGet]
        public ActionResult LOV()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.LOV, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        } 

        [HttpGet]
        public ActionResult LovValue()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.LOVValue, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SaveLov")]
        public string LovInsert(LovModel dat)
        {
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            return dat.AddLov().ToString();
        }

        [HttpPost]
        [ActionName("SaveLovValue")]
        public string LovValueInsert(LovModel _Model)
        {
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            return _Model.SaveLovValues().ToString();
        }

        [HttpPost]
        [ActionName("ChangeStatus")]
        public string ChangeStatus(string Id, string Status)
        {
            int _id = 0, _Status = 0;
            if (!(int.TryParse(Id, out _id) && int.TryParse(Status, out _Status)))
                return "{}";

            LovModel objLovModel = new LovModel(new DBLogger());
            return Convert.ToString(objLovModel.ChangeStatus(_id, _Status));
        }

        [HttpPost]
        [ActionName("ChangeStatusLov")]
        public string ChangeStatusLov(string Id, string Status)
        {
            int _id = 0, _Status = 0;
            if (!(int.TryParse(Id, out _id) && int.TryParse(Status, out _Status)))
                return "{}";

            LovModel objLovModel = new LovModel(new DBLogger());
            return Convert.ToString(objLovModel.ChangeLovValueStatus(_id, _Status));
        }

        [HttpGet]
        [ActionName("LovList")]
        public string GetAllLov()
        {
            LovModel objLovModel = new LovModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objLovModel.GetAllLov(_LoggedInUser.RoleId, "/Lov/Lov"));
           
        }

        [HttpGet]
        [ActionName("GetLov")]
        public string GetLovWorkflow()
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(_LoggedInUser.RoleId, "/Lov/Lov"));
        }

        [HttpGet]
        [ActionName("LovValueList")]
        public string GetAllLovValues()
        {
            LovModel objLovModel = new LovModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objLovModel.GetAllLovValues());
        }
        [HttpGet]
        [ActionName("GetLovValuesByListId")]
        public string GetLovValuesByListId(int ListId=0)
        {
            DataTable DT = new SocietyModel().GetLovValuesByListId(ListId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
    }
}