using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class StockModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public StockModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable ProcessingUnitList(int ProcessingUnitId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProcessingUnitList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, ProcessingUnitId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable ProcessingUnitList(int ProcessingUnitId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ProductList(int ProductId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, ProductId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable ProductList(int ProductId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ProductSubCategoryList(int ProductSubCategoryId, int ProductId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductSubCategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, ProductSubCategoryId);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.NVarchar, ProductId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable ProductSubCategoryList(int ProductSubCategoryId,int ProductId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable PackingList(int PackingId, int ProductId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PackingList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PackingId", DBFactoryParameterType.Int, PackingId);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.NVarchar, ProductId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable PackingList(int PackingId, int ProductId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable RejectionReasonList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RejectionReasonList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable RejectionReasonList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable UnitMasterList(int UnitId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UnitMasterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, UnitId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable UnitMasterList(int UnitId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertInitialStock(InitialStockEntity model, int CreatedBy)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            Dictionary<int, int> Calculator = new Dictionary<int, int>();
            List<PackingEntity> Packing = this.PackingList(model.ProductId);
            try
            {
                objDBAccess.OpenConnection();
                foreach (var item in model.ProductSubCategoryPackingMappingList)
                {
                    foreach (var temp in item.ProductSubCategoryQuantityEntity)
                    {
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertInitialStock", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@PlantDepotId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                        objDBAccess.AddParameterToCommand("@IsPlant", DBFactoryParameterType.Bit, model.IsPlant);
                        objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                        objDBAccess.AddParameterToCommand("@ProductSubcategoryId", DBFactoryParameterType.Int, temp.ProductSubCategoryId);
                        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, temp.PackingId);
                        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Int, temp.Quantity);
                        //objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.Int, model.YearId);
                        //objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, model.MonthId);
                        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, IP);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                        if (Result > 0)
                        {
                            if (Calculator.Keys.Count() == 0 || !Calculator.ContainsKey(temp.ProductSubCategoryId))
                            {
                                int a = Packing.Where(p => p.PackingId == temp.PackingId).Select(q => q.UnitDetails.UnitConvertMultiplier).FirstOrDefault()
                                    * Packing.Where(p => p.PackingId == temp.PackingId).Select(q => q.MeasurementCount).FirstOrDefault()
                                    * temp.Quantity;
                                Calculator.Add(temp.ProductSubCategoryId, a);
                            }
                            else
                            {
                                int a = Packing.Where(p => p.PackingId == temp.PackingId).Select(q => q.UnitDetails.UnitConvertMultiplier).FirstOrDefault()
                                    * Packing.Where(p => p.PackingId == temp.PackingId).Select(q => q.MeasurementCount).FirstOrDefault()
                                    * temp.Quantity;

                                Calculator[temp.ProductSubCategoryId] = Calculator[temp.ProductSubCategoryId] + a;
                            }
                        }
                    }
                }

                foreach (var item in Calculator)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("PlantStockDetailsInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PlantId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                    objDBAccess.AddParameterToCommand("@IsStockTransfer", DBFactoryParameterType.Bit, false);
                    objDBAccess.AddParameterToCommand("@IsStockSaleOrderByHQ", DBFactoryParameterType.Bit, false);
                    objDBAccess.AddParameterToCommand("@IsStockSendByHq", DBFactoryParameterType.Bit, false);
                    objDBAccess.AddParameterToCommand("@StockTransferSaleOrderId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@StockTransferOrderId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@StockSaleTransferId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, item.Key);
                    objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Money, 0);
                    objDBAccess.AddParameterToCommand("@StockInQuantity", DBFactoryParameterType.Money, item.Value);
                    objDBAccess.AddParameterToCommand("@StockOutQuantity", DBFactoryParameterType.Money, 0);
                    objDBAccess.AddParameterToCommand("@StockBalanceQuantity", DBFactoryParameterType.Money, item.Value);

                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }

                Result = !(Result == -2 || Result == -1) ? 1 : Result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertInitialStock(InitialStockEntity model, int CreatedBy)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int DepotSalesRegisterInitialEnty(InitialStockEntity model, int CreatedBy)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            DataPagingModel details = new DataPagingModel() { PageSize = 1, PageNumber = 1 };
            DataTable priceData = this.GetPriceMasterList(ref details);

            var DailyProductSalePriceList = priceData.AsEnumerable().Select(p => new
            {
                ProductSubCategoryId = p.Field<int>("sub_product_id"),
                UnitId = p.Field<int>("unit_id"),
                Quantity = p.Field<int>("quantity"),
                Price = p.Field<decimal>("price"),
                SellingDate = p.Field<DateTime>("Date")
            });

            try
            {
                objDBAccess.OpenConnection();

                foreach (var item in model.ProductSubCategoryPackingMappingList)
                {
                    foreach (var temp in item.ProductSubCategoryQuantityEntity)
                    {
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertInitialStock", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@PlantDepotId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                        objDBAccess.AddParameterToCommand("@IsPlant", DBFactoryParameterType.Bit, model.IsPlant);
                        objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                        objDBAccess.AddParameterToCommand("@ProductSubcategoryId", DBFactoryParameterType.Int, temp.ProductSubCategoryId);
                        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, temp.PackingId);
                        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Int, temp.Quantity);
                        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, IP);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }

                foreach (var item in model.ProductSubCategoryPackingMappingList)
                {
                    foreach (var temp in item.ProductSubCategoryQuantityEntity)
                    {
                        var salesRate = DailyProductSalePriceList.Where(p => p.ProductSubCategoryId == temp.ProductSubCategoryId).Select(m => m.Price).FirstOrDefault();

                        SqlCommand cmd = objDBAccess.CreateCommand("SalesRegisterInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                        objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                        objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, temp.ProductSubCategoryId);
                        objDBAccess.AddParameterToCommand("@PackagingId", DBFactoryParameterType.Int, temp.PackingId);
                        objDBAccess.AddParameterToCommand("@AvailableQuantity", DBFactoryParameterType.Int, temp.Quantity);
                        objDBAccess.AddParameterToCommand("@SaleQuantity", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@BalanceQuantity", DBFactoryParameterType.Int, temp.Quantity);
                        objDBAccess.AddParameterToCommand("@SellingPrice", DBFactoryParameterType.Money, salesRate);
                        objDBAccess.AddParameterToCommand("@SalesDate", DBFactoryParameterType.VARCHAR, DateTime.Now.ToString("MM/dd/yyyy"));
                        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, IP);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int SalesRegisterEnty(DepotSalesRegisterEntity model, int CreatedBy)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int SalesRegisterEnty(DepotSalesRegisterEntity model, int CreatedBy)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            DataPagingModel details = new DataPagingModel() { PageSize = 1, PageNumber = 1 };
            DataTable priceData = this.GetPriceMasterList(ref details);

            var DailyProductSalePriceList = priceData.AsEnumerable().Select(p => new
            {
                ProductSubCategoryId = p.Field<int>("sub_product_id"),
                UnitId = p.Field<int>("unit_id"),
                Quantity = p.Field<int>("quantity"),
                Price = p.Field<decimal>("price"),
                SellingDate = p.Field<DateTime>("Date")
            });

            try
            {
                objDBAccess.OpenConnection();

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

                        var salesRate = DailyProductSalePriceList.Where(p => p.ProductSubCategoryId == temp.ProductSubCategoryId).Select(m => m.Price).FirstOrDefault();

                        SqlCommand cmd = objDBAccess.CreateCommand("SalesRegisterInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, model.DepotId);//
                        objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                        objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, temp.ProductSubCategoryId);
                        objDBAccess.AddParameterToCommand("@PackagingId", DBFactoryParameterType.Int, temp.PackingId);
                        objDBAccess.AddParameterToCommand("@AvailableQuantity", DBFactoryParameterType.Int, availableQuantity);
                        objDBAccess.AddParameterToCommand("@SaleQuantity", DBFactoryParameterType.Int, temp.Quantity);
                        objDBAccess.AddParameterToCommand("@BalanceQuantity", DBFactoryParameterType.Int, (availableQuantity - temp.Quantity));
                        objDBAccess.AddParameterToCommand("@SellingPrice", DBFactoryParameterType.Money, salesRate);
                        objDBAccess.AddParameterToCommand("@SalesDate", DBFactoryParameterType.VARCHAR, Convert.ToDateTime( model.SalesDate).ToString("MM/dd/yyyy"));
                        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, IP);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int SalesRegisterEnty(DepotSalesRegisterEntity model, int CreatedBy)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int InsertStockPurchase(PurchaseRegisterEntity model)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockPurchaseInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                objDBAccess.AddParameterToCommand("@PartyName", DBFactoryParameterType.VARCHAR, model.PartyName);
                objDBAccess.AddParameterToCommand("@PartyNameHi", DBFactoryParameterType.NVarchar, model.PartyNameHi);
                objDBAccess.AddParameterToCommand("@BrokerName", DBFactoryParameterType.VARCHAR, model.BrokerName);
                objDBAccess.AddParameterToCommand("@BrokerNameHi", DBFactoryParameterType.NVarchar, model.BrokerNameHi);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, model.ProductSubCategoryId);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, model.UnitId);
                objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Money, model.Quantity);
                objDBAccess.AddParameterToCommand("@Rate", DBFactoryParameterType.Money, model.Rate);
                objDBAccess.AddParameterToCommand("@RateUnitId", DBFactoryParameterType.Int, model.RateUnitId);
                objDBAccess.AddParameterToCommand("@RateQuantity", DBFactoryParameterType.Int, model.RateQuantity);
                objDBAccess.AddParameterToCommand("@PurchasePaymentDate", DBFactoryParameterType.DateTime, model.PurchasePaymentDate);
                objDBAccess.AddParameterToCommand("@ExpectedDeliveryDate", DBFactoryParameterType.DateTime, model.ExpectedDeliveryDate);
                objDBAccess.AddParameterToCommand("@SaleContractNo", DBFactoryParameterType.NVarchar, model.SaleContractNo);
                objDBAccess.AddParameterToCommand("@Conditions", DBFactoryParameterType.NVarchar, model.Conditions);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, model.Remark);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, IP);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    foreach (var item in model.StockDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("StockPurchaseDetailInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@PurchaseRegisterId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.NVarchar, model.UnitId);
                        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.NVarchar, item.Quantity);
                        objDBAccess.AddParameterToCommand("@VehicleNo", DBFactoryParameterType.NVarchar, item.VehicleNo);
                        int detailId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertStockPurchase(PurchaseRegisterEntity model)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable StockPurchaseRequestList(int ProcessingUnitId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockPurchaseRequestList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, ProcessingUnitId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockPurchaseRequestList(int ProcessingUnitId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int StockPurchaseDetailsResponseSave(PurchaseRegisterDetailEntity model, int ActionId)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockPurchaseDetailsResponse", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PurchaseRegisterDetailId", DBFactoryParameterType.Int, model.PurchaseRegisterDetailId);
                objDBAccess.AddParameterToCommand("@IsQualityCheckOkay", DBFactoryParameterType.Bit, model.IsQualityCheckOkay);
                objDBAccess.AddParameterToCommand("@IsAccepted", DBFactoryParameterType.Bit, model.IsAccepted);
                objDBAccess.AddParameterToCommand("@File", DBFactoryParameterType.VARCHAR, model.AcceptanceFile);
                objDBAccess.AddParameterToCommand("@RejectionId", DBFactoryParameterType.Int, model.RejectionId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, model.HQUserId);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, IP);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, model.RemarkByHQ);
                objDBAccess.AddParameterToCommand("@IsDebitNote", DBFactoryParameterType.Bit, model.IsDebitNote);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && ActionId == 2) // After Succesfull Action By Hq set value in Plant Stock Details entry
                {
                    DataTable dt2 = this.StockPurchaseDetailById(0, model.PurchaseRegisterDetailId);

                    if (dt2 != null && dt2.Rows.Count > 0 && model.IsDebitNote == false)
                    {
                        int UnitId = (dt2.Rows[0]).Field<int>("UnitId");
                        decimal quantity = (dt2.Rows[0]).Field<decimal>("Quantity");

                        DataTable dt3 = this.UnitMasterList(0);

                        SqlCommand cmd2 = objDBAccess.CreateCommand("PlantStockDetailsInsert", DBFactoryCommandType.PROCEDURE_COMMAND);


                        int multiplier = dt3.AsEnumerable().Where(p => p.Field<int>("UnitId") == UnitId).Select(m => m.Field<int>("UnitConvertMultiplier")).FirstOrDefault();

                        var totalQuantity = multiplier * quantity;
                        objDBAccess.OpenConnection();
                        objDBAccess.AddParameterToCommand("@PlantId", DBFactoryParameterType.Int, model.PlantId);
                        objDBAccess.AddParameterToCommand("@IsStockTransfer", DBFactoryParameterType.Bit, false);
                        objDBAccess.AddParameterToCommand("@IsStockSaleOrderByHQ", DBFactoryParameterType.Bit, false);
                        objDBAccess.AddParameterToCommand("@IsStockSendByHq", DBFactoryParameterType.Bit, true);
                        objDBAccess.AddParameterToCommand("@StockTransferSaleOrderId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@StockTransferOrderId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@StockSaleTransferId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, model.ProductSubCategoryId);
                        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Money, 0);
                        objDBAccess.AddParameterToCommand("@StockInQuantity", DBFactoryParameterType.Money, totalQuantity);
                        objDBAccess.AddParameterToCommand("@StockOutQuantity", DBFactoryParameterType.Money, 0);
                        objDBAccess.AddParameterToCommand("@StockBalanceQuantity", DBFactoryParameterType.Money, totalQuantity);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int StockPurchaseDetailsResponseSave(PurchaseRegisterDetailEntity model,int ActionId)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProcessingUnitInsertUpdate(ProcessingUnitEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProcessingUnitInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, model.ProcessingUnitId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@ProcessingUnitNameEng", DBFactoryParameterType.VARCHAR, model.ProcessingUnitNameEng);
                objDBAccess.AddParameterToCommand("@ProcessingUnitNameHi", DBFactoryParameterType.NVarchar, model.ProcessingUnitNameHi);
                objDBAccess.AddParameterToCommand("@IsOperating", DBFactoryParameterType.Bit, model.IsOperating);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProcessingUnitInsertUpdate(ProcessingUnitEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProcessingUnitSetStatus(int ProcessingUnitId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProcessingUnitSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, ProcessingUnitId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProcessingUnitSetStatus(int ProcessingUnitId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProductInsertUpdate(ProductEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                objDBAccess.AddParameterToCommand("@ProductNameEng", DBFactoryParameterType.VARCHAR, model.ProductNameEng);
                objDBAccess.AddParameterToCommand("@ProductNameHi", DBFactoryParameterType.NVarchar, model.ProductNameHi);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProductInsertUpdate(ProductEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProductSetStatus(int ProductId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, ProductId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProductSetStatus(int ProductId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProductSubCategoryInsertUpdate(ProductSubCategoryEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductSubCategoryInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, model.ProductSubCategoryId);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryNameEng", DBFactoryParameterType.VARCHAR, model.ProductSubCategoryNameEng);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryNameHi", DBFactoryParameterType.NVarchar, model.ProductSubCategoryNameHi);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProductSubCategoryInsertUpdate(ProductSubCategoryEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int ProductSubCategorySetStatus(int ProductSubCategoryId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProductSubCategorySetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, ProductSubCategoryId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int ProductSubCategorySetStatus(int ProductSubCategoryId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int UnitMasterInsertUpdate(UnitMasterEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UnitMasterInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, model.UnitId);
                objDBAccess.AddParameterToCommand("@UnitNameEng", DBFactoryParameterType.VARCHAR, model.UnitNameEng);
                objDBAccess.AddParameterToCommand("@UnitNameHi", DBFactoryParameterType.NVarchar, model.UnitNameHi);
                objDBAccess.AddParameterToCommand("@UnitConvertMultiplier", DBFactoryParameterType.Int, model.UnitConvertMultiplier);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int UnitMasterInsertUpdate(UnitMasterEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int UnitMasterSetStatus(int UnitId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UnitMasterSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, UnitId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int UnitMasterSetStatus(int UnitId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int RejectReasonInsertUpdate(RejectReasonEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RejectReasonInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RejectionId", DBFactoryParameterType.Int, model.RejectionId);
                objDBAccess.AddParameterToCommand("@RejectionReasonEng", DBFactoryParameterType.VARCHAR, model.RejectionReasonEng);
                objDBAccess.AddParameterToCommand("@RejectionReasonHi", DBFactoryParameterType.NVarchar, model.RejectionReasonHi);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int RejectReasonInsertUpdate(RejectReasonEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int RejectReasonSetStatus(int RejectionId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RejectReasonSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RejectionId", DBFactoryParameterType.Int, RejectionId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int RejectReasonSetStatus(int RejectionId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int PackingInsertUpdate(PackingEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PackingInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PackingId", DBFactoryParameterType.Int, model.PackingId);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                objDBAccess.AddParameterToCommand("@PackingNameEng", DBFactoryParameterType.VARCHAR, model.PackingNameEng);
                objDBAccess.AddParameterToCommand("@PackingNameHi", DBFactoryParameterType.NVarchar, model.PackingNameHi);
                objDBAccess.AddParameterToCommand("@MeasurementCount", DBFactoryParameterType.Int, model.MeasurementCount);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, model.UnitId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Bit, model.ActionBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int PackingInsertUpdate(PackingEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int PackingSetStatus(int PackingId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PackingSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PackingId", DBFactoryParameterType.Int, PackingId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int PackingSetStatus(int PackingId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        //public int InsertStockDemandByDepot(StockDemandByDepotTilamSanghEntity model)
        //{
        //    int Result = 0;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("InsertStockDemandByDepot", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@StockDemandId", DBFactoryParameterType.Int, model.StockDemandId);
        //        objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.Int, model.ProcessingUnitId);
        //        objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
        //        objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, model.ProductSubCategoryId);
        //        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, model.UnitId);
        //        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Money, model.Quantity);
        //        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, model.CreateBy);
        //        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertStockDemandByDepot(StockDemandByDepotTilamSanghEntity model)" }, ex);
        //        Result = -1;
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return Result;
        //}

        public DataTable StockTransferSalesOrderByHqToPlantList(int UserRoleDepartmentMappingId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockTransferSalesOrderByHqToPlantList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentMappingId", DBFactoryParameterType.Int, UserRoleDepartmentMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockTransferSalesOrderByHqToPlantList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int DemandSetStatus(int DemandId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DemandSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DemandId", DBFactoryParameterType.Int, DemandId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int DemandSetStatus(int DemandId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable LiabilityAssetListSocietyWise(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiabilityAssetListSocietyWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable LiabilityAssetListSocietyWise(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int InsertPriceMaster(PriceMasterEntity model)
        {
            int Result = 0;
            try
            {
                string date = "01-01-2000";
                if (!string.IsNullOrWhiteSpace(model.Date))
                {
                    date = model.Date.Split('/')[2] + "-" + model.Date.Split('/')[1] + "-" + model.Date.Split('/')[0];
                }
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[Controllers.SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_InsertPriceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Price_Id", DBFactoryParameterType.Int, model.PriceId);
                objDBAccess.AddParameterToCommand("@SubProduct_Id", DBFactoryParameterType.Int, model.SubProductId);
                //objDBAccess.AddParameterToCommand("@Unit_Id", DBFactoryParameterType.Int, model.UnitId);
                //objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Int, model.Quantity);
                objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.Int, model.Price);
                objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.Date, date);
                objDBAccess.AddParameterToCommand("@CraetedBy", DBFactoryParameterType.Int, model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, model.CreatedIP);
                objDBAccess.AddParameterToCommand("@PackingId", DBFactoryParameterType.VARCHAR, model.PackingId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                Result = 3;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }
        public DataTable GetPriceMasterList(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (type == "date")
                    {
                        var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                        var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                        string whereDate = @" and   convert(date,date,111) ";
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
                        whereCase += " and REPLACE(lower(rtrim(ltrim(" + type + "))),'  ',' ')  like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("TilamGetPriceList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("TilamGetPriceList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();



                //string whereCase = " ";                
                //SqlCommand cmd = new SqlCommand();
                //objDBAccess.OpenConnection();               
                //cmd = objDBAccess.CreateCommand("RajfedGetTransportList", DBFactoryCommandType.PROCEDURE_COMMAND);                
                //resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.FertilizerSupplyFectory", FunctionName = "DataTable GetTransportList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        //public DataTable DepotList()
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("DepotList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable DepotList()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}


        public int InsertStockTransferSaleOrderByHqToPlant(StockTransferSaleOrderOnPlantByHqEntity model)
        {
            int Result = 0;
            string IP = Controllers.COMMON_FUNCTION.GetIpAddress();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockTransferSaleOrderByHqToPlantInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PlantId", DBFactoryParameterType.Int, model.PlantId);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, model.DepotId);
                objDBAccess.AddParameterToCommand("@FirmName", DBFactoryParameterType.NVarchar, model.FirmName);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Int, model.ProductId);
                objDBAccess.AddParameterToCommand("@OrderDate", DBFactoryParameterType.DateTime, model.OrderDate);
                objDBAccess.AddParameterToCommand("@IsStockTransfer", DBFactoryParameterType.Bit, model.IsStockTransfer);
                objDBAccess.AddParameterToCommand("@Createby", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, IP);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    foreach (var item in model.OrderDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("StockTransferSaleOrderByHqToPlantDetailInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@IdStockTransferSaleOrderOnPlantByHQ", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@PackagingId", DBFactoryParameterType.Int, item.PackagingId);
                        objDBAccess.AddParameterToCommand("@Quantity", DBFactoryParameterType.Int, item.Quantity);
                        objDBAccess.AddParameterToCommand("@ProductSubCategoryId", DBFactoryParameterType.Int, item.ProductSubCategoryId);
                        int detailId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertStockTransferSaleOrderByHqToPlant(StockTransferSaleOrderOnPlantByHqEntity model)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable InitialStockPlantDepotList(int PlantDepotId, bool IsPlant, int ProductId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_InitialStockPlantDepotList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PlantDepotIdId", DBFactoryParameterType.Int, PlantDepotId);
                objDBAccess.AddParameterToCommand("@IsPlant", DBFactoryParameterType.Bit, IsPlant);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.Bit, ProductId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable InitialStockPlantDepotList(int PlantDepotId,bool IsPlant, int ProductId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StockPurchaseDetail(int UserRoleDepartmentMappingId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_StockPurchaseDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentMappingId", DBFactoryParameterType.Int, UserRoleDepartmentMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockPurchaseDetail(int UserRoleDepartmentMappingId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StockPurchaseDetailById(int PurchaseRegisterId, int PurchaseRegisterDetailId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_StockPurchaseDetailSingleEntry", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PurchaseRegisterId", DBFactoryParameterType.Int, PurchaseRegisterId);
                objDBAccess.AddParameterToCommand("@PurchaseRegisterDetailId", DBFactoryParameterType.Int, PurchaseRegisterDetailId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockPurchaseDetailById(int PurchaseRegisterId,int PurchaseRegisterDetailId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StockTransferSalesOrderDetails(int Id)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_StockTransferSalesOrderDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockTransferSalesOrderDetails(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int StockTransferSalesOrderDelivery(int Id, int UserId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_StockTransferSalesOrderDelivery", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IdStockTransferSaleOrderOnPlantByHQ", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int StockTransferSalesOrderDelivery(int Id,int UserId)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable DepotAvailableStockList(int DepotId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_DepotAvailableStockList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, DepotId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable DepotAvailableStockList(int DepotId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int DepotInsertUpdate(DepotMasterEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_DepotInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, model.DepotId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@DepotNameEng", DBFactoryParameterType.VARCHAR, model.DepotNameEng);
                objDBAccess.AddParameterToCommand("@DepotNameHi", DBFactoryParameterType.NVarchar, model.DepotNameHi);
                objDBAccess.AddParameterToCommand("@AgentNameEng", DBFactoryParameterType.VARCHAR, model.AgentNameEng);
                objDBAccess.AddParameterToCommand("@AgentNameHi", DBFactoryParameterType.NVarchar, model.AgentNameHi);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int DepotInsertUpdate(DepotMasterEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int DepotSetStatus(int DepotId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_DepotSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, DepotId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int DepotSetStatus(int DepotId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable DepotList(int DepotId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepotList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.Int, DepotId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable DepotList(int DepotId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet TilamSanghDashboardData(int UserRoleDepartmentMappingId)
        {
            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_DashboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDepartmentMappingId", DBFactoryParameterType.Int, UserRoleDepartmentMappingId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataSet TilamSanghDashboardData(int UserRoleDepartmentMappingId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dset;
        }

        public List<PackingEntity> PackingList(int ProductId)
        {
            List<PackingEntity> data = new List<PackingEntity>();
            try
            {
                dt.Clear();
                dt = this.PackingList(0, ProductId);

                if (dt == null || dt.Rows.Count < 1)
                    return data;

                foreach (var item in dt.AsEnumerable())
                {
                    PackingEntity temp = new PackingEntity();
                    temp.PackingId = item.Field<int>("PackingId");

                    temp.ProductId = item.Field<int>("ProductId");
                    temp.ProductDetails = new ProductEntity()
                    {
                        ProductId = item.Field<int>("ProductId"),
                        ProductNameEng = item.Field<string>("ProductNameEng"),
                        ProductNameHi = item.Field<string>("ProductNameHi")
                    };

                    temp.UnitId = item.Field<int>("UnitId");
                    temp.UnitDetails = new UnitMasterEntity()
                    {
                        UnitId = item.Field<int>("UnitId"),
                        UnitNameEng = item.Field<string>("UnitNameEng"),
                        UnitNameHi = item.Field<string>("UnitNameHi"),
                        UnitConvertMultiplier = item.Field<int>("UnitConvertMultiplier")
                    };

                    temp.MeasurementCount = item.Field<int>("MeasurementCount");
                    temp.PackingNameEng = item.Field<string>("PackingNameEng");
                    temp.PackingNameHi = item.Field<string>("PackingNameHi");
                    temp.IsActive = item.Field<bool>("IsActive");
                    temp.IsForAll = item.Field<bool>("IsForAll");
                    temp.PackingNameEng = item.Field<string>("PackingNameEng");
                    data.Add(temp);
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable PackingList(int PackingId, int ProductId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return data;
        }
        public DataTable ProcessingUnitList(string WhereCon)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ProcessingUnitListByCondition", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCon", DBFactoryParameterType.NVarchar, WhereCon);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable ProcessingUnitList(string WhereCon)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable DepotList(string WhereCon)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepotListByCondition", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCon", DBFactoryParameterType.NVarchar, WhereCon);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable ProcessingUnitList(string WhereCon)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable StockPurchaseReportMIS(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_StockPurchaseReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable StockPurchaseReportMIS(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StockTransferSalesOrderReportMIS(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StockTransferSalesOrderReportList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = " public DataTable StockTransferSalesOrderByHqToPlantList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetPriceListByProductDateWise(int ProductId, string PriceDate)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_GetPriceListByProductDateWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductId", DBFactoryParameterType.NVarchar, ProductId);
                objDBAccess.AddParameterToCommand("@PriceDate", DBFactoryParameterType.NVarchar, PriceDate.Split('/')[1]+"/" + PriceDate.Split('/')[0] + "/" + PriceDate.Split('/')[2]);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public DataTable GetPriceListByProductDateWise(int ProductId, string PriceDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public int CommodityMasterInsert(CommodityMasterEntity model)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[Controllers.SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_insert_CommodityMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (model.Flag == 1 || model.Flag == 2)
                {
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@CommodityNameEn", DBFactoryParameterType.NVarchar, model.CommodityNameEn);
                    objDBAccess.AddParameterToCommand("@CommodityNameHi", DBFactoryParameterType.NVarchar, model.CommodityNameHi);
                    objDBAccess.AddParameterToCommand("@IsKharif", DBFactoryParameterType.Bit, model.IsKharif);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, _LoggedInUser.CreatedIP);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                }
                else if (model.Flag == 3)
                {
                    model.IsActive = (model.IsActive == false ? true : (model.IsActive == true ? false : model.IsActive));
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                }
                else if (model.Flag == 4)
                {
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                }


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                Result = 3;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


        public int InsertCommodityPrice(CommodityMasterPrice model)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[Controllers.SessionIdentity.Admin];
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_insert_CommodityPriceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);

                if (model.Flag == 1)
                {
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.NVarchar, 1);
                    objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.NVarchar, model.PriceFAQ);
                    objDBAccess.AddParameterToCommand("@FYEAR_ID", DBFactoryParameterType.Int, model.FinancialYear);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, _LoggedInUser.CreatedIP);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());


                    SqlCommand cmd1 = objDBAccess.CreateCommand("Tilam_insert_CommodityPriceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.NVarchar, 2);
                    objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.NVarchar, model.PriceURS);
                    objDBAccess.AddParameterToCommand("@FYEAR_ID", DBFactoryParameterType.Int, model.FinancialYear);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, _LoggedInUser.CreatedIP);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (model.Flag == 2)
                {

                    objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, model.Msp_Id);
                    objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                    objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.NVarchar, 2);
                    if (model.UnitId == 1)
                    {
                        objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.NVarchar, model.PriceFAQ);
                    }
                    else
                    {
                        objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.NVarchar, model.PriceURS);
                    }
                    objDBAccess.AddParameterToCommand("@FYEAR_ID", DBFactoryParameterType.Int, model.FinancialYear);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, _LoggedInUser.CreatedIP);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (model.Flag == 3)
                {
                    model.IsActive = (model.IsActive == false ? true : (model.IsActive == true ? false : model.IsActive));
                    objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, model.Msp_Id);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (model.Flag == 4)
                {
                    objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, model.Msp_Id);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Int, true);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                Result = 3;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


        public DataTable GetCommodityPriceList(int CommodityId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (CommodityId != 0)
                {
                    whereCase += " And TI.CommodityId='" + CommodityId + "'";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_CommodityPriceList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                resultDt = null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }



        public DataTable GetCommodityMasterList(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";

                //if (Details.SearchParameter.Count > 0)
                //{
                //    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                //    {

                //        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                //        whereCase = " and TC.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' OR TC.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                //    }

                //}

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("Get_TilamCommodityMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("Get_TilamCommodityMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                resultDt = null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }



        public int InsertPurchaseCenterMaster(PurchaseCenterEntity model)
        {
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[Controllers.SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Tilam_insert_PurchaseCenter", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (model.Flag == 2 || model.Flag == 1)
                {
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.Int, model.PurchaseCenterId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterNameEn", DBFactoryParameterType.NVarchar, model.PurchaseCenterNameEn);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterNameHi", DBFactoryParameterType.NVarchar, model.PurchaseCenterNameHi);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@Createdby", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@Createip", DBFactoryParameterType.VARCHAR, _LoggedInUser.CreatedIP);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);

                }
                else if (model.Flag == 3)
                {
                    model.IsActive = (model.IsActive == false ? true : (model.IsActive == true ? false : model.IsActive));
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.Int, model.PurchaseCenterId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, model.IsActive);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                }
                else if (model.Flag == 4)
                {
                    objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.Int, model.PurchaseCenterId);
                    objDBAccess.AddParameterToCommand("@IsDelete", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, model.Flag);
                }

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                Result = 3;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }



        public DataTable GetPurchaseCenterMaster(ref DataPagingModel Details, int DISTRICT_ID = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";
                if (DISTRICT_ID == 0)
                {
                    if (Details.SearchParameter.Count > 0)
                    {
                        if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                        {

                            var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                            whereCase = " and TP.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' OR TP.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                        }
                        if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                        {

                            string DISTRICT_ID1 = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                            whereCase += " And TP.DistrictId='" + DISTRICT_ID1 + "'";

                        }
                    }
                }
                else
                {
                    string DISTRICT_ID1 = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                    whereCase += " And TP.DistrictId='" + DISTRICT_ID + "'";
                }


                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("TilamGetPurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("TilamGetPurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }

            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                resultDt = null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }

        public DataTable GetPurchaseCenterMaster(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {

                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {
                    if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                    {

                        var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                        whereCase = " and TP.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' OR TP.PurchaseCenterNameEn like N'%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'";
                    }
                    if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                    {

                        string DISTRICT_ID = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                        whereCase += " And TP.DistrictId='" + DISTRICT_ID + "'";

                    }
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("TilamGetPurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("TilamGetPurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                resultDt = null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }



        public string InsertDailyWheatPurchase(DailyWheatPurchase model, int createdby)
        {

            string Result = "-1";
            string createdIP = Controllers.COMMON_FUNCTION.GetIpAddress();
            string DailyPurchaseId = "";
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            try
            {
                string Valid_From_Date = "";
                if (!string.IsNullOrWhiteSpace(model.Valid_From_Date))
                {
                    model.Valid_From_Date = model.Valid_From_Date.Replace("-", "/");
                    Valid_From_Date = model.Valid_From_Date.Split('/')[2] + "/" + model.Valid_From_Date.Split('/')[1] + "/" + model.Valid_From_Date.Split('/')[0];
                }
                SqlConnection sqlConnection = conn.GetConnection;
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Insert_DailyWheatPurchase";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Parameters.AddWithValue("@DailyPurchaseId", model.DailyPurchaseId);
                cmd.Parameters.AddWithValue("@CommodityId", model.CommodityId);
                cmd.Parameters.AddWithValue("@DISTRICT_ID", model.DISTRICT_ID);
                cmd.Parameters.AddWithValue("@PurchaseCenter_Id", model.PurchaseCenter_Id);
                cmd.Parameters.AddWithValue("@Valid_From_Date  ", Valid_From_Date);
                cmd.Parameters.AddWithValue("@QuantityNote", string.IsNullOrWhiteSpace(model.QuantityNote) ? "" : model.QuantityNote);
                cmd.Parameters.AddWithValue("@PaymentReleased", model.PaymentReleased);
                cmd.Parameters.AddWithValue("@PendingPayment", model.PendingPayment);
                cmd.Parameters.AddWithValue("@QtySent", model.QtySent);
                cmd.Parameters.AddWithValue("@QtyLying", model.QtyLying);
                cmd.Parameters.AddWithValue("@GunnyBags", model.GunnyBags);
                cmd.Parameters.AddWithValue("@GunnyBagsStock", model.GunnyBagsStock);
                cmd.Parameters.AddWithValue("@NoofFarmersBenefite", model.NoofFarmersBenefite);
                cmd.Parameters.AddWithValue("@WCMRecdtilldate", model.WCMRecdtilldate);
                cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                cmd.Parameters.AddWithValue("@CreatedIP", createdIP);
                DailyPurchaseId = cmd.ExecuteScalar().ToString();

                model.DailyPurchaseId = Convert.ToInt32(DailyPurchaseId);
                Result = DailyPurchaseId;

                model.QualityPricePurchase.ForEach(QualityPricePurchase =>
                {
                    if (!string.IsNullOrWhiteSpace((QualityPricePurchase.Quantity).ToString()))
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "InsertQualityPrice";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@DailyPurchaseQualityId", QualityPricePurchase.DailyPurchaseQualityId);
                        cmd.Parameters.AddWithValue("@DailyPurchaseId", model.DailyPurchaseId);
                        cmd.Parameters.AddWithValue("@QualityID", QualityPricePurchase.QualityID);
                        cmd.Parameters.AddWithValue("@Quantity", QualityPricePurchase.Quantity);
                        cmd.Parameters.AddWithValue("@NoofBags", QualityPricePurchase.NoofBags);
                        cmd.Parameters.AddWithValue("@QualityPrice", QualityPricePurchase.QualityPrice);
                        cmd.Parameters.AddWithValue("@TotalQualityPrice", QualityPricePurchase.TotalQualityPrice);
                        cmd.Parameters.AddWithValue("@CreatedBy", createdby);
                        cmd.Parameters.AddWithValue("@CreatedIP", createdIP);
                        cmd.ExecuteScalar();
                    }
                });
            }
            catch (Exception ex)
            {

            }
            return Result;

        }




        public DataTable GetDailyWheatPurchase(ref DataPagingModel Details)
        {
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = "";
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("Get_DailyWheatPurchaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("Get_DailyWheatPurchaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();




            }

            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.StockModel", FunctionName = "public int InsertPriceMaster(PriceMasterEntity model)" }, ex);
                resultDt = null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return resultDt;
        }



    }


}