using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using static Cooperative.Controllers.ServicesController;
using static Cooperative.Models.GopalCreditCardModel;

namespace Cooperative.Models
{
    public class PLDPLoanApplication
    {
        public int load_id { get; set; }
        public int application_no { get; set; }
        public int pldb_id { get; set; }
        public int branch_id { get; set; }
        public int createdby { get; set; }
        public bool IsSubmit { get; set; }
        public DateTime created_date { get; set; }
        public string UserType { get; set; }
        public List<PLDBLoanMembers> Members { get; set; }
        public List<PLDBLoanAmount> LoanAmount { get; set; }
        public List<PLDBLoanProperty> LoanProperty { get; set; }
        public List<PLDPLoanMemberPreLoan> PreLoan { get; set; }
        public List<BankAccountDetails> BankDetails { get; set; }
        public List<VehicleDetails> VehicleDetails { get; set; }
        public List<GuarantorDetails> GuarantorDetails { get; set; }
        public List<PLDBIndustryDetails> IndustryDetails { get; set; }
    }   


    public class PLDBIndustryDetails
    {
        public string RegistrationNumber { get; set; }
        public string RegistrationName { get; set; }
        public string RegistrationDate { get; set; }
        public string DateOfEstablishment { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public string BusinessLicense_RegiDescription { get; set; }
        public string KindOfBusiness { get; set; }
        public string BusinessItemName { get; set; }
        public string MonthlyRequirementLivelihood { get; set; }
        public string ProposedIndustrialActivity { get; set; }
        public string NumberOfPerson { get; set; }
        public string SourceOfDebt { get; set; }
        public string BailDocument { get; set; }
        public string Repayment { get; set; }
        public string RateOfInterest { get; set; }
        public string CollateralSecurity { get; set; }
        public string RicoFile { get; set; }
        public string ApplicantDealAfterApprovalFile { get; set; }
        public string BusinessAndRegiCertificateFile { get; set; }
        public string ProposedBuildingAndWorkStationFile { get; set; }
        public string QuotationFile { get; set; }
        public string IncomeTaxClearanceCertificate { get; set; }
        public string WorkingCapitalFile { get; set; }
    }

    public class PLDBLoanMembers
    {
        public int Id { get; set; }
        public string MembershipNumber { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Age { get; set; }
        public int FarmerCategory { get; set; }
        public int CategoryId { get; set; }
        public int Village { get; set; }
        public int VillagePanchayat { get; set; }
        public int Tehsil { get; set; }
        public string MobileNo { get; set; }
        public string Photo { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string VillageName { get; set; }
        public string GramPanchayatName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsMainMember { get; set; }
        public string Address { get; set; }

        public int Cast { get; set; }
        // public int Educated_Experienced { get; set; }
        //public int MonthlyIncome { get; set; }
        public string Source { get; set; }

        public string MemberBusiness { get; set; }
        public string MonthlyIncome { get; set; }
        public string EducationalQualification { get; set; }
        public string FamilyDependents { get; set; }
        public string MonthlyMinimumAmountForLiving { get; set; }
        public string Educated_Experienced { get; set; }
        //public string Source { get; set; }
        public string BusinessAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string TelephoneBusiness { get; set; }
        public string TelephoneResidence { get; set; }
        public string PartnershipCertifiedCopy { get; set; }
        public string TransportCompanyRegCertification { get; set; }
        public string DrivinglicenseCopy { get; set; }
        public string VehicleRantProposalCopy { get; set; }
        public string QuotationCopy { get; set; }
    }

    public class PLDBLoanAmount
    {
        public int Id { get; set; }
        public string PurposeOfLoan { get; set; }
        public string EstimatedTotalAmount { get; set; }
        public string LoanAmountApplied { get; set; }
        public int LoanamountCapital { get; set; }
        public string Period { get; set; }
        public string WorkingCapitalFile { get; set; }
        public bool IsDelete { get; set; }
        public int LoanType { get; set; }
    }

    public class PLDBLoanProperty
    {
        public int Id { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string GramPanchayat { get; set; }
        public string Khasara { get; set; }
        public string ChahiBi { get; set; }
        public string ChahiVi { get; set; }
        public string BaraniBi { get; set; }
        public string BaraniVi { get; set; }
        public string OtherDetails { get; set; }
        public string Specifications { get; set; }
        public bool IsDelete { get; set; }
        public string Address { get; set; }
        public string TotalArea { get; set; }
        public int MemberType { get; set; }
        public string PropertyDocument { get; set; }
        public string PropertyDocumentType { get; set; }
        public string Member { get; set; }

    }

    public class PLDPLoanMemberPreLoan
    {
        public int Id { get; set; }
        public string Organization { get; set; }
        public string LoanAmount { get; set; }
        public string RestAmount { get; set; }
        public string MonthlyInstallment { get; set; }
        public string Tenure { get; set; }
        public string IntrestRate { get; set; }
        public string Source { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PLDBLoanMaster
    {
        public int Id { get; set; }
        public int SchemeId { get; set; }
        public int PerposeId { get; set; }
        public int SectorId { get; set; }
        public string NameEn { get; set; }
        public string NameHi { get; set; }
        public bool IsActive { get; set; }
        public string Farming_NonFarming { get; set; }
        public int RepaymentPeriod { get; set; }
        public int DueInstallment { get; set; }
        public int GracePeriod { get; set; }
        public string ApprovedAmount { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime EffectiveDate { get; set; }
        public float NormalInterestRate { get; set; }
        public float PenalInterestRate { get; set; }
        public string Mobile { get; set; }

    }
    //bhupendra
    public class PLDBLandAmount
    {
        public int Id { get; set; }
        public string Loan_Id { get; set; }
        public string PureProfitAmount { get; set; }
        public string LandValueAmount { get; set; }
        public string LoanPayLimitAmount { get; set; }
        public string YearlyInstallment { get; set; }
        public string PureSaving { get; set; }
    }

    public class PLDBLVODocuments
    {
        public string Document_Id { get; set; }
        public string Loan_Id { get; set; }
        public string Document_Type { get; set; }
        public string Document_Url { get; set; }
        public string Is_Active { get; set; }
        public string Is_Delete { get; set; }
    }


    public class PLDBApplicationForm
    {

        public List<ApplicantDetails> ApplicantDetails { get; set; }
        public List<BankDetails> BankDetails { get; set; }
        public List<LoanDetails> LoanDetails { get; set; }
        public List<Address> PermanentAddress { get; set; }
        public List<Address> TempAddress { get; set; }

    }

    public class ApplicantDetails
    {
        public string Name { get; set; }
        public string FatherHusbandName { get; set; }
    }
    public class ApplicantDetails1
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string FatherHusbandName { get; set; } = string.Empty;
        public int? Age { get; set; } = 0; // Assuming optional age
        public int CategoryId { get; set; } = 0;
        public char Gender { get; set; } = '\0'; // Default to null char
        public string JanAadharNumber { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int EducationalQualificationId { get; set; } = 0;
        public int NoOfLoaneeBeneficiary { get; set; } = 0;
        public string OtherPropertyDetails { get; set; } = string.Empty;
        public decimal CurrentDlcValue { get; set; } = 0.00m;
        public bool? IslandMortgaged { get; set; } = false;
        public string LandMortgaged { get; set; } = string.Empty;
        public int PLDB_ID { get; set; } = 0;
        public int Branch_ID { get; set; } = 0;
        public string NomineeName { get; set; } = string.Empty;
        public int? NomineeRelationId { get; set; } = 0;
        public string NomineeJanAadharNumber { get; set; } = string.Empty;
        public string NomineeMobileNo { get; set; } = string.Empty;
        public string NomineeBusiness { get; set; } = string.Empty;
    }


    public class BankDetails
    {
        public int? BankId { get; set; } = 0;
        public string AccountNo { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public DateTime? DateOfOpenAccount { get; set; }
        public string LoanFacility { get; set; } = string.Empty;
    }
    public class LoanDetails
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public int Type { get; set; } = 0;
        public int SectorId { get; set; } = 0;
        public int SchemeId { get; set; } = 0;
        public int PurposeOfLoanId { get; set; } = 0;
        public int LoanTypeId { get; set; } = 0;
        public decimal EstimatedTotalAmount { get; set; } = 0.00m;
        public decimal LoanAmountApplied { get; set; } = 0.00m;
        public string LoanCategory { get; set; } = string.Empty;
        public int PeriodYears { get; set; } = 0;
        public string Pre_CooperativeBankName { get; set; } = string.Empty;
        public decimal Pre_LoanAmount { get; set; } = 0.00m;
        public decimal Pre_RestAmount { get; set; } = 0.00m;
        public decimal Pre_MonthlyInstallment { get; set; } = 0.00m;
        public int Pre_Tenure { get; set; } = 0;
        public decimal Pre_InterestRate { get; set; } = 0.00m;
        public string Pre_Source { get; set; } = string.Empty;
        public DateTime? DateOfSanction { get; set; }
        public decimal RateOfInterest { get; set; } = 0.00m;
        public DateTime? DisbursementDate { get; set; }
        public decimal TotalDisbursementAmount { get; set; } = 0.00m;
        public bool IsDeleted { get; set; } = false;
    }
    public class Address
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public bool IsRural { get; set; } = false;
        public string HouseNo { get; set; } = string.Empty;
        public string LaneStreetName { get; set; } = string.Empty;
        public string Locality { get; set; } = string.Empty;
        public int STATE_ID { get; set; } = 0;
        public int DISTRICT_ID { get; set; } = 0;
        public int CITY_ID { get; set; } = 0;
        public int BLOCK_ID { get; set; } = 0;
        public int WARD_ID { get; set; } = 0;
        public int GP_ID { get; set; } = 0;
        public int VILLAGE_ID { get; set; } = 0;
        public int PinCode { get; set; } = 0;
        public string AreaOfOperation { get; set; } = string.Empty;
        public byte IsActive { get; set; } = 0; // tinyint maps to byte
        public int TEHSIL_ID { get; set; } = 0;
        public int DIVISION_ID { get; set; } = 0;
        public bool IsSameAddress { get; set; } = false;
        public string CITYName { get; set; } = string.Empty;
        public string BlockName { get; set; } = string.Empty;
        public string WardName { get; set; } = string.Empty;
        public string GpName { get; set; } = string.Empty;
        public string VName { get; set; } = string.Empty;
        public string VidhanSabha { get; set; } = string.Empty;
        public int CITY_IDD { get; set; } = 0;
        public string CityTypes { get; set; } = string.Empty;
        public int IsPermanentAddress { get; set; } = 0;
    }
    public class PLDBLoan
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public PLDBLoan(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PLDBLoan()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public string InsertUpdatePLDBLoanApplication(PLDPLoanApplication Applicationdatat)
        {
            string Result = "-1";
            DBFactoryMSSQL conn = new DBFactoryMSSQL();


            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;

            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            Applicationdatat.IndustryDetails = Applicationdatat.IndustryDetails ?? new List<PLDBIndustryDetails>();

            string RegistrationDate = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].RegistrationDate))
            {
                Applicationdatat.IndustryDetails[0].RegistrationDate = Applicationdatat.IndustryDetails[0].RegistrationDate.Replace("-", "/");
                RegistrationDate = Applicationdatat.IndustryDetails[0].RegistrationDate.Split('/')[2] + "/" + Applicationdatat.IndustryDetails[0].RegistrationDate.Split('/')[1] + "/" + Applicationdatat.IndustryDetails[0].RegistrationDate.Split('/')[0];
            }

            string DateOfEstablishment = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].DateOfEstablishment))
            {
                Applicationdatat.IndustryDetails[0].DateOfEstablishment = Applicationdatat.IndustryDetails[0].DateOfEstablishment.Replace("-", "/");
                DateOfEstablishment = Applicationdatat.IndustryDetails[0].DateOfEstablishment.Split('/')[2] + "/" + Applicationdatat.IndustryDetails[0].DateOfEstablishment.Split('/')[1] + "/" + Applicationdatat.IndustryDetails[0].DateOfEstablishment.Split('/')[0];
            }

