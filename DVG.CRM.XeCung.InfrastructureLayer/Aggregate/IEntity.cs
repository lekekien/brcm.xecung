using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Aggregate
{
    public interface IEntity : IEntity<long>
    {
    }
    public interface IEntity<T>
    {
        T Id { get; }
    }
}
