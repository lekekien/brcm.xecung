using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class UserRoleHistoryEntity : DbEntity<int>
    {
        public int UserId { get; set; }
        public string ChangedBy { get; set; }
        public System.DateTime ChangedDate { get; set; }
        public string OldRole { get; set; }
        public string CurrentRole { get; set; }
    }
}
