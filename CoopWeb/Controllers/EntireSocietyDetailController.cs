using Cooperative.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Http;

namespace Cooperative.Controllers
{


    public class EntireSocietyDetailController : ApiController
    {
        #region Madhuri
        [HttpGet]

        public APIResponce GetSocietyDetailsByRegistrationNO(string Reg_no="",int Districtid=0)
         {
         
            EntireSocietyDetailModel objSocietyModel = new EntireSocietyDetailModel(new DBLogger());
            List<AdministrativeData> objData = new List<AdministrativeData>();
            List<RegistrationOfficeDetails> regoff = new List<RegistrationOfficeDetails>();
            var responce = new APIResponce();
            List<object> collection = new List<object>();
            MISModel objMISModel = new MISModel(new DBLogger());
            SocietyBasicDetail so = new SocietyBasicDetail();
            try
            {
                
                DataSet dt = (DataSet)JsonConvert.DeserializeObject(COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetSocietyDetailsByRegistrationNO(Reg_no, Districtid)), (typeof(DataSet)));


                if (dt.Tables[0].Rows.Count > 0)
                {
                    so.RegistrationNo = dt.Tables[0].Rows[0]["RegistrationNo"].ToString();
                    so.SocietyName = dt.Tables[0].Rows[0]["SocietyName"].ToString();
                    so.RegistrationDate = dt.Tables[0].Rows[0]["RegistrationDate"].ToString();
                    so.SSOID = dt.Tables[0].Rows[0]["SSOID"].ToString();
                    so.TotalNumberOfMembers = dt.Tables[0].Rows[0]["TotalNumberOfMembers"].ToString();
                    so.District = dt.Tables[0].Rows[0]["District"].ToString();
                    so.Act = dt.Tables[0].Rows[0]["Act"].ToString();
                    so.Type = dt.Tables[0].Rows[0]["Type"].ToString();
                    so.Status = dt.Tables[0].Rows[0]["Status"].ToString();

                    int cnt = dt.Tables[1].Rows.Count;

                    for (int i = 0; i < cnt; i++)
                    {
                        AdministrativeData AdministrativeData = new AdministrativeData();
                        AdministrativeData.Name = dt.Tables[1].Rows[i]["Name"].ToString();
                        AdministrativeData.ContactNo = dt.Tables[1].Rows[i]["ContactNo"].ToString();
                       // AdministrativeData.DOB = dt.Tables[1].Rows[i]["DOB"].ToString();
                        AdministrativeData.PostName = dt.Tables[1].Rows[i]["Post_Name"].ToString();
                        AdministrativeData.Occupation = dt.Tables[1].Rows[i]["Occupation"].ToString();
                        AdministrativeData.Address = dt.Tables[1].Rows[i]["Address"].ToString();
                        objData.Add(AdministrativeData);
                    }

                    int cnt1 = dt.Tables[2].Rows.Count;

                    for (int i = 0; i < cnt1; i++)
                    {
                        RegistrationOfficeDetails RegistrationOfficeDetails = new RegistrationOfficeDetails();

                        RegistrationOfficeDetails.PinCode = dt.Tables[2].Rows[i]["PinCode"].ToString();
                        RegistrationOfficeDetails.City = dt.Tables[2].Rows[i]["City"].ToString();
                        RegistrationOfficeDetails.Block = dt.Tables[2].Rows[i]["Block"].ToString();
                        RegistrationOfficeDetails.GramPanchayat = dt.Tables[2].Rows[i]["GramPanchayat"].ToString();
                        RegistrationOfficeDetails.Village = dt.Tables[2].Rows[i]["Village"].ToString();
                        RegistrationOfficeDetails.District = dt.Tables[2].Rows[i]["District"].ToString();
                        regoff.Add(RegistrationOfficeDetails);
                    }


                    so.AdministrativeData = objData;
                    so.Address = regoff;

                }
                if (dt.Tables[0].Rows.Count > 0) 
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = so;
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;


        //    return str;
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetSocietyDetailsByRegistrationNO(Reg_no));

        }

