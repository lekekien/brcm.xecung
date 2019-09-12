using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model
{
    public class CustomerHistoryIndexModel: Pager
    {
        public int CustomerId { get; set; }
    }
}
