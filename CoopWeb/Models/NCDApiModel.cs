using Cooperative.DBConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Cooperative.Models.NCDServiceModel;

namespace Cooperative.Models
{
    public class NCDApiModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        DataTable dt;
        public NCDApiModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public NCDApiModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public DataSet GetCooperativeRegistrationDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeRegistrationDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetCooperativeRegistrationDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetCooperativeRegistrationDetailsById(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeRegistrationDetailById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@is_for_ncd", DBFactoryParameterType.VARCHAR, reqModel.apiKey?.Length>10?1:0);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetCooperativeRegistrationDetailsById(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        #region Save Cooperative data into NCD table and push 2025-09-03 @Pradeep
        public string SaveCooperativeDataIntoNCD(string SocietyIdEnc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_SaveCooperativeNcdData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                return objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "SaveCooperativeDataIntoNCD(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return "";
        }

        #endregion

        public string GenerateNcdId(string sectorId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GenerateNcdId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@sectorId", DBFactoryParameterType.VARCHAR, sectorId);
                var datatable= objDBAccess.GetDataTable();
                if (datatable != null)
                {
                    return datatable.Rows[0]["NCDID"].ToString();
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GenerateNcdId(string sectorId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDAgricultureDetailsByState(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeAgricultureDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDAgricultureDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDAgroProcessingResponseModel> GetNCDAgroProcessingDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDAgroProcessingResponseModel> modelList = new List<NCDAgroProcessingResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeAgroProcessingDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDAgroProcessingResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDAgroProcessingDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDBeeFarmingResponseModel> GetNCDBeeFarmingDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDBeeFarmingResponseModel> modelList = new List<NCDBeeFarmingResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeBeeFarmingDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDBeeFarmingResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDBeeFarmingDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDConsumerResponseModel> GetNCDConsumerDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDConsumerResponseModel> modelList = new List<NCDConsumerResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeConsumerDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDConsumerResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDConsumerDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDCreditThriftResponseModel> GetNCDCreditThriftDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDCreditThriftResponseModel> modelList = new List<NCDCreditThriftResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeCreditThriftDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDCreditThriftResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDCreditThriftDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDDairyResponseModel> GetNCDDairyCooperativeDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDDairyResponseModel> modelList = new List<NCDDairyResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeDairyDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDDairyResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDDairyCooperativeDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDEducationTraningResponseModel> GetNCDEducationTrainingDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDEducationTraningResponseModel> modelList = new List<NCDEducationTraningResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeEducationTrainingDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDEducationTraningResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDEducationTrainingDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDPacsLampsFssResponseModel> GetNCDPacsDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDPacsLampsFssResponseModel> modelList = new List<NCDPacsLampsFssResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativePACSDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null) { modelList = ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[0]); }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDDairyCooperativeDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDFisheryResponseModel> GetNCDFisheryCooperativeDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDFisheryResponseModel> modelList = new List<NCDFisheryResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDFisheryCooperativeDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDFisheryResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDFisheryCooperativeDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDHandicraftResponseModel> GetNCDHandicraftDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDHandicraftResponseModel> modelList = new List<NCDHandicraftResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeHandicraftDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDHandicraftResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDHandicraftDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDHandloomResponseModel> GetNCDHandloomDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDHandloomResponseModel> modelList = new List<NCDHandloomResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeHandloomDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDHandloomResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDHandloomDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDHousingResponseModel> GetNCDHousingDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDHousingResponseModel> modelList = new List<NCDHousingResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeHousingDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDHousingResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDHousingDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDJuteResponseModel> GetNCDJuteCoirDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDJuteResponseModel> modelList = new List<NCDJuteResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeJuteCoirDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDJuteResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDJuteCoirDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDLabourResponseModel> GetNCDLabourDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDLabourResponseModel> modelList = new List<NCDLabourResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeLabourDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDLabourResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDLabourDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDLiveStockResponseModel> GetNCDLiveStockDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDLiveStockResponseModel> modelList = new List<NCDLiveStockResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeLiveStockDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDLiveStockResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDLiveStockDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDMarketingResponseModel> GetNCDMarketingDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDMarketingResponseModel> modelList = new List<NCDMarketingResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeMarketingDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDMarketingResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDMarketingDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDCmiscellaneousResponseModel> GetNCDMiscellaneousCreditDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDCmiscellaneousResponseModel> modelList = new List<NCDCmiscellaneousResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeCmiscellaneousDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDCmiscellaneousResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDMiscellaneousCreditDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDMiscellaneousResponseModel> GetNCDMiscellaneousNonCreditDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDMiscellaneousResponseModel> modelList = new List<NCDMiscellaneousResponseModel>();
            try

            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeMiscellaneousDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDMiscellaneousResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDMiscellaneousDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDMultipurposeResponseModel> GetNCDMultipurposeDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDMultipurposeResponseModel> modelList = new List<NCDMultipurposeResponseModel>();
            try

            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeMultipurposeDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDMultipurposeResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDMultipurposeDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDSericultureResponseModel> GetNCDSericultureDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDSericultureResponseModel> modelList = new List<NCDSericultureResponseModel>();
            try

            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeSericultureDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDSericultureResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDSericultureDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDSocialResponseModel> GetNCDSocialWelfareDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDSocialResponseModel> modelList = new List<NCDSocialResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeSocialWelfareDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDSocialResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDSocialWelfareDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public List<NCDSugarResponseModel> GetNCDSugarDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDSugarResponseModel> modelList = new List<NCDSugarResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeSugarDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);

                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDSugarResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDSugarDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDTourismResponseModel> GetNCDTourismDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDTourismResponseModel> modelList = new List<NCDTourismResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeTourismDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDTourismResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDTourismDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDTransportResponseModel> GetNCDTransportDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDTransportResponseModel> modelList = new List<NCDTransportResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeTransportDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDTransportResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDTransportDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDTribalResponseModel> GetNCDTribalDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDTribalResponseModel> modelList = new List<NCDTribalResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeTribalDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDTribalResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDTribalDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDUCBResponseModel> GetNCDUCBDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDUCBResponseModel> modelList = new List<NCDUCBResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeUCBDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDUCBResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDUCDDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public List<NCDWomenWelfareResponseModel> GetNCDWomenWelfareDetailsByState(NCDApiRequestModel reqModel)
        {
            List<NCDWomenWelfareResponseModel> modelList = new List<NCDWomenWelfareResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeWomenWelfareDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null)
                {
                    modelList = ConvertDTToList<NCDWomenWelfareResponseModel>(ds.Tables[0]);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDWomenWelfareDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public List<NCDKhadiGramResponseModel> GetNCDKhadiGramDetailsByState(NCDApiRequestModel reqModel)
        {
            var modelList = new List<NCDKhadiGramResponseModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeKhadiGramDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds != null) { modelList = ConvertDTToList<NCDKhadiGramResponseModel>(ds.Tables[0]); }
                return modelList;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDKhadiGramDetailsByState()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDMemberDetailsByState(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeMemberDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDMemberDetailsByState(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDAreaOfOprationsRuralDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeAreaOfOprationsDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, "Rural");
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDAreaOfOprationsRuralDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDAreaOfOprationsUrbanDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeAreaOfOprationsDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, "Urban");
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDAreaOfOprationsUrbanDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDSocietyImplementingSchemesDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDSocietyImplementingSchemesDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDSocietyImplementingSchemesDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDCooperativeRegistrationsLandsDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeRegistrationsLandsDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDCooperativeRegistrationsLandsDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetNCDAuditYearDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeAuditYearDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDAuditYearDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNCDBoardofDirectorDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_NCDCooperativeBoardofDirector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, reqModel.cooperative_id);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, reqModel.pageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, reqModel.pageSize);
                DataSet ds = objDBAccess.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDApiModel", FunctionName = "GetNCDBoardofDirectorDetails(NCDApiRequestModel reqModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        /// <summary>
        /// ConvertDTToList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<T> ConvertDTToList<T>(DataTable table) where T : new()
        {
            var properties = typeof(T).GetProperties();
            var result = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (var prop in properties)
                {
                    if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        try
                        {
                            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var value = Convert.ChangeType(row[prop.Name], targetType);
                            prop.SetValue(obj, value);
                        }
                        catch
                        {

                        }
                    }
                }

                result.Add(obj);
            }

            return result;
        }
        public class NCDApiResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public int? total_societies { get; set; }
            public int? functional_societies { get; set; }
            public int? non_functional_societies { get; set; }
            public int? under_liquidation_societies { get; set; }
            public int? total_members { get; set; }
            public Int32 pageNumber { get; set; } = 1;
            public Int32 pageSize { get; set; } = 10;
            public Int32 totalPages { get; set; } = 0;
            public object result { get; set; }
        }
        public class NCDApiErrorResponse
        {
            public string status { get; set; }
            [JsonProperty("Error Message")]
            public string error_message { get; set; }
        }

        public class NCDApiCommonResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public string sector { get; set; }
            public Int32 pageNumber { get; set; }
            public Int32 pageSize { get; set; }
            public object result { get; set; }
        }
        public class NCDApiCommonResponse2
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public string api_type { get; set; }
            public Int32 records { get; set; }
            public Int32 pageNumber { get; set; }
            public Int32 pageSize { get; set; }
            public Int32 totalPages { get; set; }
            public object result { get; set; }
        }

        public class NCDApiRequestModel
        {
            public string apiKey { get; set; }
            public string cooperative_id { get; set; }
            public Int32 pageNumber { get; set; }
            public Int32 pageSize { get; set; }
        }
        public class NCDRootResponse
        {
            public string status { get; set; }

            public string state_name { get; set; }

            public int? state_code { get; set; }

            public int? sector_of_operation { get; set; }

            public string cooperative_id { get; set; }
            public string secure_key { get; set; }

            [JsonProperty("CooperativeRegistrations")]
            public List<NCDCooperativeRegistrationResponseModel> CooperativeRegistrations { get; set; }

            [JsonProperty("MembersDetails")]
            public List<NCDMemberDetailsModel> MembersDetails { get; set; }

            [JsonProperty("AreaOfOperationLevelUrban")]
            public List<NCD_AreaofOperationUrbanDetails> AreaOfOperationLevelUrban { get; set; }

            [JsonProperty("AreaOfOperationLevel")]
            public List<NCD_AreaofOperationRuralDetails> AreaOfOperationLevel { get; set; }

            [JsonProperty("SocietyImplementingSchemes")]
            public List<NCD_SocietyImplementingSchemesDetails> SocietyImplementingSchemes { get; set; }

            [JsonProperty("CooperativeRegistrationsLands")]
            public List<NCD_CooperativeRegistrationsLandsDetails> CooperativeRegistrationsLands { get; set; }

            [JsonProperty("SocietyAuditYears")]
            public List<NcdSocietyAuditYearsModel> SocietyAuditYears { get; set; }

            [JsonProperty("KhadiSectorOtherData")]
            public List<NcdKhadiSectorOtherData> KhadiSectorOtherData { get; set; }

        }
        public class NCDRootResponse<TSector> : NCDRootResponse
        {
            public List<TSector> SectorData { get; set; }
        }
        public class DynamicPropertyResolver : DefaultContractResolver
        {
            private readonly string _sectorPropertyName;

            public DynamicPropertyResolver(string sectorPropertyName)
            {
                _sectorPropertyName = sectorPropertyName;
            }

            protected override string ResolvePropertyName(string propertyName)
            {
                if (propertyName == "SectorData")
                    return _sectorPropertyName;   // 👈 change dynamically
                return base.ResolvePropertyName(propertyName);
            }
        }
    }
}


