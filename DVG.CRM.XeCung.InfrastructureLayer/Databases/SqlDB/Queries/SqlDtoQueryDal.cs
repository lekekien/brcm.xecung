
using DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB.Helpers;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB.Queries
{
    public class SqlDtoQueryDal<TDto, TId> : IDtoQueryDal<TDto, TId> where TDto : class, IDto
    {
        private readonly string _dtoName = SqlDalHelper.GetDtoName<TDto>();
        private string _funcPrefix = AppSettings.Instance.GetString("FuncPrefix");

        public IEnumerable<TDto> List(ICondition condition)
        {
            string conditionName = (condition is DbEntityBase) ? string.Empty : condition.GetType().Name.ToLower().Replace(_dtoName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _dtoName + "_getlist" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return SqlDalHelper.List<TDto>(storeName, condition);
        }

        public ListWithTotalRow<TDto> ListWithTotalRow(ICondition condition)
        {
            string conditionName = (condition is DbEntityBase) ? string.Empty : condition.GetType().Name.ToLower().Replace(_dtoName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _dtoName + "_getlistwithtotalrow" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return SqlDalHelper.ListWithTotalRow<TDto>(storeName, condition);
        }
        public TDto GetById(TId id)
        {
            string storeName = _funcPrefix + _dtoName + "_getbyid";
            return SqlDalHelper.List<TDto>(storeName, new IdCondition<TId>()
            {
                Id = id
            }).ToList().FirstOrDefault();
        }

        public int CountTotalRecord(ICondition condition)
        {
            string conditionName = (condition is DbEntityBase) ? string.Empty : condition.GetType().Name.ToLower().Replace(_dtoName, string.Empty)
                .Replace("condition", string.Empty);
            string storeName = _funcPrefix + _dtoName + "_getcount" + (string.IsNullOrEmpty(conditionName) ? string.Empty : "_" + conditionName);
            return SqlDalHelper.CountTotalRecord(storeName, condition);
        }

        
    }
    public class SqlDtoQueryDal<TDto> : SqlDtoQueryDal<TDto, int>, IDtoQueryDal<TDto> where TDto : class, IDto { }
}
