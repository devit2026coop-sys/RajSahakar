using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using CoopDAL;
using CoopDB;
using Cooperative.Filters;
using System.Data.SqlTypes;

namespace Cooperative.Models
{
    public class SocietyModel
    {
        DBLogger objDBLogger = new DBLogger();
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public SocietyModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public SocietyModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        //create by Madhuri
        public int LiquidationRegistration(LiquidationFormDataEntity _Model)
        {
            int Result = 0;
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiquidationRegistrations", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@LiqudationDataId", DBFactoryParameterType.Int, _Model.LiqudationDataId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _Model.DistrictId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, _Model.Month);
                objDBAccess.AddParameterToCommand("@DetailsOfAcceptatnce", DBFactoryParameterType.NVarchar, _Model.DetailsOfAcceptatnce);
                objDBAccess.AddParameterToCommand("@AcceptedAmount", DBFactoryParameterType.Int, _Model.AcceptedAmount);
                objDBAccess.AddParameterToCommand("@PrincipalAmountOfRecovery", DBFactoryParameterType.Int, _Model.PrincipalAmountOfRecovery);
                objDBAccess.AddParameterToCommand("@InterestAmountOfRecovery", DBFactoryParameterType.Int, _Model.InterestAmountOfRecovery);
                objDBAccess.AddParameterToCommand("@RecoveryBalance", DBFactoryParameterType.Int, _Model.RecoveryBalance);
                objDBAccess.AddParameterToCommand("@FdAmount", DBFactoryParameterType.Int, _Model.FdAmount);
                objDBAccess.AddParameterToCommand("@FundSavingAccount", DBFactoryParameterType.Int, _Model.FundSavingAccount);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _Model.IsActive);



                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegistration(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable LiquidationMprDataList(int DistrictId, string Month)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiquidationMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.NVarchar, Month);
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

