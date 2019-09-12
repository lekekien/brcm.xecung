using AutoMapper;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.Data.Conditions.Video;
using DVG.CRM.XeCung.Data.Dtos;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Videos;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos
{
    public class VideoAppService : IVideoAppService
    {
        private readonly IVideoRepository VideoRepository;
        private readonly IDtoQueryDal<VideoSearchDto> VideoSearchDtoQueryDal;
        private readonly IEntityQueryDal<VideoEntity, int> VideoEntityQuery;
        public VideoAppService(IVideoRepository videoRepository
            , IDtoQueryDal<VideoSearchDto> videoSearchDtoQueryDal
            , IEntityQueryDal<VideoEntity, int> videoEntityQuery
            )
        {
            this.VideoRepository = videoRepository;
            this.VideoSearchDtoQueryDal = videoSearchDtoQueryDal;
            this.VideoEntityQuery = videoEntityQuery;
        }
        public Response Create(AuthenticatedUserModel currUser, VideoInfoModel model)
        {
            Response response = null;
            model.CreatedDate = DateTime.Now;
            var videoEntity = Mapper.Map<VideoInfoModel, VideoEntity>(model);
            videoEntity.CreatedBy = currUser.UserName;
            var newVideo = VideoFactory.Instance.CreateNew(videoEntity);
            //add estimated production cost
            foreach (var item in model.EstimatedProductionCostRecords)
            {
                newVideo = newVideo.AddEstimatedProductionCosts(item.ServiceID, item.ProductionCostType, item.CostType, item.CostContent, item.Amount, item.SpendDate);
            }
            //add estimated production cost
            foreach (var item in model.ActualProductionCostRecords)
            {
                newVideo = newVideo.AddActualProductionCosts(item.ServiceID, item.ProductionCostType, item.CostType, item.CostContent, item.Amount, item.SpendDate);
            }
            if(newVideo.IsValid)
            {
                response = this.VideoRepository.Add(newVideo);
            }
            if (response.Code == SystemCode.Success)
            {
                return new Response(SystemCode.Success, "Thêm video thành công!", null);
            }
            return response;
        }

        public Response Delete(AuthenticatedUserModel currUser, VideoInfoModel model)
        {
            Response response = null;
            var videoEntity = Mapper.Map<VideoInfoModel, VideoEntity>(model);
            var deleteVideo = VideoFactory.Instance.CreateExisting(videoEntity);
            response = this.VideoRepository.Delete(deleteVideo);
            if (response.Code == SystemCode.Success)
            {
                return new Response(SystemCode.Success, "Xóa video thành công!", null);
            }
            return response;
        }

        public Response Edit(AuthenticatedUserModel currUser, VideoEditModel model)
        {
            // Lấy thông tin video
            var videoDomain = this.VideoRepository.GetById(model.Id);

            if (videoDomain == null)
            {
                return new Response(SystemCode.Warning, "Video không tồn tại.", null);
            }

            try
            {
                // Nếu user là admin hoặc manager hoặc là người tạo video thì mới cho sửa
                if (currUser.HasRole(RoleInSystem.Admin, RoleInSystem.Manager) || videoDomain.CreatedBy == currUser.UserName)
                {
                    // Cập nhật thông tin video
                    videoDomain = videoDomain.Update(Mapper.Instance.Map<VideoEditModel, VideoEntity>(model));

                    if (!videoDomain.IsValid)
                    {
                        return new Response(SystemCode.Warning, string.Join(Environment.NewLine, Utils.GetErrorMessage(videoDomain.ValidationResult, videoDomain.BusinessRuleViolation)), null);
                    }
                    return this.VideoRepository.Update(videoDomain);
                }
                else
                {
                    return new Response(SystemCode.Error, "Bạn không có quyền sửa video", null);
                }
                
            }
            catch (Exception ex)
            {
                return new Response(SystemCode.Error, ex.ToString(), null);
            }

        }

        public VideoEditModel GetByCode(AuthenticatedUserModel currUser, string VideoCode)
        {
            var videoEntity = this.VideoEntityQuery.List(new VideoCodeCondition() { VideoCode = VideoCode }).FirstOrDefault();
            if (videoEntity == null)
            {
                return null;
            }
            var videoEdit = Mapper.Instance.Map<VideoEntity, VideoEditModel>(videoEntity);
            return videoEdit;
        }

        public Response Search(AuthenticatedUserModel currUser, VideoIndexModel model)
        {
            var filterCondition = new VideoSearchFilterCondition()
            {
                FilterKeyword = string.IsNullOrEmpty(model.FilterKeyword) ? "" : model.FilterKeyword.Trim().ToLower(),
                VideoType = model.VideoType,
                SpendDate = model.SpendDate,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize
            };
            var ListWithTotalRow = this.VideoSearchDtoQueryDal.ListWithTotalRow(filterCondition);
            return new Response(SystemCode.Success, "", new { Result = ListWithTotalRow.List, TotalRecord = ListWithTotalRow.TotalRow });
        }
    }
}
