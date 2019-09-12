using ServiceStack.DataAnnotations;
using System;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities
{
    [Serializable]
    public class DbEntity<TId> : DbEntityBase<TId>, IDbEntity<TId>
    {
        [PrimaryKey]
        public virtual TId Id { get; set; }
    }

    public class DbEntity : DbEntity<int>, IDbEntity
    {
    }
}