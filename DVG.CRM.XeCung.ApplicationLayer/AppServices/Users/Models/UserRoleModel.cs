using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
    }
}
