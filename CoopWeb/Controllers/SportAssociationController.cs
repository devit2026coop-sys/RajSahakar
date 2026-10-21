using Cooperative.Filters;
using Cooperative.Models;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class SportAssociationController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        #region SportAssociation
        [HttpGet]
        public ActionResult SportAssociation()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Deleteandstatus")]
        public JsonResult delete_record(int id,int Action)
        {
            Action = (Action == 0?1:(Action == 1?0: Action));

            SportAssociationModel objSportAssociationModel = new SportAssociationModel(new FileLogger());
            return Json(objSportAssociationModel.DeleteMenufun(id, Action), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("SportAssociationList")]
        public string GetAllSportAssociations(string WhereClause)
        {
            WhereClause = "Where IsActive in (1,0)";
            SportAssociationModel objSportAssociationModel = new SportAssociationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSportAssociationModel.GetAllSportAssociation());
        }
        #endregion
        [HttpPost]
        [ActionName("SaveSportAssociationnew")]
        public int SaveSocietyRegisteredOfficeDetail(SportAssociationModel objSportAssociation)
        {

            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            objSportAssociation.CreatedBy = objSportAssociation.UpdatedBy = _LoggedInUser.UserId;
            objSportAssociation.CreatedIP = objSportAssociation.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            int Resut = objSportAssociation.AddSportAssociation(objSportAssociation);//same address then

            return Resut;

        }

    }
}