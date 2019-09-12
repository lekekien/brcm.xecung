using System;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions
{
    public interface ICondition
    {
        IReadOnlyDictionary<string, Tuple<Type, object>> Conditions { get; }
    }
}