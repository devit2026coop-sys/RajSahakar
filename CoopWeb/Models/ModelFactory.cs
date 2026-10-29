using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;

namespace Cooperative.Models
{
    #region Model : User Entity
    /// <summary>
    /// User registration entity
    /// </summary>
    public class UserEntity
    {
        [Key]
        public Int64 UserId { get; set; }


        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "User Title - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(10, ErrorMessage = "User Title - Max 10 characters allowed")]
        public string UserTitle { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Officer Name En - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Officer Name En - Max 100 characters allowed")]
        public string OfficerNameEn { get; set; }

        public int OfficerId { get; set; }


        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Officer Name HI - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Officer Name Hi - Max 100 characters allowed")]
        public string OfficerNameHi { get; set; }

        [Column(TypeName = "char"), Required(AllowEmptyStrings = false, ErrorMessage = "Gender - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(1, ErrorMessage = "Gender - Max 1 character allowed")]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Mobile No. - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "Mobile No. - 15 characters allowed")]
        public string MobileNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Phone No. - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        // [StringLength(15, MinimumLength = 15, ErrorMessage = "Phone No. - 15 characters allowed")]
        public string PhoneNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Email Address - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, ErrorMessage = "Email Address - 50 characters allowed")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address - Not valid")]
        public string EmailId { get; set; }

        [Column(TypeName = "nvarchar")] //Required(AllowEmptyStrings = false, ErrorMessage = "Image Url - Required")
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(250, ErrorMessage = "Image Url - 250 characters allowed")]
        public string ImageUrl { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Password - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, ErrorMessage = "Password - 50 characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public int StateId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Division Name - Required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Division Name - Required")]
        public int DivisionId { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessage = "District Name - Required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "District Name - Required")]
        public int DistrictId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Tehsil Name - Required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tehsil Name - Required")]
        public int TehsilId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "City Name - Required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City Name - Required")]
        public int CityId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Ward No. - Required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ward No. - Required")]
        public int WardId { get; set; }


        public int BlockId { get; set; }

        public int GPId { get; set; }

        public bool IsRuralUrban { get; set; }


        public int VillageId { get; set; }
        public int ResionId { get; set; }
        public string ActId { get; set; }
        public string WorkScopResionIds { get; set; }

        public string Ids { get; set; }

        // By Bhavesh For Citizen
        public string DisplayNameEn { get; set; }
        public int CitizenId { get; set; }
        public int CitizenTitle { get; set; }
        public string DisplayNameHi { get; set; }


        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address En - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address En - 500 characters allowed")]
        public string AddressEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address Hi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address Hi - 500 characters allowed")]
        public string AddressHi { get; set; }

