using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class ContractEntity : DbEntity<int>
    {
        public string Name { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int CustomerId { get; set; }
        public int Status { get; set; }
        public int VideoPrice { get; set; }
        public int AdvertisedPrice { get; set; }
        public string LinkService { get; set; }
        public System.DateTime? IssueInvoiceRevenueDate { get; set; }
        public System.DateTime? LastmodifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
