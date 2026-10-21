using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class StockController : Controller
    {
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        int CurrentUser = 0;

        public StockController()
        {

            if (_loggedInUSerDetails != null)
                CurrentUser = Convert.ToInt32(_loggedInUSerDetails.UserId);
        }

        [HttpGet]
        public ActionResult ProcessingUnit()
        {
            return View();
        }


        [HttpPost]
        public int ProcessingUnitInsertUpdate(ProcessingUnitEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.ProcessingUnitId > 0 ? 2 : 1;
            return objStockModel.ProcessingUnitInsertUpdate(model);
        }

        [HttpPost]
        public int ProcessingUnitSetStatus(int ProcessingUnitId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.ProcessingUnitSetStatus(ProcessingUnitId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string ProcessingUnitList(int ProcessingUnitId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.ProcessingUnitList(ProcessingUnitId));
        }

        [HttpGet]
        public ActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public int ProductInsertUpdate(ProductEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.ProductId > 0 ? 2 : 1;
            return objStockModel.ProductInsertUpdate(model);
        }

        [HttpPost]
        public int ProductSetStatus(int ProductId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.ProductSetStatus(ProductId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string ProductList(int ProductId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.ProductList(ProductId));
        }

        [HttpGet]
        public ActionResult SubProduct()
        {
            return View();
        }

        [HttpPost]
        public int ProductSubCategoryInsertUpdate(ProductSubCategoryEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.ProductSubCategoryId > 0 ? 2 : 1;
            return objStockModel.ProductSubCategoryInsertUpdate(model);
        }

        [HttpPost]
        public int ProductSubCategorySetStatus(int ProductSubCategoryId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.ProductSubCategorySetStatus(ProductSubCategoryId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string ProductSubCategoryList(int ProductSubCategoryId, int ProductId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.ProductSubCategoryList(ProductSubCategoryId, ProductId));
        }

        [HttpGet]
        public ActionResult Packing()
        {
            return View();
        }

        [HttpPost]
        public int PackingInsertUpdate(PackingEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.PackingId > 0 ? 2 : 1;
            return objStockModel.PackingInsertUpdate(model);
        }

        [HttpPost]
        public int PackingSetStatus(int PackingId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.PackingSetStatus(PackingId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string PackingList(int PackingId, int ProductId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.PackingList(PackingId, ProductId));
        }

        [HttpGet]
        public ActionResult RejectReason()
        {
            return View();
        }

        [HttpPost]
        public int RejectReasonInsertUpdate(RejectReasonEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.RejectionId > 0 ? 2 : 1;
            return objStockModel.RejectReasonInsertUpdate(model);
        }

        [HttpPost]
        public int RejectReasonSetStatus(int RejectionId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.RejectReasonSetStatus(RejectionId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string RejectionReasonList()
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.RejectionReasonList());
        }

        [HttpGet]
        public ActionResult Units()
        {
            return View();
        }

        [HttpPost]
        public int UnitMasterInsertUpdate(UnitMasterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ActionBy = CurrentUser;
            model.ActionId = model.UnitId > 0 ? 2 : 1;
            return objStockModel.UnitMasterInsertUpdate(model);
        }

        [HttpPost]
        public int UnitMasterSetStatus(int UnitId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.UnitMasterSetStatus(UnitId, ActionId);
        }

        [HttpPost]
        //[OutputCache(Duration = 30, VaryByParam = "ProcessingUnitId")]
        public string UnitMasterList(int UnitId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.UnitMasterList(UnitId));
        }

        [HttpGet]
        public ActionResult InitialStockPlant()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InitialStockDepot()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CaptureInitialStockByProductId(int ProductId, bool ShowAll = false)
        {
            StockModel objStockModel = new StockModel(new DBLogger());

            List<PackingEntity> packingList = objStockModel.PackingList(0, ProductId).AsEnumerable().Select(p => new PackingEntity()
            {
                PackingId = p.Field<int>("PackingId"),
                PackingNameEng = p.Field<string>("PackingNameEng"),
                PackingNameHi = p.Field<string>("PackingNameHi"),
                IsForAll = p.Field<bool>("IsForAll")
            }).Where(q => ShowAll ? 1 == 1 : q.IsForAll).ToList();

            List<ProductSubCategoryEntity> productSubCategorylist = objStockModel.ProductSubCategoryList(0, ProductId).AsEnumerable().Select(p => new ProductSubCategoryEntity()
            {
                ProductSubCategoryId = p.Field<int>("ProductSubCategoryId"),
                ProductSubCategoryNameEng = p.Field<string>("ProductSubCategoryNameEng"),
                ProductSubCategoryNameHi = p.Field<string>("ProductSubCategoryNameHi")
            }).ToList();

            if (!(packingList.Count() > 0 && productSubCategorylist.Count() > 0))
                return Json("{}");


            InitialStockEntity objInitialStockEntity = new InitialStockEntity();
            objInitialStockEntity.PackingList = packingList;
            objInitialStockEntity.ProductSubCategoryList = productSubCategorylist;
            objInitialStockEntity.ProductSubCategoryPackingMappingList = new List<ProductSubCategoryPackingMappingEntity>();

            foreach (var productSubCategory in objInitialStockEntity.ProductSubCategoryList)
            {
                ProductSubCategoryPackingMappingEntity item = new ProductSubCategoryPackingMappingEntity();
                item.ProductSubCategoryNameEng = productSubCategory.ProductSubCategoryNameEng;
                item.ProductSubCategoryNameHi = productSubCategory.ProductSubCategoryNameHi;
                item.ProductSubCategoryId = productSubCategory.ProductSubCategoryId;

                List<ProductSubCategoryQuantityEntity> temp = new List<ProductSubCategoryQuantityEntity>();

                foreach (var packing in objInitialStockEntity.PackingList)
                {
                    temp.Add(new ProductSubCategoryQuantityEntity()
                    {
                        PackingId = packing.PackingId,
                        ProductSubCategoryId = productSubCategory.ProductSubCategoryId,
                        Quantity = 0
                    });
                }

                item.ProductSubCategoryQuantityEntity = temp;
                objInitialStockEntity.ProductSubCategoryPackingMappingList.Add(item);
            }

            return Json(objInitialStockEntity);
        }

        [HttpPost]
        public JsonResult ShowInitialStockData(int ProductId, bool IsPlant, bool ShowAll = false)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            InitialStockEntity objInitialStockEntity = new InitialStockEntity();

            DataTable dt = objStockModel.InitialStockPlantDepotList(_loggedInUSerDetails.PlantId > 0 ? _loggedInUSerDetails.PlantId : _loggedInUSerDetails.DepotId, _loggedInUSerDetails.PlantId > 0 ? true : false, ProductId); // Replace 1 with Actual PlantId By Logic

            //if (dt == null || dt.Rows.Count < 1)
            //    return Json("[]");

            var temp2 = dt.AsEnumerable().Select(p => new
            {
                ProductId = p.Field<int>("ProductId"),
                ProductSubcategoryId = p.Field<int>("ProductSubcategoryId"),
                PackagingId = p.Field<int>("PackagingId"),
                Quantity = p.Field<int>("Quantity")
            }).ToList();

            var tempProductSubCategories = temp2.Select(q => q.ProductSubcategoryId).Distinct();

            var json = JsonConvert.SerializeObject(this.CaptureInitialStockByProductId(ProductId, ShowAll));
            objInitialStockEntity = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());

            foreach (var item in tempProductSubCategories)
            {
                var t = objInitialStockEntity.ProductSubCategoryPackingMappingList.Where(p => p.ProductSubCategoryId == item).FirstOrDefault();

                if (t != null)
                {
                    foreach (var item2 in temp2.Where(q => q.ProductSubcategoryId == item))
                    {
                        if (t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First() != null)
                        {
                            t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First().Quantity = item2.Quantity;
                        }
                    }
                }
            }


            return Json(objInitialStockEntity);
        }

        [HttpPost]
        public int InsertInitialStock(InitialStockEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.ProcessingUnitId = _loggedInUSerDetails.DistrictId;
            int Result = 0;
            if (model.IsPlant)
                Result = objStockModel.InsertInitialStock(model, Convert.ToInt32(_loggedInUSerDetails.UserId));
            else if (!model.IsPlant)
                Result = objStockModel.DepotSalesRegisterInitialEnty(model, Convert.ToInt32(_loggedInUSerDetails.UserId));

            return Result;
        }

        [HttpGet]
        public ActionResult StockPurchase()
        {
            return View();
        }

        [HttpPost]
        public string StockPurchaseRequestList(int ProcessingUnitId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.StockPurchaseRequestList(ProcessingUnitId));
        }

        //[HttpGet]
        //public ActionResult StockPurchaseDelivery()
        //{
        //    return View();
        //}



        [HttpGet]
        public ActionResult StockSales()
        {
            return View();
        }

        [HttpPost]
        public int InsertStockSales(DepotSalesRegisterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());

            int ZeroQuantityCount = 0;
            int TotalRecordCheck = 0;

            foreach (var item in model.ProductSalesEntryDetails)
            {
                foreach (var temp in item.ProductSubCategoryQuantityEntity)
                {
                    int availableQuantity = model.ProductAvailableDetails
                        .Where(p => p.ProductSubCategoryId == temp.ProductSubCategoryId)
                        .Select(q => q.ProductSubCategoryQuantityEntity
                        .Where(l => l.PackingId == temp.PackingId)
                        .Select(m => m.Quantity)
                        .FirstOrDefault())
                        .FirstOrDefault();

                    if (availableQuantity < temp.Quantity)
                        return -3;

                    TotalRecordCheck++;

                    if (temp.Quantity == 0)
                        ZeroQuantityCount++;
                }
            }

            if (TotalRecordCheck == ZeroQuantityCount)
                return -4;



            int Result = objStockModel.SalesRegisterEnty(model, CurrentUser);
            return Result;
        }

        [HttpPost]
        public int InsertStockPurchase(PurchaseRegisterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.CreateBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
            int Result = objStockModel.InsertStockPurchase(model);
            return Result;
        }


        //[HttpGet]
        //public ActionResult StockDemandByDepot()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public int InsertStockDemandByDepot(StockDemandByDepotTilamSanghEntity model)
        //{
        //    model.CreateBy = CurrentUser;
        //    StockModel objStockModel = new StockModel(new DBLogger());
        //    int Result = objStockModel.InsertStockDemandByDepot(model);
        //    return Result;
        //}

        [HttpPost]
        public string StockTransferSalesOrderByHqToPlantList()
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.StockTransferSalesOrderByHqToPlantList(_loggedInUSerDetails.UserRoleDeptMappingId));
        }

        [HttpPost]
        public int DemandSetStatus(int DemandId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.DemandSetStatus(DemandId, ActionId);
        }

        //public ActionResult AssetLiabilitySocietyWise()
        //{
        //    return View();
        //}

        public ActionResult StockTransferSaleOrderByHQ()
        {
            return View();
        }

        [HttpPost]
        public int InsertStockTransferSaleOrderByHqToPlant(StockTransferSaleOrderOnPlantByHqEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());

            model.DepotId = model.IsStockTransfer ? model.DepotId : 0;
            model.FirmName = !model.IsStockTransfer ? model.FirmName : string.Empty;
            model.CreateBy = CurrentUser;

            int Result = objStockModel.InsertStockTransferSaleOrderByHqToPlant(model);
            return Result;
        }



        [HttpGet]
        public ActionResult PriceMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PriceMaster(PriceMasterEntity Details)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            int result = objStockModel.InsertPriceMaster(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPriceMasterList(DataPagingModel Details)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable DT = objStockModel.GetPriceMasterList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public string DepotList()
        //{
        //    StockModel objStockModel = new StockModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONString(objStockModel.DepotList());
        //}

        [HttpGet]
        public ActionResult StockPurchaseVerificationByHq()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StockPurchaseVerificationByPlant()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetShowInitialStockData()
        {
            StockModel objStockModel = new StockModel(new DBLogger());

            DataTable dt = objStockModel.StockPurchaseDetail(_loggedInUSerDetails.UserRoleDeptMappingId);

            if (dt == null || dt.Rows.Count < 1)
                return Json("[]");


            List<PurchaseRegisterEntity> finalData = new List<PurchaseRegisterEntity>();

            foreach (var item in dt.AsEnumerable())
            {
                int tempPurchaseRegisterid = item.Field<int>("PurchaseRegisterId");

                if (finalData.Count() == 0 || finalData.Where(p => p.PurchaseRegisterId == tempPurchaseRegisterid).Count() == 0)
                {
                    PurchaseRegisterEntity temp = new PurchaseRegisterEntity();

                    temp.PurchaseRegisterId = tempPurchaseRegisterid;
                    temp.ProcessingUnitId = item.Field<int>("ProcessingUnitId");

                    temp.PlantDetails = new ProcessingUnitEntity()
                    {
                        ProcessingUnitId = item.Field<int>("ProcessingUnitId"),
                        ProcessingUnitNameEng = item.Field<string>("ProcessingUnitNameEng"),
                        ProcessingUnitNameHi = item.Field<string>("ProcessingUnitNameHi")
                    };

                    temp.PartyName = item.Field<string>("PartyName");
                    temp.PartyNameHi = item.Field<string>("PartyNameHi");
                    temp.BrokerName = item.Field<string>("BrokerName");
                    temp.BrokerNameHi = item.Field<string>("BrokerNameHi");

                    temp.ProductId = item.Field<int>("ProductId");
                    temp.ProductDetails = new ProductEntity()
                    {
                        ProductId = item.Field<int>("ProductId"),
                        ProductNameEng = item.Field<string>("ProductNameEng"),
                        ProductNameHi = item.Field<string>("ProductNameHi")
                    };

                    temp.ProductSubCategoryId = item.Field<int>("ProductSubCategoryId");
                    temp.ProductSubCategoryDetails = new ProductSubCategoryEntity()
                    {
                        ProductId = item.Field<int>("ProductId"),
                        ProductSubCategoryId = item.Field<int>("ProductSubCategoryId"),
                        ProductSubCategoryNameEng = item.Field<string>("ProductSubCategoryNameEng"),
                        ProductSubCategoryNameHi = item.Field<string>("ProductSubCategoryNameHi")
                    };

                    temp.UnitId = item.Field<int>("UnitId");
                    temp.UnitDetails = new UnitMasterEntity()
                    {
                        UnitId = item.Field<int>("UnitId"),
                        UnitNameEng = item.Field<string>("UnitNameEng"),
                        UnitNameHi = item.Field<string>("UnitNameHi")
                    };

                    temp.Quantity = item.Field<decimal>("Quantity");
                    temp.Rate = item.Field<decimal>("Rate");
                    temp.RateUnitDetails = new UnitMasterEntity()
                    {
                        UnitId = item.Field<int>("RateUnitId"),
                        UnitNameEng = item.Field<string>("RateUnitEng"),
                        UnitNameHi = item.Field<string>("RateUnitHi")
                    };

                    temp.RateQuantity = item.Field<int>("RateQuantity");
                    temp.PurchasePaymentDate = item.Field<string>("PurchasePaymentDate");
                    temp.ExpectedDeliveryDate = item.Field<string>("ExpectedDeliveryDate");
                    temp.SaleContractNo = item.Field<string>("SaleContractNo");
                    temp.Conditions = item.Field<string>("Conditions");
                    temp.Remark = item.Field<string>("Remarks");

                    temp.StockDetails = new List<PurchaseRegisterDetailEntity>();

                    var _details = dt.AsEnumerable().Where(q => q.Field<int>("PurchaseRegisterId") == tempPurchaseRegisterid);

                    if (_details != null)
                    {
                        foreach (var item2 in _details)
                        {
                            PurchaseRegisterDetailEntity A = new PurchaseRegisterDetailEntity();
                            A.PurchaseRegisterDetailId = item2.Field<int>("PurchaseRegisterDetailId");
                            A.UnitId = item2.Field<int>("DetailUnitId");
                            A.UnitDetails = new UnitMasterEntity()
                            {
                                UnitId = item2.Field<int>("DetailUnitId"),
                                UnitNameEng = item2.Field<string>("DetailUnitNameEng"),
                                UnitNameHi = item2.Field<string>("DetailUnitNameHi")
                            };

                            A.Quantity = item2.Field<decimal>("DetailQuantity");
                            A.VehicleNo = item2.Field<string>("VehicleNo");
                            A.IsTakeAction = item2.Field<int>("IsTakeAction") == 1 ? true : false;
                            A.IsTakeActionByHq = item2.Field<int>("IsTakeActionByHq") == 1 ? true : false;

                            temp.StockDetails.Add(A);
                        }
                    }


                    finalData.Add(temp);
                }
            }

            return Json(finalData);
        }

        [HttpPost]
        public int SaveResponseOnPurchseDetails(PurchaseRegisterDetailEntity model, int mode)
        {
            model.HQUserId = Convert.ToInt32(_loggedInUSerDetails.UserId);

            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    if (!Directory.Exists("~/Content/TilamSangh/StockDeliveryFiles"))
                    {
                        Directory.CreateDirectory("~/Content/TilamSangh/StockDeliveryFiles");
                    }

                    if ((model.IsAccepted ?? false) == true && mode == 1)
                    {
                        model.AcceptanceFile = fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/TilamSangh/StockDeliveryFiles"), fileName));
                    }
                    else if ((model.IsDebitNote ?? false) == true && mode == 2)
                    {
                        model.AcceptanceFile = fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/TilamSangh/StockDeliveryFiles"), fileName));
                    }
                }
                catch { }
            }

            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.StockPurchaseDetailsResponseSave(model, mode);
        }

        [HttpPost]
        public JsonResult StockPurchaseDetailById(int PurchaseRegisterId, int PurchaseRegisterDetailId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            var result = objStockModel.StockPurchaseDetailById(PurchaseRegisterId, PurchaseRegisterDetailId);

            if (result == null)
                return Json("[]");

            PurchaseRegisterEntity obj = new PurchaseRegisterEntity();
            obj.StockDetails = new List<PurchaseRegisterDetailEntity>();

            foreach (var item in result.AsEnumerable())
            {
                PurchaseRegisterDetailEntity temp = new PurchaseRegisterDetailEntity();
                temp.PurchaseRegisterDetailId = item.Field<int>("PurchaseRegisterDetailId");
                temp.PurchaseRegisterId = item.Field<int>("PurchaseRegisterId");

                temp.UnitId = item.Field<int>("UnitId");
                temp.UnitDetails = new UnitMasterEntity()
                {
                    UnitId = item.Field<int>("UnitId"),
                    UnitNameEng = item.Field<string>("UnitNameEng"),
                    UnitNameHi = item.Field<string>("UnitNameHi")
                };
                temp.Quantity = item.Field<decimal>("Quantity");
                temp.VehicleNo = item.Field<string>("VehicleNo");
                temp.IsQualityCheckOkay = item.Field<Nullable<bool>>("IsQualityCheckOkay");

                temp.IsAccepted = item.Field<Nullable<bool>>("IsAccepted");
                temp.AcceptanceFile = item.Field<string>("AcceptanceFile");
                temp.RejectionId = item.Field<int>("RejectionId");

                temp.RejectDetailsOfPlant = new RejectReasonEntity()
                {
                    RejectionId = item.Field<int>("RejectionId"),
                    RejectionReasonEng = item.Field<string>("PlantRejectionReasonEng"),
                    RejectionReasonHi = item.Field<string>("PlantRejectionReasonHi")
                };

                temp.PlantUserId = item.Field<int>("PlantUserId");
                temp.RemarkByPlant = item.Field<string>("RemarkByPlant");
                temp.PlantUserActivityDateTime = item.Field<string>("PlantUserActivityDateTime");

                temp.PlantUserDetails = new UserEntity()
                {
                    UserId = item.Field<int>("PlantUserId"),
                    OfficerNameEn = item.Field<string>("PlantUserNameEng"),
                    OfficerNameHi = item.Field<string>("PlantUserNameHi")
                };

                temp.IsApprovedByHQ = item.Field<Nullable<bool>>("IsApprovedByHQ");
                temp.RejectionIdByHQ = item.Field<int>("RejectionIdByHQ");

                temp.RejectDetailsOfHq = new RejectReasonEntity()
                {
                    RejectionId = item.Field<int>("RejectionIdByHQ"),
                    RejectionReasonEng = item.Field<string>("RejectionReasonHqEng"),
                    RejectionReasonHi = item.Field<string>("RejectionReasonHqHi")
                };

                temp.HQUserId = item.Field<int>("HQUserId");
                temp.RemarkByHQ = item.Field<string>("RemarkByHQ");
                temp.HQUserActivityDateTime = item.Field<string>("HQUserActivityDateTime");

                temp.HqUserDetails = new UserEntity()
                {
                    UserId = item.Field<int>("HQUserId"),
                    OfficerNameEn = item.Field<string>("HqUserNameEng"),
                    OfficerNameHi = item.Field<string>("HqUserNameHi")
                };


                temp.IsDebitNote = item.Field<Nullable<bool>>("IsDebitNote");
                temp.DebitNoteFile = item.Field<string>("DebitNoteFile");

                obj.StockDetails.Add(temp);
            }

            return Json(obj);
        }

        [HttpPost]
        public JsonResult StockTransferSalesOrderDetails(int Id, int ProductId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            InitialStockEntity objInitialStockEntity = new InitialStockEntity();

            DataTable dt = objStockModel.StockTransferSalesOrderDetails(Id);

            if (dt == null || dt.Rows.Count < 1)
                return Json("[]");

            var temp2 = dt.AsEnumerable().Select(p => new
            {
                ProductId = p.Field<int>("ProductId"),
                ProductSubcategoryId = p.Field<int>("ProductSubcategoryId"),
                PackagingId = p.Field<int>("PackagingId"),
                Quantity = p.Field<int>("Quantity")
            }).ToList();

            var tempProductSubCategories = temp2.Select(q => q.ProductSubcategoryId).Distinct();

            var json = JsonConvert.SerializeObject(this.CaptureInitialStockByProductId(ProductId, false));
            objInitialStockEntity = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());

            foreach (var item in tempProductSubCategories)
            {
                var t = objInitialStockEntity.ProductSubCategoryPackingMappingList.Where(p => p.ProductSubCategoryId == item).FirstOrDefault();

                if (t != null)
                {
                    foreach (var item2 in temp2.Where(q => q.ProductSubcategoryId == item))
                    {
                        if (t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First() != null)
                        {
                            t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First().Quantity = item2.Quantity;
                        }
                    }
                }
            }

            return Json(objInitialStockEntity);
        }

        [HttpGet]
        public ActionResult StockTransferSalesOrderAcceptance()
        {
            return View();
        }

        [HttpPost]
        public int StockTransferSalesOrderDelivery(int Id)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.StockTransferSalesOrderDelivery(Id, CurrentUser);
        }

        [HttpPost]
        public JsonResult GetSalesRegisterDataForDepot(int ProductId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DepotSalesRegisterEntity objEntity = new DepotSalesRegisterEntity();
            InitialStockEntity temp = new InitialStockEntity();

            DataTable dt = objStockModel.DepotAvailableStockList(_loggedInUSerDetails.DistrictId); // Replace 1 with Actual Depot By Logic

            if (dt == null || dt.Rows.Count < 1)
                return Json("[]");

            var temp2 = dt.AsEnumerable().Select(p => new
            {
                ProductId = p.Field<int>("ProductId"),
                ProductSubcategoryId = p.Field<int>("ProductSubcategoryId"),
                PackagingId = p.Field<int>("PackagingId"),
                Quantity = p.Field<int>("Quantity")
            }).ToList();

            var tempProductSubCategories = temp2.Select(q => q.ProductSubcategoryId).Distinct();

            var json = JsonConvert.SerializeObject(this.CaptureInitialStockByProductId(ProductId, false));
            temp = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());

            objEntity.DepotId = _loggedInUSerDetails.DistrictId;// Convert.ToInt32( dt.Rows[0]["DepotId"]);
            objEntity.ProductSubCategoryList = temp.ProductSubCategoryList;
            objEntity.PackingList = temp.PackingList;
            objEntity.ProductSalesEntryDetails = temp.ProductSubCategoryPackingMappingList;

            foreach (var item in tempProductSubCategories)
            {
                var t = temp.ProductSubCategoryPackingMappingList.Where(p => p.ProductSubCategoryId == item).FirstOrDefault();

                if (t != null)
                {
                    foreach (var item2 in temp2.Where(q => q.ProductSubcategoryId == item))
                    {
                        if (t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).FirstOrDefault() != null)
                        {
                            t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First().Quantity = item2.Quantity;
                        }
                    }
                }
            }

            objEntity.ProductAvailableDetails = objEntity.ProductSalesEntryDetails; // Data has been transformed after iteration
            //objEntity.ProductSalesEntryDetails.ForEach(p=>p.ProductSubCategoryQuantityEntity.ForEach(q=>  q.Quantity = 0 ));


            return Json(objEntity);
        }

        [HttpGet]
        public ActionResult Depot()
        {
            return View();
        }

        [HttpPost]
        public int DepotInsertUpdate(DepotMasterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            model.CreateBy = CurrentUser;
            model.ActionId = model.DepotId > 0 ? 2 : 1;
            return objStockModel.DepotInsertUpdate(model);
        }

        [HttpPost]
        public int DepotSetStatus(int DepotId, int ActionId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return objStockModel.DepotSetStatus(DepotId, ActionId);
        }

        [HttpPost]
        public string DepotList(int DepotId = 0)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objStockModel.DepotList(DepotId));
        }

        [HttpPost]
        public string TilamSanghDashboardData()
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataSet DT = objStockModel.TilamSanghDashboardData(_loggedInUSerDetails.UserRoleDeptMappingId);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }

        #region MIS Report Part of Tilam Sangh Module
        public ActionResult InitialStockReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowInitialStockDataMIS(int ProductId, bool IsPlant, int PlantDepotId, bool ShowAll = false)
        {
            // Prepare Skeleton for Data Capture
            List<InitialStockEntity> finalData = new List<InitialStockEntity>();
            List<ProcessingUnitEntity> PlantDepotList = new List<ProcessingUnitEntity>();
            var json = JsonConvert.SerializeObject(this.CaptureInitialStockByProductId(ProductId, ShowAll));
            InitialStockEntity objInitialStockEntity = new InitialStockEntity();
            objInitialStockEntity = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());


            StockModel objStockModel = new StockModel(new DBLogger());

            if (IsPlant)
            {
                PlantDepotList = objStockModel.ProcessingUnitList(PlantDepotId).AsEnumerable()
                    .Where(p => p.Field<bool>("IsOperating") == true && p.Field<bool>("IsActive") == true)
                    .Select(q => new ProcessingUnitEntity { ProcessingUnitId = q.Field<int>("ProcessingUnitId"), ProcessingUnitNameEng = q.Field<string>("ProcessingUnitNameEng"), ProcessingUnitNameHi = q.Field<string>("ProcessingUnitNameHi") }).ToList();
            }
            else
            {
                PlantDepotList = objStockModel.DepotList(PlantDepotId).AsEnumerable()
                    .Where(p => p.Field<bool>("IsActive") == true)
                    .Select(q => new ProcessingUnitEntity { ProcessingUnitId = q.Field<int>("DepotId"), ProcessingUnitNameEng = q.Field<string>("AgentNameEng") + " , " + q.Field<string>("DepotNameEng"), ProcessingUnitNameHi = q.Field<string>("AgentNameHi") + " , " + q.Field<string>("DepotNameHi") }).ToList();
            }

            foreach (var plantDepotDetail in PlantDepotList) // Loop all Plant/Depot and push into collection
            {
                InitialStockEntity temp = new InitialStockEntity();
                temp = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());
                temp.IsPlant = IsPlant;

                temp.ProcessingUnitDetails = new ProcessingUnitEntity()
                {
                    ProcessingUnitId = plantDepotDetail.ProcessingUnitId,
                    ProcessingUnitNameEng = plantDepotDetail.ProcessingUnitNameEng,
                    ProcessingUnitNameHi = plantDepotDetail.ProcessingUnitNameHi
                };

                DataTable dt = objStockModel.InitialStockPlantDepotList(plantDepotDetail.ProcessingUnitId, IsPlant, ProductId);

                var temp2 = dt.AsEnumerable().Select(p => new
                {
                    ProductId = p.Field<int>("ProductId"),
                    ProductSubcategoryId = p.Field<int>("ProductSubcategoryId"),
                    PackagingId = p.Field<int>("PackagingId"),
                    Quantity = p.Field<int>("Quantity")
                }).ToList();

                var tempProductSubCategories = temp2.Select(q => q.ProductSubcategoryId).Distinct();

                foreach (var item in tempProductSubCategories)
                {
                    var t = temp.ProductSubCategoryPackingMappingList.Where(p => p.ProductSubCategoryId == item).FirstOrDefault();

                    if (t != null)
                    {
                        foreach (var item2 in temp2.Where(q => q.ProductSubcategoryId == item))
                        {
                            if (t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First() != null)
                            {
                                t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First().Quantity = item2.Quantity;
                            }
                        }
                    }
                }

                finalData.Add(temp);
            }

            return Json(finalData);
        }

        public ActionResult PurchaseRegisterReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowPurchaseRegisterDataMIS(int ProductId, int ProductSubCategoryId, int PlantId, string ContractNo)
        {
            StockModel objStockModel = new StockModel(new DBLogger());

            string WhereClause = " AND 1=1 ";

            if (ProductId > 0)
                WhereClause += " AND A.ProductId = " + ProductId;

            if (ProductSubCategoryId > 0)
                WhereClause += " AND E.ProductSubCategoryId = " + ProductSubCategoryId;

            if (PlantId > 0)
                WhereClause += " AND A.ProcessingUnitId=" + PlantId;

            if (!string.IsNullOrEmpty(ContractNo))
                WhereClause += " AND A.SaleContractNo like '%" + ContractNo + "%'";

            DataTable dt = objStockModel.StockPurchaseReportMIS(WhereClause);

            if (dt == null || dt.Rows.Count < 1)
                return Json("[]");


            List<PurchaseRegisterEntity> finalData = new List<PurchaseRegisterEntity>();

            foreach (var item in dt.AsEnumerable())
            {
                int tempPurchaseRegisterid = item.Field<int>("PurchaseRegisterId");

                if (finalData.Count() == 0 || finalData.Where(p => p.PurchaseRegisterId == tempPurchaseRegisterid).Count() == 0)
                {
                    PurchaseRegisterEntity temp = new PurchaseRegisterEntity();

                    temp.PurchaseRegisterId = tempPurchaseRegisterid;
                    temp.ProcessingUnitId = item.Field<int>("ProcessingUnitId");

                    temp.PlantDetails = new ProcessingUnitEntity()
                    {
                        ProcessingUnitId = item.Field<int>("ProcessingUnitId"),
                        ProcessingUnitNameEng = item.Field<string>("ProcessingUnitNameEng"),
                        ProcessingUnitNameHi = item.Field<string>("ProcessingUnitNameHi")
                    };

                    temp.PartyName = item.Field<string>("PartyName");
                    temp.PartyNameHi = item.Field<string>("PartyNameHi");
                    temp.BrokerName = item.Field<string>("BrokerName");
                    temp.BrokerNameHi = item.Field<string>("BrokerNameHi");

                    temp.ProductId = item.Field<int>("ProductId");
                    temp.ProductDetails = new ProductEntity()
                    {
                        ProductId = item.Field<int>("ProductId"),
                        ProductNameEng = item.Field<string>("ProductNameEng"),
                        ProductNameHi = item.Field<string>("ProductNameHi")
                    };

                    temp.ProductSubCategoryId = item.Field<int>("ProductSubCategoryId");
                    temp.ProductSubCategoryDetails = new ProductSubCategoryEntity()
                    {
                        ProductId = item.Field<int>("ProductId"),
                        ProductSubCategoryId = item.Field<int>("ProductSubCategoryId"),
                        ProductSubCategoryNameEng = item.Field<string>("ProductSubCategoryNameEng"),
                        ProductSubCategoryNameHi = item.Field<string>("ProductSubCategoryNameHi")
                    };

                    temp.UnitId = item.Field<int>("UnitId");
                    temp.UnitDetails = new UnitMasterEntity()
                    {
                        UnitId = item.Field<int>("UnitId"),
                        UnitNameEng = item.Field<string>("UnitNameEng"),
                        UnitNameHi = item.Field<string>("UnitNameHi")
                    };

                    temp.Quantity = item.Field<decimal>("Quantity");
                    temp.Rate = item.Field<decimal>("Rate");
                    temp.RateUnitDetails = new UnitMasterEntity()
                    {
                        UnitId = item.Field<int>("RateUnitId"),
                        UnitNameEng = item.Field<string>("RateUnitEng"),
                        UnitNameHi = item.Field<string>("RateUnitHi")
                    };

                    temp.RateQuantity = item.Field<int>("RateQuantity");
                    temp.PurchasePaymentDate = item.Field<string>("PurchasePaymentDate");
                    temp.ExpectedDeliveryDate = item.Field<string>("ExpectedDeliveryDate");
                    temp.SaleContractNo = item.Field<string>("SaleContractNo");
                    temp.Conditions = item.Field<string>("Conditions");
                    temp.Remark = item.Field<string>("Remarks");

                    temp.StockDetails = new List<PurchaseRegisterDetailEntity>();

                    var _details = dt.AsEnumerable().Where(q => q.Field<int>("PurchaseRegisterId") == tempPurchaseRegisterid);

                    if (_details != null)
                    {
                        foreach (var item2 in _details)
                        {
                            PurchaseRegisterDetailEntity A = new PurchaseRegisterDetailEntity();
                            A.PurchaseRegisterDetailId = item2.Field<int>("PurchaseRegisterDetailId");
                            A.UnitId = item2.Field<int>("DetailUnitId");
                            A.UnitDetails = new UnitMasterEntity()
                            {
                                UnitId = item2.Field<int>("DetailUnitId"),
                                UnitNameEng = item2.Field<string>("DetailUnitNameEng"),
                                UnitNameHi = item2.Field<string>("DetailUnitNameHi")
                            };

                            A.Quantity = item2.Field<decimal>("DetailQuantity");
                            A.VehicleNo = item2.Field<string>("VehicleNo");
                            A.IsTakeAction = item2.Field<int>("IsTakeAction") == 1 ? true : false;
                            A.IsTakeActionByHq = item2.Field<int>("IsTakeActionByHq") == 1 ? true : false;

                            A.IsQualityCheckOkay = item2.Field<bool?>("IsQualityCheckOkay");
                            A.IsAccepted = item2.Field<bool?>("IsAccepted");
                            A.AcceptanceFile = item2.Field<string>("AcceptanceFile");

                            A.RejectionId = item2.Field<int>("RejectionId");
                            A.RejectDetailsOfPlant = new RejectReasonEntity()
                            {
                                RejectionId = item2.Field<int>("RejectionId"),
                                RejectionReasonEng = item2.Field<string>("PlantRejectionEng"),
                                RejectionReasonHi = item2.Field<string>("PlantRejectionHi")
                            };
                            A.RemarkByPlant = item2.Field<string>("RemarkByPlant");


                            A.PlantUserId = item2.Field<int>("PlantUserId");
                            A.PlantUserDetails = new UserEntity()
                            {
                                UserId = item2.Field<int>("PlantUserId"),
                                OfficerNameEn = item2.Field<string>("PlantUserNameEng"),
                                OfficerNameHi = item2.Field<string>("PlantUserNameHi")
                            };

                            A.PlantUserActivityDateTime = item2.Field<string>("PlantUserActivityDateTime");

                            A.IsApprovedByHQ = item2.Field<bool?>("IsApprovedByHq");
                            A.RejectDetailsOfHq = new RejectReasonEntity()
                            {
                                RejectionId = item2.Field<int>("RejectionIdByHQ"),
                                RejectionReasonEng = item2.Field<string>("HqRejectionEng"),
                                RejectionReasonHi = item2.Field<string>("HqRejectionHi")
                            };
                            A.RemarkByHQ = item2.Field<string>("RemarkByHQ");

                            A.HQUserId = item2.Field<int>("HQUserId");
                            A.HqUserDetails = new UserEntity()
                            {
                                UserId = item2.Field<int>("HQUserId"),
                                OfficerNameEn = item2.Field<string>("HqUserNameEng"),
                                OfficerNameHi = item2.Field<string>("hqUserNameHi")
                            };

                            A.HQUserActivityDateTime = item2.Field<string>("HqUserActivityDateTime");
                            A.IsDebitNote = item2.Field<bool>("IsDebitNote");
                            A.DebitNoteFile = item2.Field<string>("DebitNoteFile");

                            temp.StockDetails.Add(A);
                        }
                    }

                    finalData.Add(temp);
                }
            }

            return Json(finalData);
        }


        [HttpPost]
        public JsonResult StockTransferSalesOrderReportMIS(int OrderType, int OrderForPlantId, int ProductId, string OrderDate, int DepotId, string FirmName)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            string WhereClause = string.Empty;

            if (OrderType > -1)
                WhereClause += " AND A.IsStockTransfer=" + OrderType;

            if (OrderType == 0 && !string.IsNullOrEmpty(FirmName))
                WhereClause += " AND A.FirmName like '%" + FirmName + "%'";

            if (OrderType == 1 && DepotId > 0)
                WhereClause += " A.DepotId=" + DepotId;

            if (OrderForPlantId > 0)
                WhereClause += " AND A.PlantId=" + OrderForPlantId;

            if (ProductId > 0)
                WhereClause += " AND A.ProductId=" + ProductId;

            if (!string.IsNullOrEmpty(OrderDate))
                WhereClause += " AND CONVERT(NVARCHAR(200),A.OrderDate,103) ='" + OrderDate + "'";

            List<StockTransferSaleOrderOnPlantByHqEntity> obj = new List<StockTransferSaleOrderOnPlantByHqEntity>();
            DataTable dt = objStockModel.StockTransferSalesOrderReportMIS(WhereClause); // Search Result for Stock Transfer and Sales Order

            if (!(dt != null && dt.Rows.Count > 0))
                return Json("");

            foreach (var item in dt.AsEnumerable())
            {
                StockTransferSaleOrderOnPlantByHqEntity temp = new StockTransferSaleOrderOnPlantByHqEntity();

                temp.IdStockTransferSaleOrderOnPlantByHQ = item.Field<int>("IdStockTransferSaleOrderOnPlantByHQ");

                temp.PlantId = item.Field<int>("PlantId");
                temp.PlantDetails = new ProcessingUnitEntity()
                {
                    ProcessingUnitId = item.Field<int>("PlantId"),
                    ProcessingUnitNameEng = item.Field<string>("ProcessingUnitNameEng"),
                    ProcessingUnitNameHi = item.Field<string>("ProcessingUnitNameHi")
                };


                temp.DepotId = item.Field<int>("DepotId");
                temp.DepotDetails = new DepotMasterEntity()
                {
                    DepotId = item.Field<int>("DepotId"),
                    DepotNameEng = item.Field<string>("DepotNameEng"),
                    DepotNameHi = item.Field<string>("DepotNameHi")
                };

                temp.FirmName = item.Field<string>("FirmName");

                temp.ProductId = item.Field<int>("ProductId");
                temp.ProductDetails = new ProductEntity()
                {
                    ProductId = item.Field<int>("ProductId"),
                    ProductNameEng = item.Field<string>("ProductNameEng"),
                    ProductNameHi = item.Field<string>("ProductNameHi")
                };


                temp.OrderDate = Convert.ToDateTime(item.Field<string>("OrderDate"));
                temp.IsStockTransfer = item.Field<bool>("IsStockTransfer");
                temp.IsInTransit = item.Field<bool>("IsInTransit");
                temp.TransitDate = Convert.ToDateTime(item.Field<string>("TransitDate"));
                temp.OrderTakenDateTimeByDepot = Convert.ToDateTime(item.Field<string>("OrderTakenDateTimeByDepot"));

                var json = JsonConvert.SerializeObject(StockTransferSalesOrderDetails(item.Field<int>("IdStockTransferSaleOrderOnPlantByHQ"), item.Field<int>("ProductId")));
                temp.OrderDetailShow = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());

                obj.Add(temp);
            }

            return Json(obj);
        }

        public ActionResult StockTransferSalesOrderReport()
        {
            return View();
        }

        public JsonResult GetPriceListByProductAndDateWise(int ProductId, string date)
        {
            if (!DataFormatter.IsValidDateTime(date))
                return Json(new { Result = false, data = "date is not in correct format i.e. dd/mm/yyyy" }); // Validation failed. No Action required

            StockModel objStockModel = new StockModel(new DBLogger());
            InitialStockEntity objInitialStockEntity = new InitialStockEntity();

            var json = JsonConvert.SerializeObject(this.CaptureInitialStockByProductId(ProductId, false));
            objInitialStockEntity = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]).ToString());



            DataTable dt = objStockModel.GetPriceListByProductDateWise(ProductId, date);

            if (dt != null && dt.Rows.Count > 0)
            {

                var temp2 = dt.AsEnumerable().Select(p => new
                {
                    ProductId = p.Field<int>("ProductId"),
                    ProductSubcategoryId = p.Field<int>("ProductSubcategoryId"),
                    PackagingId = p.Field<int>("PackagingId"),
                    Quantity = p.Field<decimal>("Quantity")
                }).ToList();

                var tempProductSubCategories = temp2.Select(q => q.ProductSubcategoryId).Distinct();

                foreach (var item in tempProductSubCategories)
                {
                    var t = objInitialStockEntity.ProductSubCategoryPackingMappingList.Where(p => p.ProductSubCategoryId == item).FirstOrDefault();

                    if (t != null)
                    {
                        foreach (var item2 in temp2.Where(q => q.ProductSubcategoryId == item))
                        {
                            if (t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).FirstOrDefault() != null)
                            {
                                t.ProductSubCategoryQuantityEntity.Where(m => m.ProductSubCategoryId == item && m.PackingId == item2.PackagingId).First().Price = item2.Quantity;
                            }
                        }
                    }
                }
            }

            return Json(new { Result = true, data = objInitialStockEntity });
        }

        [HttpPost]
        public JsonResult InsertDailyPrice(string date, InitialStockEntity model)
        {
            if (!DataFormatter.IsValidDateTime(date))
                return Json(new { Result = false });

            StockModel objStockModel = new StockModel(new DBLogger());
            PriceMasterEntity temp = new PriceMasterEntity();
            int createdby = Convert.ToInt32(_loggedInUSerDetails.UserId);
            string createdIP = Controllers.COMMON_FUNCTION.GetIpAddress();

            foreach (var item in model.ProductSubCategoryPackingMappingList)
            {
                foreach (var item2 in item.ProductSubCategoryQuantityEntity)
                {
                    if (item2.Price > 0)
                    {
                        temp = new PriceMasterEntity() { Date = date, SubProductId = item2.ProductSubCategoryId, Price = item2.Price, PackingId = item2.PackingId, CreatedBy = createdby, CreatedIP = createdIP };
                        int result = objStockModel.InsertPriceMaster(temp);
                    }
                }
            }

            return Json(new { Result = true });
        }


        public ActionResult DailyPriceReport()
        {
            return View();
        }


        [HttpPost]
        public JsonResult DailyPriceReportMIS(int ProductId, string FromDate, string ToDate)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            string WhereClause = string.Empty;

            string message = "";

            if (!DataFormatter.IsValidDateTime(FromDate))
                message += "From date is not in correct format";

            if (!DataFormatter.IsValidDateTime(ToDate))
                message += "To date is not in correct format";

            if (!string.IsNullOrEmpty(message))
                return Json(new { Result = false, data = message }); // Validation failed. No Action required

            DateTime parsedDateTime;

            string[] formats = { "dd/MM/yyyy" };
            DateTime fromdate, todate;

            DateTime.TryParseExact(FromDate, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDateTime);
            fromdate = parsedDateTime;

            DateTime.TryParseExact(ToDate, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDateTime);
            todate = parsedDateTime;


            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = fromdate <= todate ? fromdate : todate; date <= (fromdate >= todate ? fromdate : todate); date = date.AddDays(1))
                allDates.Add(date);


            List<InitialStockEntity> finaldata = new List<InitialStockEntity>();

            for (int i = 0; i < allDates.Count; i++)
            {
                var json = JsonConvert.SerializeObject(GetPriceListByProductAndDateWise(ProductId, allDates[i].ToString("dd/MM/yyy").Replace("-","/")));
                InitialStockEntity temp = new InitialStockEntity();
                temp = JsonConvert.DeserializeObject<InitialStockEntity>(((JObject.Parse(json))["Data"]["data"]).ToString());
                temp.StockDate = allDates[i].ToString("dd/MM/yyy");
                finaldata.Add(temp);
            }

            return Json(new { Result = true, data = finaldata });
        }

        public ActionResult DailySalesReport()
        {
            return View();
        }
        

        public ActionResult GetEncValue()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CalculateEncValue(string plaintTextVal)
        {
            if (string.IsNullOrEmpty(plaintTextVal))
                return Json(new { Result = false, Message = "Please Provide Value first for Encryption" });
            else
                return Json(new { Result = true, Message = COMMON_FUNCTION.Encrypt(plaintTextVal) });
        }

        [HttpPost]
        public JsonResult CalculateDecryptValue(string encVal)
        {
            if (string.IsNullOrEmpty(encVal))
                return Json(new { Result = false, Message = "Please Provide Value first for Decryption" });
            else
                return Json(new { Result = true, Message = COMMON_FUNCTION.Decrypt(encVal) });
        }

        #endregion

        public ActionResult CommodityMaster()
        {
            return View();
        }



        [HttpPost]
        public JsonResult InsertCommodityMaster(CommodityMasterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            int Result = objStockModel.CommodityMasterInsert(model);
            return Json(new { Result = true });

        }

        public JsonResult GetCommodityMaster(DataPagingModel Details)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            DataTable DT = objStockModel.GetCommodityMasterList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult InsertCommodityPrice(CommodityMasterPrice model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            int Result = objStockModel.InsertCommodityPrice(model);
            return Json(new { Result = true });

        }
        public JsonResult GetCommodityPriceList(int CommodityId)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            DataTable DT = objStockModel.GetCommodityPriceList(CommodityId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

        }


        //bhu

        public ActionResult PurchaseCenter()
        {
            return View();
        }


        [HttpPost]
        public JsonResult InsertPurchaseCenter(PurchaseCenterEntity model)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            int Result = objStockModel.InsertPurchaseCenterMaster(model);
            return Json(new { Result = true });

        }


        public JsonResult GetPurchaseCenter(DataPagingModel Details, int DISTRICT_ID = 0)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            DataTable DT = objStockModel.GetPurchaseCenterMaster(ref Details, DISTRICT_ID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DailyWheatPurchase()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveDailyWheatPurchase(DailyWheatPurchase Modeldata)
        {
            string result = "";
            StockModel objStockModel = new StockModel(new DBLogger());
            int createdby = Convert.ToInt32(_loggedInUSerDetails.UserId);
            result = objStockModel.InsertDailyWheatPurchase(Modeldata, createdby);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetDailyWheatPurchase(DataPagingModel Details)
        {
            StockModel objStockModel = new StockModel(new DBLogger());
            DataTable dt = new DataTable();
            DataTable DT = objStockModel.GetDailyWheatPurchase(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

    }
}