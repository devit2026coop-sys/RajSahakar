﻿using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class FertilizerSupplyModel
    {

    }
    public class GSSDemandModel
    {
        public int SocietyId { get; set; }
        public int KVSSId { get; set; }
        public int DemandId { get; set; }
        public int RequestId { get; set; }
        public int FertilizerId { get; set; }
        public float Quantity { get; set; }
        public string SSOId { get; set; }
        public string Request_No { get; set; }
        public int RequestType { get; set; }
        public string RoutChart { get; set; }
        public string EntryFile { get; set; }
        public string KVSSName { get; set; }
        public string FertilizerName { get; set; }
        public string ROName { get; set; }
    }


    public class TransportMasterModel
    {
        public string Transportid { get; set; }
        public string TransportNameEn { get; set; }
        public string TransportNameHi { get; set; }
        public string AddressEn { get; set; }
        public string AddressHi { get; set; }
        public string MiningId { get; set; }
        public string FinancialYear { get; set; }
        public string Email { get; set; }
    }

    public class WarehouseRSWCModel
    {
        public string Id { get; set; }
        public string Receipt_No { get; set; }
        public string OldReceipt_No { get; set; }
        public string Wharehouse_Id { get; set; }
        public string Lisence_No { get; set; }
        public string Received_From_District { get; set; }
        public string Received_From_Address { get; set; }
        public string Item_Kind { get; set; }
        public string Quality { get; set; }
        public string Packages_Lots { get; set; }
        public string Packages_Quantity { get; set; }
        public string Packages_Quantity_Unit { get; set; }
        public string Godown_Id { get; set; }
        public string Stack_No { get; set; }
        public string Valid_From_Date { get; set; }
        public string Valid_To_Date { get; set; }
        public string NetQuantity { get; set; }
        public string QuantityNote { get; set; }
        public string PurchaseCenterId { get; set; }
        public string ReceivedFromKVSS { get; set; }
        public string SamplerName { get; set; }
        public string ConditionOfGoods { get; set; }
        public string EntryFile { get; set; }

        public string PrivateMarkofDepositor { get; set; }
        public string StorageRateCharge { get; set; }
        public string IndemnifiedInsurance { get; set; }
        public string InsuredName { get; set; }

        public string ReceiverAadharNo { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverFName { get; set; }
        public string ReleaseQuantity { get; set; }
        public string ReleasedQuantity { get; set; }
        public string Order_Id { get; set; }
        public bool IsSubmit { get; set; }
        public string WarehousName { get; set; }
        public string DISTRICTNAME { get; set; }
        public string KvssName { get; set; }
        public string PurchaseCenter { get; set; }
        public string CommodityName { get; set; }
        public string Price { get; set; }
        public string GoDownName { get; set; }


        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string AutoCalculateQuantity { get; set; }
        public string Moisture { get; set; }
        public string DispatchNo { get; set; }

        public string Kharif { get; set; }
        public string LicenceValidUpTo { get; set; }
        public string FY_Id { get; set; }
        public string FY { get; set; }

        public string ReleasedPackages_Lots { get; set; }
        public string QuantityLossGain { get; set; }      
        public string Quantity { get; set; }
        public string ReleaseOrderId { get; set; }
        public bool IsCWR { get; set; }
        public string DateOfIssue { get; set; }
        public int Crop_year { get; set; }
        public string AgencyName { get; set; }



    }

    public class DeliveryOrderFilterModel
    {
        public int WarehousId { get; set; }
        public int CommodityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
    public class PurchaseCenterModel
    {
        public string DistrictCode { get; set; }
        public string PurchaseCenterName_En { get; set; }
        public string PurchaseCenterName_Hi { get; set; }
        public int CommodityId { get; set; }
        public string KVSS_Hi { get; set; }
        public string KVSS_En { get; set; }
        public int Rajfed_PCID { get; set; }
        public int Rajfed_KVSS_Id { get; set; }
        public int PurchaseCenterId { get; set; }
    }
    public class FertilizerSupplyFectory
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;


        public FertilizerSupplyFectory(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
        }
        public int InsertGSSDemand(GSSDemandModel _Model)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertGSSDemad", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _Model.SSOId);
                objDBAccess.AddParameterToCommand("@Society_id", DBFactoryParameterType.Int, _Model.SocietyId);
                objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Float, _Model.Quantity);
                objDBAccess.AddParameterToCommand("@Created_by", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@FertilizerId", DBFactoryParameterType.Int, _Model.FertilizerId);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.Int, _Model.RequestId);
                objDBAccess.AddParameterToCommand("@Kvss_Id", DBFactoryParameterType.Int, _Model.KVSSId);
                objDBAccess.AddParameterToCommand("@RoutChart", DBFactoryParameterType.NVarchar, _Model.RoutChart);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "InsertGSSDemand(GSSDemandModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GSSDemandList(ref DataPagingModel Details, string SSOID)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetGSSDemad", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetGSSDemad", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetGSSDemad", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GSSDEmandList(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetKVSSData(ref DataPagingModel Details, string ROId, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetRORequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RajfedROId", DBFactoryParameterType.NVarchar, ROId);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetRORequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RajfedROId", DBFactoryParameterType.NVarchar, ROId);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();


                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable RajfedGetKVSSRequestList(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable KVSSRequestList(ref DataPagingModel Details, string SSOID, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();


                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable RajfedGetKVSSRequestList(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable KVSSRequestDetails(ref DataPagingModel Details, string Request_No)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetKVSSRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Request_No", DBFactoryParameterType.Int, Request_No);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetKVSSRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Request_No", DBFactoryParameterType.Int, Request_No);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable KVSSRequestDetails(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable OrderDetails(ref DataPagingModel Details, string OrderNo)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetOrderDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetOrderDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.Int, OrderNo);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable KVSSRequestDetails(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetFertilizerList()
        {
            DataTable resultDt = new DataTable();
            try
            {
                string WhereCase = "";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetFertilizerList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GetFertilizerList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public string InsertKVSSRequest(List<GSSDemandModel> _Model)
        {
            string request_No = "";
            try
            {
                //string request_No = "";

                _Model.ForEach(x =>
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateKVSSRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Request_from", DBFactoryParameterType.Int, x.RequestId);
                    objDBAccess.AddParameterToCommand("@GSS_DemandId", DBFactoryParameterType.Int, x.DemandId);
                    objDBAccess.AddParameterToCommand("@Request_By", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@Request_No", DBFactoryParameterType.NVarchar, request_No);
                    // if (request_No == "")
                    request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    // else
                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                });


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "InsertKVSSRequest(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }

        public string SendRORequest(List<GSSDemandModel> _Model)
        {
            string request_No = "";
            try
            {
                //string request_No = "";

                _Model.ForEach(x =>
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedSendRORequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Request_from", DBFactoryParameterType.Int, x.RequestId);
                    objDBAccess.AddParameterToCommand("@GSS_DemandId", DBFactoryParameterType.Int, x.DemandId);
                    objDBAccess.AddParameterToCommand("@Request_By", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@Request_No", DBFactoryParameterType.NVarchar, x.RequestId);
                    objDBAccess.AddParameterToCommand("@RO_Request_no", DBFactoryParameterType.NVarchar,string.IsNullOrWhiteSpace(x.Request_No)? request_No:x.Request_No);
                    objDBAccess.AddParameterToCommand("@ROFile", DBFactoryParameterType.NVarchar, x.EntryFile);
                    // if (request_No == "")
                    request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    // else
                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                });


            }
            catch (Exception ex)
            {
                request_No = "0";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "InsertKVSSRequest(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }



        public string UpdateKVSSRequest(string RequestNo, string RequestFile, int Type, int TransportId)
        {
            string request_No = "";
            try
            {
                //string request_No = "";

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateKVSSRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, RequestNo);
                objDBAccess.AddParameterToCommand("@RequestFile", DBFactoryParameterType.NVarchar, RequestFile);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                objDBAccess.AddParameterToCommand("@TransportId", DBFactoryParameterType.Int, Type);
                // if (request_No == "")
                request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                // else
                //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "UpdateKVSSRequest(string RequestNo,string RequestFile)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }

        public DataTable GetDemandSupplyList(ref DataPagingModel Details, string RequestType, int ListType)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetDemandSupply", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.Int, ListType);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetDemandSupply", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.Int, ListType);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "GetDemandSupplyList(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetTransportList(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {




                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                else
                {
                    whereCase = " and financial_year=(select FIN_YEAR_MASTR_ID from FinancialYear where FIN_YEAR_START = Year(GETDATE()) and FIN_YEAR_END = Year(GETDATE()) + 1)";
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetTransportList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetTransportList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();



                //string whereCase = " ";                
                //SqlCommand cmd = new SqlCommand();
                //objDBAccess.OpenConnection();               
                //cmd = objDBAccess.CreateCommand("RajfedGetTransportList", DBFactoryCommandType.PROCEDURE_COMMAND);                
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GetTransportList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetKVSSByDistrictId(int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string WhereCase = "";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetKVSSByDistrictId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GetFertilizerList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetKVSSList()
        {
            DataTable resultDt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetKVSSList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetConstituencyListByDistrictId(int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetConstituency", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetConstituency(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetGSSByBlockId(int BlockId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGSSByBlockId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GetGSSByBlockId(int BlockId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetGSSBySSOId(string SSOId)
        {
            DataTable resultDt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGSSBySSOId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetGSSBySSOId(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
       // New Add 01-03-2022 
        public DataTable GetGSSKVSSBySSOId(string SSOId,string TypeName)
        {
            DataTable resultDt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGSSORKVSSBySSOId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@TypeName", DBFactoryParameterType.NVarchar, TypeName);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetGSSKVSSBySSOId(string SSOId,string TypeName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetDirectorDemandDetails(ref DataPagingModel Details, string RequestNo, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetDADemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, RequestNo);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetDADemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, RequestNo);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();


                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "GetDirectorDemandDetails(ref DataPagingModel Details, string RequestNo, string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetDirectorDemandList(ref DataPagingModel Details, int DistrictId, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetDADemandList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetDADemandList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();


                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "GetDirectorDemandList(ref DataPagingModel Details, string RequestNo, string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public int InsertTransport(TransportMasterModel _Model)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertTransport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Transport_id", DBFactoryParameterType.NVarchar, _Model.Transportid);
                objDBAccess.AddParameterToCommand("@Transport_name_en", DBFactoryParameterType.NVarchar, _Model.TransportNameEn);
                objDBAccess.AddParameterToCommand("@Transport_name_hi", DBFactoryParameterType.NVarchar, _Model.TransportNameHi);
                objDBAccess.AddParameterToCommand("@Address_en", DBFactoryParameterType.NVarchar, _LoggedInUser.AddressEn);
                objDBAccess.AddParameterToCommand("@Address_hi", DBFactoryParameterType.NVarchar, _Model.AddressHi);
                objDBAccess.AddParameterToCommand("@Mining_id", DBFactoryParameterType.NVarchar, _Model.MiningId);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, _Model.FinancialYear.Split('-')[0]);
                objDBAccess.AddParameterToCommand("@CraetedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Email", DBFactoryParameterType.NVarchar, _Model.Email);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "int InsertTransport(TransportMasterModel _Model)" }, ex);
                Result = 3;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetGSSDemandReport(ref DataPagingModel Details, string SSOId, string Type)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_dated,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var y in Details.SearchParameter)
                    {
                        if (y.Key != "searchText" && y.Key != "FromDate" && y.Key != "ToDate")
                        {
                            whereCase += " and " + y.Key + "='" + y.Value + "'";
                        }

                    }





                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetGSSDemandReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetGSSDemandReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetGSSDemandReport(ref DataPagingModel Details, string SSOId,string Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetKVSSReport(ref DataPagingModel Details, string SSOId, string Type, string ReportType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_dated,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var y in Details.SearchParameter)
                    {
                        if (y.Key != "searchText" && y.Key != "FromDate" && y.Key != "ToDate")
                        {
                            whereCase += " and " + y.Key + "='" + y.Value + "'";
                        }

                    }





                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetKVSSReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetKVSSReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetGSSDemandReport(ref DataPagingModel Details, string SSOId,string Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetSupplyReport(ref DataPagingModel Details, string Type, string ReportType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_dated,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var y in Details.SearchParameter)
                    {
                        if (y.Key != "searchText" && y.Key != "FromDate" && y.Key != "ToDate")
                        {
                            whereCase += " and " + y.Key + "='" + y.Value + "'";
                        }

                    }





                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetSupllyReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetSupllyReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetSupplyReport(ref DataPagingModel Details, string SSOId,string Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetADReport(ref DataPagingModel Details, string Type, string ReportType, int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_dated,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var y in Details.SearchParameter)
                    {
                        if (y.Key != "searchText" && y.Key != "FromDate" && y.Key != "ToDate")
                        {
                            whereCase += " and " + y.Key + "='" + y.Value + "'";
                        }

                    }





                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetADReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetADReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

                //objDBAccess.OpenConnection();
                //SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetKVSSDemand", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetADReport(ref DataPagingModel Details, string SSOId,string Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public string InsertUpdateRSWC(List<WarehouseRSWCModel> Details)
        {
            string Receipt_No = "";
            string qry = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
               
                foreach (WarehouseRSWCModel _Model in Details)
                {
                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.Valid_From_Date))
                    {
                        fromDate = _Model.Valid_From_Date.Split('/')[2] + "/" + _Model.Valid_From_Date.Split('/')[1] + "/" + _Model.Valid_From_Date.Split('/')[0];
                    }
                    string todateDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.Valid_To_Date))
                    {
                        todateDate = _Model.Valid_To_Date.Split('/')[2] + "/" + _Model.Valid_To_Date.Split('/')[1] + "/" + _Model.Valid_To_Date.Split('/')[0];
                    }
                    string DateOfIssue = "";
                    if (!string.IsNullOrWhiteSpace(_Model.DateOfIssue))
                    {
                        DateOfIssue = _Model.DateOfIssue.Split('/')[2] + "/" + _Model.DateOfIssue.Split('/')[1] + "/" + _Model.DateOfIssue.Split('/')[0];
                    }
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertUpdateRSWC", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, _Model.Id);
                    objDBAccess.AddParameterToCommand("@Receipt_No", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(_Model.Receipt_No) ? Receipt_No : _Model.Receipt_No);
                    objDBAccess.AddParameterToCommand("@OldReceipt_No", DBFactoryParameterType.NVarchar, _Model.OldReceipt_No);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, _Model.Wharehouse_Id);
                    objDBAccess.AddParameterToCommand("@Lisence_No", DBFactoryParameterType.NVarchar, _Model.Lisence_No);
                    objDBAccess.AddParameterToCommand("@Received_From_District", DBFactoryParameterType.NVarchar, _Model.Received_From_District);
                    objDBAccess.AddParameterToCommand("@Received_From_Address", DBFactoryParameterType.NVarchar, _Model.Received_From_Address);
                    objDBAccess.AddParameterToCommand("@Item_Kind", DBFactoryParameterType.NVarchar, _Model.Item_Kind);
                    objDBAccess.AddParameterToCommand("@Quality", DBFactoryParameterType.NVarchar, _Model.Quality);
                    objDBAccess.AddParameterToCommand("@Packages_Lots", DBFactoryParameterType.NVarchar, _Model.Packages_Lots);
                    objDBAccess.AddParameterToCommand("@Packages_Quantity", DBFactoryParameterType.NVarchar, _Model.Packages_Quantity);
                    objDBAccess.AddParameterToCommand("@Packages_Quantity_Unit", DBFactoryParameterType.NVarchar, _Model.Packages_Quantity_Unit);
                    objDBAccess.AddParameterToCommand("@Godown_Id", DBFactoryParameterType.Int, _Model.Godown_Id);
                    objDBAccess.AddParameterToCommand("@Stack_No", DBFactoryParameterType.NVarchar, _Model.Stack_No);
                    objDBAccess.AddParameterToCommand("@Valid_From_Date", DBFactoryParameterType.NVarchar, fromDate);
                    objDBAccess.AddParameterToCommand("@Valid_To_Date", DBFactoryParameterType.NVarchar, todateDate);
                    objDBAccess.AddParameterToCommand("@Net_Quantity", DBFactoryParameterType.NVarchar, _Model.NetQuantity);
                    objDBAccess.AddParameterToCommand("@QuantityNote", DBFactoryParameterType.NVarchar, _Model.QuantityNote);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.NVarchar, _Model.PurchaseCenterId);
                    objDBAccess.AddParameterToCommand("@ReceivedFromKVSS", DBFactoryParameterType.NVarchar, _Model.ReceivedFromKVSS);
                    objDBAccess.AddParameterToCommand("@SamplerName", DBFactoryParameterType.NVarchar, _Model.SamplerName);
                    objDBAccess.AddParameterToCommand("@GoodsCondition", DBFactoryParameterType.NVarchar, _Model.ConditionOfGoods);
                    objDBAccess.AddParameterToCommand("@EntryFile", DBFactoryParameterType.NVarchar, _Model.EntryFile);
                    objDBAccess.AddParameterToCommand("@PrivateMarkofDepositor", DBFactoryParameterType.NVarchar, _Model.PrivateMarkofDepositor);
                    objDBAccess.AddParameterToCommand("@StorageRateCharge", DBFactoryParameterType.NVarchar, _Model.StorageRateCharge);
                    objDBAccess.AddParameterToCommand("@IndemnifiedInsurance", DBFactoryParameterType.NVarchar, _Model.IndemnifiedInsurance);
                    objDBAccess.AddParameterToCommand("@InsuredName", DBFactoryParameterType.NVarchar, _Model.InsuredName);
                    objDBAccess.AddParameterToCommand("@IsSubmit", DBFactoryParameterType.Bit, _Model.IsSubmit);
                    objDBAccess.AddParameterToCommand("@Auto_calculate_quantity", DBFactoryParameterType.VARCHAR, _Model.AutoCalculateQuantity);
                    objDBAccess.AddParameterToCommand("@Moisture", DBFactoryParameterType.VARCHAR, _Model.Moisture);
                    objDBAccess.AddParameterToCommand("@DispatchNo", DBFactoryParameterType.NVarchar, _Model.DispatchNo);
                    objDBAccess.AddParameterToCommand("@IsCWR", DBFactoryParameterType.Bit, _Model.IsCWR);
                    objDBAccess.AddParameterToCommand("@DateOfIssue", DBFactoryParameterType.VARCHAR, DateOfIssue);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.VARCHAR, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@CreatedBySSO", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                    objDBAccess.AddParameterToCommand("@Crop_year", DBFactoryParameterType.VARCHAR, _Model.Crop_year);
                    qry = objDBAccess.getSqlCommandText();
                    Receipt_No = objDBAccess.ExecuteScalarCommand().ToString();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public int InsertUpdateRSWC("+Newtonsoft.Json.JsonConvert.SerializeObject(Details)+")" + qry  }, ex);
                Receipt_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Receipt_No;
        }
        public string UpdateRSWC(List<WarehouseRSWCModel> Details)
        {
            string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                string allIds = string.Join(",", Details.Select(x => x.Id));
                foreach (WarehouseRSWCModel _Model in Details)
                {


                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.Valid_From_Date))
                    {
                        fromDate = _Model.Valid_From_Date.Split('/')[2] + "/" + _Model.Valid_From_Date.Split('/')[1] + "/" + _Model.Valid_From_Date.Split('/')[0];
                    }
                    string todateDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.Valid_To_Date))
                    {
                        todateDate = _Model.Valid_To_Date.Split('/')[2] + "/" + _Model.Valid_To_Date.Split('/')[1] + "/" + _Model.Valid_To_Date.Split('/')[0];
                    }


                    string DateOfIssue = "";
                    if (!string.IsNullOrWhiteSpace(_Model.DateOfIssue))
                    {
                        DateOfIssue = _Model.DateOfIssue.Split('/')[2] + "/" + _Model.DateOfIssue.Split('/')[1] + "/" + _Model.DateOfIssue.Split('/')[0];
                    }

                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedUpdateWR", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, _Model.Id);
                    objDBAccess.AddParameterToCommand("@Receipt_No", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(_Model.Receipt_No) ? Receipt_No : _Model.Receipt_No);
                    objDBAccess.AddParameterToCommand("@OldReceipt_No", DBFactoryParameterType.NVarchar, _Model.OldReceipt_No);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, _Model.Wharehouse_Id);
                    objDBAccess.AddParameterToCommand("@Lisence_No", DBFactoryParameterType.NVarchar, _Model.Lisence_No);
                    objDBAccess.AddParameterToCommand("@Received_From_District", DBFactoryParameterType.NVarchar, _Model.Received_From_District);
                    objDBAccess.AddParameterToCommand("@Received_From_Address", DBFactoryParameterType.NVarchar, _Model.Received_From_Address);
                    objDBAccess.AddParameterToCommand("@Item_Kind", DBFactoryParameterType.NVarchar, _Model.Item_Kind);
                    objDBAccess.AddParameterToCommand("@Quality", DBFactoryParameterType.NVarchar, _Model.Quality);
                    objDBAccess.AddParameterToCommand("@Packages_Lots", DBFactoryParameterType.NVarchar, _Model.Packages_Lots);
                    objDBAccess.AddParameterToCommand("@Packages_Quantity", DBFactoryParameterType.NVarchar, _Model.Packages_Quantity);
                    objDBAccess.AddParameterToCommand("@Packages_Quantity_Unit", DBFactoryParameterType.NVarchar, _Model.Packages_Quantity_Unit);
                    objDBAccess.AddParameterToCommand("@Godown_Id", DBFactoryParameterType.Int, _Model.Godown_Id);
                    objDBAccess.AddParameterToCommand("@Stack_No", DBFactoryParameterType.NVarchar, _Model.Stack_No);
                    objDBAccess.AddParameterToCommand("@Valid_From_Date", DBFactoryParameterType.NVarchar, fromDate);
                    objDBAccess.AddParameterToCommand("@Valid_To_Date", DBFactoryParameterType.NVarchar, todateDate);
                    objDBAccess.AddParameterToCommand("@Net_Quantity", DBFactoryParameterType.NVarchar, _Model.NetQuantity);
                    objDBAccess.AddParameterToCommand("@QuantityNote", DBFactoryParameterType.NVarchar, _Model.QuantityNote);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.NVarchar, _Model.PurchaseCenterId);
                    objDBAccess.AddParameterToCommand("@ReceivedFromKVSS", DBFactoryParameterType.NVarchar, _Model.ReceivedFromKVSS);
                    objDBAccess.AddParameterToCommand("@SamplerName", DBFactoryParameterType.NVarchar, _Model.SamplerName);
                    objDBAccess.AddParameterToCommand("@GoodsCondition", DBFactoryParameterType.NVarchar, _Model.ConditionOfGoods);
                    objDBAccess.AddParameterToCommand("@EntryFile", DBFactoryParameterType.NVarchar, _Model.EntryFile);
                    objDBAccess.AddParameterToCommand("@PrivateMarkofDepositor", DBFactoryParameterType.NVarchar, _Model.PrivateMarkofDepositor);
                    objDBAccess.AddParameterToCommand("@StorageRateCharge", DBFactoryParameterType.NVarchar, _Model.StorageRateCharge);
                    objDBAccess.AddParameterToCommand("@IndemnifiedInsurance", DBFactoryParameterType.NVarchar, _Model.IndemnifiedInsurance);
                    objDBAccess.AddParameterToCommand("@InsuredName", DBFactoryParameterType.NVarchar, _Model.InsuredName);
                    objDBAccess.AddParameterToCommand("@IsSubmit", DBFactoryParameterType.Bit, _Model.IsSubmit);
                    objDBAccess.AddParameterToCommand("@AllId", DBFactoryParameterType.VARCHAR, allIds);
                    objDBAccess.AddParameterToCommand("@Auto_calculate_quantity", DBFactoryParameterType.VARCHAR, _Model.AutoCalculateQuantity);
                    objDBAccess.AddParameterToCommand("@Moisture", DBFactoryParameterType.VARCHAR, _Model.Moisture);
                    objDBAccess.AddParameterToCommand("@DispatchNo", DBFactoryParameterType.NVarchar, _Model.DispatchNo);
                    objDBAccess.AddParameterToCommand("@DateOfIssue", DBFactoryParameterType.VARCHAR, DateOfIssue);                    
                    Receipt_No = objDBAccess.ExecuteScalarCommand().ToString();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string UpdateRSWC(List<WarehouseRSWCModel> Details)" }, ex);
                Receipt_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Receipt_No;
        }
        public DataTable GetPurchaseCenterList(int DistrictId, int CommodityId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string WhereCase = "";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetPurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetPurchaseCenterList(int KVSSId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWarehouseRSWCDetails(string ReceiptNo)
        {
            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, ReceiptNo);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetDirectorDemandDetails(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetReleasedCommodity(string ReceiptNo)
        {
            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RajfedGetReleasedCommodity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, ReceiptNo);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetDirectorDemandDetails(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWarehouseRSWCList(ref DataPagingModel Details, int WharehouseId,string RequestType)
        {
            DataTable resultDt = new DataTable();
            string qry = "";
            try
            {


                string whereCase = " and Wharehouse_Id="+ WharehouseId;

                if (RequestType == "1")
                {
                    whereCase += "  and EntryFile!=''";
                }
                else if (RequestType == "2")
                {
                    whereCase += "  and EntryFile=''";
                }

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseRSWCList(ref DataPagingModel Details, int WharehouseId)"+ qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetCommodityReleaseOrderList(ref DataPagingModel Details,string RequestType,int WharehouseId=0)
        {            
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " and EntryFile!='' and isnull(isGenerateWRSummery,0)=1";
                if (RequestType == "1")
                {
                    whereCase += " and TotalPackages_Lots>ReleasedPackagesLots ";
                }
                else if (RequestType == "3")
                {
                    whereCase = " and EntryFile!='' and TotalPackages_Lots=ReleasedPackagesLots  and isnull(isGenerateWRSummery,0)=1";
                }
                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,(created_date),111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }                   

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                    
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable  GetCommodityReleaseOrderList(ref DataPagingModel Details,string RequestType,int WharehouseId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetPurchaseCenterKVSS(int DistrictId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetPurchaseCenterKVSS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetPurchaseCenterKVSS(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataSet GetFertilizerSupplierDashboardData(int LoggedInUserMappingId)
        {
            DataSet resultDt = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("FertilizerSupplierDashboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LoggedInUserMappingId", DBFactoryParameterType.Int, LoggedInUserMappingId);
                resultDt = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataSet GetFertilizerSupplierDashboardData(int LoggedInUserMappingId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public string UpdateRSWC(string ReceiptNo, string Type, string Value)
        {
            string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();


                SqlCommand cmd = objDBAccess.CreateCommand("RajfedUpdateRSWC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, ReceiptNo);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Value", DBFactoryParameterType.NVarchar, Value);
                Receipt_No = objDBAccess.ExecuteScalarCommand().ToString();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public string UpdateRSWC(string ReceiptNo,string Type,string Value)" }, ex);
                Receipt_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Receipt_No;
        }

        public string InsertCommodityReleased(List<WarehouseRSWCModel> Details, string Receipt_No = "")
        {
            //string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                foreach (WarehouseRSWCModel _Model in Details)
                {


                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.Valid_From_Date))
                    {
                        fromDate = _Model.Valid_From_Date.Split('/')[2] + "/" + _Model.Valid_From_Date.Split('/')[1] + "/" + _Model.Valid_From_Date.Split('/')[0];
                    }

                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertCommodityReleased", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Rswc_id", DBFactoryParameterType.NVarchar, _Model.Id);
                    objDBAccess.AddParameterToCommand("@Released_quantity", DBFactoryParameterType.NVarchar, _Model.ReleaseQuantity);
                    objDBAccess.AddParameterToCommand("@Realease_dated", DBFactoryParameterType.NVarchar, fromDate);
                    objDBAccess.AddParameterToCommand("@Realease_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, Receipt_No);
                    objDBAccess.AddParameterToCommand("@Released_file", DBFactoryParameterType.NVarchar, _Model.EntryFile);
                    objDBAccess.AddParameterToCommand("@Released_order_id", DBFactoryParameterType.NVarchar, _Model.Order_Id);
                    objDBAccess.AddParameterToCommand("@Released_packages_lots", DBFactoryParameterType.NVarchar, _Model.ReleasedPackages_Lots);
                    objDBAccess.AddParameterToCommand("@Loss_Gain", DBFactoryParameterType.NVarchar, _Model.QuantityLossGain);
                    objDBAccess.AddParameterToCommand("@ReleaseOrderId", DBFactoryParameterType.NVarchar, _Model.ReleaseOrderId);
                    

                    Receipt_No = objDBAccess.ExecuteScalarCommand().ToString();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string InsertCommodityReleased(List<WarehouseRSWCModel> Details, string Receipt_No = )" }, ex);
                Receipt_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Receipt_No;
        }



        public string InsertReleasedCommodity(List<WarehouseRSWCModel> Details ,string[] Files)
        {
            string result = "";

            var reciept = Details.Select(x => x.Receipt_No).Distinct().ToList();


            DBFactoryMSSQL conn = new DBFactoryMSSQL();


            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;

            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();




            try
            {

                string Receipt_No = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int index = 0;
                foreach (var recpt in reciept)
                {
                    List<WarehouseRSWCModel> DetailsList = Details.Where(a => a.Receipt_No == recpt).ToList();
                    string ReceiptNo = DetailsList[0].Receipt_No;
                    //DataTable ReleasedCommodity = GetReleasedCommodity(ReceiptNo);

                    Receipt_No = "";

                    foreach (WarehouseRSWCModel _Model in DetailsList)
                    {


                        string fromDate = "";
                        if (!string.IsNullOrWhiteSpace(_Model.Valid_From_Date))
                        {
                            fromDate = _Model.Valid_From_Date.Split('/')[2] + "/" + _Model.Valid_From_Date.Split('/')[1] + "/" + _Model.Valid_From_Date.Split('/')[0];
                        }

                        //SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertCommodityReleased", DBFactoryCommandType.PROCEDURE_COMMAND);
                        //objDBAccess.OpenConnection();
                        //cmd.Transaction = transaction;
                        //cmd.Connection = objDBAccess.GetConnection;
                        //objDBAccess.AddParameterToCommand("@Rswc_id", DBFactoryParameterType.NVarchar, _Model.Id);
                        //objDBAccess.AddParameterToCommand("@Released_quantity", DBFactoryParameterType.NVarchar, _Model.ReleaseQuantity);
                        //objDBAccess.AddParameterToCommand("@Realease_dated", DBFactoryParameterType.NVarchar, fromDate);
                        //objDBAccess.AddParameterToCommand("@Realease_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                        //objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, Receipt_No);
                        //objDBAccess.AddParameterToCommand("@Released_file", DBFactoryParameterType.NVarchar, "Relese_" + (ReleasedCommodity.Rows.Count + 1) + "_" + ReceiptNo.Replace("/", "_") + ".pdf");
                        //objDBAccess.AddParameterToCommand("@Released_order_id", DBFactoryParameterType.NVarchar, _Model.Order_Id);
                        //objDBAccess.AddParameterToCommand("@Released_packages_lots", DBFactoryParameterType.NVarchar, _Model.ReleasedPackages_Lots);
                        //objDBAccess.AddParameterToCommand("@Loss_Gain", DBFactoryParameterType.NVarchar, _Model.QuantityLossGain);
                        //objDBAccess.AddParameterToCommand("@ReleaseOrderId", DBFactoryParameterType.NVarchar, _Model.ReleaseOrderId);
                        //Receipt_No = objDBAccess.ExecuteScalarCommand().ToString();


                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "RajfedInsertCommodityReleased";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@Rswc_id", _Model.Id);
                        cmd.Parameters.AddWithValue("@Released_quantity", _Model.ReleaseQuantity);
                        cmd.Parameters.AddWithValue("@Realease_dated", fromDate);
                        cmd.Parameters.AddWithValue("@Realease_by", _LoggedInUser.UserId);
                        cmd.Parameters.AddWithValue("@RequestNo", Receipt_No);
                        cmd.Parameters.AddWithValue("@Released_file", Files[index].Split('/')[Files[index].Split('/').Length - 1]);// "Relese_" + (ReleasedCommodity.Rows.Count + 1) + "_" + ReceiptNo.Replace("/", "_") + ".pdf");
                        cmd.Parameters.AddWithValue("@Released_order_id", _Model.Order_Id);
                        cmd.Parameters.AddWithValue("@Released_packages_lots", _Model.ReleasedPackages_Lots);
                        cmd.Parameters.AddWithValue("@Loss_Gain", _Model.QuantityLossGain);
                        cmd.Parameters.AddWithValue("@ReleaseOrderId", _Model.ReleaseOrderId);
                        Receipt_No = cmd.ExecuteScalar().ToString(); ;


                    }

                    ++index;

                    //string requestNo1 = InsertCommodityReleased(DetailsList);
                    //if (result == "-1" && result == "0")
                    //{
                    //    result = "Error";
                    //    return result;
                    //}
                    //else
                    //{
                    //    result = "Success";
                    //}
                    //DetailsList.ForEach(x =>
                    //{
                    //    x.Id = "0";
                    //    x.EntryFile = "Relese_" + (ReleasedCommodity.Rows.Count + 1) + "_" + ReceiptNo.Replace("/", "_") + ".pdf";
                    //    x.Receipt_No = ReceiptNo;
                    //});
                    //InsertCommodityReleased(DetailsList, requestNo1).ToString();
                }

                //transaction.Rollback();
                transaction.Commit();
                result = "Success";
            }
            catch (Exception Ex)
            {
                result = "Error";
                transaction.Rollback();

            }
            finally {
                sqlConnection.Close();
            }

           

            return result;
        }





                public string InsertUpdateCommodityReleaseOrder(List<WarehouseRSWCModel> Details, string Request_No = "",string UniqeId = "")
        {
            //string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                foreach (WarehouseRSWCModel _Model in Details)
                {


                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.DateOfIssue))
                    {
                        fromDate = _Model.DateOfIssue.Split('/')[2] + "/" + _Model.DateOfIssue.Split('/')[1] + "/" + _Model.DateOfIssue.Split('/')[0];
                    }
                    else {
                        fromDate = DateTime.Now.ToString("yyyy/MM/dd");
                    }

                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertUpdateCommodityReleaseOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@release_order_id", DBFactoryParameterType.NVarchar, _Model.Id);
                    objDBAccess.AddParameterToCommand("@receipt_number", DBFactoryParameterType.NVarchar, _Model.Receipt_No);
                    objDBAccess.AddParameterToCommand("@request_no", DBFactoryParameterType.NVarchar, Request_No);
                    objDBAccess.AddParameterToCommand("@receiver_aadhar_no", DBFactoryParameterType.NVarchar, _Model.ReceiverAadharNo);
                    objDBAccess.AddParameterToCommand("@receiver_name", DBFactoryParameterType.NVarchar, _Model.ReceiverName);
                    objDBAccess.AddParameterToCommand("@receiver_fname", DBFactoryParameterType.NVarchar, _Model.ReceiverFName);
                    objDBAccess.AddParameterToCommand("@release_quantity", DBFactoryParameterType.NVarchar, _Model.ReleaseQuantity);
                    objDBAccess.AddParameterToCommand("@order_file", DBFactoryParameterType.NVarchar, _Model.EntryFile);
                    objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CompanyName", DBFactoryParameterType.NVarchar, _Model.CompanyName);
                    objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, _Model.Address);
                    objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, _Model.MobileNo);
                    objDBAccess.AddParameterToCommand("@Email", DBFactoryParameterType.NVarchar, _Model.Email);
                    objDBAccess.AddParameterToCommand("@GUID", DBFactoryParameterType.NVarchar, UniqeId);

                    objDBAccess.AddParameterToCommand("@Warehouse_Id", DBFactoryParameterType.NVarchar, _Model.Wharehouse_Id);
                    objDBAccess.AddParameterToCommand("@Commodity_Id", DBFactoryParameterType.NVarchar, _Model.Item_Kind);
                    objDBAccess.AddParameterToCommand("@FY_Id", DBFactoryParameterType.NVarchar, _Model.FY_Id);
                    objDBAccess.AddParameterToCommand("@OldReceipt_No", DBFactoryParameterType.NVarchar, _Model.OldReceipt_No);
                    objDBAccess.AddParameterToCommand("@DateOfIssue", DBFactoryParameterType.NVarchar, fromDate);

                    Request_No = objDBAccess.ExecuteScalarCommand().ToString();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string InsertUpdateCommodityReleaseOrder(List<WarehouseRSWCModel> Details, string Request_No = ,string UniqeId =)" }, ex);
                Request_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Request_No;
        }

        public string DeleteReleaseOrder(string Request_No = "")
        {
            //string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
               

                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedDeleteCommodityReleaseOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, Request_No);                   
                    Request_No = objDBAccess.ExecuteScalarCommand().ToString();
           
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string DeleteReleaseOrder(string Request_No = "+ Request_No + ")" }, ex);
                Request_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Request_No;
        }

        public string DeleteWRSummary(string Request_No = "")
        {
            //string Receipt_No = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();


                SqlCommand cmd = objDBAccess.CreateCommand("RajfedDeleteWRSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, Request_No);
                Request_No = objDBAccess.ExecuteScalarCommand().ToString();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string DeleteWRSummary(string Request_No = "+ Request_No + ")" }, ex);
                Request_No = "0";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Request_No;
        }



        public string InsertUpdateWRSummary(List<WarehouseRSWCModel> _Model, string request_No = "",string UniqeId="")
        {
            //string request_No = "";
            try
            {
                //string request_No = "";

                _Model.ForEach(x =>
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertUpdateWRSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@summary_id", DBFactoryParameterType.NVarchar, x.Id);
                    objDBAccess.AddParameterToCommand("@request_no", DBFactoryParameterType.NVarchar, request_No);
                    objDBAccess.AddParameterToCommand("@receipt_no", DBFactoryParameterType.NVarchar, x.Receipt_No);
                    objDBAccess.AddParameterToCommand("@summary_file", DBFactoryParameterType.NVarchar, x.EntryFile);
                    objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@GUID", DBFactoryParameterType.NVarchar, UniqeId);
                    // if (request_No == "")
                    request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    // else
                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                });


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string InsertUpdateWRSummary(List<WarehouseRSWCModel> _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }

        public DataTable GetWRSummaryDetailsList(string RequestNo)
        {
            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RajfedGetWRSummaryDetailsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, RequestNo);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetDirectorDemandDetails(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWRSummaryList(ref DataPagingModel Details, string RequestType, int WharehouseId = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " and EntryFile!=''";
                if (RequestType == "1")
                {
                    whereCase += " and isnull(isGenerateWRSummery,0) =0";
                }
                else if (RequestType == "2")
                {
                    return GetRajfedWRSummary(ref Details, RequestType, WharehouseId);
                }
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    //else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    //{

                    //    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    //    whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    //}

                    foreach (var x in Details.SearchParameter.Where(x=>x.Key!= "FromDate" && x.Key!= "ToDate"))
                    {
                        if (x.Key == "Agency")
                        {
                            if (x.Value == "NAFED")
                                whereCase += " and IsNull(d.NccfId,0)=0";
                            else
                                whereCase += " and IsNull(d.NccfId,0)>0";
                        }
                        else { whereCase += " and " + x.Key + "='" + x.Value + "'"; }
                        
                    }

                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWRSummaryList(ref DataPagingModel Details, string RequestType, int WharehouseId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetRajfedWRSummary(ref DataPagingModel Details, string RequestType, int WharehouseId = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {

                //var a = COMMON_FUNCTION.Encrypt("407667304550");

                string whereCase = " and EntryFile!=''";
                whereCase = "";
                if (RequestType == "3")
                {
                    whereCase = " and isnull(IsPaymentReleased,0)=0";
                }
                else if (RequestType == "4")
                {
                    whereCase = " and isnull(IsPaymentReleased,0)=1";
                }

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,(createddate),111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    //else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    //{

                    //    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    //    whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    //}

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {

                        if (x.Key == "Agency")
                        {
                            if (x.Value == "NAFED")
                                whereCase += " and IsNull(IsNccf,0)=0";
                            else
                                whereCase += " and IsNull(IsNccf,0)=1";
                        }
                        else { whereCase += " and " + x.Key + "='" + x.Value + "'"; }

                        //whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }

                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetRajfedWRSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetRajfedWRSummary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetRajfedWRSummary(ref DataPagingModel Details, string RequestType, int WharehouseId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable GetCommodityReleaseOrderDetails(string RequestNo)
        {
            DataTable resultDt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RajfedGetCommodityReleaseOrderDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestNo", DBFactoryParameterType.NVarchar, RequestNo);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable GetDirectorDemandDetails(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }



        public DataTable GetWarehouseReleaseOrder(ref DataPagingModel Details, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " ";
                if (RequestType == "1")
                {
                    whereCase = " and isnull(order_file,'')!='' and isnull(release_file,'')=''";//
                }
                else if (RequestType == "2")
                {
                    //return GetWROrder(ref Details, RequestType);
                    //whereCase = " and isnull(release_file,'')!='' ";//and isnull(release_file,'')=''
                }
                else if (RequestType == "3")
                {
                  
                    whereCase = " and isnull(release_file,'')!='' ";//and isnull(release_file,'')=''
                }
                else
                {
                    whereCase = " and isnull(order_file,'')!='' ";//and isnull(release_file,'')=''
                }


                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "wrs.created_date" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseReleaseOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseReleaseOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseReleaseOrder(ref DataPagingModel Details, string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetDeliveryOrder(ref DataPagingModel Details, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string HavingCase = " having 1=1 " ;

                string whereCase = " ";
                if (RequestType == "PandingDO")
                {
                    HavingCase += " and isnull(sum(cr.released_quantity) ,0)<sum(distinct ro.release_quantity)";//
                }
                else if (RequestType == "ReleasedDO")
                {
                    HavingCase += " and isnull(sum(cr.released_quantity) ,0)>=sum(distinct ro.release_quantity)";//and isnull(release_file,'')=''
                }
                //else
                //{
                //    whereCase = " and isnull(order_file,'')!='' ";//and isnull(release_file,'')=''
                //}


                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_dated,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var y in Details.SearchParameter)
                    {
                        if (y.Key != "searchText" && y.Key != "FromDate" && y.Key != "ToDate")
                        {
                            whereCase += " and " + y.Key + "='" + y.Value + "'";
                        }

                    }
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser.WarehouseId > 0)
                {
                    whereCase += " and warehouse_id=" + _LoggedInUser.WarehouseId;
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWROrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@Having", DBFactoryParameterType.NVarchar, HavingCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWROrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@Having", DBFactoryParameterType.NVarchar, HavingCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetDeliveryOrder(ref DataPagingModel Details, string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWarehouseDeliveryOrder(ref DataPagingModel Details, int WarehouseId,string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string HavingCase = " having  max(ro.warehouse_id)=" + WarehouseId;

                string whereCase = " " ;
                if (RequestType == "PandingDO")
                {
                    HavingCase += " and isnull(cast(sum(cr.released_quantity) as decimal(18,2)) ,0)<cast(sum(distinct ro.release_quantity) as decimal(18,2))";//
                }
                else if (RequestType == "ReleasedDO")
                {
                    HavingCase += " and isnull(cast(sum(cr.released_quantity) as decimal(18,2)) ,0)>=cast(sum(distinct ro.release_quantity) as decimal(18,2))";//and isnull(release_file,'')=''
                }
                //else
                //{
                //    whereCase = " and isnull(order_file,'')!='' ";//and isnull(release_file,'')=''
                //}


                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWROrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@Having", DBFactoryParameterType.NVarchar, HavingCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWROrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@Having", DBFactoryParameterType.NVarchar, HavingCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseDeliveryOrder(ref DataPagingModel Details, int WarehouseId,string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetDeliveryOrderWRList(ref DataPagingModel Details, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " and  EntryFile!='' and isnull(isGenerateWRSummery,0)=1";
                if (RequestType == "OpenWR")
                {
                    whereCase += " and TotalPackages_Lots>ReleasedPackagesLots ";
                }
                else if (RequestType == "CloseWR")
                {
                    whereCase += " and EntryFile!='' and TotalPackages_Lots=ReleasedPackagesLots  and isnull(isGenerateWRSummery,0)=1";
                }
                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,(created_date),111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }


                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser.WarehouseId > 0)
                {
                    whereCase += " and warehouse_id=" + _LoggedInUser.WarehouseId;

                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetDeliveryOrderWRList(ref DataPagingModel Details, string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWarehouseDeliveryOrderWRList(ref DataPagingModel Details,  int WharehouseId,string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = " and wharehouse_id=" + WharehouseId + " and EntryFile!='' and isnull(isGenerateWRSummery,0)=1";
                if (RequestType == "OpenWR")
                {
                    whereCase += " and TotalPackages_Lots>ReleasedPackagesLots ";
                }
                else if (RequestType == "CloseWR")
                {
                    whereCase += " and EntryFile!='' and TotalPackages_Lots=ReleasedPackagesLots  and isnull(isGenerateWRSummery,0)=1";
                }
                if (Details.SearchParameter.Count > 0)
                {

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate" || x.Key == "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,(created_date),111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }


                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseDeliveryOrderWRList(ref DataPagingModel Details,  int WharehouseId,string RequestType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }









        public DataTable GetRajfedWRSummaryPayment(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = "   and isnull(wrs.IsPaymentReleased,0)=0  ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "FromDate"||x.Key== "ToDate").Any())
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,created_date,111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        whereCase += " and " + x.Key + "='" + x.Value + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetRajfedWRSummaryPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetRajfedWRSummaryPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 0);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetRajfedWRSummaryPayment(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetAvailableCommodity(int Wharehouse_id,int CommodityId,int FinancialYear)
        {
            DataTable resultDt = new DataTable();
            try
            {                
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();             

                cmd = objDBAccess.CreateCommand("RajfedGetAvailableCommodity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_id", DBFactoryParameterType.Int, Wharehouse_id);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinancialYear);               
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable RajfedGetAvailableCommodity(ref DataPagingModel Details, int WharehouseId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }



        public DataTable GetDODetailsWR(int DONo)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RajfedGetDODetailsWR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DONo", DBFactoryParameterType.Int, DONo);          
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetDODetailsWR(int DONo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }



        public string InsertUpdateWRPayment(List<WarehouseRSWCModel> _Model, string request_No = "")
        {
            try
            {
                _Model.ForEach(x =>
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertUpdateWRPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@summary_id", DBFactoryParameterType.NVarchar, x.Receipt_No);
                    objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                });


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string InsertUpdateWRPayment(List<WarehouseRSWCModel> _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }

        public int CheckIsItHoliday(string Date)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CheckIsItHoliday", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.NVarchar, Date);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "CheckIsItHoliday(string Date)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public bool DeleteWR( string Receipt_No = "")
        {
            try
            {
                
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedDeleteWR", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Receipt_No", DBFactoryParameterType.NVarchar, Receipt_No);                    
                    objDBAccess.ExecuteScalarCommand();

                return true;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string DeleteWR(string Receipt_No)" }, ex);
                return false;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            
        }

        public string InsertFertilizerByExcel(SocietyRegistrationEntity obj)
        {
            string Result = "-1";
            string connString = "";
            try
            {
                objDBAccess.OpenConnection();                
                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyExcel/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [Sheet1$]";//[" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();

                        try
                        {

                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(" 'Sheet1' does not exists in upload Excel. OR Error-" + ex.Message.Replace("'", ""));
                        }

                        try
                        {
                            int i = 0;
                            String savedata = "";
                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;
                                int FertilizerId = Convert.ToInt32(Dr["FertilizerId"]);
                                string DistrictName = Dr["DistrictName"].ToString().Trim(); 
                                int MonthId = Convert.ToInt32(Dr["MonthId"]);  
                                int YearId = Convert.ToInt32(Dr["YearId"]);
                                Decimal Total = Convert.ToDecimal(Dr["Total"]);
                                Decimal Warehouse_Rakepoint = Convert.ToDecimal(Dr["Warehouse_Rakepoint"]);
                                Decimal Wholesaler = Convert.ToDecimal(Dr["Wholesaler"]);
                                Decimal Retailer = Convert.ToDecimal(Dr["Retailer"]);
                                Decimal Company_GIT = Convert.ToDecimal(Dr["Company_GIT"]);
                                Decimal Wholesaler_GIT = Convert.ToDecimal(Dr["Wholesaler_GIT"]);
                            
                                if (DistrictName != "")
                                {
                                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertFertilizerExcelDetailsByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                                    cmd1.Parameters.AddWithValue("@FertilizerId", FertilizerId);
                                    cmd1.Parameters.AddWithValue("@DistrictName", DistrictName.Trim());
                                    cmd1.Parameters.AddWithValue("@MonthId", MonthId);
                                    cmd1.Parameters.AddWithValue("@YearId", YearId);
                                    cmd1.Parameters.AddWithValue("@Total", Total);
                                    cmd1.Parameters.AddWithValue("@Warehouse_Rakepoint", Warehouse_Rakepoint);
                                    cmd1.Parameters.AddWithValue("@Wholesaler", Wholesaler);
                                    cmd1.Parameters.AddWithValue("@Retailer", Retailer);
                                    cmd1.Parameters.AddWithValue("@Company_GIT", Company_GIT);
                                    cmd1.Parameters.AddWithValue("@Wholesaler_GIT", Wholesaler_GIT);
                                    savedata = (objDBAccess.ExecuteScalarCommand().ToString());
                             
                                }

                            }
                            if (savedata.Length > 1)
                            {
                                Result = "Successfully Saved";
                            }
                            else {
                                Result = "Not Successfully Saved";

                            }
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetWarehouseCWCList(ref DataPagingModel Details, int WharehouseId, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = "";// " and Wharehouse_Id=" + WharehouseId;

                if (RequestType == "1")
                {
                    whereCase += "  and EntryFile!=''";
                }
                else if (RequestType == "2")
                {
                    whereCase += "  and EntryFile=''";
                }

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "created_dated" || type == "request_date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date," + type + ",111) ";
                        if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                        {
                            string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                            string tDate = fDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                        else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                        {
                            string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                            string fDate = tDate;
                            whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                        }
                    }
                    else if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWarehouseCWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseCWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, WharehouseId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseCWCList(ref DataPagingModel Details, int WharehouseId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable RajfedGetWRPhysicalEntry(ref DataPagingModel Details, int WharehouseId, string RequestType)
        {
            DataTable resultDt = new DataTable();
            try
            {


                string whereCase = "";// " and Wharehouse_Id=" + WharehouseId;

                
                string commodity_id = "";
                string croup_year = "";
                string entry_date = "";
                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();
                     commodity_id = Details.SearchParameter.Where(x => x.Key == "commodity_id").Select(x => x.Value).FirstOrDefault();
                     croup_year = Details.SearchParameter.Where(x => x.Key == "croup_year").Select(x => x.Value).FirstOrDefault();
                     entry_date = "";
                    if (Details.SearchParameter.Where(x => x.Key == "entry_date").Select(x => x.Value).Any())
                    {
                        var edate = Details.SearchParameter.Where(x => x.Key == "entry_date").Select(x => x.Value).FirstOrDefault();
                        entry_date = edate.Split('/')[2] + "-" + edate.Split('/')[1] + "-" + edate.Split('/')[0];
                    }

                   
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedGetWRPhysicalEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                    objDBAccess.AddParameterToCommand("@warehouse_id", DBFactoryParameterType.Int, WharehouseId);
                    objDBAccess.AddParameterToCommand("@commodity_id", DBFactoryParameterType.NVarchar, commodity_id);
                    objDBAccess.AddParameterToCommand("@croup_year", DBFactoryParameterType.NVarchar, croup_year);

                    objDBAccess.AddParameterToCommand("@EntryDate", DBFactoryParameterType.NVarchar, entry_date);


                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedGetWRPhysicalEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd = objDBAccess.CreateCommand("RajfedGetWRPhysicalEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

                objDBAccess.AddParameterToCommand("@warehouse_id", DBFactoryParameterType.Int, WharehouseId);
                objDBAccess.AddParameterToCommand("@commodity_id", DBFactoryParameterType.NVarchar, commodity_id);
                objDBAccess.AddParameterToCommand("@croup_year", DBFactoryParameterType.NVarchar, croup_year);

                objDBAccess.AddParameterToCommand("@EntryDate", DBFactoryParameterType.NVarchar, entry_date);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = " public DataTable GetWarehouseCWCList(ref DataPagingModel Details, int WharehouseId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public string RajfedInsertUpdatedWRPhysicalEntry(List<WarehouseRSWCModel> _Model, string request_No = "")
        {
            try
            {
                _Model.ForEach(x =>
                {



                    var edate = x.DateOfIssue;
                    string entry_date = edate.Split('/')[2] + "-" + edate.Split('/')[1] + "-" + edate.Split('/')[0];



                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("RajfedInsertUpdatedWRPhysicalEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@id", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(x.Id) ? "0" : x.Id);
                    objDBAccess.AddParameterToCommand("@warehouse_id", DBFactoryParameterType.Int, x.Wharehouse_Id);
                    objDBAccess.AddParameterToCommand("@commodity_id", DBFactoryParameterType.NVarchar, x.Item_Kind);
                    objDBAccess.AddParameterToCommand("@quntity", DBFactoryParameterType.NVarchar, x.Quantity);
                    objDBAccess.AddParameterToCommand("@croup_year", DBFactoryParameterType.NVarchar, x.Crop_year);
                    objDBAccess.AddParameterToCommand("@createdby", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@entry_date", DBFactoryParameterType.NVarchar, entry_date);
                    objDBAccess.AddParameterToCommand("@TotalWR", DBFactoryParameterType.NVarchar, x.Packages_Lots);



                    request_No = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                });




            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public string RajfedInsertUpdatedWRPhysicalEntry(" + Newtonsoft.Json.JsonConvert.SerializeObject(_Model) + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return request_No;
        }

        public string RSWC_Add_PurchaseCenter(string DistrictCode, string PurchaseCenterName_En, string PurchaseCenterName_Hi, int CommodityId, string KVSS_Hi, string KVSS_En, int Rajfed_PCID, int Rajfed_KVSS_Id,int PurchaseCenterId)
        {
            DataTable resultDt = new DataTable();
            try
            {



                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("RSWC_Add_PurchaseCenter", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.NVarchar, DistrictCode);
                objDBAccess.AddParameterToCommand("@PurchaseCenterName_En", DBFactoryParameterType.NVarchar, PurchaseCenterName_En);
                objDBAccess.AddParameterToCommand("@PurchaseCenterName_Hi", DBFactoryParameterType.NVarchar, PurchaseCenterName_Hi);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.NVarchar, CommodityId);
                objDBAccess.AddParameterToCommand("@KVSS_Hi", DBFactoryParameterType.NVarchar, KVSS_Hi);
                objDBAccess.AddParameterToCommand("@KVSS_En", DBFactoryParameterType.NVarchar, KVSS_En);
                objDBAccess.AddParameterToCommand("@Rajfed_PCID", DBFactoryParameterType.NVarchar, Rajfed_PCID);
                objDBAccess.AddParameterToCommand("@Rajfed_KVSS_Id", DBFactoryParameterType.NVarchar, Rajfed_KVSS_Id);
                objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.NVarchar, PurchaseCenterId);

                return Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "public DataTable RSWC_Add_PurchaseCenter(" + DistrictCode + "," + PurchaseCenterName_En + "," + PurchaseCenterName_Hi + "," + CommodityId + "," + KVSS_Hi + "," + KVSS_En + "," + Rajfed_PCID + "," + Rajfed_KVSS_Id + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "Error";



        }

    }
}