        [Column(TypeName = "nvarchar")] //Required(AllowEmptyStrings = false, ErrorMessage = "PIN - Required")
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(6, ErrorMessage = "PIN - 6 characters allowed")]
        //[DataType(DataType.PostalCode)]
        public string PIN { get; set; }

        public string SSOID { get; set; }


        public int RoleId { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public string RoleShortName_EN { get; set; }
        public string RoleShortName_HI { get; set; }

        public string CreatedIP { get; set; }

        //[Range(1, Int32.MaxValue)]
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }

        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string UpdatedIP { get; set; }
        public DateTime LastLoginDetail { get; set; }
        public DateTime LastLoginAttempt { get; set; }
        public int NoOfAttempts { get; set; }
        public bool IsFirstLogin { get; set; }
        public DateTime LastLoginTemp { get; set; }
        public int IsActive { get; set; }
        public string Status { get; set; }
        public DateTime DeactivatedDate { get; set; }
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Role  - Required")]
        public List<RoleArray> RoleArray { get; set; }
        public string UserType { get; set; }
        public string ServiceId { get; set; }
        public string KIOSKCODE { get; set; }
        public string DepartmentEn { get; set; }
        public string DesignationEn { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int UnitId { get; set; }
        public int LevelId { get; set; }

        public int PanchayatSamitiId { get; set; }
        public string AadharNo { get; set; }
        public string AadharRefId { get; set; } = string.Empty;
        public int UserRoleDeptMappingId { get; set; }
        public string DashBoardURL { get; set; }
        public int OrganizationId { get; set; }
        public int WarehouseId { get; set; }
        public int PlantId { get; set; }
        public int DepotId { get; set; }
        public int PLDBBrancchId { get; set; }
        public int PLDBId { get; set; }
        public string EmitraReturnUrl { get; set; }
        public string DISTRICT_ENG { get; set; }
        public string DISTRICT_MANGAL { get; set; }
        public string DIVISION_ENG { get; set; }
        public string DIVISION_MANGAL { get; set; }
        public string ResionName { get; set; }
        public string IsFromSWC { get; set; }
        //UniquerequestId
        public int GccId { get; set; }
        public int NccfId { get; set; }
        #endregion
    }
    public class RoleArray
    {
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public string LevelName { get; set; }
        public int LevelId { get; set; }
        public int DesignationLevelMappingId { get; set; }
        public int OrganizationLevelsMappingID { get; set; }
        public int OfficeId { get; set; }
        public string DesignationName { get; set; }
        public int DesignationId { get; set; }
        public string PostName { get; set; }
        public int PostId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string DivisionName { get; set; }
        public int DivisionId { get; set; }
        public string District { get; set; }
        public int DistrictId { get; set; }
        public string UrbanRural { get; set; }
        public bool IsUrbanRural { get; set; }
        public string Tehsil { get; set; }
        public int TehsilId { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string Ward { get; set; }
        public int WardId { get; set; }
        public string PanchayatSamiti { get; set; }
        public int PanchayatSamitiId { get; set; }
        public string GramPanchayat { get; set; }
        public int GramPanchayatId { get; set; }
        public string Village { get; set; }
        public int VillageId { get; set; }
        public int IsActive { get; set; }

        public string CreatedIP { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }

        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string UpdatedIP { get; set; }
        public long MappingFromId { get; set; }
        public int USERID { get; set; }
        public int ResionId { get; set; }
        public string ActId { get; set; }
        public string WorkScopResionIds { get; set; }

        public int ProcessingUnitId { get; set; }
        public int DepotId { get; set; }
        public int WarehouseId { get; set; }
        public int PLDBBrancchId { get; set; }
        public int PLDBId { get; set; }


        public int ApexGccMappingId { get; set; }



    }




    #region Model : Error Log Entity
    /// <summary>
    /// Entity for error logging
    /// </summary>
    public class ErrorLogEntity
    {
        public string PageName { get; set; }
        public string FunctionName { get; set; }
    }
    #endregion

    #region Model : Email Entity
    /// <summary>
    /// Entity for E-mail
    /// </summary>
    public class EmailEntity
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
    }
    #endregion

    #region Model : DB Log Entity
    /// <summary>
    /// Entity for db log entry
    /// </summary>
    public class DBLogEntity
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime LogEntryDatetime { get; set; }
    }
    #endregion

    #region Model : User Role Entity
    public class UserRoleEntity
    {
        public long RoleID { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public string RoleShortName_EN { get; set; }
        public string RoleShortName_HI { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public int LevelID { get; set; }
        public string WhereClause { get; set; }
    }
    #endregion

    #region Model : Menu Action Entity
    public class MenuActionEntity
    {
        public long MenuActionId { get; set; }
        public string MenuActionEn { get; set; }
        public string MenuActionHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
    }
    #endregion

    #region Model : Menu Entity
    public class MenuEntity
    {
        public long MenuId { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuNameHi { get; set; }
        public string MenuUrl { get; set; }
        public string ActionIds { get; set; }
        public string ParentMenuNameEn { get; set; }
        public string ParentMenuNameHi { get; set; }
        public int ParentId { get; set; }
        public int Priority { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
    }
    #endregion

    #region Model : FinancialYear Entity
    public class FinancialYearEntity
    {
        public int FIN_YEAR_MASTR_ID { get; set; }
        public string FIN_YEAR_START { get; set; }
        public string FIN_YEAR_END { get; set; }
        public int IS_ACTIVE { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public DateTime MODIFICATION_DATE { get; set; }
        public int IS_DELETED { get; set; }
        public string FIN_YEAR_NAME { get; set; }
        public int VERSION { get; set; }
    }
    #endregion

    #region Model : Widget Entity
    public class WidgetEntity
    {
        public int WidgetId { get; set; }
        public int WidgetTypeId { get; set; }
        public string WidgetTitleNameEn { get; set; }
        public string WidgetTitleNameHi { get; set; }
        public string WidgetSubTitleNameEn { get; set; }
        public string WidgetSubTitleNameHi { get; set; }
        public string WidgetColor { get; set; }
        public string WidgetDataQuery { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIP { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string Version { get; set; }
        public string Remarks { get; set; }
    }
    #endregion

    #region Model : Questionnaire Response For Approval Entity
    public class LogInEntity
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    #endregion

    #region Model : Create Menu as per Logged in User
    public class MenuIterate
    {
        public int Id { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuNameHI { get; set; }
        public bool IsParent { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public int Priority { get; set; }
        public int MenuLevel { get; set; }
        public bool IsChecked { get; set; }
        public int MenuId { get; set; }
        public string MenuActionId { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string ParentMenuNameEn { get; set; }
        public string ParentMenuNameHi { get; set; }
        public int MappingId { get; set; }
        public string MenuActionList { get; set; }
        public string AssignedActionID { get; set; }
    }

    public class MenuLevel1
    {
        public int Id { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuNameHI { get; set; }
        public bool IsParent { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public int Priority { get; set; }
        public int MenuLevel { get; set; }
        public bool IsChecked { get; set; }
        public int MenuId { get; set; }
        public string MenuActionId { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string ParentMenuNameEn { get; set; }
        public string ParentMenuNameHi { get; set; }
        public int MappingId { get; set; }
        public string MenuActionList { get; set; }
        public string AssignedActionID { get; set; }
        public List<MenuLevel2> MenuLevel2 { get; set; }

    }

    public class MenuLevel2
    {
        public int Id { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuNameHI { get; set; }
        public bool IsParent { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public int Priority { get; set; }
        public int MenuLevel { get; set; }
        public bool IsChecked { get; set; }
        public int MenuId { get; set; }
        public string MenuActionId { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string ParentMenuNameEn { get; set; }
        public string ParentMenuNameHi { get; set; }
        public int MappingId { get; set; }
        public string MenuActionList { get; set; }
        public string AssignedActionID { get; set; }
        public List<MenuIterate> MenuLevel3 { get; set; }

    }

    #endregion



    #region Model : Range Collection
    public class RangeCollection
    {
        public int Start { get; set; }
        public int End { get; set; }
    }
    #endregion


    public class SocietyAmendment
    {
        public int SocietyID { get; set; }
        public int RegiYearId { get; set; }
        public string SocietyIDEnc { get; set; }
        public string SocietyDirectory { get; set; }
        public string ResolutionNo { get; set; }
        public string ByeLawsFile { get; set; }
        public string AGM1File { get; set; }
        public string AGM2File { get; set; }
        public string AGM3File { get; set; }
        public string Name { get; set; }
        public string MemberListExcelFile { get; set; }
        public string Remark { get; set; }

        public DateTime PaasedAGMDate { get; set; }
        public List<SocietyWorkScopeEntity> lstSocietyWorkScope { get; set; }

        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public bool IsRural { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int TEHSIL_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public int RoleId { get; set; }
        public int PinCode { get; set; }
        public int ClassA { get; set; }
        public int ClassB { get; set; }
        public int ClassC { get; set; }
        public string BoardType { get; set; }
        public List<AdministrativeData> AdministrativeDataList { get; set; }
    }

    #region Model : Socity Registration Entity
    public class SocietyRegistrationEntity
    {
        public int SocietyID { get; set; }
        public string SocietyIDEnc { get; set; }
        public string RegistrationNo { get; set; }
        public string OldRegistrationNo { get; set; }
        public string SocietyName { get; set; }
        public int SocietyTypeId { get; set; }
        public string FileNo { get; set; }
        public DateTime SanctionDate { get; set; }
        public int BusinessId { get; set; }
        public int LiablityId { get; set; }
        public int MemberLiablity { get; set; }
        public int ClassId { get; set; }
        public string SubClassId { get; set; }
        public int SocietyCategoryId { get; set; }
        public int StatusId { get; set; }
        public int IsActive { get; set; }
        public string Remarks { get; set; }
        public string Certificate { get; set; }
        public string Request_Id { get; set; }
        public decimal Amount { get; set; }
        public int ApplicationFileBy { get; set; }
        public int AssistantId { get; set; }
        public int SeqId { get; set; }
        public int RehAct { get; set; }
        public string BankPassbookFile { get; set; }
        public int OpeningBalance { get; set; }
        public decimal TotalShareAmount { get; set; }
        public string BankOpeningCertificate { get; set; }
        public string BRN { get; set; }
        public int InspectorId { get; set; }
        public int RegistrarId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateStr { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string ExcelUrl { get; set; }
        public string ExcelSheetFile { get; set; }
        public int FilledProfileLevel { get; set; }
        public int SaveStatus { get; set; }
        public int TotalMembersApplyingForSocociety { get; set; }
        public double EntranceFeesCollected { get; set; }
        public double SingleShareValue { get; set; }
        public double PaidUpShareCapital { get; set; }
        public string ByeLawsSocietyFile { get; set; }
        public string SanctionDocument { get; set; }
        public string WorkPlanShowingEconomicViabilityFile { get; set; }
        public string CooperativesListFile { get; set; }
        public DateTime GeneralMeetingDate { get; set; }
        public string GeneralMeetingProceedingFile { get; set; }
        public string BalanceSheetFile { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ActId { get; set; }
        public string ActIdEnc { get; set; }
        public string RegistrationCertificateFile { get; set; }
        public string RulesAndRegulationFile { get; set; }
        public string SSOID { get; set; }
        public string SSOROle { get; set; }
        public string UserType { get; set; }
        public string ActNameHi { get; set; }
        public string ActNameEn { get; set; }
        public long UserId { get; set; }
        public long UserRoleDeptMappingId { get; set; }
        public string Photo { get; set; }
        public string TypeNameEn { get; set; }
        public string TypeNameHi { get; set; }
        public string Status { get; set; }
        public long IsAdministrativeDetailsAvailable { get; set; }
        public int NoOfBranches { get; set; }
        public DateTime ElectionDate { get; set; }
        public string WhereClause { get; set; }
        public int flag { get; set; }
        public List<SocietyWorkScopeEntity> lstSocietyWorkScope { get; set; }
        public List<SocietyISstateYesEntity> lstIsStateYes { get; set; }
        public string KIOSKCODE { get; set; }
        public string OTP { get; set; }
        public bool IsNewRegistration { get; set; }
        //FOR SPORT SOCIETY
        public bool IsStatLevel { get; set; }
        public int AssociationId { get; set; }
        public DateTime AgmDate { get; set; }
        public string RSSCFileName { get; set; }
        public string DLSCFileName { get; set; }
        public string SOCFileName { get; set; }
        public string OldExecutiveCommittee { get; set; }
        public string MemberListExcelFile { get; set; }

        public string RSSCRegNo { get; set; }
        public string DLSCRegNo { get; set; }
        public string SLSARegNo { get; set; }
        public string SOCRegNo { get; set; }
        public DateTime RSSCAffiliatedDate { get; set; }
        public DateTime DLSCAffiliatedDate { get; set; }
        public DateTime SOCAffiliatedDate { get; set; }
        public bool IsAffiliatedByRSSC { get; set; }
        public bool IsAffiliatedBySOC { get; set; }
        public bool IsAffiliatedByDLSC { get; set; }
        public int ServicesId { get; set; }


        public string ProceesStatusTextEn { get; set; }
        public string ProceesStatusTextHi { get; set; }

        public int LevelId { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public int WardId { get; set; }
        public int PanchayatSamitiId { get; set; }
        public int GPId { get; set; }
        public int InspectionID { get; set; }
        public int TotalNumberOfMembersInAGM { get; set; }
        public string ObjectsFileName { get; set; }
        public string AffidavitFile { get; set; }
        public string SLSAFileName { get; set; }
        public bool IsAffiliationBySLSA { get; set; }
        public DateTime SLSAAffiliatedDate { get; set; }
        public string SERICEID { get; set; }
        public string EMitraAppRequestID { get; set; }
        public int UniqueId { get; set; }

        public string AdministrativeId { get; set; }

        public string ElectionMasterId { get; set; }

        public string ElectionStartDate { get; set; }

        public string ElectionEndDate { get; set; }

        public string Name { get; set; }
        public string ContactNo { get; set; }



        public string AADHAARNo { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }

        public string CEOSocietyFile { get; set; }
        public string ChairmanFile { get; set; }

        public string ClassA { get; set; }
        public string ClassB { get; set; }
        public string ClassC { get; set; }
        public string SocietyDirectory { get; set; }
        public string DESIGNATIONDESCENGLISH { get; set; }
        public string DESIGNATIONDESCHINDI { get; set; }
        public int EMitraTransactionId { get; set; }


        //SocietyRegOfficeDetail
        public int SocietyRegOfficeDetailId { get; set; }



        public bool IsRural { get; set; }
        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int TEHSIL_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public int DeleteFlag { get; set; }
        public int RoleId { get; set; }
        public int PinCode { get; set; }
        public string SSO_ID { get; set; }
        public int SocietyIdForAdministrativeDetails { get; set; }
        public bool IsSameCEOAddress { get; set; }

        public string AreaOfOperation { get; set; }

        //copy data
        //public string SocietyIdEnccopy { get; set; }
        public bool IsRuralcopy { get; set; }
        public int SocietyRegisrationID_copy { get; set; }

        public string HouseBuildingNocopy { get; set; }
        public string LaneStreetNamecopy { get; set; }
        public string Localitycopy { get; set; }
        public int STATE_IDcopy { get; set; }
        public int DIVISION_ID_copy { get; set; }
        public int DISTRICT_ID_copy { get; set; }
        public int SocietyIdcopy { get; set; }
        public int CITY_IDcopy { get; set; }
        // public int BLOCK_IDcopy { get; set; }
        public int WARD_IDcopy { get; set; }
        public int GP_IDcopy { get; set; }
        public int VILLAGE_IDcopy { get; set; }
        public int PinCodecopy { get; set; }
        public int BLOCK_ID_copy { get; set; }
        public int CityTypes_copy { get; set; }

        // sport data entity
        public int HouseBuildingNoSports { get; set; }
        public int LaneStreetNameSports { get; set; }
        public int LocalitySports { get; set; }
        public int STATE_IDSports { get; set; }
        public int SocietyIdsport { get; set; }
        public int IsRuralAddSports { get; set; }
        //copy data 
        public int HouseBuildingNoSportcopy { get; set; }
        public int LaneStreetNameSportcopy { get; set; }
        public int LocalitySportcopy { get; set; }
        public int IsRuralAddSportscopy { get; set; }

        //Secretary data
        public string HouseBuildingNoSecretary { get; set; }
        public string LaneStreetNameSecretary { get; set; }
        public string LocalitySecretary { get; set; }

        public int STATE_IDSecretary { get; set; }
        public int SocietyIdSecretary { get; set; }
        public bool IsRuralSecretary { get; set; }
        public int CityTypesSecretary { get; set; }
        public int CITY_IDSecretary { get; set; }
        public int WARD_IDSecretary { get; set; }
        public int DIVISION_IDSecretary { get; set; }
        public int DISTRICT_IDSecretary { get; set; }
        public int BLOCK_IDSecretary { get; set; }
        public int GP_IDSecretary { get; set; }
        public int VILLAGE_IDSecretary { get; set; }
        public int PinCodeSecretary { get; set; }

        //treasurer data 
        public string HouseBuildingNoTreasurer { get; set; }
        public string LaneStreetNameTreasurer { get; set; }
        public string LocalityTreasurer { get; set; }
        public int STATE_IDTreasurer { get; set; }
        public int SocietyIdTreasurer { get; set; }
        public bool IsRuralTreasurer { get; set; }
        public int DIVISION_IDTreasurer { get; set; }
        public int DISTRICT_IDTreasurer { get; set; }
        public int BLOCK_IDTreasurer { get; set; }
        public int GP_IDTreasurer { get; set; }
        public int VILLAGE_IDTreasurer { get; set; }
        public int PinCodeTreasurer { get; set; }
        public int CityTypesTreasurer { get; set; }
        public int CITY_IDTreasurer { get; set; }
        public int WARD_IDTreasurer { get; set; }
        public int AddressFor { get; set; }

        public int ServiceId { get; set; }
        public string AADHARNO { get; set; }
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string AppAuthKey { get; set; }
        public string SocietyReportFile { get; set; }
        public List<AdministrativeData> AdministrativeDataList { get; set; }
        public SocietyRegisteredOfficeDetailsEntity AddressDetails { get; set; }
        public List<SocietyRegisteredOfficeDetailsEntity> SocietyRegisteredOfficeDetailsLst { get; set; }
        public int OrganizationId { get; set; }
        public int IsUploadAble { get; set; }
        public long FileSize { get; set; }
        public int ChangeStatus { get; set; }
        public bool IsSWS { get; set; }
        public bool IsAmendment { get; set; }
        public string AmendmentType { get; set; }
        public string SocietyCode { get; set; }
        public List<AmendmentDocuments> AmendmentDocuments { get; set; }

        public double? SecurityDeposit { get; set; }
        public double? WorkingCapital { get; set; }
        public double? LoanAmt { get; set; }


        public int FunctionalStatusId { get; set; }
        public int FinancialYearId { get; set; }
        //public int SocietyProfilingStatusId { get; set; }
        //public string RejectFileSocietyProfilingPath { get; set; }
        //public DateTime RejectSocietyProfilingDate { get; set; }
        public bool IsValidateBRN { get; set; }

        public string IsMerge { get; set; }

        public int SocietyProfilingStatusId { get; set; }
        public int IsEditSocietyProfiling2001 { get; set; }
    }

    #region
    public class SocietyISstateYesEntity
    {
        public int IsStateLevel { get; set; }

    }

    #endregion
    #endregion

    #region Model :  Society work scope Details
    public class SocietyWorkScopeEntity
    {
        public int WorkScopeLevel_ID { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        // public int TEHSIL_ID { get; set; }
        public int GP_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int WARD_ID { get; set; }
        public int IsRuralUrban { get; set; }
        public string WorkScopeLevel { get; set; }
        public string STATE_Name { get; set; }
        public string DIVISION_Name { get; set; }
        public string DISTRICT_Name { get; set; }
        public string GP_ID_Name { get; set; }
        public string IsStatLevel { get; set; }
        // public string TEHSIL_Name { get; set; }
        public string BLOCK_Name { get; set; }
        public string VILLAGE_Name { get; set; }
        public string WorkScopeLevel_Name { get; set; }
        public string CityTypes_Name { get; set; }
        public string CITY_Name { get; set; }
        public string WARD_Name { get; set; }

        public string SportsAssociationName { get; set; }
        public string DirectorName { get; set; }
        public string MobileNo { get; set; }
        public int AssociationId { get; set; }
        public int UpdatedBy { get; set; }

        public int WorkScopeId { get; set; }
    }
    #endregion

    #region Model : Save Society Bearer Details
    public class SocietyBearerEntity
    {
        public string Name { get; set; }
        public string Aadhaar { get; set; }
        public int GenderId { get; set; }
        public DateTime DOB { get; set; }
        public int OccupationId { get; set; }
        public int MemberTypeId { get; set; }
        public string MAddress { get; set; }
        public string Photo { get; set; }
    }
    #endregion

    #region Model : Socity Authorizec Personnel Details Entity
    public class SocietyAuthorizedPersonEntity
    {

        public int SocietyAuthPersonalDetailId { get; set; }
        public string SocietyId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public int GENDER_ID { get; set; }
        public string TelNoWithSTDCode { get; set; }
        public string FaxNoWithSTDCode { get; set; }
        public string MobileNo { get; set; }
        public string EmaiId { get; set; }
        public bool IsRural { get; set; }
        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public int PinCode { get; set; }
        public string CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public int UpdationDate { get; set; }
        public DateTime CreatedIP { get; set; }
        public int UpdatedIP { get; set; }
    }
    #endregion

    #region Model : Socity Registered Office Details Entity
    public class SocietyRegisteredOfficeDetailsEntity
    {
        public int SocietyRegOfficeDetailId { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public int ActId { get; set; }
        public bool IsRural { get; set; }
        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int TEHSIL_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public int DeleteFlag
        {
            get; set;
        }

        public int PinCode { get; set; }
        // public string AreaOfOperation { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string SSO_ID { get; set; }
        public string UserType { get; set; }
        public int SocietyIdForAdministrativeDetails { get; set; }
        public bool IsSameCEOAddress { get; set; }

        //copy data
        //public string SocietyIdEnccopy { get; set; }
        public bool IsRuralcopy { get; set; }
        public int SocietyRegisrationID_copy { get; set; }

        public string HouseBuildingNocopy { get; set; }
        public string LaneStreetNamecopy { get; set; }
        public string Localitycopy { get; set; }
        public int STATE_IDcopy { get; set; }
        public int DIVISION_ID_copy { get; set; }
        public int DISTRICT_ID_copy { get; set; }
        public int SocietyIdcopy { get; set; }
        public int CITY_IDcopy { get; set; }
        // public int BLOCK_IDcopy { get; set; }
        public int WARD_IDcopy { get; set; }
        public int GP_IDcopy { get; set; }
        public int VILLAGE_IDcopy { get; set; }
        public int PinCodecopy { get; set; }
        public int BLOCK_ID_copy { get; set; }
        public int CityTypes_copy { get; set; }

        // sport data entity
        public int HouseBuildingNoSports { get; set; }
        public int LaneStreetNameSports { get; set; }
        public int LocalitySports { get; set; }
        public int STATE_IDSports { get; set; }
        public int SocietyIdsport { get; set; }
        public int IsRuralAddSports { get; set; }
        //copy data 
        public int HouseBuildingNoSportcopy { get; set; }
        public int LaneStreetNameSportcopy { get; set; }
        public int LocalitySportcopy { get; set; }
        public int IsRuralAddSportscopy { get; set; }

        //Secretary data
        public string HouseBuildingNoSecretary { get; set; }
        public string LaneStreetNameSecretary { get; set; }
        public string LocalitySecretary { get; set; }

        public int STATE_IDSecretary { get; set; }
        public int SocietyIdSecretary { get; set; }
        public bool IsRuralSecretary { get; set; }
        public int CityTypesSecretary { get; set; }
        public int CITY_IDSecretary { get; set; }
        public int WARD_IDSecretary { get; set; }
        public int DIVISION_IDSecretary { get; set; }
        public int DISTRICT_IDSecretary { get; set; }
        public int BLOCK_IDSecretary { get; set; }
        public int GP_IDSecretary { get; set; }
        public int VILLAGE_IDSecretary { get; set; }
        public int PinCodeSecretary { get; set; }

        //treasurer data 
        public string HouseBuildingNoTreasurer { get; set; }
        public string LaneStreetNameTreasurer { get; set; }
        public string LocalityTreasurer { get; set; }
        public int STATE_IDTreasurer { get; set; }
        public int SocietyIdTreasurer { get; set; }
        public bool IsRuralTreasurer { get; set; }
        public int DIVISION_IDTreasurer { get; set; }
        public int DISTRICT_IDTreasurer { get; set; }
        public int BLOCK_IDTreasurer { get; set; }
        public int GP_IDTreasurer { get; set; }
        public int VILLAGE_IDTreasurer { get; set; }
        public int PinCodeTreasurer { get; set; }
        public int CityTypesTreasurer { get; set; }
        public int CITY_IDTreasurer { get; set; }
        public int WARD_IDTreasurer { get; set; }
        public int AddressFor { get; set; }
        public int EMitraTransactionId { get; set; }
        public int ServiceId { get; set; }
        public string KIOSKCODE { get; set; }
        public int SubmitStatus { get; set; }
    }

    #endregion

    #region Model : Save Society Administrative Details
    public class SocietyAdministrativeEntity
    {
        public string AdministrativeId { get; set; }
        public int SocietyId { get; set; }
        public int ActId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ContactNo { get; set; }
        public int UserType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public bool IsActive { get; set; }
        public string AADHAARNo { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string CEOSocietyFile { get; set; }
        public string ChairmanFile { get; set; }
        public string AdministrativeFile1 { get; set; }
        public string AdministrativeFile2 { get; set; }
        public string AdministrativeFile3 { get; set; }
        public string AdministrativeFile4 { get; set; }
        public string AdministrativeFile5 { get; set; }
        public string AdministrativeFile6 { get; set; }
        public string AdministrativeFile7 { get; set; }
        public string AdministrativeFile8 { get; set; }
        public string AdministrativeFile9 { get; set; }
        public string AdministrativeFile10 { get; set; }
        public string ClassA { get; set; }
        public string ClassB { get; set; }
        public string ClassC { get; set; }
        public string SocietyDirectory { get; set; }
        public string DESIGNATIONDESCENGLISH { get; set; }
        public string DESIGNATIONDESCHINDI { get; set; }
        public int EMitraTransactionId { get; set; }
        public string MembershipNo { get; set; }
        public int ShareAllotted { get; set; }
        public decimal ShareCapitalAmount { get; set; }
        public int loaneeType { get; set; }
        public int SubmitStatus { get; set; }
        public string BoardType { get; set; }
    }
    #endregion

    #region Model : Society Audit Report Entity
    public class SocietyAuditReportEntity
    {
        public int SocietyAuditReportId { get; set; }
        public int SocietyAuditorAssigmentsId { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSocietyWorking { get; set; }
        public int FinancialYear { get; set; }
        public string AuditReport { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UserType { get; set; }
        public string CreationDatesStr { get; set; }
        public int ApprovedBy { get; set; }
        public int SocietyName { get; set; }
        public bool IsPermanentSave { get; set; }
        public DateTime AuditStartDate { get; set; }
        public DateTime AuditEndDate { get; set; }
        public string HousingSocietyMember { get; set; }

        public bool IsDeterminedFormat { get; set; }
        public bool IsApprovedByGeneralAssembly { get; set; }
        public bool IsDesiredDocumentAttached { get; set; }
        public bool IsAllAuditReportDocumentsAttached { get; set; }
        public bool IsComplianceReportApprovedByGeneralAssembly { get; set; }
        public int ClassificationId { get; set; }
        public string TotalActivMembers { get; set; }
        public string ACategory { get; set; }
        public string BCategory { get; set; }
        public string CCategory { get; set; }
        public string CeoName { get; set; }
        public bool IsIrregularity { get; set; }
        public int IrregularityType { get; set; }
        public string DescriptionofIrregularity { get; set; }
        public string ResponsibleOfficerName { get; set; }
        public string IrregularityAmount { get; set; }
        public string ActionDescription { get; set; }
        public string IrregularityOfficerName { get; set; }
        public int SectionId { get; set; }
        public bool IsProfit { get; set; }
        public string TotalMembers { get; set; }
        public string AccumulateProfitLoss { get; set; }
        public string Grossprofitor { get; set; }
        public int TotalAmonut { get; set; }
        public string Membersshareamount { get; set; }
        public string StateGovtAmount { get; set; }
        public bool IsActionisrequired { get; set; }
    }
    #endregion


    #region Model : Society memberdata Log
    public class SocietymemberdataEntity
    {

        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }

        public string Adharno { get; set; }
        public string Adharfile { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string Name { get; set; }
        public string FatherHusbandName { get; set; }
        public int MobileNo { get; set; }
        public int Gender { get; set; }

        public string DOB { get; set; }

        public string Category { get; set; }
        public string Post { get; set; }
        public string Photo { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public bool IsElected { get; set; }
        public bool IsAmendment { get; set; }
        public string BRN { get; set; }


    }
    #endregion


    public class AdministrativeData
    {
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public string NameOfSociety { get; set; }
        public string PostinSociety { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ContactNo { get; set; }
        public string AADHAARNo { get; set; }
        public string DOB { get; set; }
        public string AppointedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string UserType { get; set; }
        public int AdministrativeId { get; set; }
        public int OldAdministrativeId { get; set; }
        public bool IsActive { get; set; }
        public string PostId { get; set; }
        public int CategoryId { get; set; }
        public int AdministratorDesignationId { get; set; }
        public string Address { get; set; }
        public string MEMBERTYPE { get; set; }
        public string PostName { get; set; }
        public string Occupation { get; set; }
        public int NominatedBy { get; set; }
        public string BoardType { get; set; }
        public string MembershipNo { get; set; }
        public int ShareAllotted { get; set; }
        public decimal ShareCapitalAmount { get; set; }
        public int loaneeType { get; set; }
        public int SubmitStatus { get; set; }


        public int RequestType { get; set; }
        public int RequestStatus { get; set; }
        public string RemovalFile { get; set; }
        public string RemovalType { get; set; }
        public string SSOID { get; set; }

        public string AadhrCopy { get; set; }

        public int? IsJanAadhar { get; set; } = null;
    }

    #region Model : Auditor Assignment Entity
    public class SocietyAuditorAssigmentEntity
    {
        public int Id { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIDEnc { get; set; }
        public bool IsEmpanelledCA { get; set; }
        public int IsFirm { get; set; }
        public int AuditorId { get; set; }
        public int DistrictId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string UpdatedIP { get; set; }
        public string SSOID { get; set; }
        //public string FirmId { get; set; }
        public string WhereClause { get; set; }
        public int LastFinancialYear { get; set; }
        public decimal Auditorfees { get; set; }
        public int FinancialYear { get; set; }

        public DateTime FeeDate { get; set; }
        public string ChallanNo { get; set; }
        public string InspectorName { get; set; }
        public int ApprovedBy { get; set; }
        public bool IsPermanentSave { get; set; }
        public DateTime AudiAppoOrderDate { get; set; }
        public DateTime ResolutionDate { get; set; }
        public string ResolutionPDF { get; set; }
        public string AuditorAssignFilePath { get; set; }
        public bool HaveAuditReport { get; set; }
        public DateTime AuditStartDate { get; set; }
        public DateTime AuditEndDate { get; set; }

        //bhu

        public bool IsDeterminedFormat { get; set; }
        public bool IsApprovedByGeneralAssembly { get; set; }
        public bool IsDesiredDocumentAttached { get; set; }
        public bool IsAllAuditReportDocumentsAttached { get; set; }
        public bool IsComplianceReportApprovedByGeneralAssembly { get; set; }
        public int ClassificationId { get; set; }
        public string TotalActivMembers { get; set; }
        public string ACategory { get; set; }
        public string BCategory { get; set; }
        public string CCategory { get; set; }
        public string CeoName { get; set; }
        public bool IsIrregularity { get; set; }
        public int IrregularityType { get; set; }
        public string DescriptionofIrregularity { get; set; }
        public string ResponsibleOfficerName { get; set; }
        public string IrregularityAmount { get; set; }
        public string ActionDescription { get; set; }
        public string IrregularityOfficerName { get; set; }
        public int SectionId { get; set; }
        public bool IsProfit { get; set; }
        public string TotalMembers { get; set; }
        public string AccumulateProfitLoss { get; set; }
        public string Grossprofitor { get; set; }
        public int TotalAmonut { get; set; }
        public string Membersshareamount { get; set; }
        public string StateGovtAmount { get; set; }
        public bool IsActionisrequired { get; set; }


    }
    #endregion

    #region Model : File Uploader Entity
    public class FileUploaderEntity
    {
        public string ID { get; set; }
        public string DirectoryName { get; set; }
        public string SubDirectoryName { get; set; }
        public string File { get; set; }
        public string WhereClause { get; set; }
        public string NewDirectory { get; set; }
        public int IsNewDirectory { get; set; }
        public int Flag { get; set; }
        public string WebConfigText { get; set; }
        public string Message { get; set; }
        public bool IsWeb { get; set; }

    }
    #endregion


    #region Model : File Uploader Entity
    public class DirectoryCreaterEntity
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public string SubDirectoryName { get; set; }
        public string File { get; set; }
        public string WhereClause { get; set; }
        public string WebConfigText { get; set; }
        public string VirtualPath { get; set; }
        public string Message { get; set; }

    }
    #endregion

    #region Model : Upload File Detail Society Inspector Assignment Entity
    public class UploadFileDetailSocietyInspectorAssignmentEntity
    {
        public int Id { get; set; }
        public int SocietyInspectorAssignId { get; set; }
        public int UploadTypeId { get; set; }
        public string UploadFileName { get; set; }
        public string UploadExtraFileTitle { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string DocumentType { get; set; }
        public bool IsPermanentSave { get; set; }
    }
    #endregion

    public class InspectiondataEntity
    {

        #region ENTITY        
        public long InspectionID { get; set; }
        public DateTime DateOfStartInspection { get; set; }
        public DateTime DateOfEndInspection { get; set; }
        public string Admin { get; set; }
        public string Financial { get; set; }
        public string TradeBusiness { get; set; }
        public string InspectionObservation { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public bool Selfdeclaration { get; set; }
        public string SocietyIdEnc { get; set; }
        public Int64 Year { get; set; }
        public int Tenure { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }
        public long ActId { get; set; }
        public string ComplinceFile { get; set; }
        public string Compliance { get; set; }
        public string ReplyRemark { get; set; }
        public int InspOfficerSocietyId { get; set; }
        public string InspectionFile { get; set; }
        public string InspectionFileRemark { get; set; }
        #endregion

    }

    public class TableHeaderAns
    {
        #region ENTITY
        public long AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int TableId { get; set; }
        public string Answear { get; set; }
        public int InspectionId { get; set; }
        public int CreatedBy { get; set; }
        public string SSO_ID { get; set; }
        public int UserType { get; set; }
        public int IsDeleted { get; set; }
        public int CurrentUserType { get; set; }


        #endregion

    }
    public class InspectionAns
    {
        #region ENTITY
        public long AnswerId { get; set; }
        public int InspectionId { get; set; }
        public int QuestionId { get; set; }
        public string Answear { get; set; }
        public long CreatedBy { get; set; }
        public string InspectorRemark { get; set; }
        public string SSO_ID { get; set; }
        public string CEORemark { get; set; }
        public bool IsObjection { get; set; }
        public List<TableHeaderAns> lstTableHeaderAns { get; set; }
        public bool Isliquideter { get; set; }
        public string Isliquideterremark { get; set; }
        public int UserType { get; set; }
        public int CurrentUserType { get; set; }



        #endregion
    }


    public class InspectionBasicData
    {
        #region ENTITY
        public int SocietyID { get; set; }
        public string SocietyIDEnc { get; set; }
        public int OfficerId { get; set; }
        public int FYearId { get; set; }


        #endregion
    }


    #region Model : Inspection Ans Save


    public class InspectionAnsSave
    {
        public List<InspectionAns> InspectionAnsList { get; set; }
        public List<TableHeaderAns> TableHeaderAnsList { get; set; }

    }

    #endregion

    #region Election VoterMember Candidate

    public class ElectionSocietyData
    {
        public int NoOfSociety { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string ElectionMasterIdEnc { get; set; }
        public int ElectionMasterId { get; set; }
        public int El_QuestionId { get; set; }
        public List<SocityIds> SocityIdlst { get; set; }

        public List<Election_SocityIds> Election_SocityIds { get; set; }
        public int ElectionSocietyStatus { get; set; }
        public string ElectionStopDocument { get; set; }
        public string ElectionRemark { get; set; }




        public List<Socitydates> Socitydateslst { get; set; }
        public int DIVISION_ID { get; set; }
        public int DistrictId { get; set; }
        public int CITY_ID { get; set; }
        public int CityTypes { get; set; }
        public int TypeId { get; set; }
        public int WardId { get; set; }
        public int BlockId { get; set; }
        public int GP_ID { get; set; }
        public int VillageId { get; set; }
        public long UserId { get; set; }
        public string SSOID { get; set; }
        public string UserType { get; set; }
        public string WhereClause { get; set; }
        public bool IsRural { get; set; }
        public int SocietyTypeId { get; set; }
        public int ElectionFor { get; set; }
        public int ReturningOfficerID { get; set; }
        public int StepInElection { get; set; }
        public int ElectionForId { get; set; }

        public int SocietyCategoryId { get; set; }


    }
    public class SocityIds
    {
        public string SocietyIdEnc { get; set; }
    }

    public class Socitydates
    {
        public int ElectionMasterId { get; set; }
        public int El_QuestionId { get; set; }
        public DateTime Stap1Date { get; set; }
        public DateTime Stap2Date { get; set; }
        public DateTime Stap3Date { get; set; }
        public DateTime Stap4Date { get; set; }
        public DateTime Stap5Date { get; set; }
        public DateTime Stap6Date { get; set; }
        public DateTime Stap7Date { get; set; }
        public DateTime Stap8Date { get; set; }
        public DateTime Stap9Date { get; set; }
        public DateTime Stap10Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string OtherTime { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class Election_SocityIds
    {
        public int SocietyId1 { get; set; }
    }

    public class SocietyVoterMemberEntity
    {
        public string AdministrativeId { get; set; }
        public int SocietyId { get; set; }
        public int ActId { get; set; }
        public string Name { get; set; }
        public string FHName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int UserType { get; set; }
        public int CreatedBy { get; set; }
        public string MemberId { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public bool IsCEO { get; set; }
        public string AADHAARNo { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string MemberPDFForm { get; set; }
        public string CandidatePDFForm { get; set; }
        public string RowNO { get; set; }
        public string CEOSocietyFile { get; set; }
        public string ChairmanFile { get; set; }
        public string AdministrativeFile { get; set; }
        public string SocietyDirectory { get; set; }
        public string DESIGNATIONDESCENGLISH { get; set; }
        public string DESIGNATIONDESCHINDI { get; set; }
        public string ExcelUrl { get; set; }
        public int ElectionMasterId { get; set; }
        public string BODPDFForm { get; set; }
        public bool IsMemberOfBOD { get; set; }
        public bool IsCandidate { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsPublish { get; set; }
        public int StepType { get; set; }
        public string ActivityStatus { get; set; }
    }

    public class VoterMemberData
    {
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public string FHName { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string AADHAARNo { get; set; }
        public string DOB { get; set; }
        public string AppointedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string UserType { get; set; }
        public int AdministrativeId { get; set; }
        public int MemberId { get; set; }
        public bool IsActive { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public int Presentedby { get; set; }
        public string NewMemberName { get; set; }
        public string NewRemark { get; set; }
        public int AdministratorDesignationId { get; set; }
        public string MemberPDFForm { get; set; }
        public string CandidatePDFForm { get; set; }
        public string RowNO { get; set; }
        public bool IsCEO { get; set; }
        public int ElectionMasterId { get; set; }
        public string BODPDFForm { get; set; }
        public bool IsMemberOfBOD { get; set; }
        public bool IsCandidate { get; set; }
        public bool IsDelete { get; set; }
        public string DeleteRemarks { get; set; }
        public string AddRemarks { get; set; }
        public string AddDocument { get; set; }
        public string DeleteDocument { get; set; }

        public int ActionOnCandidate { get; set; }
        public string ActionRemark { get; set; }
        public string ActionDocument { get; set; }
        public string ActionDate { get; set; }
        public bool IsPostSubmited { get; set; }
        public string MembershipNo { get; set; }
        public string CandidateSymbol { get; set; }
        public string OfficerSymbol { get; set; }
        public string Status { get; set; }
        public string ActivityStatus { get; set; }
    }
    public class LetterDataEntity
    {
        public int Id { get; set; }
        public string SocietyIdEnc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Hours { get; set; }
        public string AuditWorkDetails { get; set; }
        public int AuditorAssignId { get; set; }
    }
    public class ElectionOfficerComment
    {
        public int OfficerCommentId { get; set; }
        public int SocietyId { get; set; }
        public int ElectionMasterId { get; set; }
        public string OfficerComment { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
    }

    public class SocietyElectionOfficer
    {
        public int NoOfSociety { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string ElectionMasterIdEnc { get; set; }
        public int SocietyElecOffiId { get; set; }
        public int SocietyId { get; set; }
        public int SocietyIdEnc { get; set; }
        public int ElectionMasterId { get; set; }
        public int OfficerId { get; set; }
        public int CreateBY { get; set; }
        public int UpdateBY { get; set; }
        public string CreateIP { get; set; }
        public string UpdateIP { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int DIVISION_ID { get; set; }
        public int DistrictId { get; set; }
        public int CITY_ID { get; set; }
        public int CityTypes { get; set; }
        public int WardId { get; set; }
        public int BlockId { get; set; }
        public int GP_ID { get; set; }
        public long UserId { get; set; }
        public string SSOID { get; set; }
        public string UserType { get; set; }
        public string WhereClause { get; set; }
        public bool IsRural { get; set; }
        public int StepId { get; set; }
    }



    #endregion


    public class CaseRegistraion
    {
        public int CaseRegID { get; set; }
        public int Court { get; set; }
        public int Act { get; set; }
        public int section { get; set; }
        public int Case { get; set; }
        public int TypeOfCase { get; set; }
        public string CaseDescription { get; set; }
        public DateTime DateOfAffiliation { get; set; }
        public List<LstDistrictmasterData> DistrictmasterData { get; set; }
        public int isruralurban { get; set; }
        public int districtid { get; set; }
        public int citytypeid { get; set; }
        public int cityblockid { get; set; }
        public int wardgpid { get; set; }
        public int villageid { get; set; }
        public string WhereClause { get; set; }
        public string SSOID { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string CaseStatus { get; set; }
        public string AppAuthKey { get; set; }
        public string ExcelUrl { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedIP { get; set; }



    }
    public class LstDistrictmasterData
    {
        public int valuecode { get; set; }
        public string valueeng { get; set; }
    }

    public class JsonCourt
    {
        public List<result> result { get; set; }

    }
    public class message
    {
        public string msg { get; set; }
        public string AppealNo { get; set; }
        public string PortalRegNo { get; set; }
        public string recordid { get; set; }

        public int Staus { get; set; }
        public string AppAuthKey { get; set; }

        public string caseid { get; set; }
        public string WhereClause { get; set; }

    }

    public class result
    {
        public List<row> row { get; set; }
        public List<message> message { get; set; }
    }

    public class row
    {
        //public string valuenamehi { get; set; }
        //public string valueeng { get; set; }
        //public string valuecode { get; set; }
        //public string valueid { get; set; }

        public string case_no { get; set; }
        public string court_name { get; set; }
        public string createdon { get; set; }
        public string status { get; set; }
        public string noticefilepath { get; set; }

    }
    public class CaseDetails
    {
        public List<AppliantScopeEntity> lstAppliantScope { get; set; }
        public List<RespondScopeEntity> lstRespondScope { get; set; }
    }

    #region Model :  Appellant scope Details
    public class AppliantScopeEntity
    {
        public int CaseRegID { get; set; }
        public string AppellantName { get; set; }
        public string FatherHasbandName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string AddharID { get; set; }
        public int GenderID { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
        public string AppellantType { get; set; }
        public string AppellantTypeId { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string DistrictID { get; set; }
        public string IsActive { get; set; }
        public string IsPrimery { get; set; }
        public int IsActiveID { get; set; }
        public int IsPrimeryID { get; set; }
        public int flag { get; set; }
    }
    #endregion

    #region Model :  Respond scope Details
    public class RespondScopeEntity
    {
        public int CaseRegID { get; set; }
        public string AppellantName { get; set; }
        public string FatherHasbandName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string AddharID { get; set; }
        public int GenderID { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
        public string AppellantType { get; set; }
        public string AppellantTypeId { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string DistrictID { get; set; }
        public string IsActive { get; set; }
        public string IsPrimery { get; set; }
        public int IsActiveID { get; set; }
        public int IsPrimeryID { get; set; }
        public int flag { get; set; }
    }
    #endregion

    public class AdvocateDetails
    {
        public List<AdvocateScopeEntity> lstAdvocateScope { get; set; }
    }

    #region Model :  Advocate scope Details
    public class AdvocateScopeEntity
    {
        public int CaseRegID { get; set; }
        public string AdvocateName { get; set; }
        public string AdvocateType { get; set; }
        public int AdvocateTypeID { get; set; }
        public string AdvocateMobileNo { get; set; }
    }
    #endregion

    public class OICDetails
    {
        public int CaseRegID { get; set; }
        public string OICName { get; set; }
        public int OICType { get; set; }
        public string OICMobileNo { get; set; }
    }



    public class XDocument
    {
        public string m_xDoc { get; set; }
    }


    public class EnclouserDetails
    {
        public int CaseRegID { get; set; }
        public int NoofEnclouser { get; set; }
        public List<Enclouseruploadlst> lstEnclouserupload { get; set; }


    }
    public class Enclouseruploadlst
    {
        public string EnclouserName { get; set; }
        public string EnclouserType { get; set; }
        public int EnclouserId { get; set; } // Add by Jack for UploadType Id : Inspector Assignment Id dated 01-June-2019
    }


    public class SocietyAmendmentEntity
    {
        public int AmendmentId { get; set; }

        public string msg { get; set; }
        public string AppealNo { get; set; }
        public string recordid { get; set; }

        public int AGM_BODM_MstID { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public DateTime ByeLawsDate { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingPDF { get; set; }
        public string SocietyDirectory { get; set; }
        public bool Is_AGM_BODM { get; set; }

        public int ByeLawsMstID { get; set; }
        public int RegiYearId { get; set; }
        public string ByeLawsFile { get; set; }
        public string ResolutionNo { get; set; }
        public string ResolutionFile { get; set; }

        public DateTime PaasedAGMDate { get; set; }

        public string Remark { get; set; }
        public DateTime InsertDate { get; set; }
        public long InsertBy { get; set; }
        public string InsertIP { get; set; }
        public string ExcelBalanceSheetFile { get; set; }
        public string AGM1File { get; set; }
        public string AGM2File { get; set; }
        public string AGM3File { get; set; }
        public string ComparisonFile { get; set; }
        public string Name { get; set; }

        //public string ByeLawsFile { get; set; }
        public string flAGMDeliveryProof { get; set; }
        public string flSGMNotice { get; set; }
        public string flSGMAttendanceSheet { get; set; }
        public string flByLowComparativeDocument { get; set; }
        public string flAttendanceSheet { get; set; }
        public string flResoloutionNotice { get; set; }
        public string AmendmentType { get; set; }
        public string flNameChangeNoticePDF { get; set; }

        public int TotalMember { get; set; }
        public int TotalPresantMember { get; set; }
        public int MeetingQuorum { get; set; }
        public int TotalVoteInSupport { get; set; }
        public int TotalVoteInOpposition { get; set; }

        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public bool IsRural { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int TEHSIL_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public string PinCode { get; set; }
        public string BRN { get; set; }
        public string Section { get; set; }
        public bool IsPermanentSave { get; set; }
        public int SocietyTypeId { get; set; }
    }


    public class AmendmentDocuments
    {
        public string SocietyId { get; set; }
        public string AmendmentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }

    }

    public class AGM_BODM_Mst1
    {
        public int AGM_BODM_MstID { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public DateTime ByeLawsDate { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingPDF { get; set; }
        public bool Is_AGM_BODM { get; set; }
        public string Remark { get; set; }
        public DateTime InsertDate { get; set; }
        public long InsertBy { get; set; }
        public string InsertIP { get; set; }
    }

    public class ByeLawsMst1
    {
        public int AmendmentId { get; set; }
        public int ByeLawsMstID { get; set; }
        public int SocietyId { get; set; }
        public string SocietyIdEnc { get; set; }
        public int RegiYearId { get; set; }
        public string ByeLawsFile { get; set; }
        public string ResolutionNo { get; set; }
        public string ResolutionFile { get; set; }
        public DateTime PaasedAGMDate { get; set; }
        public string Remark { get; set; }
        public DateTime InsertDate { get; set; }
        public long InsertBy { get; set; }
        public string InsertIP { get; set; }
        public string AGM1File { get; set; }
        public string AGM2File { get; set; }
        public string AGM3File { get; set; }
        public string Name { get; set; }
        public string ComparisonFile { get; set; }

        //public string Name { get; set; }

        //public string ByeLawsFile { get; set; }
        public string flAGMDeliveryProof { get; set; }
        public string flSGMNotice { get; set; }
        public string flSGMAttendanceSheet { get; set; }
        public string flByLowComparativeDocument { get; set; }
        public string flAttendanceSheet { get; set; }
        public string flResoloutionNotice { get; set; }
        public string AmendmentType { get; set; }
        public string flNameChangeNoticePDF { get; set; }
        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public bool IsRural { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int TEHSIL_ID { get; set; }
        public int CityTypes { get; set; }
        public int CITY_ID { get; set; }
        public int BLOCK_ID { get; set; }
        public int WARD_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public string PinCode { get; set; }
        public string BRN { get; set; }

        public int TotalMember { get; set; }
        public int TotalPresantMember { get; set; }
        public int MeetingQuorum { get; set; }
        public int TotalVoteInSupport { get; set; }
        public int TotalVoteInOpposition { get; set; }
        public string Section { get; set; }

        public bool IsPermanentSave { get; set; }

    }
    #region Model : SocietyMemberRegistration Entity
    public class OrganizationUserRegistrationEntity
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public string AADHARNo { get; set; }
        public string Business { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int AnimalCount { get; set; }
        public int BuyCount { get; set; }
        public int DepositedAmount { get; set; }
        public bool IsDisability { get; set; }

        // GSS Specific Members
        public int OrganizationtypeId { get; set; }
        public string FatherHusbandName { get; set; }
        public string MembershipNo { get; set; }
        public DateTime MembershipDate { get; set; }
        public string NomineeName { get; set; }
        public int NomineeRelationId { get; set; }
        public int VillageId { get; set; }
        public int BLOCK_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int GP_ID { get; set; }
        public int EconomicSectionId { get; set; }
        public int CategoryId { get; set; }
        public string SRDHNo { get; set; }
        public string PANNo { get; set; }
        public string SahkariSavingAcNo { get; set; }
        public string SahkariSavingAcIFSC { get; set; }
        public int MembershipFee { get; set; }
        public int ContributionAmt { get; set; }
        public bool IsLandOwnership { get; set; }
        public bool IsOtherBankLonee { get; set; }
        public string PatwarMandalName { get; set; }

        // For PLDB Member Registration
        public string CurrentAddress { get; set; }
        public int EducationalQualificationId { get; set; }
        public DateTime DOB { get; set; }
        public string PhotoPath { get; set; }
        public int RequestId { get; set; }
        public string RequestIdEnc { get; set; }
        public string OldMembershipNo { get; set; }

        public int StatusId { get; set; }

        //add bhupndra

        public int PLDB_ID { get; set; }
        public int BRANCH_ID { get; set; }
        public string HouseBuildingNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int pincode { get; set; }
        public int WARD_ID { get; set; }
        public int CITY_ID { get; set; }




        public List<GSSLandDetail> GSSLandDetails { get; set; }
        public List<SownCropDetail> SownCropDetails { get; set; }
        public List<OtherBankLoanDetail> OtherBankLoanDetails { get; set; }



        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string KIOSKID { get; set; }
        public string SSOID { get; set; }
        public string WhereClause { get; set; }
        public string UserType { get; set; }

    }
    #endregion

    #region Model : Department Entity
    public class DepartmentEntity
    {
        public int ID { get; set; }
        public string DEPTNAMEENGLISH { get; set; }
        public string DEPTNAMEHINDI { get; set; }
        public int CreatedBy { get; set; }
    }
    #endregion


    #region Model : UserAverage Entity
    public class UserAverageEntity
    {
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public int NoOFFarmer { get; set; }
        public decimal Average { get; set; }
        public string CreatedIP { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedIP { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
    #endregion

    #region Model : ConfedDashBoardData
    public class ORDERSData
    {
        public string DISTRICT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public int NumberOfOrder { get; set; }
        public string SELLERNAME { get; set; }
        public string CATEGORY { get; set; }


    }

    public class ORDERSList
    {
        public List<ORDERSData> ORDERS { get; set; }
        public string RESPONSECODE { get; set; }
        public string MESSAGE { get; set; }
    }
    #endregion

    #region Model : ConfedDashBoardData



    public class LiquidationFormDataEntity
    {
        public int LiqudationDataId { get; set; }
        public string DistrictId { get; set; }
        public string Month { get; set; }
        public string DetailsOfAcceptatnce { get; set; }
        public int AcceptedAmount { get; set; }
        public int PrincipalAmountOfRecovery { get; set; }
        public int InterestAmountOfRecovery { get; set; }
        public int RecoveryBalance { get; set; }
        public int FdAmount { get; set; }
        public int FundSavingAccount { get; set; }

        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedIP { get; set; }

        public DateTime UpdatedDate { get; set; }


    }
    public class BhandarFormDataEntity
    {
        public long BhandarDataId { get; set; }
        public int BhandarId { get; set; }
        public int FYearId { get; set; }
        public int MonthId { get; set; }
        public decimal ControlledBusinessTarget { get; set; }
        public decimal UnControlledBusinessTarget { get; set; }
        public decimal ControlledBusinessfulfillment { get; set; }
        public decimal UnControlledBusinessFulfillment { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedIP { get; set; }


    }


    #endregion

    public class StLoanEntity
    {
        public string DISTRICT { get; set; }
        public string TotalRegistrations { get; set; }
        public string ApprovedByPacs { get; set; }
        public string ApprovedByBranch { get; set; }
        public string DMRCreated_LoanDisbursed { get; set; }
        public string MCLAMOUNT { get; set; }
        public string SANCTIONAMOUNT { get; set; }

    }
    public class StLoanlstEntity
    {
        public List<StLoanEntity> Table { get; set; }

    }

    public class MSP
    {
        public string PurchaseCenter_Kendra { get; set; }
        public string PurchaseCenter_ID { get; set; }
        public string KVSSSamiti_Name { get; set; }
        public string KVSSSamiti_Name_EN { get; set; }
        public string District_Name_EN { get; set; }
        public string PurchaseCenterName_En { get; set; }
        public string PurchaseCenterName_Hi { get; set; }
        public string COMMODITY { get; set; }
        public string Districtid { get; set; }
        public string ID { get; set; }


    }

    #region Model : KVSSData
    public class KVSSDataEntity
    {
        public int KVSSDataId { get; set; }
        public int KVSSId { get; set; }
        public int FYearId { get; set; }
        public decimal AgriProduction { get; set; }
        public decimal AgriAadan { get; set; }
        public decimal MaterialControl { get; set; }
        public decimal MaterialUncontrol { get; set; }
        public decimal MaterialMedical { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string InsertIP { get; set; }
        public string UpdateIP { get; set; }
        public Boolean IsActive { get; set; }
        public string FIN_YEAR_NAME { get; set; }
        public string KvssNameEn { get; set; }
        public string KvssNameHi { get; set; }
        public long InsertBy { get; set; }
        public long UpdateBy { get; set; }
    }


    #endregion

    #region Model : RajfedServicesData
    public class RajfedServicesData
    {
        public string Crop { get; set; }
        public decimal Rate { get; set; }
        public int Centers { get; set; }
        public decimal PurchaseTarget { get; set; }
        public decimal RegTarget { get; set; }
        public int Registration { get; set; }
        public int DateAlloted { get; set; }
        public int RemaningDateAllot { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int PurchaseFarmerCount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RemaningPaymentRs { get; set; }
        public int PaymentFarmerCount { get; set; }
        public int RemaningPaymentFarmers { get; set; }
        public int Type { get; set; }
        public DateTime dt { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }

    }


    #endregion

    public class AuditorRequestEntity
    {

        public long AuditorId { get; set; }
        public int IsFirm { get; set; }
        public int FirmType { get; set; }
        public string FirmName { get; set; }
        public string FirmAddress { get; set; }
        public int DistrictId { get; set; }
        public int StateId { get; set; }
        public string FirmPinCode { get; set; }
        public int NoOfPartners { get; set; }
        public string BranchAddress { get; set; }
        public string BranchPinCode { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string ICFAIRegistrationNo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RBIPanelUniqueCode { get; set; }
        public string EnrollmentNo { get; set; }
        public string SSO_ID { get; set; }
        public string OtpVal { get; set; }
        public string OtpValEnc { get; set; }
    }

    public class StLoanFormDataEntity
    {
        public long ApexLoanDisbursementFYearId { get; set; }
        public long ApexLoanDisbursementTranID { get; set; }
        public long CCBId { get; set; }
        public int TotalTarget { get; set; }
        public int FIN_YEAR_MASTR_ID { get; set; }
        public int Total_Application_Received { get; set; }
        public int Approved_By_Pacs { get; set; }
        public int Approved_By_Branch { get; set; }
        public decimal DMR_Credited_And_Loan_Disbursement { get; set; }
        public decimal TillDate { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public List<StLoanFormDataTranEntity> StLoanFormDataTranlst { get; set; }
        public string ExcelUrl { get; set; }
    }

    public class StLoanFormDataTranEntity
    {

        public long ApexLoanDisbursementTranID { get; set; }
        public long CCBId { get; set; }
        public int Total_Application_Received { get; set; }
        public int Approved_By_Pacs { get; set; }
        public int Approved_By_Branch { get; set; }
        public decimal DMR_Credited_And_Loan_Disbursement { get; set; }
        public decimal TillDate { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedIP { get; set; }
    }
    public class FertiMngReport
    {
        public int FertiMNGReportId { get; set; }
        public int FinYearId { get; set; }
        public int MonthId { get; set; }
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public int KVSSId { get; set; }
        public int GPId { get; set; }
        public int GSSId { get; set; }
        public int FertiId { get; set; }
        public int TotReceipt { get; set; }
        public int TotSale { get; set; }
        public int TotStockEnd { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
    }


    public class BRNEntity
    {
        public string result { get; set; }
        public string Status { get; set; }
        public string BRN { get; set; }
        public string Applicant_No { get; set; }
        public string Area { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public string Village { get; set; }
        public string LocalBody { get; set; }
        public string Ward { get; set; }
        public string BO_Name { get; set; }
        public string BO_HouseNo { get; set; }
        public string BO_Lane { get; set; }
        public string BO_Locality { get; set; }
        public string BO_PinCode { get; set; }
        public string BO_TelNo { get; set; }
        public string BO_Email { get; set; }
        public string BO_PanNo { get; set; }
        public string BO_TanNo { get; set; }
        public string HO_Name { get; set; }
        public string HO_HouseNo { get; set; }
        public string HO_Lane { get; set; }
        public string HO_Locality { get; set; }
        public string HO_PinCode { get; set; }
        public string HO_TelNo { get; set; }
        public string HO_EMail { get; set; }
        public string HO_PanNo { get; set; }
        public string HO_TanNo { get; set; }
        public string NIC_Code { get; set; }
        public string Year { get; set; }
        public string Ownership { get; set; }
        public string Total_Person { get; set; }
        public string ActAuthorityRegNo { get; set; }
        public string Applicant_Name { get; set; }
        public string Applicant_EMail { get; set; }
        public string Applicant_Address { get; set; }
    }


    public class PM_KISAN_Entity
    {
        public int PATWARI_APPROVED { get; set; }
        public int PDF_DOWNLOAD { get; set; }
        public int COLLECTER_TOTAL { get; set; }
        public int TEHSILDAR_REJ { get; set; }
        public int PATWAR { get; set; }
        public int TEH_ESIGN_STATUS { get; set; }
        public int PATWARI_REJ { get; set; }
        public int PAT_ESIGN_STATUS { get; set; }
        public int FAMILY_REG { get; set; }
        public int TOTAL_PAT { get; set; }
        public int FINAL_UPLOAD { get; set; }
        public string DISTRICTNAME { get; set; }
        public int COLLECTOR_ESIGN_STATUS { get; set; }
        public int COLLECTER_APPROVED { get; set; }
        public int TOTAL_TEH { get; set; }
        public int DISTRICTCODE { get; set; }
        public int TEHSILDAR_APPROVED { get; set; }
        public int COLLECTER_REJ { get; set; }



    }


    #region Code Written By Jitendra dated 19-July-2019
    public class AuditReportDashboardEntity
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int AuditableSocieties { get; set; }
        public int AuditedSocietiesTillLastMonth { get; set; }
        public int AuditedSocietiesCurrentMonth { get; set; }
        public string ExcelUrl { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string SSOID { get; set; }
    }

    #region Model : GSSLandDetail Entity
    public class GSSLandDetail
    {
        public int ID { get; set; }
        public string VillageName { get; set; }
        public string AccountNo { get; set; }
        public string KhasaraNo { get; set; }
        public int TotalLandInHactor { get; set; }
        public int IrrigationLand { get; set; }
        public int NonIrrigationLand { get; set; }
        public int PartInIrrigationLand { get; set; }
        public int PartInNonIrrigationLand { get; set; }
    }

    public class SownCropDetail
    {
        public int ID { get; set; }
        public string LandInHactor { get; set; }
        public bool IsIrrigated { get; set; }
        public string SownCropName { get; set; }
        public bool IsKharif { get; set; }
    }

    public class OtherBankLoanDetail
    {
        public int ID { get; set; }
        public string InstitutionName { get; set; }
        public int ApprovedLoanAmt { get; set; }
        public int RemainingLoanAmtInTime { get; set; }
        public int RemainingLoanAmtTimeBarred { get; set; }
    }
    #endregion
    #endregion
    public class ElectionResultDataData
    {
        public int SocietyId { get; set; }
        public int MemberId { get; set; }
        public string SocietyIdEnc { get; set; }
        public int ElectionMasterId { get; set; }
        public string TotalVotes { get; set; }
        public bool IsWinner { get; set; }
        public string CreatedBy { get; set; }
        public string ResultDocument { get; set; }
        public string ResultRemark { get; set; }
        public int PostId { get; set; }
        public int Type { get; set; }
        public string ActivityStatus { get; set; }
    }

    public class StatusReport
    {
        public bool Status { get; set; }
        public string StatusDescription { get; set; }
        public string FileName { get; set; }
    }

    public class SocietyProcessStatus
    {
        public int SocietyProceesStatusId { get; set; }
        public string ProceesStatusTextEn { get; set; }
        public string ProceesStatusTextHi { get; set; }
        public int IsActive { get; set; }
        public string ActId { get; set; }
        public int IsEnduserRelated { get; set; }
        public int ActionId { get; set; }
    }


    public class ApplicationNextAction
    {
        public int ApplicationNextActionId { get; set; }
        public string SocietyProceesStatusIdChild { get; set; }
        public string SocietyProceesStatusIdParent { get; set; }
        public int WorkOnActId { get; set; }

        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int LevelID { get; set; }
        public int NextRoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionId { get; set; }
    }

    public class FertilizerReportModel
    {
        public string DistrictNameEn { get; set; }
        public string DistrictNameHi { get; set; }
        public int DistrictId { get; set; }
        public List<decimal> Balance { get; set; }
    }

    public class FertilizerReportDataModel
    {
        public string RecordOnDate { get; set; }
        public List<string> Fertilizerheads { get; set; }
        public List<string> FertilizerheadsHi { get; set; }
        public List<FertilizerReportModel> data { get; set; }
    }

    public class FertilizerRetailerModel
    {
        public string RetailerName { get; set; }
        public string BlockName { get; set; }
        public string MobileNumber { get; set; }
        public List<decimal> Balance { get; set; }
    }

    public class FertilizerRetailerDataModel
    {
        public string RecordOnDate { get; set; }
        public List<string> Fertilizerheads { get; set; }
        public List<string> FertilizerheadsHi { get; set; }
        public List<FertilizerRetailerModel> data { get; set; }
    }

    public class Mobile_LoginEntity
    {
        public bool IsUserExists { get; set; } = false;
        public string SSO_ID { get; set; } = string.Empty;
        public string OfficerNameEn { get; set; } = string.Empty;
        public string OfficerNameHi { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int UserId { get; set; } = 0;
        public int UserType { get; set; } = 0;
        public string UserTypeName { get; set; } = string.Empty;
        public List<string> Roles_En { get; set; } = new List<string>();
        public List<string> Roles_Hi { get; set; } = new List<string>();

        public List<int> RoleId { get; set; } = new List<int>();
        public List<string> ActId { get; set; } = new List<string>();

        public List<int> OfficeId { get; set; } = new List<int>();
        public List<int> DistrictId { get; set; } = new List<int>();


    }



    public class SocietyMappingEntity
    {
        public int SocietyMappingRequestId { get; set; }
        public int ActId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string AADHARNo { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string SocietyId { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public int CategoryId { get; set; }
        public string PostName { get; set; }
        public string Occupation { get; set; }
        public string RequestBySSOId { get; set; }
        public string RegNo { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string AADHARNoEnc { get; set; }
        public string SocietyIDEnc { get; set; }
        public string UserType { get; set; } = string.Empty;
        public int ActionId { get; set; } = 0;
        public string FileName { get; set; }
        public string ReasonMapping { get; set; }
    }

    public class ElectionSymbol
    {
        public int symbol_id { get; set; }
        public string symbol_name_en { get; set; }
        public string symbol_name_hi { get; set; }
        public string symbol_url { get; set; }
        public bool is_active { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }

    }

    public class ProcessingUnitEntity
    {
        public int ProcessingUnitId { get; set; }
        public string ProcessingUnitNameEng { get; set; }
        public string ProcessingUnitNameHi { get; set; }
        public int DistrictId { get; set; }
        public bool IsOperating { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public int ActionId { get; set; }
    }

    public class ProductEntity
    {
        public int ProductId { get; set; }
        public string ProductNameEng { get; set; }
        public string ProductNameHi { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public int ActionId { get; set; }
    }

    public class ProductSubCategoryEntity
    {
        public int ProductSubCategoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductSubCategoryNameEng { get; set; }
        public string ProductSubCategoryNameHi { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public int ActionId { get; set; }
    }

    public class UnitMasterEntity
    {
        public int UnitId { get; set; }
        public string UnitNameEng { get; set; }
        public string UnitNameHi { get; set; }
        public int UnitConvertMultiplier { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public int ActionId { get; set; }
    }

    public class PackingEntity
    {
        public int PackingId { get; set; }
        public int ProductId { get; set; }
        public string PackingNameEng { get; set; }
        public string PackingNameHi { get; set; }
        public int UnitId { get; set; }
        public int MeasurementCount { get; set; }
        public bool IsForAll { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public DateTime ActionDateTime { get; set; }
        public string SystemIP { get; set; }
        public int ActionId { get; set; }
        public UnitMasterEntity UnitDetails { get; set; }
        public ProductEntity ProductDetails { get; set; }
    }

    public class RejectReasonEntity
    {
        public int RejectionId { get; set; }
        public string RejectionReasonEng { get; set; }
        public string RejectionReasonHi { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ActionBy { get; set; }
        public int ActionId { get; set; }
    }

    public class ProductSubCategoryPackingMappingEntity
    {
        public int ProductSubCategoryId { get; set; }
        public string ProductSubCategoryNameEng { get; set; }
        public string ProductSubCategoryNameHi { get; set; }
        public List<ProductSubCategoryQuantityEntity> ProductSubCategoryQuantityEntity { get; set; }
    }

    public class ProductSubCategoryQuantityEntity
    {
        public int ProductSubCategoryId { get; set; }
        public int PackingId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SellingPrice { get; set; }
    }

    public class InitialStockEntity
    {
        public int ProcessingUnitId { get; set; }
        public bool IsPlant { get; set; }
        public int ProductId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public string StockDate { get; set; }
        public ProcessingUnitEntity ProcessingUnitDetails { get; set; }
        public List<ProductSubCategoryEntity> ProductSubCategoryList { get; set; }
        public List<PackingEntity> PackingList { get; set; }
        public List<ProductSubCategoryPackingMappingEntity> ProductSubCategoryPackingMappingList { get; set; }
    }

    public class PurchaseRegisterEntity
    {
        public int PurchaseRegisterId { get; set; }
        public int ProcessingUnitId { get; set; }
        public string PartyName { get; set; }
        public string PartyNameHi { get; set; }
        public string BrokerName { get; set; }
        public string BrokerNameHi { get; set; }
        public int ProductId { get; set; }
        public int ProductSubCategoryId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public int RateUnitId { get; set; }
        public int RateQuantity { get; set; }
        public string PurchasePaymentDate { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string SaleContractNo { get; set; }
        public string SaleContractFile { get; set; }
        public string Conditions { get; set; }
        public string PurchaseRegistryEntryDate { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateIP { get; set; }

        public ProcessingUnitEntity PlantDetails { get; set; }
        public ProductEntity ProductDetails { get; set; }
        public ProductSubCategoryEntity ProductSubCategoryDetails { get; set; }
        public UnitMasterEntity UnitDetails { get; set; }
        public UnitMasterEntity RateUnitDetails { get; set; }
        public List<PurchaseRegisterDetailEntity> StockDetails { get; set; }
    }


    public class PurchaseRegisterDetailEntity
    {
        public int PurchaseRegisterDetailId { get; set; }
        public int PurchaseRegisterId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public string VehicleNo { get; set; }
        public bool? IsQualityCheckOkay { get; set; }
        public bool? IsAccepted { get; set; }
        public string AcceptanceFile { get; set; }
        public int RejectionId { get; set; }
        public string RemarkByPlant { get; set; }
        public int PlantUserId { get; set; }
        public string PlantUserActivityDateTime { get; set; }
        public int PlantUserIP { get; set; }
        public bool? IsApprovedByHQ { get; set; }
        public int? RejectionIdByHQ { get; set; }
        public string RemarkByHQ { get; set; }
        public int? HQUserId { get; set; }
        public string HQUserActivityDateTime { get; set; }
        public string HQUserActivityIP { get; set; }
        public bool? IsDebitNote { get; set; }
        public string DebitNoteFile { get; set; }


        public bool IsTakeAction { get; set; }
        public bool IsTakeActionByHq { get; set; }
        public int PlantId { get; set; }
        public int ProductSubCategoryId { get; set; }

        public UnitMasterEntity UnitDetails { get; set; }
        public UserEntity PlantUserDetails { get; set; }
        public UserEntity HqUserDetails { get; set; }
        public RejectReasonEntity RejectDetailsOfPlant { get; set; }
        public RejectReasonEntity RejectDetailsOfHq { get; set; }
    }

    public class StockDemandByDepotTilamSanghEntity
    {
        public int StockDemandId { get; set; }
        public int ProcessingUnitId { get; set; }
        public int ProductId { get; set; }
        public int ProductSubCategoryId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateIP { get; set; }
    }


    public class StockTransferSaleOrderOnPlantByHqEntity
    {
        public int IdStockTransferSaleOrderOnPlantByHQ { get; set; }
        public int PlantId { get; set; }
        public int DepotId { get; set; }
        public string FirmName { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsStockTransfer { get; set; }
        public bool IsInTransit { get; set; }
        public DateTime TransitDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreateIP { get; set; }
        public int OrderTakenByUserId { get; set; }
        public DateTime OrderTakenDateTimeByDepot { get; set; }
        public string OrderTakenByDepotIP { get; set; }
        public List<StockTransferSaleOrderOnPlantByHQDetailsEntity> OrderDetails { get; set; }

        public ProcessingUnitEntity PlantDetails { get; set; }
        public DepotMasterEntity DepotDetails { get; set; }
        public ProductEntity ProductDetails { get; set; }
        public InitialStockEntity OrderDetailShow { get; set; }


        public List<ProductSubCategoryEntity> ProductSubCategoryList { get; set; }
        public List<PackingEntity> PackingList { get; set; }
        public List<ProductSubCategoryPackingMappingEntity> ProductSubCategoryPackingMappingList { get; set; }
    }

    public class StockTransferSaleOrderOnPlantByHQDetailsEntity
    {
        public int DetailId { get; set; }
        public int IdStockTransferSaleOrderOnPlantByHQ { get; set; }
        public int PackagingId { get; set; }
        public int Quantity { get; set; }
        public int ProductSubCategoryId { get; set; }
    }


    public class LiabilityAssetMasterEntity
    {
        public int LiabilityAssetMasterId { get; set; }
        public string NameEng { get; set; }
        public string NameHi { get; set; }
        public bool IsLiability { get; set; }
        public bool IsFurtherExtended { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public int ActionId { get; set; }
    }

    public class LiabilityAssetListForSocietyEntity
    {
        public List<LiabilityAssetSocietyWiseEntity> AssetList { get; set; }
        public List<LiabilityAssetSocietyWiseEntity> LiabilityList { get; set; }
        public bool IsAccountInCCB { get; set; }
        public bool IsInvestedInCCB { get; set; }
        public int InvestmentAmountInCCB { get; set; }
    }

    public class LiabilityAssetSocietyWiseEntity
    {
        public int AssetLiabilityId { get; set; }
        public string NameEng { get; set; }
        public string NameHi { get; set; }
        public decimal Amount { get; set; }
        public bool IsLiability { get; set; }
        public bool IsFurtherExtended { get; set; }
        public int SocietyId { get; set; }
        public int InvestmentId { get; set; }
        public int FinancialHealthId { get; set; }
        public List<LiabilityAssetChildEntity> AmountDistribution { get; set; }
    }

    public class LiabilityAssetChildEntity
    {
        public int Id { get; set; }
        public string WhereDid { get; set; }
        public decimal Amount { get; set; }
    }

    public class SocietyFinancialHealthEntity
    {
        public int Id { get; set; }
        public string AADHARNo { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string CategoryId { get; set; }
        public string CategoryNameEng { get; set; }
        public string CategoryNameHi { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string SocietyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreateIP { get; set; }
        public List<LiabilityAssetSocietyWiseEntity> AssetList { get; set; }
        public List<LiabilityAssetSocietyWiseEntity> LiabilityList { get; set; }
        public List<InvestmentDetails> InvestmentDetails { get; set; }

        public bool IsAccountInCCB { get; set; }


        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSCNo { get; set; }
        public string BalanceInAccount { get; set; }
        public string FDBalanceInAccount { get; set; }


        public bool IsInvestedInCCB { get; set; }
        public int InvestmentAmountInCCB { get; set; }
    }

    public class InvestmentDetails
    {
        public int InvestmentId { get; set; }
        public string SFinancialHealthId { get; set; }
        public string WhereDid { get; set; }
        public decimal Amount { get; set; }
    }

    public class DocumentListEntity
    {
        public List<string> DocumentTypes { get; set; }
        public List<DocumentDescriptionEntity> DocumentList { get; set; }
    }

    public class DocumentDescriptionEntity
    {
        public string DocumentNameEnglish { get; set; }
        public string DocumentNameHindi { get; set; }
        public List<DocumenTypeDetailEntity> DocumenTypeDetails { get; set; }
    }

    public class DocumenTypeDetailEntity
    {
        public string DocumentType { get; set; }
        public string URL { get; set; }
    }


    public class LWA
    {
        public string TotalActive_Head_Office { get; set; }
        public string Total_Head_Office { get; set; }
        public string TotalActive_Branch_Office { get; set; }
        public string Total_Branch_Office { get; set; }
        public string TotalActive_PACS { get; set; }
        public string Total_PACS { get; set; }
        public string Total_Applications { get; set; }
        public string Total_UID_Authentication { get; set; }
        public string Total_Cert_Issued { get; set; }
        public string Total_Cert_Distributed { get; set; }
        public string Total_Rechecking_Cases { get; set; }
        public string Total_GRA_Cases_Pending { get; set; }
        public string Total_Not_Eligible_Cases { get; set; }
        public string Total_Not_Intrested_Cases { get; set; }
        public string New_Beneficiaries_2019 { get; set; }
        public string NOD_TOTAL_InCrores { get; set; }
        public string OD_TOTAL_InCrores { get; set; }
        public string Avg_Loan_Waiver_Amount { get; set; }
    }




    public class PriceMasterEntity
    {
        public string PriceId { get; set; }
        public int SubProductId { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Date { get; set; }
        public int PackingId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
    }


    public class DepotMasterEntity
    {
        public int DepotId { get; set; }
        public int DistrictId { get; set; }
        public string DepotNameEng { get; set; }
        public string DepotNameHi { get; set; }
        public string AgentNameEng { get; set; }
        public string AgentNameHi { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public int ActionId { get; set; }
    }


    public class DepotSalesRegisterEntity
    {
        public int DepotId { get; set; }
        public int ProductId { get; set; }
        public string SalesDate { get; set; }
        public List<ProductSubCategoryEntity> ProductSubCategoryList { get; set; }
        public List<PackingEntity> PackingList { get; set; }
        public List<ProductSubCategoryPackingMappingEntity> ProductAvailableDetails { get; set; }
        public List<ProductSubCategoryPackingMappingEntity> ProductSalesEntryDetails { get; set; }
    }

    public class GoDownMasterEntity
    {
        public int GoDownId { get; set; }
        public int WarehouseId { get; set; }
        public string GoDownNameEng { get; set; }
        public string GoDownNameHi { get; set; }
        public int GoDownTypeId { get; set; }
        public bool IsRural { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public int GPId { get; set; }
        public int VillageId { get; set; }
        public int CityTypeId { get; set; }
        public int CityId { get; set; }
        public int WardId { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string IP { get; set; }
        public int ActionId { get; set; }
    }

    public class RajfedWarehouseMasterEntity
    {
        public int WarehousId { get; set; }
        public string WarehousNameEn { get; set; }
        public string WarehousNameHi { get; set; }
        public int DistrictId { get; set; }
        public int IsCity { get; set; }
        public int BlockCityId { get; set; }
        public string Address { get; set; }
        public bool IsCentralWarehouse { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceValidUpTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string IP { get; set; }
        public int ActionId { get; set; }
    }

    public class CommodityEntity
    {
        public int CommodityId { get; set; }
        public string CommodityNameEn { get; set; }
        public string CommodityNameHi { get; set; }
        public bool IsKharif { get; set; }
        public int IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string IP { get; set; }
        public int ActionId { get; set; }
    }

    public class CommodityMSPEntity
    {
        public int Msp_Id { get; set; }
        public int CommodityId { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public int FYEAR_ID { get; set; }
        public int IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public string IP { get; set; }
        public int ActionId { get; set; }

        public decimal NafedChargesa { get; set; }
        public decimal HighCharges { get; set; }
        public decimal msp_nccf { get; set; }

    }

    public class Params
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CCSComplaintRegisterModel
    {
        public int ComplaintId { get; set; }
        public string ComplainerName { get; set; }
        public string ComplainerFatherName { get; set; }
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int CCSId { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public decimal AmountDeposited { get; set; }
        public string DepositDate { get; set; }
        public List<byte[]> AttachmentFile { get; set; }
        public string AADHAR { get; set; }
        public string AADHARMasked { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public string UserType { get; set; }
        public string SSOId { get; set; }
        public string MembershipId { get; set; }
        public string Remarks { get; set; }

        public string AccountNoInSociety { get; set; }
        public string AccountIFSCInSociety { get; set; }
        public string AccountHolderName { get; set; }
        public string PassbookCrossCheque { get; set; }
        public string PassbookCrossChequeFileBase64String { get; set; }
        public string BPNo { get; set; }

        public SocietyDetailModel SocietyDetails { get; set; }
        public DistrictModel DistrictDetails { get; set; }
        public List<CCSComplaintRegisterDetailModel> FileDetails { get; set; }

        public string ComplaintIdEnc { get; set; }
        public string PassbookCrossChequeView { get; set; }
        public string ReceiptNo { get; set; }


        public string AttestedAadhaar { get; set; }

        public string AffidavitRs { get; set; }

        public string InvestmentFDR { get; set; }

        public string AttestedPan { get; set; }

        public DistrictModel District { get; set; }
        public CCSModel CCS { get; set; }
        public string CreateDate { get; set; }


        public bool IsPermanentLocked { get; set; }
    }

    public class CCSModel
    {
        public int SocietyId { get; set; }
        public string SocietyNameEng { get; set; }
        public string SocietyNameHi { get; set; }
    }

    public class DistrictModel
    {
        public int DistrictId { get; set; }
        public string DistrictEng { get; set; }
        public string DistrictHi { get; set; }
    }

    public class SocietyDetailModel
    {
        public int SocietyIdId { get; set; }
        public string SocietyIdEnc { get; set; }
        public string SocietyEng { get; set; }
        public string SocietyHi { get; set; }
    }

    public class DocTypeDetailModel
    {
        public int DocTypeId { get; set; }
        public string DocTypeNameEng { get; set; }
        public string DocTypeNameHi { get; set; }
    }

    public class CCSComplaintRegisterDetailModel
    {
        public int CCSComplaintRegisterDetailId { get; set; }
        public int CCSComplaintRegisterId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] File1 { get; set; }
        public string FileBase64String { get; set; }


        public HttpPostedFile File { get; set; }
        public int FileTypeId { get; set; }
        public string AccountNo { get; set; }
        public decimal PaidAmount { get; set; }
        public string PayDate { get; set; }
        public decimal MaturityAmount { get; set; }
        public string MaturityDate { get; set; }
        public string FileTypeEng { get; set; }
        public string FileTypeHi { get; set; }

        public string FilePathView { get; set; }

        public DocTypeDetailModel DocTypeDetails { get; set; }

        public CCSComplaintRegisterDetailModel CCSDoc { get; set; }
    }

    public class CCSStatus
    {
        public string RejectFile { get; set; }
        public string OTP { get; set; }
        public string TXN { get; set; }
        public string SocietyIDEnc { get; set; }
        public string StatusId { get; set; }
        public string liquidatorName { get; set; }
        public string Statusdate { get; set; }
        public string liquidatorPostName { get; set; }
        public string liquidatorContactNo { get; set; }
        public string SocietyRegiNo { get; set; }
        public string liquidatorSSOID { get; set; }
    }

    public class CCSPropertDetailEntity
    {
        private int PropertyDetailId { get; set; }
        public int DistrictId { get; set; }
        public int CCSId { get; set; }
        public int PropertyTypeId { get; set; }
        public int LocationStateId { get; set; }
        public int LocationDistrictId { get; set; }
        public bool IsRural { get; set; }
        public string LocationAddress { get; set; }
        public string LocationLandMark { get; set; }
        public string ImageName1 { get; set; }
        public string ImageName2 { get; set; }
        public string Base64StringImage1 { get; set; }
        public string Base64StringImage2 { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MobileNo { get; set; }
        public string ISWeb { get; set; }
        public int subjectId { get; set; }
        public string EnrantName { get; set; }
    }


    public class CCSLetter
    {
        public string ComplaintId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Place { get; set; }
        public string CEOName { get; set; }
        public string CEOMobile { get; set; }
        public string complainant_letter { get; set; }
        public string ceo_letter { get; set; }
        public string ReceiptNo { get; set; }
        public List<SocietyDetails> SocietyDetails { get; set; }
    }


    public class SocietyDetails
    {
        public string ComplaintId { get; set; }
        public string ComplainerName { get; set; }
        public string ComplainerFatherName { get; set; }
        public string Address { get; set; }
        public string SocietyName { get; set; }
        public string AmountDeposited { get; set; }
        public string MobileNo { get; set; }
        public string AccountNo { get; set; }
        public string complainant_letter { get; set; }
        public string ReceiptNo { get; set; }
    }

    public class SocietyDetailsforAction
    {
        public int complainerId { get; set; }
        public int actionId { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string DateTime { get; set; }
        public string MobileNo { get; set; }
        public string DisposeFile { get; set; }

        public int IsSystemGen { get; set; }
        public string Presentdate { get; set; }
        public string TXN { get; set; }
        public string ComplaintId { get; set; }
        public string ComplainerName { get; set; }
        public string ComplainerFatherName { get; set; }
        public string Address { get; set; }
        public string SocietyName { get; set; }
        public string AmountDeposited { get; set; }
        public string AccountNo { get; set; }
        public string complainant_letter { get; set; }
        public string ReceiptNo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string CEOName { get; set; }
        public string CEOMobile { get; set; }
        public string ceo_letter { get; set; }
        public string District { get; set; }
        public string OfficeAddress { get; set; }
        public string HeaderImage { get; set; }
        public List<SocietyDetails> SocietyDetails { get; set; }
        public List<CCSLetter> CCSLetterDetails { get; set; }
        public string Email { get; set; }
        public string Notice { get; set; }
    }

    #region AuditOrderEntity
    public class AuditOrderEntity
    {
        public int LevelId { get; set; }
        public int PlaceId { get; set; }
        public string OrderNo { get; set; }
        public string FileName { get; set; }
        public int SocietyCount { get; set; }
        public string SSOId { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int InActiveFlag { get; set; }
    }

    #endregion
    #region GrievanceEntity
    public class Grievance
    {
        public int GrievanceId { get; set; }
        public int GCategoryId { get; set; }
        public int ApplicationID { get; set; }
        public int DepartmentID { get; set; }
        public int PastGrievanceI { get; set; }
        public string Description { get; set; }
        public string MobileNo { get; set; }
        public string ComplaintNo { get; set; }
        public string SocietyIdENC { get; set; }
        public string AttachmentFileName { get; set; }
        public string SSO_ID { get; set; }
        public int IsActive { get; set; }
        public int CurrentStatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedIP { get; set; }
        public string OfficerNameEn { get; set; }

    }

    public class GrievanceApproval
    {
        public int GApprovalId { get; set; }
        public int GrievanceId { get; set; }
        public int ApprovalStatus { get; set; }
        public string Description { get; set; }
        public string AttachmentFileName { get; set; }
        public string SSO_ID { get; set; }
        public int IsActive { get; set; }
        public int CurrentStatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedIP { get; set; }
    }
    #endregion GrievanceEntity

    public class postvarfname
    {
        public int rating { get; set; }
        public string Societyid { get; set; }
        public string Comment { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

    }

    #region SWS
    public class SWSEntity
    {

        public string SWSID { get; set; }
        public string ActID { get; set; }
        public string ActivityID { get; set; }
        public string IsNew { get; set; }
        public string IsView { get; set; }
        public string RegNo { get; set; }
        public string Userdetails { get; set; }
        public string Establishmentname { get; set; }
        public string Total_Employees { get; set; }
        public string ProposedInvestment { get; set; }
        public string Operational_Date { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CategoryofEstablishment { get; set; }
        public string NatureOfBusiness { get; set; }
        public string PlotNo { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string RuralUrban { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string District { get; set; }
        public string PIN { get; set; }
        public string BusinessDetail { get; set; }
        public string PrimaryGroup { get; set; }
        public string BRN { get; set; }
        public string PAN { get; set; }
        public string TIN { get; set; }
        public string VAT { get; set; }
        public string STDCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string PostalAddress { get; set; }
        public string Est_PlotNo { get; set; }
        public string Est_Street { get; set; }
        public string Est_Area { get; set; }
        public string Est_RuralUrban { get; set; }
        public string Est_District { get; set; }
        public string Est_Tehsil { get; set; }
        public string Est_Ward { get; set; }
        public string Est_Village { get; set; }
        public string Est_PIN { get; set; }
        public string ActName { get; set; }
        public string SSO_ID { get; set; }
        public string SocietyId { get; set; }
        public string SWCSJsonData { get; set; }
        public int IsNewRequestFromSWCS { get; set; }

    }

    #endregion SWS
    public class PurchaseCenterEntity
    {
        public int PurchaseCenterId { get; set; }
        public int DISTRICT_ID { get; set; }
        public string PurchaseCenterNameEn { get; set; }
        public string PurchaseCenterNameHi { get; set; }
        public bool IsActive { get; set; }
        public int Createdby { get; set; }
        public string Createip { get; set; }
        public int Flag { get; set; }
        public bool IsDelete { get; set; }
    }
    public class CommodityMasterEntity
    {
        public int CommodityId { get; set; }
        public string CommodityNameEn { get; set; }
        public string CommodityNameHi { get; set; }
        public bool IsKharif { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public bool IsDelete { get; set; }
        public int Flag { get; set; }
    }
    public class CommodityMasterPrice
    {
        public int CommodityId { get; set; }
        public int Msp_Id { get; set; }
        public int UnitId { get; set; }
        public decimal PriceFAQ { get; set; }
        public decimal PriceURS { get; set; }
        public int FinancialYear { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public bool IsDelete { get; set; }
        public float NafedChargesa { get; set; }
        public float msp { get; set; }
        public decimal highCharges { get; set; }
        public int Flag { get; set; }

    }
    public class DailyWheatPurchase
    {
        public int DailyPurchaseId { get; set; }
        public int CommodityId { get; set; }
        public int DISTRICT_ID { get; set; }
        public int PurchaseCenter_Id { get; set; }
        public string Valid_From_Date { get; set; }
        public string QuantityNote { get; set; }
        public int PaymentReleased { get; set; }
        public int PendingPayment { get; set; }
        public string QtySent { get; set; }
        public string QtyLying { get; set; }
        public string GunnyBags { get; set; }
        public string GunnyBagsStock { get; set; }
        public int NoofFarmersBenefite { get; set; }
        public int WCMRecdtilldate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
        public List<QualityPricePurchase> QualityPricePurchase { get; set; }
    }
    public class QualityPricePurchase
    {
        public int DailyPurchaseQualityId { get; set; }
        public int DailyPurchaseId { get; set; }
        public int QualityID { get; set; }
        public int Quantity { get; set; }
        public int NoofBags { get; set; }
        public int QualityPrice { get; set; }
        public int TotalQualityPrice { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class InspactorTargets
    {

        public int Inspector_ID { get; set; }
        public string InspacorName { get; set; }
        public int DistictId { get; set; }
        public string Urban_Rural { get; set; }
        public int GramPanchayat_ID { get; set; }
        public string GramPanchayat { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string City_Block { get; set; }
        public int City_Block_ID { get; set; }
        public string MonthId { get; set; }
        public string SurveyDate { get; set; }
        public string Document { get; set; }

        //madhuri
        public bool IsTspArea { get; set; }
        public int AreaType { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public string AllotmentLetter { get; set; }
        public string CompetentAcceptanceLetter { get; set; }

        public int InspactorTargetsId { get; set; }
        public bool IsMahilaMpacs { get; set; }
        public int LoanBankID { get; set; }
        public int LoanBranchId { get; set; }
        public int AssemblyId { get; set; }
        public string ProposedNameMPACS { get; set; }
    }
    public class InspactorStatusDetails
    {
        public int Inspector_ID { get; set; }
        public string Remarks { get; set; }
        public string StatusId { get; set; }
        public string Inspectorfile { get; set; }
        public string InspactorTargetsId { get; set; }
        public string PrecentNameGSS { get; set; }
        public string NewGSSName { get; set; }
        public int NoofMember { get; set; }
        public int NewGSSprecious { get; set; }
        public int NewGSSFinanceAmount { get; set; }
        public string GP_ENG { get; set; }
        public string panchayat { get; set; }
        public string DISTRICT_ENG { get; set; }
        public string DISTRICT_MANGAL { get; set; }


        //madhuri
        public string DateOfSubmission { get; set; }
        public string DateOfApproval { get; set; }
        public string SanctionDate { get; set; }
        public string SanctionLetter { get; set; }
        public bool IsMahilaMpacs { get; set; }
        public string BLOCK_ENG { get; set; }
        public string DR2JrPlanForwardedDate { get; set; }



    }

    public class AuditEmpanelledModel
    {
        public string AuditEmpanelledId { get; set; }
        public string ApplicantNameEn { get; set; }
        public string ApplicantNameHi { get; set; }
        public string RegistrationNo { get; set; }
        public string RBIRegistrationDate { get; set; }
        public string AddressEn { get; set; }
        public string AddressHi { get; set; }
        public string PinCode { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string ApplicantType { get; set; }
        public string RBICode { get; set; }
        public string RegistrationDate { get; set; }
        public string NabardCode { get; set; }
        public string NbardRegistrationDate { get; set; }
        public string IsCompletecourse { get; set; }
        public string CertificateName { get; set; }
        public string ExpieranceAreaRemark { get; set; }
        public string Ammount { get; set; }
        public string AmmountDetails { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public bool IsSubmit { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string remark { get; set; }
        public string FirmPracticeCertificate { get; set; }
        public string FirmMembershipCertificate { get; set; }
        public string FirmRegistrationCertificate { get; set; }
        public string FeeChallan { get; set; }
        public string FirmRegistrationNABARD { get; set; }
        public string FirmTermsConditions { get; set; }
        public string FirmDiplomaCertificate { get; set; }
        public string FeeDepositDate { get; set; }


        public bool IsRural { get; set; }
        public string HouseNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public string STATE_ID { get; set; }
        public string DIVISION_ID { get; set; }
        public string DISTRICT_ID { get; set; }
        public string CITY_ID { get; set; }
        public string BLOCK_ID { get; set; }
        public string WARD_ID { get; set; }
        public string GP_ID { get; set; }
        public string VILLAGE_ID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantFatherName { get; set; }
        public string ApplicantAadharNo { get; set; }
        public string FirmRegistrationRBI { get; set; }
        public string IsFirm { get; set; }
        public string IsPartnerFirm { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }

        public List<AuditformPartnership> AuditformPartnership1 { get; set; }
        public List<AuditfirmAddressdetails> AuditfirmAddressdetails1 { get; set; }
        public List<FirmParemenentTraniningDetails> FirmParemenentTraniningDetails1 { get; set; }

    }
    public class AuditformPartnership
    {

        public int AuditEmpanelledId { get; set; }
        public int AuditPartnershipId { get; set; }
        public string Name { get; set; }
        public string PartnerName { get; set; }
        public int MembershipNumber { get; set; }
        public string MembershipDate { get; set; }
        public int CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public string CreatedIP { get; set; }

        public string PracticeCertificateFileName { get; set; }
        public string MembershipCertificateFileName { get; set; }
        public string AuditCertificateFileName { get; set; }

    }


    public class AuditfirmAddressdetails
    {

        public int AuditEmpanelledId { get; set; }
        public int AuditfirmAddressId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public int CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public int CreatedIP { get; set; }

    }






    public class FirmParemenentTraniningDetails
    {
        public int AuditEmpanelledId { get; set; }
        public int FirmParemenentTraniningId { get; set; }
        public string AssistantName { get; set; }
        public string Education { get; set; }
        public string Experiance { get; set; }
        public int CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public int CreatedIP { get; set; }
    }

    public class GodownEntity
    {
        //Added New
        public string ApplicantName { get; set; }
        public int GodownId { get; set; }
        public string Address { get; set; }
        public int SanctionAmt { get; set; }
        public string SanctionDate { get; set; }
        public string SanctionDoc { get; set; }
        public int GodownCapacity { get; set; }
        public int LandType { get; set; }
        public int LandSize { get; set; }
        public string CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public int UpdationDate { get; set; }
        public DateTime CreatedIP { get; set; }
        public int UpdatedIP { get; set; }
        public int status { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public int ActualCost { get; set; }
        public string HouseNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int CITY_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int WARD_ID { get; set; }
        public int PinCode { get; set; }
        public int ConstituencyId { get; set; }
        public string Photo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string SSOID { get; set; }
        public int KVSSGSSId { get; set; }
        public int BLOCK_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public string SchemeName { get; set; }
        public int CCB { get; set; }
        public int CCBBranchId { get; set; }
        public DateTime CompletionDate { get; set; }

        public string Usegodown { get; set; }
        public string CurrentStatus { get; set; }
        public int IsOldGodown { get; set; }
        public string ComplitionYear { get; set; }

        //new Add 01-03-022
        public List<Chcdata> TblCHCDetails { get; set; }
        public List<Benificalmem> TblDetails { get; set; }
    }
    //new Add 01-03-022
    public class Chcdata
    {
        public string EstablishmentDate { get; set; }
        public int Cost { get; set; }
        public string Instrument { get; set; }
        public string RegistrationNo { get; set; }
        public int Price { get; set; }
        public string Manufacture { get; set; }

    }
    //new Add 01-03-022
    public class Benificalmem
    {
        public int Year { get; set; }
        public int Number { get; set; }
    }

    public class GodownMasterEntity
    {
        public string MasterType { get; set; }
        public string MasterValue { get; set; }
    }

    public class UpdateDistrictEntity
    {
        public int SocietyId { get; set; }
        public string RegistrationNo { get; set; }
        public int DistrictId { get; set; }
        public int SocietyTypeId { get; set; }
        public int SocietyCategoryId { get; set; }
        public int ClassId { get; set; }
        public int SubClassId { get; set; }
    }

    public class UserDetailsEntity1
    {
        public string SSOID { get; set; }
        public string AadhaarId { get; set; }
        public string BhamashahId { get; set; }
        public string BhamashahMemberId { get; set; }
        public string DisplayName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string TelephoneNumber { get; set; }
        public string MailPersonal { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string ST { get; set; }
        public string JpegPhoto { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string MailOfficial { get; set; }
        public string EmployeeNumber { get; set; }
        public string DepartmentId { get; set; }
    }

    public class LoanFormDataEntity
    {

        public int LoanDataId { get; set; }
        public long AprStamt { get; set; }
        public long AprMtamt { get; set; }
        public long Aprltamt { get; set; }
        public long RecStamt { get; set; }
        public long RecMtamt { get; set; }
        public long Recltamt { get; set; }
        public long loastamt { get; set; }
        public long loamtamt { get; set; }
        public long loaltamt { get; set; }

        public int BranchId { get; set; }
        public int DistrictId { get; set; }
        public int YearName { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedIP { get; set; }
        public DateTime UpdatedDate { get; set; }
    }


    //Madhuri




    public class CoopMovRj
    {



        public int RJCoopMovId { get; set; }
        public int DistrictId { get; set; }
        public string Year { get; set; }
        public int CooperativeSociety { get; set; }
        public int Membership { get; set; }
        public double ShareCapital { get; set; }
        public double WorkingCapital { get; set; }

        public double St_Loan_Advancement { get; set; }
        public double St_Loan_Recovered { get; set; }
        public double Mt_Loan_Advancement { get; set; }
        public double Mt_Loan_Recovered { get; set; }
        public double Lt_Loan_Advancement { get; set; }
        public double Lt_Loan_Recovered { get; set; }
        public double Purchase { get; set; }
        public double Sale { get; set; }




        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }



        public string CreatedIP { get; set; }



        public DateTime UpdatedDate { get; set; }




    }

    public class CoopASt
    {

        public int DataId { get; set; }
        public int DistrictId { get; set; }





        public string Year { get; set; }
        public int Membership_Gov { get; set; }
        public int Membership_Soc { get; set; }
        public int Number { get; set; }
        public double ShareCapital_Gov { get; set; }
        public double ShareCapital_Soc { get; set; }
        public double Deposit { get; set; }
        public double Reserve { get; set; }
        public double OwnedFunds { get; set; }
        public double Borrowing { get; set; }
        public double WorkingCapital { get; set; }
        public double LoanAdvances { get; set; }
        public double LoanRecoveries { get; set; }
        public double ProfitDuringTheYear { get; set; }

        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }



        public string CreatedIP { get; set; }



        public DateTime UpdatedDate { get; set; }




    }

    public class MainCooperativeSocieties
    {



        public int DataId { get; set; }
        public int DistrictId { get; set; }
        public long NoOfSocieties_Apex { get; set; }
        public long Membership_Apex { get; set; }
        public double ShareCapital_Apex { get; set; }
        public double Reserve_Apex { get; set; }
        public double Deposits_Apex { get; set; }
        public double Borrowing_Apex { get; set; }
        public double WorkingCapital_Apex { get; set; }

        public long NoOfSocieties_CCBs { get; set; }
        public long Membership_CCBs { get; set; }
        public double ShareCapital_CCBs { get; set; }
        public double Reserve_CCBs { get; set; }
        public double Deposits_CCBs { get; set; }
        public double Borrowing_CCBs { get; set; }
        public double WorkingCapital_CCBs { get; set; }

        public long NoOfSocieties_UB { get; set; }
        public long Membership_UB { get; set; }
        public double ShareCapital_UB { get; set; }
        public double Reserve_UB { get; set; }
        public double Deposits_UB { get; set; }
        public double Borrowing_UB { get; set; }
        public double WorkingCapital_UB { get; set; }

        public long NoOfSocieties_Agri { get; set; }
        public long Membership_Agri { get; set; }
        public double ShareCapital_Agri { get; set; }
        public double Reserve_Agri { get; set; }
        public double Deposits_Agri { get; set; }
        public double Borrowing_Agri { get; set; }
        public double WorkingCapital_Agri { get; set; }

        public long NoOfSocieties_PLDB { get; set; }
        public long Membership_PLDB { get; set; }
        public double ShareCapital_PLDB { get; set; }
        public double Reserve_PLDB { get; set; }
        public double Deposits_PLDB { get; set; }
        public double Borrowing_PLDB { get; set; }
        public double WorkingCapital_PLDB { get; set; }

        public long NoOfSocieties_House { get; set; }
        public long Membership_House { get; set; }
        public double ShareCapital_House { get; set; }
        public double Reserve_House { get; set; }
        public double Deposits_House { get; set; }
        public double Borrowing_House { get; set; }
        public double WorkingCapital_House { get; set; }

        public long NoOfSocieties_indsociety { get; set; }
        public long Membership_indsociety { get; set; }
        public double ShareCapital_indsociety { get; set; }
        public double Reserve_indsociety { get; set; }
        public double Deposits_indsociety { get; set; }
        public double Borrowing_indsociety { get; set; }
        public double WorkingCapital_indsociety { get; set; }

        public long NoOfSocieties_labourcontract { get; set; }
        public long Membership_labourcontract { get; set; }
        public double ShareCapital_labourcontract { get; set; }
        public double Reserve_labourcontract { get; set; }
        public double Deposits_labourcontract { get; set; }
        public double Borrowing_labourcontract { get; set; }
        public double WorkingCapital_labourcontract { get; set; }

        public long NoOfSocieties_Marketing { get; set; }
        public long Membership_Marketing { get; set; }
        public double ShareCapital_Marketing { get; set; }
        public double Reserve_Marketing { get; set; }
        public double Deposits_Marketing { get; set; }
        public double Borrowing_Marketing { get; set; }
        public double WorkingCapital_Marketing { get; set; }

        public long NoOfSocieties_MilkSupply { get; set; }
        public long Membership_MilkSupply { get; set; }
        public double ShareCapital_MilkSupply { get; set; }
        public double Reserve_MilkSupply { get; set; }
        public double Deposits_MilkSupply { get; set; }
        public double Borrowing_MilkSupply { get; set; }
        public double WorkingCapital_MilkSupply { get; set; }

        public long NoOfSocieties_MilkUnion { get; set; }
        public long Membership_MilkUnion { get; set; }
        public double ShareCapital_MilkUnion { get; set; }
        public double Reserve_MilkUnion { get; set; }
        public double Deposits_MilkUnion { get; set; }
        public double Borrowing_MilkUnion { get; set; }
        public double WorkingCapital_MilkUnion { get; set; }


        public long NoOfSocieties_Othersociety { get; set; }
        public long Membership_Othersociety { get; set; }
        public double ShareCapital_Othersociety { get; set; }
        public double Reserve_Othersociety { get; set; }
        public double Deposits_Othersociety { get; set; }
        public double Borrowing_Othersociety { get; set; }
        public double WorkingCapital_Othersociety { get; set; }


        public long NoOfSocieties_PrimaryBhander { get; set; }
        public long MembershipPrimaryBhandery { get; set; }
        public double ShareCapital_PrimaryBhander { get; set; }
        public double Reserve_PrimaryBhander { get; set; }
        public double Deposits_PrimaryBhander { get; set; }
        public double Borrowing_PrimaryBhander { get; set; }
        public double WorkingCapital_PrimaryBhander { get; set; }


        public long NoOfSocieties_WholesaleBhander { get; set; }
        public long MembershipPWholesaleBhander { get; set; }
        public double ShareCapital_WholesaleBhander { get; set; }
        public double Reserve_WholesaleBhander { get; set; }
        public double Deposits_WholesaleBhander { get; set; }
        public double Borrowing_WholesaleBhander { get; set; }
        public double WorkingCapital_WholesaleBhander { get; set; }

        public string Year { get; set; }

        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedIP { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class PrimaryAgricultureCreditSocieties
    {
        public int DataId { get; set; }
        public int DistrictId { get; set; }
        public string Fyear { get; set; }
        public long Membership_SocietyNo { get; set; }
        public long Membership_TotalAm { get; set; }
        public long Membership_ScNO { get; set; }
        public long Membership_ScAmount { get; set; }
        public long Membership_StNo { get; set; }
        public long Membership_StAmount { get; set; }
        public long BorrowingMember_SocietyNo { get; set; }
        public long BorrowingMember_TotalAm { get; set; }
        public long BorrowingMember_ScNO { get; set; }
        public long BorrowingMember_ScAmount { get; set; }
        public long BorrowingMember_StNo { get; set; }
        public long BorrowingMember_StAmount { get; set; }
        public long MemberaIndebted_SocietyNo { get; set; }
        public long MemberaIndebted_TotalAm { get; set; }
        public long MemberaIndebted_ScNO { get; set; }
        public long MemberaIndebted_ScAmount { get; set; }
        public long MemberaIndebted_StNo { get; set; }
        public long MemberaIndebted_StAmount { get; set; }
        public long MemberaDefaulting_SocietyNo { get; set; }
        public long MemberaDefaulting_TotalAm { get; set; }
        public long MemberaDefaulting_ScNO { get; set; }
        public long MemberaDefaulting_ScAmount { get; set; }
        public long MemberaDefaulting_StNo { get; set; }
        public long MemberaDefaulting_StAmount { get; set; }
        public long St_Loan_Advancement_SocietyNo { get; set; }
        public long St_Loan_Advancement_TotalAm { get; set; }
        public long St_Loan_Advancement_ScNO { get; set; }
        public long St_Loan_Advancement_ScAmount { get; set; }
        public long St_Loan_Advancement_StNo { get; set; }
        public long St_Loan_Advancement_StAmount { get; set; }
        public long Mt_Loan_Advancement_SocietyNo { get; set; }
        public long Mt_Loan_Advancement_TotalAm { get; set; }
        public long Mt_Loan_Advancement_ScNO { get; set; }
        public long Mt_Loan_Advancement_ScAmount { get; set; }
        public long Mt_Loan_Advancement_StNo { get; set; }
        public long Mt_Loan_Advancement_StAmount { get; set; }
        public long St_Loan_Recovered_SocietyNo { get; set; }
        public long St_Loan_Recovered_TotalAm { get; set; }
        public long St_Loan_Recovered_ScNO { get; set; }
        public long St_Loan_Recovered_ScAmount { get; set; }
        public long St_Loan_Recovered_StNo { get; set; }
        public long St_Loan_Recovered_StAmount { get; set; }
        public long Mt_Loan_Recovered_SocietyNo { get; set; }
        public long Mt_Loan_Recovered_TotalAm { get; set; }
        public long Mt_Loan_Recovered_ScNO { get; set; }
        public long Mt_Loan_Recovered_ScAmount { get; set; }
        public long Mt_Loan_Recovered_StNo { get; set; }
        public long Mt_Loan_Recovered_StAmount { get; set; }
        public long St_Loan_Outstanding_SocietyNo { get; set; }
        public long St_Loan_Outstanding_TotalAm { get; set; }
        public long St_Loan_Outstanding_ScNO { get; set; }
        public long St_Loan_Outstanding_ScAmount { get; set; }
        public long St_Loan_Outstanding_StNo { get; set; }
        public long St_Loan_Outstanding_StAmount { get; set; }
        public long MT_Loan_Outstanding_SocietyNo { get; set; }
        public long MT_Loan_Outstanding_TotalAm { get; set; }
        public long MT_Loan_Outstanding_ScNO { get; set; }
        public long MT_Loan_Outstanding_ScAmount { get; set; }
        public long MT_Loan_Outstanding_StNo { get; set; }
        public long MT_Loan_Outstanding_StAmount { get; set; }

        public long St_Loan_OverDue_SocietyNo { get; set; }
        public long St_Loan_OverDue_TotalAm { get; set; }

        public long St_Loan_OverDue_ScNO { get; set; }
        public long St_Loan_OverDue_ScAmount { get; set; }

        public long St_Loan_OverDue_StNo { get; set; }
        public long St_Loan_OverDue_StAmount { get; set; }

        public long Mt_Loan_OverDue_SocietyNo { get; set; }
        public long Mt_Loan_OverDue_TotalAm { get; set; }
        public long Mt_Loan_OverDue_ScNO { get; set; }
        public long Mt_Loan_OverDue_ScAmount { get; set; }
        public long Mt_Loan_OverDue_StNo { get; set; }
        public long Mt_Loan_OverDue_StAmount { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedIP { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class RajfedPurchaseCenter
    {

        public int Rajfed_KVSS_Id { get; set; }
        public int Rajfed_PCID { get; set; }
        public int PurchaseCenterId { get; set; }

    }

    public class CCSComplaintDetails
    {
        public int Id { get; set; }

        public string FinYrMonth { get; set; }

        public int? OtherComplaints { get; set; }

        public int? DocumentsReceived { get; set; }

        public int? NoticesIssued { get; set; }

        public int? HearingDocuments { get; set; }

        public int? Pp_DocumentsShared { get; set; }

        public int? CourtFiled { get; set; }

        public int? BudsActFIR { get; set; }

        public int? FIR_Registered { get; set; }

    }

    public class MPRArticleEntity
    {
        public int Id { get; set; }
        public int FinYearId { get; set; }
        public int MonthId { get; set; }
        public int? TsActive { get; set; }
        public int? TsInactive { get; set; }
        public int? TsUnderLiquidation { get; set; }
        public int? PmgmActive { get; set; }
        public int? PmgmInactive { get; set; }
        public int? PmgmUnderLiquidation { get; set; }
        public int? CmgmActive { get; set; }
        public int? CmgmInactive { get; set; }
        public int? CmgmUnderLiquidation { get; set; }
        public int? TmgmCount { get; set; }
        public int? EmgmActive { get; set; }
        public int? EmgmInactive { get; set; }
        public int? EmgmUnderLiquidation { get; set; }
        public int? PyActive { get; set; }
        public int? PyInactive { get; set; }
        public int? PyUnderLiquidation { get; set; }
        public int? InYearPendingCase { get; set; }
        public int? LastMonthCaseReceived { get; set; }
        public int? InMonthCaseReceived { get; set; }
        public int? LastMonthCaseDisposed { get; set; }
        public int? InMonthCaseDisposed { get; set; }
        public int? NoCaseReport { get; set; }
        public int? NoPendingCaseReport { get; set; }
        public int? No1YearPending { get; set; }
        public int? No1to3YearPending { get; set; }
        public int? No3to5YearPending { get; set; }
        public int? No5YearPending { get; set; }
        public decimal InYearPendingCaseRs { get; set; }
        public decimal LastMonthCaseReceivedRs { get; set; }
        public decimal InMonthCaseReceivedRs { get; set; }
        public decimal LastMonthCaseDisposedRs { get; set; }
        public decimal InMonthCaseDisposedRs { get; set; }
    }

    public class CCSSurveySubmitForm
    {
        public int ID { get; set; }
        public string CCSType { get; set; }
        public string District { get; set; }
        public string CCSName { get; set; }
        public string RegiNo { get; set; }
        public DateTime RegiDate { get; set; }
        public string Area_of_Operation { get; set; }
        public string HO_Address { get; set; }
        public string Num_location_branches { get; set; }
        public string Ordinary { get; set; }

        // TakeactionStatus
        public int TakeactionStatus { get; set; }
        public string Remarks { get; set; }
        public string Nominal { get; set; }
        public string BOD_details { get; set; }
        public string BOD_meeting_regularly_or_not { get; set; }
        public int General_meeting { get; set; }
        public DateTime Audit_completed { get; set; }
        public DateTime long_audit_compliance { get; set; }
        public int Status_inquiry { get; set; }
        public int Complaints_Received { get; set; }
        public int Complaints_Disposed { get; set; }
        public string EmployeeDetails { get; set; }
        public string ResolutiontakenBODRevocable { get; set; }

        public string Fund_Reserve { get; set; }
        public int Deposit_SB { get; set; }
        public int Deposit_FD { get; set; }
        public int Deposit_RD { get; set; }
        public string appropriation_policy_applicable_in_committee { get; set; }
        public int Investment_CCB { get; set; }
        public int Investment_OtherBank { get; set; }
        public int Investment_Share_Purchase { get; set; }
        public List<CCSSurveySubmitFormInvestment> ccsRegistrationDatalist { get; set; }

        public int Total { get; set; }
        public string loan_Disbursement_Objective { get; set; }
        public int loan_Disbursement_Amount { get; set; }
        public string Debt_Recovery_Demand { get; set; }
        public int Debt_Recovery_Recovery { get; set; }
        public int Debt_Recovery_RecoveryPer { get; set; }
        public int Term_fee_CaseNumber { get; set; }
        public int Term_fee_Amount { get; set; }
        public int StakeAmount_AuthorizedAmount { get; set; }
        public int StakeAmount_PaidupCapital { get; set; }
        public int StakeAmount_WorkingCapital { get; set; }
        public int Profit_AccumulatedProfit { get; set; }
        public int Profit_YearlyProfit { get; set; }
        public int GeneralMeeting { get; set; }
        public int ProfitDistribution { get; set; }
        public string Number_estgasau_registered { get; set; }
        public int Status_inquiry1 { get; set; }
        public string Physical_Verification_10_Tested { get; set; }
        public int immovablepropertyheld_Society_Type { get; set; }
        public int immovablepropertyheld_Society_Bookvalue { get; set; }
        public int immovablepropertyheld_Society_PresentValue { get; set; }
        public string CommentCreditPolicy { get; set; }
        public string Testing10BiggestDebtQuestionnaires { get; set; }
        public int Submit_Status { get; set; }

        public string Comment_compliance_auditreport_lastthreeyears { get; set; }

        public string Detailed_CommitteimplementationRCSActRulesandByelaws { get; set; }


    }
    public class CCSSurveySubmitFormInvestment
    {
        public string Investment_AddMorelabel { get; set; }
        public int Investment_AddMore { get; set; }
        public int AdministrativeId { get; set; }
    }



    public class GSSComplaint
    {

        public int Id { get; set; }
        public string SSOID { get; set; }
        public string NameEn { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public bool IsMember { get; set; }
        public bool IsAppRejected { get; set; }
        public bool IsGetLoan { get; set; }
        public bool IsApplyForLoan { get; set; }
        public string txtRemark { get; set; }
        public string txtGetloan { get; set; }
        public string RejectedDate { get; set; }
        public string AadharNo { get; set; }

        public string CoopBankName { get; set; }

        public string GssSocietyName { get; set; }



    }

    public class GSSComplaintNew
    {
        public int Id { get; set; }

        public string SSOID { get; set; }

        public string NameEn { get; set; }

        public string EmailId { get; set; }

        public string Gender { get; set; }

        public string MobileNo { get; set; }

        public string Address { get; set; }

        public bool IsMember { get; set; }

        public bool IsAppRejected { get; set; }

        public bool IsGetLoan { get; set; }

        public bool IsApplyForLoan { get; set; }

        public string txtRemark { get; set; }

        public string txtGetloan { get; set; }

        public string RejectedDate { get; set; }

        public string AadharNo { get; set; }

        public string CoopBankName { get; set; }

        public string GssSocietyName { get; set; }


        // NEW PROPERTIES

        public string ComplaintTypeId { get; set; }

        public string ComplaintType { get; set; }

        public string MembershipNo { get; set; }

        public string CCBId { get; set; }

        public string BranchId { get; set; }

        public string CCBPacsId { get; set; }
        public int IsComplaintReason { get; set; }

        public string ApplicationCreatedDate { get; set; }

        public string ApprovedAmount { get; set; }

        public string ReceivedAmount { get; set; }

        public string UploadDocument { get; set; }

        public string ApplicationDate { get; set; }

        public string ApprovalDate { get; set; }

        public string Remark { get; set; }
        public int ComplaintNo { get; set; }
    }


    public class GetDetailsForMapSociety_ResultData
    {
        public int SocietyID { get; set; }
        public string DISTRICT_ENG { get; set; }
        public int IsAuthorizedUser { get; set; }
        public Nullable<int> ActId { get; set; }
        public string AdhaNoPure { get; set; }
        public string ContactNo { get; set; }
        public Nullable<System.DateTime> OTPDateTime { get; set; }
        public string SSOID { get; set; }
        public string MaskingMobileNO { get; set; }
        public string AadharNo { get; set; }
        public string ActNameEn { get; set; }
        public string OTP { get; set; }
        public string SocietyIDEnc { get; set; }
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public string TypeameEng { get; set; }
        public string SocietyName { get; set; }
        public string txnid { get; set; }
        public string SocietyDirectory { get; set; }
        public int TotalMember { get; set; }
        public Nullable<bool> IsAmendment { get; set; }
        public Nullable<bool> IsAmendmentEdit { get; set; }
        public Nullable<int> ApprovalStatusId { get; set; }
        public string ProceesStatusTextEn { get; set; }
        public Nullable<int> MaxAppliFlowStatusID { get; set; }
        public bool IsObjection { get; set; }
    }


    public class UserPermission
    {
        public string SourceDirName { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Modify { get; set; }
        public bool FullAccess { get; set; }
        public string UserType { get; set; }
        public int Action { get; set; }
    }


    public class Pacs
    {

        public int BranchId { get; set; }
        public int BankID { get; set; }
        public string PacsNameEn { get; set; }
        public string PacsNameHi { get; set; }
        public string PacsCode { get; set; }

    }
    public class CommonDataModel
    {

        public int Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

    }

    public class ApiResponseGKK
    {
        public string Data { get; set; }
        public int Status { get; set; }
        public string DataJson { get; set; }
    }
    public class LandRecordGKK
    {
        public int ID { get; set; }
        public int DISTRICT_LG_CODE { get; set; }
        public string District { get; set; }
        public int TEHSIL_LG_CODE { get; set; }
        public string Tehsil { get; set; }
        public int ILR_CODE { get; set; }
        public string ILR { get; set; }
        public int PATWAR_CODE { get; set; }
        public string PatwarCircle { get; set; }
        public int VILLAGE_CODE { get; set; }
        public string Village { get; set; }
        public string KhataNo { get; set; }
        public string KhasraNo { get; set; }
        public int Year { get; set; }
        public string OwnerName { get; set; }
        public string Caste { get; set; }
        public string OwnerHissa { get; set; }
        public string Area { get; set; }
        public string LandClass { get; set; }
        public string SourceOfIrrigation { get; set; }
        public decimal Uncultivated_Area { get; set; }

        public List<CropGKK> Crops { get; set; }
    }
    public class CropGKK
    {
        public int AID { get; set; }
        public int CropId { get; set; }
        public string CROP_HIN { get; set; }
        public decimal Area { get; set; }
        public decimal IrrigatedArea { get; set; }
        public decimal NonIrrigatedArea { get; set; }
        public string IrrigationStatus { get; set; }
    }



    public class GodownProposalEntity
    {
        public string ApplicantName { get; set; }
        public string SocietyAvailabilityRemark { get; set; }
        public int GodownId { get; set; }
        public string Address { get; set; }
        public int SanctionAmt { get; set; }
        public string SanctionDate { get; set; }
        public string SanctionDoc { get; set; }
        public int GodownCapacity { get; set; }
        public int LandType { get; set; }
        public int LandSize { get; set; }
        public string CreatedBy { get; set; }
        public int CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public int UpdationDate { get; set; }
        public DateTime CreatedIP { get; set; }
        public int UpdatedIP { get; set; }
        public int status { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public int ActualCost { get; set; }
        public string HouseNo { get; set; }
        public string LaneStreetName { get; set; }
        public string Locality { get; set; }
        public int STATE_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public int CITY_ID { get; set; }
        public int DIVISION_ID { get; set; }
        public int WARD_ID { get; set; }
        public int PinCode { get; set; }
        public int ConstituencyId { get; set; }
        public string Photo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string SSOID { get; set; }
        public int KVSSGSSId { get; set; }
        public int BLOCK_ID { get; set; }
        public int GP_ID { get; set; }
        public int VILLAGE_ID { get; set; }
        public string SchemeName { get; set; }
        public int CCB { get; set; }
        public int CCBBranchId { get; set; }
        public int SocietyID { get; set; }
        public int MTCapacityId { get; set; }
        public int SocietyAvailabilityId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Usegodown { get; set; }
        public string CurrentStatus { get; set; }
        public int IsOldGodown { get; set; }
        public string ComplitionYear { get; set; }
        public decimal? LandSize1 { get; set; }
        public decimal? LandSize2 { get; set; }
        public decimal? TotalLandSize1 { get; set; }
        public decimal? TotalLandSize2 { get; set; }
        public string GodownDocuments { get; set; }
        public string DocumentPath { get; set; }
        public string FormationPath { get; set; }
        public string PWDCertificateFilePath { get; set; }
        public string RelatedGodownDocumentFilePath { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNumber { get; set; }
        public int? NoOfGodown { get; set; }
        public int? AssemblyId { get; set; }
        public List<TblTimeBoundDetails> TblGodownTimeBoundDetails { get; set; }

    }
    public class TblTimeBoundDetails
    {
        public int ID { get; set; }
        public int SocietyID { get; set; }
        public int MTCapacityId { get; set; }
        public int SchemeId { get; set; }
        public int FinancialYearID { get; set; }
        public string CreatedBy { get; set; }

    }

    public class GenerateASPDFResponse
    {
        public string PdfPath { get; set; }
        public DataTable Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ReturnPath { get; set; }
    }
    public class ElectionFormEntity
    {
        public int SocietyID { get; set; }
        public int SocietyCategoryId { get; set; }

        public int ElectionId { get; set; }

        public string RegistrationNo { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime LastElectionDate { get; set; }

        public DateTime ElectionDate { get; set; }
        public DateTime FirstBoardNominationDate { get; set; }

        public DateTime AdminAppointmentDate { get; set; }

        public DateTime DeputyAdminAppointmentDate { get; set; }

        public string ReasonForNonElection { get; set; }

        public int? MemberId { get; set; }

        public string PostName { get; set; }

        public DateTime VacancyDate { get; set; }

        public int? ReasonForVacancyId { get; set; }

        public string ExcludedVoterListDocument { get; set; }
        public string RecommendationDocument { get; set; }
        public bool? IsNewSociety { get; set; }
        public bool? IsVacantPost { get; set; }
        public string BylawsDocument { get; set; }
        public string CreatedBy { get; set; }

        public List<SocietyElectionVacancyEntity> VacancyList { get; set; }
    }
    public class SocietyElectionVacancyEntity
    {
        public int? VacancyId { get; set; }

        public int? ElectionId { get; set; }

        public int? VacantPostId { get; set; }

        public DateTime? VacancyDate { get; set; }

        public int? ReasonForVacancyId { get; set; }

        public string OtherReason { get; set; }

        public bool IsActive { get; set; } = true;
    }
    public class TaggedItemFormEntity
    {
        public int TagId { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public string PurchaseDate { get; set; }
        public string ExpirationDate { get; set; }
        public string PackagingType { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; } 
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }

        public int SocietyID { get; set; }
        public string RegistrationNo { get; set; }
        public string DistrictName { get; set; }
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public string SocietyType { get; set; }
    }

    public class FertilizerFormEntity
    {
        public int SocietyID { get; set; }
        public int FDRId { get; set; }
        public string RegistrationNo { get; set; }
        public string DistrictName { get; set; }
        public string SecretaryName { get; set; }
        public string SecretaryMobNo { get; set; }
        public bool? IsLicense { get; set; }
        public string LicenseNo { get; set; }
        public string DemandMonth { get; set; }
        public string DemandYear { get; set; }
        public bool? IsCreditFacility { get; set; }
        public string CreatedBy { get; set; }
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public int GPId { get; set; }
        public string SocietyType { get; set; }
        public string IFMSPOSId { get; set; }
        public List<FertilizerDemandEntity> DemandList { get; set; }
        public List<FertilizerCreditFacilityEntity> CreditFacilityList { get; set; }
        public List<FertilizerPurchaseEntity> PurchaseList { get; set; }
    }

    public class FertilizerDemandEntity
    {
        public int? DemandId { get; set; }
        public int? FDRId { get; set; }
        public int? FertilizerItemId { get; set; }
        public string FertilizerCompany { get; set; }
        public decimal? FertilizerQuantity { get; set; }
        public decimal FertilizerRate { get; set; }
        public decimal FertilizerCost { get; set; }
        public int? MonthId { get; set; }
        public int? YearId { get; set; }
        public string OtherFertilizerName { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class FertilizerCreditFacilityEntity
    {
        public int? CreditFacilityId { get; set; }
        public int FDRId { get; set; }
        public int FertilizerItemId { get; set; }
        public decimal FertilizerSale { get; set; }
        public decimal FertilizerQuantity { get; set; }
        public decimal FertilizerRate { get; set; }
        public decimal FertilizerCost { get; set; }
        public int? MonthId { get; set; }
        public int? YearId { get; set; }
        public decimal PresentAvailability { get; set; }
        public decimal CreditLimit { get; set; }
        public string OtherFertilizerName { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class FertilizerPurchaseEntity
    {
        public int? PurchaseId { get; set; }
        public int? FDRId { get; set; }
        public int? FertilizerItemId { get; set; }
        public decimal? FertilizerQuantity { get; set; }
        public bool IsActive { get; set; } = true;
    }



    public class KPIForPacsSaveModel
    {
        public int SocietyId { get; set; }
        public int? OwnBuilding { get; set; }
        public int? LandAvailable { get; set; }
        public int? GodownAvailable { get; set; }
        public int? GodownlessAvailable { get; set; }
        public int? Business { get; set; }
        public int? MembershipIncrease { get; set; }
        public int? loaneemember { get; set; }
        public int? profit { get; set; }
        public int? sharecapital { get; set; }
        public int? deposit { get; set; }
        public int? imbalance { get; set; }
        public int? dividend { get; set; }
        public int? otherKCCLoan { get; set; }
        public int? kccrecovery { get; set; }
        public int? gccLoan { get; set; }
        public int? otherLoanrecvry { get; set; }
        public int? auditcompleted { get; set; }
        public int? auditclassfiton { get; set; }
        public int? agmconvend { get; set; }
        public int? bod { get; set; }
        public int? compliancaudit { get; set; }
        public int? Fertilizer { get; set; }
        public int? Seed { get; set; }
        public int? Pesticide { get; set; }
        public int? Animal { get; set; }
        public int? CSC { get; set; }
        public int? PMKSK { get; set; }
        public int? CHC { get; set; }
        public int? Engaged { get; set; }
        public int? FPO { get; set; }
        public int? PMBJK { get; set; }
        public int? KUSUM { get; set; }
        public int? PDS { get; set; }
        public int? Specify { get; set; }
        public int? epacs { get; set; }
        public int? Audit { get; set; }
        public int? Dynamic { get; set; }
        public int? Migration { get; set; }
        public int? CCBID { get; set; }
    }


    // current year 
    public class AuditMprCommitteeCurrentYear
    {
        public long Id { get; set; }

        [Required]
        [StringLength(9)]
        public string FinancialYear { get; set; } = string.Empty;

        [Range(1, 12)]
        public byte Month { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public int CommitteeTypeId { get; set; }

        // Current-year committee details
        public int? TotalCommitteesCurrentYear { get; set; }

        // Proposals
        public int? ProposalDepartment { get; set; }
        public int? ProposalCA { get; set; }

        // Allocations
        public int? AllocationDepartment { get; set; }
        public int? AllocationCA { get; set; }

        // Monthly audited proposals
        public int? AuditedProposalDepartment { get; set; }
        public int? AuditedProposalCA { get; set; }

        // Monthly audited allocations
        public int? AuditedAllocationDepartment { get; set; }
        public int? AuditedAllocationCA { get; set; }

        // Audit fields
        [Required]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }



    public class AuditMprCurrentYearRequest
    {
        public string FinancialYear { get; set; }
        public int Month { get; set; }
        public int DistrictId { get; set; } 
        public int Office { get; set; } 
        public int RoleId { get; set; } 
        public int UserRoleMappingId { get; set; } 
        public long UpdatedBy { get; set; }
        public List<AuditMprCurrentYearRecord> Records { get; set; }
    }

    public class AuditMprCurrentYearRecord
    {
        public int CommitteeTypeId { get; set; }

        public int TotalCommitteesCurrentYear { get; set; }

        public int ProposalDepartment { get; set; }
        public int ProposalCA { get; set; }

        public int AllocationDepartment { get; set; }
        public int AllocationCA { get; set; }

        public int ProposalAllocationRemaining { get; set; }

        public int PreviousAuditedProposalDepartment { get; set; }
        public int PreviousAuditedProposalCA { get; set; }

        public int PreviousAuditedAllocationDepartment { get; set; }
        public int PreviousAuditedAllocationCA { get; set; }

        public int CurrentAuditedProposalDepartment { get; set; }
        public int CurrentAuditedProposalCA { get; set; }

        public int CurrentAuditedAllocationDepartment { get; set; }
        public int CurrentAuditedAllocationCA { get; set; }
    }


}

