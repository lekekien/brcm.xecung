using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Repository.Interfaces
{
    //public interface IRepository<TAggregate, TDbEntity, TId> where TAggregate : IAggregateRoot<TId> where TDbEntity : IDbEntity<TId>
    //{
    //    TAggregate FindById(TId id);

    //    IEnumerable<TAggregate> List(ICondition condition);

    //    void Add(TAggregate entity);

    //    int AddGetId(TAggregate entity);

    //    void DeleteById(TId id);

    //    void Delete(ICondition condition);

    //    void Update(TAggregate entity);

    //    void SetWriteDbContext(IDbContext writeContext);
    //}

    //public interface IRepository<TAggregate, TDbEntity> : IRepository<TAggregate, TDbEntity, int>
    //    where TAggregate : IAggregateRoot<int> where TDbEntity : IDbEntity
    //{
    //}

    public interface IRepository<T,TId> where T : IAggregateRoot<TId>
    {
        IQueryable<T> GetAll();
        T GetById(TId id);
        void Create(T entity);
        void Update(T entity);
        void Delete(TId id);
    }

}
