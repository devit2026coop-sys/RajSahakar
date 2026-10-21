using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Cooperative.Controllers;
using Cooperative.Models;
using System.Collections.Generic;
using System.Reflection;
using static iTextSharp.text.pdf.PdfDocument;

namespace Cooperative
{
    public partial class importDataExcel : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void DownloadFile(string filePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            File.Delete(filePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        protected void btnupload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string folderPath = ConfigurationManager.AppSettings["FilesDir"];
                string filePath = Server.MapPath(folderPath + fileName);
                FileUpload1.SaveAs(filePath);
                DataTable tbl = ReportManager.ImportToGrid(filePath, extension, false);
                GridView2.DataSource = tbl;
                GridView2.DataBind();

            }
        }
        protected void btntransfer_Click_pacs(object sender, EventArgs e)
        {
            foreach (GridViewRow grow in GridView2.Rows)
            {

                CheckBox chkbox = (CheckBox)grow.FindControl("chkbox");
                //if (chkbox.Checked)
                //{
                //    SqlDataAdapter da = new SqlDataAdapter("insert into TestPacsUsers values('" + grow.Cells[3].Text + "','" + grow.Cells[4].Text + "','" + grow.Cells[5].Text + "','" + grow.Cells[6].Text + "','" + grow.Cells[7].Text + "','" + grow.Cells[8].Text + "','" + grow.Cells[9].Text + "','" + grow.Cells[10].Text + "','" + grow.Cells[11].Text + "','" + grow.Cells[12].Text + "','" + grow.Cells[13].Text + "','" + grow.Cells[14].Text + "','" + grow.Cells[15].Text + "')", con);
                //    DataSet ds = new DataSet();
                //    da.Fill(ds);
                //}


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString))
                {
                    SqlConnection sqlConnection = con;
                    sqlConnection.Open();
                    //con.Open();

                    // Define the SQL INSERT command
                    //string insertQuery = "INSERT INTO TestPacsUsers (ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, ColumnName6, ColumnName7, ColumnName8, ColumnName9, ColumnName10, ColumnName11, ColumnName12, ColumnName13, ColumnName14, ColumnName15) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11, @Value12, @Value13)";
                    string insertQuery = "INSERT INTO TestPacsUsers (DISTRICT,BANKNAME,BRANCH_ID,BRANCH_NAME,PACS_ID,PACS_DESC,PACS_manager,MobileNoPACSManager,Email,SSOIDPACSManager,BranchManager,MobileNoBranchManager,SSOIDBranchManager) " +
                        // "VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11, @Value12, @Value13)";
                        "VALUES (@DISTRICT, @BANKNAME, @BRANCH_ID, @BRANCH_NAME, @PACS_ID, @PACS_DESC, @PACS_manager, @MobileNoPACSManager,  @Email,@SSOIDPACSManager, @BranchManager, @MobileNoBranchManager, @SSOIDBranchManager)";//email in last

                    if (chkbox.Checked)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            // Add parameters
                            cmd.Parameters.AddWithValue("@DISTRICT", grow.Cells[2].Text);
                            cmd.Parameters.AddWithValue("@BANKNAME", grow.Cells[3].Text);
                            cmd.Parameters.AddWithValue("@BRANCH_ID", grow.Cells[4].Text);
                            cmd.Parameters.AddWithValue("@BRANCH_NAME", grow.Cells[5].Text);
                            cmd.Parameters.AddWithValue("@PACS_ID", grow.Cells[6].Text);
                            cmd.Parameters.AddWithValue("@PACS_DESC", grow.Cells[7].Text);
                            cmd.Parameters.AddWithValue("@PACS_manager", grow.Cells[8].Text);
                            cmd.Parameters.AddWithValue("@MobileNoPACSManager", grow.Cells[9].Text);



                            //cmd.Parameters.AddWithValue("@Email", grow.Cells[10].Text);
                            //cmd.Parameters.AddWithValue("@SSOIDPACSManager", grow.Cells[11].Text);
                            //cmd.Parameters.AddWithValue("@BranchManager", grow.Cells[12].Text);
                            //cmd.Parameters.AddWithValue("@MobileNoBranchManager", grow.Cells[13].Text);
                            //cmd.Parameters.AddWithValue("@SSOIDBranchManager", grow.Cells[14].Text);

                            cmd.Parameters.AddWithValue("@Email", grow.Cells[14].Text);
                            cmd.Parameters.AddWithValue("@SSOIDPACSManager", grow.Cells[10].Text);
                            cmd.Parameters.AddWithValue("@BranchManager", grow.Cells[11].Text);
                            cmd.Parameters.AddWithValue("@MobileNoBranchManager", grow.Cells[12].Text);
                            cmd.Parameters.AddWithValue("@SSOIDBranchManager", grow.Cells[13].Text);






                            // Execute the command
                            cmd.ExecuteNonQuery();
                            //cmd.ExecuteScalar();
                        }
                    }
                }

            }
        }
        protected void btntransfer_Click_MahilaNidhi(object sender, EventArgs e)
        {
            foreach (GridViewRow grow in GridView2.Rows)
            {

                CheckBox chkbox = (CheckBox)grow.FindControl("chkbox");
                //if (chkbox.Checked)
                //{
                //    SqlDataAdapter da = new SqlDataAdapter("insert into TestPacsUsers values('" + grow.Cells[3].Text + "','" + grow.Cells[4].Text + "','" + grow.Cells[5].Text + "','" + grow.Cells[6].Text + "','" + grow.Cells[7].Text + "','" + grow.Cells[8].Text + "','" + grow.Cells[9].Text + "','" + grow.Cells[10].Text + "','" + grow.Cells[11].Text + "','" + grow.Cells[12].Text + "','" + grow.Cells[13].Text + "','" + grow.Cells[14].Text + "','" + grow.Cells[15].Text + "')", con);
                //    DataSet ds = new DataSet();
                //    da.Fill(ds);
                //}


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString))
                {
                    SqlConnection sqlConnection = con;
                    sqlConnection.Open();
                    //con.Open();

                    // Define the SQL INSERT command
                    //string insertQuery = "INSERT INTO TestPacsUsers (ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, ColumnName6, ColumnName7, ColumnName8, ColumnName9, ColumnName10, ColumnName11, ColumnName12, ColumnName13, ColumnName14, ColumnName15) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11, @Value12, @Value13)";

                    // string insertQuery = "INSERT INTO MahilaNidhiData (ColumnName2,ColumnName3,ColumnName4,ColumnName5,ColumnName6,ColumnName7,ColumnName8,ColumnName9,ColumnName10,ColumnName11,ColumnName12,ColumnName13,ColumnName14,ColumnName15,ColumnName16,ColumnName17,ColumnName18,ColumnName19,ColumnName20,ColumnName21,ColumnName22,ColumnName23,ColumnName24,ColumnName25,ColumnName26,ColumnName27,ColumnName28,ColumnName29,ColumnName30,ColumnName31,ColumnName32,ColumnName33,ColumnName34,ColumnName35,ColumnName36) " +


                    string insertQuery = "INSERT INTO MahilaNidhiData ( RM_Name, AADHAAR_ID, DISTRICT_ID, DISTRICT_NAME, BLOCK_ID, BLOCK_NAME, CLUSTER_ID, CLUSTER_NAME, VO_ID, VO_Name, SHG_ID, SHG_NAME, MEMBER_ID, MEMBER_NAME, FATHER_HUSBAND_NAME, ActualPurpose, MOBILE_NUM, SHG_AccountNo, BRANCH_CODE, LOAN_AMOUNT, Status, ApplyedLoan, BANK_SENT_DATE, PROJECT_TYPE, JanAadharNo, First_Witness_Name, First_Witness_FatherName, First_Witness_AadharNo, Second_Witness_Name, Second_Witness_FatherName, Second_Witness_AadharNo, COOP_Status, IsActive, IsDeleted, CeratedDate, CeratedBy)" +


                     "VALUES (@Value2,@Value3,@Value4,@Value5,@Value6,@Value7,@Value8,@Value9,@Value10,@Value11,@Value12,@Value13,@Value14,@Value15,@Value16,@Value17,@Value18,@Value19,@Value20,@Value21,@Value22,@Value23,@Value24,@Value25,@Value26,@Value27,@Value28,@Value29,@Value30,@Value31,@Value32,@Value33,@Value34,@Value35,@Value36,@Value37)";


                    if (chkbox.Checked)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            // Add parameters
                            // cmd.Parameters.AddWithValue("@Value1", grow.Cells[1].Text);
                            cmd.Parameters.AddWithValue("@Value2", grow.Cells[2].Text);
                            cmd.Parameters.AddWithValue("@Value3", grow.Cells[3].Text);
                            cmd.Parameters.AddWithValue("@Value4", grow.Cells[4].Text);
                            cmd.Parameters.AddWithValue("@Value5", grow.Cells[5].Text);
                            cmd.Parameters.AddWithValue("@Value6", grow.Cells[6].Text);
                            cmd.Parameters.AddWithValue("@Value7", grow.Cells[7].Text);
                            cmd.Parameters.AddWithValue("@Value8", grow.Cells[8].Text);
                            cmd.Parameters.AddWithValue("@Value9", grow.Cells[9].Text);
                            cmd.Parameters.AddWithValue("@Value10", grow.Cells[10].Text);
                            cmd.Parameters.AddWithValue("@Value11", grow.Cells[11].Text);
                            cmd.Parameters.AddWithValue("@Value12", grow.Cells[12].Text);
                            cmd.Parameters.AddWithValue("@Value13", grow.Cells[13].Text);
                            cmd.Parameters.AddWithValue("@Value14", grow.Cells[14].Text);
                            cmd.Parameters.AddWithValue("@Value15", grow.Cells[15].Text);
                            cmd.Parameters.AddWithValue("@Value16", grow.Cells[16].Text);
                            cmd.Parameters.AddWithValue("@Value17", grow.Cells[17].Text);
                            cmd.Parameters.AddWithValue("@Value18", grow.Cells[18].Text);
                            cmd.Parameters.AddWithValue("@Value19", grow.Cells[19].Text);
                            cmd.Parameters.AddWithValue("@Value20", grow.Cells[20].Text);
                            cmd.Parameters.AddWithValue("@Value21", grow.Cells[21].Text);
                            cmd.Parameters.AddWithValue("@Value22", grow.Cells[22].Text);
                            cmd.Parameters.AddWithValue("@Value23", grow.Cells[23].Text);
                            cmd.Parameters.AddWithValue("@Value24", grow.Cells[24].Text);
                            cmd.Parameters.AddWithValue("@Value25", grow.Cells[25].Text);
                            cmd.Parameters.AddWithValue("@Value26", grow.Cells[26].Text);
                            cmd.Parameters.AddWithValue("@Value27", grow.Cells[27].Text);
                            cmd.Parameters.AddWithValue("@Value28", grow.Cells[28].Text);
                            cmd.Parameters.AddWithValue("@Value29", grow.Cells[29].Text);
                            cmd.Parameters.AddWithValue("@Value30", grow.Cells[30].Text);
                            cmd.Parameters.AddWithValue("@Value31", grow.Cells[31].Text);
                            cmd.Parameters.AddWithValue("@Value32", grow.Cells[32].Text);
                            cmd.Parameters.AddWithValue("@Value33", grow.Cells[33].Text);
                            cmd.Parameters.AddWithValue("@Value34", grow.Cells[34].Text);
                            cmd.Parameters.AddWithValue("@Value35", grow.Cells[35].Text);
                            cmd.Parameters.AddWithValue("@Value36", grow.Cells[36].Text);
                            cmd.Parameters.AddWithValue("@Value37", grow.Cells[37].Text);




                            //cmd.Parameters.AddWithValue("@Value2", grow.Cells[2].Text);
                            //cmd.Parameters.AddWithValue("@DISTRICT", grow.Cells[2].Text);
                            //cmd.Parameters.AddWithValue("@BANKNAME", grow.Cells[3].Text);
                            //cmd.Parameters.AddWithValue("@BRANCH_ID", grow.Cells[4].Text);
                            //cmd.Parameters.AddWithValue("@BRANCH_NAME", grow.Cells[5].Text);
                            //cmd.Parameters.AddWithValue("@PACS_ID", grow.Cells[6].Text);
                            //cmd.Parameters.AddWithValue("@PACS_DESC", grow.Cells[7].Text);
                            //cmd.Parameters.AddWithValue("@PACS_manager", grow.Cells[8].Text);
                            //cmd.Parameters.AddWithValue("@MobileNoPACSManager", grow.Cells[9].Text);



                            ////cmd.Parameters.AddWithValue("@Email", grow.Cells[10].Text);
                            ////cmd.Parameters.AddWithValue("@SSOIDPACSManager", grow.Cells[11].Text);
                            ////cmd.Parameters.AddWithValue("@BranchManager", grow.Cells[12].Text);
                            ////cmd.Parameters.AddWithValue("@MobileNoBranchManager", grow.Cells[13].Text);
                            ////cmd.Parameters.AddWithValue("@SSOIDBranchManager", grow.Cells[14].Text);

                            //cmd.Parameters.AddWithValue("@Email", grow.Cells[14].Text);
                            //cmd.Parameters.AddWithValue("@SSOIDPACSManager", grow.Cells[10].Text);
                            //cmd.Parameters.AddWithValue("@BranchManager", grow.Cells[11].Text);
                            //cmd.Parameters.AddWithValue("@MobileNoBranchManager", grow.Cells[12].Text);
                            //cmd.Parameters.AddWithValue("@SSOIDBranchManager", grow.Cells[13].Text);






                            // Execute the command
                            cmd.ExecuteNonQuery();
                            //cmd.ExecuteScalar();
                        }
                    }
                }

            }
        }
        private DateTime? TryParseDate(string dateString)
        {
            DateTime parsedDate;
            string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss" , "dd.MM.yyyy","dd/M/yyyy" };

            if (DateTime.TryParseExact(dateString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            return null;
        }
        protected void btntransfer_Click_SLDB(object sender, EventArgs e)
        {
            foreach (GridViewRow grow in GridView2.Rows)
            {
                CheckBox chkbox = (CheckBox)grow.FindControl("chkbox");          
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString))
                {
                    SqlConnection sqlConnection = con;
                    sqlConnection.Open();
                  
                    string insertQuery = "INSERT INTO PLDB_OTS_SchemeData ( PLDBName,PLDBBranch,ApplicantName,FatherHusbandName,JanAadharNumber,MobileNo,Gender,VName,GpName,BlockName,TehsilName,VidhanSabha,DistrictName,CategoryName,NomineeName,NomineeJanAadharNumber,NomineeMobileNo,LoanAccNo,NoOfLoanBen,Sector,Purpose,LoanSanctionAmt,DateOfSanction,RateOfInterest,DateOfFirstDisbursement,TotalDisbursedAmt,IsLandMortgaged,LandMortgagedland,OtherPropertyDetails,CurrentDlcValueOfProperty,DateOfOD,ODPrincipal,ODInsurancePremium,ODInterest,ODPenalInterast,ODTotal,NODPrincipal,NODInsurancePremium,NODInterest,NODPenalInterest,NODTotal,CDPrincipal,CDInterest,CDPenalInterest,CDTotalInterest,CDTotal,PrincipalOutstandingODNOD,TotalLoanOutstanding,LegalActionStageCompletedDate,LegalActionStageCompletedAction,DateOfSettlement,PrincipalOutstandingAsOnDateSettlement,TotalInterestAsOnDateSettlement,TotalLoanOutstandingAsOnDateSettlement,InterestFromSettlementPre,InterestFromSettlement,TotalAmount,IsNiActLegalActionTaken,CurrentStatusOfNiActCaseNotice)" +
                                         "VALUES (@Value2,@Value3,@Value4,@Value5,@Value6,@Value7,@Value8,@Value9,@Value10,@Value11,@Value12,@Value13,@Value14,@Value15,@Value16,@Value17,@Value18,@Value19,@Value20,@Value21,@Value22,@Value23,@Value24,@Value25,@Value26,@Value27,@Value28,@Value29,@Value30,@Value31,@Value32,@Value33,@Value34,@Value35,@Value36,@Value37,@Value38,@Value39,@Value40,@Value41,@Value42,@Value43,@Value44,@Value45,@Value46,@Value47,@Value48,@Value49,@Value50,@Value51,@Value52,@Value53,@Value54,@Value55,@Value56,@Value57,@Value58,@Value59,@Value60)";

                    if (chkbox.Checked)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            // Add parameters
                            // cmd.Parameters.AddWithValue("@Value1", grow.Cells[1].Text);
                            cmd.Parameters.AddWithValue("@Value2", grow.Cells[2].Text != "&nbsp;" ? grow.Cells[2].Text : "");

                            if (grow.Cells[2].Text == "" || grow.Cells[2].Text == "&nbsp;" || grow.Cells[3].Text == "" || grow.Cells[3].Text == "&nbsp;")
                            {
                                return;
                            }

                            cmd.Parameters.AddWithValue("@Value3", grow.Cells[3].Text != "&nbsp;" ? grow.Cells[3].Text : "");
                            cmd.Parameters.AddWithValue("@Value4", grow.Cells[4].Text != "&nbsp;" ? grow.Cells[4].Text : "");
                            cmd.Parameters.AddWithValue("@Value5", grow.Cells[5].Text != "&nbsp;" ? grow.Cells[5].Text : "");

                            cmd.Parameters.AddWithValue("@Value6", string.IsNullOrWhiteSpace(grow.Cells[6].Text) || grow.Cells[6].Text == "&nbsp;" ? "" : grow.Cells[6].Text);
                            //cmd.Parameters.AddWithValue("@Value6", grow.Cells[6].Text.ToString() ? "&nbsp;" : "");

                            cmd.Parameters.AddWithValue("@Value7", string.IsNullOrWhiteSpace(grow.Cells[7].Text) || grow.Cells[7].Text == "&nbsp;" ? "" : grow.Cells[7].Text);
                            cmd.Parameters.AddWithValue("@Value8", grow.Cells[8].Text != "&nbsp;" ? grow.Cells[8].Text.Length > 0 ? grow.Cells[8].Text.Substring(0, 1) :"": "");
                            cmd.Parameters.AddWithValue("@Value9", grow.Cells[9].Text != "&nbsp;" ? grow.Cells[9].Text : "");
                            cmd.Parameters.AddWithValue("@Value10", grow.Cells[10].Text != "&nbsp;" ? grow.Cells[10].Text : "");
                            cmd.Parameters.AddWithValue("@Value11", grow.Cells[11].Text != "&nbsp;" ? grow.Cells[11].Text : "");
                            cmd.Parameters.AddWithValue("@Value12", grow.Cells[12].Text != "&nbsp;" ? grow.Cells[12].Text : "");
                            cmd.Parameters.AddWithValue("@Value13", grow.Cells[13].Text != "&nbsp;" ? grow.Cells[13].Text : "");
                            cmd.Parameters.AddWithValue("@Value14", grow.Cells[14].Text != "&nbsp;" ? grow.Cells[14].Text : "");
                            cmd.Parameters.AddWithValue("@Value15", grow.Cells[15].Text != "&nbsp;" ? grow.Cells[15].Text : "");
                            cmd.Parameters.AddWithValue("@Value16", string.IsNullOrWhiteSpace(grow.Cells[16].Text) || grow.Cells[16].Text == "&nbsp;" ? "" : grow.Cells[16].Text);


                            cmd.Parameters.AddWithValue("@Value17", string.IsNullOrWhiteSpace(grow.Cells[17].Text) || grow.Cells[17].Text == "&nbsp;" ? "" : grow.Cells[17].Text);
                            cmd.Parameters.AddWithValue("@Value18", string.IsNullOrWhiteSpace(grow.Cells[18].Text) || grow.Cells[18].Text == "&nbsp;" ? "" : grow.Cells[18].Text);
                            // cmd.Parameters.AddWithValue("@Value18", grow.Cells[18].Text);
                            cmd.Parameters.AddWithValue("@Value19", grow.Cells[19].Text);//Loan A/c No.
                            cmd.Parameters.AddWithValue("@Value20", grow.Cells[20].Text != "&nbsp;" ? grow.Cells[20].Text : "0");//No. of Loanee / beneficiaries

                            cmd.Parameters.AddWithValue("@Value21", grow.Cells[21].Text);
                            cmd.Parameters.AddWithValue("@Value22", grow.Cells[22].Text);
                            cmd.Parameters.AddWithValue("@Value23", grow.Cells[23].Text);


                            // cmd.Parameters.AddWithValue("@Value24", grow.Cells[24].Text);
                            cmd.Parameters.AddWithValue("@Value24", !string.IsNullOrEmpty(grow.Cells[24].Text) && grow.Cells[24].Text != "0" && grow.Cells[24].Text != "&nbsp;" && grow.Cells[24].Text != "NIL" ? TryParseDate(grow.Cells[24].Text) : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Value25", grow.Cells[25].Text);

                            // cmd.Parameters.AddWithValue("@Value26", grow.Cells[26].Text);
                            cmd.Parameters.AddWithValue("@Value26", !string.IsNullOrEmpty(grow.Cells[26].Text) && grow.Cells[26].Text != "0" && grow.Cells[26].Text != "&nbsp;" && grow.Cells[26].Text != "NIL" ? TryParseDate(grow.Cells[26].Text) : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Value27", grow.Cells[27].Text);
                            cmd.Parameters.AddWithValue("@Value28", grow.Cells[28].Text == "Yes" || grow.Cells[28].Text == "Y" ? 'Y' : 'N');
                            cmd.Parameters.AddWithValue("@Value29", grow.Cells[29].Text != "&nbsp;" ? grow.Cells[29].Text:"");
                            cmd.Parameters.AddWithValue("@Value30", grow.Cells[30].Text != "&nbsp;" ?  grow.Cells[30].Text:"0");
                            cmd.Parameters.AddWithValue("@Value31", grow.Cells[31].Text != "&nbsp;" ? grow.Cells[31].Text : "0");


                            cmd.Parameters.AddWithValue("@Value32", !string.IsNullOrEmpty(grow.Cells[32].Text) && grow.Cells[32].Text != "0" && grow.Cells[32].Text != "&nbsp;" && grow.Cells[32].Text != "NIL" ? TryParseDate(grow.Cells[32].Text) : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Value33", grow.Cells[33].Text != "&nbsp;" ? grow.Cells[33].Text : "0");
                            cmd.Parameters.AddWithValue("@Value34", !string.IsNullOrEmpty(grow.Cells[34].Text) && grow.Cells[34].Text != "&nbsp;" ? grow.Cells[34].Text : "0");//insurance Premium
                            cmd.Parameters.AddWithValue("@Value35", grow.Cells[35].Text != "&nbsp;" ? grow.Cells[35].Text : "0");
                            cmd.Parameters.AddWithValue("@Value36", grow.Cells[36].Text != "&nbsp;" ? grow.Cells[36].Text : "0");
                            cmd.Parameters.AddWithValue("@Value37", grow.Cells[37].Text != "&nbsp;" ? grow.Cells[37].Text : "0");

                            cmd.Parameters.AddWithValue("@Value38", grow.Cells[38].Text != "&nbsp;" ? grow.Cells[38].Text : "0");
                            cmd.Parameters.AddWithValue("@Value39", !string.IsNullOrEmpty(grow.Cells[39].Text) && grow.Cells[39].Text != "&nbsp;" ? grow.Cells[39].Text : "0"); //insurance Premium
                            cmd.Parameters.AddWithValue("@Value40", grow.Cells[40].Text != "&nbsp;" ? grow.Cells[40].Text : "0");
                            cmd.Parameters.AddWithValue("@Value41", grow.Cells[41].Text != "&nbsp;" ? grow.Cells[41].Text : "0");
                            cmd.Parameters.AddWithValue("@Value42", grow.Cells[42].Text != "&nbsp;" ? grow.Cells[42].Text : "0");
                            cmd.Parameters.AddWithValue("@Value43", grow.Cells[43].Text != "&nbsp;" ? grow.Cells[43].Text : "0");
                            cmd.Parameters.AddWithValue("@Value44", grow.Cells[44].Text != "&nbsp;" ? grow.Cells[44].Text : "0");
                            cmd.Parameters.AddWithValue("@Value45", grow.Cells[45].Text != "&nbsp;" ? grow.Cells[45].Text : "0");
                            cmd.Parameters.AddWithValue("@Value46", grow.Cells[46].Text != "&nbsp;" ? grow.Cells[46].Text : "0");
                            cmd.Parameters.AddWithValue("@Value47", grow.Cells[47].Text != "&nbsp;" ? grow.Cells[47].Text : "0");
                            cmd.Parameters.AddWithValue("@Value48", grow.Cells[48].Text != "&nbsp;" ? grow.Cells[48].Text : "0");
                            cmd.Parameters.AddWithValue("@Value49", grow.Cells[49].Text != "&nbsp;" ? grow.Cells[49].Text : "0");

                            cmd.Parameters.AddWithValue("@Value50", !string.IsNullOrEmpty(grow.Cells[50].Text) && grow.Cells[50].Text != "0" && grow.Cells[50].Text != "&nbsp;" && grow.Cells[50].Text != "_" && grow.Cells[50].Text != "-" && grow.Cells[50].Text != "NIL" ? TryParseDate(grow.Cells[50].Text) : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Value51", grow.Cells[51].Text != "&nbsp;" ? grow.Cells[51].Text : "0");
                            cmd.Parameters.AddWithValue("@Value52", !string.IsNullOrEmpty(grow.Cells[52].Text) && grow.Cells[52].Text != "0" && grow.Cells[52].Text != "&nbsp;" && grow.Cells[52].Text != "NIL" && grow.Cells[52].Text != "NO" ? grow.Cells[52].Text : (object)DBNull.Value);
                            // cmd.Parameters.AddWithValue("@Value52", grow.Cells[52].Text);
                            cmd.Parameters.AddWithValue("@Value53", !string.IsNullOrEmpty(grow.Cells[53].Text) && grow.Cells[53].Text != "&nbsp;" && grow.Cells[53].Text != "NIL" && grow.Cells[53].Text != "NO" ? grow.Cells[53].Text : "0");
                            cmd.Parameters.AddWithValue("@Value54", !string.IsNullOrEmpty(grow.Cells[54].Text) && grow.Cells[54].Text != "&nbsp;" && grow.Cells[54].Text != "NIL" && grow.Cells[54].Text != "NO" ? grow.Cells[54].Text : "0");
                            cmd.Parameters.AddWithValue("@Value55", !string.IsNullOrEmpty(grow.Cells[55].Text) && grow.Cells[55].Text != "&nbsp;" && grow.Cells[55].Text != "NIL" && grow.Cells[55].Text != "NO" ? grow.Cells[55].Text : "0");
                            cmd.Parameters.AddWithValue("@Value56", !string.IsNullOrEmpty(grow.Cells[56].Text) && grow.Cells[56].Text != "&nbsp;" && grow.Cells[56].Text != "NIL" && grow.Cells[56].Text != "NO" ? grow.Cells[56].Text : "0");
                            cmd.Parameters.AddWithValue("@Value57", !string.IsNullOrEmpty(grow.Cells[57].Text) && grow.Cells[57].Text != "&nbsp;" && grow.Cells[57].Text != "NIL" && grow.Cells[57].Text != "NO" ? grow.Cells[57].Text : "0");
                            cmd.Parameters.AddWithValue("@Value58", !string.IsNullOrEmpty(grow.Cells[58].Text) && grow.Cells[58].Text != "&nbsp;" && grow.Cells[58].Text != "NIL" && grow.Cells[58].Text != "NO" ? grow.Cells[58].Text : "0");
                            cmd.Parameters.AddWithValue("@Value59", grow.Cells[59].Text == "Yes" || grow.Cells[59].Text == "Y" ? 'Y' : 'N');
                            cmd.Parameters.AddWithValue("@Value60", grow.Cells[60].Text != "&nbsp;" ? grow.Cells[60].Text : "");


                            // Execute the command
                            cmd.ExecuteNonQuery();
                            //cmd.ExecuteScalar();
                        }
                    }
                }
            }
        }

        protected void btntransfer_Click_SLDB_Users(object sender, EventArgs e)
        {
            foreach (GridViewRow grow in GridView2.Rows)
            {
                CheckBox chkbox = (CheckBox)grow.FindControl("chkbox");
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString))
                {
                    SqlConnection sqlConnection = con;
                    sqlConnection.Open();

                    string insertQuery = "INSERT INTO Pldb_UsersDataExcel ( DISTRICT,BLOCK_NAME,PLDB_NAME,PLDB_BRANCH_NAME,DESIGNATION,NAME,Mobile_No,EMAIL_ID,SSO_ID)" +
                     "VALUES (@DISTRICT,@BLOCK_NAME,@PLDB_NAME,@PLDB_BRANCH_NAME,@DESIGNATION,@NAME,@MOBILE_NO,@EMAIL_ID,@SSO_ID)";

                    if (chkbox.Checked)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {

                            cmd.Parameters.AddWithValue("@DISTRICT", grow.Cells[2].Text != "&nbsp;" ? grow.Cells[2].Text : "");

                            //if (grow.Cells[4].Text == "" || grow.Cells[4].Text == "&nbsp;" || grow.Cells[5].Text == "" || grow.Cells[5].Text == "&nbsp;")
                            //{
                            //    return;
                            //}
                            cmd.Parameters.AddWithValue("@BLOCK_NAME", grow.Cells[3].Text != "&nbsp;" ? grow.Cells[3].Text : "");
                            cmd.Parameters.AddWithValue("@PLDB_NAME", grow.Cells[4].Text != "&nbsp;" ? grow.Cells[4].Text : "");
                            cmd.Parameters.AddWithValue("@PLDB_BRANCH_NAME", grow.Cells[5].Text != "&nbsp;" ? grow.Cells[5].Text : "");
                            cmd.Parameters.AddWithValue("@DESIGNATION", grow.Cells[6].Text != "&nbsp;" ? grow.Cells[6].Text : "");
                            cmd.Parameters.AddWithValue("@NAME", grow.Cells[7].Text == "&nbsp;" ? "" : grow.Cells[7].Text);
                            cmd.Parameters.AddWithValue("@MOBILE_NO", grow.Cells[8].Text != "&nbsp;" ? grow.Cells[8].Text : "");
                            cmd.Parameters.AddWithValue("@EMAIL_ID", grow.Cells[9].Text != "&nbsp;" ? grow.Cells[9].Text : "");
                            cmd.Parameters.AddWithValue("@SSO_ID", grow.Cells[10].Text != "&nbsp;" ? grow.Cells[10].Text : "");

                            cmd.ExecuteNonQuery();

                        }
                    }
                }

            }
        }

    }

}