            try
            {
                string LoanId = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                // _LoggedInUser.UserRoleDeptMappingId
                // _LoggedInUser.pl
                // objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                //if (Applicationdatat.load_id == 0)
                //{

                cmd.CommandText = "InsertUpdatePLDBLoanApplication";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@LoadId", Applicationdatat.load_id);
                cmd.Parameters.AddWithValue("@PldbId", Applicationdatat.pldb_id);
                cmd.Parameters.AddWithValue("@BranchId", Applicationdatat.branch_id);
                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@SSID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@IsSubmit", Applicationdatat.IsSubmit);
                cmd.Parameters.AddWithValue("@UserType", _LoggedInUser.UserType.ToUpper());


                cmd.Parameters.AddWithValue("@RegistrationNumber", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].RegistrationNumber) ? null : Applicationdatat.IndustryDetails[0].RegistrationNumber);
                cmd.Parameters.AddWithValue("@RegistrationName", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].RegistrationName) ? null : Applicationdatat.IndustryDetails[0].RegistrationName);
                cmd.Parameters.AddWithValue("@RegistrationDate", string.IsNullOrWhiteSpace(RegistrationDate) ? null : RegistrationDate);
                cmd.Parameters.AddWithValue("@DateOfEstablishment", string.IsNullOrWhiteSpace(DateOfEstablishment) ? null : DateOfEstablishment);
                cmd.Parameters.AddWithValue("@OwnerName", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].OwnerName) ? null : Applicationdatat.IndustryDetails[0].OwnerName);
                cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].Address) ? null : Applicationdatat.IndustryDetails[0].Address);
                cmd.Parameters.AddWithValue("@RicoFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].RicoFile) ? null : Applicationdatat.IndustryDetails[0].RicoFile);

                cmd.Parameters.AddWithValue("@BusinessLicense_RegiDescription", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].BusinessLicense_RegiDescription) ? null : Applicationdatat.IndustryDetails[0].BusinessLicense_RegiDescription);
                cmd.Parameters.AddWithValue("@KindOfBusiness", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].KindOfBusiness) ? null : Applicationdatat.IndustryDetails[0].KindOfBusiness);
                cmd.Parameters.AddWithValue("@BusinessItemName", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].BusinessItemName) ? null : Applicationdatat.IndustryDetails[0].BusinessItemName);
                cmd.Parameters.AddWithValue("@MonthlyRequirementLivelihood", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].MonthlyRequirementLivelihood) ? 0 : Convert.ToDecimal(Applicationdatat.IndustryDetails[0].MonthlyRequirementLivelihood));
                cmd.Parameters.AddWithValue("@ProposedIndustrialActivity", Applicationdatat.IndustryDetails[0].ProposedIndustrialActivity);
                cmd.Parameters.AddWithValue("@NumberOfPerson", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].NumberOfPerson) ? 0 : Convert.ToInt32(Applicationdatat.IndustryDetails[0].NumberOfPerson));
                cmd.Parameters.AddWithValue("@SourceOfDebt", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].SourceOfDebt) ? null : Applicationdatat.IndustryDetails[0].SourceOfDebt);

                cmd.Parameters.AddWithValue("@BailDocument", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].BailDocument) ? null : Applicationdatat.IndustryDetails[0].BailDocument);
                cmd.Parameters.AddWithValue("@Repayment", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].Repayment) ? 0 : Convert.ToDecimal(Applicationdatat.IndustryDetails[0].Repayment));
                cmd.Parameters.AddWithValue("@RateOfInterest", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].RateOfInterest) ? 0 : Convert.ToDecimal(Applicationdatat.IndustryDetails[0].RateOfInterest));
                cmd.Parameters.AddWithValue("@CollateralSecurity", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].CollateralSecurity) ? null : Applicationdatat.IndustryDetails[0].CollateralSecurity);

                cmd.Parameters.AddWithValue("@ApplicantDealAfterApprovalFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].ApplicantDealAfterApprovalFile) ? null : Applicationdatat.IndustryDetails[0].ApplicantDealAfterApprovalFile);
                cmd.Parameters.AddWithValue("@BusinessAndRegiCertificateFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].BusinessAndRegiCertificateFile) ? null : Applicationdatat.IndustryDetails[0].BusinessAndRegiCertificateFile);
                cmd.Parameters.AddWithValue("@ProposedBuildingAndWorkStationFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].ProposedBuildingAndWorkStationFile) ? null : Applicationdatat.IndustryDetails[0].ProposedBuildingAndWorkStationFile);
                cmd.Parameters.AddWithValue("@QuotationFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].QuotationFile) ? null : Applicationdatat.IndustryDetails[0].QuotationFile);
                cmd.Parameters.AddWithValue("@IncomeTaxClearanceCertificate", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].IncomeTaxClearanceCertificate) ? null : Applicationdatat.IndustryDetails[0].IncomeTaxClearanceCertificate);
                cmd.Parameters.AddWithValue("@WorkingCapitalFile", string.IsNullOrWhiteSpace(Applicationdatat.IndustryDetails[0].WorkingCapitalFile) ? null : Applicationdatat.IndustryDetails[0].WorkingCapitalFile);

                LoanId = cmd.ExecuteScalar().ToString();
                Applicationdatat.load_id = Convert.ToInt32(LoanId.Split(',')[0]);
                Result = LoanId.Split(',')[1];

                // }

                Applicationdatat.Members.ForEach(member =>
                {
                    if (!string.IsNullOrWhiteSpace(member.MembershipNumber))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdatePLDBLoanMembers";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@MemberId", member.Id);
                        cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                        cmd.Parameters.AddWithValue("@MembershipNo", member.MembershipNumber);
                        cmd.Parameters.AddWithValue("@createdby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@updatedby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@Photo", string.IsNullOrWhiteSpace(member.Photo) ? "" : member.Photo);
                        cmd.Parameters.AddWithValue("@IsDelete", member.IsDelete);
                        cmd.Parameters.AddWithValue("@IsMainMember", member.IsMainMember);

                        cmd.Parameters.AddWithValue("@MemberBusiness", string.IsNullOrWhiteSpace(member.MemberBusiness) ? "" : member.MemberBusiness);
                        cmd.Parameters.AddWithValue("@MonthlyIncome", string.IsNullOrWhiteSpace(member.MonthlyIncome) ? "0" : member.MonthlyIncome);
                        cmd.Parameters.AddWithValue("@EducationalQualification", string.IsNullOrWhiteSpace(member.EducationalQualification) ? "" : member.EducationalQualification);
                        cmd.Parameters.AddWithValue("@FamilyDependents", string.IsNullOrWhiteSpace(member.FamilyDependents) ? "0" : member.FamilyDependents);
                        cmd.Parameters.AddWithValue("@MonthlyMinimumAmountForLiving", string.IsNullOrWhiteSpace(member.MonthlyMinimumAmountForLiving) ? "0" : member.MonthlyMinimumAmountForLiving);
                        cmd.Parameters.AddWithValue("@Educated_Experienced", string.IsNullOrWhiteSpace(member.Educated_Experienced) ? "0" : member.Educated_Experienced);
                        cmd.Parameters.AddWithValue("@Source", string.IsNullOrWhiteSpace(member.Source) ? "" : member.Source);
                        cmd.Parameters.AddWithValue("@BusinessAddress", string.IsNullOrWhiteSpace(member.BusinessAddress) ? "" : member.BusinessAddress);
                        cmd.Parameters.AddWithValue("@PermanentAddress", string.IsNullOrWhiteSpace(member.PermanentAddress) ? "" : member.PermanentAddress);
                        cmd.Parameters.AddWithValue("@TelephoneBusiness", string.IsNullOrWhiteSpace(member.TelephoneBusiness) ? "" : member.TelephoneBusiness);
                        cmd.Parameters.AddWithValue("@TelephoneResidence", string.IsNullOrWhiteSpace(member.TelephoneResidence) ? "" : member.TelephoneResidence);
                        cmd.Parameters.AddWithValue("@PartnershipCertifiedCopy", string.IsNullOrWhiteSpace(member.PartnershipCertifiedCopy) ? "" : member.PartnershipCertifiedCopy);
                        cmd.Parameters.AddWithValue("@TransportCompanyRegCertification", string.IsNullOrWhiteSpace(member.TransportCompanyRegCertification) ? "" : member.TransportCompanyRegCertification);
                        cmd.Parameters.AddWithValue("@DrivinglicenseCopy", string.IsNullOrWhiteSpace(member.DrivinglicenseCopy) ? "" : member.DrivinglicenseCopy);
                        cmd.Parameters.AddWithValue("@VehicleRantProposalCopy", string.IsNullOrWhiteSpace(member.VehicleRantProposalCopy) ? "" : member.VehicleRantProposalCopy);
                        cmd.Parameters.AddWithValue("@VehicleQuotationCopy", string.IsNullOrWhiteSpace(member.QuotationCopy) ? "" : member.QuotationCopy);
                        cmd.ExecuteScalar();

                    }
                });


                Applicationdatat.LoanAmount = Applicationdatat.LoanAmount ?? new List<PLDBLoanAmount>();

                Applicationdatat.LoanAmount.ForEach(amount =>
                {
                    if (!string.IsNullOrWhiteSpace(amount.PurposeOfLoan) && !string.IsNullOrWhiteSpace(amount.EstimatedTotalAmount) && !string.IsNullOrWhiteSpace(amount.LoanAmountApplied))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdatePLDBLoanAmount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@Id", amount.Id);
                        cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                        cmd.Parameters.AddWithValue("@loan_purpose_id", amount.PurposeOfLoan);
                        cmd.Parameters.AddWithValue("@estimated_total_amount", amount.EstimatedTotalAmount);
                        cmd.Parameters.AddWithValue("@loan_amount_applied", amount.LoanAmountApplied);
                        cmd.Parameters.AddWithValue("@createdby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@updatedby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@IsDelete", amount.IsDelete);
                        cmd.Parameters.AddWithValue("@LoanType", amount.LoanType);

                        cmd.Parameters.AddWithValue("@LoanamountCapital", amount.LoanamountCapital);
                        cmd.Parameters.AddWithValue("@Period", amount.Period);


                        cmd.ExecuteScalar();

                    }
                });


                Applicationdatat.PreLoan = Applicationdatat.PreLoan ?? new List<PLDPLoanMemberPreLoan>();
                Applicationdatat.PreLoan.ForEach(prloan =>
                {

                    if (!string.IsNullOrWhiteSpace(prloan.Organization) && !string.IsNullOrWhiteSpace(prloan.LoanAmount.ToString()) && !string.IsNullOrWhiteSpace(prloan.MonthlyInstallment) && !string.IsNullOrWhiteSpace(prloan.RestAmount.ToString()) && !string.IsNullOrWhiteSpace(prloan.Tenure.ToString()) && !string.IsNullOrWhiteSpace(prloan.IntrestRate.ToString()) && !string.IsNullOrWhiteSpace(prloan.Source))

                        if (!string.IsNullOrWhiteSpace(prloan.Organization) && !string.IsNullOrWhiteSpace(prloan.LoanAmount) && !string.IsNullOrWhiteSpace(prloan.MonthlyInstallment))

                        {
                            cmd = new SqlCommand();
                            cmd.CommandText = "InsertUpdatePLDBLoanMemberPreLoan";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Id", prloan.Id);
                            cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                            cmd.Parameters.AddWithValue("@organization", prloan.Organization);
                            cmd.Parameters.AddWithValue("@loan_amount", prloan.LoanAmount);
                            cmd.Parameters.AddWithValue("@monthly_installment", prloan.MonthlyInstallment);


                            cmd.Parameters.AddWithValue("@RestAmount", Convert.ToDecimal(prloan.RestAmount));
                            cmd.Parameters.AddWithValue("@Tenure", Convert.ToInt32(prloan.Tenure));
                            cmd.Parameters.AddWithValue("@IntrestRate", Convert.ToDecimal(prloan.IntrestRate));
                            cmd.Parameters.AddWithValue("@Source", prloan.Source);


                            cmd.Parameters.AddWithValue("@createdby", _LoggedInUser.UserId);
                            cmd.Parameters.AddWithValue("@updatedby", _LoggedInUser.UserId);
                            cmd.Parameters.AddWithValue("@IsDelete", prloan.IsDelete);
                            cmd.ExecuteScalar();

                        }
                });

                Applicationdatat.LoanProperty = Applicationdatat.LoanProperty ?? new List<PLDBLoanProperty>();
                Applicationdatat.LoanProperty.ForEach(property =>
                {
                    if (!string.IsNullOrWhiteSpace(property.Khasara) || (property.MemberType == 2 && property.PropertyDocumentType != "KhasaraNo"))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdatePLDBLoanProperty";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@Id", property.Id);
                        cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                        //  cmd.Parameters.AddWithValue("@VILLAGE_ID", string.IsNullOrWhiteSpace(property.Village) ? "0" : property.Village);
                        // cmd.Parameters.AddWithValue("@GP_ID", string.IsNullOrWhiteSpace(property.GramPanchayat) ? "0" : property.GramPanchayat);
                        //  cmd.Parameters.AddWithValue("@BLOCK_ID", string.IsNullOrWhiteSpace(property.Tehsil) ? "0" : property.Tehsil);
                        cmd.Parameters.AddWithValue("@VILLAGE_ID", 0);
                        cmd.Parameters.AddWithValue("@GP_ID", 0);
                        cmd.Parameters.AddWithValue("@BLOCK_ID", 0);
                        cmd.Parameters.AddWithValue("@khasara", string.IsNullOrWhiteSpace(property.Khasara) ? "" : property.Khasara);
                        cmd.Parameters.AddWithValue("@area_chahi_bi", string.IsNullOrWhiteSpace(property.ChahiBi) ? "" : property.ChahiBi);
                        cmd.Parameters.AddWithValue("@area_chahi_vi", string.IsNullOrWhiteSpace(property.ChahiVi) ? "" : property.ChahiVi);
                        cmd.Parameters.AddWithValue("@area_barani_bi", string.IsNullOrWhiteSpace(property.BaraniBi) ? "" : property.BaraniBi);
                        cmd.Parameters.AddWithValue("@area_barani_vi", string.IsNullOrWhiteSpace(property.BaraniVi) ? "" : property.BaraniVi);
                        cmd.Parameters.AddWithValue("@other_property_details", string.IsNullOrWhiteSpace(property.OtherDetails) ? "" : property.OtherDetails);
                        cmd.Parameters.AddWithValue("@specifications", string.IsNullOrWhiteSpace(property.Specifications) ? "" : property.Specifications);
                        cmd.Parameters.AddWithValue("@createdby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@updatedby", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@IsDelete", property.IsDelete);
                        cmd.Parameters.AddWithValue("@MemberType", property.MemberType);
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(property.Address) ? "" : property.Address);
                        cmd.Parameters.AddWithValue("@TotalArea", string.IsNullOrWhiteSpace(property.TotalArea) ? "0" : property.TotalArea);
                        cmd.Parameters.AddWithValue("@PropertyDocument", string.IsNullOrWhiteSpace(property.PropertyDocument) ? "" : property.PropertyDocument);
                        cmd.Parameters.AddWithValue("@PropertyDocumentType", string.IsNullOrWhiteSpace(property.PropertyDocumentType) ? null : property.PropertyDocumentType);
                        cmd.Parameters.AddWithValue("@Member", string.IsNullOrWhiteSpace(property.Member) ? "" : property.Member);
                        cmd.Parameters.AddWithValue("@village_name", string.IsNullOrWhiteSpace(property.Village) ? "" : property.Village);
                        cmd.Parameters.AddWithValue("@tehsil_name", string.IsNullOrWhiteSpace(property.Tehsil) ? "" : property.Tehsil);

                        cmd.ExecuteScalar();
                    }
                });


                Applicationdatat.BankDetails = Applicationdatat.BankDetails ?? new List<BankAccountDetails>();
                Applicationdatat.BankDetails.ForEach(bankdetails =>
                {
                    string date = "";
                    if (!string.IsNullOrWhiteSpace(bankdetails.DateOfOpenAccount))
                    {
                        bankdetails.DateOfOpenAccount = bankdetails.DateOfOpenAccount.Replace("-", "/");
                        date = bankdetails.DateOfOpenAccount.Split('/')[2] + "/" + bankdetails.DateOfOpenAccount.Split('/')[1] + "/" + bankdetails.DateOfOpenAccount.Split('/')[0];
                    }

                    if (!string.IsNullOrWhiteSpace(bankdetails.BankName.ToString()) && !string.IsNullOrWhiteSpace(bankdetails.AccountType.ToString()) && !string.IsNullOrWhiteSpace(bankdetails.LoanFacility))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdatePLDBLoanBankDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@Id", bankdetails.Id);
                        cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                        cmd.Parameters.AddWithValue("@BankName", bankdetails.BankName);
                        cmd.Parameters.AddWithValue("@AccountType", bankdetails.AccountType);
                        cmd.Parameters.AddWithValue("@DateOfOpenAccount", date);
                        cmd.Parameters.AddWithValue("@LoanFacility", bankdetails.LoanFacility);
                        cmd.Parameters.AddWithValue("@AccountNo", bankdetails.AccountNo);
                        cmd.Parameters.AddWithValue("@Member", string.IsNullOrWhiteSpace(bankdetails.Member) ? null : bankdetails.Member);
                        cmd.ExecuteScalar();

                    }
                });

                Applicationdatat.GuarantorDetails = Applicationdatat.GuarantorDetails ?? new List<GuarantorDetails>();

                Applicationdatat.GuarantorDetails.ForEach(property =>
                {
                    string date = "";
                    if (!string.IsNullOrWhiteSpace(property.DOB))
                    {
                        property.DOB = property.DOB.Replace("-", "/");
                        date = property.DOB.Split('/')[2] + "/" + property.DOB.Split('/')[1] + "/" + property.DOB.Split('/')[0];
                    }
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdatePLDBLoanGuarantor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@Guarantor_Id", string.IsNullOrWhiteSpace(property.Guarantor_Id) ? "0" : property.Guarantor_Id);
                    cmd.Parameters.AddWithValue("@Loan_Id", Applicationdatat.load_id);
                    cmd.Parameters.AddWithValue("@AadhaarNumber", string.IsNullOrWhiteSpace(property.AadhaarNumber) ? "" : property.AadhaarNumber);
                    cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(property.Name) ? "" : property.Name);
                    cmd.Parameters.AddWithValue("@FatherName", string.IsNullOrWhiteSpace(property.FatherName) ? "" : property.FatherName);
                    cmd.Parameters.AddWithValue("@DOB", date);
                    cmd.Parameters.AddWithValue("@Mobile", string.IsNullOrWhiteSpace(property.Mobile) ? "" : property.Mobile);
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(property.Address) ? "" : property.Address);
                    cmd.Parameters.AddWithValue("@Category", string.IsNullOrWhiteSpace(property.Category) ? "0" : property.Category);
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                    cmd.ExecuteScalar();
                });

                Applicationdatat.VehicleDetails = Applicationdatat.VehicleDetails ?? new List<VehicleDetails>();
                Applicationdatat.VehicleDetails.ForEach(property =>
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdatePLDBLoanVehicleDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@Vehicle_id", string.IsNullOrWhiteSpace(property.VehicleId) ? "0" : property.VehicleId);
                    cmd.Parameters.AddWithValue("@loan_id", Applicationdatat.load_id);
                    cmd.Parameters.AddWithValue("@VehicleType", string.IsNullOrWhiteSpace(property.VehicleType) ? "" : property.VehicleType);
                    cmd.Parameters.AddWithValue("@Model", string.IsNullOrWhiteSpace(property.Model) ? "" : property.Model);
                    cmd.Parameters.AddWithValue("@Capacity", string.IsNullOrWhiteSpace(property.Capacity) ? "" : property.Capacity);
                    cmd.Parameters.AddWithValue("@AverageFuelConsumption", string.IsNullOrWhiteSpace(property.AverageFuelConsumption) ? "" : property.AverageFuelConsumption);
                    cmd.Parameters.AddWithValue("@VehicleVendorName", string.IsNullOrWhiteSpace(property.VehicleVendorName) ? "" : property.VehicleVendorName);
                    cmd.Parameters.AddWithValue("@ChassisPrice", string.IsNullOrWhiteSpace(property.ChassisPrice) ? "" : property.ChassisPrice);
                    cmd.Parameters.AddWithValue("@BodyPrice", string.IsNullOrWhiteSpace(property.BodyPrice) ? "" : property.BodyPrice);
                    cmd.Parameters.AddWithValue("@OtherExpenses", string.IsNullOrWhiteSpace(property.OtherExpenses) ? "" : property.OtherExpenses);
                    cmd.Parameters.AddWithValue("@ApplicantOwnContribution", string.IsNullOrWhiteSpace(property.ApplicantOwnContribution) ? "" : property.ApplicantOwnContribution);
                    cmd.Parameters.AddWithValue("@CommutedLoanAmount", string.IsNullOrWhiteSpace(property.CommutedLoanAmount) ? "" : property.CommutedLoanAmount);
                    cmd.Parameters.AddWithValue("@AreaVehicleDriven", string.IsNullOrWhiteSpace(property.AreaVehicleDriven) ? "" : property.AreaVehicleDriven);
                    cmd.Parameters.AddWithValue("@StatePermitNo", string.IsNullOrWhiteSpace(property.StatePermitNo) ? "" : property.StatePermitNo);
                    cmd.Parameters.AddWithValue("@StatePermitValidity", string.IsNullOrWhiteSpace(property.StatePermitValidity) ? "" : property.StatePermitValidity);
                    cmd.Parameters.AddWithValue("@MonthlyIncomeByVehicle", string.IsNullOrWhiteSpace(property.MonthlyIncomeByVehicle) ? "" : property.MonthlyIncomeByVehicle);
                    cmd.Parameters.AddWithValue("@ItemsBroughtByVehicle", string.IsNullOrWhiteSpace(property.ItemsBroughtByVehicle) ? "" : property.ItemsBroughtByVehicle);
                    cmd.Parameters.AddWithValue("@IncomeBasis", string.IsNullOrWhiteSpace(property.IncomeBasis) ? "" : property.IncomeBasis);
                    cmd.Parameters.AddWithValue("@VehicleRunDaysInMonth", string.IsNullOrWhiteSpace(property.VehicleRunDaysInMonth) ? "" : property.VehicleRunDaysInMonth);
                    cmd.Parameters.AddWithValue("@VehicleRunPerDay", string.IsNullOrWhiteSpace(property.VehicleRunPerDay) ? "" : property.VehicleRunPerDay);
                    cmd.Parameters.AddWithValue("@ProposedBuildingAndWorkStation", string.IsNullOrWhiteSpace(property.ProposedBuildingAndWorkStation) ? "" : property.ProposedBuildingAndWorkStation);
                    cmd.Parameters.AddWithValue("@FarePerKM", string.IsNullOrWhiteSpace(property.FarePerKM) ? "" : property.FarePerKM);
                    cmd.Parameters.AddWithValue("@WorkingCapitalFileFile", string.IsNullOrWhiteSpace(property.WorkingCapitalFileFile) ? "" : property.WorkingCapitalFileFile);
                    cmd.Parameters.AddWithValue("@ApplicantDealAfterApprovalFile", string.IsNullOrWhiteSpace(property.ApplicantDealAfterApprovalFile) ? "" : property.ApplicantDealAfterApprovalFile);
                    cmd.Parameters.AddWithValue("@EstimatedMonthlyEarnings", string.IsNullOrWhiteSpace(property.EstimatedMonthlyEarnings) ? "" : property.EstimatedMonthlyEarnings);
                    cmd.Parameters.AddWithValue("@OtherIncomeByVehicle", string.IsNullOrWhiteSpace(property.OtherIncomeByVehicle) ? "" : property.OtherIncomeByVehicle);
                    cmd.Parameters.AddWithValue("@MonthlyExpenditureFuel", string.IsNullOrWhiteSpace(property.MonthlyExpenditureFuel) ? "" : property.MonthlyExpenditureFuel);
                    cmd.Parameters.AddWithValue("@DriverSalaryAllowance", string.IsNullOrWhiteSpace(property.DriverSalaryAllowance) ? "" : property.DriverSalaryAllowance);
                    cmd.Parameters.AddWithValue("@RoadAndOtherTaxes", string.IsNullOrWhiteSpace(property.RoadAndOtherTaxes) ? "" : property.RoadAndOtherTaxes);
                    cmd.Parameters.AddWithValue("@RepairMaintenanceService", string.IsNullOrWhiteSpace(property.RepairMaintenanceService) ? "" : property.RepairMaintenanceService);
                    cmd.Parameters.AddWithValue("@InsurancePremiums", string.IsNullOrWhiteSpace(property.InsurancePremiums) ? "" : property.InsurancePremiums);
                    cmd.Parameters.AddWithValue("@OilSpareParts", string.IsNullOrWhiteSpace(property.OilSpareParts) ? "" : property.OilSpareParts);
                    cmd.Parameters.AddWithValue("@GarageRent", string.IsNullOrWhiteSpace(property.GarageRent) ? "" : property.GarageRent);
                    cmd.Parameters.AddWithValue("@WearAndTear", string.IsNullOrWhiteSpace(property.WearAndTear) ? "" : property.WearAndTear);
                    cmd.Parameters.AddWithValue("@OtherMiscellaneousExpenses", string.IsNullOrWhiteSpace(property.OtherMiscellaneousExpenses) ? "" : property.OtherMiscellaneousExpenses);
                    cmd.Parameters.AddWithValue("@InterestOnLoan", string.IsNullOrWhiteSpace(property.InterestOnLoan) ? "" : property.InterestOnLoan);
                    cmd.ExecuteScalar();
                });


                transaction.Commit();
            }
            catch (Exception ex)
            {
                Result = "-1";
                transaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " InsertUpdatePLDBLoanApplication(PLDPLoanApplication Applicationdatat)" }, ex);
            }
            finally
            {
                sqlConnection.Close();
            }
            return Result;
        }

        public DataTable GetLoanApplicationList(ref DataPagingModel Details, string SSOId, string RequestLevel)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "SearchText"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(application_no))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR REPLACE(lower(rtrim(ltrim(PLDBName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'OR REPLACE(lower(rtrim(ltrim(BranchName_En))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(BranchNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(PLDBNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim((select top 1 s.FatherHusbandName  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where  isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(ProceesStatusTextHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR convert(varchar(15),created_date,103)= '" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "' OR REPLACE(lower(rtrim(ltrim((select top 1 s.Name  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' )";
                    }



                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@RequestLevel", DBFactoryParameterType.NVarchar, RequestLevel);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@RequestLevel", DBFactoryParameterType.NVarchar, RequestLevel);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "DataTable GetLoanApplicationList()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetPLDBMemberDetails(string MembershipNo, int DistrictId, int BranchId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBLoanMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, MembershipNo);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.PLDPLoanApplication", FunctionName = "public DataTable GetPLDBUserRegistrationDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetLoanApplicationDetails(string Table, int LoanId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";

                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Loan_Id", DBFactoryParameterType.Int, LoanId);
                objDBAccess.AddParameterToCommand("@Table", DBFactoryParameterType.NVarchar, Table);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "DataTable GetLoanApplicationDetails(string Table,int LoanId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetLoanProcessStatus(int Level, int ActionId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";

                cmd = objDBAccess.CreateCommand("PLDBGetLoanProcessStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.Int, Level);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                //objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " public DataTable GetLoanProcessStatus()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }




        public DataSet GetPLDBDashboard(int Level = 1, int BranchId = 0, int PLDBId = 0)
        {
            DataSet ds = new DataSet();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                cmd = objDBAccess.CreateCommand("PLDBGetDashboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                ds = objDBAccess.GetDataSet();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "DataSet GetPLDBDashboard()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }


        public string UpdateLoanStatus(PLDBLoanMaster Details, string Level, List<PLDBLVODocuments> LVODocuments, List<PLDBLandAmount> PldbLandAmount)
        {
            string Result = "-1";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("PLDBUpdateLoanStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, Details.Status);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Details.Remark);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@LoanId", DBFactoryParameterType.Int, Details.Id);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.NVarchar, Level);
                objDBAccess.AddParameterToCommand("@ApprovedAmount", DBFactoryParameterType.Int, string.IsNullOrWhiteSpace(Details.ApprovedAmount) ? "0" : Details.ApprovedAmount);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                //bhupendra


                if (PldbLandAmount != null)
                {
                    PldbLandAmount = PldbLandAmount ?? new List<PLDBLandAmount>();
                    PldbLandAmount.ForEach(property =>
                    {

                        cmd = objDBAccess.CreateCommand("PLDBUpdateLVOData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, property.Id);
                        //objDBAccess.AddParameterToCommand("@loan_id", DBFactoryParameterType.NVarchar, property.Loan_Id);
                        objDBAccess.AddParameterToCommand("@Pure_Profit", DBFactoryParameterType.NVarchar, property.PureProfitAmount);
                        objDBAccess.AddParameterToCommand("@land_amount_value", DBFactoryParameterType.Int, property.LandValueAmount);
                        objDBAccess.AddParameterToCommand("@loan_paylimit_amount", DBFactoryParameterType.NVarchar, property.LoanPayLimitAmount);
                        objDBAccess.AddParameterToCommand("@yearly_installment", DBFactoryParameterType.NVarchar, property.YearlyInstallment);
                        objDBAccess.AddParameterToCommand("@pure_saving", DBFactoryParameterType.NVarchar, property.PureSaving);
                        //objDBAccess.AddParameterToCommand("@createdby", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@updatedby", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    });
                }








                LVODocuments = LVODocuments ?? new List<PLDBLVODocuments>();
                LVODocuments.ForEach(x =>
                {
                    cmd = objDBAccess.CreateCommand("InsertUpdateLVODocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@document_id", DBFactoryParameterType.Int, x.Document_Id);
                    objDBAccess.AddParameterToCommand("@loan_id", DBFactoryParameterType.NVarchar, x.Loan_Id);
                    objDBAccess.AddParameterToCommand("@document_type", DBFactoryParameterType.NVarchar, x.Document_Type);
                    objDBAccess.AddParameterToCommand("@document_url", DBFactoryParameterType.NVarchar, x.Document_Url);
                    objDBAccess.AddParameterToCommand("@is_active", DBFactoryParameterType.NVarchar, x.Is_Active);
                    objDBAccess.AddParameterToCommand("@is_delete", DBFactoryParameterType.Bit, x.Is_Delete);
                    objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                });


                //Result = "Success";
            }
            catch (Exception ex)
            {
                Result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "public string UpdateLoanStatus(PLDBLoanMaster Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeletePLDBSectorrecord(int sectorId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Deletesectors", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@sectorId", DBFactoryParameterType.Int, sectorId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteChangeStatusClasses(int ClassId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeletePLDBSchemerecord(int scheme_id, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("[DeleteScheme]", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@scheme_id", DBFactoryParameterType.Int, scheme_id);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteChangeStatusClasses(int ClassId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #region PLDB Masters
        public string InsertUpdateSchemeMaster(PLDBLoanMaster Details)
        {
            string Result = "3";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("InsertUpdateSchemeMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Details.Id);
                objDBAccess.AddParameterToCommand("@NameEn", DBFactoryParameterType.NVarchar, Details.NameEn);
                objDBAccess.AddParameterToCommand("@NameHi", DBFactoryParameterType.NVarchar, Details.NameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, Details.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@PerposeId", DBFactoryParameterType.Int, Details.PerposeId);
                objDBAccess.AddParameterToCommand("@sector_id", DBFactoryParameterType.Int, Details.SectorId);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " InsertUpdateSchemeMaster(PLDBLoanMaster Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetSchemeMaster(string WhereCase)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = objDBAccess.CreateCommand("GetSchemeMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetSchemeMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string InsertUpdateSectorMaster(PLDBLoanMaster Details)
        {
            string Result = "3";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("InsertUpdateSectorMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Details.Id);
                //objDBAccess.AddParameterToCommand("@SchemeId", DBFactoryParameterType.Int, Details.SchemeId);
                objDBAccess.AddParameterToCommand("@NameEn", DBFactoryParameterType.NVarchar, Details.NameEn);
                objDBAccess.AddParameterToCommand("@NameHi", DBFactoryParameterType.NVarchar, Details.NameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, Details.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Farming_NonFarming", DBFactoryParameterType.VARCHAR, Details.Farming_NonFarming);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " InsertUpdateSectorMaster(PLDBLoanMaster Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetSectorMaster(string WhereCase)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = objDBAccess.CreateCommand("GetSectorMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetSectorMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string InsertUpdatePurposeMaster(PLDBLoanMaster Details)
        {
            string Result = "3";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("InsertUpdatePurposeMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Details.Id);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, Details.SectorId);
                objDBAccess.AddParameterToCommand("@NameEn", DBFactoryParameterType.NVarchar, Details.NameEn);
                objDBAccess.AddParameterToCommand("@NameHi", DBFactoryParameterType.NVarchar, Details.NameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, Details.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@EffectiveDate", DBFactoryParameterType.DateTime, Details.EffectiveDate);
                objDBAccess.AddParameterToCommand("@NormalInterestRate", DBFactoryParameterType.Float, Details.NormalInterestRate);
                objDBAccess.AddParameterToCommand("@PenalInterestRate", DBFactoryParameterType.Float, Details.PenalInterestRate);
                objDBAccess.AddParameterToCommand("@Farming_NonFarming", DBFactoryParameterType.VARCHAR, Details.Farming_NonFarming);
                objDBAccess.AddParameterToCommand("@RepaymentPeriod", DBFactoryParameterType.Int, Details.RepaymentPeriod);
                objDBAccess.AddParameterToCommand("@DueInstallment", DBFactoryParameterType.Int, Details.DueInstallment);
                objDBAccess.AddParameterToCommand("@GracePeriod", DBFactoryParameterType.Int, Details.GracePeriod);
                objDBAccess.AddParameterToCommand("@scheme_id", DBFactoryParameterType.Int, Details.SchemeId);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " InsertUpdateSectorMaster(PLDBLoanMaster Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetPurposeMaster(string WhereCase)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = objDBAccess.CreateCommand("GetPurposeMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetPurposeMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int DeletePurpose(int Purpose_Id, int Action)
        {
            int Return = 1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeletePurposeMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Purpose_Id);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, Action);
                Return = objDBAccess.ExecuteCommand();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetPurposeMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Return;
        }

        public DataTable GetSchemebyType(int id)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSchemeListbyType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsfarmingType", DBFactoryParameterType.NVarchar, id);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeletePurpose(int Purpose_Id, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetPurposebyType(int id)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetPurposeListbyType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsfarmingType", DBFactoryParameterType.NVarchar, id);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = " GetPurposebyType(int id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet GetApplicationDetails(string LoanId)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PLDBGetLoanApplicationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LoanId", DBFactoryParameterType.NVarchar, LoanId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserLevel", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.PLDPLoanApplication", FunctionName = "public DataTable GetApplicationDetails(" + LoanId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }


        public DataSet GetOtsApplicantDetails(string otsApplicationId, int summaryId, int type = 0, int isAccountCloseInSinglePayment = 0, string accountCloseDate = null, string NoticeNo = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetOtsApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetOtsApplicantDetails");
                objDBAccess.AddParameterToCommand("@otsIdEnc", DBFactoryParameterType.VARCHAR, otsApplicationId);
                objDBAccess.AddParameterToCommand("@SummaryId", DBFactoryParameterType.Int, summaryId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, type);
                objDBAccess.AddParameterToCommand("@AccountCloseDate", DBFactoryParameterType.VARCHAR, accountCloseDate);
                objDBAccess.AddParameterToCommand("@NoticeNo", DBFactoryParameterType.VARCHAR, NoticeNo);
                objDBAccess.AddParameterToCommand("@IsAccountCloseInSinglePayment", DBFactoryParameterType.Int, isAccountCloseInSinglePayment);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetOtsApplicantDetails(string otsApplicationId, int summaryId, int type = 0, int isAccountCloseInSinglePayment=0" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet SearchOtsCitizenApplicantDetails(OtsCitizenApplicantDetails applicantDetails)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetOtsCitizenApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_searchOtsApplication");
                objDBAccess.AddParameterToCommand("@LoanAccNo", DBFactoryParameterType.VARCHAR, applicantDetails.LoanAccNo);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.VARCHAR, applicantDetails.RegistrationNo);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, applicantDetails.MobileNo);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, applicantDetails.PldbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, applicantDetails.BranchId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "SearchOtsCitizenApplicantDetails(OtsCitizenApplicantDetails applicantDetails)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetOtsCitizenApplicantDetails(string EncPldbOtsSchemeId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetOtsCitizenApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_getOtsApplication");
                objDBAccess.AddParameterToCommand("@EncPldbOtsSchemeId", DBFactoryParameterType.NVarchar, EncPldbOtsSchemeId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetOtsCitizenApplicantDetails(string EncPldbOtsSchemeId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public int SaveRepaymentSummary(OtsRepaymentSummary otsRepayment)
        {
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OTSRePayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_addPRReceipt");
                objDBAccess.AddParameterToCommand("@OtsIdEnc", DBFactoryParameterType.NVarchar, otsRepayment.OtsIdEnc);
                objDBAccess.AddParameterToCommand("@PaymentModeId", DBFactoryParameterType.Int, otsRepayment.PaymentModeId);
                objDBAccess.AddParameterToCommand("@PaidAmount", DBFactoryParameterType.VARCHAR, otsRepayment.PaidAmount);
                objDBAccess.AddParameterToCommand("@PaymentDate", DBFactoryParameterType.VARCHAR, otsRepayment.PaymentDate);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UTR_RefNo", DBFactoryParameterType.VARCHAR, otsRepayment.UTR_RefNo);
                objDBAccess.AddParameterToCommand("@DepositByPerson", DBFactoryParameterType.VARCHAR, otsRepayment.DepositByPerson);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, otsRepayment.Remark);
                objDBAccess.AddParameterToCommand("@ChequeNo", DBFactoryParameterType.NVarchar, otsRepayment.ChequeNo);
                objDBAccess.AddParameterToCommand("@ChequeDate", DBFactoryParameterType.NVarchar, otsRepayment.ChequeDate);
                objDBAccess.AddParameterToCommand("@ChequeOther", DBFactoryParameterType.NVarchar, otsRepayment.ChequeOther);
                objDBAccess.AddParameterToCommand("@IsAccountCloseInSinglePayment", DBFactoryParameterType.Int, otsRepayment.IsAccountCloseInSinglePayment);
                objDBAccess.AddParameterToCommand("@AccountCloseDate", DBFactoryParameterType.VARCHAR, otsRepayment.AccountCloseDate);
                objDBAccess.AddParameterToCommand("@IsFinalPayment", DBFactoryParameterType.Bit, otsRepayment.IsFinalPayment);
                objDBAccess.AddParameterToCommand("@InterestAmount", DBFactoryParameterType.VARCHAR, otsRepayment.InterestAmount);
                objDBAccess.AddParameterToCommand("@PenalInterestAmount", DBFactoryParameterType.VARCHAR, otsRepayment.PenalInterestAmount);
                objDBAccess.AddParameterToCommand("@PrincipalAmount", DBFactoryParameterType.VARCHAR, otsRepayment.PrincipalAmount);
                objDBAccess.AddParameterToCommand("@OtherAmount", DBFactoryParameterType.VARCHAR, otsRepayment.OtherAmount);
                objDBAccess.AddParameterToCommand("@FinalRemark", DBFactoryParameterType.NVarchar, otsRepayment.FinalRemark);
                objDBAccess.AddParameterToCommand("@RecoveryPersonType", DBFactoryParameterType.Int, otsRepayment.RecoveryPersonType);
                objDBAccess.AddParameterToCommand("@RecoveryPersonName", DBFactoryParameterType.NVarchar, otsRepayment.RecoveryPersonName);
                objDBAccess.AddParameterToCommand("@RecoveryPersonDesignation", DBFactoryParameterType.NVarchar, otsRepayment.RecoveryPersonDesignation);

                objDBAccess.AddParameterToCommand("@IpAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIp());
                return Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "SaveRepaymentSummary(OtsRepaymentSummary otsRepayment)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return -1;
        }

        public int SaveFinalPaymentSummary(OtsFinalPaymentSummary otsFinalpayment)
        {
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OTSPaymentSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_addFRReceipt");
                objDBAccess.AddParameterToCommand("@OtsApplicationId", DBFactoryParameterType.NVarchar, otsFinalpayment.OtsApplicationId);

                objDBAccess.AddParameterToCommand("@Actual", DBFactoryParameterType.NVarchar, otsFinalpayment.Actual);
                objDBAccess.AddParameterToCommand("@InsurancePremium", DBFactoryParameterType.NVarchar, otsFinalpayment.InsurancePremium);
                objDBAccess.AddParameterToCommand("@OtherAmount", DBFactoryParameterType.NVarchar, otsFinalpayment.OtherAmount);
                objDBAccess.AddParameterToCommand("@CurrentActual", DBFactoryParameterType.NVarchar, otsFinalpayment.CurrentActual);
                objDBAccess.AddParameterToCommand("@CurrentInsurancePremium", DBFactoryParameterType.NVarchar, otsFinalpayment.CurrentInsurancePremium);
                objDBAccess.AddParameterToCommand("@CurrentInterest", DBFactoryParameterType.NVarchar, otsFinalpayment.CurrentInterest);
                objDBAccess.AddParameterToCommand("@CurrentPenalInterest", DBFactoryParameterType.NVarchar, otsFinalpayment.CurrentPenalInterest);
                objDBAccess.AddParameterToCommand("@CurrentOtherAmount", DBFactoryParameterType.NVarchar, otsFinalpayment.CurrentOtherAmount);
                objDBAccess.AddParameterToCommand("@RecoveryCharge", DBFactoryParameterType.NVarchar, otsFinalpayment.RecoveryCharge);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, otsFinalpayment.Remark);

                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@IpAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIp());
                return objDBAccess.ExecuteCommand();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " SaveFinalPaymentSummary(OtsFinalPaymentSummary otsFinalpayment)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }


        public int SaveFinalReceipt(PLDB_OTSSchemeData pLDB_OTSScheme)
        {
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OTSSchemeDataUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_UpdateOTSSchemeData");
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.NVarchar, pLDB_OTSScheme.PLDB_OTS_SchemeID);

                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, pLDB_OTSScheme.Status);
                objDBAccess.AddParameterToCommand("@CertificateGenerated", DBFactoryParameterType.NVarchar, pLDB_OTSScheme.CertificateGenerated);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, pLDB_OTSScheme.Remark);
                objDBAccess.AddParameterToCommand("@IpAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIp());

                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                return objDBAccess.ExecuteCommand();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " SaveFinalPaymentSummary(OtsFinalPaymentSummary otsFinalpayment)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 0;
        }
        #endregion

        public DataTable GetPLDBDetailsData(int Id)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = objDBAccess.CreateCommand("GetPLDBDatailsData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetPurposeMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetPLDBDetailsStatusData(string StatusVal, int PLDBid, int BranchId, int Level)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            String whereCase = "";
            if (StatusVal != "")
            {
                // whereCase += " and ss.ProceesStatusTextEn=" + "''" + statusval + "''";
                if (StatusVal == "APPLICATIONSUBMITTED")
                {
                    whereCase += " and SPS.SocietyProceesStatusId in(1036,1037,1038)";
                }
                else if (StatusVal == "APPROVEANDGENERATEMEMBERSHIP")
                {
                    whereCase += " and SPS.SocietyProceesStatusId in(1043)";
                }
                else if (StatusVal == "OBJECTIONSUBMITTED ")
                {
                    whereCase += " and SPS.SocietyProceesStatusId in(1039,1045)";
                }
                else if (StatusVal == "OBJECTIONRESPONSE")
                {
                    whereCase += " and SPS.SocietyProceesStatusId  in(1041,1042)";
                }
                else if (StatusVal == "REJECT")
                {
                    whereCase += " and  SPS.SocietyProceesStatusId  in(1044)";
                }
                else if (StatusVal == "TOTALREGISTRATION")
                {
                    whereCase += " ";
                }
                else
                {
                    whereCase += "and  SPS.ProceesStatusTextEn= " + "'" + StatusVal + "'";
                }

            }
            else
            {
                whereCase = "and 1=1";
            }
            try
            {

                cmd = objDBAccess.CreateCommand("GetPLDBDatailsDataByStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, StatusVal);
                objDBAccess.AddParameterToCommand("@PLDBid", DBFactoryParameterType.Int, PLDBid);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@whereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@ReportLevel", DBFactoryParameterType.NVarchar, Level);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetPurposeMaster(string WhereCase)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetLoanApplicationListByStatus(ref DataPagingModel Details, string statusval, int PLDBid, int BranchId, int Level)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "SearchText"))
                    {
                        whereCase += " and " + x.Key + "=" + x.Value + "";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(application_no))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR REPLACE(lower(rtrim(ltrim(PLDBName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'OR REPLACE(lower(rtrim(ltrim(BranchName_En))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(BranchNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(PLDBNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim((select top 1 s.FatherHusbandName  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where  isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(ProceesStatusTextHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR convert(varchar(15),created_date,103)= '" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "' OR REPLACE(lower(rtrim(ltrim((select top 1 s.Name  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' )";
                    }



                }
                if (statusval != "")
                {
                    // whereCase += " and ss.ProceesStatusTextEn=" + "''" + statusval + "''";
                    if (statusval == "APPLICATIONSUBMITTED")
                    {
                        whereCase += " and ss.status_id  in(1,2,3)";
                    }
                    else if (statusval == "OBJECTIONSUBMITTED")
                    {
                        whereCase += " and ss.status_id  in(7,8,9,10)";
                    }
                    else if (statusval == "OBJECTIONRESPONSE")
                    {
                        whereCase += " and ss.status_id  in(5,6)";
                    }
                    else if (statusval == "REJECT")
                    {
                        whereCase += " and ss.status_id  in(11)";
                    }
                    else if (statusval == "REJECT")
                    {
                        whereCase += " and ss.status_id  in(11)";
                    }
                    else if (statusval == "TotalLoanApplication")
                    {
                        whereCase += " ";
                    }
                    else
                    {
                        whereCase += "and and ss.status_id  in(" + statusval + ")";
                    }

                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationStatusWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, StatusVal);
                objDBAccess.AddParameterToCommand("@PLDBid", DBFactoryParameterType.Int, PLDBid);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                objDBAccess.AddParameterToCommand("@whereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@ReportLevel", DBFactoryParameterType.NVarchar, Level);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "DataTable GetLoanApplicationList()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetPLDBLoanApplicationLVOData(ref DataPagingModel Details, string SSOId, string Type)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "SearchText"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(application_no))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR REPLACE(lower(rtrim(ltrim(PLDBName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'OR REPLACE(lower(rtrim(ltrim(BranchName_En))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(BranchNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(PLDBNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim((select top 1 s.FatherHusbandName  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where  isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(ProceesStatusTextHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR convert(varchar(15),created_date,103)= '" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "' OR REPLACE(lower(rtrim(ltrim((select top 1 s.Name  from PLDBLoanMembers m inner join SocietyMemberRegistration s on s.MembershipNo=m.MembershipNo  where   isnull(m.IsDelete,0)=0 and  m.loan_id=ap.loan_id)))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' )";
                    }
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationLVOData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationLVOData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationAPI", FunctionName = "DataTable GetPLDBLoanApplicationLVOData()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet GetLoanApplicationLVODetails(string LoanId)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PLDBGetLoanApplicationLVODetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LoanId", DBFactoryParameterType.NVarchar, LoanId);
                // objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                // objDBAccess.AddParameterToCommand("@UserLevel", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.PLDPLoanApplication", FunctionName = "public DataTable GetApplicationDetails(" + LoanId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }

        public DataTable GetPLDBLoanApplicationCountLVO(string SSOId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetPLDBLoanApplicationCountLVO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationAPI", FunctionName = "DataTable GetPLDBLoanApplicationLVOData()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveGeoData(List<ServicesController.PLDBGeoData> Data)
        {
            int result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                Data.ForEach(x =>
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("PLDBSaveGeoData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@LoanId", DBFactoryParameterType.NVarchar, x.LoanId);
                    objDBAccess.AddParameterToCommand("@PropartyId", DBFactoryParameterType.NVarchar, x.PropartyId);
                    objDBAccess.AddParameterToCommand("@PropartyType", DBFactoryParameterType.NVarchar, x.PropartyType);
                    objDBAccess.AddParameterToCommand("@SubmitType", DBFactoryParameterType.NVarchar, x.SubmitType);
                    objDBAccess.AddParameterToCommand("@Latitude", DBFactoryParameterType.NVarchar, x.Latitude);
                    objDBAccess.AddParameterToCommand("@Longitude", DBFactoryParameterType.NVarchar, x.Longitude);
                    objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, x.Photo);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, x.SSOID);
                    objDBAccess.ExecuteCommand();
                    result = 1;
                });

            }
            catch (Exception ex)
            {
                result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SaveGeoData", FunctionName = "public DataTable SaveGeoData(" + JsonConvert.SerializeObject(Data) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }




        #region  OTSList by Madhuri

        public DataTable GetOTSList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string IsLandMortgaged = "";

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
                    whereCase += " and DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and PLDB_Id= " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Any())
                {
                    whereCase += " and PLDB_BranchId= " + Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Any())
                {
                    IsLandMortgaged = Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(p.PLDBName,'',' ') like '%" + Value + "%' or replace(p.PLDBBranch,' ','')  like '%" + Value
                        + "%' or replace(p.NoticeNo,' ','') like '%" + Value + "%' or replace(p.ApplicantName,' ','') like '%" + Value + "%' or replace(p.FatherHusbandName,' ','') like '%" + Value + "%' or replace(p.JanAadharNumber,' ','') like '%" + Value
                        + "%' or replace(p.MobileNo,' ','') like '%" + Value + "%' or replace(p.LoanAccNo,' ','') like '%" + Value + "%' or replace(sector,' ','') like '%" + Value + "%' or replace(Purpose,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetOTSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    if (Details.SearchParameter.Where(x => x.Key == "_103Cases").Any())
                    {
                        objDBAccess.AddParameterToCommand("@Is103Case", DBFactoryParameterType.Int, Details.SearchParameter.Where(x => x.Key == "_103Cases").Select(x => x.Value).FirstOrDefault());
                    }

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetOTSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetOTSList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        //old
        //[CustomAuthentication]
        //public int InsertOTSReportFile(OTSDocumentsModel _Model)
        //{

        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    int Result = 0;
        //    string qry = "";
        //    try
        //    {

        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("InsertOTSReportFile", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@OTSId", DBFactoryParameterType.Int, _Model.OTSId);
        //        // objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, _Model.DocumentURL);
        //        objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocId);
        //        objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.Remark);
        //        objDBAccess.AddParameterToCommand("@TaamilDoc", DBFactoryParameterType.NVarchar, _Model.TaamilDoc);
        //        objDBAccess.AddParameterToCommand("@OtherDoc", DBFactoryParameterType.NVarchar, _Model.OtherDoc);
        //        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);


        //        qry = objDBAccess.getSqlCommandText();
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = " InsertOTSReportFile(GccDocumentsModel _Model)" + qry }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}


        //New
        [CustomAuthentication]
        public int InsertOTSReportFile(OTSDocumentsNewModel _Model)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertOTSReportFile_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OTSId", DBFactoryParameterType.Int, _Model.OTSId);
                // objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, _Model.DocumentURL);
                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.RemarkNew);
                objDBAccess.AddParameterToCommand("@TaamilDoc", DBFactoryParameterType.NVarchar, _Model.TaamilDoc_New);
                objDBAccess.AddParameterToCommand("@OtherDoc", DBFactoryParameterType.NVarchar, _Model.OtherDoc_New);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);

                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = " InsertOTSReportFile(OTSDocumentsNewModel _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        [CustomAuthentication]
        public int UploadOTSAadharFile(OTSAadharJanaadharModel _Model)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SP_UploadOTS_AadharFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OtsIdEnc", DBFactoryParameterType.NVarchar, _Model.OtsIdEnc);
                objDBAccess.AddParameterToCommand("@Aadhar_Janaadhar_no", DBFactoryParameterType.NVarchar, _Model.Aadhar_Janaadhar_no);
                objDBAccess.AddParameterToCommand("@Aadhar_Janaadhar_FileName", DBFactoryParameterType.NVarchar, _Model.Aadhar_Janaadhar_FileName);

                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = " UploadOTSAadharFile(OTSAadharJanaadharModel _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        [CustomAuthentication]
        public int Save_PldbApplicationForm(PLDB_OTSSchemeData _Model)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_InsertOTSApplication", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.Int, _Model.PLDB_OTS_SchemeID);
                objDBAccess.AddParameterToCommand("@PLDBName", DBFactoryParameterType.NVarchar, _Model.PLDBName);
                objDBAccess.AddParameterToCommand("@PLDBBranch", DBFactoryParameterType.NVarchar, _Model.PLDBBranch);
                objDBAccess.AddParameterToCommand("@ApplicantName", DBFactoryParameterType.NVarchar, _Model.ApplicantName);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, _Model.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@JanAadharNumber", DBFactoryParameterType.NVarchar, _Model.JanAadharNumber);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, _Model.MobileNo);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, _Model.Gender);
                objDBAccess.AddParameterToCommand("@VName", DBFactoryParameterType.NVarchar, _Model.VName);
                //objDBAccess.AddParameterToCommand("@GpName", DBFactoryParameterType.NVarchar, _Model.GpName);
                objDBAccess.AddParameterToCommand("@BlockName", DBFactoryParameterType.NVarchar, _Model.BlockName);
                objDBAccess.AddParameterToCommand("@TehsilName", DBFactoryParameterType.NVarchar, _Model.TehsilName);
                objDBAccess.AddParameterToCommand("@VidhanSabha", DBFactoryParameterType.NVarchar, _Model.VidhanSabha);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, _Model.DistrictName);
                objDBAccess.AddParameterToCommand("@CategoryName", DBFactoryParameterType.NVarchar, _Model.CategoryName);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, _Model.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeJanAadharNumber", DBFactoryParameterType.NVarchar, _Model.NomineeJanAadharNumber);
                objDBAccess.AddParameterToCommand("@NomineeMobileNo", DBFactoryParameterType.NVarchar, _Model.NomineeMobileNo);
                objDBAccess.AddParameterToCommand("@LoanAccNo", DBFactoryParameterType.NVarchar, _Model.LoanAccNo);
                objDBAccess.AddParameterToCommand("@NoOfLoanBen", DBFactoryParameterType.Int, _Model.NoOfLoanBen);
                objDBAccess.AddParameterToCommand("@Sector", DBFactoryParameterType.NVarchar, _Model.Sector);
                objDBAccess.AddParameterToCommand("@Purpose", DBFactoryParameterType.NVarchar, _Model.Purpose);
                objDBAccess.AddParameterToCommand("@LoanSanctionAmt", DBFactoryParameterType.Float, _Model.LoanSanctionAmt);

                objDBAccess.AddParameterToCommand("@RateOfInterest", DBFactoryParameterType.Float, _Model.RateOfInterest);
                objDBAccess.AddParameterToCommand("@TotalDisbursedAmt", DBFactoryParameterType.Float, _Model.TotalDisbursedAmt);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.NVarchar, _Model.IsLandMortgaged);
                objDBAccess.AddParameterToCommand("@LandMortgagedland", DBFactoryParameterType.NVarchar, _Model.LandMortgagedland);
                objDBAccess.AddParameterToCommand("@OtherPropertyDetails", DBFactoryParameterType.NVarchar, _Model.OtherPropertyDetails);
                objDBAccess.AddParameterToCommand("@CurrentDlcValueOfProperty", DBFactoryParameterType.Float, _Model.CurrentDlcValueOfProperty);

                objDBAccess.AddParameterToCommand("@ODPrincipal", DBFactoryParameterType.Float, _Model.ODPrincipal);
                objDBAccess.AddParameterToCommand("@ODInsurancePremium", DBFactoryParameterType.Float, _Model.ODInsurancePremium);
                objDBAccess.AddParameterToCommand("@ODInterest", DBFactoryParameterType.Float, _Model.ODInterest);
                objDBAccess.AddParameterToCommand("@ODPenalInterast", DBFactoryParameterType.Float, _Model.ODPenalInterast);
                objDBAccess.AddParameterToCommand("@ODTotal", DBFactoryParameterType.Float, _Model.ODTotal);
                objDBAccess.AddParameterToCommand("@NODPrincipal", DBFactoryParameterType.Float, _Model.NODPrincipal);
                objDBAccess.AddParameterToCommand("@NODInsurancePremium", DBFactoryParameterType.Float, _Model.NODInsurancePremium);
                objDBAccess.AddParameterToCommand("@NODInterest", DBFactoryParameterType.Float, _Model.NODInterest);
                objDBAccess.AddParameterToCommand("@NODPenalInterest", DBFactoryParameterType.Float, _Model.NODPenalInterest);
                objDBAccess.AddParameterToCommand("@NODTotal", DBFactoryParameterType.Float, _Model.NODTotal);
                objDBAccess.AddParameterToCommand("@CDPrincipal", DBFactoryParameterType.Float, _Model.CDPrincipal);
                objDBAccess.AddParameterToCommand("@CDInterest", DBFactoryParameterType.Float, _Model.CDInterest);
                objDBAccess.AddParameterToCommand("@CDPenalInterest", DBFactoryParameterType.Float, _Model.CDPenalInterest);
                objDBAccess.AddParameterToCommand("@CDTotalInterest", DBFactoryParameterType.Float, _Model.CDTotalInterest);
                objDBAccess.AddParameterToCommand("@CDTotal", DBFactoryParameterType.Float, _Model.CDTotal);
                objDBAccess.AddParameterToCommand("@PrincipalOutstandingODNOD", DBFactoryParameterType.Float, _Model.PrincipalOutstandingODNOD);
                objDBAccess.AddParameterToCommand("@TotalLoanOutstanding", DBFactoryParameterType.Float, _Model.TotalLoanOutstanding);

                objDBAccess.AddParameterToCommand("@LegalActionStageCompletedAction", DBFactoryParameterType.NVarchar, _Model.LegalActionStageCompletedAction);

                objDBAccess.AddParameterToCommand("@PrincipalOutstandingAsOnDateSettlement", DBFactoryParameterType.Float, _Model.PrincipalOutstandingAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@TotalInterestAsOnDateSettlement", DBFactoryParameterType.Float, _Model.TotalInterestAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@TotalLoanOutstandingAsOnDateSettlement", DBFactoryParameterType.Float, _Model.TotalLoanOutstandingAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@InterestFromSettlementPre", DBFactoryParameterType.Float, _Model.InterestFromSettlementPre);
                objDBAccess.AddParameterToCommand("@InterestFromSettlement", DBFactoryParameterType.Float, _Model.InterestFromSettlement);
                objDBAccess.AddParameterToCommand("@TotalAmount", DBFactoryParameterType.Float, _Model.TotalAmount);
                objDBAccess.AddParameterToCommand("@IsNiActLegalActionTaken", DBFactoryParameterType.NVarchar, _Model.IsNiActLegalActionTaken);
                objDBAccess.AddParameterToCommand("@CurrentStatusOfNiActCaseNotice", DBFactoryParameterType.NVarchar, _Model.CurrentStatusOfNiActCaseNotice);
                objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, _Model.PLDB_Id);
                objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _Model.PLDB_BranchId);
                objDBAccess.AddParameterToCommand("@VId", DBFactoryParameterType.Int, _Model.VId);
                objDBAccess.AddParameterToCommand("@GpId", DBFactoryParameterType.Int, _Model.GpId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, _Model.BlockId);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, _Model.TehsilId);
                objDBAccess.AddParameterToCommand("@VidhanSabhaId", DBFactoryParameterType.Int, _Model.VidhanSabhaId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _Model.DistrictId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, _Model.CategoryId);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, _Model.SectorId);
                objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, _Model.PurposeId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                objDBAccess.AddParameterToCommand("@DateOfFirstDisbursement", DBFactoryParameterType.NVarchar, _Model.DateOfFirstDisbursement);
                //objDBAccess.AddParameterToCommand("@DateOfSettlement", DBFactoryParameterType.VARCHAR, _Model.DateOfSettlement);
                objDBAccess.AddParameterToCommand("@LegalActionStageCompletedDate", DBFactoryParameterType.NVarchar, _Model.LegalActionStageCompletedDate);
                objDBAccess.AddParameterToCommand("@DateOfOD", DBFactoryParameterType.NVarchar, _Model.DateOfOD);
                objDBAccess.AddParameterToCommand("@DateOfSanction", DBFactoryParameterType.NVarchar, _Model.DateOfSanction);
                objDBAccess.AddParameterToCommand("@PendingLevelId", DBFactoryParameterType.Int, _Model.PendingLevelId);
                objDBAccess.AddParameterToCommand("@MutationNo", DBFactoryParameterType.NVarchar, _Model.MutationNo);
                objDBAccess.AddParameterToCommand("@MutationDate", DBFactoryParameterType.VARCHAR, _Model.MutationDate);
                objDBAccess.AddParameterToCommand("@LandMortgagedPendingLevelId", DBFactoryParameterType.Int, _Model.LandMortgagedPendingLevelId);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationNo", DBFactoryParameterType.NVarchar, _Model.LandMortgagedMutationNo);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationDate", DBFactoryParameterType.VARCHAR, _Model.LandMortgagedMutationDate);


                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = " InsertOTSReportFile(GccDocumentsModel _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        [CustomAuthentication]
        public int SavePLDB_PendingLevelData(PLDB_OTSSchemeData _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string mutationDocumentPath = null, landMortgagedMutationDocumentPath = null;
            string destDir = HttpContext.Current.Server.MapPath("~/Content/OTS/MutationDoc/");

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            if (_Model.MutationDocument != null && _Model.MutationDocument != "")
            {
                if (File.Exists(_Model.MutationDocument))
                {
                    mutationDocumentPath = Path.GetFileName(_Model.MutationDocument);
                    File.Move(_Model.MutationDocument, Path.Combine(destDir, mutationDocumentPath));
                }
            }
            if (_Model.LandMortgagedMutationDocument != null && _Model.LandMortgagedMutationDocument != "")
            {
                if (File.Exists(_Model.LandMortgagedMutationDocument))
                {
                    landMortgagedMutationDocumentPath = Path.GetFileName(_Model.LandMortgagedMutationDocument);
                    File.Move(_Model.LandMortgagedMutationDocument, Path.Combine(destDir, landMortgagedMutationDocumentPath));
                }
            }
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_UpdatePldbPendingLevelData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.Int, _Model.PLDB_OTS_SchemeID);
                objDBAccess.AddParameterToCommand("@PendingLevelId", DBFactoryParameterType.Int, _Model.PendingLevelId);
                objDBAccess.AddParameterToCommand("@MutationNo", DBFactoryParameterType.NVarchar, _Model.MutationNo);
                objDBAccess.AddParameterToCommand("@MutationDate", DBFactoryParameterType.VARCHAR, _Model.MutationDate);
                objDBAccess.AddParameterToCommand("@MutationDocument", DBFactoryParameterType.NVarchar, mutationDocumentPath);
                objDBAccess.AddParameterToCommand("@LandMortgagedPendingLevelId", DBFactoryParameterType.Int, _Model.LandMortgagedPendingLevelId);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationNo", DBFactoryParameterType.NVarchar, _Model.LandMortgagedMutationNo);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationDate", DBFactoryParameterType.VARCHAR, _Model.LandMortgagedMutationDate);
                //new added
                objDBAccess.AddParameterToCommand("@ReleasedLandAreaHectare", DBFactoryParameterType.Float, _Model.ReleasedLandAreaHectare);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationDocument", DBFactoryParameterType.NVarchar, landMortgagedMutationDocumentPath);
                objDBAccess.AddParameterToCommand("@IsMortgageMandatory", DBFactoryParameterType.Int, _Model.IsMortgageMandatory);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress().ToString());

                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = "public int SavePLDB_PendingLevelData(PLDB_OTSSchemeData _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetOTSListApplicationFormViewData(string Ids, string Action, string MobileNo = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_OTSApplicationFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, MobileNo);
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
        #endregion


        #region OTS Pr Status Update

        public int UpdateRepaymentSummary(int Id, string Remark = "")
        {
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_OTSRePayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_DeletePaymentStatus");
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@OTSRePaymentSummaryId", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@IpAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIp());
                return Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "UpdateRepaymentSummary(int Id,string Remark=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return -1;
        }

        #endregion

        #region GetRecoveryPaymentStatistics Added by Narpat 
        public DataSet GetPldbPaymentRecoveryStatistics(int districtId = 0, int pldbId = 0, int pldbBranchId = 0)
        {
            string actionType = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                string spname = "PldbRecoveryPaymentStatisticsReport";
                if (districtId > 0 && pldbId > 0 && pldbBranchId == 0) { actionType = "GetRecoveryPaymentByPldb"; }
                else if (districtId > 0 && pldbId > 0 && pldbBranchId > 0) { actionType = "GetRecoveryPaymentByByPldbBranch"; }
                else { actionType = "GetPldbRecoveryPaymentSummary"; }

                cmd = objDBAccess.CreateCommand(spname, DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.Int, pldbBranchId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MisModel", FunctionName = "GetPldbPaymentRecoveryStatistics(string Action = '" + actionType + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetNiActApplicationDetail(string otsApplicationId, int summaryId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("NIActDetailsSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNIActDetailByotsId");
                //objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, 0);
                //objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.NVarchar, 0);
                //objDBAccess.AddParameterToCommand("@PLDB_Branch_ID", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@otsIdEnc", DBFactoryParameterType.NVarchar, otsApplicationId);
                objDBAccess.AddParameterToCommand("@SummaryId", DBFactoryParameterType.Int, summaryId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetOtsNiActApplicantDetails(string otsApplicationId, int summaryId" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public int SavePLDBNIActDetails(NIActDetailsModel nIActDetails)
        {
            int result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NIActDetailsSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "SaveNiActDetails");
                objDBAccess.AddParameterToCommand("@otsIdEnc", DBFactoryParameterType.VARCHAR, nIActDetails.OtsIdEnc);
                objDBAccess.AddParameterToCommand("@IsChequesCollectedFromLoanee", DBFactoryParameterType.Bit, nIActDetails.IsChequesCollectedFromLoanee);
                objDBAccess.AddParameterToCommand("@IsChequesPresentedBankForClearing", DBFactoryParameterType.Bit, nIActDetails.IsChequesPresentedBankForClearing);
                objDBAccess.AddParameterToCommand("@ChequeNo", DBFactoryParameterType.VARCHAR, nIActDetails.ChequeNo);
                objDBAccess.AddParameterToCommand("@Bankdetails", DBFactoryParameterType.NVarchar, nIActDetails.Bankdetails);
                objDBAccess.AddParameterToCommand("@ChequeDate", DBFactoryParameterType.VARCHAR, nIActDetails.ChequeDate);
                objDBAccess.AddParameterToCommand("@ChequeAmt", DBFactoryParameterType.VARCHAR, nIActDetails.ChequeAmt);
                objDBAccess.AddParameterToCommand("@IsChequeDishonoured", DBFactoryParameterType.Bit, nIActDetails.IsChequeDishonoured);
                objDBAccess.AddParameterToCommand("@IsChequeDishonourActionTaken", DBFactoryParameterType.Bit, nIActDetails.IsChequeDishonourActionTaken);
                objDBAccess.AddParameterToCommand("@ActionTakenDate", DBFactoryParameterType.VARCHAR, nIActDetails.ActionTakenDate);
                objDBAccess.AddParameterToCommand("@CaseNo", DBFactoryParameterType.NVarchar, nIActDetails.CaseNo);
                objDBAccess.AddParameterToCommand("@CourtDetails", DBFactoryParameterType.NVarchar, nIActDetails.CourtDetails);
                objDBAccess.AddParameterToCommand("@LastHearingDate", DBFactoryParameterType.VARCHAR, nIActDetails.LastHearingDate);
                objDBAccess.AddParameterToCommand("@NextHearingDate", DBFactoryParameterType.VARCHAR, nIActDetails.NextHearingDate);
                objDBAccess.AddParameterToCommand("@Judgement", DBFactoryParameterType.NVarchar, nIActDetails.Judgement);
                objDBAccess.AddParameterToCommand("@JudgementFavourOfBank", DBFactoryParameterType.NVarchar, nIActDetails.JudgementFavourOfBank);
                objDBAccess.AddParameterToCommand("@JudgementFavourOfLoanee", DBFactoryParameterType.NVarchar, nIActDetails.JudgementFavourOfLoanee);
                objDBAccess.AddParameterToCommand("@RecoveryAmt", DBFactoryParameterType.VARCHAR, nIActDetails.RecoveryAmt);
                objDBAccess.AddParameterToCommand("@DishonourActionReason", DBFactoryParameterType.NVarchar, nIActDetails.DishonourActionReason);
                objDBAccess.AddParameterToCommand("@OTSSettlementProposed", DBFactoryParameterType.NVarchar, nIActDetails.OTSSettlementProposed);
                objDBAccess.AddParameterToCommand("@IsSettlementProposed", DBFactoryParameterType.Bit, nIActDetails.IsSettlementProposed);
                objDBAccess.ExecuteCommand();
                result = 1;

            }
            catch (Exception ex)
            {
                result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.PLDPLoanApplication", FunctionName = "SavePLDBNIActDetails(" + JsonConvert.SerializeObject(nIActDetails) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveCaseNotMortgageStatus(CaseNotMortageModel caseNotMortage)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_UpdateCaseNotMortgageStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "_Update");
                objDBAccess.AddParameterToCommand("@otsIdEnc", DBFactoryParameterType.VARCHAR, caseNotMortage.OtsIdEnc);
                objDBAccess.AddParameterToCommand("@CaseNotMortgagedStatus", DBFactoryParameterType.Int, caseNotMortage.CaseNotMortgagedStatus);
                objDBAccess.ExecuteCommand();
                result = 1;

            }
            catch (Exception ex)
            {
                result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.PLDPLoanApplication", FunctionName = "SaveCaseNotMortgageStatus(" + JsonConvert.SerializeObject(caseNotMortage) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        #endregion


        [CustomAuthentication]
        public int Save_PldbSummaryODSubsidyApplicationForm(PLDB_SummaryODSubsidy _Model)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            string qry = "";
            int IsBackend = _Model.PLDB_OTS_SchemeID > 0 ? 2 : 1;
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_InsertPldbSubsidyODApplication", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.Int, _Model.PLDB_OTS_SchemeID);
                objDBAccess.AddParameterToCommand("@PLDBName", DBFactoryParameterType.NVarchar, _Model.PLDBName);
                objDBAccess.AddParameterToCommand("@PLDBBranch", DBFactoryParameterType.NVarchar, _Model.PLDBBranch);
                objDBAccess.AddParameterToCommand("@ApplicantName", DBFactoryParameterType.NVarchar, _Model.ApplicantName);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, _Model.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@JanAadharNumber", DBFactoryParameterType.NVarchar, _Model.JanAadharNumber);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, _Model.MobileNo);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, _Model.Gender);
                objDBAccess.AddParameterToCommand("@VName", DBFactoryParameterType.NVarchar, _Model.VName);
                objDBAccess.AddParameterToCommand("@GpName", DBFactoryParameterType.NVarchar, _Model.GpName);
                objDBAccess.AddParameterToCommand("@BlockName", DBFactoryParameterType.NVarchar, _Model.BlockName);
                objDBAccess.AddParameterToCommand("@TehsilName", DBFactoryParameterType.NVarchar, _Model.TehsilName);
                objDBAccess.AddParameterToCommand("@VidhanSabha", DBFactoryParameterType.NVarchar, _Model.VidhanSabha);
                objDBAccess.AddParameterToCommand("@CityName", DBFactoryParameterType.NVarchar, _Model.CityName);
                objDBAccess.AddParameterToCommand("@WardName", DBFactoryParameterType.NVarchar, _Model.WardName);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, _Model.DistrictName);
                objDBAccess.AddParameterToCommand("@CategoryName", DBFactoryParameterType.NVarchar, _Model.CategoryName);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, _Model.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeJanAadharNumber", DBFactoryParameterType.NVarchar, _Model.NomineeJanAadharNumber);
                objDBAccess.AddParameterToCommand("@NomineeMobileNo", DBFactoryParameterType.NVarchar, _Model.NomineeMobileNo);
                objDBAccess.AddParameterToCommand("@LoanAccNo", DBFactoryParameterType.NVarchar, _Model.LoanAccNo);
                objDBAccess.AddParameterToCommand("@NoOfLoanBen", DBFactoryParameterType.Int, _Model.NoOfLoanBen);
                objDBAccess.AddParameterToCommand("@Sector", DBFactoryParameterType.NVarchar, _Model.Sector);
                objDBAccess.AddParameterToCommand("@Purpose", DBFactoryParameterType.NVarchar, _Model.Purpose);
                objDBAccess.AddParameterToCommand("@LoanSanctionAmt", DBFactoryParameterType.Float, _Model.LoanSanctionAmt);
                objDBAccess.AddParameterToCommand("@RateOfInterest", DBFactoryParameterType.Float, _Model.RateOfInterest);
                objDBAccess.AddParameterToCommand("@TotalDisbursedAmt", DBFactoryParameterType.Float, _Model.TotalDisbursedAmt);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.NVarchar, _Model.IsLandMortgaged);
                objDBAccess.AddParameterToCommand("@LandMortgagedland", DBFactoryParameterType.NVarchar, _Model.LandMortgagedland);
                objDBAccess.AddParameterToCommand("@OtherPropertyDetails", DBFactoryParameterType.NVarchar, _Model.OtherPropertyDetails);
                objDBAccess.AddParameterToCommand("@CurrentDlcValueOfProperty", DBFactoryParameterType.Float, _Model.CurrentDlcValueOfProperty);
                objDBAccess.AddParameterToCommand("@ODPrincipal", DBFactoryParameterType.Float, _Model.ODPrincipal);
                objDBAccess.AddParameterToCommand("@ODInsurancePremium", DBFactoryParameterType.Float, _Model.ODInsurancePremium);
                objDBAccess.AddParameterToCommand("@ODInterest", DBFactoryParameterType.Float, _Model.ODInterest);
                objDBAccess.AddParameterToCommand("@ODPenalInterast", DBFactoryParameterType.Float, _Model.ODPenalInterast);
                objDBAccess.AddParameterToCommand("@ODTotal", DBFactoryParameterType.Float, _Model.ODTotal);
                objDBAccess.AddParameterToCommand("@NODPrincipal", DBFactoryParameterType.Float, _Model.NODPrincipal);
                objDBAccess.AddParameterToCommand("@NODInsurancePremium", DBFactoryParameterType.Float, _Model.NODInsurancePremium);
                objDBAccess.AddParameterToCommand("@NODInterest", DBFactoryParameterType.Float, _Model.NODInterest);
                objDBAccess.AddParameterToCommand("@NODPenalInterest", DBFactoryParameterType.Float, _Model.NODPenalInterest);
                objDBAccess.AddParameterToCommand("@NODTotal", DBFactoryParameterType.Float, _Model.NODTotal);
                objDBAccess.AddParameterToCommand("@CDPrincipal", DBFactoryParameterType.Float, _Model.CDPrincipal);
                objDBAccess.AddParameterToCommand("@CDInterest", DBFactoryParameterType.Float, _Model.CDInterest);
                objDBAccess.AddParameterToCommand("@CDPenalInterest", DBFactoryParameterType.Float, _Model.CDPenalInterest);
                objDBAccess.AddParameterToCommand("@CDTotalInterest", DBFactoryParameterType.Float, _Model.CDTotalInterest);
                objDBAccess.AddParameterToCommand("@CDTotal", DBFactoryParameterType.Float, _Model.CDTotal);
                objDBAccess.AddParameterToCommand("@PrincipalOutstandingODNOD", DBFactoryParameterType.Float, _Model.PrincipalOutstandingODNOD);
                objDBAccess.AddParameterToCommand("@TotalLoanOutstanding", DBFactoryParameterType.Float, _Model.TotalLoanOutstanding);
                objDBAccess.AddParameterToCommand("@LegalActionStageCompletedAction", DBFactoryParameterType.NVarchar, _Model.LegalActionStageCompletedAction);
                objDBAccess.AddParameterToCommand("@PrincipalOutstandingAsOnDateSettlement", DBFactoryParameterType.Float, _Model.PrincipalOutstandingAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@TotalInterestAsOnDateSettlement", DBFactoryParameterType.Float, _Model.TotalInterestAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@TotalLoanOutstandingAsOnDateSettlement", DBFactoryParameterType.Float, _Model.TotalLoanOutstandingAsOnDateSettlement);
                objDBAccess.AddParameterToCommand("@InterestFromSettlementPre", DBFactoryParameterType.Float, _Model.InterestFromSettlementPre);
                objDBAccess.AddParameterToCommand("@InterestFromSettlement", DBFactoryParameterType.Float, _Model.InterestFromSettlement);
                objDBAccess.AddParameterToCommand("@TotalAmount", DBFactoryParameterType.Float, _Model.TotalAmount);
                objDBAccess.AddParameterToCommand("@IsNiActLegalActionTaken", DBFactoryParameterType.NVarchar, _Model.IsNiActLegalActionTaken);
                objDBAccess.AddParameterToCommand("@CurrentStatusOfNiActCaseNotice", DBFactoryParameterType.NVarchar, _Model.CurrentStatusOfNiActCaseNotice);
                objDBAccess.AddParameterToCommand("@PLDB_Id", DBFactoryParameterType.Int, _Model.PLDB_Id);
                objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _Model.PLDB_BranchId);
                objDBAccess.AddParameterToCommand("@VId", DBFactoryParameterType.Int, _Model.VId);
                objDBAccess.AddParameterToCommand("@GpId", DBFactoryParameterType.Int, _Model.GpId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, _Model.BlockId);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, _Model.TehsilId);
                objDBAccess.AddParameterToCommand("@VidhanSabhaId", DBFactoryParameterType.Int, _Model.VidhanSabhaId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _Model.DistrictId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, _Model.CategoryId);
                objDBAccess.AddParameterToCommand("@SectorId", DBFactoryParameterType.Int, _Model.SectorId);
                objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, _Model.PurposeId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                // objDBAccess.AddParameterToCommand("@DateOfSettlement", DBFactoryParameterType.DateTime, string.IsNullOrEmpty(_Model.DateOfSettlement) ? (object)DBNull.Value : (object)Convert.ToDateTime(_Model.DateOfSettlement));

                objDBAccess.AddParameterToCommand("@DateOfFirstDisbursement", DBFactoryParameterType.VARCHAR, _Model.DateOfFirstDisbursement.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LegalActionStageCompletedDate", DBFactoryParameterType.VARCHAR, _Model.LegalActionStageCompletedDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@DateOfOD", DBFactoryParameterType.VARCHAR, _Model.DateOfOD.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@DateOfSanction", DBFactoryParameterType.VARCHAR, _Model.DateOfSanction.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@MutationDate", DBFactoryParameterType.VARCHAR, _Model.MutationDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationDate", DBFactoryParameterType.VARCHAR, _Model.LandMortgagedMutationDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ChequeDate", DBFactoryParameterType.VARCHAR, _Model.ChequeDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ActionTakenDate", DBFactoryParameterType.VARCHAR, _Model.ActionTakenDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LastHearingDate", DBFactoryParameterType.VARCHAR, _Model.LastHearingDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@NextHearingDate", DBFactoryParameterType.VARCHAR, _Model.NextHearingDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@Notice99Date", DBFactoryParameterType.VARCHAR, _Model.Notice99Date.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@Order99Date", DBFactoryParameterType.VARCHAR, _Model.Order99Date.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@Order100Date", DBFactoryParameterType.VARCHAR, _Model.Order100Date.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LandAuctionDate", DBFactoryParameterType.VARCHAR, _Model.LandAuctionDate.ToString("yyyy-MM-dd"));

                objDBAccess.AddParameterToCommand("@PendingLevelId", DBFactoryParameterType.Int, _Model.PendingLevelId);
                objDBAccess.AddParameterToCommand("@MutationNo", DBFactoryParameterType.NVarchar, _Model.MutationNo);
                objDBAccess.AddParameterToCommand("@LandMortgagedPendingLevelId", DBFactoryParameterType.Int, _Model.LandMortgagedPendingLevelId);
                objDBAccess.AddParameterToCommand("@LandMortgagedMutationNo", DBFactoryParameterType.NVarchar, _Model.LandMortgagedMutationNo);
                objDBAccess.AddParameterToCommand("@IsChequesCollectedFromLoanee", DBFactoryParameterType.Bit, _Model.IsChequesCollectedFromLoanee);
                objDBAccess.AddParameterToCommand("@IsChequesPresentedBankForClearing", DBFactoryParameterType.Bit, _Model.IsChequesPresentedBankForClearing);
                objDBAccess.AddParameterToCommand("@ChequeNo", DBFactoryParameterType.VARCHAR, _Model.ChequeNo);
                objDBAccess.AddParameterToCommand("@Bankdetails", DBFactoryParameterType.NVarchar, _Model.Bankdetails);
                objDBAccess.AddParameterToCommand("@ChequeAmt", DBFactoryParameterType.Float, _Model.ChequeAmt);
                objDBAccess.AddParameterToCommand("@IsChequeDishonoured", DBFactoryParameterType.Bit, _Model.IsChequeDishonoured);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, _Model.IsRural);
                objDBAccess.AddParameterToCommand("@IsDeathCase", DBFactoryParameterType.Bit, _Model.IsDeathCase);
                objDBAccess.AddParameterToCommand("@IsChequeDishonourActionTaken", DBFactoryParameterType.Bit, _Model.IsChequeDishonourActionTaken);
                objDBAccess.AddParameterToCommand("@CaseNo", DBFactoryParameterType.NVarchar, _Model.CaseNo);
                objDBAccess.AddParameterToCommand("@CourtDetails", DBFactoryParameterType.NVarchar, _Model.CourtDetails);
                objDBAccess.AddParameterToCommand("@Judgement", DBFactoryParameterType.NVarchar, _Model.Judgement);
                objDBAccess.AddParameterToCommand("@JudgementFavourOfBank", DBFactoryParameterType.NVarchar, _Model.JudgementFavourOfBank);
                objDBAccess.AddParameterToCommand("@JudgementFavourOfLoanee", DBFactoryParameterType.NVarchar, _Model.JudgementFavourOfLoanee);
                objDBAccess.AddParameterToCommand("@RecoveryAmt", DBFactoryParameterType.VARCHAR, _Model.RecoveryAmt);
                objDBAccess.AddParameterToCommand("@DishonourActionReason", DBFactoryParameterType.NVarchar, _Model.DishonourActionReason);
                objDBAccess.AddParameterToCommand("@OTSSettlementProposed", DBFactoryParameterType.NVarchar, _Model.OTSSettlementProposed);
                objDBAccess.AddParameterToCommand("@IsSettlementProposed", DBFactoryParameterType.Bit, _Model.IsSettlementProposed);
                objDBAccess.AddParameterToCommand("@IsNotice99", DBFactoryParameterType.Bit, _Model.IsNotice99);
                objDBAccess.AddParameterToCommand("@IsOrder99", DBFactoryParameterType.Bit, _Model.IsOrder99);
                objDBAccess.AddParameterToCommand("@IsNotice100", DBFactoryParameterType.Bit, _Model.IsNotice100);
                objDBAccess.AddParameterToCommand("@IsLandAuction", DBFactoryParameterType.Bit, _Model.IsLandAuction);
                objDBAccess.AddParameterToCommand("@IsMutation", DBFactoryParameterType.Bit, _Model.IsMutation);
                objDBAccess.AddParameterToCommand("@CurrentDlcValueOfOtherProperty", DBFactoryParameterType.Float, _Model.CurrentDlcValueOfOtherProperty);
                objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, _Model.CITY_ID);
                objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, _Model.WARD_ID);
                objDBAccess.AddParameterToCommand("@Principal", DBFactoryParameterType.Float, _Model.Principal);
                objDBAccess.AddParameterToCommand("@OtherCharges", DBFactoryParameterType.Float, _Model.OtherCharges);
                objDBAccess.AddParameterToCommand("@IntSubvention", DBFactoryParameterType.Float, _Model.IntSubvention);
                objDBAccess.AddParameterToCommand("@IsBackend", DBFactoryParameterType.Int, IsBackend);

                qry = objDBAccess.getSqlCommandText();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = " Save_PldbSummaryODSubsidyApplicationForm(PLDB_SummaryODSubsidy _Model)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSubsidySummaryList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string IsLandMortgaged = "";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and p.DistrictId = " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and p.PLDB_Id= " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Any())
                {
                    whereCase += " and p.PLDB_BranchId= " + Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Any())
                {
                    IsLandMortgaged = Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(p.PLDBName,'',' ') like '%" + Value + "%' or replace(p.PLDBBranch,' ','')  like '%" + Value
                        + "%' or replace(p.NoticeNo,' ','') like '%" + Value + "%' or replace(p.ApplicantName,' ','') like '%" + Value + "%' or replace(p.FatherHusbandName,' ','') like '%" + Value + "%' or replace(p.JanAadharNumber,' ','') like '%" + Value
                        + "%' or replace(p.MobileNo,' ','') like '%" + Value + "%' or replace(p.LoanAccNo,' ','') like '%" + Value + "%' or replace(sector,' ','') like '%" + Value + "%' or replace(Purpose,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSubsidySummaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    if (Details.SearchParameter.Where(x => x.Key == "_103Cases").Any())
                    {
                        objDBAccess.AddParameterToCommand("@Is103Case", DBFactoryParameterType.Int, Details.SearchParameter.Where(x => x.Key == "_103Cases").Select(x => x.Value).FirstOrDefault());
                    }

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSubsidySummaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetSubsidySummaryList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable PldbLoanApplicationDetailsList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                //District Filter
                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and p.PldbId = " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }
                // Block Filter 
                if (Details.SearchParameter.Any(x => x.Key == "BLOCK_ID"))
                {
                    whereCase += " AND p.BranchId = " + Details.SearchParameter.FirstOrDefault(x => x.Key == "BLOCK_ID").Value;
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " AND ( " +
                 " REPLACE(p.ApplicantName, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.FatherHusbandName, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.MobileNo, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.LoanAccNo, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.AadharNumber, ' ', '') LIKE '%" + Value + "%' " +
                 " )";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("sp_GetPLDBLoanApplicationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, _LoggedInUser.PLDBId);
                    objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _LoggedInUser.PLDBBrancchId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("sp_GetPLDBLoanApplicationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = "PldbLoanApplicationDetailsList(string RNO, string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSubsidyListApplicationFormViewData(string Ids, string Action, string MobileNo = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_Get_SubsidySummaryApplicationFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.NVarchar, Ids);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, MobileNo);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = "Get_GopalApplicationFormViewData(string RNO, string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataSet GetPLDBLoanApplicationListFormViewData(string Ids)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_GetPLDBLoanApplicationListFormViewData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbOtsSchemeId", DBFactoryParameterType.Int, Ids);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "PLDBLoan",
                    FunctionName = "GetPLDBLoanApplicationListFormViewData"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }



        [CustomAuthentication]
        public int Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            int result = 0;
            string qry = string.Empty;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_InsetLoanApplicationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);

                // Loan Application Details

                objDBAccess.AddParameterToCommand("@ApplicantName", DBFactoryParameterType.NVarchar, model.ApplicantName);
                objDBAccess.AddParameterToCommand("@ApplicantName2", DBFactoryParameterType.NVarchar, model.ApplicantName2);
                objDBAccess.AddParameterToCommand("@ApplicantName3", DBFactoryParameterType.NVarchar, model.ApplicantName3);
                objDBAccess.AddParameterToCommand("@ApplicantName4", DBFactoryParameterType.NVarchar, model.ApplicantName4);
                objDBAccess.AddParameterToCommand("@ApplicantName5", DBFactoryParameterType.NVarchar, model.ApplicantName5);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, model.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@FatherHusbandName2", DBFactoryParameterType.NVarchar, model.FatherHusbandName2);
                objDBAccess.AddParameterToCommand("@FatherHusbandName3", DBFactoryParameterType.NVarchar, model.FatherHusbandName3);
                objDBAccess.AddParameterToCommand("@FatherHusbandName4", DBFactoryParameterType.NVarchar, model.FatherHusbandName4);
                objDBAccess.AddParameterToCommand("@FatherHusbandName5", DBFactoryParameterType.NVarchar, model.FatherHusbandName5);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, model.Gender);
                objDBAccess.AddParameterToCommand("@Gender2", DBFactoryParameterType.NVarchar, model.Gender2);
                objDBAccess.AddParameterToCommand("@Gender3", DBFactoryParameterType.NVarchar, model.Gender3);
                objDBAccess.AddParameterToCommand("@Gender4", DBFactoryParameterType.NVarchar, model.Gender4);
                objDBAccess.AddParameterToCommand("@Gender5", DBFactoryParameterType.NVarchar, model.Gender5);
                objDBAccess.AddParameterToCommand("@LoanneAge", DBFactoryParameterType.Int, model.LoanneAge);
                objDBAccess.AddParameterToCommand("@LoanneAge2", DBFactoryParameterType.Int, model.LoanneAge2);
                objDBAccess.AddParameterToCommand("@LoanneAge3", DBFactoryParameterType.Int, model.LoanneAge3);
                objDBAccess.AddParameterToCommand("@LoanneAge4", DBFactoryParameterType.Int, model.LoanneAge4);
                objDBAccess.AddParameterToCommand("@LoanneAge5", DBFactoryParameterType.Int, model.LoanneAge5);
                objDBAccess.AddParameterToCommand("@CategoryName", DBFactoryParameterType.NVarchar, model.CategoryName);
                objDBAccess.AddParameterToCommand("@FarmerType", DBFactoryParameterType.NVarchar, model.FarmerType);
                objDBAccess.AddParameterToCommand("@LoanAccNo", DBFactoryParameterType.NVarchar, model.LoanAccNo);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, model.MobileNo);
                objDBAccess.AddParameterToCommand("@AadharNumber", DBFactoryParameterType.NVarchar, model.AadharNumber);
                objDBAccess.AddParameterToCommand("@PANNumber", DBFactoryParameterType.NVarchar, model.PANNumber);
                objDBAccess.AddParameterToCommand("@VName", DBFactoryParameterType.NVarchar, model.VName);
                objDBAccess.AddParameterToCommand("@TehsilName", DBFactoryParameterType.NVarchar, model.TehsilName);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, model.DistrictName);
                objDBAccess.AddParameterToCommand("@LegislativeAssemblyArea", DBFactoryParameterType.NVarchar, model.LegislativeAssemblyArea);
                objDBAccess.AddParameterToCommand("@ParliamentaryConstituency", DBFactoryParameterType.NVarchar, model.ParliamentaryConstituency);
                objDBAccess.AddParameterToCommand("@LoanApplicationDate", DBFactoryParameterType.VARCHAR, model.LoanApplicationDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ODDate", DBFactoryParameterType.VARCHAR, model.ODDate?.ToString("yyyy-MM-dd") ?? "");
                objDBAccess.AddParameterToCommand("@LoanApplicationNo", DBFactoryParameterType.NVarchar, model.LoanApplicationNo);
                objDBAccess.AddParameterToCommand("@LoanPurpose", DBFactoryParameterType.NVarchar, model.LoanPurpose);
                objDBAccess.AddParameterToCommand("@ProjectCostAmount", DBFactoryParameterType.Float, model.ProjectCostAmount);
                objDBAccess.AddParameterToCommand("@ODPrincipalAmount", DBFactoryParameterType.Float, model.ODPrincipalAmount);
                objDBAccess.AddParameterToCommand("@ODInterestAmount", DBFactoryParameterType.Float, model.ODInterestAmount);
                objDBAccess.AddParameterToCommand("@PenaltyInterestOtherCharges", DBFactoryParameterType.Float, model.PenaltyInterestOtherCharges);
                objDBAccess.AddParameterToCommand("@InsurancePremium", DBFactoryParameterType.Float, model.InsurancePremium);
                objDBAccess.AddParameterToCommand("@TotalODAmount", DBFactoryParameterType.Float, model.TotalODAmount);
                objDBAccess.AddParameterToCommand("@AppliedLoanAmount", DBFactoryParameterType.Float, model.AppliedLoanAmount);
                objDBAccess.AddParameterToCommand("@SubsidyLoanAmount", DBFactoryParameterType.Float, model.SubsidyLoanAmount);
                objDBAccess.AddParameterToCommand("@PhotoVerifierName", DBFactoryParameterType.NVarchar, model.PhotoVerifierName);
                objDBAccess.AddParameterToCommand("@GP_Other", DBFactoryParameterType.NVarchar, model.GP_Other);
                objDBAccess.AddParameterToCommand("@Village_Other", DBFactoryParameterType.NVarchar, model.Village_Other);
                objDBAccess.AddParameterToCommand("@PhotoVerifierDesignation", DBFactoryParameterType.NVarchar, model.PhotoVerifierDesignation);
                objDBAccess.AddParameterToCommand("@IsRegistrationCertificateReceived", DBFactoryParameterType.Bit, model.IsRegistrationCertificateReceived);
                objDBAccess.AddParameterToCommand("@IsEstimateQuotationAttached", DBFactoryParameterType.Bit, model.IsEstimateQuotationAttached);
                objDBAccess.AddParameterToCommand("@AIsJamabandiAttached", DBFactoryParameterType.Bit, model.AIsJamabandiAttached);
                objDBAccess.AddParameterToCommand("@AIsKhasraGirdawariCopyAttached", DBFactoryParameterType.Bit, model.AIsKhasraGirdawariCopyAttached);
                objDBAccess.AddParameterToCommand("@IsAvadhipaar", DBFactoryParameterType.Bit, model.IsAvadhipaar);
                objDBAccess.AddParameterToCommand("@AIsLandMapAttached", DBFactoryParameterType.Bit, model.AIsLandMapAttached);
                objDBAccess.AddParameterToCommand("@AIsOwnershipAffidavitAttached", DBFactoryParameterType.Bit, model.AIsOwnershipAffidavitAttached);
                objDBAccess.AddParameterToCommand("@BApplicantregistrationAttact", DBFactoryParameterType.Bit, model.BApplicantregistrationAttact);
                objDBAccess.AddParameterToCommand("@BAIsLandMapAttached", DBFactoryParameterType.Bit, model.BAIsLandMapAttached);
                objDBAccess.AddParameterToCommand("@IsOtherGramPanchayat", DBFactoryParameterType.Bit, model.IsOtherGramPanchayat);
                objDBAccess.AddParameterToCommand("@IsFinalSubmit", DBFactoryParameterType.Bit, model.IsFinalSubmit);
                objDBAccess.AddParameterToCommand("@IsLoanDetailsAgrimMICRCheckTaken", DBFactoryParameterType.Bit, model.IsLoanDetailsAgrimMICRCheckTaken);
                objDBAccess.AddParameterToCommand("@BBankApprovedValuerName", DBFactoryParameterType.NVarchar, model.BBankApprovedValuerName);
                objDBAccess.AddParameterToCommand("@ProposedMortgageLand", DBFactoryParameterType.NVarchar, model.ProposedMortgageLand);
                objDBAccess.AddParameterToCommand("@AverageLandRatePerHectare", DBFactoryParameterType.Float, model.AverageLandRatePerHectare);
                objDBAccess.AddParameterToCommand("@TotalLandRatePerHectare", DBFactoryParameterType.Float, model.TotalLandRatePerHectare);
                objDBAccess.AddParameterToCommand("@LVOReportLoanValue", DBFactoryParameterType.Float, model.LVOReportLoanValue);
                objDBAccess.AddParameterToCommand("@LVOReportLoanPayValue", DBFactoryParameterType.VARCHAR, model.LVOReportLoanPayValue);
                objDBAccess.AddParameterToCommand("@LoanAcceptDate", DBFactoryParameterType.VARCHAR, model.LoanAcceptDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LoanAcceptPay", DBFactoryParameterType.Float, model.LoanAcceptPay);
                objDBAccess.AddParameterToCommand("@Interestrate", DBFactoryParameterType.Float, model.Interestrate);
                objDBAccess.AddParameterToCommand("@Accept_NAccpetDate", DBFactoryParameterType.VARCHAR, model.Accept_NAccpetDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@bankPartyEnrollementNumber", DBFactoryParameterType.Float, model.bankPartyEnrollementNumber);
                objDBAccess.AddParameterToCommand("@bankPartyEnrollementNumberDate", DBFactoryParameterType.VARCHAR, model.bankPartyEnrollementNumberDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@AuthorizationLetterIssueDate", DBFactoryParameterType.VARCHAR, model.AuthorizationLetterIssueDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@FarmCompanyAuthorizedLoanName", DBFactoryParameterType.NVarchar, model.FarmCompanyAuthorizedLoanName);
                objDBAccess.AddParameterToCommand("@ChequeNo", DBFactoryParameterType.NVarchar, model.ChequeNo);
                objDBAccess.AddParameterToCommand("@Bankdetails", DBFactoryParameterType.NVarchar, model.Bankdetails);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, model.BlockId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, model.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, model.WardId);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, model.TehsilId);
                objDBAccess.AddParameterToCommand("@AssemblyId", DBFactoryParameterType.Int, model.AssemblyId);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, model.PldbId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, model.BranchId);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, model.GP_ID);
                objDBAccess.AddParameterToCommand("@VId", DBFactoryParameterType.Int, model.VId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Int, model.IsRural);
                objDBAccess.AddParameterToCommand("@PLDBSectorId", DBFactoryParameterType.Int, model.SectorId);
                objDBAccess.AddParameterToCommand("@PLDBSectorId2", DBFactoryParameterType.Int, model.SectorId2);
                objDBAccess.AddParameterToCommand("@PLDBSectorId3", DBFactoryParameterType.Int, model.SectorId3);
                objDBAccess.AddParameterToCommand("@PLDBSectorId4", DBFactoryParameterType.Int, model.SectorId4);
                objDBAccess.AddParameterToCommand("@PLDBSectorId5", DBFactoryParameterType.Int, model.SectorId5);
                objDBAccess.AddParameterToCommand("@PLDBSectorId6", DBFactoryParameterType.Int, model.SectorId6);
                objDBAccess.AddParameterToCommand("@PLDBSectorId7", DBFactoryParameterType.Int, model.SectorId7);
                objDBAccess.AddParameterToCommand("@PurposeId", DBFactoryParameterType.Int, model.PurposeId);
                objDBAccess.AddParameterToCommand("@PurposeId2", DBFactoryParameterType.Int, model.PurposeId2);
                objDBAccess.AddParameterToCommand("@PurposeId3", DBFactoryParameterType.Int, model.PurposeId3);
                objDBAccess.AddParameterToCommand("@PurposeId4", DBFactoryParameterType.Int, model.PurposeId4);
                objDBAccess.AddParameterToCommand("@PurposeId5", DBFactoryParameterType.Int, model.PurposeId5);
                objDBAccess.AddParameterToCommand("@PurposeId6", DBFactoryParameterType.Int, model.PurposeId6);
                objDBAccess.AddParameterToCommand("@PurposeId7", DBFactoryParameterType.Int, model.PurposeId7);
                objDBAccess.AddParameterToCommand("@BankBranch", DBFactoryParameterType.NVarchar, model.BankBranch);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, model.FinancialYearId);
                objDBAccess.AddParameterToCommand("@PLDB_OTS_SchemeID", DBFactoryParameterType.Int, model.PLDB_OTS_SchemeID);

                // Guarantor Details

                var g = model.GuarantorDetails ?? new PLDB_GuarantorDetailsModel();

                objDBAccess.AddParameterToCommand("@First_Name", DBFactoryParameterType.NVarchar, g.First_Name);
                objDBAccess.AddParameterToCommand("@First_MobileNo", DBFactoryParameterType.NVarchar, g.First_MobileNo);
                objDBAccess.AddParameterToCommand("@First_AadharNo", DBFactoryParameterType.NVarchar, g.First_AadharNo);
                objDBAccess.AddParameterToCommand("@First_ChequeNumber", DBFactoryParameterType.NVarchar, g.First_ChequeNumber);
                objDBAccess.AddParameterToCommand("@First_Bank", DBFactoryParameterType.NVarchar, g.First_Bank);
                objDBAccess.AddParameterToCommand("@First_Branch", DBFactoryParameterType.NVarchar, g.First_Branch);
                objDBAccess.AddParameterToCommand("@First_Bhumi_Area", DBFactoryParameterType.NVarchar, g.First_Bhumi_Area);
                objDBAccess.AddParameterToCommand("@First_Bhumi_Mulyaa", DBFactoryParameterType.Float, g.First_Bhumi_Mulyaa);
                objDBAccess.AddParameterToCommand("@First_Other_Area", DBFactoryParameterType.NVarchar, g.First_Other_Area);
                objDBAccess.AddParameterToCommand("@First_Other_Mulyaa", DBFactoryParameterType.Float, g.First_Other_Mulyaa);
                objDBAccess.AddParameterToCommand("@First_OccupationOrServiceType", DBFactoryParameterType.NVarchar, g.First_OccupationOrServiceType);
                objDBAccess.AddParameterToCommand("@First_OccupationOrServiceTypeId", DBFactoryParameterType.Int, g.First_OccupationOrServiceTypeId);
                objDBAccess.AddParameterToCommand("@IsFirstAgrimMICRCheckTaken", DBFactoryParameterType.Bit, g.IsFirstAgrimMICRCheckTaken);
                objDBAccess.AddParameterToCommand("@Second_Name", DBFactoryParameterType.NVarchar, g.Second_Name);
                objDBAccess.AddParameterToCommand("@Second_MobileNo", DBFactoryParameterType.NVarchar, g.Second_MobileNo);
                objDBAccess.AddParameterToCommand("@Second_AadharNo", DBFactoryParameterType.NVarchar, g.Second_AadharNo);
                objDBAccess.AddParameterToCommand("@Second_ChequeNumber", DBFactoryParameterType.NVarchar, g.Second_ChequeNumber);
                objDBAccess.AddParameterToCommand("@Second_Bank", DBFactoryParameterType.NVarchar, g.Second_Bank);
                objDBAccess.AddParameterToCommand("@Second_Branch", DBFactoryParameterType.NVarchar, g.Second_Branch);
                objDBAccess.AddParameterToCommand("@Second_Bhumi_Area", DBFactoryParameterType.NVarchar, g.Second_Bhumi_Area);
                objDBAccess.AddParameterToCommand("@Second_Bhumi_Mulyaa", DBFactoryParameterType.Float, g.Second_Bhumi_Mulyaa);
                objDBAccess.AddParameterToCommand("@Second_Other_Area", DBFactoryParameterType.NVarchar, g.Second_Other_Area);
                objDBAccess.AddParameterToCommand("@Second_Other_Mulyaa", DBFactoryParameterType.Float, g.Second_Other_Mulyaa);
                objDBAccess.AddParameterToCommand("@Second_OccupationOrServiceType", DBFactoryParameterType.NVarchar, g.Second_OccupationOrServiceType);
                objDBAccess.AddParameterToCommand("@Second_OccupationOrServiceTypeId", DBFactoryParameterType.Int, g.Second_OccupationOrServiceTypeId);
                objDBAccess.AddParameterToCommand("@IsSecondAgrimMICRCheckTaken", DBFactoryParameterType.Bit, g.IsSecondAgrimMICRCheckTaken);

                //// Loan Disbursement

                var d = model.LoanDisbursement ?? new PLDB_LoanDisbursementApprovalModel();

                objDBAccess.AddParameterToCommand("@IsLoanDistribution", DBFactoryParameterType.Bit, d.IsLoanDistribution);
                objDBAccess.AddParameterToCommand("@IsApprovalOnNotesheet", DBFactoryParameterType.Bit, d.IsApprovalOnNotesheet);
                objDBAccess.AddParameterToCommand("@FirstInstallmentDate", DBFactoryParameterType.VARCHAR, d.FirstInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@FirstInstallmentMoney", DBFactoryParameterType.Float, d.FirstInstallmentMoney);
                objDBAccess.AddParameterToCommand("@SecondInstallmentDate", DBFactoryParameterType.VARCHAR, d.SecondInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@SecondInstallmentMoney", DBFactoryParameterType.Float, d.SecondInstallmentMoney);
                objDBAccess.AddParameterToCommand("@ThirdInstallmentDate", DBFactoryParameterType.VARCHAR, d.ThirdInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@ThirdInstallmentMoney", DBFactoryParameterType.Float, d.ThirdInstallmentMoney);
                objDBAccess.AddParameterToCommand("@VendorChequeNumber", DBFactoryParameterType.NVarchar, d.VendorChequeNumber);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, d.ReceiptNumber);
                objDBAccess.AddParameterToCommand("@VendorAmount", DBFactoryParameterType.Float, d.VendorAmount);
                objDBAccess.AddParameterToCommand("@BillNumber", DBFactoryParameterType.NVarchar, d.BillNumber);
                objDBAccess.AddParameterToCommand("@GSTNo", DBFactoryParameterType.NVarchar, d.GSTNo);
                objDBAccess.AddParameterToCommand("@BillDate", DBFactoryParameterType.VARCHAR, d.BillDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@CertificateFirstInstallmentDate", DBFactoryParameterType.VARCHAR, d.CertificateFirstInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@CertificateSecondInstallmentDate", DBFactoryParameterType.VARCHAR, d.CertificateSecondInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@CertificateThirdInstallmentDate", DBFactoryParameterType.VARCHAR, d.CertificateThirdInstallmentDate.ToString("yyyy-MM-dd"));
                objDBAccess.AddParameterToCommand("@LoanTimeInYear", DBFactoryParameterType.Int, d.LoanTimeInYear);
                objDBAccess.AddParameterToCommand("@InstallmentYearWise", DBFactoryParameterType.NVarchar, d.InstallmentYearWise);
                objDBAccess.AddParameterToCommand("@InsuranceMedicalCertificate", DBFactoryParameterType.NVarchar, d.InsuranceMedicalCertificate);
                objDBAccess.AddParameterToCommand("@LoanSupervisorName", DBFactoryParameterType.NVarchar, d.LoanSupervisorName);
                objDBAccess.AddParameterToCommand("@BranchSecretaryName", DBFactoryParameterType.NVarchar, d.BranchSecretaryName);
                objDBAccess.AddParameterToCommand("@SecretaryName", DBFactoryParameterType.NVarchar, d.SecretaryName);
                objDBAccess.AddParameterToCommand("@LoanSubCommitteeMembers", DBFactoryParameterType.NVarchar, d.LoanSubCommitteeMembers);
                objDBAccess.AddParameterToCommand("@Member1", DBFactoryParameterType.NVarchar, d.Member1);
                objDBAccess.AddParameterToCommand("@Member2", DBFactoryParameterType.NVarchar, d.Member2);
                objDBAccess.AddParameterToCommand("@Member3", DBFactoryParameterType.NVarchar, d.Member3);
                objDBAccess.AddParameterToCommand("@Member4", DBFactoryParameterType.NVarchar, d.Member4);
                objDBAccess.AddParameterToCommand("@Member5", DBFactoryParameterType.NVarchar, d.Member5);
                objDBAccess.AddParameterToCommand("@AdministratorName", DBFactoryParameterType.NVarchar, d.AdministratorName);
                objDBAccess.AddParameterToCommand("@LoanDocument", DBFactoryParameterType.NVarchar, d.LoanDocument);
                objDBAccess.AddParameterToCommand("@LoanDocument2", DBFactoryParameterType.NVarchar, d.LoanDocument2);
                objDBAccess.AddParameterToCommand("@LoanDocument3", DBFactoryParameterType.NVarchar, d.LoanDocument3);
                objDBAccess.AddParameterToCommand("@InstallmentYearWiseId", DBFactoryParameterType.Int, d.InstallmentYearWiseId);

                //Address by Bhavesh
                objDBAccess.AddParameterToCommand("@IsIssueofPassbook", DBFactoryParameterType.Bit, d.IsIssueofPassbook);

                // Execute Stored Procedure

                qry = objDBAccess.getSqlCommandText();
                //old
                //result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                // NEW
                string response = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                if (!string.IsNullOrEmpty(response))
                {
                    string[] arr = response.Split('|');

                    if (arr.Length > 1)
                    {
                        result = Convert.ToInt32(arr[0]);

                        model.encGccid = arr[1];
                    }
                    else
                    {
                        result = Convert.ToInt32(response);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.PLDPLoanApplicationDBModel",
                    FunctionName = "Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel model) " + qry
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }



        public int Delete_PldbLoanApplicationSubsidy(int id)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_DeletePLDBSubsidyRecord", DBFactoryCommandType.PROCEDURE_COMMAND);

                cmd.Parameters.AddWithValue("@Id", id);

                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDBAccess.CloseConnection();   // close connection
            }

            return result;
        }

        #region PldbLoanApplicationLoanDisbursementCasesList
        public DataTable PldbLoanApplicationLoanDisbursementCasesList(ref DataPagingModel Details, bool isExport = false)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";

                //District Filter
                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and p.PldbId = " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }
                // Block Filter 
                if (Details.SearchParameter.Any(x => x.Key == "BLOCK_ID"))
                {
                    whereCase += " AND p.BranchId = " + Details.SearchParameter.FirstOrDefault(x => x.Key == "BLOCK_ID").Value;
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " AND ( " +
                 " REPLACE(p.ApplicantName, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.FatherHusbandName, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.MobileNo, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.LoanAccNo, ' ', '') LIKE '%" + Value + "%' OR " +
                 " REPLACE(p.AadharNumber, ' ', '') LIKE '%" + Value + "%' " +
                 " )";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("sp_GetPLDBLoanApplicationLoanDisbursementCasesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, _LoggedInUser.PLDBId);
                    objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _LoggedInUser.PLDBBrancchId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                    //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Int, isExport);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("sp_GetPLDBLoanApplicationLoanDisbursementCasesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, _LoggedInUser.PLDBId);
                objDBAccess.AddParameterToCommand("@PLDB_BranchId", DBFactoryParameterType.Int, _LoggedInUser.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                // objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Int, isExport);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplicationDBModel", FunctionName = "PldbLoanApplicationDetailsList(string RNO, string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion PldbLoanApplicationLoanDisbursementCasesList


        #region GetSubsidySummaryODCasesList
        public DataTable GetSubsidySummaryODCasesList(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string IsLandMortgaged = "";

                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and p.DistrictId = " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Any())
                {
                    whereCase += " and p.PLDB_Id= " + Details.SearchParameter.Where(x => x.Key == "PLDB_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Any())
                {
                    whereCase += " and p.PLDB_BranchId= " + Details.SearchParameter.Where(x => x.Key == "PLDB_Branch_ID").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Any())
                {
                    IsLandMortgaged = Details.SearchParameter.Where(x => x.Key == "IsLandMortgaged").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(p.PLDBName,'',' ') like '%" + Value + "%' or replace(p.PLDBBranch,' ','')  like '%" + Value
                        + "%' or replace(p.NoticeNo,' ','') like '%" + Value + "%' or replace(p.ApplicantName,' ','') like '%" + Value + "%' or replace(p.FatherHusbandName,' ','') like '%" + Value + "%' or replace(p.JanAadharNumber,' ','') like '%" + Value
                        + "%' or replace(p.MobileNo,' ','') like '%" + Value + "%' or replace(p.LoanAccNo,' ','') like '%" + Value + "%' or replace(sector,' ','') like '%" + Value + "%' or replace(Purpose,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSubsidySummaryInterestSubventionODCasesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    if (Details.SearchParameter.Where(x => x.Key == "_103Cases").Any())
                    {
                        objDBAccess.AddParameterToCommand("@Is103Case", DBFactoryParameterType.Int, Details.SearchParameter.Where(x => x.Key == "_103Cases").Select(x => x.Value).FirstOrDefault());
                    }

                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSubsidySummaryInterestSubventionODCasesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.VARCHAR, IsLandMortgaged);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);

                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PLDPLoanApplication", FunctionName = "GetSubsidySummaryList(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #endregion  GetSubsidySummaryODCasesList


        #region UpdateLoanDocuments

        public void UpdateLoanDocuments(int ApplicationId, int DocumentNo, string FileName)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("USP_UpdateLoanDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationId", DBFactoryParameterType.Int, ApplicationId);
                objDBAccess.AddParameterToCommand("@DocumentNo", DBFactoryParameterType.Int, DocumentNo);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.VARCHAR, FileName);
                objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.PLDBLoanApplication",
                        FunctionName = "UpdateLoanDocuments(int ApplicationId, int DocumentNo, string FileName)"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }

        #endregion UpdateLoanDocuments
    }


    public class BankAccountDetails
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public string DateOfOpenAccount { get; set; }
        public string LoanFacility { get; set; }
        public bool IsDelete { get; set; }
        public string AccountNo { get; set; }
        public string Member { get; set; }

    }

    public class VehicleDetails
    {

        public string VehicleId { get; set; }
        public string LoanId { get; set; }
        public string VehicleType { get; set; }
        public string Model { get; set; }
        public string Capacity { get; set; }
        public string AverageFuelConsumption { get; set; }
        public string VehicleVendorName { get; set; }
        public string ChassisPrice { get; set; }
        public string BodyPrice { get; set; }
        public string OtherExpenses { get; set; }
        public string ApplicantOwnContribution { get; set; }
        public string CommutedLoanAmount { get; set; }
        public string AreaVehicleDriven { get; set; }
        public string StatePermitNo { get; set; }
        public string StatePermitValidity { get; set; }
        public string MonthlyIncomeByVehicle { get; set; }
        public string ItemsBroughtByVehicle { get; set; }
        public string IncomeBasis { get; set; }
        public string VehicleRunDaysInMonth { get; set; }
        public string VehicleRunPerDay { get; set; }
        public string ProposedBuildingAndWorkStation { get; set; }
        public string FarePerKM { get; set; }
        public string WorkingCapitalFileFile { get; set; }
        public string ApplicantDealAfterApprovalFile { get; set; }
        public string EstimatedMonthlyEarnings { get; set; }
        public string OtherIncomeByVehicle { get; set; }
        public string MonthlyExpenditureFuel { get; set; }
        public string DriverSalaryAllowance { get; set; }
        public string RoadAndOtherTaxes { get; set; }
        public string RepairMaintenanceService { get; set; }
        public string InsurancePremiums { get; set; }
        public string OilSpareParts { get; set; }
        public string GarageRent { get; set; }
        public string WearAndTear { get; set; }
        public string OtherMiscellaneousExpenses { get; set; }
        public string InterestOnLoan { get; set; }
    }

    public class GuarantorDetails
    {
        public string Guarantor_Id { get; set; }
        public string Loan_Id { get; set; }
        public string AadhaarNumber { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string UpdatedDateTime { get; set; }

    }

    public class AttachmentDetails
    {
        public int Id { get; set; }
        public string WorkingCapitalFileFile { get; set; }
        public string ApplicantDealAfterApprovalFile { get; set; }
        public string BusinessAndRegiCertificateFile { get; set; }
        public string ProposedBuildingAndWorkStation { get; set; }
        public string Quotation { get; set; }

        public bool IsDelete { get; set; }
    }
    public class OtsRepaymentSummary
    {
        public int OTSRePaymentSummaryId { get; set; }
        public int OtsApplicationId { get; set; }
        public string OtsIdEnc { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentDate { get; set; }
        public int PaymentModeId { get; set; }
        public string UTR_RefNo { get; set; }
        public string DepositByPerson { get; set; }
        public string Remark { get; set; }

        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeOther { get; set; }
        public string AccountCloseDate { get; set; }
        public int IsAccountCloseInSinglePayment { get; set; }
        public bool IsFinalPayment { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal PenalInterestAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal OtherAmount { get; set; }
        public string FinalRemark { get; set; }
        public string RecoveryPersonDesignation { get; set; }
        public string RecoveryPersonName { get; set; }
        public int RecoveryPersonType { get; set; } = 0;
        public int CreatedBy { get; set; }
    }

    public class PldbSmsParam
    {
        public string PldbName { get; set; }
        public string BranchName { get; set; }
        public string MobileNo { get; set; }
        public string AccNo { get; set; }
        public string Amount { get; set; }
        public string DistrictName { get; set; }
        public string LoaneeName { get; set; }
        public string ReceiptNo { get; set; }
        public string Date { get; set; }
        public string TemplateKey { get; set; }

    }

    public class NIActDetailsModel
    {
        public string OtsIdEnc { get; set; }
        public bool IsChequesCollectedFromLoanee { get; set; }
        public bool IsChequesPresentedBankForClearing { get; set; }
        public string ChequeNo { get; set; }
        public string Bankdetails { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeAmt { get; set; }
        public bool IsChequeDishonoured { get; set; }
        public bool IsChequeDishonourActionTaken { get; set; }
        public string ActionTakenDate { get; set; }
        public string CaseNo { get; set; }
        public string CourtDetails { get; set; }
        public string LastHearingDate { get; set; }
        public string NextHearingDate { get; set; }
        public string Judgement { get; set; }
        public string JudgementFavourOfBank { get; set; }
        public string JudgementFavourOfLoanee { get; set; }
        public string RecoveryAmt { get; set; }
        public string DishonourActionReason { get; set; }
        public string OTSSettlementProposed { get; set; }
        public bool IsSettlementProposed { get; set; }

    }
    public class CaseNotMortageModel
    {
        public string OtsIdEnc { get; set; }
        public int CaseNotMortgagedStatus { get; set; }
        public string Remark { get; set; }
    }


    public class OtsFinalPaymentSummary
    {
        public int OtsApplicationId { get; set; }
        public string ReceiptNo { get; set; }
        public string OtsIdEnc { get; set; }
        public decimal ReceivedAmount { get; set; }

        public decimal Actual { get; set; }
        public decimal InsurancePremium { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal CurrentActual { get; set; }
        public decimal CurrentInsurancePremium { get; set; }
        public decimal CurrentInterest { get; set; }
        public decimal CurrentPenalInterest { get; set; }
        public decimal CurrentOtherAmount { get; set; }
        public decimal RecoveryCharge { get; set; }
        public decimal ReliefAmount { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
    }


    public class PLDB_OTSSchemeDataList
    {
        public List<PLDB_OTSSchemeData> PLDB_OTSSchemeData { get; set; }
    }


    public class PLDB_OTSSchemeData
    {
        public int PLDB_OTS_SchemeID { get; set; }
        public string PLDBName { get; set; }
        public string PLDBNameHi { get; set; }
        public string PLDBBranch { get; set; }
        public string BranchNameHi { get; set; }
        public string ApplicantName { get; set; }
        public string FatherHusbandName { get; set; } = "";
        public string JanAadharNumber { get; set; } = "";
        public string MobileNo { get; set; } = "";
        public string Gender { get; set; } = "";
        public string VName { get; set; } = "";
        public string GpName { get; set; } = "";
        public string BlockName { get; set; } = "";
        public string TehsilName { get; set; } = "";
        public string VidhanSabha { get; set; } = "";
        public string DistrictName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string NomineeName { get; set; } = "";
        public string NomineeJanAadharNumber { get; set; } = "";
        public string NomineeMobileNo { get; set; } = "";
        public string LoanAccNo { get; set; } = "";
        public int NoOfLoanBen { get; set; }
        public string Sector { get; set; } = "";
        public string Purpose { get; set; } = "";
        public decimal LoanSanctionAmt { get; set; }
        public string DateOfSanction { get; set; }
        public decimal RateOfInterest { get; set; }
        public string DateOfFirstDisbursement { get; set; }
        public decimal TotalDisbursedAmt { get; set; }
        public string IsLandMortgaged { get; set; } = "";
        public string LandMortgagedland { get; set; } = "";
        public string OtherPropertyDetails { get; set; } = "";
        public decimal CurrentDlcValueOfProperty { get; set; }
        public string DateOfOD { get; set; }
        public decimal ODPrincipal { get; set; }  /*32*/
        public decimal ODInsurancePremium { get; set; } /*33*/
        public decimal ODInterest { get; set; }  /*34*/
        public decimal ODPenalInterast { get; set; }  /*35*/
        public decimal ODTotal { get; set; } /*36=(32+33+34+35)*/
        public decimal NODPrincipal { get; set; } /*37*/
        public decimal NODInsurancePremium { get; set; } /*38*/
        public decimal NODInterest { get; set; } /*39*/
        public decimal NODPenalInterest { get; set; } /*40*/
        public decimal NODTotal { get; set; } /*41=(37+38+39+40)*/
        public decimal CDPrincipal { get; set; } /*42*/
        public decimal CDInterest { get; set; } /*43*/
        public decimal CDPenalInterest { get; set; } /*44*/
        public decimal CDTotalInterest { get; set; } /*45=(43+44)*/
        public decimal CDTotal { get; set; } /*46=(42+45)*/
        public decimal PrincipalOutstandingODNOD { get; set; } /*47*/
        public decimal TotalLoanOutstanding { get; set; } /*48*/
        public string LegalActionStageCompletedDate { get; set; }
        public string LegalActionStageCompletedAction { get; set; } = "";
        public string DateOfSettlement { get; set; } /*51*/
        public decimal PrincipalOutstandingAsOnDateSettlement { get; set; } /*52*/
        public decimal TotalInterestAsOnDateSettlement { get; set; } /*53*/
        public decimal TotalLoanOutstandingAsOnDateSettlement { get; set; } /*54=(52+53)*/
        public decimal InterestFromSettlementPre { get; set; } /*55*/
        public decimal InterestFromSettlement { get; set; } /*56*/
        public decimal TotalAmount { get; set; } /*57=(54+55+56)*/
        public string IsNiActLegalActionTaken { get; set; } = "";
        public string CurrentStatusOfNiActCaseNotice { get; set; } = "";
        public int PLDB_Id { get; set; }
        public int PLDB_BranchId { get; set; }
        public int VId { get; set; }
        public int GpId { get; set; }
        public int BlockId { get; set; }
        public int TehsilId { get; set; }
        public int VidhanSabhaId { get; set; }
        public int DistrictId { get; set; }
        public int CategoryId { get; set; }
        public int SectorId { get; set; }
        public int PurposeId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; } = "";
        public string NoticeNo { get; set; } = "";
        public string CertificateGenerated { get; set; } = "";
        public string Remark { get; set; } = "";
        public int Status { get; set; } = 0;
        public decimal AlreadyPaidAmount { get; set; } = 0;
        public int PendingLevelId { get; set; }
        public string MutationNo { get; set; }
        public string MutationDate { get; set; }
        public string MutationDocument { get; set; }
        public int LandMortgagedPendingLevelId { get; set; }
        public string LandMortgagedMutationNo { get; set; }
        public string LandMortgagedMutationDate { get; set; }
        public string LandMortgagedMutationDocument { get; set; }
        public int IsMortgageMandatory { get; set; }
        public int IsShowForNewNotice { get; set; }
        public decimal? ReleasedLandAreaHectare { get; set; }

    }

    public class PLDB_SummaryODSubsidy
    {
        public int PLDB_OTS_SchemeID { get; set; }
        public string PLDBName { get; set; }
        public string PLDBNameHi { get; set; }
        public string PLDBBranch { get; set; }
        public string BranchNameHi { get; set; }
        public string ApplicantName { get; set; }
        public string FatherHusbandName { get; set; } = "";
        public string JanAadharNumber { get; set; } = "";
        public string MobileNo { get; set; } = "";
        public string Gender { get; set; } = "";
        public string VName { get; set; } = "";
        public string GpName { get; set; } = "";
        public string BlockName { get; set; } = "";
        public string TehsilName { get; set; } = "";
        public string VidhanSabha { get; set; } = "";
        public string CityName { get; set; } = "";
        public string WardName { get; set; } = "";
        public string DistrictName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string NomineeName { get; set; } = "";
        public string NomineeJanAadharNumber { get; set; } = "";
        public string NomineeMobileNo { get; set; } = "";
        public string LoanAccNo { get; set; } = "";
        public int NoOfLoanBen { get; set; }
        public string Sector { get; set; } = "";
        public string Purpose { get; set; } = "";
        public decimal LoanSanctionAmt { get; set; }
        public DateTime DateOfSanction { get; set; }
        public decimal RateOfInterest { get; set; }
        public DateTime DateOfFirstDisbursement { get; set; }
        public decimal TotalDisbursedAmt { get; set; }
        public string IsLandMortgaged { get; set; } = "";
        public string LandMortgagedland { get; set; } = "";
        public string OtherPropertyDetails { get; set; } = "";
        public decimal CurrentDlcValueOfProperty { get; set; }
        public DateTime DateOfOD { get; set; }
        public decimal ODPrincipal { get; set; }  /*32*/
        public decimal ODInsurancePremium { get; set; } /*33*/
        public decimal ODInterest { get; set; }  /*34*/
        public decimal ODPenalInterast { get; set; }  /*35*/
        public decimal ODTotal { get; set; } /*36=(32+33+34+35)*/
        public decimal NODPrincipal { get; set; } /*37*/
        public decimal NODInsurancePremium { get; set; } /*38*/
        public decimal NODInterest { get; set; } /*39*/
        public decimal NODPenalInterest { get; set; } /*40*/
        public decimal NODTotal { get; set; } /*41=(37+38+39+40)*/
        public decimal CDPrincipal { get; set; } /*42*/
        public decimal CDInterest { get; set; } /*43*/
        public decimal CDPenalInterest { get; set; } /*44*/
        public decimal CDTotalInterest { get; set; } /*45=(43+44)*/
        public decimal CDTotal { get; set; } /*46=(42+45)*/
        public decimal PrincipalOutstandingODNOD { get; set; } /*47*/
        public decimal TotalLoanOutstanding { get; set; } /*48*/
        public DateTime LegalActionStageCompletedDate { get; set; }
        public string LegalActionStageCompletedAction { get; set; } = "";
        // public string DateOfSettlement { get; set; } /*51*/
        public decimal PrincipalOutstandingAsOnDateSettlement { get; set; } /*52*/
        public decimal TotalInterestAsOnDateSettlement { get; set; } /*53*/
        public decimal TotalLoanOutstandingAsOnDateSettlement { get; set; } /*54=(52+53)*/
        public decimal InterestFromSettlementPre { get; set; } /*55*/
        public decimal InterestFromSettlement { get; set; } /*56*/
        public decimal TotalAmount { get; set; } /*57=(54+55+56)*/
        public string IsNiActLegalActionTaken { get; set; } = "";
        public string CurrentStatusOfNiActCaseNotice { get; set; } = "";
        public int PLDB_Id { get; set; }
        public int PLDB_BranchId { get; set; }
        public int VId { get; set; }
        public int GpId { get; set; }
        public int BlockId { get; set; }
        public int TehsilId { get; set; }
        public int VidhanSabhaId { get; set; }
        public int DistrictId { get; set; }
        public int CategoryId { get; set; }
        public int SectorId { get; set; }
        public int PurposeId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; } = "";
        public string NoticeNo { get; set; } = "";
        public string CertificateGenerated { get; set; } = "";
        public string Remark { get; set; } = "";
        public int Status { get; set; } = 0;
        public decimal AlreadyPaidAmount { get; set; } = 0;
        public int PendingLevelId { get; set; }
        public string MutationNo { get; set; }
        public DateTime MutationDate { get; set; }
        public string MutationDocument { get; set; }
        public int LandMortgagedPendingLevelId { get; set; }
        public string LandMortgagedMutationNo { get; set; }
        public DateTime LandMortgagedMutationDate { get; set; }
        public string LandMortgagedMutationDocument { get; set; }
        public int IsMortgageMandatory { get; set; }
        public bool IsNotice99 { get; set; }
        public DateTime Notice99Date { get; set; }
        public bool IsOrder99 { get; set; }
        public DateTime Order99Date { get; set; }
        public bool IsNotice100 { get; set; }
        public DateTime Order100Date { get; set; }
        public bool IsLandAuction { get; set; }
        public DateTime LandAuctionDate { get; set; }
        public bool IsMutation { get; set; }
        public decimal CurrentDlcValueOfOtherProperty { get; set; }
        public bool IsChequesCollectedFromLoanee { get; set; }
        public bool IsChequesPresentedBankForClearing { get; set; }
        public string ChequeNo { get; set; }
        public string Bankdetails { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal ChequeAmt { get; set; }
        public bool IsChequeDishonoured { get; set; }
        public bool IsRural { get; set; }
        public bool IsDeathCase { get; set; }
        public bool IsChequeDishonourActionTaken { get; set; }
        public DateTime ActionTakenDate { get; set; }
        public string CaseNo { get; set; }
        public string CourtDetails { get; set; }
        public DateTime LastHearingDate { get; set; }
        public DateTime NextHearingDate { get; set; }
        public string Judgement { get; set; }
        public string JudgementFavourOfBank { get; set; }
        public string JudgementFavourOfLoanee { get; set; }
        public string RecoveryAmt { get; set; }
        public string DishonourActionReason { get; set; }
        public string OTSSettlementProposed { get; set; }
        public bool IsSettlementProposed { get; set; }
        public int? CITY_ID { get; set; }
        public int? WARD_ID { get; set; }
        public decimal? Principal { get; set; }
        public decimal? OtherCharges { get; set; }
        public decimal? IntSubvention { get; set; }
        //public decimal? ReccoveryTotalAmount { get; set; }



        // public decimal LoanSanctionAmt { get; set; }


    }
    public class OtsCitizenApplicantDetails
    {
        public string LoanAccNo { get; set; }
        public string RegistrationNo { get; set; }
        public string MobileNo { get; set; }
        public int DistrictId { get; set; }
        public int PldbId { get; set; }
        public int BranchId { get; set; }
    }
    public class OtsCommonFilterModel
    {
        public string ActionType { get; set; }
        public int DistrictId { get; set; }
        public int PldbId { get; set; }
        public int BranchId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


    public class PLDB_LoanApplicationDetailsModel
    {
        public int PLDB_OTS_SchemeID { get; set; }   // Identity column
        public string encGccid { get; set; }
        public int FinancialYearId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantName2 { get; set; }
        public string ApplicantName3 { get; set; }
        public string ApplicantName4 { get; set; }
        public string ApplicantName5 { get; set; }
        public string FatherHusbandName { get; set; }
        public string FatherHusbandName2 { get; set; }
        public string FatherHusbandName3 { get; set; }
        public string FatherHusbandName4 { get; set; }
        public string FatherHusbandName5 { get; set; }
        public string Gender { get; set; }
        public string Gender2 { get; set; }
        public string Gender3 { get; set; }
        public string Gender4 { get; set; }
        public string Gender5 { get; set; }
        public int? LoanneAge { get; set; }
        public int? LoanneAge2 { get; set; }
        public int? LoanneAge3 { get; set; }
        public int? LoanneAge4 { get; set; }
        public int? LoanneAge5 { get; set; }
        public string CategoryName { get; set; }
        public string FarmerType { get; set; }
        public string LoanAccNo { get; set; }
        public string MobileNo { get; set; }
        public string AadharNumber { get; set; }
        public string PANNumber { get; set; }
        public string VName { get; set; }
        public string GpName { get; set; }
        public string TehsilName { get; set; }
        public string DistrictName { get; set; }
        public string LegislativeAssemblyArea { get; set; }
        public string ParliamentaryConstituency { get; set; }
        public DateTime LoanApplicationDate { get; set; }

        public DateTime? ODDate { get; set; }
        public string LoanApplicationNo { get; set; }
        public string LoanPurpose { get; set; }
        public decimal? ProjectCostAmount { get; set; }
        public decimal? AppliedLoanAmount { get; set; }
        public decimal? SubsidyLoanAmount { get; set; }
        public string PhotoVerifierName { get; set; }
        public string GP_Other { get; set; }
        public string Village_Other { get; set; }
        public string PhotoVerifierDesignation { get; set; }
        public bool? IsRegistrationCertificateReceived { get; set; }
        public bool? IsEstimateQuotationAttached { get; set; }
        public bool? AIsJamabandiAttached { get; set; }
        public bool? AIsKhasraGirdawariCopyAttached { get; set; }
        public bool? AIsLandMapAttached { get; set; }
        public bool? AIsOwnershipAffidavitAttached { get; set; }
        public bool? BApplicantregistrationAttact { get; set; }
        public bool? BAIsLandMapAttached { get; set; }
        public bool? IsOtherGramPanchayat { get; set; }
        public bool? IsAvadhipaar { get; set; }
        public bool? IsFinalSubmit { get; set; }
        public bool? IsLoanDetailsAgrimMICRCheckTaken { get; set; }
        public string BBankApprovedValuerName { get; set; }
        public string ProposedMortgageLand { get; set; }
        public decimal? AverageLandRatePerHectare { get; set; }
        public decimal? TotalLandRatePerHectare { get; set; }
        public decimal? LVOReportLoanValue { get; set; }
        public decimal? LVOReportLoanPayValue { get; set; }
        public DateTime LoanAcceptDate { get; set; }
        public decimal? LoanAcceptPay { get; set; }
        public decimal? Interestrate { get; set; }
        public DateTime Accept_NAccpetDate { get; set; }
        public decimal? bankPartyEnrollementNumber { get; set; }
        public DateTime bankPartyEnrollementNumberDate { get; set; }
        public DateTime AuthorizationLetterIssueDate { get; set; }
        public string FarmCompanyAuthorizedLoanName { get; set; }
        public string ChequeNo { get; set; }
        public string Bankdetails { get; set; }
        public string BankBranch { get; set; }
        public bool? IsRural { get; set; }
        public int? BlockId { get; set; }
        public int? GP_ID { get; set; }
        public int? VId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? WardId { get; set; }
        public int? TehsilId { get; set; }
        public int? AssemblyId { get; set; }
        public int? PldbId { get; set; }
        public int? BranchId { get; set; }
        public int? SectorId { get; set; }
        public int? SectorId2 { get; set; }
        public int? SectorId3 { get; set; }
        public int? SectorId4 { get; set; }
        public int? SectorId5 { get; set; }
        public int? SectorId6 { get; set; }
        public int? SectorId7 { get; set; }
        public int? PurposeId { get; set; }
        public int? PurposeId2 { get; set; }
        public int? PurposeId3 { get; set; }
        public int? PurposeId4 { get; set; }
        public int? PurposeId5 { get; set; }
        public int? PurposeId6 { get; set; }
        public int? PurposeId7 { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public decimal? ODPrincipalAmount { get; set; }
        public decimal? InsurancePremium { get; set; }
        public decimal? ODInterestAmount { get; set; }
        public decimal? PenaltyInterestOtherCharges { get; set; }
        public decimal? TotalODAmount { get; set; }
        public PLDB_GuarantorDetailsModel GuarantorDetails { get; set; }
        public PLDB_LoanDisbursementApprovalModel LoanDisbursement { get; set; }

        //public DateTime? ODDate { get; set; }

    }


    public class PLDB_GuarantorDetailsModel
    {
        public int GuarantorID { get; set; }
        public int PLDB_OTS_SchemeID { get; set; }

        // First Guarantor
        public string First_Name { get; set; }
        public string First_MobileNo { get; set; }
        public string First_AadharNo { get; set; }
        public string First_ChequeNumber { get; set; }
        public string First_Bank { get; set; }
        public string First_Branch { get; set; }
        public string First_Bhumi_Area { get; set; }
        public decimal? First_Bhumi_Mulyaa { get; set; }
        public string First_Other_Area { get; set; }
        public decimal? First_Other_Mulyaa { get; set; }
        public string First_OccupationOrServiceType { get; set; }
        public int? First_OccupationOrServiceTypeId { get; set; }
        public bool? IsFirstAgrimMICRCheckTaken { get; set; }

        // Second Guarantor
        public string Second_Name { get; set; }
        public string Second_MobileNo { get; set; }
        public string Second_AadharNo { get; set; }
        public string Second_ChequeNumber { get; set; }
        public string Second_Bank { get; set; }
        public string Second_Branch { get; set; }
        public string Second_Bhumi_Area { get; set; }
        public decimal? Second_Bhumi_Mulyaa { get; set; }
        public string Second_Other_Area { get; set; }
        public decimal? Second_Other_Mulyaa { get; set; }
        public string Second_OccupationOrServiceType { get; set; }
        public int? Second_OccupationOrServiceTypeId { get; set; }
        public bool? IsSecondAgrimMICRCheckTaken { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }


    public class PLDB_LoanDisbursementApprovalModel
    {
        public int DisbursementID { get; set; }
        public int PLDB_OTS_SchemeID { get; set; }
        public bool IsLoanDistribution { get; set; }
        public bool IsApprovalOnNotesheet { get; set; }
        public DateTime FirstInstallmentDate { get; set; }
        public decimal? FirstInstallmentMoney { get; set; }
        public DateTime SecondInstallmentDate { get; set; }
        public decimal? SecondInstallmentMoney { get; set; }
        public DateTime ThirdInstallmentDate { get; set; }
        public decimal? ThirdInstallmentMoney { get; set; }
        public string VendorChequeNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal? VendorAmount { get; set; }
        public string BillNumber { get; set; }
        public string GSTNo { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime CertificateFirstInstallmentDate { get; set; }
        public DateTime CertificateSecondInstallmentDate { get; set; }
        public DateTime CertificateThirdInstallmentDate { get; set; }
        public int? LoanTimeInYear { get; set; }
        public string InstallmentYearWise { get; set; }
        public string InsuranceMedicalCertificate { get; set; }
        public string LoanSupervisorName { get; set; }
        public string BranchSecretaryName { get; set; }
        public string SecretaryName { get; set; }
        public string LoanSubCommitteeMembers { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public string Member3 { get; set; }
        public string Member4 { get; set; }
        public string Member5 { get; set; }
        public string AdministratorName { get; set; }
        public string LoanDocument { get; set; }
        public string LoanDocument2 { get; set; }
        public string LoanDocument3 { get; set; }
        public int? InstallmentYearWiseId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsIssueofPassbook { get; set; }
  
    }
}