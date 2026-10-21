using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class FinancialHealthModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        public FinancialHealthModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public FinancialHealthModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public DataTable GetSocietyBasicDetailForStatus(ref DataPagingModel Details, int SearchType = 0)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";
                //if (SearchType != 0)
                //{
                //    if (SearchType == 3)
                //    {
                //        whereCase += " and Societies.RegistrationNo like 'MSCS%' ";
                //    }
                //    else if (SearchType ==2)
                //    {
                //        whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                //    }
                //}
                if (_loggedInUSerDetails.LevelId == 3)
                {
                    if (SearchType == 3)
                    {
                        whereCase += "and ( Societies.RegistrationNo  like 'MSCS%'   ) ";
                    }
                    else if (SearchType == 2)
                    {
                        whereCase += "and ( Societies.RegistrationNo not like 'MSCS%'  and  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + " ) ";
                    }
                    else 
                    {
                       // whereCase += "and  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + "  ";
                        whereCase += "and ( Societies.RegistrationNo  like 'MSCS%'  or  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + " ) ";
                    }
                }
                else if (SearchType != 0)
                {
                    if (SearchType == 3)
                    {
                        whereCase += " and Societies.RegistrationNo like 'MSCS%' ";
                    }
                    else if (SearchType == 2)
                    {
                        whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                    }
                }


                //if (Details.SearchParameter.Keys.Contains("rdoSelect#2"))
                if(Details.SearchParameter.Where(x => x.Key == "rdoSelect" && x.Value == "2").Any())
                {
                    whereCase += " AND CASE  WHEN (SELECT Count(1) from SocietyFinancialHealth where SocietyFinancialHealth.Societyid=Societies.Societyid) > 0 THEN 1 ELSE 0 END = 1";
                }
                //else if (Details.SearchParameter.Keys.Contains("rdoSelect#1") || Details.SearchParameter.Count==0)
                else if (Details.SearchParameter.Where(x => x.Key == "rdoSelect" && x.Value == "1").Any() || Details.SearchParameter.Count == 0)
                {
                    whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                }
                string searchText = "";
                if (Details.SearchParameter.Count > 0)
                {
                    if (Details.SearchParameter.Keys.Contains("searchText"))
                    {
                        searchText = Details.SearchParameter["searchText"];
                        //var searchValue = Details.SearchParameter["searchText"];// Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        //whereCase += " and (Societies.RegistrationNo  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        //whereCase += " OR Societies.SocietyName  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";
                    }

                    //if (Details.SearchParameter.Keys.Contains("SetStatusType#7") || Details.SearchParameter.Keys.Contains("SetStatusType#8") || Details.SearchParameter.Keys.Contains("SetStatusType#9") || Details.SearchParameter.Keys.Contains("SetStatusType#10") || Details.SearchParameter.Keys.Contains("SetStatusType#11"))
                   if (Details.SearchParameter.Where(x => x.Key == "SetStatusType").Any())// && (x.Value == "7" || x.Value == "8" || x.Value == "9" || x.Value == "10" || x.Value == "11")).Any())
                   {
                        //var searchValue = "0";
                        //if (Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "7").Any())
                        //{
                        //    searchValue = Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "7").Select(x => x.Value).FirstOrDefault(); //Details.SearchParameter["SetStatusType#7"];
                        //}
                        //else if(Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "8").Any())
                        //{
                        //     searchValue = Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "8").Select(x => x.Value).FirstOrDefault(); //Details.SearchParameter["SetStatusType#8"];
                        //}
                        //else if (Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "9").Any())
                        //{
                        //     searchValue = Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "9").Select(x => x.Value).FirstOrDefault();// Details.SearchParameter["SetStatusType#9"];
                        //}
                        //else if (Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "10").Any())
                        //{
                        //     searchValue = Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "10").Select(x => x.Value).FirstOrDefault(); //Details.SearchParameter["SetStatusType#10"];
                        //}
                        //else 
                        //{
                        //     searchValue = Details.SearchParameter.Where(x => x.Key == "SetStatusType" && x.Value == "11").Select(x => x.Value).FirstOrDefault(); //Details.SearchParameter["SetStatusType#11"];
                        //}

                        whereCase += "and Societies.IsActive=" + Details.SearchParameter.Where(x => x.Key == "SetStatusType").Select(x=>x.Value).FirstOrDefault();

                    }

                    if (Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").Any())
                    {
                        whereCase += " and Societies.SocietyTypeId=  " + Details.SearchParameter.Where(x => x.Key == "SocietyTypeId").Select(x => x.Value).FirstOrDefault();
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "ClassId").Any())
                    {
                        whereCase += " and Societies.ClassId=  " + Details.SearchParameter.Where(x => x.Key == "ClassId").Select(x => x.Value).FirstOrDefault();
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "SubClassId").Any())
                    {
                        whereCase += " and Societies.SubClassId=  " + Details.SearchParameter.Where(x => x.Key == "SubClassId").Select(x => x.Value).FirstOrDefault();
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "CategoryId").Any())
                    {
                        whereCase += " and Societies.SocietyCategoryId=  " + Details.SearchParameter.Where(x => x.Key == "CategoryId").Select(x => x.Value).FirstOrDefault();
                    }
                }

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyBasicDetailNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@UserMappingId", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                cmd = objDBAccess.CreateCommand("GetSocietyBasicDetailNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@UserMappingId", DBFactoryParameterType.NVarchar, _loggedInUSerDetails.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);

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

        public DataTable GetSocietyBasicDetail(ref DataPagingModel Details, int SearchType = 0)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = " ";
                //if (SearchType != 0)
                //{
                //    if (SearchType == 3)
                //    {
                //        whereCase += " and Societies.RegistrationNo like 'MSCS%' ";
                //    }
                //    else if (SearchType ==2)
                //    {
                //        whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                //    }
                //}
                if (_loggedInUSerDetails.LevelId == 3)
                {
                    if (SearchType == 3)
                    {
                        whereCase += "and ( Societies.RegistrationNo  like 'MSCS%'   ) ";
                    }
                    else if (SearchType == 2)
                    {
                        whereCase += "and ( Societies.RegistrationNo not like 'MSCS%'  and  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + " ) ";
                    }
                    else
                    {
                        // whereCase += "and  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + "  ";
                        whereCase += "and ( Societies.RegistrationNo  like 'MSCS%'  or  SocietyRegOfficeDetails.DISTRICT_ID=" + _loggedInUSerDetails.DistrictId + " ) ";
                    }
                }
                else if (SearchType != 0)
                {
                    if (SearchType == 3)
                    {
                        whereCase += " and Societies.RegistrationNo like 'MSCS%' ";
                    }
                    else if (SearchType == 2)
                    {
                        whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                    }
                }


                if (Details.SearchParameter.Keys.Contains("rdoSelect#2"))
                {
                    whereCase += " AND CASE  WHEN (SELECT Count(1) from SocietyFinancialHealth where SocietyFinancialHealth.Societyid=Societies.Societyid) > 0 THEN 1 ELSE 0 END = 1";
                }
                else if (Details.SearchParameter.Keys.Contains("rdoSelect#1") || Details.SearchParameter.Count == 0)
                {
                    whereCase += " and Societies.RegistrationNo not like 'MSCS%' ";
                }

                if (Details.SearchParameter.Count > 0)
                {
                    if (Details.SearchParameter.Keys.Contains("searchText"))
                    {
                        var searchValue = Details.SearchParameter["searchText"];// Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and (Societies.RegistrationNo  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " OR Societies.SocietyName  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";
                    }
                    if (Details.SearchParameter.Keys.Contains("SetStatusType#7") || Details.SearchParameter.Keys.Contains("SetStatusType#8") || Details.SearchParameter.Keys.Contains("SetStatusType#9") || Details.SearchParameter.Keys.Contains("SetStatusType#10") || Details.SearchParameter.Keys.Contains("SetStatusType#11"))
                    {
                        var searchValue = "0";
                        if (Details.SearchParameter.Keys.Contains("SetStatusType#7"))
                        {
                            searchValue = Details.SearchParameter["SetStatusType#7"];
                        }
                        else if (Details.SearchParameter.Keys.Contains("SetStatusType#8"))
                        {
                            searchValue = Details.SearchParameter["SetStatusType#8"];
                        }
                        else if (Details.SearchParameter.Keys.Contains("SetStatusType#9"))
                        {
                            searchValue = Details.SearchParameter["SetStatusType#9"];
                        }
                        else if (Details.SearchParameter.Keys.Contains("SetStatusType#10"))
                        {
                            searchValue = Details.SearchParameter["SetStatusType#10"];
                        }
                        else
                        {
                            searchValue = Details.SearchParameter["SetStatusType#11"];
                        }

                        whereCase += "and Societies.IsActive=" + searchValue;

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



        public int InsertFinancialHealthData(SocietyFinancialHealthEntity model) // SocietyFinancialHealth
        {
            int Result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                int SuccessId = 0;

                cmd = objDBAccess.CreateCommand("CEODetailsInsert", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, model.Id);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, model.SocietyId);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, model.Name);
                objDBAccess.AddParameterToCommand("@AADHARNoEnc", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.Encrypt(model.AADHARNo));
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.VARCHAR, model.AADHARNo);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.VARCHAR, model.FatherName);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.VARCHAR, model.DOB);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, model.CategoryId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.VARCHAR, model.Address);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, model.MobileNo);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, model.CreateIP);

                objDBAccess.AddParameterToCommand("@BankName", DBFactoryParameterType.VARCHAR, model.BankName);
                objDBAccess.AddParameterToCommand("@BankAccountNo", DBFactoryParameterType.VARCHAR, model.BankAccountNo);
                objDBAccess.AddParameterToCommand("@BankIFSCNo", DBFactoryParameterType.VARCHAR, model.BankIFSCNo);
                objDBAccess.AddParameterToCommand("@BalanceInAccount", DBFactoryParameterType.VARCHAR, model.BalanceInAccount);
                objDBAccess.AddParameterToCommand("@FDBalanceInAccount", DBFactoryParameterType.VARCHAR, model.FDBalanceInAccount);


                SuccessId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //if (SuccessId < 1)
                //    return SuccessId;

                int NiveshId = 0;
                model.AssetList.AddRange(model.LiabilityList.AsEnumerable()); // Combine both Asset and liability in Single colection

                foreach (var item in model.AssetList)
                {
                    cmd = objDBAccess.CreateCommand("SocietyFinancialHealthInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@AssetLiabilityId", DBFactoryParameterType.Int, item.AssetLiabilityId);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, model.SocietyId);
                    objDBAccess.AddParameterToCommand("@Amount", DBFactoryParameterType.Money, item.Amount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, model.CreateIP);

                    objDBAccess.AddParameterToCommand("@IsAccountInCCB", DBFactoryParameterType.Bit, model.IsAccountInCCB);
                    objDBAccess.AddParameterToCommand("@IsInvestedInCCB", DBFactoryParameterType.Bit, model.IsInvestedInCCB);
                    objDBAccess.AddParameterToCommand("@InvestmentAmountInCCB", DBFactoryParameterType.Int, model.InvestmentAmountInCCB);

                    SuccessId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                    if (Convert.ToInt32(model.InvestmentDetails.First().SFinancialHealthId) == item.AssetLiabilityId)
                    {
                        NiveshId = SuccessId;
                    }
                }

                foreach (var item in model.InvestmentDetails)
                {
                    cmd = objDBAccess.CreateCommand("InvestmentDetailsInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SFinancialHealthId", DBFactoryParameterType.Int, NiveshId);
                    objDBAccess.AddParameterToCommand("@WhereDid", DBFactoryParameterType.NVarchar, item.WhereDid);
                    objDBAccess.AddParameterToCommand("@Amount", DBFactoryParameterType.Money, item.Amount);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, model.CreateIP);
                    SuccessId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                Result = SuccessId;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FinancialHealthModel", FunctionName = " public string InsertFinancialHealthData(SocietyFinancialHealthEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyHealthCEODetails(string SocietyIdEnc)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyHealthCEODetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FinancialHealthModel", FunctionName = "public DataTable GetSocietyHealthCEODetails(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAllSSO()
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetAllSSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FinancialHealthModel", FunctionName = "public DataTable GetSocietyHealthCEODetails(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable UpdateSSODetails(string SSOID, string NAME, string EMAILID, string MOBILENO)
        {
            dt.Clear();
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("UpdateSSODetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, SSOID);
                objDBAccess.AddParameterToCommand("@NAME", DBFactoryParameterType.VARCHAR, NAME);
                objDBAccess.AddParameterToCommand("@EMAILID", DBFactoryParameterType.VARCHAR, EMAILID);
                objDBAccess.AddParameterToCommand("@MOBILENO", DBFactoryParameterType.VARCHAR, MOBILENO);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FinancialHealthModel", FunctionName = "public DataTable GetSocietyHealthCEODetails(string SocietyIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
    


    

}

    //public class SocietyFinancialHealth
    //{
    //    public int load_id { get; set; }
    //    public int application_no { get; set; }
    //    public int pldb_id { get; set; }
    //    public int branch_id { get; set; }
    //    public int createdby { get; set; }
    //    public List<AssetLiabilityData> AssetLiabilityList { get; set; }
    //    public List<CeoData> CEOList { get; set; }
    //    public List<InvestmentDetails> InvestmentDetailsList { get; set; }
    //}

    //public class AssetLiabilityData
    //{
    //    public int Id { get; set; }
    //    public int AssetLiabilityId { get; set; }
    //    public decimal Amount { get; set; }
    //}

    //public class CeoData
    //{
    //    public int AdministrativeId { get; set; }
    //    public string Name { get; set; }
    //    public string FatherName { get; set; }
    //    public string AADHARNo { get; set; }
    //    public DateTime DOB { get; set; }
    //    public int CategoryId { get; set; }
    //    public string Address { get; set; }
    //    public string MobileNo { get; set; }
    //}

    //public class InvestmentDetails
    //{
    //    public int InvestmentId { get; set; }
    //    public string SFinancialHealthId { get; set; }
    //    public string WhereDid { get; set; }
    //    public decimal Amount { get; set; }
    //}


}