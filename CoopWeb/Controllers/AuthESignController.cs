using System.Web.Mvc;
using Cooperative.Models;


namespace Cooperative.Controllers
{
    public class AuthESignController : BaseController
    {
        AuthESignModel _eSignModel = new AuthESignModel(new DBLogger());
        public ActionResult Esign()
        {
            return View();
        }







    }
}