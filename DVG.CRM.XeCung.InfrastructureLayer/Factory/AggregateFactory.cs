using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using System;
using System.Collections;

namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public abstract class AggregateFactory<TAggregate, TDbEntity, TId> : IAggregateFactory<TAggregate, TDbEntity, TId> where TAggregate : IAggregateRoot<TId> where TDbEntity : IFactoryMap
    {
        private static readonly Hashtable _instance = new Hashtable();

        private static readonly IIdentityFactory<TId> _identity =
            DVGServiceLocator.Current.GetInstance<IIdentityFactory<TId>>();

        protected static T GetInstance<T>() where T : IAggregateFactory<TAggregate, TDbEntity, TId>
        {
            var type = typeof(T);
            if (!_instance.ContainsKey(typeof(T)))
            {
                var instance = (T)Activator.CreateInstance(type, true);
                _instance.Add(type, instance);
                return instance;
            }

            return (T)_instance[type];
        }

        public IIdentityFactory<TId> IdentityFactory => _identity;

        public abstract TAggregate Create(TDbEntity entity);
    }

    public abstract class AggregateFactory<TAggregate, TDbEntity> : AggregateFactory<TAggregate, TDbEntity, int>, IAggregateFactory<TAggregate, TDbEntity> where TAggregate : IAggregateRoot<int> where TDbEntity : IFactoryMap
    {
    }
}