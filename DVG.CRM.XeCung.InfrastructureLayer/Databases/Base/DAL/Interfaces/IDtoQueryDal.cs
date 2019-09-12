using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System.Collections.Generic;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces
{
    public interface IDtoQueryDal<T, Tid> where T : IDto
    {
        IEnumerable<T> List(ICondition condition);
        ListWithTotalRow<T> ListWithTotalRow(ICondition condition);
        T GetById(Tid id);

        int CountTotalRecord(ICondition condition);
    }

    public interface IDtoQueryDal<T> : IDtoQueryDal<T, int> where T : IDto
    {
    }
}