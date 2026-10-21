using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Models
{

    public class ComplainUnderSectionModel
    {
        public string ComplainID { get; set; }
        public string SSOID { get; set; }
        public string UserID { get; set; }

        public string SocietyId { get; set; }
        public string ComplantBy { get; set; }
        public string IsComplentForSociety { get; set; }
        [AllowHtml]
        public string ComplaintDetails { get; set; }
        public string StatusId { get; set; }
        public string ComplainDate { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string TXN { get; set; }
        public string ComplainLetter { get; set; }
        public string IOComplainLetter { get; set; }
        public List<ComplainUnderSectionModel> ComplainUnderSectionList { get; set; }

        public List<ComplainantDetails> ComplainantDetailsList { get; set; }
        public List<ComplainantDetails> ComplaintAgainstList { get; set; }
        public List<IODetail> IODetailList { get; set; }
        public List<ComplainTrail> ComplainTrailList { get; set; }
    }



    public class ComplainantDetails
    {
        public string CDetailID { get; set; }
        public string ComplainID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Post { get; set; }
        public string IsSocietyMember { get; set; }
        public string Ctype { get; set; }
        public string IsActive { get; set; }
    }

    public class IODetail
    {
        public string IODetailID { get; set; }
        public string ComplainID { get; set; }
        public string SSOID { get; set; }
        public string UserId { get; set; }
        public string IsActive { get; set; }


        [AllowHtml]
        public string IODetails { get; set; }
    }

    public class ComplainTrail
    {
        public string ComplainTrailID { get; set; }
        public string ComplainID { get; set; }
        public string ComplaintStatusId { get; set; }
        public string FileName { get; set; }
        public string Remark { get; set; }
        public string IsActive { get; set; }
    }



    public class ComplainUnderSection
    {
        private ILogger objILogger1;
        private DBFactoryMSSQL objDBAccess1;
        private DataTable dt1;

        public ComplainUnderSection(ILogger objErrorLogger)
        {
            objILogger1 = objErrorLogger;
            objDBAccess1 = new DBFactoryMSSQL();
            dt1 = new DataTable();
        }

        public ComplainUnderSection()
        {
            objDBAccess1 = new DBFactoryMSSQL();
            dt1 = new DataTable();
        }


        public string InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)
        {
            string Result = "-1";
            DBFactoryMSSQL conn = new DBFactoryMSSQL();

            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            try
            {
                string ComplainID = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


                string complainDate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.ComplainDate))
                {
                    Applicationdatat.ComplainDate = Applicationdatat.ComplainDate.Replace("-", "/");
                    complainDate = Applicationdatat.ComplainDate.Split('/')[2] + "/" + Applicationdatat.ComplainDate.Split('/')[1] + "/" + Applicationdatat.ComplainDate.Split('/')[0];
                }



                Applicationdatat.CreatedBy = Applicationdatat.UpdatedBy = _LoggedInUser.UserId;
                Applicationdatat.CreatedIP = Applicationdatat.UpdatedIP = COMMON_FUNCTION.GetIpAddress();



                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "InsertUpdateComplainUnderSection";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ComplainID", Applicationdatat.ComplainID);
                cmd.Parameters.AddWithValue("@SocietyId", Convert.ToInt32(Applicationdatat.SocietyId)); // Applicationdatat.SocietyId);
                cmd.Parameters.AddWithValue("@ComplantBy", Convert.ToBoolean(Applicationdatat.ComplantBy));
                cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@UserID", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@IsComplentForSociety", Convert.ToBoolean(Applicationdatat.IsComplentForSociety));
                cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(1));//Applicationdatat.StatusId);
                cmd.Parameters.AddWithValue("@ComplainDate", string.IsNullOrWhiteSpace(complainDate) ? "" : complainDate);
                cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                cmd.Parameters.AddWithValue("@CreatedBy", Applicationdatat.CreatedBy);
                //cmd.Parameters.AddWithValue("@CreationDate", "");
                cmd.Parameters.AddWithValue("@UpdatedBy", Applicationdatat.UpdatedBy);
                //cmd.Parameters.AddWithValue("@UpdationDate", "");
                cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);// _LoggedInUser.CreatedIP);
                cmd.Parameters.AddWithValue("@UpdatedIP", Applicationdatat.UpdatedIP);
                cmd.Parameters.AddWithValue("@ComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.ComplainLetter) ? "" : Applicationdatat.ComplainLetter);
                cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
                
                ComplainID = cmd.ExecuteScalar().ToString();
                Applicationdatat.ComplainID = Convert.ToInt32(ComplainID).ToString();
                Result = ComplainID;

                Applicationdatat.ComplainantDetailsList.ForEach(CDetails =>
                {
                    if (!string.IsNullOrWhiteSpace(CDetails.Name))
                    {



                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdateComplainantDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@CDetailID", CDetails.CDetailID);
                        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                        cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
                        cmd.Parameters.AddWithValue("@MobileNo", string.IsNullOrWhiteSpace(CDetails.MobileNo) ? "" : CDetails.MobileNo);
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
                        cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(CDetails.Post) ? "" : CDetails.Post);
                        cmd.Parameters.AddWithValue("@IsSocietyMember", Convert.ToBoolean(CDetails.IsSocietyMember));
                        cmd.Parameters.AddWithValue("@Ctype", Convert.ToBoolean(CDetails.Ctype));
                        cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

                        cmd.ExecuteScalar();
                    }
                });

                Applicationdatat.ComplaintAgainstList.ForEach(CDetails =>
                {
                    if (!string.IsNullOrWhiteSpace(CDetails.Name))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdateComplainantDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@CDetailID", CDetails.CDetailID);
                        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                        cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
                        cmd.Parameters.AddWithValue("@MobileNo", string.IsNullOrWhiteSpace(CDetails.MobileNo) ? "" : CDetails.MobileNo);
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(CDetails.Address) ? "" : CDetails.Address);
                        cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(CDetails.Post) ? "" : CDetails.Post);
                        cmd.Parameters.AddWithValue("@IsSocietyMember", Convert.ToBoolean(CDetails.IsSocietyMember));
                        cmd.Parameters.AddWithValue("@Ctype", Convert.ToBoolean(CDetails.Ctype));
                        cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

                        cmd.ExecuteScalar();
                    }
                });

                //  Applicationdatat.IODetailList = Applicationdatat.IODetailList ?? new List<PLDBLoanAmount>();

                Applicationdatat.IODetailList.ForEach(IODetail =>
                {
                    //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
                    //{
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdateIODetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@IODetailID", IODetail.IODetailID);
                    cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                    cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                    cmd.Parameters.AddWithValue("@UserId", IODetail.UserId);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);
                    cmd.ExecuteScalar();
                    //}
                });

                //   Applicationdatat.ComplainTrailList = Applicationdatat.ComplainTrailList ?? new List<ComplainUnderSectionModel>();
                Applicationdatat.ComplainTrailList.ForEach(ComplainTrail =>
                {
                    if (string.IsNullOrWhiteSpace(ComplainTrail.ComplaintStatusId))
                    {

                        for (int StatusId = 1; StatusId <= 2; StatusId++)
                        {
                            cmd = new SqlCommand();
                            cmd.CommandText = "InsertUpdateComplainTrail";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@ComplainTrailID", ComplainTrail.ComplainTrailID);
                            cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                            cmd.Parameters.AddWithValue("@ComplaintStatusId", Convert.ToInt32(StatusId));
                            cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                            cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                            cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                            cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
                            
                            cmd.ExecuteScalar();
                        }

                    }


                    // if (!string.IsNullOrWhiteSpace(ComplainTrail.Remark))
                    ////  {
                    //      cmd = new SqlCommand();
                    //      cmd.CommandText = "InsertUpdateComplainTrail";
                    //      cmd.CommandType = CommandType.StoredProcedure;
                    //      cmd.Connection = sqlConnection;
                    //      cmd.Transaction = transaction;
                    //      cmd.Parameters.AddWithValue("@ComplainTrailID", ComplainTrail.ComplainTrailID);
                    //      cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                    //      cmd.Parameters.AddWithValue("@ComplaintStatusId", 1);
                    //      cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                    //      cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                    //      cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

                    //      cmd.ExecuteScalar();
                    // }
                });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Result = "-1";
                transaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplainUnderSectionModel", FunctionName = "  InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)" }, ex);
            }
            finally
            {
                sqlConnection.Close();
            }
            return Result;
        }


        public string InsertUpdateIODetail(ComplainUnderSectionModel Applicationdatat)
        {
            string Result = "-1";
            DBFactoryMSSQL conn = new DBFactoryMSSQL();

            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = new SqlCommand();

                Applicationdatat.IODetailList = Applicationdatat.IODetailList ?? new List<IODetail>();

                Applicationdatat.IODetailList.ForEach(IODetail =>
                {
                    //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
                    //{
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdateIODetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@IODetailID", IODetail.IODetailID);
                    cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                    cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                    cmd.Parameters.AddWithValue("@UserId", _LoggedInUser.UserId);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);

                    cmd.ExecuteScalar();
                    //}
                });

                //   Applicationdatat.ComplainTrailList = Applicationdatat.ComplainTrailList ?? new List<ComplainUnderSectionModel>();
                Applicationdatat.ComplainTrailList.ForEach(ComplainTrail =>
                {
                    // if (!string.IsNullOrWhiteSpace(ComplainTrail.Remark))
                    //  {
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdateComplainTrail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@ComplainTrailID", ComplainTrail.ComplainTrailID);
                    cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                    cmd.Parameters.AddWithValue("@ComplaintStatusId", Convert.ToInt32(ComplainTrail.ComplaintStatusId));
                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

                    cmd.ExecuteScalar();
                    // }
                });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Result = "-1";
                transaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplainUnderSectionModel", FunctionName = "  InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)" }, ex);
            }
            finally
            {
                sqlConnection.Close();
            }
            return Result;
        }



        public DataTable GetApprovedSocietyList(ref DataPagingModel Details, string RequestType)
        {
            // DataTable dt = new DataTable();
            dt1.Clear();
            int UserRoleMappingId = 1;
            try
            {
                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {

                    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    if (RequestType == "2")
                    {
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(SocietyName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(D.RegistrationNo))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'EN')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'HI')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(OfficerNameEn))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(OfficerNameHi))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";

                    }
                    else
                    {
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(SocietyName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(D.RegistrationNo))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'EN')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'HI')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";
                    }
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess1.CreateCommand("GetApproveSocietyByUserRoleDeptMappingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess1.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.NVarchar, UserRoleMappingId);
                    objDBAccess1.AddParameterToCommand("@RequestType", DBFactoryParameterType.Int, RequestType);
                    objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess1.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess1.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess1.CreateCommand("GetApproveSocietyByUserRoleDeptMappingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.NVarchar, UserRoleMappingId);
                objDBAccess1.AddParameterToCommand("@RequestType", DBFactoryParameterType.Int, RequestType);
                objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess1.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt1 = objDBAccess1.GetTable();

            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetApprovedSocietyList()" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return dt1;

        }


        public DataSet GetSocietyComplianCourtModuleDetailsForIO()
        {
            DataSet ds = new DataSet();

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess1.OpenConnection();
                SqlCommand cmd = objDBAccess1.CreateCommand("GetSocietyComplianCourtModuleDetailsForIO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                ds = objDBAccess1.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetApprovedSocietyList()" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return ds;

        }

        public DataSet GetComplainDetails(int ComplainID)
        {
            DataSet ds = new DataSet();
          
            try
            {               
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                

                cmd = objDBAccess1.CreateCommand("GetComplainDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@ComplainID", DBFactoryParameterType.Int, ComplainID);
                ds = objDBAccess1.GetDataSet();

            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetComplainDetails("+ ComplainID + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return ds;

        }



        //public class ComplainUnderSectionModel
        //{
        //    public int ComplainID { get; set; }        
        //    public string SSOID { get; set; }
        //    public int UserID { get; set; }

        //    public int SocietyId { get; set; }
        //    public bool ComplantBy { get; set; }
        //    public bool IsComplentForSociety { get; set; }
        //    public string ComplaintDetails { get; set; }
        //    public int StatusId { get; set; }
        //    public DateTime ComplainDate { get; set; }
        //    public int IsActive { get; set; }
        //    public int CreatedBy { get; set; }
        //    public DateTime CreationDate { get; set; }
        //    public int UpdatedBy { get; set; }
        //    public DateTime UpdationDate { get; set; }
        //    public string CreatedIP { get; set; }
        //    public string UpdatedIP { get; set; }

        //    public List<ComplainUnderSectionModel> ComplainUnderSectionList { get; set; }
        //    public List<ComplainantDetails> ComplainantDetailsList { get; set; }
        //    public List<IODetail> IODetailList { get; set; }
        //    public List<ComplainTrail> ComplainTrailList { get; set; }
        //}



        //public class ComplainantDetails
        //{
        //    public int CDetailID { get; set; }
        //    public int ComplainID { get; set; }
        //    public string Name { get; set; }
        //    public string MobileNo { get; set; }
        //    public string Address { get; set; }
        //    public string Post { get; set; }
        //    public bool IsSocietyMember { get; set; }
        //    public bool Ctype { get; set; }
        //    public int IsActive { get; set; }
        //}

        //public class IODetail
        //{
        //    public int IODetailID { get; set; }
        //    public int ComplainID { get; set; }
        //    public string SSOID { get; set; }
        //    public int IsActive { get; set; }
        //}

        //public class ComplainTrail
        //{
        //    public int ComplainTrailID { get; set; }
        //    public int ComplainID { get; set; }
        //    public int ComplaintStatusId { get; set; }
        //    public string FileName { get; set; }
        //    public string Remark { get; set; }
        //    public int IsActive { get; set; }
        //}

    }
}