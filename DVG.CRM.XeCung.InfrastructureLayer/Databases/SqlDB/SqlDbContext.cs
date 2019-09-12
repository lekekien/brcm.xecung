
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using FluentData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB
{
    public class SqlDbContext: IDisposable, DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.IDbContext
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

        //protected NpgsqlConnection _connection;
        protected FluentData.IDbContext _connection;
        //protected DbTransaction _transaction;

        private static Dictionary<string, int> _dictConnection;
        protected DBPosition _dbPosition = DBPosition.Default;

        public SqlDbContext(bool isInit = true)
        {
            _connectionString = GetConnectionString(_dbPosition);
            //_connectionString = "Server=172.16.0.79;Port=5432;User Id=user_dev;Password=45-C8-2A-38-43-CA;Database=bcrm_bxh_thai_ddd;CommandTimeout=320";

            if (null == _dictConnection)
            {
                _dictConnection = new Dictionary<string, int>();
            }

            if (isInit)
            {
                InitConnection();
            }
        }

        public SqlDbContext(string connectionString, bool isInit = true)
        {
            _connectionString = connectionString;
            //_connectionString = "Server=172.16.0.79;Port=5432;User Id=user_dev;Password=45-C8-2A-38-43-CA;Database=bcrm_bxh_thai_ddd;CommandTimeout=320";

            if (isInit)
            {
                InitConnection();
            }
        }

        public SqlDbContext(DBPosition dbPosition, bool isInit = true)
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

        public IDbContext CreateConnection()
        {
            var conn = new DbContext().ConnectionString(_connectionString, new SqlServerProvider());
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
        /// Begins a new database transaction.
        /// </summary>
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// <returns>An object representing the new transaction.</returns>
        public void BeginTransaction()
        {
           _connection.UseTransaction(true);
        }

        /// <summary>
        /// Begins a new database transaction with the specified
        /// transaction isolation level.
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <returns>An object representing the new transaction.</returns>
        public void BeginTransaction(FluentData.IsolationLevel isolationLevel)
        {
            _connection.UseTransaction(true);
            _connection.IsolationLevel(isolationLevel);
        }

        /// <summary>
        /// Commits the current database transaction.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        public void CommitTransaction()
        {
            _connection.Commit();
        }

        /// <summary>
        /// Rolls back the current transaction from a pending state.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="CommitTransaction"/>
        /// </summary>
        public void RollbackTransaction()
        {
            _connection.Rollback();
        }

        //// Checks the state of the current transaction
        //private void CheckTransactionState(bool mustBeOpen)
        //{
        //    if (mustBeOpen)
        //    {
        //        if (null == _transaction)
        //            throw new InvalidOperationException("Transaction is not open.");
        //    }
        //    else
        //    {
        //        if (null != _transaction)
        //            throw new InvalidOperationException("Transaction is already open.");
        //    }
        //}

        /// <summary>
        /// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="sqlText">The text of the query.</param>
        /// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
        public IStoredProcedureBuilder StoreProcedure(string storeName)
        {
            return _connection.StoredProcedure(storeName);
        }
        public FluentData.IDbCommand SqlCommand(string sqlText)
        {
            return _connection.Sql(sqlText);
        }
        //public IStoredProcedureBuilder StoreProcedure(string sqlText)
        //{
        //    return CreateStoreCommand(sqlText);
        //}

        public IStoredProcedureBuilder StoreProcedureWithCurrentTransaction(string storeName, ICondition condition = null)
        {
            var command = StoreProcedure(storeName, condition);
            _connection.UseTransaction(true);
            return command;
        }

        public IStoredProcedureBuilder StoreProcedure(string storeName, ICondition condition)
        {
            var command = StoreProcedure(storeName);
            if (condition != null)
            {
                foreach (var key in condition.Conditions.Keys)
                {
                    command.Parameter(key, condition.Conditions[key].Item2);
                }
            }
            return command;
        }
        public virtual void Close()
        {
            if (null != _connection)
                _connection.Dispose();
        }
        public void Dispose()
        {
            if (null != _connection)
                _connection.Dispose();
        }
    }
}
