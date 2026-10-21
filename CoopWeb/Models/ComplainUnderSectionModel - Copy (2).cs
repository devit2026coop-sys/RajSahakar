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
            
        public string Scheme { get; set; }
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
        public string OfficerNameEn { get; set; }
        public string OfficerNameHi { get; set; }
        public string Period { get; set; }
        public string Places { get; set; }
        public string DateTime { get; set; }
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
                string fileName = Guid.NewGuid().ToString() + ".pdf";
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
                cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
                cmd.Parameters.AddWithValue("@YognaName", string.IsNullOrWhiteSpace(Applicationdatat.Scheme)?"": Applicationdatat.Scheme);
             
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
                string SerialNo = "";
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
                    cmd.Parameters.AddWithValue("@Period", string.IsNullOrWhiteSpace(IODetail.Period) ? "" : IODetail.Period);
                    cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL??"" : "राजस्थान");

                    SerialNo = cmd.ExecuteScalar().ToString();
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
                            cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                            cmd.Parameters.AddWithValue("@IOComplainLetter", "");
                            cmd.Parameters.AddWithValue("@IODetails", "");

                            cmd.ExecuteScalar();
                        }

                    }

                });

               var result= GenerateComplainLetter(Applicationdatat, SerialNo, fileName);
                if (result.status == "1")
                {
                    Result = "1";
                    transaction.Commit();
                }
                else {
                    Result = "-1";
                    transaction.Rollback();
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
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                cmd.CommandText = "UpdateComplainUnderSection";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ComplainID", Applicationdatat.ComplainID);
                cmd.Parameters.AddWithValue("@StatusId", Convert.ToInt32(Applicationdatat.StatusId)); // Applicationdatat.SocietyId);
                cmd.Parameters.AddWithValue("@SSOID", string.IsNullOrWhiteSpace( Applicationdatat.IODetailList[0].SSOID)?"": Applicationdatat.IODetailList[0].SSOID);
                cmd.Parameters.AddWithValue("@IOUserId", Applicationdatat.IODetailList[0].UserId);
                cmd.Parameters.AddWithValue("@Period", Applicationdatat.IODetailList[0].Period);
                cmd.Parameters.AddWithValue("@ComplaintDetails", string.IsNullOrWhiteSpace(Applicationdatat.ComplaintDetails) ? "" : Applicationdatat.ComplaintDetails);
                cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace( Applicationdatat.ComplainTrailList[0].FileName)?"": Applicationdatat.ComplainTrailList[0].FileName);//Applicationdatat.StatusId);
                cmd.Parameters.AddWithValue("@ComplainDate", string.IsNullOrWhiteSpace(complainDate) ? "" : complainDate);
                cmd.Parameters.AddWithValue("@UpdatedBy", Applicationdatat.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedIP", Applicationdatat.UpdatedIP);
                cmd.Parameters.AddWithValue("@ComplainLetter", fileName);
                cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(Applicationdatat.ComplainTrailList[0].Remark) ? "" : Applicationdatat.ComplainTrailList[0].Remark);
                cmd.Parameters.AddWithValue("@CityName", _LoggedInUser.DistrictId > 0? _LoggedInUser.DISTRICT_MANGAL??"": "राजस्थान");
                string SerialNo = cmd.ExecuteScalar().ToString();
               
                //Applicationdatat.ComplainID = Convert.ToInt32(ComplainID).ToString();
                Result = ComplainID;

                if (Applicationdatat.SocietyId == "5"|| Applicationdatat.SocietyId == "4")
                {
                    var result = GenerateComplainLetter(Applicationdatat, SerialNo, fileName);
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
                else {
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



        public ResultMessage GenerateComplainLetter(ComplainUnderSectionModel ApplicationModel,string SerialNo, string FileName)
        {
            ResultMessage result = new ResultMessage();
            try
            {


                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string ResionName = _LoggedInUser.ResionName ?? "";
                string city = _LoggedInUser.DistrictId > 0 ? (ResionName.IndexOf("City") >0? _LoggedInUser.DISTRICT_MANGAL +"(शहर)": ResionName.IndexOf("Rural") > 0 ? _LoggedInUser.DISTRICT_MANGAL + "(ग्रामीण)" : _LoggedInUser.DISTRICT_MANGAL) : "राजस्थान";
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

                string filepath =HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles");

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
                string reportTime = ApplicationModel.IODetailList[0].DateTime.Split(' ')[1]+" "+ ApplicationModel.IODetailList[0].DateTime.Split(' ')[2]; ;
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
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियाँ, " + city  ) + "</label></b>");
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
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> अतः आपको  जरिये नोटिस निर्देशित किया जाता हैं कि उक्त प्रकरण में सुनवाई  दिनांक "+reportDate+" को "+ reportTime + "  बजे  कार्यालय "+designation+ ", सहकारी समितियाँ, " + city+"  "+address+"  पर उक्त जाँच  बिन्दुओ  के सम्बन्ध में समिति की ओर से  जवाब मय  आवश्यक रिकार्ड / दस्तावेजों के व्यत्तिगत रूप से उपस्तिथ होकर प्रस्तुत करें  अन्यथा आपके विरुद्ध अधिनियम की धारा 28(11 ) के अंतर्गत कार्यवाही प्रारम्भ करने के साथ-साथ अधिनियम की धारा 115 सहपठित धारा 151 सीपीसी के तहत्त जरिये वारंट आपकी उपस्थिति सुनिश्चित की जावेगी।  सूचत रहे।  ");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> यह नोटिस आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर से जारी किया गया। ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + ";text-align:right;'> जांच अधिकारी एवं कार्यालय-" + designation + ",<br/> सहकारी, समितियाँ, " + city );
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
                   

                    //string JSONString = string.Empty;

                    //var files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles")).OrderByDescending(d => new System.IO.FileInfo(d).CreationTime).ToArray();

                    //JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles")).OrderBy(p => p.cr));
                    

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
                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId != "3")
                {

                    string complainDate = "";
                    if (!string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].DateTime))
                    {
                        string date = Applicationdatat.IODetailList[0].DateTime.Split(' ')[0];
                        date = date.Replace("-", "/");
                        complainDate = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0]+" "+ Applicationdatat.IODetailList[0].DateTime.Split(' ')[1]+" "+ Applicationdatat.IODetailList[0].DateTime.Split(' ')[2];
                    }
                   // "2020/04/12"
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
                      cmd.Parameters.AddWithValue("@CityName",  string.IsNullOrWhiteSpace(_LoggedInUser.DISTRICT_MANGAL) ? "" : _LoggedInUser.DISTRICT_MANGAL);
                      cmd.Parameters.AddWithValue("@DateTime", complainDate);
                      cmd.Parameters.AddWithValue("@Place", string.IsNullOrWhiteSpace(IODetail.Places) ? "" : IODetail.Places);


                      SerialNo = cmd.ExecuteScalar().ToString();
                      //}
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
                    cmd.Parameters.AddWithValue("@ComplaintStatusId", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter)?3:9);
                    cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(ComplainTrail.FileName) ? "" : ComplainTrail.FileName);
                    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrWhiteSpace(ComplainTrail.Remark) ? "" : ComplainTrail.Remark);
                    cmd.Parameters.AddWithValue("@IsActive", Applicationdatat.IsActive == "True" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ComplaintDetails", "");
                    cmd.Parameters.AddWithValue("@ComplainLetter", "");
                    cmd.Parameters.AddWithValue("@IOComplainLetter", string.IsNullOrWhiteSpace(Applicationdatat.IOComplainLetter) ? "" : Applicationdatat.IOComplainLetter);
                    cmd.Parameters.AddWithValue("@IODetails", string.IsNullOrWhiteSpace(Applicationdatat.IODetailList[0].IODetails) ? "" : Applicationdatat.IODetailList[0].IODetails);

                    cmd.ExecuteScalar();
                    // }
                });

                if (Applicationdatat.ComplainTrailList[0].ComplaintStatusId != "3")
                {
                    var result = GenerateComplainLetterIO(Applicationdatat, SerialNo, fileName);
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
                objILogger1.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplianUnderSection", FunctionName = "public DataTable GetComplainTrail("+ ComplainID + ")" }, ex);
            }
            finally
            {
                objDBAccess1.CloseConnection();
                dt1.Dispose();
            }
            return dt;

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