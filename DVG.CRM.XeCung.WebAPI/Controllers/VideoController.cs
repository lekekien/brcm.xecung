using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using DVG.CRM.XeCung.WebAPI.Filters;
using DVG.CRM.XeCung.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : BaseController
    {
        private readonly IVideoAppService VideoAppService;
        private readonly IExpenditureAppService ExpenditureAppService;
        
        public VideoController(IAuthenticationAppService authenticationAppService, IVideoAppService videoAppService, IExpenditureAppService expenditureAppService)
                : base(authenticationAppService)
        {
            this.VideoAppService = videoAppService;
            this.ExpenditureAppService = expenditureAppService;
        }
        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Create(VideoInfoModel model)
        {
            var response = this.VideoAppService.Create(UserContext, model);
            return Json(response);
        }
        [Route("initcreateview")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult InitCreateView()
        {
            // Lấy ra danh sách các video type
            var allVideoType = Utils.GetAllEnumValueAndDescription<VideoType>();      
            var allExpenditureInKeyValue = this.ExpenditureAppService.GetAllExpenditureInKeyValue();
            var allCostContent = Utils.GetAllEnumValueAndDescription<CostContent>();
            return Json(new Response(SystemCode.Success, "", new
            {
                AllVideoType = allVideoType,
                AllExpenditureInKeyValue = allExpenditureInKeyValue,
                AllCostContent = allCostContent
            }));
        }
        [Route("search")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Search(VideoIndexModel model)
        {
            var response = this.VideoAppService.Search(UserContext, model);
            return Json(response);
        }
        [Route("delete")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Delete(VideoInfoModel model)
        {
            var response = this.VideoAppService.Delete(UserContext, model);
            return Json(response);

        }
        [Route("initeditview")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult InitEditView(VideoIdForRequestModel model)
        {
            var videoInfo = this.VideoAppService.GetByCode(UserContext, model.VideoCode);
            if (videoInfo == null)
            {
                return Json(new Response(SystemCode.Warning, "This customer is not found !", null));
            }

            videoInfo.Token = ChecksumHelper.GenToken(new { Id = model.Id, CurrUser = UserContext.UserName });
            var allVideoType = Utils.GetAllEnumValueAndDescription<VideoType>();
            var allExpenditureInKeyValue = this.ExpenditureAppService.GetAllExpenditureInKeyValue();
            var allCostContent = Utils.GetAllEnumValueAndDescription<CostContent>();

            return Json(new Response(SystemCode.Success, "", new {
                VideoEdit = videoInfo,
                AllVideoType = allVideoType,
                AllExpenditureInKeyValue = allExpenditureInKeyValue,
                AllCostContent = allCostContent
            }));
        }
        [Route("edit")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Edit(VideoEditModel model)
        {
            //var validateResponse = ChecksumHelper.ValidateToken(new { Id = model.Id, CurrUser = UserContext.UserName }, model.Token);
            //if (!validateResponse.IsValid)
            //{
            //    return Json(new Response(SystemCode.Warning, "You don't have a permission to process this action!", null));
            //}
            var response = this.VideoAppService.Edit(UserContext, model);
            return Json(response);
            //test
        }
    }
}