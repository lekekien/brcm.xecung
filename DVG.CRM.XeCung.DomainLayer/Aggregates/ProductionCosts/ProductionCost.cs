using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.ProductionCosts
{
    public class ProductionCost : Entity<int>
    {
        public ProductionCost()
        {

        }
        public ProductionCost(int id,int serviceID, int productionCostType, int costType, int costContent, decimal amount, System.DateTime? spendDate)
        {
            this.Id = id;
            this.ServiceID = serviceID;
            this.ProductionCostType = productionCostType;
            this.CostType = costType;
            this.CostContent = costContent;
            this.Amount = amount;
            this.SpendDate = spendDate;
        }
        public int ServiceID { get; set; }
        public int ProductionCostType { get; set; }
        public int CostType { get; set; }
        public int CostContent { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime? SpendDate { get; set; }
    }
}
