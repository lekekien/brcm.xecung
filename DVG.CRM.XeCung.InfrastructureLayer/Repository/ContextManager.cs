using DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Repository
{
    public class ContextManager
    {
        private readonly string _contextKey;
        private static PostgresSQL _currentContext;

        public ContextManager()
        {
            _contextKey = "ContextKey.PostgresSQL";
        }

        public static PostgresSQL GetContext()
        {
            if (_currentContext == null)
                _currentContext = new PostgresSQL();
            return _currentContext;
        }

        public void Finish()
        {
            _currentContext?.Dispose();
        }
    }
}
