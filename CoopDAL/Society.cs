using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Cooperative.Models;
using Models;

namespace CoopDAL
{
    public class Society
    {
        public List<SearchSociety_Result> SearchSociety(string RegistrationNo)
        {
            List<SearchSociety_Result> result = new List<SearchSociety_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SearchSociety(RegistrationNo, "").ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SearchSociety(" + RegistrationNo + ")" }, ex);

            }
            return result;
        }

        public List<GetSociety_Result> GetSociety(string SocietyId)
        {
            List<GetSociety_Result> result = new List<GetSociety_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.GetSociety(SocietyId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSociety(" + SocietyId + ")" }, ex);

            }
            return result;
        }

        public List<SocietyListBydepartment_0_Result> SocietyListBydepartment_0(string WhereClause, int IsForEmpanelledAgency, int LevelId, int Division_District_Id, int FYearId, int CategoryId, int UserRoleDepartmentId, int DistrictId, int DivisionId, string Resion)
        {
            List<SocietyListBydepartment_0_Result> result = new List<SocietyListBydepartment_0_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyListBydepartment_0(WhereClause, LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId, Resion, DistrictId, DivisionId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListBydepartment_0(WhereClause,LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId)" }, ex);

            }
            return result;
        }

        public List<SocietyListBydepartment_1_Result> SocietyListBydepartment_1(string WhereClause, int IsForEmpanelledAgency, int LevelId, int Division_District_Id, int FYearId, int CategoryId, int UserRoleDepartmentId, int DistrictId, int DivisionId, string Resion)
        {
            List<SocietyListBydepartment_1_Result> result = new List<SocietyListBydepartment_1_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyListBydepartment_1(WhereClause, LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId, Resion, DistrictId, DivisionId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListBydepartment_1(WhereClause,LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId)" }, ex);

            }
            return result;
        }

        public List<SocietyListBydepartment_2_v2_Result> SocietyListBydepartment_2_v2(string WhereClause, int IsForEmpanelledAgency, int LevelId, int Division_District_Id, int FYearId, int CategoryId, int UserRoleDepartmentId, int DistrictId, int DivisionId, string Resion)
        {
            List<SocietyListBydepartment_2_v2_Result> result = new List<SocietyListBydepartment_2_v2_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyListBydepartment_2_v2(WhereClause, LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId, Resion, DistrictId, DivisionId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListBydepartment_2_v2_Result(WhereClause,LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId)" }, ex);

            }
            return result;
        }

