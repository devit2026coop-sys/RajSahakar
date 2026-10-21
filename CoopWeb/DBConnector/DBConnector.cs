using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace Cooperative.DBConnector
{
    public class DBFactoryMSSQL : IDisposable
    {
        SqlConnection _con;
        SqlCommand _cmd;
        SqlDataAdapter _da;
        SqlTransaction _tran;
        private readonly string ConnectionString = "wmsprod";

        /// <summary>
        /// Default constructor for DBFactoryMSSQL
        /// </summary>
        public DBFactoryMSSQL()
        {
            _con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        }

        ~DBFactoryMSSQL()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                this.SaveSqlCommandText();
                _da = new SqlDataAdapter(_cmd);
                _da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {

                dt.Dispose();
                throw;
            }
            finally
            {
            }
        }
        public string GetStringValueNew()
        {
            DataTable dt = new DataTable();
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                _da = new SqlDataAdapter(_cmd);
                _da.Fill(dt);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                dt.Dispose();
                throw;
            }
            finally
            {

            }
        }
        public void SaveSqlCommandText(SqlCommand cmd = null)
        {
            DataTable objdt = new DataTable();
            SqlConnection objcon = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
            SqlCommand objcmdstr = new SqlCommand("Select dbo.fn_getKeyValue('LogSqlCommand') KeyValue", objcon);
            SqlDataAdapter objda = new SqlDataAdapter(objcmdstr);
            objda.Fill(objdt);

            if (objdt.Rows[0][0].ToString() == "T")
            {
                if (cmd == null)
                    cmd = this._cmd;
                DBFactoryMSSQL conn = new DBFactoryMSSQL();
                SqlConnection sqlConnection = conn.GetConnection;
                sqlConnection.Open();
                try
                {
                    string cmdtxt = "CommandType is not stored procedure type.";
                    if (cmd.CommandType == CommandType.StoredProcedure)
                    {
                        cmdtxt = "Exec " + cmd.CommandText + " ";
                        foreach (SqlParameter item in cmd.Parameters)
                        {
                            cmdtxt += item.ParameterName + "=''" + item.SqlValue + "'',";
                        }
                    }
                    SqlCommand cmdstr = new SqlCommand();
                    cmdstr.CommandText = "INSERT INTO SPCallingTracker (ProcedureName,Parameter) values (N'FromWeb',N'" + cmdtxt + "')";
                    cmdstr.CommandType = CommandType.Text;
                    cmdstr.Connection = sqlConnection;
                    cmdstr.ExecuteScalar();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                if (_cmd != null)
                {
                    _cmd.Dispose();
                }
                if (_con != null)
                {
                    if (_con.State == ConnectionState.Open)
                        _con.Close();
                    _con.Dispose();
                }

            }
        }

        /// <summary>
        /// A Parametrised constructor
        /// </summary>
        /// <param name="ConfigString">Specify the connection string section name value for which the connection string will be used.</param>
        public DBFactoryMSSQL(String ConnectionString)
        {
            _con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        }

        // <summary>
        /// Connection object property which is readonly.
        /// </summary>
        public SqlConnection GetConnection
        {
            get
            {
                try
                {
                    if (_con == null)
                    {
                        throw new Exception("Connection object is not intialised. Initalise it with connection string of config file");
                    }
                    else if (_con.ConnectionString.Length == 0)
                    {
                        throw new Exception("Connection string is not initialised");
                    }
                    return _con;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Create command that needs to be executed for the execution operation.
        /// </summary>
        /// <param name="commandText">command string or stored procedure name</param>
        /// <param name="cmdType">type of command which is either text or stored procedure</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string commandText, DBFactoryCommandType cmdType)
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                _cmd = new SqlCommand();
                _cmd.CommandText = commandText;
                _cmd.CommandTimeout = 0;
                _cmd.Connection = _con;
                if (Convert.ChangeType(cmdType, cmdType.GetType()).Equals(DBFactoryCommandType.PROCEDURE_COMMAND))
                {
                    _cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    _cmd.CommandType = CommandType.Text;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cmd;
        }

        /// <summary>
        /// Use this method when the command object is required to pass the Parameters
        /// </summary>
        /// <param name="ParameterName">Name of the parameter</param>
        /// <param name="parameterType">Parameter datatype</param>
        /// <param name="value">value of the parameter</param>
        /// <param name="parameterDirection">Direction of the parameter, default is 'IN' </param>
        public void AddParameterToCommand(string ParameterName, DBFactoryParameterType parameterType, object value, DBFactoryParameterDirection parameterDirection = DBFactoryParameterDirection.IN)
        {
            try
            {
                if (_cmd == null)
                {
                    throw new Exception("Command Object is not initalized yet.");
                }
                SqlParameter param = new SqlParameter();
                param.ParameterName = ParameterName;
                #region Parameter Type
                switch ((DBFactoryParameterType)Convert.ChangeType(parameterType, parameterType.GetType()))
                {
                    case DBFactoryParameterType.Int:
                        param.SqlDbType = SqlDbType.Int;
                        param.Value = Convert.ToInt32(value);
                        break;
                    case DBFactoryParameterType.BigInt:
                        param.SqlDbType = SqlDbType.BigInt;
                        param.Value = Convert.ToInt64(value);
                        break;
                    //case DBFactoryParameterType.Numeric:
                    //    param.SqlDbType = SqlDbType.Numeric;
                    //    param.Value = value;
                    //    break;
                    case DBFactoryParameterType.Bit:
                        param.SqlDbType = SqlDbType.Bit;
                        param.Value = Convert.ToBoolean(value);
                        break;
                    case DBFactoryParameterType.VARCHAR:
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Value = Convert.ToString(value);
                        break;
                    case DBFactoryParameterType.NVarchar:
                        param.SqlDbType = SqlDbType.NVarChar;
                        param.Value = Convert.ToString(value);
                        break;
                    case DBFactoryParameterType.Date:
                        param.SqlDbType = SqlDbType.Date;
                        param.Value = Convert.ToDateTime(value);
                        break;
                    case DBFactoryParameterType.DateTime:
                        param.SqlDbType = SqlDbType.DateTime;
                        param.Value = Convert.ToDateTime(value);
                        break;
                    case DBFactoryParameterType.Time:
                        param.SqlDbType = SqlDbType.Time;
                        param.Value = Convert.ChangeType(value, value.GetType());

                        break;
                    case DBFactoryParameterType.Float:
                        param.SqlDbType = SqlDbType.Float;
                        param.Value = Convert.ToSingle(value);
                        break;
                    case DBFactoryParameterType.Double:
                        param.SqlDbType = SqlDbType.Decimal;
                        param.Value = Convert.ToDouble(value);
                        break;
                    case DBFactoryParameterType.Money:
                        param.SqlDbType = SqlDbType.Real;
                        param.Value = Convert.ToDecimal(value);
                        break;
                    case DBFactoryParameterType.Binary:
                        param.SqlDbType = SqlDbType.Binary;
                        param.Value = Convert.ToByte(value);
                        break;
                }
                #endregion
                #region Parameter Direction
                switch ((DBFactoryParameterDirection)Convert.ChangeType(parameterDirection, parameterDirection.GetType()))
                {
                    case DBFactoryParameterDirection.IN:
                        param.Direction = ParameterDirection.Input;
                        break;
                    case DBFactoryParameterDirection.INOUT:
                        param.Direction = ParameterDirection.InputOutput;
                        break;
                    case DBFactoryParameterDirection.OUT:
                        param.Direction = ParameterDirection.Output;
                        break;
                    case DBFactoryParameterDirection.RETURN:
                        param.Direction = ParameterDirection.ReturnValue;
                        break;
                }
                #endregion
                _cmd.Parameters.Add(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Use this method to open connection explicitly
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                if (_con.State == ConnectionState.Closed)
                {
                    _con.Close();
                    _con.Open();
                }
                else if (_con.State == ConnectionState.Closed)
                {
                    _con.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Use this method to close the connection explicitly.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                if (_con.State == ConnectionState.Connecting || _con.State == ConnectionState.Executing || _con.State == ConnectionState.Fetching || _con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to change the Database for the current connection. This requires that new DB must reside on the same server.
        /// </summary>
        /// <param name="DBName">Database name for which connection needs to be opened and that</param>
        public void ChangeDBWithCurrentConnection(string DBName)
        {
            try
            {
                _con.ChangeDatabase(DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute the command to make changes to the DB which include Insert,Update, Delete or function execution
        /// </summary>
        /// <returns>No of rows affected</returns>
        public int ExecuteCommand()
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Execute the command to make changes to the DB which will return the scalar value from the DB
        /// </summary>
        /// <returns>Scalar value returned by the DB</returns>
        public object ExecuteScalarCommand()
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                return _cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        /// <summary>
        /// Execute the command to make Query which call in sql db
        /// </summary>
        /// <returns>Execution query</returns>
        public string getSqlCommandText(SqlCommand cmd = null)
        {
            try
            {
                if (cmd != null)
                    this._cmd = cmd;
                string cmdtxt = "CommandType is not stored procedure type.";
                if (this._cmd.CommandType == CommandType.StoredProcedure)
                {
                    cmdtxt = "Exec " + this._cmd.CommandText + " ";
                    foreach (SqlParameter item in this._cmd.Parameters)
                    {
                        cmdtxt += item.ParameterName + "='" + item.SqlValue + "',";
                    }
                }
                return cmdtxt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Fill the DataTable and return the same.
        /// </summary>
        /// <returns>Datatable returne from the DB</returns>
        public DataTable GetTable()
        {
            DataTable dt = new DataTable();
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                _da = new SqlDataAdapter(_cmd);
                _da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Fill the DataSet and return the same.
        /// </summary>
        /// <returns>Dataset with data returned from the DB</returns>
        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                _da = new SqlDataAdapter(_cmd);
                _da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                ds.Dispose();
                throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Use this method to get value of parameter
        /// </summary>
        /// <param name="parameterName">Name of Parameter for which you want to get the value</param>
        /// <returns>returns the value of specified parameter and will be null if the parameter is not found</returns>
        public object GetParameterValue(string parameterName)
        {
            object returnValue = null;
            try
            {
                foreach (SqlParameter param in _cmd.Parameters)
                {
                    if (param.ParameterName.ToLower().Equals(parameterName))
                    {
                        returnValue = param.Value;
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get readonly trasaction object 
        /// </summary>
        public SqlTransaction GetTransaction
        {
            get
            {
                try
                {
                    if (_tran == null)
                    {
                        throw new Exception("Transaction is not initialized");
                    }
                    return _tran;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Start transaction for the current connection
        /// </summary>
        /// <returns>Returns the trasaction object that can be used to set the transaction for other command objects</returns>
        public SqlTransaction StartTransaction()
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                return _con.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set the transaction and connection
        /// </summary>
        /// <param name="tran">Tranasaction object obtained from GetTransaction property for current object</param>
        /// <param name="connection">Connection object obtained from GetConnection property for current connection</param>
        public void SetTransactionForCommand(SqlTransaction tran, SqlConnection connection)
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                else if (_con.State != ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                else if (_cmd == null)
                {
                    throw new Exception("Command object is not initialized");
                }
                _cmd.Connection = connection;
                _cmd.Transaction = tran;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Commit transaction after complition of successfull transaction
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (_tran == null)
                {
                    throw new Exception("Transaction is not started yet");
                }
                _tran.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Rollback transaction on faliure of any command execution
        /// </summary>
        /// <param name="savepointname">Specify save point name up to which transaction  needs to be rolledback. Specifying empty string will rollback the complete transaction.</param>
        public void RollbackTransaction(string savepointname = "")
        {
            try
            {
                if (_tran == null)
                {
                    throw new Exception("Transaction is not started yet");
                }
                if (savepointname.Length == 0)
                {
                    _tran.Rollback();
                }
                else
                {
                    _tran.Rollback(savepointname);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add save point to the current Transaction object.
        /// </summary>
        /// <param name="SavePointName">Save point name</param>
        public void AddTransactionSavePoint(string SavePointName)
        {
            try
            {
                if (_tran == null)
                {
                    throw new Exception("Transaction is not started yet");
                }
                _tran.Save(SavePointName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// Type of command to be used by the command object; Procedure is for Stored Procedure and Text is for inline-sql command
    /// </summary>
    public enum DBFactoryCommandType
    {
        /// <summary>
        /// To be used for Stored Procedure
        /// </summary>
        PROCEDURE_COMMAND,
        /// <summary>
        /// To be used for inline string command
        /// </summary>
        TEXT_COMMAND
    }

    /// <summary>
    /// Parameter Direction for the parameter 
    /// </summary>
    public enum DBFactoryParameterDirection
    {
        /// <summary>
        /// Takes values to the DB and does not reflect changes done at DB side
        /// </summary>       
        IN,
        /// <summary>
        /// Parameter doesn't take any value to the DB but it can get back the value from the DB. Make sure to provide DataType and Size
        /// </summary>
        OUT,
        /// <summary>
        /// Parameter takes value to the DB and can get back the value from the DB. Make sure to provide DataType and Size
        /// </summary>
        INOUT,
        /// <summary>
        /// Parameter used to fatch returned value from the stored procedure
        /// </summary>
        RETURN
    }

    /// <summary>
    /// Specifies the Parameter Data Type
    /// </summary>
    public enum DBFactoryParameterType
    {
        /// <summary>
        /// Integer values for 32 bit range
        /// </summary>
        Int,
        /// <summary>
        /// Integer values for 64 bit range
        /// </summary>
        BigInt,
        /// <summary>
        /// Numeric data type specifying Integer and Fractional part
        /// </summary>
        Numeric,
        /// <summary>
        /// Boolean Data Type
        /// </summary>
        Bit,
        /// <summary>
        /// Takes string values with variable length string
        /// </summary>
        VARCHAR,
        /// <summary>
        /// Takes string values with variable length string for UTF-8 String
        /// </summary>
        NVarchar,
        /// <summary>
        /// Date part only. DB will store date and time with time part as 12 noon
        /// </summary>
        Date,
        /// <summary>
        /// Date with time part
        /// </summary>
        DateTime,
        Time,
        Float,
        /// <summary>
        /// Float value with double pricision
        /// </summary>
        Double,
        Money,
        Binary
    }
}