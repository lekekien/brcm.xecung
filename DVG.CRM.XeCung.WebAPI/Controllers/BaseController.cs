using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        private readonly IAuthenticationAppService AuthenticationAppService;
        public BaseController(IAuthenticationAppService authenticationAppService)
        {
            this.AuthenticationAppService = authenticationAppService;
        }
        /// <summary>
        /// Thông tin người dùng đang đăng nhập vào hệ thống
        /// </summary>
        public AuthenticatedUserModel UserContext
        {
            get { return GetCurrentUser(); }
        }
        private AuthenticatedUserModel GetCurrentUser()
        {
            return this.AuthenticationAppService.GetCurrentUser();
        }
    }
}