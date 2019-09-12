using System;

namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public class GuidIdentityFactory : IIdentityFactory<Guid>
    {
        public Guid CreateId()
        {
            return Guid.NewGuid();
        }
    }
}