using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb.Helpers;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb.Queries
{
    public class EntityQueryDal<TDbEntity, TId> : IEntityQueryDal<TDbEntity, TId> where TDbEntity : class, IDbEntity<TId>
    {
        private readonly string _tableName = PostgreDalHelper.GetTableName<TDbEntity>().ToLower();
        private readonly string _funcPrefix = AppSettings.Instance.GetString("FuncPrefix");

        public IEnumerable<TDbEntity> GetAll()
        {
            return PostgreDalHelper.GetAll<TDbEntity>(_tableName);
        }

        public TDbEntity GetById(TId id)
        {
            var idName = Activator.CreateInstance<TDbEntity>().IdName.ToLower();
            return PostgreDalHelper.GetById<TDbEntity, TId>(_tableName, id, idName);
        }

        public IEnumerable<TDbEntity> List(ICondition condition)
        {
            string conditionName = (condition is TDbEntity) ? string.Empty : condition.GetType().Name.ToLower().Replace(_tableName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _tableName + "_getlist" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return PostgreDalHelper.List<TDbEntity>(storeName, condition);
        }

        public int CountTotalRecord(ICondition condition)
        {
            string conditionName = (condition is TDbEntity) ? string.Empty : condition.GetType().Name.ToLower().Replace(_tableName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _tableName + "_getcount" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return PostgreDalHelper.CountTotalRecord(storeName, condition);
        }
    }

    public class EntityQueryDal<T> : EntityQueryDal<T, int>, IEntityQueryDal<T> where T : class, IDbEntity
    {
    }
}