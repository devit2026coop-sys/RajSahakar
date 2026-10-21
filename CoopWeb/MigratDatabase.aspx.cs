using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Paramedical
{
    public partial class MigratDatabase : System.Web.UI.Page
    {
        public string constr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                chkTables_Load();// Call CheckBox List Bind Method.  
            }
        }


        protected void SetConnection()
        {
            var conn = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString.ToString();
            //constr = "Data Source=" + con[2].ToString().Split('=')[2] + ";Initial Catalog=" + con[3].ToString().Split('=')[1] + ";Integrated Security=False;User ID=" + con[6].ToString().Split('=')[1] + ";Password=" + con[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            constr = conn;// "Data Source=" + con[2].ToString().Split('=')[2] + ";Initial Catalog=Coop_Rajeev;Integrated Security=False;User ID=" + con[6].ToString().Split('=')[1] + ";Password=" + con[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        }


        public void GetDataFromProduction()
        {
            SetConnection();
            TreeNode tn = new TreeNode();
            // string dbname = "elibdcetest";
            SqlConnection con1 = new SqlConnection(constr);
            string dbname = con1.Database;
            tn.Text = dbname;// dblist.Rows[i][0].ToString();
            //  tn.ImageUrl = "~/Img/dbObj.png";
         
            //tnDb.ChildNodes.Add(tn);
            DataTable dt = GetTables();

            //ListBox1.SelectedIndex;
            //For Table and its Columns
            TreeNode tnTable = new TreeNode();
            tnTable.Text = "Tables";
            //  tnTable.ImageUrl = "~/Img/FolderImg.png";
            for (int j = 0; j < dt.Rows.Count; j++)
            {

                TruncateTableRow(dt.Rows[j][0].ToString());
                bool isAll = false;
                int PageNumber = 1;
                int PageSize = 2000;
                while (!isAll)
                {
                    var data = SelectTableInsertScript(dt.Rows[j][0].ToString(), PageNumber, PageSize);
                    if (data == "")
                    {
                        isAll = true;
                    }
                    else
                    {
                        InsertRow(dt.Rows[j][0].ToString(), data);
                        ++PageNumber;
                    }
                    
                }
                //if (j == 0)
                //{
                //    SetIdentityOffOn(dt.Rows[j][0].ToString(), "ON");
                //}else if(j== dt.Rows.Count)
                //{
                //    SetIdentityOffOn(dt.Rows[j][0].ToString(), "OFF");
                //}



                //// ListBox2.Items.Add(dt.Rows[i][2].ToString());
                //TreeNode tn1 = new TreeNode();
                //tn1.Text = dt.Rows[j][0].ToString();
                ////  tn1.ImageUrl = "~/Img/Tables.png";
                //DataTable dtColumn = new DataTable();
                //dtColumn = GetTableColumn(dt.Rows[j][0].ToString(), dbname);
                //TreeNode tnClumn = new TreeNode();
                //tnClumn.Text = "Columns";
                ////   tnClumn.ImageUrl = "~/Img/FolderImg.png";
                //for (int k = 0; k < dtColumn.Rows.Count; k++)
                //{
                //    TreeNode tncolumn = new TreeNode();
                //    tncolumn.Text = dtColumn.Rows[k][0].ToString() + "(" + dtColumn.Rows[k][1].ToString() + "," + dtColumn.Rows[k][2].ToString() + ")";
                //    //  tncolumn.ImageUrl = "~/Img/TblColumn.png";
                //    tnClumn.ChildNodes.Add(tncolumn);
                //}
                //tn1.ChildNodes.Add(tnClumn);
                //tnTable.ChildNodes.Add(tn1);
            }
            
          
        }

        public DataTable GetTables()
        {
            DataTable ds = new DataTable();
            //registrationNo = "121/JODHPUR/1992-93";
            // Create a request for the URL.

            WebRequest request = WebRequest.Create("https://rajsahakarapp.rajasthan.gov.in/api/Services/GetTables");

            //WebRequest request = WebRequest.Create("https://htmeapps.rajasthan.gov.in/paramedicalcouncil/api/Services/GetTables");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);
                string data1 = data.ToString().Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", "").Replace("</string>", "");
                ds = JsonConvert.DeserializeObject<DataTable>(data1.ToString());

            }
            // Close the response.
            response.Close();

            return ds;

        }
        public DataTable GetTableColumn(string Table, string Db)
        {
            DataTable ds = new DataTable();
            //registrationNo = "121/JODHPUR/1992-93";
            // Create a request for the URL.
            WebRequest request = WebRequest.Create("https://rajsahakarapp.rajasthan.gov.in/api/Services/GetTableColumn?Table=" + Table);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);

                ds = JsonConvert.DeserializeObject<DataTable>(data.ToString());

            }
            // Close the response.
            response.Close();

            return ds;

        }
        public DataTable FillDataTable(string qstr)
        {
            String connString = constr;
            //"Data Source=" + m_SQLServer + ";User ID=" + m_Username + ";Password=" + m_Password + ";";
            string msgg = "";
            try
            {
                SetConnection();
                using (SqlConnection sqlConn = new SqlConnection(constr))
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

        //SET IDENTITY_INSERT [dbo].[AdminMenu] ON

        protected DataTable SelectTable(string Table, int PageNumber, int PageSize)
        {
            DataTable ds = new DataTable();
            //registrationNo = "121/JODHPUR/1992-93";
            // Create a request for the URL.

            
            //WebRequest request = WebRequest.Create("https://localhost:44368/api/Services/GetDataFromProd?Table=" + Table + "&PageNumber=" + PageNumber + "&PageSize=" + PageSize);

            WebRequest request = WebRequest.Create("https://rajsahakarapp.rajasthan.gov.in/api/Services/GetDataFromProd?Table=" + Table+ "&PageNumber="+ PageNumber + "&PageSize="+ PageSize);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);
                ds = JsonConvert.DeserializeObject<DataTable>(data.ToString());
            }
            // Close the response.
            response.Close();
            return ds;
        }

        protected string SelectTableInsertScript(string Table, int PageNumber, int PageSize)
        {
            DataTable ds = new DataTable();
            //registrationNo = "121/JODHPUR/1992-93";
            // Create a request for the URL.


            //WebRequest request = WebRequest.Create("https://localhost:44368/api/Services/GetDataFromProd?Table=" + Table + "&PageNumber=" + PageNumber + "&PageSize=" + PageSize);

            WebRequest request = WebRequest.Create("https://rajsahakarapp.rajasthan.gov.in/api/Services/GetDataFromProd?Table=" + Table + "&PageNumber=" + PageNumber + "&PageSize=" + PageSize);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            string dataResult = "";
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);
                dataResult=data.ToString();
            }
            // Close the response.
            response.Close();
            return dataResult;
        }


        protected void InsertRow(string tableName,string table)
        {

            string sqlQuery = "";// SET IDENTITY_INSERT [dbo].[" + tableName + "] ON";           
            try
            {
               //var columns= table.Columns.Cast<DataColumn>()
               //                   .Select(x => x.ColumnName)
               //                   .ToArray();

                
                
                int index = 1;
                //SetIdentityOffOn(tableName, "OFF");
                //foreach (DataRow data in table.Rows)
                //{
                //    sqlQuery += " INSERT INTO [dbo].[" + tableName + "](" + string.Join(",", columns) + ")values( ";

                //    string rowValues = "";
                //    foreach (var col in columns)
                //    {
                //        rowValues += (rowValues != "" ? "," : "") + ""+(data[col]==null?"NULL":"'"+ data[col] + "'") +"";
                //    }

                //    sqlQuery += rowValues + ")";    

                //    ++index;
                // }
                table = " SET IDENTITY_INSERT [dbo].[" + tableName + "] ON " + table + "  SET IDENTITY_INSERT [dbo].[" + tableName + "] OFF";
                ExecuteQuery(table);
                //SetIdentityOffOn(tableName, "OFF");

                // Response.Write("<script language='javascript'>alert('Query Executed')</script>");
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
               
            }
        }


        protected void ExecuteQuery(string query)
        {
            //query = query.Replace("'s ", "");

            SetConnection();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = null;
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                //Response.Write("<script language='javascript'>alert('Query Executed')</script>");
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }


        protected void TruncateTableRow(string table)
        
        {

            string sqlQuery = "TRUNCATE TABLE [dbo].[" + table + "]";

            SetConnection();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = null;
            try
            {
                con.Open();
                cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                // Response.Write("<script language='javascript'>alert('Query Executed')</script>");
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
               string s=GenerateTableScript(table);
                ExecuteQuery(s);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }


        protected void SetIdentityOffOn(string table,string OffOn)
        {

            string sqlQuery = "SET IDENTITY_INSERT [dbo].[" + table + "] "+ OffOn + "";

           
            SetConnection();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = null;
            try
            {
                con.Open();
                cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                //Response.Write("<script language='javascript'>alert('Query Executed')</script>");
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }


        protected string GenerateTableScript(string TableName)
        {
            string sqlTable = "";
            sqlTable += "DECLARE @table_name SYSNAME";
            sqlTable += "SELECT @table_name = 'dbo."+ TableName + "'";
            sqlTable += "DECLARE ";
            sqlTable += "      @object_name SYSNAME";
            sqlTable += "    , @object_id INT";
            sqlTable += " SELECT ";
            sqlTable += "      @object_name = '[' + s.name + '].[' + o.name + ']'";
            sqlTable += "    , @object_id = o.[object_id]";
            sqlTable += "FROM sys.objects o WITH (NOWAIT)";
            sqlTable += "JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]";
            sqlTable += "WHERE s.name + '.' + o.name = @table_name";
            sqlTable += "    AND o.[type] = 'U'";
            sqlTable += "    AND o.is_ms_shipped = 0";
            sqlTable += "DECLARE @SQL NVARCHAR(MAX) = ''";
            sqlTable += ";WITH index_column AS ";
            sqlTable += "(";
            sqlTable += "    SELECT";
            sqlTable += "          ic.[object_id]";
            sqlTable += "        , ic.index_id";
            sqlTable += "        , ic.is_descending_key";
            sqlTable += "        , ic.is_included_column";
            sqlTable += "        , c.name";
            sqlTable += "    FROM sys.index_columns ic WITH (NOWAIT)";
            sqlTable += "    JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id";
            sqlTable += "    WHERE ic.[object_id] = @object_id";
            sqlTable += "),";
            sqlTable += "fk_columns AS ";
            sqlTable += "(";
            sqlTable += "     SELECT ";
            sqlTable += "          k.constraint_object_id";
            sqlTable += "        , cname = c.name";
            sqlTable += "        , rcname = rc.name";
            sqlTable += "    FROM sys.foreign_key_columns k WITH (NOWAIT)";
            sqlTable += "    JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id ";
            sqlTable += "    JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id";
            sqlTable += "    WHERE k.parent_object_id = @object_id";
            sqlTable += ")";
            sqlTable += "SELECT @SQL = 'CREATE TABLE ' + @object_name + CHAR(13) + '(' + CHAR(13) + STUFF((";
            sqlTable += "    SELECT CHAR(9) + ', [' + c.name + '] ' + ";
            sqlTable += "        CASE WHEN c.is_computed = 1";
            sqlTable += "            THEN 'AS ' + cc.[definition] ";
            sqlTable += "            ELSE UPPER(tp.name) + ";
            sqlTable += "                CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary', 'text')";
            sqlTable += "                       THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'";
            sqlTable += "                     WHEN tp.name IN ('nvarchar', 'nchar', 'ntext')";
            sqlTable += "                       THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'";
            sqlTable += "                     WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset') ";
            sqlTable += "                       THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'";
            sqlTable += "                     WHEN tp.name = 'decimal' ";
            sqlTable += "                       THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'";
            sqlTable += "                    ELSE ''";
            sqlTable += "                END +";
            sqlTable += "                CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +";
            sqlTable += "                CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +";
            sqlTable += "                CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END + ";
            sqlTable += "                CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END ";
            sqlTable += "        END + CHAR(13)";
            sqlTable += "    FROM sys.columns c WITH (NOWAIT)";
            sqlTable += "    JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id";
            sqlTable += "    LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id";
            sqlTable += "    LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id";
            sqlTable += "    LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id";
            sqlTable += "    WHERE c.[object_id] = @object_id";
            sqlTable += "    ORDER BY c.column_id";
            sqlTable += "    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')";
            sqlTable += "    + ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + ";
            sqlTable += "                    (SELECT STUFF((";
            sqlTable += "                         SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END";
            sqlTable += "                         FROM sys.index_columns ic WITH (NOWAIT)";
            sqlTable += "                         JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id";
            sqlTable += "                         WHERE ic.is_included_column = 0";
            sqlTable += "                             AND ic.[object_id] = k.parent_object_id ";
            sqlTable += "                             AND ic.index_id = k.unique_index_id     ";
            sqlTable += "                         FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))";
            sqlTable += "            + ')' + CHAR(13)";
            sqlTable += "            FROM sys.key_constraints k WITH (NOWAIT)";
            sqlTable += "            WHERE k.parent_object_id = @object_id ";
            sqlTable += "                AND k.[type] = 'PK'), '') + ')'  + CHAR(13)";
            sqlTable += "    + ISNULL((SELECT (";
            sqlTable += "        SELECT CHAR(13) +";
            sqlTable += "             'ALTER TABLE ' + @object_name + ' WITH' ";
            sqlTable += "            + CASE WHEN fk.is_not_trusted = 1 ";
            sqlTable += "                THEN ' NOCHECK' ";
            sqlTable += "                ELSE ' CHECK' ";
            sqlTable += "              END + ";
            sqlTable += "              ' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY('";
            sqlTable += "              + STUFF((";
            sqlTable += "                SELECT ', [' + k.cname + ']'";
            sqlTable += "                FROM fk_columns k";
            sqlTable += "                WHERE k.constraint_object_id = fk.[object_id]";
            sqlTable += "                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')";
            sqlTable += "               + ')' +";
            sqlTable += "              ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('";
            sqlTable += "              + STUFF((";
            sqlTable += "                SELECT ', [' + k.rcname + ']'";
            sqlTable += "                FROM fk_columns k";
            sqlTable += "                WHERE k.constraint_object_id = fk.[object_id]";
            sqlTable += "                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')";
            sqlTable += "               + ')'";
            sqlTable += "            + CASE ";
            sqlTable += "                WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE' ";
            sqlTable += "                WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'";
            sqlTable += "                WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT'";
            sqlTable += "                ELSE '' ";
            sqlTable += "              END";
            sqlTable += "            + CASE ";
            sqlTable += "                WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'";
            sqlTable += "                WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'";
            sqlTable += "                WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT' ";
            sqlTable += "                ELSE '' ";
            sqlTable += "              END ";
            sqlTable += "            + CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR(13)";
            sqlTable += "        FROM sys.foreign_keys fk WITH (NOWAIT)";
            sqlTable += "        JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id";
            sqlTable += "        WHERE fk.parent_object_id = @object_id";
            sqlTable += "        FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')";
            sqlTable += "    + ISNULL(((SELECT";
            sqlTable += "         CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END ";
            sqlTable += "                + ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +";
            sqlTable += "                STUFF((";
            sqlTable += "                SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END";
            sqlTable += "                FROM index_column c";
            sqlTable += "                WHERE c.is_included_column = 0";
            sqlTable += "                    AND c.index_id = i.index_id";
            sqlTable += "                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')' ";
            sqlTable += "                + ISNULL(CHAR(13) + 'INCLUDE (' + ";
            sqlTable += "                    STUFF((";
            sqlTable += "                    SELECT ', [' + c.name + ']'";
            sqlTable += "                    FROM index_column c";
            sqlTable += "                    WHERE c.is_included_column = 1";
            sqlTable += "                        AND c.index_id = i.index_id";
            sqlTable += "                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR(13)";
            sqlTable += "        FROM sys.indexes i WITH (NOWAIT)";
            sqlTable += "        WHERE i.[object_id] = @object_id";
            sqlTable += "            AND i.is_primary_key = 0";
            sqlTable += "            AND i.[type] = 2";
            sqlTable += "        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')";
            sqlTable += "    ), '')";
            sqlTable += "";
            sqlTable += "select @SQL";

            return sqlTable;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                

              
                    foreach (ListItem chk in chkTables.Items)
                    {



                        if (chk.Selected|| chkAllTalbles.Checked)
                        {
                            TruncateTableRow(chk.Text);
                            bool isAll = false;
                            int PageNumber = 1;
                            int PageSize = 500;
                            while (!isAll)
                            {
                                var data = SelectTableInsertScript(chk.Text, PageNumber, PageSize);
                                if (data == "")
                                {
                                    isAll = true;
                                }
                                else
                                {
                                    InsertRow(chk.Text, data);
                                    ++PageNumber;
                                }

                            }
                        }
                    
                }

               // GetDataFromProduction();
            }
        }

        protected void chkTables_Load()
        {


           var data= GetTables();
            chkTables.DataSource = data;
            chkTables.DataTextField = "name";
            chkTables.DataValueField = "name";
            chkTables.DataBind();
        }


        //public string GetTablesSchema(string Table)
        //{
        //    DataTable ds = new DataTable();
        //    //registrationNo = "121/JODHPUR/1992-93";
        //    // Create a request for the URL.
        //    //WebRequest request = WebRequest.Create("https://rajevaluation.rajasthan.gov.in/api/Service/GetTablesSchema?TableName=" + Table);

        //    WebRequest request = WebRequest.Create("https://rajsahakarapp.rajasthan.gov.in/api/Services/GetDataFromProd?Table=" + Table );

        //    // If required by the server, set the credentials.
        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    // Get the response.
        //    WebResponse response = request.GetResponse();
        //    // Display the status.
        //    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        //    // Get the stream containing content returned by the server.
        //    // The using block ensures the stream is automatically closed.
        //    using (Stream dataStream = response.GetResponseStream())
        //    {
        //        // Open the stream using a StreamReader for easy access.
        //        StreamReader reader = new StreamReader(dataStream);
        //        // Read the content.
        //        string responseFromServer = reader.ReadToEnd();
        //        var data = JsonConvert.DeserializeObject<Object>(responseFromServer);
        //        return data.ToString();
        //        //return responseFromServer.ToString().Replace("\r","").Replace("\t", "");
        //        ds = JsonConvert.DeserializeObject<DataTable>(data.ToString());
        //        //if (ds.Rows.Count > 0)
        //        //{
        //        //    return ds.Rows[0][0].ToString();
        //        //}
        //    }
        //    // Close the response.
        //    response.Close();
        //    return "";
        //}

    }
}