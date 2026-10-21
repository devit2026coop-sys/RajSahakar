using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class SocietyFinancialHealthController : Controller
    {
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: SocietyFinancialHealth
        public ActionResult SocietyFinancialHealth()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyBasicDetail(DataPagingModel Details,int SearchType=0)
        {
            FinancialHealthModel objFinancialHealthModel = new FinancialHealthModel(new DBLogger());
            DataTable DT = objFinancialHealthModel.GetSocietyBasicDetail(ref Details, SearchType);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONString(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSocietyBasicDetailForStatus(DataPagingModel Details, int SearchType = 0)
        {
            FinancialHealthModel objFinancialHealthModel = new FinancialHealthModel(new DBLogger());
            DataTable DT = objFinancialHealthModel.GetSocietyBasicDetailForStatus(ref Details, SearchType);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONString(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int SaveFinancialHealthRecord(SocietyFinancialHealthEntity model)
        {
            //SocietyFinancialHealth SocietyFinancialHealthdata = new SocietyFinancialHealth();
            //try
            //{
            //    System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            //    SocietyFinancialHealthdata = Request.Form["Applicationdata"].ToString().Equals("[]") ? new SocietyFinancialHealth() : (SocietyFinancialHealth)s.Deserialize(Request.Form["Applicationdata"].ToString(), typeof(SocietyFinancialHealth));
            //}
            //catch (System.Exception ex)
            //{
            //    //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            //}


            FinancialHealthModel objFinancialHealthModel = new FinancialHealthModel(new DBLogger());
            model.CreatedBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
            model.CreateIP = COMMON_FUNCTION.GetIpAddress();
            int Result = objFinancialHealthModel.InsertFinancialHealthData(model);
            return Result;
        }

        [HttpPost]
        public JsonResult ShowSocietyHealthRecord(string SocietyIdEnc)
        {
            FinancialHealthModel objFinancialHealthModel = new FinancialHealthModel(new DBLogger());
            var data = objFinancialHealthModel.GetSocietyHealthCEODetails(SocietyIdEnc);

            if (data == null)
                return Json("[]");

            SocietyFinancialHealthEntity temp = new SocietyFinancialHealthEntity();


            foreach (DataRow dr in data.Rows)
            {
                temp.Id = Convert.ToInt32(dr["AdministrativeId"].ToString());
                temp.AADHARNo = dr["Aadh"].ToString();
                temp.Name = dr["Name"].ToString();
                temp.FatherName = dr["FatherName"].ToString();
                temp.DOB = Convert.ToDateTime( dr["DOB"].ToString()).ToString("dd/MM/yyyy");
                temp.CategoryNameEng = dr["CODENAME"].ToString();
                temp.CategoryNameHi = dr["CODENAME_MANGAL"].ToString();
                temp.Address = dr["Address"].ToString();
                temp.MobileNo = dr["ContactNo"].ToString();
                temp.BankName = dr["BankName"].ToString();
                temp.BankAccountNo = dr["BankAccountNo"].ToString();
                temp.BankIFSCNo = dr["BankIFSCNo"].ToString();
                temp.BalanceInAccount = dr["BalanceInAccount"].ToString();
                temp.FDBalanceInAccount = dr["FDBalanceInAccount"].ToString();
                temp.CategoryId = dr["CategoryId"].ToString();
            }


            //var CEODetails = data.AsEnumerable().Select(p => new
            //{
            //    Name = p.Field<string>("Name"),
            //    ContactNo = p.Field<string>("ContactNo"),
            //    AadharNo = p.Field<string>("Aadh"),
            //    DOB = p.Field<DateTime>("DOB"),
            //    CODENAME = p.Field<string>("CODENAME"),
            //    CODENAME_MANGAL = p.Field<string>("CODENAME_MANGAL"),
            //    MEMBERTYPE = p.Field<string>("MEMBERTYPE"),
            //    FatherName = p.Field<string>("FatherName"),
            //    Address = p.Field<string>("Address"),
            //    BankName = p.Field<string>("BankName"),
            //    BankAccountNo = p.Field<string>("BankAccountNo"),
            //    BankIFSCNo = p.Field<string>("BankIFSCNo"),
            //    BalanceInAccount = p.Field<string>("BalanceInAccount"),
            //    FDBalanceInAccount = p.Field<string>("FDBalanceInAccount"),
            //}).ToList();

            //if (CEODetails.Count() > 0)
            //{
            //    temp.AADHARNo = CEODetails[0].AadharNo;
            //    temp.Name = CEODetails[0].Name;
            //    temp.FatherName = CEODetails[0].FatherName;
            //    temp.DOB = CEODetails[0].DOB.ToString("dd/MM/yyyy");
            //    temp.CategoryNameEng = CEODetails[0].CODENAME;
            //    temp.CategoryNameHi = CEODetails[0].CODENAME_MANGAL;
            //    temp.Address = CEODetails[0].Address;
            //    temp.MobileNo = CEODetails[0].ContactNo;
            //}

            LiabilityAssetListForSocietyEntity objAssetLiabilityData = LiabilityAssetListSocietyWise(SocietyIdEnc);
            temp.AssetList = objAssetLiabilityData.AssetList;
            temp.LiabilityList = objAssetLiabilityData.LiabilityList;

            temp.IsAccountInCCB = objAssetLiabilityData.IsAccountInCCB;
            temp.IsInvestedInCCB = objAssetLiabilityData.IsInvestedInCCB;
            temp.InvestmentAmountInCCB = objAssetLiabilityData.InvestmentAmountInCCB;

            return Json(temp);
        }


        public LiabilityAssetListForSocietyEntity LiabilityAssetListSocietyWise(string SocietyIdEnc)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            var dt = objStockModel.LiabilityAssetListSocietyWise(SocietyIdEnc);
            LiabilityAssetListForSocietyEntity temp = new LiabilityAssetListForSocietyEntity();

            if (dt == null || dt.Rows.Count==0)
                return temp;

            bool tempBool = false;
            temp.IsAccountInCCB = bool.TryParse(dt.Rows[0]["IsAccountInCCB"].ToString(),out tempBool) ? tempBool : false;
            temp.IsInvestedInCCB = bool.TryParse(dt.Rows[0]["IsInvestedInCCB"].ToString(), out tempBool) ? tempBool : false;
            temp.InvestmentAmountInCCB = Convert.ToInt32(dt.Rows[0]["InvestmentAmountInCCB"].ToString());


            temp.AssetList = new List<LiabilityAssetSocietyWiseEntity>();
            temp.LiabilityList = new List<LiabilityAssetSocietyWiseEntity>();

            var iterator = dt.AsEnumerable() // Catch all the data from db
                .Select(p => new
                {
                    NameEng = p.Field<string>("NameEng"),
                    NameHi = p.Field<string>("NameHi"),
                    GrandTotal = p.Field<decimal>("GrandTotal"),
                    IsLiability = p.Field<bool>("IsLiability"),
                    IsFurtherExtended = p.Field<bool>("IsFurtherExtended"),
                    WhereDid = p.Field<string>("WhereDid"),
                    SubTotal = p.Field<decimal>("SubTotal"),
                    InvestmentId = p.Field<int>("InvestmentId"),
                    ChildSFinancialHealthId = p.Field<int>("ChildSFinancialHealthId"),
                    ParentSFinancialHealthId = p.Field<int>("ParentSFinancialHealthId"),
                    IsAccountInCCB = p.Field<bool>("IsAccountInCCB"),
                    IsInvestedInCCB = p.Field<bool>("IsInvestedInCCB"),
                    InvestmentAmountInCCB = p.Field<int>("InvestmentAmountInCCB"),
                });


            int itemCounter = 0;
            foreach (var item in iterator)
            {
                if (item.IsLiability)
                    itemCounter = temp.LiabilityList.Where(p => p.NameEng == item.NameEng).Count();
                else
                    itemCounter = temp.AssetList.Where(p => p.NameEng == item.NameEng).Count();

                //STEP 1 : Push data in collection
                if (itemCounter == 0) // Check Duplicate Entry
                {
                    if (item.IsLiability) // Push data In Liability Collection
                    {
                        temp.LiabilityList.Add(new LiabilityAssetSocietyWiseEntity()
                        {
                            NameEng = item.NameEng,
                            NameHi = item.NameHi,
                            Amount = item.GrandTotal,
                            IsLiability = true,
                            IsFurtherExtended = item.IsFurtherExtended,
                            FinancialHealthId = item.ParentSFinancialHealthId,
                            AmountDistribution = new List<LiabilityAssetChildEntity>()
                        });
                    }
                    else // Push data In Asset Collection
                    {
                        temp.AssetList.Add(new LiabilityAssetSocietyWiseEntity()
                        {
                            NameEng = item.NameEng,
                            NameHi = item.NameHi,
                            Amount = item.GrandTotal,
                            IsLiability = false,
                            IsFurtherExtended = item.IsFurtherExtended,
                            FinancialHealthId = item.ParentSFinancialHealthId,
                            AmountDistribution = new List<LiabilityAssetChildEntity>(),
                        });
                    }
                }

                //STEP 2 : Push data in appropriate collection  (Liability / Asset List)

                if (item.IsFurtherExtended) // Push data in collection if further Heads available in Liability/Asset Heads
                {
                    if (item.IsLiability) // Add liability Sub Child Value
                    {
                        temp.LiabilityList.Where(p => p.FinancialHealthId == item.ParentSFinancialHealthId).First().AmountDistribution.Add(new LiabilityAssetChildEntity()
                        {
                            Id = item.InvestmentId,
                            WhereDid = item.WhereDid,
                            Amount = item.SubTotal
                        });
                    }
                    else // Add Asset Sub Child Value
                    {
                        temp.AssetList.Where(p => p.FinancialHealthId == item.ParentSFinancialHealthId).First().AmountDistribution.Add(new LiabilityAssetChildEntity()
                        {
                            Id = item.InvestmentId,
                            WhereDid = item.WhereDid,
                            Amount = item.SubTotal
                        });
                    }
                }
            }

            return temp;
        }
    }
}