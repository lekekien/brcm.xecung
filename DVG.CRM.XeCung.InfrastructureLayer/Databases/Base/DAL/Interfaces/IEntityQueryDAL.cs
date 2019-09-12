using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces
{
    public interface IEntityQueryDal<T, TId> where T : IDbEntity<TId>
    {
        T GetById(TId id);

        IEnumerable<T> GetAll();

        IEnumerable<T> List(ICondition condition);

        int CountTotalRecord(ICondition condition);
    }

    public interface IEntityQueryDal<T> : IEntityQueryDal<T, int> where T : IDbEntity<int>
    {
    }
}