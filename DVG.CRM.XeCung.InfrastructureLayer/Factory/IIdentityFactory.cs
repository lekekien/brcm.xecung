namespace DVG.CRM.XeCung.InfrastructureLayer.Factory
{
    public interface IIdentityFactory<TId>
    {
        TId CreateId();
    }
}