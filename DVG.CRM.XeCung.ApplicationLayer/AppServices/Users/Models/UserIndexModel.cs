using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserIndexModel : Pager
    {
        public string FilterKeyWord { get; set; }
    }
}
