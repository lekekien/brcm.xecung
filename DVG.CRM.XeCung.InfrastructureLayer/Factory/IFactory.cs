using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;

namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public interface IFactory<TAggregate, TInput, TId> where TAggregate : IAggregateRoot<TId>
    {
        TAggregate CreateNew(TInput input);

        TAggregate CreateExisting(TId id, TInput input);
    }

    public interface IFactory<TAggregate, TInput> : IFactory<TAggregate, TInput, int>
        where TAggregate : IAggregateRoot<int>
    {
    }
}