using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Cooperative.Models
{
    public class WrHolidayModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long Id { get; set; }
        public DateTime HolidayDate { get; set; }
        public string EndTime { get; set; }
        public string AMPM { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }

        public WrHolidayModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public WrHolidayModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int AddWRHoliday()
        {
            int Result = 0;
            objDBAccess.OpenConnection();
          
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("InsertWrHoliday", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@HolidayDate", DBFactoryParameterType.Date, HolidayDate);
                objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, EndTime);
                objDBAccess.AddParameterToCommand("@AMPM", DBFactoryParameterType.NVarchar, "AM");
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);

                Object oj = objDBAccess.ExecuteScalarCommand();
                Result = Convert.ToInt32(oj);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WrHolidayModel", FunctionName = "AddWRHoliday()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        

        //public DataTable GetAllLov(int RoleId, string Menuurl)
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("LOVList2", DBFactoryCommandType.PROCEDURE_COMMAND);

        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "GetAllLov()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //        dt.Dispose();
        //    }

        //    return dt;
        //}

        public DataTable GetAllWrHoliday()
        {
            DataTable dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WrHolidayList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WrHolidayModel", FunctionName = "GetAllWrHoliday()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int ChangeStatus(int Id, int Status)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WrHolidayStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);
                object obj = objDBAccess.ExecuteScalarCommand();
                Result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WrHolidayModel", FunctionName = "ChangeStatus(int Id, int Status)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }
    }

  
}