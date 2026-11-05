using java.math;
using java.util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class GopalCreditCardModel
    {
        public class DashBoardDataModel
        {
            [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Please enter a number between 0 and 255.")]
            [Range(0, 255, ErrorMessage = "Please enter value between 0 and 255.")]
            public int ActionId { get; set; }

            [RegularExpression(@"^(?:0|[1-9]\d{0,9})$", ErrorMessage = "Please enter a valid integer value.")]
            [Range(0, int.MaxValue, ErrorMessage = "Please enter value between 0 and maximum int value.")]
            public int GopalCreditCardId { get; set; }
            public string Encrypted_GopalCreditCardId { get; set; }

            public string SSOID { get; set; }
            public string UserType { get; set; }
            public string KIOSKCODE { get; set; }
            public string Name_ENG { get; set; }
            public string RegistrationNo { get; set; }
            public string Encrypted_RegistrationNo { get; set; }
            public string RegistrationDate_DDMMYYYY { get; set; }
            public string ActivationStatus { get; set; }

            //madhuri
            public string ProceesStatusTextEn { get; set; }
            public string ProceesStatusTextHi { get; set; }
            public int StatusId { get; set; }
            public string EncGccId { get; set; }
            public string AadharNoEnc { get; set; }

            public bool IsMahilaNidhi { get; set; }
            public bool IsEligibleForRenewable { get; set; }
            public bool HasReApplied { get; set; }
            public bool IsSatisfiedWithInttCalcByPacs { get; set; }
            public string ApplicantSignedForm { get; set; }
            public bool IsApplicantSigned { get; set; }
            public bool IsGurantor1Signed { get; set; }
            public bool IsGurantor2Signed { get; set; }
            public bool IsPacsManagerSigned { get; set; }
            public string ApplicantSignedFormPacs { get; set; }
            public string ApplicantSignedFormGuarantor { get; set; }
        }
        public string JanAadharNo { get; set; }
        public class GopalCreditCardFormModel
        {
            public int gcc_id { get; set; } //madhuri
            public string PBBankAccountNo { get; set; }
            public bool Status { get; set; }
            #region JanAadhar Detail
            public string JanAadharNo { get; set; }
            public string Encrypted_JanAadharNo { get; set; }
            public string JanAadharMID { get; set; }
            public string Encrypted_JanAadharMID { get; set; }
            public string JanAadharEnrollmentId { get; set; }
            public string AadharNo { get; set; }
            public string Name_ENG { get; set; }
            public string Name_HI { get; set; }
            public string FatherName_ENG { get; set; }
            public string FatherName_HI { get; set; }
            public string MotherName_ENG { get; set; }
            public string MotherName_HI { get; set; }
            public string SpouseName_ENG { get; set; }
            public string SpouseName_HI { get; set; }
            public string MaritalStatus { get; set; }
            public string RelationType { get; set; }
            public string DOB { get; set; }
            public string Age { get; set; }
            public string Gender { get; set; }
            public string MobileNo { get; set; }
            public string BankAccountNo { get; set; }
            public string BankName { get; set; }
            public string BankIFSCCode { get; set; }
            public string BankBranchName { get; set; }
            public string BankMICRCode { get; set; }
            public string Income { get; set; }
            public string Occupation { get; set; }
            public string PANCardNo { get; set; }
            public string Caste { get; set; }
            public string Category { get; set; }
            public string MemberPhoto_Base64 { get; set; }
            public string MemberPhoto { get; set; }

            public bool IsAddressSameAsJanAadhar { get; set; }
            public string HouseBuildingNo { get; set; }
            public string LaneStreetName { get; set; }
            public string Locality { get; set; }
            public string Address_ENG { get; set; }
            public string Address_HI { get; set; }
            public string Original_Address_ENG { get; set; }
            public string Original_Address_HI { get; set; }
            public string PinCode { get; set; }
            public string Original_PinCode { get; set; }
            public string DistrictName_ENG { get; set; }
            public string DistrictName_HI { get; set; }
            public string District_Code { get; set; }
            public string Original_District_Code { get; set; }
            public int DISTRICT_ID { get; set; }
            public int Original_DISTRICT_ID { get; set; }
            public string Block_ENG { get; set; }
            public string Block_HI { get; set; }
            public string Block_Code { get; set; }
            public string Original_Block_Code { get; set; }
            public int BLOCK_ID { get; set; }
            public int Original_BLOCK_ID { get; set; }

            public string GP_ENG { get; set; }
            public string GP_HI { get; set; }
            public string GP_Code { get; set; }
            public string Original_GP_Code { get; set; }
            public int GP_ID { get; set; }
            public int Original_GP_ID { get; set; }
            public string Village_ENG { get; set; }
            public string Village_HI { get; set; }
            public string Village_Code { get; set; }
            public string Original_Village_Code { get; set; }
            public int VILLAGE_ID { get; set; }
            public int Original_VILLAGE_ID { get; set; }
            public string City_ENG { get; set; }
            public string City_HI { get; set; }
            public string City_Code { get; set; }
            public string Original_City_Code { get; set; }
            public int CITY_ID { get; set; }
            public int Original_CITY_ID { get; set; }
            public string Ward_ENG { get; set; }
            public string Ward_HI { get; set; }
            public string Ward_Code { get; set; }
            public string Original_Ward_Code { get; set; }
            public string WARD_ID { get; set; }
            public string Original_WARD_ID { get; set; }
            public bool IsRural { get; set; }
            public bool Original_IsRural { get; set; }
            #endregion

            public bool? IsMemberOfDCS { get; set; }
            public string DCSName { get; set; }
            public string DCSOwnerName { get; set; }
            public string DCSOwnerMobileNo { get; set; }
            public string DCSMembershipNo { get; set; }
            public string DCSAddress { get; set; }
            public bool? IsPACSLAMPSMember { get; set; }
            public string PACSLAMPSMembershipNo { get; set; }
            //public bool IsResidedInRuralArea { get; set; }
            public bool DoesTheMilkSaleToDCS { get; set; }
            public string EstimatedAmountOfMilkSoldToDCS { get; set; }
            public bool? IsRecommendationOfDCSUploaded { get; set; }
            public string RecommendationOfDCSDOC_Path { get; set; }
            public string DocumentOfProposedWork_Path { get; set; }
            public string DesiredCreditLimitLoanAmount { get; set; }
            public string LoanAmountRecommendedByDCS { get; set; }
            public string CopyOfDCSPassbook_Path { get; set; }
            public bool ECSMandateConfirmation { get; set; }
            public bool GopalCreditCardLoanDeclaration { get; set; }
            public bool GopalCreditCardDeclaration { get; set; }
            public bool GopalCreditCardDeclaration2 { get; set; }
            public bool GopalCreditCardDeclaration4 { get; set; }
            public bool? isValidRecommendationOfDCSDOC { get; set; }
            public bool? isValidDocumentOfProposedWork { get; set; }
            public bool? isValidCopyOfDCSPassbook { get; set; }

            public string RecommendationOfDCSDOC { get; set; }
            public string DocumentOfProposedWork { get; set; }
            public string CopyOfDCSPassbook { get; set; }

            public long CreatedBy { get; set; }
            public string CreatedByIP { get; set; }
            public string UpdatedByIP { get; set; }
            public string SSOID { get; set; }
            public string KIOSKCODE { get; set; }
            public string UserType { get; set; }
            public long UpdatedBy { get; set; }
            public string DCSRegistrationNo { get; set; }
            public string RegistrationNo { get; set; }
            public string RegistrationDate_DDMMYYYY { get; set; }
            public string LoanDistrictId { get; set; }
            public string LoanDistrictName_ENG { get; set; }
            public string LoanDistrictName_HI { get; set; }
            public string LoanBankID { get; set; }
            public string LoanBankName_ENG { get; set; }
            public string LoanBankName_HI { get; set; }
            public string LoanBranchId { get; set; }
            public string LoanBranchName_ENG { get; set; }
            public string LoanBranchName_HI { get; set; }
            public string PACS_ID { get; set; }
            public string PACSName_ENG { get; set; }
            public bool? IsCopyOfDCSPassbookAttached { get; set; }
            public bool? IsResideInPACSLAMPSArea { get; set; }
            public bool? IsPACSBankAccountAvailable { get; set; }

            //By Madhuri
            public string GccReportFile { get; set; }
            public int StatusId { get; set; }
            public string GccIdDBEnc { get; set; }
            public string DocResStatus { get; set; }
            public string AadharDoc { get; set; }
            public string PanCardDoc { get; set; }
            public int NoOfCows { get; set; }
            public int NoOfbuffaloes { get; set; }
            public int OtherAnimal { get; set; }
            public string AgriculturalProperty { get; set; }
            public decimal CurrentMarketValue_AP { get; set; }
            public string AgriculturalPropertyDoc { get; set; }
            public string OtherProperty { get; set; }
            public decimal CurrentMarketValue_OP { get; set; }
            public string OtherPropertyDoc { get; set; }
            public string PurposeOfLoan { get; set; }
            public string PurposeOfLoan_En { get; set; }
            public string PurposeOfLoan_Hi { get; set; }
            public string PurposeOfLoanOther { get; set; }
            public int CurrentLoanDistrictId { get; set; }
            public int CurrentLoanBankID { get; set; }
            public int CurrentLoanBranchId { get; set; }
            public string CurrentBankIFSCCode { get; set; }


            public decimal SanctionAmt { get; set; }

            //public string DisbursedDate { get; set; }
            //public string PaymentDate { get; set; }

            public DateTime? DisbursedDate { get; set; }
            public DateTime? PaymentDate { get; set; }
            public decimal FinalSanctionAmt { get; set; }


            public string JanAadharNoEnc { get; set; } //madhuri
            public string AadharNoEnc { get; set; } //madhuri
            public string PANCardNoEnc { get; set; } //madhuri


            public int ShgId { get; set; }
            public int ShgVoID { get; set; }
            public int ShgClfID { get; set; }
            public int ShgDistrictId { get; set; }
            public int ShgBlockID { get; set; }

            public string District_CodeG { get; set; }
            public string Block_CodeG { get; set; }
            public string GP_CodeG { get; set; }
            public string Village_CodeG { get; set; }
            public string GKhasraNo { get; set; }
            public string GKhataNo { get; set; }

            public int Mn_DistrictId { get; set; }
            public string Mn_DistrictName { get; set; }
            public int Mn_BlockID { get; set; }
            public string Mn_BlockName { get; set; } = string.Empty;
            public string Mn_ClusterId { get; set; } = string.Empty;
            public string Mn_ClusterName { get; set; } = string.Empty;
            public string Mn_VoID { get; set; } = string.Empty;
            public string Mn_VoName { get; set; } = string.Empty;
            public string Mn_ShgId { get; set; } = string.Empty;
            public string Mn_ShgName { get; set; } = string.Empty;
            public string Mn_MemberId { get; set; } = string.Empty;
            public string SHG_AccountNo { get; set; } = string.Empty;
            public string SHG_IFSC_Code { get; set; } = string.Empty;
            public string SHG_BankName { get; set; } = string.Empty;
            public string SHG_BranchName { get; set; } = string.Empty;
            public string SHG_Mem_BankName { get; set; } = string.Empty;
            public string SHG_Mem_BranchName { get; set; } = string.Empty;
            public string SHG_Mem_AccountNo { get; set; } = string.Empty;
            public string SHG_Mem_IFSC_Code { get; set; } = string.Empty;

            public bool IsEligibleForRenewable { get; set; }
            public int RenewRefGccId { get; set; }
            public bool IsSatisfiedWithInttCalcByPacs { get; set; }
            public string NocDocument { get; set; }
            public List<AgricultureLandDetailModel> AgricultureLandDetailList { get; set; }
            public List<CropLoanDetailsModel> CropLoanDetailsList { get; set; }
            public List<GuarantorDetailsModel> GuarantorDetailsList { get; set; }

            public List<JanAaadharFamilyMemberModel> janAaadharFamilyMemberList { get; set; }

            public bool? IsWorking { get; set; }
            public bool? IsInfrastructure { get; set; }
            public bool? IsApparatus { get; set; }
            public string IsWorkingChild { get; set; }
            public string IsInfrastructureChild { get; set; }
            public string IsApparatusChild { get; set; }
            public bool IsApplicantRuralResidence { get; set; }
            public int? NoOfbuffaloesOnMoka { get; set; } = 0;
            public int? NoOfCowsOnMoka { get; set; } = 0;
            public decimal? LandValueOnMoka { get; set; } = 0;
            public string RemarkMoka { get; set; }
            public bool MmpbyDeathStatus { get; set; }
            public string MmpbyInsuranceNo { get; set; }
            public string MmpbyInsuranceExpiredOn { get; set; }
            public string ApplicantSignedForm { get; set; }
            public bool IsApplicantSigned { get; set; }
            public bool IsGurantor1Signed { get; set; }
            public bool IsGurantor2Signed { get; set; }
            public bool IsPacsManagerSigned { get; set; }
            public string ApplicantSignedFormPacs { get; set; }
            public string ApplicantSignedFormGuarantor { get; set; }

            public bool IsTheCattleinsuredMMPBY { get; set; }

        }
        public class JanAaadharFamilyMemberModel
        {
            public string NAME { get; set; }
            public string NAME_ENG { get; set; }
            public string NAME_HI { get; set; }
            public string FATHER_NAME_ENG { get; set; }
            public string FATHER_NAME_HND { get; set; }
            public string GENDER { get; set; }
            public string MEMBER_TYPE { get; set; }
            public string Masked_MOBILE_NO { get; set; }
            public string Encrypted_MOBILE_NO { get; set; }
            public string JAN_AADHAR { get; set; }
            public string Encrypted_JAN_AADHAR { get; set; }
            public string Masked_JAN_MEMBER_ID { get; set; }
            public string Encrypted_JAN_MEMBER_ID { get; set; }
            public string Encrypted_ENR_ID { get; set; }
            public string Encrypted_AADHAR_REF_NO { get; set; }
            public bool IsMemberSelected { get; set; }
            public int Age { get; set; }
            public string JanAadharNoEnc { get; set; } //madhuri           

        }
        public class GCC_RequiredPointsModel
        {
            public Int32 GCC_RequiredPointsId { get; set; }
            public string GopalCreditCardIdEnc { get; set; }
            public int TypesOfProperty { get; set; }
            public string RegistrationNo { get; set; }
            public string KhasraNo { get; set; }
            public string KhataNo { get; set; }
            public string LandMeasurement { get; set; }
            public string LandType { get; set; }
            public string Other { get; set; }
            public string JamabandiDoc { get; set; }
            public string JamabandiDocRemark { get; set; }
            public string ResidentialPlotAddress { get; set; }
            public string EstimatedPrice { get; set; }
            public string CopyOfLease { get; set; }
            public string CopyOfLeaseRemark { get; set; }
            public string LoanAmountRecommendByPACS { get; set; }
            public short IsRegisteredBankFirstCharge { get; set; }
            public short IsPropertyMortgagedEarlier { get; set; }
            public bool IsPermanentSave { get; set; }
            public string EarlierLoanAmount { get; set; }
            public string EarlierLoanFromBank { get; set; }
            public string EarlierLoanAccountNo { get; set; }
            public string EarlierLoanSanctionDate { get; set; }
        }


        public class AgricultureLandDetailModel
        {
            public int gccId { get; set; }
            public int id { get; set; }
            public string Village { get; set; }
            public string PatwarCircle { get; set; }
            public decimal Irrigated { get; set; }
            public decimal NonIrrigated { get; set; }
            public string KhasraNumber { get; set; }
            public string KhataNumber { get; set; }
            public string LandOwnereName { get; set; }
            public bool IsActive { get; set; }
            public bool IsDelete { get; set; }

        }
        public class CropLoanDetailsModel
        {
            public int gccId { get; set; }
            public int id { get; set; }
            public string financeInstituteName { get; set; }
            public decimal NOD { get; set; }
            public decimal OD { get; set; }
            public bool IsActive { get; set; }
            public bool IsDelete { get; set; }

        }

        public class GuarantorDetailsModel
        {
            public Int32 id { get; set; }
            public int gccId { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }
            public string FatherSpouseName { get; set; }
            public string DOB { get; set; }
            public string JanAadharNumber { get; set; }
            public string JanMemberId { get; set; }
            public string AadharNo { get; set; }
            public bool IsVerified { get; set; }
            public string PANCardNo { get; set; }
            public string MobileNo { get; set; }
            public string Address { get; set; }
            public string GuarranterLandProperty { get; set; }
            public string Photo { get; set; }
            public string Signature { get; set; }
            public bool IsActive { get; set; }
            public bool IsDelete { get; set; }
            public string JanAadharNoEnc { get; set; } //madhuri
            public string AadharNoEnc { get; set; } //madhuri
            public string PANCardNoEnc { get; set; } //madhuri
            public string AadharNoMasked { get; set; }
            public string PANCardNoMasked { get; set; }
            public string JanAadharNoMasked { get; set; }

        }

        public class GccApplicationFlowModel
        {
            public Int32 id { get; set; }
            public int GccId { get; set; }
            public int UserId { get; set; }
            public bool IsActive { get; set; }
            public bool IsDelete { get; set; }
            public int DocId { get; set; }
            public int PurposeOfLoan { get; set; }
            public string PurposeOfLoanOther { get; set; }
            public string DocResStatus { get; set; }
            public string DocumentURL { get; set; }
            public string Remark { get; set; }
            public int StatusId { get; set; }
            public decimal LoanAmountRecommendedByDCS { get; set; }
            public bool IsEligibleForRenewable { get; set; }
            public bool IsSatisfiedWithInttCalcByPacs { get; set; }
            public string NocDocument { get; set; }
            public int PaymentSummaryId { get; set; }
            //public int? GeneralNumber { get; set; }

        }




        public class GccDocumentsModel
        {
            public Int32 id { get; set; }
            public int GccId { get; set; }
            public int DocId { get; set; }
            public string DocumentURL { get; set; }
            public string RegistrationNo { get; set; }
        }

        public class GccEditableDocumentslist
        {
            public List<GccEditableDocumentsModel> GccEditableDocuments { get; set; }
        }
        public class GccEditableDocumentsModel
        {
            //public Int32 id { get; set; }
            public int FieldId { get; set; }
            public int GccId { get; set; }
            public string Status { get; set; }
            public string FieldsName { get; set; }
            public string FieldShortName { get; set; }
            public string Type { get; set; }
        }
    }
}