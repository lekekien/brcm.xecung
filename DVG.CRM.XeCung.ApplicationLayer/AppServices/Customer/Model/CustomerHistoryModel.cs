using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model
{
    public class CustomerHistoryModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Action { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
