using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class UsersDto : IDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string OtpPrivateKey { get; set; }
        public string RandomKey { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ExpiredRandomKey { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public short Status { get; set; }
    }
}
