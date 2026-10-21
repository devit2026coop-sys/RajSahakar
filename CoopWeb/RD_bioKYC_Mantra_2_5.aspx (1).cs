using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Xml;
using System.Data;

public partial class RD_bioKYC_Mantra_2_5 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltl.Text = "";
        }
    } 
    protected void btn_verify_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbtn_search.SelectedValue == "1")
            {
                ltl.Text = "";
                if (txt_uidno.Text == "")
                {
                    ltl.Text = "Please Enter Aadhaar No";
                }
                else if (hdn_biodata.Value == "")
                {
                    ltl.Text = "Biometric data did not captured.";
                }
                else
                {
                    string finalResponse = ConvertFromBase64(hdn_biodata.Value);

                    string licenseKey = "";//RD KYC Production
                    

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(finalResponse);

                    XmlNode nodeDeviceInfo = xml.SelectSingleNode("PidData/DeviceInfo");
                    XmlNode rdsInfo = xml.SelectSingleNode("PidData/Resp");
                    XmlNode nodeSkey = xml.SelectSingleNode("PidData/Skey");
                    XmlNode nodeHmac = xml.SelectSingleNode("PidData/Hmac");
                    XmlNode nodeData = xml.SelectSingleNode("PidData/Data");

                    string fType = rdsInfo.Attributes["fType"].Value;
                    //string iCount = rdsInfo.Attributes["iCount"].Value;
                    //string pCount = rdsInfo.Attributes["pCount"].Value;
                    string iCount = "1";
                    string pCount = "1";
                    string errCode = rdsInfo.Attributes["errCode"].Value;
                    string errInfo = rdsInfo.Attributes["errInfo"].Value;
                    string fCount = rdsInfo.Attributes["fCount"].Value;
                    //string ts = rdsInfo.Attributes["ts"].Value;
                    string nmPoints = rdsInfo.Attributes["nmPoints"].Value;
                    string qScore = rdsInfo.Attributes["qScore"].Value;

                    string srno = "";

                    string deviceError = "";
                    string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/kyc/bio/prod?client_id=<client_id>";
                    HttpWebRequest requestUID = (HttpWebRequest)WebRequest.Create(url);
                    requestUID.Method = "POST";

                    // If required by the server, set the credentials.
                    requestUID.Credentials = CredentialCache.DefaultCredentials;
                    requestUID.ContentType = "application/xml";     // content type
                    // Wrap the request stream with a text-based writer
                    StreamWriter writerUID = new StreamWriter(requestUID.GetRequestStream());

                    var tts = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss");

                    /**
                     * KYC 2.5 Request
                     */

                    //RD KYC Production
                   string xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\"?><authrequest uid='" + txt_uidno.Text + "' ra='F' rc='n' lr='' de='' pfr='N' tid='' ver='2.5' rdsID='" + nodeDeviceInfo.Attributes["rdsId"].Value + "' rdsVer='" + nodeDeviceInfo.Attributes["rdsVer"].Value + "' dpID='" + nodeDeviceInfo.Attributes["dpId"].Value + "' dc='" + nodeDeviceInfo.Attributes["dc"].Value + "' mi='" + nodeDeviceInfo.Attributes["mi"].Value + "' mc='" + nodeDeviceInfo.Attributes["mc"].Value + "' subaua='XXXX' ip='' fdc='' idc='' bt='' macadd='' lot='' lov='' mec='' lk='" + licenseKey + "' ts='" + tts + "'><deviceInfo fType='" + fType + "' iCount='" + iCount + "' pCount='" + pCount + "' errCodeRDS='" + errCode + "' errInfoRDS='" + errInfo + "' fCount='" + fCount + "' nmPoints='" + nmPoints + "' qScore='" + qScore + "' deviceSrNO='" + srno + "' deviceError='" + deviceError + "' /><Skey ci='" + nodeSkey.Attributes["ci"].Value + "'>" + nodeSkey.InnerText + "</Skey><Hmac>" + nodeHmac.InnerText + "</Hmac><Data type='" + nodeData.Attributes["type"].Value + "'>" + nodeData.InnerText + "</Data></authrequest>";
                   
                    writerUID.WriteLine(xmlString);
                    writerUID.Close();
                    WebResponse responseUID = default(WebResponse);
                    ////' GET RESPONSE FROM WEB SERVICE ''''
                    responseUID = requestUID.GetResponse();
                    Stream strUID = responseUID.GetResponseStream();
                    StreamReader srUID = new StreamReader(strUID);
                    string finalResponseUID = srUID.ReadToEnd();
                    string Ads = "";
                    string uidno = "";
                    string fname = "";
                    string pname = "";
                    string dobdate = "";
                    string pgender = "";
                    string phone1 = "";
                    string email1 = "";
                    string pht = "";
                    XmlDocument xml1 = new XmlDocument();
                    xml1.LoadXml(finalResponseUID);
                    string ret = string.Empty;
                    foreach (XmlNode node in xml1.SelectNodes("//auth[@status]"))
                    {
                        ret = node.Attributes["status"].Value;
                    }

                    if (ret.ToUpper() == "Y")
                    {
                        XmlNode node2 = xml1.SelectSingleNode("authresponse/UidData/pht");
                        string base64BinaryStr = node2.InnerText;
                        byte[] byteArray = Convert.FromBase64String(base64BinaryStr);
                        string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                        pht = "data:image/png;base64," + base64String;

                        XmlNode node3 = xml1.SelectSingleNode("authresponse/UidData/pa");
                        fname = node3.Attributes["co"].Value;

                        XmlNode node4 = xml1.SelectSingleNode("authresponse/UidData/pi");
                        uidno = txt_uidno.Text;
                        pname = node4.Attributes["name"].Value;
                        dobdate = node4.Attributes["dob"].Value;
                        pgender = node4.Attributes["gender"].Value;
                        phone1 = node4.Attributes["phone"].Value;
                        email1 = node4.Attributes["email"].Value;

                        string house = node3.Attributes["house"].Value;
                        string street = node3.Attributes["street"].Value;
                        string lm = node3.Attributes["lm"].Value;
                        string loc = node3.Attributes["loc"].Value;
                        string vtc = node3.Attributes["vtc"].Value;
                        string dist = node3.Attributes["dist"].Value;
                        string state = node3.Attributes["state"].Value;
                        string pc = node3.Attributes["pc"].Value;

                        Ads = house + ", " + street + ", " + lm + ", " + loc + ", " + vtc + ", " + dist + ", " + state + ", " + pc;

                    }
                    else if (ret.ToUpper() == "N")
                    {
                        ltl.Text = "Authentication fail";
                    }
                    else
                    {

                    }

                    StringBuilder htmlTableString = new StringBuilder();
                    htmlTableString.AppendLine("<table class='signup' cellpadding='5' cellspacing='5' style='border-collapse: collapse; width:730px'>");
                    htmlTableString.AppendLine("<tr><td colspan='3'><h1>UID DETAILS</h1></td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>UID No.: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + uidno + "</td><td style='padding-left: 10px';ForeColor='Red' rowspan='8'><img src='" + pht + "'></img></td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Name:</b> </td><td style='padding-left: 10px';ForeColor='Red'>" + pname + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Father's Name: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + fname + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>DOB: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + dobdate + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Gender: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + pgender + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Phone No.:</b></td><td style='padding-left: 10px';ForeColor='Red'>" + phone1 + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Address: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + Ads + "</td></tr>");
                    htmlTableString.AppendLine("<tr><td><b>Email: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + email1 + "</td></tr>");
                    htmlTableString.AppendLine("</table>");
                    ltl.Text = htmlTableString.ToString();
                    btnQckviewPrint.Visible = true;
                }
            }
            else if (rbtn_search.SelectedValue == "2")
            {

            }
        }
        catch (Exception ex)
        {
            ltl.Text = ex.ToString();
        }
    }
    public static string ConvertFromBase64(string base64String)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
    }
    protected void rbtn_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtn_search.SelectedValue == "1")
        {
            txt_uidno.Text = "";
            txt_uidno.Enabled = true;
            btn_otp.Visible = false;
            btn_capture.Visible = true;
            btn_verify.Visible = true;
        }
        else if (rbtn_search.SelectedValue == "2")
        {
            btn_otp.Visible = true;
            btn_verify.Visible = false;
            btn_capture.Visible = false;
        }
    }

}