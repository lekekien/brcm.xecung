using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class ProductionCostEntity : DbEntity<int>
    {
        public int ServiceID { get; set; }
        public int ProductionCostType { get; set; }
        public int CostType { get; set; }
        public int CostContent { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime? SpendDate { get; set; }
    }
}
