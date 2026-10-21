using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using CoopDAL;

namespace Cooperative.Controllers
{
    //[Error]
    //[CustomAuthentication]

    public class RajeevikaMasterController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        [ActionName("GetMasterData")]
        public string GetMasterDataDetails(string MasterType = "", string MasterCode = "", string MasterCode2 = "")
        {

            string _MasterData = "{}";
            int Code = 0;
            int Code1 = 0;

            if (string.IsNullOrEmpty(MasterType) || !int.TryParse(MasterCode, out Code) || !int.TryParse(MasterCode2, out Code1)  )
            {
                // Response.Redirect("~/Views/Error/General.cshtml");
            }

            RajeevikaMasterModel master = new RajeevikaMasterModel(new DBLogger());


            switch (MasterType.ToUpper())
            {
                case "DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DistrictList(Code)); break; }
                case "BLOCK": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.BlockList(Code)); break; }
                case "CLF": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CLFList(Code, Code1) ); break; }
                case "VO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.VOList(Code)); break; }
                case "SHG": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SHGList(Code)); break; }

               
                default: break;

            }
            return _MasterData;
        }



        [HttpGet]
        public string CheckShgMembersExist(string JanAadharNo="",string AadharNo="")
        {
            if (JanAadharNo == "" && AadharNo == "")
                return "{}";

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            RajeevikaMasterModel master = new RajeevikaMasterModel(new DBLogger());

            DataTable DT = master.CheckShgMembersExist(JanAadharNo, AadharNo);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
             
        }
        


    }
}