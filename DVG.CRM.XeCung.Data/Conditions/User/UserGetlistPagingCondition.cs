using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions
{
    public class CountUserGetlistPaging : Condition
    {
        public string FilterKeyWord { get; set; }
    }
    public class UserGetlistPagingCondition : CountUserGetlistPaging
    {
        public UserGetlistPagingCondition(CountUserGetlistPaging condition, int pageIndex, int pageSize)
        {
            this.FilterKeyWord = condition.FilterKeyWord;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
