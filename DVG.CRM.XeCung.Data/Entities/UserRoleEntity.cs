using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class UserRoleEntity : DbEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
