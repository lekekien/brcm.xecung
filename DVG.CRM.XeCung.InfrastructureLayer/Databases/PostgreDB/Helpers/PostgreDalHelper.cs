using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Logs;
using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb.Helpers
{
    public class PostgreDalHelper
    {
        public static string GetTableName<TEntity>() where TEntity : class
        {
            return typeof(TEntity).Name.Replace("Entity", string.Empty);
        }

        public static string GetDtoName<TDto>() where TDto : IDto
        {
            return typeof(TDto).Name.Replace("Dto", string.Empty).ToLower();
        }

        public static void ExecuteCommandEntityStore(string storeName, ICondition condition, IDbContext commandDbContext)
        {
            try
            {
                PostgresSQL db = ContextManager.GetContext();
                using (NpgsqlCommand command = db.StoreProcedureWithCurrentTransaction(storeName, condition))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        internal static void ExecuteCommandEntityStore<TId>(string storeName, TId id, IDbContext commandDbContext)
        {
            try
            {
                PostgresSQL db = commandDbContext as PostgresSQL;
                using (NpgsqlCommand command = db.StoreProcedureWithCurrentTransaction(storeName))
                {
                    db.AddParameter(command, "_id", id, PostgresSQL.TypeMap[id.GetType()]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public static void ExecuteCommandEntityStore<TEntity, TId>(string storeName, TEntity entity, IDbContext commandDbContext) where TEntity : IDbEntity<TId>
        {
            try
            {
                PostgresSQL db = commandDbContext as PostgresSQL;
                using (NpgsqlCommand command = db.StoreProcedureWithCurrentTransaction(storeName, entity))
                {
                    command.ExecuteNonQuery();
                    //  result = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Logger.FatalLog(ex);
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public static TId ExecuteScalarEntityStore<TEntity, TId>(string storeName, TEntity entity, IDbContext commandDbContext) where TEntity : IDbEntity<TId>
        {
            try
            {
                PostgresSQL db = commandDbContext as PostgresSQL;
                using (NpgsqlCommand command = db.StoreProcedureWithCurrentTransaction(storeName, entity))
                {
                    return (TId)command.ExecuteScalar();
                }
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
                using (PostgresSQL db = new PostgresSQL(PostgresSQL.DBPosition.Slave))
                {
                    using (NpgsqlCommand command = db.CreateCommand("select * from " + tableName))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var entities = db.Mapper<TEntity>(reader);
                                reader.Close();
                                return entities;
                            }
                        }
                    }
                }

                return new List<TEntity>();
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
                using (PostgresSQL db = new PostgresSQL(PostgresSQL.DBPosition.Slave))
                {
                    using (NpgsqlCommand command = db.CreateCommand("select * from " + tableName + " where " + idName + " = " + id))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var entities = db.Mapper<TEntity>(reader);
                                reader.Close();
                                return entities.FirstOrDefault();
                            }
                        }
                    }
                }

                return default(TEntity);
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
                using (PostgresSQL db = new PostgresSQL(PostgresSQL.DBPosition.Slave))
                {
                    using (NpgsqlCommand command = db.StoreProcedure(storeName, condition))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                entities = db.Mapper<T>(reader);
                                reader.Close();
                            }
                        }
                    }
                }
                return entities;
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
                using (PostgresSQL db = new PostgresSQL(PostgresSQL.DBPosition.Slave))
                {
                    using (NpgsqlCommand command = db.StoreProcedure(storeName, condition))
                    {
                        return (int)command.ExecuteScalar();
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