using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities
{
    public interface IDbEntity<TId> : IDto, ICondition
    {
        //TId Id { get; set; }
        TId GetId();

        void SetId(TId id);

        string IdName { get; }
    }

    public interface IDbEntity : IDbEntity<int>
    {
    }
}