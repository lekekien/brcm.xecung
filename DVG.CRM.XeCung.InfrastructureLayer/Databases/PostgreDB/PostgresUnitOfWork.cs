using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using DVG.CRM.XeCung.InfrastructureLayer.Repository.Interfaces;
using System;
using System.Collections;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb
{
    public class PostgresUnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly PostgresSQL _writeContext;

        public PostgresUnitOfWork()
        {
            _writeContext = new PostgresSQL(PostgresSQL.DBPosition.Master);
        }
       
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            try
            {
                _writeContext.CommitTransaction();
                _writeContext.BeginTransaction();
            }
            catch (Exception e)
            {
                _writeContext.RollbackTransaction();
                throw e;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _writeContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void BeginTransaction()
        {
            _writeContext.BeginTransaction();
        }

        public void Commit()
        {
            _writeContext.CommitTransaction();
        }

        public void Rollback()
        {
            _writeContext.RollbackTransaction();
        }

        public IDbContext GetDbContext()
        {
            return _writeContext;
        }
    }
}