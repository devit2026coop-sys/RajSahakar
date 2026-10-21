using Cooperative.Controllers;
using Cooperative.DBConnector;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Cooperative.Models.GCCServiceModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using org.w3c.dom.css;
using System.ComponentModel.Composition.Primitives;


namespace Cooperative.Models
{
    public class CoopMobileService
    {

        public string file_flag = "";
        public string debit_account = "";
        public string debit_amount = "";
        public string debit_ifsc = "";
        public string Payids = "", dir = "", FileName = "";
        public string debit_bank_name = "";
        public string merchantCode = "";
        public string lblErrorMessageOTP = "";
        public string PayfileName = "";
        int total_record = 0, InsertRpp = 0;
        double total_fileAmount = 0;
        private ILogger objILogger;


        private DBFactoryMSSQL objDBAccess;
        decimal CA_Amt = 0, SF_Amt = 0, TOPUP_Amt = 0, TotalCalcSize;
        csCommon Objsbm2 = new csCommon();
        public CoopMobileService()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public string GetAuditCounts(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCountSocietyAssignedToAuditor_v1", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, obj.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, obj.FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@Division_Id", DBFactoryParameterType.NVarchar, obj.DivisionId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, obj.Month);
                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.NVarchar, obj.IsEmpanelledCA);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "GetAuditCounts(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }

        public string getMemberInfo(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("getSocietyMemberInfo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "getMemberInfo(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }

        public string saveGeotagInfo(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("saveSocietygeotagInfo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                objDBAccess.AddParameterToCommand("@PresidentName", DBFactoryParameterType.NVarchar, obj.PresidentName);
                objDBAccess.AddParameterToCommand("@PresidentContact", DBFactoryParameterType.NVarchar, obj.PresidentContact);
                objDBAccess.AddParameterToCommand("@SecretaryName", DBFactoryParameterType.NVarchar, obj.SecretaryName);
                objDBAccess.AddParameterToCommand("@SecretaryContact", DBFactoryParameterType.NVarchar, obj.SecretaryContact);
                objDBAccess.AddParameterToCommand("@ChargeType", DBFactoryParameterType.NVarchar, obj.ChargeType);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, obj.Status);
                objDBAccess.AddParameterToCommand("@Latitude", DBFactoryParameterType.NVarchar, obj.Latitude);
                objDBAccess.AddParameterToCommand("@longitude", DBFactoryParameterType.NVarchar, obj.longitude);
                objDBAccess.AddParameterToCommand("@CaptureImage", DBFactoryParameterType.NVarchar, obj.CaptureImage);
                objDBAccess.AddParameterToCommand("@location", DBFactoryParameterType.NVarchar, obj.location);
                objDBAccess.AddParameterToCommand("@usertype", DBFactoryParameterType.NVarchar, obj.usertype);
                objDBAccess.AddParameterToCommand("@ssoid", DBFactoryParameterType.NVarchar, obj.insertbyssoid);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "saveGeotagInfo(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }

        public string blockbyDistrictId(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("blockbyDistrictId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Int, obj.isRural);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "saveGeotagInfo(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }

        public string SocietiesbyBlockid(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietiesbyBlockid", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.Blockid);
                objDBAccess.AddParameterToCommand("@isRural", DBFactoryParameterType.Int, obj.isRural);
                objDBAccess.AddParameterToCommand("@cityId", DBFactoryParameterType.Int, obj.cityId);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "saveGeotagInfo(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }

        public string GetAppversionByName(MobileServiceEntity obj)
        {

            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAppversionByName", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AppName", DBFactoryParameterType.Int, obj.appname);
                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


               
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "GetAppversionByName(" + ex + ")" }, ex);

                // insert row values
                urldt.Rows.Add(new Object[]{
                                "Please try again",1,
                                "Please try again"
                                });
                json = COMMON_FUNCTION.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            return json;
        }


        public string GetAuditdetail(MobileServiceEntity obj)
        {
            string json = string.Empty;
            DataSet ds = new DataSet();
            var urldt = new System.Data.DataTable("tableName");
            urldt.Columns.Add("message", typeof(string));
            urldt.Columns.Add("status", typeof(int));
            urldt.Columns.Add("data", typeof(string));
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditdetailAPI", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, obj.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, obj.whereCase);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, obj.FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, obj.Month); //20-6-2025
                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.NVarchar, obj.IsEmpanelledCA);//20-6-2025
                objDBAccess.AddParameterToCommand("@filtertype", DBFactoryParameterType.NVarchar, obj.filtertype);

                ds = objDBAccess.GetDataSet();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    json = csCommon.DataTableToJSONString(ds.Tables[0]);
                    // create table
                    urldt.Rows.Add(new Object[]{
                                "success",0,
                               json
                                });

                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }
                else
                {
                    // insert row values
                    urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                    json = csCommon.DataTableToJSONString(urldt);
                    json = json.Replace("\"[", "[").Replace("]\"", "]");
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "CoopMobileService", FunctionName = "GetAuditdetail(" + ex + ")" }, ex);
                urldt.Rows.Add(new Object[]{
                                "record not exists",1,
                                "record not exists"
                                });
                json = csCommon.DataTableToJSONString(urldt);
                json = json.Replace("\"[", "[").Replace("]\"", "]");
            }
            objDBAccess.CloseConnection();
            return json;
        }


    }

    public class MobileServiceEntity
    {

        public int userid { get; set; }
        public int? roleid { get; set; }

        public string FIN_YEAR_NAME { get; set; }

        public string DistrictId { get; set; }
        public string DivisionId { get; set; }
        public int Month { get; set; }
        public string IsEmpanelledCA { get; set; }

        public int UserRoleDeptMappingId { get; set; }

        public string whereCase { get; set; }

        public string filtertype { get; set; }

        public int SocietyId { get; set; }

        public string PresidentName { get; set; }
        public string PresidentContact { get; set; }
        public string SecretaryName { get; set; }
        public string SecretaryContact { get; set; }
        public string ChargeType { get; set; }
        public string Status { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public string CaptureImage { get; set; }

        public string location { get; set; }

        public string Blockid { get; set; }

        public string usertype { get; set; }

        public string insertbyssoid { get; set; }

        public int isRural { get; set; }
        public int cityId { get; set; }

        public string appname { get; set; }


    }


}