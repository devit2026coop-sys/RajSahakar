using Cooperative.Models;
using java.net;
using Newtonsoft.Json;
using RAJSSO;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class SWSController : Controller
    {
        // GET: SWS
        public ActionResult SWS_landing(string SWSID, string ActID, string ActivityID, string IsNew, string RegNo)
        {
            if (Request.Form["userdetails"] != null)
            {
                try
                {
                    SWSEntity objSWC = new SWSEntity();
                    objSWC.SWSID = SWSID;
                    objSWC.ActID = ActID;
                    objSWC.IsNew = IsNew;
                    objSWC.ActID = ActID;
                    objSWC.ActivityID = ActivityID;
                    objSWC.RegNo = RegNo;
                    objSWC.Userdetails = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";
                    objSWC.Establishmentname = Request.Form["Establishmentname"];
                    objSWC.Total_Employees = Request.Form["Total_Employees"];
                    objSWC.ProposedInvestment = Request.Form["ProposedInvestment"];
                    objSWC.Operational_Date = Request.Form["Operational_Date"];
                    objSWC.Mobile = Request.Form["Mobile"];
                    objSWC.Email = Request.Form["Email"];
                    objSWC.CategoryofEstablishment = Request.Form["CategoryofEstablishment"];
                    objSWC.NatureOfBusiness = Request.Form["NatureOfBusiness"];
                    objSWC.PlotNo = Request.Form["PlotNo"];
                    objSWC.Street = Request.Form["Street"];
                    objSWC.Area = Request.Form["Area"];
                    objSWC.RuralUrban = Request.Form["RuralUrban"];
                    objSWC.City = Request.Form["City"];
                    objSWC.Ward = Request.Form["Ward"];
                    objSWC.Village = Request.Form["Village"];
                    objSWC.Tehsil = Request.Form["Tehsil"];
                    objSWC.District = Request.Form["District"];
                    objSWC.PIN = Request.Form["PIN"];
                    objSWC.BusinessDetail = Request.Form["BusinessDetail"];
                    objSWC.PrimaryGroup = Request.Form["PrimaryGroup"];
                    objSWC.BRN = Request.Form["BRN"];
                    objSWC.PAN = Request.Form["PAN"];
                    objSWC.TIN = Request.Form["TIN"];
                    objSWC.VAT = Request.Form["VAT"];
                    objSWC.STDCode = Request.Form["STDCode"];
                    objSWC.FirstName = Request.Form["FirstName"];
                    objSWC.LastName = Request.Form["LastName"];
                    objSWC.Gender = Request.Form["Gender"];
                    objSWC.DOB = Request.Form["DOB"];
                    objSWC.PostalAddress = Request.Form["PostalAddress"];
                    objSWC.Est_PlotNo = Request.Form["Est_PlotNo"];
                    objSWC.Est_Street = Request.Form["Est_Street"];
                    objSWC.Est_Area = Request.Form["Est_Area"];
                    objSWC.Est_RuralUrban = Request.Form["Est_RuralUrban"];
                    objSWC.Est_District = Request.Form["Est_District"];
                    objSWC.Est_Tehsil = Request.Form["Est_Tehsil"];
                    objSWC.Est_Ward = Request.Form["Est_Ward"];
                    objSWC.Est_Village = Request.Form["Est_Village"];
                    objSWC.Est_PIN = Request.Form["Est_PIN"];

                    //objSWC.Est_PIN = "302021";
                    //objSWC.Est_Village = "NA";
                    //objSWC.Est_Ward = "NA";
                    //objSWC.Est_Tehsil = "NA";
                    //objSWC.Est_District = "Dausa";
                    //objSWC.Est_RuralUrban = "URBAN";
                    //objSWC.Est_Area = "LOCALITY";
                    //objSWC.Est_Street = "LANE";
                    //objSWC.Est_PlotNo = "373";
                    //objSWC.FirstName="GEETA";
                    //objSWC.LastName = "GEETA";
                    //objSWC.BRN = "0802470000000088";
                    //objSWC.Establishmentname = "AAAAAAAA";
                    //objSWC.ActID = "1";
                    //objSWC.SWSID = "78388";
                    //objSWC.ProposedInvestment = "5000000";
                    //objSWC.Email = "vinayyadav@gmail.com";
                    //objSWC.Mobile = "9999999999";


                    RAJSSO.SSO SSO = new RAJSSO.SSO();
                    SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                    SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                    string Get_Userdetails = ssoclient.GetTokenDetail(objSWC.Userdetails);
                    //string Get_Userdetails = "sAMAccountName = VINAYYADAV329; Role = CITIZEN";

                    if (!string.IsNullOrEmpty(Get_Userdetails) && Get_Userdetails != "User Login Id or Password is incorrect")
                    {
                        try
                        {
                            string key = Get_Userdetails.Split(';')[0].Split('=')[1];
                            string[] Role = Get_Userdetails.Split(';')[1].Split('=')[1].Split('|');
                            objSWC.SSO_ID = key;

                            SocietyController objSociety = new SocietyController();
                            objSWC.IsNewRequestFromSWCS = objSociety.CheckNewRequestFromSWCS(objSWC.SWSID, objSWC.SSO_ID);

                            objSWC.SWCSJsonData = JsonConvert.SerializeObject(objSWC);
                            System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = objSWC;

                            string JSONString = string.Empty;
                            JSONString = JsonConvert.SerializeObject(objSWC);
                            //return JSONString;

                            Response.Cookies.Add(new System.Web.HttpCookie("SWC_DATA", "")
                            {
                                Value = JSONString,
                                HttpOnly = true
                            });

                            Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
                            {
                                Value = key,
                                HttpOnly = true
                            });

                            Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
                            {
                                Value = objSWC.Userdetails,
                                HttpOnly = true
                            });

                            Response.Cookies.Add(new System.Web.HttpCookie("SWC", "")
                            {
                                Value = "YES",
                                HttpOnly = true
                            });

                            Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
                            {
                                Value = Role[0],
                                HttpOnly = true
                            });

                            Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
                            {
                                Value = "en-us",
                                HttpOnly = true
                            });

                            DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", "SWC", "SWC_DATA", SessionIdentity.Culture });
                            return RedirectToAction("SSOMapping", "Login");
                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new FileLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, ex);
                        }
                    }
                    else
                    {
                        Session.Clear();
                        //Session.Abandon();

                        if (Response.Cookies != null && Response.Cookies.Count > 0)
                            foreach (var item in Response.Cookies.AllKeys)
                                Response.Cookies.Remove(item);

                        string poststring = COMMON_FUNCTION.posttopage(GlobalConstant.RAJNIVESH_URL, "", GlobalConstant.RAJNIVESH_APPNAME);
                        Response.Write(poststring);
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SWSController", FunctionName = "SWS_landing" }, ex);
                }
            }
            return View();
        }

        public void DeleteUnwantedCookies(string[] CokkieKeyNotToDelete = null)
        {
            string[] Keys = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var Key in Keys)
            {
                if (CokkieKeyNotToDelete == null || !CokkieKeyNotToDelete.Any(P => P == Key))
                {
                    System.Web.HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie(Key)
                    {
                        Value = Guid.NewGuid().ToString(),
                        Expires = DateTime.Now.AddDays(-30),
                        HttpOnly = true
                    });
                }
            }
        }
        
        [HttpPost]
        public string UpdateSwscStatus()
        {


            return "";
        }
    }
}