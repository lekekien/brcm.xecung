using DVG.CRM.XeCung.InfrastructureLayer.Caching.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Caching
{
    public class StackExchangeRedisCached : ICached
    {
        IDistributedCache ClientCache;

        public StackExchangeRedisCached(IDistributedCache clientCache)
        {
            this.ClientCache = clientCache;
        }


        public T Get<T>(string key, HttpContext context = null)
        {
            var keyValue = this.ClientCache.Get(key);
            if (keyValue == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(keyValue))
            {
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public bool Remove(string key)
        {
            try
            {
                this.ClientCache.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                return false;
            }

        }

        public bool Set<T>(string key, T item, int expireInMinute = 0)
        {
            if (item == null)
            {
                return false;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, item);
                this.ClientCache.Set(key, memoryStream.ToArray(), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireInMinute) });
                return true;
            }
        }
    }
}
