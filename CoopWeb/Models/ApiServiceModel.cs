using Cooperative.Controllers;
using Cooperative.DBConnector;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Cooperative.Models.GCCServiceModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cooperative.Models
{
    public class ApiServiceModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public ApiServiceModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public int AddBeneficiary(BeneficiaryModel beneficiary)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_CMK_Beneficiary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, beneficiary.RegNo);
                objDBAccess.AddParameterToCommand("@FarmerName", DBFactoryParameterType.NVarchar, beneficiary.FarmerName);
                objDBAccess.AddParameterToCommand("@DateofBirth", DBFactoryParameterType.VARCHAR, beneficiary.DateOfBirth);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, beneficiary.FatherName);
                objDBAccess.AddParameterToCommand("@AadhaarHash", DBFactoryParameterType.VARCHAR, beneficiary.AadhaarHash);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, beneficiary.MobileNo);
                objDBAccess.AddParameterToCommand("@StateCode", DBFactoryParameterType.Int, beneficiary.StateCode);
                objDBAccess.AddParameterToCommand("@StateName", DBFactoryParameterType.VARCHAR, beneficiary.StateName);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.Int, beneficiary.DistrictCode);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.VARCHAR, beneficiary.DistrictName);
                objDBAccess.AddParameterToCommand("@SubDistrictCode", DBFactoryParameterType.Int, beneficiary.SubDistrictCode);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, beneficiary.SubDistrictName);
                objDBAccess.AddParameterToCommand("@VillageCode", DBFactoryParameterType.Int, beneficiary.VillageCode);
                objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.VARCHAR, beneficiary.VillageName);
                objDBAccess.AddParameterToCommand("@TownCode", DBFactoryParameterType.Int, beneficiary.TownCode);
                objDBAccess.AddParameterToCommand("@WardCode", DBFactoryParameterType.Int, beneficiary.WardCode);
                objDBAccess.AddParameterToCommand("@WardName", DBFactoryParameterType.VARCHAR, beneficiary.WardName);
                objDBAccess.AddParameterToCommand("@LandSeedingStatus", DBFactoryParameterType.Int, beneficiary.LandSeedingStatus);
                objDBAccess.AddParameterToCommand("@HomeAddress", DBFactoryParameterType.NVarchar, beneficiary.HomeAddress);
                objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.VARCHAR, beneficiary.PinCode);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, beneficiary.Gender);
                objDBAccess.AddParameterToCommand("@Category", DBFactoryParameterType.VARCHAR, beneficiary.FarmerCategory);
                objDBAccess.AddParameterToCommand("@Religion", DBFactoryParameterType.VARCHAR, beneficiary.Religion);
                objDBAccess.AddParameterToCommand("@Email", DBFactoryParameterType.VARCHAR, beneficiary.Email);
                objDBAccess.AddParameterToCommand("@PFMSBeneficiaryCode", DBFactoryParameterType.VARCHAR, beneficiary.PFMSBeneficiaryCode);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "public int AddBeneficiary(BeneficiaryModel beneficiary)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;

        }

        public DataSet GetBeneficiaryDbtStatus(string MobileNo, string DistrictId = "", string RegNo = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBeneficiaryDbtStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.VARCHAR, DistrictId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, MobileNo);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, RegNo);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetBeneficiaryDbtStatus(string MobileNo,string DistrictId=\"\", string RegNo=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public DataSet Get_CMK_StatusTrackerSummary()
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_GetStatusTrackerSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "Get_CMK_StatusTrackerSummary()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public DataTable Get_CMK_MobileData(int DistrictId = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_GetMobileData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.VARCHAR, DistrictId);
                dt = objDBAccess.GetDataSet()?.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "Get_CMK_MobileData(int DistrictId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable Get_CMK_BeneficiaryDbtData(int installmentId = 0, int DistrictId = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_GetBeneficiaryDbtData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InstallmentId", DBFactoryParameterType.Int, installmentId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetDataSet()?.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "Get_CMK_BeneficiaryDbtData(int installmentId =0,int DistrictId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet GetBeneficiaryStatus(string AadhaarNo)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBeneficiaryDbtStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AadhaarNo", DBFactoryParameterType.VARCHAR, AadhaarNo);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetEmitraPlusDbtStatus");
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetBeneficiaryStatus(string MobileNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public int AddCmkDbtResp(CMK_DBT_ResponseModel cMK_DBT_Response)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_Insert_DBT_Response", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.District);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.Name);
                objDBAccess.AddParameterToCommand("@Aadhaar", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.Aadhaar);
                objDBAccess.AddParameterToCommand("@DBTAmount", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.DBTAmount);
                objDBAccess.AddParameterToCommand("@CreditAccountNumber", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditAccountNumber);
                objDBAccess.AddParameterToCommand("@CreditBankCode", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditBankCode);
                objDBAccess.AddParameterToCommand("@CreditPaymentReferenceNo", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditPaymentReferenceNo);
                objDBAccess.AddParameterToCommand("@CreditRemarks", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditRemarks);
                objDBAccess.AddParameterToCommand("@CreditStatus", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditStatus);
                objDBAccess.AddParameterToCommand("@CreditName", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditName);
                objDBAccess.AddParameterToCommand("@CreditTransactionDate", DBFactoryParameterType.VARCHAR, cMK_DBT_Response.CreditTransactionDate);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddCmkDbtResp(CMK_DBT_ResponseModel cMK_DBT_Response)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }
        public int AddUpdateBeneficiaryLOC_PROD(BeneficiaryModel beneficiary)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_Insert_IncrementalData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BeneficiaryDataId", DBFactoryParameterType.VARCHAR, beneficiary.BeneficiaryDataId);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, beneficiary.RegNo);
                objDBAccess.AddParameterToCommand("@FarmerName", DBFactoryParameterType.VARCHAR, beneficiary.FarmerName);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.VARCHAR, beneficiary.FatherName);
                objDBAccess.AddParameterToCommand("@AadhaarHash", DBFactoryParameterType.VARCHAR, beneficiary.AadhaarHash);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, beneficiary.MobileNo);
                objDBAccess.AddParameterToCommand("@StateCode", DBFactoryParameterType.VARCHAR, beneficiary.StateCode);
                objDBAccess.AddParameterToCommand("@StateName", DBFactoryParameterType.VARCHAR, beneficiary.StateName);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.VARCHAR, beneficiary.DistrictCode);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.VARCHAR, beneficiary.DistrictName);
                objDBAccess.AddParameterToCommand("@SubDistrictCode", DBFactoryParameterType.VARCHAR, beneficiary.SubDistrictCode);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, beneficiary.SubDistrictName);
                objDBAccess.AddParameterToCommand("@VillageCode", DBFactoryParameterType.VARCHAR, beneficiary.VillageCode);
                objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.VARCHAR, beneficiary.VillageName);
                objDBAccess.AddParameterToCommand("@TownCode", DBFactoryParameterType.VARCHAR, beneficiary.TownCode);
                objDBAccess.AddParameterToCommand("@WardCode", DBFactoryParameterType.VARCHAR, beneficiary.WardCode);
                objDBAccess.AddParameterToCommand("@WardName", DBFactoryParameterType.VARCHAR, beneficiary.WardName);
                objDBAccess.AddParameterToCommand("@LandSeedingStatus", DBFactoryParameterType.VARCHAR, beneficiary.LandSeedingStatus);
                objDBAccess.AddParameterToCommand("@HomeAddress", DBFactoryParameterType.VARCHAR, beneficiary.HomeAddress);
                objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.VARCHAR, beneficiary.PinCode);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, beneficiary.Gender);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.VARCHAR, beneficiary.FinancialYear);
                objDBAccess.AddParameterToCommand("@Quadrimester", DBFactoryParameterType.VARCHAR, beneficiary.Quadrimester);
                objDBAccess.AddParameterToCommand("@PMKisanInstallmentNo", DBFactoryParameterType.VARCHAR, beneficiary.PMKisanInstallmentNo);
                objDBAccess.AddParameterToCommand("@Eligiblity", DBFactoryParameterType.VARCHAR, beneficiary.Eligiblity);
                objDBAccess.AddParameterToCommand("@RegDate", DBFactoryParameterType.VARCHAR, beneficiary.RegDate);
                objDBAccess.AddParameterToCommand("@MultiplePaymentFlag", DBFactoryParameterType.VARCHAR, beneficiary.MultiplePaymentFlag);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.VARCHAR, beneficiary.DistrictId);
                objDBAccess.AddParameterToCommand("@IsDiscontinued", DBFactoryParameterType.VARCHAR, beneficiary.IsDiscontinued);
                objDBAccess.AddParameterToCommand("@FarmerCategory", DBFactoryParameterType.VARCHAR, beneficiary.FarmerCategory);
                objDBAccess.AddParameterToCommand("@GID", DBFactoryParameterType.VARCHAR, beneficiary.GID);
                objDBAccess.AddParameterToCommand("@TownName", DBFactoryParameterType.VARCHAR, beneficiary.TownName);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateBeneficiaryLOC_PROD(BeneficiaryModel beneficiary)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }


        public int AddUpdateBeneficiaryDataByJson(string beneficiaryJson)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_ImportPMK_Data", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "AddPullServiceData");
                objDBAccess.AddParameterToCommand("@BeneficiaryJson", DBFactoryParameterType.NVarchar, beneficiaryJson);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateBeneficiaryDataByJson(string beneficiaryJson)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }

        public int AddUpdateBeneficiaryDataByDataTable(DataTable dt)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_ImportPMK_Data_ByDataTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                SqlParameter tvpParam = new SqlParameter
                {
                    ParameterName = "@Beneficiaries",
                    SqlDbType = SqlDbType.Structured,
                    Value = dt,
                    TypeName = "dbo.BeneficiaryType_26"
                };
                cmd.Parameters.Add(tvpParam);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateBeneficiaryDataByDataTable(DataTable dt)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }

        //public int AddUpdateBeneficiaryDataByDataTable(DataTable dt)
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("p_ImportPMK_Data_ByDataTable26", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        SqlParameter tvpParam = new SqlParameter
        //        {
        //            ParameterName = "@Beneficiaries",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = dt,
        //            TypeName = "dbo.BeneficiaryType26"
        //        };
        //        cmd.Parameters.Add(tvpParam);
        //        return objDBAccess.ExecuteCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateBeneficiaryDataByDataTable(DataTable dt)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return 0;
        //}

        //public int AddUpdateBeneficiaryDataByDataTable(DataTable dt)
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("p_ImportPMK_Data_ByDataTable_New", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        SqlParameter tvpParam = new SqlParameter
        //        {
        //            ParameterName = "@Beneficiaries",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = dt,
        //            TypeName = "dbo.BeneficiaryTypeNew"
        //        };
        //        cmd.Parameters.Add(tvpParam);
        //        return objDBAccess.ExecuteCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateBeneficiaryDataByDataTable(DataTable dt)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return 0;
        //}

        public int AddUpdateIneligibleDataByDataTable(DataTable dt)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_ImportPMK_IneligibleData_ByDataTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                SqlParameter tvpParam = new SqlParameter
                {
                    ParameterName = "@IneligibleDt",
                    SqlDbType = SqlDbType.Structured,
                    Value = dt,
                    TypeName = "dbo.IneligibleDataType"
                };
                cmd.Parameters.Add(tvpParam);
                return objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "AddUpdateIneligibleDataByDataTable(DataTable dt)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }

        public DataSet GetSocietyApplicationStatus(string RegNo = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyApplicationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegNo);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetSocietyApplicationStatus(string RegNo = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public DataSet GetBRAP_DashboardData()
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBRAP_DashboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetBRAP_DashboardData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public DataSet GetSocietyRegistrationDetails(int PageSize = 50, int PageNumber = 0, int ActId = 0, int Fy = 0, string StartDate = "", string EndDate = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyRegDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.VARCHAR, ActId);
                objDBAccess.AddParameterToCommand("@Fy", DBFactoryParameterType.VARCHAR, Fy);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.VARCHAR, StartDate);
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.VARCHAR, EndDate);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetSocietyRegistrationDetails(int ActId = 0, int Fy = 0, string StartDate = \"\", string EndDate = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }

        public DataSet GetSocietyDetails(int PageSize = 50, int PageNumber = 0, int DistrictId = 0, int ActId = 0, int SocietyTypeId = 0, int SocietyStatusId = 0, string SocietyName = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyRegDetails_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyStatusId", DBFactoryParameterType.Int, SocietyStatusId);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.VARCHAR, SocietyName);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetSocietyDetails(int ActId = 0, int Fy = 0, string StartDate = \"\", string EndDate = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }



        #region MyRegion
        public DataSet GetGccApplicationStatus(string JanAadhaarNo = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGccApplicationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@JanAadhaarNoEnc", DBFactoryParameterType.VARCHAR, JanAadhaarNo);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetGccApplicationStatus(string JanAadhaarNo )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        #endregion

        #region 2001SocietyDetails
        public DataSet Get2001SocietyDetails(string RegNo = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyByRegNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.VARCHAR, DistrictId);
                //objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, MobileNo);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, RegNo);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetBeneficiaryDbtStatus(string MobileNo,string DistrictId=\"\", string RegNo=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        #endregion

        #region ePacs API Data 2025-09-08 #Pradeep

        public DataSet GetePacsApiData(int DistrictId = 0, string FinancialYear = "", int PageNumber = 1, int PageSize = 100)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ePacsDataApi", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Fy", DBFactoryParameterType.VARCHAR, FinancialYear);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, PageNumber);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetePacsApiData(int DistrictId = 0, string FinancialYear = \"\", int PageNumber = 1, int PageSize = 100)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        #endregion


        #region Cm Kisan API Data 2026-01-13 #Pradeep

        public DataSet GeteCmKisanData(int DistrictId = 0, int PageNumber = 1, int PageSize = 100000)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetCMKData_RAJFED", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, PageNumber);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GeteCmKisanData(int DistrictId = 0, int PageNumber = 1, int PageSize = 100000)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        #endregion


        #region Gopal Credit API Data 2026-01-29
        public DataSet GetGopalCreditCardData(int Districtcode = 0, int PageNumber = 1, int PageSize = 100000)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetGopalCreditData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.Int, Districtcode);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, PageNumber);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApiServiceModel", FunctionName = "GetGopalCreditData(int DistrictId = 0, int PageNumber = 1, int PageSize = 100000)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        #endregion

    }

    public class CMK_DBT_ResponseModel
    {
        public string District { get; set; }
        public string Name { get; set; }
        public string Aadhaar { get; set; }
        public string DBTAmount { get; set; }
        public string CreditAccountNumber { get; set; }
        public string CreditBankCode { get; set; }
        public string CreditPaymentReferenceNo { get; set; }
        public string CreditRemarks { get; set; }
        public string CreditStatus { get; set; }
        public string CreditName { get; set; }
        public string CreditTransactionDate { get; set; }

    }
    public class socRegDetails
    {
        public DataPagingModel Details { get; set; }
        public int ActId { get; set; }
        public int Fy { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }


    }

    public class PdcsResponseModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<PdcsModel> Data { get; set; }
    }
    public class PdcsModel
    {
        public string RegNo { get; set; }
        public string PdcsName { get; set; }
        public string PresidentName { get; set; }
        public string AdharNo { get; set; }
        public string MobileNo { get; set; }
        public string SocietyIdEnc { get; set; }
        public Int32 SocietyTypeId { get; set; }
        public float AverageMilk { get; set; }
    }
}