using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Models
{

    public class FAQModel
    {
        public int Id { get; set; }
        public string QuestionEN { get; set; }
        public string QuestionHin { get; set; }
        public string AnswerEN { get; set; }
        public string AnswerHin { get; set; }
        public string IsActive { get; set; }
        public string Priority { get; set; }

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public FAQModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public FAQModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public string InsertUpdateFAQ(FAQModel Details)
        {
            string Result = "4";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("InsertUpdateFAQ", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int,Convert.ToInt32( Details.Id));
                objDBAccess.AddParameterToCommand("@QuestionEN", DBFactoryParameterType.NVarchar, Details.QuestionEN);
                objDBAccess.AddParameterToCommand("@QuestionHin", DBFactoryParameterType.NVarchar, Details.QuestionHin);
                objDBAccess.AddParameterToCommand("@AnswerEN", DBFactoryParameterType.NVarchar, Details.AnswerEN);
                objDBAccess.AddParameterToCommand("@AnswerHin", DBFactoryParameterType.NVarchar, Details.AnswerHin);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, Details.IsActive == "True"?1:0);
                objDBAccess.AddParameterToCommand("@Priority", DBFactoryParameterType.Int, Convert.ToInt32(Details.Priority));

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FAQModel", FunctionName = "InsertUpdateFAQ(FAQModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetFAQList(string WhereCase)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = objDBAccess.CreateCommand("GetFAQList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FAQModel", FunctionName = "GetFAQList(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int DeleteFAQ(int Id, int Action)
        {
            int Return = 1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteFAQ", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, Action);
                Return = objDBAccess.ExecuteCommand();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FAQModel", FunctionName = "DeleteFAQ(int Id, int Action)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Return;
        }


    }
}