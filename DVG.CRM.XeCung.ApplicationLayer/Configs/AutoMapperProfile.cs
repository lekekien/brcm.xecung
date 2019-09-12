using AutoMapper;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models;
using DVG.CRM.XeCung.Data.Dtos;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerCareHistorys;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerHistories;
using DVG.CRM.XeCung.DomainLayer.Aggregates.ProductionCosts;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users.UserPermissions;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Videos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Configs
{
    public class AutoMapperProfile
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                // Domain => Entity
                cfg.CreateMap<User, UsersEntity>()
                    .ForMember(d => d.UserName, opt => opt.MapFrom(d => d.LoginInfo.UserName))
                    .ForMember(d => d.Password, opt => opt.MapFrom(d => d.LoginInfo.Password))
                    .ForMember(d => d.Status, opt => opt.MapFrom(d => (short)d.Status));
                cfg.CreateMap<UserRole, UserRoleEntity>().ForMember(d => d.RoleId, opt => opt.MapFrom(d => (int)d.Role));
                cfg.CreateMap<ProductionCost, ProductionCostEntity>();
                cfg.CreateMap<UserRoleHistory, UserRoleHistoryEntity>();
                cfg.CreateMap<UserHistory, UserHistoryEntity>();
                cfg.CreateMap<Customer, CustomerEntity>();
                cfg.CreateMap<CustomerHistory, CustomerHistoryEntity>();
                cfg.CreateMap<CustomerCareHistory, CustomerCareHistoryEntity>();
                cfg.CreateMap<Video, VideoEntity>();

                // Entity => Application model
                cfg.CreateMap<UsersEntity, UserInfoModel>();
                cfg.CreateMap<VideoEntity, VideoInfoModel>();
                cfg.CreateMap<VideoEntity, VideoEditModel>();
                cfg.CreateMap<CustomerEntity, CustomerUpdateModel>();
                // Application => Entity
                cfg.CreateMap<VideoEditModel, VideoEntity>();
                cfg.CreateMap<UserInfoModel, UsersEntity>();
                cfg.CreateMap<CustomerUpdateModel, CustomerEntity>();
                cfg.CreateMap<VideoInfoModel, VideoEntity>();
                // DTO => Application model
                cfg.CreateMap<UsersInAllDto, UserSearchModel>();
                cfg.CreateMap<CustomerDetailDto, CustomerDetailModel>();
                cfg.CreateMap<CustomerHistoryDto, CustomerHistoryModel>();
                cfg.CreateMap<CustomerCareHistoryDto, CustomerCareHistoryModel>();
                cfg.CreateMap<CustomerNoteHistoryDto, CustomerNoteHistoryModel>();
            });
        }
    }
}
