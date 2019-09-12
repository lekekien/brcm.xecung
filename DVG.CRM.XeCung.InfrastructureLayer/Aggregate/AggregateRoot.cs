using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Aggregate
{
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot<T>
    {
        public AggregateRoot()
        {
            this.BusinessRuleViolation = new ValidationResult();
        }
    }

    public abstract class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
        public AggregateRoot()
        {
            this.BusinessRuleViolation = new ValidationResult();
        }
        public void SetId(int id)
        {
            Id = id;
        }
    }
}
