using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Collections.Specialized;
using System.Threading;
namespace FactoryAndBoiler.Payment
{
    public partial class RajFabPaymentGateway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Thread.Sleep(10000);
                  string keypath = Server.MapPath("~/Content/rajfab.key");
                  // string keypath =" https://rajfab.rajasthan.gov.in/payment/rajfab.key";
                  EgrassFabEncrypt oEgrassFabEncrypt = new EgrassFabEncrypt();


                    //string encData = "ccwBeXXlZvBHd0mTQu+H2XwMbA4j444LkrTNDRvNs6nJK+1b1T1ZC69unHLfmJGunCvo9loQmNLyMy8XKQeHu7SxNXMRqxMlkUBWIsFSSnqH+XvLANGyrd21Yr1e4ghEfDsLWyHJJ/vdpMRY7NbDGpnCtuY3xma7VIl12w4FTvTp2ma+2Eo9p5W1LcqfKh1Y";
                    //encData = oEgrassFabEncrypt.Encrypt(encData, keypath);
                   
                
                   //////------------- DatabaseRelated oDatabaseRelated=new DatabaseRelated();
                    string qstr = "";
                    ////qstr = "select * from tmpEgrass where isactive=1";
                    //qstr = "insert into tmpEgrass select top 1 cast(AUIN as bigint)+1,ENCDATA,Head_Name1,Head_Amount1,Location,DistrictCode,OfficeCode,isactive from tmpEgrass order by id desc";
                    //DataSet ds1 = new DataSet();
                    //ds1= oDatabaseRelated.GetDataset(qstr, CommandType.Text, null);

                    // qstr = "select top 1 * from tmpEgrass order by id desc";
                   // String AUIN = Session["AUIN"].ToString();
                    // qstr = "select top 1 * from PaymentPrnLog where PRN='" + AUIN + "'";
                   // qstr = "select top 1 * from PaymentPrnLog where PaymentStatus=1";

                   // DataSet ds = new DataSet();
                // ds = oDatabaseRelated.GetDataset(qstr, CommandType.Text, null);

