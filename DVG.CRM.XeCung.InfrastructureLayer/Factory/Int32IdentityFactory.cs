namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public class Int32IdentityFactory : IIdentityFactory<int>
    {
        public int CreateId()
        {
            return 0;
        }
    }
}