using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb.Helpers;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System.Collections.Generic;
using System.Linq;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.PostgreDb.Queries
{
    public class DtoQueryDal<TDto, TId> : IDtoQueryDal<TDto, TId> where TDto : class, IDto
    {
        private readonly string _dtoName = PostgreDalHelper.GetDtoName<TDto>();
        private string _funcPrefix = AppSettings.Instance.GetString("FuncPrefix");

        public IEnumerable<TDto> List(ICondition condition)
        {
            string conditionName = (condition is DbEntityBase) ? string.Empty : condition.GetType().Name.ToLower().Replace(_dtoName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _dtoName + "_getlist" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return PostgreDalHelper.List<TDto>(storeName, condition);
        }

        public TDto GetById(TId id)
        {
            string storeName = _funcPrefix + _dtoName + "_getbyid";
            return PostgreDalHelper.List<TDto>(storeName, new IdCondition<TId>()
            {
                Id = id
            }).FirstOrDefault();
        }

        public int CountTotalRecord(ICondition condition)
        {
            string conditionName = (condition is DbEntityBase) ? string.Empty : condition.GetType().Name.ToLower().Replace(_dtoName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _dtoName + "_getcount" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return PostgreDalHelper.CountTotalRecord(storeName, condition);
        }

        public ListWithTotalRow<TDto> ListWithTotalRow(ICondition condition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DtoQueryDal<TDto> : DtoQueryDal<TDto, int>, IDtoQueryDal<TDto> where TDto : class, IDto { }
}