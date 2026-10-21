using Cooperative.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Cooperative.Models
{
    public class JanAadharModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public JanAadharMemberDetails UserDetails { get; set; }

        public class JanAaadharFamilyMemberModel
        {
            public string AADHAR_ID { get; set; }
            public string NAME { get; set; }
            public string DOB { get; set; }
            public string JAN_MEMBER_ID { get; set; }
            public string Masked_JAN_MEMBER_ID { get; set; }
            public string Encrypted_JAN_MEMBER_ID { get; set; }

            public string SRDR_MID { get; set; }
            public string JAN_AADHAR { get; set; }
            public string Encrypted_JAN_AADHAR { get; set; }
            public string NAME_HINDI { get; set; }
            public string SRDR_ID { get; set; }
            public string JANAADHAARMEMID { get; set; }
            public string ENR_ID { get; set; }
            public string Encrypted_ENR_ID { get; set; }
            public string NFSA_STATUS { get; set; }
            public string FATHER_NAME_HND { get; set; }
            public string MOBILE_NO { get; set; }
            public string Masked_MOBILE_NO { get; set; }
            public string Encrypted_MOBILE_NO { get; set; }
            public string FATHER_NAME_ENG { get; set; }
            public string AADHAR_REF_NO { get; set; }
            public string Encrypted_AADHAR_REF_NO { get; set; }
            public string GENDER { get; set; }
            public string MEMBER_TYPE { get; set; }
            public string tid { get; set; }
            public string transactionId { get; set; }
            public string JanAadharOTP { get; set; }
            public int? Age { get; set; } = 0;
        }

        public class JanAadharMemberDetails
        {
            public string JanAadharNo { get; set; }
            public string Encrypted_JanAadharNo { get; set; }
            public string JanAadharMID { get; set; }
            public string Encrypted_JanAadharMID { get; set; }
            public string JanAadharEnrollmentId { get; set; }
            public string AadharNo { get; set; }
            public string Encrypted_AadharNo { get; set; }
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
            public string Encrypted_BankAccountNo { get; set; }
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
            public JanAadharMemberAddressDetails Address { get; set; }
            public bool IsRajasthanResident { get; set; }
            public string ssoid { get; set; }
        }
        public class JanAadharMemberAddressDetails
        {
            public bool IsRural { get; set; }
            public string Address_ENG { get; set; }
            public string Address_HI { get; set; }
            public string PinCode { get; set; }
            public string DistrictName_ENG { get; set; }
            public string DistrictName_HI { get; set; }
            public string District_Code { get; set; }
            public int DISTRICT_ID { get; set; }
            public string Block_ENG { get; set; }
            public string Block_HI { get; set; }
            public string Block_Code { get; set; }
            public int BLOCK_ID { get; set; }
            public string City_ENG { get; set; }
            public string City_HI { get; set; }
            public string City_Code { get; set; }
            public int CITY_ID { get; set; }
            
            public string GP_ENG { get; set; }
            public string GP_HI { get; set; }
            public string GP_Code { get; set; }
            public int GP_ID { get; set; }
            
            public string Village_ENG { get; set; }
            public string Village_HI { get; set; }
            public string Village_Code { get; set; }
            public int VILLAGE_ID { get; set; }

            public string Ward_ENG { get; set; }
            public string Ward_HI { get; set; }
            public string Ward_Code { get; set; }
            public int WARD_ID { get; set; }
        }

    }
}