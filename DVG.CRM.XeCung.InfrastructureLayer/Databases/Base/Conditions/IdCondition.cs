namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions
{
    public class IdCondition<TId> : Condition
    {
        public TId Id { get; set; }
    }
}