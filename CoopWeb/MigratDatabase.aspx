<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MigratDatabase.aspx.cs" Inherits="Paramedical.MigratDatabase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        &nbsp;&nbsp;&nbsp;
            <br />
&nbsp;&nbsp; Tables<br />
&nbsp;<asp:CheckBox ID="chkAllTalbles" runat="server" />All
            <br />
&nbsp;<div style="overflow: auto;    max-height: 300px;    max-width: 350px;margin-top:10px;">
<asp:CheckBoxList ID="chkTables" runat="server"  AutoPostBack="false">
            </asp:CheckBoxList>
          </div>
            
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Featch Data" style="margin-top:20px;"/>
          
    </form>
</body>
</html>
