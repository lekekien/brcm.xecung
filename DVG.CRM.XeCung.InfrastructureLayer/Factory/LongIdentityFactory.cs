namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public class LongIdentityFactory : IIdentityFactory<long>
    {
        public long CreateId()
        {
            return DateTime.Now.Ticks;
        }
    }
}