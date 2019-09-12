using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerHistories
{
    public class CustomerHistory : Entity<int>
    {
        public CustomerHistory(int id, string action, string createdBy, System.DateTime createdDate)
        {
            this.Id = id;
            this.Action = action;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
        }
        public string Action { get; private set; }
        public string CreatedBy { get; private set; }
        public System.DateTime CreatedDate { get; private set; }
    }

}
