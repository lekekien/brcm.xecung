using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using DVG.CRM.XeCung.Web.API.Controllers;
using DVG.CRM.XeCung.WebAPI.Filter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        ICustomerAppService CustomerAppService;
        IUserAppService UserAppService;
        public CustomerController(IAuthenticationAppService authenticationAppService, 
                                  ICustomerAppService customerAppService,
                                  IUserAppService userAppService)
                                : base(authenticationAppService)
        {
            this.CustomerAppService = customerAppService;
            this.UserAppService = userAppService;
        }
        [Route("init")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Init()
        {
            // Lấy ra danh sách các customer type
            var allCustomerScource = Utils.GetAllEnumValueAndDescription<CustomerScource>();
            // Lấy ra danh sách các customer type
            var allCustomerStatus = Utils.GetAllEnumValueAndDescription<CustomerStatus>();
            // Lấy ra danh sách các customer status
            var allCustomerType = Utils.GetAllEnumValueAndDescription<CustomerType>();
            // Lấy ra danh sách các assignee
            //var managedUsers = UserContext.HasRole(RoleInSystem.Admin, RoleInSystem.Manager) ? this.UserAppService.GetList() : new List<UserSearchModel>() { new UserSearchModel() { Id = UserContext.Id, UserName = UserContext.UserName } };

            return Json(new Response(SystemCode.Success, "", new
            {
                AllCustomerScource = allCustomerScource,
                AllCustomerStatus = allCustomerStatus,
                AllCustomerType = allCustomerType,
                //AllKeyValueAssignee = managedUsers.Select(o => new KeyValuePair<long, string>(o.Id, o.UserName)),
            }));
        }
        [Route("initupdateview")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult InitEditView()
        {
            var allCustomerScource = Utils.GetAllEnumValueAndDescription<CustomerScource>();
            var allCustomerType = Utils.GetAllEnumValueAndDescription<CustomerType>();
            var allCustomerStatus = Utils.GetAllEnumValueAndDescription<CustomerStatus>();
            return Json(new Response(SystemCode.Success, "", new
            {
                AllCustomerScource = allCustomerScource,
                AllCustomerType = allCustomerType,
                AllCustomerStatus = allCustomerStatus
            }));
        }
        [Route("update")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Update(CustomerUpdateModel model)
        {
            var response = new Response();
            if (model.Id > 0)
            {
                var validateResponse = ChecksumHelper.ValidateToken(new { Id = model.Id, CurrUser = UserContext.UserName }, model.Token);
                if (!validateResponse.IsValid)
                {
                    return Json(new Response(SystemCode.Warning, "Thay đổi customer không thành công", null));
                }
                response = this.CustomerAppService.Edit(UserContext, model);
            }
            else
            {
                response = this.CustomerAppService.Create(UserContext, model);
            }
            return Json(response);
        }
        [Route("search")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Search(CustomerIndexModel model)
        {
            var response = this.CustomerAppService.Search(UserContext, model);
            return Json(response);
        }
        [Route("getbyid/{customerId}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetById(int customerId)
        {
            var response = this.CustomerAppService.GetById(UserContext, customerId);
            return Json(response);
        }
        [Route("getdetail/{customerId}")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetDetail(int customerId)
        {
            var response = this.CustomerAppService.GetDetail(UserContext, customerId);
            return Json(response);
        }
        [Route("getlistactivitydate")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult GetListcustomerActivity(CustomerHistoryIndexModel model)
        {
            return Json(this.CustomerAppService.GetListCustomerHistory(model));
        }
    }
}
