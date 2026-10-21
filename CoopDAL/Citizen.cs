using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopDB;
using Models;

namespace CoopDAL
{
    public class Citizen
    {
        public List<GetDetailsForMapSociety_Result> GetSocityDataForOtpSending2(string WhereClause, int DistrictId, int ActId)
        {
           

            List<GetDetailsForMapSociety_Result> result = new List<GetDetailsForMapSociety_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    return DB.GetDetailsForMapSociety(!string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty, DistrictId, ActId).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetDetailsForMapSociety(string WhereClause)" }, ex);

            }

            return result;
        }


        public List<GetCertificateforVerify_Result> GetCertificateforVerify(string RegistrationNo, string Type = "")
        {
            List<GetCertificateforVerify_Result> result = new List<GetCertificateforVerify_Result>();
            try
            {
                using (CoopEntities DB = new CoopEntities())
                {
                    //DB.ti
                    return DB.GetCertificateforVerify(RegistrationNo, Type).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetCertificateforVerify(" + RegistrationNo + ")" }, ex);

            }
            return result;
        }

     

    }
}
