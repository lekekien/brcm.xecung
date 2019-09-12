using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Cachings
{
    public interface IUserTokenCache
    {
        bool Set(string key, string tokenKey);
        List<string> Get(string key);
        bool Remove(string key, string tokenKey);
    }
}
