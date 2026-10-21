using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Cooperative.Controllers;

namespace Cooperative.Models
{
    public class VendorRegistrationPaymentModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        private DataSet ds;
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public VendorRegistrationPaymentModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public VendorRegistrationPaymentModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        #region VendorRegistration
        public int UsersVendorRegistration(string ActionName)
        {
            int Return = 0;
            dt.Clear();
            try
            {
                string status = "";

                SqlCommand cmd = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, ActionName);
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        status = new csCommon().VendorUserRegistration(
                                    dt.Rows[i]["BeneficiaryId"].ToString(),
                                    dt.Rows[i]["name"].ToString(),
                                    dt.Rows[i]["fatherName"].ToString(),
                                    dt.Rows[i]["ifscCode"].ToString(),
                                    dt.Rows[i]["bankActNo"].ToString(),
                                    dt.Rows[i]["address"].ToString(),
                                    dt.Rows[i]["contactNo"].ToString(),
                                    dt.Rows[i]["stateSchemeCode"].ToString(),
                                    dt.Rows[i]["agencyCode"].ToString(),
                                    dt.Rows[i]["stateCode"].ToString(),
                                    dt.Rows[i]["districtCode"].ToString(),
                                    dt.Rows[i]["officeId"].ToString()
                        );
                    }
                }

                Return = 1;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.VendorRegistrationPaymentModel", FunctionName = "UsersVendorRegistration()" + ActionName }, ex);
                Return = 0;
            }
            finally
            {
                dt.Dispose();
            }

            return Return;

        }
        #endregion VendorRegistration


        #region VendorCodeVerification
        public int UsersVendorCodeVerification(string ActionName)
        {
            int Return = 0;
            dt.Clear();
            try
            {
                string status = "";

                SqlCommand cmd = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, ActionName);
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        status = new csCommon().VendorCodeVerification(
                                    dt.Rows[i]["BeneficiaryId"].ToString(),
                                    dt.Rows[i]["name"].ToString(),
                                    dt.Rows[i]["BankActNo"].ToString(),
                                    dt.Rows[i]["StateSchemeCode"].ToString(),
                                    dt.Rows[i]["AgencyCode"].ToString()
                        );
                    }
                }
                Return = 1;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.VendorRegistrationPaymentModel", FunctionName = "UsersVendorCodeVerification()" + ActionName }, ex);
                Return = 0;
            }
            finally
            {
                dt.Dispose();
            }

            return Return;

        }
        #endregion VendorCodeVerification      



        #region VendorPaymentProcess
        public int UsersVendorPaymentTransaction(string ActionName)
        {
            string Bill_no = "";
            int Return = 0;
            dt.Clear();

            try
            {
                string status = "";

                SqlCommand cmd = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, "Get_pendingPayment_transaction_bill");
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        Bill_no = dt.Rows[i]["Billno"].ToString();

                        DataTable dt_in = new DataTable();

                        SqlCommand cmd_in = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, "Get_pending_Payment_Transaction");
                        objDBAccess.AddParameterToCommand("@Billno", DBFactoryParameterType.VARCHAR, Bill_no);
                        dt_in = objDBAccess.GetTable();

                        string json = "";

                        if (dt_in.Rows.Count > 0)
                        {
                            for (int i_in = 0; i_in < dt_in.Rows.Count; i_in++)
                            {
                                if (json == "")
                                {
                                    //json = "{ \"dtVendorData\": [{\"uniqueCode\":\"" + dt_in.Rows[i_in]["uniqueCode"].ToString() + "\", \"appId\" : \"" + dt_in.Rows[i_in]["appId"].ToString() + "\", \"pfmsVendorCode\" : \"" + dt_in.Rows[i_in]["pfmsVendorCode"].ToString() + "\" , \"sanctionNo\" : \"" + dt_in.Rows[i_in]["sanctionNo"].ToString() + "\", \"sanctionDate\" : \"" + dt_in.Rows[i_in]["sanctionDate"].ToString() + "\", \"totalAmount\" : \"" + dt_in.Rows[i_in]["totalAmount"].ToString() + "\", \"payableAmount\" : \"" + dt_in.Rows[i_in]["payableAmount"].ToString() + "\", \"deptCode\" : \"" + dt_in.Rows[i_in]["deptCode"].ToString() + "\", \"BillNo\" : \"" + dt_in.Rows[i_in]["BillNo"].ToString() + "\", \"BillDate\" : \"" + dt_in.Rows[i_in]["BillDate"].ToString() + "\", \"IACode\" : \"" + dt_in.Rows[i_in]["IACode"].ToString() + "\", \"StateSchemeCode\" : \"" + dt_in.Rows[i_in]["StateSchemeCode"].ToString() + "\" , \"IFSCCode\" : \"" + dt_in.Rows[i_in]["IFSCCode"].ToString() + "\" , \"BankActNo\" : \"" + dt_in.Rows[i_in]["BankActNo"].ToString() + "\" , \"dtVendorDeduction\" : [] }]}" ;
                                    json = "{ \"dtVendorData\": [{\"uniqueCode\":\"" + dt_in.Rows[i_in]["uniqueCode"].ToString() + "\", \"appId\" : \"" + dt_in.Rows[i_in]["appId"].ToString() + "\", \"pfmsVendorCode\" : \"" + dt_in.Rows[i_in]["pfmsVendorCode"].ToString() + "\" , \"sanctionNo\" : \"" + dt_in.Rows[i_in]["sanctionNo"].ToString() + "\", \"sanctionDate\" : \"" + dt_in.Rows[i_in]["sanctionDate"].ToString() + "\", \"totalAmount\" : \"" + dt_in.Rows[i_in]["totalAmount"].ToString() + "\", \"payableAmount\" : \"" + dt_in.Rows[i_in]["payableAmount"].ToString() + "\", \"deptCode\" : \"" + dt_in.Rows[i_in]["deptCode"].ToString() + "\", \"BillNo\" : \"" + dt_in.Rows[i_in]["BillNo"].ToString() + "\", \"BillDate\" : \"" + dt_in.Rows[i_in]["BillDate"].ToString() + "\", \"IACode\" : \"" + dt_in.Rows[i_in]["IACode"].ToString() + "\", \"StateSchemeCode\" : \"" + dt_in.Rows[i_in]["StateSchemeCode"].ToString() + "\" , \"dtVendorDeduction\" : [] }]}";
                                }
                                else
                                {
                                    json = json + "," + "{ \"dtVendorData\": [{\"uniqueCode\":\"" + dt_in.Rows[i_in]["uniqueCode"].ToString() + "\", \"appId\" : \"" + dt_in.Rows[i_in]["appId"].ToString() + "\", \"pfmsVendorCode\" : \"" + dt_in.Rows[i_in]["pfmsVendorCode"].ToString() + "\" , \"sanctionNo\" : \"" + dt_in.Rows[i_in]["sanctionNo"].ToString() + "\", \"sanctionDate\" : \"" + dt_in.Rows[i_in]["sanctionDate"].ToString() + "\", \"totalAmount\" : \"" + dt_in.Rows[i_in]["totalAmount"].ToString() + "\", \"payableAmount\" : \"" + dt_in.Rows[i_in]["payableAmount"].ToString() + "\", \"deptCode\" : \"" + dt_in.Rows[i_in]["deptCode"].ToString() + "\", \"BillNo\" : \"" + dt_in.Rows[i_in]["BillNo"].ToString() + "\", \"BillDate\" : \"" + dt_in.Rows[i_in]["BillDate"].ToString() + "\", \"IACode\" : \"" + dt_in.Rows[i_in]["IACode"].ToString() + "\", \"StateSchemeCode\" : \"" + dt_in.Rows[i_in]["StateSchemeCode"].ToString() + "\" , \"dtVendorDeduction\" : [] }]}";
                                }
                            }

                            json = "[" + json + "]";
                            status = new csCommon().VendorPaymentTransaction(json, Bill_no);
                        }
                    }
                }
                Return = 1;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.VendorRegistrationPaymentModel", FunctionName = "UsersVendorPaymentTransaction()" + ActionName }, ex);
                Return = 0;
            }
            finally
            {
                dt.Dispose();
            }

            return Return;

        }
        #endregion


        #region VendorPaymentStatus
        public int UsersVendorPaymentStatus(string ActionName)
        {
            int Return = 0;
            dt.Clear();
            try
            {
                string status = "";

                SqlCommand cmd = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, ActionName);
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        status = new csCommon().VendorPaymentStatus(
                            dt.Rows[i]["uniqueCode"].ToString(),
                            dt.Rows[i]["deptCode"].ToString(),
                            dt.Rows[i]["Servicetype"].ToString()
                        );
                    }
                }
                Return = 1;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.VendorRegistrationPaymentModel", FunctionName = "UsersVendorPaymentStatus()" + ActionName }, ex);
                Return = 0;
            }
            finally
            {
                dt.Dispose();
            }
            return Return;

        }
        #endregion


        #region VendorPaymentRejectStatus
        public int UsersVendorPaymentRejectStatus(string ActionName)
        {
            int Return = 0;
            dt.Clear();
            try
            {
                string status = "";

                SqlCommand cmd = objDBAccess.CreateCommand("SP_Get_BeneficiaryVendorDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.VARCHAR, ActionName);
                dt = objDBAccess.GetTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        status = new csCommon().VendorPaymentRejectStatus(
                            dt.Rows[i]["uniqueCode"].ToString(),
                            dt.Rows[i]["deptCode"].ToString(),
                            dt.Rows[i]["Servicetype"].ToString()
                        );
                    }
                }
                Return = 1;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.VendorRegistrationPaymentModel", FunctionName = "UsersVendorPaymentRejectStatus()" + ActionName }, ex);
                Return = 0;
            }
            finally
            {
                dt.Dispose();
            }
            return Return;

        }
        #endregion


    }


}