        public class AdministrativeData
        {
            //public int SocietyId { get; set; }
            //public string SocietyIdEnc { get; set; }
            //public string NameOfSociety { get; set; }
            //public string PostinSociety { get; set; }
            public string Name { get; set; }
            //public string FatherName { get; set; }
            public string ContactNo { get; set; }
            //public string AADHAARNo { get; set; }
            //public string DOB { get; set; }
            //public string AppointedDate { get; set; }
            //public string Gender { get; set; }
            //public string Photo { get; set; }
            //public string UserType { get; set; }
            //public int AdministrativeId { get; set; }
            //public int OldAdministrativeId { get; set; }
            //public bool IsActive { get; set; }
            //public string PostId { get; set; }
            //public int CategoryId { get; set; }
            //public int AdministratorDesignationId { get; set; }
            public string Address { get; set; }
            //public string MEMBERTYPE { get; set; }
            public string PostName { get; set; }
            public string Occupation { get; set; }
            //public int NominatedBy { get; set; }
            //public string BoardType { get; set; }
            //public string MembershipNo { get; set; }
            //public int ShareAllotted { get; set; }
            //public decimal ShareCapitalAmount { get; set; }
            //public int loaneeType { get; set; }
            //public int SubmitStatus { get; set; }


            //public int RequestType { get; set; }
            //public int RequestStatus { get; set; }
            //public string RemovalFile { get; set; }
            //public string RemovalType { get; set; }
            //public string SSOID { get; set; }


        }

        public class RegistrationOfficeDetails
        {

            public string PinCode { get; set; }
            public string City { get; set; }
            public string Block { get; set; }
            public string GramPanchayat { get; set; }
            public string Village { get; set; }
            public string District { get; set; }
           

        }

        public class SocietyBasicDetail
        {

   

            public string RegistrationNo { get; set; }
            public string SocietyName { get; set; }
            public string BRN { get; set; }

            public string RegistrationDate { get; set; }
            public string SSOID { get; set; }
            public string TotalNumberOfMembers { get; set; }

            public string District { get; set; }
            public string Act { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }

            public List<RegistrationOfficeDetails> Address { get; set; }
           

            public List<AdministrativeData> AdministrativeData { get; set; }

        }


        [HttpGet]
        public static void ImportExcelToSql()
        {
            string excelFilePath = @"Downloads\TT SSO ID MAPPING LIST ALL BRANCH.xlsx";
            string connectionString = "Data Source=ACRUX\\SQL2019;Initial Catalog=Coop_Rajeev;Integrated Security=False;User ID=developer;Password=P@ssw0rd123;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";

            // Load the Excel file
            FileInfo existingFile = new FileInfo(excelFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                // Loop through all worksheets in the Excel file
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null) continue; // Skip empty sheets

                    Console.WriteLine($"Importing data from sheet: {worksheet.Name}");

                    // Read data into a DataTable
                    DataTable dataTable = new DataTable(worksheet.Name);

                    // Add columns to DataTable
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dataTable.Columns.Add(worksheet.Cells[1, col].Text); // Assume the first row has headers
                    }

                    // Add rows to DataTable
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Start from the second row
                    {
                        var newRow = dataTable.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Text;
                        }
                        dataTable.Rows.Add(newRow);
                    }

                    // Insert data into SQL Server
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Use SqlBulkCopy to efficiently insert data
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                        {
                            bulkCopy.DestinationTableName = "PacsTestTable"; // Change to your target table name
                            bulkCopy.BatchSize = 1000; // Adjust as necessary

                            try
                            {
                                bulkCopy.WriteToServer(dataTable);
                                Console.WriteLine($"Successfully imported {dataTable.Rows.Count} rows from sheet: {worksheet.Name}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error importing data from sheet: {worksheet.Name}. Exception: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }



        #endregion



    }
}