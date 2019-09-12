using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserHistoryModel
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Action { get; set; }
    }
    public class UserHistoryIndexModel : Pager
    {
        public int UserId { get; set; }

    }
}
