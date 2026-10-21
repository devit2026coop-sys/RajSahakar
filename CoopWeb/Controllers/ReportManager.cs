using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Text;
using System.Reflection;
//using Microsoft.Office.Interop.Excel;

public class ReportManager
{
	public ReportManager()
	{
	}
    //public static string CreateDataInExcel(string filePath, DataSet dataSet)
    //{
    //    Application oXL;
    //    _Workbook oWB;
    //    _Worksheet oSheet;
    //    Range oRng;
    //    try
    //    {
    //        oXL = new Application();
    //        oXL.Visible = false;
    //        oWB = (_Workbook)(oXL.Workbooks.Add(Missing.Value));
    //        oSheet = (_Worksheet)oWB.ActiveSheet;
    //        int iRow = 2;
    //        if (dataSet.Tables[0].Rows.Count > 0)
    //        {
    //            for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
    //            {
    //                oSheet.Cells[1, j + 1] = dataSet.Tables[0].Columns[j].ColumnName;
    //            }
    //            for (int rowNo = 0; rowNo < dataSet.Tables[0].Rows.Count; rowNo++)
    //            {
    //                for (int colNo = 0; colNo < dataSet.Tables[0].Columns.Count; colNo++)
    //                {
    //                    oSheet.Cells[iRow, colNo + 1] = dataSet.Tables[0].Rows[rowNo][colNo].ToString();
    //                }
    //                iRow++;
    //            }
    //        }
    //        oRng = oSheet.get_Range("A1", "IV1");
    //        oRng.EntireColumn.AutoFit();
    //        oXL.Visible = false;
    //        oXL.UserControl = false;
    //        oWB.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlShared, false, false, null, null);
    //        oWB.Close(null, null, null);
    //        oXL.Workbooks.Close();
    //        oXL.Quit();
    //        //ReleaseComObject(oRng);
    //        //ReleaseComObject(oXL);
    //        //ReleaseComObject(oSheet);
    //        //ReleaseComObject(oWB);
    //        return filePath;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    //private void ReleaseComObject(object reference)
    //{
    //    try
    //    {
    //        while (System.Runtime.InteropServices.Marshal.ReleaseComObject(reference) <= 0)
    //        {
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public static System.Data.DataTable ImportToGrid(string filePath, string extension, bool isHeader)
    {
        string conStr = "";
        string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.16.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source={0};Persist Security Info=False;Extended Properties='Excel 8.0;HDR={1}'";

        switch (extension)
        {
           
            case ".xls": //Excel 97-03
            conStr = Excel03ConString;
                break;
            case ".xlsx": //Excel 07
            conStr = Excel07ConString;
                break;
        }
        conStr = String.Format(conStr, filePath, isHeader);
        OleDbConnection conExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        System.Data.DataTable dt = new System.Data.DataTable();
        cmdExcel.Connection = conExcel;

        //Get the name of First Sheet
        conExcel.Open();
        System.Data.DataTable dtExcelSchema;
        dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        conExcel.Close();

        //Read Data from First Sheet
        conExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        conExcel.Close();
        return dt;
    }
    public static bool BulkInsertDataTable(string tableName, System.Data.DataTable dataTable)
    {
        bool isSuccuss;
        try
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString))
            {
                conn.Open();
                SqlConnection SqlConnectionObj = conn;
                SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
                isSuccuss = true;
            }
        }
        catch (Exception ex)
        {
            isSuccuss = false;
        }
        return isSuccuss;
    }
}