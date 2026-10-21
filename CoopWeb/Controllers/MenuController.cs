using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class MenuController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        #region MenuAction


        [HttpGet]
        public ActionResult MenuAction()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.MenuAction, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("MenuActionInsert")]
        public string InsertMenuAction(MenuActionModel _Model)
        {
            ModelState.Remove("CreationDate"); ModelState.Remove("UpdationDate"); ModelState.Remove("UpdatedBy");

            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);


            //return Json(_Model.AddMenu());
            MenuActionModel objMenuModel = new MenuActionModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objMenuModel.InsertMenuAction(_Model), (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        //[HttpGet]
        //[ActionName("MenuActionList")]
        //public JsonResult GetAllActionList()
        //{
        //    MenuActionModel objMenuActionModel = new MenuActionModel(new DBLogger());
        //    return Json(objMenuActionModel.GetAllMenuAction(), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ActionName("MenuActionList")]
        public string GetAllActionList(string WhereClause)
        {
            MenuActionModel objMenuActionModel = new MenuActionModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMenuActionModel.GetAllMenuAction(WhereClause));
        }
        [HttpGet]
        [ActionName("GetMenu")]
        public string GetMenuWorkflow()
        {
            WorkFlowModel objWorkFlowModel = new WorkFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWorkFlowModel.GetAllWorkflow(_LoggedInUser.RoleId, "menu/menu"));
        }

        [HttpPost]
        public string ChangeMenuActionStatus(string Id, string Action)
        {
            int MenuActionId = 0, ActionId = 0;
            if (!int.TryParse(Id, out MenuActionId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            MenuActionModel objMenuActionModel = new MenuActionModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objMenuActionModel.ChangeMenuActionStatus(MenuActionId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }
        #endregion

        #region Menu

        [HttpGet]
        public ActionResult Menu()
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
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Menu, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SaveMenu")]
        public string MenuInsert(MenuModel _Model)
        {
            ModelState.Remove("CreationDate"); ModelState.Remove("UpdationDate"); ModelState.Remove("UpdatedBy");
            _Model.CreatedBy = _LoggedInUser.UserId;
            _Model.UpdatedBy = _LoggedInUser.UserId;
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            //return Json(_Model.AddMenu());
            MenuModel objMenuModel = new MenuModel(new DBLogger());
            //return CommonMessageFactory.Response(CommonMessageFactory.Message(objMenuModel.AddMenu(_Model), (Request.Cookies[SessionIdentity.Culture]).Value));

            return objMenuModel.AddMenu(_Model).ToString();
        }


        //

        //


        [HttpGet]
        [ActionName("DeleteMenu")]
        public JsonResult delete_record(int id)
        {
            MenuModel objMenuActionModel = new MenuModel(new DBLogger());
            return Json(objMenuActionModel.DeleteMenu(id), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //[ActionName("MenuList")]
        //public JsonResult GetAllMenuList()
        //{
        //    MenuModel objMenuModel = new MenuModel(new DBLogger());
        //    return Json(objMenuModel.GetAllMenu(), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public string ChangeStatus(string Id, string Action)
        {
            int MenuId = 0, ActionId = 0;
            if (!int.TryParse(Id, out MenuId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            MenuModel objMenuModel = new MenuModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objMenuModel.ChangeStatus(MenuId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }


        [HttpGet]
        [ActionName("MenuListforParent")]
        public string GetAllMenuParent(string WhereClause = "")
        {
            WhereClause = "and 1=1";
            MenuModel objMenuModel = new MenuModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMenuModel.GetAllMenuParent(WhereClause));
        }



        [HttpGet]
        [ActionName("MenuList")]
        public string GetAllMenu(string WhereClause = "")
        {
            if (WhereClause == "" || WhereClause == "undefined")
            {
                WhereClause = "and 1=1";
            }
            else if (WhereClause.Contains("ParentId"))
            {
                WhereClause = "And M.ParentId=" + WhereClause.Replace("ParentId", "");
            }
            else if (WhereClause != "")
            {
                WhereClause = "AND M.MenuId=" + WhereClause;
            }
            MenuModel objMenuModel = new MenuModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMenuModel.GetAllMenu(WhereClause));
        }

        [HttpGet]
        public JsonResult GetAllMenuList(string WhereClause = "")
        {
            string language = (Request.Cookies[SessionIdentity.Culture]).Value.ToLower();
            if (WhereClause == "" || WhereClause == "undefined")
            {
                WhereClause = "and 1=1";
            }
            else if (WhereClause.Contains("ParentId"))
            {
                WhereClause = "And M.ParentId=" + WhereClause.Replace("ParentId", "");
            }
            else if (WhereClause != "")
            {
                WhereClause = "AND M.MenuId=" + WhereClause;
            }
            MenuModel objMenuModel = new MenuModel(new FileLogger());
            DataTable dt = new DataTable();
            dt=objMenuModel.GetAllMenu(WhereClause);
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
                                list3.Add(new MenuIterate() { Id = Level3.Field<int>("MenuId"), IsChecked = Level3.Field<bool>("IsChecked"), IsParent = false, MenuIcon = Level3.Field<string>("MenuIcon"), MenuLevel = 3, MenuNameEn = Level3.Field<string>("MenuNameEn"), MenuNameHI = Level3.Field<string>("MenuNameHI"), MenuUrl = Level3.Field<string>("MenuUrl"), ParentId = Level3.Field<int>("ParentId"), Priority = Level3.Field<int>("Priority"),
                                    MenuId = Level3.Field<int>("MenuId"),
                                    MenuActionId = Level3.Field<string>("MenuActionId"),
                                    IsActive = Level3.Field<int>("IsActive"),
                                    //CreatedBy = Level3.Field<int>("CreatedBy"),
                                    //UpdatedBy = Level3.Field<int>("UpdatedBy"),
                                    ParentMenuNameEn = Level3.Field<string>("ParentMenuNameEn"),
                                    ParentMenuNameHi = Level3.Field<string>("ParentMenuNameHi"),
                                });

                            }
                            //MenuItemData += "</ul>";
                        }
                        else
                        {
                           // MenuItemData += "<li> <a href=\"" + Level2.MenuUrl + "\"> <i class=\"" + Level2.MenuIcon + "\" aria-hidden=\"true\"></i> <span> " + (language == "en-us" ? Level2.MenuNameEn : Level2.MenuNameHI) + " </span> </a>";
                        }
                        //MenuItemData += "</li>";

                        list2.Add(new MenuLevel2() { Id = Level2.Field<int>("MenuId"), IsChecked = Level2.Field<bool>("IsChecked"), IsParent = false, MenuIcon = Level2.Field<string>("MenuIcon"), MenuLevel = 3, MenuNameEn = Level2.Field<string>("MenuNameEn"), MenuNameHI = Level2.Field<string>("MenuNameHI"), MenuUrl = Level2.Field<string>("MenuUrl"), ParentId = Level2.Field<int>("ParentId"), Priority = Level2.Field<int>("Priority"),
                            MenuId = Level2.Field<int>("MenuId"),
                            MenuActionId = Level2.Field<string>("MenuActionId"),
                            IsActive = Level2.Field<int>("IsActive"),
                            //CreatedBy = Level2.Field<int>("CreatedBy"),
                            //UpdatedBy = Level2.Field<int>("UpdatedBy"),
                            ParentMenuNameEn = Level2.Field<string>("ParentMenuNameEn"),
                            ParentMenuNameHi = Level2.Field<string>("ParentMenuNameHi"),
                            MenuLevel3 =list3 });

                    }
                    //MenuItemData += "</ul>";
                }
                // MenuItemData += " </li>";

                list.Add(new MenuLevel1() { Id = Level1.Field<int>("MenuId"), IsChecked = Level1.Field<bool>("IsChecked"), IsParent = false,
                    MenuIcon = Level1.Field<string>("MenuIcon"), MenuLevel = 3, MenuNameEn = Level1.Field<string>("MenuNameEn"),
                    MenuNameHI = Level1.Field<string>("MenuNameHI"), MenuUrl = Level1.Field<string>("MenuUrl"),
                    ParentId = Level1.Field<int>("ParentId"), Priority = Level1.Field<int>("Priority") ,
                    MenuId = Level1.Field<int>("MenuId"),
                    MenuActionId = Level1.Field<string>("MenuActionId"),
                    IsActive = Level1.Field<int>("IsActive"),
                    //CreatedBy = Level1.Field<int>("CreatedBy"),
                    //UpdatedBy = Level1.Field<int>("UpdatedBy"),
                    ParentMenuNameEn = Level1.Field<string>("ParentMenuNameEn"),
                    ParentMenuNameHi = Level1.Field<string>("ParentMenuNameHi")
                    , MenuLevel2=list2});

            }
            // MenuItemData += "</ul>";
            return Json(new { DataList = list }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]       
        public string GetAllSubMenuList(int ParentId)
        {
            MenuModel objMenuModel = new MenuModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objMenuModel.GetAllSubMenuList(ParentId));
        }

        #endregion
    }
}