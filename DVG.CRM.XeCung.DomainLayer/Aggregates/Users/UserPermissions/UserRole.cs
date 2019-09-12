using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users.UserPermissions
{
    public class UserRole : Entity<int>
    {
        public UserRole(int id, RoleInSystem role)
        {
            this.Id = id;
            this.Role = role;
        }
        public RoleInSystem Role { get; private set; }
    }
}
