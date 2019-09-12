using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserSearchModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string DisplayName { get; set; }
        public short Status { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public string StrLastActivityDate
        {
            get
            {
                return LastActivityDate > System.DateTime.MinValue ? String.Format("{0: MM/dd/yyyy}", LastActivityDate) : "No Activity yet";
            }
        }
    }
}
