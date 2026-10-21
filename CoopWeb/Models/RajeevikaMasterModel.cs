using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using CoopDAL;

namespace Cooperative.Models
{
    public class RajeevikaMasterModel
    {
        private ILogger objILogger;
        private RajeevikaDBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public RajeevikaMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new RajeevikaDBFactoryMSSQL();
            dt = new DataTable();
        }     

        public DataTable DistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDistrictList");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable BlockList(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBlockList");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable CLFList(int DistrictId, int BlockId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int,  BlockId);               
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCLFList");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable VOList(int ClfId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ClfId", DBFactoryParameterType.Int, ClfId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetVOList");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable SHGList(int VoId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@VoId", DBFactoryParameterType.Int, VoId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSHGList");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable CheckShgMembersExist(string JanAadharNo="", string AadharNo="")
        {
            if (JanAadharNo != null && JanAadharNo != "")
                JanAadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadharNo));
            if (AadharNo != null && AadharNo != "")
                AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(AadharNo));


            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@JanAadharNo", DBFactoryParameterType.NVarchar, JanAadharNo);
                objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.NVarchar, AadharNo);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "CheckShgMembersExist");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



        public DataTable CommonAllDetails(int DivisionId, int DistrictId, int BlockId,int VoId,string Action)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Coop_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@VoId", DBFactoryParameterType.Int, VoId);               
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajeevikaMasterModel", FunctionName = "CommonAllDetails(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

    }
}

