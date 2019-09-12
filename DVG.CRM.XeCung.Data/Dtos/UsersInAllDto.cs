using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class UsersInAllDto : IDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Email { get; set; }
        public short Status { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
