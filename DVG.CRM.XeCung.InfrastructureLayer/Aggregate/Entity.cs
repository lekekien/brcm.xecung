using DVG.CRM.XeCung.InfrastructureLayer.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Aggregate
{
    public abstract class Entity : Entity<long>
    {
    }

    public abstract class Entity<T> : SelfValidatableObject, IEntity<T>
    {
        public T Id { get; protected set; }
    }
}
