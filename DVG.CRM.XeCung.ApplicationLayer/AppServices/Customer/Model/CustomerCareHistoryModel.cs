using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer
{
    public class CustomerCareHistoryModel
    {
        public int Id { get; set; }
        public System.DateTime CareStartTime { get; set; }
        public long AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public short Status { get; set; }
    }
}
