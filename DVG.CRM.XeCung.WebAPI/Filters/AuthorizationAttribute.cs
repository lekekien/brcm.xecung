using DVG.CRM.XeCung.ApplicationLayer.Cachings;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.WebAPI.Filters
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private readonly IAuthenticationAppService AuthenticationAppService;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IUserTokenCache UserTokenCache;

        public AuthorizationAttribute(IAuthenticationAppService authenticationAppService, IHttpContextAccessor httpContextAccessor, IUserTokenCache userTokenCache)
        {
            this.AuthenticationAppService = authenticationAppService;
            this.HttpContextAccessor = httpContextAccessor;
            this.UserTokenCache = userTokenCache;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var result = new JsonResult(new Response(SystemCode.NotLogIn, "", null));
            if (!this.AuthenticationAppService.IsAuthenticated())
            {
                filterContext.Result = result;
                return;
            };
            var token = HttpContextAccessor.HttpContext.Request.Cookies["UserToken"];
            if (string.IsNullOrEmpty(token))
            {
                filterContext.Result = result;
                return;
            };

            var currentUser = this.AuthenticationAppService.GetCurrentUser();
            if (currentUser == null || DateTime.Now.Ticks > currentUser.ExpiredRandomKey)
            {
                filterContext.Result = result;
                return;
            }

            var lstToken = this.UserTokenCache.Get(string.Format(AppSettings.Instance.GetString("FormatUserSession"), currentUser.Id));
            if (!lstToken.Contains(token))
            {
                filterContext.Result = result;
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
