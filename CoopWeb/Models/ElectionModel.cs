using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Linq;
namespace Cooperative.Models
{
    public class ElectionModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public ElectionModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public ElectionModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        #region Election

        public DataTable SocietyList(SocietyRegistrationEntity Obj)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                //string WhereClause = " and 1=1" + Obj.WhereClause;
                string WhereClause = " 1=1 " + Obj.WhereClause;
                if (Obj.UserType == "C" || Obj.UserType == "K")
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "' ") : "";
                }

                if (Obj.SocietyIDEnc != null)
                {
                    WhereClause += Obj.SocietyIDEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + Obj.SocietyIDEnc + "')as int) ") : "";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable SocietyList(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable SocietyListForElection(ref DataPagingModel Details, ElectionSocietyData Obj)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                if (Obj.TypeId == 9)
                {
                    Obj.ElectionForId = 0;
                }

                //string WhereClause = " and 1=1" + Obj.WhereClause;
                string WhereClause = " " + Obj.WhereClause;
                if (Obj.UserType == "C" || Obj.UserType == "K")
                {
                    if (Obj != null && Obj.SSOID != null)
                        return dt;
                    // WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "' ") : "";
                }
                if (Obj.TypeId > 0)
                {
                    if (Obj.TypeId == 7)
                    { } ////WhereClause += " and societies.SocietyTypeId in (7,10)" ;
                    else
                        WhereClause += " and societies.SocietyTypeId=" + Obj.TypeId;
                }

                if (Obj.DIVISION_ID > 0 && Obj.DistrictId == 0)
                {
                    WhereClause += " and SROD.DISTRICT_ID in (select DISTRICT_ID from District where DIVISION_ID=" + Obj.DIVISION_ID + " ) ";
                }
                if (Obj.DIVISION_ID > 0 && Obj.DistrictId != 0)
                {
                    WhereClause += " and SROD.DIVISION_ID=" + Obj.DIVISION_ID;
                }

                if (Obj.SocietyCategoryId > 0)
                {
                    WhereClause += " and societies.SocietyCategoryId=" + Obj.SocietyCategoryId;
                }

                if (Obj != null && Obj.IsRural == true)
                {
                    if (Obj.DistrictId > 0)
                    {
                        WhereClause += " and SROD.DISTRICT_ID=" + Obj.DistrictId;
                    }
                    if (Obj.CITY_ID > 0)
                    {
                        WhereClause += " and SROD.CITY_ID=" + Obj.CITY_ID;
                    }
                    if (Obj.WardId > 0)
                    {
                        WhereClause += " and SROD.WARD_ID=" + Obj.WardId;
                    }
                }
                else if (Obj != null && Obj.IsRural == false)
                {
                    if (Obj.DistrictId > 0)
                    {
                        WhereClause += " and SROD.DISTRICT_ID=" + Obj.DistrictId;
                    }
                    if (Obj.BlockId > 0)
                    {
                        WhereClause += " and SROD.BLOCK_ID=" + Obj.BlockId;
                    }
                    if (Obj.GP_ID > 0)
                    {
                        WhereClause += " and SROD.GP_ID=" + Obj.GP_ID;
                    }
                    if (Obj.VillageId > 0)
                    {
                        WhereClause += " and SROD.VILLAGE_ID=" + Obj.VillageId;
                    }
                }

                var searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(searchText))
                {
                    WhereClause += " and (SocietyName like '%" + searchText + "%' or RegistrationNo like '%" + searchText + "%')";
                }


                objDBAccess.OpenConnection();

                SqlCommand cmd = new SqlCommand();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("SocietyListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ElectionForId", DBFactoryParameterType.NVarchar, Obj.ElectionForId);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }


                cmd = objDBAccess.CreateCommand("SocietyListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionForId", DBFactoryParameterType.NVarchar, Obj.ElectionForId);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable SocietyListForElection(ElectionSocietyData Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public bool UpdateElectionMasterOrderPDFSign(string ElectionMasterId, bool IsSign)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            bool result = true;
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ElectionMasterOrderPDFSign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                objDBAccess.AddParameterToCommand("@IsSign", DBFactoryParameterType.Bit, IsSign);
                objDBAccess.ExecuteCommand();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "UpdateElectionMasterOrderPDFSign(string ElectionMasterId, bool IsSign)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public DataTable updateElectionOrderPDF(string PDFPath, int OrderId, int flag = 0)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateElectionOrderPDF", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PDFPath", DBFactoryParameterType.NVarchar, PDFPath);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.Int, OrderId);
                objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, flag);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "updateElectionOrderPDF(string PDFPath, int OrderId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionEmailSMSDetails(int MessageId, int ElectionOrder, int ElectionMasterId, int SocietyId)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetElectionEmailSMSDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@MessageId", DBFactoryParameterType.Int, MessageId);
                objDBAccess.AddParameterToCommand("@ElectionOrder", DBFactoryParameterType.Int, ElectionOrder);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = " GetElectionEmailSMSDetails(int MessageId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string ElecCurrentStatus(string OrderId)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            string strResult = "";
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ElectionOrderCurrentStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.NVarchar, OrderId);
                strResult = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "ElecCurrentStatus(string OrderId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return strResult;
        }






        public DataTable GetElectionOrderList(string Todate, string FormDate, int StatusId, string Datewise, int IsReturingOfficer)
        {
            string whereCase = "";
            //IsReturingOfficer = 0;

            if (FormDate != "" && Todate != "")
            {
                FormDate = FormDate.Replace("-", "/");
                FormDate = FormDate.Split('/')[2] + "-" + FormDate.Split('/')[1] + "-" + FormDate.Split('/')[0];
                Todate = Todate.Replace("-", "/");
                Todate = Todate.Split('/')[2] + "-" + Todate.Split('/')[1] + "-" + Todate.Split('/')[0];
                whereCase += "where  t.ElectionStartDate" + " between '" + FormDate + "' and '" + Todate + "'";
            }
            if (FormDate != "" && Todate == "")
            {
                FormDate = FormDate.Replace("-", "/");
                FormDate = FormDate.Split('/')[2] + "-" + FormDate.Split('/')[1] + "-" + FormDate.Split('/')[0];
                whereCase += "where  t.ElectionStartDate" + " between '" + FormDate + "' and '" + FormDate + "'";
            }
            if (FormDate == "" && Todate != "")
            {
                Todate = Todate.Replace("-", "/");
                Todate = Todate.Split('/')[2] + "-" + Todate.Split('/')[1] + "-" + Todate.Split('/')[0];
                whereCase += "where  t.ElectionStartDate" + " between '" + Todate + "' and '" + Todate + "'";
            }
            if (StatusId != 0 && Datewise != "" && FormDate == "" && Todate == "")
            {
                Datewise = Datewise.Replace("-", " ");
                Datewise = Datewise.Split('/')[2] + "-" + Datewise.Split('/')[1] + "-" + Datewise.Split('/')[0];
                whereCase += "where  t.ElectionStartDate" + "=" + " '" + Datewise + "' and " + "t.ElectionStatus" + "='" + StatusId + "'";
            }

            if (StatusId != 0 && Datewise == "" && FormDate == "" && Todate == "")
            {

                whereCase += "where  t.ElectionStartDate" + "=" + " '" + "" + "' or " + "t.ElectionStatus" + "='" + StatusId + "'";
            }
            if (StatusId == 0 && Datewise != "" && FormDate == "" && Todate == "")
            {
                Datewise = Datewise.Replace("-", " ");
                Datewise = Datewise.Split('/')[2] + "-" + Datewise.Split('/')[1] + "-" + Datewise.Split('/')[0];
                whereCase += "where  t.ElectionStartDate" + "=" + " '" + Datewise + "' or " + "t.ElectionStatus" + "='" + StatusId + "'";
            }
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OrderListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@IsReturingOfficer", DBFactoryParameterType.Int, IsReturingOfficer);
                objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionOrderList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int UpdatedElectionStatus(ElectionSocietyData _Model)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();


            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdatedElectionStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, _Model.ElectionMasterId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, _Model.ElectionSocietyStatus);
                objDBAccess.AddParameterToCommand("@ElectionStopDocument", DBFactoryParameterType.NVarchar, _Model.ElectionStopDocument);
                objDBAccess.AddParameterToCommand("@ElectionDescription", DBFactoryParameterType.NVarchar, _Model.ElectionRemark);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, 1);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable UpdatedElectionStatus()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }


            foreach (Election_SocityIds item in _Model.Election_SocityIds)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("UpdatedElectionStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.BigInt, _Model.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, item.SocietyId1);
                    objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, _Model.ElectionSocietyStatus);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, 2);
                    dt = objDBAccess.GetTable();
                    //int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                }
            }
            return Result;
        }



        public DataTable ElectionOfficerForSociety(string OredrId)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                string WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ElectionOfficerForSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.NVarchar, OredrId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable ElectionOfficerForSociety(string OredrId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyListByOrderID(string OredrId)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {

                // narottam
                string WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ElectionSocietyListByOrderId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.NVarchar, OredrId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietyListByOrderID(string OredrId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietiesByelectionOfficerId(string OfficerId)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {

                // narottam
                string WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietiesByelectionOfficerId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficerId", DBFactoryParameterType.Int, OfficerId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietiesByelectionOfficerId(string OredrId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietiesWihEOByOrderId(string OrderId)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {

                // narottam
                string WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietiesWihEOByOrderId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.Int, OrderId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietiesWihEOByOrderId(string OredrId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable ElectionPublishList(string id)
        {
            string WhereClause = "1=1";
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetElectionQuestionsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@OrderId", DBFactoryParameterType.NVarchar, id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable ElectionPublishList(string id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionDates(string id)
        {
            string WhereClause = "1=1";
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetElectionDates", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionDates(string id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocietylist(string id)
        {

            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyListByElectionID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionDates(string id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public int InsertElectionSociety(ElectionSocietyData _Model)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            if (_Model != null && _Model.SocityIdlst != null && _Model.SocityIdlst.Count > 0)
            {

                try
                {
                    //select * from ElectionMaster
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertElectionSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Model.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@NoOfSociety", DBFactoryParameterType.NVarchar, _Model.SocityIdlst.Count);
                    objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, _Model.CreationDate);
                    objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, _Model.UpdationDate);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                    objDBAccess.AddParameterToCommand("@ReturningOfficerID", DBFactoryParameterType.NVarchar, _Model.ReturningOfficerID);
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.NVarchar, _Model.DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _Model.DistrictId);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, _Model.SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@StepInElection", DBFactoryParameterType.NVarchar, _Model.StepInElection);
                    objDBAccess.AddParameterToCommand("@ElectionFor", DBFactoryParameterType.NVarchar, _Model.ElectionFor);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "InsertElectionSociety(ElectionSocietyData _Model)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }

                string ids = string.Join(",", _Model.SocityIdlst.Select(x => x.SocietyIdEnc));

                foreach (SocityIds item in _Model.SocityIdlst)
                {
                    try
                    {
                        //SocietyForElection
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.BigInt, Result);
                        objDBAccess.AddParameterToCommand("@SocityIdEnc", DBFactoryParameterType.NVarchar, item.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                       // objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, _Model.CreationDate);
                       // objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, _Model.UpdationDate);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                        objDBAccess.AddParameterToCommand("@IsSocietyCompleted", DBFactoryParameterType.NVarchar, 0);
                        objDBAccess.AddParameterToCommand("@SocietiId", DBFactoryParameterType.NVarchar, ids);

                        int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "int  InsertElectionSociety(ElectionSocietyData _Model)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                }

            }



            return Result;
        }

        public int InsertElectionSocietyDate(List<Socitydates> objElectionSocietyData, int ElectionMasterId, string HeaderContantDetails, bool IsInsert = true)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            int Result1 = 0;

            //, int CustomHeaderValue


            foreach (Socitydates item in objElectionSocietyData)
            {
                try
                {
                    var Dateformat = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                    //  Member.DOB = DateTime.ParseExact(txt_DOB.Text, Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None);
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertElectionProDates", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@El_QuestionId", DBFactoryParameterType.Int, item.El_QuestionId);

                    if (!(item.Stap1Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap1Date", DBFactoryParameterType.DateTime, item.Stap1Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap2Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap2Date", DBFactoryParameterType.DateTime, item.Stap2Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap3Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap3Date", DBFactoryParameterType.DateTime, item.Stap3Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap4Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap4Date", DBFactoryParameterType.DateTime, item.Stap4Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap5Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap5Date", DBFactoryParameterType.DateTime, item.Stap5Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                    if (!(item.Stap6Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap6Date", DBFactoryParameterType.DateTime, item.Stap6Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap7Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap7Date", DBFactoryParameterType.DateTime, item.Stap7Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap8Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap8Date", DBFactoryParameterType.DateTime, item.Stap8Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap9Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap9Date", DBFactoryParameterType.DateTime, item.Stap9Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                    if (!(item.Stap10Date.ToString("dd/MM/yyyy").Contains("0001")))
                        objDBAccess.AddParameterToCommand("@Stap10Date", DBFactoryParameterType.DateTime, item.Stap10Date.ToString("dd/MM/yyyy").ConvertString_ToValidDate());



                    objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, item.StartTime);
                    objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, item.EndTime);
                    objDBAccess.AddParameterToCommand("@OtherTime", DBFactoryParameterType.NVarchar, item.OtherTime);
                    objDBAccess.AddParameterToCommand("@IsInsert", DBFactoryParameterType.Bit, IsInsert);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, 1);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }



                catch (Exception ex)
                {
                    Result = 0;
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "InsertElectionSocietyDate(ElectionSocietyData _Model)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }

            if (HeaderContantDetails != null && HeaderContantDetails != "")
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertElectionProDates", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@HeaderContentDetails", DBFactoryParameterType.NVarchar, HeaderContantDetails);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, 2);
                    Result1 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    objDBAccess.CloseConnection();
                }

            }


            return Result;

        }

        public int InsertSocietyElectionOfficer(List<SocietyElectionOfficer> _Model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;

            string newId = string.Join(",", _Model.Select(x => x.OfficerId));

            foreach (SocietyElectionOfficer item in _Model)
            {
                try
                {
                    var Dateformat = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                    //  item.ElectionMasterId = Convert.ToInt32(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc));
                    //  Member.DOB = DateTime.ParseExact(txt_DOB.Text, Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None);
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyElectionOfficer", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyElecOffiId", DBFactoryParameterType.Int, item.SocietyElecOffiId);
                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, item.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, item.SocietyId);
                    objDBAccess.AddParameterToCommand("@StepId", DBFactoryParameterType.Int, item.StepId);
                    objDBAccess.AddParameterToCommand("@OfficerId", DBFactoryParameterType.Int, item.OfficerId);
                    objDBAccess.AddParameterToCommand("@CreateBY", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UpdateBY", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime, System.DateTime.Now);
                    objDBAccess.AddParameterToCommand("@UpdateDate", DBFactoryParameterType.DateTime, System.DateTime.Now);
                    objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                    objDBAccess.AddParameterToCommand("@UpdateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                    objDBAccess.AddParameterToCommand("@NewId", DBFactoryParameterType.NVarchar, newId);
                    //   objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, item.StartDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(item.StartDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));
                    //   objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, item.EndDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(item.EndDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));

                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "int InsertSocietyElectionOfficer(List<SocietyElectionOfficer> _Model)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            return Result;

        }
        #endregion

        #region Voter and Candidate

        public DataTable GetElectionSocietyListData(int StepType)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("SocietyMemberVoterElectionDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionOfficerId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionSocietyListData()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string InsertSocietyMemberDetails(List<VoterMemberData> objData, bool IsSubmit, bool IsPublish)
        {
            string Result = "-1";

            try
            {
                string SocietyCurrentStatus = "";
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }
                    if (obj.MemberPDFForm != null)
                    {
                        if (obj.MemberPDFForm.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.MemberPDFForm.ToString().Split('/');
                            obj.MemberPDFForm = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.MemberPDFForm.ToString().Contains("Balnk"))
                        {

                            obj.MemberPDFForm = "";
                        }
                    }
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    if (i == 0)
                    {
                        ////cmd = objDBAccess.CreateCommand("CheckSocietyMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        ////objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        ////objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, _LoggedInUser.UserType.ToUpper() == "C");// obj.IsCEO);
                        ////objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                        ////if (_LoggedInUser.UserType.ToUpper() == "C") //(obj.IsCEO)
                        ////{
                        ////    objDBAccess.AddParameterToCommand("@LevelFlag", DBFactoryParameterType.Int, 0);
                        ////}
                        ////else
                        ////{
                        ////    objDBAccess.AddParameterToCommand("@LevelFlag", DBFactoryParameterType.Int, 1);
                        ////}
                        ////int FlagTest = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        ////if (FlagTest > 0)
                        ////{
                        ////    Result = "-2";
                        ////    break;
                        ////}
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);

                        cmd = objDBAccess.CreateCommand("InsertMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, _LoggedInUser.UserType.ToUpper() == "C"); // obj.IsCEO);
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@FHName", DBFactoryParameterType.NVarchar, obj.FHName);
                        objDBAccess.AddParameterToCommand("@MemberPDFForm", DBFactoryParameterType.NVarchar, obj.MemberPDFForm);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                        objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Bit, obj.IsDelete);
                        objDBAccess.AddParameterToCommand("@DeleteRemarks", DBFactoryParameterType.NVarchar, obj.DeleteRemarks);
                        objDBAccess.AddParameterToCommand("@AddRemarks", DBFactoryParameterType.NVarchar, obj.AddRemarks);
                        //objDBAccess.AddParameterToCommand("@AddDocument", DBFactoryParameterType.NVarchar, obj.AddDocument);
                        objDBAccess.AddParameterToCommand("@DeleteDocument", DBFactoryParameterType.NVarchar, obj.DeleteDocument);
                        objDBAccess.AddParameterToCommand("@IsSubmit", DBFactoryParameterType.NVarchar, _LoggedInUser.UserType.ToUpper() == "C" ? IsSubmit : true);

                        if (_LoggedInUser.UserType.ToUpper() == "C")
                        {
                            if (IsSubmit == true)
                            {

                                SocietyCurrentStatus = "Permanently Added" + " " + obj.Name + " " + " in  Voter List by CEO";
                                objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                            }
                            else
                            {
                                if (obj.IsDelete == true)
                                {
                                    SocietyCurrentStatus = "Deleted " + " " + obj.Name + " " + " in  voter list by CEO";
                                    objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);

                                }
                                else
                                {

                                    SocietyCurrentStatus = "Added" + " " + obj.Name + " " + " in  Voter List by CEO";
                                    objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                                }

                            }
                        }
                        else
                        {

                            if (IsPublish == true)
                            {
                                SocietyCurrentStatus = "Permanently Added" + " " + obj.Name + " " + " in  Voter List by Election Officer";
                                objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                            }
                            else
                            {

                                if (IsSubmit == true)
                                {
                                    if (obj.IsDelete == true)
                                    {
                                        SocietyCurrentStatus = "Deleted " + " " + obj.Name + " " + " in  voter list by Election Officer";
                                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                                    }
                                    else
                                    {
                                        SocietyCurrentStatus = "Added" + " " + obj.Name + " " + " in  Voter List by Election Officer";
                                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                                    }

                                }
                                else
                                {
                                    if (obj.IsDelete == true)
                                    {
                                        SocietyCurrentStatus = "Deleted " + " " + obj.Name + " " + " in  voter list by Election Officer";
                                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);
                                    }
                                    else
                                    {
                                        SocietyCurrentStatus = "Added" + " " + obj.Name + " " + " in  Voter List by Election Officer";
                                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, SocietyCurrentStatus);

                                    }

                                }

                            }

                        }
                        objDBAccess.AddParameterToCommand("@IsPublish", DBFactoryParameterType.NVarchar, _LoggedInUser.UserType.ToUpper() == "C" ? false : IsPublish);


                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        //if ((Result.Split('@').Length > 1))
                        //    EMitraTransactionId = int.Parse(Result.Split('@')[1]);

                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string InsertSocietyMemberDetails(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string InsertElectionCandidateDetails(List<VoterMemberData> objData, bool IsSubmit, bool IsPublish)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }
                    if (obj.MemberPDFForm != null)
                    {
                        if (obj.MemberPDFForm.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.MemberPDFForm.ToString().Split('/');
                            obj.MemberPDFForm = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.MemberPDFForm.ToString().Contains("Balnk"))
                        {

                            obj.MemberPDFForm = "";
                        }
                    }
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();


                    if (obj.Name.Length > 0 && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);

                        cmd = objDBAccess.CreateCommand("InsertElectionCandidateDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, _LoggedInUser.UserType.ToUpper() == "C");
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@FHName", DBFactoryParameterType.NVarchar, obj.FHName);
                        objDBAccess.AddParameterToCommand("@MemberPDFForm", DBFactoryParameterType.NVarchar, obj.MemberPDFForm);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, obj.MembershipNo);
                        objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Bit, obj.IsDelete);
                        objDBAccess.AddParameterToCommand("@DeleteRemarks", DBFactoryParameterType.NVarchar, obj.DeleteRemarks);
                        objDBAccess.AddParameterToCommand("@AddRemarks", DBFactoryParameterType.NVarchar, obj.AddRemarks);
                        objDBAccess.AddParameterToCommand("@DeleteDocument", DBFactoryParameterType.NVarchar, obj.DeleteDocument);
                        objDBAccess.AddParameterToCommand("@IsSubmit", DBFactoryParameterType.NVarchar, _LoggedInUser.UserType.ToUpper() == "C" ? IsSubmit : true);
                        objDBAccess.AddParameterToCommand("@IsPublish", DBFactoryParameterType.NVarchar, _LoggedInUser.UserType.ToUpper() == "C" ? false : IsPublish);






                        objDBAccess.AddParameterToCommand("@Presentedby", DBFactoryParameterType.Int, obj.Presentedby);
                        objDBAccess.AddParameterToCommand("@NewMemberName", DBFactoryParameterType.NVarchar, obj.NewMemberName);
                        objDBAccess.AddParameterToCommand("@NewRemark", DBFactoryParameterType.NVarchar, obj.NewRemark);

                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, obj.ActivityStatus);




                        //if (obj.MemberPDFForm != "" && IsSubmit == false)
                        //{
                        //    string statsus = "Election officer upload Nomination form" + "-" + obj.Name;
                        //    objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, statsus);
                        //}

                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        Result = "1";
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string InsertSocietyMemberDetails(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public ResultMessage InsertUpdateElectionMemberFile(List<VoterMemberData> objData, int Status, int StepType)
        {

            ResultMessage result = new ResultMessage() { status = "-1" };
            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


                string ids = string.Join(",", objData.Where(x => x.MemberId > 0 && x.IsDelete == false).Select(x => x.MemberId));

                foreach (var obj in objData)
                {

                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();


                    cmd = objDBAccess.CreateCommand("InsertUpdateElectionMemberFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@id", DBFactoryParameterType.Int, obj.MemberId);
                    objDBAccess.AddParameterToCommand("@election_id", DBFactoryParameterType.NVarchar, obj.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@filename", DBFactoryParameterType.NVarchar, obj.Photo);
                    objDBAccess.AddParameterToCommand("@status", DBFactoryParameterType.NVarchar, Status);
                    objDBAccess.AddParameterToCommand("@remarks", DBFactoryParameterType.NVarchar, obj.AddRemarks);
                    objDBAccess.AddParameterToCommand("@createdby", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@step_id", DBFactoryParameterType.Int, StepType);
                    objDBAccess.AddParameterToCommand("@ids", DBFactoryParameterType.NVarchar, ids);
                    objDBAccess.AddParameterToCommand("@isdelete", DBFactoryParameterType.Bit, obj.IsDelete);
                    objDBAccess.AddParameterToCommand("@issubmit", DBFactoryParameterType.Bit, Status);
                    objDBAccess.AddParameterToCommand("@delete_pdf", DBFactoryParameterType.Bit, obj.DeleteDocument);
                    result.data = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    result.status = "1";

                    i++;
                }
            }
            catch (Exception ex)
            {
                result.status = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string InsertUpdateElectionMemberFile(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public ResultMessage DeleteElectionCandidate(VoterMemberData objData)
        {

            ResultMessage result = new ResultMessage() { status = "-1" };
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("DeleteElectionCandidate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, objData.ElectionMasterId);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, objData.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, objData.AADHAARNo);
                result.data = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                result.status = "1";



            }
            catch (Exception ex)
            {
                result.status = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string DeleteElectionCandidate(VoterMemberData objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public ResultMessage DeleteElectionMemberList(int Id, string Remarks, string Delete_pdf)
        {

            ResultMessage result = new ResultMessage() { status = "-1" };
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("DeleteElectionmemberList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Remarks);
                objDBAccess.AddParameterToCommand("@Delete_pdf", DBFactoryParameterType.NVarchar, Delete_pdf);
                result.data = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                result.status = "1";

            }
            catch (Exception ex)
            {
                result.status = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string DeleteElectionmemberList(int Id,string Remarks)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public string InsertElectionResult(List<ElectionResultDataData> objData, bool IsSubmit)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;
                string SelectedCandidate = string.Join(",", objData.Where(x => x.IsWinner).Select(x => x.MemberId));
                foreach (var obj in objData)
                {

                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();

                    cmd = objDBAccess.CreateCommand("InsertElectionResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                    objDBAccess.AddParameterToCommand("@TotalVotes", DBFactoryParameterType.Int, obj.TotalVotes);
                    objDBAccess.AddParameterToCommand("@IsWinner", DBFactoryParameterType.Bit, obj.IsWinner);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@Documet", DBFactoryParameterType.NVarchar, obj.ResultDocument);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, obj.ResultRemark);
                    objDBAccess.AddParameterToCommand("@IsSubmit", DBFactoryParameterType.NVarchar, IsSubmit);

                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, obj.Type);
                    objDBAccess.AddParameterToCommand("@SelectedCandidate", DBFactoryParameterType.NVarchar, SelectedCandidate);
                    objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.NVarchar, obj.PostId);

                    objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, obj.ActivityStatus);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedIP);


                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());@Remark
                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    //if ((Result.Split('@').Length > 1))
                    //    EMitraTransactionId = int.Parse(Result.Split('@')[1]);


                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string InsertElectionResult(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string UpdateElectionStatusFile(string FileName, string Type, int SocietyId, int ElectionMasterId)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                //int CheckInsertUpdate = 0;



                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                cmd = objDBAccess.CreateCommand("UpdateElectionStatusFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);


                //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());@Remark
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                //if ((Result.Split('@').Length > 1))
                //    EMitraTransactionId = int.Parse(Result.Split('@')[1]);


                i++;

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string InsertElectionResult(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string UpdateSocietyMemberDetails(List<VoterMemberData> objData, bool IsSubmitCandidate, bool IsPublish)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;
                string SelectedCandidate = string.Join(",", objData.Select(x => x.MemberId));
                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.CandidatePDFForm != null)
                    {
                        if (obj.CandidatePDFForm.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.CandidatePDFForm.ToString().Split('/');
                            obj.CandidatePDFForm = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.CandidatePDFForm.ToString().Contains("Balnk"))
                        {

                            obj.CandidatePDFForm = "";
                        }
                    }
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();


                    if (i == 0)
                    {
                        //cmd = objDBAccess.CreateCommand("CheckSocietyMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, obj.IsCEO);
                        //objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                        //objDBAccess.AddParameterToCommand("@LevelFlag", DBFactoryParameterType.Int, 2);
                        //int FlagTest = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        //if (FlagTest > 0)
                        //{
                        //    Result = "-2";
                        //    break;
                        //}
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);

                        cmd = objDBAccess.CreateCommand("UpdateMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, obj.IsCEO);
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@CandidatePDFForm", DBFactoryParameterType.NVarchar, obj.CandidatePDFForm);
                        objDBAccess.AddParameterToCommand("@BODPDFForm", DBFactoryParameterType.NVarchar, obj.BODPDFForm);
                        objDBAccess.AddParameterToCommand("@IsCandidate", DBFactoryParameterType.Bit, obj.IsCandidate);
                        objDBAccess.AddParameterToCommand("@SelectedCandidate", DBFactoryParameterType.NVarchar, SelectedCandidate);
                        objDBAccess.AddParameterToCommand("@IsSubmitCandidate", DBFactoryParameterType.NVarchar, IsSubmitCandidate);
                        objDBAccess.AddParameterToCommand("@ActionOnCandidate", DBFactoryParameterType.Int, obj.ActionOnCandidate);
                        objDBAccess.AddParameterToCommand("@ActionRemark", DBFactoryParameterType.NVarchar, obj.ActionRemark);
                        objDBAccess.AddParameterToCommand("@ActionDocument", DBFactoryParameterType.NVarchar, obj.ActionDocument);
                        objDBAccess.AddParameterToCommand("@IsPublishCandidate", DBFactoryParameterType.Bit, IsPublish);
                        objDBAccess.AddParameterToCommand("@IsPostSubmited", DBFactoryParameterType.Bit, obj.IsPostSubmited);
                        objDBAccess.AddParameterToCommand("@CandidateSymbol", DBFactoryParameterType.NVarchar, obj.CandidateSymbol);
                        objDBAccess.AddParameterToCommand("@SocietyCurrentStatus", DBFactoryParameterType.NVarchar, obj.ActivityStatus);

                        objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.NVarchar, obj.IsDelete);

                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string UpdateSocietyMemberDetails(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string InsertUpdateElecionPost(List<VoterMemberData> objData, bool IsSubmitCandidate, bool IsPublish)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string SelectedCandidate = string.Join(",", objData.Select(x => x.MemberId));
                foreach (var obj in objData)
                {

                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();

                    if (obj.Name.Length > 0 && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);

                        cmd = objDBAccess.CreateCommand("InsertUpdateElecionPost", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@BODPDFForm", DBFactoryParameterType.NVarchar, obj.BODPDFForm);
                        objDBAccess.AddParameterToCommand("@ActionOnCandidate", DBFactoryParameterType.Int, obj.ActionOnCandidate);
                        objDBAccess.AddParameterToCommand("@ActionRemark", DBFactoryParameterType.NVarchar, obj.ActionRemark);
                        objDBAccess.AddParameterToCommand("@ActionDocument", DBFactoryParameterType.NVarchar, obj.ActionDocument);
                        objDBAccess.AddParameterToCommand("@IsSubmitElectionPost", DBFactoryParameterType.Bit, obj.IsPostSubmited);
                        objDBAccess.AddParameterToCommand("@SelectedCandidate", DBFactoryParameterType.NVarchar, SelectedCandidate);
                        objDBAccess.AddParameterToCommand("@IsPublishPost", DBFactoryParameterType.NVarchar, IsPublish);
                        objDBAccess.AddParameterToCommand("@OfficerSymbol", DBFactoryParameterType.NVarchar, obj.OfficerSymbol);

                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string UpdateSocietyMemberDetails(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string UpdateSocietyElectedMemberDetails(List<VoterMemberData> objData)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.BODPDFForm != null)
                    {
                        if (obj.BODPDFForm.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.BODPDFForm.ToString().Split('/');
                            obj.BODPDFForm = temp.Length > 4 ? temp[4].ToString() : "";
                        }
                        if (obj.BODPDFForm.ToString().Contains("Balnk"))
                        {

                            obj.BODPDFForm = "";
                        }
                    }
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();


                    if (i == 0)
                    {
                        cmd = objDBAccess.CreateCommand("CheckSocietyMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, obj.IsCEO);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, obj.ElectionMasterId);
                        objDBAccess.AddParameterToCommand("@LevelFlag", DBFactoryParameterType.Int, 3);
                        int FlagTest = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        if (FlagTest > 0)
                        {
                            Result = "-2";
                            break;
                        }
                    }

                    if (obj.Name.Length > 0 && obj.CategoryId > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);

                        cmd = objDBAccess.CreateCommand("UpdateMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.Int, obj.MemberId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, obj.IsCEO);
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@BODPDFForm", DBFactoryParameterType.NVarchar, obj.BODPDFForm);
                        objDBAccess.AddParameterToCommand("@IsCandidate", DBFactoryParameterType.Bit, false);
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "string UpdateSocietyMemberDetails(List<VoterMemberData> objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable InsertSocityVoterMemberByExcel(SocietyVoterMemberEntity obj)
        {
            int Result = -1;
            DataTable dt = new DataTable();
            string connString = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocityExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (Result > 0)
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

                    //OleDbConnection oleDbXLS = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" +"Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';");
                    //OleDbCommand OleDbcomSelect = new OleDbCommand(@"SELECT * FROM [MemberDetail$]", oleDbXLS);
                    //OleDbDataAdapter OleDbdaMain = new OleDbDataAdapter();
                    //DataTable oReturn = new DataTable();
                    //oleDbXLS.Open();
                    //OleDbdaMain.SelectCommand = OleDbcomSelect;
                    //OleDbdaMain.Fill(oReturn);

                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        if (SheetRow.ItemArray[2].ToString() == "MemberDetail$")
                        {
                            string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                            OleDbCommand cmd2 = new OleDbCommand(query, conn);
                            OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                            da.Fill(dt);

                            dt.Columns.Add("SmallDOB", typeof(string));
                            foreach (DataRow Dr in dt.Rows)
                            {
                                if (Dr["DOB"] != null && Dr["DOB"].ToString().Length > 0)
                                {
                                    //DateTime dtOcal = DateTime.ParseExact(Dr["DOB"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    //  string sdsdd = Dr["DOB"].ToString("dd/MM/YYYY");
                                    //var DateArr = Dr["DOB"].ToString().Split(' ');
                                    //string strDate = DateArr[0];
                                    //var DateArr1 = strDate.Split('/');
                                    //if(DateArr1.Length==3)
                                    //{
                                    //    string strDate2 = DateArr1[1] + "/" + DateArr1[0] + "/" + DateArr1[2];
                                    //    DataRow drNew = dt.NewRow();
                                    //    Dr["DOB"] = strDate2;
                                    //   // dt.Rows.Add(drNew);
                                    //}
                                    Dr["SmallDOB"] = Convert.ToDateTime(Dr["DOB"]).ToString("dd/MM/yyyy");
                                    //_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                                }
                            }
                            break;

                            //////objDBAccess.OpenConnection();
                            //////SqlCommand cmd1 = objDBAccess.CreateCommand("InsertSocityExcelDetailsBySheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                            //////cmd1.Parameters.AddWithValue("@UDTSocietyBulkInsert", dt);
                            //////cmd1.Parameters.AddWithValue("@UDTSocietyAdministriveBulkInsert", null);
                            //////cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                            //////cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                            //////cmd1.Parameters.AddWithValue("@SheetName", SheetRow.ItemArray[2].ToString());
                            //////Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable InsertSocityVoterMemberByExcel(SocietyVoterMemberEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetVoterMemberDetailsBySocietyId(string SocietyIdEnc, bool IsCEO, int ElectionMasterId, int StepType)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("MemberDetailsBySocietyId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, IsCEO);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);

                //objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(SearchText) ? COMMON_FUNCTION.Encrypt(SearchText) : ""));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetVoterMemberDetailsBySocietyId(string SocietyIdEnc, bool IsCEO, int ElectionMasterId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetElectionMemberFile(string SocietyIdEnc, bool IsCEO, int ElectionMasterId, int StepType)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetElectionMemberFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, IsCEO);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);

                //objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(SearchText) ? COMMON_FUNCTION.Encrypt(SearchText) : ""));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionMemberFile(string SocietyIdEnc, bool IsCEO, int ElectionMasterId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionMemberFileSubmitDate(string SocietyIdEnc, bool IsCEO, int ElectionMasterId, int StepType)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetElectionMemberFileSubmitDate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@IsCEO", DBFactoryParameterType.Bit, IsCEO);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);

                //objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(SearchText) ? COMMON_FUNCTION.Encrypt(SearchText) : ""));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionMemberFileSubmitDate(string SocietyIdEnc, bool IsCEO, int ElectionMasterId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionDashboard()
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetElectionDashboardCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionDashboard()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetElectionOrdersForDashboard()
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetElectionOrdersForDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionOrdersForDashboard()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionSocietyDetails(int ElectionMasterId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetElectionSocietyDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable  GetElectionSocietyDetails(int ElectionMasterId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetSocietyListForElection(int StepType, string UserId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyListForElectionBySSOID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@ElectionOfficerId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietyListForElection()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyListForElection_New(ref DataPagingModel Details, int StepType, string UserId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string whereCase = UserId == "0" ? " and  s.SSOID='" + _LoggedInUser.SSOID + "' " : " and ElectionOfficerId='" + UserId + "'";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "SubmissionDate")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and (select top 1
			case when(SELECT StepId  FROM[SocietyForElection] where ElectionMasterId = e.ElectionMasterId  and  SocietyId = e.SocietyId) = 1 then  convert(date,Stap1Date,111) else 
			case when(SELECT StepId  FROM[SocietyForElection] where ElectionMasterId = e.ElectionMasterId  and  SocietyId = e.SocietyId) = 2 then  convert(date,Stap2Date,111) else 
			case when(SELECT StepId  FROM[SocietyForElection] where ElectionMasterId = e.ElectionMasterId  and  SocietyId = e.SocietyId) = 3 then  convert(date,Stap3Date,111) else 
			case when(SELECT StepId  FROM[SocietyForElection] where ElectionMasterId = e.ElectionMasterId  and  SocietyId = e.SocietyId) = 4 then  convert(date,Stap4Date,111) else 
			case when(SELECT StepId  FROM[SocietyForElection] where ElectionMasterId = e.ElectionMasterId  and  SocietyId = e.SocietyId) = 5 then  convert(date,Stap5Date,111) 
                     end  end end end end from SocietyElectionDates  where ElectionMasterId = e.ElectionMasterId and El_QuestionId = 1)";
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

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyListForElectionBySSOID_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                cmd = objDBAccess.CreateCommand("GetSocietyListForElectionBySSOID_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StepType", DBFactoryParameterType.Int, StepType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetSocietyListForElection_New(ref DataPagingModel Details, int StepType, string UserId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetSocietyElectionDetail(int ElectionMasterId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("GetSocietyElectionDetailByElectionMasterId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietyElectionDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetCommentListbySociety(int SocietyId)
        {
            dt.Clear();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("Get_SocietyCommentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetSocietyElectionDetail()" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }





        public DataTable updateElectionOrderForSocietyPDF(int ElectionMasterId, int SocietyId, string OrderFile)
        {
            DBFactoryMSSQL objDBAccess;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GenerateElectionOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionMasterId);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@OrderFile", DBFactoryParameterType.NVarchar, OrderFile);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@createip", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedIP);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "public DataTable updateElectionOrderForSocietyPDF(int ElectionMasterId, int SocietyId,string OrderFile)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetAllElectionSymbol(string WhereCase)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllElectionSymbole", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereCase);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetAllElectionSymbol(string WhereCase)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCandidateElectionSymbol(int ElectionMasterId, int CandidateId, int Type)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCandidateElectionSymbol", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@CandidateId", DBFactoryParameterType.NVarchar, CandidateId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetCandidateElectionSymbol(int ElectionMasterId, int CandidateId, int Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetElectionSocietyReportList(ref DataPagingModel Details, ElectionSocietyData objElectionSocietyData)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            string WhereClause = "";

            //if (Details.SearchParameter.Count>0)
            //{
            //    foreach (var x in Details.SearchParameter.Where(x => x.Key == "ComplaintDate"))
            //    {
            //            WhereClause += " and ElectionOfficerComment.CreationDate='" + x.Value + "'";
            //    }  
            //}

            if (objElectionSocietyData != null && objElectionSocietyData.IsRural == true)
            {
                if (objElectionSocietyData.DistrictId > 0)
                {
                    WhereClause += " and SROD.DISTRICT_ID=" + objElectionSocietyData.DistrictId;
                }
                if (objElectionSocietyData.CITY_ID > 0)
                {
                    WhereClause += " and SROD.CITY_ID=" + objElectionSocietyData.CITY_ID;
                }
                if (objElectionSocietyData.WardId > 0)
                {
                    WhereClause += " and SROD.WARD_ID=" + objElectionSocietyData.WardId;
                }
            }
            else if (objElectionSocietyData != null && objElectionSocietyData.IsRural == false)
            {
                if (objElectionSocietyData.DistrictId > 0)
                {
                    WhereClause += " and SROD.DISTRICT_ID=" + objElectionSocietyData.DistrictId;
                }
                if (objElectionSocietyData.BlockId > 0)
                {
                    WhereClause += " and SROD.BLOCK_ID=" + objElectionSocietyData.BlockId;
                }
                if (objElectionSocietyData.GP_ID > 0)
                {
                    WhereClause += " and SROD.GP_ID=" + objElectionSocietyData.GP_ID;
                }
                if (objElectionSocietyData.VillageId > 0)
                {
                    WhereClause += " and SROD.VILLAGE_ID=" + objElectionSocietyData.VillageId;
                }
            }
            try
            {

                objDBAccess.OpenConnection();

                SqlCommand cmd = new SqlCommand();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetElectionSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }


                cmd = objDBAccess.CreateCommand("GetElectionSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetCandidateElectionSymbol(int ElectionMasterId, int CandidateId, int Type)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetElectionForEdit(int ElectionMasterId)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetElectionForEdit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.NVarchar, ElectionMasterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetElectionForEdit(int ElectionMasterId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string InsertUpdateElectionSymbol(ElectionSymbol objData)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("InsertUpdateElectionSymbole", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@symbol_id", DBFactoryParameterType.Int, objData.symbol_id);
                objDBAccess.AddParameterToCommand("@symbol_name_en", DBFactoryParameterType.NVarchar, objData.symbol_name_en);
                objDBAccess.AddParameterToCommand("@symbol_name_hi", DBFactoryParameterType.NVarchar, objData.symbol_name_hi);
                objDBAccess.AddParameterToCommand("@symbol_url", DBFactoryParameterType.NVarchar, objData.symbol_url);
                objDBAccess.AddParameterToCommand("@is_active", DBFactoryParameterType.Bit, objData.is_active);
                objDBAccess.AddParameterToCommand("@created_by", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@updated_by", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = " string InsertUpdateElectionSymbol(ElectionSymbol objData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string InsertElectionCandidateMemberByExcel(VoterMemberData _Model, DataTable dtSheet)
        {
            string result = "-1";
            DataTable dt = new DataTable();
            string connString = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();

                // string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (dtSheet.Rows.Count > 0)
                {

                    List<VoterMemberData> objData = new List<VoterMemberData>();
                    foreach (DataRow Dr in dtSheet.Rows)
                    {
                        string dob1 = Dr["DOB"].ToString().Replace("-", "/").Replace(".", "/").Replace("_", "/");
                        string dob = dob1.Split('/')[2] + "/" + dob1.Split('/')[1] + "/" + dob1.Split('/')[0];

                        objData.Add(new VoterMemberData()
                        {
                            AADHAARNo = Dr["AadharNo"].ToString(),
                            DOB = dob,
                            Address = Dr["Address"].ToString(),
                            Name = Dr["Name"].ToString(),
                            FHName = Dr["FatherName"].ToString(),
                            Gender = Dr["Gender"].ToString(),
                            CategoryId = Convert.ToInt32(Dr["Category"].ToString()),
                            ContactNo = Dr["ContactNo"].ToString(),
                            ElectionMasterId = _Model.ElectionMasterId,
                            SocietyIdEnc = _Model.SocietyIdEnc,
                        });





                    }

                    result = InsertElectionCandidateDetails(objData, false, false);

                }
            }
            catch (Exception ex)
            {
                result = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable InsertSocityVoterMemberByExcel(SocietyVoterMemberEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }



        public DataTable InsertOfficerCommentBySociety(ElectionOfficerComment ElectionOfficerComment)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Insert_ElectionOfficerComment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficerCommentId", DBFactoryParameterType.Int, ElectionOfficerComment.OfficerCommentId);
                objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, ElectionOfficerComment.ElectionMasterId);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, ElectionOfficerComment.SocietyId);
                objDBAccess.AddParameterToCommand("@OfficerComment", DBFactoryParameterType.NVarchar, ElectionOfficerComment.OfficerComment);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "public DataTable updateElectionOrderForSocietyPDF(int ElectionMasterId, int SocietyId,string OrderFile)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetElectionSocieties(int DistrictId, string SearchText)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetElectionSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SerchText", DBFactoryParameterType.NVarchar, SearchText);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetElectionSocieties(int DistrictId,string SearchText)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyDetailsWithMembers(string SocietyIdENC)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyDetailsWithMembers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetSocietyDetailsWithMembers(int DistrictId,string SearchText)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietiesForElectionUpdate(string SearchText)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietiesForElectionUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "GetSocietiesForElectionUpdate(int DistrictId,string SearchText)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetInspactorName(ref DataPagingModel Details)
        {
            dt.Clear();
            try
            {

                string WhereClause = "";
                int TempLevelID = 0;
                string DistrictID = "0";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string SearchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {

                    SearchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                }


                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {

                    DistrictID = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                    // whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetInspactorListDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@DistrictID", DBFactoryParameterType.NVarchar, DistrictID);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }



                cmd = objDBAccess.CreateCommand("GetInspactorListDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@DistrictID", DBFactoryParameterType.NVarchar, DistrictID);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetInspactorName()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public string InsertSocietyProfileNew(SocietyRegistrationEntity _Model)
        {
            string Result = "-1";
            string SocietyIdEnc = "";
            int i = 0;

            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                SqlCommand cmd = new SqlCommand();
                //objDBAccess.OpenConnection();
                _Model.SocietyIDEnc = _Model.SocietyIDEnc ?? "";
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;

                cmd.CommandText = "InsertNewSocietyFromElection";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;


                cmd.Parameters.AddWithValue("@SocietyIdEnc", _Model.SocietyIDEnc);
                cmd.Parameters.AddWithValue("@SocietyName", _Model.SocietyName.ToUpper().Trim());
                cmd.Parameters.AddWithValue("@SocietyTypeId", _Model.SocietyTypeId);
                cmd.Parameters.AddWithValue("@SocietyClassId", _Model.ClassId);
                cmd.Parameters.AddWithValue("@SocietySubClassId", _Model.SubClassId);
                cmd.Parameters.AddWithValue("@SocietyCategoryId", _Model.SocietyCategoryId);
                cmd.Parameters.AddWithValue("@ActId", _Model.ActId);
                cmd.Parameters.AddWithValue("@UserType", _Model.UserType);
                cmd.Parameters.AddWithValue("@IsPermanentSave", _Model.SaveStatus);
                cmd.Parameters.AddWithValue("@IsActive", _Model.IsActive);
                cmd.Parameters.AddWithValue("@IsSWS", _Model.IsSWS);
                cmd.Parameters.AddWithValue("@IsAmendmentAtProfiling", _Model.IsAmendment);
                cmd.Parameters.AddWithValue("@AmendmentType", _Model.AmendmentType);
                cmd.Parameters.AddWithValue("@SocietyCode", _Model.SocietyCode ?? "");
                cmd.Parameters.AddWithValue("@DistrictId", _Model.AddressDetails.DISTRICT_ID);
                string tempEncAadhar = "NOT";

                //_Model.AADHARNO = "307691961366";

                if (_Model.AADHARNO != null && _Model.AADHARNO.Length == 12)
                    tempEncAadhar = COMMON_FUNCTION.Encrypt(_Model.AADHARNO);



                if (_Model.KIOSKCODE.Length > 0 && System.Web.HttpContext.Current.Session[SessionIdentity.Identity] != null)
                    cmd.Parameters.AddWithValue("@AADHARNO", tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);
                else
                    cmd.Parameters.AddWithValue("@AADHARNO", tempEncAadhar == "NOT" ? _Model.AADHARNO : tempEncAadhar);



                #region Old Executive Committee

                cmd.Parameters.AddWithValue("@OldExecutiveCommittee", "");
                #endregion


                #region RegistrationCertificateFile

                cmd.Parameters.AddWithValue("@RegistrationCertificateFile", "");
                #endregion

                #region ByeLawsSocietyFile

                cmd.Parameters.AddWithValue("@ByeLawsSocietyFile", "");
                #endregion

                #region RulesAndRegulationFile

                cmd.Parameters.AddWithValue("@RulesAndRegulationFile", "");

                #endregion

                #region BalanceSheetPDF

                cmd.Parameters.AddWithValue("@BalanceSheetExcelFile", "");
                #endregion

                #region ProceedingsGeneralMeetingPDF

                cmd.Parameters.AddWithValue("@GeneralMeetingProcedureFile", "");
                #endregion

                #region WorkPlanShowingEconomicPDF

                cmd.Parameters.AddWithValue("@WorkPlanEcoViabilityFile", "");

                #endregion

                #region MemberListExcelFile

                cmd.Parameters.AddWithValue("@MemberListExcelFile", "");

                #endregion

                #region ObjectsFileName

                cmd.Parameters.AddWithValue("@ObjectsFile", "");

                #endregion

                cmd.Parameters.AddWithValue("@BusinessId", _Model.BusinessId);
                cmd.Parameters.AddWithValue("@LiablityId", _Model.LiablityId);
                cmd.Parameters.AddWithValue("@TotalNumberOfMembers", _Model.TotalMembersApplyingForSocociety);
                cmd.Parameters.AddWithValue("@NoOfMemberInAGM", _Model.TotalNumberOfMembersInAGM);
                cmd.Parameters.AddWithValue("@EntranceFeesCollected", _Model.EntranceFeesCollected);
                cmd.Parameters.AddWithValue("@PaidUpShareCapital", _Model.PaidUpShareCapital);
                cmd.Parameters.AddWithValue("@MemberLiablity", _Model.MemberLiablity);
                cmd.Parameters.AddWithValue("@SingleShareValue", _Model.SingleShareValue);
                cmd.Parameters.AddWithValue("@IsNewRegistration", _Model.IsNewRegistration);
                cmd.Parameters.AddWithValue("@BRN", _Model.BRN == null ? "" : _Model.BRN);
                cmd.Parameters.AddWithValue("@RegistrationDate", _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                cmd.Parameters.AddWithValue("@RegistrationNo", (_Model.RegistrationNo == "undefined" || _Model.RegistrationNo == "" || _Model.RegistrationNo == null) ? "" : _Model.RegistrationNo.ToUpper().Trim().Replace(" ", ""));
                cmd.Parameters.AddWithValue("@RegistrationNoOld", (_Model.OldRegistrationNo == "undefined" || _Model.OldRegistrationNo == "" || _Model.OldRegistrationNo == null) ? "" : _Model.OldRegistrationNo.ToUpper().Trim().Replace(" ", ""));

                cmd.Parameters.AddWithValue("@SSOID", "");
                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                cmd.Parameters.AddWithValue("@NoOfBranches", _Model.NoOfBranches);
                cmd.Parameters.AddWithValue("@ElectionDate", _Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.ElectionDate).ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.ElectionDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                cmd.Parameters.AddWithValue("@KIOSKCODE", _Model.KIOSKCODE);

                cmd.Parameters.AddWithValue("@IsRSSC", _Model.IsStatLevel);
                cmd.Parameters.AddWithValue("@AgmDate", _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());//_Model.IsNewRegistration == true ? DateTime.MaxValue : (_Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate().ToString().Contains("0001") ? DateTime.MaxValue : _Model.AgmDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate()));
                cmd.Parameters.AddWithValue("@SportsAssociationId", _Model.AssociationId);

                cmd.Parameters.AddWithValue("@IsAffiliatedByRSSC", _Model.IsAffiliatedByRSSC);
                cmd.Parameters.AddWithValue("@RSSCRegNo", _Model.RSSCRegNo == "undefined" ? "" : _Model.RSSCRegNo);
                cmd.Parameters.AddWithValue("@RSSCAffiliatedDate", _Model.IsAffiliatedByRSSC == false ? DateTime.MaxValue : _Model.RSSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region RSSCFileName
                if (_Model.RSSCFileName != null)
                {
                    if (_Model.RSSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.RSSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.RSSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.RSSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    cmd.Parameters.AddWithValue("@RSSCFileName", _Model.RSSCFileName == "undefined" ? "" : _Model.RSSCFileName);
                }
                else
                    cmd.Parameters.AddWithValue("@RSSCFileName", "");
                #endregion

                cmd.Parameters.AddWithValue("@IsAffiliatedBySOC", _Model.IsAffiliatedBySOC);
                cmd.Parameters.AddWithValue("@SOCRegNo", _Model.SOCRegNo == "undefined" ? "" : _Model.SOCRegNo);
                cmd.Parameters.AddWithValue("@SOCAffiliatedDate", _Model.IsAffiliatedBySOC == false ? DateTime.MaxValue : _Model.SOCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                #region SOCFileName
                if (_Model.SOCFileName != null)
                {
                    if (_Model.SOCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SOCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SOCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SOCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    cmd.Parameters.AddWithValue("@SOCFileName", _Model.SOCFileName == "undefined" ? "" : _Model.SOCFileName);
                }
                else
                    cmd.Parameters.AddWithValue("@SOCFileName", "");
                #endregion


                cmd.Parameters.AddWithValue("@IsAffiliatedByDLSC", _Model.IsAffiliatedByDLSC);
                cmd.Parameters.AddWithValue("@DLSCRegNo", _Model.DLSCRegNo);
                cmd.Parameters.AddWithValue("@DLSCAffiliatedDate", _Model.IsAffiliatedByDLSC == false ? DateTime.MaxValue : _Model.DLSCAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region DLSCFileName
                if (_Model.DLSCFileName != null)
                {
                    if (_Model.DLSCFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.DLSCFileName.Contains("undefined"))
                    {
                        var Arr = _Model.DLSCFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.DLSCFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    cmd.Parameters.AddWithValue("@DLSCFileName", _Model.DLSCFileName == "undefined" ? "" : _Model.DLSCFileName);
                }
                else
                    cmd.Parameters.AddWithValue("@DLSCFileName", "");
                #endregion

                cmd.Parameters.AddWithValue("@IsAffiliationBySLSA", _Model.IsAffiliationBySLSA);
                cmd.Parameters.AddWithValue("@SLSARegNo", _Model.SLSARegNo);
                cmd.Parameters.AddWithValue("@SLSAAffiliatedDate", _Model.IsAffiliationBySLSA == false ? DateTime.MaxValue : _Model.SLSAAffiliatedDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());

                #region SLSAFileName
                if (_Model.SLSAFileName != null)
                {
                    if (_Model.SLSAFileName.ToString().Contains("/") && _Model.SocietyIDEnc.Length > 15 && !_Model.SLSAFileName.Contains("undefined"))
                    {
                        var Arr = _Model.SLSAFileName.ToString().Split('/');
                        if (Arr.Length > 1)
                        {
                            _Model.SLSAFileName = Arr[Arr.Length - 1].ToString();
                        }
                    }
                    cmd.Parameters.AddWithValue("@SLSAFileName", _Model.SLSAFileName == "undefined" ? "" : _Model.SLSAFileName);
                }
                else
                    cmd.Parameters.AddWithValue("@SLSAFileName", "");
                #endregion



                if (_Model.KIOSKCODE.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@ServicesId", _Model.ServicesId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ServicesId", 5530);
                }

                Result = Convert.ToString(cmd.ExecuteScalar());

                if (Result.IndexOf("-") > -1)
                {
                    transaction.Rollback();
                    return Result;
                }


                var RES = Result.Split('@');



                int R = 0;
                if (RES.Length > 0)
                {
                    SocietyIdEnc = Result.Split('@')[0];

                    //R = int.Parse(Result.Split('@')[1]);
                }
                _Model.SocietyRegisteredOfficeDetailsLst = _Model.SocietyRegisteredOfficeDetailsLst ?? new List<SocietyRegisteredOfficeDetailsEntity>();
                cmd = new SqlCommand();
                cmd.CommandText = "InsertElectionMaster";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                string ElectionStartDate = "";
                string ElectionEndDate = "";
                if (!string.IsNullOrWhiteSpace(_Model.ElectionStartDate))
                {
                    _Model.ElectionStartDate = _Model.ElectionStartDate.Replace("-", "/");
                    ElectionStartDate = _Model.ElectionStartDate.Split('/')[2] + "/" + _Model.ElectionStartDate.Split('/')[1] + "/" + _Model.ElectionStartDate.Split('/')[0];
                }

                if (!string.IsNullOrWhiteSpace(_Model.ElectionEndDate))
                {
                    _Model.ElectionEndDate = _Model.ElectionEndDate.Replace("-", "/");
                    ElectionEndDate = _Model.ElectionEndDate.Split('/')[2] + "/" + _Model.ElectionEndDate.Split('/')[1] + "/" + _Model.ElectionEndDate.Split('/')[0];
                }

                //SqlCommand cmdAdministrative = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@ElectionMasterId", string.IsNullOrWhiteSpace(_Model.ElectionMasterId) ? "0" : _Model.ElectionMasterId);
                cmd.Parameters.AddWithValue("@ElectionStartDate", ElectionStartDate);
                cmd.Parameters.AddWithValue("@ElectionEndDate", ElectionEndDate);
                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@UpdatedBy", _LoggedInUser.UserId);
                cmd.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                cmd.Parameters.AddWithValue("@UpdatedIP", COMMON_FUNCTION.GetIpAddress());
                cmd.Parameters.AddWithValue("@DIVISION_ID", _Model.SocietyRegisteredOfficeDetailsLst[0].DIVISION_ID);
                cmd.Parameters.AddWithValue("@DistrictId", _Model.SocietyRegisteredOfficeDetailsLst[0].DISTRICT_ID);
                cmd.Parameters.AddWithValue("@SocietyTypeId", _Model.SocietyTypeId);
                cmd.Parameters.AddWithValue("@SocietyIdEnc", SocietyIdEnc);
                cmd.Parameters.AddWithValue("@ElectionResultFile", SocietyIdEnc);
                string ElectionMasterId = "0";
                ElectionMasterId = Convert.ToString(cmd.ExecuteScalar());

                _Model.AdministrativeDataList = _Model.AdministrativeDataList ?? new List<AdministrativeData>();


                cmd = new SqlCommand();
                cmd.CommandText = "UpdateSocietyMemberType";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@SocietyId", SocietyIdEnc);
                var rest = Convert.ToString(cmd.ExecuteScalar());



                foreach (AdministrativeData obj in _Model.AdministrativeDataList)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    //if (obj.Name.Length > 0 && obj.CategoryId > 0 )
                    //{
                    obj.DOB = obj.DOB == null ? "" : obj.DOB;
                    var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                    var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                    //objDBAccess.OpenConnection();

                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertSocietyElectionAdministrative";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;

                    //SqlCommand cmdAdministrative = objDBAccess.CreateCommand("InsertSocietyAdministrativeDetails_Edit_Request_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                    cmd.Parameters.AddWithValue("@EMitraTransactionId", R);
                    cmd.Parameters.AddWithValue("@AdministrativeId", obj.AdministrativeId);
                    cmd.Parameters.AddWithValue("@SocietyId", SocietyIdEnc);
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@FatherName", obj.FatherName);
                    cmd.Parameters.AddWithValue("@ContactNo", obj.ContactNo ?? "");
                    cmd.Parameters.AddWithValue("@UserType", obj.UserType ?? "");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                    cmd.Parameters.AddWithValue("@UpdatedBy", _LoggedInUser.UserId);
                    cmd.Parameters.AddWithValue("@AppointedDate", DOJConvert);
                    cmd.Parameters.AddWithValue("@IsActive", obj.IsActive);
                    cmd.Parameters.AddWithValue("@ClassA", (_Model.ClassA == null || _Model.ClassA.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassA));
                    cmd.Parameters.AddWithValue("@ClassB", (_Model.ClassB == null || _Model.ClassB.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassB));
                    cmd.Parameters.AddWithValue("@ClassC", (_Model.ClassC == null || _Model.ClassC.ToString() == "undefined") ? 0 : int.Parse(_Model.ClassC));
                    cmd.Parameters.AddWithValue("@flag", i);
                    cmd.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                    cmd.Parameters.AddWithValue("@DOB", DOBConvert);
                    cmd.Parameters.AddWithValue("@AADHARENO", (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                    cmd.Parameters.AddWithValue("@GENDER", obj.Gender ?? "");
                    cmd.Parameters.AddWithValue("@Photo", obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                    cmd.Parameters.AddWithValue("@AdministratorDesignationId", obj.AdministratorDesignationId);
                    cmd.Parameters.AddWithValue("@PostId", obj.PostId);
                    cmd.Parameters.AddWithValue("@CategoryId", obj.CategoryId);
                    cmd.Parameters.AddWithValue("@Address", obj.Address ?? "");
                    cmd.Parameters.AddWithValue("@SSOID", _Model.SSOID);
                    cmd.Parameters.AddWithValue("@KIOSKCODE", _Model.KIOSKCODE);
                    cmd.Parameters.AddWithValue("@MEMBERTYPE", obj.MEMBERTYPE);
                    cmd.Parameters.AddWithValue("@POSTNAME", obj.PostName);
                    cmd.Parameters.AddWithValue("@Occupation", obj.Occupation);
                    cmd.Parameters.AddWithValue("@NominatedBy", obj.NominatedBy);
                    cmd.Parameters.AddWithValue("@BoardType", obj.BoardType);
                    cmd.Parameters.AddWithValue("@MembershipNo", obj.MembershipNo);
                    cmd.Parameters.AddWithValue("@ShareAllotted", obj.ShareAllotted);
                    cmd.Parameters.AddWithValue("@ShareCapitalAmount", obj.ShareCapitalAmount);
                    cmd.Parameters.AddWithValue("@loaneeType", obj.loaneeType);
                    cmd.Parameters.AddWithValue("@SubmitStatus", obj.SubmitStatus);
                    cmd.Parameters.AddWithValue("@UserTypeCK", _Model.UserType);
                    cmd.Parameters.AddWithValue("@ElectionMasterId", ElectionMasterId);                 
                    cmd.Parameters.AddWithValue("@ServicesId", 0);
                    Result = Convert.ToString(cmd.ExecuteScalar());

                    if (Result.IndexOf("-") > -1)
                    {
                        Result = "-1";
                        transaction.Rollback();
                        return Result;
                    }


                    if ((Result.Split('@').Length > 1))
                        R = int.Parse(Result.Split('@')[1]);

                    //}
                    i++;
                }

                //ADDRESS
                string AddressResult = "-1";
                int EMitraTransactionId = 0;
                int SubmitStatus = _Model.SocietyRegisteredOfficeDetailsLst[0].SubmitStatus;

                foreach (SocietyRegisteredOfficeDetailsEntity obj in _Model.SocietyRegisteredOfficeDetailsLst)
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "InsertSocietyRegOfficeDetail_Election";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;

                    cmd.Parameters.AddWithValue("@SocietyRegOfficeDetailId", obj.SocietyRegOfficeDetailId);
                    cmd.Parameters.AddWithValue("@SocietyIdEnc", SocietyIdEnc);
                    cmd.Parameters.AddWithValue("@IsRural", obj.IsRural);
                    cmd.Parameters.AddWithValue("@HouseNo", obj.HouseBuildingNo);
                    cmd.Parameters.AddWithValue("@LaneStreetName", obj.LaneStreetName);
                    cmd.Parameters.AddWithValue("@Locality", obj.Locality);
                    cmd.Parameters.AddWithValue("@STATE_ID", obj.STATE_ID);
                    cmd.Parameters.AddWithValue("@DIVISION_ID", obj.DIVISION_ID);
                    cmd.Parameters.AddWithValue("@DISTRICT_ID", obj.DISTRICT_ID);
                    cmd.Parameters.AddWithValue("@CityTypes", obj.CityTypes);
                    cmd.Parameters.AddWithValue("@CITY_ID", obj.CITY_ID);
                    cmd.Parameters.AddWithValue("@BLOCK_ID", obj.BLOCK_ID);
                    cmd.Parameters.AddWithValue("@WARD_ID", obj.WARD_ID);
                    cmd.Parameters.AddWithValue("@GP_ID", obj.GP_ID);
                    cmd.Parameters.AddWithValue("@VILLAGE_ID", obj.VILLAGE_ID);
                    cmd.Parameters.AddWithValue("@PinCode", obj.PinCode);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserId);
                    cmd.Parameters.AddWithValue("@UpdatedBy", _LoggedInUser.UserId);
                    cmd.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                    cmd.Parameters.AddWithValue("@UpdatedIP", COMMON_FUNCTION.GetIpAddress());
                    cmd.Parameters.AddWithValue("@IsSameCEOAddress", obj.IsSameCEOAddress);
                    cmd.Parameters.AddWithValue("@AddressFor", obj.AddressFor);
                    cmd.Parameters.AddWithValue("@EMitraTransactionId", R);
                    cmd.Parameters.AddWithValue("@KIOSKCODE", obj.KIOSKCODE);
                    cmd.Parameters.AddWithValue("@SSOID", obj.SSO_ID ?? "");
                    cmd.Parameters.AddWithValue("@SubmitStatus", obj.SubmitStatus);
                    cmd.Parameters.AddWithValue("@UserType", obj.UserType ?? "");
                    //if (obj.KIOSKCODE.Length > 0)
                    //{
                    //   objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, obj.ServiceId);
                    //}
                    //else
                    //{
                    //   if (obj.SocietyIdEnc.Length > 0)
                    cmd.Parameters.AddWithValue("@ServicesId", 0);// Update Profile
                                                                  //}
                    Result = Convert.ToString(cmd.ExecuteScalar());
                    if ((Result.Split('@').Length > 1))
                        EMitraTransactionId = int.Parse(Result.Split('@')[1]);
                }

                //END
                if (Result != "-1" && Result != "0")
                {
                    Result += "#" + ElectionMasterId;
                    //Result = "1";
                    transaction.Commit();
                }
                else
                {
                    Result = "0";
                    transaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                Result = "0";
                transaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }


        public string GenerateElectionResultLetter(SocietyRegistrationEntity _Model, string Transid = "")
        {

            string result = "-1";
            try
            {
                string fileNameCerti = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";

                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:90px;' src='" + HttpContext.Current.Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:30px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:25px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग, कार्यालय निर्वाचन अधिकारी") + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                //sb.Append("<br/>");
                sb.Append("<label style='font-size:20px;'> " + UnicodeToKrutidev.GetKrutidev(" " + _Model.SocietyName + " प्राथमिक दुग्ध उत्पादन सहकारी समिति लि.") + " </label>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td ><br/>");

                sb.Append("<table style='width:100%'cellpadding='0'cellspacing='0' >");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;font-size:13px;'> पं.स.     : " + _Model.RegistrationNo + "</td>");
                sb.Append("<td style='width:49%;text-align:right;font-size:13px;'> दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + "    </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;font-size:13px;'> कोड न.     : " + _Model.SocietyCode + "  </td>");
                sb.Append("<td style='width:49%;text-align:right;'>   </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;font-size:13px;'> नाम निर्वाचन अधिकारी : " + _LoggedInUser.OfficerNameEn + "  </td>");
                sb.Append("<td style='width:49%;text-align:right;'>   </td>");
                sb.Append("</tr>");
                sb.Append("</table><br/>");

                sb.Append("<p style='text-indent: 60px;'> राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण जयपुर के कार्यक्रमानुसार समिति का निर्वाचन दिनांक " + _Model.ElectionStartDate + " से " + _Model.ElectionEndDate + " तक संपन्न किया, प्रक्रिया में  निर्वाचित संचालन मंडल एवं पदाधिकारियों का विवरण निम्न प्रकार है: - </ p ><br/>");
                sb.Append("<table cellspacing='0' cellpadding='5'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse; text-align:center;'>");

                sb.Append("<tr>");
                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc; padding:10px;'>  क्र. </th>");
                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>  नाम पदाधिकारी </th>");
                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>  पद </th>");
                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>  मोबईल नम्बर </th>");
                //sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>  आधार नम्बर </th>");
                sb.Append("</tr>");
                int i = 1;
                foreach (var item in _Model.AdministrativeDataList)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;' >  " + i + " </td>");
                    sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>  " + item.Name + "</td>");
                    sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>  " + item.PostName + "  </td>");
                    sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'> " + item.ContactNo + "  </td>");
                    //  sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'> " + (string.IsNullOrWhiteSpace(item.AADHAARNo) || item.AADHAARNo.Length < 12 ? "" : "XXXXXXXXXX" + item.AADHAARNo.Substring(8, 4)) + "  </td>");
                    sb.Append("</tr>");
                    i++;
                }
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/ElectionOrder")))
                {
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("../Content/ElectionOrder"));
                }
                string filepath = System.Web.HttpContext.Current.Server.MapPath("../Content/ElectionOrder/") + fileNameCerti;

                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                {   //esign code

                    if (COMMON_FUNCTION.EsinghPDF(Transid, filepath, _LoggedInUser.DISTRICT_ENG, "Election Officer").status == "1")
                    {
                        result =  fileNameCerti;
                    }
                };
            }
            catch (Exception ex)
            {
                result = "-1";
            }
            return result;
        }

        public string InsertSocietyProfileNew(SocietyRegistrationEntity _Model, string Transid)
        {
            string Result = "-1";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {

                string ElectionMasterId = "0";

                _Model.SocietyIDEnc = _Model.SocietyIDEnc ?? "";
                _Model.SocietyIDEnc = _Model.SocietyIDEnc.Length < 15 ? "" : _Model.SocietyIDEnc;
                string electionResultPDF = "";

                string pdfRewsult = GenerateElectionResultLetter(_Model, Transid);
                if (pdfRewsult != "-1")
                {
                    electionResultPDF = pdfRewsult;




                    objDBAccess.OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd = objDBAccess.CreateCommand("InsertElectionMaster", DBFactoryCommandType.PROCEDURE_COMMAND);


                    string ElectionStartDate = "";
                    string ElectionEndDate = "";
                    if (!string.IsNullOrWhiteSpace(_Model.ElectionStartDate))
                    {
                        _Model.ElectionStartDate = _Model.ElectionStartDate.Replace("-", "/");
                        ElectionStartDate = _Model.ElectionStartDate.Split('/')[2] + "/" + _Model.ElectionStartDate.Split('/')[1] + "/" + _Model.ElectionStartDate.Split('/')[0];
                    }

                    if (!string.IsNullOrWhiteSpace(_Model.ElectionEndDate))
                    {
                        _Model.ElectionEndDate = _Model.ElectionEndDate.Replace("-", "/");
                        ElectionEndDate = _Model.ElectionEndDate.Split('/')[2] + "/" + _Model.ElectionEndDate.Split('/')[1] + "/" + _Model.ElectionEndDate.Split('/')[0];
                    }

                    objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(_Model.ElectionMasterId) ? "0" : _Model.ElectionMasterId);
                    objDBAccess.AddParameterToCommand("@ElectionStartDate", DBFactoryParameterType.NVarchar, ElectionStartDate);
                    objDBAccess.AddParameterToCommand("@ElectionEndDate", DBFactoryParameterType.NVarchar, ElectionEndDate);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.NVarchar, _Model.SocietyRegisteredOfficeDetailsLst[0].DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _Model.SocietyRegisteredOfficeDetailsLst[0].DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, _Model.SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@ElectionResultFile", DBFactoryParameterType.NVarchar, electionResultPDF);

                    Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());



                }
                else
                {
                    return "-1";
                }




            }
            catch (Exception ex)
            {
                Result = "-1";
                //transaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;


        }



        public string SetAssignElectionOfficerRole(int UserId)
        {
            string result = "-1";
            //DataTable dt = null;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AssignElectionOfficerRole", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                result = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "UpdateUserRoleAppr(int MappingID, int IsApproved, int RoleID,string ActId, string PrimaryResionId , string DairyResionId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                //dt.Dispose();
            }
            return result;
        }
        public string AddElectionOfficer(UserEntity obj, List<RoleArray> objRoleListData)
        {
            string Result = "-1";
            int ResultFinal = -1;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                obj.ActId = obj.ActId != null ? obj.ActId : "0";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AddElectionUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, obj.UserId);
                objDBAccess.AddParameterToCommand("@UserTitle", DBFactoryParameterType.Int, obj.UserTitle);
                objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, obj.OfficerNameEn);
                objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, obj.OfficerNameHi);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, obj.Gender);
                objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, DateTime.Now);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, obj.PhoneNo);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, obj.EmailId);
                objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, obj.ImageUrl);
                //objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, Encryption.Encrypt(obj.Password, EncryptionKey.PasswordHash));
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, 6);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, obj.DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Bit, obj.IsRuralUrban);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, obj.TehsilId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, obj.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, obj.WardId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, obj.GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, obj.VillageId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.AddressEn);
                objDBAccess.AddParameterToCommand("@AddressHi", DBFactoryParameterType.NVarchar, obj.AddressHi);
                objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.NVarchar, obj.PIN);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSOID);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, obj.ResionId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, obj.ActId.Length > 1 ? obj.ActId.Substring(0, obj.ActId.Length - 1) : "0");
                //Result = objDBAccess.ExecuteScalarCommand());
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();


            }
            catch (Exception ex)
            {
                Result = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Add(UserEntity obj,List<RoleArray> objRoleListData)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion
    }
}