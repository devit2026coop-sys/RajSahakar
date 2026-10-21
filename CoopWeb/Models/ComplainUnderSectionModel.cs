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
        public string SocietyName { get; set; }

        public int ComplantBystatus { get; set; }
        public bool Investigation1 { get; set; }

        public string Investigation1date { get; set; }
        public string Investigation1todate { get; set; }

        [AllowHtml]
        public string Investigation1Detilas { get; set; }

        public bool Investigation2 { get; set; }
        public string Investigation2date { get; set; }
        public string Investigation2todate { get; set; }

        [AllowHtml]
        public string Investigation2Detilas { get; set; }

        public bool Investigation3 { get; set; }
        public string Investigation3date { get; set; }
        public string Investigation3todate { get; set; }
        [AllowHtml]
        public string Investigation3Detilas { get; set; }


        public int IOType { get; set; }

        public string EncloserFileName { get; set; }

        public string EncloserFileName2 { get; set; }
        public string Scheme { get; set; }
        public List<ComplainUnderSectionModel> ComplainUnderSectionList { get; set; }

        public List<ComplainantDetails> ComplainantDetailsList { get; set; }
        public List<ComplainantDetails> ComplaintAgainstList { get; set; }
        public List<ComplainantPersonDetailsList> PersonDetailsList { get; set; }
        public List<IODetail> IODetailList { get; set; }
        public List<ComplainTrail> ComplainTrailList { get; set; }
        public string AcceptAndPublishReport { get; set; }
        [AllowHtml]
        public string ComplaintDetailsForAcceptAndPublish { get; set; }


        
        public bool OtherMatter { get; set; }
        public string OtherDescription { get; set; }
        public string Investigation4date { get; set; }
        public string Investigation4todate { get; set; }
        public string SerialNo { get; set; }

        public string ApplicantLetterSerialNo { get; set; }
        public string ApplicantLetterDate { get; set; }
        public string ApplicantSocietyName { get; set; }
        public string ApplicantSocietyId { get; set; }
        public string ApplicantMembers { get; set; }

        [AllowHtml]
        public string ResultForFormation { get; set; }
        [AllowHtml]
        public string ResultForBusiness { get; set; }
        [AllowHtml]
        public string ResultForFinancial { get; set; }
        [AllowHtml]
        public string ResultForOtherEnquiry { get; set; }
        [AllowHtml]
        public string ResultOther { get; set; }

        public int IsOld { get; set; }

        
    }



    public class ComplainantPersonDetailsList
    {
        public string CPersonId { get; set; }
        public string ComplainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Post { get; set; }
        public string Mobile { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedIP { get; set; }
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

        public int Is_Old_Status { get; set; }
    }

    public class IODetail
    {
        public string IODetailID { get; set; }
        public string ComplainID { get; set; }
        public string SSOID { get; set; }
        public string UserId { get; set; }
        public string IsActive { get; set; }
        public string OfficerNameEn { get; set; }
        public string OfficerNameHi { get; set; }
        public string Period { get; set; }
        public string Places { get; set; }
        public string DateTime { get; set; }
        [AllowHtml]
        public string IODetails { get; set; }

        public string Designation { get; set; }
        
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

        //balwanth sir
        //public string InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)
        //{
        //    string Result = "-1";
        //    DBFactoryMSSQL conn = new DBFactoryMSSQL();

        //    SqlConnection sqlConnection = conn.GetConnection;
        //    SqlTransaction transaction;
        //    sqlConnection.Open();
        //    transaction = sqlConnection.BeginTransaction();

        //    try
        //    {
        //        string ComplainID = "";
        //        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


        //        string complainDate = "";
        //        if (!string.IsNullOrWhiteSpace(Applicationdatat.ComplainDate))
        //        {
        //            Applicationdatat.ComplainDate = Applicationdatat.ComplainDate.Replace("-", "/");
        //            complainDate = Applicationdatat.ComplainDate.Split('/')[2] + "/" + Applicationdatat.ComplainDate.Split('/')[1] + "/" + Applicationdatat.ComplainDate.Split('/')[0];
        //        }



        //        Applicationdatat.CreatedBy = Applicationdatat.UpdatedBy = _LoggedInUser.UserId;
        //        Applicationdatat.CreatedIP = Applicationdatat.UpdatedIP = COMMON_FUNCTION.GetIpAddress();



        //        SqlCommand cmd = new SqlCommand();
        //        string fileName = Guid.NewGuid().ToString() + ".pdf";
        //        cmd.CommandText = "InsertUpdateComplainUnderSection";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Connection = sqlConnection;
        //        cmd.Transaction = transaction;
        //        cmd.Parameters.AddWithValue("@ComplainID", Applicationdatat.ComplainID);
        //        cmd.Parameters.AddWithValue("@SocietyId", Convert.ToInt32(Applicationdatat.SocietyId)); // Applicationdatat.SocietyId);
        //        cmd.Parameters.AddWithValue("@ComplantBy", Convert.ToBoolean(Applicationdatat.ComplantBy));
        //        cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
        //        cmd.Parameters.AddWithValue("@UserID", _LoggedInUser.UserId);
        //        cmd.Parameters.AddWithValue("@IsComplentForSociety", Convert.ToBoolean(Applicationdatat.IsComplentForSociety));
        //        cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
        //        cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(1));//Applicationdatat.StatusId);
        //        cmd.Parameters.AddWithValue("@ComplainDate", string.IsNullOrWhiteSpace(complainDate) ? "" : complainDate);
        //        cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
        //        cmd.Parameters.AddWithValue("@CreatedBy", Applicationdatat.CreatedBy);
        //        //cmd.Parameters.AddWithValue("@CreationDate", "");
        //        cmd.Parameters.AddWithValue("@UpdatedBy", Applicationdatat.UpdatedBy);
        //        //cmd.Parameters.AddWithValue("@UpdationDate", "");
        //        cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);// _LoggedInUser.CreatedIP);
        //        cmd.Parameters.AddWithValue("@UpdatedIP", Applicationdatat.UpdatedIP);
        //        cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
        //        cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
        //        cmd.Parameters.AddWithValue("@YognaName", string.IsNullOrWhiteSpace(Applicationdatat.Scheme) ? "" : Applicationdatat.Scheme);

        //        ComplainID = cmd.ExecuteScalar().ToString();

        //        Applicationdatat.ComplainID = Convert.ToInt32(ComplainID).ToString();
        //        Result = ComplainID;

        //        Applicationdatat.ComplainantDetailsList.ForEach(CDetails =>
        //        {
        //            if (!string.IsNullOrWhiteSpace(CDetails.Name))
        //            {



        //                cmd = new SqlCommand();
        //                cmd.CommandText = "InsertUpdateComplainantDetails";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Connection = sqlConnection;
        //                cmd.Transaction = transaction;
        //                cmd.Parameters.AddWithValue("@CDetailID", CDetails.CDetailID);
        //                cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
        //                cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
        //                cmd.Parameters.AddWithValue("@MobileNo", string.IsNullOrWhiteSpace(CDetails.MobileNo) ? "" : CDetails.MobileNo);
        //                cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
        //                cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(CDetails.Post) ? "" : CDetails.Post);
        //                cmd.Parameters.AddWithValue("@IsSocietyMember", Convert.ToBoolean(CDetails.IsSocietyMember));
        //                cmd.Parameters.AddWithValue("@Ctype", Convert.ToBoolean(CDetails.Ctype));
        //                cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

        //                cmd.ExecuteScalar();
        //            }
        //        });

        //        Applicationdatat.ComplaintAgainstList.ForEach(CDetails =>
        //        {
        //            if (!string.IsNullOrWhiteSpace(CDetails.Name))
        //            {
        //                cmd = new SqlCommand();
        //                cmd.CommandText = "InsertUpdateComplainantDetails";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Connection = sqlConnection;
        //                cmd.Transaction = transaction;
        //                cmd.Parameters.AddWithValue("@CDetailID", CDetails.CDetailID);
        //                cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
        //                cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
        //                cmd.Parameters.AddWithValue("@MobileNo", string.IsNullOrWhiteSpace(CDetails.MobileNo) ? "" : CDetails.MobileNo);
        //                cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(CDetails.Address) ? "" : CDetails.Address);
        //                cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(CDetails.Post) ? "" : CDetails.Post);
        //                cmd.Parameters.AddWithValue("@IsSocietyMember", Convert.ToBoolean(CDetails.IsSocietyMember));
        //                cmd.Parameters.AddWithValue("@Ctype", Convert.ToBoolean(CDetails.Ctype));
        //                cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

        //                cmd.ExecuteScalar();
        //            }
        //        });

        //        //  Applicationdatat.IODetailList = Applicationdatat.IODetailList ?? new List<PLDBLoanAmount>();
        //        string SerialNo = "";
        //        Applicationdatat.IODetailList.ForEach(IODetail =>
        //        {

        //            string complainDateDR = "";
        //            if (!string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].DateTime))
        //            {
        //                string date = Applicationdatat.IODetailList[0].DateTime.Split(' ')[0];
        //                date = date.Replace("-", "/");
        //                complainDateDR = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[1] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[2];
        //            }
        //            //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
        //            //{
        //            cmd = new SqlCommand();
        //            cmd.CommandText = "InsertUpdateIODetail";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Connection = sqlConnection;
        //            cmd.Transaction = transaction;
        //            cmd.Parameters.AddWithValue("@IODetailID", IODetail.IODetailID);
        //            cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
        //            cmd.Parameters.AddWithValue("@SSOID", IODetail.SSOID);
        //            cmd.Parameters.AddWithValue("@UserId", IODetail.UserId);
        //            cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
        //            cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
        //            cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);
        //            cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(IODetail.Period) ? "" : IODetail.Period);
        //            cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");

        //            cmd.Parameters.AddWithValue("@DateTime", string.IsNullOrWhiteSpace(complainDateDR) ? "" : complainDateDR);
        //            cmd.Parameters.AddWithValue("@Place", string.IsNullOrWhiteSpace(IODetail.Places) ? "" : IODetail.Places);
        //            SerialNo = cmd.ExecuteScalar().ToString();
        //            //}
        //        });

        //        //   Applicationdatat.ComplainTrailList = Applicationdatat.ComplainTrailList ?? new List<ComplainUnderSectionModel>();
        //        Applicationdatat.ComplainTrailList.ForEach(ComplainTrail =>
        //        {
        //            if (string.IsNullOrWhiteSpace(ComplainTrail.ComplaintStatusId))
        //            {

        //                for (int StatusId = 1; StatusId <= 2; StatusId++)
        //                {
        //                    cmd = new SqlCommand();
        //                    cmd.CommandText = "InsertUpdateComplainTrail";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Connection = sqlConnection;
        //                    cmd.Transaction = transaction;

        //                    cmd.Parameters.AddWithValue("@ComplainTrailID", ComplainTrail.ComplainTrailID);
        //                    cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
        //                    cmd.Parameters.AddWithValue("@ComplaintStatusId", Convert.ToInt32(StatusId));
        //                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
        //                    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
        //                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
        //                    cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
        //                    cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
        //                    cmd.Parameters.AddWithValue("@IOComplainLetter", "");
        //                    cmd.Parameters.AddWithValue("@IODetails", "");

        //                    cmd.ExecuteScalar();
        //                }

        //            }

        //        });

        //        var result = GenerateComplainLetter(Applicationdatat, SerialNo, fileName);
        //        if (result.status == "1")
        //        {
        //            Result = "1";
        //            transaction.Commit();
        //        }
        //        else
        //        {
        //            Result = "-1";
        //            transaction.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = "-1";
        //        transaction.Rollback();
        //        DBLogger objDBLogger = new DBLogger();
        //        objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplainUnderSectionModel", FunctionName = "  InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)" }, ex);
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //    return Result;
        //}



        public string InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)
        {

            //bhupendra
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


                string Investigation1date = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation1date))
                {
                    Applicationdatat.Investigation1date = Applicationdatat.Investigation1date.Replace("-", "/");
                    Investigation1date = Applicationdatat.Investigation1date.Split('/')[2] + "/" + Applicationdatat.Investigation1date.Split('/')[1] + "/" + Applicationdatat.Investigation1date.Split('/')[0];
                }

                string Investigation1todate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation1todate))
                {
                    Applicationdatat.Investigation1todate = Applicationdatat.Investigation1todate.Replace("-", "/");
                    Investigation1todate = Applicationdatat.Investigation1todate.Split('/')[2] + "/" + Applicationdatat.Investigation1todate.Split('/')[1] + "/" + Applicationdatat.Investigation1todate.Split('/')[0];
                }

                string Investigation2date = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation2date))
                {
                    Applicationdatat.Investigation2date = Applicationdatat.Investigation2date.Replace("-", "/");
                    Investigation2date = Applicationdatat.Investigation2date.Split('/')[2] + "/" + Applicationdatat.Investigation2date.Split('/')[1] + "/" + Applicationdatat.Investigation2date.Split('/')[0];
                }

                string Investigation2todate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation2todate))
                {
                    Applicationdatat.Investigation2todate = Applicationdatat.Investigation2todate.Replace("-", "/");
                    Investigation2todate = Applicationdatat.Investigation2todate.Split('/')[2] + "/" + Applicationdatat.Investigation2todate.Split('/')[1] + "/" + Applicationdatat.Investigation2todate.Split('/')[0];
                }

                string Investigation3date = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation3date))
                {
                    Applicationdatat.Investigation3date = Applicationdatat.Investigation3date.Replace("-", "/");
                    Investigation3date = Applicationdatat.Investigation3date.Split('/')[2] + "/" + Applicationdatat.Investigation3date.Split('/')[1] + "/" + Applicationdatat.Investigation3date.Split('/')[0];
                }

                string Investigation3todate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation3todate))
                {
                    Applicationdatat.Investigation3todate = Applicationdatat.Investigation3todate.Replace("-", "/");
                    Investigation3todate = Applicationdatat.Investigation3todate.Split('/')[2] + "/" + Applicationdatat.Investigation3todate.Split('/')[1] + "/" + Applicationdatat.Investigation3todate.Split('/')[0];
                }

                string Investigation4date = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation4date))
                {
                    Applicationdatat.Investigation4date = Applicationdatat.Investigation4date.Replace("-", "/");
                    Investigation4date = Applicationdatat.Investigation4date.Split('/')[2] + "/" + Applicationdatat.Investigation4date.Split('/')[1] + "/" + Applicationdatat.Investigation4date.Split('/')[0];
                }

                string Investigation4todate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation4todate))
                {
                    Applicationdatat.Investigation4todate = Applicationdatat.Investigation4todate.Replace("-", "/");
                    Investigation4todate = Applicationdatat.Investigation4todate.Split('/')[2] + "/" + Applicationdatat.Investigation4todate.Split('/')[1] + "/" + Applicationdatat.Investigation4todate.Split('/')[0];
                }

                string ApplicantLetterDate = "";
                if (!string.IsNullOrWhiteSpace(Applicationdatat.ApplicantLetterDate))
                {
                    Applicationdatat.ApplicantLetterDate = Applicationdatat.ApplicantLetterDate.Replace("-", "/");
                    ApplicantLetterDate = Applicationdatat.ApplicantLetterDate.Split('/')[2] + "/" + Applicationdatat.ApplicantLetterDate.Split('/')[1] + "/" + Applicationdatat.ApplicantLetterDate.Split('/')[0];
                }


                if (!Applicationdatat.OtherMatter)
                {
                    Applicationdatat.OtherDescription = "";
                }


                if (Applicationdatat.Investigation1Detilas == null)
                {
                    Applicationdatat.Investigation1Detilas = "";
                }
                if (Applicationdatat.Investigation2Detilas == null)
                {
                    Applicationdatat.Investigation2Detilas = "";
                }

                if (Applicationdatat.Investigation3Detilas == null)
                {
                    Applicationdatat.Investigation3Detilas = "";
                }

                Applicationdatat.CreatedBy = Applicationdatat.UpdatedBy = _LoggedInUser.UserId;
                Applicationdatat.CreatedIP = Applicationdatat.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
                string LetterNo = "";
                SqlCommand cmd = new SqlCommand();
                //int ComplainID = 0;
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                cmd.CommandText = "InsertUpdateComplainUnderSection";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ComplainID", Applicationdatat.ComplainID);
                cmd.Parameters.AddWithValue("@SocietyId", Convert.ToInt32(Applicationdatat.SocietyId)); // Applicationdatat.SocietyId);
                cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@UserID", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@IsComplentForSociety", Convert.ToBoolean(Applicationdatat.IsComplentForSociety));
                cmd.Parameters.AddWithValue("@ComplaintDetails", "fgfg");

                // if (IsNew != 1)
                //{


                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(Applicationdatat.StatusId));
                cmd.Parameters.AddWithValue("@IsOld", Convert.ToInt32(Applicationdatat.IsOld));

                // var StatusId = Convert.ToInt32(Applicationdatat.StatusId);

                //  }
                //  else
                //  {
                //  cmd.Parameters.AddWithValue("@StatusId", 2);
                //  }
                // cmd.Parameters.AddWithValue("@StatusId", ComplainUnderSection.OldStatus);
                //cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(2));//bj
                cmd.Parameters.AddWithValue("@ComplainDate", string.IsNullOrWhiteSpace(complainDate) ? "" : complainDate);
                cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                cmd.Parameters.AddWithValue("@UpdatedBy", _LoggedInUser.UserRoleDeptMappingId);
                cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.UpdatedIP);// 
                cmd.Parameters.AddWithValue("@UpdatedIP", Applicationdatat.UpdatedIP);
                cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
                cmd.Parameters.AddWithValue("@YognaName", string.IsNullOrWhiteSpace(Applicationdatat.Scheme) ? "" : Applicationdatat.Scheme);

                cmd.Parameters.AddWithValue("@Investigation1", Convert.ToBoolean(Applicationdatat.Investigation1));
                cmd.Parameters.AddWithValue("@Investigation2", Convert.ToBoolean(Applicationdatat.Investigation2));
                cmd.Parameters.AddWithValue("@Investigation3", Convert.ToBoolean(Applicationdatat.Investigation3));
                cmd.Parameters.AddWithValue("@OtherMatter", Convert.ToBoolean(Applicationdatat.OtherMatter));
                cmd.Parameters.AddWithValue("@Investigation4date", string.IsNullOrWhiteSpace(Investigation4date) ? "" : Investigation4date);
                cmd.Parameters.AddWithValue("@Investigation4todate", string.IsNullOrWhiteSpace(Investigation4todate) ? "" : Investigation4todate);


                cmd.Parameters.AddWithValue("@Investigation1date", string.IsNullOrWhiteSpace(Investigation1date) ? "" : Investigation1date);
                cmd.Parameters.AddWithValue("@Investigation1todate", string.IsNullOrWhiteSpace(Investigation1todate) ? "" : Investigation1todate);
                cmd.Parameters.AddWithValue("@Investigation1Detilas", Applicationdatat.Investigation1Detilas);

                cmd.Parameters.AddWithValue("@Investigation2date", string.IsNullOrWhiteSpace(Investigation2date) ? "" : Investigation2date);
                cmd.Parameters.AddWithValue("@Investigation2todate", string.IsNullOrWhiteSpace(Investigation2todate) ? "" : Investigation2todate);
                cmd.Parameters.AddWithValue("@Investigation2Detilas", Applicationdatat.Investigation2Detilas);

                cmd.Parameters.AddWithValue("@Investigation3date", string.IsNullOrWhiteSpace(Investigation3date) ? "" : Investigation3date);
                cmd.Parameters.AddWithValue("@Investigation3todate", string.IsNullOrWhiteSpace(Investigation3todate) ? "" : Investigation3todate);
                cmd.Parameters.AddWithValue("@Investigation3Detilas", Applicationdatat.Investigation3Detilas);
                cmd.Parameters.AddWithValue("@ComplantBystatus", Applicationdatat.ComplantBystatus);
                cmd.Parameters.AddWithValue("@EncloserFileName", string.IsNullOrWhiteSpace(Applicationdatat.EncloserFileName) ? "" : Applicationdatat.EncloserFileName);


                cmd.Parameters.AddWithValue("@OtherDescription", string.IsNullOrWhiteSpace(Applicationdatat.OtherDescription) ? "" : Applicationdatat.OtherDescription);
                cmd.Parameters.AddWithValue("@IOType", Applicationdatat.IOType);

                cmd.Parameters.AddWithValue("@ApplicantLetterSerialNo", string.IsNullOrWhiteSpace(Applicationdatat.ApplicantLetterSerialNo) ? "" : Applicationdatat.ApplicantLetterSerialNo);
                cmd.Parameters.AddWithValue("@ApplicantLetterDate", string.IsNullOrWhiteSpace(ApplicantLetterDate) ? "" : ApplicantLetterDate);
                cmd.Parameters.AddWithValue("@ApplicantSocietyId", string.IsNullOrWhiteSpace(Applicationdatat.ApplicantSocietyId) ? "0" : Applicationdatat.ApplicantSocietyId);
                cmd.Parameters.AddWithValue("@ApplicantMembers", string.IsNullOrWhiteSpace(Applicationdatat.ApplicantMembers) ? "0" : Applicationdatat.ApplicantMembers);



                ComplainID = cmd.ExecuteScalar().ToString();

                Applicationdatat.ComplainID = Convert.ToInt32(ComplainID).ToString();
                Result = ComplainID;


                //bhu

                Applicationdatat.PersonDetailsList.ForEach(PersonDetailsList =>
                {
                    if (!string.IsNullOrWhiteSpace(PersonDetailsList.Name))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdateUnderPersonDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@PersonId", PersonDetailsList.CPersonId);
                        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                        cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(PersonDetailsList.Name) ? "" : PersonDetailsList.Name);
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(PersonDetailsList.Address) ? "" : PersonDetailsList.Address);
                        cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(PersonDetailsList.Post) ? "" : PersonDetailsList.Post);
                        cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                        cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);
                        cmd.ExecuteScalar();
                    }
                });
                //bhu
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


                        //cmd.Parameters.AddWithValue("@Is_Old_Status", Convert.ToInt32(CDetails.Is_Old_Status);
                        cmd.ExecuteScalar();
                    }
                });

                //Applicationdatat.ComplaintAgainstList.ForEach(CDetails =>
                //{
                //    if (!string.IsNullOrWhiteSpace(CDetails.Name))
                //    {
                //        cmd = new SqlCommand();
                //        cmd.CommandText = "InsertUpdateComplainantDetails";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Connection = sqlConnection;
                //        cmd.Transaction = transaction;
                //        cmd.Parameters.AddWithValue("@CDetailID", CDetails.CDetailID);
                //        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                //        cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(CDetails.Name) ? "" : CDetails.Name);
                //        cmd.Parameters.AddWithValue("@MobileNo", string.IsNullOrWhiteSpace(CDetails.MobileNo) ? "" : CDetails.MobileNo);
                //        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(CDetails.Address) ? "" : CDetails.Address);
                //        cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(CDetails.Post) ? "" : CDetails.Post);
                //        cmd.Parameters.AddWithValue("@IsSocietyMember", Convert.ToBoolean(CDetails.IsSocietyMember));
                //        cmd.Parameters.AddWithValue("@Ctype", Convert.ToBoolean(CDetails.Ctype));
                //        cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);

                //        cmd.ExecuteScalar();
                //    }
                //});

                //  Applicationdatat.IODetailList = Applicationdatat.IODetailList ?? new List<PLDBLoanAmount>();
                string SerialNo = "";
                Applicationdatat.IODetailList.ForEach(IODetail =>
                {

                    string complainDateDR = "";
                    if (!string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].DateTime))
                    {
                        string date = Applicationdatat.IODetailList[0].DateTime.Split(' ')[0];
                        date = date.Replace("-", "/");
                        complainDateDR = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[1] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[2];
                    }
                    //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
                    //{
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertUpdateIODetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@IODetailID", IODetail.IODetailID);
                    cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                    cmd.Parameters.AddWithValue("@SSOID", IODetail.SSOID);
                    cmd.Parameters.AddWithValue("@UserId", IODetail.UserId);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);
                    cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(IODetail.Period) ? "" : IODetail.Period);
                    cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                    cmd.Parameters.AddWithValue("@DateTime", string.IsNullOrWhiteSpace(complainDateDR) ? "" : complainDateDR);
                    cmd.Parameters.AddWithValue("@Place", string.IsNullOrWhiteSpace(IODetail.Places) ? "" : IODetail.Places);



                    SerialNo = cmd.ExecuteScalar().ToString();
                    //}
                });

                //   Applicationdatat.ComplainTrailList = Applicationdatat.ComplainTrailList ?? new List<ComplainUnderSectionModel>();
                Applicationdatat.ComplainTrailList.ForEach(ComplainTrail =>
                {
                    //Applicationdatat.IsOld
                    if (Applicationdatat.IsOld == 1 && Convert.ToInt32(Applicationdatat.StatusId) > 2)
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdateComplainTrail";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@ComplainTrailID", 0);
                        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                        cmd.Parameters.AddWithValue("@ComplaintStatusId", Convert.ToInt32(Applicationdatat.StatusId));
                        cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(Applicationdatat.EncloserFileName2) ? "" : (Applicationdatat.EncloserFileName2));
                        
                        cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);

                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        //cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                        cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
                        cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                        cmd.Parameters.AddWithValue("@IOComplainLetter", "");
                        cmd.Parameters.AddWithValue("@IODetails", "");
                        cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                        cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                        cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].Period) ? "0" : Applicationdatat.IODetailList[0].Period);
                        cmd.Parameters.AddWithValue("@SerialNo", string.IsNullOrWhiteSpace(Applicationdatat.SerialNo) ? "0" : Applicationdatat.SerialNo);

                        LetterNo = Convert.ToString(cmd.ExecuteScalar());


                    }
                    else
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
                                cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                                cmd.Parameters.AddWithValue("@IOComplainLetter", "");
                                cmd.Parameters.AddWithValue("@IODetails", "");
                                cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                                cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].Period) ? "0" : Applicationdatat.IODetailList[0].Period);
                                cmd.Parameters.AddWithValue("@SerialNo", string.IsNullOrWhiteSpace(Applicationdatat.SerialNo) ? "0" : Applicationdatat.SerialNo);

                                LetterNo = Convert.ToString(cmd.ExecuteScalar());
                            }

                        }
                    }

                });
                if(Applicationdatat.IsOld == 1)
                {
                    Result = "1";
                    transaction.Commit();
                }
              

                if (Applicationdatat.IsOld!= 1 && Convert.ToInt32(Applicationdatat.StatusId) >2) {
                var result = GenerateComplainLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);

                if (result.status == "1")
                {
                    Result = "1";
                    transaction.Commit();
                }
                else
                {
                    Result = "-1";
                    transaction.Rollback();
                }
            }
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

        public string UpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)
        {
            string Result = "-1";
            DBFactoryMSSQL conn = new DBFactoryMSSQL();


            string Investigation1date = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation1date))
            {
                Applicationdatat.Investigation1date = Applicationdatat.Investigation1date.Replace("-", "/");
                Investigation1date = Applicationdatat.Investigation1date.Split('/')[2] + "/" + Applicationdatat.Investigation1date.Split('/')[1] + "/" + Applicationdatat.Investigation1date.Split('/')[0];
            }

            string Investigation1todate = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation1todate))
            {
                Applicationdatat.Investigation1todate = Applicationdatat.Investigation1todate.Replace("-", "/");
                Investigation1todate = Applicationdatat.Investigation1todate.Split('/')[2] + "/" + Applicationdatat.Investigation1todate.Split('/')[1] + "/" + Applicationdatat.Investigation1todate.Split('/')[0];
            }

            string Investigation2date = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation2date))
            {
                Applicationdatat.Investigation2date = Applicationdatat.Investigation2date.Replace("-", "/");
                Investigation2date = Applicationdatat.Investigation2date.Split('/')[2] + "/" + Applicationdatat.Investigation2date.Split('/')[1] + "/" + Applicationdatat.Investigation2date.Split('/')[0];
            }

            string Investigation2todate = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation2todate))
            {
                Applicationdatat.Investigation2todate = Applicationdatat.Investigation2todate.Replace("-", "/");
                Investigation2todate = Applicationdatat.Investigation2todate.Split('/')[2] + "/" + Applicationdatat.Investigation2todate.Split('/')[1] + "/" + Applicationdatat.Investigation2todate.Split('/')[0];
            }

            string Investigation3date = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation3date))
            {
                Applicationdatat.Investigation3date = Applicationdatat.Investigation3date.Replace("-", "/");
                Investigation3date = Applicationdatat.Investigation3date.Split('/')[2] + "/" + Applicationdatat.Investigation3date.Split('/')[1] + "/" + Applicationdatat.Investigation3date.Split('/')[0];
            }

            string Investigation3todate = "";
            if (!string.IsNullOrWhiteSpace(Applicationdatat.Investigation3todate))
            {
                Applicationdatat.Investigation3todate = Applicationdatat.Investigation3todate.Replace("-", "/");
                Investigation3todate = Applicationdatat.Investigation3todate.Split('/')[2] + "/" + Applicationdatat.Investigation3todate.Split('/')[1] + "/" + Applicationdatat.Investigation3todate.Split('/')[0];
            }

            if (Applicationdatat.Investigation1Detilas == null)
            {
                Applicationdatat.Investigation1Detilas = "";
            }
            if (Applicationdatat.Investigation2Detilas == null)
            {
                Applicationdatat.Investigation2Detilas = "";
            }

            if (Applicationdatat.Investigation3Detilas == null)
            {
                Applicationdatat.Investigation3Detilas = "";
            }


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

                string fileName = Guid.NewGuid().ToString() + ".pdf";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UpdateComplainUnderSection";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ComplainID", Applicationdatat.ComplainID);
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(Applicationdatat.StatusId));
                
                // Applicationdatat.SocietyId);
                cmd.Parameters.AddWithValue("@SSOID", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].SSOID) ? "" : Applicationdatat.IODetailList[0].SSOID);
                cmd.Parameters.AddWithValue("@IOUserId", Applicationdatat.IODetailList[0].UserId);
                cmd.Parameters.AddWithValue("@Period", Applicationdatat.IODetailList[0].Period);
                cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
                cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].FileName) ? "" : Applicationdatat.ComplainTrailList[0].FileName);//Applicationdatat.StatusId);
                cmd.Parameters.AddWithValue("@ComplainDate", string.IsNullOrWhiteSpace(complainDate) ? "" : complainDate);
                cmd.Parameters.AddWithValue("@UpdatedBy", Applicationdatat.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedIP", Applicationdatat.UpdatedIP);
                cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
                cmd.Parameters.AddWithValue("@YognaName", string.IsNullOrWhiteSpace(Applicationdatat.Scheme) ? "" : Applicationdatat.Scheme);

                cmd.Parameters.AddWithValue("@Investigation1", Convert.ToBoolean(Applicationdatat.Investigation1));
                cmd.Parameters.AddWithValue("@Investigation2", Convert.ToBoolean(Applicationdatat.Investigation2));
                cmd.Parameters.AddWithValue("@Investigation3", Convert.ToBoolean(Applicationdatat.Investigation3));

                cmd.Parameters.AddWithValue("@Investigation1date", string.IsNullOrWhiteSpace(Investigation1date) ? "" : Investigation1date);
                cmd.Parameters.AddWithValue("@Investigation1todate", string.IsNullOrWhiteSpace(Investigation1todate) ? "" : Investigation1todate);
                cmd.Parameters.AddWithValue("@Investigation1Detilas", Applicationdatat.Investigation1Detilas);

                cmd.Parameters.AddWithValue("@Investigation2date", string.IsNullOrWhiteSpace(Investigation2date) ? "" : Investigation2date);
                cmd.Parameters.AddWithValue("@Investigation2todate", string.IsNullOrWhiteSpace(Investigation2todate) ? "" : Investigation2todate);
                cmd.Parameters.AddWithValue("@Investigation2Detilas", Applicationdatat.Investigation2Detilas);

                cmd.Parameters.AddWithValue("@Investigation3date", string.IsNullOrWhiteSpace(Investigation3date) ? "" : Investigation3date);
                cmd.Parameters.AddWithValue("@Investigation3todate", string.IsNullOrWhiteSpace(Investigation3todate) ? "" : Investigation3todate);
                cmd.Parameters.AddWithValue("@Investigation3Detilas", Applicationdatat.Investigation3Detilas);
                cmd.Parameters.AddWithValue("@ComplantBystatus", Applicationdatat.ComplantBystatus);
                cmd.Parameters.AddWithValue("@EncloserFileName", string.IsNullOrWhiteSpace(Applicationdatat.EncloserFileName) ? "" : Applicationdatat.EncloserFileName);
                cmd.Parameters.AddWithValue("@OtherDescription", string.IsNullOrWhiteSpace(Applicationdatat.OtherDescription) ? "" : Applicationdatat.OtherDescription);
                cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                cmd.Parameters.AddWithValue("@IOType", Applicationdatat.IOType);
                cmd.Parameters.AddWithValue("@SerialNo", string.IsNullOrWhiteSpace(Applicationdatat.SerialNo) ? "0" : Applicationdatat.SerialNo);


                cmd.Parameters.AddWithValue("@ResultForFormation", string.IsNullOrWhiteSpace(Applicationdatat.ResultForFormation) ? "" : Applicationdatat.ResultForFormation);
                cmd.Parameters.AddWithValue("@ResultForBusiness", string.IsNullOrWhiteSpace(Applicationdatat.ResultForBusiness) ? "" : Applicationdatat.ResultForBusiness);
                cmd.Parameters.AddWithValue("@ResultForFinancial", string.IsNullOrWhiteSpace(Applicationdatat.ResultForFinancial) ? "" : Applicationdatat.ResultForFinancial);
                cmd.Parameters.AddWithValue("@ResultForOtherEnquiry", string.IsNullOrWhiteSpace(Applicationdatat.ResultForOtherEnquiry) ? "" : Applicationdatat.ResultForOtherEnquiry);
                cmd.Parameters.AddWithValue("@ResultOther", string.IsNullOrWhiteSpace(Applicationdatat.ResultOther) ? "" : Applicationdatat.ResultOther);



                string SerialNo = cmd.ExecuteScalar().ToString();

                //Applicationdatat.ComplainID = Convert.ToInt32(ComplainID).ToString();
                Result = ComplainID;


                //bhu

                Applicationdatat.PersonDetailsList.ForEach(PersonDetailsList =>
                {
                    if (!string.IsNullOrWhiteSpace(PersonDetailsList.Name))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertUpdateUnderPersonDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@PersonId", PersonDetailsList.CPersonId);
                        cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                        cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(PersonDetailsList.Name) ? "" : PersonDetailsList.Name);
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(PersonDetailsList.Address) ? "" : PersonDetailsList.Address);
                        cmd.Parameters.AddWithValue("@Mobile", string.IsNullOrWhiteSpace(PersonDetailsList.Mobile) ? "" : PersonDetailsList.Mobile);

                        cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(PersonDetailsList.Post) ? "" : PersonDetailsList.Post);
                        cmd.Parameters.AddWithValue("@CreatedBy", Applicationdatat.CreatedBy);
                        cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);
                        cmd.ExecuteScalar();
                    }
                });


                //bhu

                if (Applicationdatat.StatusId == "5")
                {
                    var result = GenerateComplainChangeIOLetter(Applicationdatat, SerialNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.StatusId == "10")
                {
                    var result = GenerateComplainTimeLimitApprovalLetter(Applicationdatat, SerialNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.StatusId == "7")
                {
                    var result = GeneratePublishedComplainLetter(Applicationdatat, SerialNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.StatusId == "8")
                {
                    var result = GenerateComplainWithdrawalLetter(Applicationdatat, SerialNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.StatusId == "6")
                {
                    var result = AcceptReport(Applicationdatat, SerialNo, fileName, sqlConnection, transaction);
                    Result = "1";
                    transaction.Commit();
                }
                else
                {
                    Result = "1";
                    transaction.Commit();
                }

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

        public ResultMessage GenerateComplainLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {

                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_ORDER_ISSUED");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string period = ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString();//( string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi)+", "+ ApplicationModel.IODetailList[0].Designation;
                    string drName = Data.Rows[0]["DRUser"].ToString();//(string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi)+", "+_LoggedInUser.RoleName_HI;
                                                                      //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    string khand = Data.Rows[0]["ComplantBy"].ToString() == "1" ? "(क)" : Data.Rows[0]["ComplantBy"].ToString() == "2" ? "(ख)" : Data.Rows[0]["ComplantBy"].ToString() == "3" ? "(ग)" : "";
                    //sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    //sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    //sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    //sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    //sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    //sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    //sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    //sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जाँच आदेश") + "</label></b><br/>");
                    //sb.Append("<b><label  style='font-size:15px;'>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55(1) ( " + khand + " ) के अंतर्गत)</label></b></td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    //sb.Append("<table style='width:100%'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + SerialNo + "</b></td>");
                    //sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    //sb.Append("</tr>");
                    //sb.Append("</table>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> " + societyName + " के सम्बन्ध में शिकायत प्राप्त होने पर राजस्थान सहकारी सोसाइटी  अधिनियम 2001 की धारा 55 के तहत निम्नानुसार बिंदुओं पर जाँच करवाया जाना उचित समझा गया हैं :- </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> " + ApplicationModel.ComplaintDetails + "");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> अतः मैं " + drName + "    ,  " + designation + " , सहकारी समितियाँ, " + city + " मुझ में  निहित राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के तहत्त प्रदत्त शक्तियों का प्रयोग करते हुए उक्त बिंदुंओ की जांच हेतु श्री / श्रीमती " + IOUser + ",  को जांच अधिकारी नियुक्त करता हूं। जांच अधिकारी जांच कार्यवाही पूर्ण करते हुए जांच रिपोर्ट " + period + " दिन की अवधि में  आवश्यक रूप से प्रस्तुत करें।  </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> यह जांच आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से जारी किया गया हैं। </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + ";text-align:right;'> " + designation + ",<br> सहकारी समितियाँ, " + city + " ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                    //sb.Append("</table>");
                    //sb.Append("</div>");


                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जाँच आदेश") + "</label></b><br/>");
                    sb.Append("<b><label  style='font-size:15px;'>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55(1)" + khand + " के अंतर्गत)</label></b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + Data.Rows[0]["OrderNo"].ToString() + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");


                    if (Data.Rows[0]["ComplantBy"].ToString() == "1")
                    {
                        sb.Append("<p  style='text-indent: 20px;'> <b>"+ ApplicationModel.ApplicantSocietyName + "</b> द्वारा अपने पत्र क्रमांक <b>"+ ApplicationModel.ApplicantLetterSerialNo + "</b> दिनांक <b>" + ApplicationModel.ApplicantLetterDate + "</b> के माध्यम से ");
                    }
                    else if (Data.Rows[0]["ComplantBy"].ToString() == "2")
                    {
                        sb.Append("<p  style='text-indent: 20px;'> <b>" + Data.Rows[0]["SocietyName"].ToString() + " </b> के संचालक मण्डल के सदस्यों द्वारा बहुमत के आधार पर आवेदन पत्र दिनांक <b>" + ApplicationModel.ApplicantLetterDate + "</b> प्रस्तुत कर ");
                    }
                    else if (Data.Rows[0]["ComplantBy"].ToString() == "3")
                    {
                        sb.Append("<p  style='text-indent: 20px;'> <b>" + Data.Rows[0]["SocietyName"].ToString() + " </b>  के <b>" + ApplicationModel.ApplicantMembers + "</b> सदस्यों द्वारा आवेदन पत्र दिनांक <b>" + ApplicationModel.ApplicantLetterDate + "</b> प्रस्तुत कर ");
                    }

                    if (Data.Rows[0]["ComplantBy"].ToString() == "1" || Data.Rows[0]["ComplantBy"].ToString() == "2" || Data.Rows[0]["ComplantBy"].ToString() == "3")
                    {
                        sb.Append(" <b> " + Data.Rows[0]["SocietyName"].ToString() + " </b> के संबंध में गठन/अवधि विशेष में किये गये कारोबार/वित्तीय स्थिति के बारे में जांच करने का अनुरोध किया गया है। </p>");
                    }
                    else
                    {
                        sb.Append("<p> <b> " + Data.Rows[0]["SocietyName"].ToString() + " </b>  के संबंध में प्राप्त जानकारी के आधार पर गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच किया जाना आवश्यक है। </p>");
                    }

                    sb.Append("<p  style='text-indent: 20px;'> अतः मैं, <b> " + drName + " </b> मुझमें निहित राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(1)" + khand + " की शक्तियों का प्रयोग करते हुए  <b> " + Data.Rows[0]["SocietyName"].ToString() + " </b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में निम्नांकित बिन्दुओं की जांच ");
                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append(" हेतु <b> " + IOUser + "</b>  को जांच अधिकारी नियुक्त करता हू -</p>");
                    }
                    else
                    {
                        sb.Append(" हेतु स्वयं के स्तर से सम्पादित किये जाने का निर्णय लेता हू -</p>");
                    }
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.Investigation1Detilas) && ApplicationModel.Investigation1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> गठन से संबंधित जांच बिन्दु (जांच अवधि " + ApplicationModel.Investigation1date + " से " + ApplicationModel.Investigation1todate + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.Investigation1Detilas) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.Investigation2Detilas) && ApplicationModel.Investigation2)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> कारोबार से संबंधित जांच बिन्दु (जांच अवधि " + ApplicationModel.Investigation2date + " से " + ApplicationModel.Investigation2todate + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.Investigation2Detilas) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.Investigation3Detilas) && ApplicationModel.Investigation3)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> वित्तीय स्थिति से संबंधित जांच बिन्दु (जांच अवधि " + ApplicationModel.Investigation3date + " से " + ApplicationModel.Investigation3todate + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.Investigation3Detilas) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.OtherDescription) && ApplicationModel.OtherMatter)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> जांच से संबंधित कोई भी अन्य मामले  (जांच अवधि " + ApplicationModel.Investigation4date + " से " + ApplicationModel.Investigation4todate + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.OtherDescription) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p   style='text-indent: 20px;'> जांच अधिकारी उपर्युक्त जांच बिन्दुओं पर राजस्थान सहकारी सोसाइटी अधिनियम, 2001, नियम, 2003 एवं समिति के उपनियमानुसार " + period + " दिवस में जांच कर विस्तृत जांच रिपोर्ट मय दायित्व निर्धारण करते हुए अधोहस्ताक्षरकर्ता को प्रस्तुत किया जाना सुनिश्चित करें। </p>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> यह आदेश आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर एवं कार्यालय मुद्रा अंकित कर जारी किया गया।  </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> प्रतिलिपि : <br/>");
                    //if (ApplicationModel.ComplantBystatus == 1)
                    //{
                    //    sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (आवेदनकर्ता) <br/>");
                    //    sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //    sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //    sb.Append("4. जांच अधिकारी <br/>");
                    //    sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //    sb.Append("6. गार्ड पत्रावली");
                    //}
                    //else if (ApplicationModel.ComplantBystatus == 2 || ApplicationModel.ComplantBystatus == 3 || ApplicationModel.ComplantBystatus == 4)
                    //{
                    //    //sb.Append("<td style='padding: 15px;" + fontSize + "'> प्रतिलिपि : <br/>");
                    //    sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (शीर्ष /केन्द्रीय सोसाइटी) <br/>");
                    //    sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //    sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //    sb.Append("4. जांच अधिकारी <br/>");
                    //    sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //    if (ApplicationModel.ComplantBystatus == 3)
                    //    {
                    //        sb.Append("6. गार्ड पत्रावली");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("6. आवेदनकर्ता ");
                    //        sb.Append("7. गार्ड पत्रावली");
                    //    }
                    //}
                    //sb.Append("</td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:right;padding-bottom:20px;'></td>");

                    //sb.Append("</tr>");


                    sb.Append("</table>");
                    sb.Append("</div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";



                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost =System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost?("9460934860").Split('#'):Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl,"HIN");
                            }
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GeneratePublishedComplainLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName,SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_PUBLISH_ENQUIRE_RESULT");
                if (Data.Rows.Count > 0)
                {

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;
                                                                        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string khand = Data.Rows[0]["ComplantBy"].ToString() == "1" ? "(क)" : Data.Rows[0]["ComplantBy"].ToString() == "2" ? "(ख)" : Data.Rows[0]["ComplantBy"].ToString() == "3" ? "(ग)" : "";

                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("आदेश") + "</label></b><br/>");
                    sb.Append("<b><label  style='font-size:15px;'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(5) के अन्तर्गत) </label></b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + Data.Rows[0]["ResultOrderNo"].ToString() + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");


                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["ResultOrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["ResultOrderDate"].ToString() + "</b> द्वारा  राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(1)"+khand+" के अन्तर्गत <b>" + societyName + "</b>  के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में निम्नांकित बिन्दुओं की जांच हेतु " + IOUser + "  को जांच अधिकारी नियुक्त कर जांच कार्य पूर्ण करने हेतु निर्देशित किया गया था - </p>");
                    }
                    else
                    {
                        sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["ResultOrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["ResultOrderDate"].ToString() + "</b> द्वारा  राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(1)" + khand + " के अन्तर्गत <b>" + societyName + "</b>  के संबंध में गठन/अवधि विशेष में किये गये कारोबार/वित्तीय स्थिति के बारे में निम्नांकित बिन्दुओं की जांच अधोहस्ताक्षरकर्ता द्वारा स्वयं के स्तर से किये जाने का निर्णय लिया गया था - </p>");
                    }


                    sb.Append("</td>");
                    sb.Append("</tr>");

                    if (!string.IsNullOrWhiteSpace(Data.Rows[0]["Investigation1Detilas"].ToString()))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> गठन से संबंधित जांच बिन्दु (जांच अवधि " + Data.Rows[0]["Investigation1date"].ToString() + " से " + Data.Rows[0]["Investigation1todate"].ToString() + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(Data.Rows[0]["Investigation1Detilas"].ToString()) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(Data.Rows[0]["Investigation2Detilas"].ToString()))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style= '" + fontSize + "'>");
                        sb.Append("<p  style=''><b> कारोबार से संबंधित जांच बिन्दु (जांच अवधि " + Data.Rows[0]["Investigation2date"].ToString() + " से " + Data.Rows[0]["Investigation2todate"].ToString() + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(Data.Rows[0]["Investigation2Detilas"].ToString()) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(Data.Rows[0]["Investigation3Detilas"].ToString()))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> वित्तीय स्थिति से संबंधित जांच बिन्दु (जांच अवधि " + Data.Rows[0]["Investigation3date"].ToString() + " से " + Data.Rows[0]["Investigation3todate"].ToString() + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(Data.Rows[0]["Investigation3Detilas"].ToString()) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(Data.Rows[0]["OtherDescription"].ToString()) && ApplicationModel.OtherMatter)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> जांच से संबंधित कोई भी अन्य मामले  (जांच अवधि " + Data.Rows[0]["Investigation4date"].ToString() + " से " + Data.Rows[0]["Investigation4todate"].ToString() + " तक) </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(Data.Rows[0]["OtherDescription"].ToString()) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }


                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p   style='text-indent: 20px;'>	जांच अधिकारी द्वारा अपने पत्र क्रमांक <b>" + Data.Rows[0]["IOReportNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["IOReportDate"].ToString() + "</b> द्वारा प्रकरण की जांच कर जांच रिपोर्ट मय संलग्नकों के अधोहस्ताक्षरकर्ता को प्रस्तुत की गई है। प्रस्तुत जांच रिपोर्ट का परीक्षण किया गया, जिसके आधार पर विचाराधीन प्रकरण में निम्नानुसार जांच परिणाम जारी किये जाते हैं - </p>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    else
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p   style='text-indent: 20px;'> अधोहस्ताक्षरकर्ता द्वारा विचाराधीन प्रकरण में जांच कार्य पूर्ण किया जाकर निम्नानुसार जांच परिणाम जारी किये जाते हैं - </p>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }


                    if (!string.IsNullOrWhiteSpace(ApplicationModel.ResultForFormation))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> गठन से संबंधित जांच परिणाम </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.ResultForFormation) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.ResultForBusiness))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style= '" + fontSize + "'>");
                        sb.Append("<p  style=''><b> कारोबार से संबंधित जांच परिणाम </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.ResultForBusiness) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.ResultForFinancial))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> वित्तीय स्थिति से संबंधित जांच परिणाम </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.ResultForFinancial) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    if (!string.IsNullOrWhiteSpace(ApplicationModel.ResultForOtherEnquiry) && ApplicationModel.OtherMatter)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> अन्य मामले से संबंधित जांच परिणाम </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.ResultForOtherEnquiry) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    if (!string.IsNullOrWhiteSpace(ApplicationModel.ResultOther))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p  style=''><b> अन्य जांच परिणाम </b></p>");
                        sb.Append("" + HTMLCLOSE.AutoCloseHtmlTags(ApplicationModel.ResultOther) + "");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> प्रतिलिपि : <br/>");
                    //sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (आवेदनकर्ता) <br/>");
                    //sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //sb.Append("4. जांच अधिकारी <br/>");
                    //sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //sb.Append("6. गार्ड पत्रावली");
                    //sb.Append("</td>");
                    ////sb.Append("</td>");
                    //sb.Append("</tr>");



                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> " + societyName + " के सम्बन्ध में शिकायत प्राप्त होने पर राजस्थान सहकारी सोसाइटी  अधिनियम 2001 की धारा 55 के तहत निम्नानुसार बिंदुओं पर जाँच करवाया जाना उचित समझा गया हैं :- </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> " + ApplicationModel.ComplaintDetailsForAcceptAndPublish + "");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> अतः मैं " + drName + "    ,  " + designation + " , सहकारी समितियाँ, " + city + " मुझ में  निहित राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के तहत्त प्रदत्त शक्तियों का प्रयोग करते हुए उक्त बिंदुंओ की जांच हेतु श्री / श्रीमती " + IOUser + ",  को जांच अधिकारी नियुक्त करता हूं। जांच अधिकारी जांच कार्यवाही पूर्ण करते हुए जांच रिपोर्ट " + period + " दिन की अवधि में  आवश्यक रूप से प्रस्तुत करें।  </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> यह जांच आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से जारी किया गया हैं। </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + ";text-align:right;'> " + designation + ",<br> सहकारी समितियाँ, " + city + " ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";

                    FileName = Data.Rows[0]["ComplainLetter"].ToString();

                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile) {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }                            
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainWithdrawalLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_WIDRAWAL");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    string khand = Data.Rows[0]["ComplantBy"].ToString() == "1" ? "(क)" : Data.Rows[0]["ComplantBy"].ToString() == "2" ? "(ख)" : Data.Rows[0]["ComplantBy"].ToString() == "3" ? "(ग)" : "";

                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("आदेश") + "</label></b><br/>");
                    sb.Append("<b><label  style='font-size:15px;'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(4) के अन्तर्गत) </label></b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + Data.Rows[0]["WithdrawalOrderNo"].ToString() + " </b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> के द्वारा <b>" + societyName + "</b>  के संबंध में धारा 55(1)" + khand + " के अन्तर्गत गठन / धि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच आदेश जारी किये गये थे। </p>");
                    sb.Append("<p style='text-indent: 20px;'> उक्त जांच आदेश को निम्न कारणों के मद्देनजर प्रत्याहरित किया जाना उचित समझा गया है - </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append(ApplicationModel.ComplaintDetailsForAcceptAndPublish);
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> अतः मैं, <b>" + drName + "</b>  मुझमें निहित राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(4) की शक्तियों का प्रयोग करते हुए <b>" + societyName + "</b> के संबंध में गठन/अवधि विशेष में किये गये कारोबार/वित्तीय स्थिति के बारे में पूर्व में जारी समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> को तत्काल प्रभाव से प्रत्याहरित करता हूँ। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा अंकित कर जारी किया गया। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> प्रतिलिपि : <br/>");
                    //sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (आवेदनकर्ता) <br/>");
                    //sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //sb.Append("4. जांच अधिकारी <br/>");
                    //sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //sb.Append("6. गार्ड पत्रावली");
                    //sb.Append("</td>");
                    //// sb.Append("</td>");
                    //sb.Append("</tr>");



                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> " + societyName + " के सम्बन्ध में शिकायत प्राप्त होने पर राजस्थान सहकारी सोसाइटी  अधिनियम 2001 की धारा 55 के तहत निम्नानुसार बिंदुओं पर जाँच करवाया जाना उचित समझा गया हैं :- </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> " + ApplicationModel.ComplaintDetailsForAcceptAndPublish + "");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> अतः मैं " + drName + "    ,  " + designation + " , सहकारी समितियाँ, " + city + " मुझ में  निहित राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के तहत्त प्रदत्त शक्तियों का प्रयोग करते हुए उक्त बिंदुंओ की जांच हेतु श्री / श्रीमती " + IOUser + ",  को जांच अधिकारी नियुक्त करता हूं। जांच अधिकारी जांच कार्यवाही पूर्ण करते हुए जांच रिपोर्ट " + period + " दिन की अवधि में  आवश्यक रूप से प्रस्तुत करें।  </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'><p> यह जांच आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से जारी किया गया हैं। </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + ";text-align:right;'> " + designation + ",<br> सहकारी समितियाँ, " + city + " ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr><td style=''></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";



                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {


                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn, "350", "30", "150", "30");
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainTimeLimitApprovalLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {

                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_TIME_EXTANTION_REQUEST_APPROAL");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;
                                                                        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    string khand = Data.Rows[0]["ComplantBy"].ToString() == "1" ? "(क)" : Data.Rows[0]["ComplantBy"].ToString() == "2" ? "(ख)" : Data.Rows[0]["ComplantBy"].ToString() == "3" ? "(ग)" : "";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("आदेश") + "</label></b><br/>");
                    sb.Append("<b><label  style='font-size:15px;'> (राजस्थान सहकारी सोसाइटी नियम, 2003 के नियम 75(3) के अन्तर्गत) </label></b></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");

                    sb.Append("<table style='width:100%'>");

                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + Data.Rows[0]["ApprovedTimeOrderNo"].ToString() + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");

                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक " + Data.Rows[0]["OrderNo"].ToString() + " दिनांक " + Data.Rows[0]["OrderDate"].ToString() + " के द्वारा <b>" + societyName + "</b>  के संबंध में धारा 55(1)" + khand + "  के अन्तर्गत गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच आदेश जारी करते हुए श्री <b>" + IOUser + "</b>  को जांच अधिकारी नियुक्त कर "+periodOld+" दिवस में जांच कार्य पूर्ण कर जांच रिपोर्ट प्रस्तुत करने हेतु निर्देशित किया गया था।  </p>");
                    }
                    else
                    {
                        sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक " + Data.Rows[0]["OrderNo"].ToString() + " दिनांक " + Data.Rows[0]["OrderDate"].ToString() + " के द्वारा <b>" + societyName + "</b> के संबंध में धारा 55(1)" + khand + " के अन्तर्गत गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच आदेश जारी करते हुए  " + periodOld + " दिवस में जांच कार्य पूर्ण कर जांच रिपोर्ट प्रस्तुत करने हेतु निर्णय लिया गया था।   </p>");
                    }

                    sb.Append("<p style='text-indent: 20px;'> जांच अधिकारी द्वारा पत्र क्रमांक " + Data.Rows[0]["ExpandRequestNo"].ToString() + " दिनांक " + Data.Rows[0]["ExpandRequestDate"].ToString() + " द्वारा जांच कार्य पूर्ण किये जाने हेतु युक्तियुक्त कारणों का उल्लेख करते हुए "+ Data.Rows[0]["RequestInvestigationPeriod"].ToString() + " दिवस का अतिरिक्त समय दिये जाने हेतु निवेदन किया गया है। </ p>");
                    sb.Append("<p style='text-indent: 20px;'> अतः विचाराधीन जांच पूर्ण किये जाने हेतु राजस्थान सहकारी सोसाइटी नियम, 2003 के नियम 75(3) के प्रावधानान्तर्गत जांच अवधि "+ApplicationModel.IODetailList[0].Period+ " दिवस /  दिनांक "+ApplicationModel.ComplainDate+" तक बढ़ाई जाती है। </ p>");

                    sb.Append("</td>");
                    sb.Append("</tr>");


                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='" + fontSize + "'>");
                        sb.Append("<p   style='text-indent: 20px;'> जांच अधिकारी उपर्युक्त जांच बिन्दुओं पर राजस्थान सहकारी सोसाइटी अधिनियम, 2001, नियम, 2003 एवं समिति के उपनियमानुसार निर्धारित अवधि में जांच कर विस्तृत जांच रिपोर्ट मय दायित्व निर्धारण करते हुए अधोहस्ताक्षरकर्ता को प्रस्तुत किया जाना सुनिश्चित करें।  </p>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा अंकित कर जारी किया गया। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> प्रतिलिपि : <br/>");
                    //if (ApplicationModel.ComplantBystatus == 1)
                    //{
                    //    sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (आवेदनकर्ता) <br/>");
                    //    sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //    sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //    sb.Append("4. जांच अधिकारी <br/>");
                    //    sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //    sb.Append("6. गार्ड पत्रावली");
                    //    //sb.Append("</td>");
                    //}
                    //else if (ApplicationModel.ComplantBystatus == 2 || ApplicationModel.ComplantBystatus == 2 || ApplicationModel.ComplantBystatus == 3)
                    //{
                    //    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> प्रतिलिपि : <br/>");
                    //    sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (शीर्ष /केन्द्रीय सोसाइटी) <br/>");
                    //    sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //    sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //    sb.Append("4. जांच अधिकारी <br/>");
                    //    sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //    if (ApplicationModel.ComplantBystatus == 3)
                    //    {
                    //        sb.Append("6. गार्ड पत्रावली");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("6. आवेदनकर्ता ");
                    //        sb.Append("7. गार्ड पत्रावली");
                    //    }
                    //}
                    //sb.Append("</td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";



                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                        }
                    }
                    result.data = FileName;



                }
            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            } 
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainChangeIOLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_CHANGE_IO");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string OldIOUser= Data.Rows[0]["OldIOUser"].ToString();
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;                                                                        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    string khand = Data.Rows[0]["ComplantBy"].ToString() == "1" ? "(क)" : Data.Rows[0]["ComplantBy"].ToString() == "2" ? "(ख)" : Data.Rows[0]["ComplantBy"].ToString() == "3" ? "(ग)" : "";

                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जाँच आदेश") + "</label></b><br/>");
                    sb.Append("<b><label  style='font-size:15px;'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(4) के अन्तर्गत) </label></b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + Data.Rows[0]["ChangeIOOrderNo"].ToString() + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> इस कार्यालय के समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> के द्वारा <b>" + societyName + "</b> के संबंध में धारा 55(1)" + khand + " के अन्तर्गत गठन / अवधि विशेष में किये गये कारोबार/वित्तीय स्थिति के बारे में जांच आदेश जारी किये गये थे। </p>");


                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<p style='text-indent: 20px;'> उक्त जांच आदेश में आंशिक संशोधन करते हुए राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(4) के प्रावधानान्तर्गत श्री <b>" + OldIOUser + "</b> के स्थान पर श्री <b>" + IOUser + "</b> को तत्काल प्रभाव से जांच अधिकारी नियुक्त कर निर्देशित किया जाता है कि नवनियुक्त जांच अधिकारी पूर्व में जारी समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> में वर्णित बिन्दुओं की जांच राजस्थान सहकारी सोसाइटी अधिनियम, 2001, नियम, 2003 एवं समिति के उपनियमानुसार "+ApplicationModel.IODetailList[0].Period+" दिवस में जांच कर विस्तृत जांच रिपोर्ट मय दायित्व निर्धारण करते हुए अधोहस्ताक्षरकर्ता को प्रस्तुत किया जाना सुनिश्चित करें।  </p>");
                    }
                    else
                    {
                        sb.Append("<p style='text-indent: 20px;'> उक्त जांच आदेश में आंशिक संशोधन करते हुए राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(4) के प्रावधानान्तर्गत श्री <b>" + OldIOUser + "</b> को जांच कार्य से मुक्त किया जाता है। समसंख्यक आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> में वर्णित बिन्दुओं की जांच राजस्थान सहकारी सोसाइटी अधिनियम, 2001, नियम, 2003 एवं समिति के उपनियमानुसार अधोहस्ताक्षरकर्ता द्वारा स्वयं सम्पादित की जावेगी।  </p>");
                    }

                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> यह आदेश आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर एवं कार्यालय मुद्रा अंकित कर जारी किया गया। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='" + fontSize + "'> प्रतिलिपि : <br/>");
                    //sb.Append("1. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी .................................... (आवेदनकर्ता) <br/>");
                    //sb.Append("2. फंक्शनल अधिकारी, प्रधान कार्यालय, जयपुर। <br/>");
                    //sb.Append("3. खण्डीय / इकाई अधिकारी(संबंधित) <br/>");
                    //sb.Append("4. जांच अधिकारी <br/>");
                    //sb.Append("5. प्रशासक / अध्यक्ष / मुख्य कार्यकारी अधिकारी...................................... (समिति, जिसके कार्यकलापों की जांच की जानी है।) <br/>");
                    //sb.Append("6. गार्ड पत्रावली");
                    //sb.Append("</td>");
                    ////sb.Append("</td>");
                    //sb.Append("</tr>");



                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";



                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                        }
                    }
                    result.data = FileName;
                }
            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainCEONoticeLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_NOTICE_IISUED");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //string city = "जयपुर";
                    //string cityEn = "Jaipur";
                    //string cityType = "शहर";
                    //string designation = "उप रजिस्ट्रार";
                    //string designationEn = "Deputy Registrar";
                    //string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString();// ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string period = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString();// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString();// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;


                    string reportDate = ApplicationModel.IODetailList[0].DateTime.Split(' ')[0];
                    string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1] + " " + ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
                    string address = ApplicationModel.IODetailList[0].Places;

                    //string lbl_Kramank = "COOP/2019/KOTA/200003";
                    string oldKramank = "................";
                    string oldDinank = ".................";
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                    sb.Append("<b><label style='" + fontSize + "'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(2) के अन्तर्गत) </label></b></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + Data.Rows[0]["NoticeOrderNo"].ToString() + "</td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'>");
                    //sb.Append("<p>1. अध्यक्ष / प्रशासक / मुख्य कार्यकारी अधिकारी  <br/> " + societyName + " <br/> ");
                    //sb.Append("2. आवेदनकर्ता )  <br/>");
                    //sb.Append("3.  ......................................... (अन्य कोई व्यक्ति) <br/> द्वारा: मुख्य कार्यकारी अधिकारी..................... (संस्था जिसके कार्यकलापों की जांच की जानी है)</ p>");
                    //sb.Append("</td>");
                    sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'>");
                    sb.Append("<br/><br/><p> " + societyName + " <br/></p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");



                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");

                    if (Data.Rows[0]["IOType"].ToString() == "2")
                    {
                        sb.Append("<p  style='text-indent: 20px;'> <b>" + drName + "</b>  के आदेश क्रमांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> (प्रति संलग्न) द्वारा अधोहस्ताक्षरकर्ता को <b>" + societyName + "</b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच अधिकारी नियुक्त किया गया है। </p>");
                    }
                    else
                    {
                        sb.Append("<p  style='text-indent: 20px;'>इस कार्यालय के आदेश क्रमांक <b>"+ Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> (प्रति संलग्न) द्वारा <b>" + societyName + "</b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच किये जाने का निर्णय लिया गया है।  </p>");
                    }

                    sb.Append("<p  style='text-indent: 20px;'> जांच कार्य सम्पादित करने हेतु निम्नानुसार स्थान, दिनांक एवं समय का निर्धारण किया जाता है -  </p>");
                    sb.Append("<p style='text-indent: 20px;'> स्थान  : <b> " + address + "    </b></p> ");
                    sb.Append("<p style='text-indent: 20px;'> दिनांक : <b> " + reportDate + " </b></p>");
                    sb.Append("<p style='text-indent: 20px;'> समय  : <b> " + reportTime + "  </b></p>");

                    sb.Append("<p  style='text-indent: 20px;'> अतः आपको निर्देशित किया जाता है कि आप जांच बिन्दुओं से संबंधित समस्त रिकार्ड, दस्तावेजों एवं साक्ष्यों के साथ निर्धारित स्थान, दिनांक एवं समय पर उपस्थित रहकर जांच कार्य में अपेक्षित सहयोग प्रदान किया जाना सुनिश्चित करें। </p>");

                    sb.Append("<p > निर्धारित स्थान, दिनांक एवं समय पर निम्नलिखित व्यक्तियों का उपस्थित  रहना अनिवार्य हैं : - </p>");

                    if (ApplicationModel.PersonDetailsList.Count > 0)
                    {
                       // sb.Append("<tr>");
                        //sb.Append("<td style='text-align: left;'><br/>");
                        sb.Append("<table style='width:100%' cellpadding='5' cellspacing='0' border='1' >");
                        sb.Append("<tr>");
                        sb.Append("<th style='pading:10px;width:49%;text-align:left;" + fontSize + "'> नाम </th>");
                        sb.Append("<th style='pading:10px;width:49%;text-align:left;" + fontSize + "'> मोबाइल नंबर </th>");
                        sb.Append("<th style='pading:10px;width:49%;text-align:left;" + fontSize + "'>  पता </th>");
                        sb.Append("<th style='pading:10px;width:49%;text-align:left;" + fontSize + "'> पद </th>");
                        sb.Append("</tr>");
                        //sb.Append("<tr>");

                        ApplicationModel.PersonDetailsList.ForEach(x =>
                        {
                            sb.Append("<tr>");
                            sb.Append("<td style='pading:10px;width:49%;text-align:left;" + fontSize + "'> " + x.Name + " </td>");
                            sb.Append("<td style='pading:10px;width:49%;text-align:left;" + fontSize + "'> " + x.Mobile + " </td>");
                            sb.Append("<td style='pading:10px;width:49%;text-align:left;" + fontSize + "'>  " + x.Address + " </td>");
                            sb.Append("<td style='pading:10px;width:49%;text-align:left;" + fontSize + "'> " + x.Post + " </td>");
                            sb.Append("</tr>");
                        });

                        //sb.Append("</tr>");
                        sb.Append("</table>");
                        //sb.Append("</td>");
                        //sb.Append("</tr>");
                    }

                    sb.Append("<p  style='text-indent: 20px;'> यह नोटिस आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को जारी किया जाता है। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> प्रतिलिपि : <br/>");

                    //sb.Append("1. जांच आदेश जारी करने वाले अधिकारी को सूचनार्थ प्रेषित है।  ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>" + designation + " सहकारी समितियां, " + city + " के जाँच आदेश क्रमांक <b>" + oldKramank + "</b> दिनांक <b>" + oldDinank + "</b> के द्वारा अधोहस्ताक्षरकर्ता को " + societyName + " के सम्बन्ध में राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के अंतर्गत निम्न बिंदुओं पर जाँच हेतु जाँच अधिकारी नियुक्त किया गया हैं :-  <br/> <label style='width:35px;text-decoration: underline;'> जाँच बिंदु </label> :- ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> " + ApplicationModel.IODetailList[0].IODetails + "");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> अतः आपको  जरिये नोटिस निर्देशित किया जाता हैं कि उक्त प्रकरण में सुनवाई  दिनांक " + reportDate + " को " + reportTime + "  बजे  कार्यालय " + designation + ", सहकारी समितियाँ, " + city + "  " + address + "  पर उक्त जाँच  बिन्दुओ  के सम्बन्ध में समिति की ओर से  जवाब मय  आवश्यक रिकार्ड / दस्तावेजों के व्यत्तिगत रूप से उपस्तिथ होकर प्रस्तुत करें  अन्यथा आपके विरुद्ध अधिनियम की धारा 28(11 ) के अंतर्गत कार्यवाही प्रारम्भ करने के साथ-साथ अधिनियम की धारा 115 सहपठित धारा 151 सीपीसी के तहत्त जरिये वारंट आपकी उपस्थिति सुनिश्चित की जावेगी।  सूचत रहे।  ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> यह नोटिस आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर से जारी किया गया। ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + ";text-align:right;'> जांच अधिकारी एवं कार्यालय-" + designation + ",<br/> सहकारी, समितियाँ, " + city);
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";



                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl,"HIN");
                            }

                            ApplicationModel.PersonDetailsList.ForEach(x =>
                            {
                                string mobileNo = isLocalHost ? "9460934860" : x.Mobile;
                                new EsanchaarController().SendSocietySMS(sms, mobileNo, "HIN");
                            });
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainReportSubmitLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_SUBMIT_REPORT");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //string city = "जयपुर";
                    //string cityEn = "Jaipur";
                    //string cityType = "शहर";
                    //string designation = "उप रजिस्ट्रार";
                    //string designationEn = "Deputy Registrar";
                    //string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;


                    string reportDate = ApplicationModel.IODetailList[0].DateTime.Split(' ')[0];
                    string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1] + " " + ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
                    string address = ApplicationModel.IODetailList[0].Places;

                    //string lbl_Kramank = "COOP/2019/KOTA/200003";
                    string oldKramank = "................";
                    string oldDinank = ".................";
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    //sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                    //sb.Append("<b><label style='" + fontSize + "'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(2) के अन्तर्गत) </label></b></td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + Data.Rows[0]["IOReportNo"].ToString() + "</td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'>");
                    sb.Append("<br/><br/><br/><p> <b>"+drName+"</b>  </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> विषय :  जांच रिपोर्ट प्रस्तुत करने हेतु। </p>");
                    sb.Append("<p style='text-indent: 20px;'> प्रसंग :  आपका आदेश क्रमांक " + Data.Rows[0]["OrderNo"].ToString() + " दिनांक : " + Data.Rows[0]["OrderDate"].ToString() + " </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append(" महोदय, ");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> उपर्युक्त विषयान्तर्गत निवेदन है कि प्रासंगिक आदेश द्वारा अधोहस्ताक्षरकर्ता को <b>" + societyName + "</b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच अधिकारी नियुक्त कर "+periodNew+" दिवस में जांच कार्य पूर्ण कर जांच रिपोर्ट प्रस्तुत करने के निर्देश प्रदान किये गये थे।  </p>");
                    sb.Append("<p style='text-indent: 20px;'> उक्त क्रम में जांच कार्य पूर्ण कर जांच रिपोर्ट मय संबंधित रिकार्ड / दस्तावेजों / साक्ष्यों के अग्रिम आवश्यक कार्यवाही हेतु संलग्न कर प्रेषित की जा रही है। </p>");
                    sb.Append("<p> संलग्न : उपर्युक्तानुसार। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";

                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainTimeLimitRequestLetter(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_TIME_EXTANTION_REQUEST");
                if (Data.Rows.Count > 0)
                {
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //string city = "जयपुर";
                    //string cityEn = "Jaipur";
                    //string cityType = "शहर";
                    //string designation = "उप रजिस्ट्रार";
                    //string designationEn = "Deputy Registrar";
                    //string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    string ResionName = _LoggedInUser.ResionName ?? "";
                    string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    //string cityType = ApplicationModel.CityType;
                    string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;


                    string reportDate = ApplicationModel.IODetailList[0].DateTime.Split(' ')[0];
                    string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1] + " " + ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
                    string address = ApplicationModel.IODetailList[0].Places;

                    //string lbl_Kramank = "COOP/2019/KOTA/200003";
                    string oldKramank = "................";
                    string oldDinank = ".................";
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city) + "</label></b>");
                    sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    //sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                    //sb.Append("<b><label style='" + fontSize + "'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(2) के अन्तर्गत) </label></b></td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + Data.Rows[0]["ExpandRequestNo"].ToString() + "</td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'>");
                    sb.Append("<p><br/> <br/> <b>" + drName + "</b> <br/> </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> विषय :  जांच अवधि बढ़ाये जाने हेतु। </p>");
                    sb.Append("<p style='text-indent: 20px;'> प्रसंग :  आपका आदेश क्रमांक <b>"+ Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक : <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append(" महोदय, ");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> उपर्युक्त विषयान्तर्गत निवेदन है कि प्रासंगिक आदेश द्वारा अधोहस्ताक्षरकर्ता को <b>" + societyName + "</b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच अधिकारी नियुक्त कर "+ periodOld + " दिवस में जांच कार्य पूर्ण कर जांच रिपोर्ट प्रस्तुत करने के निर्देश प्रदान किये गये हैं। </p>");
                    sb.Append("<p style='text-indent: 20px;'> उक्त क्रम में निवेदन है कि निम्नलिखित कारणों से निर्धारित अवधि में जांच कार्य पूर्ण किया जाना संभव नहीं है - </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    sb.Append(" " + ApplicationModel.ComplaintDetails + " ");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>");
                    sb.Append("<p style='text-indent: 20px;'> अतः प्रासंगिक आदेशों के क्रम में आज दिनांक तक की गई जांच कार्यवाही के आधार पर अन्तरिम जांच रिपोर्ट संलग्न कर निवेदन है कि प्रकरण की जांच हेतु " + periodNew + " दिवस / दिनांक ............................ तक की अवधि बढ़ाये जाने के आदेश प्रदान करें। </p>");
                    sb.Append("<p> संलग्न : उपर्युक्तानुसार । </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    //string fileName = Guid.NewGuid().ToString() + ".pdf";

                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    {
                        result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                        if (result.status == "1")
                        {
                            string sms = Data.Rows[0]["sms"].ToString();
                            bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                            string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                            foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                        }
                    }
                    result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage AcceptReport(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                DataTable Data = GetSection55LetterDetails(ApplicationModel.ComplainID, sqlConnection, transaction, "SMS_ON_ACCEPT_REPORT");
                if (Data.Rows.Count > 0)
                {
                    FileName = Data.Rows[0]["ComplainLetter"].ToString();
                    //OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    ////string city = "जयपुर";
                    ////string cityEn = "Jaipur";
                    ////string cityType = "शहर";
                    ////string designation = "उप रजिस्ट्रार";
                    ////string designationEn = "Deputy Registrar";
                    ////string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                    //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    //string ResionName = _LoggedInUser.ResionName ?? "";
                    //string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                    //string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                    ////string cityType = ApplicationModel.CityType;
                    //string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                    //string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                    //string societyName = Data.Rows[0]["SocietyName"].ToString(); // ApplicationModel.SocietyName;
                    //// string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                    //string periodOld = Data.Rows[0]["OldInvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    //string periodNew = Data.Rows[0]["InvestigationPeriod"].ToString();// ApplicationModel.IODetailList[0].Period;
                    //string IOUser = Data.Rows[0]["IOUser"].ToString(); ;// string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                    //string drName = Data.Rows[0]["DRUser"].ToString(); ;// string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;


                    //string reportDate = ApplicationModel.IODetailList[0].DateTime.Split(' ')[0];
                    //string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1] + " " + ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
                    //string address = ApplicationModel.IODetailList[0].Places;

                    ////string lbl_Kramank = "COOP/2019/KOTA/200003";
                    //string oldKramank = "................";
                    //string oldDinank = ".................";
                    ////System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //string devFontSize = "17px";
                    //string fontSize = "font-size: 13px;";
                    //sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    //sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    //sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    //sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    //sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    //sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city) + "</label></b>");
                    //sb.Append("</td></tr>");
                    ////sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    ////sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                    ////sb.Append("<b><label style='" + fontSize + "'> (राजस्थान सहकारी सोसाइटी अधिनियम, 2001 की धारा 55(2) के अन्तर्गत) </label></b></td>");
                    ////sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    //sb.Append("<table style='width:100%'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + Data.Rows[0]["ExpandRequestNo"].ToString() + "</td>");
                    //sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'>");
                    //sb.Append("<p><br/> <br/> <b>" + drName + "</b> <br/> </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("</table>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    //sb.Append("<p style='text-indent: 20px;'> विषय :  जांच अवधि बढ़ाये जाने हेतु। </p>");
                    //sb.Append("<p style='text-indent: 20px;'> प्रसंग :  आपका आदेश क्रमांक <b>" + Data.Rows[0]["OrderNo"].ToString() + "</b> दिनांक : <b>" + Data.Rows[0]["OrderDate"].ToString() + "</b> </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    //sb.Append(" महोदय, ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>");
                    //sb.Append("<p style='text-indent: 20px;'> उपर्युक्त विषयान्तर्गत निवेदन है कि प्रासंगिक आदेश द्वारा अधोहस्ताक्षरकर्ता को <b>" + societyName + "</b> के संबंध में गठन / अवधि विशेष में किये गये कारोबार / वित्तीय स्थिति के बारे में जांच अधिकारी नियुक्त कर " + periodOld + " दिवस में जांच कार्य पूर्ण कर जांच रिपोर्ट प्रस्तुत करने के निर्देश प्रदान किये गये हैं। </p>");
                    //sb.Append("<p style='text-indent: 20px;'> उक्त क्रम में निवेदन है कि निम्नलिखित कारणों से निर्धारित अवधि में जांच कार्य पूर्ण किया जाना संभव नहीं है - </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px;" + fontSize + "'>");
                    //sb.Append(" " + ApplicationModel.ComplaintDetails + " ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>");
                    //sb.Append("<p style='text-indent: 20px;'> अतः प्रासंगिक आदेशों के क्रम में आज दिनांक तक की गई जांच कार्यवाही के आधार पर अन्तरिम जांच रिपोर्ट संलग्न कर निवेदन है कि प्रकरण की जांच हेतु " + periodNew + " दिवस / दिनांक ............................ तक की अवधि बढ़ाये जाने के आदेश प्रदान करें। </p>");
                    //sb.Append("<p> संलग्न : उपर्युक्तानुसार । </p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                    //sb.Append("</table>");
                    //sb.Append("</div>");
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    //string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                    ////string fileName = Guid.NewGuid().ToString() + ".pdf";

                    //if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                    //{
                    //    result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn);
                    //    if (result.status == "1")
                    //    {
                    string sms = Data.Rows[0]["sms"].ToString();
                    bool isLocalHost = System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost");
                    string[] mobile = isLocalHost ? ("9460934860").Split('#') : Data.Rows[0]["SMSMobile"].ToString().Split('#');
                    foreach (string mbl in mobile)
                            {
                                new EsanchaarController().SendSocietySMS(sms, mbl, "HIN");
                            }
                    //    }
                    //}
                    //result.data = FileName;

                }

            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainLetter_OLD(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName)
        {
            ResultMessage result = new ResultMessage();
            try
            {


                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string ResionName = _LoggedInUser.ResionName ?? "";
                string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                //string cityType = ApplicationModel.CityType;
                string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                string societyName = ApplicationModel.SocietyName;
                // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                string period = ApplicationModel.IODetailList[0].Period;
                string IOUser = string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                string drName = string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियाँ, " + city) + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जाँच आदेश") + "</label></b><br/>");
                sb.Append("<b><label  style='font-size:15px;'>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55 के अंतर्गत)</label></b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + SerialNo + "</b></td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p> " + societyName + " के सम्बन्ध में शिकायत प्राप्त होने पर राजस्थान सहकारी सोसाइटी  अधिनियम 2001 की धारा 55 के तहत निम्नानुसार बिंदुओं पर जाँच करवाया जाना उचित समझा गया हैं :- </p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'> " + ApplicationModel.ComplaintDetails + "");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p> अतः मैं " + drName + "    ,  " + designation + " , सहकारी समितियाँ, " + city + " मुझ में  निहित राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के तहत्त प्रदत्त शक्तियों का प्रयोग करते हुए उक्त बिंदुंओ की जांच हेतु श्री / श्रीमती " + IOUser + ",  को जांच अधिकारी नियुक्त करता हूं। जांच अधिकारी जांच कार्यवाही पूर्ण करते हुए जांच रिपोर्ट " + period + " दिन की अवधि में  आवश्यक रूप से प्रस्तुत करें।  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p> यह जांच आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से जारी किया गया हैं। </p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + ";text-align:right;'> " + designation + ",<br> सहकारी समितियाँ, " + city + " ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                //string fileName = Guid.NewGuid().ToString() + ".pdf";



                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                {


                    result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn, "350", "30", "150", "30");
                }
                result.data = FileName;



            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public ResultMessage GenerateComplainLetterIO(ComplainUnderSectionModel ApplicationModel, string SerialNo, string FileName)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //string city = "जयपुर";
                //string cityEn = "Jaipur";
                //string cityType = "शहर";
                //string designation = "उप रजिस्ट्रार";
                //string designationEn = "Deputy Registrar";
                //string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string ResionName = _LoggedInUser.ResionName ?? "";
                string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(शहर)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
                string cityEn = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") > 0 ? _LoggedInUser.DISTRICT_ENG + "(City)" : ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_ENG + "(Rural)" : _LoggedInUser.DISTRICT_MANGAL) : "Rajasthan";
                //string cityType = ApplicationModel.CityType;
                string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                string societyName = ApplicationModel.SocietyName;
                // string lbl_Kramank = "COOP/" + city + "/ धारा 55/" + DateTime.Now.Year + "/" + Id;
                string period = ApplicationModel.IODetailList[0].Period;
                string IOUser = string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                string drName = string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;


                string reportDate = ApplicationModel.IODetailList[0].DateTime.Split(' ')[0];
                string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1] + " " + ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
                string address = ApplicationModel.IODetailList[0].Places;

                //string lbl_Kramank = "COOP/2019/KOTA/200003";
                string oldKramank = "................";
                string oldDinank = ".................";
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city) + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                sb.Append("<b><label style='" + fontSize + "'>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55(2) (क) के अंतर्गत)</label></b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + SerialNo + "</td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'><br/><br/><b> अध्यक्ष / मंत्री<br/> " + societyName + "</b></td>");
                sb.Append("</tr>");

                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>" + designation + " सहकारी समितियां, " + city + " के जाँच आदेश क्रमांक <b>" + oldKramank + "</b> दिनांक <b>" + oldDinank + "</b> के द्वारा अधोहस्ताक्षरकर्ता को " + societyName + " के सम्बन्ध में राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के अंतर्गत निम्न बिंदुओं पर जाँच हेतु जाँच अधिकारी नियुक्त किया गया हैं :-  <br/> <label style='width:35px;text-decoration: underline;'> जाँच बिंदु </label> :- ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> " + ApplicationModel.IODetailList[0].IODetails + "");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> अतः आपको  जरिये नोटिस निर्देशित किया जाता हैं कि उक्त प्रकरण में सुनवाई  दिनांक " + reportDate + " को " + reportTime + "  बजे  कार्यालय " + designation + ", सहकारी समितियाँ, " + city + "  " + address + "  पर उक्त जाँच  बिन्दुओ  के सम्बन्ध में समिति की ओर से  जवाब मय  आवश्यक रिकार्ड / दस्तावेजों के व्यत्तिगत रूप से उपस्तिथ होकर प्रस्तुत करें  अन्यथा आपके विरुद्ध अधिनियम की धारा 28(11 ) के अंतर्गत कार्यवाही प्रारम्भ करने के साथ-साथ अधिनियम की धारा 115 सहपठित धारा 151 सीपीसी के तहत्त जरिये वारंट आपकी उपस्थिति सुनिश्चित की जावेगी।  सूचत रहे।  ");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> यह नोटिस आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर से जारी किया गया। ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + ";text-align:right;'> जांच अधिकारी एवं कार्यालय-" + designation + ",<br/> सहकारी, समितियाँ, " + city);
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'></td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                string filepath = HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

                //string fileName = Guid.NewGuid().ToString() + ".pdf";



                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName, ""))
                {
                    result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + FileName, cityEn, designationEn, "350", "30", "150", "30");


                }
                result.data = FileName;



            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }


        public DataTable GetSection55LetterDetails(string ComplainID, SqlConnection sqlConnection, SqlTransaction transaction,string SMSKey)
        {

            //SqlConnection sqlConnection = conn.GetConnection;
            //SqlTransaction transaction;


            dt1.Clear();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int UserRoleMappingId = _LoggedInUser.UserRoleDeptMappingId;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand();
                cmd.CommandText = "GetSection55LetterDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ComplainID", ComplainID);
                cmd.Parameters.AddWithValue("@SMSKey", SMSKey);               

                //cmd.ExecuteReader();
                SqlDataReader dr = cmd.ExecuteReader();
                dt1.Load(dr);


                ////SqlCommand cmd = new SqlCommand();
                //objDBAccess1.OpenConnection();
                //cmd = objDBAccess1.CreateCommand("GetSection55LetterDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess1.AddParameterToCommand("@ComplainID", DBFactoryParameterType.Int, UserRoleMappingId);
                //dt1 = objDBAccess1.GetTable();

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
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                string SerialNo = "";
                string LetterNo = "";

                Applicationdatat.ComplainTrailList[0].ComplaintStatusId = Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "2" ? "9" : Applicationdatat.ComplainTrailList[0].ComplaintStatusId;

                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "9")
                {

                    string complainDate = "";
                    if (!string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].DateTime))
                    {
                        string date = Applicationdatat.IODetailList[0].DateTime.Split(' ')[0];
                        date = date.Replace("-", "/");
                        complainDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[1] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[2];
                    }

                    Applicationdatat.IODetailList.ForEach(IODetail =>
                    {
                        //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
                        //{
                        Applicationdatat.IOComplainLetter = fileName;
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
                        cmd.Parameters.AddWithValue("@IOComplainLetter", fileName);
                        cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);
                        cmd.Parameters.AddWithValue("@Period", "0");
                        cmd.Parameters.AddWithValue("@CityName", string.IsNullOrWhiteSpace(_LoggedInUser.DISTRICT_MANGAL) ? "" : _LoggedInUser.DISTRICT_MANGAL);
                        cmd.Parameters.AddWithValue("@DateTime", complainDate);
                        cmd.Parameters.AddWithValue("@Place", string.IsNullOrWhiteSpace(IODetail.Places) ? "" : IODetail.Places);
                        //cmd.Parameters.AddWithValue("@StatusId", Applicationdatat.ComplainTrailList[0].ComplaintStatusId);


                        SerialNo = cmd.ExecuteScalar().ToString();
                        //}
                    });

                    Applicationdatat.PersonDetailsList.ForEach(PersonDetailsList =>
                    {
                        if (!string.IsNullOrWhiteSpace(PersonDetailsList.Name))
                        {
                            cmd = new SqlCommand();
                            cmd.CommandText = "InsertUpdateUnderPersonDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@PersonId", PersonDetailsList.CPersonId);
                            cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                            cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(PersonDetailsList.Name) ? "" : PersonDetailsList.Name);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(PersonDetailsList.Address) ? "" : PersonDetailsList.Address);
                            cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(PersonDetailsList.Post) ? "" : PersonDetailsList.Post);
                            cmd.Parameters.AddWithValue("@CreatedBy", Applicationdatat.CreatedBy);
                            cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);
                            cmd.Parameters.AddWithValue("@Mobile", "");
                            cmd.ExecuteScalar();
                        }
                    });
                }
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
                    cmd.Parameters.AddWithValue("@ComplaintStatusId", Applicationdatat.ComplainTrailList[0].ComplaintStatusId);
                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ComplaintDetails", "");
                    cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].IODetails) ? "" : Applicationdatat.IODetailList[0].IODetails);
                    cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                    cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].Period) ? "0" : Applicationdatat.IODetailList[0].Period);
                    cmd.Parameters.AddWithValue("@SerialNo", string.IsNullOrWhiteSpace(Applicationdatat.SerialNo) ? "0" : Applicationdatat.SerialNo);

                    LetterNo = Convert.ToString(cmd.ExecuteScalar());
                    // }
                });

                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "9")
                {
                    var result = GenerateComplainCEONoticeLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "4")
                {
                    var result = GenerateComplainTimeLimitRequestLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "3")
                {
                    var result = GenerateComplainReportSubmitLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else
                {
                    Result = "1";
                    transaction.Commit();
                }
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

        public string InsertUpdateIODetail1(ComplainUnderSectionModel Applicationdatat)
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
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                string SerialNo = "";
                string LetterNo = "";

                Applicationdatat.ComplainTrailList[0].ComplaintStatusId = Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "2" ? "9" : Applicationdatat.ComplainTrailList[0].ComplaintStatusId;

                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "9")
                {

                    string complainDate = "";
                    if (!string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].DateTime))
                    {
                        string date = Applicationdatat.IODetailList[0].DateTime.Split(' ')[0];
                        date = date.Replace("-", "/");
                        complainDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[1] + " " + Applicationdatat.IODetailList[0].DateTime.Split(' ')[2];
                    }

                    Applicationdatat.IODetailList.ForEach(IODetail =>
                    {
                        //if (!string.IsNullOrWhiteSpace(IODetail.ComplainID))
                        //{
                        Applicationdatat.IOComplainLetter = fileName;
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
                        cmd.Parameters.AddWithValue("@IOComplainLetter", fileName);
                        cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(IODetail.IODetails) ? "" : IODetail.IODetails);
                        cmd.Parameters.AddWithValue("@Period", "0");
                        cmd.Parameters.AddWithValue("@CityName", string.IsNullOrWhiteSpace(_LoggedInUser.DISTRICT_MANGAL) ? "" : _LoggedInUser.DISTRICT_MANGAL);
                        cmd.Parameters.AddWithValue("@DateTime", complainDate);
                        cmd.Parameters.AddWithValue("@Place", string.IsNullOrWhiteSpace(IODetail.Places) ? "" : IODetail.Places);
                        //cmd.Parameters.AddWithValue("@StatusId", Applicationdatat.ComplainTrailList[0].ComplaintStatusId);


                        SerialNo = cmd.ExecuteScalar().ToString();
                        //}
                    });

                    Applicationdatat.PersonDetailsList.ForEach(PersonDetailsList =>
                    {
                        if (!string.IsNullOrWhiteSpace(PersonDetailsList.Name))
                        {
                            cmd = new SqlCommand();
                            cmd.CommandText = "InsertUpdateUnderPersonDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@PersonId", PersonDetailsList.CPersonId);
                            cmd.Parameters.AddWithValue("@ComplainID", Convert.ToInt32(Applicationdatat.ComplainID));
                            cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(PersonDetailsList.Name) ? "" : PersonDetailsList.Name);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(PersonDetailsList.Address) ? "" : PersonDetailsList.Address);
                            cmd.Parameters.AddWithValue("@Post", string.IsNullOrWhiteSpace(PersonDetailsList.Post) ? "" : PersonDetailsList.Post);
                            cmd.Parameters.AddWithValue("@CreatedBy", Applicationdatat.CreatedBy);
                            cmd.Parameters.AddWithValue("@CreatedIP", Applicationdatat.CreatedBy);
                            cmd.ExecuteScalar();
                        }
                    });
                }
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
                    cmd.Parameters.AddWithValue("@ComplaintStatusId", Applicationdatat.ComplainTrailList[0].ComplaintStatusId);
                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ComplaintDetails", "");
                    cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].IODetails) ? "" : Applicationdatat.IODetailList[0].IODetails);
                    cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL ?? "" : "राजस्थान");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                    cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].Period)?"0": Applicationdatat.IODetailList[0].Period);
                    cmd.Parameters.AddWithValue("@SerialNo", string.IsNullOrWhiteSpace(Applicationdatat.SerialNo) ? "0" : Applicationdatat.SerialNo);

                    LetterNo = Convert.ToString(cmd.ExecuteScalar());
                    // }
                });

                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "9")
                {
                    var result = GenerateComplainCEONoticeLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                } 
                else if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "4")
                {
                    var result = GenerateComplainTimeLimitRequestLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId == "3")
                {
                    var result = GenerateComplainReportSubmitLetter(Applicationdatat, LetterNo, fileName, sqlConnection, transaction);
                    if (result.status == "1")
                    {
                        Result = "1";
                        transaction.Commit();
                    }
                    else
                    {
                        Result = "-1";
                        transaction.Rollback();
                    }
                }
                else
                {
                    Result = "1";
                    transaction.Commit();
                }
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

        public DataTable SocietyComplaintList1(ref DataPagingModel Details, string RequestType, string SSOID)
        {

            dt1.Clear();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int UserRoleMappingId = _LoggedInUser.UserRoleDeptMappingId;
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
                    objDBAccess1.AddParameterToCommand("@RequestType", DBFactoryParameterType.Int, RequestType);//3 citizen
                    objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess1.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess1.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess1.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess1.CreateCommand("GetApproveSocietyByUserRoleDeptMappingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.NVarchar, UserRoleMappingId);
                objDBAccess1.AddParameterToCommand("@RequestType", DBFactoryParameterType.Int, RequestType);//3 citizen
                objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess1.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
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


        public DataTable GetApprovedSocietyList()
        {
            // DataTable dt = new DataTable();
            dt1.Clear();
            int UserRoleMappingId = 1;
            try
            {
                objDBAccess1.OpenConnection();
                SqlCommand cmd = objDBAccess1.CreateCommand("GetApproveSocietyByUserRoleDeptMappingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
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
        public DataTable GetApprovedSocietyList(ref DataPagingModel Details, string RequestType)
        {
            // DataTable dt = new DataTable();
            dt1.Clear();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int UserRoleMappingId = _LoggedInUser.UserRoleDeptMappingId;
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

                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    whereCase = " and SS.StatusId=" + Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
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

        public DataTable GetSocietyComplianCourtModuleDetailsForIO(ref DataPagingModel Details, string RequestType)
        {
            // DataTable dt = new DataTable();
            dt1.Clear();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int UserRoleMappingId = _LoggedInUser.UserRoleDeptMappingId;
            try
            {
                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {

                    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    if (RequestType == "2")
                    {
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(s.SocietyName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(s.RegistrationNo))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        //whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'EN')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        //whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'HI')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        //whereCase += " or REPLACE(lower(rtrim(ltrim(OfficerNameEn))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        //whereCase += " or REPLACE(lower(rtrim(ltrim(OfficerNameHi))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";

                    }
                    else
                    {
                        whereCase += " and (REPLACE(lower(rtrim(ltrim(SocietyName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(D.RegistrationNo))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'EN')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        whereCase += " or REPLACE(lower(rtrim(ltrim(dbo.[GetSocietyAddress](D.SocietyId,'HI')))),'  ',' ') like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%')";
                    }
                }

                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    whereCase = " and CUS.StatusId=" + Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }

                    SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess1.CreateCommand("GetSocietyComplianCourtModuleDetailsForIO", DBFactoryCommandType.PROCEDURE_COMMAND);
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

                cmd = objDBAccess1.CreateCommand("GetSocietyComplianCourtModuleDetailsForIO", DBFactoryCommandType.PROCEDURE_COMMAND);
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
                objDBAccess1.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
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


        public DataTable GetSocietydetailsDatalist(DataPagingModel Details, string Districtid = "", string SocietyTypeid = "", string Classid = "", string Subclassid = "", string CategoryId = "", string Searchbytext = "",string Type="1")
        {

            DataTable dt1 = new DataTable();

            // dt.Clear();
            try
            {
                string WhereClause = "";
                

                if (Districtid != "" && Districtid != "undefined")
                {
                    if (Convert.ToInt32(Districtid) > 0)
                    {
                        WhereClause += " And SocietyRegOfficeDetails.DISTRICT_ID=" + "'" + Convert.ToInt32(Districtid) + "'" + " ";
                    }
                }
                if (SocietyTypeid != "" && SocietyTypeid != "undefined")
                {
                    if (Convert.ToInt32(SocietyTypeid) > 0)
                    {
                        WhereClause += " And s.SocietyTypeId=" + Convert.ToInt32(SocietyTypeid) + " ";
                    }
                }
                if (Classid != "" && Classid != "undefined")
                {
                    if (Convert.ToInt32(Classid) > 0)
                    {
                        WhereClause += " And s.ClassId=" + Convert.ToInt32(Classid) + " ";
                    }
                }

                if (Subclassid != "" && Subclassid != "undefined")
                {
                    if (Convert.ToInt32(Subclassid) > 0)
                    {
                        WhereClause += " And s.SubClassId=" + Convert.ToInt32(Subclassid) + " ";
                    }
                }
                if (CategoryId != "" && CategoryId != "undefined")
                {
                    if (Convert.ToInt32(CategoryId) > 0)
                    {
                        WhereClause += " And s.CategoryId=" + Convert.ToInt32(CategoryId) + " ";
                    }
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    Details.TotalRecords = 0;
                    cmd = objDBAccess1.CreateCommand("GetSocietydetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 1);
                    objDBAccess1.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess1.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, Searchbytext);
                    objDBAccess1.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess1.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);


                    Details.TotalRecords = Convert.ToInt32(objDBAccess1.ExecuteScalarCommand());
                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess1.CreateCommand("GetSocietydetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess1.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess1.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 0);
                objDBAccess1.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess1.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, Searchbytext);
                objDBAccess1.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess1.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);

                dt1 = objDBAccess1.GetTable();





            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetAuditDataForFirmOrIndividual(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
            }
            return dt1;
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
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetComplainDetails(" + ComplainID + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return ds;

        }

        public DataTable GetComplainStatus(int ActionId)
        {
            DataTable dt = new DataTable();
            dt1.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                cmd = objDBAccess1.CreateCommand("GetComplainStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                dt = objDBAccess1.GetTable();
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
            return dt;

        }
        public DataTable GetComplainTrail(int ComplainID)
        {
            DataTable dt = new DataTable();
            dt1.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                cmd = objDBAccess1.CreateCommand("GetComplainTrail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@ComplainID", DBFactoryParameterType.Int, ComplainID);
                dt = objDBAccess1.GetTable();
            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetComplainTrail(" + ComplainID + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return dt;

        }

        public DataTable GetAdministrativeDetailsById(int SocietyId)
        {
            DataTable dt = new DataTable();
            dt1.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                cmd = objDBAccess1.CreateCommand("GetAdministrativeDetailsById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                dt = objDBAccess1.GetTable();
            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetAdministrativeDetailsById(" + SocietyId + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return dt;
        }

        public DataTable GetSection55Memberss(int ComplainID)
        {
            DataTable dt = new DataTable();
            dt1.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess1.OpenConnection();
                cmd = objDBAccess1.CreateCommand("GetSection55Memberss", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess1.AddParameterToCommand("@ComplainID", DBFactoryParameterType.Int, ComplainID);
                dt = objDBAccess1.GetTable();
            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetSection55Memberss(" + ComplainID + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return dt;
        }


        public DataTable GetInvestiUser(string DesignationLevelMappingId, int UserId)
        {
            // dt.Clear();

            DataTable dt1 = new DataTable();
            try
            {
                objDBAccess1.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess1.CreateCommand("get_investicationofficerdata", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@DesignationLevelMappingId", DBFactoryParameterType.Int, );
                objDBAccess1.AddParameterToCommand("@DesignationLevelMappingId", DBFactoryParameterType.Int, Convert.ToInt32(DesignationLevelMappingId));
                objDBAccess1.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, Convert.ToInt32(UserId));
                dt1 = objDBAccess1.GetTable();
            }
            catch (Exception ex)
            {
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetAllUser(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }



            return dt1;
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