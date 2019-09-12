using AutoMapper;
using DVG.CRM.XeCung.Data.Conditions.Contract;
using DVG.CRM.XeCung.Data.Conditions.ProductionCost;
using DVG.CRM.XeCung.Data.Conditions.Video;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.ProductionCosts;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Videos;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly ICommandDal<VideoEntity, int> VideosCommandDal;
        private readonly ICommandDal<ProductionCostEntity, int> ProductionCostCommandDal;
        private readonly IEntityQueryDal<VideoEntity, int> VideoEntityQuery;
        private readonly IEntityQueryDal<ContractEntity, int> ContractEntityQuery;
        private readonly IEntityQueryDal<ProductionCostEntity, int> ProductionCostEntityQuery;
        public VideoRepository(ICommandDal<VideoEntity, int> videosCommandDal
            , ICommandDal<ProductionCostEntity, int> productionCostCommandDal
            , IEntityQueryDal<VideoEntity, int> videoEntityQuery
            , IEntityQueryDal<ProductionCostEntity, int> productionCostEntityQuery
            , IEntityQueryDal<ContractEntity, int> contractEntityQuery
            )
        {
            this.VideosCommandDal = videosCommandDal;
            this.ProductionCostCommandDal = productionCostCommandDal;
            this.VideoEntityQuery = videoEntityQuery;
            this.ProductionCostEntityQuery = productionCostEntityQuery;
            this.ContractEntityQuery = contractEntityQuery;
        }
        public Response Add(Video model)
        {
            if (!string.IsNullOrEmpty(model.VideoCode))
            {
                if (this.VideoEntityQuery.List(new VideoCodeCondition() { VideoCode = model.VideoCode }).ToList().Count > 0)
                {
                    return new Response(SystemCode.Error, "Mã video đã tồn tại!", null);
                }
            }
            if (!string.IsNullOrEmpty(model.ContractID))
            {
                if (this.ContractEntityQuery.List(new NameCondition() { Name = model.ContractID }).ToList().Count <= 0)
                {
                    return new Response(SystemCode.Error, "Mã hợp đồng không tồn tại!", null);
                }
            }
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.VideosCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.ProductionCostCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    var newAddedVideoId = VideosCommandDal.AddGetId(Mapper.Map<Video, VideoEntity>(model));
                    //add chi phí sản xuất ước tính
                    foreach (ProductionCost item in model.EstimatedProductionCostRecords)
                    {
                        var entityItem = Mapper.Map<ProductionCost, ProductionCostEntity>(item);
                        entityItem.ServiceID = newAddedVideoId;
                        this.ProductionCostCommandDal.Add(entityItem);
                    }

                    //add chi phí sản xuất thực tế
                    foreach (ProductionCost item in model.ActualProductionCostRecords)
                    {
                        var entityItem = Mapper.Map<ProductionCost, ProductionCostEntity>(item);
                        entityItem.ServiceID = newAddedVideoId;
                        this.ProductionCostCommandDal.Add(entityItem);
                    }
                    //sync thông tin vào hợp đồng
                    var contract = this.ContractEntityQuery.List(new NameCondition() { Name = model.ContractID }).FirstOrDefault();
                    if(contract!=null && model.VideoType == VideoType.ContractVideo.GetHashCode())
                    {
                        contract.VideoPrice = contract.VideoPrice + (int)model.Revenue;
                        contract.LinkService += string.IsNullOrEmpty(contract.LinkService) ? model.Link : ";" + model.Link;
                    }
                    //Add video History

                    unitOfWork.Commit();
                    return new Response(SystemCode.Success, "", null);
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }

        public Response Delete(Video model)
        {
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.VideosCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.ProductionCostCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());

                    var productionCosts = this.ProductionCostEntityQuery.List(new ServiceIDCondition() { ServiceID = model.Id }).ToList();
                    VideosCommandDal.DeleteById(model.Id);
                    //Xóa production cost của video
                    foreach(ProductionCostEntity pc in productionCosts)
                    {
                        this.ProductionCostCommandDal.DeleteById(pc.Id);
                    }
                    //xóa video khỏi hợp đồng
                    if(model.VideoType == VideoType.ContractVideo.GetHashCode())
                    {
                        //
                    }
                    unitOfWork.Commit();
                    return new Response(SystemCode.Success, "", null);
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }

        public Video GetById(int id)
        {
            var videoEntity = this.VideoEntityQuery.GetById(id);
            if (videoEntity == null)
            {
                return null;
            }
            var videoDomain = VideoFactory.Instance.CreateExisting(videoEntity);

            return videoDomain;
        }

        public Response Update(Video model)
        {
            if (!string.IsNullOrEmpty(model.VideoCode))
            {
                if (this.VideoEntityQuery.List(new VideoCodeCondition() { VideoCode = model.VideoCode }).Where(v=>v.Id != model.Id).ToList().Count > 0)
                {
                    return new Response(SystemCode.Error, "Mã video đã tồn tại!", null);
                }
            }
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.VideosCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.ProductionCostCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());

                    this.VideosCommandDal.Update(Mapper.Map<Video, VideoEntity>(model));                   
                    unitOfWork.Commit();
                    return new Response(SystemCode.Success, "Cập nhật thông tin thành công!", null);
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }
    }
}
