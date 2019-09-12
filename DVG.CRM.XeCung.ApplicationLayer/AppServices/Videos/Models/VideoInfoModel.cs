using DVG.CRM.XeCung.ApplicationLayer.AppServices.ProductionCost.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models
{
    public class VideoInfoModel
    {
        public VideoInfoModel()
        {
            this.EstimatedProductionCostRecords = new List<ProductionCostModel>();
            this.ActualProductionCostRecords = new List<ProductionCostModel>();
        }
        public int Id { get; set; }
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
        public List<ProductionCostModel> EstimatedProductionCostRecords { get; set; }
        public List<ProductionCostModel> ActualProductionCostRecords { get; set; }

    }
}
