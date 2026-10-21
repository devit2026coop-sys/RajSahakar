using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public ActionResult Index()
        {
            return View();
        }
        #region Warehouse


        public ActionResult GenerateWR()
        {
            string Todate = "";
            int Day = -1;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            ViewBag.TodateDate = DateTime.Now.AddDays(-1).AddMonths(3).ToString("dd/MM/yyyy").Replace("-", "/");
            Todate = ViewBag.MinDate = "2019/11/01"; //DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //Todate = ViewBag.MinDate = DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //while (objNew.CheckIsItHoliday(Todate)>0)
                                                     //{
                                                     //    Day = Day - 1;
                                                     //    Todate = ViewBag.MinDate =DateTime.Now.AddDays(Day).ToString("yyy/MM/dd");

            //}

            ViewBag.MinDate = Todate;


            //int Result = objNew.CheckIsItHoliday(Todate);


            return View();
        }

        [HttpPost]
        public ActionResult Warehouse(List<WarehouseRSWCModel> Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { status = objNew.InsertUpdateRSWC(Details) }, JsonRequestBehavior.AllowGet);
        }


        public string GenerateWarehouseRSWCEng(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());




                if (!DetailsList[0].IsSubmit)
                {
                    if (DetailsList[0].Id == "0")
                    {
                        result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                    }
                    else
                    {
                        result = objNew.UpdateRSWC(DetailsList).ToString();
                        //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                    }
                    if (result == "-1" || result == "0")
                    {
                        result = "Error";
                    }
                    else
                    {
                        result = "Success";
                    }
                    return result;
                }

                if (result == "-1" || result == "0")
                {
                    result = "Error";
                }
                else
                {

                    // DataTable List = objNew.GetDirectorDemandDetails(result);

                    var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    if (otpResult.status != "1")
                    {
                        return otpResult.data;
                    }

                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string WRHTML = GenerateWRHTML(DetailsList);
                    sb.Append(WRHTML);
                    sb.Append("</div>");
                    sb.Append("</body></html>");



                    string fileId = Guid.NewGuid().ToString();

                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = fileId;// result.Replace("/", "_");
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {


                            if (DetailsList[0].Id == "0")
                            {
                                result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                            }
                            else
                            {
                                result = objNew.UpdateRSWC(DetailsList).ToString();
                                //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                            }
                            Receipt_No = result;

                            if (System.IO.File.Exists(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf"))
                            {
                                System.IO.File.Delete(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                            }

                            System.IO.File.Move(filepath + "/" + FileName + ".pdf", filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                            if (result == "-1" || result == "0")
                            {
                                return "Error";
                            }
                            else
                            {
                                result = "Success";
                            }
                            //return result;


                            FileName = Receipt_No.Replace("/", "_");



                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateRSWC(DetailsList).ToString();


                        }
                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " GenerateWarehouseRSWCEng(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }

        public string GenerateWRHTML(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            try
            {
                if (DetailsList.Count > 0)
                {

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    int index = 1;
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");
                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT UNDERTAKING)</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:35%;'>Sr.No. Of Physical Receipt (रसीद की क्रम संख्या) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> RSWC/" + DetailsList[0].OldReceipt_No + " </b> </td>");//List.Rows[0]["receipt_serial_no"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Name & Location of Warehouse (भण्डार गृह का नाम तथा स्थान) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> S/W " + DetailsList[0].WarehousName + " </b> </td>");//List.Rows[0]["WarehousNameEn"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Warehousemans license No. (भण्डार प्रबंधन का लाइसेंस नंबर) </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Lisence_No + " </b> </td>");//List.Rows[0]["lisence_no"]
                    sb.Append("<td>Valid up to (तक मान्य) </td>");
                    sb.Append("<td>" + (string.IsNullOrWhiteSpace(DetailsList[0].LicenceValidUpTo) ? "N.A." : DetailsList[0].LicenceValidUpTo) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through KVSS " + DetailsList[0].KvssName + " , Centre " + DetailsList[0].PurchaseCenter + "  </b> </td>");//List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");
                    sb.Append("<td  style='text-align:left;'><b> " + DetailsList[0].CommodityName + " </b> / " + DetailsList[0].Kharif + " " + DetailsList[0].Valid_From_Date.Split('/')[2] + "</td>");//List.Rows[0]["CommodityNameEn"]
                    sb.Append("<td colspan='2' style='text-align:left;'>Moisture (नमी) : <b> " + (string.IsNullOrWhiteSpace(DetailsList[0].Moisture) ? "0" : DetailsList[0].Moisture) + " %</b></td>");//List.Rows[0]["CommodityNameEn"]
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Class or Standared Quality and/or Grade<br/> श्रेणी या मान्य गुण प्रकार तथा / अथवा मान </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>No. of Packages or Lots <br/> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Net Quantity Measured in Qtls<br> नापी या तोली गयी शुद्ध मात्रा क्वि. में </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Name and/or Licence No. of the Weighter/Classifier/Sampler<br/> तोला / श्रेणी कर्ता / नमूना कर्ता का नाम तथा / अथवा लाइसेंस नंबर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>GNO/Stack No.<br/> गोदाम / स्टैक नंबर </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (var dr in DetailsList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.Quality) ? "FAQ" : dr.Quality.ToUpper()) + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Lots + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Quantity_Unit + " KG </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.NetQuantity + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.SamplerName + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.GoDownName) ? dr.GoDownName : dr.GoDownName) + "/" + dr.Stack_No + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.Price);//+ ((Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.StorageRateCharge) ;
                        totalLots += Convert.ToInt32(dr.Packages_Lots);
                        totalNetQty += Convert.ToDecimal(dr.NetQuantity);
                    }

                    if (DetailsList.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'style='text-align:right'>Total (कुल) </td>");
                        sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + (DetailsList[0].ConditionOfGoods == "1" ? "Good" : DetailsList[0].ConditionOfGoods == "2" ? "Fair" : "Average") + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].PrivateMarkofDepositor + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    sb.Append("<td colspan='3'  style='text-align:left;'><b> " + DetailsList[0].StorageRateCharge + " Per katta Per Month </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].IndemnifiedInsurance + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].InsuredName + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Remarks (टिप्पणी) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].QuantityNote + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Valid_From_Date + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    sb.Append("<td style='text-align:left;'><b>" + DetailsList[0].Valid_To_Date + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>MSP rate at the time of deposit (भण्डारण के समय समर्थन मूल्य भाव का भाव) </td>");
                    sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", DetailsList[0].Price) + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");


                    return sb.ToString();

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }



        [HttpPost]
        public string GetKVSS(int DistrictId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetPurchaseCenterKVSS(DistrictId));
        }

        [HttpPost]
        public string GetPurchaseCenterList(int DistrictId, int CommodityId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetPurchaseCenterList(DistrictId, CommodityId));
        }
        [HttpPost]
        public string GetWarehouseList(bool IsCWC=false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return new MasterController().RajfedWarehouseDetails(_LoggedInUser.WarehouseId, IsCWC);
        }

        [HttpPost]
        public JsonResult GetWarehouseRSWCList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseRSWCList(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetFertilizerSupplierDashboardData()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return JsonConvert.SerializeObject(objNew.GetFertilizerSupplierDashboardData(_LoggedInUser.UserRoleDeptMappingId), Formatting.Indented);
        }

        public ActionResult WarehouseLoanList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRSWC(string ReceiptNo, string Type, string Value)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { status = objNew.UpdateRSWC(ReceiptNo, Type, Value) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WarehouseCommodityReleased()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWarehouseRSWCReceiptDetails(string ReceiptNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseRSWCDetails(ReceiptNo); ;
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetWarehouseReleaseOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseReleaseOrder(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetWarehouseDeliveryOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseDeliveryOrder(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWarehouseDeliveryOrderWRList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseDeliveryOrderWRList(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDeliveryOrder(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryOrderWRList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDeliveryOrderWRList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult InsertCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string result = objNew.InsertCommodityReleased(DetailsList);

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }


        public string WRCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList, string ReceiptNo)
        {
            string result = "1";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                //result = objNew.InsertCommodityReleased(DetailsList);
                string requestNo = result;

                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {

                    DataTable List = objNew.GetWarehouseRSWCDetails(ReceiptNo);
                    DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);
                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");

                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT UNDERTAKING)</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:35%;'>Serial No. of Receipt (रसीद की क्रम संख्या) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> RSWC/" + List.Rows[0]["receipt_serial_no"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Name & Location of Warehouse (भण्डार गृह का नाम तथा स्थान) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> S/W " + List.Rows[0]["WarehousNameEn"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Warehousemans license No. (भण्डार प्रबंधन का लाइसेंस नंबर) </td>");
                    sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["lisence_no"] + " </b> </td>");
                    sb.Append("<td>Valid up to (तक मान्य) </td>");
                    sb.Append("<td>N.A.</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"] + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["CommodityNameEn"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");



                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Class or Standared Quality and/or Grade<br/> श्रेणी या मान्य गुण प्रकार तथा / अथवा मान </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>No. of Packages or Lots <br/> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Net Quantity Measured in Qtls<br> नापी या तोली गयी शुद्ध मात्रा क्वि. में </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Name and/or Licence No. of the Weighter/Classifier/Sampler<br/> तोला / श्रेणी कर्ता / नमूना कर्ता का नाम तथा / अथवा लाइसेंस नंबर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>GNO/Stack No.<br/> गोदाम / स्टैक नंबर </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["quality"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["sampler_name"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr["GoDownNameEng"].ToString()) ? dr["GoDownNameEng"] : dr["GoDownNameEng"]) + "/" + dr["stack_no"] + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }

                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'style='text-align:right'>Total<br> कुल </td>");
                        sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + (List.Rows[0]["goods_condition"].ToString() == "1" ? "Good" : List.Rows[0]["goods_condition"].ToString() == "2" ? "Fair" : "Average") + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["PrivateMarkofDepositor"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    sb.Append("<td colspan='3'  style='text-align:left;'><b> " + List.Rows[0]["StorageRateCharge"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["IndemnifiedInsurance"] + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["InsuredName"] + " </b> </td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["from_date"] + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    sb.Append("<td style='text-align:left;'><b>" + List.Rows[0]["to_date"] + " </b> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Market rate at the time of deposit (भण्डारण के समय मंडी का भाव) </td>");
                    sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", List.Rows[0]["Price"]) + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td  colspan='4' style='text-align:left;'><p style='text-indent: 20px;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion</p><p style='text-indent: 20px;'> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है। निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोडे गये माल से पूरा किया जा सकेगा। </p></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='4' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Date<br/> दिनांक </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Quantity Released<br/> निकाले  गये  माल की मात्रा </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Signature<br/> हस्ताक्षर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Quantity due on Receipt<br/> रसीद पर बकाया माल की मात्रा  </td>");
                    sb.Append("</tr>");
                    index = 1;

                    int releasedQuantity = 0;

                    foreach (DataRow dr in ReleasedCommodity.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RealeaseDated"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["released_quantity"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RemainingQuantity"] + " </td>");
                        sb.Append("</tr>");
                        releasedQuantity += Convert.ToInt32(dr["released_quantity"]);
                    }

                    foreach (var dr in DetailsList) // search whole table
                    {

                        string fromDate = "";
                        if (!string.IsNullOrWhiteSpace(dr.Valid_From_Date))
                        {
                            fromDate = dr.Valid_From_Date.Split('/')[2] + "/" + dr.Valid_From_Date.Split('/')[1] + "/" + dr.Valid_From_Date.Split('/')[0];
                        }
                        int remaingQty = (Convert.ToInt32(dr.Packages_Lots) - Convert.ToInt32(dr.ReleasedQuantity) - Convert.ToInt32(dr.ReleaseQuantity));
                        if (remaingQty < 0)
                            remaingQty = 0;
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Valid_From_Date + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleaseQuantity + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Quantity_Unit + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + remaingQty + " </td>");
                        sb.Append("</tr>");
                        //releasedQuantity += Convert.ToInt32(dr.ReleaseQuantity);
                    }

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");






                    sb.Append("</table>");



                    //sb.Append("<footer>");
                    //sb.Append("<br/><br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");

                    //sb.Append("</table>");
                    //sb.Append("</footer>");

                    //sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td  colspan='4' style='text-align:left;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion<br/> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है| निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोड़े गये माल से पूरा किया जा सकेगा |   </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Date<br/> दिनांक  </td>");
                    //sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    //sb.Append("<td style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("</table>");


                    sb.Append("</div>");
                    sb.Append("</body></html>");




                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "Relese_" + requestNo + "_" + ReceiptNo.Replace("/", "_");
                    string Receipt_No = ReceiptNo;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.data == "1")
                        {

                            string requestNo1 = objNew.InsertCommodityReleased(DetailsList);

                            if (result == "-1" && result == "0")
                            {
                                result = "Error";
                            }
                            else
                            {
                                result = "Success";
                            }



                            DetailsList.ForEach(x =>
                            {
                                x.Id = "0";
                                x.EntryFile = FileName + ".pdf";
                                x.Receipt_No = Receipt_No;
                            });


                            objNew.InsertCommodityReleased(DetailsList, requestNo1).ToString();


                        }
                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " WRCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList, string ReceiptNo)" }, ex);
                result = "Error";
            }
            return result;
        }


        public string DOCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DataList)
        {
            string result = "1";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                //result = objNew.InsertCommodityReleased(DetailsList);
                string requestNo = result;

                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {
                    var reciept = DataList.Select(x => x.Receipt_No).Distinct().ToList();
                    string esignPdf = "";

                    foreach (var recpt in reciept)
                    {
                        List<WarehouseRSWCModel> DetailsList = DataList.Where(a => a.Receipt_No == recpt).ToList();
                        string ReceiptNo = DetailsList[0].Receipt_No;






                        DataTable List = objNew.GetWarehouseRSWCDetails(ReceiptNo);
                        List<WarehouseRSWCModel> model1 = new List<WarehouseRSWCModel>();

                        int releasedPackagesLots = DetailsList.Sum(x => Convert.ToInt32(x.ReleasedPackages_Lots)); ;
                        int totalPackagesLots = 0;
                        foreach (DataRow dr in List.Rows)
                        {
                            WarehouseRSWCModel model = new WarehouseRSWCModel();
                            model.OldReceipt_No = dr["receipt_serial_no"].ToString();
                            model.WarehousName = dr["WarehousNameEn"].ToString();
                            model.Lisence_No = dr["lisence_no"].ToString();
                            model.KvssName = dr["SocietyName"].ToString();
                            model.PurchaseCenter = dr["PurchaseCenterEng"].ToString();
                            model.CommodityName = dr["CommodityNameEn"].ToString();
                            model.Quality = dr["quality"].ToString();
                            model.Packages_Lots = dr["packages_lots"].ToString();
                            model.Packages_Quantity_Unit = dr["UnitNameEng"].ToString();
                            model.NetQuantity = dr["net_quantity"].ToString();
                            model.SamplerName = dr["sampler_name"].ToString();
                            model.SamplerName = dr["Price"].ToString();
                            model.GoDownName = dr["GoDownNameEng"].ToString();
                            model.ConditionOfGoods = dr["goods_condition"].ToString();
                            model.PrivateMarkofDepositor = dr["PrivateMarkofDepositor"].ToString();
                            model.StorageRateCharge = dr["StorageRateCharge"].ToString();
                            model.IndemnifiedInsurance = dr["IndemnifiedInsurance"].ToString();
                            model.InsuredName = dr["InsuredName"].ToString();
                            model.Valid_From_Date = dr["from_date"].ToString();
                            model.Valid_To_Date = dr["to_date"].ToString();
                            model.Kharif = dr["Kharif"].ToString();
                            model.Stack_No = dr["Stack_No"].ToString();
                            model.Price = dr["Price"].ToString();
                            model.QuantityNote = dr["QuantityNote"].ToString();
                            model.ReleasedPackages_Lots = dr["ReleasedPackagesLots"].ToString();

                            releasedPackagesLots += Convert.ToInt32(dr["ReleasedPackagesLots"].ToString());
                            totalPackagesLots += Convert.ToInt32(dr["Packages_Lots"].ToString());
                            model1.Add(model);
                        }

                        DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);

                        OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                        sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                        int index = 1;
                        string filepath = Server.MapPath("~/Content/RSWC");
                        string fontSize = "font-size: 13px;";
                        string WRHTML = GenerateWRHTML(model1);
                        sb.Append(WRHTML);
                        sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                        sb.Append("<tr>");
                        sb.Append("<td  colspan='4' style='text-align:left;'><p style='text-indent: 20px;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion</p><p style='text-indent: 20px;'> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है। निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोडे गये माल से पूरा किया जा सकेगा। </p></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style=''>");
                        sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Date<br/> दिनांक </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Quantity Released<br/> निकाले  गये  माल की मात्रा </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Signature<br/> हस्ताक्षर </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Quantity due on Receipt<br/> रसीद पर बकाया माल की मात्रा  </td>");
                        sb.Append("</tr>");
                        index = 1;

                        decimal releasedQuantity = DetailsList.Sum(x => Convert.ToDecimal(x.NetQuantity));

                        foreach (DataRow dr in ReleasedCommodity.Rows) // search whole table
                        {
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RealeaseDated"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["released_quantity"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RemainingQuantity"] + " </td>");
                            sb.Append("</tr>");
                            releasedQuantity = Convert.ToDecimal(dr["RemainingQuantity"]);
                        }
                        var relesedQty = DetailsList.Sum(x => Convert.ToDecimal(x.ReleaseQuantity));
                        //foreach (var dr in DetailsList) // search whole table
                        //{

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + DetailsList[0].Valid_From_Date + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + relesedQty + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + DetailsList[0].Packages_Quantity_Unit + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");

                        if (releasedPackagesLots >= totalPackagesLots)
                        {
                            sb.Append("<td style='text-align:center;font-size:10px;'>0</td>");
                        }
                        else
                        {
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + (releasedQuantity - relesedQty) + " </td>");
                        }


                        sb.Append("</tr>");
                        //}

                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</body></html>");




                        if (!System.IO.Directory.Exists(filepath))
                        {
                            System.IO.Directory.CreateDirectory(filepath);
                        }
                        ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                        DataTable DT = new DataTable();

                        string FileName = "Relese_" + (ReleasedCommodity.Rows.Count + 1) + "_" + ReceiptNo.Replace("/", "_");
                        string Receipt_No = ReceiptNo;
                        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                        sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                        //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                        // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                        if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                        {

                            esignPdf += (string.IsNullOrWhiteSpace(esignPdf) ? "" : "#") + filepath + "/" + FileName + ".pdf";

                        }
                        else
                        {
                            return "PDF Not Generate";

                        }
                    }

                    var esignResult = COMMON_FUNCTION.EsinghPDF(TXN, esignPdf);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {
                        int index = 0;


                        result = objNew.InsertReleasedCommodity(DataList, esignPdf.Split('#'));


                        //foreach (var recpt in reciept)
                        //{
                        //    List<WarehouseRSWCModel> DetailsList = DataList.Where(a => a.Receipt_No == recpt).ToList();
                        //    string ReceiptNo = DetailsList[0].Receipt_No;
                        //    DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);
                        //    string requestNo1 = objNew.InsertCommodityReleased(DetailsList);

                        //    if (result == "-1" && result == "0")
                        //    {
                        //        result = "Error";
                        //        return result;
                        //    }
                        //    else
                        //    {
                        //        result = "Success";
                        //    }
                        //    DetailsList.ForEach(x =>
                        //    {
                        //        x.Id = "0";
                        //        x.EntryFile = "Relese_" + (ReleasedCommodity.Rows.Count+1) + "_" + ReceiptNo.Replace("/", "_") + ".pdf";
                        //        x.Receipt_No = ReceiptNo;
                        //    });
                        //    objNew.InsertCommodityReleased(DetailsList, requestNo1).ToString();    
                        //}
                    }


                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " DOCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DataList)" }, ex);
                result = "Error";
            }
            return result;
        }




        public ActionResult CommodityReleaseOrder()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            return View();
        }
        [HttpPost]
        public JsonResult GetCommodityReleaseOrderList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = new DataTable();
            if (RequestType == "1" || RequestType == "0" || RequestType == "3")
                DT = objNew.GetCommodityReleaseOrderList(ref Details, RequestType);
            else
                DT = objNew.GetWarehouseReleaseOrder(ref Details, RequestType);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommodityReleaseOrderDetails(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = new DataTable();
            DT = objNew.GetCommodityReleaseOrderDetails(RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string InsertUpdateCommodityReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }
                result = objNew.InsertUpdateCommodityReleaseOrder(DetailsList, "", uniqeId).ToString();
                //string requestNo = objNew.InsertKVSSRequest(ReqestData);

                if (result == "-1" || result == "0")
                {
                    objNew.DeleteReleaseOrder(uniqeId);
                    result = "Error";
                }
                else
                {

                    DataTable List = objNew.GetCommodityReleaseOrderDetails(result);

                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style='text-align:center;'>");

                    sb.Append("<br><label style='font-size: 22px;'>National Agricultural Cooperative Marketing Federation of India Limited</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'> (NAFED) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 12px;'><b>Branch Office :</b> 3rd Floor, Nehru Sahkar Bhavan , Bhawani Singh Road, Jaipur-302001 (Rajasthan) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>DELIVERY ORDER</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");

                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:25%;'><b>DO. NO.</b> </td>");
                    sb.Append("<td  style='text-align:left;'>" + result + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>DO. Date</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Warehouse</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + List.Rows[0]["WarehousNameEn"] + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;vertical-align: top;'><b>Deliver To</b></td>");
                    sb.Append("<td style='text-align:left;'><b>Company Name :</b> " + DetailsList[0].CompanyName + "<br><b>Receiver Name :</b> " + DetailsList[0].ReceiverName + " S/O " + DetailsList[0].ReceiverFName.Replace("S/O", "") + "<br><b>Mobile :</b> " + DetailsList[0].MobileNo + "    <b>Email :</b> " + DetailsList[0].Email + "<br><b>Address :</b> " + DetailsList[0].Address + " </td>");

                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through " + List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"] + "  </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["CommodityNameEn"] + " </b> </td>");
                    //sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");







                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> भण्डार गृह </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>WR No<br> डब्ल्यू.आर. संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Commodity<br> जिंस </td>");

                    //sb.Append("<td style='text-align:center;font-size:10px;'>Physical Receipt Sr.No. <br> फिसिकल रसीद नंबर</td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>No of Packages or Lots<br> कुल बंडलों की संख्या </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Released No of <br> Packages or Lots<br> जारी किये गये <br> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Release Packages or Lots<br> जारी करने के लिए <br> बंडलों की संख्या </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["CommodityNameEn"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["ReleasedPackagesLots"] + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["ReleaseOrderQuantity"] + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }

                    //if (List.Rows.Count > 1)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td colspan='2'style='text-align:right'>Total<br> कुल </td>");
                    //    sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("</tr>");
                    //}


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");







                    //sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + (List.Rows[0]["goods_condition"].ToString() == "1" ? "Good" : List.Rows[0]["goods_condition"].ToString() == "2" ? "Fair" : "Average") + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["PrivateMarkofDepositor"] + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    //sb.Append("<td colspan='3'  style='text-align:left;'><b> " + List.Rows[0]["StorageRateCharge"] + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["IndemnifiedInsurance"] + "  </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["InsuredName"] + " </b> </td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    //sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["from_date"] + " </b> </td>");
                    //sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    //sb.Append("<td style='text-align:left;'><b>" + List.Rows[0]["to_date"] + " </b> </td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Market rate at the time of deposit (भण्डारण के समय मंडी का भाव) </td>");
                    //sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", List.Rows[0]["Price"]) + " </b> </td>");
                    //sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    //sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    //sb.Append("</tr>");
                    //sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Authority Signature<br/> अधिकारी हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "DeliveryOrder_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateCommodityReleaseOrder(DetailsList, Receipt_No, uniqeId).ToString();
                        }
                        else { objNew.DeleteReleaseOrder(uniqeId); }
                    }
                    else
                    {
                        objNew.DeleteReleaseOrder(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteReleaseOrder(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " InsertUpdateCommodityReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        [HttpPost]
        public string InsertUpdateReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";

                }

                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                result = objNew.InsertUpdateCommodityReleaseOrder(DetailsList, "", uniqeId).ToString();

                if (result == "-1" || result == "0")
                {
                    objNew.DeleteReleaseOrder(uniqeId);
                    result = "Error";
                }
                else
                {

                    //DataTable List = objNew.GetCommodityReleaseOrderDetails(result);                    

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style='text-align:center;'>");

                    sb.Append("<br><label style='font-size: 22px;'>National Agricultural Cooperative Marketing Federation of India Limited</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'> (NAFED) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 12px;'><b>Branch Office :</b> 3rd Floor, Nehru Sahkar Bhavan , Bhawani Singh Road, Jaipur-302001 (Rajasthan) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>DELIVERY ORDER</label>");

                    sb.Append("</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:25%;'><b>DO. NO.</b> </td>");
                    sb.Append("<td  style='text-align:left;'>" + result + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>DO. Date</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Warehouse</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DetailsList[0].WarehousName + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Commodity</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DetailsList[0].CommodityName + " / " + DetailsList[0].FY + " </td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;vertical-align: top;'><b>Deliver To</b></td>");
                    sb.Append("<td style='text-align:left;'><b>Company Name :</b> " + DetailsList[0].CompanyName + "<br><b>Receiver Name :</b> " + DetailsList[0].ReceiverName + " S/O " + (string.IsNullOrWhiteSpace(DetailsList[0].ReceiverFName) ? "" : DetailsList[0].ReceiverFName.Replace("S/O", "")) + "<br><b>Mobile :</b> " + DetailsList[0].MobileNo + "    <b>Email :</b> " + DetailsList[0].Email + "<br><b>Address :</b> " + DetailsList[0].Address + " </td>");
                    sb.Append("</tr>");




                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");

                    //sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> वेयरहाउस  </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Commodity<br> जिंस </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Financial Year <br> वित्तीय वर्ष </td>");                
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Quantity(Quintal)<br> कुल मात्रा (क्विंटल) </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Released Quantity(Quintal)<br> कुल रिलीज की गयी मात्रा (क्विंटल) </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Quantity to be Release (Quintal)<br> रिलीज होने के लिए जारी कुल मात्रा (क्विंटल) </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (var dr in DetailsList) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.WarehousName + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.CommodityName + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.FY + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.NetQuantity + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleasedQuantity + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleaseQuantity + " </td>");
                        sb.Append("</tr>");
                        //totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        //totalLots += Convert.ToInt32(dr["packages_lots"]);
                        //totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");


                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Authority Signature<br/> अधिकारी हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "DeliveryOrder_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));

                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateCommodityReleaseOrder(DetailsList, Receipt_No, uniqeId).ToString();
                        }
                        else { objNew.DeleteReleaseOrder(uniqeId); }
                    }
                    else
                    {
                        objNew.DeleteReleaseOrder(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteReleaseOrder(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = "InsertUpdateReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        [HttpPost]
        public string SendOTPOnAADHAR(string _AadharNo)
        {
            return AADHARSerice.SendOTPOnAADHAR(_AadharNo);
        }

        [HttpPost]
        public string VeryifyOTPsentOnAADHAR(string OTP, string TXN, string _EncryptedAadharNo)
        {
            return AADHARSerice.VeryifyOTPsentOnAADHAR(OTP, TXN, _EncryptedAadharNo);
        }





        #endregion

        public ActionResult WRSummary()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWRSummaryList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWRSummaryDetailsList(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryDetailsList(RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string InsertWRSummary(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }


                result = objNew.InsertUpdateWRSummary(DetailsList, "", uniqeId).ToString();
                //string requestNo = objNew.InsertKVSSRequest(ReqestData);



                if (result == "-1" && result == "0")
                {
                    objNew.DeleteWRSummary(uniqeId);
                    result = "Error";
                }
                else
                {

                    string RO = "";

                    DataTable List = objNew.GetWRSummaryDetailsList(result);

                    //foreach (DataRow wr in List.Rows)
                    //{

                    //    if (Array.IndexOf(RO.Split(','), wr["RONameEn"].ToString()) < 0)
                    //        RO += (RO != "" ? "," : "") + wr["RONameEn"];
                    //}


                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 15px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rajfed_logo.jpg") + "' /></div>");

                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 28px;'>" + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य सहकारी क्रय-विक्रय संघ लि.") + "  </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD.</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    string commodityName = "";
                    //string comodity=""
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        commodityName += (commodityName == "" ? "" : ",") + dr["CommodityNameEn"];
                    }
                    commodityName = string.Join(",", commodityName.Split(',').Distinct().ToArray());

                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > क्रमांक : " + commodityName + " / WR / " + List.Rows[0]["Kharif"] + " / " + List.Rows[0]["FY"] + "/ " + result + " </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > शाखा प्रबंधक <br /> नेफैड <br /> जयपुर । </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:20px'> विषय :- समर्थन मूल्य पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की  WR भिजवाने  बाबत । </p></td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > महोदय, </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:10px'> विषयान्तर्गत निवेदन हैं कि  समर्थन मूल्य  पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की WR सलग्न कर निम्नानुसार भिजवाई जा रही  हैं :- </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Sr.No</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>R.O.</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Warehous </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>KVSS </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Purchase Cente </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>WR No.</th>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Date</th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>No. of Lots </th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>Weight(Quintal)</th>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Total Value</td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    decimal rowTotalNetQty = 0;
                    int rowTotalLots = 0;
                    string prevRO = "A";
                    string prevKVSS = "A";
                    string pCenter = "A";
                    foreach (DataRow dr in List.Rows) // search whole table
                    {

                        if (pCenter != dr["PurchaseCenterEng"].ToString() && pCenter != "A")
                        {
                            sb.Append("<tr>");
                            sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                            sb.Append("</tr>");
                            rowTotalNetQty = 0;
                            rowTotalLots = 0;
                        }
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index) + "</td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["WarehousNameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:15px;'> " + UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString()) + "    </td>");
                        //sb.Append("<td style='text-align:left;font-size:10px;'> " + (prevRO == dr["RONameEn"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString())) + " </td>");
                        //sb.Append("<td style='text-align:left;font-size:15px;'> " + (prevKVSS == dr["SocietyName"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString())) + "    </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["PurchaseCenterEng"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WRGenerateDate"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["TotalProce"] + " </td>");
                        sb.Append("</tr>");
                        rowTotalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        rowTotalLots += Convert.ToInt32(dr["packages_lots"]);


                        if (index >= List.Rows.Count)
                        {
                            //if (prevKVSS != dr["SocietyName"].ToString() && prevKVSS != "A")
                            {
                                sb.Append("<tr>");
                                sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                                sb.Append("</tr>");
                                rowTotalNetQty = 0;
                                rowTotalLots = 0;
                            }
                        }


                        prevKVSS = dr["SocietyName"].ToString();
                        prevRO = dr["RONameEn"].ToString();
                        pCenter = dr["PurchaseCenterEng"].ToString();


                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        ++index;
                    }

                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalLots + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalNetQty + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>Previous Total of WR's</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalPackages_Lots"] + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalNet_quantity"] + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalLots + Convert.ToDecimal(List.Rows[0]["PrevTotalPackages_Lots"])) + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalNetQty + Convert.ToDecimal(List.Rows[0]["PrevTotalNet_quantity"])) + "</th>");
                        sb.Append("</tr>");
                    }



                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");






                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3'  style='text-align:left;'>Enclosed:-" + List.Rows.Count + " WR </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक) :- " + DateTime.Now.ToString("dd/MM/yyyy") + "  </td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b> भवदीय <br/><br/> सहायक रजिस्ट्रार  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "WRSummary_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (result == "Success")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateWRSummary(DetailsList, Receipt_No, uniqeId).ToString();


                        }
                        else
                        {
                            objNew.DeleteWRSummary(uniqeId);
                        }
                    }
                    else
                    {
                        objNew.DeleteWRSummary(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteWRSummary(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " InsertWRSummary(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        public ActionResult WRPayment()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetWRPaymentList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetRajfedWRSummary(ref Details, RequestType); //objNew.GetRajfedWRSummaryPayment(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string InsertWRPayment(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                result = objNew.InsertUpdateWRPayment(DetailsList).ToString();

                if (result == "1")
                {
                    result = "Success";
                }
                else
                {
                    result = "Faild";
                }

                //if (result == "-1" && result == "0")
                //{
                //    result = "Error";
                //}
                //else
                //{
                //    DataTable List = objNew.GetWRSummaryDetailsList(result);
                //    object sumOrderQty;
                //    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                //    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                //    int index = 1;
                //    string requestFiles = "";
                //    string filepath = Server.MapPath("~/Content/RSWC");
                //    string devFontSize = "";
                //    string fontSize = "font-size: 13px;";
                //    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                //    sb.Append("<tr>");

                //    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");

                //    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT UNDERTAKING)</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                //    sb.Append("</div></th>");
                //    sb.Append("</tr>");
                //    sb.Append("<tr>");
                //    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                //    sb.Append("  ");
                //    sb.Append("</td>");
                //    sb.Append("</tr>");
                //    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                //    sb.Append("<tr>");
                //    sb.Append("<td colspan='3' style=''>");
                //    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                //    sb.Append("<tr>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> भण्डार गृह </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Receipt No<br> रसीद संख्या </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Physical Receipt Sr.No. <br> फिसिकल रसीद नंबर</td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>No of Packages or Lots<br> बंडलों की संख्या </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Quantity<br> मात्रा </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Total Value<br> कुल मूल्य </td>");
                //    sb.Append("</tr>");
                //    decimal totalPrice = 0;
                //    int totalLots = 0;
                //    decimal totalNetQty = 0;
                //    foreach (DataRow dr in List.Rows) // search whole table
                //    {
                //        sb.Append("<tr>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_no"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["TotalProce"] + " </td>");
                //        sb.Append("</tr>");
                //        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                //        totalLots += Convert.ToInt32(dr["packages_lots"]);
                //        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                //    }

                //    sb.Append("</table>");
                //    sb.Append("</td>");
                //    sb.Append("</tr>");
                //    sb.Append("</table>");
                //    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                //    sb.Append("<tr>");
                //    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                //    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                //    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature<br/> हस्ताक्षर </b> </td>");
                //    sb.Append("</tr>");
                //    sb.Append("</table>");

                //    sb.Append("</div>");
                //    sb.Append("</body></html>");





                //    if (!System.IO.Directory.Exists(filepath))
                //    {
                //        System.IO.Directory.CreateDirectory(filepath);
                //    }
                //    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //    DataTable DT = new DataTable();

                //    string FileName = "WRSummary_" + result;
                //    string Receipt_No = result;
                //    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                //    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                //    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                //    {
                //        result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, filepath + "/" + FileName + ".pdf", true, FileName + ".pdf");
                //        if (result == "Success")
                //        {


                //            DetailsList.ForEach(x =>
                //            {
                //                x.Id = "1";
                //                x.EntryFile = FileName + ".pdf";
                //                //x.Receipt_No = Receipt_No;
                //            });
                //            objNew.InsertUpdateWRSummary(DetailsList, Receipt_No).ToString();


                //        }
                //    }
                //    else
                //    {
                //        result = "PDF Not Generate";
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Warehouse", FunctionName = " InsertWRPayment(List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        public ActionResult ReleaseOrder()
        {
            return View();
        }

        //[HttpPost]
        public JsonResult GetAvailableCommodity(int Wharehouse_id, int CommodityId, int FinancialYear)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetAvailableCommodity(Wharehouse_id, CommodityId, FinancialYear);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDODetailsWR(int DelivberyNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDODetailsWR(DelivberyNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteWR(string Receipt_No)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { Status = objNew.DeleteWR(Receipt_No) }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CommodityReleased()
        {
            return View();
        }

    }
}