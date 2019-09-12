using System;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.ObjectForCache
{
    public interface IObjectForCache
    {
        IReadOnlyDictionary<string, Tuple<Type, object>> ObjectForCache { get; }
    }
}