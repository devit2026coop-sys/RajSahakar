using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Models;
namespace CoopDAL
{
    public class Users
    {
        public List<GetDasboardData_Result> GetDasboardData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0)
        {
            List<GetDasboardData_Result> result = new List<GetDasboardData_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.GetDasboardData(FinancialYear.ToString(), DivisionId.ToString(), DistrictId.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetDasboardData()" }, ex);

            }
            return result;
        }

        public List<InsertDashboardData_Result> InsertDashboardData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0)
        {
            List<InsertDashboardData_Result> result = new List<InsertDashboardData_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.InsertDashboardData(FinancialYear.ToString(), DivisionId.ToString(), DistrictId.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertDashboardData()" }, ex);

            }
            return result;
        }


        public List<GetUserListForFileTransfer_Result> GetUserListForFileTransfer(int ActId,int DistrictId,int UserId,int RoleId)
        {
            List<GetUserListForFileTransfer_Result> result = new List<GetUserListForFileTransfer_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.GetUserListForFileTransfer(ActId, DistrictId,UserId,RoleId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetDasboardData()" }, ex);

            }
            return result;
        }

        public List<GetUserListForFileTransfer_Result> TransferSocerties(int FromUserId, string SocietyId, int ToUserId, string fileName,string Remarks,string IP,string SSOId, int ActId)
        {
            List<GetUserListForFileTransfer_Result> result = new List<GetUserListForFileTransfer_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    DB.TransferSocerties(FromUserId, SocietyId,ToUserId,ActId, Remarks, fileName,IP,SSOId);
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.TransferSocerties", FunctionName = "TransferSocerties()" }, ex);

            }
            return result;
        }

        public List<GetUserSocietyList_Result> GetUserSocietyList(int UserId,  int ActId)
        {
            List<GetUserSocietyList_Result> result = new List<GetUserSocietyList_Result>();
            if (ActId !=1 || ActId!=3 ||ActId!=7)
            { 
            return result;
            }

            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    result = DB.GetUserSocietyList(ActId, UserId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.TransferSocerties", FunctionName = "TransferSocerties()" }, ex);

            }
            return result;
        }

    }
}
