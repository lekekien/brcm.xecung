using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Aggregate
{
    public interface IAggregateRoot : IAggregateRoot<int>
    {
        void SetId(int id);
    }

    public interface IAggregateRoot<T> : IEntity<T>
    {
    }
}
