using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Models;
namespace CoopDAL
{
    public class Godown
    {
        public GodownModel GetGodownDashboardData()
        {
            GodownModel result = new GodownModel();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result.TotalGodown = DB.Godowns.Where(x=>x.IsActive.Value==1).Count();
                    result.DistrictWise = DB.districtwiseGodown("COUNT_DISTRICTWISE_GODOWN", 0,0).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Godown", FunctionName = "GetGodownDashboardData()" }, ex);

            }
            return result;
        }
    }


    public class GodownModel
    {
        public int TotalGodown { get; set; }
        public List<districtwiseGodown_Result> DistrictWise { get; set; }

    }
}
