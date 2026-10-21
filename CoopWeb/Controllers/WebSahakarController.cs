using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data;

namespace Cooperative.Controllers
{
    public class WebSahakarController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: WebSahakar
        [CustomAuthentication]
        [LogActionFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region DynamicPageContent

        [HttpGet]
        public ActionResult DynamicPageContent()
        {

            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.DynamicPageContent, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }


        // GET: PageContent
        [HttpGet]
        [ActionName("PageContentList")]
        public string GetAllPageList(string WhereClause)
        {
            WebSahakarModel objFeedBackModel = new WebSahakarModel(new FileLogger());
            return JsonConvert.SerializeObject(objFeedBackModel.GetAllPageContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("SaveDynamicPage")]
        [ValidateInputAttribute(true)]
        public JsonResult SaveDynamicPage(WebSahakarModel _Model)
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

        [HttpPost]
        [ActionName("DeleteandstatusPages")]
        public JsonResult delete_record(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            //  Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));
            WebSahakarModel objFeedBackModel = new WebSahakarModel(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusPageContent(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion DynamicPageContent

        #region NewsContent

        [HttpGet]
        public ActionResult NewsContent()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.NewsContent, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("NewsContentList")]
        public string GetAllNewsList(string WhereClause)
        {
            NewsEntity objWebSahakarModel = new NewsEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllNewsContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadNewsContent")]
        public string UploadNewsContent(NewsEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/News/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveNewsContent")]
        [ValidateInputAttribute(true)]
        public JsonResult SaveNewsContent(NewsEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            _Model.NewsDate = DateTime.Now;

            return Json(_Model.AddNewsContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusNews")]
        public JsonResult delete_newsrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            NewsEntity objFeedBackModel = new NewsEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusNewsContent(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion NewsContent

        #region CoopertativeInstitution

        [HttpGet]
        public ActionResult CooperativeInstitution()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.CoopertativeInstitution, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("CoopertativeInstitutionList")]
        public string GetAllCoopertativeInstitutionList(string WhereClause)
        {
            CoopertativeInstitutionEntity objWebSahakarModel = new CoopertativeInstitutionEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllCoopertativeInstitutionContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadCoopertativeInstitution")]
        public string UploadCoopertativeInstitution(CoopertativeInstitutionEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.ImagePath = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Cooperatives/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveCoopertativeInstitution")]
        public JsonResult SaveCoopertativeInstitution(CoopertativeInstitutionEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddCoopertativeInstitutionContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusCoopertativeInstitution")]
        public JsonResult delete_CoopertativeInstitutionsrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            CoopertativeInstitutionEntity objFeedBackModel = new CoopertativeInstitutionEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusCoopertativeInstitutionContent(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion CoopertativeInstitution


        #region Coopertative

        [HttpGet]
        public ActionResult Cooperative()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Coopertatives, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("CooperativeList")]
        public string GetAllCoopertativeList(string WhereClause)
        {
            CoopertativeEntity objWebSahakarModel = new CoopertativeEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllCoopertativeContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadCoopertative")]
        public string UploadCoopertative(CoopertativeEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Cooperatives/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveCoopertative")]
        public JsonResult SaveCoopertative(CoopertativeEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddCoopertativeContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusCoopertative")]
        public JsonResult delete_Coopertativesrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            CoopertativeEntity objFeedBackModel = new CoopertativeEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusCoopertative(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Coopertative

        #region  Downloads

        [HttpGet]
        public ActionResult Downloads()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Downloads, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult TelePhoneDirectory()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TelephoneDir, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("DownloadsList")]
        public string GetAllDownloadsList(string WhereClause)
        {
            DownloadsEntity objWebSahakarModel = new DownloadsEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllDownloadsContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadDownloads")]
        public string UploadDownloads(DownloadsEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/downloads/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveDownloads")]
        public JsonResult SaveDownloads(DownloadsEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddDownloadsContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusDownloads")]
        public JsonResult delete_Downloadsrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            DownloadsEntity objFeedBackModel = new DownloadsEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusDownloads(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Coopertative

        #region  Fertilizer

        [HttpGet]
        public ActionResult Fertilizer()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Fertilizer, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("FertilizerList")]
        public string GetAllFertilizerList(string WhereClause)
        {
            FertilizerEntity objWebSahakarModel = new FertilizerEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllFertilizerContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadFertilizer")]
        public string UploadFertilizer(FertilizerEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Fertilizer/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveFertilizer")]
        public JsonResult SaveFertilizer(FertilizerEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddFertilizerContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusFertilizer")]
        public JsonResult delete_Fertilizerrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            FertilizerEntity objFeedBackModel = new FertilizerEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusFertilizer(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Coopertative

        #region  Message

        [HttpGet]
        public ActionResult Message()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Message, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("MessageList")]
        public string GetAllMessageList(string WhereClause)
        {
            MessageEntity objWebSahakarModel = new MessageEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllMessageContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadMessage")]
        public string UploadMessage(MessageEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Message/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveMessage")]
        public JsonResult SaveMessage(MessageEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddMessageContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusMessage")]
        public JsonResult delete_Messagerecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            MessageEntity objFeedBackModel = new MessageEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusMessage(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Coopertative


        #region  OrderCirculars

        [HttpGet]
        public ActionResult OrderandCirculars()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.OrderCirculars, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        public ActionResult OrderCircularSSOMAP()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.OrderCirculars, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("OrderCircularsList")]
        public string GetAllOrderCircularsList(string catid)
        {
            OrderCircularsEntity objWebSahakarModel = new OrderCircularsEntity(new FileLogger());

            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllOrderCircularsContent(catid), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }

        [ActionName("OrderCircularsSSOList")]
        public string GetAllOrderCircularsSSOList(string catid)
        {
            OrderCircularsEntity objWebSahakarModel = new OrderCircularsEntity(new FileLogger());

            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllOrderCircularsSSOContent(catid), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }

        [HttpGet]
        [ActionName("OrderCircularsCatList")]
        public string GetAllOrderCircularsCatList()
        {
            string ssoid = _LoggedInUser.SSOID.ToString();
            OrderCircularsEntity objWebSahakarModel = new OrderCircularsEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllOrderCircularsCatListContent(ssoid), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadOrderCirculars")]
        public string UploadOrderCirculars(OrderCircularsEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/OrderCirculars/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveOrderCirculars")]
        public JsonResult SaveOrderCirculars(OrderCircularsEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddOrderCircularsContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusOrderCirculars")]
        public JsonResult delete_OrderCircularsrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            OrderCircularsEntity objFeedBackModel = new OrderCircularsEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusOrderCirculars(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("DeleteSSOOrderCirculars")]
        public JsonResult delete_OrderCircularsSSOrecord(int id, int Action)

        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            OrderCircularsEntity objFeedBackModel = new OrderCircularsEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteSSOOrderCirculars(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("OrderCircularSSOMAP")]
        public JsonResult OrderCircularSSOMAP(OrderCircularsDetails _Modal)
        {

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Modal.lstOrderSSOMAPScope = Request.Form["lstOrderSSOMAPScope"].ToString().Equals("[]") ? new List<OrderSSOMAPEntity>() : (List<OrderSSOMAPEntity>)s.Deserialize(Request.Form["lstOrderSSOMAPScope"].ToString(), typeof(List<OrderSSOMAPEntity>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            _Modal.UserID = Convert.ToInt32(_LoggedInUser.UserId);
            OrderCircularsEntity _citobj = new OrderCircularsEntity(new FileLogger());
            return Json(_citobj.AddSSOMAPDetails(_Modal));

        }

        #endregion OrderCirculars

        #region  TelephoneDir


        [HttpGet]
        [ActionName("GetTelePhoneCatList")]
        public string GetTelePhoneCatList()
        {
            WebSahakarModel objFeedBackModel = new WebSahakarModel(new FileLogger());
            return JsonConvert.SerializeObject(objFeedBackModel.GetTelePhoneCat(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }

        [HttpGet]
        [ActionName("GetTelePhoneList")]
        public string GetTelePhoneCatList(int CatId)
        {
            WebSahakarModel objFeedBackModel = new WebSahakarModel(new FileLogger());
            return JsonConvert.SerializeObject(objFeedBackModel.GetTelePhoneListByCAT(CatId), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }

        [HttpPost]
        [ActionName("SaveTelePhoneDirectory")]
        public JsonResult SaveTelePhoneDirectory(WebSahakarModel _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddTelePhoneDirectoryContent());
            // return Json(_Model);
        }

        [HttpPost]
        public string DeleteandstatusTelePhone(string Id, string Action)
        {
            int ClassId = 0, ActionId = 0;
            if (!int.TryParse(Id, out ClassId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            WebSahakarModel objSocietyModel = new WebSahakarModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSocietyModel.DeleteandstatusTelePhone(ClassId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        #endregion TelephoneDir

        #region  Transfers

        [HttpGet]
        public ActionResult Transfers()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Transfers, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("TransfersList")]
        public string GetAllTransfersList(string WhereClause)
        {
            TransfersEntity objWebSahakarModel = new TransfersEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllTransfersContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadTransfers")]
        public string UploadTransfers(TransfersEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Transfer/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveTransfers")]
        public JsonResult SaveTransfers(TransfersEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddTransfersContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusTransfers")]
        public JsonResult delete_Transfersrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            TransfersEntity objFeedBackModel = new TransfersEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusTransfers(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Transfers

        #region  Righttoinformation

        [HttpGet]
        public ActionResult Righttoinformation()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Righttoinformation, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("RighttoinformationList")]
        public string GetAllRighttoinformationList(string WhereClause)
        {
            RighttoinformationEntity objWebSahakarModel = new RighttoinformationEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllRighttoinformationContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadRighttoinformation")]
        public string UploadRighttoinformation(RighttoinformationEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/righttoinformation/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveRighttoinformation")]
        public JsonResult SaveRighttoinformation(RighttoinformationEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddRighttoinformationContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusRighttoinformation")]
        public JsonResult delete_Righttoinformationrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            RighttoinformationEntity objFeedBackModel = new RighttoinformationEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusRighttoinformation(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion Righttoinformation

        #region  Prizes

        [HttpGet]
        public ActionResult Prizes()
        {
            //ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Prizes, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("PrizesList")]
        public string GetAllPrizesList(string WhereClause)
        {
            PrizesEntity objWebSahakarModel = new PrizesEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllPrizesContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadPrizes")]
        public string UploadPrizes(PrizesEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/prizes/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SavePrizes")]
        public JsonResult SavePrizes(PrizesEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddPrizesContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusPrizes")]
        public JsonResult delete_Prizesrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            PrizesEntity objFeedBackModel = new PrizesEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusPrizes(id, ActionId), JsonRequestBehavior.AllowGet);
        }



        #endregion Prizes

        #region  PhotoGallary

        [HttpGet]
        public ActionResult PhotoGallary()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.PhotoGallary, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("PhotoGallaryList")]
        public string GetAllPhotoGallaryList(string WhereClause)
        {
            PhotoGallaryEntity objWebSahakarModel = new PhotoGallaryEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllPhotoGallaryContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadPhotoGallary")]
        public string UploadPrizes(PhotoGallaryEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/GalleryImages/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SavePhotoGallary")]
        public JsonResult SavePhotoGallary(PhotoGallaryEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddPhotoGallaryContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusPhotoGallary")]
        public JsonResult delete_PhotoGallaryrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            PhotoGallaryEntity objFeedBackModel = new PhotoGallaryEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusPhotoGallary(id, ActionId), JsonRequestBehavior.AllowGet);
        }



        #endregion PhotoGallary

        #region  Bulletien

        [HttpGet]
        public ActionResult Bulletien()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Bulletien, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("BulletienList")]
        public string GetAllBulletienList(string WhereClause)
        {
            BulletienEntity objWebSahakarModel = new BulletienEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllBulletienContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadBulletien")]
        public string UploadPrizes(BulletienEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Bulletien/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveBulletien")]
        public JsonResult SaveBulletien(BulletienEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddBulletienContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusBulletien")]
        public JsonResult delete_Bulletienrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            BulletienEntity objFeedBackModel = new BulletienEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusBulletien(id, ActionId), JsonRequestBehavior.AllowGet);
        }



        #endregion Bulletien

        #region  Lawandrules

        [HttpGet]
        public ActionResult Lawandrules()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Lawandrules, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("LawandrulesList")]
        public string GetAllLawandrulesList(string WhereClause)
        {
            LawandrulesEntity objWebSahakarModel = new LawandrulesEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllLawandrulesContent(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }



        [HttpPost]
        [ActionName("UploadLawandrules")]
        public string UploadPrizes(LawandrulesEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Cooperatives/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        [HttpPost]
        [ActionName("SaveLawandrules")]
        public JsonResult SaveLawandrules(LawandrulesEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddLawandrulesContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusLawandrules")]
        public JsonResult delete_Lawandrulesrecord(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            PhotoGallaryEntity objFeedBackModel = new PhotoGallaryEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusPhotoGallary(id, ActionId), JsonRequestBehavior.AllowGet);
        }



        #endregion PhotoGallary

        [HttpPost]
        [ActionName("UploadPageContent")]
        public string UploadPageContent(WebSahakarModel _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.Image = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/News/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }




        [HttpGet]
        public ActionResult PM_KISSAN_DATA()
        {
            return View();
        }

        // GET: NewsContent
        [HttpGet]
        [ActionName("PMKissanSMSList")]
        public string PMKissanDataList(string WhereClause)
        {
            PM_KISAN_SMS objPM_KISAN_SMSModel = new PM_KISAN_SMS(new FileLogger());
            return JsonConvert.SerializeObject(objPM_KISAN_SMSModel.PMKissanSMSList(), Newtonsoft.Json.Formatting.Indented);
        }


        [HttpPost]
        [ActionName("SavePMKissanDataForSMS")]
        public JsonResult SavePMKissanData(PM_KISAN_SMS _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();
            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            return Json(_Model.SavePMKissanSMSData());
        }

        public int SendSMSPS()
        {
            int Result = 0;
            try
            {
                SendMSGForAdmin("9602367111"); //PS SIR
                SendMSGForAdmin("9414119911"); //RCS  (NIRAJ KUMAR PAWAN)
                SendMSGForAdmin("9694418180");  //Umesh Sir
                SendMSGForAdmin("9414035286"); // K N Sharma JI
                SendMSGForAdmin("9782699967"); //Satish Ji
                SendMSGForAdmin("9461569464"); //RAJEEV
                Result = 1;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "SendSMSPS()" }, ex);
            }
            finally
            {

            }
            return Result;
        }
        public static void SendMSGForAdmin(string MobileNo)
        {
            try
            {
                PM_KISAN_SMS objPM_KISAN_SMSModel = new PM_KISAN_SMS(new FileLogger());
                DataTable Dt = objPM_KISAN_SMSModel.PMKissanSMSList();
                if (Dt.Rows.Count > 0)
                {
                    //string SMS_MSG = "Total application recieved:5813618 \nTotal Uploaded on PM Kishan:49.48 L \nDisbursement 1st installment:32.81 L\nDisbursement 2nd installment:21.84 L";  //07-08-2019
                    //string SMS_MSG = "Total application recieved:"+ Dt.Rows[0]["TotalApplication"].ToString()  + " Lacs \nTotal Uploaded on PM Kisan:" + Dt.Rows[0]["TotalUploded"].ToString() + " Lacs \nDisbursement 1st installment:" + Dt.Rows[0]["DisbursementI"].ToString() + " Lacs \nDisbursement 2nd installment:" + Dt.Rows[0]["DisbursementII"].ToString()+ " Lacs \nDisbursement 3rd installment:" + Dt.Rows[0]["DisbursementIII"].ToString() + " Lacs \nDisbursement 4th installment:" + Dt.Rows[0]["DisbursementIV"].ToString() + " Lacs";  //20-08-2019
                    string SMS_MSG = "Total application recieved:" + Dt.Rows[0]["TotalApplication"].ToString() + " Lacs \nTotal Uploaded on PM Kisan:" + Dt.Rows[0]["TotalUploded"].ToString() + " Lacs \nDisbursement 1st installment:" + Dt.Rows[0]["DisbursementI"].ToString() + " Lacs \nDisbursement 2nd installment:" + Dt.Rows[0]["DisbursementII"].ToString() + " Lacs \nDisbursement 3rd installment:" + Dt.Rows[0]["DisbursementIII"].ToString() + " Lacs ";  //20-08-2019
                    if (MobileNo.Length > 5 && SMS_MSG.Length > 5)
                    {
                        EsanchaarController EC = new EsanchaarController();
                        string ReturnVal = EC.SendSocietySMS(SMS_MSG, MobileNo);
                    }
                }
            }
            catch (Exception ex)
            {
                // objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "SendMSG(string SocietyIDEnc ,int StatusId)" }, ex);
            }
            finally
            {
                // objDBAccess.CloseConnection();
                // dt.Dispose();
            }

        }




        [HttpPost]
        public string UploadLoanFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/News");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(fileDirectory, fileName));
                        //MemberData[0].Photo = fileName;
                    }
                }
                catch (System.Exception ex)
                {
                    return "";
                }
            }

            return fileName;

        }



        #region  Lawandrules 2001

        public ActionResult RulesLowModal()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SaveLawandrulesModel")]
        public JsonResult SaveLawandrulesModel(LawandrulesModelEntity _Model)
        {
            string Uid = _LoggedInUser.UserId.ToString();

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddLawandrulesModelContent());
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("DeleteandstatusLawandrulesModel")]
        public JsonResult delete_LawandrulesrecordModel(int id, int Action)
        {
            int ActionId = 0;
            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;

            PhotoGallaryEntity objFeedBackModel = new PhotoGallaryEntity(new FileLogger());
            return Json(objFeedBackModel.DeleteChangeStatusPhotoGallary(id, ActionId), JsonRequestBehavior.AllowGet);
        }

        // GET: NewsContent
        [HttpGet]       
        public string GetAllLawandrulesModalList(string WhereClause)
        {
            LawandrulesEntity objWebSahakarModel = new LawandrulesEntity(new FileLogger());
            return JsonConvert.SerializeObject(objWebSahakarModel.GetAllLawandrulesModalList(), Newtonsoft.Json.Formatting.Indented);
            // return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllPageContent());
        }

        [HttpPost]
        [ActionName("UploadLawandrulesModel")]
        public string UploadLawandrulesModel(LawandrulesModelEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string fileDirectory = Server.MapPath("~/Content/Cooperatives");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }


                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/Cooperatives/"), fileName));
                }
                catch { }
            }
            return JsonConvert.SerializeObject(fileName);
        }

        #endregion  Lawandrules 2001

    }
}