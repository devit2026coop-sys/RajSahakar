
using System;

namespace Cooperative.Models
{
    public class MPacsMemberShipRegistrationModel
    {
        public int MPacsMembershipId { get; set; }
        public string ReferenceNo { get; set; } = string.Empty;
        public string MembershipNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public int? Age { get; set; } = 0;
        public string Gender { get; set; }
        public string Photo_Base64 { get; set; }
        public string PhotoPath { get; set; }
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; }
        public string SpouseName { get; set; }
        public string NomineeName { get; set; }
        public string NomineeFatherName { get; set; }
        public int? NomineeRelationId { get; set; }
        public int? EducationalQualificationId { get; set; }
        public string MobileNo { get; set; } = string.Empty;
        public int? CategoryId { get; set; } = 0;
        public string JanAadharNo { get; set; }
        public string JanAadharMID { get; set; }
        public string JanAadharEnrollmentId { get; set; }
        public string JanAadharNoEnc { get; set; }
        public string Encrypted_JanAadharNo { get; set; }
        public string Encrypted_JanAadharMID { get; set; }
        public string HouseBuildingNo { get; set; } = string.Empty;
        public string LaneStreetName { get; set; } = string.Empty;
        public string Locality { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? DistrictId { get; set; } = 0;
        public bool? IsRural { get; set; } = false;
        public int? BlockId { get; set; } = 0;
        public int? GpId { get; set; } = 0;
        public int? VillageId { get; set; } = 0;
        public int? CityId { get; set; } = 0;
        public int? WardId { get; set; } = 0;
        public string PinCode { get; set; }

        public string PresentHouseBuildingNo { get; set; } = string.Empty;
        public string PresentLaneStreetName { get; set; } = string.Empty;
        public string PresentLocality { get; set; } = string.Empty;
        public string PresentAddress { get; set; } = string.Empty;
        public int? PresentDistrictId { get; set; } = 0;
        public bool? PresentIsRural { get; set; } = false;
        public int? PresentBlockId { get; set; } = 0;
        public int? PresentGpId { get; set; } = 0;
        public int? PresentVillageId { get; set; } = 0;
        public int? PresentCityId { get; set; } = 0;
        public int? PresentWardId { get; set; } = 0;
        public string PresentPinCode { get; set; }

        public int? MPacsDistrictId { get; set; } = 0;
        public int? MPacsBankId { get; set; } = 0;
        public int? MPacsBranchId { get; set; } = 0;
        public int? MPacsPacsId { get; set; } = -1;
        public string OtherPacsName { get; set; } = string.Empty;
        public int? CurrentApplicationFlowId { get; set; } = 0;
        public int? StatusId { get; set; } = 0;
        public string SSOID { get; set; } = string.Empty;
        public string UserType { get; set; }
        public string KIOSKCODE { get; set; }
        public long? CreatedBy { get; set; } = 0;
    }

    public class MPacsMemberShipRegistrationResponseModel: MPacsMemberShipRegistrationModel
    {
        public string Category { get; set; }
        public string Qualification { get; set; }
        public string NomineeRelation { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string CityName { get; set; }
        public string BlockName { get; set; }
        public string GpName { get; set; }
        public string VillageName { get; set; }
        public string PresentDistrictName { get; set; }
        public string PresentWardName { get; set; }
        public string PresentCityName { get; set; }
        public string PresentBlockName { get; set; }
        public string PresentGpName { get; set; }
        public string PresentVillageName { get; set; }
        public string MPacsDistrictName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string PacsName { get; set; }

    }

        public class MPacsMemberShipRegistrationJanaadharModel : MPacsMemberShipRegistrationModel
    {
        public bool Status { get; set; }
        public string Category { get; set; }
        public string District_Code { get; set; }
        public string Block_Code { get; set; }
        public string GP_Code { get; set; }
        public string Village_Code { get; set; }
        public string City_Code { get; set; }
        public string Ward_Code { get; set; }
    }


    public class MpacsApplicationFlowDataModel
    {
        public int MPacsMembershipId { get; set; }
        public string EncId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string ReceiptNo { get; set; }
        public int PacsId { get; set; }
        public string Remark { get; set; }
        public string DocumentName { get; set; }
        public string MembershipCertificateFile { get; set; }
        public string CommitteeApprovalReport { get; set; }
        public int DocumentId { get; set; }

        public string OTP { get; set; }
        public int RoleId { get; set; }


        
    }

}