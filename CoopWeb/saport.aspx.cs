using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Cooperative.Controllers;
using Cooperative.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OfficeOpenXml;
using System.Linq;
using ClosedXML;
using ClosedXML.Excel;
using iTextSharp.tool.xml.html.table;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using System.IO.Compression;
namespace Cooperative
{
    public partial class saport : System.Web.UI.Page
    {
        public string constr = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString;
        //CommFun comfun = new CommFun(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // txtUserName.Focus();
                    int rdo;
                    //Session["Admin"] = "";
                    ViewState["dbObjectType"] = "";
                    ViewState["oldObjectText"] = "";
                    if (!IsPostBack)
                    {
                        Random rdm = new Random();
                        //Session["rdo"] = rdm.Next();
                        this.ViewState["rno"] = rdm.Next();
                        rdo = Convert.ToInt32(this.ViewState["rno"].ToString());
                        //btnLogin.Attributes.Add("OnClick", "return md5authForSupertool(" + rdo + ",'" + txtUserName.ClientID + "','" + txtPassword.ClientID + "','" + HiddenField1.ClientID + "');");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogs er = new ErrorLogs();
                    er.WriteMessage(ex);
                    Server.ClearError();
                    Response.Redirect("~\\Error.aspx", false);
                }
            }
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode.Parent.Text == "Tables")
            {
                TextBox1.Text = "Select * from " + TreeView1.SelectedNode.Text;
            }

            if (TreeView1.SelectedNode.Parent.Text == "Columns")
            {
                string tbname = TreeView1.SelectedNode.Parent.Parent.Text;
                string cname = TreeView1.SelectedNode.Text.Substring(0, TreeView1.SelectedNode.Text.IndexOf("("));
                TextBox1.Text = "Select " + cname + " from " + tbname;
            }
            if (TreeView1.SelectedNode.Parent.Text == "Views")
            {
                //string qstr = "use " + DropDownList2.Text + " select VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS where TABLE_NAME ='" + TreeView1.SelectedNode.Text +"'";
                DataTable dtvv = new DataTable();

                dtvv = GetViewDefinition(TreeView1.SelectedNode.Parent.Parent.Text, TreeView1.SelectedNode.Text);
                if (dtvv.Rows.Count > 0)
                {
                    ViewState["dbObjectType"] = "Views";
                    ViewState["oldObjectText"] = dtvv.Rows[0][0].ToString();
                    TextBox1.Text = dtvv.Rows[0][0].ToString();
                }
            }
            if (TreeView1.SelectedNode.Parent.Text == "Store Procedure")
            {
                string DbSp = "dbo." + TreeView1.SelectedNode.Text;
                DataTable dtSpv = new DataTable();
                dtSpv = GetSPDefinition(TreeView1.SelectedNode.Parent.Parent.Parent.Text, DbSp);
                if (dtSpv.Rows.Count > 0)
                {
                    ViewState["dbObjectType"] = "Store Procedure";
                    ViewState["oldObjectText"] = dtSpv.Rows[0][0].ToString();
                    TextBox1.Text = dtSpv.Rows[0][0].ToString();
                }
            }
            if (TreeView1.SelectedNode.Parent.Text == "Functions")
            {
                string DbSp = "dbo." + TreeView1.SelectedNode.Text;
                DataTable dtSpv = new DataTable();
                dtSpv = GetSPDefinition(TreeView1.SelectedNode.Parent.Parent.Parent.Text, DbSp);
                if (dtSpv.Rows.Count > 0)
                {
                    ViewState["dbObjectType"] = "Functions";
                    ViewState["oldObjectText"] = dtSpv.Rows[0][0].ToString();
                    TextBox1.Text = dtSpv.Rows[0][0].ToString();
                }
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {

            string qry = "Cooperative@202627";
            if (txtUserName.Text.Trim().ToLower() == "superuser" && txtPassword.Text.Trim() == qry)
            {
                divQuery.Visible = true;
                divLogin.Visible = false;
                //BindTreeview();
            }
            else
            {
                divQuery.Visible = false;
                divLogin.Visible = true;

                Response.Write("<script language='javascript'>alert('Incorrect username or password, please try again.')</script>");
            }

            //try
            //{
            //    if (Page.IsValid)
            //    {
            //        string qry = "rks#devit";
            //        DataTable dt = FillDataTable(qry);
            //        if (dt.Rows.Count > 0)
            //        {
            //            //string encpws = Encryption.Encrypt(txtPassword.Text.Trim(), EncryptionKey.PasswordHash).ToString();
            //            if (txtUserName.Text.Trim().ToLower() == "agri" && txtPassword.Text.Trim() == qry)
            //            {
            //                divQuery.Visible = true;
            //                divLogin.Visible = false;
            //                BindTreeview();
            //            }
            //            else
            //            {
            //                divQuery.Visible = false;
            //                divLogin.Visible = true;

            //                Response.Write("<script language='javascript'>alert('Incorrect username or password, please try again.')</script>");
            //            }
            //        }
            //        else
            //        {
            //            divQuery.Visible = false;
            //            divLogin.Visible = true;
            //            Response.Write("<script language='javascript'>alert('Incorrect username or password, please try again.')</script>");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //    ErrorLogs er = new ErrorLogs();
            //    er.WriteMessage(ex);
            //    Response.Redirect("~\\Error.aspx");
            //}
        }
        protected void btnExecute_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text.Trim() == "")
            {
                Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
                return;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ToString());
            SqlCommand cmd = null;
            try
            {

                SqlCommand Scmd = new SqlCommand("spUpdateDBObjectHistory", con);
                Scmd.CommandType = CommandType.StoredProcedure;
                Scmd.Parameters.Add("@DBObjectType", SqlDbType.VarChar).Value = ViewState["dbObjectType"].ToString();
                Scmd.Parameters.Add("@OldSPText", SqlDbType.NVarChar).Value = ViewState["oldObjectText"].ToString();
                Scmd.Parameters.Add("@NewSPText", SqlDbType.NVarChar).Value = TextBox1.Text;
                Scmd.Parameters.Add("@IPAddress", SqlDbType.NVarChar).Value = Controllers.COMMON_FUNCTION.GetLocalIPAddress();
                Scmd.Parameters.Add("@SystemName", SqlDbType.NVarChar).Value = "";

                con.Open();
                cmd = new SqlCommand(TextBox1.Text.Trim(), con);
                // INSERT DATA IN tblUpdateDBObjectHistory TABLE
                Scmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                Response.Write("<script language='javascript'>alert('Query Executed')</script>");
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }
        //protected void btnSelect_Click(object sender, EventArgs e)
        //{
        //    lblerror.Text = string.Empty;
        //    if (TextBox1.Text.Trim() == "")
        //    {
        //        Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
        //        return;
        //    }
        //    constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
        //    SqlConnection con = new SqlConnection(constr);
        //    DataTable dt = new DataTable();

        //    SqlDataAdapter da = null;
        //    try
        //    {
        //        con.Open();
        //        //da.SelectCommand.CommandTimeout = 300;
        //        da = new SqlDataAdapter(TextBox1.Text.Trim(), con);
        //        da.Fill(dt);
        //        grd.DataSource = dt;
        //        grd.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblerror.Text = ex.ToString();
        //        //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
        //    }
        //    finally
        //    {
        //        con.Close();
        //        da.Dispose();
        //        dt.Dispose();
        //    }
        //}

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            lblerror.Text = string.Empty;
            if (TextBox1.Text.Trim() == "")
            {
                Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
                return;
            }
            constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            DataSet ds = new DataSet();

            SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new SqlDataAdapter(TextBox1.Text.Trim(), con);
                da.SelectCommand.CommandTimeout = 300;// Started From 10Oct23@04:10PM,Tue
                da.Fill(ds);

                // multiple grid                
                if (ds != null && ds.Tables.Count > 0)
                {
                    // table count
                    Label lblTT = new Label();
                    lblTT.ID = $"Label_TotalTables";
                    lblTT.Text = $"[Total Table(s) : {ds.Tables.Count}]";
                    lblTT.Attributes.Add("class", "heading"); //here
                    lblTT.Attributes.Add("style", "margin-Bottom: 25px;font-size: 19px;"); //here
                    pnlResult.Controls.Add(lblTT);

                    int i = 1;
                    foreach (DataTable dt1 in ds.Tables)
                    {
                        GridView grd = new GridView();
                        grd.ID = $"GridView_{i}";
                        //grd.BackColor = getColor(i);
                        grd.Attributes.Add("class", "GridStyle"); //here
                        grd.Attributes.Add("style", "margin-Bottom: 35px;"); //here
                        grd.DataSource = dt1; // some data source
                        grd.DataBind();
                        Label lbl = new Label();
                        lbl.ID = $"Label_{i}";
                        lbl.Text = $"[Table - {i}]  <mask> [Total Record(s) : {dt1.Rows.Count}]</mask>";
                        lbl.Attributes.Add("class", "heading"); //here
                        pnlResult.Controls.Add(lbl);
                        pnlResult.Controls.Add(grd);
                        if (!chkShowMultiResult.Checked)
                        {
                            return;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                da.Dispose();
                ds.Dispose();
            }
        }

        protected void btnExcelImport_Click(object sender, EventArgs e)
        {
            lblerror.Text = string.Empty;
            if (TextBox1.Text.Trim() == "")
            {
                Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
                return;
            }
            constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            DataTable dt = new DataTable();

            SqlDataAdapter da = null;
            try
            {
                con.Open();

                da = new SqlDataAdapter(TextBox1.Text.Trim(), con);

                da.Fill(dt);
                ExportToExcelWithTextFormat(dt, "Report");
            }
            catch
            {
            }
            finally
            {
                con.Close();
                da.Dispose();
                dt.Dispose();
            }

        }
        protected void btnCsvExport_Click(object sender, EventArgs e)
        {
            lblerror.Text = string.Empty;
            if (TextBox1.Text.Trim() == "")
            {
                Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
                return;
            }
            constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            DataTable dt = new DataTable();
            SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new SqlDataAdapter(TextBox1.Text.Trim(), con);
                da.Fill(dt);
                ExportDataTableToCsvUnicode(dt, "Report");
            }
            catch
            {
            }
            finally
            {
                con.Close();
                da.Dispose();
                dt.Dispose();
            }
        }
        public static void ExportDataTablenew(DataTable dt, string FromDate, string ToDate, string ReportHeader)
        {
            var resp = HttpContext.Current.Response;
            resp.Clear();
            resp.Buffer = true;

            // 1) Tell ASP.NET to use UTF-16 LE (Unicode)
            resp.ContentEncoding = System.Text.Encoding.Unicode;
            resp.Charset = "utf-8";

            // 2) Excel attachment header
            resp.AddHeader("Content-Disposition", "attachment; filename=Report.xlsx");
            resp.ContentType = "application/vnd.ms-excel; charset=utf-8";

            // 3) Write the UTF-16 LE BOM
            resp.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            // 4) Tab-separator hint (must still be ASCII)
            resp.Write("sep=\t\r\n");

            // 5) Optional ReportHeader / dates
            //if (!string.IsNullOrEmpty(ReportHeader))
            //    resp.Write("Report Name: " + ReportHeader + "\r\n");
            if (!string.IsNullOrEmpty(FromDate))
                resp.Write("From Date: " + FromDate
                         + "    To Date: " + ToDate + "\r\n");
            resp.Write("\r\n");

            // 6) Column headers
            bool first = true;
            foreach (DataColumn dc in dt.Columns)
            {
                resp.Write((first ? "" : "\t") + dc.ColumnName);
                first = false;
            }
            resp.Write("\r\n");

            // 7) Data rows
            foreach (DataRow dr in dt.Rows)
            {
                first = true;
                foreach (var cell in dr.ItemArray)
                {
                    resp.Write((first ? "" : "\t") + (cell?.ToString() ?? ""));
                    first = false;
                }
                resp.Write("\r\n");
            }

            // 8) Finish
            resp.End();
        }
        public void ExportToExcelWithTextFormat(DataTable dt, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dt.Columns[col].ColumnName;
                }

                // Add data with text format
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        var cell = worksheet.Cells[row + 2, col + 1];
                        string value = dt.Rows[row][col].ToString();
                        if (!value.IsNullOrWhiteSpace() && (value.IsInt() || value.IsFloat() || value.IsDecimal()))
                            cell.Value = dt.Rows[row][col];
                        else
                            cell.Value = dt.Rows[row][col].ToString();
                        cell.Value = cell.Value.ToString() == "True" ? "1" : cell.Value.ToString() == "False" ? "0" : cell.Value;
                        // Force text format to preserve leading zeros
                        cell.Style.Numberformat.Format = "@";
                    }
                }

                // Send to browser
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                HttpContext.Current.Response.BinaryWrite(stream.ToArray());
                HttpContext.Current.Response.End();

            }
        }
        public void ExportDataTableToCsvUnicode(DataTable dt, string fileName)
        {
            StringBuilder csvContent = new StringBuilder();

            // Header
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(column => $"\"{column.ColumnName}\"");
            csvContent.AppendLine(string.Join(",", columnNames));

            // Rows with leading zero and Unicode handling
            foreach (DataRow row in dt.Rows)
            {
                var fields = dt.Columns.Cast<DataColumn>().Select(col =>
                {
                    string value = row[col].ToString();

                    // Force as text to preserve leading zeros and Unicode
                    return $"\"=\"\"{value}\"\"\"";
                });

                csvContent.AppendLine(string.Join(",", fields));
            }

            // Convert to UTF-8 with BOM
            byte[] utf8BomBytes = Encoding.UTF8.GetPreamble(); // BOM
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment;filename={fileName}.csv");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "text/csv";

            // Write BOM + data
            HttpContext.Current.Response.BinaryWrite(utf8BomBytes);
            HttpContext.Current.Response.BinaryWrite(csvBytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void ExportDataTablenew1(DataTable dt, string FromDate, string ToDate, string ReportHeader)
        {
            string attachment = "attachment; filename=" + "ABC.xls" + "";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            string sTab = "";
            if (ReportHeader != "")
            {
                HttpContext.Current.Response.Write("Report Name:  " + ReportHeader);
                HttpContext.Current.Response.Write("\n");
            }
            if (FromDate != "")
            {
                HttpContext.Current.Response.Write("From Date :  " + FromDate);
                HttpContext.Current.Response.Write("    To  Date : " + ToDate);
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.Write("\n");
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(sTab + dc.ColumnName);

                sTab = "\t";
            }
            HttpContext.Current.Response.Write("\n");

            int i;
            foreach (DataRow dr in dt.Rows)
            {
                sTab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(sTab + dr[i].ToString());
                    sTab = "\t";
                }
                HttpContext.Current.Response.Write("\r\n");
                // HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }



        public DataTable FillDataTable(string qstr)
        {
            String connString = constr;
            //"Data Source=" + m_SQLServer + ";User ID=" + m_Username + ";Password=" + m_Password + ";";
            string msgg = "";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                    {

                        msgg += "\n" + e.Message;

                    };
                    sqlConn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConn;
                    cmd.CommandText = qstr;
                    SqlDataAdapter ad = new SqlDataAdapter();
                    ad.SelectCommand = cmd;

                    DataTable tblDatabases = new DataTable();
                    ad.Fill(tblDatabases);// sqlConn.GetSchema("Databases");

                    sqlConn.Close();
                    return tblDatabases;

                    //DataTable td = tblDatabases.Select("dbid>6").CopyToDataTable();
                }
            }
            catch (SqlException ex)
            {
                DataTable tberror = new DataTable("tbRs");
                tberror.Columns.Add("Error", typeof(string));
                tberror.Rows.Add(ex.Message);
                return tberror;


            }
        }
        public DataTable GetTables(string Db)
        {
            string qstr = "use " + Db + " SELECT name FROM sys.Tables where name not in ('') order by name ";
            return FillDataTable(qstr);

        }
        public DataTable GetTableColumn(string TbName, string Db)
        {
            string qstr = "use " + Db + " SELECT COLUMN_NAME,DATA_TYPE , CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TbName + "' ORDER BY ORDINAL_POSITION ";
            return FillDataTable(qstr);

        }
        //public DataTable ExecuteQuery(string qstr)
        //{65

        //}
        public DataTable GetViewList(string Db)
        {
            string qstr = "use " + Db + " SELECT  name AS view_name  FROM sys.views";
            return FillDataTable(qstr);
        }
        public DataTable GetSplist(string Db)
        {
            string qstr = "use " + Db + " SELECT name FROM sys.procedures";
            return FillDataTable(qstr);
        }
        public string msgResult(string msg)
        {
            return msg;
        }
        public DataTable GetViewDefinition(string db, string view)
        {
            string qstr = "use " + db + " select VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS where TABLE_NAME ='" + view + "'";
            return FillDataTable(qstr);

        }
        public DataTable GetSPDefinition(string Db, string Sp)
        {
            string qstr = "use " + Db + " SELECT definition FROM sys.sql_modules WHERE object_id = (OBJECT_ID(N'" + Sp + "'))";
            return FillDataTable(qstr);
        }
        public DataTable GetFunctionList(string Db)
        {
            string qstr = "use " + Db + " select routine_name from information_schema.routines where routine_type='function'";
            return FillDataTable(qstr);
        }
        public void BindTreeview()
        {
            TreeNode tn = new TreeNode();
            // string dbname = "elibdcetest";
            SqlConnection con1 = new SqlConnection(constr);
            string dbname = con1.Database;
            tn.Text = dbname;// dblist.Rows[i][0].ToString();
            //  tn.ImageUrl = "~/Img/dbObj.png";
            TreeView1.Nodes.Add(tn);
            //tnDb.ChildNodes.Add(tn);
            DataTable dt = GetTables(dbname);

            //ListBox1.SelectedIndex;
            //For Table and its Columns
            TreeNode tnTable = new TreeNode();
            tnTable.Text = "Tables";
            //  tnTable.ImageUrl = "~/Img/FolderImg.png";
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                // ListBox2.Items.Add(dt.Rows[i][2].ToString());
                TreeNode tn1 = new TreeNode();
                tn1.Text = dt.Rows[j][0].ToString();
                //  tn1.ImageUrl = "~/Img/Tables.png";
                DataTable dtColumn = new DataTable();
                dtColumn = GetTableColumn(dt.Rows[j][0].ToString(), dbname);
                TreeNode tnClumn = new TreeNode();
                tnClumn.Text = "Columns";
                //   tnClumn.ImageUrl = "~/Img/FolderImg.png";
                for (int k = 0; k < dtColumn.Rows.Count; k++)
                {
                    TreeNode tncolumn = new TreeNode();
                    tncolumn.Text = dtColumn.Rows[k][0].ToString() + "(" + dtColumn.Rows[k][1].ToString() + "," + dtColumn.Rows[k][2].ToString() + ")";
                    //  tncolumn.ImageUrl = "~/Img/TblColumn.png";
                    tnClumn.ChildNodes.Add(tncolumn);
                }
                tn1.ChildNodes.Add(tnClumn);
                tnTable.ChildNodes.Add(tn1);
            }
            tn.ChildNodes.Add(tnTable);
            //For View and its Column
            DataTable dtview = GetViewList(dbname);
            TreeNode tnView = new TreeNode();
            // tnView.ImageUrl = "~/Img/FolderImg.png";
            tnView.Text = "Views";
            for (int v = 0; v < dtview.Rows.Count; v++)
            {
                TreeNode tnVchild = new TreeNode();
                tnVchild.Text = dtview.Rows[v][0].ToString();
                //  tnVchild.ImageUrl = "~/Img/vw.png";
                DataTable dtColumn = new DataTable();
                dtColumn = GetTableColumn(dtview.Rows[v][0].ToString(), dbname);
                TreeNode tnClumn = new TreeNode();
                tnClumn.Text = "Columns";
                //  tnClumn.ImageUrl = "~/Img/FolderImg.png";
                for (int k = 0; k < dtColumn.Rows.Count; k++)
                {
                    TreeNode tncolumn = new TreeNode();
                    tncolumn.Text = dtColumn.Rows[k][0].ToString() + "(" + dtColumn.Rows[k][1].ToString() + "," + dtColumn.Rows[k][2].ToString() + ")";
                    //  tncolumn.ImageUrl = "~/Img/TblColumn.png";
                    tnClumn.ChildNodes.Add(tncolumn);
                }
                tnVchild.ChildNodes.Add(tnClumn);
                tnView.ChildNodes.Add(tnVchild);
            }
            tn.ChildNodes.Add(tnView);
            //For Sp
            TreeNode tvPrg = new TreeNode();
            tvPrg.Text = "Programmability";
            // tvPrg.ImageUrl = "~/Img/FolderImg.png";
            TreeNode tvSp = new TreeNode();
            tvSp.Text = "Store Procedure";
            //  tvSp.ImageUrl = "~/Img/FolderImg.png";
            DataTable dtSp = GetSplist(dbname);
            for (int sp = 0; sp < dtSp.Rows.Count; sp++)
            {
                TreeNode tvspname = new TreeNode();
                tvspname.Text = dtSp.Rows[sp][0].ToString();
                //  tvspname.ImageUrl = "~/Img/Spimg.png";
                tvSp.ChildNodes.Add(tvspname);
            }
            tvPrg.ChildNodes.Add(tvSp);
            TreeNode tvFn = new TreeNode();
            tvFn.Text = "Functions";
            //   tvFn.ImageUrl = "~/Img/FolderImg.png";
            DataTable dtFn = GetFunctionList(dbname);
            for (int f = 0; f < dtFn.Rows.Count; f++)
            {
                TreeNode tnFn = new TreeNode();
                tnFn.Text = dtFn.Rows[f][0].ToString();
                //      tnFn.ImageUrl = "~/Img/Function.png";
                tvFn.ChildNodes.Add(tnFn);
            }
            tvPrg.ChildNodes.Add(tvFn);
            tn.ChildNodes.Add(tvPrg);
        }

        protected void BtnDownloadExcel_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();



            table = GetData();
            GridView gv = new GridView();
            gv.DataSource = table;
            gv.DataBind();



            if (gv.Rows.Count > 0)
            {
                GridViewExportUtil.Export("Data.xls", gv);
            }
        }

        protected DataTable GetData()
        {
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                string msgg = "";



                SqlCommand cmd = new SqlCommand("ActList", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60000;



                DataTable dt = new DataTable();
                SqlDataAdapter dad = new SqlDataAdapter();



                /* Initializing the object of Dataset created in the project */



                //DsMstStream Adata = new DsMstStream();
                dad.SelectCommand = cmd;
                dad.Fill(dt);
                return dt;
            }
        }

        public class ErrorLogs
        {
            string filepath;
            public ErrorLogs()
            {
                DirectoryInfo dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "Logs");
                if (!dir.Exists)
                {
                    dir.Create();
                }
                filepath = System.AppDomain.CurrentDomain.BaseDirectory + @"logs\log_" + DateTime.Now.ToString("MMddyyyy") + ".txt";
                FileInfo logfile = new FileInfo(filepath);
                if (!logfile.Exists)
                {
                    using (StreamWriter sw = logfile.CreateText())
                    {
                        sw.WriteLine("Erros on " + DateTime.Now.ToString("MM-dd-yyyy"));
                        sw.WriteLine("===================================");
                        sw.Close();
                    }
                }
            }
            public void WriteMessage(Exception ex)
            {
                FileInfo logfile = new FileInfo(filepath);
                using (StreamWriter sw = logfile.AppendText())
                {
                    sw.WriteLine(DateTime.Now.TimeOfDay.ToString() + " : \n Error Message :" + ex.Message + "\n Inner Excpetion :" + ex.InnerException + "\n Stack Trace :" + ex.StackTrace);
                    sw.Close();
                }
            }
            public static FileInfo[] GetAllFiles()
            {
                DirectoryInfo dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "Logs");
                return dir.GetFiles();
            }
        }
        protected void btnTreeBind_Click(object sender, EventArgs e)
        {
            BindTreeview();
        }

        protected void ddldbname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddldbname.SelectedValue == "wmsprodDR")
            {
                btnExecute.Enabled = false;
            }
            else
            {
                btnExecute.Enabled = true;
            }

            constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
        }



        #region 


        protected void btnExportMultipleFilesOn_Click(object sender, EventArgs e)
        {
            lblerror.Text = string.Empty;
            if (TextBox1.Text.Trim() == "")
            {
                Response.Write("<script language='javascript'>alert('Please Enter Query')</script>");
                return;
            }
            constr = ConfigurationManager.ConnectionStrings[ddldbname.SelectedValue.ToString()].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            DataSet ds = new DataSet();

            SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new SqlDataAdapter(TextBox1.Text.Trim(), con);
                //da.SelectCommand.CommandTimeout = 300;// Started From 10Oct23@04:10PM,Tue
                da.Fill(ds);


                string firstColValue = "";

                if (ds != null && ds.Tables.Count > 0)
                {

                    // string drive = "C:";
                    string folderName = "", folderPath = "";

                    for (int i = 1; i < ds.Tables.Count; i += 2) // Start from 2nd table (index 1)
                    {
                        foreach (DataRow row in ds.Tables[i - 1].Rows)
                        {

                            firstColValue = row[0].ToString();
                            folderName = firstColValue.Split('_')[0];
                        }
                        folderPath = Server.MapPath("~/Content/ReportGenerated/" + folderName);
                        //folderPath = $@"{drive}\{folderName}";
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(ds.Tables[i], "Sheet1");

                            string path = Path.Combine(folderPath, firstColValue + ".xlsx");

                            wb.SaveAs(path);
                        }
                    }

                    if (folderPath != "") 
                    {
                        string sourceFolder = @"C:\MyFiles";      // Folder you want to zip
                        string zipPath = Server.MapPath("~/Content/ReportGenerated/" + folderName+".zip");// Destination zip file                        

                        // If zip already exists, delete it first
                        if (System.IO.File.Exists(zipPath))
                        {
                            System.IO.File.Delete(zipPath);
                        }

                        // Create zip from directory
                        ZipFile.CreateFromDirectory(folderPath, zipPath);
                    }

                }

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                da.Dispose();
                ds.Dispose();
            }
        }
        #endregion
    }

    public class GridViewExportUtil
    {
        public static void Export(string fileName, GridView gv)
        {
            try
            {

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                gv.HeaderRow.Style.Add("background-color", "#C4A567");
                gv.FooterRow.Style.Add("showfooter", "true");
                gv.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccbaa1");
                gv.RowStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("Blue");
                gv.FooterStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccbaa1");

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        //  Create a form to contain the grid
                        System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();

                        //  add the header row to the table

                        if (gv.HeaderRow != null)
                        {
                            GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                            table.Rows.Add(gv.HeaderRow);
                        }

                        gv.BorderStyle = BorderStyle.Solid;
                        //  add each of the data rows to the table
                        int count = 0;
                        foreach (GridViewRow row in gv.Rows)
                        {
                            row.Style.Add("text-align", "center");
                            if (count % 2 == 0)
                            {
                                row.Style.Add("background-color", "#F8F8F8");
                                GridViewExportUtil.PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }
                            else
                            {
                                row.Style.Add("background-color", "#E6E4E4");
                                GridViewExportUtil.PrepareControlForExport(row);
                                table.Rows.Add(row);
                            }
                            count = count + 1;
                        }

                        //  add the footer row to the table
                        if (gv.FooterRow != null)
                        {
                            PrepareControlForExport(gv.FooterRow);


                            gv.FooterRow.Style.Add("BackColor", "#5D7B9D");
                            table.Rows.Add(gv.FooterRow);
                        }

                        //  render the table into the htmlwriter
                        table.RenderControl(htw);

                        //  render the htmlwriter into the response

                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();

                    }
                }
            }
            catch
            {
                int k = 0;

            }

        }

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


    }
}