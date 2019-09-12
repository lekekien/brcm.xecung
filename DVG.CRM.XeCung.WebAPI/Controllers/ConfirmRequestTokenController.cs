using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using DVG.CRM.XeCung.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ConfirmRequestTokenController : BaseController
    {
        public ConfirmRequestTokenController(IAuthenticationAppService authenticationAppService) : base(authenticationAppService)
        { }
        [Route("gettoken/{id}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetToken(long id)
        {
            return Json(new Response()
            {
                Code = SystemCode.Success,
                Data = ChecksumHelper.GenToken(new { Id = id, CurrUser = UserContext.UserName }, AppSettings.Instance.GetString("TimeExpiredTokenForUpdateRequest", "1").ToInt(1))
            });
        }
        [Route("gettokenupdateview/{id}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetTokenForUpdateView(long id)
        {
            return Json(new Response()
            {
                Code = SystemCode.Success,
                Data = ChecksumHelper.GenToken(new { Id = id, CurrUser = UserContext.UserName }, AppSettings.Instance.GetString("TimeExpiredTokenForUpdateRequest", "15").ToInt(15))
            });
        }
    }
}