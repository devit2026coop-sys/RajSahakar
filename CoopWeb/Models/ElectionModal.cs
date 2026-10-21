using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class ElectionModal
    {
        //private ILogger objILogger;
       
        private DataTable dt;

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

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable SocietyListForElection(ElectionSocietyData Obj)
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
                if (Obj.UserType == "C" || Obj.UserType == "K"  )
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "' ") : "";
                }
                 if (Obj.DIVISION_ID > 0)
                {
                    WhereClause += " and SROD.DIVISION_ID=" + Obj.DIVISION_ID;
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
                }


                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable SocietyListForElectionOfficer(SocietyRegistrationEntity Obj)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            try
            {
                string WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForElectionOfficer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {

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
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {

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

                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertElectionSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _Model.CreatedBy);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, _Model.UpdatedBy);
                    objDBAccess.AddParameterToCommand("@NoOfSociety", DBFactoryParameterType.NVarchar, _Model.SocityIdlst.Count);
                    objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, _Model.CreationDate);
                    objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, _Model.UpdationDate);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, _Model.UpdatedIP);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }

                foreach (SocityIds item in _Model.SocityIdlst)
                {
                    try
                    {

                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.BigInt, Result);
                        objDBAccess.AddParameterToCommand("@SocityIdEnc", DBFactoryParameterType.NVarchar, item.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _Model.UpdatedBy);
                        objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, _Model.CreationDate);
                        objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, _Model.UpdationDate);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, _Model.UpdatedIP);
                        objDBAccess.AddParameterToCommand("@IsSocietyCompleted", DBFactoryParameterType.NVarchar, 1);

                        int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                }

            }



            return Result;
        }

        public int InsertElectionSocietyDate(ElectionSocietyData _Model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            if (_Model != null && _Model.Socitydateslst != null && _Model.Socitydateslst.Count > 0)
            {
                foreach (Socitydates item in _Model.Socitydateslst)
                {
                    try
                    {
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertElectionProDates", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@ElectionMasterId", DBFactoryParameterType.Int, _Model.ElectionMasterId);
                        objDBAccess.AddParameterToCommand("@El_QuestionId", DBFactoryParameterType.Int, item.El_QuestionId);
                        objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, item.StartDate.ToString().Contains("0001") ? DateTime.MaxValue : item.StartDate);
                        objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, item.EndDate.ToString().Contains("0001") ? DateTime.MaxValue : item.EndDate);
                        objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, item.StartTime);
                        objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, item.EndTime);
                        objDBAccess.AddParameterToCommand("@OtherTime", DBFactoryParameterType.NVarchar, item.OtherTime);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }

                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                }
            }
            return Result;

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
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OrderListForElection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@IsReturingOfficer", DBFactoryParameterType.Int, IsReturingOfficer);
                objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ElectionModel", FunctionName = "DataTable GetElectionOrderList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


    }
}