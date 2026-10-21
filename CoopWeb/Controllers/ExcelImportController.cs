//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Web;
//using System.Web.Mvc;
//using Cooperative.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using Cooperative.DBConnector;
using OfficeOpenXml; // Make sure you have EPPlus installed
//using Cooperative.DBConnector;
//using System;
//using System.Data;
//using System.Data.SqlClient;
using System.Web.Http;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;


namespace Cooperative.Controllers
{
    public class ExcelImportController :  ApiController
    {
        private DBFactoryMSSQL objDBAccess;
        // private readonly string connectionString = "Data Source=ACRUX\\SQL2019;Initial Catalog=Coop_Rajeev;Integrated Security=False;User ID=developer;Password=P@ssw0rd123;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        //static void Main(string[] args)
        //{

        //    string excelFilePath = @"Downloads\TT SSO ID MAPPING LIST ALL BRANCH.xlsx"; // Path to your Excel file
        //                                                                                // string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"; // SQL Server connection string

        //    // Call the method to read Excel and insert into SQL Server
        //    ImportExcelToSql(excelFilePath, connectionString);
        //}
        [System.Web.Http.HttpGet]
        public static void ImportExcelToSql()
        {
            string excelFilePath= @"Downloads\TT SSO ID MAPPING LIST ALL BRANCH.xlsx";
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
    }
}