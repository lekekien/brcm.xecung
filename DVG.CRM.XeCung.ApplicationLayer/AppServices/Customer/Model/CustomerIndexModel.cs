using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer
{
    public class CustomerIndexModel: Pager
    {
        public string FilterKeyword { get; set; }
        public int Scource { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int AssigneeId { get; set; }
        public int IsFavorite { get; set; }
        public int Order { get; set; }
        public System.DateTime? CreatedDateFrom { get; set; }
        public System.DateTime? CreatedDateTo { get; set; }
    }
}
