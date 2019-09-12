using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Cachings;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.Data.Conditions;
using DVG.CRM.XeCung.Data.Dtos;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.GoogleTOTP;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Authentication
{
    public class CustomCookieAuthenticationAppService : IAuthenticationAppService
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IDtoQueryDal<UsersDto, long> UserDtoQueryDal;
        private readonly IEntityQueryDal<UserRoleEntity> UserPermissionEntityQueryDal;
        private readonly IUserAppService Userappservice;
        private readonly IUserRespository UserRespository;
        private readonly IUserTokenCache UserTokenCache;

        public CustomCookieAuthenticationAppService(IHttpContextAccessor httpContextAccessor
                                                  , IDtoQueryDal<UsersDto, long> userDtoQueryDal
                                                  , IEntityQueryDal<UserRoleEntity> userPermissionEntityQueryDal
                                                  , IUserAppService userappservice
                                                  , IUserRespository userRespository
                                                  , IUserTokenCache userTokenCache)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.UserDtoQueryDal = userDtoQueryDal;
            this.UserPermissionEntityQueryDal = userPermissionEntityQueryDal;
            this.Userappservice = userappservice;
            this.UserRespository = userRespository;
            this.UserTokenCache = userTokenCache;
        }
        public AuthenticatedUserModel GetCurrentUser()
        {
            if (this.HttpContextAccessor.HttpContext.User == null) return null;
            var currUserInfo = this.HttpContextAccessor.HttpContext.User;
            var jsonRoles = currUserInfo.FindFirst("Roles").Value.ToString();
            return new AuthenticatedUserModel()
            {
                Id = currUserInfo.FindFirst("Id").Value.ToInt(0),
                UserName = currUserInfo.FindFirst(ClaimTypes.Name).Value,
                Roles = jsonRoles == null ? new List<RoleInSystem>() : JsonConvert.DeserializeObject<List<RoleInSystem>>(jsonRoles),
                RandomKey = currUserInfo.FindFirst("RandomKey").Value.ToString(),
                ExpiredRandomKey = currUserInfo.FindFirst("ExpiredRandomKey").Value.ToLong()
            };
        }
        public async Task<Response> Login(string userName, string password, string otpprivateKey)
        {
            var result = this.UserDtoQueryDal.List(new UserNameCondition()
            {
                UserName = userName.ToLower()
            });
            if (result.Count() > 0)
            {
                var userInfo = result.FirstOrDefault();
                if (userInfo.Password != password.ToMD5())
                {
                    return new Response(SystemCode.Error, "Password is incorrect ", null);
                }
                if (userInfo.Status == 0)
                {
                    return new Response(SystemCode.Error, "This account has been locked", null);
                }
                ////Check OTP
                //var secretkey = AppSettings.Instance.GetString("OTPSecretKey") + userInfo.OtpPrivateKey;
                //if (!GoogleTOTP.IsVaLid(secretkey, otpprivateKey))
                //{
                //    return new Response(SystemCode.Error, "Incorrect OTP Key", null);
                //}

                // Lấy ra danh sách các role của người dùng
                var roles = this.UserPermissionEntityQueryDal.List(new UserIdCondition() { UserId = userInfo.Id }).Select(o => (RoleInSystem)o.RoleId).ToList();
                //Gen Token
                var randomKey = string.Empty;
                System.DateTime expiredRandomKey = new System.DateTime();
                if (DateTime.Now > userInfo.ExpiredRandomKey || string.IsNullOrEmpty(userInfo.RandomKey))
                {
                    var userDomain = UserRespository.GetById(userInfo.Id);
                    randomKey = StringUtils.RandomString(AppSettings.Instance.GetInt32("RandomStringLength"));
                    expiredRandomKey = DateTime.Now.AddDays(AppSettings.Instance.GetInt32("ExpiredRandomKey"));
                    userDomain.UpdateRandomkey(randomKey, expiredRandomKey);
                    if (userDomain.IsValid)
                    {
                        this.UserRespository.Update(userDomain);
                    }
                    else
                    {
                        randomKey = string.Empty;
                        expiredRandomKey = new System.DateTime();
                    }
                }
                else
                {
                    randomKey = userInfo.RandomKey;
                    expiredRandomKey = userInfo.ExpiredRandomKey;
                }
                if (string.IsNullOrEmpty(randomKey) || expiredRandomKey < DateTime.Now)
                {
                    return new Response(SystemCode.Error, "The Key for this Account is expired", null);
                }
                var token = ChecksumHelper.GenToken(new { Id = userInfo.Id, CurrUser = userInfo.UserName, RandomKey = randomKey }, AppSettings.Instance.GetString("TimeExpiredTokenUser").ToInt());

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,result.FirstOrDefault().UserName),
                    new Claim("Id",userInfo.Id.ToString()),
                    new Claim("Roles",JsonConvert.SerializeObject(roles)),
                    new Claim("RandomKey", randomKey.ToString()),
                    new Claim("ExpiredRandomKey", expiredRandomKey.Ticks.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                this.UserTokenCache.Set(string.Format(AppSettings.Instance.GetString("FormatUserSession"), userInfo.Id), token);
                this.HttpContextAccessor.HttpContext.Response.Cookies.Append("UserToken", token);
                await this.HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                var authenUser = new AuthenticatedUserModel()
                {
                    Id = userInfo.Id,
                    UserName = userInfo.UserName,
                    Roles = roles,
                    Token = token,
                };
                return new Response(SystemCode.Success, "Valid", authenUser);
            }
            else
            {
                return new Response(SystemCode.Error, "Not valid", null);
            }
        }
        public bool IsAuthenticated()
        {
            return this.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? true : false;
        }
        public void LogOut()
        {
            this.HttpContextAccessor.HttpContext.SignOutAsync();
        }

        public void LogOutAndClearToken(long userId, string userToken)
        {
            if (!string.IsNullOrEmpty(userToken))
            {
                this.UserTokenCache.Remove(string.Format(AppSettings.Instance.GetString("FormatUserSession"), userId), userToken);
            }
            this.HttpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
