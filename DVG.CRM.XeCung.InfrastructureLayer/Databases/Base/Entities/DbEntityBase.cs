using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using ServiceStack.DataAnnotations;
using System;
using System.Reflection;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities
{
    [Serializable]
    public class DbEntityBase<TId> : Condition, IDbEntity<TId>
    {
        [NonSerialized]
        private PropertyInfo _idInfo;
        
        [Ignore]
        public string IdName => _idInfo == null ? null : _idInfo.Name;
        public DbEntityBase()
        {
            if (_idInfo == null)
                _idInfo = GetIdInfo();
        }

        private PropertyInfo GetIdInfo()
        {
            if (_idInfo != null)
                return _idInfo;
            var props = this.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                if (propertyInfo.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                {
                    _idInfo = propertyInfo;
                    break;
                }
            }

            return _idInfo;
        }

        public TId GetId()
        {
            if (_idInfo != null)
                return (TId)_idInfo.GetValue(this);
            else
                return default(TId);
        }

        public void SetId(TId id)
        {
            if (_idInfo != null)
                _idInfo.SetValue(this, id);
        }
    }

    public class DbEntityBase : DbEntityBase<int>, IDbEntity
    {
    }
}