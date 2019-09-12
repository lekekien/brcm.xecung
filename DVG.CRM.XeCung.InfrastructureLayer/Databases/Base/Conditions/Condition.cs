using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions
{
    [Serializable]
    public class Condition : ICondition
    {
        [NonSerialized]
        private Dictionary<string, Tuple<Type, object>> _conditions;

        private void CreateConditions()
        {
            _conditions = new Dictionary<string, Tuple<Type, object>>();
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name != "Conditions" && prop.GetCustomAttribute<IgnoreAttribute>() == null)
                {
                    var val = prop.GetValue(this);
                    _conditions.Add(AppSettings.Instance.GetString("StoredProcParameterPrefix") + prop.Name.ToLower(), new Tuple<Type, object>(prop.PropertyType, val));
                }
            }
        }
        public IReadOnlyDictionary<string, Tuple<Type, object>> Conditions
        {
            get
            {
                if (_conditions == null)
                    CreateConditions();
                return _conditions;
            }
        }
    }
}