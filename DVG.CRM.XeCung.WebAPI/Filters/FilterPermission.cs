using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.WebAPI.Filters
{
    public class FilterPermission : ActionFilterAttribute
    {
        private readonly IAuthenticationAppService AuthenticationAppService;
        private readonly RoleInSystem[] Role;
        public FilterPermission(IAuthenticationAppService authenticationAppService, params RoleInSystem[] role)
        {
            this.AuthenticationAppService = authenticationAppService;
            this.Role = role;
        }
        //public string Permission { get; set; }
        //public FilterPermission(RoleInSystem permission)
        //{
        //    Permission = new List<RoleInSystem>();
        //    Permission.Add(permission);
        //}
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var result = new JsonResult(new Response(SystemCode.NotPermitted, "", null));

            var currentUser = this.AuthenticationAppService.GetCurrentUser();
            if (!currentUser.HasRole(Role))
            {
                filterContext.Result = result;
                return;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