        public List<SocietyListBydepartment_2_v3_Result> SocietyListBydepartment_2(ref DataPagingModel Details, int LevelId, int UserRoleDepartmentId, int Division_District_Id)
        {
            List<SocietyListBydepartment_2_v3_Result> result = new List<SocietyListBydepartment_2_v3_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
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
                    

                    if (Details.PageNumber == 1)
                    {
                        Details.TotalRecords = DB.SocietyListBydepartment_2_v3(searchText, LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId, ResionId, DistrictId, DivisionId,
                            IsForEmpanelledAgency, Details.PageSize, Details.PageNumber, true).FirstOrDefault().TotalRecord;
                    }
                    result = DB.SocietyListBydepartment_2_v3(searchText, LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId, ResionId, DistrictId, DivisionId,
                            IsForEmpanelledAgency, Details.PageSize, Details.PageNumber, false).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListBydepartment_2(WhereClause,LevelId, Division_District_Id, IsForEmpanelledAgency, FYearId, CategoryId, UserRoleDepartmentId)" }, ex);

            }
            return result;
        }

        public List<SocietyListBySSOID_Result> SocietyListBySSOID(string SSOID, string SocietyIDEnc, int ActId, int SocietyTypeId, int ClassId, string SearchText, string UserType)
        {
            List<SocietyListBySSOID_Result> result = new List<SocietyListBySSOID_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyListBySSOID(SSOID, SocietyIDEnc, ActId, SocietyTypeId, ClassId, SearchText, UserType).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListBySSOID(" + SSOID + ")" }, ex);

            }
            return result;
        }


        public List<GetSocietyForFees_Result> GetSocietyForFees(string SSOID, string SearchText, string UserType)
        {
            List<GetSocietyForFees_Result> result = new List<GetSocietyForFees_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.GetSocietyForFees(SSOID, SearchText, UserType).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocietyForFees(" + SSOID + ")" }, ex);

            }
            return result;
        }

        public List<SocietyDocument> GetSocietyDocument(int SocietyId)
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyDocuments.Where(x => x.SocietyId == SocietyId && x.Isactive == true && x.DocumentType == "Other").ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetSocietyForFees()" }, ex);

            }
            return result;
        }

        public List<SocietyDocument> AddEditSocietyDocument(SocietyDocument Details, string SocietyIdEnc = "")
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti

                    bool IsExistFile = false;
                    if (SocietyIdEnc != "")
                    {
                       // var d = DB.GetDecrypt(SocietyIdEnc).FirstOrDefault();
                        Details.SocietyId = Convert.ToInt32(DB.GetDecrypt(SocietyIdEnc).FirstOrDefault());

                        IsExistFile = DB.SocietyDocuments.Where(x => x.DocumentURL == Details.DocumentURL && x.SocietyId == Details.SocietyId).Any();

                    }
                    if (!IsExistFile)
                    {
                        var data = DB.SocietyDocuments.Where(x => x.Id == Details.Id).FirstOrDefault();
                        if (data != null)
                        {
                            data.Isactive = Details.Isactive;
                            data.DocumentURL = Details.DocumentURL;
                            //data.CreatedDate = DateTime.Now;
                            data.DocumentName = Details.DocumentName;
                            data.DocumentType = Details.DocumentType;
                            data.UpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            Details.CreatedDate = DateTime.Now;
                            Details.UpdatedDate = DateTime.Now;
                            //D1etails.CreatedBy=_ll
                            DB.SocietyDocuments.Add(Details);
                            DB.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddEditSocietyDocument()" }, ex);

            }
            return result;
        }

        public int DeleteSocietyDocument(int Id)
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    var data = DB.SocietyDocuments.Where(x => x.Id == Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.Isactive = false;
                        DB.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddEditSocietyDocument()" }, ex);
                return 0;
            }
        }

        public List<AuditorAssignmentList_V1_Result> GetAuditorAssignmentList(ref DataPagingModel Details, int DivisionId, int DistrictId, string SSO_ID, int MappingId, int Level)
        {


            List<AuditorAssignmentList_V1_Result> result = new List<AuditorAssignmentList_V1_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    if (Details == null)
                    {
                        Details = new DataPagingModel();
                    }

                    string searchText = "";
                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {
                        searchText = Details.SearchParameter.Where(x => x.Key == "searchText").FirstOrDefault().Value;
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

                    if (Details.SearchParameter.Where(x => x.Key == "DistrictId").Any())
                    {
                        DistrictId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "DistrictId").FirstOrDefault().Value);
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "DivisionId").Any())
                    {
                        DivisionId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "DivisionId").FirstOrDefault().Value);
                    }
                    int ResionId = 0;
                    if (Details.SearchParameter.Where(x => x.Key == "Resion").Any())
                    {
                        ResionId = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "Resion").FirstOrDefault().Value);
                    }

                    bool isEmpanelled = false;
                    if (Details.SearchParameter.Where(x => x.Key == "inlineRadioOptions").Any())
                    {
                        isEmpanelled = Convert.ToBoolean(Details.SearchParameter.Where(x => x.Key == "inlineRadioOptions").FirstOrDefault().Value);
                    }

                    int Id = 1;
                    if (Details.SearchParameter.Where(x => x.Key == "Id").Any())
                    {
                        Id = Convert.ToInt32(Details.SearchParameter.Where(x => x.Key == "Id").FirstOrDefault().Value);
                    }



                    if (Details.PageNumber == 1)
                    {
                        Details.TotalRecords = DB.AuditorAssignmentList_V1(Details.PageSize, Details.PageNumber, true, DivisionId, DistrictId, ResionId, FYearId, isEmpanelled, Id, CategoryId, SSO_ID, Level, MappingId, searchText).FirstOrDefault().TotalRecord;
                    }
                    result = DB.AuditorAssignmentList_V1(Details.PageSize, Details.PageNumber, false, DivisionId, DistrictId, ResionId, FYearId, isEmpanelled, Id, CategoryId, SSO_ID, Level, MappingId, searchText).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetAuditorAssignmentList()" }, ex);

            }
            return result;
        }



        public List<SocietyDocument> GetSocietyAmendmentDocument(int SocietyId)
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.SocietyDocuments.Where(x => x.SocietyId == SocietyId && x.Isactive == true && x.DocumentType == "Other").ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetSocietyForFees()" }, ex);

            }
            return result;
        }

        public List<SocietyDocument> AddEditSocietyAmendmentDocument(SocietyAmendmentDocument Details, string SocietyIdEnc = "")
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti

                    bool IsExistFile = false;
                    if (SocietyIdEnc != "")
                    {
                        var d = DB.GetDecrypt(SocietyIdEnc).FirstOrDefault();
                        Details.SocietyId = Convert.ToInt32(DB.GetDecrypt(SocietyIdEnc).FirstOrDefault());

                        IsExistFile = DB.SocietyAmendmentDocuments.Where(x => x.DocumentName == Details.DocumentName && x.AmendmentId == Details.AmendmentId).Any();

                    }
                    if (!IsExistFile)
                    {
                        var data = DB.SocietyAmendmentDocuments.Where(x => x.Id == Details.Id).FirstOrDefault();
                        if (data != null)
                        {
                            //data.Isactive = Details.Isactive;
                            data.DocumentName = Details.DocumentName;
                            data.DocumentType = Details.DocumentType;
                            data.CreatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }
                        else
                        {
                            Details.CreatedDate = DateTime.Now;
                            DB.SocietyAmendmentDocuments.Add(Details);
                            DB.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddEditSocietyDocument()" }, ex);

            }
            return result;
        }

        public int DeleteSocietyAmendmentDocument(int Id)
        {
            List<SocietyDocument> result = new List<SocietyDocument>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    var data = DB.SocietyAmendmentDocuments.Where(x => x.Id == Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.IsDelete = true;
                        DB.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddEditSocietyDocument()" }, ex);
                return 0;
            }
        }

        public SocietyDashboard GetSocietyDashboardData()
        {
            SocietyDashboard result = new SocietyDashboard();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti                    
                    result.SocietyData = DB.GetSocietyCountActWise().ToList();
                    result.DistrictWiseSocietyData2001 = DB.GetSocietyCountActDistricWise(3).ToList();
                    result.DistrictWiseSocietyData1958 = DB.GetSocietyCountActDistricWise(1).ToList();
                    result.DistrictWiseSocietyData2005 = DB.GetSocietyCountActDistricWise(7).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetSocietyDashboardData()" }, ex);

            }
            return result;
        }

        public List<GetAllSocietyDocuments_Result> GetAllSocietyDocuments(int SocietyId)
        {
            List<GetAllSocietyDocuments_Result> result = new List<GetAllSocietyDocuments_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    result = DB.GetAllSocietyDocuments(SocietyId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetAllSocietyDocuments()" }, ex);

            }
            return result;
        }

        public int InsertAmendmentSenction4(int SocietyId, string Name, string ContactNo, string UserType, string CreatedIP, string AADHARENO, string DOB, string GENDER, string Photo, string CategoryId, string POSTNAME, string MEMBERTYPE, string Address, string Occupation, string FatherName, string SSOID)
        {
            int result = 0;
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    return DB.InsertAmendmentSenction4(SocietyId, Name, ContactNo, UserType, CreatedIP, AADHARENO, DOB, GENDER, Photo, CategoryId, POSTNAME, MEMBERTYPE, Address, Occupation, FatherName, SSOID).ToList()[0].Value;
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertAmendmentSenction4("+SocietyId+","+ Name + "," + ContactNo + "," + UserType + "," + CreatedIP + "," + AADHARENO + "," + DOB + "," + GENDER + "," + Photo + "," + CategoryId + "," + POSTNAME + "," + MEMBERTYPE + "," + Address + "," + Occupation + "," + FatherName + "," + SSOID + ")" }, ex);
            }
            return result;
        }

        public List<GetAmendmentHistory_Result> GetAmendmentHistory(string SSOID)
        {
            List<GetAmendmentHistory_Result> result = new List<GetAmendmentHistory_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    result = DB.GetAmendmentHistory(SSOID).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetAmendmentHistory()" }, ex);

            }
            return result;
        }

        public List<GetAmendmentIssuedDocuments_Result> GetAmendmentIssuedDocuments(int AmendmentId)
        {
            List<GetAmendmentIssuedDocuments_Result> result = new List<GetAmendmentIssuedDocuments_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    result = DB.GetAmendmentIssuedDocuments(AmendmentId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetAmendmentIssuedDocuments(int AmendmentId)" }, ex);

            }
            return result;
        }

        public List<GetAmendmentHistoryBySocietyId_Result> GetAmendmentHistoryBySocietyId(string SocietyIdENC)
        {
            List<GetAmendmentHistoryBySocietyId_Result> result = new List<GetAmendmentHistoryBySocietyId_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    result = DB.GetAmendmentHistoryBySocietyId(SocietyIdENC).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.Society", FunctionName = "GetAmendmentHistoryBySocietyId(string SocietyIdENC)" }, ex);

            }
            return result;
        }


    }

    public class SocietyDashboard
    {
        public List<GetSocietyCountActWise_Result> SocietyData { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData2001 { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData2005 { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData1958 { get; set; }
    }
}
