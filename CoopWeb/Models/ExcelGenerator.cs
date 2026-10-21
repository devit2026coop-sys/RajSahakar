using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace Cooperative.Models
{
    public class ExcelGenerator
    {
        private ILogger objILogger;
        public ExcelGenerator(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
        }
        public string GenerateExcel(DataTable dt, string fileName)
        {
            string directoryPath = HttpContext.Current.Server.MapPath("~/TempExcels/");
            string filePath = Path.Combine(directoryPath, fileName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            using (var workbook = new XLWorkbook())
            {
                if (String.IsNullOrEmpty(dt.TableName))
                    dt.TableName = "Sheet1";

                var worksheet = workbook.Worksheets.Add(dt.TableName);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j];
                    }
                }
                workbook.SaveAs(filePath);
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath);
                    foreach (string file in files)
                    {
                        DateTime creationTime = File.GetCreationTime(file);
                        TimeSpan fileAge = DateTime.Now - creationTime;
                        if (fileAge.TotalDays > 3)
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ExcelGenerator", FunctionName = "GenerateExcel(DataTable dt, string fileName)" }, ex);
                            }
                        }
                    }
                }
                return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}/TempExcels/{Path.GetFileName(filePath)}";
            }
        }
    }
}