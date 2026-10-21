using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooperative.Models
{
    public class SmsformatModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        [Key]
        public int TemplateId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Heading English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string TemplateHeadingEN { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Heading Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string TemplateHeadingHI { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Body English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TemplateBodyEN { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Body Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TemplateBodyHI { get; set; }

        public string TemplateType { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }

        public SmsformatModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public SmsformatModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddSmsFormat()
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSmsTemplate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TemplateId", DBFactoryParameterType.BigInt, TemplateId);
                objDBAccess.AddParameterToCommand("@TemplateHeadingEN", DBFactoryParameterType.NVarchar, TemplateHeadingEN);
                objDBAccess.AddParameterToCommand("@TemplateHeadingHI", DBFactoryParameterType.NVarchar, TemplateHeadingHI);
                objDBAccess.AddParameterToCommand("@TemplateBodyEN", DBFactoryParameterType.NVarchar, TemplateBodyEN);
                objDBAccess.AddParameterToCommand("@TemplateBodyHI", DBFactoryParameterType.NVarchar, TemplateBodyHI);
                objDBAccess.AddParameterToCommand("@TemplateType", DBFactoryParameterType.NVarchar, "S");
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "AddSmsFormat()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddMailFormat()
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSmsTemplate", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@TemplateId", DBFactoryParameterType.BigInt, TemplateId);



                objDBAccess.AddParameterToCommand("@TemplateHeadingEN", DBFactoryParameterType.NVarchar, TemplateHeadingEN);
                objDBAccess.AddParameterToCommand("@TemplateHeadingHI", DBFactoryParameterType.NVarchar, TemplateHeadingHI);
                objDBAccess.AddParameterToCommand("@TemplateBodyEN", DBFactoryParameterType.NVarchar, TemplateBodyEN);
                objDBAccess.AddParameterToCommand("@TemplateBodyHI", DBFactoryParameterType.NVarchar, TemplateBodyHI);
                objDBAccess.AddParameterToCommand("@TemplateType", DBFactoryParameterType.NVarchar, "M");
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "AddMailFormat()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int ChangeStatusSms(int TemplateId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SmsDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TemplateId", DBFactoryParameterType.Int, TemplateId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "ChangeStatusSms(int TemplateId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        //public int DeleteTemplate(int TemplateId)
        //{
        //    int Result = 0;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("SmsDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@TemplateId", DBFactoryParameterType.BigInt, TemplateId);

        //        Result = objDBAccess.ExecuteCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "DeleteTemplate()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}

        public IList<SmsformatModel> GetAllSmsFormat(SmsformatModel obj = null)
        {
            IList<SmsformatModel> SmsFormatEntitylist = new List<SmsformatModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SmsTemplateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (obj != null)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
                };
                DataTable dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;

                    foreach (DataRow Dr in dt.Rows)
                    {
                        SmsFormatEntitylist.Add(new SmsformatModel() { TemplateId = int.Parse(Dr["TemplateId"].ToString()), TemplateHeadingEN = Dr["TemplateHeadingEN"].ToString(), TemplateHeadingHI = Dr["TemplateHeadingHI"].ToString(), TemplateBodyEN = Dr["TemplateBodyEN"].ToString(), TemplateBodyHI = Dr["TemplateBodyHI"].ToString(), IsActive = Convert.ToInt32(Dr["IsActive"].ToString()), CreateDate = Convert.ToDateTime(Dr["Createdate"].ToString()) });
                    }

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "GetAllSmsFormat(SmsformatModel obj = null)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return SmsFormatEntitylist;
        }

        public IList<SmsformatModel> GetAllMailFormat(SmsformatModel obj = null)
        {
            IList<SmsformatModel> SmsFormatEntitylist = new List<SmsformatModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MailTemplateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (obj != null)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
                };
                DataTable dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;

                    foreach (DataRow Dr in dt.Rows)
                    {
                        SmsFormatEntitylist.Add(new SmsformatModel() { TemplateId = int.Parse(Dr["TemplateId"].ToString()), TemplateHeadingEN = Dr["TemplateHeadingEN"].ToString(), TemplateHeadingHI = Dr["TemplateHeadingHI"].ToString(), TemplateBodyEN = Dr["TemplateBodyEN"].ToString(), TemplateBodyHI = Dr["TemplateBodyHI"].ToString(), IsActive = Convert.ToInt32(Dr["IsActive"].ToString()), CreateDate = Convert.ToDateTime(Dr["Createdate"].ToString()) });
                    }

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "GetAllMailFormat(SmsformatModel obj = null)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return SmsFormatEntitylist;
        }
    }

}