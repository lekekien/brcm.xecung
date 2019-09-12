using DVG.CRM.XeCung.Data.Conditions;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Caching.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Cachings
{
    public class ExpenditureCache : IExpenditureCache
    {
        private readonly ICached CacheStorage;
        private static string AllExpenditureInKeyValueKey = AppSettings.Instance.GetString("AllExpenditureInKeyValueKey");
        IEntityQueryDal<ExpenditureEntity, int> ExpenditureEntityQuery;
        public ExpenditureCache(ICached cacheStorage, IEntityQueryDal<ExpenditureEntity, int> expenditureEntityQuery)
        {
            this.CacheStorage = cacheStorage;
            this.ExpenditureEntityQuery = expenditureEntityQuery;
        }
        public IEnumerable<KeyValuePair<int, string>> GetAllExpenditureInKeyValue()
        {
            var lstExpenditureInKeyValue = this.CacheStorage.Get<List<KeyValuePair<int, string>>>(AllExpenditureInKeyValueKey);
            if (lstExpenditureInKeyValue == null || lstExpenditureInKeyValue.Count == 0)
            {
                lstExpenditureInKeyValue = this.ExpenditureEntityQuery.List(new NonCondition()).Select(o => new KeyValuePair<int, string>(o.Id, o.ExpenditureType)).ToList();
                this.CacheStorage.Set<List<KeyValuePair<int, string>>>(AllExpenditureInKeyValueKey, lstExpenditureInKeyValue, AppSettings.Instance.GetString("LocationRedisStorageExpiration").ToInt(1440));
            }
            return lstExpenditureInKeyValue;
        }
    }
}
