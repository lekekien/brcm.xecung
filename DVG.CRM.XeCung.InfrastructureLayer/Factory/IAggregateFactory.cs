using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;

namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public interface IAggregateFactory<TAggregate, TDbEntity, TId> where TAggregate : IAggregateRoot<TId> where TDbEntity : IFactoryMap
    {
        IIdentityFactory<TId> IdentityFactory { get; }

        TAggregate Create(TDbEntity entity);
    }

    public interface IAggregateFactory<TAggregate, TDbEntity> : IAggregateFactory<TAggregate, TDbEntity, int> where TAggregate : IAggregateRoot<int> where TDbEntity : IFactoryMap
    {
    }
}