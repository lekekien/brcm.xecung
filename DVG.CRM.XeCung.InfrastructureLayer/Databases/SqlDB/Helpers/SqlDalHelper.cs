using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Logs;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB.Helpers
{
    public class SqlDalHelper
    {

        public static string GetTableName<TEntity>() where TEntity : class
        {
            return typeof(TEntity).Name.Replace("Entity", string.Empty);
        }

        public static string GetDtoName<TDto>() where TDto : IDto
        {
            return typeof(TDto).Name.Replace("Dto", string.Empty).ToLower();
        }

        public static void ExecuteCommandEntityStore(string storeName, ICondition condition, XeCung.InfrastructureLayer.Databases.Base.IDbContext commandDbContext)
        {
            try
            {
                var db = commandDbContext as SqlDbContext;
                var command = db.StoreProcedure(storeName, condition);
                command.Execute();
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        internal static void ExecuteCommandEntityStore<TId>(string storeName, TId id, XeCung.InfrastructureLayer.Databases.Base.IDbContext commandDbContext)
        {
            try
            {

                var db = commandDbContext as SqlDbContext;
                var command = db.StoreProcedure(storeName);
                command.Parameter("id", id);
                command.Execute();
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public static void ExecuteCommandEntityStore<TEntity, TId>(string storeName, TEntity entity, XeCung.InfrastructureLayer.Databases.Base.IDbContext commandDbContext) where TEntity : IDbEntity<TId>
        {
            try
            {
                var db = commandDbContext as SqlDbContext;
                var command = db.StoreProcedureWithCurrentTransaction(storeName, entity);
                command.Execute();              
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public static TId ExecuteScalarEntityStore<TEntity, TId>(string storeName, TEntity entity, XeCung.InfrastructureLayer.Databases.Base.IDbContext commandDbContext) where TEntity : IDbEntity<TId>
        {
            try
            {
                var db = commandDbContext as SqlDbContext;
                var command = db.StoreProcedureWithCurrentTransaction(storeName, entity);
                return (TId)command.QuerySingle<TId>();
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public static IEnumerable<TEntity> GetAll<TEntity>(string tableName) where TEntity : class
        {
            try
            {
                using (SqlDbContext db = new SqlDbContext(SqlDbContext.DBPosition.Slave))
                {
                    using (var command = db.SqlCommand("select * from " + tableName))
                    {
                        return command.QueryMany<TEntity>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", "Get " + tableName + " by Id", ex));
            }
        }

        public static TEntity GetById<TEntity, TId>(string tableName, TId id, string idName) where TEntity : class, IDbEntity<TId>
        {
            try
            {
                using (SqlDbContext db = new SqlDbContext(SqlDbContext.DBPosition.Slave))
                {
                    using (var command = db.SqlCommand("select * from " + tableName + " where " + idName + " = " + id))
                    {
                        return command.QuerySingle<TEntity>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", "Get " + tableName + " by Id", ex));
            }
        }

        public static IEnumerable<T> List<T>(string storeName, ICondition condition) where T : class, IDto
        {
            try
            {
                var type = typeof(T);
                IEnumerable<T> entities = new List<T>();
                using (SqlDbContext db = new SqlDbContext(SqlDbContext.DBPosition.Slave))
                {
                    using (var command = db.StoreProcedure(storeName, condition))
                    {
                        return command.QueryMany<T>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
        public static ListWithTotalRow<T> ListWithTotalRow<T>(string storeName, ICondition condition) where T : class, IDto
        {
            try
            {
                ListWithTotalRow<T> output = new ListWithTotalRow<T>();
                IEnumerable<T> entities = new List<T>();
                using (SqlDbContext db = new SqlDbContext(SqlDbContext.DBPosition.Slave))
                {
                    using (var command = db.StoreProcedure(storeName, condition))
                    {
                        output.List = command.ParameterOut("TotalRow", DataTypes.Int32).QueryMany<T>();
                        output.TotalRow = command.ParameterValue<int>("TotalRow");
                        return output;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
        public static int CountTotalRecord(string storeName, ICondition condition)
        {
            try
            {
                using (SqlDbContext db = new SqlDbContext(SqlDbContext.DBPosition.Slave))
                {
                    using (var command = db.StoreProcedure(storeName, condition))
                    {
                        return (int)command.QuerySingle<int>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
    }
}
