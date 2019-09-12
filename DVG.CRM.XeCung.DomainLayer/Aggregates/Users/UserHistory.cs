using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users
{
    public class UserHistory : Entity<int>
    {
        public UserHistory(int id, string userName, System.DateTime modifiedDate, string modifiedBy, string action)
        {
            this.Id = id;
            this.UserName = userName;
            this.ModifiedDate = modifiedDate;
            this.ModifiedBy = modifiedBy;
            this.Action = action;
        }
        public string UserName { get; private set; }
        public System.DateTime ModifiedDate { get; private set; }
        public string ModifiedBy { get; private set; }
        public string Action { get; private set; }
    }
}
