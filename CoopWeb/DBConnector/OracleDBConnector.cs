using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Linq;
using System.Web;


namespace Cooperative.DBConnector
{
    public class DBFactoryOracle : IDisposable
    {
        OracleConnection _con;
        OracleCommand _cmd;
        OracleDataAdapter _da;
        //OracleTransaction _tran;
        private readonly string ConnectionString = "wmsprod";

        /// <summary>
        /// Default constructor for DBFactoryOracle
        /// </summary>
        public DBFactoryOracle()
        {
            // Default connection string set from
            // if want deferent connection string then try parameterised constructor
            _con = new OracleConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        }

        ~DBFactoryOracle()
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
                    if (_con.State == System.Data.ConnectionState.Open)
                        _con.Close();
                    _con.Dispose();
                }

            }
        }

        /// <summary>
        /// A Parametrised constructor
        /// </summary>
        /// <param name="ConfigString">Specify the connection string section name value for which the connection string will be used.</param>
        public DBFactoryOracle(String ConnectionString)
        {
            _con = new OracleConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        }

        // <summary>
        /// Connection object property which is readonly.
        /// </summary>
        public OracleConnection GetConnection
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
        public OracleCommand CreateCommand(string commandText, DBFactoryCommandType cmdType)
        {
            try
            {
                if (_con == null)
                {
                    throw new Exception("Connection object is not initialized");
                }
                _cmd = new OracleCommand();
                _cmd.CommandText = commandText;
                _cmd.CommandTimeout = 0;
                _cmd.Connection = _con;
                if (Convert.ChangeType(cmdType, cmdType.GetType()).Equals(DBFactoryCommandType.PROCEDURE_COMMAND))
                {
                    _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    _cmd.CommandType = System.Data.CommandType.Text;
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
                OracleParameter param = new OracleParameter();
                param.ParameterName = ParameterName;
                #region Parameter Type
                switch ((DBFactoryParameterType)Convert.ChangeType(parameterType, parameterType.GetType()))
                {
                    case DBFactoryParameterType.Byte:
                        param.OracleDbType = OracleDbType.Byte;
                        param.Value = Convert.ToInt32(value);
                        break;
                    //case DBFactoryParameterType.Sbyte:
                    //    //param.OracleDbType = OracleDbType.Int16;
                    //    param.Value = Convert.ToInt64(value);
                    //    break;
                    //case DBFactoryParameterType.Short:
                    //    //param.OracleDbType = OracleDbType.Int16;
                    //    param.Value = Convert.ToInt64(value);
                    //    break;
                    //case DBFactoryParameterType.UShort:
                    //    //param.OracleDbType = OracleDbType.Int16;
                    //    param.Value = Convert.ToBoolean(value);
                    //    break;
                    case DBFactoryParameterType.Int:
                        param.OracleDbType = OracleDbType.Int32;
                        param.Value = Convert.ToString(value);
                        break;
                    case DBFactoryParameterType.UInt:
                        param.OracleDbType = OracleDbType.Int32;
                        param.Value = Convert.ToString(value);
                        break;
                    case DBFactoryParameterType.Long:
                        param.OracleDbType = OracleDbType.Int64;
                        param.Value = Convert.ToDateTime(value);
                        break;
                    case DBFactoryParameterType.ULong:
                        param.OracleDbType = OracleDbType.Int64;
                        param.Value = Convert.ToDateTime(value);
                        break;
                    //case DBFactoryParameterType.Float:
                    //    param.OracleDbType = OracleDbType.Int16;
                    //    param.Value = Convert.ToSingle(value);
                    //    break;
                    //case DBFactoryParameterType.Double:
                    //    param.OracleDbType = OracleDbType.Int64;
                    //    param.Value = Convert.ToDouble(value);
                    //    break;
                    //case DBFactoryParameterType.Decimal:
                    //    //param.OracleDbType = OracleDbType.Int16;
                    //    param.Value = Convert.ToDouble(value);
                    //    break;
                    case DBFactoryParameterType.Char:
                        param.OracleDbType = OracleDbType.Char;
                        param.Value = Convert.ToDecimal(value);
                        break;
                    case DBFactoryParameterType.NChar:
                        param.OracleDbType = OracleDbType.NChar;
                        param.Value = Convert.ToDecimal(value);
                        break;
                    case DBFactoryParameterType.VARCHAR:
                        param.OracleDbType = OracleDbType.Varchar2;
                        param.Value = Convert.ToByte(value);
                        break;
                    case DBFactoryParameterType.NVarchar:
                        param.OracleDbType = OracleDbType.NVarchar2;
                        //param.Value = Convert.ToByte(value);
                        break;
                    //case DBFactoryParameterType.Bool:
                    //    param.OracleDbType = OracleDbType.Byte;
                    //    param.Value = Convert.ToByte(value);
                    //    break;
                    case DBFactoryParameterType.Date:
                        param.OracleDbType = OracleDbType.Date;
                        param.Value = Convert.ToByte(value);
                        break;
                    case DBFactoryParameterType.Time:
                        param.OracleDbType = OracleDbType.TimeStamp;
                        param.Value = Convert.ToByte(value);
                        break;
                    //case DBFactoryParameterType.DateTime:
                    //    param.OracleDbType = OracleDbType.Date;
                    //    param.Value = Convert.ToByte(value);
                    //    break;
                }
                #endregion
                #region Parameter Direction
                switch ((DBFactoryParameterDirection)Convert.ChangeType(parameterDirection, parameterDirection.GetType()))
                {
                    case DBFactoryParameterDirection.IN:
                        param.Direction = System.Data.ParameterDirection.Input;
                        break;
                    case DBFactoryParameterDirection.INOUT:
                        param.Direction = System.Data.ParameterDirection.InputOutput;
                        break;
                    case DBFactoryParameterDirection.OUT:
                        param.Direction = System.Data.ParameterDirection.Output;
                        break;
                    case DBFactoryParameterDirection.RETURN:
                        param.Direction = System.Data.ParameterDirection.ReturnValue;
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
                if (_con.State == System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                    _con.Open();
                }
                else if (_con.State == System.Data.ConnectionState.Closed)
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
                if (_con.State == System.Data.ConnectionState.Connecting || _con.State == System.Data.ConnectionState.Executing || _con.State == System.Data.ConnectionState.Fetching || _con.State == System.Data.ConnectionState.Open)
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
                else if (_con.State != System.Data.ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                return _cmd.ExecuteNonQuery();
            }
            catch //(Exception ex)
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
                else if (_con.State != System.Data.ConnectionState.Open)
                {
                    throw new Exception("Connection is not opened for the execution");
                }
                return _cmd.ExecuteScalar();
            }
            catch //(Exception ex)
            {
                throw;
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
                _da = new OracleDataAdapter(_cmd);
                _da.Fill(dt);
                return dt;
            }
            catch //(Exception ex)
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
                _da = new OracleDataAdapter(_cmd);
                _da.Fill(ds);
                return ds;
            }
            catch //(Exception ex)
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
                foreach (OracleParameter param in _cmd.Parameters)
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
        //public OracleTransaction GetTransaction
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_tran == null)
        //            {
        //                throw new Exception("Transaction is not initialized");
        //            }
        //            return _tran;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        /// <summary>
        /// Start transaction for the current connection
        /// </summary>
        /// <returns>Returns the trasaction object that can be used to set the transaction for other command objects</returns>
        public OracleTransaction StartTransaction()
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
        public void SetTransactionForCommand(OracleTransaction tran, OracleConnection connection)
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
        //public void CommitTransaction()
        //{
        //    try
        //    {
        //        if (_tran == null)
        //        {
        //            throw new Exception("Transaction is not started yet");
        //        }
        //        _tran.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Rollback transaction on faliure of any command execution
        /// </summary>
        /// <param name="savepointname">Specify save point name up to which transaction  needs to be rolledback. Specifying empty string will rollback the complete transaction.</param>
        //public void RollbackTransaction(string savepointname = "")
        //{
        //    try
        //    {
        //        if (_tran == null)
        //        {
        //            throw new Exception("Transaction is not started yet");
        //        }
        //        if (savepointname.Length == 0)
        //        {
        //            _tran.Rollback();
        //        }
        //        else
        //        {
        //            //_tran.Rollback(savepointname);
        //            _tran.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Add save point to the current Transaction object.
        /// </summary>
        /// <param name="SavePointName">Save point name</param>
        //public void AddTransactionSavePoint(string SavePointName)
        //{
        //    try
        //    {
        //        if (_tran == null)
        //        {
        //            throw new Exception("Transaction is not started yet");
        //        }
        //        _tran.save(SavePointName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
        /// Unsigned integer 8 bytes range
        /// Range 0 to 255
        /// </summary>
        Byte,

        /// <summary>
        /// Signed integer 8 bytes range
        /// Range -128 to 127
        /// </summary>
        Sbyte,

        /// <summary>
        /// Signed integer Signed integer 16 bit range	
        /// Range -32,768 to 32,767
        /// </summary>
        Short,

        /// <summary>
        /// Unsigned integer Signed integer 16 bit range
        /// Range 0 to 65,535
        /// </summary>
        UShort,

        /// <summary>
        /// Integer values for 32 bit range
        /// Range -2,147,483,648 to 2,147,483,647
        /// </summary>
        Int,


        /// <summary>
        /// Unsigned integer values for 32 bit range
        /// Range 0 to 4294967295
        /// </summary>
        UInt,

        /// <summary>
        /// Signed integer values for 64 bit range
        /// Range -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        /// </summary>
        Long,

        /// <summary>
        /// Unsigned integer values for 64 bit range
        /// Range 0 to 18,446,744,073,709,551,615
        /// </summary>
        ULong,

        /// <summary>
        /// Single-precision floating point values for 32 bit range
        /// Range -3.402823e38 to 3.402823e38
        /// </summary>
        Float,

        /// <summary>
        /// Double-precision floating point  values for 64 bit range
        /// Range -1.79769313486232e308 to 1.79769313486232e308
        /// </summary>
        Double,

        /// <summary>
        /// A single character 16 bit range
        /// </summary>
        Char,

        /// <summary>
        /// A single Unicode character 16 bit range
        /// </summary>
        NChar,

        /// <summary>
        /// Logical Boolean type True or False 8 bit	
        /// </summary>
        Bool,

        /// <summary>
        /// Precise fractional or integral type that can represent decimal numbers with 29 significant digits
        /// for 128 bit range
        /// Range (+ or -)1.0 x 10e-28 to 7.9 x 10e28
        /// </summary>
        Decimal,


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
        /// Range 0:00:00 am 1/1/01 to 11:59:59 pm 12/31/9999
        /// </summary>
        DateTime,

        /// <summary>
        /// Time Only
        /// </summary>
        Time
    }
}