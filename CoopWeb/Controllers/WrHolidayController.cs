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
    public class WrHolidayController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];



        [HttpPost]
        [ActionName("SaveWrHoliday")]
        public string WrHolidayInsert(WrHolidayModel _Model)
        {
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            

            return _Model.AddWRHoliday().ToString();
        }


        [HttpPost]
        [ActionName("ChangeStatus")]
        public string ChangeStatus(string Id, string Status)
        {
            int _id = 0, _Status = 0;

            if (!(int.TryParse(Id, out _id) && int.TryParse(Status, out _Status)))
                return "{}";

            WrHolidayModel objWrHolidayModel = new WrHolidayModel(new DBLogger());
            return Convert.ToString(objWrHolidayModel.ChangeStatus(_id, _Status));
        }


        [HttpGet]
        [ActionName("WrHolidayList")]
        public string GetAllWrHoliday()
        {
            WrHolidayModel objWrHolidayModel = new WrHolidayModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objWrHolidayModel.GetAllWrHoliday());
        }

        //[HttpGet]
        //[ActionName("GetWrHolidayById")]
        //public string GetLovValuesByListId(int ListId = 0)
        //{
        //    DataTable DT = new SocietyModel().GetLovValuesByListId(ListId);
        //    return COMMON_FUNCTION.DataTableToJSONString(DT);
        //}
    }
}