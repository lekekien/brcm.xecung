using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions.Customers
{
    public class CustomerSearchFilterCondition : Condition
    {
        public string FilterKeyword { get; set; }
        public int Scource { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int AssigneeId { get; set; }
        public int IsFavorite { get; set; }
        public int Order { get; set; }
        public System.DateTime? CreatedDateFrom { get; set; }
        public System.DateTime? CreatedDateTo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