        public int SocietyRegistration(SocietyRegistrationEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, _Model.BusinessId);
                objDBAccess.AddParameterToCommand("@LiabilityId", DBFactoryParameterType.Int, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@MemberLiability", DBFactoryParameterType.Int, _Model.MemberLiablity);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@TotalMembers", DBFactoryParameterType.Int, _Model.TotalMembersApplyingForSocociety);
                objDBAccess.AddParameterToCommand("@EnteranceFee", DBFactoryParameterType.Int, _Model.EntranceFeesCollected);
                objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.Int, _Model.SingleShareValue);
                objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Int, _Model.PaidUpShareCapital);
                objDBAccess.AddParameterToCommand("@ByLawsFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile);
                objDBAccess.AddParameterToCommand("@WorkPlanShowingEconomicViabilityFile", DBFactoryParameterType.NVarchar, _Model.WorkPlanShowingEconomicViabilityFile);
                //objDBAccess.AddParameterToCommand("@GeneralMeetingDate", DBFactoryParameterType.DateTime, _Model.GeneralMeetingDate);
                objDBAccess.AddParameterToCommand("@GeneralMeetingDate", DBFactoryParameterType.DateTime, System.DateTime.Now);
                objDBAccess.AddParameterToCommand("@MembersPArticipatingGeneralMeeting", DBFactoryParameterType.NVarchar, _Model.TotalNumberOfMembersInAGM);
                objDBAccess.AddParameterToCommand("@GeneralMeetingProceedingFile", DBFactoryParameterType.NVarchar, _Model.GeneralMeetingProceedingFile);
                objDBAccess.AddParameterToCommand("@BalanceSheetFile", DBFactoryParameterType.NVarchar, _Model.BalanceSheetFile);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile);
                objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile);
                objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, _Model.RegistrationNo);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegistration(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AadharUpdate(AdministrativeData _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("encAadhar", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, _Model.AADHAARNo);
                objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, _Model.AdministrativeId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "AadharUpdate(AdministrativeData _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string InsertSocietyProfile(SocietyRegistrationEntity _Model)
        {
            string Result = "-1";
            int InnerResult = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;

                cmd = objDBAccess.CreateCommand("Insert_Societies_Edit_Request", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName.ToUpper().Trim());
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, _Model.SaveStatus);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _Model.IsActive);

                string tempEncAadhar = "NOT";

                //_Model.AADHARNO = "307691961366";

                if (_Model.AADHARNO != null && _Model.AADHARNO.Length == 12)
                    tempEncAadhar = COMMON_FUNCTION.Encrypt(_Model.AADHARNO);



                if (_Model.KIOSKCODE.Length > 0 && System.Web.HttpContext.Current.Session[SessionIdentity.Identity] != null)
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);
                else
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);




                #region RegistrationCertificateFile
                if (_Model.RegistrationCertificateFile != null)
                {
                    if (_Model.RegistrationCertificateFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RegistrationCertificateFile.Contains("undefined"))
                    {
                        var Arr = _Model.RegistrationCertificateFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RegistrationCertificateFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile.Contains("undefined") ? "" : _Model.RegistrationCertificateFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ByeLawsSocietyFile
                if (_Model.ByeLawsSocietyFile != null)
                {
                    if (_Model.ByeLawsSocietyFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.ByeLawsSocietyFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ByeLawsSocietyFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ByeLawsSocietyFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile.Contains("undefined") ? "" : _Model.ByeLawsSocietyFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region RulesAndRegulationFile
                if (_Model.RulesAndRegulationFile != null)
                {
                    if (_Model.RulesAndRegulationFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RulesAndRegulationFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.RulesAndRegulationFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RulesAndRegulationFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile.Contains("undefined") ? "" : _Model.RulesAndRegulationFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region BalanceSheetPDF
                if (_Model.BalanceSheetFile != null)
                {
                    if (_Model.BalanceSheetFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.BalanceSheetFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.BalanceSheetFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.BalanceSheetFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, _Model.BalanceSheetFile.Contains("undefined") ? "" : _Model.BalanceSheetFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ProceedingsGeneralMeetingPDF
                if (_Model.GeneralMeetingProceedingFile != null)
                {
                    if (_Model.GeneralMeetingProceedingFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.GeneralMeetingProceedingFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.GeneralMeetingProceedingFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.GeneralMeetingProceedingFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, _Model.GeneralMeetingProceedingFile.Contains("undefined") ? "" : _Model.GeneralMeetingProceedingFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region WorkPlanShowingEconomicPDF
                if (_Model.WorkPlanShowingEconomicViabilityFile != null)
                {
                    if (_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.WorkPlanShowingEconomicViabilityFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.WorkPlanShowingEconomicViabilityFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, _Model.WorkPlanShowingEconomicViabilityFile.Contains("undefined") ? "" : _Model.WorkPlanShowingEconomicViabilityFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region MemberListExcelFile
                if (_Model.MemberListExcelFile != null)
                {
                    if (_Model.MemberListExcelFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.MemberListExcelFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.MemberListExcelFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.MemberListExcelFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, _Model.MemberListExcelFile.Contains("undefined") ? "" : _Model.MemberListExcelFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region ObjectsFileName
                if (_Model.ObjectsFileName != null)
                {
                    if (_Model.ObjectsFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.ObjectsFileName.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ObjectsFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ObjectsFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, _Model.ObjectsFileName.Contains("undefined") ? "" : _Model.ObjectsFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, _Model.BusinessId);
                objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.Int, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@TotalNumberOfMembers", DBFactoryParameterType.Int, _Model.TotalMembersApplyingForSocociety);
                objDBAccess.AddParameterToCommand("@NoOfMemberInAGM", DBFactoryParameterType.Int, _Model.TotalNumberOfMembersInAGM);
                objDBAccess.AddParameterToCommand("@EntranceFeesCollected", DBFactoryParameterType.Double, _Model.EntranceFeesCollected);
                objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Model.PaidUpShareCapital);
                objDBAccess.AddParameterToCommand("@MemberLiablity", DBFactoryParameterType.Int, _Model.MemberLiablity);
                objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.Double, _Model.SingleShareValue);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Bit, _Model.IsNewRegistration);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN == null ? "" : _Model.BRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@NoOfBranches", DBFactoryParameterType.Int, _Model.NoOfBranches);
                objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.ElectionDate).ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.ElectionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                //FOR SPORT 
                objDBAccess.AddParameterToCommand("@IsRSSC", DBFactoryParameterType.NVarchar, _Model.IsStatLevel);
                objDBAccess.AddParameterToCommand("@AgmDate", DBFactoryParameterType.Date, _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());//_Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@SportsAssociationId", DBFactoryParameterType.Int, _Model.AssociationId);

                objDBAccess.AddParameterToCommand("@IsAffiliatedByRSSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByRSSC);
                objDBAccess.AddParameterToCommand("@RSSCRegNo", DBFactoryParameterType.NVarchar, _Model.RSSCRegNo == "undefined" ? "" : _Model.RSSCRegNo);
                objDBAccess.AddParameterToCommand("@RSSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByRSSC == false ? DateTime.MaxValue : _Model.RSSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region RSSCFileName
                if (_Model.RSSCFileName != null)
                {
                    if (_Model.RSSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RSSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.RSSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RSSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, _Model.RSSCFileName == "undefined" ? "" : _Model.RSSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliatedBySOC", DBFactoryParameterType.Bit, _Model.IsAffiliatedBySOC);
                objDBAccess.AddParameterToCommand("@SOCRegNo", DBFactoryParameterType.NVarchar, _Model.SOCRegNo == "undefined" ? "" : _Model.SOCRegNo);
                objDBAccess.AddParameterToCommand("@SOCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedBySOC == false ? DateTime.MaxValue : _Model.SOCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                #region SOCFileName
                if (_Model.SOCFileName != null)
                {
                    if (_Model.SOCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SOCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SOCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SOCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, _Model.SOCFileName == "undefined" ? "" : _Model.SOCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion


                objDBAccess.AddParameterToCommand("@IsAffiliatedByDLSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByDLSC);
                objDBAccess.AddParameterToCommand("@DLSCRegNo", DBFactoryParameterType.NVarchar, _Model.DLSCRegNo);
                objDBAccess.AddParameterToCommand("@DLSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByDLSC == false ? DateTime.MaxValue : _Model.DLSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region DLSCFileName
                if (_Model.DLSCFileName != null)
                {
                    if (_Model.DLSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.DLSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.DLSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.DLSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, _Model.DLSCFileName == "undefined" ? "" : _Model.DLSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliationBySLSA", DBFactoryParameterType.Bit, _Model.IsAffiliationBySLSA);
                objDBAccess.AddParameterToCommand("@SLSARegNo", DBFactoryParameterType.NVarchar, _Model.SLSARegNo);
                objDBAccess.AddParameterToCommand("@SLSAAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliationBySLSA == false ? DateTime.MaxValue : _Model.SLSAAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region SLSAFileName
                if (_Model.SLSAFileName != null)
                {
                    if (_Model.SLSAFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SLSAFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SLSAFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SLSAFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, _Model.SLSAFileName == "undefined" ? "" : _Model.SLSAFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                //if (_Model.KIOSKCODE != null)
                //{

                if (_Model.KIOSKCODE.Length > 0)
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, _Model.ServicesId);
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 5530);
                }
                // }
                //else
                //{
                //    if (_Model.SocietyIDEnc.Length > 0 && _Model.IsNewRegistration == false)
                //        objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                //    else
                //    {
                //        objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, _Model.IsNewRegistration == true ? 4172 : 4209);//New Society Registration ? Add Society Profile
                //    }

                //}
                //END
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                int R = int.Parse(Result.Split('@')[1]);

                if (Result.ToString().Length > 10 && Result != "-5" && R > 0)
                {
                    int FlagFirstRow = 1;
                    foreach (SocietyWorkScopeEntity obj in _Model.lstSocietyWorkScope)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertWorkScope_Edit_Request", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, Result.ToString().Contains("@") ? int.Parse(Result.Split('@')[1]) : 0);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Result.Split('@')[0]);
                        objDBAccess.AddParameterToCommand("@IsStatLevel", DBFactoryParameterType.Int, obj.IsStatLevel);
                        objDBAccess.AddParameterToCommand("@WorkScopeLevel_ID", DBFactoryParameterType.Int, obj.WorkScopeLevel_ID);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Int, obj.IsRuralUrban);
                        objDBAccess.AddParameterToCommand("@DirectorName", DBFactoryParameterType.NVarchar, obj.DirectorName);
                        objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                        objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.Int, obj.AssociationId);
                        objDBAccess.AddParameterToCommand("@FlagFirstRow", DBFactoryParameterType.Int, FlagFirstRow);
                        objDBAccess.AddParameterToCommand("@SportsAssociationName", DBFactoryParameterType.NVarchar, obj.SportsAssociationName);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                        objDBAccess.AddParameterToCommand("@AreaOfOperation", DBFactoryParameterType.NVarchar, _Model.AreaOfOperation);

                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                    }

                }
                else
                {
                    Result = "-1";
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model," + _Model.SocietyName + "," + _Model.RegistrationNo + "," + _Model.SocietyID + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

        public string UploadMemberExcelData(string filepath, string FileName, string SocietyIdEnc, int ActId)
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(objDBAccess.GetConnection))
            {
                try
                {

                    DataTable data = COMMON_FUNCTION.ReadExcelData(filepath, "MemberDetails");


                    if (ActId == 7)
                    {
                        // { "Name", "Father_Husband_Name", "Contact_No", "AadharNo", "DOB", "Gender", "Category", "POSTNAME", "Occupation", "MembershipNo", "NOOfShareAllotted", "ShareCapitalAmount", "Address", "loaneeType", "MEMBERTYPE" };
                        System.Data.DataColumn Occupation = new System.Data.DataColumn("Occupation", typeof(string));
                        Occupation.DefaultValue = "";
                        data.Columns.Add(Occupation);
                        System.Data.DataColumn MembershipNo = new System.Data.DataColumn("MembershipNo", typeof(string));
                        MembershipNo.DefaultValue = "";
                        data.Columns.Add(MembershipNo);
                        System.Data.DataColumn NOOfShareAllotted = new System.Data.DataColumn("NOOfShareAllotted", typeof(int));
                        NOOfShareAllotted.DefaultValue = 0;
                        data.Columns.Add(NOOfShareAllotted);
                        System.Data.DataColumn ShareCapitalAmount = new System.Data.DataColumn("ShareCapitalAmount", typeof(int));
                        ShareCapitalAmount.DefaultValue = 0;
                        data.Columns.Add(ShareCapitalAmount);
                        System.Data.DataColumn loaneeType = new System.Data.DataColumn("loaneeType", typeof(string));
                        loaneeType.DefaultValue = "";
                        data.Columns.Add(loaneeType);
                    }

                    System.Data.DataColumn CreateDate = new System.Data.DataColumn("CreateDate", typeof(DateTime));
                    CreateDate.DefaultValue = DateTime.Now;// ToString("yyyy/MM/dd HH:mm:tt");
                    data.Columns.Add(CreateDate);
                    System.Data.DataColumn IsActive = new System.Data.DataColumn("IsActive", typeof(bool));
                    IsActive.DefaultValue = true;
                    data.Columns.Add(IsActive);
                    System.Data.DataColumn CreateIP = new System.Data.DataColumn("CreateIP", typeof(string));
                    CreateIP.DefaultValue = COMMON_FUNCTION.GetIpAddress(); ;
                    data.Columns.Add(CreateIP);
                    System.Data.DataColumn SocietyId = new System.Data.DataColumn("SocietyId", typeof(int));
                    SocietyId.DefaultValue = 0;
                    data.Columns.Add(SocietyId);
                    System.Data.DataColumn ExcelName = new System.Data.DataColumn("ExcelName", typeof(string));
                    ExcelName.DefaultValue = FileName;
                    data.Columns.Add(ExcelName);


                    DataTable orderedTable = new DataTable();
                    orderedTable.Clear();
                    orderedTable.Columns.Add("Name");
                    orderedTable.Columns.Add("Father_Husband_Name");
                    orderedTable.Columns.Add("Contact_No");
                    orderedTable.Columns.Add("AadharNo");
                    orderedTable.Columns.Add("DOB");
                    orderedTable.Columns.Add("Gender");
                    orderedTable.Columns.Add("Category");
                    orderedTable.Columns.Add("POSTNAME");
                    orderedTable.Columns.Add("Occupation");
                    orderedTable.Columns.Add("MembershipNo");
                    orderedTable.Columns.Add("NOOfShareAllotted");
                    orderedTable.Columns.Add("ShareCapitalAmount");
                    orderedTable.Columns.Add("Address");
                    orderedTable.Columns.Add("loaneeType");
                    orderedTable.Columns.Add("MEMBERTYPE");
                    orderedTable.Columns.Add("AadharNoEnc");
                    orderedTable.Columns.Add("CreateDate");
                    orderedTable.Columns.Add("IsActive");
                    orderedTable.Columns.Add("CreateIP");
                    orderedTable.Columns.Add("SocietyId");
                    orderedTable.Columns.Add("ExcelName");



                    System.Data.DataColumn AadharNoEnc = new System.Data.DataColumn("AadharNoEnc", typeof(string));
                    AadharNoEnc.DefaultValue = AadharNoEnc;
                    data.Columns.Add(AadharNoEnc);


                    DataRow _row = orderedTable.NewRow();

                    foreach (DataRow dr in data.Rows)
                    {
                        _row = orderedTable.NewRow();

                        _row["Name"] = dr["Name"].ToString().Trim(); ;
                        _row["Father_Husband_Name"] = dr["Father_Husband_Name"].ToString().Trim(); ;
                        _row["Contact_No"] = dr["Contact_No"].ToString().Trim(); ;
                        _row["AadharNo"] = dr["AadharNo"].ToString().Trim(); ;
                        _row["DOB"] = dr["DOB"].ToString().Trim(); ;
                        _row["Gender"] = dr["Gender"].ToString().Trim(); ;
                        if (dr["Category"] != null)
                        {
                            _row["Category"] = dr["Category"].ToString().Trim().ToUpper() == "ST" ? 1 :
                            dr["Category"].ToString().Trim().ToUpper() == "SC" ? 2 :
                            dr["Category"].ToString().Trim().ToUpper() == "GEN" ? 3 :
                           dr["Category"].ToString().Trim().ToUpper() == "OBC" ? 4 : 0;
                        }
                        else
                        {
                            _row["Category"] = dr["Category"];
                        }
                        _row["POSTNAME"] = dr["POSTNAME"].ToString().Trim(); ;
                        _row["Occupation"] = dr["Occupation"].ToString().Trim(); ;
                        _row["MembershipNo"] = dr["MembershipNo"].ToString().Trim(); ;
                        _row["NOOfShareAllotted"] = dr["NOOfShareAllotted"].ToString().Trim(); ;
                        _row["ShareCapitalAmount"] = dr["ShareCapitalAmount"].ToString().Trim(); ;
                        _row["Address"] = dr["Address"].ToString().Trim(); ;
                        _row["loaneeType"] = dr["loaneeType"].ToString().Trim(); ;
                        _row["MEMBERTYPE"] = dr["MEMBERTYPE"].ToString().Trim(); ;
                        if (dr["AadharNo"] != null)
                        {
                            string val = dr["AadharNo"].ToString().Replace(" ", "").Length <= 12 ? dr["AadharNo"].ToString().Replace(" ", "") : dr["AadharNo"].ToString().Replace(" ", "").Substring(0, 12);
                            _row["AadharNoEnc"] = COMMON_FUNCTION.Encrypt(val);
                        }
                        else
                        {
                            _row["AadharNoEnc"] = dr["AadharNoEnc"];
                        }


                        _row["CreateDate"] = dr["CreateDate"].ToString().Trim(); ;
                        _row["IsActive"] = dr["IsActive"].ToString().Trim(); ;
                        _row["CreateIP"] = dr["CreateIP"].ToString().Trim(); ;
                        _row["SocietyId"] = dr["SocietyId"].ToString().Trim(); ;
                        _row["ExcelName"] = dr["ExcelName"].ToString().Trim(); ;

                        orderedTable.Rows.Add(_row);
                    }

                    //for (int i = 0; i < data.Rows.Count; i++)
                    //{



                    //    foreach (DataColumn column in data.Columns)
                    //    {
                    //        if (column.ColumnName == "Category" && data.Rows[i][column.ColumnName] != null)
                    //        {
                    //            data.Rows[i][column.ColumnName] = data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "ST" ? 1 :
                    //            data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "SC" ? 2 :
                    //            data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "GEN" ? 3 :
                    //            data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "OBC" ? 4 : 0;
                    //        }
                    //        else if (column.ColumnName == "AadharNo" && data.Rows[i][column.ColumnName] != null)
                    //        {
                    //            string val = data.Rows[i][column.ColumnName].ToString().Replace(" ", "").Length <= 12 ? data.Rows[i][column.ColumnName].ToString().Replace(" ", "") : data.Rows[i][column.ColumnName].ToString().Replace(" ", "").Substring(0, 12);
                    //            data.Rows[i]["AadharNoEnc"] = COMMON_FUNCTION.Encrypt(val);
                    //        }
                    //        else {
                    //            data.Rows[i][column.ColumnName] = data.Rows[i][column.ColumnName].ToString().Trim();
                    //        }
                    //    }
                    //}




                    DataTable dddd = new DataTable();
                    int Result = 0;
                    try
                    {

                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyMemberDetailsXL", DBFactoryCommandType.PROCEDURE_COMMAND);
                        //objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.AADHAARNo);
                        //objDBAccess.pa("@tbl",  data);
                        cmd.Parameters.AddWithValue("@SocietyIdEnc", SocietyIdEnc);
                        cmd.Parameters.AddWithValue("@tbl", orderedTable);
                        cmd.ExecuteNonQuery();
                        //SqlDataAdapter _da = new SqlDataAdapter();
                        //_da = new SqlDataAdapter(cmd);
                        //_da.Fill(dddd);


                        //Result = Convert.ToInt32(cmd.ex());
                    }
                    catch (Exception ex)
                    {
                        // DBLogger objDBLogger = new DBLogger();
                        // objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UploadMemberExcelData(DataTable data)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }


                    //sqlBulkCopy.DestinationTableName = "dbo.SocietyMemberDetailsXL";
                    //objDBAccess.OpenConnection();
                    //if (data.Rows.Count > 0)
                    //{                      

                    //    sqlBulkCopy.ColumnMappings.Add("SocietyId", "SocietyId");
                    //    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    //    sqlBulkCopy.ColumnMappings.Add("Father_Husband_Name", "Father_Husband_Name");
                    //    sqlBulkCopy.ColumnMappings.Add("Contact_No", "Contact_No");
                    //    sqlBulkCopy.ColumnMappings.Add("AadharNoEnc", "AadharNo");
                    //    sqlBulkCopy.ColumnMappings.Add("DOB", "DOB");
                    //    sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                    //    sqlBulkCopy.ColumnMappings.Add("Category", "Category");
                    //    sqlBulkCopy.ColumnMappings.Add("POSTNAME", "POSTNAME");
                    //    sqlBulkCopy.ColumnMappings.Add("Occupation", "Occupation");
                    //    sqlBulkCopy.ColumnMappings.Add("MembershipNo", "MembershipNo");
                    //    sqlBulkCopy.ColumnMappings.Add("NOOfShareAllotted", "NOOfShareAllotted");
                    //    sqlBulkCopy.ColumnMappings.Add("ShareCapitalAmount", "ShareCapitalAmount");
                    //    sqlBulkCopy.ColumnMappings.Add("Address", "Address");
                    //    sqlBulkCopy.ColumnMappings.Add("ExcelName", "ExcelName");
                    //    sqlBulkCopy.ColumnMappings.Add("loaneeType", "loaneeType");
                    //    sqlBulkCopy.ColumnMappings.Add("MEMBERTYPE", "MEMBERTYPE");                     
                    //    sqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate");                       
                    //    sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                    //    sqlBulkCopy.ColumnMappings.Add("CreateIP", "CreateIP");


                    //    sqlBulkCopy.WriteToServer(data);
                    //    //Button1.Visible = false;
                    //    objDBAccess.CloseConnection();

                    //}
                    //objDBAccess.Close();
                }
                catch (Exception ex)
                {
                    // DBLogger objDBLogger = new DBLogger();
                    //objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " UploadMemberExcelData(DataTable data)" }, ex);
                }
            }

            return "";
        }

        public bool IsValidDateTime(string dateTime)
        {
            //var DD1 = dateTime.Split('.');
            //if(DD1.Length==1)
            //    DD1=dateTime.Split('/');
            //if (DD1.Length == 1)
            //    DD1 = dateTime.Split('-');


            try
            {
                dateTime = dateTime.Replace(".", "/").Replace("_", "/").Replace("-", "/").Split(' ')[0];
                string newdate = dateTime.Split('/')[2] + "/" + dateTime.Split('/')[1] + "/" + dateTime.Split('/')[0];
                string[] formats = { "yyyy/MM/dd" };
                DateTime parsedDateTime;

                if (Convert.ToInt32(dateTime.Split('/')[2]) < 1900 || Convert.ToInt32(dateTime.Split('/')[2]) > DateTime.Now.Year)
                {
                    return false;
                }
                parsedDateTime = Convert.ToDateTime(newdate);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }


            //string Day = "";
            //string MM = "";
            //string YYYY = "";
            //if (DD1.Length > 0)
            //{
            //    if (DD1[0].ToString().Length == 1)
            //    {
            //        Day = "0" + DD1[0].ToString();
            //    }
            //    else
            //    {
            //        Day = DD1[0].ToString().Trim();
            //    }

            //    if (DD1[1].ToString().Length == 1)
            //    {
            //        MM = "0" + DD1[1].ToString();
            //    }
            //    else
            //    {
            //        MM = DD1[1].ToString().Trim();
            //    }

            //    YYYY = DD1[DD1.Length - 1].Split(' ')[0];

            //    dateTime = Day + "/" + MM + "/" + YYYY;
            //}

            //string[] formats = { "dd/MM/yyyy" };

            //return DateTime.TryParseExact(dateTime, formats, new CultureInfo("hi-IN"), DateTimeStyles.None, out parsedDateTime);
        }

        public string InsertSocityByExcel(SocietyRegistrationEntity obj)
        {
            string Result = "-1";
            string connString = "";
            try
            {


                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyExcel/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [Sheet1$]";//[" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();

                        try
                        {

                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(" 'Sheet1' does not exists in upload Excel. OR Error-" + ex.Message.Replace("'", ""));
                        }




                        dt.Columns.Add("CEOAadharEnc", typeof(System.String));
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));
                        try
                        {
                            int i = 0;
                            DateTime Dt;
                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string SocietyName = Dr["SocietyName"].ToString().Trim();
                                string RegistrationNo = Dr["RegistrationNo"].ToString().Trim().ToUpper();
                                string Act = Dr["Act"].ToString().Trim();
                                string RegistrationDate = Dr["RegistrationDate(dd/mm/yyyy)"].ToString().Trim().Replace("-", "/").Replace("-", "/").Replace(".", "/").Replace(".", "/");

                                //var formats = new[] { "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy" };
                                //;
                                //if (DateTime.TryParseExact(RegistrationDate, formats, null, DateTimeStyles.None, out Dt))
                                //{
                                //}

                                string SocietyType = "";
                                string Sports = "";
                                string State_Or_District_level_Sports_Association = "";
                                if (obj.ActId != 7)
                                {
                                    SocietyType = Dr["SocietyType"].ToString().Trim();
                                }
                                else
                                {
                                    State_Or_District_level_Sports_Association = Dr["State_Or_District_level_Sports_Association"].ToString().Trim();
                                    Sports = Dr["Sports"].ToString().Trim();
                                }
                                string CEOName = Dr["CEOName"].ToString().Trim();
                                string CEOContactNo = Dr["CEOContactNo"].ToString().Trim();
                                string CEOAadhar = Dr["CEOAadhar"].ToString().Trim();
                                string OfficeDistrict = Dr["SocietyOfficeDistrict"].ToString().Trim();
                                string OfficePanchayatSamiti = Dr["SocietyOfficePanchayatSamiti"].ToString().Trim();
                                string OfficeCity = Dr["SocietyOfficeCity"].ToString().Trim();

                                if (OfficeCity != "" || OfficePanchayatSamiti != "")
                                {
                                    if (SocietyName == "")
                                    {
                                        return "Please enter Society Name on line No " + i.ToString();
                                    }
                                    else if (RegistrationDate == "")
                                    {
                                        return "Please enter Registration Date on line No " + i.ToString();
                                    }
                                    else if (RegistrationDate.Length < 10)
                                    {
                                        return "Please enter valid Registration Date(dd/mm/yyyy) on line No " + i.ToString();
                                    }
                                    else if (!IsValidDateTime(RegistrationDate))
                                    {
                                        return "Please enter Proper Registration Date '" + RegistrationDate + "'(dd/mm/yyyy) on line No " + (i + 1).ToString();
                                    }
                                    else if (RegistrationDate.Length < 10)
                                    {
                                        return "Please enter valid Registration Date(dd/mm/yyyy) on line No " + i.ToString();
                                    }
                                    else if (RegistrationNo == "")
                                    {
                                        return "Please enter Registration Number on line No " + i.ToString();
                                    }
                                    else if (RegistrationNo.Contains("COOP"))
                                    {
                                        return "you can't Upload Online Registration Data on line No " + i.ToString();
                                    }
                                    else if (Act == "")
                                    {
                                        return "Please enter Act on line No " + i.ToString();
                                    }
                                    else if (SocietyType == "" && obj.ActId != 7)
                                    {
                                        return "Please enter Society Type on line No " + i.ToString();
                                    }
                                    else if (State_Or_District_level_Sports_Association == "" && obj.ActId == 7)
                                    {
                                        return "Please enter State_Or_District_level_Sports_Association on line No " + i.ToString();
                                    }
                                    else if (Sports == "" && obj.ActId == 7)
                                    {
                                        return "Please enter Sports on line No " + i.ToString();
                                    }
                                    // chnage by n saini  due to insert society without any CEO info
                                    //else if (CEOName == "")
                                    //{
                                    //    return "Please enter CEO Name on line No " + i.ToString();
                                    //}
                                    //else if (CEOContactNo == "")
                                    //{
                                    //    return "Please enter CEO Contact Number on line No " + i.ToString();
                                    //}
                                    //else if (CEOAadhar == "")
                                    //{
                                    //    return "Please enter CEO Aadhar Number on line No " + i.ToString();
                                    //}
                                    else if (OfficeDistrict == "")
                                    {
                                        return "Please enter Office District on line No " + i.ToString();
                                    }
                                    else if (OfficePanchayatSamiti == "" && OfficeCity == "")
                                    {
                                        return "Please enter Office PanchayatSamiti or City on line No " + i.ToString();
                                    }
                                    else if (OfficePanchayatSamiti != "" && OfficeCity != "")
                                    {
                                        return "Please enter only one in PanchayatSamiti or City on line No " + i.ToString();
                                    }
                                    // chnage by n saini  due to insert society without any CEO info
                                    //if (CEOContactNo.Length < 10 || CEOContactNo.Length > 15)
                                    //{
                                    //    return "Please enter proper CEO Contect Number on line No " + i.ToString();
                                    //}
                                    //else
                                    //{
                                    //    double myNum1 = 0;
                                    //    if (Double.TryParse(CEOContactNo, out myNum1))
                                    //    {
                                    //        // it is a number
                                    //    }
                                    //    else
                                    //    {
                                    //        return "Please enter proper CEO Contect Number on line No " + i.ToString();
                                    //        // it is not a number
                                    //    }
                                    //}
                                    //if (Dr["CEOAadhar"].ToString().Length != 12)
                                    //{
                                    //    return "Please enter 12 digit CEO Aadhar Number on line No " + i.ToString();
                                    //}
                                    //else
                                    //{
                                    //    double myNum = 0;
                                    //    if (Double.TryParse(Dr["CEOAadhar"].ToString(), out myNum))
                                    //    {
                                    //        // it is a number
                                    //    }
                                    //    else
                                    //    {
                                    //        return "Please enter only digit in CEO Aadhar Number on line No " + i.ToString();
                                    //        // it is not a number
                                    //    }
                                    //}
                                }
                                else
                                {
                                    return "Please enter value in PanchayatSamiti or City on line No " + i.ToString();
                                }
                            }
                            string ResultInner = ""; int Suc = 0, Unsuc = 0, Dupli = 0;

                            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                            objDBAccess.OpenConnection();
                            SqlCommand cmd = objDBAccess.CreateCommand("InsertSocityExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                            objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                            Result = (objDBAccess.ExecuteScalarCommand().ToString());
                            i = 0;
                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string SocietyName = Dr["SocietyName"].ToString().Trim();
                                string RegistrationNo = Dr["RegistrationNo"].ToString().Trim();
                                string Act = Dr["Act"].ToString().Trim();
                                string RegistrationDate = Dr["RegistrationDate(dd/mm/yyyy)"].ToString().Trim().Replace("-", "/").Replace("-", "/").Replace(".", "/").Replace(".", "/");
                                //string SocietyType = Dr["SocietyType"].ToString().Trim();
                                string SocietyType = "";
                                string Sports = "";
                                string State_Or_District_level_Sports_Association = "";
                                if (obj.ActId != 7)
                                {
                                    SocietyType = Dr["SocietyType"].ToString().Trim();
                                }
                                else
                                {
                                    State_Or_District_level_Sports_Association = Dr["State_Or_District_level_Sports_Association"].ToString().Trim();
                                    Sports = Dr["Sports"].ToString().Trim();
                                }
                                string CEOName = Dr["CEOName"].ToString().Trim();
                                string CEOContactNo = Dr["CEOContactNo"].ToString().Trim();
                                string CEOAadhar = Dr["CEOAadhar"].ToString().Trim();
                                string OfficeDistrict = Dr["SocietyOfficeDistrict"].ToString().Trim();
                                string OfficePanchayatSamiti = Dr["SocietyOfficePanchayatSamiti"].ToString().Trim();
                                string OfficeCity = Dr["SocietyOfficeCity"].ToString().Trim();

                                string CEOAadharEnc = "";
                                if (Dr["CEOAadhar"] != null && Dr["CEOAadhar"].ToString().Length > 0)
                                {
                                    Dr["CEOAadharEnc"] = COMMON_FUNCTION.Encrypt(Dr["CEOAadhar"].ToString());
                                    CEOAadharEnc = Dr["CEOAadharEnc"].ToString();
                                }
                                Dr["ExcelUploadId"] = Result;

                                if (OfficeCity != "" || OfficePanchayatSamiti != "")
                                {
                                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertSocityExcelDetailsByExcelSheet_Updated", DBFactoryCommandType.PROCEDURE_COMMAND);
                                    cmd1.Parameters.AddWithValue("@SocietyName", SocietyName);
                                    cmd1.Parameters.AddWithValue("@RegistrationNo", RegistrationNo.Trim());
                                    cmd1.Parameters.AddWithValue("@RegistrationDate", RegistrationDate.ConvertString_ToValidDate());
                                    cmd1.Parameters.AddWithValue("@Act", Act);
                                    cmd1.Parameters.AddWithValue("@SocietyType", SocietyType);
                                    cmd1.Parameters.AddWithValue("@StateDistrictlevel", State_Or_District_level_Sports_Association);
                                    cmd1.Parameters.AddWithValue("@Sports", Sports);
                                    cmd1.Parameters.AddWithValue("@CEOName", CEOName);
                                    cmd1.Parameters.AddWithValue("@CEOContactNo", CEOContactNo);
                                    cmd1.Parameters.AddWithValue("@CEOAadhar", CEOAadhar);
                                    cmd1.Parameters.AddWithValue("@SocietyOfficeDistrict", OfficeDistrict);
                                    cmd1.Parameters.AddWithValue("@SocietyOfficePanchayatSamiti", OfficePanchayatSamiti);
                                    cmd1.Parameters.AddWithValue("@SocietyOfficeCity", OfficeCity);
                                    cmd1.Parameters.AddWithValue("@CEOAadharEnc", CEOAadharEnc);
                                    cmd1.Parameters.AddWithValue("@ExcelUploadId", Result);
                                    cmd1.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                                    cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                    ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    if (ResultInner == "1")
                                    {
                                        Suc = Suc + 1;
                                    }
                                    else if (ResultInner == "2")
                                    {
                                        Dupli = Dupli + 1;
                                        SqlCommand cmd21 = objDBAccess.CreateCommand("InsertSocietyBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd21.Parameters.AddWithValue("@SocietyName", SocietyName);
                                        cmd21.Parameters.AddWithValue("@RegistrationNo", RegistrationNo);
                                        cmd21.Parameters.AddWithValue("@RegistrationDate", RegistrationDate.ConvertString_ToValidDate());
                                        cmd21.Parameters.AddWithValue("@Act", Act);
                                        cmd21.Parameters.AddWithValue("@SocietyType", SocietyType);
                                        cmd21.Parameters.AddWithValue("@StateDistrictlevel", State_Or_District_level_Sports_Association);
                                        cmd21.Parameters.AddWithValue("@Sports", Sports);
                                        cmd21.Parameters.AddWithValue("@CEOName", CEOName);
                                        cmd21.Parameters.AddWithValue("@CEOContactNo", CEOContactNo);
                                        cmd21.Parameters.AddWithValue("@CEOAadhar", CEOAadhar);
                                        cmd21.Parameters.AddWithValue("@SocietyOfficeDistrict", OfficeDistrict);
                                        cmd21.Parameters.AddWithValue("@SocietyOfficePanchayatSamiti", OfficePanchayatSamiti);
                                        cmd21.Parameters.AddWithValue("@SocietyOfficeCity", OfficeCity);
                                        cmd21.Parameters.AddWithValue("@ExcelUploadId", Result);
                                        cmd21.Parameters.AddWithValue("@Reasion", "Duplicate Data");
                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                    else if (ResultInner == "-1")
                                    {
                                        Unsuc = Unsuc + 1;
                                        SqlCommand cmd20 = objDBAccess.CreateCommand("InsertSocietyBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd20.Parameters.AddWithValue("@SocietyName", SocietyName);
                                        cmd20.Parameters.AddWithValue("@RegistrationNo", RegistrationNo);
                                        cmd20.Parameters.AddWithValue("@RegistrationDate", RegistrationDate.ConvertString_ToValidDate());
                                        cmd20.Parameters.AddWithValue("@Act", Act);
                                        cmd20.Parameters.AddWithValue("@SocietyType", SocietyType);
                                        cmd20.Parameters.AddWithValue("@StateDistrictlevel", State_Or_District_level_Sports_Association);
                                        cmd20.Parameters.AddWithValue("@Sports", Sports);
                                        cmd20.Parameters.AddWithValue("@CEOName", CEOName);
                                        cmd20.Parameters.AddWithValue("@CEOContactNo", CEOContactNo);
                                        cmd20.Parameters.AddWithValue("@CEOAadhar", CEOAadhar);
                                        cmd20.Parameters.AddWithValue("@SocietyOfficeDistrict", OfficeDistrict);
                                        cmd20.Parameters.AddWithValue("@SocietyOfficePanchayatSamiti", OfficePanchayatSamiti);
                                        cmd20.Parameters.AddWithValue("@SocietyOfficeCity", OfficeCity);
                                        cmd20.Parameters.AddWithValue("@ExcelUploadId", Result);
                                        cmd20.Parameters.AddWithValue("@Reasion", "Error");
                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                }

                            }
                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();

                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public int SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, "Not Mapped");
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string SocietyRegisteredOfficeDetailsInsert(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)
        {
            string Result = "-1";
            int EMitraTransactionId = 0;
            int SubmitStatus = LstSocietyRegisteredOfficeDetails[0].SubmitStatus;
            try
            {
                objDBAccess.OpenConnection();
                foreach (SocietyRegisteredOfficeDetailsEntity obj in LstSocietyRegisteredOfficeDetails)
                {
                    SqlCommand cmd = new SqlCommand();
                    //cmd = objDBAccess.CreateCommand("InsertSocietyRegOfficeDetail", DBFactoryCommandType.PROCEDURE_COMMAND);

                    cmd = objDBAccess.CreateCommand("InsertSocietyRegOfficeDetail_Edit_Request", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyRegOfficeDetailId", DBFactoryParameterType.BigInt, obj.SocietyRegOfficeDetailId);
                    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                    objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, obj.IsRural);
                    objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, obj.HouseBuildingNo);
                    objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, obj.LaneStreetName);
                    objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, obj.Locality);
                    objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                    objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                    objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                    objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                    objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                    objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                    objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, obj.PinCode);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, obj.UpdatedBy);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, obj.CreatedIP);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, obj.UpdatedIP);
                    objDBAccess.AddParameterToCommand("@IsSameCEOAddress", DBFactoryParameterType.Bit, obj.IsSameCEOAddress);
                    objDBAccess.AddParameterToCommand("@AddressFor", DBFactoryParameterType.Int, obj.AddressFor);
                    objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, obj.EMitraTransactionId == 0 ? EMitraTransactionId : obj.EMitraTransactionId);
                    objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, obj.KIOSKCODE);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSO_ID);
                    objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.NVarchar, obj.SubmitStatus);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, obj.UserType);
                    if (obj.KIOSKCODE.Length > 0)
                    {
                        objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, obj.ServiceId);
                    }
                    else
                    {
                        if (obj.SocietyIdEnc.Length > 0)
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                    }
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    if ((Result.Split('@').Length > 1))
                        EMitraTransactionId = int.Parse(Result.Split('@')[1]);
                }
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


        public int InsertSocietyBearer(SocietyBearerEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyBearer", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.BigInt, obj.SocietyID);
                //objDBAccess.AddParameterToCommand("@Post_Id", DBFactoryParameterType.Int, obj.Post_Id);
                objDBAccess.AddParameterToCommand("@Aadhaar", DBFactoryParameterType.NVarchar, obj.Aadhaar);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                objDBAccess.AddParameterToCommand("@MAddress", DBFactoryParameterType.NVarchar, obj.MAddress);
                objDBAccess.AddParameterToCommand("@GenderId", DBFactoryParameterType.Int, obj.GenderId);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, obj.DOB);
                objDBAccess.AddParameterToCommand("@OccupationId", DBFactoryParameterType.Int, obj.OccupationId);
                objDBAccess.AddParameterToCommand("@MemberTypeId", DBFactoryParameterType.Int, obj.MemberTypeId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, 1);


                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocietyBearer(SocietyBearerEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyProfilingDetails()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProfilingDetailsForSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyListForFileResize(string DisId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FileResizeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, int.Parse(DisId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable FileResizeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyPersonnelProfilingDetails()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProfilingDetailsForSocietyPersonnel", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyPersonnelProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyOfficeProfilingDetails()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProfilingDetailsForSocietyOffice", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyOfficeProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyMembersProfilingDetails()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProfilingDetailsForSocietyMembers", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyMembersProfilingDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public string InsertSocietyAdministrativeDetails(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId, string UserType = "")
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0 || (obj.MEMBERTYPE == "ADMINISTRATIVE" || obj.MEMBERTYPE == "LIQUIDATOR"))
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = new SqlCommand();

                        cmd = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, EMitraTransactionId);
                        objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@NameOfSociety", DBFactoryParameterType.NVarchar, obj.NameOfSociety);
                        objDBAccess.AddParameterToCommand("@PostinSociety", DBFactoryParameterType.NVarchar, obj.PostinSociety);

                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, obj.FatherName);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, (ClassA == null || ClassA.ToString() == "undefined") ? 0 : int.Parse(ClassA));
                        objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, (ClassB == null || ClassB.ToString() == "undefined") ? 0 : int.Parse(ClassB));
                        objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, (ClassC == null || ClassC.ToString() == "undefined") ? 0 : int.Parse(ClassC));
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                        objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                        objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                        objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);
                        objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, obj.BoardType);
                        objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                        objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.Int, obj.ShareAllotted);
                        objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.Money, obj.ShareCapitalAmount);
                        objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.Int, obj.loaneeType);
                        objDBAccess.AddParameterToCommand("SubmitStatus", DBFactoryParameterType.Int, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserTypeCK", DBFactoryParameterType.NVarchar, UserType);

                        if (_LoggedInUser.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _LoggedInUser.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }




                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                            EMitraTransactionId = int.Parse(Result.Split('@')[1]);

                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocietyAdministrativeDetails(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string InsertSocietyAdministrativeDetailsAmendment(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.Name.Length > 0 && obj.PostId != "" && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = new SqlCommand();

                        cmd = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetailsAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, EMitraTransactionId);
                        objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, ClassA.ToString() == "undefined" ? 0 : int.Parse(ClassA));
                        objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, ClassB.ToString() == "undefined" ? 0 : int.Parse(ClassB));
                        objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, ClassC.ToString() == "undefined" ? 0 : int.Parse(ClassC));
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                        objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                        objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                        objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);


                        if (_LoggedInUser.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _LoggedInUser.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }




                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        //if ((Result.Split('@').Length > 1))
                        //    EMitraTransactionId = int.Parse(Result.Split('@')[1]);

                    }
                    i++;
                }

                if (objData.Count > 0)
                {
                    // ApplicationFlowModel _Modal = new ApplicationFlowModel();
                    // _Modal.AddApplicationFlow(objData[0].SocietyIdEnc, 2, "Submit Amendments of Administrative Details(Members).", _LoggedInUser.UserId, true);

                    SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, objData[0].SocietyIdEnc);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    objDBAccess.ExecuteScalarCommand();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocietyAdministrativeDetailsAmendment(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int InsertSocietyAuditReportDetails(SocietyAuditReportEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyAuditReportInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, _Model.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@IsApprovedByRegistrar", DBFactoryParameterType.Bit, _Model.IsApproved);
                objDBAccess.AddParameterToCommand("@IsSocietyWorking", DBFactoryParameterType.Bit, _Model.IsSocietyWorking);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, _Model.FinancialYear);
                objDBAccess.AddParameterToCommand("@AuditReport", DBFactoryParameterType.VARCHAR, _Model.AuditReport);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.VARCHAR, _Model.UserType);
                objDBAccess.AddParameterToCommand("@ApprovedBy", DBFactoryParameterType.Int, _Model.ApprovedBy);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int InsertSocietyAuditReportDetails(SocietyAuditReportEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        //umesh add insert log memeber
        //public DataTable GetSocietyMembersProfilingDetails()
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("ProfilingDetailsForSocietyMembers", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyMembersProfilingDetails()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}
        public DataTable InsertSocietyAuditReportDetails1(SocietymemberdataEntity _Model)
        {
            _Model.Adharno = csCommon.Decrypt_SA(Uri.UnescapeDataString(_Model.Adharno));

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("insertSocietymemberdatalog", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, _Model.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@Adharno", DBFactoryParameterType.VARCHAR, _Model.Adharno);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.VARCHAR, _Model.Name);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.VARCHAR, _Model.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.VARCHAR, _Model.DOB);
                objDBAccess.AddParameterToCommand("@Category", DBFactoryParameterType.VARCHAR, _Model.Category);
                objDBAccess.AddParameterToCommand("@Post", DBFactoryParameterType.VARCHAR, _Model.Post);
                objDBAccess.AddParameterToCommand("@photo", DBFactoryParameterType.VARCHAR, _Model.Photo);
                objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.VARCHAR, _Model.Occupation);
                objDBAccess.AddParameterToCommand("@Adharfile", DBFactoryParameterType.VARCHAR, _Model.Adharfile);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Int, _Model.IsAmendment);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.VARCHAR, _Model.BRN);

                objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(_Model.Adharno) ? COMMON_FUNCTION.Encrypt(_Model.Adharno) : ""));




                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int InsertMemberlogdataDetails(SocietyAuditReportEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //End

        public DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyDetailsBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyDetailsByAmendmentSocietyId(string SocietyIdEnc, string SSOID)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyDetailsByAmendmentSocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyDetailsByAmendmentId(string SocietyIdEnc, string AmendmentId)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyDetailsByAmendmentId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyDetailsBySocietyIdForAmendmet(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyDetailsBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyDetailsBySocietyIdFormTempTbl(string SocietyIdEnc, int EMitraTransactionId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyDetailsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, EMitraTransactionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetGetSocietyDetailsBySocietyIdFormTempTbl(string SocietyIdEnc, int EMitraTransactionId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        //Workscope
        public DataTable GetWorkscopeBySocietyId(string SocietyIdEnc)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = objDBAccess.CreateCommand("WorkscopsBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetWorkscopeBySocietyId(string SocietyIdEnc)" }, ex);
            }



            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetWorkscopeBySocietyIdFormTempTbl(string SocietyIdEnc, int EMitraTransactionId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = objDBAccess.CreateCommand("WorkscopsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, EMitraTransactionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetWorkscopeBySocietyIdFormTempTbl(string SocietyIdEnc, int EMitraTransactionId)" }, ex);
            }



            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAllInspactor(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetInspactor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAllInspactor(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAuditorAssignReportDataShow(SocietyAuditorAssigmentEntity _Model)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorAssignReportDataShow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAuditorAssignReportDataShow(SocietyAuditorAssigmentEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }



        public DataTable SocietyAuditDetailsBySocietyId(int SocietyId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyAuditReportBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable SocietyAuditDetailsBySocietyId(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetAuditDataForFirmOrIndividual(string WhereClause = "", int FinancialYear = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AutorDataForFirmOrIndividual_copy", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinancialYear);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditDataForFirmOrIndividual(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetUploadFileType(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UploadFileUploadType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public DataTable GetUploadFileType(string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int AuditorAssignmentInsert(SocietyAuditorAssigmentEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignmentInsert", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, obj.SocietyIDEnc);

                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.Bit, obj.IsEmpanelledCA);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.Int, obj.IsFirm);
                objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.Int, obj.AuditorId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, obj.FinancialYear);
                objDBAccess.AddParameterToCommand("@Auditorfees", DBFactoryParameterType.Money, obj.Auditorfees);
                objDBAccess.AddParameterToCommand("@LastFinancialYearId", DBFactoryParameterType.Int, obj.LastFinancialYear);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@IPAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ResolutionPDF", DBFactoryParameterType.VARCHAR, obj.ResolutionPDF);
                objDBAccess.AddParameterToCommand("@AuditorAssignFilePath", DBFactoryParameterType.VARCHAR, obj.AuditorAssignFilePath);
                if (obj.ResolutionDate != null && !obj.ResolutionDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@ResolutionDate", DBFactoryParameterType.DateTime, obj.ResolutionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@ResolutionDate", DBFactoryParameterType.DateTime, DateTime.MaxValue.ToString());
                }
                if (obj.AudiAppoOrderDate != null && !obj.AudiAppoOrderDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AudiAppoOrderDate", DBFactoryParameterType.DateTime, obj.AudiAppoOrderDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@AudiAppoOrderDate", DBFactoryParameterType.DateTime, DateTime.MaxValue.ToString());
                }
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int AuditorAssignmentInsert(SocietyAuditorAssigmentEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int SaveAuditorAssigmentsDate(int SocietyAuditReportId, DateTime AuditStartDate, DateTime AuditEndDate, SocietyAuditReportEntity _Model)
        {
            int Result = 0;
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand("SaveAuditorAssigmentsStartEndDate", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@SocietyAuditReportId", DBFactoryParameterType.Int, SocietyAuditReportId);

            //string auditSDate = "";
            //string auditEDate = "";
            //if (!string.IsNullOrWhiteSpace(AuditStartDate))
            //{
            //    auditSDate=
            //}
            if (AuditStartDate != null && !AuditStartDate.ToString("dd/MM/yyyy").Contains("0001"))
            {
                objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.DateTime, AuditStartDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
            }
            if (AuditEndDate != null && !AuditEndDate.ToString("dd/MM/yyyy").Contains("0001"))
            {
                objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.DateTime, AuditEndDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
            }

            objDBAccess.AddParameterToCommand("@IsDeterminedFormat", DBFactoryParameterType.Bit, _Model.IsDeterminedFormat);
            objDBAccess.AddParameterToCommand("@IsApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsApprovedByGeneralAssembly);
            objDBAccess.AddParameterToCommand("@IsDesiredDocumentAttached", DBFactoryParameterType.Bit, _Model.IsDesiredDocumentAttached);
            objDBAccess.AddParameterToCommand("@IsAllAuditReportDocumentsAttached", DBFactoryParameterType.Bit, _Model.IsAllAuditReportDocumentsAttached);
            objDBAccess.AddParameterToCommand("@IsComplianceReportApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsComplianceReportApprovedByGeneralAssembly);
            objDBAccess.AddParameterToCommand("@ClassificationId", DBFactoryParameterType.Int, _Model.ClassificationId);
            objDBAccess.AddParameterToCommand("@TotalActivMembers", DBFactoryParameterType.NVarchar, _Model.TotalActivMembers);
            objDBAccess.AddParameterToCommand("@ACategory", DBFactoryParameterType.NVarchar, _Model.ACategory);
            objDBAccess.AddParameterToCommand("@BCategory", DBFactoryParameterType.NVarchar, _Model.BCategory);
            objDBAccess.AddParameterToCommand("@CCategory", DBFactoryParameterType.NVarchar, _Model.CCategory);
            objDBAccess.AddParameterToCommand("@CeoName", DBFactoryParameterType.NVarchar, _Model.CeoName);
            objDBAccess.AddParameterToCommand("@IsIrregularity", DBFactoryParameterType.Bit, _Model.IsIrregularity);
            objDBAccess.AddParameterToCommand("@IrregularityType", DBFactoryParameterType.Int, _Model.IrregularityType);//b
            objDBAccess.AddParameterToCommand("@DescriptionofIrregularity", DBFactoryParameterType.NVarchar, _Model.DescriptionofIrregularity);
            objDBAccess.AddParameterToCommand("@ResponsibleOfficerName", DBFactoryParameterType.NVarchar, _Model.ResponsibleOfficerName);
            objDBAccess.AddParameterToCommand("@IrregularityAmount", DBFactoryParameterType.NVarchar, _Model.IrregularityAmount);//b
            objDBAccess.AddParameterToCommand("@ActionDescription", DBFactoryParameterType.NVarchar, _Model.ActionDescription);
            objDBAccess.AddParameterToCommand("@IrregularityOfficerName", DBFactoryParameterType.NVarchar, _Model.IrregularityOfficerName);
            objDBAccess.AddParameterToCommand("@SectionId", DBFactoryParameterType.Int, _Model.SectionId);
            objDBAccess.AddParameterToCommand("@IsProfit", DBFactoryParameterType.Bit, _Model.IsProfit);
            objDBAccess.AddParameterToCommand("@TotalMembers", DBFactoryParameterType.NVarchar, _Model.TotalMembers);
            objDBAccess.AddParameterToCommand("@AccumulateProfitLoss", DBFactoryParameterType.NVarchar, _Model.AccumulateProfitLoss);
            objDBAccess.AddParameterToCommand("@Grossprofitor", DBFactoryParameterType.NVarchar, _Model.Grossprofitor);
            objDBAccess.AddParameterToCommand("@TotalAmonut", DBFactoryParameterType.Int, _Model.TotalAmonut);
            objDBAccess.AddParameterToCommand("@Membersshareamount", DBFactoryParameterType.NVarchar, _Model.Membersshareamount);
            objDBAccess.AddParameterToCommand("@StateGovtAmount", DBFactoryParameterType.NVarchar, _Model.StateGovtAmount);
            objDBAccess.AddParameterToCommand("@IsActionisrequired", DBFactoryParameterType.Bit, _Model.IsActionisrequired);




            //objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.NVarchar, SocietyAuditReportId);

            //objDBAccess.AddParameterToCommand("@IsDeterminedFormat", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsApprovedByGeneralAssembly", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsDesiredDocumentAttached", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsAllAuditReportDocumentsAttached", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsComplianceReportApprovedByGeneralAssembly", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@ClassificationId", DBFactoryParameterType.Int, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@TotalActivMembers", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@ACategory", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@BCategory", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@CCategory", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@CeoName", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsIrregularity", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IrregularityType", DBFactoryParameterType.Int, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@DescriptionofIrregularity", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@ResponsibleOfficerName", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IrregularityAmount", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@ActionDescription", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IrregularityOfficerName", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@SectionId", DBFactoryParameterType.Int, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsProfit", DBFactoryParameterType.Bit, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@TotalMembers", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@AccumulateProfitLoss", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@Grossprofitor", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@TotalAmonut", DBFactoryParameterType.Int, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@Membersshareamount", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@StateGovtAmount", DBFactoryParameterType.NVarchar, SocietyAuditReportId);
            //objDBAccess.AddParameterToCommand("@IsActionisrequired", DBFactoryParameterType.Bit, SocietyAuditReportId);  

            int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            return Result;
        }

        public int SaveAuditStartEndDate(DateTime AuditStartDate, DateTime AuditEndDate, int PKID, DateTime Feedate, SocietyAuditorAssigmentEntity _Model)
        {

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveAuditStartEndDateById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PKID", DBFactoryParameterType.Int, PKID);
                if (AuditStartDate != null && !AuditStartDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.DateTime, AuditStartDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (AuditEndDate != null && !AuditEndDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.DateTime, AuditEndDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (Feedate != null && !Feedate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@Feedate", DBFactoryParameterType.DateTime, Feedate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }

                objDBAccess.AddParameterToCommand("@IsDeterminedFormat", DBFactoryParameterType.Bit, _Model.IsDeterminedFormat);
                objDBAccess.AddParameterToCommand("@IsApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsApprovedByGeneralAssembly);
                objDBAccess.AddParameterToCommand("@IsDesiredDocumentAttached", DBFactoryParameterType.Bit, _Model.IsDesiredDocumentAttached);
                objDBAccess.AddParameterToCommand("@IsAllAuditReportDocumentsAttached", DBFactoryParameterType.Bit, _Model.IsAllAuditReportDocumentsAttached);
                objDBAccess.AddParameterToCommand("@IsComplianceReportApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsComplianceReportApprovedByGeneralAssembly);
                objDBAccess.AddParameterToCommand("@ClassificationId", DBFactoryParameterType.Int, _Model.ClassificationId);
                objDBAccess.AddParameterToCommand("@TotalActivMembers", DBFactoryParameterType.NVarchar, _Model.TotalActivMembers);
                objDBAccess.AddParameterToCommand("@ACategory", DBFactoryParameterType.NVarchar, _Model.ACategory);
                objDBAccess.AddParameterToCommand("@BCategory", DBFactoryParameterType.NVarchar, _Model.BCategory);
                objDBAccess.AddParameterToCommand("@CCategory", DBFactoryParameterType.NVarchar, _Model.CCategory);
                objDBAccess.AddParameterToCommand("@CeoName", DBFactoryParameterType.NVarchar, _Model.CeoName);
                objDBAccess.AddParameterToCommand("@IsIrregularity", DBFactoryParameterType.Bit, _Model.IsIrregularity);
                objDBAccess.AddParameterToCommand("@IrregularityType", DBFactoryParameterType.Int, _Model.IrregularityType);//b
                objDBAccess.AddParameterToCommand("@DescriptionofIrregularity", DBFactoryParameterType.NVarchar, _Model.DescriptionofIrregularity);
                objDBAccess.AddParameterToCommand("@ResponsibleOfficerName", DBFactoryParameterType.NVarchar, _Model.ResponsibleOfficerName);
                objDBAccess.AddParameterToCommand("@IrregularityAmount", DBFactoryParameterType.NVarchar, _Model.IrregularityAmount);//b
                objDBAccess.AddParameterToCommand("@ActionDescription", DBFactoryParameterType.NVarchar, _Model.ActionDescription);
                objDBAccess.AddParameterToCommand("@IrregularityOfficerName", DBFactoryParameterType.NVarchar, _Model.IrregularityOfficerName);
                objDBAccess.AddParameterToCommand("@SectionId", DBFactoryParameterType.Int, _Model.SectionId);
                objDBAccess.AddParameterToCommand("@IsProfit", DBFactoryParameterType.Bit, _Model.IsProfit);
                objDBAccess.AddParameterToCommand("@TotalMembers", DBFactoryParameterType.NVarchar, _Model.TotalMembers);
                objDBAccess.AddParameterToCommand("@AccumulateProfitLoss", DBFactoryParameterType.NVarchar, _Model.AccumulateProfitLoss);
                objDBAccess.AddParameterToCommand("@Grossprofitor", DBFactoryParameterType.NVarchar, _Model.Grossprofitor);
                objDBAccess.AddParameterToCommand("@TotalAmonut", DBFactoryParameterType.Int, _Model.TotalAmonut);
                objDBAccess.AddParameterToCommand("@Membersshareamount", DBFactoryParameterType.NVarchar, _Model.Membersshareamount);
                objDBAccess.AddParameterToCommand("@StateGovtAmount", DBFactoryParameterType.NVarchar, _Model.StateGovtAmount);
                objDBAccess.AddParameterToCommand("@IsActionisrequired", DBFactoryParameterType.Bit, _Model.IsActionisrequired);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "SaveAuditStartEndDate(DateTime AuditStartDate, DateTime AuditEndDate,int PKID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public int AuditorAssignmentInsertOldEntry(SocietyAuditorAssigmentEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignmentInsertOldEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, obj.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.Bit, obj.IsEmpanelledCA);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.Int, obj.IsFirm);
                objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.Int, obj.AuditorId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, obj.FinancialYear);
                objDBAccess.AddParameterToCommand("@Auditorfees", DBFactoryParameterType.Money, obj.Auditorfees);
                objDBAccess.AddParameterToCommand("@LastFinancialYearId", DBFactoryParameterType.Int, obj.LastFinancialYear);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@IPAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                if (obj.FeeDate != null && !obj.FeeDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@FeeDate", DBFactoryParameterType.DateTime, obj.FeeDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                objDBAccess.AddParameterToCommand("@ChallanNo", DBFactoryParameterType.NVarchar, obj.ChallanNo);
                objDBAccess.AddParameterToCommand("@InspectorName", DBFactoryParameterType.NVarchar, obj.InspectorName);
                objDBAccess.AddParameterToCommand("@ApprovedBy", DBFactoryParameterType.Int, obj.ApprovedBy);
                if (obj.AudiAppoOrderDate != null && !obj.AudiAppoOrderDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AudiAppoOrderDate", DBFactoryParameterType.DateTime, obj.AudiAppoOrderDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (obj.ResolutionDate != null && !obj.ResolutionDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@ResolutionDate", DBFactoryParameterType.DateTime, obj.ResolutionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                objDBAccess.AddParameterToCommand("@HaveAuditReport", DBFactoryParameterType.NVarchar, obj.HaveAuditReport);

                if (obj.AuditStartDate != null && !obj.AuditStartDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.DateTime, obj.AuditStartDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (obj.AuditEndDate != null && !obj.AuditEndDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.DateTime, obj.AuditEndDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }

                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int AuditorAssignmentInsert(SocietyAuditorAssigmentEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int PermanentsaveSocietyAuditorAssigments(int SocietyAuditorAssigmentsId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PermanentsaveSocietyAuditorAssigments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyAuditorAssigmentsId", DBFactoryParameterType.Int, SocietyAuditorAssigmentsId);

                int.TryParse(objDBAccess.ExecuteCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int PermanentsaveSocietyAuditorAssigments(int SocietyAuditorAssigmentsId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int AuditorAssignmentFilesInsert(List<UploadFileDetailSocietyInspectorAssignmentEntity> lst)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                int flag = 1;
                foreach (var item in lst)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignmentFilesInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyInspectorAssignId", DBFactoryParameterType.Int, item.SocietyInspectorAssignId);
                    objDBAccess.AddParameterToCommand("@UploadTypeId", DBFactoryParameterType.Int, item.UploadTypeId);
                    objDBAccess.AddParameterToCommand("@UploadFileName", DBFactoryParameterType.VARCHAR, item.UploadFileName);
                    objDBAccess.AddParameterToCommand("@UploadExtraFileTitle", DBFactoryParameterType.NVarchar, item.UploadExtraFileTitle);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, item.IsActive);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Bit, item.IsDelete);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, item.CreateBy);
                    objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.VARCHAR, item.DocumentType);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, flag);
                    int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
                    if (item.UploadTypeId == -1)
                        flag++;
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int AuditorAssignmentFilesInsert(List<UploadFileDetailSocietyInspectorAssignmentEntity> lst)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public int UpdateAuditorAssignmentFiles(string AuditorAssignFilePath, string ResolutionPDF, int Result)
        {
            int Result1 = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateAuditorAssignmentFiles", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditorAssignFilePath", DBFactoryParameterType.VARCHAR, AuditorAssignFilePath);
                objDBAccess.AddParameterToCommand("@ResolutionPDF", DBFactoryParameterType.VARCHAR, ResolutionPDF);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Result);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result1);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int AuditorAssignmentFilesInsert(List<UploadFileDetailSocietyInspectorAssignmentEntity> lst)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result1;
        }

        public DataTable GetAuditorAssignmentList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignmentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorAssignmentList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetworkscopedataList(int SocietyId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWorkscopedata", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public string SavePermanentSocietyData(string SocietyIDEnc)
        {
            string result = "-1";
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SocietyPermanentSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);

                result = objDBAccess.ExecuteCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SavePermanentData(string SocietyIDEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public string SocietyAssignOfficerSave(string SocietyIDEnc)
        {
            string result = "-1";
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SocietyAssignOfficerSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);

                result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAssignOfficerSave(string " + SocietyIDEnc + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public string SAVEBranch(int DistrictId, int BlockId, string BranchNameHi, string BranchName_En, int CityId)
        {
            string result = "-1";
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("InsertBRANCH", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@BranchNameHi", DBFactoryParameterType.NVarchar, BranchNameHi);
                objDBAccess.AddParameterToCommand("@BranchName_En", DBFactoryParameterType.NVarchar, BranchName_En);


                result = objDBAccess.ExecuteCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SavePermanentData(string SocietyIDEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        public DataTable GetAllAuditorAssignmentByDepartment(string WhereClause, int IsForEmpanelledAgency, int LevelId, int Division_District_Id, string FYearId, int CategoryId, int UserRoleDepartmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListBydepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@IsForEmpanelledAgency", DBFactoryParameterType.Int, IsForEmpanelledAgency);
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, Division_District_Id);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, FYearId == "" ? 0 : int.Parse(FYearId));
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, CategoryId);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentId", DBFactoryParameterType.Int, UserRoleDepartmentId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAllAuditorAssignmentByDepartment(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetInspactorName(string DistId, string OfficeID)
        {
            dt.Clear();
            try
            {
                string WhereClause = "";
                int TempLevelID = 0;
                string Div_DistrictID = "0";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (DistId == "0" || DistId == "" || DistId == "undefined")
                {
                    if (_LoggedInUser.LevelId == 3)
                    {

                        WhereClause = " AND URM.DistrictId = " + _LoggedInUser.DistrictId;
                        TempLevelID = 3;
                        Div_DistrictID = _LoggedInUser.DistrictId.ToString();
                    }
                    else if (_LoggedInUser.LevelId == 2)
                    {
                        WhereClause = " AND URM.DivisionId = " + _LoggedInUser.DivisionId;
                        TempLevelID = 2;
                        Div_DistrictID = _LoggedInUser.DivisionId.ToString();
                    }
                    else if (_LoggedInUser.LevelId == 1)
                    {
                        WhereClause = " ";
                        TempLevelID = 1;
                        Div_DistrictID = "0";
                    }
                }
                else
                {
                    WhereClause = " AND URM.DistrictId = " + DistId;
                    Div_DistrictID = DistId;
                    TempLevelID = 3;
                }

                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetInspactorName", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, TempLevelID);
                objDBAccess.AddParameterToCommand("@Div_DistrictID", DBFactoryParameterType.NVarchar, Div_DistrictID);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.NVarchar, OfficeID);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetInspactorName()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetInspactorForRAO(int DistId, string DivisionId, string OfficeId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRAOInspactor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictID", DBFactoryParameterType.Int, DistId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, int.Parse(OfficeId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetInspactorForRAO()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int saveAuditAssignbydeprt(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string FileName = "", int UserRoleDepartmentId = 0, string OrderNo = "")
        {
            int Result = -1;
            var Auditers = AuditerId.Split(',');
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignByDepartmentinsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditerId", DBFactoryParameterType.VARCHAR, Auditers[0]);

                if (AuditBy == "0")
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, false);
                else
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, true);

                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.VARCHAR, Ids);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.VARCHAR, Status);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.VARCHAR, IsFirm);
                objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.VARCHAR, FileName);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.VARCHAR, OrderNo);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentId", DBFactoryParameterType.Int, UserRoleDepartmentId);
                objDBAccess.AddParameterToCommand("@MultipalUser", DBFactoryParameterType.VARCHAR, AuditerId.ToString().Contains(",") ? AuditerId : "");



                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "saveAuditAssignbydeprt(string FunPassValue = '', string Ids = '')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int RejectAuditorRequest(string Ids = "", string DeleteResion = "", string DeleteBy = "")
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RejectAuditApprovedRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.VARCHAR, Ids);
                objDBAccess.AddParameterToCommand("@DeleteResion", DBFactoryParameterType.VARCHAR, DeleteResion);
                objDBAccess.AddParameterToCommand("@DeleteBy", DBFactoryParameterType.VARCHAR, DeleteBy);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "saveAuditAssignbydeprt(string FunPassValue = '', string Ids = '')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        /*24June2025   -- for calling one time only not loop */
        public int saveAuditAssignbydeprtNotRequest_old(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", long UserId = 0, string FileName = "", int UserRoleDepartmentId = 0, string AuditTillDate = "", string IsForced = "False", string SSOID = "", int FinancialYearId = 0, string OrderNo = "")
        {
            int Result = -1;
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            try
            {
                var Iddata = Ids.Split(',');
                var Auditers = AuditerId.Split(',');
                SqlCommand cmd = new SqlCommand();
                for (int i = 0; i < Iddata.Length; i++)
                {
                    if (Iddata[i].ToString() != "")
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "AuditorAssignByDepartmentinsertNotRequest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AuditerId", Auditers[0]);
                        if (AuditBy == "0")
                            cmd.Parameters.AddWithValue("@AuditBy", false);
                        else
                            cmd.Parameters.AddWithValue("@AuditBy", true);

                        cmd.Parameters.AddWithValue("@Ids", Iddata[i].ToString());
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@IsFirm", IsFirm);
                        cmd.Parameters.AddWithValue("@CreateBy", UserId);
                        cmd.Parameters.AddWithValue("@FilePath", FileName);
                        cmd.Parameters.AddWithValue("@AuditTillDate", AuditTillDate);
                        cmd.Parameters.AddWithValue("@IsForced", IsForced == "True" ? 1 : 0);
                        cmd.Parameters.AddWithValue("@UserRoleDepartmentId", UserRoleDepartmentId);
                        cmd.Parameters.AddWithValue("@SSO_ID", SSOID);
                        cmd.Parameters.AddWithValue("@FIN_YEAR_ID", FinancialYearId);
                        cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                        cmd.Parameters.AddWithValue("@MultipalUser", AuditerId.ToString().Contains(",") ? AuditerId : "");

                        Result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        if (Result == -1)
                        {
                            transaction.Rollback();
                            break;
                        }
                    }
                }

                if (Result > 0)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "saveAuditAssignbydeprtNotRequest(string FunPassValue = '', string Ids = '')" }, ex);
                transaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
            return Result;
        }

        public int saveAuditAssignbydeprtNotRequest(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", long UserId = 0, string FileName = "", int UserRoleDepartmentId = 0, string AuditTillDate = "", string IsForced = "False", string SSOID = "", int FinancialYearId = 0, string OrderNo = "")
        {
            int Result = -1;
            string qry = "";
            qry += "AuditerId=" + AuditerId + " AuditBy=" + AuditBy + " Ids=" + Ids + " Status=" + Status + " IsFirm=" + IsFirm + " UserId=" + UserId + " FileName=" + FileName + " UserRoleDepartmentId=" + UserRoleDepartmentId + " AuditTillDate=" + AuditTillDate + " IsForced=" + IsForced + " SSOID=" + SSOID + " FinancialYearId=" + FinancialYearId + " OrderNo=" + OrderNo;

            try
            {

                if (Ids != "")
                {
                    var Auditers = AuditerId.Split(',');
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignByDepartmentinsertNotRequest_V1", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@AuditerId", DBFactoryParameterType.NVarchar, Auditers[0]);
                    objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Int, AuditBy == "0" ? 0 : 1);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                    objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.Int, IsFirm);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, UserId);
                    objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, FileName);
                    objDBAccess.AddParameterToCommand("@UserRoleDepartmentId", DBFactoryParameterType.Int, UserRoleDepartmentId);
                    objDBAccess.AddParameterToCommand("@AuditTillDate", DBFactoryParameterType.NVarchar, AuditTillDate);
                    objDBAccess.AddParameterToCommand("@IsForced", DBFactoryParameterType.Int, IsForced == "True" ? 1 : 0);
                    objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess.AddParameterToCommand("@FIN_YEAR_ID", DBFactoryParameterType.Int, FinancialYearId);
                    objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                    objDBAccess.AddParameterToCommand("@MultipalUser", DBFactoryParameterType.NVarchar, AuditerId.ToString().Contains(",") ? AuditerId : "");

                    qry += objDBAccess.getSqlCommandText();
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "saveAuditAssignbydeprtNotRequest(string FunPassValue = '', string Ids = '')" + qry }, ex);
                objDBAccess.CloseConnection();
                //transaction.Rollback();
            }

            return Result;
        }

        /*24June2025   -- for calling one time only not loop */
        public int saveAuditAssignbydeprtNotRequestReOrder_old(string AuditerId = "", string AuditBy = "", string Ids = "", string AssignedIds = "", string Status = "", string IsFirm = "", long UserId = 0, string FileName = "", int UserRoleDepartmentId = 0, string AuditTillDate = "", string IsForced = "False", string SSOID = "", int FinancialYearId = 0, string OrderNo = "", string DeleteResions = "")
        {
            int Result = -1;
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            try
            {
                var Iddata = Ids.Split(',');
                var Auditers = AuditerId.Split(',');
                SqlCommand cmd = new SqlCommand();
                for (int i = 0; i < Iddata.Length; i++)
                {
                    if (Iddata[i].ToString() != "")
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "AuditorAssignByDepartmentinsertNotRequest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AuditerId", Auditers[0]);
                        if (AuditBy == "0")
                            cmd.Parameters.AddWithValue("@AuditBy", false);
                        else
                            cmd.Parameters.AddWithValue("@AuditBy", true);

                        cmd.Parameters.AddWithValue("@Ids", Iddata[i].ToString());
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@IsFirm", IsFirm);
                        cmd.Parameters.AddWithValue("@CreateBy", UserId);
                        cmd.Parameters.AddWithValue("@FilePath", FileName);
                        cmd.Parameters.AddWithValue("@AuditTillDate", AuditTillDate);
                        cmd.Parameters.AddWithValue("@IsForced", IsForced == "True" ? 1 : 0);
                        cmd.Parameters.AddWithValue("@UserRoleDepartmentId", UserRoleDepartmentId);
                        cmd.Parameters.AddWithValue("@SSO_ID", SSOID);
                        cmd.Parameters.AddWithValue("@FIN_YEAR_ID", FinancialYearId);
                        cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                        cmd.Parameters.AddWithValue("@MultipalUser", AuditerId.ToString().Contains(",") ? AuditerId : "");

                        Result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        if (Result == -1)
                        {
                            transaction.Rollback();
                            break;
                        }
                    }
                }
                string result = ReorderSocietiesDetails_old(OrderNo, DeleteResions, AssignedIds, sqlConnection, transaction);
                if (Result > 0 && result != "ERROR")
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "saveAuditAssignbydeprtNotRequestReOrder(string FunPassValue = '', string Ids = '')" }, ex);
                transaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
            return Result;
        }


        public int saveAuditAssignbydeprtNotRequestReOrder(string AuditerId = "", string AuditBy = "", string Ids = "", string AssignedIds = "", string Status = "", string IsFirm = "", long UserId = 0, string FileName = "", int UserRoleDepartmentId = 0, string AuditTillDate = "", string IsForced = "False", string SSOID = "", int FinancialYearId = 0, string OrderNo = "", string DeleteResions = "")
        {
            int Result = -1;
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            try
            {

                var Auditers = AuditerId.Split(',');

                if (Ids != "")
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignByDepartmentinsertNotRequest_V1", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@AuditerId", DBFactoryParameterType.NVarchar, Auditers[0]);
                    objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Int, AuditBy == "0" ? 0 : 1);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                    objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.Int, IsFirm);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, UserId);
                    objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, FileName);
                    objDBAccess.AddParameterToCommand("@UserRoleDepartmentId", DBFactoryParameterType.Int, UserRoleDepartmentId);
                    objDBAccess.AddParameterToCommand("@AuditTillDate", DBFactoryParameterType.NVarchar, AuditTillDate);
                    objDBAccess.AddParameterToCommand("@IsForced", DBFactoryParameterType.Int, IsForced == "True" ? 1 : 0);
                    objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess.AddParameterToCommand("@FIN_YEAR_ID", DBFactoryParameterType.Int, FinancialYearId);
                    objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                    objDBAccess.AddParameterToCommand("@MultipalUser", DBFactoryParameterType.NVarchar, AuditerId.ToString().Contains(",") ? AuditerId : "");


                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();
                }


                string result = ReorderSocietiesDetails(OrderNo, DeleteResions, AssignedIds, sqlConnection);

            }
            catch (Exception ex)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "saveAuditAssignbydeprtNotRequestReOrder(string FunPassValue = '', string Ids = '')" }, ex);
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public string ReorderSocietiesDetails_old(string OrderNo, string DeleteResions, string Id, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string Result = "ERROR";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand();
                cmd.CommandText = "ReorderSocietiesDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                cmd.Parameters.AddWithValue("@DeleteResions", DeleteResions);
                cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@Id", Id);
                Result = Convert.ToString(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "ReorderSocietiesDetails_old(string OrderNo, string DeleteResions, string Id, SqlConnection sqlConnection, SqlTransaction transaction)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }
        public string ReorderSocietiesDetails(string OrderNo, string DeleteResions, string Id, SqlConnection sqlConnection)
        {
            string Result = "ERROR";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand();
                cmd.CommandText = "ReorderSocietiesDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                //cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                cmd.Parameters.AddWithValue("@DeleteResions", DeleteResions);
                cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@Id", Id);
                Result = Convert.ToString(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "ReorderSocietiesDetails(string OrderNo, string DeleteResions, string Id, SqlConnection sqlConnection)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }



        public string SaveBankDetailmodel(SocietyRegistrationEntity _Entity)
        {

            string Result = "-1";
            _Entity.PaidUpShareCapital = _Entity.OpeningBalance;
            if (_Entity.flag == 1)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertBankStatment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, _Entity.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@BankPassBook", DBFactoryParameterType.NVarchar, _Entity.BankPassbookFile);
                    objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Entity.PaidUpShareCapital);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Double, 1);
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();

                    ApplicationFlowModel AFM = new ApplicationFlowModel();
                    AFM.AddApplicationFlow(_Entity.SocietyIDEnc, 24, "BANK ACCOUNT DETAILS UPLOADED", 0);

                }
                catch (Exception ex)
                {
                    Result = "-1";
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();

                }
            }
            else if (_Entity.flag == 2)
            {

                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertBankStatment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@BankOpeningCertificate", DBFactoryParameterType.NVarchar, _Entity.BankOpeningCertificate);
                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, _Entity.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, 2);
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();

                }
                catch (Exception ex)
                {
                    Result = "-1";
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Add(UserEntity obj,List<RoleArray> objRoleListData)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            return Result;
        }
        public string SaveBankDetailForKioskmodel(int UniqueId, string SocietyIDEnc)
        {

            string Result = "-1";

            if (UniqueId > 0)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("UpdateBankStatmentForTempToMainByKiosk", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@UniqueId", DBFactoryParameterType.Int, UniqueId);
                    dt = objDBAccess.GetTable();
                    objDBAccess.CloseConnection();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DResponse resp = JsonConvert.DeserializeObject<DResponse>(dt.Rows[0]["Result"].ToString());
                        if (resp.MSG == "Transaction Successfully Done" || resp.MSG == "Request successfully processed")
                        {
                            ApplicationFlowModel AFM = new ApplicationFlowModel();
                            AFM.AddApplicationFlow(SocietyIDEnc, 24, "BANK ACCOUNT DETAILS UPLOAD SUCCESSFULLY", 0);
                            AFM.SendMSG(SocietyIDEnc, 24);

                        }
                    }

                    Result = dt.Rows[0]["Result"].ToString();

                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();

                }
            }

            return Result;
        }

        public string GetEmitraBankStatmentLastTran(int SocietyID)
        {
            string Result = "-1";
            if (SocietyID > 0)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("GetEmitraBankStatmentLastTran", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyID);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, 1);
                    dt = objDBAccess.GetTable();
                    objDBAccess.CloseConnection();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Result = dt.Rows[0]["ResponseString"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetEmitraBankStatmentLastTran(SocietyRegistrationEntity _Entity)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }

            return Result;
        }

        public string SaveBankDetailForKiosk(SocietyRegistrationEntity _Entity)
        {
            string Result = "-1";
            _Entity.PaidUpShareCapital = _Entity.OpeningBalance;
            if (_Entity.flag == 1)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertBankStatmentForKiosk", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Entity.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@BankPassBook", DBFactoryParameterType.NVarchar, _Entity.BankPassbookFile);
                    objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Entity.PaidUpShareCapital);
                    objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.Double, _Entity.SSOID);
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();

                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();

                }
            }

            return Result;
        }

        public DataTable GetBasicdetailsData(int SocietyId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWorkscopedata", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorAssignmentList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int DeleteAuditreport(int ClassId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteAuditreportAssgin", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, ClassId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "DeleteAuditreport(int ClassId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int DeleteSocietyAuditorAssigments(int ClassId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSocietyAuditorAssigments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, ClassId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "DeleteSocietyAuditorAssigments(int ClassId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string SaveSocietyFinalSubmition(string SocietyIdEncr)
        {
            string Result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveSocietyFinalSubmition", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEncr);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "SaveSocietyFinalSubmition(string SocietyIdEncr)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyCertificateDataList(string WhereClause)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "GetSocietyCertificateDataList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocietyRegReport(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyRegistrationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable SocietyRegReport(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyDistrictList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDistrictList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int CheckSocietyByBRN(string BRN, string SSOID, string AADHARNOENC, int IsNewReg = 0, string SocietyIdENC = "")
        {
            int result = 1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CheckSocietyByBRN", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, BRN);
                objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.NVarchar, AADHARNOENC);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@IsNewReg", DBFactoryParameterType.Int, IsNewReg);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdENC);
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());  // return 1 for ALLOW and 0 for not ALLOW.
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int CheckSocietyByBRN(string BRN, string SSOID, string AADHARNOENC, int IsNewReg = 0, string SocietyIdENC = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int CheckSocietyByPdcsNo(string PdcsNo)
        {
            int result = 1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CheckSocietyByPdcsNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PdcsNo", DBFactoryParameterType.VARCHAR, PdcsNo);
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int CheckSocietyByPdcsNo(string PdcsNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public DataTable GetSocietyActList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyActList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyActList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocietyTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyTypeList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyYearhList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyYearMonthList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyMonthList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMonthList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyYearMonthList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietiesForDeshBoardBySocietyId(string SocietyIdEnc)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyImpoDate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietiesForDeshBoardBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAllActiveSocietyList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllActiveSocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAllActiveSocietyList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAllActiveSocietyAminDetailList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllActiveSocietyAminDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAllActiveSocietyList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetLovValuesByListId(int ListId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LovListId", DBFactoryParameterType.Int, ListId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetLovValuesByListId(int ListId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocityExcelDetails(long userId, int UserMapId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocityExcelDetailsByCreatedBy", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                objDBAccess.AddParameterToCommand("@UserMapId", DBFactoryParameterType.Int, UserMapId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable  GetSocityExcelDetails(int userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocityExcelDetailsCourt(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocityExcelDetailsByCreatedByCourt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable  GetSocityExcelDetails(int userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetExlSocietyDetails(int TypeId, int ExlId, int StatusId, int MappingId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelSocityDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@TypeId", DBFactoryParameterType.Int, TypeId);
                objDBAccess.AddParameterToCommand("@UserMapId", DBFactoryParameterType.Int, MappingId);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable   GetExlSocietyDetails(int ExlId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }








        public string UpdateExcelSocityDetailsByExcelUpload(string SocietyId, string AdministrativeId, string SocietyName, string RegistrationNo, string RegistrationDate, string Name, string ContactNo, string Aadh, string SocietyTypeId)
        {
            string result = "1";
            try
            {
                string newDate = "";
                if (!IsValidDateTime(RegistrationDate))
                {
                    return "-3";
                }
                else
                {

                    newDate = RegistrationDate.Split('/')[2] + "/" + RegistrationDate.Split('/')[1] + "/" + RegistrationDate.Split('/')[0];
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateExcelSocityDetailsByExcelUpload", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.NVarchar, AdministrativeId);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, SocietyName);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.NVarchar, newDate);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, ContactNo);
                objDBAccess.AddParameterToCommand("@Aadh", DBFactoryParameterType.NVarchar, Aadh);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, SocietyTypeId);
                result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                result = "0";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable   UpdateExcelSocityDetailsByExcelUpload(string SocietyId, string AdministrativeId, string SocietyName, string RegistrationNo, string RegistrationDate, string Name, string ContactNo, string Aadh)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public string InsertSocityByExcelCourt(CaseRegistraion obj)
        {
            string Result = "-1";
            string connString = "";
            try
            {
                string conString = string.Empty;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocityExcelDetailsCourt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/CourtExcel/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));

                        //  objDBAccess.OpenConnection();
                        try
                        {
                            int i = 0;

                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string CourtName = Dr["CourtName"].ToString().Trim();
                                string plaintiff = Dr["plaintiff"].ToString().Trim();
                                string Defendant = Dr["Defendant"].ToString().Trim();
                                string TypeOfCase = Dr["TypeOfCase"].ToString().Trim();
                                string CaseRegistrationDate = Dr["CaseRegistrationDate"].ToString().Trim();
                                string CaseRegistrationNumber = Dr["CaseRegistrationNumber"].ToString().Trim();
                                string CourtSection = Dr["CourtSection"].ToString().Trim();
                                string CaseBriefDescription = Dr["CaseBriefDescription"].ToString().Trim();
                                string LastHearingDate = Dr["LastHearingDate"].ToString().Trim();
                                string NextHearingDate = Dr["NextHearingDate"].ToString().Trim();
                                string CaseCurrentStatus = Dr["CaseCurrentStatus"].ToString().Trim();

                                if (CourtName != "" && CaseRegistrationNumber != "")
                                {

                                    if (CourtName == "")
                                    {
                                        return "Please enter Court Name on line No" + i.ToString();
                                    }
                                    else if (plaintiff == "")
                                    {
                                        return "Please enter plaintiff Name on line No" + i.ToString();
                                    }
                                    else if (Defendant == "")
                                    {
                                        return "Please enter Defendant Name on line No" + i.ToString();
                                    }
                                    else if (TypeOfCase == "")
                                    {
                                        return "Please enter Type Of Case on line No" + i.ToString();
                                    }
                                    else if (CaseRegistrationDate == "")
                                    {
                                        return "Please enter Case Registration Date on line No" + i.ToString();
                                    }

                                    else if (CaseRegistrationNumber == "")
                                    {
                                        return "Please enter Case Registration Number on line No " + i.ToString();
                                    }
                                    else if (CourtSection == "")
                                    {
                                        return "Please enter Court Section on line No " + i.ToString();
                                    }
                                    else if (LastHearingDate == "")
                                    {
                                        return "Please enter Last Hearing Date on line No " + i.ToString();
                                    }
                                    else if (CaseBriefDescription == "")
                                    {
                                        return "Please enter Case Brief Description on line No " + i.ToString();
                                    }
                                    else if (NextHearingDate == "")
                                    {
                                        return "Please enter Next HearingDate on line No " + i.ToString();
                                    }
                                    else if (CaseCurrentStatus == "")
                                    {
                                        return "Please enter Case Current Status on line No " + i.ToString();
                                    }
                                }
                            }
                            string ResultInner = ""; int Suc = 0, Unsuc = 0, Dupli = 0;

                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string CourtName = Dr["CourtName"].ToString().Trim();
                                string plaintiff = Dr["plaintiff"].ToString().Trim();
                                string Defendant = Dr["Defendant"].ToString().Trim();
                                string TypeOfCase = Dr["TypeOfCase"].ToString().Trim();
                                string CaseRegistrationDate = Dr["CaseRegistrationDate"].ToString().Trim();
                                string CaseRegistrationNumber = Dr["CaseRegistrationNumber"].ToString().Trim();
                                string CourtSection = Dr["CourtSection"].ToString().Trim();
                                string CaseBriefDescription = Dr["CaseBriefDescription"].ToString().Trim();
                                string LastHearingDate = Dr["LastHearingDate"].ToString().Trim();
                                string NextHearingDate = Dr["NextHearingDate"].ToString().Trim();
                                string CaseCurrentStatus = Dr["CaseCurrentStatus"].ToString().Trim();
                                if (CourtName != "" && CaseRegistrationNumber != "")
                                {
                                    Dr["ExcelUploadId"] = Result;
                                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertCaseDetailsByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);

                                    cmd1.Parameters.AddWithValue("@CourtName", CourtName);
                                    cmd1.Parameters.AddWithValue("@plaintiff", plaintiff);
                                    cmd1.Parameters.AddWithValue("@Defendant", Defendant);
                                    cmd1.Parameters.AddWithValue("@TypeOfCase", TypeOfCase);
                                    cmd1.Parameters.AddWithValue("@CaseRegistrationDate", CaseRegistrationDate);
                                    cmd1.Parameters.AddWithValue("@CaseRegistrationNumber", CaseRegistrationNumber);
                                    cmd1.Parameters.AddWithValue("@CourtSection", CourtSection);
                                    cmd1.Parameters.AddWithValue("@CaseBriefDescription", CaseBriefDescription);
                                    cmd1.Parameters.AddWithValue("@LastHearingDate", LastHearingDate);
                                    cmd1.Parameters.AddWithValue("@NextHearingDate", NextHearingDate);
                                    cmd1.Parameters.AddWithValue("@CaseCurrentStatus", CaseCurrentStatus);
                                    cmd1.Parameters.AddWithValue("@ExcelUploadId", Result);
                                    cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                    cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                    ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    if (ResultInner == "1")
                                    {
                                        Suc = Suc + 1;
                                    }
                                    else if (ResultInner == "2")
                                    {
                                        Dupli = Dupli + 1;
                                        SqlCommand cmd21 = objDBAccess.CreateCommand("InsertCaseBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd21.Parameters.AddWithValue("@CourtName", CourtName);
                                        cmd21.Parameters.AddWithValue("@plaintiff", plaintiff);
                                        cmd21.Parameters.AddWithValue("@Defendant", Defendant);
                                        cmd21.Parameters.AddWithValue("@TypeOfCase", TypeOfCase);
                                        cmd21.Parameters.AddWithValue("@CaseRegistrationDate", CaseRegistrationDate);
                                        cmd21.Parameters.AddWithValue("@CaseRegistrationNumber", CaseRegistrationNumber);
                                        cmd21.Parameters.AddWithValue("@CourtSection", CourtSection);
                                        cmd21.Parameters.AddWithValue("@CaseBriefDescription", CaseBriefDescription);
                                        cmd21.Parameters.AddWithValue("@LastHearingDate", LastHearingDate);
                                        cmd21.Parameters.AddWithValue("@NextHearingDate", NextHearingDate);
                                        cmd21.Parameters.AddWithValue("@CaseCurrentStatus", CaseCurrentStatus);
                                        cmd21.Parameters.AddWithValue("@ExcelUploadId", Result);
                                        cmd21.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                        cmd21.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                        cmd21.Parameters.AddWithValue("@Reasion", "Duplicate Data");
                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                    else if (ResultInner == "-1")
                                    {
                                        Unsuc = Unsuc + 1;
                                        SqlCommand cmd20 = objDBAccess.CreateCommand("InsertCaseBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd20.Parameters.AddWithValue("@CourtName", CourtName);
                                        cmd20.Parameters.AddWithValue("@plaintiff", plaintiff);
                                        cmd20.Parameters.AddWithValue("@Defendant", Defendant);
                                        cmd20.Parameters.AddWithValue("@TypeOfCase", TypeOfCase);
                                        cmd20.Parameters.AddWithValue("@CaseRegistrationDate", CaseRegistrationDate);
                                        cmd20.Parameters.AddWithValue("@CaseRegistrationNumber", CaseRegistrationNumber);
                                        cmd20.Parameters.AddWithValue("@CourtSection", CourtSection);
                                        cmd20.Parameters.AddWithValue("@CaseBriefDescription", CaseBriefDescription);
                                        cmd20.Parameters.AddWithValue("@LastHearingDate", LastHearingDate);
                                        cmd20.Parameters.AddWithValue("@NextHearingDate", NextHearingDate);
                                        cmd20.Parameters.AddWithValue("@CaseCurrentStatus", CaseCurrentStatus);
                                        cmd20.Parameters.AddWithValue("@ExcelUploadId", Result);
                                        cmd20.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                        cmd20.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                        cmd20.Parameters.AddWithValue("@Reasion", "Error");
                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                }

                            }
                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();

                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetExlSocietyDetailsCourt(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelSocityDetailsByExcelUploadIdCourt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable   GetExlSocietyDetails(int ExlId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable HouseSocietyListForAuditRPT(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {

                string WhereClause = " and 1=1 ";
                if (Obj.UserType == "C")
                {
                    WhereClause = " and Societies.SSOID='" + Obj.SSOID + "' ";
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("HouseSocietyListForAuditRPT", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "HouseSocietyListForAuditRPT(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAuditorAndSocietyDetailByIds(string Ids, string AuditorId, int Flag = 0, int FinancialYearId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorAndSocietyDetailByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.NVarchar, AuditorId);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                objDBAccess.AddParameterToCommand("@FYearID", DBFactoryParameterType.Int, FinancialYearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorAndSocietyDetailByIds(int Ids)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCurrentFinancialYear()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCurrentFinancialYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                //objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.NVarchar, AuditorId);
                //objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                //objDBAccess.AddParameterToCommand("@FYearID", DBFactoryParameterType.Int, FinancialYearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetCurrentFinancialYear(int Ids)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetCurrentAssessmentYear()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCurrentAssessmentYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                //objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.NVarchar, AuditorId);
                //objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                //objDBAccess.AddParameterToCommand("@FYearID", DBFactoryParameterType.Int, FinancialYearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetCurrentFinancialYear(int Ids)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetAuditorsForMultipalAssigment(string AuditorIds)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorsForMultipalAssigment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserIDs", DBFactoryParameterType.NVarchar, AuditorIds);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorsForMultipalAssigment(int AuditorIds)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string saveAuditAssignPDFFile(string FilePath = "", string Id = "")
        {
            string Result = "";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveAuditAssignPDFFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.VARCHAR, FilePath);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.VARCHAR, Id);
                Result = objDBAccess.ExecuteCommand().ToString();
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "saveAuditAssignbydeprt(string FunPassValue = '', string Ids = '')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string InsertSocityByExcelSTLoan(SocietyRegistrationEntity obj)
        {
            string Result = "-1";
            string connString = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocityExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyExcel/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        //  dt.Columns.Add("CEOAadharEnc", typeof(System.String));
                        //  dt.Columns.Add("ExcelUploadId", typeof(System.String));
                        try
                        {
                            int i = 0;

                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string DCCB_NAME = Dr["DCCB_NAME"].ToString().Trim();
                                string Total_Application_Received = Dr["Total_Application_Received"].ToString().Trim();
                                string Approved_By_Pacs = Dr["Approved_By_Pacs"].ToString().Trim();
                                string Approved_By_Branch = Dr["Approved_By_Branch"].ToString().Trim();
                                string DMR_Credited_And_Loan = Dr["DMR_Credited_And_Loan"].ToString().Trim();
                                if (Total_Application_Received.Length > 0)
                                {
                                    double myNum = 0;
                                    if (Double.TryParse(Total_Application_Received.ToString(), out myNum))
                                    {
                                        // it is a number
                                    }
                                    else
                                    {
                                        return "Please enter only digit in Total_Application_Received of " + DCCB_NAME + " on line No " + i.ToString();
                                        // it is not a number
                                    }
                                }
                                if (Approved_By_Pacs.Length > 0)
                                {
                                    double myNum = 0;
                                    if (Double.TryParse(Approved_By_Pacs.ToString(), out myNum))
                                    {
                                        // it is a number
                                    }
                                    else
                                    {
                                        return "Please enter only digit in Approved_By_Pacs of " + DCCB_NAME + " on line No " + i.ToString();
                                        // it is not a number
                                    }
                                }
                                if (Approved_By_Branch.Length > 0)
                                {
                                    double myNum = 0;
                                    if (Double.TryParse(Approved_By_Branch.ToString(), out myNum))
                                    {
                                        // it is a number
                                    }
                                    else
                                    {
                                        return "Please enter only digit in Approved_By_Branch of " + DCCB_NAME + " on line No " + i.ToString();
                                        // it is not a number
                                    }
                                }
                                if (DMR_Credited_And_Loan.Length > 0)
                                {
                                    double myNum = 0;
                                    if (Double.TryParse(DMR_Credited_And_Loan.ToString(), out myNum))
                                    {
                                        // it is a number
                                    }
                                    else
                                    {
                                        return "Please enter only digit in DMR_Credited_And_Loan of " + DCCB_NAME + " on line No " + i.ToString();
                                        // it is not a number
                                    }
                                }

                            }
                            string ResultInner = "";

                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                string DCCB_NAME = Dr["DCCB_NAME"].ToString().Trim();
                                string Total_Application_Received = Dr["Total_Application_Received"].ToString().Trim();
                                string Approved_By_Pacs = Dr["Approved_By_Pacs"].ToString().Trim();
                                string Approved_By_Branch = Dr["Approved_By_Branch"].ToString().Trim();
                                string DMR_Credited_And_Loan = Dr["DMR_Credited_And_Loan"].ToString().Trim();
                                if (Total_Application_Received.Length == 0)
                                {
                                    Total_Application_Received = "0";
                                }
                                if (Approved_By_Pacs.Length > 0)
                                {
                                    Approved_By_Pacs = "0";
                                }
                                if (Approved_By_Branch.Length > 0)
                                {
                                    Approved_By_Branch = "0";
                                }
                                if (DMR_Credited_And_Loan.Length > 0)
                                {
                                    DMR_Credited_And_Loan = "0";
                                }

                                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertSTLoanExcelDetailsByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                                cmd1.Parameters.AddWithValue("@DCCB_NAME", DCCB_NAME);
                                cmd1.Parameters.AddWithValue("@Total_Application_Received", Total_Application_Received);
                                cmd1.Parameters.AddWithValue("@Approved_By_Pacs", Approved_By_Pacs);
                                cmd1.Parameters.AddWithValue("@Approved_By_Branch", Approved_By_Branch);
                                cmd1.Parameters.AddWithValue("@DMR_Credited_And_Loan", DMR_Credited_And_Loan);
                                cmd1.Parameters.AddWithValue("@ExcelUploadId", Result);
                                cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());


                            }
                            Result = "Successfully Saved";

                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcelSTLoan(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcelSTLoan(SocietyRegistrationEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string SaveHousingSocietyAudit(SocietyAuditReportEntity obj)
        {
            string Result = "-1";
            string connString = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertHousingSocietyAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@IsSocietyWorking", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, obj.FinancialYear);
                objDBAccess.AddParameterToCommand("@AuditReport", DBFactoryParameterType.NVarchar, obj.AuditReport);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.NVarchar, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, obj.UserType);
                objDBAccess.AddParameterToCommand("@HousingSocietyMember", DBFactoryParameterType.NVarchar, obj.HousingSocietyMember);
                Result = (objDBAccess.ExecuteScalarCommand().ToString());

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SaveHousingSocietyAudit(SocietyAuditReportEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyBasicDetail(ref DataPagingModel Details)
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";


                if (_loggedInUSerDetails.DistrictId > 0 && _loggedInUSerDetails.LevelId == 3)
                    whereCase += " AND SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId;

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "RegistrationDate")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,RegistrationDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }



                if (Details.PageNumber == 1)
                {


                    cmd = objDBAccess.CreateCommand("GetSocietyBasicDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                cmd = objDBAccess.CreateCommand("GetSocietyBasicDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FinancialHealthModel", FunctionName = "DataTable GetSocietyBasicDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

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

        public DataTable RajfedSocietyDetailList()
        {

            dt.Clear();

            DataTable dset = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRajfedSocietyDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetRajfedSocietyDetailList(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public string UpdateCCSociety(string SocietyIDEnc, string StatusId)
        {

            string Result = "-1";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateCCSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                //dt = objDBAccess.GetTable();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdateCCSociety(string SocietyIDEnc, string StatusId)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }


        public string UpdateCCSocietyliquidator(string SocietyIDEnc, string liquidatorName, string liquidatorAppointedDate, string liquidatorPostName, string liquidatorContactNo, long CreateBy, string liquidatorSSOID)
        {
            string Result = "-1";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateCCSocietyliquidator", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@liquidatorName", DBFactoryParameterType.NVarchar, liquidatorName);
                objDBAccess.AddParameterToCommand("@liquidatorAppointedDate", DBFactoryParameterType.NVarchar, Convert.ToDateTime(liquidatorAppointedDate).ToString("MM/dd/yyyy"));
                objDBAccess.AddParameterToCommand("@liquidatorPostName", DBFactoryParameterType.NVarchar, liquidatorPostName);
                objDBAccess.AddParameterToCommand("@liquidatorContactNo", DBFactoryParameterType.NVarchar, liquidatorContactNo);
                objDBAccess.AddParameterToCommand("@liquidatorSSOID", DBFactoryParameterType.NVarchar, liquidatorSSOID);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreateBy);
                //dt = objDBAccess.GetTable();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdateCCSocietyliquidator(string SocietyIDEnc, string liquidatorName, string liquidatorAppointedDate, string liquidatorPostName, string liquidatorContactNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string UpdateCCSocietyReject(string SocietyIDEnc, string RejectionDate, string RejectionFileName, long CreateBy)
        {
            string Result = "-1";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateCCSocietyRejection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@RejectionFileName", DBFactoryParameterType.NVarchar, RejectionFileName);
                objDBAccess.AddParameterToCommand("@RejectionDate", DBFactoryParameterType.NVarchar, Convert.ToDateTime(RejectionDate).ToString("MM/dd/yyyy"));
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreateBy);
                //dt = objDBAccess.GetTable();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdateCCSocietyliquidator(string SocietyIDEnc, string liquidatorName, string liquidatorAppointedDate, string liquidatorPostName, string liquidatorContactNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        #region Credit Cooperative Society Complaint 
        public DataTable GetCCSComplaint_11082020(ref DataPagingModel Details, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";


                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    //else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    //{

                    //    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    //    whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    //}

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetCCSComplaint(ref DataPagingModel Details, " + DistrictId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetCCSComplaint(ref DataPagingModel Details, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        if (x.Value.IndexOf("? string:0 ?") < 0)
                        {
                            whereCase += " and " + x.Key + "='" + x.Value + "'";
                        }
                    }


                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault().IndexOf("? string:0 ?") < 0)
                        {
                            CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        }
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@CCSStatusId", DBFactoryParameterType.NVarchar, CCSStatusId);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@CCSStatusId", DBFactoryParameterType.NVarchar, CCSStatusId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetCCSComplaint(ref DataPagingModel Details, " + DistrictId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetCCSComplaint11(ref DataPagingModel Details, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";


                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    //else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    //{

                    //    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    //    whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    //}

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaint", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetCCSComplaint(ref DataPagingModel Details, " + DistrictId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public string InsertUpdateCCSComplaintLetter_10082020(CCSLetter LetterDetails)
        {
            string Result = "-1";
            try
            {
                //LetterDetails.SocietyDetails[0].complainant_letter
                string fromDate = "";
                if (!string.IsNullOrWhiteSpace(LetterDetails.Date))
                {
                    string date = LetterDetails.Date.Split(' ')[0];
                    fromDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " " + LetterDetails.Date.Split(' ')[1];
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateCCSComplaintLetter", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@complaint_id", DBFactoryParameterType.NVarchar, LetterDetails.ComplaintId);
                objDBAccess.AddParameterToCommand("@letter_date", DBFactoryParameterType.NVarchar, fromDate);
                objDBAccess.AddParameterToCommand("@place", DBFactoryParameterType.NVarchar, LetterDetails.Place);
                objDBAccess.AddParameterToCommand("@ceo_name", DBFactoryParameterType.NVarchar, LetterDetails.CEOName);
                objDBAccess.AddParameterToCommand("@ceo_mobile", DBFactoryParameterType.NVarchar, LetterDetails.CEOMobile);
                objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@complainant_letter", DBFactoryParameterType.NVarchar, LetterDetails.complainant_letter);
                objDBAccess.AddParameterToCommand("@ceo_letter", DBFactoryParameterType.NVarchar, LetterDetails.ceo_letter);
                //dt = objDBAccess.GetTable();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public string InsertUpdateCCSComplaintLetter(" + Newtonsoft.Json.JsonConvert.SerializeObject(LetterDetails) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string InsertUpdateCCSComplaintLetter(SocietyDetailsforAction LetterDetails)
        {
            string Result = "-1";
            try
            {
                string fromDate = "";
                if (!string.IsNullOrWhiteSpace(LetterDetails.Date))
                {
                    string date = LetterDetails.Date.Split(' ')[0];
                    fromDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " ";// LetterDetails.Date.Split(' ')[1];
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateCCSComplaintLetter", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@complaint_id", DBFactoryParameterType.NVarchar, LetterDetails.ComplaintId);
                objDBAccess.AddParameterToCommand("@letter_date", DBFactoryParameterType.NVarchar, fromDate);
                objDBAccess.AddParameterToCommand("@place", DBFactoryParameterType.NVarchar, LetterDetails.Place);
                objDBAccess.AddParameterToCommand("@ceo_name", DBFactoryParameterType.NVarchar, LetterDetails.CEOName);
                objDBAccess.AddParameterToCommand("@ceo_mobile", DBFactoryParameterType.NVarchar, LetterDetails.CEOMobile);
                objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@complainant_letter", DBFactoryParameterType.NVarchar, LetterDetails.complainant_letter);
                objDBAccess.AddParameterToCommand("@ceo_letter", DBFactoryParameterType.NVarchar, LetterDetails.ceo_letter);
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public string InsertUpdateCCSComplaintLetter(" + Newtonsoft.Json.JsonConvert.SerializeObject(LetterDetails) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string saveActiondetail_OLD(SocietyDetailsforAction modal)
        {
            string Result = "0";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertCCSComplaintAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CompainerId", DBFactoryParameterType.NVarchar, modal.complainerId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, modal.actionId);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, modal.Description);
                objDBAccess.AddParameterToCommand("@Place", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@DisposeFile", DBFactoryParameterType.NVarchar, modal.DisposeFile);
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public string InsertUpdateCCSComplaintLetter()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public string saveActiondetail(SocietyDetailsforAction modal)
        {
            string Result = "0";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertCCSComplaintAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CompainerId", DBFactoryParameterType.NVarchar, modal.complainerId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, modal.actionId);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, modal.Description);
                objDBAccess.AddParameterToCommand("@Place", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@DisposeFile", DBFactoryParameterType.NVarchar, modal.DisposeFile);
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public string InsertUpdateCCSComplaintLetter()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        #endregion
        public DataTable GetCCSocietyComplentForPrint(string WhereClause, int LoggedInUserDistrictId = -1)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("CCSConsolidatedReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistID", DBFactoryParameterType.Int, WhereClause);
                objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetCCSocietyComplentForPrint(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCCSNoticIssueCount(int DistrictId, string ActinDate)
        {
            dt.Clear();
            try
            {
                string date = "";
                if (!string.IsNullOrWhiteSpace(ActinDate))
                {
                    ActinDate = ActinDate.Replace("-", "/");
                    date = ActinDate.Split('/')[2] + "/" + ActinDate.Split('/')[1] + "/" + ActinDate.Split('/')[0];
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetCCSNoticIssueCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@ActinDate", DBFactoryParameterType.VARCHAR, date);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.VARCHAR, _LoggedInUser.UserRoleDeptMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetCCSNoticIssueCount(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetResionByDistrict(int DistrictId)
        {
            dt.Clear();
            try
            {


                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetResionByDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetResionByDistrict(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //public DataTable GetCCSData(string WhereClause, int CCSId = -1, int Type = -1)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = objDBAccess.CreateCommand("CCSData", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@DistID", DBFactoryParameterType.Int, WhereClause);
        //        objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
        //        objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.GetCCSData", FunctionName = "public DataTable GetCCSData(string WhereClause)" }, ex);
        //    }

        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}


        public string saveActiondetail_Old(SocietyDetailsforAction modal)
        {
            string Result = "0";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertCCSComplaintAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CompainerId", DBFactoryParameterType.NVarchar, modal.complainerId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, modal.actionId);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, modal.Description);
                objDBAccess.AddParameterToCommand("@Place", DBFactoryParameterType.NVarchar, modal.Place);
                objDBAccess.AddParameterToCommand("@DisposeFile", DBFactoryParameterType.NVarchar, modal.DisposeFile);
                if (modal.DateTime != " ")
                {
                    objDBAccess.AddParameterToCommand("@DateTime", DBFactoryParameterType.DateTime, modal.DateTime.ConvertString_ToValidDate());
                }
                else
                {
                    modal.DateTime = DateTime.MaxValue.ToString();
                    objDBAccess.AddParameterToCommand("@DateTime", DBFactoryParameterType.DateTime, modal.DateTime);
                }
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "public string InsertUpdateCCSComplaintLetter()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable getAllActionList(int compId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetCCSComplaintAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@compId", DBFactoryParameterType.Int, compId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.GetCCSData", FunctionName = "public DataTable GetCCSData(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }




        public string InsertFilePathAndOther(string fullFilePath, string RulesRegulationFile, string ObjectsFile, string SocietyID)
        {
            string Result = "-1";
            int InnerResult = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("InsertFiletrackPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@fullFilePath", DBFactoryParameterType.NVarchar, fullFilePath);
                objDBAccess.AddParameterToCommand("@RulesRegulationFile", DBFactoryParameterType.NVarchar, RulesRegulationFile);
                objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, ObjectsFile);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, int.Parse(SocietyID));
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }


        public int DeleteSocietyRecord(int SocietyID)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSocietyDuplicateData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, SocietyID);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DeleteSocietyBulkUploadFaildRecordList(int RegistrationNo )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable DeleteSocietyRecordList(string RegistrationNo)
        {

            dt.Clear();

            DataTable dset = new DataTable();
            try
            {


                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDetailsBulkUpload", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                dset = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetRajfedSocietyDetailList(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public int GetMaxEMitraAppRequestID(string SocietyIdEnc)
        {
            int Result = 0;
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetMaxEMitraAppRequestID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetMaxEMitraAppRequestID(string SocietyIdEnc)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public bool SubmitSocietyForMap(string SocietyId, string SSOId)
        {

            dt.Clear();

            DataTable dset = new DataTable();
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SetSocietySSOId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdInc", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.ExecuteScalarCommand();
            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SubmitSocietyForMap(" + SocietyId + ",  " + SSOId + ")" }, ex);
                return false;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return true;
        }

        public string InsertSocietyProfileNew(SocietyRegistrationEntity _Model)
        {
            string Result = "-1";
            int InnerResult = 0;
            string SocietyIdEnc = "";
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;

                cmd = objDBAccess.CreateCommand("Insert_Societies_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName.ToUpper().Trim());
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, _Model.SaveStatus);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _Model.IsActive);
                objDBAccess.AddParameterToCommand("@IsSWS", DBFactoryParameterType.Bit, _Model.IsSWS);
                objDBAccess.AddParameterToCommand("@IsAmendmentAtProfiling", DBFactoryParameterType.Bit, false);// _Model.IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, _Model.AmendmentType);
                objDBAccess.AddParameterToCommand("@SocietyCode", DBFactoryParameterType.NVarchar, _Model.SocietyCode ?? "");

                string tempEncAadhar = "NOT";

                //_Model.AADHARNO = "307691961366";

                if (_Model.AADHARNO != null && _Model.AADHARNO.Length == 12)
                    tempEncAadhar = COMMON_FUNCTION.Encrypt(_Model.AADHARNO);



                if (_Model.KIOSKCODE.Length > 0 && System.Web.HttpContext.Current.Session[SessionIdentity.Identity] != null)
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);
                else
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);



                #region Old Executive Committee
                if (_Model.OldExecutiveCommittee != null)
                {
                    if (_Model.OldExecutiveCommittee.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.OldExecutiveCommittee.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.OldExecutiveCommittee.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.OldExecutiveCommittee = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, _Model.OldExecutiveCommittee.Contains("undefined") ? "" : _Model.OldExecutiveCommittee);
                }
                else
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, "");
                #endregion


                #region RegistrationCertificateFile
                if (_Model.RegistrationCertificateFile != null)
                {
                    if (_Model.RegistrationCertificateFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RegistrationCertificateFile.Contains("undefined"))
                    {
                        var Arr = _Model.RegistrationCertificateFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RegistrationCertificateFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile.Contains("undefined") ? "" : _Model.RegistrationCertificateFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ByeLawsSocietyFile
                if (_Model.ByeLawsSocietyFile != null)
                {
                    if (_Model.ByeLawsSocietyFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.ByeLawsSocietyFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ByeLawsSocietyFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ByeLawsSocietyFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile.Contains("undefined") ? "" : _Model.ByeLawsSocietyFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region RulesAndRegulationFile
                if (_Model.RulesAndRegulationFile != null)
                {
                    if (_Model.RulesAndRegulationFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RulesAndRegulationFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.RulesAndRegulationFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RulesAndRegulationFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile.Contains("undefined") ? "" : _Model.RulesAndRegulationFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region BalanceSheetPDF
                if (_Model.BalanceSheetFile != null)
                {
                    if (_Model.BalanceSheetFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.BalanceSheetFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.BalanceSheetFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.BalanceSheetFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, _Model.BalanceSheetFile.Contains("undefined") ? "" : _Model.BalanceSheetFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ProceedingsGeneralMeetingPDF
                if (_Model.GeneralMeetingProceedingFile != null)
                {
                    if (_Model.GeneralMeetingProceedingFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.GeneralMeetingProceedingFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.GeneralMeetingProceedingFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.GeneralMeetingProceedingFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, _Model.GeneralMeetingProceedingFile.Contains("undefined") ? "" : _Model.GeneralMeetingProceedingFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region WorkPlanShowingEconomicPDF
                if (_Model.WorkPlanShowingEconomicViabilityFile != null)
                {
                    if (_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.WorkPlanShowingEconomicViabilityFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.WorkPlanShowingEconomicViabilityFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, _Model.WorkPlanShowingEconomicViabilityFile.Contains("undefined") ? "" : _Model.WorkPlanShowingEconomicViabilityFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region MemberListExcelFile
                if (_Model.MemberListExcelFile != null)
                {
                    if (_Model.MemberListExcelFile.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.MemberListExcelFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.MemberListExcelFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.MemberListExcelFile = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, _Model.MemberListExcelFile.Contains("undefined") ? "" : _Model.MemberListExcelFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region ObjectsFileName
                if (_Model.ObjectsFileName != null)
                {
                    if (_Model.ObjectsFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.ObjectsFileName.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ObjectsFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ObjectsFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, _Model.ObjectsFileName.Contains("undefined") ? "" : _Model.ObjectsFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, _Model.BusinessId);
                objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.Int, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@TotalNumberOfMembers", DBFactoryParameterType.Int, _Model.TotalMembersApplyingForSocociety);
                objDBAccess.AddParameterToCommand("@NoOfMemberInAGM", DBFactoryParameterType.Int, _Model.TotalNumberOfMembersInAGM);
                objDBAccess.AddParameterToCommand("@EntranceFeesCollected", DBFactoryParameterType.Double, _Model.EntranceFeesCollected);
                objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Model.PaidUpShareCapital);
                objDBAccess.AddParameterToCommand("@MemberLiablity", DBFactoryParameterType.Int, _Model.MemberLiablity);
                objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.Double, _Model.SingleShareValue);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Bit, _Model.IsNewRegistration);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN == null ? "" : _Model.BRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                objDBAccess.AddParameterToCommand("@RegistrationNoOld", DBFactoryParameterType.NVarchar, (_Model.OldRegistrationNo == "undefined" || _Model.OldRegistrationNo == "" || _Model.OldRegistrationNo == null) ? "" : _Model.OldRegistrationNo.ToUpper().Trim().Replace(" ", ""));


                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@NoOfBranches", DBFactoryParameterType.Int, _Model.NoOfBranches);
                objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.ElectionDate).ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.ElectionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                //FOR SPORT 
                objDBAccess.AddParameterToCommand("@IsRSSC", DBFactoryParameterType.NVarchar, _Model.IsStatLevel);
                objDBAccess.AddParameterToCommand("@AgmDate", DBFactoryParameterType.Date, _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());//_Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@SportsAssociationId", DBFactoryParameterType.Int, _Model.AssociationId);

                objDBAccess.AddParameterToCommand("@IsAffiliatedByRSSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByRSSC);
                objDBAccess.AddParameterToCommand("@RSSCRegNo", DBFactoryParameterType.NVarchar, _Model.RSSCRegNo == "undefined" ? "" : _Model.RSSCRegNo);
                objDBAccess.AddParameterToCommand("@RSSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByRSSC == false ? DateTime.MaxValue : _Model.RSSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region RSSCFileName
                if (_Model.RSSCFileName != null)
                {
                    if (_Model.RSSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RSSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.RSSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RSSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, _Model.RSSCFileName == "undefined" ? "" : _Model.RSSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliatedBySOC", DBFactoryParameterType.Bit, _Model.IsAffiliatedBySOC);
                objDBAccess.AddParameterToCommand("@SOCRegNo", DBFactoryParameterType.NVarchar, _Model.SOCRegNo == "undefined" ? "" : _Model.SOCRegNo);
                objDBAccess.AddParameterToCommand("@SOCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedBySOC == false ? DateTime.MaxValue : _Model.SOCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                #region SOCFileName
                if (_Model.SOCFileName != null)
                {
                    if (_Model.SOCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SOCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SOCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SOCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, _Model.SOCFileName == "undefined" ? "" : _Model.SOCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion


                objDBAccess.AddParameterToCommand("@IsAffiliatedByDLSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByDLSC);
                objDBAccess.AddParameterToCommand("@DLSCRegNo", DBFactoryParameterType.NVarchar, _Model.DLSCRegNo);
                objDBAccess.AddParameterToCommand("@DLSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByDLSC == false ? DateTime.MaxValue : _Model.DLSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region DLSCFileName
                if (_Model.DLSCFileName != null)
                {
                    if (_Model.DLSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.DLSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.DLSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.DLSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, _Model.DLSCFileName == "undefined" ? "" : _Model.DLSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliationBySLSA", DBFactoryParameterType.Bit, _Model.IsAffiliationBySLSA);
                objDBAccess.AddParameterToCommand("@SLSARegNo", DBFactoryParameterType.NVarchar, _Model.SLSARegNo);
                objDBAccess.AddParameterToCommand("@SLSAAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliationBySLSA == false ? DateTime.MaxValue : _Model.SLSAAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region SLSAFileName
                if (_Model.SLSAFileName != null)
                {
                    if (_Model.SLSAFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SLSAFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SLSAFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SLSAFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, _Model.SLSAFileName == "undefined" ? "" : _Model.SLSAFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, "");
                #endregion



                if (_Model.KIOSKCODE.Length > 0)
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, _Model.ServicesId);
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 5530);
                }

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                var RES = Result.Split('@');
                int R = -1;
                if (RES.Length > 0)
                {
                    SocietyIdEnc = Result.Split('@')[0];

                    R = int.Parse(Result.Split('@')[1]);
                }

                _Model.AmendmentDocuments = _Model.AmendmentDocuments ?? new List<AmendmentDocuments>();

                if (_Model.IsAmendment != null && _Model.AmendmentDocuments.Count > 0)
                {
                    _Model.AmendmentDocuments.ForEach(doc =>
                    {
                        if (doc.DocumentName != null && doc.DocumentName != "")
                        {
                            objDBAccess.OpenConnection();
                            cmd = new SqlCommand();
                            SqlCommand cmdAddress = new SqlCommand();

                            cmd = objDBAccess.CreateCommand("InsertSocietyAmendmentDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                            objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, 0);
                            objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.NVarchar, doc.DocumentType);
                            objDBAccess.AddParameterToCommand("@DocumentName", DBFactoryParameterType.NVarchar, doc.DocumentName.IndexOf("/") > -1 ? doc.DocumentName.Split('/')[4] : doc.DocumentName);
                            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _Model.UserId);
                            cmd.ExecuteScalar();
                        }
                    });
                }

                if (Result.ToString().Length > 10 && Result != "-5" && R > 0)
                {
                    int FlagFirstRow = 1;
                    foreach (SocietyWorkScopeEntity obj in _Model.lstSocietyWorkScope)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertWorkScope_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, Result.ToString().Contains("@") ? int.Parse(Result.Split('@')[1]) : 0);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Result.Split('@')[0]);
                        objDBAccess.AddParameterToCommand("@IsStatLevel", DBFactoryParameterType.Int, obj.IsStatLevel);
                        objDBAccess.AddParameterToCommand("@WorkScopeLevel_ID", DBFactoryParameterType.Int, obj.WorkScopeLevel_ID);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Int, obj.CITY_ID > 0 ? 0 : 1);
                        objDBAccess.AddParameterToCommand("@DirectorName", DBFactoryParameterType.NVarchar, obj.DirectorName);
                        objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                        objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.Int, obj.AssociationId);
                        objDBAccess.AddParameterToCommand("@FlagFirstRow", DBFactoryParameterType.Int, FlagFirstRow);
                        objDBAccess.AddParameterToCommand("@SportsAssociationName", DBFactoryParameterType.NVarchar, obj.SportsAssociationName);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                        objDBAccess.AddParameterToCommand("@AreaOfOperation", DBFactoryParameterType.NVarchar, _Model.AreaOfOperation);

                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                    }
                }
                else
                {
                    Result = "-1";
                }

                foreach (AdministrativeData obj in _Model.AdministrativeDataList)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0 || (obj.MEMBERTYPE == "ADMINISTRATIVE" || obj.MEMBERTYPE == "LIQUIDATOR"))
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                        objDBAccess.OpenConnection();

                        SqlCommand cmdAdministrative = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                        objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, obj.FatherName);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, (_Model.ClassA == null || _Model.ClassA.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassA));
                        objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, (_Model.ClassB == null || _Model.ClassB.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassB));
                        objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, (_Model.ClassC == null || _Model.ClassC.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassC));
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                        objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                        objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                        objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);
                        objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, obj.BoardType);
                        objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                        objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.Int, obj.ShareAllotted);
                        objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.Money, obj.ShareCapitalAmount);
                        objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.Int, obj.loaneeType);
                        objDBAccess.AddParameterToCommand("SubmitStatus", DBFactoryParameterType.Int, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserTypeCK", DBFactoryParameterType.NVarchar, _Model.UserType);

                        if (_Model.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _Model.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }

                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                            R = int.Parse(Result.Split('@')[1]);

                    }
                    i++;
                }

                //ADDRESS
                string AddressResult = "-1";
                int EMitraTransactionId = 0;
                int SubmitStatus = _Model.SocietyRegisteredOfficeDetailsLst[0].SubmitStatus;
                try
                {
                    objDBAccess.OpenConnection();
                    foreach (SocietyRegisteredOfficeDetailsEntity obj in _Model.SocietyRegisteredOfficeDetailsLst)
                    {
                        SqlCommand cmdAddress = new SqlCommand();

                        cmdAddress = objDBAccess.CreateCommand("InsertSocietyRegOfficeDetail_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@SocietyRegOfficeDetailId", DBFactoryParameterType.BigInt, obj.SocietyRegOfficeDetailId);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, obj.IsRural);
                        objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, obj.HouseBuildingNo);
                        objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, obj.LaneStreetName);
                        objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, obj.Locality);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, obj.PinCode);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, obj.UpdatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, obj.CreatedIP);
                        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, obj.UpdatedIP);
                        objDBAccess.AddParameterToCommand("@IsSameCEOAddress", DBFactoryParameterType.Bit, obj.IsSameCEOAddress);
                        objDBAccess.AddParameterToCommand("@AddressFor", DBFactoryParameterType.Int, obj.AddressFor);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, obj.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSO_ID);
                        objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.NVarchar, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, obj.UserType);
                        if (obj.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, obj.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                            EMitraTransactionId = int.Parse(Result.Split('@')[1]);
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegisteredOfficeDetailsInsert(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
                //END


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

        public string InsertSocietyProfileNew_v1(SocietyRegistrationEntity _Model)
        {
            string Result = "-1";
            int InnerResult = 0;
            string SocietyIdEnc = "";
            int i = 0;
            int step = 0;
            string query = "";
            try
            {
                //(new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = fileName, Isactive = true, SocietyId = SocietyId, DocumentName = FileName });
                Dictionary<string, string> SocietyDocuments = new Dictionary<string, string>();
                UserDetailsEntity1 dtls2 = new UserDetailsEntity1();
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;

                cmd = objDBAccess.CreateCommand("Insert_Societies_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName.ToUpper().Trim());
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, _Model.SaveStatus);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _Model.IsActive);
                objDBAccess.AddParameterToCommand("@IsSWS", DBFactoryParameterType.Bit, _Model.IsSWS);
                objDBAccess.AddParameterToCommand("@IsAmendmentAtProfiling", DBFactoryParameterType.Bit, false);// _Model.IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, _Model.AmendmentType);
                objDBAccess.AddParameterToCommand("@SocietyCode", DBFactoryParameterType.NVarchar, _Model.SocietyCode ?? "");


                objDBAccess.AddParameterToCommand("@SecurityDeposit", DBFactoryParameterType.NVarchar, _Model.SecurityDeposit ?? 0.0);
                objDBAccess.AddParameterToCommand("@WorkingCapital", DBFactoryParameterType.NVarchar, _Model.WorkingCapital ?? 0.0);
                objDBAccess.AddParameterToCommand("@LoanAmt", DBFactoryParameterType.NVarchar, _Model.LoanAmt ?? 0.0);
                //objDBAccess.AddParameterToCommand("@FileNo",  DBFactoryParameterType.NVarchar, _Model.FileNo ?? "");
                //objDBAccess.AddParameterToCommand("@SanctionDate", DBFactoryParameterType.Date, _Model.SanctionDate);
                //objDBAccess.AddParameterToCommand("@SanctionDate", DBFactoryParameterType.Date, _Model.IsNewRegistration == true ? DateTime.MaxValue : _Model.SanctionDate ? DateTime.MaxValue : _Model.SanctionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());


                string tempEncAadhar = "NOT";

                //_Model.AADHARNO = "307691961366";

                if (_Model.AADHARNO != null && _Model.AADHARNO.Length == 12)
                    tempEncAadhar = COMMON_FUNCTION.Encrypt(_Model.AADHARNO);



                if (_Model.KIOSKCODE.Length > 0 && System.Web.HttpContext.Current.Session[SessionIdentity.Identity] != null)
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);
                else
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);



                #region Old Executive Committee
                if (_Model.OldExecutiveCommittee != null)
                {
                    if (_Model.OldExecutiveCommittee.ToString().Contains("/") && !_Model.OldExecutiveCommittee.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.OldExecutiveCommittee.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.OldExecutiveCommittee = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Old Executive Committee", _Model.OldExecutiveCommittee);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, _Model.OldExecutiveCommittee.Contains("undefined") ? "" : _Model.OldExecutiveCommittee);
                }
                else
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, "");
                #endregion


                #region RegistrationCertificateFile
                if (_Model.RegistrationCertificateFile != null)
                {
                    if (_Model.RegistrationCertificateFile.ToString().Contains("/") && !_Model.RegistrationCertificateFile.Contains("undefined"))
                    {
                        var Arr = _Model.RegistrationCertificateFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RegistrationCertificateFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Registration CertificateFile", _Model.RegistrationCertificateFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile.Contains("undefined") ? "" : _Model.RegistrationCertificateFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ByeLawsSocietyFile
                if (_Model.ByeLawsSocietyFile != null)
                {
                    if (_Model.ByeLawsSocietyFile.ToString().Contains("/") && !_Model.ByeLawsSocietyFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ByeLawsSocietyFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ByeLawsSocietyFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Society Bye Laws", _Model.ByeLawsSocietyFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile.Contains("undefined") ? "" : _Model.ByeLawsSocietyFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                //#region SanctionDocumentFile
                //if (_Model.SanctionDocument != null)
                //{
                //    if (_Model.SanctionDocument.ToString().Contains("/") && !_Model.SanctionDocument.ToString().Contains("undefined"))
                //    {
                //        var Arr = _Model.SanctionDocument.ToString().Split('/');
                //        if (Arr.Length > 1)
                //        {
                //            _Model.SanctionDocument = Arr[Arr.Length - 1].ToString();
                //            SocietyDocuments.Add("Sanction Document", _Model.SanctionDocument);
                //        }
                //    }
                //    objDBAccess.AddParameterToCommand("@SanctionDocument", DBFactoryParameterType.NVarchar, _Model.SanctionDocument.Contains("undefined") ? "" : _Model.SanctionDocument);
                //}
                //else
                //    objDBAccess.AddParameterToCommand("@SanctionDocument", DBFactoryParameterType.NVarchar, "");
                //#endregion

                #region RulesAndRegulationFile
                if (_Model.RulesAndRegulationFile != null)
                {
                    if (_Model.RulesAndRegulationFile.ToString().Contains("/") && !_Model.RulesAndRegulationFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.RulesAndRegulationFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RulesAndRegulationFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Rules And Regulation", _Model.RulesAndRegulationFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile.Contains("undefined") ? "" : _Model.RulesAndRegulationFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region BalanceSheetPDF
                if (_Model.BalanceSheetFile != null)
                {
                    if (_Model.BalanceSheetFile.ToString().Contains("/") && !_Model.BalanceSheetFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.BalanceSheetFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.BalanceSheetFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Balance Sheet", _Model.BalanceSheetFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, _Model.BalanceSheetFile.Contains("undefined") ? "" : _Model.BalanceSheetFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ProceedingsGeneralMeetingPDF
                if (_Model.GeneralMeetingProceedingFile != null)
                {
                    if (_Model.GeneralMeetingProceedingFile.ToString().Contains("/") && !_Model.GeneralMeetingProceedingFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.GeneralMeetingProceedingFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.GeneralMeetingProceedingFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Proceedings of the General Meeting Held", _Model.GeneralMeetingProceedingFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, _Model.GeneralMeetingProceedingFile.Contains("undefined") ? "" : _Model.GeneralMeetingProceedingFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region WorkPlanShowingEconomicPDF
                if (_Model.WorkPlanShowingEconomicViabilityFile != null)
                {
                    if (_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("/") && !_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.WorkPlanShowingEconomicViabilityFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.WorkPlanShowingEconomicViabilityFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Work Plan Showing Economic Viability of the Society", _Model.WorkPlanShowingEconomicViabilityFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, _Model.WorkPlanShowingEconomicViabilityFile.Contains("undefined") ? "" : _Model.WorkPlanShowingEconomicViabilityFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region MemberListExcelFile
                if (_Model.MemberListExcelFile != null)
                {
                    if (_Model.MemberListExcelFile.ToString().Contains("/") && !_Model.MemberListExcelFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.MemberListExcelFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.MemberListExcelFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("List of Members of the Society (EXCEL)", _Model.MemberListExcelFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, _Model.MemberListExcelFile.Contains("undefined") ? "" : _Model.MemberListExcelFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region ObjectsFileName
                if (_Model.ObjectsFileName != null)
                {
                    if (_Model.ObjectsFileName.ToString().Contains("/") && !_Model.ObjectsFileName.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ObjectsFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ObjectsFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Society Objects", _Model.ObjectsFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, _Model.ObjectsFileName.Contains("undefined") ? "" : _Model.ObjectsFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region AffidavitFile
                //Added by Pradeep 20250523
                if (_Model.AffidavitFile != null)
                {
                    if (_Model.AffidavitFile.ToString().Contains("/") && !_Model.AffidavitFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.AffidavitFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.AffidavitFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Affidavit", _Model.AffidavitFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@AffidavitFile", DBFactoryParameterType.NVarchar, _Model.AffidavitFile.Contains("undefined") ? "" : _Model.AffidavitFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@AffidavitFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, _Model.BusinessId);
                objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.Int, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@TotalNumberOfMembers", DBFactoryParameterType.Int, _Model.TotalMembersApplyingForSocociety);
                objDBAccess.AddParameterToCommand("@NoOfMemberInAGM", DBFactoryParameterType.Int, _Model.TotalNumberOfMembersInAGM);
                objDBAccess.AddParameterToCommand("@EntranceFeesCollected", DBFactoryParameterType.Double, _Model.EntranceFeesCollected);
                objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Model.PaidUpShareCapital);
                objDBAccess.AddParameterToCommand("@MemberLiablity", DBFactoryParameterType.Int, _Model.MemberLiablity);
                objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.Double, _Model.SingleShareValue);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Bit, _Model.IsNewRegistration);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN == null ? "" : _Model.BRN);
                objDBAccess.AddParameterToCommand("@IsValidateBRN", DBFactoryParameterType.Bit, _Model.IsValidateBRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                objDBAccess.AddParameterToCommand("@RegistrationNoOld", DBFactoryParameterType.NVarchar, (_Model.OldRegistrationNo == "undefined" || _Model.OldRegistrationNo == "" || _Model.OldRegistrationNo == null) ? "" : _Model.OldRegistrationNo.ToUpper().Trim().Replace(" ", ""));


                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@NoOfBranches", DBFactoryParameterType.Int, _Model.NoOfBranches);
                objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.ElectionDate).ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.ElectionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                //FOR SPORT 
                objDBAccess.AddParameterToCommand("@IsRSSC", DBFactoryParameterType.NVarchar, _Model.IsStatLevel);
                objDBAccess.AddParameterToCommand("@AgmDate", DBFactoryParameterType.Date, _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());//_Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@SportsAssociationId", DBFactoryParameterType.Int, _Model.AssociationId);

                objDBAccess.AddParameterToCommand("@IsAffiliatedByRSSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByRSSC);
                objDBAccess.AddParameterToCommand("@RSSCRegNo", DBFactoryParameterType.NVarchar, _Model.RSSCRegNo == "undefined" ? "" : _Model.RSSCRegNo);
                objDBAccess.AddParameterToCommand("@RSSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByRSSC == false ? DateTime.MaxValue : _Model.RSSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region RSSCFileName
                if (_Model.RSSCFileName != null)
                {
                    if (_Model.RSSCFileName.ToString().Contains("/") && !_Model.RSSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.RSSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RSSCFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Certificate Of Affiliation To The State Level Sports Council", _Model.RSSCFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, _Model.RSSCFileName == "undefined" ? "" : _Model.RSSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliatedBySOC", DBFactoryParameterType.Bit, _Model.IsAffiliatedBySOC);
                objDBAccess.AddParameterToCommand("@SOCRegNo", DBFactoryParameterType.NVarchar, _Model.SOCRegNo == "undefined" ? "" : _Model.SOCRegNo);
                objDBAccess.AddParameterToCommand("@SOCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedBySOC == false ? DateTime.MaxValue : _Model.SOCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                #region SOCFileName
                if (_Model.SOCFileName != null)
                {
                    if (_Model.SOCFileName.ToString().Contains("/") && !_Model.SOCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SOCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SOCFileName = Arr[Arr.Length - 1].ToString();
                            if (_Model.IsStatLevel)
                            {
                                SocietyDocuments.Add("Certificate Of Affiliation To The State Level Sports Council", _Model.SOCFileName);
                            }
                            else
                            {
                                SocietyDocuments.Add("Certificate Of Affiliation To The District Olympic Council", _Model.SOCFileName);
                            }
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, _Model.SOCFileName == "undefined" ? "" : _Model.SOCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion


                objDBAccess.AddParameterToCommand("@IsAffiliatedByDLSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByDLSC);
                objDBAccess.AddParameterToCommand("@DLSCRegNo", DBFactoryParameterType.NVarchar, _Model.DLSCRegNo);
                objDBAccess.AddParameterToCommand("@DLSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByDLSC == false ? DateTime.MaxValue : _Model.DLSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region DLSCFileName
                if (_Model.DLSCFileName != null)
                {
                    if (_Model.DLSCFileName.ToString().Contains("/") && !_Model.DLSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.DLSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.DLSCFileName = Arr[Arr.Length - 1].ToString();
                            if (_Model.ActId == 7)
                                SocietyDocuments.Add("Certificate Of Affiliation To The District Sports Council", _Model.DLSCFileName);

                            else
                            {
                                SocietyDocuments.Add("Details Of Your Member Cooperative Societies", _Model.DLSCFileName);
                            }
                        }
                    }
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, _Model.DLSCFileName == "undefined" ? "" : _Model.DLSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliationBySLSA", DBFactoryParameterType.Bit, _Model.IsAffiliationBySLSA);
                objDBAccess.AddParameterToCommand("@SLSARegNo", DBFactoryParameterType.NVarchar, _Model.SLSARegNo);
                objDBAccess.AddParameterToCommand("@SLSAAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliationBySLSA == false ? DateTime.MaxValue : _Model.SLSAAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region SLSAFileName
                if (_Model.SLSAFileName != null)
                {
                    if (_Model.SLSAFileName.ToString().Contains("/") && !_Model.SLSAFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SLSAFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SLSAFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Certificates Of Affiliation To The State Level Sports Association", _Model.SLSAFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, _Model.SLSAFileName == "undefined" ? "" : _Model.SLSAFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, "");
                #endregion



                if (_Model.KIOSKCODE.Length > 0)
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, _Model.ServicesId);
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 5530);
                }
                step = 0;
                query += "step 0:" + objDBAccess.getSqlCommandText();
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                if (Result == "-5")
                {
                    return Result;
                }

                if (_Model.SaveStatus == 1)
                {
                    try
                    {
                        UpdateDistrictEntity obk = new UpdateDistrictEntity();

                        step = 1;

                        dtls2 = COMMON_FUNCTION.GetUserDetailsBySSOId(_Model.SSOID);
                        string PostName = "";
                        string ApplicantName = "";
                        string AADHAARNo = AADHARSerice.GetAADHARNoBySSOId(_Model.SSOID);
                        if (_Model.AdministrativeDataList != null && _Model.AdministrativeDataList.Count > 0 && _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).Any())
                        {
                            PostName = _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).FirstOrDefault().PostName;
                            ApplicantName = _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).FirstOrDefault().Name;
                            SaveSocietyApplicatnDetails(_Model.SocietyIDEnc, _Model.SSOID, ApplicantName, PostName, "Registration", "0");
                        }
                        else
                        {
                            SaveSocietyApplicatnDetails(_Model.SocietyIDEnc, _Model.SSOID, dtls2.DisplayName, "", "Registration", "0");
                        }

                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " aa SaveSocietyApplicatnDetails(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

                    }

                }

                var RES = Result.Split('@');
                int R = -1;
                if (RES.Length > 0 && Result != "-5")
                {
                    step = 2;
                    SocietyIdEnc = RES[0];
                    step = 3;
                    if (RES.Length > 1)
                        R = int.Parse(RES[1]);
                }

                foreach (var data in SocietyDocuments)
                {
                    (new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = data.Value, Isactive = true, SocietyId = 0, DocumentType = "", DocumentName = data.Key }, SocietyIdEnc);
                }


                //_Model.AmendmentDocuments = _Model.AmendmentDocuments ?? new List<AmendmentDocuments>();

                //if (_Model.IsAmendment != null && _Model.AmendmentDocuments.Count > 0)
                //{
                //    _Model.AmendmentDocuments.ForEach(doc =>
                //    {
                //        if (doc.DocumentName != null && doc.DocumentName != "")
                //        {
                //            objDBAccess.OpenConnection();
                //            cmd = new SqlCommand();
                //            SqlCommand cmdAddress = new SqlCommand();
                //            step = 4;
                //            cmd = objDBAccess.CreateCommand("InsertSocietyAmendmentDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                //            objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                //            objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, 0);
                //            objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.NVarchar, doc.DocumentType);
                //            objDBAccess.AddParameterToCommand("@DocumentName", DBFactoryParameterType.NVarchar, doc.DocumentName.IndexOf("/") > -1 ? doc.DocumentName.Split('/')[4] : doc.DocumentName);
                //            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _Model.UserId);
                //            cmd.ExecuteScalar();
                //        }
                //    });
                //}

                if (Result.ToString().Length > 10 && Result != "-5" && R > 0)
                {
                    int FlagFirstRow = 1;

                    string WorkScopeIds = string.Join(",", _Model.lstSocietyWorkScope.Select(x => x.WorkScopeId));
                    foreach (SocietyWorkScopeEntity obj in _Model.lstSocietyWorkScope)
                    {
                        objDBAccess.OpenConnection();
                        step = 6;
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertWorkScope_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, Result.ToString().Contains("@") ? int.Parse(Result.Split('@')[1]) : 0);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Result.Split('@')[0]);
                        objDBAccess.AddParameterToCommand("@IsStatLevel", DBFactoryParameterType.Int, obj.IsStatLevel);
                        objDBAccess.AddParameterToCommand("@WorkScopeLevel_ID", DBFactoryParameterType.Int, obj.WorkScopeLevel_ID);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Int, obj.CITY_ID > 0 ? 0 : 1);
                        objDBAccess.AddParameterToCommand("@DirectorName", DBFactoryParameterType.NVarchar, obj.DirectorName);
                        objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                        objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.Int, obj.AssociationId);
                        objDBAccess.AddParameterToCommand("@FlagFirstRow", DBFactoryParameterType.Int, FlagFirstRow);
                        objDBAccess.AddParameterToCommand("@SportsAssociationName", DBFactoryParameterType.NVarchar, obj.SportsAssociationName);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                        objDBAccess.AddParameterToCommand("@AreaOfOperation", DBFactoryParameterType.NVarchar, _Model.AreaOfOperation);
                        objDBAccess.AddParameterToCommand("@WorkScopeId", DBFactoryParameterType.NVarchar, obj.WorkScopeId);
                        objDBAccess.AddParameterToCommand("@WorkScopeIds", DBFactoryParameterType.NVarchar, WorkScopeIds);

                        query += "step 6:" + objDBAccess.getSqlCommandText();
                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                        WorkScopeIds += "," + InnerResult;

                    }
                }
                else
                {
                    Result = "-1";
                }

                string Ids = string.Join(",", _Model.AdministrativeDataList.Select(x => x.AdministrativeId));
                foreach (AdministrativeData obj in _Model.AdministrativeDataList)
                {
                    string qry1 = "";
                    try
                    {
                        step = 7;
                        if (obj.Photo != null && obj.Photo != "")
                        {
                            if (obj.Photo.ToString().Contains("SocietyProfiling"))
                            {
                                var temp = obj.Photo.ToString().Split('/');
                                obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                            }
                            if (obj.Photo.ToString().Contains("NewRegistration"))
                            {
                                var temp = obj.Photo.ToString().Split('/');
                                obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                            }
                            if (obj.Photo.ToString().Contains("Balnk"))
                            {
                                obj.Photo = "";
                            }
                        }
                        objDBAccess.OpenConnection();
                        //Ram??????_______________________________________________________________________________________________________________________
                        if (obj.Name?.Length > 0 && obj.CategoryId > 0 && SocietyIdEnc != "" || (obj.MEMBERTYPE == "ADMINISTRATIVE" || obj.MEMBERTYPE == "LIQUIDATOR"))
                        {
                            step = 8;
                            obj.SocietyIdEnc = SocietyIdEnc;
                            //var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                            //  var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : SqlDateTime.MaxValue;
                            var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : new DateTime(1900, 1, 1);// (DateTime)SqlDateTime.MaxValue; 
                            if (DOBConvert < (DateTime)SqlDateTime.MinValue || DOBConvert > (DateTime)SqlDateTime.MaxValue)
                            {
                                DOBConvert = (DateTime)SqlDateTime.MaxValue;
                            }
                            // var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                            var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : (DateTime)SqlDateTime.MaxValue);
                            if (DOJConvert < (DateTime)SqlDateTime.MinValue || DOJConvert > (DateTime)SqlDateTime.MaxValue)
                            {
                                DOJConvert = (DateTime)SqlDateTime.MaxValue;
                            }
                            //objDBAccess.OpenConnection();

                            SqlCommand cmdAdministrative = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                            objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                            objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                            //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                            //Ram
                            objDBAccess.AddParameterToCommand("@NameOfSociety", DBFactoryParameterType.NVarchar, obj.NameOfSociety);
                            objDBAccess.AddParameterToCommand("@PostinSociety", DBFactoryParameterType.NVarchar, obj.PostinSociety);

                            objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                            objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, obj.FatherName);
                            objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                            objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                            // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                            objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                            objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                            objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                            objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, (_Model.ClassA == null || _Model.ClassA.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassA));
                            objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, (_Model.ClassB == null || _Model.ClassB.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassB));
                            objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, (_Model.ClassC == null || _Model.ClassC.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassC));
                            objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                            objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                            objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                            objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                            objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                            objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                            objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                            objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                            objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                            objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                            objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                            objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                            objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                            objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                            objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                            objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);
                            objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, obj.BoardType);
                            objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                            objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.Int, obj.ShareAllotted);
                            objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.Money, obj.ShareCapitalAmount);
                            objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.Int, obj.loaneeType);
                            objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.Int, obj.SubmitStatus);
                            objDBAccess.AddParameterToCommand("@UserTypeCK", DBFactoryParameterType.NVarchar, _Model.UserType);
                            objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                            objDBAccess.AddParameterToCommand("@AadhrCopy", DBFactoryParameterType.NVarchar, obj.AadhrCopy);
                            objDBAccess.AddParameterToCommand("@IsJanAadhar", DBFactoryParameterType.Int, obj.IsJanAadhar);
                            if (_Model.KIOSKCODE.Length > 0)
                            {
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _Model.ServiceId);
                            }
                            else
                            {
                                if (obj.SocietyIdEnc.Length > 0)
                                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                            }
                            qry1 += "step 8:" + objDBAccess.getSqlCommandText();
                            //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                            Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                            if ((Result.Split('@').Length > 1))
                            {
                                R = int.Parse(Result.Split('@')[1]);
                                Ids += "," + Result.Split('@')[2];
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int InsertSocietyProfileNew_v1(SocietyRegistrationEntity _Model) AdministrativeDataList" + qry1 }, e);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    i++;
                }

                //ADDRESS
                string AddressResult = "-1";
                int EMitraTransactionId = 0;
                int SubmitStatus = _Model.SocietyRegisteredOfficeDetailsLst[0].SubmitStatus;
                try
                {
                    objDBAccess.OpenConnection();
                    foreach (SocietyRegisteredOfficeDetailsEntity obj in _Model.SocietyRegisteredOfficeDetailsLst)
                    {
                        obj.SocietyIdEnc = SocietyIdEnc;
                        SqlCommand cmdAddress = new SqlCommand();
                        step = 9;
                        cmdAddress = objDBAccess.CreateCommand("InsertSocietyRegOfficeDetail_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@SocietyRegOfficeDetailId", DBFactoryParameterType.BigInt, obj.SocietyRegOfficeDetailId);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, obj.IsRural);
                        objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, obj.HouseBuildingNo);
                        objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, obj.LaneStreetName);
                        objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, obj.Locality);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, obj.PinCode);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, obj.UpdatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, obj.CreatedIP);
                        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, obj.UpdatedIP);
                        objDBAccess.AddParameterToCommand("@IsSameCEOAddress", DBFactoryParameterType.Bit, obj.IsSameCEOAddress);
                        objDBAccess.AddParameterToCommand("@AddressFor", DBFactoryParameterType.Int, obj.AddressFor);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, obj.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSO_ID);
                        objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.NVarchar, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, obj.UserType);
                        if (obj.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, obj.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }
                        query += "step 9:" + objDBAccess.getSqlCommandText();
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                            EMitraTransactionId = int.Parse(Result.Split('@')[1]);
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegisteredOfficeDetailsInsert(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
                //END


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity step " + step + "; {" + JsonConvert.SerializeObject(_Model) + "})" + query }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }


        public string InsertOldSocietyProfilling(SocietyRegistrationEntity _Model)
        {
            string Result = "-1";
            int InnerResult = 0;
            string SocietyIdEnc = "";
            int i = 0;
            int step = 0;


            try
            {
                //(new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = fileName, Isactive = true, SocietyId = SocietyId, DocumentName = FileName });
                Dictionary<string, string> SocietyDocuments = new Dictionary<string, string>();
                UserDetailsEntity1 dtls2 = new UserDetailsEntity1();
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;


                cmd = objDBAccess.CreateCommand("Insert_Societies_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName.ToUpper().Trim());
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, _Model.SaveStatus);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _Model.IsActive);
                objDBAccess.AddParameterToCommand("@IsSWS", DBFactoryParameterType.Bit, _Model.IsSWS);
                objDBAccess.AddParameterToCommand("@IsAmendmentAtProfiling", DBFactoryParameterType.Bit, false);// _Model.IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, _Model.AmendmentType);
                objDBAccess.AddParameterToCommand("@SocietyCode", DBFactoryParameterType.NVarchar, _Model.SocietyCode ?? "");
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, _Model.FunctionalStatusId); //22July2025 
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, _Model.FinancialYearId);
                objDBAccess.AddParameterToCommand("@SocietyProfilingStatusId", DBFactoryParameterType.Int, _Model.SocietyProfilingStatusId);
                objDBAccess.AddParameterToCommand("@IsEditSocietyProfiling2001", DBFactoryParameterType.Int, _Model.IsEditSocietyProfiling2001);
                //objDBAccess.AddParameterToCommand("@RejectFileSocietyProfilingPath", DBFactoryParameterType.NVarchar, _Model.RejectFileSocietyProfilingPath);
                //objDBAccess.AddParameterToCommand("@RejectSocietyProfilingDate", DBFactoryParameterType.VARCHAR, _Model.RejectSocietyProfilingDate.ToString("yyyy-MM-dd"));


                string tempEncAadhar = "NOT";

                //_Model.AADHARNO = "307691961366";

                if (_Model.AADHARNO != null && _Model.AADHARNO.Length == 12)
                    tempEncAadhar = COMMON_FUNCTION.Encrypt(_Model.AADHARNO);



                if (_Model.KIOSKCODE.Length > 0 && System.Web.HttpContext.Current.Session[SessionIdentity.Identity] != null)
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);
                else
                    objDBAccess.AddParameterToCommand("@AADHARNO", DBFactoryParameterType.NVarchar, tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);



                #region Old Executive Committee
                if (_Model.OldExecutiveCommittee != null)
                {
                    if (_Model.OldExecutiveCommittee.ToString().Contains("/") && !_Model.OldExecutiveCommittee.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.OldExecutiveCommittee.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.OldExecutiveCommittee = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Old Executive Committee", _Model.OldExecutiveCommittee);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, _Model.OldExecutiveCommittee.Contains("undefined") ? "" : _Model.OldExecutiveCommittee);
                }
                else
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, "");
                #endregion


                #region RegistrationCertificateFile
                if (_Model.RegistrationCertificateFile != null)
                {
                    if (_Model.RegistrationCertificateFile.ToString().Contains("/") && !_Model.RegistrationCertificateFile.Contains("undefined"))
                    {
                        var Arr = _Model.RegistrationCertificateFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RegistrationCertificateFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Registration CertificateFile", _Model.RegistrationCertificateFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile.Contains("undefined") ? "" : _Model.RegistrationCertificateFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ByeLawsSocietyFile
                if (_Model.ByeLawsSocietyFile != null)
                {
                    if (_Model.ByeLawsSocietyFile.ToString().Contains("/") && !_Model.ByeLawsSocietyFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ByeLawsSocietyFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ByeLawsSocietyFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Society Bye Laws", _Model.ByeLawsSocietyFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile.Contains("undefined") ? "" : _Model.ByeLawsSocietyFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region RulesAndRegulationFile
                if (_Model.RulesAndRegulationFile != null)
                {
                    if (_Model.RulesAndRegulationFile.ToString().Contains("/") && !_Model.RulesAndRegulationFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.RulesAndRegulationFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RulesAndRegulationFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Rules And Regulation", _Model.RulesAndRegulationFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile.Contains("undefined") ? "" : _Model.RulesAndRegulationFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region BalanceSheetPDF
                if (_Model.BalanceSheetFile != null)
                {
                    if (_Model.BalanceSheetFile.ToString().Contains("/") && !_Model.BalanceSheetFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.BalanceSheetFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.BalanceSheetFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Balance Sheet", _Model.BalanceSheetFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, _Model.BalanceSheetFile.Contains("undefined") ? "" : _Model.BalanceSheetFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region ProceedingsGeneralMeetingPDF
                if (_Model.GeneralMeetingProceedingFile != null)
                {
                    if (_Model.GeneralMeetingProceedingFile.ToString().Contains("/") && !_Model.GeneralMeetingProceedingFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.GeneralMeetingProceedingFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.GeneralMeetingProceedingFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Proceedings of the General Meeting Held", _Model.GeneralMeetingProceedingFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, _Model.GeneralMeetingProceedingFile.Contains("undefined") ? "" : _Model.GeneralMeetingProceedingFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, "");
                #endregion

                #region WorkPlanShowingEconomicPDF
                if (_Model.WorkPlanShowingEconomicViabilityFile != null)
                {
                    if (_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("/") && !_Model.WorkPlanShowingEconomicViabilityFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.WorkPlanShowingEconomicViabilityFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.WorkPlanShowingEconomicViabilityFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Work Plan Showing Economic Viability of the Society", _Model.WorkPlanShowingEconomicViabilityFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, _Model.WorkPlanShowingEconomicViabilityFile.Contains("undefined") ? "" : _Model.WorkPlanShowingEconomicViabilityFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region MemberListExcelFile
                if (_Model.MemberListExcelFile != null)
                {
                    if (_Model.MemberListExcelFile.ToString().Contains("/") && !_Model.MemberListExcelFile.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.MemberListExcelFile.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.MemberListExcelFile = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("List of Members of the Society (EXCEL)", _Model.MemberListExcelFile);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, _Model.MemberListExcelFile.Contains("undefined") ? "" : _Model.MemberListExcelFile);
                }
                else
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                #region ObjectsFileName
                if (_Model.ObjectsFileName != null)
                {
                    if (_Model.ObjectsFileName.ToString().Contains("/") && !_Model.ObjectsFileName.ToString().Contains("undefined"))
                    {
                        var Arr = _Model.ObjectsFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.ObjectsFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Society Objects", _Model.ObjectsFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, _Model.ObjectsFileName.Contains("undefined") ? "" : _Model.ObjectsFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, "");

                #endregion

                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, _Model.BusinessId);
                objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.Int, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@TotalNumberOfMembers", DBFactoryParameterType.Int, _Model.TotalMembersApplyingForSocociety);
                objDBAccess.AddParameterToCommand("@NoOfMemberInAGM", DBFactoryParameterType.Int, _Model.TotalNumberOfMembersInAGM);
                objDBAccess.AddParameterToCommand("@EntranceFeesCollected", DBFactoryParameterType.Double, _Model.EntranceFeesCollected);
                objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.Double, _Model.PaidUpShareCapital);
                objDBAccess.AddParameterToCommand("@MemberLiablity", DBFactoryParameterType.Int, _Model.MemberLiablity);
                objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.Double, _Model.SingleShareValue);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Bit, _Model.IsNewRegistration);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN == null ? "" : _Model.BRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                objDBAccess.AddParameterToCommand("@RegistrationNoOld", DBFactoryParameterType.NVarchar, (_Model.OldRegistrationNo == "undefined" || _Model.OldRegistrationNo == "" || _Model.OldRegistrationNo == null) ? "" : _Model.OldRegistrationNo.ToUpper().Trim().Replace(" ", ""));


                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@NoOfBranches", DBFactoryParameterType.Int, _Model.NoOfBranches);
                objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.DateTime, _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.ElectionDate).ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.ElectionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                //FOR SPORT 
                objDBAccess.AddParameterToCommand("@IsRSSC", DBFactoryParameterType.NVarchar, _Model.IsStatLevel);
                objDBAccess.AddParameterToCommand("@AgmDate", DBFactoryParameterType.Date, _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());//_Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                objDBAccess.AddParameterToCommand("@SportsAssociationId", DBFactoryParameterType.Int, _Model.AssociationId);

                objDBAccess.AddParameterToCommand("@IsAffiliatedByRSSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByRSSC);
                objDBAccess.AddParameterToCommand("@RSSCRegNo", DBFactoryParameterType.NVarchar, _Model.RSSCRegNo == "undefined" ? "" : _Model.RSSCRegNo);
                objDBAccess.AddParameterToCommand("@RSSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByRSSC == false ? DateTime.MaxValue : _Model.RSSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region RSSCFileName
                if (_Model.RSSCFileName != null)
                {
                    if (_Model.RSSCFileName.ToString().Contains("/") && !_Model.RSSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.RSSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RSSCFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Certificate Of Affiliation To The State Level Sports Council", _Model.RSSCFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, _Model.RSSCFileName == "undefined" ? "" : _Model.RSSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliatedBySOC", DBFactoryParameterType.Bit, _Model.IsAffiliatedBySOC);
                objDBAccess.AddParameterToCommand("@SOCRegNo", DBFactoryParameterType.NVarchar, _Model.SOCRegNo == "undefined" ? "" : _Model.SOCRegNo);
                objDBAccess.AddParameterToCommand("@SOCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedBySOC == false ? DateTime.MaxValue : _Model.SOCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                #region SOCFileName
                if (_Model.SOCFileName != null)
                {
                    if (_Model.SOCFileName.ToString().Contains("/") && !_Model.SOCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SOCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SOCFileName = Arr[Arr.Length - 1].ToString();
                            if (_Model.IsStatLevel)
                            {
                                SocietyDocuments.Add("Certificate Of Affiliation To The State Level Sports Council", _Model.SOCFileName);
                            }
                            else
                            {
                                SocietyDocuments.Add("Certificate Of Affiliation To The District Olympic Council", _Model.SOCFileName);
                            }
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, _Model.SOCFileName == "undefined" ? "" : _Model.SOCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion


                objDBAccess.AddParameterToCommand("@IsAffiliatedByDLSC", DBFactoryParameterType.Bit, _Model.IsAffiliatedByDLSC);
                objDBAccess.AddParameterToCommand("@DLSCRegNo", DBFactoryParameterType.NVarchar, _Model.DLSCRegNo);
                objDBAccess.AddParameterToCommand("@DLSCAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliatedByDLSC == false ? DateTime.MaxValue : _Model.DLSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region DLSCFileName
                if (_Model.DLSCFileName != null)
                {
                    if (_Model.DLSCFileName.ToString().Contains("/") && !_Model.DLSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.DLSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.DLSCFileName = Arr[Arr.Length - 1].ToString();
                            if (_Model.ActId == 7)
                                SocietyDocuments.Add("Certificate Of Affiliation To The District Sports Council", _Model.DLSCFileName);

                            else
                            {
                                SocietyDocuments.Add("Details Of Your Member Cooperative Societies", _Model.DLSCFileName);
                            }
                        }
                    }
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, _Model.DLSCFileName == "undefined" ? "" : _Model.DLSCFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, "");
                #endregion

                objDBAccess.AddParameterToCommand("@IsAffiliationBySLSA", DBFactoryParameterType.Bit, _Model.IsAffiliationBySLSA);
                objDBAccess.AddParameterToCommand("@SLSARegNo", DBFactoryParameterType.NVarchar, _Model.SLSARegNo);
                objDBAccess.AddParameterToCommand("@SLSAAffiliatedDate", DBFactoryParameterType.Date, _Model.IsAffiliationBySLSA == false ? DateTime.MaxValue : _Model.SLSAAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region SLSAFileName
                if (_Model.SLSAFileName != null)
                {
                    if (_Model.SLSAFileName.ToString().Contains("/") && !_Model.SLSAFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SLSAFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SLSAFileName = Arr[Arr.Length - 1].ToString();
                            SocietyDocuments.Add("Certificates Of Affiliation To The State Level Sports Association", _Model.SLSAFileName);
                        }
                    }
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, _Model.SLSAFileName == "undefined" ? "" : _Model.SLSAFileName);
                }
                else
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, "");
                #endregion



                if (_Model.KIOSKCODE.Length > 0)
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, _Model.ServicesId);
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 5530);
                }
                step = 0;
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                if (_Model.SaveStatus == 1)
                {
                    try
                    {
                        UpdateDistrictEntity obk = new UpdateDistrictEntity();

                        step = 1;

                        dtls2 = COMMON_FUNCTION.GetUserDetailsBySSOId(_Model.SSOID);
                        string PostName = "";
                        string ApplicantName = "";
                        string AADHAARNo = AADHARSerice.GetAADHARNoBySSOId(_Model.SSOID);
                        if (_Model.AdministrativeDataList != null && _Model.AdministrativeDataList.Count > 0 && _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).Any())
                        {
                            PostName = _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).FirstOrDefault().PostName;
                            ApplicantName = _Model.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).FirstOrDefault().Name;
                            SaveSocietyApplicatnDetails(_Model.SocietyIDEnc, _Model.SSOID, ApplicantName, PostName, "Registration", "0");
                        }
                        else
                        {
                            SaveSocietyApplicatnDetails(_Model.SocietyIDEnc, _Model.SSOID, dtls2.DisplayName, "", "Registration", "0");
                        }

                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " aa SaveSocietyApplicatnDetails(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

                    }

                }


                var RES = Result.Split('@');
                int R = -1;
                if (RES.Length > 0)
                {
                    step = 2;
                    SocietyIdEnc = Result.Split('@')[0];
                    step = 3;
                    R = int.Parse(Result.Split('@')[1]);
                }

                foreach (var data in SocietyDocuments)
                {
                    (new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = data.Value, Isactive = true, SocietyId = 0, DocumentType = "", DocumentName = data.Key }, SocietyIdEnc);
                }


                //_Model.AmendmentDocuments = _Model.AmendmentDocuments ?? new List<AmendmentDocuments>();

                //if (_Model.IsAmendment != null && _Model.AmendmentDocuments.Count > 0)
                //{
                //    _Model.AmendmentDocuments.ForEach(doc =>
                //    {
                //        if (doc.DocumentName != null && doc.DocumentName != "")
                //        {
                //            objDBAccess.OpenConnection();
                //            cmd = new SqlCommand();
                //            SqlCommand cmdAddress = new SqlCommand();
                //            step = 4;
                //            cmd = objDBAccess.CreateCommand("InsertSocietyAmendmentDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                //            objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                //            objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, 0);
                //            objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.NVarchar, doc.DocumentType);
                //            objDBAccess.AddParameterToCommand("@DocumentName", DBFactoryParameterType.NVarchar, doc.DocumentName.IndexOf("/") > -1 ? doc.DocumentName.Split('/')[4] : doc.DocumentName);
                //            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _Model.UserId);
                //            cmd.ExecuteScalar();
                //        }
                //    });
                //}

                if (Result.ToString().Length > 10 && Result != "-5" && R > 0)
                {
                    int FlagFirstRow = 1;

                    string WorkScopeIds = string.Join(",", _Model.lstSocietyWorkScope.Select(x => x.WorkScopeId));
                    foreach (SocietyWorkScopeEntity obj in _Model.lstSocietyWorkScope)
                    {
                        objDBAccess.OpenConnection();
                        step = 6;
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertWorkScope_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, Result.ToString().Contains("@") ? int.Parse(Result.Split('@')[1]) : 0);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Result.Split('@')[0]);
                        objDBAccess.AddParameterToCommand("@IsStatLevel", DBFactoryParameterType.Int, obj.IsStatLevel);
                        objDBAccess.AddParameterToCommand("@WorkScopeLevel_ID", DBFactoryParameterType.Int, obj.WorkScopeLevel_ID);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Int, obj.CITY_ID > 0 ? 0 : 1);
                        objDBAccess.AddParameterToCommand("@DirectorName", DBFactoryParameterType.NVarchar, obj.DirectorName);
                        objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                        objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.Int, obj.AssociationId);
                        objDBAccess.AddParameterToCommand("@FlagFirstRow", DBFactoryParameterType.Int, FlagFirstRow);
                        objDBAccess.AddParameterToCommand("@SportsAssociationName", DBFactoryParameterType.NVarchar, obj.SportsAssociationName);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Model.UserType);
                        objDBAccess.AddParameterToCommand("@AreaOfOperation", DBFactoryParameterType.NVarchar, _Model.AreaOfOperation);
                        objDBAccess.AddParameterToCommand("@WorkScopeId", DBFactoryParameterType.NVarchar, obj.WorkScopeId);
                        objDBAccess.AddParameterToCommand("@WorkScopeIds", DBFactoryParameterType.NVarchar, WorkScopeIds);

                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                        WorkScopeIds += "," + InnerResult;

                    }
                }
                else
                {
                    Result = "-1";
                }

                string Ids = string.Join(",", _Model.AdministrativeDataList.Select(x => x.AdministrativeId));
                foreach (AdministrativeData obj in _Model.AdministrativeDataList)
                {
                    step = 7;
                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0 || (obj.MEMBERTYPE == "ADMINISTRATIVE" || obj.MEMBERTYPE == "LIQUIDATOR"))
                    {
                        obj.SocietyIdEnc = SocietyIdEnc;
                        var DOBConvert = obj.DOB == null ? DateTime.MaxValue : (obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue);
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                        objDBAccess.OpenConnection();
                        step = 8;
                        SqlCommand cmdAdministrative = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                        objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@NameOfSociety", DBFactoryParameterType.NVarchar, obj.NameOfSociety);
                        objDBAccess.AddParameterToCommand("@PostinSociety", DBFactoryParameterType.NVarchar, obj.PostinSociety);

                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, obj.FatherName);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, (_Model.ClassA == null || _Model.ClassA.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassA));
                        objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, (_Model.ClassB == null || _Model.ClassB.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassB));
                        objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, (_Model.ClassC == null || _Model.ClassC.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassC));
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _Model.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                        objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                        objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                        objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);
                        objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, obj.BoardType);
                        objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                        objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.Int, obj.ShareAllotted);
                        objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.Money, obj.ShareCapitalAmount);
                        objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.Int, obj.loaneeType);
                        objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.Int, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserTypeCK", DBFactoryParameterType.NVarchar, _Model.UserType);
                        objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                        if (_Model.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _Model.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }

                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                        {
                            R = int.Parse(Result.Split('@')[1]);
                            Ids += "," + Result.Split('@')[2];
                        }

                    }
                    i++;
                }

                //ADDRESS
                string AddressResult = "-1";
                int EMitraTransactionId = 0;
                int SubmitStatus = _Model.SocietyRegisteredOfficeDetailsLst[0].SubmitStatus;
                try
                {
                    objDBAccess.OpenConnection();
                    foreach (SocietyRegisteredOfficeDetailsEntity obj in _Model.SocietyRegisteredOfficeDetailsLst)
                    {
                        obj.SocietyIdEnc = SocietyIdEnc;
                        SqlCommand cmdAddress = new SqlCommand();
                        step = 9;
                        cmdAddress = objDBAccess.CreateCommand("InsertSocietyRegOfficeDetail_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@SocietyRegOfficeDetailId", DBFactoryParameterType.BigInt, obj.SocietyRegOfficeDetailId);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, obj.IsRural);
                        objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, obj.HouseBuildingNo);
                        objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, obj.LaneStreetName);
                        objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, obj.Locality);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.Int, obj.CityTypes);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, obj.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, obj.GP_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, obj.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, obj.PinCode);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, obj.UpdatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, obj.CreatedIP);
                        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, obj.UpdatedIP);
                        objDBAccess.AddParameterToCommand("@IsSameCEOAddress", DBFactoryParameterType.Bit, obj.IsSameCEOAddress);
                        objDBAccess.AddParameterToCommand("@AddressFor", DBFactoryParameterType.Int, obj.AddressFor);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, R);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, obj.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSO_ID);
                        objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.NVarchar, obj.SubmitStatus);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, obj.UserType);
                        if (obj.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, obj.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        if ((Result.Split('@').Length > 1))
                            EMitraTransactionId = int.Parse(Result.Split('@')[1]);
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegisteredOfficeDetailsInsert(List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
                //END


            }
            catch (Exception ex)
            {
                Result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity step " + step + "; {" + JsonConvert.SerializeObject(_Model) + "})" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }


        public DataTable GetMemberAmendmentBySocietyId(string SocietyIdEnc, int AmendmentId = 0)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetMemberAmendmentBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        [CustomAuthentication]
        public DataTable GetMemberListAmendmentBySocietyId(string SocietyIdEnc)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetMemberListAmendmentBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyAmendmentDocuments(string SocietyIdEnc, string AmendmentId)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyAmendmentDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyAmendmentDocuments(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyAppicationStatus(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyAppicationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDetailsBySocietyId(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public string UpdateSocietyProfileBy_SA_RAO_CAO_OLD(SocietyRegistrationEntity _Model)
        {
            string Result = "0";
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                cmdAddress = objDBAccess.CreateCommand("UpdateSocietyProfileBy_SA_RAO_CAO", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (_Model.ChangeStatus == 0)
                {
                    objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                    objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                    objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                    objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                    objDBAccess.AddParameterToCommand("@RagistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate);
                    objDBAccess.AddParameterToCommand("@ChangeStatus", DBFactoryParameterType.Int, _Model.ChangeStatus);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, _Model.SocietyID);
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                    objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                    objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                    objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                    objDBAccess.AddParameterToCommand("@RagistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate);
                    objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, _Model.IsRural);
                    objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, _Model.HouseBuildingNo);
                    objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, _Model.LaneStreetName);
                    objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, _Model.Locality);
                    objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, _Model.STATE_ID);
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, _Model.DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, _Model.DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, _Model.CITY_ID);
                    objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, _Model.BLOCK_ID);
                    objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, _Model.WARD_ID);
                    objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, _Model.GP_ID);
                    objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, _Model.VILLAGE_ID);
                    objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, _Model.PinCode);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _Model.UpdatedBy);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, _Model.SocietyID);
                    objDBAccess.AddParameterToCommand("@ChangeStatus", DBFactoryParameterType.Int, _Model.ChangeStatus);
                }

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " UpdateSocietyProfileBy_SA_RAO_CAO(SocietyRegistrationEntity _Model)" }, ex);
            }
            return Result;
        }
        public string UpdateSocietyProfileBy_SA_RAO_CAO(SocietyRegistrationEntity _Model)
        {
            string Result = "0";
            int i = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                cmdAddress = objDBAccess.CreateCommand("UpdateSocietyProfileBy_SA_RAO_CAO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, _Model.Request_Id);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, _Model.SocietyID);
                if (_Model.Request_Id == "1")
                {

                    if (_Model.ChangeStatus == 0)
                    {
                        objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                        objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                        objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                        objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                        objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                        objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                        objDBAccess.AddParameterToCommand("@RagistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate);
                        objDBAccess.AddParameterToCommand("@ChangeStatus", DBFactoryParameterType.Int, _Model.ChangeStatus);
                    }
                    else
                    {
                        objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                        objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                        objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                        objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                        objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                        objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, _Model.SocietyCategoryId);
                        objDBAccess.AddParameterToCommand("@RagistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate);
                        objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, _Model.IsRural);
                        objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, _Model.HouseBuildingNo);
                        objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, _Model.LaneStreetName);
                        objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, _Model.Locality);
                        objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, _Model.STATE_ID);
                        objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, _Model.DIVISION_ID);
                        objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, _Model.DISTRICT_ID);
                        objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, _Model.CITY_ID);
                        objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, _Model.BLOCK_ID);
                        objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, _Model.WARD_ID);
                        objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, _Model.GP_ID);
                        objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, _Model.VILLAGE_ID);
                        objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, _Model.PinCode);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _Model.UpdatedBy);
                        objDBAccess.AddParameterToCommand("@ChangeStatus", DBFactoryParameterType.Int, _Model.ChangeStatus);
                    }
                }
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " UpdateSocietyProfileBy_SA_RAO_CAO(SocietyRegistrationEntity _Model)" }, ex);
            }
            return Result;
        }
        public DataTable GetAllUserRatingList(DataPagingModel Details, string Rating, string ActId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string WhereClause = "";
                if (Rating != "0" && ActId != "0")
                {
                    WhereClause = "Where Rating=" + "'" + Rating + "'" + " and Societies.ActId" + "'" + ActId + "'";
                }
                if (Rating != "0" && ActId == "0")
                {
                    WhereClause = "Where Rating=" + "'" + Rating + "'";
                }
                if (Rating == "0" && ActId != "0")
                {
                    WhereClause = "Where Societies.ActId=" + "'" + ActId + "'";
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    Details.TotalRecords = 0;
                    cmd = objDBAccess.CreateCommand("Get_UserRating", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("Get_UserRating", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt1;
        }

        public int CheckNewRequestFromSWCS_Old(string SWSID, string SSOID)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("encAadhar", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SWSID", DBFactoryParameterType.NVarchar, SWSID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "CheckNewRequestFromSWCS(string SWSID, string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        //Pradeep
        public int CheckNewRequestFromSWCS(string SWSID, string SSOID = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_RajNiveshSWCS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "CheckNewRequestFromSWCS");
                objDBAccess.AddParameterToCommand("@SWSID", DBFactoryParameterType.Int, SWSID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.Int, SSOID);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "CheckNewRequestFromSWCS(string SWSID, string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAuditorSMSList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorSMSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorSMSList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string UpdateAuditorSMSSent(string Id)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateAuditorSMSSent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.ExecuteScalarCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable UpdateAuditorSMSSent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "1";
        }
        public int Bankverification(int BankDetailId)
        {

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BankdetailsVerified", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@id", DBFactoryParameterType.Bit, BankDetailId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int InsertSocietyAuditReportDetails(SocietyAuditReportEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;


        }


        public int InsertInspactorTargets(List<InspactorTargets> InspactorTargets)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();
            foreach (InspactorTargets item in InspactorTargets)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_update_InspactorTargets", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@InspacorName", DBFactoryParameterType.NVarchar, item.InspacorName);
                    objDBAccess.AddParameterToCommand("@DistictId", DBFactoryParameterType.Int, item.DistictId);
                    objDBAccess.AddParameterToCommand("@Urban_Rural", DBFactoryParameterType.NVarchar, item.Urban_Rural);
                    if (item.Urban_Rural == "Urban")
                    {
                        objDBAccess.AddParameterToCommand("@City_ID", DBFactoryParameterType.Int, item.City_Block_ID);
                    }
                    else
                    {
                        objDBAccess.AddParameterToCommand("@Block_Id", DBFactoryParameterType.Int, item.City_Block_ID);
                    }
                    objDBAccess.AddParameterToCommand("@GramPanchayat_ID", DBFactoryParameterType.Int, item.GramPanchayat_ID);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, item.MonthId);
                    objDBAccess.AddParameterToCommand("@SurveyDate", DBFactoryParameterType.NVarchar, item.SurveyDate);
                    objDBAccess.AddParameterToCommand("@Document", DBFactoryParameterType.NVarchar, item.Document);


                    //madhuri
                    objDBAccess.AddParameterToCommand("@IsTspArea", DBFactoryParameterType.Bit, item.IsTspArea);
                    objDBAccess.AddParameterToCommand("@AreaType", DBFactoryParameterType.Int, item.AreaType);
                    //objDBAccess.AddParameterToCommand("@TspAreaType", DBFactoryParameterType.Int, item.TspAreaType);
                    //objDBAccess.AddParameterToCommand("@NonTspAreaType", DBFactoryParameterType.Int, item.NonTspAreaType);
                    objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, item.VillageId);
                    objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.NVarchar, item.VillageName);
                    objDBAccess.AddParameterToCommand("@CompetentAcceptanceLetter", DBFactoryParameterType.NVarchar, item.CompetentAcceptanceLetter);
                    objDBAccess.AddParameterToCommand("@AllotmentLetter", DBFactoryParameterType.NVarchar, item.AllotmentLetter);

                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@LoanBankID", DBFactoryParameterType.Int, item.LoanBankID);
                    objDBAccess.AddParameterToCommand("@LoanBranchId", DBFactoryParameterType.Int, item.LoanBranchId);
                    objDBAccess.AddParameterToCommand("@IsMahilaMpacs", DBFactoryParameterType.Bit, item.IsMahilaMpacs);
                    objDBAccess.AddParameterToCommand("@AssemblyId", DBFactoryParameterType.Int, item.AssemblyId);
                    objDBAccess.AddParameterToCommand("@ProposedNameMPACS", DBFactoryParameterType.NVarchar, item.ProposedNameMPACS);


                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {

                }

            }
            return Result;
        }

        public DataTable GetInspactorTargetsCount(string InspactorId, string GSSSStatusId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string WhereClause = "";

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (GSSSStatusId == "0" || GSSSStatusId == null)
                {


                }
                else
                {
                    WhereClause += " and InspectorStatusId=" + "'" + GSSSStatusId + "'";
                }
                if (InspactorId == "0" || InspactorId == "")
                {

                }
                else
                {
                    WhereClause += " and Inspector_ID=" + "'" + InspactorId + "'";
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("Get_InspactorTargetsCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
            }
            return dt1;
        }


        public int SaveInspactorstausDetails(InspactorStatusDetails _InspactorStatusDetails)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Insert_InspectorStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, _InspactorStatusDetails.InspactorTargetsId);
                objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, _InspactorStatusDetails.Inspector_ID);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.Remarks);
                objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.Inspectorfile);
                objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.PrecentNameGSS);
                objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.NewGSSName);
                objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.NoofMember);
                objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.NewGSSprecious);
                objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, _InspactorStatusDetails.NewGSSFinanceAmount);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@IsMahilaMpacs", DBFactoryParameterType.Bit, _InspactorStatusDetails.IsMahilaMpacs);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {

            }


            return Result;
        }

        public DataTable GetInspectorStatusDetails(string InspactorTargetsId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("get_InserInspectorStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.NVarchar, InspactorTargetsId);
                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
            }
            return dt1;
        }


        public ResultMessage GenerateSenctionLetter(List<InspactorStatusDetails> InspactorStatusDetails, string TXNno, string SenctionDate, string fileName)
        {
            ResultMessage result = new ResultMessage();
            try
            {

                var firstElement = InspactorStatusDetails.First();

                string SenctionDate1 = "";
                if (!string.IsNullOrWhiteSpace(SenctionDate))
                {
                    SenctionDate = SenctionDate.Replace("-", "/");
                    SenctionDate1 = SenctionDate.Split('/')[0] + "." + SenctionDate.Split('/')[1] + "." + SenctionDate.Split('/')[2];
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string city = _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL : "राजस्थान";
                string cityEn = _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_ENG : "Rajasthan";

                string date = SenctionDate1;
                string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                string lbl_Kramank = "क्रमांक  प. 15  (28) सविरा / योजना -ll/08 / " + firstElement.DISTRICT_MANGAL;
                string drName = string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;

                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियॉं, " + "जयपुर") + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("आदेश") + "</label></b><br/></td>");

                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'>" + lbl_Kramank + "</td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "  मंत्री मंडल  की आज्ञा संख्या डी 124 /म /म/2011 दिनांक 21.9 . 11 के अनुसरण में विभागीय आदेश क्रमांक प  82 (1)सविरा / योजना -ll/08पार्ट-ll दिनांक 26. 9.11 के तहत उप रजिस्ट्रार, सहकारी समितिया, " + firstElement.DISTRICT_MANGAL + " के पत्राक 734 दिनांक 16. 09. 2020 तथा पत्राक 785 दिनांक 30. 9 . 20 द्वारा उपलब्ध कराई गयी सूचनाओ एवं जिला कलेक्टर एवं अध्यक्ष, जिला कमेठी " + firstElement.DISTRICT_MANGAL + " की अध्यक्षता में आयोजित बैठक के कार्यवाही विवरण दिनांक " + date + " की अनुसंशा के तहत " + firstElement.DISTRICT_MANGAL + " जिले में निम्नांकित ग्राम सेवा सहकारी समितियों के गठन की अनुमति इस शर्त के साथ प्रदान की जाती है की वक्त रजिस्ट्रशन यह सुनिश्चित कर लिया जावे कि विभागीय आदेश क्रमांक प.  82 (1)सविरा/प्लान -ll/2008/पार्ट-ll दिनांक 26.9.2011, 31.7.2015 आदेश दी. 18. 5. 16 तथा 5. 10. 16 द्वारा नई पैक्स के गठन बाबत निर्धारित मानदण्ड/शर्त पूर्ण करवा कर ही रजिस्ट्रशनकिया जावे | </p>");
                sb.Append("</td>");
                sb.Append("</tr>");



                sb.Append("<tr>");
                sb.Append("<td style='text-align:center; padding-top:15px;'>");
                sb.Append("<table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;" + fontSize + "'>");
                sb.Append("<thead> <tr> <th style='text-align:left;border:1px solid #000;width:40px;'> क्रमांक </th> <th style='text-align:left;'>  ग्राम सेवा सहकारी समिति लेम्पस का नाम </th>  <th style='text-align:left;width:150px;'> ग्राम पंचायत का नाम </th> <th style='text-align:left;width:150px;'>पंचायत का नाम </th>  </tr>  </thead>");
                sb.Append("<tbody>");

                int RowNo = 1;
                foreach (InspactorStatusDetails item in InspactorStatusDetails)
                {
                    sb.Append(" <tr> <td style='text-align:left;'> " + RowNo.ToString() + " </td><td style='text-align:left;'> " + item.PrecentNameGSS + " </td><td style='text-align:left;'> " + item.GP_ENG + "</td><td style='text-align:left;'> " + item.panchayat + "</td> </tr>");
                    RowNo++;
                }


                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + " नवगठित ग्राम सेवा सहकारी समितियों के लिए स्थानीय ग्राम सेवा पंचायत द्वारा प्रथम 5 वर्ष हेतु भवन उपलब्ध करवाया जावेगा एवं Rajasthan  Land Revenue (Allotment and Conversion of Land the Construction of Seed Godown )</p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + " Rules, 1965 (NotificationNo. F.9(60)Rev-6/2012/14 Dated 19-6-2012 द्वारा यथा संशोदित )  के नियम अनुसरण गोदाम निर्माण हेतु 1500 वर्ग मीटर का नि;शुलक भूखंड उपलब्ध करवाया जावेगा | समितियों के सर्जन एवं संचालन हेतु राज्ये सरकार  द्वारा  किसी प्रकार की आर्थिक स्वीकृत  नहीं की जावेगी | साथ ही केन्द्रीय सहकारी बैंक लि. उदयपुर को निर्देश दिए जाते है की समितियों के गठन उपरांत समितियों के आर्थिक स्वावलम्बन हेतु फडस उपलब्ध्ता  के अनुसार समितियों के सदस्यों में ऋण वितरण करना सुनिचित करे | उक्त समितियों के गठन हेतु उप रजिस्ट्रार , सहकारी समितियों " + firstElement.DISTRICT_ENG + " सहकारिता  अधिनियम  एवं उपरोक्त विभागीय आदेशों की पालना करते हुए पंजीकरण संबंधि आवश्यक कार्यवाही कर , बाद पंजीयन ,पंजीयन प्रमाण पत्र   की प्रतिलिपि विभाग को प्रषित की जावे | </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + " प. 15  (28)सविरा / योजना  -ll/08पार्ट-ll </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + " प्रतिलिपि : निम्न को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है: </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "1 निजी सचिव माननिय सहकारिता मंत्री महोदय , राजस्थान सरकार जयपुर  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "2 निजी सचिव , प्रमुख शासन सचिव , सहकारी  विभाग राजस्थान जयपुर </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "3 जिला कलेक्टर राजसमंद  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");


                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "4 प्रबन्ध निदेशक राज. सह.बैंक लि.जयपुर  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "5 अतिरिक्त रजिस्ट्रार सहकारी समितिया  उदयपुर  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "6 प्रबन्ध निदेश,केन्द्रीय सहकारी बैंक लि. उदयपुर  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "7 उप रजिस्ट्रार, सहकारी समितिया, राजसमंद  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + "" + "8 फक्सनल अधिकारी ---------------(समस्त) प्र. का. जयपुर  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");



                sb.Append("<tr><td style='padding: 60px 15px 10px 35px;  margin-bottom:20px; text-align: right;'><br/><br/><br/></td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                // string filepath = Server.MapPath("~/Content/SenctionFiles");
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SenctionFiles");
                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + fileName, ""))
                {
                    result = COMMON_FUNCTION.EsinghPDF(TXNno, filepath + "/" + fileName, cityEn, designationEn, "350", "30", "150", "30");
                }
                result.data = fileName;



            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public string ValidateOldSocietyIsExist(string SocietyIdEnc, string RegistrationNo)
        {
            string result = "0";
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("ValidateOldSocietyIsExist", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable ValidateOldSocietyIsExist(" + SocietyIdEnc + "," + RegistrationNo + ")" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public DataTable GetInspactorTargetsReport(string InspactorId, string GSSSStatusId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string WhereClause = "";


                if (GSSSStatusId == "0" || GSSSStatusId == null)
                {


                }
                else
                {
                    WhereClause += " and InspectorStatusId=" + "'" + GSSSStatusId + "'";
                }
                if (InspactorId == "0" || InspactorId == "")
                {
                    //WhereClause += " and InspectorStatusId=" + "'" + GSSSStatusId + "'";
                }
                else
                {
                    WhereClause += " and Inspector_ID=" + "'" + InspactorId + "'";
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetInspactorTargetsReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
            }
            return dt1;
        }

        //public DataTable GetInspactorTargetsDetails(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int Month)
        //{
        //    DataTable dt1 = new DataTable(); try

        //    {
        //        string WhereClause = "";
        //        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //        if (StatusId != "5")
        //        {
        //            if (StatusId == "0")
        //            { }
        //            else
        //            { WhereClause += "and IIT.InspectorStatusId=" + "'" + StatusId + "'"; }

        //            //old

        //            //if (InspactorId != "0")

        //            //{

        //            //    WhereClause += " and IIT.Inspector_ID=" + "'" + InspactorId + "'";

        //            //    //WhereClause += " and IIT.InspectorStatusId = '" + GSSSStatusId + "'";

        //            //}

        //            //else

        //            //{

        //            //    if (_LoggedInUser.DistrictId > 0)

        //            //    {

        //            //        WhereClause += " and IIT.Inspector_ID=" + "'" + _LoggedInUser.UserRoleDeptMappingId + "'";

        //            //    }

        //            //}

        //            //New

        //            if (InspactorId != "0" && string.IsNullOrWhiteSpace(GSSSStatusId))
        //            {
        //                WhereClause += " and IIT.Inspector_ID = '" + InspactorId + "'";
        //            }

        //            else if (InspactorId != "0" && !string.IsNullOrWhiteSpace(GSSSStatusId))
        //            {
        //                WhereClause += " AND ISD.Status IN (" + GSSSStatusId + ")";
        //                //WhereClause += " AND ISD.Status = '" + GSSSStatusId + "'";
        //                WhereClause += " AND IIT.Inspector_ID = " + InspactorId;
        //            }
        //            else if (_LoggedInUser.DistrictId > 0 && Flag != 7)
        //            { WhereClause += " and IIT.Inspector_ID = '" + _LoggedInUser.UserRoleDeptMappingId + "'"; }

        //        }

        //        if (GSSSStatusId == "0" || GSSSStatusId == null)
        //        {

        //            if (Flag == 1)

        //            {

        //            }

        //            else

        //            {

        //                Flag = 4;

        //                //WhereClause = " and IIT.InspactorTargetsId=" + "'" + GSSSStatusId + "'";

        //            }

        //        }

        //        else

        //        {

        //            //WhereClause += " and IIT.InspectorStatusId=" + "'" + GSSSStatusId + "'";

        //            //WhereClause += " and ISD.Status=" + "'" + GSSSStatusId + "'";

        //        }

        //        if (StatusId == "5" && (GSSSStatusId == "1"))

        //        {

        //            Flag = 3;

        //        }

        //        if (StatusId == "5" && GSSSStatusId == "8")

        //        {

        //            Flag = 5;

        //            //WhereClause = "";

        //        }

        //        if (StatusId == "6")

        //        {

        //            Flag = 5;

        //            WhereClause = "";

        //        }

        //        if (StatusId == "13")

        //        {

        //            Flag = 6;

        //            WhereClause = " and IIT.InspactorTargetsId=" + "'" + GSSSStatusId + "'";

        //        }

        //        SqlCommand cmd = new SqlCommand();

        //        objDBAccess.OpenConnection();

        //        if (Details.PageNumber == 1)
        //        {
        //            Details.TotalRecords = 0;
        //            cmd = objDBAccess.CreateCommand("Get_InspactorTargetsDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //            objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
        //            objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
        //            objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 1);
        //            objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
        //            objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
        //            objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
        //            objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
        //            objDBAccess.AddParameterToCommand("@GSSSStatusId", DBFactoryParameterType.NVarchar, GSSSStatusId);
        //            Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //        }
        //        if (Details.IsAllRecords)
        //        {
        //            Details.PageSize = Details.TotalRecords;
        //        }
        //        cmd = objDBAccess.CreateCommand("Get_InspactorTargetsDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
        //        objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
        //        objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 0);
        //        objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
        //        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
        //        objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
        //        objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
        //        objDBAccess.AddParameterToCommand("@GSSSStatusId", DBFactoryParameterType.NVarchar, GSSSStatusId);
        //        dt1 = objDBAccess.GetTable();

        //    }

        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt1;

        //}

        public DataTable GetInspactorTargetsDetails(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int Month)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string WhereClause = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (StatusId != "5")
                {
                    if (StatusId == "0")
                    { }
                    else
                    {
                        WhereClause += "and IIT.InspectorStatusId=" + "'" + StatusId + "'";
                    }

                    //old
                    //if (InspactorId != "0")
                    //{
                    //    WhereClause += " and IIT.Inspector_ID=" + "'" + InspactorId + "'";

                    //    //WhereClause += " and IIT.InspectorStatusId = '" + GSSSStatusId + "'";
                    //}
                    //else
                    //{
                    //    if (_LoggedInUser.DistrictId > 0)
                    //    {
                    //        WhereClause += " and IIT.Inspector_ID=" + "'" + _LoggedInUser.UserRoleDeptMappingId + "'";
                    //    }
                    //}

                    //New
                    if (InspactorId != "0" && string.IsNullOrWhiteSpace(GSSSStatusId))
                    {
                        WhereClause += " and IIT.Inspector_ID = '" + InspactorId + "'";

                    }
                    else if (InspactorId != "0" && !string.IsNullOrWhiteSpace(GSSSStatusId))
                    {
                        WhereClause += " AND ISD.Status IN (" + GSSSStatusId + ")";
                        //WhereClause += " AND ISD.Status = '" + GSSSStatusId + "'";
                        WhereClause += " AND IIT.Inspector_ID = " + InspactorId;
                    }
                    else if (_LoggedInUser.DistrictId > 0 && Flag != 7)
                    {
                        WhereClause += " and IIT.Inspector_ID = '" + _LoggedInUser.UserRoleDeptMappingId + "'";
                    }
                }
                if (GSSSStatusId == "0" || GSSSStatusId == null)
                {
                    if (Flag == 1)
                    {

                    }
                    else if (Flag != 7)
                    {
                        Flag = 4;
                        //WhereClause = " and IIT.InspactorTargetsId=" + "'" + GSSSStatusId + "'";

                    }

                }
                else
                {
                    //WhereClause += " and IIT.InspectorStatusId=" + "'" + GSSSStatusId + "'";
                    //WhereClause += " and ISD.Status=" + "'" + GSSSStatusId + "'";
                }
                if (StatusId == "5" && (GSSSStatusId == "1"))
                {
                    Flag = 3;
                }
                if (StatusId == "5" && GSSSStatusId == "8")
                {
                    Flag = 5;
                    //WhereClause = "";
                }
                if (StatusId == "6")
                {
                    Flag = 5;
                    WhereClause = "";

                }
                if (StatusId == "13")
                {
                    Flag = 6;
                    WhereClause = " and IIT.InspactorTargetsId=" + "'" + GSSSStatusId + "'";

                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    Details.TotalRecords = 0;
                    cmd = objDBAccess.CreateCommand("Get_InspactorTargetsDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
                    objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@GSSSStatusId", DBFactoryParameterType.NVarchar, GSSSStatusId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("Get_InspactorTargetsDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@GSSSStatusId", DBFactoryParameterType.NVarchar, GSSSStatusId);

                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt1;
        }

        public int SaveInspactorStatusDR_old(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string TXNno, string SenctionDate)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            string SenctionDate1 = "";
            string fileName = "";
            if (!string.IsNullOrWhiteSpace(SenctionDate))
            {
                SenctionDate = SenctionDate.Replace("-", "/");
                SenctionDate1 = SenctionDate.Split('/')[2] + "/" + SenctionDate.Split('/')[1] + "/" + SenctionDate.Split('/')[0];
            }

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            if (status == "4")
            {
                fileName = Guid.NewGuid().ToString() + ".pdf";
            }
            foreach (InspactorStatusDetails item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_InspectorStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, Inspectorfile);
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    objDBAccess.AddParameterToCommand("@SenctionDate", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(SenctionDate1) ? "" : SenctionDate1);
                    objDBAccess.AddParameterToCommand("@SenctionLatter", DBFactoryParameterType.NVarchar, fileName);

                    objDBAccess.AddParameterToCommand("@DateOfSubmission", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(item.DateOfSubmission) ? "" : item.DateOfSubmission);
                    objDBAccess.AddParameterToCommand("@DateOfApproval", DBFactoryParameterType.NVarchar, item.DateOfApproval);

                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            if (status == "4")
            {
                var resultLetter = GenerateSenctionLetter(InspactorStatusDetails, TXNno, SenctionDate, fileName);
            }

            return Result;
        }

        public int SaveInspactorStatusDR(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string TXNno, string SenctionDate, string IDs = "")
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            string SenctionDate1 = "";
            string fileName = "";
            if (!string.IsNullOrWhiteSpace(SenctionDate))
            {
                SenctionDate = SenctionDate.Replace("-", "/");
                SenctionDate1 = SenctionDate.Split('/')[2] + "/" + SenctionDate.Split('/')[1] + "/" + SenctionDate.Split('/')[0];
            }

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            if (status == "4")
            {
                fileName = Guid.NewGuid().ToString() + ".pdf";
            }
            foreach (var item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_InspectorStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, item.Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, Inspectorfile);
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);


                    objDBAccess.AddParameterToCommand("@DateOfSubmission", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(item.DateOfSubmission) ? "" : item.DateOfSubmission);
                    objDBAccess.AddParameterToCommand("@DateOfApproval", DBFactoryParameterType.NVarchar, item.DateOfApproval);

                    objDBAccess.AddParameterToCommand("@SanctionDate", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(item.SanctionDate) ? "" : item.SanctionDate);
                    objDBAccess.AddParameterToCommand("@SanctionLetter", DBFactoryParameterType.NVarchar, item.SanctionLetter);
                    objDBAccess.AddParameterToCommand("@IDs", DBFactoryParameterType.NVarchar, IDs);

                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            //if (status == "5")
            //{
            //    var resultLetter = GenerateSenctionLetter(InspactorStatusDetails, TXNno, SenctionDate, fileName);
            //}

            return Result;
        }

        public int SaveAGMStatusDR(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string SenctionDate, string MultiInspactorIds, int DistrictId, int DivisionId, int LevelId, int OfficeId)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            string SenctionDate1 = "";
            string fileName = "";
            if (!string.IsNullOrWhiteSpace(SenctionDate))
            {
                SenctionDate = SenctionDate.Replace("-", "/");
                SenctionDate1 = SenctionDate.Split('/')[2] + "/" + SenctionDate.Split('/')[1] + "/" + SenctionDate.Split('/')[0];
            }

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            if (status == "4")
            {
                fileName = Guid.NewGuid().ToString() + ".pdf";
            }
            foreach (var item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_AGMStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, Inspectorfile);
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    objDBAccess.AddParameterToCommand("@AGMTime", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@AGMDate", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@AGMPlace", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@AGMNotice", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@IsAGMConduct", DBFactoryParameterType.Int, 0);
                    //Added by jaya
                    objDBAccess.AddParameterToCommand("@MultiInspactorIds", DBFactoryParameterType.NVarchar, MultiInspactorIds);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                    objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, OfficeId);

                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }

            return Result;
        }

        public int SaveGNStatus(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string AgmTime, string status, string AgmDate, string AgmPlace, string fileName)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            foreach (var item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_AGMStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, item.Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);

                    objDBAccess.AddParameterToCommand("@AGMTime", DBFactoryParameterType.NVarchar, AgmTime);
                    objDBAccess.AddParameterToCommand("@AGMDate", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(AgmDate) ? "" : AgmDate);
                    objDBAccess.AddParameterToCommand("@AGMPlace", DBFactoryParameterType.NVarchar, AgmPlace);
                    objDBAccess.AddParameterToCommand("@AGMNotice", DBFactoryParameterType.NVarchar, fileName);
                    objDBAccess.AddParameterToCommand("@IsAGMConduct", DBFactoryParameterType.Int, 0);

                    objDBAccess.AddParameterToCommand("@MultiInspactorIds", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, 0);
                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }

            return Result;
        }

        public int SaveAGMFStatus(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string IsAGMConduct, string status, string agmConductDate, string agmReasons, string fileNames)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            foreach (var item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_AGMStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, item.Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);

                    objDBAccess.AddParameterToCommand("@AGMTime", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@AGMDate", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(agmConductDate) ? "" : agmConductDate);
                    objDBAccess.AddParameterToCommand("@AGMPlace", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@AGMNotice", DBFactoryParameterType.NVarchar, agmReasons);
                    objDBAccess.AddParameterToCommand("@IsAGMConduct", DBFactoryParameterType.Int, IsAGMConduct);
                    objDBAccess.AddParameterToCommand("@MultiInspactorIds", DBFactoryParameterType.NVarchar, "");
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@AGMUploadDocs", DBFactoryParameterType.NVarchar, fileNames);
                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }

            return Result;
        }

        public DataTable GetInspactorTargetsReportDetails(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int DistrictId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string WhereClause = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (StatusId != "5")
                {
                    if (StatusId == "0")
                    { }
                    else
                    {
                        WhereClause += "and IIT.InspectorStatusId=" + "'" + StatusId + "'";
                    }

                    if (InspactorId != "0" && InspactorId != null)
                    {
                        WhereClause += " and IIT.Inspector_ID=" + "'" + InspactorId + "'";
                    }
                    //if (_LoggedInUser.DistrictId > 0)
                    //{
                    //    WhereClause += " and IIT.Inspector_ID=" + "'" + _LoggedInUser.UserRoleDeptMappingId + "'";
                    //}

                }
                if (string.IsNullOrWhiteSpace(GSSSStatusId))
                {
                    Flag = 2;
                    //WhereClause += " and IIT.InspectorStatusId=" + "'" + GSSSStatusId + "'";
                }
                else if (GSSSStatusId == "8")
                {
                    Flag = 3;
                    WhereClause += " AND IST.Status = '" + GSSSStatusId + "'";
                }
                else
                {
                    Flag = 1;
                    // WhereClause += " and IIT.InspectorStatusId=" + "'" + GSSSStatusId + "'";
                    WhereClause += " and IST.Status=" + "'" + GSSSStatusId + "'";
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    Details.TotalRecords = 0;
                    cmd = objDBAccess.CreateCommand("GetInspactorTargetsReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("GetInspactorTargetsReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);

                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt1;
        }
        public string SaveCCSCompaintTransferRequest(int ComplaintId, string TransferDistraict, string TransferFromDistraict, string TransferRemarks, string status, int TransferId, string ResionName)
        {

            string result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("SaveCCSCompaintTransferRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TransferId", DBFactoryParameterType.Int, TransferId);
                objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                objDBAccess.AddParameterToCommand("@TransferToDistrict", DBFactoryParameterType.Int, TransferDistraict);
                objDBAccess.AddParameterToCommand("@TransferFromDistrict", DBFactoryParameterType.Int, TransferFromDistraict);
                objDBAccess.AddParameterToCommand("@TransferBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@AprovedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@AprovedSSO", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, TransferRemarks);
                objDBAccess.AddParameterToCommand("@TransferBySSO", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                objDBAccess.AddParameterToCommand("@ResionName", DBFactoryParameterType.NVarchar, ResionName);
                cmd.ExecuteNonQuery();
                result = "1";
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

        public string UpdateSocietyMAPRequest(string SocietyId, string SSOId, int RequestId, string Remarks, string Status)
        {

            string result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("UpdateSocietyMAPRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@AprovedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@AprovedBySSO", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Remarks);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ReqSocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@MAPSSOId", DBFactoryParameterType.NVarchar, SSOId);
                //objDBAccess.AddParameterToCommand("@isAmendment", DBFactoryParameterType.Int, isAmendment);
                cmd.ExecuteNonQuery();
                result = "1";
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


        public DataTable GetCCSComplaintTransfer(ref DataPagingModel Details, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "0";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }


                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {

                        CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaintTransfer", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@CCSStatusId", DBFactoryParameterType.NVarchar, CCSStatusId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaintTransfer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.NVarchar, Details.SearchType);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@CCSStatusId", DBFactoryParameterType.NVarchar, CCSStatusId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetCCSComplaint(ref DataPagingModel Details, " + DistrictId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public DataTable GetEmitraTransaction(string SearchText, string ServiceType)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetEmitraTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@ServiceType", DBFactoryParameterType.NVarchar, ServiceType);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetEmitraTransaction()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public string TransferSocietiesToUser(int FromUser, int ToUser, int ActId, int RoleId)
        {
            string Result = "-1";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TransferSocietiesToUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ToUser", DBFactoryParameterType.Int, ToUser);
                objDBAccess.AddParameterToCommand("@FromUser", DBFactoryParameterType.Int, FromUser);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "TransferSocietiesToUser(int FromUser,int ToUser,int ActId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetErrorLog(ref DataPagingModel Details, int Id, int Type, int StatusId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string WhereCase = "";
                string FromDate = "", TodateDate = "";
                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    WhereCase = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "FromDate").Any())
                {
                    string date = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    date = date.Replace("-", "/");
                    FromDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0];
                }
                if (Details.SearchParameter.Where(x => x.Key == "TodateDate").Any())
                {
                    string date = Details.SearchParameter.Where(x => x.Key == "TodateDate").Select(x => x.Value).FirstOrDefault();
                    date = date.Replace("-", "/");
                    TodateDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0];
                }
                if (FromDate != "" && TodateDate == "")
                {
                    TodateDate = FromDate;
                }
                if (FromDate == "" && TodateDate != "")
                {
                    FromDate = TodateDate;
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1 && (Type == 1 || Type == 9))
                {
                    cmd = objDBAccess.CreateCommand("GetErrorLog", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.NVarchar, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.NVarchar, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.NVarchar, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                    objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                    objDBAccess.AddParameterToCommand("@TodateDate", DBFactoryParameterType.NVarchar, TodateDate);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                    objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetErrorLog", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.NVarchar, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.NVarchar, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.NVarchar, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                objDBAccess.AddParameterToCommand("@TodateDate", DBFactoryParameterType.NVarchar, TodateDate);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetErrorLog()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }


        public int ChangeGodownStatus(int GodownId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GodownDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Godownd", DBFactoryParameterType.Int, GodownId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "ChangeRoleStatus(int RoleId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataSet GetgodownDetails(int Gid)
        {
            DataSet appData = new DataSet();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDetailsforgodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
                qry = objDBAccess.getSqlCommandText();
                appData = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetgodownDetails(int Gid)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return appData;
        }
        public DataTable GetCCBDetails(int districtid)
        {
            DataTable appData = new DataTable();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCBList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, districtid);
                qry = objDBAccess.getSqlCommandText();
                appData = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetCCBDetails(int districtid)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return appData;
        }


        //public DataTable getGodownlist(int Status=0)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("getGodownlist", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyOfficeProfilingDetails()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}


        public DataTable getGodownlist(ref DataPagingModel Details, Int64 MappingId, int Status = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "";
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        if (x.Value.IndexOf("? string:0 ?") < 0)
                        {
                            whereCase += " and " + x.Key + "='" + x.Value + "'";
                        }
                    }


                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault().IndexOf("? string:0 ?") < 0)
                        {
                            CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        }
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("getGodownlist", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, MappingId);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);


                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("getGodownlist", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, MappingId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Status);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable getGodownlist(ref DataPagingModel Details, Int64 MappingId, int Status = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetTotalSocietyforreg(ref DataPagingModel Details, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        if (x.Value.IndexOf("? string:0 ?") < 0)
                        {
                            whereCase += " and " + x.Key + "='" + x.Value + "'";
                        }
                    }


                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault().IndexOf("? string:0 ?") < 0)
                        {
                            CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        }
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("getTotalScoietyForRegi", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("getTotalScoietyForRegi", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetTotalSocietyforreg(ref DataPagingModel Details, int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetTotalSociety(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string CCSStatusId = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,DepositDate,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "searchText" && x.Key != "CCSStatusId"))
                    {
                        if (x.Value.IndexOf("? string:0 ?") < 0)
                        {
                            whereCase += " and " + x.Key + "='" + x.Value + "'";
                        }
                    }


                    if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Any())
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault().IndexOf("? string:0 ?") < 0)
                        {
                            CCSStatusId = Details.SearchParameter.Where(x => x.Key == "CCSStatusId").Select(x => x.Value).FirstOrDefault();
                        }
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("getTotalScoiety", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("getTotalScoiety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetTotalSociety(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetOfflineSocietyList(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string ActId = "";
                string DistrictId = "0";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string ApplicationStatus = "";
                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                    {

                        ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "ActId").Any())
                    {

                        ActId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                    {
                        DistrictId = Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("OfflineSocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, (DistrictId == "0" ? _LoggedInUser.DistrictId : Convert.ToInt32(DistrictId)));
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("OfflineSocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, (DistrictId == "0" ? _LoggedInUser.DistrictId : Convert.ToInt32(DistrictId)));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetOfflineSocietyList(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetOfflineSocietyList_V1(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                string searchText = "";
                string ActId = "";
                string DistrictId = "0";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string ApplicationStatus = "";
                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                    {

                        ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "ActId").Any())
                    {

                        ActId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                    {

                        searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                        // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                    {
                        DistrictId = Details.SearchParameter.Where(x => x.Key == "DistrictId").Select(x => x.Value).FirstOrDefault();
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("OfflineSocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, (DistrictId == "0" ? _LoggedInUser.DistrictId : Convert.ToInt32(DistrictId)));
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("OfflineSocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, (DistrictId == "0" ? _LoggedInUser.DistrictId : Convert.ToInt32(DistrictId)));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetOfflineSocietyList(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public int GodownDetailsInsert(GodownEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("insertGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                objDBAccess.AddParameterToCommand("@SanctionAmt", DBFactoryParameterType.Int, obj.SanctionAmt);
                objDBAccess.AddParameterToCommand("@SanctionDate", DBFactoryParameterType.NVarchar, obj.SanctionDate);
                objDBAccess.AddParameterToCommand("@SanctionDoc", DBFactoryParameterType.NVarchar, obj.SanctionDoc);
                objDBAccess.AddParameterToCommand("@GodownCapacity", DBFactoryParameterType.Int, obj.GodownCapacity);
                objDBAccess.AddParameterToCommand("@LandType", DBFactoryParameterType.Int, obj.LandType);
                objDBAccess.AddParameterToCommand("@LandSize", DBFactoryParameterType.Int, obj.LandSize);
                objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, obj.HouseNo);
                objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, obj.LaneStreetName);
                objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, obj.Locality);
                objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, obj.STATE_ID);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, obj.DISTRICT_ID);
                objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, obj.CITY_ID);
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, obj.DIVISION_ID);
                objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, obj.WARD_ID);
                objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.Int, obj.PinCode);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo);
                objDBAccess.AddParameterToCommand("@longitude", DBFactoryParameterType.NVarchar, obj.Longitude);
                objDBAccess.AddParameterToCommand("@latitude", DBFactoryParameterType.NVarchar, obj.Latitude);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, obj.Description);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@ConstituencyId", DBFactoryParameterType.NVarchar, obj.ConstituencyId);
                objDBAccess.AddParameterToCommand("@KVSSGSSId", DBFactoryParameterType.NVarchar, obj.KVSSGSSId);
                objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, obj.BLOCK_ID);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, obj.GP_ID);
                objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, obj.VILLAGE_ID);
                objDBAccess.AddParameterToCommand("@SchemeName", DBFactoryParameterType.NVarchar, obj.SchemeName);
                objDBAccess.AddParameterToCommand("@CCB", DBFactoryParameterType.NVarchar, obj.CCB);
                objDBAccess.AddParameterToCommand("@CurrentStatus", DBFactoryParameterType.NVarchar, obj.CurrentStatus);
                objDBAccess.AddParameterToCommand("@Usegodown", DBFactoryParameterType.NVarchar, obj.Usegodown);
                objDBAccess.AddParameterToCommand("@IsOldGodown", DBFactoryParameterType.NVarchar, obj.IsOldGodown);
                objDBAccess.AddParameterToCommand("@ComplitionYear", DBFactoryParameterType.NVarchar, obj.ComplitionYear);


                //Result = objDBAccess.ExecuteCommand();

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && obj.TblCHCDetails != null)
                {

                    int FlagFirstRow = 1;
                    int InnerResult = 0;

                    foreach (Chcdata obj1 in obj.TblCHCDetails)
                    {
                        if (obj1.Cost > 0)
                        {

                            SqlCommand cmd2 = objDBAccess.CreateCommand("InsertGodownCHC", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, Result);
                            objDBAccess.AddParameterToCommand("@EstablishmentDate", DBFactoryParameterType.DateTime, obj.TblCHCDetails[0].EstablishmentDate);
                            objDBAccess.AddParameterToCommand("@TotalCost", DBFactoryParameterType.Int, obj.TblCHCDetails[0].Cost);
                            objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.Int, obj1.Price);
                            objDBAccess.AddParameterToCommand("@Manufacture", DBFactoryParameterType.NVarchar, obj1.Manufacture);
                            objDBAccess.AddParameterToCommand("@Instrument", DBFactoryParameterType.NVarchar, obj1.Instrument);
                            objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj1.RegistrationNo);
                            objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, 1);
                            InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                            FlagFirstRow++;
                        }
                    }


                }
                if (Result > 0 && obj.TblDetails != null)
                {

                    int FlagFirstRow = 1;
                    int InnerResult = 0;

                    foreach (Benificalmem obj1 in obj.TblDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InsertGodownCHC", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, obj1.Year);
                        objDBAccess.AddParameterToCommand("@Number", DBFactoryParameterType.Int, obj1.Number);
                        objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, 2);
                        InnerResult = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        FlagFirstRow++;
                    }

                }
                else
                {
                    Result = 1;
                }
            }
            catch (Exception ex)
            {
                Result = -1;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int GodownDetailsupdate(GodownEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("updateGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, obj.GodownId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, obj.status);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, obj.Description);
                objDBAccess.AddParameterToCommand("@ActualCost", DBFactoryParameterType.Int, obj.ActualCost);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo);
                objDBAccess.AddParameterToCommand("@Longitude", DBFactoryParameterType.NVarchar, obj.Longitude);
                objDBAccess.AddParameterToCommand("@Latitude", DBFactoryParameterType.NVarchar, obj.Latitude);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CompletionDate", DBFactoryParameterType.NVarchar, obj.CompletionDate);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int addhistory(string SSOID)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("addhistory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, SSOID);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@HostName", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetLocalIPAddress());
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "addhistory(" + SSOID + ")" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int Godownupdate(GodownEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("updateGodownwithoutstatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GodownId", DBFactoryParameterType.Int, obj.GodownId);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, obj.Description);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo);
                objDBAccess.AddParameterToCommand("@Longitude", DBFactoryParameterType.NVarchar, obj.Longitude);
                objDBAccess.AddParameterToCommand("@Latitude", DBFactoryParameterType.NVarchar, obj.Latitude);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, obj.CreatedBy);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetGodownStatus()
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetGodownStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetGodownStatus(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetGssKvssDetails(string Type, int DistrictId, int BlockId, int CityId, int GPId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetKVSSGSSDetailsByType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.NVarchar, BlockId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.NVarchar, CityId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.NVarchar, GPId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " public DataTable GetGodownStatus(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public int SocietyDistrictupdate(UpdateDistrictEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("updateScoeityDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj.RegistrationNo);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int SocietyRegistrationupdate(UpdateDistrictEntity obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("updateSocietyRegistationNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj.RegistrationNo);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyAuthorizedPersonnelDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddAttachemnt(string Name, string Type, string Registrationno, bool IsExist)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AddAttachemnt", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Registrationno", DBFactoryParameterType.NVarchar, Registrationno);
                objDBAccess.AddParameterToCommand("@IsExist", DBFactoryParameterType.NVarchar, IsExist);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "AddAttachemnt(string SWSID, string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetEmitraServiseData()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetServicesEmitraData", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetEmitraServiseData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetJANSOOCHNASocietyDetails(int DistrictId = 0, int BlockId = 0, int CityId = 0, int GPId = 0, int VillageId = 0, bool IsRural = false, string ApplicationNo = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetJANSOOCHNASocietyDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.NVarchar, BlockId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.NVarchar, CityId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.NVarchar, GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.NVarchar, VillageId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.NVarchar, IsRural);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, ApplicationNo);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GeJANSOOCHNASocietyDetaisl()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string InsertUpdateSocietyDocuments(string Id, string SocietyId, string DocumentType, string DocumentName, string DocumentURL)
        {
            string Result = "-1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateSocietyDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.NVarchar, DocumentType);
                objDBAccess.AddParameterToCommand("@DocumentName", DBFactoryParameterType.NVarchar, DocumentName);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Isactive", DBFactoryParameterType.NVarchar, true);
                objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "InsertUpdateSocietyDocuments(string SocietyIdEncr)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetSocietyDocuments(string SocietyId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDocuments()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietieForAmendment(int ActId, string SSOID, int AmendmentType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietieForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, AmendmentType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietieForAmendment(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveSocietyApplicatnDetails(string SocietyId, string SSOID, string ApplicantName, string ApploicantPost, string Type, string ReferencaeId)
        {
            string query = "";
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@ApplicantName", DBFactoryParameterType.NVarchar, ApplicantName);
                objDBAccess.AddParameterToCommand("@ApploicantPost", DBFactoryParameterType.NVarchar, ApploicantPost);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@ReferencaeId", DBFactoryParameterType.NVarchar, ReferencaeId);

                query = objDBAccess.getSqlCommandText();
                cmd.ExecuteScalar();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable SaveSocietyApplicatnDetails(string WhereClause = '')" + query }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 1;
        }

        public DataTable GetSocietyApplicantDetails(string SocietyId, int AmendmentId = 0)
        {
            DataTable appData = new DataTable();
            if (SocietyId == "") return appData;

            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                qry = objDBAccess.getSqlCommandText();
                appData = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string SocietyId, int AmendmentId = 0)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return appData;
        }
        public int RSWCUpdateRajfedId(List<RajfedPurchaseCenter> data)
        {
            int Result = 0;
            try
            {

                objDBAccess.OpenConnection();

                foreach (var item in data)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("RSWCUpdateRajfedId", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Rajfed_KVSS_Id", DBFactoryParameterType.Int, item.Rajfed_KVSS_Id);
                    objDBAccess.AddParameterToCommand("@Rajfed_PCID", DBFactoryParameterType.NVarchar, item.Rajfed_PCID);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.NVarchar, item.PurchaseCenterId);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "RSWCUpdateRajfedId(List<RajfedPurchaseCenter> data)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllPurchaseCenter()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllPurchaseCenter", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDocuments()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        #region Amit Kumar
        public DataTable GetDivisions()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("DivisionList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetDivisions()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetDistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetDistrictList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetFilterResion(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDistrictDivisionReasion", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetFilterResion()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyList(int ResionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, ResionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetFilterResion()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetFinancialYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, ResionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetFinancialYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion

        #region "RAVI RAJ SINGH"
        public int ValidateSocietybyId(string SocietyId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ValidateSocietybyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.NVarchar, SocietyId);
                return Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "ValidateSocietybyId(string SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion
        #region "Ram Swaroop"
        //public int InsertCCSSurvey(List<CCSSurveySubmitFormInvestment> objData,CCSSurveySubmitForm _Entity)
        //{
        //    //, UserEntity userName
        //    UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    int i = 0;
        //    int Result = 0;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand Cmd = objDBAccess.CreateCommand("TblAdminister",DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.ID);
        //        objDBAccess.AddParameterToCommand("@CCSType",DBFactoryParameterType.NVarchar, _Entity.CCSType);
        //        objDBAccess.AddParameterToCommand("@OfficerName",DBFactoryParameterType.NVarchar, loginuserName.OfficerNameEn);
        //        objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.NVarchar, _Entity.District);
        //        objDBAccess.AddParameterToCommand("@CCSName", DBFactoryParameterType.NVarchar, _Entity.CCSName);                
        //        objDBAccess.AddParameterToCommand("@RegiNo", DBFactoryParameterType.NVarchar, _Entity.RegiNo);
        //        objDBAccess.AddParameterToCommand("@RegiDate", DBFactoryParameterType.DateTime, _Entity.RegiDate);
        //        objDBAccess.AddParameterToCommand("@Area_of_Operation", DBFactoryParameterType.NVarchar, _Entity.Area_of_Operation);
        //        objDBAccess.AddParameterToCommand("@HO_Address", DBFactoryParameterType.NVarchar, _Entity.HO_Address);
        //        objDBAccess.AddParameterToCommand("@Num_location_branches", DBFactoryParameterType.NVarchar, _Entity.Num_location_branches);
        //        objDBAccess.AddParameterToCommand("@Ordinary", DBFactoryParameterType.NVarchar, _Entity.Ordinary);
        //        objDBAccess.AddParameterToCommand("@Nominal", DBFactoryParameterType.NVarchar, _Entity.Nominal);
        //        objDBAccess.AddParameterToCommand("@BOD_details", DBFactoryParameterType.NVarchar, _Entity.BOD_details);
        //        objDBAccess.AddParameterToCommand("@BOD_meeting_regularly_or_not", DBFactoryParameterType.NVarchar, _Entity.BOD_meeting_regularly_or_not);              
        //        objDBAccess.AddParameterToCommand("@General_meeting", DBFactoryParameterType.BigInt, _Entity.General_meeting);
        //        objDBAccess.AddParameterToCommand("@Audit_completed", DBFactoryParameterType.DateTime, _Entity.Audit_completed);
        //        objDBAccess.AddParameterToCommand("@long_audit_compliance", DBFactoryParameterType.DateTime, _Entity.long_audit_compliance);
        //        objDBAccess.AddParameterToCommand("@Status_inquiry", DBFactoryParameterType.Int, _Entity.Status_inquiry);
        //        objDBAccess.AddParameterToCommand("@Complaints_Received", DBFactoryParameterType.NVarchar, _Entity.Complaints_Received);
        //        objDBAccess.AddParameterToCommand("@Complaints_Disposed", DBFactoryParameterType.NVarchar, _Entity.Complaints_Disposed);
        //        objDBAccess.AddParameterToCommand("@EmployeeDetails", DBFactoryParameterType.NVarchar, _Entity.EmployeeDetails);
        //        objDBAccess.AddParameterToCommand("@ResolutiontakenBODRevocable", DBFactoryParameterType.VARCHAR, _Entity.ResolutiontakenBODRevocable);
        //        objDBAccess.AddParameterToCommand("@Fund_Reserve", DBFactoryParameterType.NVarchar, _Entity.Fund_Reserve);
        //        objDBAccess.AddParameterToCommand("@Deposit_SB", DBFactoryParameterType.BigInt, _Entity.Deposit_SB);
        //        objDBAccess.AddParameterToCommand("@Deposit_FD", DBFactoryParameterType.BigInt, _Entity.Deposit_FD);
        //        objDBAccess.AddParameterToCommand("@Deposit_RD", DBFactoryParameterType.BigInt, _Entity.Deposit_RD);
        //        objDBAccess.AddParameterToCommand("@appropriation_policy_applicable_in_committee", DBFactoryParameterType.NVarchar, _Entity.appropriation_policy_applicable_in_committee);
        //        objDBAccess.AddParameterToCommand("@Investment_CCB", DBFactoryParameterType.BigInt, _Entity.Investment_CCB);
        //        objDBAccess.AddParameterToCommand("@Investment_OtherBank", DBFactoryParameterType.BigInt, _Entity.Investment_OtherBank);
        //        objDBAccess.AddParameterToCommand("@Investment_Share_Purchase", DBFactoryParameterType.BigInt, _Entity.Investment_Share_Purchase);            
        //        objDBAccess.AddParameterToCommand("@Total", DBFactoryParameterType.BigInt, _Entity.Total);
        //        objDBAccess.AddParameterToCommand("@loan_Disbursement_Objective", DBFactoryParameterType.NVarchar, _Entity.loan_Disbursement_Objective);
        //        objDBAccess.AddParameterToCommand("@loan_Disbursement_Amount", DBFactoryParameterType.BigInt, _Entity.loan_Disbursement_Amount);
        //        objDBAccess.AddParameterToCommand("@Debt_Recovery_Demand", DBFactoryParameterType.NVarchar, _Entity.Debt_Recovery_Demand);
        //        objDBAccess.AddParameterToCommand("@Debt_Recovery_Recovery", DBFactoryParameterType.BigInt, _Entity.Debt_Recovery_Recovery);
        //        objDBAccess.AddParameterToCommand("@Debt_Recovery_RecoveryPer", DBFactoryParameterType.BigInt, _Entity.Debt_Recovery_RecoveryPer);
        //        objDBAccess.AddParameterToCommand("@Term_fee_CaseNumber", DBFactoryParameterType.BigInt, _Entity.Term_fee_CaseNumber);
        //        objDBAccess.AddParameterToCommand("@Term_fee_Amount", DBFactoryParameterType.BigInt, _Entity.Term_fee_Amount);
        //        objDBAccess.AddParameterToCommand("@StakeAmount_AuthorizedAmount", DBFactoryParameterType.BigInt, _Entity.StakeAmount_AuthorizedAmount);
        //        objDBAccess.AddParameterToCommand("@StakeAmount_PaidupCapital", DBFactoryParameterType.BigInt, _Entity.StakeAmount_PaidupCapital);
        //        objDBAccess.AddParameterToCommand("@StakeAmount_WorkingCapital", DBFactoryParameterType.BigInt, _Entity.StakeAmount_WorkingCapital);
        //        objDBAccess.AddParameterToCommand("@Profit_AccumulatedProfit", DBFactoryParameterType.BigInt, _Entity.Profit_AccumulatedProfit);
        //        objDBAccess.AddParameterToCommand("@Profit_YearlyProfit", DBFactoryParameterType.BigInt, _Entity.Profit_YearlyProfit);
        //        objDBAccess.AddParameterToCommand("@GeneralMeeting", DBFactoryParameterType.BigInt, _Entity.GeneralMeeting);
        //        objDBAccess.AddParameterToCommand("@ProfitDistribution", DBFactoryParameterType.BigInt, _Entity.ProfitDistribution);
        //        objDBAccess.AddParameterToCommand("@Number_estgasau_registered", DBFactoryParameterType.NVarchar, _Entity.Number_estgasau_registered);
        //        objDBAccess.AddParameterToCommand("@Physical_Verification_10_Tested", DBFactoryParameterType.NVarchar, _Entity.Physical_Verification_10_Tested);
        //        objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_Type", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_Type);
        //        objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_Bookvalue", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_Bookvalue);
        //        objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_PresentValue", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_PresentValue);
        //        objDBAccess.AddParameterToCommand("@CommentCreditPolicy", DBFactoryParameterType.NVarchar, _Entity.CommentCreditPolicy);
        //        objDBAccess.AddParameterToCommand("@Testing10BiggestDebtQuestionnaires", DBFactoryParameterType.NVarchar, _Entity.Testing10BiggestDebtQuestionnaires);
        //        objDBAccess.AddParameterToCommand("@Status_inquiry1", DBFactoryParameterType.Int, _Entity.Status_inquiry1);
        //        objDBAccess.AddParameterToCommand("@Submit_Status", DBFactoryParameterType.Int, _Entity.Submit_Status);
        //        objDBAccess.AddParameterToCommand("@Comment_compliance_auditreport_lastthreeyears", DBFactoryParameterType.NVarchar, _Entity.Comment_compliance_auditreport_lastthreeyears);
        //        objDBAccess.AddParameterToCommand("@Detailed_CommitteimplementationRCSActRulesandByelaws", DBFactoryParameterType.NVarchar, _Entity.Detailed_CommitteimplementationRCSActRulesandByelaws);
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //        if (Result > 0)
        //        {
        //            foreach (var obj in objData)
        //            {
        //                if (obj.Investment_AddMorelabel != "" && obj.Investment_AddMorelabel != null && obj.Investment_AddMore != 0)
        //                {
        //                    objDBAccess.OpenConnection();
        //                    SqlCommand Cmd1 = objDBAccess.CreateCommand("INSERT_CCS_financial_Investment", DBFactoryCommandType.PROCEDURE_COMMAND);
        //                    objDBAccess.AddParameterToCommand("@Investment_Addmore", DBFactoryParameterType.BigInt, obj.Investment_AddMore);
        //                    objDBAccess.AddParameterToCommand("@Investment_AddMorelabel", DBFactoryParameterType.NVarchar, obj.Investment_AddMorelabel);
        //                    objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.BigInt, obj.AdministrativeId);
        //                    objDBAccess.AddParameterToCommand("@CCS_AID", DBFactoryParameterType.BigInt, Result);                            
        //                    objDBAccess.ExecuteScalarCommand();
        //                }
        //                i++;
        //            }
        //        }           

        //       // return Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "RSWCUpdateRajfedId(SocietyRegistrationEntity _Model)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return Result;
        //}

        //public DataTable getccssurveydetails(string WhereClause = "")
        //{
        //    dt.Clear();
        //    try
        //    {
        //        UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("getCCSsurveydetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //       // objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.OfficerNameEn);
        //        objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditorAssignmentList(string WhereClause = '')" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}
        //public DataSet getCCSSurveydetailsview(int Gid)
        //{
        //    DataSet appData = new DataSet();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmdAddress = new SqlCommand();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyViewDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
        //        appData = objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string WhereClause = '')" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return appData;
        //}
        //public DataSet Investmentdetails1(int Gid)
        //{
        //    DataSet appData = new DataSet();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmdAddress = new SqlCommand();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyInvestmentDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
        //        appData = objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string WhereClause = '')" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return appData;
        //}
        #endregion

        //public DataTable Investmentdetails1(int Gid)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmdAddress = new SqlCommand();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyInvestmentDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string WhereClause = '')" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}
        //public int DeleteInvestmentDetails(int Id)
        //{
        //    dt.Clear();
        //        try
        //        {
        //            objDBAccess.OpenConnection();
        //            SqlCommand cmdAddress = new SqlCommand();
        //            SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyInvestmentDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //            objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Id);
        //            dt = objDBAccess.GetTable();
        //        }
        //        catch (Exception ex)
        //        {
        //            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string WhereClause = '')" }, ex);
        //        }
        //        finally
        //        {
        //            objDBAccess.CloseConnection();
        //        }
        //        return dt;            


        public DataTable GetCCSConsolidatedReportData(ref DataPagingModel Details, string fromDate, string toDate, int LoggedInUserDistrictId, int LoggedInUserMappingId, int Type = -1)
        {
            dt.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                string districtId = "0";
                string CCSId = "0";
                string searchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "District").Any())
                {
                    districtId = Details.SearchParameter.Where(x => x.Key == "District").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "CCSId").Any())
                {
                    CCSId = Details.SearchParameter.Where(x => x.Key == "CCSId").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {
                    searchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                }

                int lockedType = 0;
                if (Details.SearchParameter.Where(x => x.Key.Contains("complaintsdownloaded")).Any())
                {
                    string searchlockedType = string.Join(",", Details.SearchParameter.Where(x => x.Key.Contains("complaintsdownloaded")).Select(x => x.Value));
                    if (searchlockedType == "1")
                        lockedType = 1;
                    else if (searchlockedType == "2")
                        lockedType = 2;

                }




                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("CCSConsolidatedReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, fromDate);
                    objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.NVarchar, toDate);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                    objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                    objDBAccess.AddParameterToCommand("@LoggedInUserMappingId", DBFactoryParameterType.Int, LoggedInUserMappingId);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, Type);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.VARCHAR, searchText);
                    objDBAccess.AddParameterToCommand("@LockedType", DBFactoryParameterType.VARCHAR, lockedType);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                objDBAccess.OpenConnection();
                objDBAccess.CreateCommand("CCSConsolidatedReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, fromDate);
                objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.NVarchar, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                objDBAccess.AddParameterToCommand("@LoggedInUserMappingId", DBFactoryParameterType.Int, LoggedInUserMappingId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, Type);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.VARCHAR, searchText);
                objDBAccess.AddParameterToCommand("@LockedType", DBFactoryParameterType.VARCHAR, lockedType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkaar.Models.CitizenModel", FunctionName = "public DataTable GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int InsertCCSComplaintData(CCSComplaintDetails _Entity)
        {
            //, UserEntity userName
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("Insert_CCSComplaintDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYrMonth", DBFactoryParameterType.NVarchar, _Entity.FinYrMonth);
                objDBAccess.AddParameterToCommand("@Other_Complaints_count", DBFactoryParameterType.Int, _Entity.OtherComplaints);
                objDBAccess.AddParameterToCommand("@Documents_Received_count", DBFactoryParameterType.Int, _Entity.DocumentsReceived);
                objDBAccess.AddParameterToCommand("@Notices_Issued_count", DBFactoryParameterType.Int, _Entity.NoticesIssued);
                objDBAccess.AddParameterToCommand("@Hearing_Documents_count", DBFactoryParameterType.Int, _Entity.HearingDocuments);
                objDBAccess.AddParameterToCommand("@Pp_Documents_Shared", DBFactoryParameterType.Int, _Entity.Pp_DocumentsShared);
                objDBAccess.AddParameterToCommand("@Court_Filed_cases", DBFactoryParameterType.Int, _Entity.CourtFiled);
                objDBAccess.AddParameterToCommand("@Buds_Act_FIR_cases", DBFactoryParameterType.Int, _Entity.BudsActFIR);
                objDBAccess.AddParameterToCommand("@FIR_Registered_count", DBFactoryParameterType.Int, _Entity.FIR_Registered);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@OfficerName", DBFactoryParameterType.NVarchar, loginuserName.OfficerNameEn);
                objDBAccess.AddParameterToCommand("@DISTRICT_ENG", DBFactoryParameterType.NVarchar, loginuserName.DISTRICT_ENG);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                objDBAccess.AddParameterToCommand("@RoleName", DBFactoryParameterType.NVarchar, loginuserName.RoleName_EN);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertCCSComplaintData(CCSComplaintDetails _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int InsertCCSSurvey(List<CCSSurveySubmitFormInvestment> objData, CCSSurveySubmitForm _Entity)
        {
            //, UserEntity userName
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int i = 0;
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("TblAdminister", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.ID);
                objDBAccess.AddParameterToCommand("@CCSType", DBFactoryParameterType.NVarchar, _Entity.CCSType);
                objDBAccess.AddParameterToCommand("@OfficerName", DBFactoryParameterType.NVarchar, loginuserName.OfficerNameEn);
                objDBAccess.AddParameterToCommand("@DISTRICT_ENG", DBFactoryParameterType.NVarchar, loginuserName.DISTRICT_ENG);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                objDBAccess.AddParameterToCommand("@RoleName", DBFactoryParameterType.NVarchar, loginuserName.RoleName_EN);

                objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.NVarchar, _Entity.District);
                objDBAccess.AddParameterToCommand("@CCSName", DBFactoryParameterType.NVarchar, _Entity.CCSName);
                objDBAccess.AddParameterToCommand("@RegiNo", DBFactoryParameterType.NVarchar, _Entity.RegiNo);
                objDBAccess.AddParameterToCommand("@RegiDate", DBFactoryParameterType.DateTime, _Entity.RegiDate);
                objDBAccess.AddParameterToCommand("@Area_of_Operation", DBFactoryParameterType.NVarchar, _Entity.Area_of_Operation);
                objDBAccess.AddParameterToCommand("@HO_Address", DBFactoryParameterType.NVarchar, _Entity.HO_Address);
                objDBAccess.AddParameterToCommand("@Num_location_branches", DBFactoryParameterType.NVarchar, _Entity.Num_location_branches);
                objDBAccess.AddParameterToCommand("@Ordinary", DBFactoryParameterType.NVarchar, _Entity.Ordinary);
                objDBAccess.AddParameterToCommand("@Nominal", DBFactoryParameterType.NVarchar, _Entity.Nominal);
                objDBAccess.AddParameterToCommand("@BOD_details", DBFactoryParameterType.NVarchar, _Entity.BOD_details);
                objDBAccess.AddParameterToCommand("@BOD_meeting_regularly_or_not", DBFactoryParameterType.NVarchar, _Entity.BOD_meeting_regularly_or_not);
                objDBAccess.AddParameterToCommand("@General_meeting", DBFactoryParameterType.BigInt, _Entity.General_meeting);
                objDBAccess.AddParameterToCommand("@Audit_completed", DBFactoryParameterType.DateTime, _Entity.Audit_completed);
                objDBAccess.AddParameterToCommand("@long_audit_compliance", DBFactoryParameterType.DateTime, _Entity.long_audit_compliance);
                objDBAccess.AddParameterToCommand("@Status_inquiry", DBFactoryParameterType.Int, _Entity.Status_inquiry);
                objDBAccess.AddParameterToCommand("@Complaints_Received", DBFactoryParameterType.NVarchar, _Entity.Complaints_Received);
                objDBAccess.AddParameterToCommand("@Complaints_Disposed", DBFactoryParameterType.NVarchar, _Entity.Complaints_Disposed);
                objDBAccess.AddParameterToCommand("@EmployeeDetails", DBFactoryParameterType.NVarchar, _Entity.EmployeeDetails);
                objDBAccess.AddParameterToCommand("@ResolutiontakenBODRevocable", DBFactoryParameterType.VARCHAR, _Entity.ResolutiontakenBODRevocable);
                objDBAccess.AddParameterToCommand("@Fund_Reserve", DBFactoryParameterType.NVarchar, _Entity.Fund_Reserve);
                objDBAccess.AddParameterToCommand("@Deposit_SB", DBFactoryParameterType.BigInt, _Entity.Deposit_SB);
                objDBAccess.AddParameterToCommand("@Deposit_FD", DBFactoryParameterType.BigInt, _Entity.Deposit_FD);
                objDBAccess.AddParameterToCommand("@Deposit_RD", DBFactoryParameterType.BigInt, _Entity.Deposit_RD);
                objDBAccess.AddParameterToCommand("@appropriation_policy_applicable_in_committee", DBFactoryParameterType.NVarchar, _Entity.appropriation_policy_applicable_in_committee);
                objDBAccess.AddParameterToCommand("@Investment_CCB", DBFactoryParameterType.BigInt, _Entity.Investment_CCB);
                objDBAccess.AddParameterToCommand("@Investment_OtherBank", DBFactoryParameterType.BigInt, _Entity.Investment_OtherBank);
                objDBAccess.AddParameterToCommand("@Investment_Share_Purchase", DBFactoryParameterType.BigInt, _Entity.Investment_Share_Purchase);
                objDBAccess.AddParameterToCommand("@Total", DBFactoryParameterType.BigInt, _Entity.Total);
                objDBAccess.AddParameterToCommand("@loan_Disbursement_Objective", DBFactoryParameterType.NVarchar, _Entity.loan_Disbursement_Objective);
                objDBAccess.AddParameterToCommand("@loan_Disbursement_Amount", DBFactoryParameterType.BigInt, _Entity.loan_Disbursement_Amount);
                objDBAccess.AddParameterToCommand("@Debt_Recovery_Demand", DBFactoryParameterType.NVarchar, _Entity.Debt_Recovery_Demand);
                objDBAccess.AddParameterToCommand("@Debt_Recovery_Recovery", DBFactoryParameterType.BigInt, _Entity.Debt_Recovery_Recovery);
                objDBAccess.AddParameterToCommand("@Debt_Recovery_RecoveryPer", DBFactoryParameterType.BigInt, _Entity.Debt_Recovery_RecoveryPer);
                objDBAccess.AddParameterToCommand("@Term_fee_CaseNumber", DBFactoryParameterType.BigInt, _Entity.Term_fee_CaseNumber);
                objDBAccess.AddParameterToCommand("@Term_fee_Amount", DBFactoryParameterType.BigInt, _Entity.Term_fee_Amount);
                objDBAccess.AddParameterToCommand("@StakeAmount_AuthorizedAmount", DBFactoryParameterType.BigInt, _Entity.StakeAmount_AuthorizedAmount);
                objDBAccess.AddParameterToCommand("@StakeAmount_PaidupCapital", DBFactoryParameterType.BigInt, _Entity.StakeAmount_PaidupCapital);
                objDBAccess.AddParameterToCommand("@StakeAmount_WorkingCapital", DBFactoryParameterType.BigInt, _Entity.StakeAmount_WorkingCapital);
                objDBAccess.AddParameterToCommand("@Profit_AccumulatedProfit", DBFactoryParameterType.BigInt, _Entity.Profit_AccumulatedProfit);
                objDBAccess.AddParameterToCommand("@Profit_YearlyProfit", DBFactoryParameterType.BigInt, _Entity.Profit_YearlyProfit);
                objDBAccess.AddParameterToCommand("@GeneralMeeting", DBFactoryParameterType.BigInt, _Entity.GeneralMeeting);
                objDBAccess.AddParameterToCommand("@ProfitDistribution", DBFactoryParameterType.BigInt, _Entity.ProfitDistribution);
                objDBAccess.AddParameterToCommand("@Number_estgasau_registered", DBFactoryParameterType.NVarchar, _Entity.Number_estgasau_registered);
                objDBAccess.AddParameterToCommand("@Physical_Verification_10_Tested", DBFactoryParameterType.NVarchar, _Entity.Physical_Verification_10_Tested);
                objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_Type", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_Type);
                objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_Bookvalue", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_Bookvalue);
                objDBAccess.AddParameterToCommand("@immovablepropertyheld_Society_PresentValue", DBFactoryParameterType.BigInt, _Entity.immovablepropertyheld_Society_PresentValue);
                objDBAccess.AddParameterToCommand("@CommentCreditPolicy", DBFactoryParameterType.NVarchar, _Entity.CommentCreditPolicy);
                objDBAccess.AddParameterToCommand("@Testing10BiggestDebtQuestionnaires", DBFactoryParameterType.NVarchar, _Entity.Testing10BiggestDebtQuestionnaires);
                objDBAccess.AddParameterToCommand("@Status_inquiry1", DBFactoryParameterType.Int, _Entity.Status_inquiry1);
                objDBAccess.AddParameterToCommand("@Submit_Status", DBFactoryParameterType.Int, _Entity.Submit_Status);
                objDBAccess.AddParameterToCommand("@Comment_compliance_auditreport_lastthreeyears", DBFactoryParameterType.NVarchar, _Entity.Comment_compliance_auditreport_lastthreeyears);
                objDBAccess.AddParameterToCommand("@Detailed_CommitteimplementationRCSActRulesandByelaws", DBFactoryParameterType.NVarchar, _Entity.Detailed_CommitteimplementationRCSActRulesandByelaws);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                if (Result > 0)
                {
                    foreach (var obj in objData)
                    {
                        if (obj.Investment_AddMorelabel != "" && obj.Investment_AddMorelabel != null && obj.Investment_AddMore != 0)
                        {
                            objDBAccess.OpenConnection();
                            SqlCommand Cmd1 = objDBAccess.CreateCommand("INSERT_CCS_financial_Investment", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@Investment_Addmore", DBFactoryParameterType.BigInt, obj.Investment_AddMore);
                            objDBAccess.AddParameterToCommand("@Investment_AddMorelabel", DBFactoryParameterType.NVarchar, obj.Investment_AddMorelabel);
                            objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.BigInt, obj.AdministrativeId);
                            objDBAccess.AddParameterToCommand("@CCS_AID", DBFactoryParameterType.BigInt, Result);
                            objDBAccess.ExecuteScalarCommand();
                        }
                        i++;
                    }
                }

                // return Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertCCSSurvey(List<CCSSurveySubmitFormInvestment> objData, CCSSurveySubmitForm _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        //public DataSet getCCSSurveydetailsview(int Gid)
        //{
        //    DataSet appData = new DataSet();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmdAddress = new SqlCommand();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyViewDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
        //        appData = objDBAccess.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable getCCSSurveydetailsview(string WhereClause = '')" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return appData;
        //}
        public DataTable getccssurveydetails(string WhereClause = "")
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (loginuserName.DistrictId != 0)
            {
                WhereClause = " CCS.INS_DistrictId =" + loginuserName.DistrictId;

            }
            else
            {
                WhereClause = " where 1=1";
            }
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("getCCSsurveydetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable getccssurveydetails(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet getCCSSurveydetailsview(int Gid)
        {
            DataSet appData = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyViewDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
                appData = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable getCCSSurveydetailsview(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return appData;
        }
        public DataTable GetCCSData(string WhereClause, int CCSId = -1, int Type = -1)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("CCSData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistID", DBFactoryParameterType.Int, WhereClause);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.GetCCSData", FunctionName = "public DataTable GetCCSData(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable Investmentdetails1(int Gid)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSSurveyInvestmentDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Gid", DBFactoryParameterType.Int, Gid);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable Investmentdetails1(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable getCCSSurveydetailsforwork(string WhereClause = "")
        {
            //int DistrictId;
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (loginuserName.DistrictId != 0)
            {
                WhereClause = " and CCS.INS_DistrictId =" + loginuserName.DistrictId;

            }
            else
            {
                WhereClause = " and 1=1";
            }
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("getCCSsurveydetailsforDR", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable getCCSSurveydetailsforwork(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertActiononCCSSurvey(CCSSurveySubmitForm _Entity)
        {
            //, UserEntity userName
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("TblAdministertakeaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.ID);
                objDBAccess.AddParameterToCommand("@Submit_Status", DBFactoryParameterType.Int, _Entity.Submit_Status);
                objDBAccess.AddParameterToCommand("@TakeactionStatus", DBFactoryParameterType.Int, _Entity.TakeactionStatus);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, _Entity.Remarks);
                objDBAccess.AddParameterToCommand("@RoleIDDr", DBFactoryParameterType.Int, loginuserName.RoleId);
                objDBAccess.AddParameterToCommand("@RoleNameOFDR", DBFactoryParameterType.NVarchar, loginuserName.RoleName_EN);
                objDBAccess.AddParameterToCommand("@Approvedbydr", DBFactoryParameterType.NVarchar, loginuserName.OfficerNameEn);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertActiononCCSSurvey(CCSSurveySubmitForm _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


        #region ObjectedSocieties by madhuri 2 jan 2025

        public DataTable GetSocityDataForObjection(string searchText = "")
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_ObjectedSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyDetails");
                objDBAccess.AddParameterToCommand("@searchText", DBFactoryParameterType.NVarchar, searchText);
                dt = objDBAccess.GetTable();


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSocityDataForObjection(string searchText=\"\", int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int insertObjectedSocieties(string Remark = "", int societyId = 0, string AmendmentIds = "", string doc = "")
        {

            int Result = -1;
            UserEntity _loggedUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_ObjectedSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "insertObjectedSocieties");
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@societyId", DBFactoryParameterType.Int, societyId);
                objDBAccess.AddParameterToCommand("@AmendmentIds", DBFactoryParameterType.NVarchar, AmendmentIds);
                objDBAccess.AddParameterToCommand("@Doc", DBFactoryParameterType.NVarchar, doc);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _loggedUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIp", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());

                //dt = objDBAccess.GetTable();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "insertObjectedSocieties(string Remark=\"\",int societyId=0, string AmendmentIds=\"\",string doc=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        public DataTable AllObjectedSocieties(string searchText = "")
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_ObjectedSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetObjectedSocieties");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSocityDataForObjection(string searchText=\"\", int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public int ChangeStatus(int Id = 0)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_ObjectedSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ChangeStatus");
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                Result = Convert.ToInt32(objDBAccess.ExecuteCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSocityDataForObjection(string searchText=\"\", int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        #endregion

        #region society Directory copy one to another

        public DataTable GetAllUpdatedRegNoSocieties(string searchText = "")
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_MoveDirectorySocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetAllUpdatedRegNoSocieties");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetAllUpdatedRegNoSocieties(string searchText = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public int RegNoChangeStatus(int SocietyId = 0)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("sp_MoveDirectorySocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ChangeStatus");
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                Result = Convert.ToInt32(objDBAccess.ExecuteCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "RegNoChangeStatus(int Id = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        #endregion

        #region RemoveUnwantedFilesOrDuplicateFiles in societyProfiling
        public DataTable GetAlldocumentsRelatedToSociety(int SocietyId)
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("All_SocietyDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSocityDataForObjection(string searchText=\"\", int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        #endregion

        #region MergeSocieties
        public int MergeSocieties(string OldSocietyIds, int NewSocietyId, string SocietyName = "", string Remark = "")
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MergeSocieties2001", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "MergeSocieties");
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, SocietyName);
                objDBAccess.AddParameterToCommand("@NewSocietyId", DBFactoryParameterType.Int, NewSocietyId);
                objDBAccess.AddParameterToCommand("@OldSocietyIds", DBFactoryParameterType.NVarchar, OldSocietyIds);


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " MergeSocieties(string OldSocietyIds, int NewSocietyId, string SocietyName = \"\", string Remark = \"\")" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion

        #region SocietyAuditList
        public DataTable SocietyListBydepartment_2(ref DataPagingModel Details, int LevelId, int UserRoleDepartmentId, int Division_District_Id)
        {
            dt.Clear();
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                //string whereCase = " ";
                string searchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {
                    searchText = Details.SearchParameter.Where(x => x.Key == "searchText").FirstOrDefault().Value;
                }
                int IsForEmpanelledAgency = 0;
                if (Details.SearchParameter.Where(x => x.Key == "IsForEmpanelledAgency").Any())
                {
                    IsForEmpanelledAgency = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "IsForEmpanelledAgency").FirstOrDefault().Value);
                }
                int FYearId = 0;
                if (Details.SearchParameter.Where(x => x.Key == "FYearId").Any())
                {
                    FYearId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "FYearId").FirstOrDefault().Value);
                }
                int CategoryId = 0;
                if (Details.SearchParameter.Where(x => x.Key == "CategoryId").Any())
                {
                    CategoryId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "CategoryId").FirstOrDefault().Value);
                }
                int DistrictId = LevelId == 3 ? Division_District_Id : 0;
                if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                {
                    DistrictId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "DistrictId").FirstOrDefault().Value);
                }
                int DivisionId = LevelId == 2 ? Division_District_Id : 0; ;
                if (Details.SearchParameter.Where(x => x.Key == "DivisionId").Any())
                {
                    DivisionId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "DivisionId").FirstOrDefault().Value);
                }
                int ResionId = 0;
                if (Details.SearchParameter.Where(x => x.Key == "Resion").Any())
                {
                    ResionId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "Resion").FirstOrDefault().Value);
                }
                int SocietyTypeId = 0;
                if (Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").Any())
                {
                    SocietyTypeId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").FirstOrDefault().Value);
                }

                cmd = objDBAccess.CreateCommand("SocietyListBydepartment_2_v3", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@where", DBFactoryParameterType.Int, IsForEmpanelledAgency);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@RegionId", DBFactoryParameterType.Int, ResionId);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentId", DBFactoryParameterType.Int, UserRoleDepartmentId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, CategoryId);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, FYearId);
                objDBAccess.AddParameterToCommand("@IsForEmpanelledAgency", DBFactoryParameterType.Int, IsForEmpanelledAgency);
                objDBAccess.AddParameterToCommand("@LevelID", DBFactoryParameterType.Int, Division_District_Id);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                // Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                dt = objDBAccess.GetTable();
                Details.TotalRecords = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0]["TotalRecord"]) : 0;


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyListBydepartment_2(ref DataPagingModel Details, int LevelId, int UserRoleDepartmentId, int Division_District_Id)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion

        public DataTable GetGSSTargetProceesStatusListIdsByIds(int StatusId, string InspactorTargetsId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGssTargetProceesStatusListByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, InspactorTargetsId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetGSSTargetProceesStatusListIdsByIds(int StatusId, string InspactorTargetsId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #region
        public int UpdatePdcsDetails(PdcsModel pdcs)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("p_PdcsRcdfProfileUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, pdcs.RegNo);
                objDBAccess.AddParameterToCommand("@PdcsName", DBFactoryParameterType.NVarchar, pdcs.PdcsName);
                objDBAccess.AddParameterToCommand("@PresidentName", DBFactoryParameterType.NVarchar, pdcs.PresidentName);
                objDBAccess.AddParameterToCommand("@AdharNo", DBFactoryParameterType.VARCHAR, pdcs.AdharNo);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, pdcs.MobileNo);
                objDBAccess.AddParameterToCommand("@AverageMilk", DBFactoryParameterType.VARCHAR, pdcs.AverageMilk);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, pdcs.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, loginuserName.UserId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdatePdcsDetails(PdcsModel pdcs)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        #endregion


        #region
        //public int RevertLetter(int InspectorTargetId)
        //{
        //    UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        //    int Result = 0;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand Cmd = objDBAccess.CreateCommand("RevertSanction", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@InspectorTargetId", DBFactoryParameterType.Int, InspectorTargetId);             

        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdatePdcsDetails(PdcsModel pdcs)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return Result;
        //}
        public int RevertLetter(string InspectorTargetIds)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("RevertSanction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspectorTargetIds", DBFactoryParameterType.NVarchar, InspectorTargetIds);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UpdatePdcsDetails(PdcsModel pdcs)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        #endregion


        public DataTable GetAGMMultiInspectorDetails(int InspactorTargetsId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAGMMultiInspectorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, InspactorTargetsId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetAGMMultiInspectorDetails(int InspactorTargetsId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetInspectorDetailsForCertificate(string InspactorTargetsId)
        {
            DataTable dt1 = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetInspectorDetailsForCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.NVarchar, InspactorTargetsId);
                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
            }
            return dt1;
        }


        #region 'GetCSSComplaintOnlineDistrictWise'

        public DataSet GetCSSComplaintOnlineDistrictWise(int districtId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetCSSComplaintOnlineDistrictWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCSSComplaintOnlineDistrictWise" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        #endregion


        #region 'GetCCSComplaintList'
        public DataSet GetCCSComplaintList()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_CCSComplaintDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetCCSComplaintList" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        #endregion

        public DataTable SocietyListWithDepartmentUser(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;

                if (Obj != null && Obj.SSOID != null)
                    WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";

                if (Obj.SocietyIDEnc != null && Obj.UserType == "K")
                {
                    WhereClause += Obj.SocietyIDEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + Obj.SocietyIDEnc + "')as int) ") : "";
                }

                WhereClause += Obj.ActId > 0 ? (WhereClause = " and [Societies].ActId =" + Obj.ActId + "") : "";
                WhereClause += Obj.SocietyTypeId > 0 ? (WhereClause = " and Societies.[SocietyTypeId] =" + Obj.SocietyTypeId + "") : "";
                WhereClause += Obj.ClassId > 0 ? (WhereClause = " and Societies.[ClassId] =" + Obj.ClassId + "") : "";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Election_SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, Obj.SSOID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GodownModel", FunctionName = "SocietyListWithDepartmentUser(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable KVSSGSSbyDRorMDCCB(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_KVSSGSSbyDRorMDCCB", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, Obj.UserType);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, Obj.SSOID);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, Obj.DistrictId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, Obj.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, Obj.RoleId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "KVSSGSSbyDRorMDCCB(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }


        public DataTable GetReasonForVacancyDetailByListId(string ListCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetReasonForVacancyDetailByListId(string ListCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyDetailBySocietyId(int SocietyId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSocietyDetailBySocietyId(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int ElectionDetailsInsert(ElectionFormEntity obj)
        {
            int Result = -1;

            UserEntity _LoginUserId = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertElectionDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionId", DBFactoryParameterType.Int, obj.ElectionId);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, obj.SocietyID);
                objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.Int, obj.SocietyCategoryId);
                objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                objDBAccess.AddParameterToCommand("@ReasonForVacancyId", DBFactoryParameterType.Int, obj.ReasonForVacancyId);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj.RegistrationNo);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, obj.RegistrationDate);
                objDBAccess.AddParameterToCommand("@LastElectionDate", DBFactoryParameterType.VARCHAR, obj.LastElectionDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.VARCHAR, obj.ElectionDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@AdminAppointmentDate", DBFactoryParameterType.VARCHAR, obj.AdminAppointmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@DeputyAdminAppointmentDate", DBFactoryParameterType.VARCHAR, obj.DeputyAdminAppointmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ReasonForNonElection", DBFactoryParameterType.NVarchar, obj.ReasonForNonElection);
                objDBAccess.AddParameterToCommand("@PostName", DBFactoryParameterType.NVarchar, obj.PostName);
                objDBAccess.AddParameterToCommand("@ExcludedVoterListDocument", DBFactoryParameterType.NVarchar, obj.ExcludedVoterListDocument);
                objDBAccess.AddParameterToCommand("@BylawsDocument", DBFactoryParameterType.NVarchar, obj.BylawsDocument);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoginUserId.UserId);
                objDBAccess.AddParameterToCommand("@IsNewSociety", DBFactoryParameterType.Bit, obj.IsNewSociety);
                objDBAccess.AddParameterToCommand("@FirstBoardNominationDate", DBFactoryParameterType.VARCHAR, obj.FirstBoardNominationDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@IsVacantPost", DBFactoryParameterType.Bit, obj.IsVacantPost);
                objDBAccess.AddParameterToCommand("@RecommendationDocument", DBFactoryParameterType.NVarchar, obj.RecommendationDocument);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && obj.VacancyList != null)
                {
                    foreach (var item in obj.VacancyList)
                    {
                        SqlCommand vacancyCmd = objDBAccess.CreateCommand("InsertSocietyElectionVacancy", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@VacancyId", DBFactoryParameterType.Int, item.VacancyId);
                        objDBAccess.AddParameterToCommand("@ElectionId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@VacantPostId", DBFactoryParameterType.Int, item.VacantPostId);
                        objDBAccess.AddParameterToCommand("@VacancyDate", DBFactoryParameterType.VARCHAR, item.VacancyDate);
                        objDBAccess.AddParameterToCommand("@ReasonForVacancyId", DBFactoryParameterType.Int, item.ReasonForVacancyId);
                        objDBAccess.AddParameterToCommand("@OtherReason", DBFactoryParameterType.NVarchar, item.OtherReason);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoginUserId.UserId);
                        objDBAccess.ExecuteScalarCommand();
                    }
                }

            }
            catch (Exception ex)
            {
                Result = -1;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "ElectionDetailsInsert(SocietyAuthorizedPersonEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int InsertFertilizerDemandDetails(FertilizerFormEntity obj)
        {
            int Result = -1;

            UserEntity _LoginUserId = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {

                DataTable dtFertilizerDemandDetails = new DataTable();

                dtFertilizerDemandDetails.Columns.Add("FertilizerItemId", typeof(int));
                dtFertilizerDemandDetails.Columns.Add("SocietyId", typeof(int));
                dtFertilizerDemandDetails.Columns.Add("MonthId", typeof(int));
                dtFertilizerDemandDetails.Columns.Add("YearId", typeof(int));

                if (obj.DemandList != null)
                {
                    foreach (var item in obj.DemandList)
                    {
                        DataRow row = dtFertilizerDemandDetails.NewRow();

                        row["FertilizerItemId"] = item.FertilizerItemId;
                        row["SocietyId"] = obj.SocietyID;
                        row["MonthId"] = item.MonthId;
                        row["YearId"] = item.YearId;

                        dtFertilizerDemandDetails.Rows.Add(row);
                    }
                }
                 
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertFertilizerDemandRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FDRId", DBFactoryParameterType.Int, obj.FDRId);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, obj.SocietyID);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj.RegistrationNo);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, obj.DistrictName);
                objDBAccess.AddParameterToCommand("@ManagerName", DBFactoryParameterType.NVarchar, obj.SecretaryName);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.SecretaryMobNo);
                objDBAccess.AddParameterToCommand("@IsLicense", DBFactoryParameterType.Bit, obj.IsLicense);
                objDBAccess.AddParameterToCommand("@LicenseNo", DBFactoryParameterType.NVarchar, obj.LicenseNo);
                objDBAccess.AddParameterToCommand("@IsCreditFacility", DBFactoryParameterType.Bit, obj.IsCreditFacility);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.VARCHAR, obj.DemandMonth);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.VARCHAR, obj.DemandYear);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoginUserId.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, obj.GPId);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, obj.SocietyType);
                objDBAccess.AddParameterToCommand("@IFMSPOSId", DBFactoryParameterType.NVarchar, obj.IFMSPOSId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoginUserId.UserRoleDeptMappingId);

                SqlParameter parameter = cmd.Parameters.Add("@FertilizerDemandDetails", SqlDbType.Structured);
                parameter.TypeName = "dbo.FertilizerDemandDetailsType";
                parameter.Value = dtFertilizerDemandDetails;

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && obj.DemandList != null)
                {
                    foreach (var item in obj.DemandList)
                    {
                        SqlCommand vacancyCmd = objDBAccess.CreateCommand("InsertFertilizerDemandDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DemandId", DBFactoryParameterType.Int, item.DemandId);
                        objDBAccess.AddParameterToCommand("@FDRId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@FertilizerItemId", DBFactoryParameterType.Int, item.FertilizerItemId);
                        objDBAccess.AddParameterToCommand("@FertilizerCompany", DBFactoryParameterType.NVarchar, item.FertilizerCompany);
                        objDBAccess.AddParameterToCommand("@FertilizerQuantity", DBFactoryParameterType.Float, item.FertilizerQuantity);
                        objDBAccess.AddParameterToCommand("@FertilizerRate", DBFactoryParameterType.Float, 0);
                        objDBAccess.AddParameterToCommand("@FertilizerCost", DBFactoryParameterType.Float, 0);
                        objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, item.MonthId);
                        objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.Int, item.YearId);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoginUserId.UserId);
                        objDBAccess.AddParameterToCommand("@OtherFertilizerName", DBFactoryParameterType.NVarchar, item.OtherFertilizerName);
                        objDBAccess.ExecuteScalarCommand();
                    }
                }

                if (Result > 0 && obj.CreditFacilityList != null)
                {
                    foreach (var item in obj.CreditFacilityList)
                    {
                        SqlCommand vacancyCmd = objDBAccess.CreateCommand("InsertFertilizerCreditFacilityDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@CreditFacilityId", DBFactoryParameterType.Int, item.CreditFacilityId);
                        objDBAccess.AddParameterToCommand("@FDRId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@FertilizerItemId", DBFactoryParameterType.Int, item.FertilizerItemId);
                        objDBAccess.AddParameterToCommand("@FertilizerSale", DBFactoryParameterType.Float, item.FertilizerSale);
                        objDBAccess.AddParameterToCommand("@FertilizerQuantity", DBFactoryParameterType.Float, item.FertilizerQuantity);
                        objDBAccess.AddParameterToCommand("@FertilizerRate", DBFactoryParameterType.Float, 0);
                        objDBAccess.AddParameterToCommand("@FertilizerCost", DBFactoryParameterType.Float, 0);
                        objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, item.MonthId);
                        objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.Int, item.YearId);
                        objDBAccess.AddParameterToCommand("@PresentAvailability", DBFactoryParameterType.Float, item.PresentAvailability);
                        objDBAccess.AddParameterToCommand("@CreditLimit", DBFactoryParameterType.Float, item.CreditLimit);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoginUserId.UserId);
                        objDBAccess.AddParameterToCommand("@OtherFertilizerName", DBFactoryParameterType.NVarchar, item.OtherFertilizerName);
                        objDBAccess.ExecuteScalarCommand();
                    }
                }

                if (Result > 0 && obj.PurchaseList != null)
                {
                    foreach (var item in obj.PurchaseList)
                    {
                        SqlCommand vacancyCmd = objDBAccess.CreateCommand("InsertPurchaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@PurchaseId", DBFactoryParameterType.Int, item.PurchaseId);
                        objDBAccess.AddParameterToCommand("@FDRId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@FertilizerItemId", DBFactoryParameterType.Int, item.FertilizerItemId);
                        objDBAccess.AddParameterToCommand("@FertilizerQuantity", DBFactoryParameterType.Float, item.FertilizerQuantity);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoginUserId.UserId);
                        objDBAccess.ExecuteScalarCommand();
                    }
                }

            }
            catch (Exception ex)
            {
                Result = -1;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertFertilizerDemandDetails(FertilizerFormEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int InsertTaggingItemDetails(TaggedItemFormEntity obj)
        {
            int Result = -1;

            UserEntity _LoginUserId = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_InsertTaggingItem", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TagId", DBFactoryParameterType.Int, obj.TagId);
                objDBAccess.AddParameterToCommand("@ProductTypeId", DBFactoryParameterType.Int, obj.ProductTypeId);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, obj.ProductId);
                objDBAccess.AddParameterToCommand("@CompanyId", DBFactoryParameterType.Int, obj.CompanyId);
                objDBAccess.AddParameterToCommand("@PurchaseDate", DBFactoryParameterType.NVarchar, obj.PurchaseDate);
                objDBAccess.AddParameterToCommand("@ExpirationDate", DBFactoryParameterType.NVarchar, obj.ExpirationDate);
                objDBAccess.AddParameterToCommand("@PackagingType", DBFactoryParameterType.NVarchar, obj.PackagingType);
                objDBAccess.AddParameterToCommand("@MeasurementUnit", DBFactoryParameterType.NVarchar, obj.MeasurementUnit);
                objDBAccess.AddParameterToCommand("@ItemQuantity", DBFactoryParameterType.Float, obj.ItemQuantity);
                objDBAccess.AddParameterToCommand("@ItemPrice", DBFactoryParameterType.Float, obj.ItemPrice);
                objDBAccess.AddParameterToCommand("@TotalPrice", DBFactoryParameterType.Float, obj.TotalPrice);
                objDBAccess.AddParameterToCommand("@CompanyName", DBFactoryParameterType.NVarchar, obj.CompanyName);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoginUserId.UserId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoginUserId.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, obj.SocietyID);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, obj.RegistrationNo);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, obj.DistrictName);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.BlockId);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, obj.SocietyType);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                Result = -1;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertTaggingItemDetails(TaggedItemFormEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataSet GetTaggingItemByUser()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetTaggingItemByUser", DBFactoryCommandType.PROCEDURE_COMMAND); 
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId); 
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetTaggingItemByUser()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetTaggedItemDetailById(int TagId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetTaggedItemDetailById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TagId", DBFactoryParameterType.BigInt, TagId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Society", FunctionName = "GetTaggedItemDetailById(int TagId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataTable DeleteTaggedItemById(int TagId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_DeleteTaggedItemById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TagId", DBFactoryParameterType.BigInt, TagId);
                return objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Society", FunctionName = "DeleteTaggedItemById(int TagId = 0)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataTable GetElectionList()
        {
            UserEntity _LoginUserId = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable dt = new DataTable();

            objDBAccess.OpenConnection();

            SqlCommand cmd = objDBAccess.CreateCommand("GetElectionList", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoginUserId.UserId);
            objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoginUserId.RoleId);

            dt = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return dt;
        }

        public int DeleteElection(int ElectionId)
        {
            int Result = 0;

            objDBAccess.OpenConnection();

            SqlCommand cmd = objDBAccess.CreateCommand("DeleteElection", DBFactoryCommandType.PROCEDURE_COMMAND);

            objDBAccess.AddParameterToCommand("@ElectionId", DBFactoryParameterType.Int, ElectionId);

            Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            objDBAccess.CloseConnection();

            return Result;
        }


        public int SaveInspactorStatusDRForNewEsign(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string IDs = "")
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string CreatedIP = COMMON_FUNCTION.GetIpAddress();

            foreach (var item in InspactorStatusDetails)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Insert_InspectorStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@InspactorTargetsId", DBFactoryParameterType.Int, item.InspactorTargetsId);
                    objDBAccess.AddParameterToCommand("@Inspector_ID", DBFactoryParameterType.Int, item.Inspector_ID);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, status);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, item.Remarks);
                    objDBAccess.AddParameterToCommand("@Documentname", DBFactoryParameterType.NVarchar, Inspectorfile);
                    objDBAccess.AddParameterToCommand("@PrecentNameGSS", DBFactoryParameterType.NVarchar, item.PrecentNameGSS);
                    objDBAccess.AddParameterToCommand("@NewGSSName", DBFactoryParameterType.NVarchar, item.NewGSSName);
                    objDBAccess.AddParameterToCommand("@NoofMember", DBFactoryParameterType.NVarchar, item.NoofMember);
                    objDBAccess.AddParameterToCommand("@NewGSSprecious", DBFactoryParameterType.NVarchar, item.NewGSSprecious);
                    objDBAccess.AddParameterToCommand("@NewGSSFinanceAmount", DBFactoryParameterType.NVarchar, item.NewGSSFinanceAmount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);


                    objDBAccess.AddParameterToCommand("@DateOfSubmission", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(item.DateOfSubmission) ? "" : item.DateOfSubmission);
                    objDBAccess.AddParameterToCommand("@DateOfApproval", DBFactoryParameterType.NVarchar, item.DateOfApproval);

                    objDBAccess.AddParameterToCommand("@SanctionDate", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(item.SanctionDate) ? "" : item.SanctionDate);
                    objDBAccess.AddParameterToCommand("@SanctionLetter", DBFactoryParameterType.NVarchar, item.SanctionLetter);
                    objDBAccess.AddParameterToCommand("@IDs", DBFactoryParameterType.NVarchar, IDs);

                    //string SenctionDate
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InspactorStatusDetails" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            return Result;
        }

        public DataTable GetElectionSocietyCategoryList()
        {
            DataTable dt = new DataTable();

            objDBAccess.OpenConnection();

            SqlCommand cmd = objDBAccess.CreateCommand("GetElectionSocietyCategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);

            dt = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return dt;
        }


        public DataTable GetElectionVacancyList(int ElectionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyElectionVacancyByElectionId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionId", DBFactoryParameterType.Int, ElectionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetElectionVacancyList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable SaveKPIForPacs(KPIForPacsSaveModel model)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_SaveKPIForPacs", DBFactoryCommandType.PROCEDURE_COMMAND);

                cmd.Parameters.AddWithValue("@action", "SaveData");
                cmd.Parameters.AddWithValue("@SocietyId", model.SocietyId);
                cmd.Parameters.AddWithValue("@OwnBuilding", (object)model.OwnBuilding ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LandAvailable", (object)model.LandAvailable ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GodownAvailable", (object)model.GodownAvailable ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GodownlessAvailable", (object)model.GodownlessAvailable ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Business", (object)model.Business ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MembershipIncrease", (object)model.MembershipIncrease ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@loaneemember", (object)model.loaneemember ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@profit", (object)model.profit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sharecapital", (object)model.sharecapital ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deposit", (object)model.deposit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imbalance", (object)model.imbalance ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dividend", (object)model.dividend ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@otherKCCLoan", (object)model.otherKCCLoan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@kccrecovery", (object)model.kccrecovery ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@gccLoan", (object)model.gccLoan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@otherLoanrecvry", (object)model.otherLoanrecvry ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@auditcompleted", (object)model.auditcompleted ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@auditclassfiton", (object)model.auditclassfiton ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@agmconvend", (object)model.agmconvend ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@bod", (object)model.bod ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@compliancaudit", (object)model.compliancaudit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Fertilizer", (object)model.Fertilizer ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Seed", (object)model.Seed ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pesticide", (object)model.Pesticide ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Animal", (object)model.Animal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CSC", (object)model.CSC ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PMKSK", (object)model.PMKSK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CHC", (object)model.CHC ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Engaged", (object)model.Engaged ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FPO", (object)model.FPO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PMBJK", (object)model.PMBJK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@KUSUM", (object)model.KUSUM ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PDS", (object)model.PDS ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Specify", (object)model.Specify ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@epacs", (object)model.epacs ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Audit", (object)model.Audit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dynamic", (object)model.Dynamic ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Migration", (object)model.Migration ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CCBID", (object)model.CCBID ?? DBNull.Value);

                return objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCSSComplaintOnlineDistrictWise" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;

        }



        //GetKPIForPacsReport

        public DataTable GetKPIForPacsReport(int scoietyid)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_SaveKPIForPacs", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@action", "GetDataList");
                cmd.Parameters.AddWithValue("@SocietyId", scoietyid);

                return objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetCSSComplaintOnlineDistrictWise" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;

        }


        public DataTable GetSAKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetSAKPITable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetSAKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetRAKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetRAKPITable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetRAKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetUserDetailsBySSO()
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetUserDetailsBySSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);        
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);        
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetUserDetailsBySSO(string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int SaveKPIResult(List<KPIResultModel> model)
        {
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                foreach (var item in model)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("USP_SaveSAKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, item.KPIFor);
                    objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, item.Month);
                    objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, item.OfficeId);
                    objDBAccess.AddParameterToCommand("@KPIIndicatorsId", DBFactoryParameterType.Int, item.KPIIndicatorsId);
                    objDBAccess.AddParameterToCommand("@Target", DBFactoryParameterType.Float, item.KPIIndicatorsTarget);
                    objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, item.CCBId);
                    objDBAccess.AddParameterToCommand("@KPI_MarksId", DBFactoryParameterType.Int, item.KPI_MarksId);
                    objDBAccess.AddParameterToCommand("@Achievement", DBFactoryParameterType.Float, item.KPIIndicatorsAchievement);
                    objDBAccess.AddParameterToCommand("@ProgressPercentage", DBFactoryParameterType.Float, item.ProgressPercentage);
                    objDBAccess.AddParameterToCommand("@WeightedMarks", DBFactoryParameterType.Float, item.WeightedMarks);
                    objDBAccess.AddParameterToCommand("@MarksObtained", DBFactoryParameterType.Float, item.MarksObtained);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UserRoleMappingDepId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                    objDBAccess.ExecuteCommand();
                    Result++;
                }
                //Result = 1;   // Success
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyModel", FunctionName = "SaveKPIResult" }, ex);
                Result = 0;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable GetBhandarKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetBhandarKPITable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetBhandarKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetKPIAllDetails(int officeId, int month, string kpiFor)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, officeId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetKPIDetails(int officeId, int month, string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public string GetKPICurrentFinancialYear()
        {
            string fy = "";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetCurrentFinancialYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    fy = dt.Rows[0]["FinancialYear"].ToString();
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyModel", FunctionName = "GetKPICurrentFinancialYear" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return fy;
        }

        public DataTable GetFertilizerCategory()
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetFertilizerCategory", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "GetFertilizerSubCategory(int FertilizerId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataSet GetFertilizerDetailById(string actionType = "", int DemandRequestId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("USP_GetFertilizerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@DemandRequestId", DBFactoryParameterType.BigInt, DemandRequestId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Society", FunctionName = "GetFertilizerDetailById()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
    }
}

