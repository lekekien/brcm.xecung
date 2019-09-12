using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Customers
{
    public class CustomerFactory : Factory
    {
        public static CustomerFactory Instance => GetInstance<CustomerFactory>();

        public Customer CreateNew(CustomerEntity entity)
        {
            return new Customer(0, entity.CustomerCode, entity.Name, entity.PhoneNumber, entity.Email, entity.Scource, entity.Type, entity.Status,
                                entity.FacebookLink, entity.Company, entity.Position, entity.Birthday, entity.AssigneeId, entity.Description, entity.BlockStatus, entity.IsFavorite, entity.CreatedBy, entity.CreatedDate, entity.LastModifiedBy, entity.LastModifiedDate, entity.FullTextSearch);
        }

        public Customer CreateExisting(CustomerEntity entity)
        {
            return new Customer(entity.Id, entity.CustomerCode, entity.Name, entity.PhoneNumber, entity.Email, entity.Scource, entity.Type, entity.Status,
                                entity.FacebookLink, entity.Company, entity.Position, entity.Birthday, entity.AssigneeId,
                                entity.Description, entity.BlockStatus, entity.IsFavorite, entity.CreatedBy, entity.CreatedDate, entity.LastModifiedBy, entity.LastModifiedDate, entity.FullTextSearch);
        }
    }
}
