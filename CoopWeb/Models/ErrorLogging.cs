using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using Cooperative.DBConnector;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class FileLogger : ILogger
    {
        public void ErrorHandle(ErrorLogEntity errorLogModel, Exception ex)
        {
            //string DirctoryPath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM",System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));
            //string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year) + "/" + string.Format("{0}_{1}_{2}.txt", DateTime.Now.ToString("dd", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));

            try
            {
                //if (!Directory.Exists(DirctoryPath))
                //{
                //    DirectoryInfo directory = Directory.CreateDirectory(DirctoryPath);
                //}

                //if (!File.Exists(FilePath))
                //{
                //    using (StreamWriter sw = File.CreateText(FilePath))
                //    {
                //    }
                //}

                //using (StreamWriter sw = File.AppendText(FilePath))
                //{
                //    sw.WriteLine("---------------------------------------------------------");
                //    sw.WriteLine("Log Occurence Time : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture) + "\n");
                //    sw.WriteLine("Page Name : " + errorLogModel.PageName + "\n");
                //    sw.WriteLine("Function Name : " + errorLogModel.FunctionName + "\n");
                //    sw.WriteLine("Message : " + ex.Message + "\n");
                //    sw.WriteLine(ex.Message + " HelpLink: " + Convert.ToString(ex.HelpLink) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) + " Source: " + Convert.ToString(ex.Source) + " HResult: " + Convert.ToString(ex.HResult) + " TargetSite: " + Convert.ToString(ex.TargetSite) + " Data: " + Convert.ToString(ex.Data) + "\n");
                //    sw.WriteLine("---------------------------------------------------------\n");
                //}  
            }
            catch (Exception exception)
            {
                if (HttpContext.Current.IsDebuggingEnabled)
                    throw new Exception("System admin says : " + exception.Message);
            }    
        }
    }

    public class FileTestLogger : ILogger
    {
        //public void ErrorHandleTest(ErrorLogEntity errorLogModel, string ex)
        //{
        //    string DirctoryPath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));
        //    string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year) + "/" + string.Format("{0}_{1}_{2}.txt", DateTime.Now.ToString("dd", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));

        //    try
        //    {
        //        if (!Directory.Exists(DirctoryPath))
        //        {
        //            DirectoryInfo directory = Directory.CreateDirectory(DirctoryPath);
        //        }

        //        if (!File.Exists(FilePath))
        //        {
        //            using (StreamWriter sw = File.CreateText(FilePath))
        //            {
        //            }
        //        }

        //        using (StreamWriter sw = File.AppendText(FilePath))
        //        {
        //            sw.WriteLine("---------------------------------------------------------");
        //            sw.WriteLine("Log Occurence Time : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture) + "\n");
        //            sw.WriteLine("Page Name : " + errorLogModel.PageName + "\n");
        //            sw.WriteLine("Function Name : " + errorLogModel.FunctionName + "\n");
        //            sw.WriteLine("Message : " + ex + "\n");
        //            sw.WriteLine("---------------------------------------------------------\n");
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        if (HttpContext.Current.IsDebuggingEnabled)
        //            throw new Exception("System admin says : " + exception.Message);
        //    }
        //}
        public void ErrorHandle(ErrorLogEntity errorLogModel, Exception ex)
        {
           // string DirctoryPath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));
           // string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year) + "/" + string.Format("{0}_{1}_{2}.txt", DateTime.Now.ToString("dd", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));

            try
            {
                //if (!Directory.Exists(DirctoryPath))
                //{
                //    DirectoryInfo directory = Directory.CreateDirectory(DirctoryPath);
                //}

                //if (!File.Exists(FilePath))
                //{
                //    using (StreamWriter sw = File.CreateText(FilePath))
                //    {
                //    }
                //}

                //using (StreamWriter sw = File.AppendText(FilePath))
                //{
                //    sw.WriteLine("---------------------------------------------------------");
                //    sw.WriteLine("Log Occurence Time : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture) + "\n");
                //    sw.WriteLine("Page Name : " + errorLogModel.PageName + "\n");
                //    sw.WriteLine("Function Name : " + errorLogModel.FunctionName + "\n");
                //    sw.WriteLine("Message : " + ex.Message + "\n");
                //    sw.WriteLine(ex.Message + " HelpLink: " + Convert.ToString(ex.HelpLink) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) + " Source: " + Convert.ToString(ex.Source) + " HResult: " + Convert.ToString(ex.HResult) + " TargetSite: " + Convert.ToString(ex.TargetSite) + " Data: " + Convert.ToString(ex.Data) + "\n");
                //    sw.WriteLine("---------------------------------------------------------\n");
                //}
            }
            catch (Exception exception)
            {
                if (HttpContext.Current.IsDebuggingEnabled)
                    throw new Exception("System admin says : " + exception.Message);
            }
        }
    }

    public class DBLogger : ILogger
    {
        private DBFactoryMSSQL objDBAccess;
        public DBLogger()
        {
            objDBAccess = new DBFactoryMSSQL();
        }
        public void ErrorHandle(ErrorLogEntity errorLogModel, Exception ex)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ErrorLoggingByWeb", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ErrorMessage", DBFactoryParameterType.NVarchar, "Page Name : "+ errorLogModel.PageName + " Function Name : "+ errorLogModel.FunctionName + " Message : "+ ex.Message + " HelpLink: " + Convert.ToString(ex.HelpLink) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) + " Source: " + Convert.ToString(ex.Source) + " HResult: " + Convert.ToString(ex.HResult)+ " TargetSite: " + Convert.ToString(ex.TargetSite) + " Data: " + Convert.ToString(ex.Data));
                objDBAccess.ExecuteCommand();
            }
            catch (Exception exception)
            {
                ILogger objFileLogger = new FileLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { FunctionName = "ErrorHandle()", PageName = "Cooperative.Models.ErrorLogging" }, ex);

                if (HttpContext.Current.IsDebuggingEnabled)
                    throw new Exception("System admin says : " + exception.Message);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }

        public void ErrorHandleMobile(ErrorLogEntity errorLogModel, Exception ex)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ErrorLoggingByWeb", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ErrorMessage", DBFactoryParameterType.NVarchar, "Page Name : " + errorLogModel.PageName + " Function Name : " + errorLogModel.FunctionName + " Exception Message : " + ex.Message);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, "MobileLog");
                objDBAccess.ExecuteCommand();
            }
            catch (Exception exception)
            {
                ILogger objFileLogger = new FileLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { FunctionName = "ErrorHandleMobile(ErrorLogEntity errorLogModel, Exception ex)", PageName = "AgriIfarms.Models.ErrorLogging" }, ex);

                if (HttpContext.Current.IsDebuggingEnabled)
                    throw new Exception("System admin says : " + exception.Message);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }
    }

    public class EmailLogger : ILogger
    {
        public void ErrorHandle(ErrorLogEntity objErrorLogEntity, Exception ex)
        {
            try
            {
                // Read all text from template
                StringBuilder sb = new StringBuilder();
                sb.Append(System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Templates/ErrorEmail.txt")));

                Communication.SendMail(new EmailEntity()
                {
                    To = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"],
                    Body = sb.Replace("##!Message!##", string.Format("Error : {0} </br> Details Message : {1} </br> Trace Path : {2}", ex.Message,ex.InnerException,ex.StackTrace)).ToString(),
                    IsBodyHtml = true,
                    Subject = "Application Error on " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt")
                });
            }
            catch (Exception exception)
            {
                ILogger objFileLogger = new FileLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { FunctionName = "ErrorHandle()", PageName = "Cooperative.Models.ErrorLogging" }, ex);

                if (HttpContext.Current.IsDebuggingEnabled)
                    throw new Exception("System admin says : " + exception.Message);
            }
        }
    }
}