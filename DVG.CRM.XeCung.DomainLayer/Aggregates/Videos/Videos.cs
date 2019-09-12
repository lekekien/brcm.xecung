using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.ProductionCosts;
using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Videos
{
    public class Video : AggregateRoot<int>
    {
        #region constructors
        public Video(int id,string createdBy, string videoCode, string title , int videoType, string link, string note, System.DateTime? publishDate,
            System.DateTime? spendDate, System.DateTime? invoiceIssuedDate, System.DateTime? createdDate, decimal revenue,
            System.DateTime? receiptIssuedDate, string contractID, decimal estimatedProductionCost, decimal actualProductionCost,
            List<ProductionCost> estimatedProductionCostRecords = null, List<ProductionCost> actualProductionCostRecords = null)
        {
            this.Id = id;
            this.CreatedBy = createdBy;
            this.VideoCode = videoCode;
            this.Title = title;
            this.VideoType = videoType;
            this.Link = link;
            this.Note = note;
            this.PublishDate = publishDate;
            this.SpendDate = spendDate;
            this.InvoiceIssuedDate = invoiceIssuedDate;
            this.CreatedDate = createdDate;
            this.Revenue = revenue;
            this.ReceiptIssuedDate = receiptIssuedDate;
            this.ContractID = contractID;
            this.EstimatedProductionCost = estimatedProductionCost;
            this.ActualProductionCost = actualProductionCost;
            if(estimatedProductionCostRecords != null)
            {
                this.EstimatedProductionCostRecords = estimatedProductionCostRecords;
            }
            else
            {
                this.EstimatedProductionCostRecords = new List<ProductionCost>();
            }
            if (actualProductionCostRecords != null)
            {
                this.ActualProductionCostRecords = actualProductionCostRecords;
            }
            else
            {
                this.ActualProductionCostRecords = new List<ProductionCost>();
            }
            
        }
        #endregion
        #region properties
        public string CreatedBy { get; set; }
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
        public List<ProductionCost> EstimatedProductionCostRecords { get; set; }
        public List<ProductionCost> ActualProductionCostRecords { get; set; }
        #endregion

        #region behaviors
        public Video AddEstimatedProductionCosts(int serviceID, int productionCostType, int costType, int costContent, decimal amount, System.DateTime? spendDate)
        {
            ProductionCost productionCost = new ProductionCost(0, serviceID, productionCostType, costType, costContent, amount, spendDate);
            this.EstimatedProductionCostRecords.Add(productionCost);
            return this;
        }
        public Video AddActualProductionCosts(int serviceID, int productionCostType, int costType, int costContent, decimal amount, System.DateTime?spendDate)
        {
            ProductionCost productionCost = new ProductionCost(0,serviceID, productionCostType, costType, costContent, amount, spendDate);
            this.ActualProductionCostRecords.Add(productionCost);
            return this;
        }
        public Video Update(VideoEntity entity)
        {
            //update video history if needed
            if(this.CreatedBy != entity.CreatedBy)
            {
                //update change createdby to history table
                this.CreatedBy = entity.CreatedBy;
            }
            if (this.VideoCode != entity.VideoCode)
            {
                this.VideoCode = entity.VideoCode;
            }
            if (this.Title != entity.Title)
            {
                this.Title = entity.Title;
            }
            if (this.VideoType != entity.VideoType)
            {
                this.VideoType = entity.VideoType;
            }
            if (this.Link != entity.Link)
            {
                this.Link = entity.Link;
            }
            if (this.Note != entity.Note)
            {
                this.Note = entity.Note;
            }
            if (this.PublishDate != entity.PublishDate)
            {
                this.PublishDate = entity.PublishDate;
            }
            if (this.SpendDate != entity.SpendDate)
            {
                this.SpendDate = entity.SpendDate;
            }
            if (this.InvoiceIssuedDate != entity.InvoiceIssuedDate)
            {
                this.InvoiceIssuedDate = entity.InvoiceIssuedDate;
            }
            if (this.ReceiptIssuedDate != entity.ReceiptIssuedDate)
            {
                this.ReceiptIssuedDate = entity.ReceiptIssuedDate;
            }
            if (this.ContractID != entity.ContractID)
            {
                this.ContractID = entity.ContractID;
            }
            if (this.EstimatedProductionCost != entity.EstimatedProductionCost)
            {
                this.EstimatedProductionCost = entity.EstimatedProductionCost;
            }
            if (this.ActualProductionCost != entity.ActualProductionCost)
            {
                this.ActualProductionCost = entity.ActualProductionCost;
            }
            return this;
        }
        #endregion
    }
}
