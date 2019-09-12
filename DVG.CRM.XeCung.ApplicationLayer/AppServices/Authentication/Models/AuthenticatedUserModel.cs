using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Authentication.Models
{
    public class AuthenticatedUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<RoleInSystem> Roles { get; set; }
        public bool HasRole(params RoleInSystem[] checkedRole)
        {
            for (var index = 0; index < checkedRole.Length; index++)
            {
                if (this.Roles.Contains(checkedRole[index]))
                {
                    return true;
                }
            }
            return false;
        }
        public string Token { get; set; }
        public string RandomKey { get; set; }
        public long ExpiredRandomKey { get; set; }
    }
}
