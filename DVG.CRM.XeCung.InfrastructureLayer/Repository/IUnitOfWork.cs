using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext GetDbContext();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
