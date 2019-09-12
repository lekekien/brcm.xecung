using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces
{
    public interface ICommandDal<TEntity, TId> where TEntity : IDbEntity<TId>
    {
        void Add(TEntity entity);

        TId AddGetId(TEntity entity);

        void Update(TEntity entity);

        void DeleteById(TId id);

        void SetWriteDbContext(IDbContext writeContext);

        void Delete(ICondition condition);
    }

    public interface ICommandDal<TEntity> : ICommandDal<TEntity, int> where TEntity : IDbEntity
    {
    }
}