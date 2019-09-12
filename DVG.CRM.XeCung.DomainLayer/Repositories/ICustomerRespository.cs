using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Repositories
{
    public interface ICustomerRespository
    {
        Response Add(Customer model);
        Response Edit(Customer model);
        Customer GetById(int id);
    }
}
