using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.IO;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    public class SmsformatController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: Smsformat
        #region Smsfomat
        [HttpGet]
        public ActionResult Smsformat()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SMSFormat, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Mailformat()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.MailFormat, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SaveSmsFormat")]
        public JsonResult MenuSmsActionInsert(SmsformatModel _Model)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            return Json(_Model.AddSmsFormat());
        }

        [HttpPost]
        public string ChangeStatusSMS(string Id, string Action)
        {
            int TemplateId = 0, ActionId = 0;
            if (!int.TryParse(Id, out TemplateId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SmsformatModel objSmsformatModel = new SmsformatModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSmsformatModel.ChangeStatusSms(TemplateId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }


        [HttpPost]
        [ActionName("SaveMailFormat")]
        public JsonResult MenuMailActionInsert(SmsformatModel _Model)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            return Json(_Model.AddMailFormat());
        }
        //[HttpGet]
        //[ActionName("GetSms")]
        //public string GetSmsWorkflow()
        //{
        //    WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(_LoggedInUser.RoleId, "/Smsformat/Smsformat"));
        //}
        [HttpGet]
        [ActionName("SmsFormatList")]
        public JsonResult GetAllSmsFormatList()
        {
            SmsformatModel objMenuActionModel = new SmsformatModel(new DBLogger());
            return Json(objMenuActionModel.GetAllSmsFormat(), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //[ActionName("DeleteTemplate")]
        //public JsonResult delete_record(int id)
        //{
        //    string res = string.Empty;

        //    SmsformatModel objMenuActionModel = new SmsformatModel(new DBLogger());
        //    return Json(objMenuActionModel.DeleteTemplate(id), JsonRequestBehavior.AllowGet);

        //}


        [HttpGet]
        [ActionName("MailFormatList")]
        public JsonResult GetAllMailFormatList()
        {
            SmsformatModel objMenuActionModel = new SmsformatModel(new DBLogger());
            return Json(objMenuActionModel.GetAllMailFormat(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FileUpload()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveFiles()
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Request.Files != null)
            {
                var file = Request.Files[0];
                actualFileName = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;

                try
                {
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/UploadedFiles"), fileName));
                    Message = "File uploaded successfully";
                    flag = true;
                  
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }

            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
        
        #endregion
    }
}