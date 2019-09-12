using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer
{
    public class CustomerNoteHistoryModel
    {
        public int Id { get; set; }
        public string Note { get; private set; }
        public string CreatedBy { get; private set; }
        public System.DateTime CreatedDate { get; private set; }
    }
}
