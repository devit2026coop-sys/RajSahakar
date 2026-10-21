using CoopDAL;
using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using java.security;
using javax.swing;
using Newtonsoft.Json;
using sun.misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;


namespace Cooperative.Controllers
{
    [LogActionFilter]
    [CustomAuthentication]
    [Error]
    public class UserController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        #region USER
        [HttpGet]
        public ActionResult DashBoard()
        {

            UserModel OBJUser = new UserModel(new FileLogger());

            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("", 0));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            string DashBoardURL = _LoggedInUser.DashBoardURL.ToString();
            int OrganizationId = _LoggedInUser.OrganizationId;
            if (OrganizationId == 1 && _LoggedInUser.LevelId < 3)
                return View("ApexDashboard");

            else if (DashBoardURL != "")
            {
                if (DashBoardURL.Contains("/"))
                {
                    return RedirectToAction(DashBoardURL.Split('/')[1], DashBoardURL.Split('/')[0]);
                }
                else
                {
                    return View(DashBoardURL);
                }
            }
            else
                return View();


        }
        [HttpGet]
        public ActionResult UserPermission() { return View(); }

        [HttpGet]
        public ActionResult DashBoard_new()
        {

            UserModel OBJUser = new UserModel(new FileLogger());

            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("", 0));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            string DashBoardURL = "DashBoard_new"; //_LoggedInUser.DashBoardURL.ToString();
            int OrganizationId = _LoggedInUser.OrganizationId;
            if (OrganizationId == 1 && _LoggedInUser.LevelId < 3)
                return View("ApexDashboard");

            else if (DashBoardURL != "")
            {
                if (DashBoardURL.Contains("/"))
                {
                    return RedirectToAction(DashBoardURL.Split('/')[1], DashBoardURL.Split('/')[0]);
                }
                else
                {
                    return View(DashBoardURL);
                }
            }
            else
                return View();


        }

        [HttpGet]
        public ActionResult DashBoard_new1()
        {

            UserModel OBJUser = new UserModel(new FileLogger());

            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("", 0));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            string DashBoardURL = "DashBoard_new1"; //_LoggedInUser.DashBoardURL.ToString();
            int OrganizationId = _LoggedInUser.OrganizationId;
            if (OrganizationId == 1 && _LoggedInUser.LevelId < 3)
                return View("ApexDashboard");

            else if (DashBoardURL != "")
            {
                if (DashBoardURL.Contains("/"))
                {
                    return RedirectToAction(DashBoardURL.Split('/')[1], DashBoardURL.Split('/')[0]);
                }
                else
                {
                    return View(DashBoardURL);
                }
            }
            else
                return View();


        }


        public ActionResult GCCDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DashBoardNew()
        {
            return View();
        }

        [HttpGet]
        public ActionResult WebDashboard()
        {
            return View();
        }

        [HttpGet]
        public string GetWebDashboard()
        {
            UserModel objUserModel = new UserModel(new DBLogger());
            var final = objUserModel.GetWebDashboardData();
            return COMMON_FUNCTION.DataTableToJSONString(final);
        }


        [HttpGet]
        public ActionResult ElectionDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ApexDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserProfile, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult UserRegistrationRequestApprovable()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [ChildActionOnly]
        [ActionName("MyMenuGenerator")]
        public ActionResult MenuDetailsByUserId(string UserId, string RoleId)
        {
            UserModel objUserModel = new UserModel(new DBLogger());
            DataTable dt = objUserModel.GetMenuDetailsByUserId(Convert.ToInt32(UserId), Convert.ToInt32(RoleId), _LoggedInUser.UserRoleDeptMappingId);
            if (UserId == "3111")
            {
                if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
                {
                    if (Request.Cookies["SSOROLE"].Value == "KIOSK")
                    {

                        if (Request.Cookies["EmitraServiceid"] != null)
                        {
                            if (Request.Cookies["EmitraServiceid"].Value == "4172")
                            {
                                foreach (DataRow Dr in dt.Rows)
                                {


                                }
                            }
                            else if (Request.Cookies["EmitraServiceid"].Value == "5530")//REG/PRoffiling/Objection /Mapping 
                            {
                                DataTable tblFiltered = null;
                                tblFiltered = dt.AsEnumerable().Where(row => row.Field<int>("MenuId") == 104 || row.Field<int>("MenuId") == 61).CopyToDataTable();

                                dt.Clear();

                                dt = tblFiltered;
                            }
                            else if (Request.Cookies["EmitraServiceid"].Value == "5570")//Credit Cooperative Society Complaint Form
                            {
                                DataTable tblFiltered = null;
                                tblFiltered = dt.AsEnumerable().Where(row => row.Field<int>("MenuId") == 21368 || row.Field<int>("MenuId") == 61).CopyToDataTable();
                                dt.Clear();
                                dt = tblFiltered;
                            }
                            else if (Request.Cookies["EmitraServiceid"].Value == "5691") //Upload Bank Statement
                            {
                                DataTable tblFiltered = null;
                                tblFiltered = dt.AsEnumerable().Where(row => row.Field<int>("MenuId") == 21390 || row.Field<int>("MenuId") == 101 || row.Field<int>("MenuId") == 21272 || row.Field<int>("MenuId") == 61).CopyToDataTable();
                                dt.Clear();
                                dt = tblFiltered;
                            }
                            else if (Request.Cookies["EmitraServiceid"].Value == "5531")//Fee Payments/Verification
                            {
                                DataTable tblFiltered = null;
                                tblFiltered = dt.AsEnumerable().Where(row => row.Field<int>("MenuId") == 102 || row.Field<int>("MenuId") == 101 || row.Field<int>("MenuId") == 21272).CopyToDataTable();
                                dt.Clear();
                                dt = tblFiltered;
                            }
                            //else if (Request.Cookies["EmitraServiceid"].Value == "5570")
                            //{

                            //}
                        }
                    }
                }

            }
            List<MenuIterate> list = new List<MenuIterate>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (var item in dt.AsEnumerable())
                {
                    list.Add(new MenuIterate() { Id = item.Field<int>("MenuId"), IsParent = item.Field<string>("MenuUrl") == "#" ? true : false, MenuNameEn = item.Field<string>("MenuNameEn"), MenuNameHI = item.Field<string>("MenuNameHi"), ParentId = item.Field<int>("ParentId"), MenuUrl = item.Field<string>("MenuUrl"), MenuIcon = item.Field<string>("MenuIcon"), Priority = item.Field<int>("Priority"), MenuLevel = item.Field<int>("MenuLevel") });
                }
            }
            //string MenuItem = "<ul class=\"menu-bar navbar-nav list-unstyled components\">";
            //MenuItem += Menu(list, 0);
            //MenuItem += "</ul>";
            //ViewBag.DataMenu = MenuItem;

            string MenuItem = Menu(list, 0);
            ViewBag.DataMenu = MenuItem;

            return PartialView("_UserMenu", MenuItem);
        }


        public string Menu_1(List<MenuIterate> data, int MenuId)
        {
            string language = (Request.Cookies[SessionIdentity.Culture]).Value.ToLower();
            string MenuItemData = string.Empty;
            if (data.Where(Q => Q.ParentId == MenuId && Q.IsParent == true).Count() > 0)
            {

                foreach (var item2 in data.Where(Q => Q.ParentId == MenuId && Q.IsParent == true).OrderBy(O => O.Priority).Select(Q => Q))
                {
                    MenuItemData += "<li class=\"sub-dropdown\">";
                    MenuItemData += "<i class=\"" + item2.MenuIcon + "\" aria-hidden=\"true\"></i> <span class=\"dev-citymenu\"> " + (language == "en-us" ? item2.MenuNameEn : item2.MenuNameHI) + " </span>";
                    MenuItemData += " <ul class=\"sub-dropdown-content\">";
                    MenuItemData += Menu(data, item2.Id);
                    MenuItemData += " </ul>";
                    MenuItemData += " </li>";
                }
            }
            else
            {
                foreach (var item2 in data.Where(Q => Q.ParentId == MenuId && Q.IsParent == false).OrderBy(O => O.Priority).Select(M => M))
                {
                    MenuItemData += "<li> <a href=\"" + item2.MenuUrl + "\"> <i class=\"" + item2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? item2.MenuNameEn : item2.MenuNameHI) + " </span> </a> </li>";
                }
            }
            return MenuItemData;
        }

        public string Menu(List<MenuIterate> data, int MenuId)
        {
            string language = (Request.Cookies[SessionIdentity.Culture]).Value.ToLower();
            string MenuItemData = string.Empty;
            //bool isSubParent = data.Where;
            //if (data.Where(Q => Q.ParentId == MenuId && Q.IsParent == true).Count() > 0)

            MenuItemData += "<ul class=\"menu-bar navbar-nav list-unstyled components\" id=\"menuList\">";

            foreach (var Level1 in data.Where(Q => Q.MenuLevel == 1).OrderBy(O => O.Priority).Select(Q => Q))
            {
                MenuItemData += "<li id=\"" + Level1.Id + "\" class=\"sub-dropdown\">";
                MenuItemData += "<i class=\"" + Level1.MenuIcon + "\" aria-hidden=\"true\"></i> <span class=\"dev-citymenu\"> " + (language == "en-us" ? Level1.MenuNameEn : Level1.MenuNameHI) + " </span>";
                if (data.Where(Q => Q.MenuLevel == 2 && Q.ParentId == Level1.Id).Any())
                {
                    MenuItemData += "<ul class='sub-dropdown-content'>";
                    foreach (var Level2 in data.Where(Q => Q.MenuLevel == 2 && Q.ParentId == Level1.Id).OrderBy(O => O.Priority).Select(Q => Q))
                    {
                        if (data.Where(Q => Q.MenuLevel == 3 && Q.ParentId == Level2.Id).Any())
                        {
                            MenuItemData += "<li class='sub-dropdown-second'> <a href=\"" + Level2.MenuUrl + "\"> <i class=\"" + Level2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level2.MenuNameEn : Level2.MenuNameHI) + " </span><i  class='fa fa-caret-down'></i> </a>";
                            MenuItemData += "<ul class='sub-dropdown-content-second'>";
                            foreach (var Level3 in data.Where(Q => Q.MenuLevel == 3 && Q.ParentId == Level2.Id).OrderBy(O => O.Priority).Select(Q => Q))
                            {
                                MenuItemData += "<li> <a href=\"" + Level3.MenuUrl + "\"> <i class=\"" + Level3.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level3.MenuNameEn : Level3.MenuNameHI) + " </span> </a>";
                            }
                            MenuItemData += "</ul>";
                        }
                        else
                        {
                            MenuItemData += "<li> <a href=\"" + Level2.MenuUrl + "\"> <i class=\"" + Level2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level2.MenuNameEn : Level2.MenuNameHI) + " </span> </a>";
                        }
                        MenuItemData += "</li>";
                    }
                    MenuItemData += "</ul>";
                }
                MenuItemData += " </li>";
            }
            MenuItemData += "</ul>";
            return MenuItemData;
        }



        [HttpGet]
        [ActionName("UserRegistration")]
        public ActionResult UserAdd()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistration, (Request.Cookies[SessionIdentity.Culture]).Value);
                //MasterEntryIntoSql objMasterEntryIntoSql = new MasterEntryIntoSql();
                //objMasterEntryIntoSql.Insert("BDH_CITY,BDH_WARD,EMITRA_STATE,PRAJ_BLOCK,PRAJ_DISTRICT,PRAJ_DIVISION,PRAJ_GP,PRAJ_VILLAGE,RVNU_TEHSIL"); //PRAJ_VILLAGE
            }
            return View();
        }

        //[HttpPost]
        //[ActionName("SaveUser")]
        //public string UserInsert(UserEntity _Model)
        //{
        //    string[] keys = { };
        //    if (_Model.IsRuralUrban)
        //        keys = new[] { "Password", "TehsilId", "CityId", "WardId" };
        //    else
        //        keys = new[] { "Password", "BlockId", "GPId", "VillageId" };

        //    foreach (string str in keys)
        //        ModelState.Remove(str);

        //    if (!ModelState.IsValid)
        //        return JsonModelError.Get(ModelState.Values);

        //    UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
        //    return CommonMessageFactory.Response(CommonMessageFactory.Message(objUserRegistrationModel.Add(_Model), (Request.Cookies[SessionIdentity.Culture]).Value));
        //}


        [HttpPost]
        [ActionName("SaveUser")]
        public string UserInsert(UserEntity _Model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<RoleArray> RoleData = new List<RoleArray>();
            try
            {
                RoleData = Request.Form["RoleArray"].ToString().Equals("[]") ? new List<RoleArray>() : (List<RoleArray>)s.Deserialize(Request.Form["RoleArray"].ToString(), typeof(List<RoleArray>));
            }
            catch { return CommonMessageFactory.Response("Provided input data is not in correct format."); }
            //string[] keys = { };
            //if (_Model.IsRuralUrban)
            //    keys = new[] { "Password", "TehsilId", "CityId", "WardId" };
            //else
            //    keys = new[] { "Password", "BlockId", "GPId", "VillageId" };

            //foreach (string str in keys)
            //    ModelState.Remove(str);

            //if (!ModelState.IsValid)
            //    return JsonModelError.Get(ModelState.Values);

            string fileName = string.Empty;
            _Model.ImageUrl = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ImageUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/UserImages"), fileName));
                }
                catch { }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = "";
            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(objUserRegistrationModel.Add(_Model, RoleData).ToString());
        }

        [HttpPost]
        [ActionName("SaveUserReqAppForm")]
        public string InsertUserReqAppForm(string Ids, string IdsRej, string SSO_ID)
        {

            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(objUserRegistrationModel.AddUserReq(Ids, IdsRej, SSO_ID).ToString());

        }


        [HttpPost]
        public string ChangeStatus(string Id, string Action)
        {
            int UserId = 0, ActionId = 0;
            if (!int.TryParse(Id, out UserId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objUserRegistrationModel.ChangeStatus(UserId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }


        [HttpPost]
        public JsonResult GetUserById(UserEntity _Model)
        {
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            return Json(CommonMessageFactory.Response(CommonMessageFactory.Message(1, (Request.Cookies[SessionIdentity.Culture]).Value)));
        }

        [HttpGet]
        [ActionName("GetUsers")]
        public string GetUsersWorkflow()
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(_LoggedInUser.RoleId, "/User/UserRegistration"));
        }
        [HttpPost]
        [ActionName("ListUsers")]
        public string GetAllUsers(string WhereClause)
        {
            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRegistrationModel.GetAll(WhereClause));
        }

        #endregion

        #region Role
        // GET: UserRoll
        [HttpGet]
        public ActionResult UserRole()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRole, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SaveUserRole")]
        public string UserRoleInsert(UserRoleEntity _Model)//UserRoleEntity _Model
        {
            UserRoleModel obj = new UserRoleModel();
            ModelState.Remove("CreationDate");
            ModelState.Remove("IsActive");
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            return CommonMessageFactory.ResponseByCulture(obj.AddRole(_Model), _Model.RoleID > 0 ? 2 : 1, (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        [HttpGet]
        [ActionName("RoleList")]
        public string GetAllRole(string WhereClause = "")
        {
            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllRole(WhereClause));
        }
        [HttpPost]
        public string ChangeRoleStatus(string Id, string Action)
        {
            int RoleId = 0, ActionId = 0;
            if (!int.TryParse(Id, out RoleId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objUserRoleModel.ChangeRoleStatus(RoleId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }


        [HttpPost]
        [ActionName("RoleRightsList")]
        public string GetRoleRightsList(string WhereClause, int MenuID = 0)
        {
            //
            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            UserRoleModel objUserRole = new UserRoleModel();
            objUserRole.MenuId = MenuID;
            string dt = COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllMenu(objUserRole));
            string dtfilter = dt.Replace("MenuActionList\":\"", "MenuActionList\":");
            string dtfilter2 = dtfilter.Replace("}]\"}", "}]}");
            //



            string dt1 = COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetRoleRights(WhereClause));
            string Grand = "{\"ParentData\":" + dtfilter2 + ",\"ChildData\":" + dt1 + "}";
            return Grand;
            //return dtfilter2;

        }


        [HttpPost]
        [ActionName("GetAllRoleRightsList")]
        public string GetAllRoleRightsList(string WhereClause, int MenuID = 0)
        {
            //
            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            UserRoleModel objUserRole = new UserRoleModel();
            objUserRole.MenuId = MenuID;
            string dt = COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllMenu(objUserRole));
            string dtfilter = dt.Replace("MenuActionList\":\"", "MenuActionList\":");
            string dtfilter2 = dtfilter.Replace("}]\"}", "}]}");
            //



            string dt1 = COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetRoleRights(WhereClause));
            string Grand = "{\"ParentData\":" + dtfilter2 + ",\"ChildData\":" + dt1 + "}";
            return Grand;
            //return dtfilter2;

        }



        public JsonResult GetAllMenuList(string WhereClause, int MenuID = 0, int RoleId = 0)
        {
            string language = (Request.Cookies[SessionIdentity.Culture]).Value.ToLower();
            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            UserRoleModel objUserRole = new UserRoleModel();
            objUserRole.MenuId = MenuID;
            DataTable dt = new DataTable();
            dt = objUserRoleModel.GetAllMenu(objUserRole, RoleId);



            List<MenuLevel1> list = new List<MenuLevel1>();
            foreach (var Level1 in dt.AsEnumerable().Where(Q => Q.Field<int>("MenuLevel") == 1).OrderBy(O => O.Field<int>("Priority")).Select(Q => Q))
            {
                List<MenuLevel2> list2 = new List<MenuLevel2>();

                // MenuItemData += "<li class=\"sub-dropdown\">";
                //MenuItemData += "<i class=\"" + Level1.MenuIcon + "\" aria-hidden=\"true\"></i> <span class=\"dev-citymenu\"> " + (language == "en-us" ? Level1.MenuNameEn : Level1.MenuNameHI) + " </span>";
                if (dt.AsEnumerable().Where(Q => Q.Field<int>("MenuLevel") == 2 && Q.Field<int>("ParentId") == Level1.Field<int>("MenuId")).Any())
                {
                    // MenuItemData += "<ul class='sub-dropdown-content'>";
                    foreach (var Level2 in dt.AsEnumerable().Where(Q => Q.Field<int>("MenuLevel") == 2 && Q.Field<int>("ParentId") == Level1.Field<int>("MenuId")).OrderBy(O => O.Field<int>("Priority")).Select(Q => Q))
                    {


                        List<MenuIterate> list3 = new List<MenuIterate>();
                        if (dt.AsEnumerable().Where(Q => Q.Field<int>("MenuLevel") == 3 && Q.Field<int>("ParentId") == Level2.Field<int>("MenuId")).Any())
                        {
                            //MenuItemData += "<li  class='sub-dropdown-second'> <a href=\"" + Level2.MenuUrl + "\"> <i class=\"" + Level2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level2.MenuNameEn : Level2.MenuNameHI) + " </span><i  class='fa fa-caret-down'></i> </a>";
                            //MenuItemData += "<ul class='sub-dropdown-content-second'>";
                            foreach (var Level3 in dt.AsEnumerable().Where(Q => Q.Field<int>("MenuLevel") == 3 && Q.Field<int>("ParentId") == Level2.Field<int>("MenuId")).OrderBy(O => O.Field<int>("Priority")).Select(Q => Q))
                            {

                                // MenuItemData += "<li> <a href=\"" + Level3.MenuUrl + "\"> <i class=\"" + Level3.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level3.MenuNameEn : Level3.MenuNameHI) + " </span> </a>";
                                list3.Add(new MenuIterate()
                                {
                                    Id = Level3.Field<int>("MenuId"),
                                    IsChecked = Level3.Field<bool>("IsChecked"),
                                    IsParent = false,
                                    MenuIcon = Level3.Field<string>("MenuIcon"),
                                    MenuLevel = 3,
                                    MenuNameEn = Level3.Field<string>("MenuNameEn"),
                                    MenuNameHI = Level3.Field<string>("MenuNameHI"),
                                    MenuUrl = Level3.Field<string>("MenuUrl"),
                                    ParentId = Level3.Field<int>("ParentId"),
                                    Priority = Level3.Field<int>("Priority"),
                                    MenuId = Level3.Field<int>("MenuId"),
                                    MenuActionId = Level3.Field<string>("MenuActionId"),
                                    IsActive = Level3.Field<int>("IsActive"),
                                    ////CreatedBy = Level3.Field<int>("CreatedBy"),
                                    ////UpdatedBy = Level3.Field<int>("UpdatedBy"),
                                    ParentMenuNameEn = Level3.Field<string>("Parent_Name_En"),
                                    ParentMenuNameHi = Level3.Field<string>("Parent_Name_Hi"),
                                    MenuActionList = Level3.Field<string>("MenuActionList"),
                                    AssignedActionID = Level3.Field<string>("AssignedActionID")

                                });

                            }
                            //MenuItemData += "</ul>";
                        }
                        else
                        {
                            // MenuItemData += "<li> <a href=\"" + Level2.MenuUrl + "\"> <i class=\"" + Level2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level2.MenuNameEn : Level2.MenuNameHI) + " </span> </a>";
                        }
                        //MenuItemData += "</li>";

                        list2.Add(new MenuLevel2()
                        {
                            Id = Level2.Field<int>("MenuId"),
                            IsChecked = Level2.Field<bool>("IsChecked"),
                            IsParent = false,
                            MenuIcon = Level2.Field<string>("MenuIcon"),
                            MenuLevel = 2,
                            MenuNameEn = Level2.Field<string>("MenuNameEn"),
                            MenuNameHI = Level2.Field<string>("MenuNameHI"),
                            MenuUrl = Level2.Field<string>("MenuUrl"),
                            ParentId = Level2.Field<int>("ParentId"),
                            Priority = Level2.Field<int>("Priority"),
                            MenuId = Level2.Field<int>("MenuId"),
                            MenuActionId = Level2.Field<string>("MenuActionId"),
                            IsActive = Level2.Field<int>("IsActive"),
                            ////CreatedBy = Level2.Field<int>("CreatedBy"),
                            ////UpdatedBy = Level2.Field<int>("UpdatedBy"),
                            ParentMenuNameEn = Level2.Field<string>("Parent_Name_En"),
                            ParentMenuNameHi = Level2.Field<string>("Parent_Name_Hi"),
                            MenuActionList = Level2.Field<string>("MenuActionList"),
                            MenuLevel3 = list3,
                            AssignedActionID = Level2.Field<string>("AssignedActionID")
                        });

                    }
                    //MenuItemData += "</ul>";
                }
                // MenuItemData += " </li>";

                list.Add(new MenuLevel1()
                {
                    Id = Level1.Field<int>("MenuId"),
                    IsChecked = Level1.Field<bool>("IsChecked"),
                    IsParent = false,
                    MenuIcon = Level1.Field<string>("MenuIcon"),
                    MenuLevel = 1,
                    MenuNameEn = Level1.Field<string>("MenuNameEn"),
                    MenuNameHI = Level1.Field<string>("MenuNameHI"),
                    MenuUrl = Level1.Field<string>("MenuUrl"),
                    ParentId = Level1.Field<int>("ParentId"),
                    Priority = Level1.Field<int>("Priority"),
                    MenuId = Level1.Field<int>("MenuId"),
                    MenuActionId = Level1.Field<string>("MenuActionId"),
                    IsActive = Level1.Field<int>("IsActive"),
                    //CreatedBy = Level1.Field<int>("CreatedBy"),
                    //UpdatedBy = Level1.Field<int>("UpdatedBy"),
                    ParentMenuNameEn = Level1.Field<string>("Parent_Name_En"),
                    ParentMenuNameHi = Level1.Field<string>("Parent_Name_Hi"),
                    MenuActionList = Level1.Field<string>("MenuActionList"),
                    MenuLevel2 = list2,
                    AssignedActionID = Level1.Field<string>("AssignedActionID")
                });

            }
            // MenuItemData += "</ul>";
            return Json(new { DataList = list }, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region RoleRights


        [HttpPost]
        [ActionName("SaveAssignRights")]
        public string AssignRightsInsert(List<RolemenuAction> list)
        {

            string _test = string.Empty;
            UserRoleModel objMenuModel = new UserRoleModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objMenuModel.AddAssignRights(list), (Request.Cookies[SessionIdentity.Culture]).Value));
        }


        #endregion

        [HttpGet]
        public ActionResult UserTransAddiChargeRequest()
        {
            return View();
        }

        #region UserRoleDeptMapping
        [HttpGet]
        public ActionResult UserRoleDeptMapping()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRoleDeptMapping, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        [ActionName("SelectUser")]
        public string SelectUserList()
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectUserList());
        }

        [HttpGet]
        [ActionName("SelectDepartment")]
        public string SelectDepartmentList()
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectDepartmentList());
        }

        [HttpGet]
        [ActionName("SelectDesignation")]
        public string SelectDesignationList()
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectDesignationList());
        }

        [HttpGet]
        [ActionName("SelectPost")]
        public string SelectPostList()
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectPostList());
        }

        [HttpGet]
        [ActionName("SelectRole")]
        public string SelectRoleList(int LevelId = 0)
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.SelectRoleList(LevelId));
        }

        [HttpPost]
        [ActionName("UserRoleDeptMappingInsert")]
        public string UserRoleDeptMappingInsert(UserRoleDeptMappingModel _Model)
        {
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new DBLogger());
            _Model.CreatedBy = _LoggedInUser.UserId;
            return CommonMessageFactory.ResponseByCulture(objUserRoleDeptMappingModel.UserRoleDeptMappingInsert(_Model), (_Model.MappingID > 0 ? 2 : 1), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        [HttpPost]
        [ActionName("GetUserRoleDeptMapping")]
        public JsonResult GetUserRoleDeptMapping(DataPagingModel Details)
        {
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new FileLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.GetUserRoleDeptMapping());

            DataTable DT = objUserRoleDeptMappingModel.GetUserRoleDeptMapping(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string ChangeStatusUserRoleDept(string Id, string Action)
        {
            int MappingId = 0, ActionId = 0;
            if (!int.TryParse(Id, out MappingId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objUserRoleDeptMappingModel.ChangeStatusUserRoleDept(MappingId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        [HttpPost]
        [ActionName("AssignMoreRights")]
        public string AssignMoreRights(string WhereClause)
        {
            //
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new DBLogger());
            string dt = COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.GetAllMenu());
            string dtfilter = dt.Replace("MenuActionList\":\"", "MenuActionList\":");
            string dtfilter2 = dtfilter.Replace("}]\"}", "}]}");
            //

            string dt1 = COMMON_FUNCTION.DataTableToJSONString(objUserRoleDeptMappingModel.GetUserRoleRights(WhereClause));
            string Grand = "{\"ParentData\":" + dtfilter2 + ",\"ChildData\":" + dt1 + "}";
            return Grand;


        }

        [HttpPost]
        [ActionName("SaveAssignMoreRights")]
        public string AssignMoreRightsInsert(List<UserRolemenuAction> list)
        {

            string _test = string.Empty;
            UserRoleDeptMappingModel objUserRoleDeptMappingModel = new UserRoleDeptMappingModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objUserRoleDeptMappingModel.AddAssignMoreRights(list), (Request.Cookies[SessionIdentity.Culture]).Value));
        }


        [HttpPost]
        [ActionName("SaveUserTransAddiRoleRequest")]
        public string SaveUserTransAddiRoleRequest(RoleArray _Model)
        {
            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            if (_Model.USERID == 0)
            {
                _Model.USERID = Convert.ToInt32(_LoggedInUser.UserId);
            }
            return CommonMessageFactory.Response(objUserRegistrationModel.ApplyTransAddiRole(_Model).ToString());
        }
        #endregion

        [HttpGet]
        [ActionName("UserReqList")]
        public string GetAllReqList(string WhereClause = "", bool isExport = false)
        {

            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            if (isExport)
            {
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(objUserRoleModel.GetAllUserReq(WhereClause), "UserDetailList.xlsx");
                new FileController()?.DownloadFromUrl(filename);
                return filename;
            }
            else
                return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllUserReq(WhereClause));

            //   return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllUserReq(WhereClause));
        }
        [HttpGet]
        [ActionName("GetUserReqDetail")]
        public string GetUserReqDetail(int MappingID)
        {

            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetUserReqDetail(MappingID));
        }

        [HttpPost]
        [ActionName("UpdateUserRoleApprForm")]
        public string UpdateUserRoleApprForm(int MappingID, string IsApproved, int RoleID, string ActId, string RequestType = "New", string PrimaryResionId = "", string DairyResionId = "", int UserId = 0)
        {
            //int UserId = 0, ActionId = 0;
            //if (!int.TryParse(Id, out UserId) && !int.TryParse(Action, out ActionId))
            //    return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);
            //ActionId = (Convert.ToInt32(Action));
            RoleID = IsApproved.ToLower() == "true" || IsApproved.ToLower() == "1" ? RoleID : 0;
            IsApproved = IsApproved.ToLower() == "true" || IsApproved.ToLower() == "1" ? "1" : "4";
            PrimaryResionId = IsApproved.ToLower() == "true" || IsApproved.ToLower() == "1" ? PrimaryResionId : "";
            ActId = IsApproved.ToLower() == "true" || IsApproved.ToLower() == "1" ? ActId : "";

            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            DataTable dt = objUserRegistrationModel.UpdateUserRoleAppr(MappingID, Convert.ToInt32(IsApproved), RoleID, ActId, PrimaryResionId, DairyResionId, UserId);

            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("MobileNo"))
            {
                string MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);

                if (MobileNo.Length == 10)
                {
                    DataTable MessageTable = new DataTable();
                    int MessageFlag = Convert.ToInt32(dt.Rows[0]["ExistFlag"]);

                    if (MessageFlag == 1) // 2 Means rejected 1 Means approved
                        MessageTable = MessageOnActivity.GetMessageBodyActivityKey("SMS_ON_USER_REQUEST_APPROVAL");
                    else if (MessageFlag == 2) // 2 Means rejected 1 Means approved
                        MessageTable = MessageOnActivity.GetMessageBodyActivityKey("SMS_ON_USER_REQUEST_REJECT");


                    if (MessageTable != null && MessageTable.Rows.Count > 0)
                    {
                        string Message = Convert.ToString(MessageTable.Rows[0]["SMS_MSG"]);

                        if (!string.IsNullOrEmpty(Message))
                        {
                            Message = Message.Replace("##Name##", Convert.ToString(dt.Rows[0]["OfficerNameEn"]));


                            new EsanchaarController().SendSocietySMS(Message.Replace(Convert.ToString(dt.Rows[0]["MobileNo"]), Message), MobileNo);


                            // COMMON_FUNCTION.SendSMS(MobileNo, Message.Replace(Convert.ToString(dt.Rows[0]["MobileNo"]), Message));
                        }
                    }
                }
            }
            return COMMON_FUNCTION.DataTableToJSONString(dt);

            //if (RequestType.Contains("New"))
            //{
            //    if (Result.Length > 4)
            //    {
            //        //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            //        //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            //        //var f = ssoclient.RoleUserMapping("RAJSAHKAR.TEST", "Rajasthan@12", Result.ToUpper().Trim(), "RUMAP", "ADD");
            //    }
            //}
            //return CommonMessageFactory.Response(CommonMessageFactory.Message((Result.Length > 4 ? 1 : 0), (Request.Cookies[SessionIdentity.Culture]).Value));

        }
        [HttpPost]
        [ActionName("UpdateUserRoleForm")]
        public string UpdateUserRoleForm(int MappingID, string ActId, string RequestType = "New", string PrimaryResionId = "", string DairyResionId = "")
        {
            //int UserId = 0, ActionId = 0;
            //if (!int.TryParse(Id, out UserId) && !int.TryParse(Action, out ActionId))
            //    return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);
            //ActionId = (Convert.ToInt32(Action));
            ActId = ActId.StartsWith("0,") || ActId == "0" ? "" : ActId;
            if (ActId.Contains("3"))
            {
                PrimaryResionId = PrimaryResionId.StartsWith("0,") || PrimaryResionId == "0" ? "" : PrimaryResionId;
                DairyResionId = DairyResionId.StartsWith("0,") || DairyResionId == "0" ? "" : DairyResionId;
            }
            else
            {
                PrimaryResionId = "";
                DairyResionId = "";
            }
            //ActId = ActId.Split(',').Equals("0") ? "" : ActId;
            //PrimaryResionId = PrimaryResionId.Split(',').Equals("0")  ? "" : PrimaryResionId;
            //DairyResionId = DairyResionId.Split(',').Equals("0") ? "" : DairyResionId;
            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRegistrationModel.UpdateUserRole(MappingID, ActId, PrimaryResionId, DairyResionId));
        }


        [HttpGet]
        public ActionResult TransferRequest()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TransferRequest, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [ChildActionOnly]
        [ActionName("MyUsersRolesShowDDL")]
        public string RolesDetailsByUserIdForDDL(string UserId)
        {

            string WhereClause = "";
            WhereClause = UserId;
            UserModel objUserModel = new UserModel(new DBLogger());
            DataTable dt = objUserModel.GetRolesDetailsByUserId(WhereClause);

            //string ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            //angular.forEach($scope.CategoryList, function(item) {
            //    if (_language)
            //    {
            //        ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            //    }
            //    else
            //    {
            //        ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            //    }
            //});

            //UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string ddHTML = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["LevelId"].ToString() == "1")
                {
                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString())
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "</option>";
                    }
                    else
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "</option>";
                    }
                }
                else if (dr["LevelId"].ToString() == "2")
                {
                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString())
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["DIVISION_ENG"] + " " + dr["OfficeNameEn"] + ")</option>";
                    }
                    else
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["DIVISION_ENG"] + " " + dr["OfficeNameEn"] + ")</option>";
                    }
                }
                else if (dr["LevelId"].ToString() == "3")
                {
                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString())
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["DISTRICT_ENG"] + " " + dr["OfficeNameEn"] + ")</option>";
                    }
                    else
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["DISTRICT_ENG"] + " " + dr["OfficeNameEn"] + ")</option>";
                    }
                }
                else if (dr["LevelId"].ToString() == "4" && (dr["OrganizationId"].ToString() == "17" || dr["OrganizationId"].ToString() == "11"))
                {
                    //if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString() && dr["OrganizationId"].ToString() == "17")
                    //{
                    //    ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["WarehousNameEn"] + ")</option>";
                    //}
                    //else
                    //{
                    //    ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["WarehousNameEn"] + ")</option>";
                    //}
                    //if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString() && dr["OrganizationId"].ToString() == "11")
                    //{
                    //    ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["BranchName_En"] + ")</option>";
                    //}
                    //else
                    //{
                    //    ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["BranchName_En"] + ")</option>";
                    //}


                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString() && dr["OrganizationId"].ToString() == "17")
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["WarehousNameEn"] + ")</option>";
                    }
                    else if (dr["OrganizationId"].ToString() == "17")
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["WarehousNameEn"] + ")</option>";
                    }
                    else if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString() && dr["OrganizationId"].ToString() == "11")
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["BranchName_En"] + ")</option>";
                    }
                    else if (dr["OrganizationId"].ToString() == "11")
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["BranchName_En"] + ")</option>";
                    }

                }
                else if (dr["LevelId"].ToString() == "4")
                {
                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString())
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["DISTRICT_ENG"] + " " + dr["OfficeNameEn"] + ", " + dr["BLOCK_ENG"] + ")</option>";
                    }
                    else
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["DISTRICT_ENG"] + " " + dr["OfficeNameEn"] + ", " + dr["BLOCK_ENG"] + ")</option>";
                    }
                }
                else if (dr["LevelId"].ToString() == "7" || dr["LevelId"].ToString() == "8" || dr["LevelId"].ToString() == "9")
                {
                    if (dr["MappingID"].ToString() == _LoggedInUser.UserRoleDeptMappingId.ToString())
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "(" + dr["place"] + ")</option>";
                    }
                    else
                    {
                        ddHTML += "<option value='" + dr["MappingID"] + "'>" + dr["RoleShortName_EN"] + "(" + dr["place"] + ")</option>";
                    }
                }

            }
            return (ddHTML.ToString());
        }

        [ChildActionOnly]
        [ActionName("MyUsersRolesShow")]
        public string RolesDetailsByUserId(string UserId)
        {

            string WhereClause = "";
            WhereClause = UserId + "  and URDM.RoleID = " + _LoggedInUser.RoleId;
            UserModel objUserModel = new UserModel(new DBLogger());
            DataTable dt = objUserModel.GetRolesDetailsByUserId(WhereClause);
            string ddHTML = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["RoleID"].ToString() == _LoggedInUser.RoleId.ToString())
                {
                    ddHTML += "<option value='" + dr["RoleID"] + "'  selected='selected' >" + dr["RoleShortName_EN"] + "</option>";
                }
                else
                {
                    ddHTML += "<option value='" + dr["RoleID"] + "'>" + dr["RoleShortName_EN"] + "</option>";
                }

            }
            return (ddHTML.ToString());
        }
        public void DeleteUnwantedCookies(string[] CokkieKeyNotToDelete = null)
        {
            string[] Keys = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var Key in Keys)
            {
                if (CokkieKeyNotToDelete == null || !CokkieKeyNotToDelete.Any(P => P == Key))
                {
                    System.Web.HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie(Key)
                    {
                        Value = Guid.NewGuid().ToString(),
                        Expires = DateTime.Now.AddDays(-30),
                        HttpOnly = true
                    });
                }
            }
        }
        public SSOWSSoap.SSOUserDetail GetUserDetails(string ssoid)
        {
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            usrdtl = ssoclient.GetUserDetailXML(ssoid, "RAJSAHKAR.TEST", "Rajasthan@12");
            return usrdtl;
        }
        public List<LoginIP> LoadJson()
        {
            using (StreamReader r = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/loginIP.json")))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<LoginIP>>(json);
            }
        }
        //Create by rajeev (Do not changes and alter without concern with rajeev)
        public ActionResult LoadBusinesslist()
        {
            var IPSddress = LoadJson();
            string key = Request.QueryString["sso"];
            string[] Role = "HQ | ".Split('|');
            Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
            {
                Value = key,
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
            {
                Value = "en-us",
                HttpOnly = true
            });
            (new SocietyModel()).addhistory("valid-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
            DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
            return RedirectToAction("SSOMapping", "Login");
        }
        [HttpGet]
        [ActionName("GetConfedData")]
        public string GetConfedOrderDetails(string YearWiseOrAll)
        {
            var result = "";
            try
            {
                if (YearWiseOrAll == "undefined")
                {
                    YearWiseOrAll = "2";
                }

                string uri = "";
                string MM = "";
                string DD = "";

                if (DateTime.Now.Month < 10)
                    MM = "0" + DateTime.Now.Month.ToString();
                else
                    MM = DateTime.Now.Month.ToString();


                if (DateTime.Now.Day < 10)
                    DD = "0" + DateTime.Now.Day.ToString();
                else
                    DD = DateTime.Now.Day.ToString();


                string Date = DateTime.Now.Year.ToString() + "-" + MM + "-" + DD;
                string Date2 = DateTime.Now.Year.ToString() + "-04" + "-01";
                ORDERSList obj = new ORDERSList();

                if (YearWiseOrAll == "1")//OVER ALL
                    uri = "https://ebazaar.rajasthan.gov.in/Cooperative/OrderList?OdrFromDate=2011-01-01&OdrToDate=" + Date;
                else
                    uri = "https://ebazaar.rajasthan.gov.in/Cooperative/OrderList?OdrFromDate=" + Date2 + "&OdrToDate=" + Date;

                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                //DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(result);
                ORDERSList yourObject = JsonConvert.DeserializeObject<ORDERSList>(result);
                Dictionary<string, decimal> AuthorList = new Dictionary<string, decimal>();
                foreach (ORDERSData Obj in yourObject.ORDERS)
                {
                    if (!AuthorList.ContainsKey(Obj.DISTRICT_NAME))
                    {
                        AuthorList.Add(Obj.DISTRICT_NAME, Obj.AMOUNT);
                    }
                    else if (AuthorList.ContainsKey(Obj.DISTRICT_NAME))
                    {
                        AuthorList[Obj.DISTRICT_NAME] += Obj.AMOUNT;
                    }
                }

                result = COMMON_FUNCTION.DataTableToJSONString(MyDictionaryToJson(AuthorList));
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserControl", FunctionName = "GetConfedData" }, ex);
            }
            finally
            {

            }
            return result;

        }

        //[HttpGet]
        //[ActionName("GetConfedData")]
        //public string GetConfedOrderDetails(SocietyRegistrationEntity objSocietyRegistrationEntity)
        //{
        //    var result = "";
        //    try
        //    {
        //        ORDERSList obj = new ORDERSList();
        //        string uri = "http://ebazzartest.rajasthan.gov.in/Cooperative/OrderList?OdrFromDate=2018-01-01&OdrToDate=2019-11-11 ";

        //        using (WebClient webClient = new WebClient())
        //        {
        //            using (var stream = webClient.OpenRead(uri))
        //            {
        //                using (var streamReader = new StreamReader(stream))
        //                {
        //                    result = streamReader.ReadToEnd();
        //                }
        //            }
        //        }

        //        //DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(result);
        //        ORDERSList yourObject = JsonConvert.DeserializeObject<ORDERSList>(result);
        //        Dictionary<string, decimal> AuthorList = new Dictionary<string, decimal>();
        //        foreach (ORDERSData Obj in yourObject.ORDERS)
        //        {
        //            if (!AuthorList.ContainsKey(Obj.DISTRICT_NAME))
        //            {
        //                AuthorList.Add(Obj.DISTRICT_NAME, Obj.AMOUNT);
        //            }
        //            else if (AuthorList.ContainsKey(Obj.DISTRICT_NAME))
        //            {
        //                AuthorList[Obj.DISTRICT_NAME] += Obj.AMOUNT;
        //            }
        //        }

        //        result = COMMON_FUNCTION.DataTableToJSONString(MyDictionaryToJson(AuthorList));
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserControl", FunctionName = "GetConfedData" }, ex);
        //    }
        //    finally
        //    {

        //    }
        //    return result;

        //}

        [HttpPost]
        [ActionName("Getlwadata")]
        public string lwadata()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://lwa.rajasthan.gov.in/API/DashBoardData");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();

            }

            return result;

        }

        DataTable MyDictionaryToJson(Dictionary<string, decimal> dict)
        {
            DataTable foundPrices = new DataTable();
            foundPrices.Columns.Add("District", typeof(string));
            foundPrices.Columns.Add("Amount", typeof(string));
            foreach (string str in dict.Keys)
            {
                DataRow dr = foundPrices.NewRow(); //Creating Row  
                dr["District"] = str;
                dr["Amount"] = dict[str].ToString();
                foundPrices.Rows.Add(dr);

            }
            return foundPrices;
        }

        [ActionName("GetDistrictwiseSocietyByTypeStatus")]
        public string GetDistrictwiseSocietyByTypeStatus(string SocietyType, string SocietyStatus)
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetDistrictwiseSocietyByTypeStatus(SocietyType, SocietyStatus));
        }


        [HttpGet]
        [ActionName("GetUserRoleAll")]
        public string GetUserRoleAll()
        {

            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetUserRoleAll(_LoggedInUser.UserId));
        }

        [HttpGet]
        [OutputCache(Duration = 14400)]
        [ActionName("GetRajFedData")]
        public string GetRajFedData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            var result = "";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/" + date;

                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);
                if (theDataSet.Tables.Count > 0)
                    result = COMMON_FUNCTION.DataTableToJSONString(theDataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserControl", FunctionName = "GetRajFedData" }, ex);
            }
            finally
            {

            }
            return result;
        }
        [HttpGet]
        [ActionName("GetSMFApplicationStatusForDashboard")]
        public string GetSMFApplicationStatus()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetSMFApplicationStatus());
        }

        [HttpGet]
        [ActionName("GetStLoanDataForDashBoard")]
        public string GetStLoanDataForDashBoard()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetStLoanDataForDashBoard());
        }

        [HttpGet]
        public ActionResult AuditorDashBoard()
        {

            return View();
        }

        [HttpGet]

        [OutputCache(Duration = 14400)]
        [ActionName("GetUserCountForDashBoard")]
        public string GetUserCountForDashBoard()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetUserCountForDashBoard());
        }

        [HttpGet]
        [ActionName("GetPacsMemberCountReport")]
        public string GetPacsMemberCountReport()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetPacsMemberCountReport());
        }

        [HttpGet]
        [ActionName("GetPacsStatusReport")]
        public string GetPacsStatusReport()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetPacsStatusReport());
        }

        [HttpGet]
        [ActionName("GetDivisionReport")]
        public string GetDivisionReport(string FYearID, string MonthId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetDivisionReport(FYearID, MonthId));
        }

        [HttpGet]
        [ActionName("GetDistrictReport")]
        public string AuditDistrictDataReport(string FYearID, string MonthId, string DivisionId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.AuditDistrictDataReport(FYearID, MonthId, DivisionId));
        }

        [HttpPost]
        [ActionName("GetReturningOfficer")]
        public string GetReturningOfficer(int LevelId, int PlaceID = 0)   // LevelId = 1 for Apex , 2 fro Central , 3 for Primary
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetReturningOfficerList(LevelId, PlaceID));
        }

        [HttpGet]
        public ActionResult UserDetailList()
        {
            return View();
        }
        [HttpGet]
        [ActionName("GetUserDetailList")]
        public string GetUserDetailList(string WhereClause = "")
        {

            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllUser(WhereClause));
        }

        [HttpGet]
        [ActionName("HousingSocietyAuditsCountYearWise")]
        public string GetHousingSocietyAuditsCountYearWise()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetHousingSocietyAuditsCountYearWise());
        }

        [HttpGet]
        [ActionName("GetUserLevelWiseCount")]
        public string GetUserLevelWiseCount()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetUserLevelWiseCount());
        }

        [HttpGet]
        public ActionResult ApproveSocietyMappingRequest()
        {
            return View();
        }


        [HttpPost]
        public string GetSocietyMappingRequestList()
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataTable dt = objCitizenModel.GetSocietyMappingRequestList(_LoggedInUser.RoleId, Convert.ToInt32(_LoggedInUser.UserId));
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public int SaveSocietyMappingProcessRequest(string RequestId, string NextActionId, string NextRoleId, string NextUserId, string Remark)
        {
            int _RequestId = 0, _NextActionId = 0, _NextRoleId, _NextUserId;

            int.TryParse(RequestId, out _RequestId);
            int.TryParse(NextActionId, out _NextActionId);
            int.TryParse(NextRoleId, out _NextRoleId);
            int.TryParse(NextUserId, out _NextUserId);

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            int Result = objCitizenModel.SaveSocietyMappingProcessRequest(_RequestId, _NextActionId, _NextRoleId, _NextUserId, Remark, _LoggedInUser.SSOID, Convert.ToInt32(_LoggedInUser.UserId), _LoggedInUser.RoleId);
            if (Result > 0)
            {
                MessageOnActivity.Message(_RequestId, _NextActionId);
            }
            return Result;
        }

        [HttpPost]
        public string GetSSocietyMappingRequestActivityLog(int RequestId)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataTable dt = objCitizenModel.SocietyMappingRequestActivityLog(RequestId);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpGet]
        public ActionResult MAPPING()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 14400)]
        [ActionName("CommonFunctionForDashBoard")]
        public string GetCommonFunctionForDashBoard()
        {
            UserModel objUserRoleModel = new UserModel(new DBLogger());
            var Result = objUserRoleModel.GetCommonFunctionForDashBoard();
            return JsonConvert.SerializeObject(Result, Newtonsoft.Json.Formatting.Indented);
        }

        [HttpGet]
        public ActionResult TilamSanghDashboard()
        {
            ViewBag.UserLevel = _LoggedInUser.LevelId;
            ViewBag.UserRoleId = _LoggedInUser.RoleId;
            return View();
        }

        [HttpGet]
        public ActionResult FertilizerSupplierDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DashboardForWarehouseManager()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CCSComplaintReport()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetUserDetails()
        {
            return Json(_LoggedInUser);
        }

        //[HttpPost]
        //[ActionName("MappingChangeStatus")]
        //public JsonResult MappingChangeStatus(int MappingID, int Action)
        //{
        //    Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));

        //    UserRegistrationModel objOfficeModel = new UserRegistrationModel(new DBLogger());
        //    return Json(objOfficeModel.MappingChangeStatus(MappingID, Action), JsonRequestBehavior.AllowGet);
        //}

        [ActionName("MappingChangeStatus")]
        public string MappingChangeStatus(int MappingID, int Action, int UserId, int Flag)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));


            DataTable dt1 = new DataTable();
            UserRegistrationModel objOfficeModel = new UserRegistrationModel(new DBLogger());
            return objOfficeModel.MappingChangeStatus(MappingID, Action, UserId, Flag);

            //dt1 = objOfficeModel.MappingChangeStatus(MappingID, Action, UserId, Flag);

            //return COMMON_FUNCTION.DataTableToJSONString(dt1);
            //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt1)}, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ActionName("CheckRoleAtCuttentLocation")]
        public string CheckRoleAtCuttentLocation(int MappingID, string ActId, string RequestType = "New", string PrimaryResionId = "", string DairyResionId = "")
        {
            ActId = ActId.StartsWith("0,") || ActId == "0" ? "" : ActId;
            if (ActId.Contains("3"))
            {
                PrimaryResionId = PrimaryResionId.StartsWith("0,") || PrimaryResionId == "0" ? "" : PrimaryResionId;
                DairyResionId = DairyResionId.StartsWith("0,") || DairyResionId == "0" ? "" : DairyResionId;
            }
            else
            {
                PrimaryResionId = "";
                DairyResionId = "";
            }

            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRegistrationModel.CheckRoleAtCuttentLocation(MappingID, ActId, PrimaryResionId, DairyResionId));
        }

        [HttpPost]
        public JsonResult GetActionIdCCSComplaintReport(DataPagingModel Details, int ActionId, int DistrictId, bool IsRowCount = false)
        {
            string WhereClause = "";
            // ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;

            MISModel objModel = new MISModel(new DBLogger());
            DataSet ds = new DataSet();
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;

            // _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
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

                DT = objModel.GetActionIdCCSComplaintReport(ref Details, ActionId, WhereClause);
            }

            // DataTable DT = objNew.GetWRSummaryList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


            // return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);

        }


        [HttpGet]
        [OutputCache(Duration = 14400)]
        [ActionName("GetSocietyTypeWiseAuditProgress")]
        public string GetSocietyTypeWiseAuditProgress(string FYearID, string MonthId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetSocietyTypeWiseAuditProgress(FYearID, MonthId));
        }



        [HttpGet]
        public ActionResult SLDBDashBoard()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.UserLevel = _LoggedInUser.LevelId;
            return View();
        }

        public JsonResult GetComplainUnderSection(DataPagingModel Details, int ActionId, int DistrictId, bool IsRowCount = false)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataSet ds = new DataSet();
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = new DataTable();
            DT = objModel.GetComplainUnderSection(DistrictId == 0 ? _LoggedInUser.DistrictId : DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult GetNewDashboardData(int DivisionId, int DistrictId, int FY)
        {
            MISModel objModel = new MISModel(new DBLogger());
            DataSet ds = new DataSet();
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            DataSet DT = new DataSet();
            DT = objModel.GetNewDashboard(DivisionId, DistrictId, FY);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        #region Send SMS to User
        [HttpGet]
        public ActionResult SendSMStoUser()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpPost]
        [ActionName("SaveAndSendSMS")]
        public JsonResult SaveAndSendSMS(List<string> selectedItemsUserID, List<string> selectedItemsMobileNo, string message)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            string addUserId = "0";
            for (int i = 0; i < selectedItemsUserID.Count; i++)
            {
                addUserId += selectedItemsUserID[i] + ",";
            }

            string userId = addUserId.Remove(addUserId.Length - 1, 1);
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            string result = objUserRoleModel.SaveAndSendSMS(userId, message);
            if (result != "-1")
            {
                for (int i = 0; i < selectedItemsMobileNo.Count; i++)
                {
                    int id = 0;
                    List<string> mobList = selectedItemsMobileNo[i].Split(',').ToList();
                    EsanchaarController EC = new EsanchaarController();
                    string IsOk = EC.SendSMS("HIN", message, mobList, id);
                }
            }


            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region USER

        //bhupendra--start

        [HttpGet]
        public ActionResult UserDetailListUpdate()
        {
            return View();
        }
        [HttpGet]
        [ActionName("GetUserDetailListUpdateSSOID")]
        public string GetUserDetailListUpdateSSOID(string WhereClause = "")
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllUserUpdateSSoid(WhereClause));
        }
        [HttpPost]
        [ActionName("GetUserDetailSSOIDInfo")]
        public string GetUserDetailSSOIDInfo(string SSOID)
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return objUserRoleModel.ChangeUserDetailSSoidData(SSOID);

        }
        [HttpPost]
        [ActionName("UserUpdateInfoNewSSOID")]
        public string UserUpdateInfoNewSSOID(UserModelupdatenewssoid _Object)
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.SetUserUpdateInfoNewSSOID(_Object));

        }

        [HttpGet]
        [OutputCache(Duration = 14400)]
        [ActionName("GetSocietyStatusWiseDataList")]
        public string GetSocietyStatusWiseDataList(string FYearID, string MonthId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objUserModel.GetSocietyStatusWiseDataList(FYearID, MonthId));
        }

        [HttpGet]

        [OutputCache(Duration = 14400)]
        [ActionName("GetDashboardCountSocietyAuditprogrss")]
        public string GetDashboardCountSocietyAuditprogrss(string FYearID, string MonthId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objUserModel.GetDashboardCountSocietyAuditprogrss(FYearID, MonthId));
        }

        //bhupendra--End

        #endregion



        #region GrievanceApproval

        [HttpPost]
        public string UploadGrievanceFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/GrievanceDocuments");

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

        [HttpGet]
        [ActionName("GetGrievanceHistory")]
        public string GetGrievanceHistory(int GrievanceId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objUserModel.GrievanceHistoryList(GrievanceId));
        }


        public ActionResult ApprovalGrievance()
        {
            return View();
        }



        [HttpPost]
        [ActionName("SaveGrievanceApproval")]
        public int SaveGrievanceApproval(GrievanceApproval _Entity)
        {

            UserModel objUserModel = new UserModel();
            _Entity.IsActive = 1;
            _Entity.CreatedBy = _LoggedInUser.UserRoleDeptMappingId;
            _Entity.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            _Entity.SSO_ID = _LoggedInUser.SSOID;
            return objUserModel.SaveGrievanceApproval(_Entity);

        }

        public ActionResult SocietyApplicationView(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        #endregion GrievanceApproval


        public ActionResult GetSupport()
        {
            return View();
        }
        public ActionResult GodownDashboard()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.UserLevel = _LoggedInUser.LevelId;
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 50)]
        public JsonResult GetDasboardData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0)
        {
            Users objUserModel = new Users();
            return Json(new { DataList = objUserModel.GetDasboardData(FinancialYear, DivisionId, DistrictId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InsertDashboardData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0)
        {
            Users objUserModel = new Users();
            return Json(new { DataList = objUserModel.InsertDashboardData(FinancialYear, DivisionId, DistrictId) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CivilList()
        {
            return View();
        }
        [HttpGet]
        [ActionName("GetCivilOfficerType")]
        public string GetCivilOfficerType()
        {
            UserRoleModel CivilOfficerType = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(CivilOfficerType.CivilOfficerTypeList());
        }

        [HttpGet]
        [ActionName("DISTRICT_LIST")]
        public string DISTRICT_LIST()
        {
            UserRoleModel DistrictItems = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(DistrictItems.DistrictList());
        }
        [HttpGet]
        [ActionName("GetOfficerCivilList")]
        public string GetOfficerCivilList()
        {
            UserRoleModel objUserModel = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetOfficerCivilList());
        }
        [HttpPost]
        [ActionName("InsertCivilList")]
        public String InsertCivilList(CiviLList _Model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<TblCHisttoryDetails> objData = new List<TblCHisttoryDetails>();
            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<TblCHisttoryDetails>() : (List<TblCHisttoryDetails>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<TblCHisttoryDetails>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }
            string fileName1 = "";
            if (Request.Files != null && Request.Files.Count == 1)
            {

                if (Request.Files.AllKeys[0] == "OfficerImage")
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var file1 = Request.Files[0];
                        fileName1 = Guid.NewGuid() + Path.GetExtension(file1.FileName);
                        // _Model.OfficerImage = "~/Content/CivilOfficerPhoto/" + fileName1;
                        _Model.OfficerImage = fileName1;
                        file1.SaveAs(Path.Combine(Server.MapPath("~/Content/CivilOfficerPhoto"), fileName1));
                    }

                }
            }

            UserRoleModel objUserRoleModel = new UserRoleModel(new DBLogger());
            return objUserRoleModel.InsertCivilList(objData, _Model).ToString();
        }

        [HttpPost]
        [ActionName("DeleteCivilList")]
        public string DeleteCivilList(int Id)
        {
            UserRoleModel objUserModel = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.DeleteCivilList(Id));
        }

        [HttpPost]
        [ActionName("GetCivilListforedit")]
        public string GetCivilListforedit(int Id)
        {
            UserRoleModel objUserModel = new UserRoleModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objUserModel.GetCivilListforedit(Id));

        }

        public int IsEMITRATHIRDPARTYPENDENCYSERVICECall()
        {
            UserModel objUserModel = new UserModel(new DBLogger());
            bool IsLive = true;

            if (Request.Url.Host.Contains("localhost"))
            {
                IsLive = false;
            }
            return objUserModel.IsEMITRATHIRDPARTYPENDENCYSERVICECall(IsLive);

        }



        #region User Permission For Server Access
        [HttpPost]
        [ActionName("SetUserPermission")]
        public string SetUserPermission(UserPermission permission)
        {
            permission.UserType = "IIS_IUSRS";
            string folderPath = Server.MapPath("/" + permission.SourceDirName);
            if (!System.IO.Directory.Exists(folderPath)) { return ""; }
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

                FileSystemRights rights = 0;
                if (permission.Read)
                    rights |= FileSystemRights.Read;
                if (permission.Write)
                    rights |= FileSystemRights.Write;
                if (permission.Modify)
                    rights |= FileSystemRights.Modify;
                if (permission.FullAccess)
                    rights = FileSystemRights.FullControl;

                InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
                PropagationFlags propagationFlags = PropagationFlags.None;

                FileSystemAccessRule accessRuleAdd = new FileSystemAccessRule(
                    permission.UserType, //new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                    rights,
                    inheritanceFlags,
                    propagationFlags,
                    AccessControlType.Allow);

                if (permission.Action == 1)
                    dirSecurity.AddAccessRule(accessRuleAdd);
                else
                    dirSecurity.RemoveAccessRule(accessRuleAdd);
                dirInfo.SetAccessControl(dirSecurity);
                return "Permission set successfully";
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.UserController", FunctionName = " SetUserPermission(UserPermission permissions) " + ex }, ex);
                return "";
            }
        }
        #endregion



        #region Update User Profile by madhuri

        [HttpPost]
        [ActionName("SaveUserProfile")]
        public string UserInserts(UserEntity _Model)
        {
            string fileName = string.Empty;
            _Model.ImageUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ImageUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/UserImages"), fileName));
                }
                catch { }
            }
            _Model.CreatedBy = 0;
            _Model.UpdatedBy = 0;


            UserModel objUserRegistrationModel = new UserModel(new DBLogger());
            int _Response = objUserRegistrationModel.objUserRegistrationModel(_Model);


            if (_Response == 1)
            {
                //_LoggedInUser.ImageUrl = _Model.ImageUrl;
                _LoggedInUser.OfficerNameHi = _Model.OfficerNameHi;
                _LoggedInUser.OfficerNameEn = _Model.OfficerNameEn;
                _LoggedInUser.AddressHi = _Model.AddressHi;
                _LoggedInUser.AddressEn = _Model.AddressEn;
                _LoggedInUser.EmailId = _Model.EmailId;
                _LoggedInUser.MobileNo = _Model.MobileNo;  
            }


            return CommonMessageFactory.ResponseByCulture(_Response, (_Model.UserId > 0 ? 2 : 1), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        #endregion


        //
        [HttpGet]
        [ActionName("UpdateDecrptyDetails")]
        public void UpdateDecrptyDetails()
        {
            try
            {
                UserModel objUserModel = new UserModel(new DBLogger());
                DataTable DT = new DataTable();
                string Drpyt = "";
                string Message = "";
                DT = objUserModel.GetAllDoc("");
                //string pathString = "";
                string fileName = string.Empty;
                List<SaveDcrypted> objList = new List<SaveDcrypted>();

                foreach (DataRow Dr in DT.Rows)
                {
                    var t = DT.Columns;
                    foreach (var i in t) //For Each Row
                    {

                        try
                        {
                            SaveDcrypted OBJ = new SaveDcrypted();
                            Drpyt = COMMON_FUNCTION.Decrypt(Dr["JanAadharNoEnc"].ToString());
                            OBJ.PK_ID = Convert.ToInt32(Dr["GopalCreditCardId"].ToString());
                            OBJ.ColumnValueName = Drpyt;
                            OBJ.IsAbleToSave = 1;
                            objList.Add(OBJ);

                        }
                        catch (Exception ex)
                        {
                            SaveDcrypted OBJ = new SaveDcrypted();
                            OBJ.PK_ID = Convert.ToInt64(Dr[Dr["PK_ID"].ToString()].ToString());
                            OBJ.ColumnValueName = fileName;
                            OBJ.ErrorMsg = ex.Message;
                            OBJ.IsAbleToSave = 0;
                            objList.Add(OBJ);

                            DBLogger objDBLogger = new DBLogger();
                            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Controller.User", FunctionName = "UpdateReferencebyUID()" }, ex);
                            Message = "Error while sampleimage File Save";
                        }

                    }
                    if (objList.Count > 0)
                    {
                        csCommon obj = new csCommon();
                        obj.UpdateDcrypted(objList);
                    }

                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "UpdatePhotosAndDoc" + " Message: " + Convert.ToString(ex.Message) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) }, ex);


            }

        }
        //


    }
}
