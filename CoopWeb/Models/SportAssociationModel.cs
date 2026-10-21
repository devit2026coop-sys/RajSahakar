using Cooperative.DBConnector;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class SportAssociationModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long AssociationId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Sport Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Sport Name (English) - 100 characters allowed")]
        public string AssociationNameEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Sport Name (Hindi) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Sport Name (Hindi) - 100 characters allowed")]
        public string AssociationNameHi { get; set; }
        public string SportNameEn { get; set; }
        public string SportNameHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }

        public SportAssociationModel(ILogger objErrorLogger)
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public SportAssociationModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }



        public int AddSportAssociation(SportAssociationModel objSportAssociation)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSportAssociation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.BigInt, objSportAssociation.AssociationId);
                objDBAccess.AddParameterToCommand("@AssociationNameEn", DBFactoryParameterType.NVarchar, objSportAssociation.AssociationNameEn);
                objDBAccess.AddParameterToCommand("@AssociationNameHi", DBFactoryParameterType.NVarchar, objSportAssociation.AssociationNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objSportAssociation.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SportAssociationModel", FunctionName = "AddSportAssociation(SportAssociationEntity objSportAssociation)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int DeleteMenufun(int AssociationId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSportAssociation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.BigInt, AssociationId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SportAssociationModel", FunctionName = "DeleteMenufun(int AssociationId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetAllSportAssociation()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSportsAssociationsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
               dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SportAssociationModel", FunctionName = "GetAllSportAssociation(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
    }
}