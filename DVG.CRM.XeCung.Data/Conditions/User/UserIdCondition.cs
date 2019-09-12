using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions
{
    public class UserIdCondition : Condition
    {
        public int UserId { get; set; }
    }
    public class UserIdPagingCondition : UserIdCondition
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
