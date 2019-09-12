using AutoMapper;
using DVG.CRM.XeCung.Data.Conditions;
using DVG.CRM.XeCung.Data.Conditions.Customers;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerCareHistorys;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerHistories;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Repositories
{
    public class CustomerRespository : ICustomerRespository
    {
        private readonly ICommandDal<CustomerEntity, int> CustomerCommandDal;
        private readonly ICommandDal<CustomerCareHistoryEntity, int> CustomerCareHistoryCommandDal;
        private readonly ICommandDal<CustomerHistoryEntity, int> CustomerHistoryCommandDal;
        private readonly ICommandDal<CustomerNoteHistoryEntity, int> CustomerNoteHistoryCommandDal;
        private readonly IEntityQueryDal<CustomerEntity, int> CustomerEntityQuery;
        private readonly IEntityQueryDal<CustomerCareHistoryEntity, int> CustomerCareHistoryEntityQuery;
        //private readonly IDtoQueryDal<CustomerCareHistoryLastestDto, int> CustomerCareHistoryLastestQuery;

        public CustomerRespository(ICommandDal<CustomerEntity, int> customerCommandDal
                                , IEntityQueryDal<CustomerEntity, int> customerEntityQuery
                                , ICommandDal<CustomerCareHistoryEntity, int> customerCareHistoryCommandDal
                                , ICommandDal<CustomerNoteHistoryEntity, int> customerNoteHistoryCommandDal
                                , ICommandDal<CustomerHistoryEntity, int> customerHistoryCommandDal
                                //, IDtoQueryDal<CustomerCareHistoryLastestDto, int> customerCareHistoryLastestQuery
                                , IEntityQueryDal<CustomerCareHistoryEntity, int> customerCareHistoryEntityQuery)
        {
            this.CustomerCommandDal = customerCommandDal;
            this.CustomerEntityQuery = customerEntityQuery;
            this.CustomerCareHistoryCommandDal = customerCareHistoryCommandDal;
            this.CustomerHistoryCommandDal = customerHistoryCommandDal;
            this.CustomerNoteHistoryCommandDal = customerNoteHistoryCommandDal;
            //this.CustomerCareHistoryLastestQuery = customerCareHistoryLastestQuery;
            this.CustomerCareHistoryEntityQuery = customerCareHistoryEntityQuery;
        }

        public Response Add(Customer domain)
        {
            if (!string.IsNullOrEmpty(domain.Email))
            {
                if (this.CustomerEntityQuery.CountTotalRecord(new EmailCondition() { Email = domain.Email.Trim().ToLower() }) > 0)
                {
                    return new Response(SystemCode.Warning, "Email đã tồn tại!", null);
                }
            }
            if (!string.IsNullOrEmpty(domain.PhoneNumber))
            {
                if (this.CustomerEntityQuery.CountTotalRecord(new PhoneNumberCondition() { PhoneNumber = domain.Email.Trim() }) > 0)
                {
                    return new Response(SystemCode.Warning, "SĐT đã tồn tại", null);
                }
            }
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.CustomerCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.CustomerCareHistoryCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.CustomerHistoryCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    var entity = Mapper.Map<Customer, CustomerEntity>(domain);
                    var customerId = this.CustomerCommandDal.AddGetId(entity);
                    if (customerId <= 0)
                    {
                        return new Response(SystemCode.Error, "Có lỗi xảy ra khi thêm mới", null);
                    }
                    //Add lịch sử thay đổi
                    var customerHistory = domain.ListCustomerHistories.FirstOrDefault();
                    var customerHistoryItem = Mapper.Map<CustomerHistory, CustomerHistoryEntity>(customerHistory);
                    customerHistoryItem.CustomerId = customerId;
                    this.CustomerHistoryCommandDal.Add(customerHistoryItem);
                    // Add/Update lịch sử chăm sóc
                    if (entity.AssigneeId > 0)
                    {
                        foreach (var item in domain.LastestCustomerCareHistories)
                        {
                            var customerCareItem = Mapper.Map<CustomerCareHistory, CustomerCareHistoryEntity>(item);
                            customerCareItem.CustomerId = customerId;
                            if (customerCareItem.Id == 0)
                            {
                                this.CustomerCareHistoryCommandDal.Add(customerCareItem);
                            }
                            else
                            {
                                this.CustomerCareHistoryCommandDal.Update(customerCareItem);
                            }
                        }
                    }
                    unitOfWork.Commit();
                    return new Response(SystemCode.Success, "Thêm mới khách hàng thành công", null);
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }
        public Response Edit(Customer domain)
        {
            if (!string.IsNullOrEmpty(domain.Email))
            {
                if (this.CustomerEntityQuery.CountTotalRecord(new EmailCondition() { Id = domain.Id, Email = domain.Email.Trim().ToLower() }) > 1)
                {
                    return new Response(SystemCode.Warning, "Email đã tồn tại!", null);
                }
            }
            if (!string.IsNullOrEmpty(domain.PhoneNumber))
            {
                if (this.CustomerEntityQuery.CountTotalRecord(new PhoneNumberCondition() { Id = domain.Id, PhoneNumber = domain.Email.Trim() }) > 1)
                {
                    return new Response(SystemCode.Warning, "SĐT đã tồn tại", null);
                }
            }
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.CustomerCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.CustomerCareHistoryCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.CustomerHistoryCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    var entity = Mapper.Map<Customer, CustomerEntity>(domain);
                    this.CustomerCommandDal.Update(entity);
                    //Add lịch sử thay đổi
                    var customerHistory = domain.ListCustomerHistories.FirstOrDefault();
                    var customerHistoryItem = Mapper.Map<CustomerHistory, CustomerHistoryEntity>(customerHistory);
                    customerHistoryItem.CustomerId = entity.Id;
                    this.CustomerHistoryCommandDal.Add(customerHistoryItem);
                    // Add/Update lịch sử chăm sóc
                    if (entity.AssigneeId > 0)
                    {
                        foreach (var item in domain.LastestCustomerCareHistories)
                        {
                            var customerCareItem = Mapper.Map<CustomerCareHistory, CustomerCareHistoryEntity>(item);
                            customerCareItem.CustomerId = entity.Id;
                            if (customerCareItem.Id == 0)
                            {
                                this.CustomerCareHistoryCommandDal.Add(customerCareItem);
                            }
                            else
                            {
                                this.CustomerCareHistoryCommandDal.Update(customerCareItem);
                            }

                        }
                    }
                    unitOfWork.Commit();
                    return new Response(SystemCode.Success, "Thêm mới khách hàng thành công", null);
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }
        public Customer GetById(int id)
        {
            var listEntity = this.CustomerEntityQuery.List(new IdCondition { Id = id}).ToList();
            if (listEntity == null)
            {
                return null;
            }
            var entity = listEntity.FirstOrDefault();
            var customerDomain = CustomerFactory.Instance.CreateExisting(entity);

            var lastestCustomerCare = this.CustomerCareHistoryEntityQuery.List(new CustomerIdCondition() { CustomerId = entity.Id }).ToList();
            if (lastestCustomerCare.Count > 0)
            {
                var customerCareHistoryEntity = this.CustomerCareHistoryEntityQuery.GetById(lastestCustomerCare.First().Id);
                customerDomain.AddCustomerCareHistory(customerCareHistoryEntity);
            }
            return customerDomain;
        }
    }
}
