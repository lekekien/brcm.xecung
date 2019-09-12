using DVG.CRM.XeCung.DomainLayer.Aggregates.Users;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Repositories
{
    public interface IUserRespository
    {
        bool Add(User model);
        bool Update(User model);
        User GetById(int id);
        Response RemoveUserRole(int id);
    }
}
