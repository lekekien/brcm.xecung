using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using System;
using System.Collections;

namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public class Factory<TId>
    {
        private static readonly Hashtable _instance = new Hashtable();

        private static readonly IIdentityFactory<TId> _identity =
            DVGServiceLocator.Current.GetInstance<IIdentityFactory<TId>>();

        protected static T GetInstance<T>()
        {
            var type = typeof(T);
            if (!_instance.ContainsKey(type))
            {
                var instance = (T)Activator.CreateInstance(type, true);
                _instance.Add(type, instance);
                return instance;
            }

            return (T)_instance[type];
        }

        public IIdentityFactory<TId> IdentityFactory => _identity;
    }

    public class Factory : Factory<int>
    {
    }
}