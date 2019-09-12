using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class VideoSearchDto : IDto
    {
        public int Id { get; set; }
        public string VideoCode { get; set; }
        public int VideoType { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string ContractID { get; set; }
        public decimal ActualProductionCost { get; set; }
        public System.DateTime? SpendDate { get; set; }
        public System.DateTime? InvoiceIssuedDate { get; set; }
        public decimal Revenue { get; set; }
    }
}
