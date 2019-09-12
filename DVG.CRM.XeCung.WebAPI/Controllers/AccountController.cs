using System.Threading.Tasks;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserAppService UserAppService;
        private readonly IAuthenticationAppService AuthenticationAppService;
        public AccountController(IUserAppService userAppService, IAuthenticationAppService authenticationAppService)
        {
            this.UserAppService = userAppService;
            this.AuthenticationAppService = authenticationAppService;
        }
        [Route("login")]
        [HttpPost]
        public JsonResult Login([FromBody] LogonViewModel loginInfo)
        {
            var taskLogOn = this.AuthenticationAppService.Login(loginInfo.UserName, loginInfo.Password, loginInfo.OtpPrivateKey);
            Task.WaitAll(taskLogOn);
            var response = taskLogOn.Result;
            // Kiểm tra xem thông tin tài khoản và đăng nhập của người dùng có đúng hay không
            if (response.Code == SystemCode.Success)
            {
                bool isUpdateActive = this.UserAppService.UpdateActivityDate((int)response.Data.Id);
                //if (isUpdateActive)
                //{
                //    return Json(new Response(SystemCode.Success, response.Message, response.Data));
                //}
                //return Json(new Response(SystemCode.Error,"Update Activity date fails", response.Data));
                return Json(new Response(SystemCode.Success, response.Message, response.Data));
            }
            else
            {
                return Json(new Response(SystemCode.Error, response.Message, null));
            }
        }
    }
}