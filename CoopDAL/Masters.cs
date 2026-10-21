using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Models;

namespace CoopDAL
{
    public class Masters
    {




        public List<GetDistrictDivisionReasion_Result> GetDistrictDivisionReasion()
        {
            using (CoopEntities DB = new CoopEntities())
            {
                return DB.GetDistrictDivisionReasion().ToList();
            }
        }


        public List<TitleList_Result> TitleList()
        {
            List<TitleList_Result> result = new List<TitleList_Result>();
            try
            {

                using (CoopEntities DB = new CoopEntities())
                {
                    result= DB.TitleList().ToList();
                }              
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.MasterModel", FunctionName = "TitleList()" }, ex);
              
            }
           
            return result;
        }


        public List<DepartmentList_Result> DepartmentList()
        {

            List<DepartmentList_Result> result = new List<DepartmentList_Result>();
            try
            {

                using (CoopEntities DB = new CoopEntities())
                {
                    result = DB.DepartmentList().ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DepartmentList()" }, ex);

            }
            return result;
        }

        public List<DesignationtList_Result> DesignationtList(int MasterCode = 0)
        {

            List<DesignationtList_Result> result = new List<DesignationtList_Result>();
            try
            {

                using (CoopEntities DB = new CoopEntities())
                {
                    string whereCase = "";
                    if (MasterCode == -2)
                    {
                        whereCase = " and IS_FOR_ADMINISTRATOR=1 and IS_ACTIVE in(0,1)";                       
                    }
                    else if (MasterCode == -1)
                    {
                        whereCase = "  and IS_ACTIVE in(1)";                        
                    }
                    else if (MasterCode == -3)
                    {
                        whereCase = "   AND IS_ACTIVE in (1,0)";
                    }
                    else if (MasterCode > 0)
                    {
                        whereCase = "   and IS_ACTIVE in(1) AND DEPTCODE=" + MasterCode;                      
                    }


                    result = DB.DesignationtList(whereCase).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DesignationtList()" }, ex);

            }
            return result;
        }

    }
}
