using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.ObjectForCache
{
    public class ObjectForCache
    {
        private Dictionary<string, Tuple<Type, object>> _objectForCache;

        private void CreateObjectForCache()
        {
            _objectForCache = new Dictionary<string, Tuple<Type, object>>();
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name != "Model" && prop.GetCustomAttribute<IgnoreAttribute>() == null)
                {
                    var val = prop.GetValue(this);
                    _objectForCache.Add("_" + prop.Name.ToLower(), new Tuple<Type, object>(prop.PropertyType, val));
                }
            }
        }

        public IReadOnlyDictionary<string, Tuple<Type, object>> Conditions
        {
            get
            {
                if (_objectForCache == null)
                    CreateObjectForCache();
                return _objectForCache;
            }
        }
    }
}