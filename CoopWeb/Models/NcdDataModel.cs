using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Cooperative.Models.NCDApiModel;
using static Cooperative.Models.NCDServiceModel;

namespace Cooperative.Models
{
    public class NcdDataModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public NcdDataModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public NcdDataModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }


        public DataSet GetBlockByStateDistrict(string statecode, string districtcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdBlockDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBlocksByStateDistrict");
                objDBAccess.AddParameterToCommand("@StateCode", DBFactoryParameterType.NVarchar, statecode);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetBlockByStateDistrict(string statecode, string districtcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetGramPanchayatByStateDistrictBlock(string statecode, string districtcode, string blockcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdGramPanchayat", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGramPanchayatByStateDistrictBlock");
                objDBAccess.AddParameterToCommand("@StateCode", DBFactoryParameterType.NVarchar, statecode);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);
                objDBAccess.AddParameterToCommand("@BlockCode", DBFactoryParameterType.NVarchar, blockcode);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetGramPanchayatByStateDistrictBlock(string statecode, string districtcode, string blockcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetVillageByStateDistrictBlockGramPanchayat(string statecode, string districtcode, string blockcode, string grampanchayatcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdVillageswithBlock", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetVillageByStateDistrictBlockGramPanchayat");
                objDBAccess.AddParameterToCommand("@StateCode", DBFactoryParameterType.NVarchar, statecode);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);
                objDBAccess.AddParameterToCommand("@BlockCode", DBFactoryParameterType.NVarchar, blockcode);
                objDBAccess.AddParameterToCommand("@GramPanchayatCode", DBFactoryParameterType.NVarchar, grampanchayatcode);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetVillageByStateDistrictBlockGramPanchayat(string statecode, string districtcode, string blockcode, string grampanchayatcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNCDAuditCategories()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdAuditCategoriesForSector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNCDAuditCategories");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDAuditCategories()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNCDCooperativeSocietyBanks(string bankTypeId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCooperativeSocietyBanksforSector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNCDCooperativeSocietyBanks");
                objDBAccess.AddParameterToCommand("@BankTypeId", DBFactoryParameterType.VARCHAR, bankTypeId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDCooperativeSocietyBanks()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNCDRegistrationAuthorities()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdRegistrationAuthoritiesDetailsSector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNCDRegistrationAuthorities");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDRegistrationAuthorities()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSocietyFunctionalStatus()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_SocietyFunctionalStatusProc", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyFunctionalStatus");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSocietyFunctionalStatus()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNcdDesignations()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdDesignationsProc", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNcdDesignations");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNcdDesignations()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetFinancialYears()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearProc", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetFinancialYears");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetFinancialYears()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNCDDistrictDetailsoo()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdDistrictsForSector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetAllDistricts");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDDistrictDetails(string Action = 'GetNCDDistrict')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNCDBlockDetails(string districtcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdBlocksForDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBlocksByDistrict");
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDBlockDetails(string districtcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNCDGramPanchayatDetails(string districtcode, string blockcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdGramPanchayatForBlock", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGramPanchayatByDistrictBlock");
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);
                objDBAccess.AddParameterToCommand("@BlockCode", DBFactoryParameterType.NVarchar, blockcode);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDGramPanchayatDetails(string districtcode, string blockcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNCDVillageDetails(string districtcode, string blockcode, string grampanchayatcode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdVillagesForGramPanchayat", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetVillageByDistrictBlockGram");
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, districtcode);
                objDBAccess.AddParameterToCommand("@BlockCode", DBFactoryParameterType.NVarchar, blockcode);
                objDBAccess.AddParameterToCommand("@GramPanchayatCode", DBFactoryParameterType.NVarchar, grampanchayatcode);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNCDVillageDetails(string districtcode, string blockcode, string grampanchayatcode)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetBeeBeehiveTypes()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_Ncd_bee_beehive_typesforPrimaryBee", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBeeBeehiveTypes");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetBeeBeehiveTypes()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetFacilities(string actionType)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NCD_FacilitiesforConsumerCooperativeSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetFacilities(string actionType)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNcdOfficeBuildingTypes()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_Ncd_GetNcdOfficeBuildingTypes", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNcdOfficeBuildingTypes");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetNcdOfficeBuildingTypes()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetDistrictwithStatecode(string statecode)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdDistrictsWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDistrictByStateCode");
                objDBAccess.AddParameterToCommand("@StateorUT", DBFactoryParameterType.NVarchar, statecode);


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetDistrictByStatecode(string statecode = '" + statecode + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetAreaOperationDetails(int operation_area_location)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdAreaOfOperations_Get", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetAreaOfOperations");
                objDBAccess.AddParameterToCommand("@Operation_Area_Location", DBFactoryParameterType.Int, operation_area_location);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetAreaOfOperations(int operationAreaLocation : '" + operation_area_location + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSectorDetailsByIsCredit(string actionName, int? sectorId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdSectorsByCreditDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionName);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetNCDStateDetails(string actionName = '" + actionName + "', int? sectorId = '" + sectorId + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetUrbanCategoryDetails()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdUrbanCategoryLocalBody", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetUrbalCategory");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetNCDStateDetails()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetUrbanLocalBodyByTypecode(int? localbody_type_code)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdUrbanCategoryLocalBody", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetUrbanLocalBodyByTypecode");
                objDBAccess.AddParameterToCommand("@localbody_type_code", DBFactoryParameterType.Int, localbody_type_code);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetUrbanLocalBodyByTypecode(int localbody_type_code = '" + localbody_type_code + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetWardDetailsByCode(int? local_body_code)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdUrbanCategoryLocalBody", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetWardDetailByCode");
                objDBAccess.AddParameterToCommand("@local_body_code", DBFactoryParameterType.Int, local_body_code);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetUrbanLocalBodyByTypecode(int local_body_code = '" + local_body_code + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetWaterBodyTypeDetail()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetWaterBodyDetail");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetWaterBodyTypeDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetCooperativeDataByRegistrationNo(string RegistrationNo)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetCooperativeNcdDataByRegistrationNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetCooperativeDataByRegistrationNo(string RegistrationNo = '" + RegistrationNo + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet SaveNCDData(string ncdJson, int createdBy, string cooperative_id = "")
        {
            string result = string.Empty;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_SaveRcsNcdData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ncdJson", DBFactoryParameterType.NVarchar, ncdJson);
                objDBAccess.AddParameterToCommand("@createdBy", DBFactoryParameterType.Int, createdBy);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, cooperative_id);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "SaveNcd(string ncdJson, int createdBy = '" + createdBy + "', string cooperative_id = '" + cooperative_id + "')"
                }, ex);
                return null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }



        public DataSet GetSectorDashboard(string fromDate = null, string toDate = null, int districtId = 0, int functionalStatusId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetSectorDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                // Pass dates correctly
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSectorDashboard(string fromDate = '" + fromDate + "', string toDate = '" + toDate + "', int? districtId = '" + districtId + "', int? functionalStatusId = '" + functionalStatusId + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetDistrictDashboard(string fromDate = null, string toDate = null, int districtId = 0, int functionalStatusId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDistrictDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetDistrictDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetDashboard(string action, int? stateId = null, int? districtId = null, int? functionalStatusId = null, string fromDate = null, string toDate = null)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, action);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetSocietyMemberDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            DataSet ds = new DataSet();
            List<string> sectorColumns = new List<string>();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_SocietyMemberDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSocietyMemberDashboard(string fromDate = '" + fromDate + "', string toDate = '" + toDate + "', int? districtId = '" + districtId + "', int? functionalStatusId = '" + functionalStatusId + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return (ds);
        }

        public DataSet GetSocietyFacilityByPrimaryActivity(int? primaryId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyFacility", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyFacility");
                objDBAccess.AddParameterToCommand("@PrimaryKey", DBFactoryParameterType.Int, primaryId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetSocietyFacilityByPrimaryActivity(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetTypeOfWomenCooperative(int? primaryId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyFacility", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetTypesOfWomenCoop");
                objDBAccess.AddParameterToCommand("@PrimaryKey", DBFactoryParameterType.Int, primaryId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTypeOfWomenCooperative(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSecondryActivityBySectorOperation(int? sectorOperation)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSecondryActivity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSecondryActivity");
                objDBAccess.AddParameterToCommand("@SectorOperation", DBFactoryParameterType.Int, sectorOperation);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetSocietyFacilityByPrimaryActivity(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetTypeofBeesCooperative()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetTypeBeesCoop");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTypeofBeesCooperative()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetKhadiGramSocietyActivity()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSecondryActivity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGetKhadiGramActivity");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetSocietyFacilityByPrimaryActivity(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetDetailsByCooperative(string cooperativeId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetDetailByCooperative", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDetails");
                objDBAccess.AddParameterToCommand("@CooperativeId", DBFactoryParameterType.NVarchar, cooperativeId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetDetailsByCooperative(string cooperativeId = '" + cooperativeId + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public NCDRootResponse GetNcdCooperativeData(string cooperative_id)
        {
            try
            {
                NCDApiModel ncdApiModel = new NCDApiModel(new DBLogger());
                NCDApiRequestModel reqModel = new NCDApiRequestModel();
                reqModel.cooperative_id = cooperative_id;
                NCDRootResponse rootResponse = null;
                List<NCDCooperativeRegistrationResponseModel> lstCooperativeRegistration = new List<NCDCooperativeRegistrationResponseModel>();
                var ds = ncdApiModel.GetCooperativeRegistrationDetailsById(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {

                    lstCooperativeRegistration = ncdApiModel.ConvertDTToList<NCDCooperativeRegistrationResponseModel>(ds.Tables[0]);

                    if (ds.Tables.Count >= 8)
                    {
                        if (lstCooperativeRegistration[0].sector_of_operation == 1 || lstCooperativeRegistration[0].sector_of_operation == 20 || lstCooperativeRegistration[0].sector_of_operation == 22)
                        {
                            rootResponse = new NCDRootResponse<NCDPacsLampsFssResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 7)
                        {
                            rootResponse = new NCDRootResponse<NCDUCBResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDUCBResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 9)
                        {
                            rootResponse = new NCDRootResponse<NCDDairyResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDDairyResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 10)
                        {
                            rootResponse = new NCDRootResponse<NCDFisheryResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDFisheryResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 11)
                        {
                            rootResponse = new NCDRootResponse<NCDSugarResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSugarResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 13)
                        {
                            rootResponse = new NCDRootResponse<NCDHandloomResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHandloomResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 14)
                        {
                            rootResponse = new NCDRootResponse<NCDHandicraftResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHandicraftResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 15)
                        {
                            rootResponse = new NCDRootResponse<NCDWomenWelfareResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDWomenWelfareResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 16)
                        {
                            rootResponse = new NCDRootResponse<NCDMultipurposeResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMultipurposeResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 18)
                        {
                            rootResponse = new NCDRootResponse<NCDCreditThriftResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDCreditThriftResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 29)
                        {
                            rootResponse = new NCDRootResponse<NCDMiscellaneousResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMiscellaneousResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 31)
                        {
                            rootResponse = new NCDRootResponse<NCDAgroProcessingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDAgroProcessingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 35)
                        {
                            rootResponse = new NCDRootResponse<NCDCmiscellaneousResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDCmiscellaneousResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 47)
                        {
                            rootResponse = new NCDRootResponse<NCDHousingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHousingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 51)
                        {
                            rootResponse = new NCDRootResponse<NCDLabourResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDLabourResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 54)
                        {
                            rootResponse = new NCDRootResponse<NCDLiveStockResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDLiveStockResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 68)
                        {
                            rootResponse = new NCDRootResponse<NCDTransportResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTransportResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 77)
                        {
                            rootResponse = new NCDRootResponse<NCDAgricultureResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDAgricultureResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 79)
                        {
                            rootResponse = new NCDRootResponse<NCDBeeFarmingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDBeeFarmingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 80)
                        {
                            rootResponse = new NCDRootResponse<NCDConsumerResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDConsumerResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 82)
                        {
                            rootResponse = new NCDRootResponse<NCDMarketingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMarketingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 84)
                        {
                            rootResponse = new NCDRootResponse<NCDEducationTraningResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDEducationTraningResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 90)
                        {
                            rootResponse = new NCDRootResponse<NCDJuteResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDJuteResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 96)
                        {
                            rootResponse = new NCDRootResponse<NCDSericultureResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSericultureResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 98)
                        {
                            rootResponse = new NCDRootResponse<NCDSocialResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSocialResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 99)
                        {
                            rootResponse = new NCDRootResponse<NCDTourismResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTourismResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 102)
                        {
                            rootResponse = new NCDRootResponse<NCDTribalResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTribalResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 117)
                        {
                            rootResponse = new NCDRootResponse<NCDKhadiGramResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDKhadiGramResponseModel>(ds.Tables[7])
                            };
                            rootResponse.KhadiSectorOtherData = ncdApiModel.ConvertDTToList<NcdKhadiSectorOtherData>(ds.Tables[8]);
                        }
                    }

                    rootResponse.CooperativeRegistrations = lstCooperativeRegistration;
                    rootResponse.status = "Success";
                    rootResponse.state_code = 8;
                    rootResponse.state_name = "RAJASTHAN";
                    rootResponse.cooperative_id = rootResponse.CooperativeRegistrations[0].cooperative_id;
                    rootResponse.sector_of_operation = rootResponse.CooperativeRegistrations[0].sector_of_operation;
                    rootResponse.MembersDetails = ncdApiModel.ConvertDTToList<NCDMemberDetailsModel>(ds.Tables[1]);
                    rootResponse.AreaOfOperationLevel = ncdApiModel.ConvertDTToList<NCD_AreaofOperationRuralDetails>(ds.Tables[2]);
                    rootResponse.AreaOfOperationLevelUrban = ncdApiModel.ConvertDTToList<NCD_AreaofOperationUrbanDetails>(ds.Tables[3]);
                    rootResponse.SocietyImplementingSchemes = ncdApiModel.ConvertDTToList<NCD_SocietyImplementingSchemesDetails>(ds.Tables[4]);
                    rootResponse.CooperativeRegistrationsLands = ncdApiModel.ConvertDTToList<NCD_CooperativeRegistrationsLandsDetails>(ds.Tables[5]);
                    rootResponse.SocietyAuditYears = ncdApiModel.ConvertDTToList<NcdSocietyAuditYearsModel>(ds.Tables[6]);
                    return rootResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = $"GetNcdCooperativeData({cooperative_id})"
                }, ex);
                return null;
            }
        }

        public string UpdateNCDData(string ncdJson, string actionName, string ncd_id, int updatedBy)
        {
            string result = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_UpdateRcsNcdData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "UpdateNcdBasicDetails");
                objDBAccess.AddParameterToCommand("@ncdJson", DBFactoryParameterType.NVarchar, ncdJson);
                objDBAccess.AddParameterToCommand("@ncd_id", DBFactoryParameterType.NVarchar, ncd_id);
                objDBAccess.AddParameterToCommand("@updatedBy", DBFactoryParameterType.Int, 1);


                //cmd.ExecuteNonQuery();

                object newId = cmd.ExecuteScalar();

                if (newId != null && newId != DBNull.Value)
                    result = newId.ToString();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = "UpdateNCDData()"
                }, ex);
                return null;

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public DataSet GetNcdDetailsById(string cooperative_id, string ncd_id, string cooperative_society_name = null)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_UpdateRcsNcdData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNcdListData");
                //objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.NVarchar, cooperative_id);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.NVarchar, cooperative_id);
                objDBAccess.AddParameterToCommand("@ncd_id", DBFactoryParameterType.NVarchar, ncd_id);
                objDBAccess.AddParameterToCommand("@cooperative_society_name", DBFactoryParameterType.NVarchar, cooperative_society_name);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = "GetNcdOfficeBuildingTypes()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNcdBasicDetailsList(ref DataPagingModel Details, bool isExport = false)
        {
            var ds = new DataSet();
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string FunctionalStatus = "";
                string sectorOperation = "0";
                string cooperative_society_name = "";
                string registrationNo = "";
                string districtcode = "";
                string sectorOperationcode = "";

                if (Details.SearchParameter.Where(x => x.Key == "FunctionalStatus").Any())
                {
                    FunctionalStatus = Details.SearchParameter.Where(x => x.Key == "FunctionalStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "sectorOperation").Any())
                {
                    sectorOperation = Details.SearchParameter.Where(x => x.Key == "sectorOperation").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "cooperative_society_name").Any())
                {
                    cooperative_society_name = Details.SearchParameter.Where(x => x.Key == "cooperative_society_name").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "registrationNo").Any())
                {
                    registrationNo = Details.SearchParameter.Where(x => x.Key == "registrationNo").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "districtcode").Any())
                {
                    districtcode = Details.SearchParameter.Where(x => x.Key == "districtcode").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "sectorOperationcode").Any())
                {
                    sectorOperationcode = Details.SearchParameter.Where(x => x.Key == "sectorOperationcode").Select(x => x.Value).FirstOrDefault();
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("p_NcdCooperativeDetailsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FunctionalStatus", DBFactoryParameterType.VARCHAR, FunctionalStatus);
                objDBAccess.AddParameterToCommand("@sectorOperation", DBFactoryParameterType.VARCHAR, sectorOperation);
                objDBAccess.AddParameterToCommand("@cooperative_society_name", DBFactoryParameterType.VARCHAR, cooperative_society_name);
                objDBAccess.AddParameterToCommand("@registrationNo", DBFactoryParameterType.VARCHAR, registrationNo);
                objDBAccess.AddParameterToCommand("@districtcode", DBFactoryParameterType.VARCHAR, districtcode);
                objDBAccess.AddParameterToCommand("@sectorOperationcode", DBFactoryParameterType.VARCHAR, sectorOperationcode);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                return objDBAccess.GetDataSet();


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Ncddata", FunctionName = "GetNcdBasicDetailsList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                ds.Dispose();
            }

            return ds;
        }

        // for View Data

        public DataSet GetNcdBasicDetailsView(string eid = null, int? functionalStatus = null, int? sectorOperation = null, string cooperative_society_name = null, string registrationNo = null)

        {

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NcdDetailsListdata", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNcdListDataView");
                objDBAccess.AddParameterToCommand("@eid", DBFactoryParameterType.NVarchar, eid);
                objDBAccess.AddParameterToCommand("@FunctionalStatus", DBFactoryParameterType.Int, functionalStatus);
                objDBAccess.AddParameterToCommand("@sectorOperation", DBFactoryParameterType.Int, sectorOperation);
                objDBAccess.AddParameterToCommand("@cooperative_society_name", DBFactoryParameterType.NVarchar, cooperative_society_name);
                objDBAccess.AddParameterToCommand("@registrationNo", DBFactoryParameterType.NVarchar, registrationNo);


                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = "GetNcdBasicDetailsView"
                }, ex);
                return null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }

        public DataSet GetLivestockProductProduce()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetLivestockProduce");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetLivestockProductProduce()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetJuteProductProduce()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetJuteProduce");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetJuteProductProduce()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetJuteRawMeterial()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetJuteRawMeterial");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetJuteRawMeterial()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetTypeSocialCulturalActivitiesDetail()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetTypeSociaActivitiesDetails");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTypeSocialCulturalActivitiesDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetTransportVehicleOwenership(int? sectorOperation)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSecondryActivity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetVehicleOwenership");
                objDBAccess.AddParameterToCommand("@SectorOperation", DBFactoryParameterType.Int, sectorOperation);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTransportVehicleOwenership(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }



        public DataSet GetHandicraftRawMeterial()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetHandicraftRawMeterial");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetHandicraftRawMeterial()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetHandicraftProductProduced()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetHandicraftProductproduced");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTransportVehicleOwenership(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetAgricultureFarmingMechanisms()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetFarmingMechanisms");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetAgricultureFarmingMechanisms()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetEducationCourseOfferedBySector(int? sectorId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sectorId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetEducationCourseOffered");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetEducationCourseOfferedBySector()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSugarProductProduce()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSugarProduce");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetSugarProductProduce()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetBeeFarmingProductProduced()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetBeeFarmingProduct");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetBeeFarmingProductProduced()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetAffiliatedUnionFeederationDetail()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NCDAffiliatedUnionFeederation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetAffiliatedUnionFeederation");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetAffiliatedUnionFeederationLevel()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetTrunoverProfitLossDetail(int district_code = 0, string actionType = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetFinancialTrunOverDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, actionType);
                objDBAccess.AddParameterToCommand("@Districtcode", DBFactoryParameterType.Int, district_code);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetTrunoverProfitLossDetail(int district_code ='" + district_code + "', string actionType = '" + actionType + "', bool isExport = '" + isExport + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetFinancialTrunoverProfitLossDetail(ref DataPagingModel Details, string societyname = "", int location = 0, int district_code = 0, int sectorcode = 0, string actionType = "", bool isExport = false)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetFinancialTrunOverDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.VARCHAR, societyname);
                objDBAccess.AddParameterToCommand("@Location", DBFactoryParameterType.Int, location);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.Int, district_code);
                objDBAccess.AddParameterToCommand("@SectorCode", DBFactoryParameterType.Int, sectorcode);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, actionType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetFinancialTrunoverProfitLossDetail(ref DataPagingModel Details = '" + Details + "', string societyname = '" + societyname + "', int location =  '" + location + "', int district_code =  '" + district_code + "', int sectorcode = '" + district_code + "', string actionType = '" + actionType + "', bool isExport = '" + isExport + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet SaveCooperativeBoardDirector(CooperativeBoardofDirector _directorModel)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_SaveBoardofDirector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "SaveData");
                objDBAccess.AddParameterToCommand("@ncd_bod_id", DBFactoryParameterType.Int, _directorModel.ncd_bod_id);
                objDBAccess.AddParameterToCommand("@bod_Member_name", DBFactoryParameterType.VARCHAR, _directorModel.bod_Member_name);
                objDBAccess.AddParameterToCommand("@gender", DBFactoryParameterType.Int, _directorModel.gender);
                objDBAccess.AddParameterToCommand("@mobile_number", DBFactoryParameterType.BigInt, _directorModel.mobile_number);
                objDBAccess.AddParameterToCommand("@email_id", DBFactoryParameterType.VARCHAR, _directorModel.email_id);
                objDBAccess.AddParameterToCommand("@id_proof_type", DBFactoryParameterType.Int, _directorModel.id_proof_type);
                objDBAccess.AddParameterToCommand("@id_proof_no", DBFactoryParameterType.VARCHAR, _directorModel.id_proof_no);
                objDBAccess.AddParameterToCommand("@bod_designation", DBFactoryParameterType.Int, _directorModel.bod_designation);
                objDBAccess.AddParameterToCommand("@from_date", DBFactoryParameterType.Date, _directorModel.from_date);
                objDBAccess.AddParameterToCommand("@to_date", DBFactoryParameterType.Date, _directorModel.to_date);
                objDBAccess.AddParameterToCommand("@is_any_other_cooperative", DBFactoryParameterType.Int, _directorModel.is_any_other_cooperative);
                objDBAccess.AddParameterToCommand("@other_cooperative_society_name", DBFactoryParameterType.VARCHAR, _directorModel.other_cooperative_society_name);
                objDBAccess.AddParameterToCommand("@sector_code", DBFactoryParameterType.Int, _directorModel.sector_code);
                objDBAccess.AddParameterToCommand("@dist_code", DBFactoryParameterType.Int, _directorModel.dist_code);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, _directorModel.cooperativeId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "SaveCooperativeDirector(CooperativeBoardofDirector model)"
                }, ex);
                return null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }

        public DataSet GetBoardofDirectorByCooperativeId(string cooperativeId = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_SaveBoardofDirector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetDetail");
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, cooperativeId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetBoardofDirectorByCooperativeId(string cooperativeId = '" + cooperativeId + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetCommonProductProduceBySector(int sector_of_operation = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_WaterBodyTypeDetatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCommonProduct");
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, sector_of_operation);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetCommonProductProduceBySector(int sector_of_operation = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetDistrictWiseNCDRegistrationCount(int type = 0, string actionType = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetDistrictWiseNCDRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, type);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetDistrictWiseNCDRegistrationCount()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
    }

    // Insert Data Start From Here

    public class NcdViewModel
    {
        public string Action { get; set; }
        public int OfficeBuilding11a_wcs { get; set; }           // 1 or 0 (or bool if you sent true/false)
        public int? Office_building_type_id { get; set; }        // optional
        public decimal? AuthorisedShareCapital13a_wcs { get; set; }
        public decimal? AnnualTurnOver13b_wcs { get; set; }
        public int RawMaterial13e_wcs { get; set; }              // 1 or 0
        public int? Facility_id { get; set; }


        //MCS


        public int? OfficeBuilding11a_mcs { get; set; }
        public int? OfficeBuildingTypeId_mcs { get; set; }
        public decimal? AuthorizedShareCapital12a_mcs { get; set; }
        public decimal? ShareCapitalMembers12b_mcs { get; set; }
        public decimal? ShareCapitalGovernment12b_mcs { get; set; }
        public decimal? ShareCapitalTotal12b_mcs { get; set; }
        public decimal? AnnualTurnover13a_mcs { get; set; }
        public decimal? TotalDeposits13b_mcs { get; set; }
        public decimal? LoanOutstanding13c_mcs { get; set; }
        public int? MembersIndividual13d_mcs { get; set; }
        public int? MembersInstitutional13d_mcs { get; set; }
        public int? MembersTotal13d_mcs { get; set; }
        public int? FacilityId_mcs { get; set; }

    }

    public class NCDJsonModel
    {
        public string ncdJson { get; set; }

    }



    public class NcdCooperativeDetail
    {

        public int ncd_coop_reg_id { get; set; }

        public string cooperative_society_name { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public string GPName { get; set; }
        public string VillageName { get; set; }
        public string registration_number { get; set; }
        public string FunctionalStatus { get; set; }
        public string Sectorname { get; set; }
        public string IsCredit { get; set; }
        public string authorityname { get; set; }
        public string LocationArea { get; set; }
        public string cooperative_id { get; set; }
        public string reference_year { get; set; }
        public string audityears { get; set; }
        public string date_registration { get; set; }
        public string Action { get; set; }

        public string Location { get; set; }
        public string StateUT { get; set; }

        public string BlockOrULB { get; set; }
        public string RegistrationNumber { get; set; }
        public string NumberOfMembers { get; set; }

        public string StateCode { get; set; }
        public string DistrictCode { get; set; }

        public string CooperativeId { get; set; }
        public string CreatedBy { get; set; }
        public string fulladdress { get; set; }
        public string contact_person { get; set; }
        public string designation { get; set; }
        public int Statecode { get; set; }
        public string Pincode { get; set; }
        public string Mobile { get; set; }
        public string landline { get; set; }
        public string email { get; set; }
        public string sector_of_operation_Type { get; set; }
        public string UrbanRuralArea { get; set; }
        public string waterbody { get; set; }
        public string NumberofmemberofSociety { get; set; }
        public string Profitmaking { get; set; }
        public string netloss { get; set; }
        public string msme { get; set; }
        public string isdevidentpaid { get; set; }
        public string bankname { get; set; }
        public string banktype { get; set; }
        public string affilated { get; set; }
        public string functionstatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        //Table

        public string m_general { get; set; }
        public string f_general { get; set; }
        public string t_general { get; set; }
        public string fsc { get; set; }
        public string f_obc { get; set; }
        public string m_obc { get; set; }
        public string msc { get; set; }
        public string fst { get; set; }
        public string tsc { get; set; }
        public string tst { get; set; }
        public string t_obc { get; set; }
        public string mst { get; set; }
    }



    public class CooperativeBoardofDirector
    {
        public int? ncd_bod_id { get; set; }
        public string bod_Member_name { get; set; }
        public string cooperativeId { get; set; }
        public int gender { get; set; }
        public int? dist_code { get; set; }
        public int? sector_code { get; set; }
        public long mobile_number { get; set; }
        public string email_id { get; set; }
        public int? id_proof_type { get; set; }
        public string id_proof_no { get; set; }
        public int? bod_designation { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public int? is_any_other_cooperative { get; set; }
        public string other_cooperative_society_name { get; set; }
    }

}
