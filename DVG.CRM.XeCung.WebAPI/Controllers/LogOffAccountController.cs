using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.WebAPI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogOffAccountController : BaseController
    {
        private readonly IAuthenticationAppService AuthenticationAppService;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public LogOffAccountController(IAuthenticationAppService authenticationAppService, IHttpContextAccessor httpContextAccessor) : base(authenticationAppService)
        {
            this.AuthenticationAppService = authenticationAppService;
            this.HttpContextAccessor = httpContextAccessor;
        }

        [Route("logoff")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult LogOff()
        {
            var token = HttpContextAccessor.HttpContext.Request.Cookies["UserToken"];
            if (string.IsNullOrEmpty(token))
            {
                new Response(SystemCode.Success, "Not Permitted", null);
            };
            this.AuthenticationAppService.LogOutAndClearToken(UserContext.Id, token);
            return Json(new Response(SystemCode.Success, "LoggedOut", null));
        }
    }
}