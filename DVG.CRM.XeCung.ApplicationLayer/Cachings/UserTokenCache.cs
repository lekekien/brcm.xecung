using DVG.CRM.XeCung.InfrastructureLayer.Caching.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Cachings
{
    public class UserTokenCache : IUserTokenCache
    {
        private readonly ICached MemCached;
        private static int ExpiredTokenCache = AppSettings.Instance.GetString("ExpiredUserTokenCache").ToInt(1440);

        public UserTokenCache(ICached memCached)
        {
            this.MemCached = memCached;
        }

        public List<string> Get(string key)
        {
            var lstToken = this.MemCached.Get<List<string>>(key);
            if (lstToken == null)
            {
                lstToken = new List<string>();
            }
            return lstToken;
        }

        public bool Remove(string key, string tokenKey)
        {
            var lstToken = this.MemCached.Get<List<string>>(key);
            if (lstToken == null)
            {
                lstToken = new List<string>();
            }

            if (lstToken.Contains(tokenKey))
            {
                lstToken.Remove(tokenKey);
            }
            this.MemCached.Set<List<string>>(key, lstToken, ExpiredTokenCache);
            return true;
        }

        public bool Set(string key, string tokenKey)
        {
            var lstToken = this.MemCached.Get<List<string>>(key);
            if (lstToken == null)
            {
                lstToken = new List<string>();
            }
            lstToken.Add(tokenKey);
            this.MemCached.Set<List<string>>(key, lstToken, ExpiredTokenCache);
            return true;
        }
    }
}
