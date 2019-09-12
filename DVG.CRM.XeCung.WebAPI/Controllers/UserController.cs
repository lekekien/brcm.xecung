using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.GoogleTOTP;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using DVG.CRM.XeCung.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserAppService UserAppService;
        public UserController(IAuthenticationAppService authenticationAppService, IUserAppService userAppService)
                : base(authenticationAppService)
        {
            this.UserAppService = userAppService;
        }

        [Route("search")]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Search(UserIndexModel model)
        {
            int totalRecord = 0;
            var currUser = UserContext;
            var result = this.UserAppService.GetListPaging(model, UserContext, out totalRecord).ToList();
            return Json(new Response(SystemCode.Success, "Valid", new { ListUser = result, TotalRecord = totalRecord }));
        }

        [Route("update")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Update([FromBody]UserInforViewModel user)
        {
            var currentUser = UserContext;
            //check trùng email, phone, username
            var userCheckByUserName = UserAppService.GetByConditon((int)user.Id, null, null, user.UserName.ToLower());
            if (userCheckByUserName != null && userCheckByUserName.Count > 0)
            {
                return Json(new Response(SystemCode.Error, "Tên user đã tồn tại.", null));
            }
            var userCheckEmail = UserAppService.GetByConditon((int)user.Id, user.Email.ToLower(), null, null);
            if (userCheckEmail != null && userCheckEmail.Count > 0)
            {
                return Json(new Response(SystemCode.Error, "Địa chỉ email đã tồn tại.", null));
            }

            var userCheckByPhone = UserAppService.GetByConditon((int)user.Id, null, user.PhoneNumber.ToLower(), null);
            if (userCheckByPhone != null && userCheckByPhone.Count > 0)
            {
                return Json(new Response(SystemCode.Error, "Số điện thoại đã tồn tại.", null));
            }
            try
            {
                if (user != null)
                {
                    if (user.Id > 0)
                    {
                        var validateResponse = ChecksumHelper.ValidateToken(new { Id = user.Id, CurrUser = currentUser.UserName }, user.Token);
                        if (!validateResponse.IsValid)
                        {
                            return Json(new Response(SystemCode.Warning, "Cập nhật thông tin thành công", null));
                        }
                        var isUpdatted = this.UserAppService.Update(currentUser, user);
                        if (!isUpdatted)
                        {
                            return Json(new Response(SystemCode.Error, "Cập nhật thông tin không thành công", null));
                        }
                        return Json(new Response(SystemCode.Success, "Cập nhật thông tin thành công", null));
                    }
                    else
                    {
                        user.Password = "123456";
                        var isCreated = this.UserAppService.Create(currentUser, user);
                        if (!isCreated)
                        {
                            return Json(new Response(SystemCode.Error, "Thêm user không thành công", null));
                        }
                        return Json(new Response(SystemCode.Success, "Thêm user thành công", null));
                    }
                }
                return Json(new Response(SystemCode.Error, "Có lỗi xảy ra. Liên hệ IT để được hỗ trợ.", null));
            }
            catch (Exception ex)
            {
                return Json(new Response(SystemCode.Error, ex.Message, null));
            }
        }

        [Route("getdropdownlistpermitted")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        [TypeFilter(typeof(FilterPermission), Arguments = new object[] { new RoleInSystem[] { RoleInSystem.Admin } })]
        public JsonResult GetDropdownListPermitted()
        {
            //Lấy list Role
            var listRole = Utils.GetAllEnumValueAndDescription<RoleInSystem>();
            return Json(new Response(SystemCode.Success, "", new
            {
                ListRole = listRole,
            }));
        }

        [Route("block")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Block([FromBody]UserInforViewModel user)
        {
            var validateResponse = ChecksumHelper.ValidateToken(new { Id = user.Id, CurrUser = UserContext.UserName }, user.Token);
            if (!validateResponse.IsValid)
            {
                return Json(new Response(SystemCode.Warning, "Khóa user không thành công!", null));
            }
            if (!this.UserAppService.Block(user.Id))
            {
                return Json(new Response(SystemCode.Error, "Khóa user không thành công!", null));
            }
            return Json(new Response(SystemCode.Success, "Khóa user thành công", null));
        }
        [Route("unblock")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Unblock([FromBody]UserInforViewModel user)
        {
            var validateResponse = ChecksumHelper.ValidateToken(new { Id = user.Id, CurrUser = UserContext.UserName }, user.Token);
            if (!validateResponse.IsValid)
            {
                return Json(new Response(SystemCode.Warning, "Mở khóa user không thành công", null));
            }
            if (!this.UserAppService.Unblock(user.Id))
            {
                return Json(new Response(SystemCode.Error, "Mở khóa user không thành công", null));
            }
            return Json(new Response(SystemCode.Success, "Mở khóa user thành công", null));
        }
        [Route("resetpassword")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult ResetPassword([FromBody]ChangePassWord user)
        {
            var validateToken = ChecksumHelper.ValidateToken(new { Id = user.Id, CurrUser = UserContext.UserName }, user.Token);
            if (!validateToken.IsValid)
            {
                return Json(new Response(SystemCode.Warning, "Reset mật khẩu không thành công!", null));
            }
            if (!this.UserAppService.ChangePassword(user.Id, user.NewPassword))
            {
                return Json(new Response(SystemCode.Error, "Reset mật khẩu không thành công!", null));
            }
            return Json(new Response(SystemCode.Success, "Reset mật khẩu thành công!", null));
        }
        [Route("getbyidpermitted/{userId}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        [TypeFilter(typeof(FilterPermission), Arguments = new object[] { new RoleInSystem[] { RoleInSystem.Admin } })]
        public JsonResult GetByIdPermitted(int userId)
        {
            var user = UserAppService.GetById(userId);
            if (user != null)
            {
                return Json(new Response(SystemCode.Success, "", user));
            }
            return Json(new Response(SystemCode.Error, "Có lỗi xảy ra!", null));
        }

        [Route("getbyid/{userId}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetById(int userId)
        {
            var user = this.UserAppService.GetById(userId);
            if (user != null)
            {
                return Json(new Response(SystemCode.Success, "", user));
            }
            else
            {
                return Json(new Response(SystemCode.Error, "", null));
            }
        }
        [Route("changepassword")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult ChangePassword([FromBody]ChangePassWord user)
        {
            var password = this.UserAppService.GetById(user.Id).Password;
            var validateToken = ChecksumHelper.ValidateToken(new { Id = user.Id, CurrUser = UserContext.UserName }, user.Token);
            if (!validateToken.IsValid)
            {
                return Json(new Response(SystemCode.Warning, "Đổi mật khẩu không thành công!", null));
            }
            //check xem mật khẩu cũ có đúng hay không?
            if (password != user.CurrentPassword.ToMD5())
            {
                return Json(new Response(SystemCode.Warning, "Mật khẩu hiện tại không chính xác", null));
            }
            if (!this.UserAppService.ChangePassword(user.Id, user.NewPassword))
            {
                return Json(new Response(SystemCode.Error, "Đổi mật khẩu không thành công!", null));
            }
            return Json(new Response(SystemCode.Success, "Đổi mật khẩu thành công", null));
        }
        [Route("geturl/{userId}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public ActionResult AllocateOTP(int userId)
        {
            // Cập nhật lại OTP private key cho nhân viên này
            var user = this.UserAppService.GetById(userId);
            // Sinh mã OTP private key trong bảng user
            var randomString = StringUtils.RandomString(5);
            //userLayer.UpdateOTPPrivateKey(userId, randomString);
            var isUpdate = this.UserAppService.UpdateOTPPrivateKey(userId, randomString);
            var secretKey = AppSettings.Instance.GetString("OTPSecretKey") + randomString;
            var dataInfo = new
            {
                UserName = user.UserName,
                SecretKey = secretKey,
                ExpiredTime = DateTime.Now.AddMinutes(5)
            };
            var data = HttpUtility.UrlEncode(JsonConvert.SerializeObject(dataInfo).Encrypt());
            //var link = string.Format("/User/ShowQRCode?data={0}", data);
            //var fullLink = AppSettings.Instance.GetString("ServerDomain").ToString() + link;

            return Json(new Response(SystemCode.Success, "", new { UrlEndCode = data }));
        }
        [Route("getotpprivatekey")]
        [HttpPost]
        //[TypeFilter(typeof(AuthorizationAttribute))]
        public ActionResult ShowQRCode(UrlOtpModel urlModel)
        {
            var dataDecrypt = JsonConvert.DeserializeObject<dynamic>(urlModel.UrlOtp.Decrypt());
            var username = (string)dataDecrypt.UserName;
            var secretKey = (string)dataDecrypt.SecretKey;
            var expiredTime = (System.DateTime)dataDecrypt.ExpiredTime;

            //Sinh ra ảnh QR code
            var googleOPTAuthenticator = new GoogleTOTP();
            var qRCodeImage = googleOPTAuthenticator.GenerateImage(secretKey, "AUTOPORTAL.CRM.XECUNG-" + username);

            var retVal = new OTPManagerModel();
            if (expiredTime > DateTime.Now)
            {
                retVal.Image = qRCodeImage;
                retVal.Status = 1;
            }
            else
            {
                retVal.Image = "Mã QRCode đã hết hạn.Liên hệ IT để được hỗ trợ.";
                retVal.Status = 2;
            }
            return Json(new Response(SystemCode.Success, "", new { OTPData = retVal }));
        }
    }
}