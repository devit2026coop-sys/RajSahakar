using Cooperative.Filters;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    public class PageTitleMetaSetting
    {
        public static string SetPageTitle(string PageName, string Language = "en-US")
        {
            switch (Language.ToLower())
            {
                case "en-us":
                    {
                        switch (PageName)
                        {
                            case PageTitleFactory.Home: return "Home - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserRegistration: return "User Registration - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Role: return "Role - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.MenuAction: return "MenuAction - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Menu: return "Menu - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Division: return "Division - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.District: return "District - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Department: return "Department - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.FinancialYear: return "Financial Year - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Designation: return "Designation - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Post: return "Post - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Gender: return "Gender - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Religion: return "Religion - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Cast: return "Cast - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Category: return "Category - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.State: return "State - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Tehsil: return "Tehsil - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Block: return "Panchayat Samiti (Block) - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.GP: return "Gram Panchayat - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Village: return "Village - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Ward: return "Ward - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.City: return "City - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Widget: return "Widget - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.DashBoard: return "DashBoard -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.WorkFlow: return "Workflow -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.LOV: return "List of Value -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.LOVValue: return "Lov Value -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SMSFormat: return "SMS Templates -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.MailFormat: return "Mail Templates -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.FileUpload: return "File Upload- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserRoleDeptMapping: return "User Role Dept Mapping - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SubMenu: return "Sub Menu- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserRole: return "User Role - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserProfile: return "User Profile - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyRegistration: return "Society Profiling - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyProfilingView: return "Society Profiling  View- Directorate of Cooperative Society (Government of Rajasthan)";

                            case PageTitleFactory.AuditorData: return "Auditor Data- Directorate of Cooperative Society (Government of Rajasthan)";

                            case PageTitleFactory.Classes: return "Classes  View- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SubClasses: return "Sub Classes  View- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Buissness: return "Buissness  View- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyType: return "Society Type- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Unit: return "Unit - Directorate of Cooperative Society (Government of Rajasthan)";

                            case PageTitleFactory.AssignAuditor: return "Assign Auditor - Directorate of Cooperative Society (Government of Rajasthan)";

                            case PageTitleFactory.Auditor: return "Auditor- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserRegistrationRequest: return "User Registration Request - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserRegistrationRequestApprovable: return "User Registration Request Approvable - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Inspactor: return "Inspactor -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.AuditorAssignmentByDepartment: return "Auditor Assignment ByDepartment - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UserSocietyMappingValid: return "User Society Mapping Validation -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SportAssociation: return "Sport Association -  Directorate of Cooperative Society (Government of Rajasthan)";

                            case PageTitleFactory.SocietyAuditReport: return "Society Audit Report Upload -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.VeryFiedUser: return "VeryFied User - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Organisation: return "Organisation -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.ApplicationFlow: return "Application Flow - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Office: return "Office -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Index: return "Index -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyPayments: return "Society Payment -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyCertificateGeneration: return "Society Certificate Generation- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.SocietyApplicationView: return "Society Application View -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.TransferRequest: return "User Transfer Request -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.UnAuthorizeAccess: return "UnAuthorize Access -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Election: return "Election  -  Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.ElectionImportantDates: return "Election Important Dates  - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.Publish: return "Publish   - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.Trainning: return "Trainning  - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.FeedBacks: return "FeedBack  - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.DeptTrainningAcceptance: return "DepartmentTrainningRevert  - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.MemberApplication: return "MemberApplication   - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.ImpoDateAction: return "Important Date and Action   - Directorate of Cooperative Society(Government of Rajasthan)";
                            case PageTitleFactory.Voters: return "Voters(Member) of Society -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.VotersElection: return "Voters(Member) of Society -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.CandidateList: return "Candidate List of Society -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.ElectedMemberList: return "Elected Member List of Society -   Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.BankStatement: return "Bank Statement- Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.ErrorLog: return "ErrorLog-  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.CaseRegistraion: return "Case Registraion-  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.DynamicPageContent: return "DynamicPageContent -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.NewsContent: return "NewsContent -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.CoopertativeInstitution: return "CoopertativeInstitution -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Coopertatives: return "Coopertatives -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Downloads: return "Downloads -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Fertilizer: return "Fertilizer -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.Message: return "Message -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.AuditorRequest: return "AuditorRequest -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.OrderCirculars: return "Order and Circulars - Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.InspectionReport: return "Inspection Report -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.TelephoneDir: return "Telephone Directory -  Directorate of Cooperative Society (Government of Rajasthan)";
                            case PageTitleFactory.PurchaseCenter: return "Purchase Center (Government of Rajasthan)";
                            case PageTitleFactory.GopalCreditCardRegistration: return "Gopal Credit Card Registration";
                            default: return "Directorate of Evaluation Organisation (Government of Rajasthan)";

                        }

                    }
                case "hi-in":
                    {
                        switch (PageName)
                        {
                            case PageTitleFactory.Home: return "गृह - प्रगतिशील सहकारी समितियां(राजस्थान सरकार)";
                            case PageTitleFactory.UserRegistration: return "उपयोगकर्ता पंजीकरण - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Role: return "भूमिका - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.MenuAction: return "मेनू कार्रवाई - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Menu: return "मेनू - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Division: return "संभाग - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.District: return "जिला - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Department: return "विभाग - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.FinancialYear: return "वित्तीय वर्ष - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Designation: return "पद - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Post: return "पदनाम - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Gender: return "लिंग - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Religion: return "धर्म - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Cast: return "जाति  - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Category: return "वर्ग - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.State: return "राज्य - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Tehsil: return "तहसील - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Block: return "पंचायत समिति (ब्लॉक) - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.GP: return "ग्राम  पंचायत  - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Village: return "गाँव - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Ward: return "वार्ड  - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.City: return "शहर  - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.Widget: return "विजिट  - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.DashBoard: return "डैशबोर्ड -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.WorkFlow: return "कार्य प्रवाह -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.LOV: return "मूल्यों की सूची -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.LOVValue: return "मूल्य के मूल्यों की सूची -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.SMSFormat: return "एसएमएस टेम्पलेट्स -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.MailFormat: return "मेल टेम्पलेट्स -  प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.FileUpload: return "फाइल अपलोड- प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.UserRoleDeptMapping: return "उपयोगकर्ता भूमिका विभाग मानचित्रण - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.SubMenu: return "उप मेनू - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.UserRole: return "उपयोगकर्ता भूमिका - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.UserProfile: return "उपयोगकर्ता प्रोफ़ाइल - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyRegistration: return "सोसायटी प्रोफाइलिंग - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyProfilingView: return "सोसायटी प्रोफाइलिंग विवरण - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";

                            case PageTitleFactory.AuditorData: return "लेखा परीक्षक डेटा - प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";

                            case PageTitleFactory.Classes: return "कक्षाएं प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                            case PageTitleFactory.SubClasses: return "उप वर्ग देखें- सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Buissness: return "व्यापार देखें- सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyType: return "सोसाइटी टाइप- सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Unit: return "इकाई- सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.AssignAuditor: return "लेखा परीक्षक असाइनमेंट- सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Auditor: return "लेखा परीक्षक - सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.UserRegistrationRequest: return "उपयोगकर्ता पंजीकरण अनुरोध -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.UserRegistrationRequestApprovable: return "उपयोगकर्ता पंजीकरण अनुरोध स्वीकार्य -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Inspactor: return "इंस्पेक्टर -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.AuditorAssignmentByDepartment: return "विभाग द्वारा लेखा परीक्षक असाइनमेंट -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.UserSocietyMappingValid: return "उपयोगकर्ता सोसाइटी मैपिंग मान्य -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SportAssociation: return "खेल संघ -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyAuditReport: return "सोसायटी लेखा परीक्षा रिपोर्ट अपलोड करें -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.VeryFiedUser: return "सत्यापित उपयोगकर्ता -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Organisation: return "संस्था -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Office: return "कार्यालय -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.ApplicationFlow: return "आवेदन प्रवाह -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Index: return "सूची -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyPayments: return "सोसाइटी भुगतान -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyCertificateGeneration: return "सोसायटी प्रमाणपत्र जनरेशन -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.SocietyApplicationView: return "सोसायटी आवेदन देखें -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.TransferRequest: return "उपयोगकर्ता स्थानांतरण अनुरोध -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.UnAuthorizeAccess: return "अनाधिकृत उपयोग -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Election: return "चुनाव   -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.ElectionImportantDates: return "चुनाव महत्वपूर्ण तिथियां   -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Publish: return "प्रकाशित   -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.FeedBacks: return "प्रशिक्षण -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.DeptTrainningAcceptance: return "विभाग प्रशिक्षण वापसी -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.MemberApplication: return "सदस्य आवेदन   -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.ImpoDateAction: return "महत्वपूर्ण तिथि और कार्य  -  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Voters: return "संस्था के मतदाता(सदस्य)-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.VotersElection: return "संस्था के मतदाता(सदस्य)-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.CandidateList: return "संस्था की उम्मीदवार सूची-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.ElectedMemberList: return "संस्था की निर्वाचित सदस्य सूची-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.BankStatement: return "बैंक विवरण-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.ErrorLog: return "त्रुटि संग्रह-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.CaseRegistraion: return "केस पंजीकरण-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.DynamicPageContent: return "पृष्ठ सामग्री-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.NewsContent: return "समाचार सामग्री-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.CoopertativeInstitution: return "सहकारी संस्था-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Coopertatives: return "सहकारिता-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Downloads: return "डाउनलोड-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Fertilizer: return "उर्वरक-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.Message: return "संदेश-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.AuditorRequest: return "लेखा परीक्षक अनुरोध-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.OrderCirculars: return "आदेश और परिपत्र-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.InspectionReport: return "निरीक्षण रिपोर्ट-  सहकारी समिति निदेशालय (राजस्थान सरकार)";
                            case PageTitleFactory.TelephoneDir: return "दूरभाष निदेशिका-  सहकारी समिति निदेशालय (राजस्थान सरकार)";                            
                            case PageTitleFactory.PurchaseCenter: return "खरीद केंद्र (राजस्थान सरकार)";
                            case PageTitleFactory.GopalCreditCardRegistration: return "गोपाल क्रेडिट कार्ड पंजीकरण";
                            default: return "प्रगतिशील सहकारी समितियां (राजस्थान सरकार)";
                        }
                    }
                default:
                    return string.Empty;
            }
        }
    }


    public class PageTitleFactory
    {
        private PageTitleFactory() { }

        public const string DashBoard = "DashBoard";
        public const string Home = "Home";
        public const string UserRegistration = "UserRegistration";
        public const string Role = "Role";
        public const string MenuAction = "MenuAction";
        public const string Menu = "Menu";
        public const string Scheme = "Scheme";
        public const string Division = "Division";
        public const string District = "District";
        public const string FinancialYear = "FinancialYear";
        public const string Department = "Department";
        public const string Designation = "Designation";
        public const string Post = "Post";

        public const string Gender = "Gender";
        public const string Religion = "Religion";
        public const string Cast = "Cast";
        public const string Category = "Category";
        public const string State = "State";
        public const string Tehsil = "Tehsil";
        public const string Block = "Block";
        public const string GP = "GP";
        public const string Village = "Village";
        public const string Ward = "Ward";
        public const string City = "City";
        public const string Widget = "Widget";

        public const string WorkFlow = "WorkFlow";

        public const string LOV = "LOV";
        public const string LOVValue = "LOVValue";
        public const string SMSFormat = "SMS Format";
        public const string MailFormat = "Mail Format";
        public const string FileUpload = "FileUpload";
        public const string UserRoleDeptMapping = "UserRoleDeptMapping";
        public const string SubMenu = "SubMenu";
        public const string UserRole = "UserRole";
        public const string UserProfile = "UserProfile";
        public const string SocietyRegistration = "SocietyRegistration";
        public const string SocietyProfilingView = "SocietyProfilingView";

        public const string AuditorData = "AuditorData";

        public const string Classes = "Classes";
        public const string SubClasses = "Sub Classes";
        public const string Buissness = "Buissness";
        public const string SocietyType = "SocietyType";
        public const string Unit = "Unit";
        public const string Organisation = "Organisation";
        public const string AssignAuditor = "AssignAuditor";
        public const string Auditor = "Auditor";
        public const string UserRegistrationRequest = "User Registration Request";
        public const string UserRegistrationRequestApprovable = "User RegistrationRequest Approvable";
        public const string Inspactor = "Inspactor";
        public const string AuditorAssignmentByDepartment = "Auditor Assignment By Department";
        public const string UserSocietyMappingValid = "User Society MappingValid";
        public const string SportAssociation = "Sport Association";
        public const string SocietyAuditReport = "SocietyAuditReport";
        public const string VeryFiedUser = "VeryFied User";
        public const string ApplicationFlow = "Application Flow";
        public const string Office = "Office";
        public const string Index = "Index";
        public const string SocietyPayments = "Society Payment";
        public const string SocietyCertificateGeneration = "Society Certificate Generation";
        public const string GetSocietyByEmitra = "Get Society Detail By Emitra";
        public const string SocietyApplicationView = "SocietyApplictionView";
        public const string TransferRequest = "User Transfer Request";
        public const string UnAuthorizeAccess = "UnAuthorize Access";
        public const string Election = "Election";
        public const string ElectionImportantDates = "ElectionImportantDates";
        public const string Publish = "Publish";
        public const string Trainning = "Trainning";
        public const string FeedBacks = "FeedBacks";
        public const string DeptTrainningAcceptance = "DepartmentTrainningRevert";
        public const string MemberApplication = "MemberApplication";
        public const string ImpoDateAction = "Important Date and Action";
        public const string Voters = "Voters(Member) of Society";
        public const string VotersElection = "Voters(Member) of Society for Election";
        public const string CandidateList = "Candidate List of Society for Election";
        public const string ElectedMemberList = "Elected Member List of Society";
        public const string BankStatement = "BankStatement of Society ";
        public const string ErrorLog = "ErrorLog";
        public const string CaseRegistraion = "CaseRegistraion";
        public const string TrainningAttendance = "Training Attendance";
        public const string TrainningReport = "Training Report";
        public const string SocietyRegisterReport = "Society Register Report";
        public const string DynamicPageContent = "Dynamic Page Content";
        public const string NewsContent = "News Content";
        public const string CoopertativeInstitution = "Coopertative Institution";
        public const string Coopertatives = "Coopertatives";
        public const string Downloads = "Downloads";
        public const string Fertilizer = "Fertilizer";
        public const string Message = "Message";
        public const string AuditorRequest = "Auditor Request";
        public const string OrderCirculars = "Order and Circulars";
        public const string InspectionReport = "InspectionReport";
        public const string TelephoneDir = "TelephoneDir";
        public const string WrHoliday = "Warehouse Holiday";
        public const string PurchaseCenter = "Purchase Center";
        public const string LokGuarenteeServiceWise = "LokGuarentee ServiceWise";
        public const string AuditProgressBlock = "Audit Progress Block ";
        public const string GopalCreditCardRegistration = "Gopal Credit Card Registration";  //pankaj
        public const string WorkFlowConfiguration = "WorkFlowConfiguration";
    }
}