using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class CustomerEntity: DbEntity<int>
    {
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Scource { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string FacebookLink { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public System.DateTime? Birthday { get; set; }
        public int AssigneeId { get; set; }
        public string Description { get; set; }
        public int BlockStatus { get; set; }
        public int IsFavorite { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public string FullTextSearch { get; set; }
    }
}
