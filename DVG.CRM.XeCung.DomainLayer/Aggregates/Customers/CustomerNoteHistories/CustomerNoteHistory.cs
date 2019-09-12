using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerNoteHistories
{
    public class CustomerNoteHistory : Entity<int>
    {
        public CustomerNoteHistory(int id, string note, string createdBy, System.DateTime createdTime)
        {
            this.Id = id;
            this.Note = note;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdTime;
        }

        public string Note { get; private set; }
        public string CreatedBy { get; private set; }
        public System.DateTime CreatedDate { get; private set; }
    }
}
