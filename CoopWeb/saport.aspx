<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="saport.aspx.cs" Inherits="Cooperative.saport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="~/Content/stylesheets/custom.css" rel="stylesheet" />
    <style type="text/css">
        #pnlResult > div {
            overflow: auto;
            max-height: 500px;
            max-width: 100%;
        }

            #pnlResult > div > table tr th {
                position: sticky;
                top: 0px;
            }

            #pnlResult > div > table {
                border-collapse: separate !important;
            }

        .GridStyle {
            border: 5px solid rgb(217, 231, 255);
            background-color: White;
            font-family: arial;
            font-size: 15px;
            border-collapse: collapse;
            margin-bottom: 0px;
            width: 100%;
            border: none;
        }

            .GridStyle tr {
                border: 2px solid rgb(217, 231, 255);
                color: Black;
                height: 25px;
                padding: 5px;
            }
            /* Your grid header column style */
            .GridStyle th {
                padding: 5px;
                background-color: rgb(217, 231, 255);
                font-weight: bold;
                color: Black;
                border: 2px solid rgb(173 188 214) !important;
            }
            /* Your grid header link style */
            .GridStyle tr th a, .GridStyle tr th a:visited {
                color: Black;
            }

            .GridStyle tr th, .GridStyle tr td table tr td {
                border: none;
            }

            .GridStyle td {
                border: 2px solid rgb(217, 231, 255);
                padding: 5px;
            }

        .heading {
            font-size: large;
            font-weight: bolder;
            font-family: arial;
            margin-bottom: 5px;
            color: #e33852d6;
            display: block;
        }

        mask {
            margin-left: 20px;
        }

        input[type="submit"] {
            display: inline-block;
            margin-right: 10px;
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <div id="divQuery" runat="server" visible="false">
            <table cellpedding="5" width="100%">
                <%-- <tr>
                 <td><asp:DropDownList ID="ddldbname" AutoPostBack="true" runat="server" Width="173px" OnSelectedIndexChanged="ddldbname_SelectedIndexChanged">
                        <asp:ListItem Text="wmsprod" Value="wmsprod"></asp:ListItem>
                        <asp:ListItem Text="wmsprodDR" Value="wmsprodDR"></asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>--%>
                <tr>
                    <td colspan="2" align="right">
                        <asp:HyperLink ID="HlnkLogout" runat="server" Font-Bold="true" NavigateUrl="~/saport.aspx">Log out</asp:HyperLink>
                        <asp:Button ID="btnTreeBind" runat="server" OnClick="btnTreeBind_Click" Text="TreeBind" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="max-width: 350px;">
                        <asp:Panel ID="Panel2" runat="server" Height="360px" ScrollBars="Both"
                            Width="100%">
                            <asp:TreeView ID="TreeView1" runat="server"
                                OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            </asp:TreeView>
                            <%--<asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" Height="565px" 
                onselectedindexchanged="ListBox1_SelectedIndexChanged" Width="232px"></asp:ListBox>--%>
                        </asp:Panel>
                    </td>
                    <td valign="top" align="left" width="99%">
                        <asp:TextBox ID="TextBox1" runat="server" Height="350px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnExecute" runat="server" Text="Execute Query" OnClick="btnExecute_Click" />
                        <asp:Button ID="btnSelect" runat="server" Text="Select Query" OnClick="btnSelect_Click" />
                        <asp:Button ID="btnExcelImport" runat="server" Text="Export Excel" OnClick="btnExcelImport_Click" />
                        <asp:Button ID="btnCsvExport" runat="server" Text="Export CSV" OnClick="btnCsvExport_Click" />
                        <asp:Button ID="BtnDownloadExcel" runat="server" Text="Excel Import New" OnClick="BtnDownloadExcel_Click" />
                        DB :<asp:DropDownList ID="ddldbname" AutoPostBack="true" runat="server" Width="173px" OnSelectedIndexChanged="ddldbname_SelectedIndexChanged">
                            <asp:ListItem Text="wmsprod" Value="wmsprod"></asp:ListItem>
                            <asp:ListItem Text="RajeevikaConStr" Value="RajeevikaConStr"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox ID="chkShowMultiResult" runat="server" Text="Show Multiple Result" Font-Bold="true" />
                        <asp:Button ID="MultipleFileExport" runat="server" Text="Excel Import Multiple Data " OnClick="btnExportMultipleFilesOn_Click" />

                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlResult" runat="server"></asp:Panel>
        </div>
        <div id="divLogin" runat="server" visible="true">
            <br />
            <br />
            User Name :
            <asp:TextBox ID="txtUserName" runat="server" MaxLength="50"></asp:TextBox><br />
            <br />
            Password : &nbsp;
            <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox><br />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Submit" OnClick="btnLogin_Click" />
        </div>
    </form>
</body>
</html>