               // if (ds.Tables.Count > 0)
                {
                   // qstr = "SELECT Factory_plot +' ' + Factory_locality as FactAddress,Factory_city,Factory_pincode,(select name from District where id=Factory_districtid) as District FROM [USER] where id=" + ds.Tables[0].Rows[0]["UserID"].ToString() + "";
                    //DataSet ds2 = new DataSet();
                    //  ds2 = oDatabaseRelated.GetDataset(qstr, CommandType.Text, null);
                    string factAdrees, Pincode, city = "";
                    factAdrees = "ffsddssd";// ds2.Tables[0].Rows[0]["FactAddress"].ToString();
                    //if (ds2.Tables[0].Rows[0]["Factory_pincode"].ToString().Length == 6)
                    //{
                    //    Pincode = ds2.Tables[0].Rows[0]["Factory_pincode"].ToString();
                    //    // city = ds2.Tables[0].Rows[0]["Factory_city"].ToString();
                    //    city = ds2.Tables[0].Rows[0]["District"].ToString();
                    //}
                    //else
                    //{
                       
                    //}
                    Pincode = "302004";
                    city = "Jaipur";
                    string dtFrom = "";
                    string dtTo = "";

                    //if (ds.Tables[0].Rows[0]["Head_Name1"].ToString() == "023000104000000677")
                   // if (ds.Tables[0].Rows[0]["Head_Name1"].ToString() == "023000104000000000")
                   // {
                    //    qstr = "select DATEPART(year, Licence_Period_From) as Licence_Period_From,Licence_Period_To from OccupierForm2 where RegisterationFormId='" + ds.Tables[0].Rows[0]["ApplicationID"].ToString() + "'";
                    //    DataSet dtp = new DataSet();
                    //    // dtp = oDatabaseRelated.GetDataset(qstr, CommandType.Text, null);
                    //    if (dtp.Tables.Count > 0)
                    //    {
                    //        if (dtp.Tables[0].Rows.Count > 0)
                    //        {
                    //            dtFrom = dtp.Tables[0].Rows[0]["Licence_Period_From"].ToString() + "/04/01";
                    //            dtTo = dtp.Tables[0].Rows[0]["Licence_Period_To"].ToString() + "/03/31";
                    //        }
                    //        else
                    //        {
                    //            dtFrom = DateTime.Now.ToString("yyyy/MM/dd");
                    //            dtTo = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dtFrom = DateTime.Now.ToString("yyyy/MM/dd");
                    //        dtTo = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd");
                    //    }
                    //}
                    //else
                    //{
                    //    dtFrom = DateTime.Now.ToString("yyyy/MM/dd");
                    //    dtTo = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd");

                    //}

                    dtFrom = DateTime.Now.ToString("yyyy/MM/dd");
                    dtTo = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd");


                    string RemitterName = "RemitterName"; //ds.Tables[0].Rows[0]["FactoryName"].ToString();
                    //if (ds.Tables[0].Rows[0]["FactoryName"].ToString().Length > 38)
                   // {
                      //  RemitterName = ds.Tables[0].Rows[0]["FactoryName"].ToString().Substring(0, 37);
                   // }

                    string finyear = FinancialYear.Current.ToString();
                    string CHECKSUM = oEgrassFabEncrypt.Encrypt("0425-Sahakarita" + "|10000|F@bE*D$g^s#", keypath);
                    //string paystring = "AUIN=" + ds.Tables[0].Rows[0]["AUIN"].ToString() + "|Head_Name1=" + ds.Tables[0].Rows[0]["Head_Name1"].ToString() + "|Head_Amount1=" + ds.Tables[0].Rows[0]["Head_Amount1"].ToString() + "|Head_Name2=0|Head_Amount2=0|Head_Name3=0|Head_Amount3=0|Head_Name4=0|Head_Amount4=0|Head_Name5=0|Head_Amount5=0|Head_Name6=0|Head_Amount6=0|Head_Name7=0|Head_Amount7=0|Head_Name8=0|Head_Amount8=0|Head_Name9=0|Head_Amount9=0|RemitterName=Abc Chemicals|Discount=0|TotalAmount=100|MerchantCode=32|PaymentMode=N|REGTINNO=302004|Location=" + ds.Tables[0].Rows[0]["Location"].ToString() + "|DistrictCode=" + ds.Tables[0].Rows[0]["DistrictCode"].ToString() + "|OfficeCode=" + ds.Tables[0].Rows[0]["OfficeCode"].ToString() + "|DepartmentCode=32|FromDate=2018/12/03|ToDate=2019/12/31|Address=Jhalana Industrial area jaipur|PIN=302004|City=Jaipur|Remarks=SampleRemark|Filler=A|ChallanYear=1819|Checksum=" + CHECKSUM + "";

                    // string paystring = "AUIN=" + ds.Tables[0].Rows[0]["PRN"].ToString() + "|Head_Name1=" + ds.Tables[0].Rows[0]["Head_Name1"].ToString() + "|Head_Amount1=" + ds.Tables[0].Rows[0]["Head_Amount1"].ToString() + "|Head_Name2=0|Head_Amount2=0|Head_Name3=0|Head_Amount3=0|Head_Name4=0|Head_Amount4=0|Head_Name5=0|Head_Amount5=0|Head_Name6=0|Head_Amount6=0|Head_Name7=0|Head_Amount7=0|Head_Name8=0|Head_Amount8=0|Head_Name9=0|Head_Amount9=0|RemitterName=" + RemitterName + "|Discount=0|TotalAmount=" + ds.Tables[0].Rows[0]["Head_Amount1"].ToString() + "|MerchantCode=32|PaymentMode=N|REGTINNO=302004|Location=2100|DistrictCode=12|OfficeCode=395|DepartmentCode=32|FromDate=" + dtFrom + "|ToDate=" + dtTo + "|Address=" + factAdrees + "|PIN=" + Pincode + "|City=" + city + "|Remarks=SampleRemark|Filler=A|ChallanYear=1819|Checksum=" + CHECKSUM + "";

                    string paystring = "AUIN=43244465|Head_Name1=CA Empeneled Auditor Registration|Head_Amount1=10000|Head_Name2=0|Head_Amount2=0|Head_Name3=0|Head_Amount3=0|Head_Name4=0|Head_Amount4=0|Head_Name5=0|Head_Amount5=0|Head_Name6=0|Head_Amount6=0|Head_Name7=0|Head_Amount7=0|Head_Name8=0|Head_Amount8=0|Head_Name9=0|Head_Amount9=0|RemitterName=" + RemitterName + "|Discount=0|TotalAmount=10000|MerchantCode=32|PaymentMode=N|REGTINNO=302004|Location=2100|DistrictCode=12|OfficeCode=395|DepartmentCode=32|FromDate=" + dtFrom + "|ToDate=" + dtTo + "|Address=" + factAdrees + "|PIN=" + Pincode + "|City=" + city + "|Remarks=SampleRemark|Filler=A|ChallanYear=1819|Checksum=" + CHECKSUM + "";

                    string ENCDATA1 = oEgrassFabEncrypt.Encrypt(paystring, keypath);
                    //this.AUIN.Value = ds.Tables[0].Rows[0]["AUIN"].ToString();
                    //this.ENCDATA.Value = ENCDATA1;
                    NameValueCollection data = new NameValueCollection();
                    data.Add("Merchant_code", "32");
                    data.Add("AUIN", "43244465");//ds.Tables[0].Rows[0]["PRN"].ToString());
                    data.Add("ENCDATA", ENCDATA1);
                    RedirectAndPOST(this.Page, "https://egras.raj.nic.in/samplemerchantprelogin.aspx", data);

                }

                  
            }
        }
        
    public static void RedirectAndPOST(Page page, string destinationUrl,  NameValueCollection data)
    {
    //Prepare the Posting form
        string strForm = PreparePOSTForm(destinationUrl, data);
        //Add a literal control the specified page holding 
        //the Post Form, this is to submit the Posting form with the request.
        page.Controls.Add(new LiteralControl(strForm));
    }
        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + 
                           formID + "\" action=\"" + url + 
                           "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + 
                               "\" value=\"" + data[key] + "\">");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + 
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
            }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //Response.Redirect("http://164.100.153.101/egrassectest/samplemerchantprelogin.aspx");
        }
    }
}