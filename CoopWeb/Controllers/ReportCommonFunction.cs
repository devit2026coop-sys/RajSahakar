using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Cooperative.Controllers
{
    public class ReportCommonFunction
    {
        public static string FertilizerreportDistrictWise(DataTable temp)
        {
            if (temp == null || temp.Rows.Count < 1)
                return "[]";

            List<FertilizerReportModel> obj = new List<FertilizerReportModel>();
            var districts = (from r in temp.AsEnumerable() select r["DISTRICT_ENG"]).Distinct().ToList();
            var fertilizers = (from r in temp.AsEnumerable() select new { FertilizerName_En = r.Field<string>("FertilizerName_En"), FertilizerName_Hi = r.Field<string>("FertilizerName_Hi") }).Distinct().ToList();

            foreach (var item in districts)
            {
                var temp2 = (from r in temp.AsEnumerable() where r.Field<string>("DISTRICT_ENG") == item.ToString() select r).ToList();

                FertilizerReportModel obj1 = new FertilizerReportModel();
                obj1.DistrictNameEn = item.ToString();
                obj1.DistrictNameHi = temp2[0]["DISTRICT_MANGAL"].ToString();
                obj1.DistrictId = Convert.ToInt32(temp2[0]["DISTRICT_ID"]);
                obj1.Balance = new List<decimal>();
                foreach (var item2 in fertilizers)
                {
                    obj1.Balance.Add(temp2.Any(row => item2.FertilizerName_En.ToString() == row.Field<String>("FertilizerName_En")) ? Convert.ToDecimal(temp2.Where(row => item2.FertilizerName_En.ToString() == row.Field<String>("FertilizerName_En")).Select(s => s.Field<decimal>("Total")).ToList().FirstOrDefault()) : default(decimal));
                }

                obj.Add(obj1);
            }

            FertilizerReportModel GrandTotal = new FertilizerReportModel();
            GrandTotal.DistrictNameEn = "Grand Total";
            GrandTotal.DistrictNameHi = "कुल योग";
            GrandTotal.DistrictId = -1;
            GrandTotal.Balance = new List<decimal>();
            for (int i = 0; i < fertilizers.Count(); i++)
            {
                GrandTotal.Balance.Add(obj.Select(P => P.Balance[i]).ToArray().Sum(g => g));
            }
            obj.Add(GrandTotal);

            FertilizerReportDataModel FinalData = new FertilizerReportDataModel();
            FinalData.Fertilizerheads = fertilizers.Select(item => Convert.ToString(item.FertilizerName_En)).ToList();
            FinalData.FertilizerheadsHi = fertilizers.Select(item => Convert.ToString(item.FertilizerName_Hi)).ToList();
            FinalData.data = obj;
            FinalData.RecordOnDate = temp.Rows[0]["OnDate"].ToString();

            var json = new JavaScriptSerializer().Serialize(FinalData);

            return json;
        }

        public static string FertilizerRetailerDetailer(DataTable temp)
        {
            if (temp == null || temp.Rows.Count < 1)
                return "[]";

            List<FertilizerRetailerModel> obj = new List<FertilizerRetailerModel>();
            var retailers = (from r in temp.AsEnumerable() select r["RetailerName"]).Distinct().ToList();
            var fertilizers = (from r in temp.AsEnumerable() select new { FertilizerName_En = r.Field<string>("FertilizerName_En"), FertilizerName_Hi = r.Field<string>("FertilizerName_Hi") }).Distinct().ToList();




            foreach (var item in retailers)
            {
                var temp2 = (from r in temp.AsEnumerable() where r.Field<string>("RetailerName") == item.ToString() select r).ToList();

                FertilizerRetailerModel obj1 = new FertilizerRetailerModel();
                obj1.RetailerName = item.ToString();
                obj1.BlockName = temp2[0]["BlockName"].ToString();
                obj1.MobileNumber = temp2[0]["Mobile"].ToString();
                obj1.Balance = new List<decimal>();
                foreach (var item2 in fertilizers)
                {
                    obj1.Balance.Add(temp2.Any(row => item2.FertilizerName_En.ToString() == row.Field<String>("FertilizerName_En")) ? Convert.ToDecimal(temp2.Where(row => item2.FertilizerName_En.ToString() == row.Field<String>("FertilizerName_En")).Select(s => s.Field<decimal>("Total")).ToList().FirstOrDefault()) : default(decimal));
                }

                obj.Add(obj1);
            }

            FertilizerRetailerModel GrandTotal = new FertilizerRetailerModel();
            GrandTotal.RetailerName = "Grand Total";
            GrandTotal.BlockName = "कुल योग";
            GrandTotal.MobileNumber = "";
            GrandTotal.Balance = new List<decimal>();
            for (int i = 0; i < fertilizers.Count(); i++)
            {
                GrandTotal.Balance.Add(obj.Select(P => P.Balance[i]).ToArray().Sum(g => g));
            }
            obj.Add(GrandTotal);

            FertilizerRetailerDataModel FinalData = new FertilizerRetailerDataModel();
            FinalData.Fertilizerheads = fertilizers.Select(item => Convert.ToString(item.FertilizerName_En)).ToList();
            FinalData.FertilizerheadsHi = fertilizers.Select(item => Convert.ToString(item.FertilizerName_Hi)).ToList();
            FinalData.data = obj;
            FinalData.RecordOnDate = temp.Rows[0]["OnDate"].ToString();

            var json = new JavaScriptSerializer().Serialize(FinalData);

            return json;
        }
    }
}