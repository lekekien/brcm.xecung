using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Videos
{
    public class VideoFactory : Factory
    {
        public static VideoFactory Instance => GetInstance<VideoFactory>();
        public Video CreateNew(VideoEntity entity)
        {
            return new Video(0, entity.CreatedBy, entity.VideoCode, entity.Title, entity.VideoType, entity.Link, entity.Note, entity.PublishDate, entity.SpendDate, entity.InvoiceIssuedDate, entity.CreatedDate, entity.Revenue, entity.ReceiptIssuedDate, entity.ContractID, entity.EstimatedProductionCost, entity.ActualProductionCost);
        }

        public Video CreateExisting(VideoEntity entity)
        {
            return new Video(entity.Id, entity.CreatedBy, entity.VideoCode, entity.Title, entity.VideoType, entity.Link, entity.Note, entity.PublishDate, entity.SpendDate, entity.InvoiceIssuedDate, entity.CreatedDate, entity.Revenue, entity.ReceiptIssuedDate, entity.ContractID, entity.EstimatedProductionCost, entity.ActualProductionCost);
        }
    }
}
