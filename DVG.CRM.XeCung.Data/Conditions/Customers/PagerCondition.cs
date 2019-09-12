using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions.Customers
{
    public class HistoryConditon: Condition
    {
        public int CustomerId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
