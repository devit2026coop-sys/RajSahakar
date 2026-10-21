using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using CoopDAL;
using CoopDB;
using Models;


namespace Cooperative.Models
{
    public class EntireSocietyDetailModel
    {
        DBLogger objDBLogger = new DBLogger();
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public EntireSocietyDetailModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public EntireSocietyDetailModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        //create by Madhuri
      

        #region Madhuri
       
        public DataSet GetSocietyDetailsByRegistrationNO(string Reg_no,int Districtid)
        {
            DataSet dt = new DataSet();

            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("EntireSocietyDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, Reg_no);
                objDBAccess.AddParameterToCommand("@Districtid", DBFactoryParameterType.NVarchar, Districtid);
                dt = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntireSocietyDetailModel", FunctionName = "DataTable GetSocietyDetailsByRegistrationNO(string Reg_no)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion
    }

}

