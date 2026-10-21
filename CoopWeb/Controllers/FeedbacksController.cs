using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class FeedbacksController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        [ActionName("FeedBacksList")]
        public string GetAllFeedBack(string WhereClause)
        {

            FeedbacksModel objFeedBackModel = new FeedbacksModel(new FileLogger());
             return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllFeedback());
        }
        [HttpGet]
        public ActionResult FeedBacks()
        {

            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FeedBacks, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }


        [HttpPost]
        [ActionName("SaveFeedBack")]
        public JsonResult MenuSmsActionInsert(FeedbacksModel _Model)
        {
            string[] Uid = _Model.UserIds.Split(',');
                      
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

           // return Json(_Model.AddFeedBack());
             return Json(_Model);
        }




        //[HttpPost]
        //[ActionName("DeleteandstatusFeedBack")]
        //public JsonResult delete_record(int id, int Action)
        //{
        //    Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));
        //    FeedbackModel objFeedBackModel = new FeedBackModel(new FileLogger());
        //    return Json(objFeedBackModel.DeleteChangeStatusFeedBack(id, Action), JsonRequestBehavior.AllowGet);
        //}

    }
}
