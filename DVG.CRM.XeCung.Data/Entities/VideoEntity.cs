using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class VideoEntity : DbEntity<int>
    {
        public string VideoCode { get; set; }
        public string Title { get; set; }
        public int VideoType { get; set; }
        public string Link { get; set; }
        public string Note { get; set; }
        public System.DateTime? PublishDate { get; set; }
        public System.DateTime? SpendDate { get; set; }
        public System.DateTime? InvoiceIssuedDate { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public decimal Revenue { get; set; }
        public System.DateTime? ReceiptIssuedDate { get; set; }
        public string ContractID { get; set; }
        public decimal EstimatedProductionCost { get; set; }
        public decimal ActualProductionCost { get; set; }
        public string CreatedBy { get; set; }
    }
}
