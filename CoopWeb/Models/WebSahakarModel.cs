using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI;
using System.Web;
using Cooperative.Controllers;
using System.Web.Mvc;

namespace Cooperative.Models
{
    public class WebSahakarModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public long PageId { get; set; }
        public string PageName { get; set; }
        [DataType(DataType.Html)]
        public string PageDescEn { get; set; }
        [DataType(DataType.Html)]
        public string PageDescHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public string UserIds { get; set; }
        public List<TrainningUser> lstTrainningUsers { get; set; }
        public string Image { get; set; }
        public class TrainningUser
        {
            //public int TrainningId { get; set; }
            public int UserId { get; set; }

        }

        public WebSahakarModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public WebSahakarModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        #region DynamicPageContent

        public int AddPageContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterPageContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageId", DBFactoryParameterType.Int, PageId);
                objDBAccess.AddParameterToCommand("@PageDescEn", DBFactoryParameterType.NVarchar, PageDescEn);
                objDBAccess.AddParameterToCommand("@PageDescHi", DBFactoryParameterType.NVarchar, PageDescHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@Image", DBFactoryParameterType.NVarchar, Image);
                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddPageContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllPageContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPageContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllPageContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusPageContent(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsPageContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageId", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusPageContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion DynamicPageContent

        #region TelePhone
        public long TeleDirectoryID { get; set; }
        public long TeleCatID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string STDCODE { get; set; }
        public string OFFICE { get; set; }
        public string RESI { get; set; }
        public string FAX { get; set; }
        public string EMAILID { get; set; }

        public DataTable GetTelePhoneCat()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetTeleDirectoryCat", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "GetAllPageContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetTelePhoneListByCAT(int catid)
        {
            dt.Clear();
            try
            {
                string WhereClause = "Where TeleCatID=" + catid + " and MasterTeleDirectory.IsActive IN (1,0)";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetTeleDirectoryByCatID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "GetAllPageContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int AddTelePhoneDirectoryContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterTeleDirectory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TeleDirectoryID", DBFactoryParameterType.Int, TeleDirectoryID);
                objDBAccess.AddParameterToCommand("@TeleCatID", DBFactoryParameterType.Int, TeleCatID);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                objDBAccess.AddParameterToCommand("@Designation", DBFactoryParameterType.NVarchar, Designation);
                objDBAccess.AddParameterToCommand("@Mobile", DBFactoryParameterType.NVarchar, Mobile);
                objDBAccess.AddParameterToCommand("@STDCODE", DBFactoryParameterType.NVarchar, STDCODE);
                objDBAccess.AddParameterToCommand("@OFFICE", DBFactoryParameterType.NVarchar, OFFICE);
                objDBAccess.AddParameterToCommand("@RESI", DBFactoryParameterType.NVarchar, RESI);
                objDBAccess.AddParameterToCommand("@FAX", DBFactoryParameterType.NVarchar, FAX);
                objDBAccess.AddParameterToCommand("@EMAILID", DBFactoryParameterType.NVarchar, EMAILID);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "AddPageContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteandstatusTelePhone(int ClassId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteandstatusTelePhone", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TeleDirectoryID", DBFactoryParameterType.Int, ClassId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "DeleteAuditreport(int ClassId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion
    }

    public class NewsEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public NewsEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public NewsEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int NewsID { get; set; }
        public string NewsNameEN { get; set; }
        [AllowHtml]
        public string NewsDescriptionEN { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime NewsDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string NewsNameHI { get; set; }
        [AllowHtml]
        public string NewsDescriptionHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public int IsNew { get; set; }
        public string WhereClause { get; set; }

        public string DocumentDisplayAs { get; set; }
        public string DocumentPath { get; set; }

        public List<NewsEntity> FileList { get; set; }

        #region NewsContent

        public int AddNewsContent()
        {
            int Result = 0;
            try
            {

                NewsEntity _Model = new NewsEntity();
                string files = "";
                if (FileList != null)
                {
                    string fileName = "";

                    for (int i = 0; i < FileList.Count; i++)
                    {

                        fileName += FileList[i].FileName + ",";
                    }

                     files = fileName.Remove(fileName.Length - 1, 1);

                }

                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterNews", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@NewsID", DBFactoryParameterType.Int, NewsID);
                objDBAccess.AddParameterToCommand("@NewsNameEN", DBFactoryParameterType.NVarchar, NewsNameEN);
                objDBAccess.AddParameterToCommand("@NewsNameHI", DBFactoryParameterType.NVarchar, NewsNameHI);
                objDBAccess.AddParameterToCommand("@NewsDescriptionEN", DBFactoryParameterType.NVarchar, NewsDescriptionEN);
                objDBAccess.AddParameterToCommand("@NewsDescriptionHI", DBFactoryParameterType.NVarchar, NewsDescriptionHI);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, StartDate);
                objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, StartTime);
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, EndDate);
                objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, EndTime);
                objDBAccess.AddParameterToCommand("@IsNew", DBFactoryParameterType.Bit, IsNew);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@NewsDate", DBFactoryParameterType.DateTime, NewsDate);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, files);

                objDBAccess.AddParameterToCommand("@MainImage", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@OtherImage1", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@OtherImage2", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@OtherImage3", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, 0);
                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllNewsContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " where 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusNewsContent(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsNewsContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@NewsID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion NewsContent

    }

    public class CoopertativeInstitutionEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public CoopertativeInstitutionEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public CoopertativeInstitutionEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int CoopInstitutionId { get; set; }
        public string TitleNameEn { get; set; }
        public string TitleNameHi { get; set; }
        public string Link { get; set; }
        public string ImagePath { get; set; }
        public string TitleDescriptionEn { get; set; }
        public string TitleDescriptionHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region CoopertativeInstitution

        public int AddCoopertativeInstitutionContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterCooperativeInstitution", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CoopInstitutionId", DBFactoryParameterType.Int, CoopInstitutionId);
                objDBAccess.AddParameterToCommand("@TitleNameEn", DBFactoryParameterType.NVarchar, TitleNameEn);
                objDBAccess.AddParameterToCommand("@TitleNameHi", DBFactoryParameterType.NVarchar, TitleNameHi);
                objDBAccess.AddParameterToCommand("@ImagePath", DBFactoryParameterType.NVarchar, ImagePath);
                objDBAccess.AddParameterToCommand("@Link", DBFactoryParameterType.NVarchar, Link);
                objDBAccess.AddParameterToCommand("@TitleDescriptionEn", DBFactoryParameterType.NVarchar, TitleDescriptionEn);
                objDBAccess.AddParameterToCommand("@TitleDescriptionHi", DBFactoryParameterType.NVarchar, TitleDescriptionHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllCoopertativeInstitutionContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCooperativeInstitutionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusCoopertativeInstitutionContent(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsCoopertativeInstitutionContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CoopInstitutionId", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion CoopertativeInstitution

    }


    public class OrderCircularsDetails
    {
        public List<OrderSSOMAPEntity> lstOrderSSOMAPScope { get; set; }
        public int UserID { get; set; }

    }

    #region Model :  OrderSSOMAP scope Details
    public class OrderSSOMAPEntity
    {
        public string SSOId { get; set; }
        public int Catid { get; set; }

    }

    #endregion OrderSSOMAP

    public class OrderCircularsEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public OrderCircularsEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public OrderCircularsEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int OrderCircularsID { get; set; }
        public int OrderCatID { get; set; }
        public string CatName { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region OrderCirculars

        public int AddOrderCircularsContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterOrderCirculars", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderCircularsID", DBFactoryParameterType.Int, OrderCircularsID);
                objDBAccess.AddParameterToCommand("@OrderCatID", DBFactoryParameterType.Int, OrderCatID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddOrderCircularsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllOrderCircularsCatListContent(string ssoid)
        {
            dt.Clear();
            try
            {
                WhereClause = " AND MasterOrderCatSSO.ssoid='" + ssoid + "'";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOrderCircularsCatListBySSOID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllOrderCircularsCatListContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllOrderCircularsContent(string catid)
        {
            dt.Clear();
            try
            {
                WhereClause = "and OrderCatID=" + catid + "";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOrderCircularsListByCatIdNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllOrderCircularsContent(string catid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllOrderCircularsSSOContent(string catid)
        {
            dt.Clear();
            try
            {
                WhereClause = "and catid=" + catid + "";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOrderCircularsSSOListByCatId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteSSOOrderCirculars(int PageId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSSOOrderCirculars", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ordercatmapid", DBFactoryParameterType.BigInt, PageId);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusOrderCirculars(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteChangeStatusOrderCirculars(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsOrderCirculars", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderCircularsID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusOrderCirculars(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddSSOMAPDetails(OrderCircularsDetails Obj)
        {
            int Result = 0;
            try
            {
                foreach (OrderSSOMAPEntity objapp in Obj.lstOrderSSOMAPScope)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterOrderCircularsSSOMap", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@OrderaCatMapID", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@CatID", DBFactoryParameterType.Int, objapp.Catid);
                    objDBAccess.AddParameterToCommand("@ssoid", DBFactoryParameterType.NVarchar, objapp.SSOId);
                    objDBAccess.AddParameterToCommand("@UserID", DBFactoryParameterType.Int, Obj.UserID);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddCaseDetails(CaseDetails Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion Downloads

    }

    public class FertilizerEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public FertilizerEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public FertilizerEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int FertilizerID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region Fertilizer

        public int AddFertilizerContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterFertilizer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FertilizerID", DBFactoryParameterType.Int, FertilizerID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllFertilizerContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusFertilizer(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsFertilizer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FertilizerID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion Fertilizer

    }

    public class MessageEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public MessageEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MessageEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int MessageID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region Message

        public int AddMessageContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterMessage", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MessageID", DBFactoryParameterType.Int, MessageID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllMessageContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MessageList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusMessage(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsMessage", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MessageID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion Message

    }


    public class CoopertativeEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public CoopertativeEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public CoopertativeEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int CooperativeID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region Coopertative

        public int AddCoopertativeContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterCooperatives", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CooperativeID", DBFactoryParameterType.Int, CooperativeID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllCoopertativeContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CooperativesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusCoopertative(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsCoopertative", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CooperativeID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion Coopertative

    }

    #region Transfers

    public class TransfersEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public TransfersEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public TransfersEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int TransferID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddTransfersContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterTransfer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TransferID", DBFactoryParameterType.Int, TransferID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllTransfersContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TransfersList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusTransfers(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsTransfers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TransferID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion Transfers

    #region Righttoinformation

    public class RighttoinformationEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public RighttoinformationEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public RighttoinformationEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int RTIID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddRighttoinformationContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterRTI", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RTIID", DBFactoryParameterType.Int, RTIID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllRighttoinformationContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RighttoinformationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusRighttoinformation(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsRighttoinformation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RTIID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion Righttoinformation


    #region Prizes

    public class PrizesEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public PrizesEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PrizesEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int PrizesID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddPrizesContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterPrizes", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PrizesID", DBFactoryParameterType.Int, PrizesID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllPrizesContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PrizesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusPrizes(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsPrizes", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PrizesID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion Prizes



    #region PhotoGallary

    public class PhotoGallaryEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public PhotoGallaryEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PhotoGallaryEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int PhotoGallaryID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddPhotoGallaryContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterPhotoGallary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PhotoGallaryID", DBFactoryParameterType.Int, PhotoGallaryID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllPhotoGallaryContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PhotoGallaryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusPhotoGallary(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsPhotoGallary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PhotoGallaryID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion PhotoGallary

    #region Bulletien

    public class BulletienEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public BulletienEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public BulletienEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int BulletienID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddBulletienContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterBulletien", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BulletienID", DBFactoryParameterType.Int, BulletienID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllBulletienContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BulletienList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllBulletienContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusBulletien(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsBulletien", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BulletienID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion Bulletien


    #region Lawandrules

    public class LawandrulesEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public LawandrulesEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public LawandrulesEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int LawandrulesID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddLawandrulesContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterLawandrules", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LawandrulesID", DBFactoryParameterType.Int, LawandrulesID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public DataTable GetAllLawandrulesContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LawandrulesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllLawandrulesModalList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LawandrulesModelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusLawandrules(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsPhotoGallary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PhotoGallaryID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    #endregion PhotoGallary

    public class LawandrulesModelEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public LawandrulesModelEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public LawandrulesModelEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int LawandrulesmodelID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }





        public int AddLawandrulesModelContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertLawandrulesModel", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LawandrulesmodelID", DBFactoryParameterType.Int, LawandrulesmodelID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public DataTable GetAllLawandrulesModelContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LawandrulesModelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusLawandrulesModel(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsPhotoGallary", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PhotoGallaryID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }


    public class DownloadsEntity
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public DownloadsEntity(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DownloadsEntity()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int DownloadID { get; set; }
        public string TitleEN { get; set; }
        public string TitleHI { get; set; }
        public string FileName { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }



        #region Downloads

        public int AddDownloadsContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterDownloads", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DownloadID", DBFactoryParameterType.Int, DownloadID);
                objDBAccess.AddParameterToCommand("@TitleEN", DBFactoryParameterType.NVarchar, TitleEN);
                objDBAccess.AddParameterToCommand("@TitleHI", DBFactoryParameterType.NVarchar, TitleHI);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, FileName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                //                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "AddNewsContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllDownloadsContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DownloadsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "GetAllNewsContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusDownloads(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsDownloads", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DownloadID", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "DeleteChangeStatusNewsContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion Downloads



    }

    public class PM_KISAN_SMS
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public decimal TotalApplication { get; set; }
        public decimal TotalUploded { get; set; }
        public decimal DisbursementI { get; set; }
        public decimal DisbursementII { get; set; }
        public decimal DisbursementIV { get; set; }
        public decimal DisbursementIII { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }


        public PM_KISAN_SMS(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PM_KISAN_SMS()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        #region PM_Kissan_SMS

        public int SavePMKissanSMSData()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(DateTime.Now);
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertPMKisanSmsData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TotalApplication", DBFactoryParameterType.NVarchar, TotalApplication);
                objDBAccess.AddParameterToCommand("@TotalUploded", DBFactoryParameterType.NVarchar, TotalUploded);
                objDBAccess.AddParameterToCommand("@DisbursementI", DBFactoryParameterType.NVarchar, DisbursementI);
                objDBAccess.AddParameterToCommand("@DisbursementII", DBFactoryParameterType.NVarchar, DisbursementII);
                objDBAccess.AddParameterToCommand("@DisbursementIII", DBFactoryParameterType.NVarchar, DisbursementIII);
                objDBAccess.AddParameterToCommand("@DisbursementIV", DBFactoryParameterType.NVarchar, DisbursementIV);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                if (Result > 0)
                {

                  
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "SavePMKissanSMSData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

       


        public DataTable PMKissanSMSList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " and 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PMKissanSMSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WebSahakar", FunctionName = "PMKissanDataList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        #endregion PM_Kissan_SMS
    }
}