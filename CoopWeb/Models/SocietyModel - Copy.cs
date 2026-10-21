using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;

namespace Cooperative.Models
{
    public class SocietyModel
    {
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
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
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UploadMemberExcelData(DataTable data)" }, ex);
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
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = " UploadMemberExcelData(DataTable data)" }, ex);
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

            dateTime = dateTime.Replace(".", "/").Replace("_", "/").Replace("-", "/").Split(' ')[0];





            string newdate = dateTime.Split('/')[2] + "/" + dateTime.Split('/')[1] + "/" + dateTime.Split('/')[0];
            string[] formats = { "yyyy/MM/dd" };
            DateTime parsedDateTime;
            try
            {
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
                                else {
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


        public DataTable GetAuditDataForFirmOrIndividual(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AutorDataForFirmOrIndividual_copy", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
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
        public int SaveAuditorAssigmentsDate(int SocietyAuditReportId, DateTime AuditStartDate, DateTime AuditEndDate)
        {
            int Result = 0;
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand("SaveAuditorAssigmentsStartEndDate", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@SocietyAuditReportId", DBFactoryParameterType.Int, SocietyAuditReportId);
            if (AuditStartDate != null && !AuditStartDate.ToString("dd/MM/yyyy").Contains("0001"))
            {
                objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.DateTime, AuditStartDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
            }
            if (AuditEndDate != null && !AuditEndDate.ToString("dd/MM/yyyy").Contains("0001"))
            {
                objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.DateTime, AuditEndDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
            }
            int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
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
        public DataTable GetAllAuditorAssignmentByDepartment(string WhereClause, int IsForEmpanelledAgency, int LevelId, int Division_District_Id)
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
        public DataTable GetInspactorName(string DistId)
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (DistId == "0" || DistId == "")
                {
                    DistId = _LoggedInUser.DistrictId.ToString();
                }
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetInspactorName", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, " and URM.DistrictId=" + DistId);

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
        public int saveAuditAssignbydeprt(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "")
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignByDepartmentinsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditerId", DBFactoryParameterType.VARCHAR, AuditerId);
                if (AuditBy == "0")
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, false);
                else
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, true);

                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.VARCHAR, Ids);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.VARCHAR, Status);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.VARCHAR, IsFirm);
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

        public int saveAuditAssignbydeprtNotRequest(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", long UserId = 0)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditorAssignByDepartmentinsertNotRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditerId", DBFactoryParameterType.VARCHAR, AuditerId);
                if (AuditBy == "0")
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, false);
                else
                    objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.Bit, true);

                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.VARCHAR, Ids);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.VARCHAR, Status);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.VARCHAR, IsFirm);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, UserId);
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

        public int CheckSocietyByBRN(string BRN, string SSOID, string AADHARNOENC, int IsNewReg = 0,string SocietyIdENC="")
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyDistrictList(string BRN)" }, ex);
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

        public DataTable GetSocityExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocityExcelDetailsByCreatedBy", DBFactoryCommandType.PROCEDURE_COMMAND);
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

        public DataTable GetExlSocietyDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelSocityDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
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
        public string UpdateExcelSocityDetailsByExcelUpload(string SocietyId, string AdministrativeId, string SocietyName, string RegistrationNo, string RegistrationDate, string Name, string ContactNo, string Aadh)
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

        public DataTable GetAuditorAndSocietyDetailByIds(string Ids)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorAndSocietyDetailByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
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


        public string UpdateCCSocietyliquidator(string SocietyIDEnc, string liquidatorName, string liquidatorAppointedDate, string liquidatorPostName, string liquidatorContactNo, long CreateBy)
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

        public DataTable GetCCSComplaint(ref DataPagingModel Details, int DistrictId)
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


        public string InsertUpdateCCSComplaintLetter(CCSLetter LetterDetails)
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
        public DataTable GetCCSData(string WhereClause, int CCSId = -1, int Type=-1)
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
        

        #endregion

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

        public DataTable SocietyDetailsBySocietyIdAmendmend(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyDetailsBySocietyIdAmendmend", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable SocietyDetailsBySocietyIdAmendmend(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetMemberAmendmentBySocietyId(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetMemberAmendmentBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
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


    }

}