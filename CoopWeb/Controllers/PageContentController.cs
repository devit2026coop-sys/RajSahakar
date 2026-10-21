using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;

namespace Cooperative.Controllers
{
    public class PageContentController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: PageContent
        [HttpGet]
        [ActionName("PageContentList")]
        public string GetAllPageList(string WhereClause)
        {
            PageContentModel objFeedBackModel = new PageContentModel(new FileLogger());
            return JsonConvert.SerializeObject(objFeedBackModel.GetAllPageContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }
        [HttpGet]
        public ActionResult DynamicPageContent()
        {

            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FeedBacks, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }


        [HttpPost]
        [ActionName("SaveDynamicPage")]
        public JsonResult SaveDynamicPage(PageContentModel _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
           // if (!ModelState.IsValid)
           //     return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddPageContent());
           // return Json(_Model);
        }
    }
}