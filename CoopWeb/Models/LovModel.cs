using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class LovModel
    {


        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long LovListId { get; set; }
        public int SchemeId { get; set; }
        public string ListCode { get; set; }
        public string ListDescription { get; set; }
        public string ListDescriptionHi { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }

        public LovModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public LovModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int AddLov()
        {
            int Result = 0;
            objDBAccess.OpenConnection();
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("InsertLovList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LovListId", DBFactoryParameterType.Int, LovListId);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);
                objDBAccess.AddParameterToCommand("@Listdescription", DBFactoryParameterType.NVarchar, ListDescription);
                objDBAccess.AddParameterToCommand("@SchemeId", DBFactoryParameterType.BigInt, SchemeId);
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "AddLov()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int SaveLovValues()
        {
            int Result = 0;
            objDBAccess.OpenConnection();
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("LovValueInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LovListId", DBFactoryParameterType.Int, LovListId);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);
                objDBAccess.AddParameterToCommand("@Listdescription", DBFactoryParameterType.NVarchar, ListDescription);
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "SaveLovValues()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllLov(int RoleId, string Menuurl)
        {
            DataTable dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LOVList2", DBFactoryCommandType.PROCEDURE_COMMAND);
               
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "GetAllLov()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllLovValues()
        {
            DataTable dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LOVValueList", DBFactoryCommandType.PROCEDURE_COMMAND);
               
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "GetAllLovValues()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
             
        public int ChangeLovValueStatus(int Id, int Status)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LOVValueStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);
                object obj = objDBAccess.ExecuteScalarCommand();
                Result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "ChangeLovValueStatus(int Id, int Status)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }


        public int ChangeStatus(int Id, int Status)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LOVStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);
                object obj = objDBAccess.ExecuteScalarCommand();
                Result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LovModel", FunctionName = "ChangeStatus(int Id, int Status)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }



    }
    public class LovValues
    {
        public long LovValuesId { get; set; }
        public long LovListId { get; set; }
        public string LovCode { get; set; }
        public string LovDescription { get; set; }

        public int IsActive { get; set; }
    }
}