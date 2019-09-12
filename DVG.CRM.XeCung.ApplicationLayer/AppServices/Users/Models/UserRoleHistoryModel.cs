using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserRoleHistoryModel
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string ChangedBy { get; set; }
        public System.DateTime ChangedDate { get; set; }
        public string OldRole { get; set; }
        public string CurrentRole { get; set; }
    }
    public class UserRoleHistoryIndexModel : Pager
    {
        public int UserId { get; set; }
    }
}
