using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class UsersEntity : DbEntity<int>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public short Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string OTPPrivateKey { get; set; }
        public string RandomKey { get; set; }
        public System.DateTime ExpiredRandomKey { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public System.DateTime? LastActivityDate { get; set; }
    }
}
