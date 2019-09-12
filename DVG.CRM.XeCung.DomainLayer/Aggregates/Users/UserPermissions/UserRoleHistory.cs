using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users.UserPermissions
{
    public class UserRoleHistory : Entity<int>
    {
        public UserRoleHistory(int id, string changeBy, System.DateTime changeDate, string oldRole, string currentRole)
        {
            this.Id = id;
            this.ChangedBy = changeBy;
            this.ChangedDate = changeDate;
            this.OldRole = oldRole;
            this.CurrentRole = currentRole;
        }
        public string ChangedBy { get; private set; }
        public System.DateTime ChangedDate { get; private set; }
        public string OldRole { get; private set; }
        public string CurrentRole { get; private set; }
    }
}
