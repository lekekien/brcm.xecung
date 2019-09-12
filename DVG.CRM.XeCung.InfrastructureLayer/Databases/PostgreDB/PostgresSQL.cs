using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using IDataReader = System.Data.IDataReader;
using IDbCommand = System.Data.IDbCommand;
using IDbContext = DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.IDbContext;
using IsolationLevel = System.Data.IsolationLevel;
using ParameterDirection = System.Data.ParameterDirection;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb
{
    public class PostgresSQL : IDisposable, IDbContext
    {
        public enum DBPosition
        {
            Manual = -1,

            [Description("ConnectionString")]
            Default = 0,

            [Description("MasterConnection")]
            Master = 1,

            [Description("SlaveConnection")]
            Slave = 2,

            [Description("ExternalConnection")]
            External = 3
        }

        private static Dictionary<Type, DbType> typeMap;

        public static Dictionary<Type, DbType> TypeMap
        {
            get
            {
                if (typeMap == null)
                    CreateTypeMap();
                return typeMap;
            }
        }

        private static void CreateTypeMap()
        {
            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(System.DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(System.DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            //typeMap[typeof(System.Data.Linq.Binary)] = DbType.Binary;
        }

        /// <summary>
        /// Postgres is a class that will handle the postgres datbase.
        /// </summary>
        private string _connectionString;

        protected NpgsqlConnection _connection;
        protected DbTransaction _transaction;

        private static Dictionary<string, int> _dictConnection;
        protected DBPosition _dbPosition = DBPosition.Default;

        public PostgresSQL(bool isInit = true)
        {
            _connectionString = GetConnectionString(_dbPosition);

            if (null == _dictConnection)
            {
                _dictConnection = new Dictionary<string, int>();
            }

            if (isInit)
            {
                InitConnection();
            }
        }

        public PostgresSQL(string connectionString, bool isInit = true)
        {
            _connectionString = connectionString;
            //_connectionString = "Server=172.16.0.79;Port=5432;User Id=user_dev;Password=45-C8-2A-38-43-CA;Database=bcrm_bxh_thai_ddd;CommandTimeout=320";

            if (isInit)
            {
                InitConnection();
            }
        }

        public PostgresSQL(DBPosition dbPosition, bool isInit = true)
        {
            _dbPosition = dbPosition;

            _connectionString = GetConnectionString(_dbPosition);
            //_connectionString = "Server=172.16.0.79;Port=5432;User Id=user_dev;Password=45-C8-2A-38-43-CA;Database=bcrm_bxh_thai_ddd;CommandTimeout=320";

            if (isInit)
            {
                InitConnection();
            }
        }

        protected void InitConnection()
        {
            try
            {
                _connection = CreateConnection();
                _connection.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string GetConnectionString(DBPosition dbPosition, string defaultValue = "ConnectionString")
        {
            string connectionName = string.Empty;
            if (dbPosition == DBPosition.Manual)
            {
                if (!string.IsNullOrEmpty(defaultValue)) connectionName = defaultValue;
            }
            else
            {
                connectionName = StringUtils.GetEnumDescription(dbPosition);
            }

            if (string.IsNullOrEmpty(connectionName)) connectionName = "ConnectionString";

            return AppSettings.Instance.GetConnection(connectionName);
        }

        /// <summary>
        /// GetConnectionString - will get the connection string for use.
        /// </summary>
        /// <returns>The connection string</returns>
        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = GetConnectionString(_dbPosition);
            return _connectionString;
        }

        public NpgsqlConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(GetConnectionString());
            return conn;
        }

        /// <summary>
        /// Returns a SQL statement parameter name that is specific for the data provider.
        /// For example it returns ? for OleDb provider, or @paramName for MS SQL provider.
        /// </summary>
        /// <param name="paramName">The data provider neutral SQL parameter name.</param>
        /// <returns>The SQL statement parameter name.</returns>
        protected internal string CreateSqlParameterName(string paramName)
        {
            return "@" + paramName;
        }

        /// <summary>
        /// Creates a .Net data provider specific name that is used by the
        /// <see cref="AddParameter"/> method.
        /// </summary>
        /// <param name="baseParamName">The base name of the parameter.</param>
        /// <returns>The full data provider specific parameter name.</returns>
        protected string CreateCollectionParameterName(string baseParamName)
        {
            return "@" + baseParamName;
        }

        /// <summary>
        /// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
        /// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        public virtual IDataReader ExecuteReader(IDbCommand command)
        {
            return command.ExecuteReader();
        }

        ///// <summary>
        ///// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        ///// </summary>
        ///// <param name="commandType"></param>
        ///// <param name="commandText"></param>
        ///// <param name="parameters"></param>
        ///// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        //protected internal virtual IDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] parameters)
        //{
        //    var command = CreateCommand(commandType, commandText, parameters);
        //    return command.ExecuteReader();
        //}

        /// <summary>
        /// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
        /// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        public virtual int ExecuteNonQuery(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }

        ///// <summary>
        ///// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        ///// </summary>
        ///// <param name="commandType"></param>
        ///// <param name="commandText"></param>
        ///// <param name="parameters"></param>
        ///// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        //protected internal virtual int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] parameters)
        //{
        //    var command = CreateCommand(commandType, commandText, parameters);
        //    return command.ExecuteNonQuery();
        //}

        /// <summary>
        /// Map records from the DataReader
        /// </summary>
        /// <param name="reader">The <see cref="System.Data.IDataReader"/> object.</param>
        /// <returns>List entity of records.</returns>
        protected List<T> MapRecords<T>(IDataReader reader)
        {
            throw new Exception();
        }

        /// <summary>
        /// Map records from the DataReader
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> command.</param>
        /// <returns>List entity of records.</returns>
        public List<T> GetList<T>(IDbCommand command)
        {
            List<T> returnValue;
            using (IDataReader reader = this.ExecuteReader(command))
            {
                returnValue = MapRecords<T>(reader);
            }
            return returnValue;
        }

        public object GetFirtDataRecord(IDbCommand command)
        {
            object returnValue = null;
            using (IDataReader reader = this.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    returnValue = reader[0];
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Adds a new parameter to the specified command. It is not recommended that
        /// you use this method directly from your custom code. Instead use the
        /// <c>AddParameter</c> method of the PostgresSQL classes.
        /// </summary>
        /// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A reference to the added parameter.</returns>
        public IDbDataParameter AddParameter(NpgsqlCommand cmd, string paramName, object value)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            if (value is System.DateTime)
            {
                parameter.Value = (System.DateTime.MinValue == System.DateTime.Parse(value.ToString()) ? DBNull.Value : value);
            }
            else
            {
                parameter.Value = (value ?? DBNull.Value);
            }
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the specified command. It is not recommended that
        /// you use this method directly from your custom code. Instead use the
        /// <c>AddParameter</c> method of the PostgresSQL classes.
        /// </summary>
        /// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dataType">The DbType of the parameter.</param>
        /// <returns>A reference to the added parameter.</returns>
        public IDbDataParameter AddParameter(NpgsqlCommand cmd, string paramName, object value, DbType dataType)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            parameter.DbType = dataType;

            if (value is System.DateTime)
            {
                parameter.Value = (System.DateTime.MinValue == System.DateTime.Parse(value.ToString()) ? DBNull.Value : value);
            }
            else
            {
                parameter.Value = (value ?? DBNull.Value);
            }
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the specified command. It is not recommended that
        /// you use this method directly from your custom code. Instead use the
        /// <c>AddParameter</c> method of the PostgresSQL classes.
        /// </summary>
        /// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paraDirection">The direction of the parameter.</param>
        /// <returns>A reference to the added parameter.</returns>
        public IDbDataParameter AddParameter(IDbCommand cmd, string paramName, object value, ParameterDirection paraDirection)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            if (value is System.DateTime)
            {
                parameter.Value = (System.DateTime.MinValue == System.DateTime.Parse(value.ToString()) ? DBNull.Value : value);
            }
            else
            {
                parameter.Value = (value ?? DBNull.Value);
            }
            parameter.Direction = paraDirection;
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            CheckTransactionState(false);
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// Begins a new database transaction with the specified
        /// transaction isolation level.
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            CheckTransactionState(false);
            _transaction = _connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        /// <summary>
        /// Commits the current database transaction.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        public void CommitTransaction()
        {
            CheckTransactionState(true);
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// Rolls back the current transaction from a pending state.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="CommitTransaction"/>
        /// </summary>
        public void RollbackTransaction()
        {
            CheckTransactionState(true);
            _transaction.Rollback();
            _transaction = null;
        }

        // Checks the state of the current transaction
        private void CheckTransactionState(bool mustBeOpen)
        {
            if (mustBeOpen)
            {
                if (null == _transaction)
                    throw new InvalidOperationException("Transaction is not open.");
            }
            else
            {
                if (null != _transaction)
                    throw new InvalidOperationException("Transaction is already open.");
            }
        }

        /// <summary>
        /// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="sqlText">The text of the query.</param>
        /// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
        public NpgsqlCommand CreateCommand(string sqlText)
        {
            return CreateCommand(sqlText, false);
        }

        public NpgsqlCommand StoreProcedure(string sqlText)
        {
            return CreateCommand(sqlText, true);
        }

        public NpgsqlCommand StoreProcedureWithCurrentTransaction(string sqlText, ICondition condition = null)
        {
            var command = StoreProcedure(sqlText, condition);
            command.Transaction = (NpgsqlTransaction)_transaction;
            return command;
        }

        public NpgsqlCommand StoreProcedure(string sqlText, ICondition condition)
        {
            var command = StoreProcedure(sqlText);
            if (condition != null)
            {
                foreach (var key in condition.Conditions.Keys)
                {
                    AddParameter(command, key, condition.Conditions[key].Item2,
                        TypeMap[condition.Conditions[key].Item1]);
                }
            }
            return command;
        }

        public IList<T> Mapper<T>(NpgsqlDataReader reader, bool close = true) where T : class
        {
            try
            {
                IList<T> entities = new List<T>();

                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T item = default(T);
                        if (item == null)
                            item = Activator.CreateInstance<T>();
                        Mapper(reader, item);
                        entities.Add(item);
                    }

                    if (close)
                    {
                        reader.Close();
                    }
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Mapper<T>(IDataRecord reader, T entity) where T : class
        {
            Type type = typeof(T);

            if (entity != null)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i);
                    try
                    {
                        var propertyInfo = type.GetProperties().FirstOrDefault(info => info.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

                        if (propertyInfo != null)
                        {
                            if ((reader[i] != null) && (reader[i] != DBNull.Value))
                            {
                                propertyInfo.SetValue(entity, reader[i], null);
                            }
                            else
                            {
                                if (propertyInfo.PropertyType == typeof(System.DateTime) ||
                                    propertyInfo.PropertyType == typeof(System.DateTime?))
                                {
                                    propertyInfo.SetValue(entity, System.DateTime.MinValue, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(string))
                                {
                                    propertyInfo.SetValue(entity, string.Empty, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(bool) ||
                                    propertyInfo.PropertyType == typeof(bool?))
                                {
                                    propertyInfo.SetValue(entity, false, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(decimal) ||
                                    propertyInfo.PropertyType == typeof(decimal?))
                                {
                                    propertyInfo.SetValue(entity, decimal.Zero, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(double) ||
                                propertyInfo.PropertyType == typeof(double?))
                                {
                                    propertyInfo.SetValue(entity, double.Parse("0"), null);
                                }
                                else if (propertyInfo.PropertyType == typeof(float) ||
                           propertyInfo.PropertyType == typeof(float?))
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(short) ||
                           propertyInfo.PropertyType == typeof(short?))
                                {
                                    propertyInfo.SetValue(entity, short.Parse("0"), null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                            }
                        }
                        //else
                        //{
                        //    (entity as EntityBase)[fieldName] = reader[i];
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="sqlText">The text of the query.</param>
        /// <param name="procedure">Specifies whether the sqlText parameter is
        /// the name of a stored procedure.</param>
        /// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
        public NpgsqlCommand CreateCommand(string sqlText, bool procedure)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sqlText, _connection);
            cmd.CommandText = sqlText;
            if (procedure)
                cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public object GetParameterValueFromCommand(IDbCommand command, int paramterIndex)
        {
            var parameter = command.Parameters[paramterIndex] as SqlParameter;
            return parameter != null ? parameter.Value : null;
        }

        public virtual void Close()
        {
            if (null != _connection)
                _connection.Close();
        }

        public void Dispose()
        {
            Close();
            if (null != _connection)
                _connection.Dispose();
        }

        /// <summary>
        /// CheckDBConnectionInfo - will check the db connection information.
        /// </summary>
        //public override void CheckDBConnectionInfo()
        //{
        //	// check the arguments
        //	if (base.Server.Length == 0)
        //	{
        //		throw new Exception("Invalid Server Name");
        //	}
        //	// check the arguments
        //	if (base.DBName.Length == 0)
        //	{
        //		throw new Exception("Invalid Database Name");
        //	}
        //	// check the arguments
        //	if (base.SAUserName.Length == 0)
        //	{
        //		throw new Exception("Invalid SA Name");
        //	}
        //	// check the arguments
        //	if (base.SAPassword.Length == 0)
        //	{
        //		throw new Exception("Invalid SA Password");
        //	}
        //}
        /// <summary>
        /// GetUserPassword - get the user password.
        /// </summary>
        /// <param name="userName">the user to look up the paassword.</param>
        /// <returns>the password associated witht the user</returns>
        ///
    }
}