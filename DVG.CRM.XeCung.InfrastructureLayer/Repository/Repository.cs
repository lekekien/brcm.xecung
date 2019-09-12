using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Factory;
using DVG.CRM.XeCung.InfrastructureLayer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Repository
{
    //public class Repository<TAggregate, TDbEntity, TId> : IRepository<TAggregate, TDbEntity, TId> where TAggregate : IAggregateRoot<TId> where TDbEntity : IDbEntity<TId>
    //{
    //    private readonly ICommandDal<TDbEntity, TId> _iCommandDal;
    //    private readonly IEntityQueryDal<TDbEntity, TId> _iQueryDal;
    //    private readonly IFactory<TAggregate, TDbEntity, TId> _factory;
    //    private ICached _cachedClient;

    //    public Repository(ICommandDal<TDbEntity, TId> iCommandDal, IEntityQueryDal<TDbEntity, TId> iQueryDal, IFactory<TAggregate, TDbEntity, TId> factory)
    //    {
    //        _iCommandDal = iCommandDal;
    //        _iQueryDal = iQueryDal;
    //        _factory = factory;
    //    }

    //    public TAggregate FindById(TId id)
    //    {
    //        var entity = _iQueryDal.GetById(id);

    //        return entity == null ? default(TAggregate) : _factory.CreateExisting(entity.GetId(), entity);
    //    }

    //    public IEnumerable<TAggregate> List(ICondition condition)
    //    {
    //        var list = new List<TAggregate>();
    //        var entities = _iQueryDal.List(condition);
    //        if (entities.Any())
    //        {
    //            foreach (var dbEntity in entities)
    //            {
    //                list.Add(_factory.CreateExisting(dbEntity.GetId(), dbEntity));
    //            }
    //        }
    //        return list;
    //    }

    //    public void Add(TAggregate aggregate)
    //    {
    //        _iCommandDal.Add(AutoMapper.Mapper.Map<TAggregate, TDbEntity>(aggregate));
    //    }

    //    public void DeleteById(TId id)
    //    {
    //        _iCommandDal.DeleteById(id);
    //    }

    //    public void Delete(ICondition condition)
    //    {
    //        _iCommandDal.Delete(condition);
    //    }

    //    public void Update(TAggregate aggregate)
    //    {
    //        _iCommandDal.Update(AutoMapper.Mapper.Map<TAggregate, TDbEntity>(aggregate));
    //    }

    //    public void SetWriteDbContext(IDbContext writeContext)
    //    {
    //        _iCommandDal.SetWriteDbContext(writeContext);
    //    }

    //    public int AddGetId(TAggregate aggregate)
    //    {
    //        return _iCommandDal.AddGetId(AutoMapper.Mapper.Map<TAggregate, TDbEntity>(aggregate));
    //    }
    //}

    //public class Repository<TAggregate, TDbEntity> : Repository<TAggregate, TDbEntity, int>,
    //    IRepository<TAggregate, TDbEntity> where TAggregate : IAggregateRoot<int> where TDbEntity : IDbEntity
    //{
    //    public Repository(ICommandDal<TDbEntity> iCommandDal, IEntityQueryDal<TDbEntity> iQueryDal, IFactory<TAggregate, TDbEntity> factory) : base(iCommandDal, iQueryDal, factory)
    //    {
    //    }
    //}
}
