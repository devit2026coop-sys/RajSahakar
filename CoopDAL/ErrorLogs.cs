using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Models;

namespace CoopDAL
{
    public static class ErrorLogs
    {
        public static void LogError(ErrorModel errorLogModel, Exception ex)
        {
            using (CoopEntities DB = new CoopEntities())
            {
                DB.ErrorLoggingByWeb("Page Name : " + errorLogModel.PageName + " Function Name : " + errorLogModel.FunctionName + " Message : " + ex.Message + " HelpLink: " + Convert.ToString(ex.HelpLink) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) + " Source: " + Convert.ToString(ex.Source) + " HResult: " + Convert.ToString(ex.HResult) + " TargetSite: " + Convert.ToString(ex.TargetSite) + " Data: " + Convert.ToString(ex.Data));
            }
        }
    }
}
