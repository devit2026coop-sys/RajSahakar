using Cooperative.DBConnector;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Cooperative.Models
{
    public class FileUploadersModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public FileUploadersModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable GetAllDirectory(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetDirectoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploaders", FunctionName = "GetAllDirectory(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int InsertDirectory(FileUploaderEntity _Model)
        {
            int Result = -1;

            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                SqlCommand cmdd = objDBAccess.CreateCommand("InsertDirectory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DirectoryName", DBFactoryParameterType.NVarchar, _Model.IsNewDirectory == 1 ? _Model.NewDirectory : _Model.DirectoryName);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.NVarchar, _Model.Flag);
                objDBAccess.AddParameterToCommand("@IsDirectory", DBFactoryParameterType.Bit, _Model.IsNewDirectory);
                objDBAccess.AddParameterToCommand("@File", DBFactoryParameterType.NVarchar, _Model.File);
                objDBAccess.AddParameterToCommand("@SubDirectoryName", DBFactoryParameterType.NVarchar, _Model.SubDirectoryName);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploadersModel", FunctionName = "int InsertDirectory(FileUploaderEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

        public int SaveSocietyAllFile(string Query)
        {
            int Result = -1;

            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                SqlCommand cmdd = objDBAccess.CreateCommand(Query, DBFactoryCommandType.TEXT_COMMAND);
                Result = Convert.ToInt32(objDBAccess.ExecuteCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FileUploadersModel", FunctionName = "int InsertDirectory(FileUploaderEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

    }
}