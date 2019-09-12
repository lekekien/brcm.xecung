using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Caching.Interfaces
{
    public interface ICached
    {
        bool Set<T>(string key, T item, int expireInMinute = 0);

        bool Remove(string key);

        T Get<T>(string key, HttpContext context = null);
    }
}
