using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cooperative.Controllers;

/// <summary>
/// Created By  :   Ritesh Khandelwal
/// Created On  :   2026-05-21, Thursday
/// Created For :   Create Common Response Helper class.
///                 Common method use into Controller methods for Sp response Data Set handle null and Total records etc.
/// REMARK      :   For optimize code and reduce same code line which are exits in several methods. Create a common reusable method and use this for reduce code.
/// </summary>

public static class ApiDataSetHelper
{
    public static void ApplyDataSetToResponse(DataSet ds, APIResponceWithTotalRec response, int pageNo, int pageSize)
    {
        if (ds == null || ds.Tables.Count == 0)
        {
            SetEmpty(response, pageNo, pageSize);
            return;
        }

        DataTable mainTable;
        DataTable totalTable;

        //-------------------------------------------------
        // SAFE TABLE DETECTION (ROBUST)
        //-------------------------------------------------
        if (ds.Tables.Count == 1)
        {
            var t = ds.Tables[0];

            if (t.Columns.Contains("TotalRecords"))
            {
                mainTable = null;
                totalTable = t;
            }
            else
            {
                mainTable = t;
                totalTable = null;
            }
        }
        else
        {
            mainTable = ds.Tables[0];
            totalTable = ds.Tables.Count > 1 ? ds.Tables[1] : null;
        }

        //-------------------------------------------------
        // MAIN DATA
        //-------------------------------------------------
        if (mainTable != null && mainTable.Rows.Count > 0)
        {
            var data = ConvertDataTable(mainTable);

            response.Status = "Success";
            response.Message = "Success";
            response.Data = data;
        }
        else
        {
            response.Status = "Success";
            response.Message = "Data Not Found";
            response.Data = null;
        }

        //-------------------------------------------------
        // TOTAL RECORDS
        //-------------------------------------------------
        response.TotalRecords = GetTotalRecords(totalTable);

        //-------------------------------------------------
        // PAGINATION
        //-------------------------------------------------
        response.PageNumber = pageNo;
        response.PageSize = pageSize;
    }

    private static List<Dictionary<string, object>> ConvertDataTable(DataTable table)
    {
        var result = new List<Dictionary<string, object>>();

        foreach (DataRow row in table.Rows)
        {
            var dict = new Dictionary<string, object>();

            foreach (DataColumn col in table.Columns)
            {
                dict[col.ColumnName] = row[col] ?? DBNull.Value;
            }

            result.Add(dict);
        }

        return result;
    }

    private static int GetTotalRecords(DataTable totalTable)
    {
        if (totalTable == null ||
            totalTable.Rows.Count == 0 ||
            !totalTable.Columns.Contains("TotalRecords"))
            return 0;

        return Convert.ToInt32(totalTable.Rows[0]["TotalRecords"] ?? 0);
    }

    private static void SetEmpty(APIResponceWithTotalRec response, int pageNo, int pageSize)
    {
        response.Status = "Success";
        response.Message = "Data Not Found";
        response.Data = null;
        response.TotalRecords = 0;
        response.PageNumber = pageNo;
        response.PageSize = pageSize;
    }
}