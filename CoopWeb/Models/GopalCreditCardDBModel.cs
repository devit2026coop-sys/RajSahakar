using CoopDAL;
using CoopDB;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.Filters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using java.util;
using Models;
using Newtonsoft.Json;
using PdfiumViewer;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Xml;
using static Cooperative.Models.GopalCreditCardModel;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Models
{
    public class GopalCreditCardDBModel
    {
        DBLogger objDBLogger = new DBLogger();
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public GopalCreditCardDBModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public GopalCreditCardDBModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        //create by Madhuri

        public int Get_MasterDataIDByCode(string CODE, string Action)
        {
            int id = 0;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Get_MasterDataIDByCode", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CODE", DBFactoryParameterType.NVarchar, CODE);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                dt = objDBAccess.GetTable();

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        id = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return id;
        }

        public string Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)
        {
            string sqlquery = "";
            string Result = "-1";
            int GopalCreditCardId = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //int SubmitStatus = LstSocietyRegisteredOfficeDetails[0].SubmitStatus;
            try
            {

                if (model.JanAadharNo != null && model.JanAadharNo != "")
                    model.JanAadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(model.JanAadharNo));
                if (model.AadharNo != null && model.AadharNo != "")
                    model.AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(model.AadharNo));
                if (model.PANCardNo != null && model.PANCardNo != "")
                    model.PANCardNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(model.PANCardNo));

                //janAaadharFamilyMemberList
                if (model?.janAaadharFamilyMemberList != null)
                    foreach (var a in model.janAaadharFamilyMemberList)
                    {
                        if (a.Encrypted_JAN_AADHAR != null && a.Encrypted_JAN_AADHAR != "")
                            a.Encrypted_JAN_AADHAR = csCommon.Decrypt_SA(Uri.UnescapeDataString(a.Encrypted_JAN_AADHAR));
                    }

                //GuarrantorDetails
                if (model?.GuarantorDetailsList != null)
                    foreach (var a in model.GuarantorDetailsList)
                    {
                        if (a.AadharNo != null && a.AadharNo != "")
                            a.AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(a.AadharNo));

                        if (a.PANCardNo != null && a.PANCardNo != "")
                            a.PANCardNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(a.PANCardNo));

                        if (a.JanAadharNumber != null && a.JanAadharNumber != "")
                            a.JanAadharNumber = csCommon.Decrypt_SA(Uri.UnescapeDataString(a.JanAadharNumber));
                    }

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Save_GopalCreditCardForm()-step-1" }, ex);

            }
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("Insert_GopalCreditCardFormData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, model.gcc_id);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, model.StatusId);
                objDBAccess.AddParameterToCommand("@PBBankAccountNo", DBFactoryParameterType.NVarchar, model.PBBankAccountNo);
                objDBAccess.AddParameterToCommand("@JanAadharNo", DBFactoryParameterType.NVarchar, model.JanAadharNo);
                objDBAccess.AddParameterToCommand("@JanAadharMID", DBFactoryParameterType.NVarchar, !String.IsNullOrEmpty(model.Encrypted_JanAadharMID) == true ? COMMON_FUNCTION.Decrypt(model.Encrypted_JanAadharMID) : "");
                objDBAccess.AddParameterToCommand("@JanAadharEnrollmentId", DBFactoryParameterType.NVarchar, model.JanAadharEnrollmentId);
                objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.NVarchar, model.AadharNo);
                objDBAccess.AddParameterToCommand("@Name_ENG", DBFactoryParameterType.NVarchar, model.Name_ENG);
                objDBAccess.AddParameterToCommand("@Name_HI", DBFactoryParameterType.NVarchar, model.Name_HI);
                objDBAccess.AddParameterToCommand("@FatherName_ENG", DBFactoryParameterType.NVarchar, model.FatherName_ENG);
                objDBAccess.AddParameterToCommand("@FatherName_HI", DBFactoryParameterType.NVarchar, model.FatherName_HI);
                objDBAccess.AddParameterToCommand("@MotherName_ENG", DBFactoryParameterType.NVarchar, model.MotherName_ENG);
                objDBAccess.AddParameterToCommand("@MotherName_HI", DBFactoryParameterType.NVarchar, model.MotherName_HI);
                objDBAccess.AddParameterToCommand("@SpouseName_ENG", DBFactoryParameterType.NVarchar, model.SpouseName_ENG);
                objDBAccess.AddParameterToCommand("@SpouseName_HI", DBFactoryParameterType.NVarchar, model.SpouseName_HI);
                objDBAccess.AddParameterToCommand("@MaritalStatus", DBFactoryParameterType.NVarchar, model.MaritalStatus);
                objDBAccess.AddParameterToCommand("@RelationType", DBFactoryParameterType.NVarchar, model.RelationType);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.NVarchar, model.DOB);
                objDBAccess.AddParameterToCommand("@Age", DBFactoryParameterType.NVarchar, model.Age);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, model.Gender);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, model.MobileNo);
                objDBAccess.AddParameterToCommand("@BankAccountNo", DBFactoryParameterType.NVarchar, model.BankAccountNo);
                objDBAccess.AddParameterToCommand("@BankName", DBFactoryParameterType.NVarchar, model.BankName);
                objDBAccess.AddParameterToCommand("@BankIFSCCode", DBFactoryParameterType.NVarchar, model.BankIFSCCode);
                objDBAccess.AddParameterToCommand("@BankBranchName", DBFactoryParameterType.NVarchar, model.BankBranchName);
                objDBAccess.AddParameterToCommand("@BankMICRCode", DBFactoryParameterType.NVarchar, model.BankMICRCode);
                objDBAccess.AddParameterToCommand("@Caste", DBFactoryParameterType.NVarchar, model.Caste);
                objDBAccess.AddParameterToCommand("@Category", DBFactoryParameterType.NVarchar, model.Category);
                objDBAccess.AddParameterToCommand("@MemberPhoto", DBFactoryParameterType.NVarchar, model.MemberPhoto);
                objDBAccess.AddParameterToCommand("@IsMemberOfDCS", DBFactoryParameterType.Bit, model.IsMemberOfDCS);
                objDBAccess.AddParameterToCommand("@DCSName", DBFactoryParameterType.NVarchar, model.DCSName);
                objDBAccess.AddParameterToCommand("@DCSOwnerName", DBFactoryParameterType.NVarchar, model.DCSOwnerName);
                objDBAccess.AddParameterToCommand("@DCSOwnerMobileNo", DBFactoryParameterType.NVarchar, model.DCSOwnerMobileNo);
                objDBAccess.AddParameterToCommand("@DCSMembershipNo", DBFactoryParameterType.NVarchar, model.DCSMembershipNo);
                objDBAccess.AddParameterToCommand("@DCSAddress", DBFactoryParameterType.NVarchar, model.DCSAddress);
                objDBAccess.AddParameterToCommand("@IsPACSLAMPSMember", DBFactoryParameterType.Bit, model.IsPACSLAMPSMember);
                objDBAccess.AddParameterToCommand("@PACSLAMPSMembershipNo", DBFactoryParameterType.NVarchar, model.PACSLAMPSMembershipNo);
                //objDBAccess.AddParameterToCommand("@IsResidedInRuralArea", DBFactoryParameterType.Bit, model.IsResidedInRuralArea);
                objDBAccess.AddParameterToCommand("@DoesTheMilkSaleToDCS", DBFactoryParameterType.Bit, model.DoesTheMilkSaleToDCS);
                objDBAccess.AddParameterToCommand("@EstimatedAmountOfMilkSoldToDCS", DBFactoryParameterType.Float, model.EstimatedAmountOfMilkSoldToDCS);
                objDBAccess.AddParameterToCommand("@IsRecommendationOfDCSUploaded", DBFactoryParameterType.Bit, model.IsRecommendationOfDCSUploaded);
                objDBAccess.AddParameterToCommand("@RecommendationOfDCSDOC_Path", DBFactoryParameterType.NVarchar, model.RecommendationOfDCSDOC_Path);
                objDBAccess.AddParameterToCommand("@DocumentOfProposedWork_Path", DBFactoryParameterType.NVarchar, model.DocumentOfProposedWork_Path);
                objDBAccess.AddParameterToCommand("@DesiredCreditLimitLoanAmount", DBFactoryParameterType.Float, model.DesiredCreditLimitLoanAmount);
                objDBAccess.AddParameterToCommand("@LoanAmountRecommendedByDCS", DBFactoryParameterType.Float, model.LoanAmountRecommendedByDCS);
                objDBAccess.AddParameterToCommand("@AttachACopyOfDCSPassbook_Path", DBFactoryParameterType.NVarchar, model.CopyOfDCSPassbook_Path);
                objDBAccess.AddParameterToCommand("@ECSMandateConfirmation", DBFactoryParameterType.Bit, model.ECSMandateConfirmation);
                objDBAccess.AddParameterToCommand("@GopalCreditCardLoanDeclaration", DBFactoryParameterType.Bit, model.GopalCreditCardLoanDeclaration);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedByIP", DBFactoryParameterType.NVarchar, model.CreatedByIP);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, model.SSOID);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, model.KIOSKCODE);
                objDBAccess.AddParameterToCommand("@LoanDistrictId", DBFactoryParameterType.NVarchar, model.LoanDistrictId);
                objDBAccess.AddParameterToCommand("@LoanBankID", DBFactoryParameterType.NVarchar, model.LoanBankID);
                objDBAccess.AddParameterToCommand("@LoanBranchId", DBFactoryParameterType.NVarchar, model.LoanBranchId);
                objDBAccess.AddParameterToCommand("@PACS_ID", DBFactoryParameterType.NVarchar, model.PACS_ID);
                objDBAccess.AddParameterToCommand("@DCSRegistrationNo", DBFactoryParameterType.NVarchar, model.DCSRegistrationNo);
                objDBAccess.AddParameterToCommand("@IsCopyOfDCSPassbookAttached", DBFactoryParameterType.Bit, model.IsCopyOfDCSPassbookAttached);
                objDBAccess.AddParameterToCommand("@IsResideInPACSLAMPSArea", DBFactoryParameterType.Bit, model.IsResideInPACSLAMPSArea);
                objDBAccess.AddParameterToCommand("@IsPACSBankAccountAvailable", DBFactoryParameterType.Bit, model.IsPACSBankAccountAvailable);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, model.District_Code);
                //By Madhuri
                objDBAccess.AddParameterToCommand("@AadharDoc", DBFactoryParameterType.NVarchar, model.AadharDoc);
                objDBAccess.AddParameterToCommand("@PanCardDoc", DBFactoryParameterType.NVarchar, model.PanCardDoc);
                objDBAccess.AddParameterToCommand("@PANCardNo", DBFactoryParameterType.NVarchar, model.PANCardNo);
                objDBAccess.AddParameterToCommand("@NoOfCows", DBFactoryParameterType.Int, model.NoOfCows);
                objDBAccess.AddParameterToCommand("@NoOfbuffaloes", DBFactoryParameterType.Int, model.NoOfbuffaloes);
                objDBAccess.AddParameterToCommand("@OtherAnimal", DBFactoryParameterType.Int, model.OtherAnimal);
                objDBAccess.AddParameterToCommand("@AgriculturalProperty", DBFactoryParameterType.NVarchar, model.AgriculturalProperty);
                objDBAccess.AddParameterToCommand("@CurrentMarketValue_AP", DBFactoryParameterType.NVarchar, model.CurrentMarketValue_AP);
                objDBAccess.AddParameterToCommand("@AgriculturalPropertyDoc", DBFactoryParameterType.NVarchar, model.AgriculturalPropertyDoc);
                objDBAccess.AddParameterToCommand("@OtherProperty", DBFactoryParameterType.NVarchar, model.OtherProperty);
                objDBAccess.AddParameterToCommand("@CurrentMarketValue_OP", DBFactoryParameterType.NVarchar, model.CurrentMarketValue_OP);
                objDBAccess.AddParameterToCommand("@OtherPropertyDoc", DBFactoryParameterType.NVarchar, model.OtherPropertyDoc);
                objDBAccess.AddParameterToCommand("@PurposeOfLoan", DBFactoryParameterType.NVarchar, model.PurposeOfLoan);
                objDBAccess.AddParameterToCommand("@PurposeOfLoanOther", DBFactoryParameterType.NVarchar, model.PurposeOfLoanOther);
                objDBAccess.AddParameterToCommand("@CurrentBankIFSCCode", DBFactoryParameterType.NVarchar, model.CurrentBankIFSCCode);
                objDBAccess.AddParameterToCommand("@CurrentLoanDistrictId", DBFactoryParameterType.Int, model.CurrentLoanDistrictId);
                objDBAccess.AddParameterToCommand("@CurrentLoanBankID", DBFactoryParameterType.Int, model.CurrentLoanBankID);
                objDBAccess.AddParameterToCommand("@CurrentLoanBranchId", DBFactoryParameterType.Int, model.CurrentLoanBranchId);
                objDBAccess.AddParameterToCommand("@JanAadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(model.Encrypted_JanAadharNo) ? model.Encrypted_JanAadharNo : ""));
                objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(model.AadharNo) ? COMMON_FUNCTION.Encrypt(model.AadharNo) : ""));
                objDBAccess.AddParameterToCommand("@PANCardNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(model.PANCardNo) ? COMMON_FUNCTION.Encrypt(model.PANCardNo) : ""));

                objDBAccess.AddParameterToCommand("@ShgDistrictId", DBFactoryParameterType.Int, model.ShgDistrictId);
                objDBAccess.AddParameterToCommand("@ShgBlockID", DBFactoryParameterType.Int, model.ShgBlockID);
                objDBAccess.AddParameterToCommand("@ShgClfID", DBFactoryParameterType.Int, model.ShgClfID);
                objDBAccess.AddParameterToCommand("@ShgVoID", DBFactoryParameterType.Int, model.ShgVoID);
                objDBAccess.AddParameterToCommand("@ShgId", DBFactoryParameterType.Int, model.ShgId);
                objDBAccess.AddParameterToCommand("@RenewRefGccId", DBFactoryParameterType.Int, model.RenewRefGccId);
                //Added new 22-01-2026
                objDBAccess.AddParameterToCommand("@IsWorking", DBFactoryParameterType.Bit, model.IsWorking);
                objDBAccess.AddParameterToCommand("@IsInfrastructure", DBFactoryParameterType.Bit, model.IsInfrastructure);
                objDBAccess.AddParameterToCommand("@IsApparatus", DBFactoryParameterType.Bit, model.IsApparatus);
                objDBAccess.AddParameterToCommand("@IsWorkingChild", DBFactoryParameterType.VARCHAR, model.IsWorkingChild);
                objDBAccess.AddParameterToCommand("@IsInfrastructureChild", DBFactoryParameterType.VARCHAR, model.IsInfrastructureChild);
                objDBAccess.AddParameterToCommand("@IsApparatusChild", DBFactoryParameterType.VARCHAR, model.IsApparatusChild);

                /*MahilaNidhi*/
                /*
                objDBAccess.AddParameterToCommand("@Mn_DistrictId", DBFactoryParameterType.Int, model.Mn_DistrictId);
                objDBAccess.AddParameterToCommand("@Mn_DistrictName", DBFactoryParameterType.NVarchar, model.Mn_DistrictName);
                objDBAccess.AddParameterToCommand("@Mn_BlockId", DBFactoryParameterType.BigInt, model.Mn_BlockID);
                objDBAccess.AddParameterToCommand("@Mn_BlockName", DBFactoryParameterType.NVarchar, model.Mn_BlockName);
                objDBAccess.AddParameterToCommand("@Mn_ClusterId", DBFactoryParameterType.NVarchar, model.Mn_ClusterId);
                objDBAccess.AddParameterToCommand("@Mn_ClusterName", DBFactoryParameterType.NVarchar, model.Mn_ClusterName);
                objDBAccess.AddParameterToCommand("@Mn_VoId", DBFactoryParameterType.NVarchar, model.Mn_VoID);
                objDBAccess.AddParameterToCommand("@Mn_VoName", DBFactoryParameterType.NVarchar, model.Mn_VoName);
                objDBAccess.AddParameterToCommand("@Mn_ShgId", DBFactoryParameterType.NVarchar, model.Mn_ShgId);
                objDBAccess.AddParameterToCommand("@Mn_ShgName", DBFactoryParameterType.NVarchar, model.Mn_ShgName);
                objDBAccess.AddParameterToCommand("@Mn_MemberId", DBFactoryParameterType.NVarchar, model.Mn_MemberId);
                objDBAccess.AddParameterToCommand("@SHG_AccountNo", DBFactoryParameterType.NVarchar, model.SHG_AccountNo);
                objDBAccess.AddParameterToCommand("@SHG_IFSC_Code", DBFactoryParameterType.NVarchar, model.SHG_IFSC_Code);
                objDBAccess.AddParameterToCommand("@SHG_BankName", DBFactoryParameterType.NVarchar, model.SHG_BankName);
                objDBAccess.AddParameterToCommand("@SHG_BranchName", DBFactoryParameterType.NVarchar, model.SHG_BranchName);
                objDBAccess.AddParameterToCommand("@SHG_Mem_BankName", DBFactoryParameterType.NVarchar, model.SHG_Mem_BankName);
                objDBAccess.AddParameterToCommand("@SHG_Mem_BranchName", DBFactoryParameterType.NVarchar, model.SHG_Mem_BranchName);
                objDBAccess.AddParameterToCommand("@SHG_Mem_AccountNo", DBFactoryParameterType.NVarchar, model.SHG_Mem_AccountNo);
                objDBAccess.AddParameterToCommand("@SHG_Mem_IFSC_Code", DBFactoryParameterType.NVarchar, model.SHG_Mem_IFSC_Code);
                */
                sqlquery = objDBAccess.getSqlCommandText();

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                if ((Result.Split('@').Length > 1))
                    GopalCreditCardId = int.Parse(Result.Split('@')[1]);

                objDBAccess.CloseConnection();

                if (GopalCreditCardId > 0)
                {
                    string Result_Address = "-1";
                    objDBAccess.OpenConnection();
                    SqlCommand cmd_Address = new SqlCommand();
                    try
                    {
                        #region Save Address                       

                        cmd_Address = objDBAccess.CreateCommand("Insert_GopalCreditCardAddressData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@GopalCreditCardId", DBFactoryParameterType.Int, GopalCreditCardId);
                        objDBAccess.AddParameterToCommand("@IsAddressSameAsJanAadhar", DBFactoryParameterType.Bit, model.IsAddressSameAsJanAadhar);
                        objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, model.IsRural);
                        objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, model.HouseBuildingNo);
                        objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, model.LaneStreetName);
                        objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, model.Locality);
                        objDBAccess.AddParameterToCommand("@Address_ENG", DBFactoryParameterType.NVarchar, model.Address_ENG);
                        objDBAccess.AddParameterToCommand("@Address_HI", DBFactoryParameterType.NVarchar, model.Address_HI);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, model.District_Code);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.NVarchar, model.City_Code);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.NVarchar, model.Ward_Code);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, model.Block_Code);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, model.GP_Code);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, model.Village_Code);
                        objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, model.PinCode);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedByIP", DBFactoryParameterType.NVarchar, model.CreatedByIP);

                        int GopalCreditCardAddressId = 0;
                        Result_Address = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result_Address.Split('@').Length > 1))
                            GopalCreditCardAddressId = int.Parse(Result_Address.Split('@')[1]);

                        sqlquery += objDBAccess.getSqlCommandText();
                        objDBAccess.CloseConnection();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Address Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);
                        objDBAccess.CloseConnection();
                    }

                    try
                    {
                        #region AgricultureLandDetailModel
                        var AgricultureLandDetail = model.AgricultureLandDetailList?.ToList();
                        foreach (var item in AgricultureLandDetail)
                        {

                            string Result_AgricultureLandDetailModel = "-1";
                            objDBAccess.OpenConnection();
                            SqlCommand cmd_AgricultureLandDetailModel = new SqlCommand();

                            cmd_Address = objDBAccess.CreateCommand("Insert_GCC_otherDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "Insert_GCC_AgricultureLandDetail");
                            objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GopalCreditCardId);
                            objDBAccess.AddParameterToCommand("@Village", DBFactoryParameterType.NVarchar, item.Village);
                            objDBAccess.AddParameterToCommand("@PatwarCircle", DBFactoryParameterType.NVarchar, item.PatwarCircle);
                            objDBAccess.AddParameterToCommand("@Irrigated", DBFactoryParameterType.Double, item.Irrigated);
                            objDBAccess.AddParameterToCommand("@KhasraNumber", DBFactoryParameterType.NVarchar, item.KhasraNumber);
                            objDBAccess.AddParameterToCommand("@KhataNumber", DBFactoryParameterType.NVarchar, item.KhataNumber);
                            objDBAccess.AddParameterToCommand("@LandOwnereName", DBFactoryParameterType.NVarchar, item.LandOwnereName);


                            int GopalCreditCardAgricultureLandDetailModel = 0;
                            Result_AgricultureLandDetailModel = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                            if ((Result_AgricultureLandDetailModel.Split('@').Length > 1))
                                GopalCreditCardAgricultureLandDetailModel = int.Parse(Result_AgricultureLandDetailModel.Split('@')[1]);

                            sqlquery += objDBAccess.getSqlCommandText();
                            objDBAccess.CloseConnection();
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {

                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "AgricultureLand Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);
                        objDBAccess.CloseConnection();
                    }

                    try
                    {
                        #region CropLoanDetails
                        var CropLoanDetails = model.CropLoanDetailsList?.ToList();
                        foreach (var item in CropLoanDetails)
                        {
                            string Result_CropLoanDetails = "-1";
                            objDBAccess.OpenConnection();
                            SqlCommand cmd_CropLoanDetails = new SqlCommand();


                            cmd_Address = objDBAccess.CreateCommand("Insert_GCC_otherDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "Insert_GCC_CropLoanDetails");

                            objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GopalCreditCardId);
                            objDBAccess.AddParameterToCommand("@financeInstituteName", DBFactoryParameterType.NVarchar, item.financeInstituteName);
                            objDBAccess.AddParameterToCommand("@NOD", DBFactoryParameterType.Double, item.NOD);
                            objDBAccess.AddParameterToCommand("@OD", DBFactoryParameterType.Double, item.OD);


                            int GopalCreditCardCropLoanDetails = 0;
                            Result_CropLoanDetails = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                            if ((Result_CropLoanDetails.Split('@').Length > 1))
                                GopalCreditCardCropLoanDetails = int.Parse(Result_CropLoanDetails.Split('@')[1]);

                            sqlquery += objDBAccess.getSqlCommandText();
                            objDBAccess.CloseConnection();
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "CropLand Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);
                        objDBAccess.CloseConnection();
                    }

                    try
                    {
                        #region GuarantorDetailsModel
                        var GuarantorDetails = model.GuarantorDetailsList?.ToList();
                        foreach (var item in GuarantorDetails)
                        {
                            string Result_GuarantorDetailsModel = "-1";
                            objDBAccess.OpenConnection();
                            SqlCommand cmd_GuarantorDetailsModel = new SqlCommand();

                            cmd_Address = objDBAccess.CreateCommand("Insert_GCC_otherDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "Insert_GCC_GuarantorDetails");
                            objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GopalCreditCardId);
                            objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, item.Name);
                            objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, item.Gender);
                            objDBAccess.AddParameterToCommand("@FatherSpouseName", DBFactoryParameterType.NVarchar, item.FatherSpouseName);
                            objDBAccess.AddParameterToCommand("@JanAadharNumber", DBFactoryParameterType.NVarchar, item.JanAadharNumber);
                            objDBAccess.AddParameterToCommand("@JanMemberId", DBFactoryParameterType.VARCHAR, item.JanMemberId);
                            objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.VARCHAR, item.AadharNo);
                            objDBAccess.AddParameterToCommand("@IsVerified", DBFactoryParameterType.Bit, item.IsVerified ? 1 : 0);
                            objDBAccess.AddParameterToCommand("@PANCardNo", DBFactoryParameterType.NVarchar, item.PANCardNo);
                            objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, item.MobileNo);
                            objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, item.Address);
                            objDBAccess.AddParameterToCommand("@GuarranterLandProperty", DBFactoryParameterType.NVarchar, item.GuarranterLandProperty);
                            objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, item.Photo);
                            objDBAccess.AddParameterToCommand("@Signature", DBFactoryParameterType.NVarchar, item.Signature);
                            objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.VARCHAR, item.DOB);

                            objDBAccess.AddParameterToCommand("@JanAadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(item.JanAadharNumber) ? COMMON_FUNCTION.Encrypt(item.JanAadharNumber) : ""));
                            objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(item.AadharNo) ? COMMON_FUNCTION.Encrypt(item.AadharNo) : ""));
                            objDBAccess.AddParameterToCommand("@PANCardNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(item.PANCardNo) ? COMMON_FUNCTION.Encrypt(item.PANCardNo) : ""));

                            int GopalCreditCardGuarantorDetailsModel = 0;
                            Result_GuarantorDetailsModel = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                            if ((Result_GuarantorDetailsModel.Split('@').Length > 1))
                                GopalCreditCardGuarantorDetailsModel = int.Parse(Result_GuarantorDetailsModel.Split('@')[1]);


                            sqlquery += objDBAccess.getSqlCommandText();
                            objDBAccess.CloseConnection();
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GuarantorDetails Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);
                        objDBAccess.CloseConnection();
                    }

                    try
                    {
                        #region FamilyMemberData

                        var familyMemberData = model.janAaadharFamilyMemberList?.Where(x => x.IsMemberSelected == true).ToList();
                        if (familyMemberData != null && familyMemberData.Count > 0)
                        {
                            foreach (var item in familyMemberData)
                            {
                                string Result_Member = "-1";
                                objDBAccess.OpenConnection();
                                SqlCommand cmd_Member = new SqlCommand();

                                cmd_Member = objDBAccess.CreateCommand("Insert_GopalCreditCardFamilyMemberData", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("@GopalCreditCardId", DBFactoryParameterType.Int, GopalCreditCardId);
                                objDBAccess.AddParameterToCommand("@JanAadharNo", DBFactoryParameterType.NVarchar, !String.IsNullOrEmpty(item.Encrypted_JAN_AADHAR) == true ? COMMON_FUNCTION.Decrypt(item.Encrypted_JAN_AADHAR) : "");
                                objDBAccess.AddParameterToCommand("@JanAadharMID", DBFactoryParameterType.NVarchar, !String.IsNullOrEmpty(item.Encrypted_JAN_MEMBER_ID) == true ? COMMON_FUNCTION.Decrypt(item.Encrypted_JAN_MEMBER_ID) : "");
                                //objDBAccess.AddParameterToCommand("@JanAadharEnrollmentId", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.Decrypt(item.Encrypted_ENR_ID));
                                objDBAccess.AddParameterToCommand("@JanAadharEnrollmentId", DBFactoryParameterType.NVarchar, item.Encrypted_ENR_ID);
                                objDBAccess.AddParameterToCommand("@Name_ENG", DBFactoryParameterType.NVarchar, item.NAME);
                                objDBAccess.AddParameterToCommand("@Name_HI", DBFactoryParameterType.NVarchar, item.NAME_HI);
                                objDBAccess.AddParameterToCommand("@FatherName_ENG", DBFactoryParameterType.NVarchar, item.FATHER_NAME_ENG);
                                objDBAccess.AddParameterToCommand("@FatherName_HI", DBFactoryParameterType.NVarchar, item.FATHER_NAME_HND);
                                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, item.GENDER);
                                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, !String.IsNullOrEmpty(item.Encrypted_MOBILE_NO) == true ? COMMON_FUNCTION.Decrypt(item.Encrypted_MOBILE_NO) : "");

                                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                                objDBAccess.AddParameterToCommand("@CreatedByIP", DBFactoryParameterType.NVarchar, model.CreatedByIP);

                                objDBAccess.AddParameterToCommand("@JanAadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(item.Encrypted_JAN_AADHAR) ? item.Encrypted_JAN_AADHAR : ""));


                                int GopalCreditCardMemberId = 0;
                                Result_Member = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                                if ((Result_Member.Split('@').Length > 1))
                                    GopalCreditCardMemberId = int.Parse(Result_Member.Split('@')[1]);

                                sqlquery += objDBAccess.getSqlCommandText();
                                objDBAccess.CloseConnection();
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "FamilyMemberData Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);
                        objDBAccess.CloseConnection();
                    }


                    try
                    {
                        string AppRequestID = "";
                        if (Result.Split('#').Length > 1)
                        {

                            AppRequestID = Result.Split('#')[1];
                        }
                        if (_LoggedInUser.UserType == "K" && AppRequestID != "")
                        {
                            var Encrypted_GopalCreditCardId = !String.IsNullOrEmpty(GopalCreditCardId.ToString()) == true ? COMMON_FUNCTION.Encrypt(GopalCreditCardId.ToString()) : "";
                            EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
                            int KioskGccPayment = ObjEmitraModel.MakePayment(AppRequestID, model.Name_ENG, "0", "", GopalCreditCardId.ToString(), Convert.ToInt32(1));

                            if (KioskGccPayment > 0)
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel model)" + sqlquery }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        //2024-07-22 #Pradeep
        public DataSet GetApplicationDetail(string GccIdEnc)
        {
            try
            {
                objDBAccess.OpenConnection();
                // UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("p_GCC_RequiredPoints", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GopalCreditCardIdEnc", DBFactoryParameterType.VARCHAR, GccIdEnc);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetGccAppDetails");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetApplicationDetail(string GccIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public string Save_GopalCreditCardFormPart2(GCC_RequiredPointsModel gCC_RequiredPoints)
        {
            string Result = "-1";
            try
            {
                string jamabandiDoc = null, copyOfLease = null;
                string destDir = HttpContext.Current.Server.MapPath("~/Content/GopalCreditCard/" + gCC_RequiredPoints.RegistrationNo?.Replace("/", "").Replace("\\", "").Replace(" ", ""));

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                if (gCC_RequiredPoints.JamabandiDoc != null && gCC_RequiredPoints.JamabandiDoc != "")
                {
                    if (File.Exists(gCC_RequiredPoints.JamabandiDoc))
                    {
                        jamabandiDoc = Path.GetFileName(gCC_RequiredPoints.JamabandiDoc);
                        File.Move(gCC_RequiredPoints.JamabandiDoc, Path.Combine(destDir, jamabandiDoc));
                    }
                }
                if (gCC_RequiredPoints.CopyOfLease != null && gCC_RequiredPoints.CopyOfLease != "")
                {
                    if (File.Exists(gCC_RequiredPoints.CopyOfLease))
                    {
                        copyOfLease = Path.GetFileName(gCC_RequiredPoints.CopyOfLease);
                        File.Move(gCC_RequiredPoints.CopyOfLease, Path.Combine(destDir, copyOfLease));
                    }
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GCC_RequiredPoints", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "Insert");
                objDBAccess.AddParameterToCommand("@GopalCreditCardIdEnc", DBFactoryParameterType.VARCHAR, gCC_RequiredPoints.GopalCreditCardIdEnc);
                objDBAccess.AddParameterToCommand("@TypesOfProperty", DBFactoryParameterType.Int, gCC_RequiredPoints.TypesOfProperty);
                objDBAccess.AddParameterToCommand("@KhasraNo", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.KhasraNo);
                objDBAccess.AddParameterToCommand("@KhataNo", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.KhataNo);
                objDBAccess.AddParameterToCommand("@LandMeasurement", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.LandMeasurement);
                objDBAccess.AddParameterToCommand("@LandType", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.LandType);
                objDBAccess.AddParameterToCommand("@Other", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.Other);
                objDBAccess.AddParameterToCommand("@JamabandiDoc", DBFactoryParameterType.NVarchar, jamabandiDoc);
                objDBAccess.AddParameterToCommand("@ResidentialPlotAddress", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.ResidentialPlotAddress);
                objDBAccess.AddParameterToCommand("@EstimatedPrice", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.EstimatedPrice);
                objDBAccess.AddParameterToCommand("@CopyOfLease", DBFactoryParameterType.VARCHAR, copyOfLease);
                objDBAccess.AddParameterToCommand("@LoanAmountRecommendByPACS", DBFactoryParameterType.VARCHAR, gCC_RequiredPoints.LoanAmountRecommendByPACS);
                objDBAccess.AddParameterToCommand("@IsRegisteredBankFirstCharge", DBFactoryParameterType.Bit, gCC_RequiredPoints.IsRegisteredBankFirstCharge);
                objDBAccess.AddParameterToCommand("@IsPropertyMortgagedEarlier", DBFactoryParameterType.Bit, gCC_RequiredPoints.IsPropertyMortgagedEarlier);
                objDBAccess.AddParameterToCommand("@EarlierLoanAmount", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.EarlierLoanAmount);
                objDBAccess.AddParameterToCommand("@EarlierLoanFromBank", DBFactoryParameterType.NVarchar, gCC_RequiredPoints.EarlierLoanFromBank);
                objDBAccess.AddParameterToCommand("@EarlierLoanAccountNo", DBFactoryParameterType.VARCHAR, gCC_RequiredPoints.EarlierLoanAccountNo);
                objDBAccess.AddParameterToCommand("@EarlierLoanSanctionDate", DBFactoryParameterType.VARCHAR, gCC_RequiredPoints.EarlierLoanSanctionDate);
                objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.Bit, gCC_RequiredPoints.IsPermanentSave);

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegisteredOfficeDetailsInsert(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        [CustomAuthentication]
        public DataTable ApexBankSocietyDetailList()
        {

            dt.Clear();

            DataTable dset = new DataTable();
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetApexBankSocietyDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                dset = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "ApexBankSocietyDetailList(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public DataTable Get_GopalApplicationFormViewData(string RNO, string Action)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_GopalApplicationFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RNO);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Get_GopalApplicationFormViewData(string RNO, string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable Get_CheckExistsInMMPBY(string JanaadhaarNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Get_CheckExistsInMMPBY", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, JanaadhaarNo);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Get_CheckExistsInMMPBY(string JanaadhaarNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable Get_GopalApplicationFormViewData_v1(string Action = "", int gccid = 0)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_GopalApplicationFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@gccid", DBFactoryParameterType.Int, gccid);

                qry = objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "Get_GopalApplicationFormViewData_v1(string RNO, string Action)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable Get_DashBoardData(GopalCreditCardModel.DashBoardDataModel model)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_GopalCreditCardDashBoardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GopalCreditCardId", DBFactoryParameterType.Int, model.GopalCreditCardId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, model.SSOID);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, model.KIOSKCODE);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable Get_DCSDetailByRegistrationNo(string RNO)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_DCSDetailByRegistrationNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RNO);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable Get_LoanBankBranch(int DistrictId = 0, int CCBId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCB_BranchesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, CCBId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Get_LoanBankBranch(int DistrictId = 0, int CCBId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable Get_LoanBranch_PACS(int BranchId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_CCB_Branch_PACS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Branch_ID", DBFactoryParameterType.Int, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Get_LoanBranch_PACS(int BranchId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CheckGCCRegistrationStatus(string JanMemberIds, string JanAadharNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_CheckGCCRegistrationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@JanAadharNo", DBFactoryParameterType.NVarchar, JanAadharNo);
                objDBAccess.AddParameterToCommand("@JanMemberIds", DBFactoryParameterType.NVarchar, JanMemberIds);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "CheckGCCRegistrationStatus(string JanAadharNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string PushGCCTransactionToJanaadhar_old(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int result = 0)
        {
            string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
            //string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Transaction/Detail?client_id="+ JanAdhaarClientID;
            string url = "http://172.21.245.45/Service/action/transaction";
            string currentDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var scheme = "CO_GKCC";
            var xml = $@"<root>
                    <Transaction>
                    <entitlementId>{_Model.RegistrationNo}</entitlementId>
                    <entitlementMemId>{_Model.JanAadharMID}</entitlementMemId>
                    <janaadhaarId>{_Model.JanAadharNo}</janaadhaarId>
                    <janaadhaarMemId>{_Model.JanAadharMID}</janaadhaarMemId>
                    <transactionId>{result}</transactionId>
                    <dueTransactionId>{result}</dueTransactionId>
                    <aadharNo>{_Model.AadharNo}</aadharNo>
                    <eid></eid>
                    <bankAccNo>{_Model.BankAccountNo}</bankAccNo>
                    <ifsc>{_Model.CurrentBankIFSCCode}</ifsc>
                    <micr></micr>
                    <paymentAmount>{_AppFlowModel.LoanAmountRecommendedByDCS}</paymentAmount>
                    <paymentDate>{currentDate}</paymentDate>
                    <status>Success</status>
                    </Transaction>
                    </root>";
            var responseString = "";
            responseString = Task.Run(() =>
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient())
                {

                    try
                    {
                        var postData = new StringContent($"scheme={scheme}&reqXml={Uri.EscapeDataString(xml)}", Encoding.UTF8, "application/x-www-form-urlencoded");
                        HttpResponseMessage response = client.PostAsync(url, postData).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                    catch (Exception iex)
                    {
                        ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalcreditCardDBModel", FunctionName = $"PushGCCTransactionToJanaadhar()" }, iex);
                        throw iex;
                    }
                }
            }).Result;
            return responseString;

        }
        public string PushGCCTransactionToJanaadhar_Staging(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int result = 0)
        {
            string response = "";
            try
            {
                string scheme = "CO_GKCC";
                string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
                //string Url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Transaction/Detail?client_id="+ JanAdhaarClientID;
                string Url = "http://172.21.245.45/Service/action/transaction";

                string dateTime = DateTime.Now.ToString("dd/MM/yyyy");


                _Model.JanAadharNo = csCommon.Decrypt_SA(_Model.JanAadharNo);
                _Model.AadharNo = COMMON_FUNCTION.Decrypt(_Model.AadharNoEnc);

                StringBuilder requestXml = new StringBuilder();
                requestXml.Append("<root>");
                requestXml.Append("<Transaction>");
                requestXml.Append($"<entitlementId>{_Model.RegistrationNo}</entitlementId>");
                requestXml.Append($"<entitlementMemId>{_Model.JanAadharMID}</entitlementMemId>");
                requestXml.Append($"<janaadhaarId>{_Model.JanAadharNo}</janaadhaarId>");
                requestXml.Append($"<janaadhaarMemId>{_Model.JanAadharMID}</janaadhaarMemId>");
                requestXml.Append($"<transactionId>{result}</transactionId>");
                requestXml.Append($"<dueTransactionId>{result}</dueTransactionId>");
                requestXml.Append($"<aadharNo>{_Model.AadharNo}</aadharNo>");
                requestXml.Append("<eid></eid>");
                requestXml.Append($"<bankAccNo>{_Model.BankAccountNo}</bankAccNo>");
                requestXml.Append($"<ifsc>{_Model.CurrentBankIFSCCode}</ifsc>");
                requestXml.Append("<micr></micr>");
                requestXml.Append($"<paymentAmount>{_AppFlowModel.LoanAmountRecommendedByDCS}</paymentAmount>");
                requestXml.Append($"<paymentDate>{dateTime}</paymentDate>");
                requestXml.Append("<status>Success</status>");
                requestXml.Append("</Transaction>");
                requestXml.Append("</root>");

                // URL-encode the XML
                string encodedXml = Uri.EscapeDataString(requestXml.ToString());

                // Construct POST data as application/x-www-form-urlencoded
                string postData = $"scheme={scheme}&reqXml={encodedXml}";
                byte[] dataBytes = Encoding.UTF8.GetBytes(postData);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = dataBytes.Length;

                using (Stream os = req.GetRequestStream())
                {
                    os.Write(dataBytes, 0, dataBytes.Length);
                }

                using (WebResponse resp = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        response = sr.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalcreditCardDBModel",
                    FunctionName = "PushGCCTransactionToJanaadhar2"
                }, ex);
            }

            return response;
        }

        public string PushGCCTransactionToJanaadhar(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int result = 0)
        {
            string responseString = "";
            try
            {
                string scheme = "CO_GKCC";
                string JanAdhaarClientID = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
                string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Transaction/Detail?client_id=" + JanAdhaarClientID;

                string dateTime = DateTime.Now.ToString("dd/MM/yyyy");

                _Model.JanAadharNo = csCommon.Decrypt_SA(_Model.JanAadharNo);
                _Model.AadharNo = COMMON_FUNCTION.Decrypt(_Model.AadharNoEnc);

                StringBuilder requestXml = new StringBuilder();
                requestXml.Append("<root>");
                requestXml.Append($"<scheme>{scheme}</scheme>");
                requestXml.Append("<Transaction>");
                requestXml.Append($"<entitlementId>{_Model.RegistrationNo}</entitlementId>");
                requestXml.Append($"<entitlementMemId>{_Model.JanAadharMID}</entitlementMemId>");
                requestXml.Append($"<janaadhaarId>{_Model.JanAadharNo}</janaadhaarId>");
                requestXml.Append($"<janaadhaarMemId>{_Model.JanAadharMID}</janaadhaarMemId>");
                requestXml.Append($"<transactionId>{result}</transactionId>");
                requestXml.Append($"<dueTransactionId>{result}</dueTransactionId>");
                requestXml.Append($"<aadharNo>{_Model.AadharNo}</aadharNo>");
                requestXml.Append("<eid></eid>");
                requestXml.Append($"<bankAccNo>{_Model.BankAccountNo}</bankAccNo>");
                requestXml.Append($"<ifsc>{_Model.CurrentBankIFSCCode}</ifsc>");
                requestXml.Append("<micr></micr>");
                requestXml.Append($"<paymentAmount>{_AppFlowModel.LoanAmountRecommendedByDCS}</paymentAmount>");
                requestXml.Append($"<paymentDate>{dateTime}</paymentDate>");
                requestXml.Append("<status>Success</status>");
                requestXml.Append("</Transaction>");
                requestXml.Append("</root>");
                var content = new StringContent(requestXml.ToString(), Encoding.UTF8, "application/xml");
                responseString = Task.Run(() =>
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (HttpClient client = new HttpClient())
                    {

                        try
                        {
                            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                            response.EnsureSuccessStatusCode();
                            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        }
                        catch (Exception iex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalcreditCardDBModel", FunctionName = $"PushGCCTransactionToJanaadhar()" }, iex);
                            return "";//throw iex;
                        }
                    }
                }).Result;


                var a = InsertPushDataStatus(_Model.gcc_id, requestXml.ToString(), responseString);


                return responseString;
            }

            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalcreditCardDBModel",
                    FunctionName = "PushGCCTransactionToJanaadhar2"
                }, ex);
            }
            return responseString;
        }



        #region Madhuri
        [CustomAuthentication]
        public DataTable GetGCCFlowList(ref DataPagingModel Details, bool isExport = false)
        {
            dt.Clear();
            string sqlquery = "";

            try
            {
                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string OverDue = "0";
                string search_input = "";

                if (Details.SearchParameter.Where(x => x.Key == "OverDue30days").Any())
                {
                    OverDue = "30";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "OverDue10days").Any())
                {
                    OverDue = "10";
                }


                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and gcc.LoanDistrictId= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    //whereCase += " and (replace(gcc.Name_ENG,'' '','''') like ''%" + Value + "%'' or replace(gcc.RegistrationNo,'' '','''')  like ''%" + Value + "%'' or replace(A.Remark,'' '','''')  like ''%" + Value + "%'' or  replace(us.OfficerNameEn,'' '','''') like ''%" + Value + "%'' or replace(us.OfficerNameHi,'' '','''') like ''%" + Value + "%'' or replace(SPS.ProceesStatusTextEn,'' '','''') like ''%" + Value + "%'' or replace(SPS.ProceesStatusTextHi,'' '','''') like ''%" + Value + "%'')";
                    //whereCase += " and (replace(gcc.Name_ENG,'' '','''') like ''%" + Value + "%'' or replace(gcc.RegistrationNo,'' '','''')  like ''%" + Value + "%'' /* or replace(A.Remark,'' '','''')  like ''%" + Value + "%'' or  replace(us.OfficerNameEn,'' '','''') like ''%" + Value + "%'' or replace(us.OfficerNameHi,'' '','''') like ''%" + Value + "%'' or replace(SPS.ProceesStatusTextEn,'' '','''') like ''%" + Value + "%'' or replace(SPS.ProceesStatusTextHi,'' '','''') like ''%" + Value + "%'' */ )";
                    search_input = Value;
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetGSSFlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);

                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);

                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@Export", DBFactoryParameterType.Bit, isExport);
                objDBAccess.AddParameterToCommand("@Input", DBFactoryParameterType.NVarchar, search_input);


                sqlquery = objDBAccess.getSqlCommandText();

                dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0 && !isExport)
                {
                    Details.TotalRecords = Int32.Parse(dt.Rows[0]["TotalRecords"].ToString());
                    dt.Columns.Add("Encrypted_RegistrationNo");
                    dt.Columns.Add("Encrypted_GopalCreditCardId");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Encrypted_RegistrationNo"] = !String.IsNullOrEmpty(dt.Rows[i]["RegistrationNo"].ToString()) == true ? COMMON_FUNCTION.Encrypt(dt.Rows[i]["RegistrationNo"].ToString()) : "";
                        dt.Rows[i]["Encrypted_GopalCreditCardId"] = !String.IsNullOrEmpty(dt.Rows[i]["GopalCreditCardId"].ToString()) == true ? COMMON_FUNCTION.Encrypt(dt.Rows[i]["GopalCreditCardId"].ToString()) : "";

                    }
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetGCCFlowList(ref DataPagingModel Details) " + sqlquery }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        //public DataSet GetApplicationFlowGCC(string GCC_Id)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        DataSet dt = new DataSet();    
        //        objDBAccess.OpenConnection();
        //        //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowGCC", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@GCC_id", DBFactoryParameterType.NVarchar, GCC_Id);
        //        objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
        //        //dt = objDBAccess.GetTable();
        //        dt = objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetApplicationFlowGCC(string GCC_Id)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //        dt.Dispose();
        //    }

        //    return dt;
        //}


        public DataSet GetApplicationFlowGCC(string GCC_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowGCC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GCC_id", DBFactoryParameterType.NVarchar, GCC_Id);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
                ds = objDBAccess.GetDataSet();
                objDBAccess.CloseConnection();
                return ds;

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetApplicationFlowGCC(string GCC_Id)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }


        public DataTable GetGCCProceesStatusListIdsByIds(int StatusId, string GCC_Id)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetGCCProceesStatusListByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GCC_Id);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetGCCProceesStatusListIdsByIds(int StatusId, string GCC_Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        [CustomAuthentication]
        public int AddApplicationFlow(int GccId, int StatusId, string Remark, long UserId, string RegistrationNo = "", string DocResStatus = "", int TotalInstallment = 0, decimal LateInterestRate = 0, decimal InterestRate = 0, string fileName = "")
        {
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                objDBAccess.AddParameterToCommand("@DocResStatus", DBFactoryParameterType.NVarchar, DocResStatus);

                objDBAccess.AddParameterToCommand("@TotalInstallment", DBFactoryParameterType.Int, TotalInstallment);
                objDBAccess.AddParameterToCommand("@LateInterestRate", DBFactoryParameterType.Float, LateInterestRate);
                objDBAccess.AddParameterToCommand("@InterestRate", DBFactoryParameterType.Float, InterestRate);
                objDBAccess.AddParameterToCommand("@SanctionDoc", DBFactoryParameterType.NVarchar, fileName);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                //SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }

        [CustomAuthentication]
        public int AddApplicationFlow_V1(GccApplicationFlowModel _Model, int TotalInstallment = 0, decimal LateInterestRate = 0, decimal InterestRate = 0)
        {


            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, _Model.StatusId);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _Model.GccId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _Model.UserId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@DocResStatus", DBFactoryParameterType.NVarchar, _Model.DocResStatus);

                objDBAccess.AddParameterToCommand("@TotalInstallment", DBFactoryParameterType.Int, TotalInstallment);
                objDBAccess.AddParameterToCommand("@LateInterestRate", DBFactoryParameterType.Double, LateInterestRate);
                objDBAccess.AddParameterToCommand("@InterestRate", DBFactoryParameterType.Double, InterestRate);
                objDBAccess.AddParameterToCommand("@SanctionDoc", DBFactoryParameterType.NVarchar, _Model.DocumentURL);


                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocId);
                objDBAccess.AddParameterToCommand("@PurposeOfLoan", DBFactoryParameterType.Int, _Model.PurposeOfLoan);
                objDBAccess.AddParameterToCommand("@PurposeOfLoanOther", DBFactoryParameterType.NVarchar, _Model.PurposeOfLoanOther ?? "");
                objDBAccess.AddParameterToCommand("@LoanAmountRecommendedByDCS", DBFactoryParameterType.Double, _Model.LoanAmountRecommendedByDCS);
                objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, _Model.DocumentURL);
                objDBAccess.AddParameterToCommand("@IsEligibleForRenewable", DBFactoryParameterType.Bit, _Model.IsEligibleForRenewable);


                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                //SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }

        [CustomAuthentication]
        public int AddApplicationFlow_Recovery(GccApplicationFlowModel _Model, int TotalInstallment = 0, decimal LateInterestRate = 0, decimal InterestRate = 0)
        {
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCApplicationFlowRecovery", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, _Model.StatusId);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _Model.GccId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _Model.UserId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@DocResStatus", DBFactoryParameterType.NVarchar, _Model.DocResStatus);

                objDBAccess.AddParameterToCommand("@TotalInstallment", DBFactoryParameterType.Int, TotalInstallment);
                objDBAccess.AddParameterToCommand("@LateInterestRate", DBFactoryParameterType.Double, LateInterestRate);
                objDBAccess.AddParameterToCommand("@InterestRate", DBFactoryParameterType.Double, InterestRate);
                objDBAccess.AddParameterToCommand("@SanctionDoc", DBFactoryParameterType.NVarchar, _Model.DocumentURL);

                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocId);
                objDBAccess.AddParameterToCommand("@PurposeOfLoan", DBFactoryParameterType.Int, _Model.PurposeOfLoan);
                objDBAccess.AddParameterToCommand("@PurposeOfLoanOther", DBFactoryParameterType.NVarchar, _Model.PurposeOfLoanOther ?? "");
                objDBAccess.AddParameterToCommand("@LoanAmountRecommendedByDCS", DBFactoryParameterType.Double, _Model.LoanAmountRecommendedByDCS);
                objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, Path.GetFileName(_Model.DocumentURL));
                objDBAccess.AddParameterToCommand("@IsEligibleForRenewable", DBFactoryParameterType.Bit, _Model.IsEligibleForRenewable);
                objDBAccess.AddParameterToCommand("@IsSatisfiedWithInttCalcByPacs", DBFactoryParameterType.Bit, _Model.IsSatisfiedWithInttCalcByPacs);

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                //SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }


        [CustomAuthentication]
        public int InsertGCCReportFile(GccDocumentsModel _Model)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCReportFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _Model.GccId);
                objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, _Model.DocumentURL);
                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser?.UserId);

                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = " InsertGCCReportFile(GccDocumentsModel _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataSet GetGCCDashboardData()
        {
            DataSet ds = new DataSet();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SQLquery = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GCC_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                SQLquery = objDBAccess.getSqlCommandText();
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "public DataSet GetGCCDashboardData()" + SQLquery }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        [CustomAuthentication]
        public DataTable GetCurrentUserCCBDetail()
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCurrentUserCCBDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataSet GetGccDoc(int GccId)
        {
            try
            {
                objDBAccess.OpenConnection();
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GccDocumentStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "getDocumentStatus");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetGccDoc(int GccId). GccId=" + GccId }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }


        public DataSet GetGccOtherDoc(int GccId)
        {
            string query = "";
            try
            {

                objDBAccess.OpenConnection();
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSetGccDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetGccDocuments");
                query = objDBAccess.getSqlCommandText();
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetGccOtherDoc(int GccId)" + query }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetGopalCreditById(int GopalCreditCardId)
        {
            string query = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPaymentById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GopalCreditCardId", DBFactoryParameterType.Int, GopalCreditCardId);
                query = objDBAccess.getSqlCommandText();
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalCreditCardDBModel",
                    FunctionName = "GetGopalCreditById(int GccId) " + query
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }



        //public DataSet GetGccObjectionPanel(int GccId)
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //        SqlCommand cmd = objDBAccess.CreateCommand("GccDocumentStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
        //        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "getInputFieldStatus");

        //        return objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetGccOtherDoc(int GccId)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return null;
        //}


        [CustomAuthentication]
        public string SaveGccEditableField(string str, int gccid)
        {
            string Result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("InsertEditableField", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "InsertDocumentStatus");
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, gccid);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@inputString", DBFactoryParameterType.VARCHAR, str);




                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "SaveGccEditableField(string str,int gccid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;

            #endregion
        }



        #region Madhuri 


        public static string ConvertTextToPdf(string textFilePath)
        {
            try
            {
                string pdfPath = Path.ChangeExtension(textFilePath, ".pdf");

                // Create a new PDF document
                using (FileStream stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document pdfDoc = new Document();
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Read text from file and add it to the PDF
                    string text = System.IO.File.ReadAllText(textFilePath);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(text));

                    pdfDoc.Close();
                }

                // Console.WriteLine($"Converted {textFilePath} to PDF.");
                return pdfPath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "ConvertTextToPdf(string textFilePath)" }, ex);

                return string.Empty;
            }
        }
        public static string ConvertImageToPdf(string imagePath)
        {
            try
            {
                string pdfPath = Path.ChangeExtension(imagePath, ".pdf");

                // Create a new PDF document
                using (PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument())
                {
                    // Create an empty page in the PDF document
                    PdfSharp.Pdf.PdfPage page = pdf.AddPage();

                    // Get the graphics object to draw on the page
                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                    {
                        // Load the image
                        using (XImage image = XImage.FromFile(imagePath))
                        {
                            // Draw the image on the full page
                            gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                        }
                    }

                    // Save the PDF document to the specified path
                    pdf.Save(pdfPath);
                }

                //Console.WriteLine($"Converted {imagePath} to PDF.");
                return pdfPath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "ConvertJpgToPdf(string jpgPath) " }, ex);
                return string.Empty;
            }
        }
        public static string ConvertPdfToJpg(string pdfFilePath, string outputFolder)
        {
            string outputFilePath = "";
            try
            {

                string baseFileName = Path.GetFileNameWithoutExtension(pdfFilePath);
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(pdfFilePath))
                {
                    for (int pageIndex = 0; pageIndex < 1; pageIndex++)
                    {
                        using (var image = pdfDocument.Render(pageIndex, 300, 300, PdfRenderFlags.CorrectFromDpi))
                        {
                            outputFilePath = Path.Combine(outputFolder, $"{baseFileName}.jpg");
                            image.Save(outputFilePath, ImageFormat.Jpeg);
                        }
                    }
                }

                return outputFilePath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardModel", FunctionName = "ConvertPdfToJpg(string pdfFilePath, string outputFolder) " }, ex);
                return "";
            }
        }
        public static void MergePDF(string filePath, string imagePaths = "")
        {
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (IOException)
            {
                // Wait and retry if file is in use
                System.Threading.Thread.Sleep(500); // Adjust the delay as needed
                System.IO.File.Delete(filePath);
            }

            Document document = new Document(PageSize.A4, 36, 36, 36, 36);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            if (imagePaths.Contains(".pdf"))
            {
                string[] arrPath_Temp = imagePaths.Split('#');

                for (var i = 0; i < arrPath_Temp.Length; i++)
                {
                    string fileExtension = Path.GetExtension(arrPath_Temp[i]).ToLower();

                    if (fileExtension == ".pdf")
                    {
                        arrPath_Temp[i] = GopalCreditCardDBModel.ConvertPdfToJpg(arrPath_Temp[i], Path.GetDirectoryName(filePath).ToString());

                    }
                }

                imagePaths = string.Join("#", arrPath_Temp);
            }

            document.Open();

            // Title and Header
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            Font underlineFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, Font.UNDERLINE);


            iTextSharp.text.Paragraph sanctionOrder = new iTextSharp.text.Paragraph(new Chunk("Guarantor Details", underlineFont));
            sanctionOrder.Alignment = Element.ALIGN_CENTER;
            sanctionOrder.SpacingAfter = 20;
            document.Add(sanctionOrder);

            // Main Table
            PdfPTable mainTable = new PdfPTable(3);
            mainTable.WidthPercentage = 100;
            mainTable.SetWidths(new float[] { 1, 4, 3 });

            // Main Table Header
            mainTable.AddCell(new PdfPCell(new Phrase("Sr. No.", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Photo", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            mainTable.AddCell(new PdfPCell(new Phrase("Sign", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            string[] arrPath = imagePaths.Split('#');
            iTextSharp.text.Image imgG1Photo = iTextSharp.text.Image.GetInstance(arrPath[0]);
            imgG1Photo.ScaleToFit(150f, 200f);
            imgG1Photo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            iTextSharp.text.Image imgG1Sign = iTextSharp.text.Image.GetInstance(arrPath[1]);
            imgG1Sign.ScaleToFit(150f, 150f);
            imgG1Sign.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            iTextSharp.text.Image imgG2Photo = iTextSharp.text.Image.GetInstance(arrPath[2]);
            imgG2Photo.ScaleToFit(150f, 200f);
            imgG2Photo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            iTextSharp.text.Image imgG2Sign = iTextSharp.text.Image.GetInstance(arrPath[3]);
            imgG2Sign.ScaleToFit(150f, 150f);
            imgG2Sign.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            // Main Table Data Row
            mainTable.AddCell(new PdfPCell(new Phrase("1", regularFont)));
            mainTable.AddCell(new PdfPCell(imgG1Photo) { HorizontalAlignment = 1, VerticalAlignment = 5 });
            mainTable.AddCell(new PdfPCell(imgG1Sign) { HorizontalAlignment = 1, VerticalAlignment = 5 });


            mainTable.AddCell(new PdfPCell(new Phrase("2", regularFont)));
            mainTable.AddCell(new PdfPCell(imgG2Photo) { HorizontalAlignment = 1, VerticalAlignment = 5 });
            mainTable.AddCell(new PdfPCell(imgG2Sign) { HorizontalAlignment = 1, VerticalAlignment = 5 });


            document.Add(mainTable);
            document.Close();
        }
        #endregion

        #region Assign User 
        public DataTable GetUserForGccAssign(int GccId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetUserForGccAssign");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetUserForGccAssign(" + GccId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public string GccAssignOfficerSave(int GccId, int UserId)
        {
            string result = "-1";
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GccAssignUser");
                result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GccAssignOfficerSave(string " + GccId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }


        public DataTable GetUserListForFileTransfer(int CCB_BankID, int CCB_BranchID, int CCB_PacsID)
        {
            DataTable result = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CCB_BankID", DBFactoryParameterType.Int, CCB_BankID);
                objDBAccess.AddParameterToCommand("@CCB_BranchID", DBFactoryParameterType.Int, CCB_BranchID);
                objDBAccess.AddParameterToCommand("@CCB_PacsID", DBFactoryParameterType.Int, CCB_PacsID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetUserListForFileTransfer");
                result = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetUserListForFileTransfer(int CCB_BankID , int CCB_BranchID , int CCB_PacsID)" }, ex);

            }
            return result;
        }


        public DataTable GetUserGccList(int UserId, int MappingID)
        {
            DataTable result = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetUserGccList");
                result = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetUserGccList(int UserId , int MappingID)" }, ex);

            }
            return result;
        }

        public DataTable GetTransferToUserList(int UserId, int MappingID)
        {
            DataTable result = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetTransferToUserList");
                result = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetTransferToUserList(int UserId, int MappingID)" }, ex);

            }
            return result;
        }

        [CustomAuthentication]
        public int TransferGcc(string GccIDS, int UserId, int OldUserId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int result = 0;
            string sqlquery = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_UserForGccAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccIDS", DBFactoryParameterType.NVarchar, GccIDS);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@OldUserId", DBFactoryParameterType.Int, OldUserId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIp", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedIP);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "SaveGccTransferToUserList");
                sqlquery = objDBAccess.getSqlCommandText();
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "TransferGcc(string GccIDS, int UserId, int OldUserId)" + sqlquery }, ex);

            }
            return result;
        }
        #endregion

        #region Remove unnecessary guarantors

        public DataTable GetUnnecessaryGuarantors()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetUnnecessaryFiles", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetInactiveGuarrantor");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = " GetUnnecessaryGuarantors()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int RemoveFileOfUnnecessaryGuarantors(int Id)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetUnnecessaryFiles", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "StatusChange");

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " RemoveFileOfUnnecessaryGuarantors(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        #endregion

        #region 
        [CustomAuthentication]
        public int InsertPushDataStatus(int GccId, string Request = "", string Response = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@Response", DBFactoryParameterType.NVarchar, Response);
                objDBAccess.AddParameterToCommand("@Request", DBFactoryParameterType.NVarchar, Request);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Response);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);


                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = " InsertPushDataStatus(int GccId,string Response=\"\"" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion

        #region GccMapSSO
        public DataTable GetSSOMapRequestList(string registrationNo, int pacsId = 0)
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGccSSOMapRequestList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _LoggedInUser.GccId);
                objDBAccess.AddParameterToCommand("@PacsId", DBFactoryParameterType.Int, pacsId);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.VARCHAR, registrationNo ?? "");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSSOMapRequestList(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }
        public string UpdateGccMAPRequest(string GccIdEnc, string SSOId, string Remarks)
        {

            string result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("UpdateGccMAPRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@AprovedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@AprovedBySSO", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Remarks);
                //objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ReqGccId", DBFactoryParameterType.NVarchar, GccIdEnc);
                objDBAccess.AddParameterToCommand("@MAPSSOId", DBFactoryParameterType.NVarchar, SSOId);
                result = objDBAccess.ExecuteScalarCommand().ToString();
                //cmd.ExecuteNonQuery();
                //result = "1";
            }
            catch (Exception ex)
            {
                result = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable SaveCCSCompaintTransferRequest(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        #endregion

        #region RecoveryGcc
        //public DataTable GetRecoverableGccList(ref DataPagingModel Details, bool IsExport = false)
        //{
        //    dt.Clear();
        //    UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        //    objDBAccess.OpenConnection();
        //    SqlCommand cmd = new SqlCommand();
        //    try
        //    {
        //        //string whereCase = " ";
        //        string SearchParamsJson = JsonConvert.SerializeObject(Details.SearchParameter);
        //        Console.WriteLine(SearchParamsJson);

        //        cmd = objDBAccess.CreateCommand("GetRecoverableGccList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
        //        objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
        //        objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
        //        objDBAccess.AddParameterToCommand("@SearchParameterJson", DBFactoryParameterType.NVarchar, SearchParamsJson);
        //        objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.NVarchar, IsExport);

        //        //Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //        dt = objDBAccess.GetTable();
        //        Details.TotalRecords = dt!=null && dt.Rows.Count>0?   Convert.ToInt32(dt.Rows[0]["TotalRecords"]) :0;


        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyBasicDetail()" }, ex);
        //    }

        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        public int SetGccRecoverablePayment(string JsonStr)
        {
            int Result = -1;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("InsertGccPaymentRecovery", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SearchParameterJson", DBFactoryParameterType.NVarchar, JsonStr);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIp", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.CreatedIP);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardModel", FunctionName = " SetGccRecoverablePayment(string JsonStr)" + JsonStr }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion

        //changes by jaya
        #region check registration no Exist or not
        public bool CheckRegistrationNoExists(string registrationNo)
        {
            bool exists = false;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CheckRegistrationNoExists", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, registrationNo);
                int result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                exists = result > 0;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalCreditCardModel",
                    FunctionName = "CheckRegistrationNoExists",
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return exists;
        }

        #endregion

        public int UpdateGeneralNumber(int gccId, int paymentSummaryId, string generalNumber, int StatusId)
        {
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("UpdateGeneralNumberByGcc", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, gccId);
                objDBAccess.AddParameterToCommand("@PaymentSummaryId", DBFactoryParameterType.Int, paymentSummaryId);
                objDBAccess.AddParameterToCommand("@GeneralNumber", DBFactoryParameterType.NVarchar, generalNumber);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);

                DataSet ds = objDBAccess.GetDataSet();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["RowsAffected"]);
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalCreditCardDBModel",
                    FunctionName = "UpdateGeneralNumber(int gccId, int paymentSummaryId, int generalNumber)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return 0;
        }

        #region check gurantor exist and OD case
        public int ValidateGurantor(string janAadharNo, string memberId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CheckGccGurantor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@JanAadharNoEnc", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.Encrypt(janAadharNo));
                objDBAccess.AddParameterToCommand("@JanMemberId", DBFactoryParameterType.VARCHAR, memberId);
                int result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                return result;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GopalCreditCardModel",
                    FunctionName = "ValidateGurantor",
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return 0;
        }

        #endregion

        #region Update moka report data
        public int UpdateMokaReportData(GopalCreditCardFormModel _Modal)
        {
            string sqlquery = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd_CropLoanDetails = new SqlCommand();
                objDBAccess.CreateCommand("Insert_GCC_otherDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateMokaReportData");

                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _Modal.gcc_id);
                objDBAccess.AddParameterToCommand("@IsApplicantRuralResidence", DBFactoryParameterType.Int, _Modal.IsApplicantRuralResidence);
                objDBAccess.AddParameterToCommand("@NoOfCowsOnMoka", DBFactoryParameterType.Int, _Modal.NoOfCowsOnMoka);
                objDBAccess.AddParameterToCommand("@NoOfbuffaloesOnMoka", DBFactoryParameterType.Int, _Modal.NoOfbuffaloesOnMoka);
                objDBAccess.AddParameterToCommand("@LandValueOnMoka", DBFactoryParameterType.Double, _Modal.LandValueOnMoka);
                objDBAccess.AddParameterToCommand("@RemarkMoka", DBFactoryParameterType.NVarchar, _Modal.RemarkMoka);

                sqlquery = objDBAccess.getSqlCommandText();
                var result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                return result;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "UpdateMokaReportData(GopalCreditCardFormModel _Modal) " + sqlquery }, ex);
                objDBAccess.CloseConnection();
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }

        #endregion

        #region Update esign info of GCC
        public int UpdateEsingInfoOfGCC(GopalCreditCardFormModel _Modal)
        {
            string sqlquery = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd_CropLoanDetails = new SqlCommand();
                objDBAccess.CreateCommand("Insert_GCC_otherDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateEsingInfo");

                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, _Modal.gcc_id);
                objDBAccess.AddParameterToCommand("@IsApplicantSigned", DBFactoryParameterType.Int, _Modal.IsApplicantSigned);
                objDBAccess.AddParameterToCommand("@IsGurantor1Signed", DBFactoryParameterType.Int, _Modal.IsGurantor1Signed);
                objDBAccess.AddParameterToCommand("@IsGurantor2Signed", DBFactoryParameterType.Int, _Modal.IsGurantor2Signed);
                objDBAccess.AddParameterToCommand("@IsPacsManagerSigned", DBFactoryParameterType.Double, _Modal.IsPacsManagerSigned);

                sqlquery = objDBAccess.getSqlCommandText();
                var result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                return result;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "UpdateEsingInfoOfGCC(GopalCreditCardFormModel _Modal) " + sqlquery }, ex);
                objDBAccess.CloseConnection();
            }
            finally
            {
                objDBAccess.CloseConnection();

            }
            return 0;
        }

        #endregion



        public int UpdateGCCApplicantAadhar(int gccid, string AadhaarNo)
        {
            string sqlquery = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd_CropLoanDetails = new SqlCommand();
                objDBAccess.CreateCommand("UpdateGCCApplicantAadhar", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, gccid);
                objDBAccess.AddParameterToCommand("@AadhaarNo", DBFactoryParameterType.NVarchar, AadhaarNo);
                objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(AadhaarNo) ? COMMON_FUNCTION.Encrypt(AadhaarNo) : "");

                sqlquery = objDBAccess.getSqlCommandText();
                var result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                return result;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "UpdateGCCApplicantAadhar(int gccid,string AadhaarNo) " + sqlquery }, ex);
                objDBAccess.CloseConnection();
            }
            finally
            {
                objDBAccess.CloseConnection();

            }
            return 0;
        }


        public int InsertGCCApplicationSigned(int GccId = 0, string GopalCreditCardForm = "", string GccApplicationFlow = "")
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGCCApplicationSigned", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GccId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser?.UserId);
                objDBAccess.AddParameterToCommand("@DocType", DBFactoryParameterType.VARCHAR, "GCCApplicationSigned");
                objDBAccess.AddParameterToCommand("@GopalCreditCardForm", DBFactoryParameterType.NVarchar, GopalCreditCardForm);
                objDBAccess.AddParameterToCommand("@GccApplicationFlow", DBFactoryParameterType.NVarchar, GccApplicationFlow);
                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = " InsertGCCApplicationSigned(GccDocumentsModel _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetApplicationFlowByGCCId(int GCCId = 0, int UserId = 0)
        {
            dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetApplicationFlowByGCC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GCCId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                dt = objDBAccess.GetTable();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetApplicationFlowByGCCId(int GCCId = 0, int UserId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetEsignStatusByGCC(int GCCId = 0)
        {
            dt = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetEsignStatusByGCC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GccId", DBFactoryParameterType.Int, GCCId);
                dt = objDBAccess.GetTable();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "GetEsignStatusByGCC(int GCCId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
    }
}

