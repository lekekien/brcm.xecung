using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Interfaces
{
    public interface ICustomerAppService
    {
        Response Create(AuthenticatedUserModel currUser, CustomerUpdateModel model);
        Response Edit(AuthenticatedUserModel currUser, CustomerUpdateModel model);
        Response Search(AuthenticatedUserModel currUser, CustomerIndexModel model);
        Response GetById(AuthenticatedUserModel currUser, int id);
        Response GetDetail(AuthenticatedUserModel currUser, int id);
        Response GetListCustomerHistory(CustomerHistoryIndexModel model);
        Response GetListCustomerCareHistory(CustomerHistoryIndexModel model);
        Response GetListCustomerNoteHistory(CustomerHistoryIndexModel model);
    }
}
