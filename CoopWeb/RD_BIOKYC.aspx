<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RD_BIOKYC.aspx.cs" Inherits="Cooperative.RD_BIOKYC" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
        }
        
        input[type=text], select
        {
            padding: 5px 10px;
            border: 1px solid #808080;
            border-radius: 5px;
            width: 200px;
        }
        
        select
        {
            width: 220px;
        }
        
        input[type=submit]
        {
            padding: 5px 10px;
            border: 1px solid #808080;
            border-radius: 5px;
        }
        
        #vs ul li
        {
            width: 33%;
            float: left;
        }
        
        .main
        {
            width: 400px;
            float: left;
            margin: 5px;
        }
        
        fieldset
        {
            width: 400px;
            float: left;
            margin-right: 20px;
        }
        
        .lbl
        {
            width: 160px;
            float: left;
            height: 30px;
            font-size: 14px;
        }
        
        .inp
        {
            width: 350px;
            float: left;
            height: 30px;
        }
        
        h2
        {
            margin: 5px auto;
        }
        
        fieldset .lbl
        {
            width: 120px;
        }
        
        .footer
        {
            background-color: #984B07;
            display: block;
            text-align: center;
            width: 100%;
            color: #fff;
            padding: 20px 0px 10px 0px;
        }
        
        .radio input
        {
            float: left;
            margin-right: 5px;
            margin-left: 5px;
            margin-top: 10px;
        }
        
        .radio label
        {
            float: left;
            margin-right: 10px;
            margin-top: 6px;
        }
    </style> 
    <script language="javascript" type="text/javascript">
        function thisPrint() {
            var divToPrint = document.getElementById("printTable");
            newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            return false;
        }
    </script>
    <script src="jquery-1.12.4.js" type="text/javascript"></script>
    <script type="text/javascript">
        var uri = "http://127.0.0.1:11100/rd/"; //Non-Secure

        function startCaptureRD(fpdevice, authType, fcallback) {
            var error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers.<br>3. Start/ restart the registered device service.";
            rdCapture(uri + 'capture', authType, function (err, data) {
                if (err != null) {
                    fcallback({ type: "rd", status: "error", data: error_msg });
                } else {
                    var $xml = $($.parseXML(String(data)));
                    if ($xml.find("Resp").attr("errCode") === '0') {
                        fcallback({ type: "rd", status: "success", data: Base64.encode(String(data)) });
                    }
                    else if ($xml.find("Resp").attr("errCode") === '999') {
                        fcallback({ type: "rd", status: "failed", data: error_msg });
                    }
                    else {
                        fcallback({ type: "rd", status: "failed", data: "Error: " + $xml.find("Resp").attr("errInfo") + " (" + $xml.find("Resp").attr("errCode") + ")" });
                        console.log(String(data));
                    }
                }
            });
        }

        var rdCapture = function (url, authType, callback) {
        
            var xhr = new XMLHttpRequest();
            xhr.open('CAPTURE', url, true);
            xhr.responseType = 'text';    //json
            var wadh = "";
            if (authType.toLowerCase() === "ekyc") { wadh = "RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw="; }
         
            var pidXml = '<PidOptions ver="1.0"><Opts fCount="1" fType="0" format="0" pidVer="2.0" timeout="20000" posh="UNKNOWN" env="P" wadh="' + wadh + '" /></PidOptions>';//prod
            console.log(pidXml);

            xhr.onload = function () {
                if (xhr.status == 200) { callback(null, xhr.response); }
                else { callback(xhr.status); }
            };
            xhr.onerror = function () {
                callback(xhr.status);
                console.log(xhr.status);
            };
            xhr.ontimeout = function () {
                callback(xhr.status);
                console.log(xhr.response);
            };
            xhr.abort = function () {
                callback(xhr.status);
                console.log(xhr.response);
            };
            xhr.send(pidXml);
        };

        var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }

        function getCapturedData(fpData) {
            console.log(fpData);
            $("#<%= hdn_biodata.ClientID %>").val(fpData.data);
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table class="signup" cellpadding="5" cellspacing="5" style="border-collapse: collapse">
                    <tr>
                        <td colspan="3">
                            <h1>
                                UID Verification
                            </h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; padding-left: 10px">
                            <div class="lbl">
                                Select Option :</div>
                        </td>
                        <td style="width: 400px;">
                            <div class="inp">
                                <asp:RadioButtonList ID="rbtn_search" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" CssClass="radio" OnSelectedIndexChanged="rbtn_search_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="1" Selected="True">By BioMetric</asp:ListItem> 
                                    <asp:ListItem Value="2">By OTP</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td align="center" style="width: 20%;">
                            <asp:HiddenField ID="hdn_otpresult" runat="server" />
                            <asp:HiddenField ID="hdn_uidnumber" runat="server" />
                            <asp:HiddenField ID="hdn_biodata" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 10px">
                            <b>Enter UID No :</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_uidno" runat="server" Width="200px" />
                            &nbsp;
                            <asp:Button ID="btn_otp" runat="server" Visible="false" Text="Get OTP" />
                        </td>
                        <td align="center">
                            <asp:RequiredFieldValidator ID="txt_uidnoreq" runat="server" InitialValue="" ErrorMessage="Please Enter UID Number"
                                ControlToValidate="txt_uidno" ValidationGroup="a" ForeColor="Red" Display="Dynamic" />
                        </td>
                    </tr> 
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 10px">
                            &nbsp;
                        </td>
                        <td> 
                            <asp:Button ID="btn_capture" runat="server" Heigth="30px" OnClientClick="startCaptureRD('mantra','ekyc',getCapturedData); return true;"
                                Text="Capture" Visible="true" />
                            <asp:Button ID="btn_verify" runat="server" Heigth="30px" OnClick="btn_verify_Click"
                                Text=" Verify UID Number" ValidationGroup="a" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                        DisplayAfter="0">
                        <ProgressTemplate>
                            <div> 
                                <img src="progress.gif" style="float: none" alt="" />
                                Verifying...
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <br />
                    <br />
                    <div id="printTable">
                        <asp:Literal ID="ltl" runat="server" />
                    </div>
                    <asp:Button ID="btnQckviewPrint" runat="server" Text=" Print " OnClientClick="return thisPrint();"
                        Visible="false" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
    </form>
</body>
</html>
