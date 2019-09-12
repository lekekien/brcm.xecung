using AutoMapper;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.ApplicationLayer.Pagers;
using DVG.CRM.XeCung.Data.Conditions.Customers;
using DVG.CRM.XeCung.Data.Dtos;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer
{

    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerRespository CustomerRespository;
        private readonly IEntityQueryDal<CustomerEntity, int> CustomerEntityQuery;
        private readonly IDtoQueryDal<CustomerSearchDto, int> CustomerSearchDtoQueryDal;
        private readonly IDtoQueryDal<CustomerDetailDto, int> CustomerDetailDtoQueryDal;
        private readonly IDtoQueryDal<CustomerHistoryDto, int> CustomerHistoryDtoQueryDal;
        private readonly IDtoQueryDal<CustomerCareHistoryDto, int> CustomerCareHistoryDtoQueryDal;
        private readonly IDtoQueryDal<CustomerNoteHistoryDto, int> CustomerNoteHistoryDtoQueryDal;
        public CustomerAppService(ICustomerRespository customerRespository,
                                  IEntityQueryDal<CustomerEntity, int> customerEntityQuery,
                                  IDtoQueryDal<CustomerSearchDto, int> customerSearchDtoQueryDal,
                                  IDtoQueryDal<CustomerDetailDto, int> customerDetailDtoQueryDal,
                                  IDtoQueryDal<CustomerHistoryDto, int> customerHistoryDtoQueryDal,
                                  IDtoQueryDal<CustomerCareHistoryDto, int> customerCareHistoryDtoQueryDal,
                                 IDtoQueryDal<CustomerNoteHistoryDto, int> customerNoteHistoryDtoQueryDal)
        {
            this.CustomerRespository = customerRespository;
            this.CustomerEntityQuery = customerEntityQuery;
            this.CustomerSearchDtoQueryDal = customerSearchDtoQueryDal;
            this.CustomerDetailDtoQueryDal = customerDetailDtoQueryDal;
            this.CustomerHistoryDtoQueryDal = customerHistoryDtoQueryDal;
            this.CustomerCareHistoryDtoQueryDal = customerCareHistoryDtoQueryDal;
            this.CustomerNoteHistoryDtoQueryDal = customerNoteHistoryDtoQueryDal;

        }
        public Response Create(AuthenticatedUserModel currUser, CustomerUpdateModel model)
        {
            var response = new Response();
            //Check role
            if (!currUser.HasRole(RoleInSystem.Admin, RoleInSystem.CustomerManager))
            {
                return new Response(SystemCode.Warning, "Bạn không có quyền tạo khách hàng", null);
            }
            // Tạo Mã KH
            string customerCode = Utils.GetEnumDescription((CustomerTypeShortName)model.Type) + StringUtils.RandomString(5);
            var count = 0;
            do
            {
                count = this.CustomerEntityQuery.CountTotalRecord(new CustomerCodeCondition { CustomerCode = customerCode });
                customerCode = count > 0 ? Utils.GetEnumDescription((CustomerTypeShortName)model.Type) + StringUtils.RandomString(5) : customerCode;
            }
            while (count > 0);
            // Tạo mới thông tin khách mời
            var customerEntity = new CustomerEntity()
            {
                CustomerCode = customerCode,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Scource = model.Scource,
                Type = model.Type,
                Status = model.Status,
                FacebookLink = model.FacebookLink,
                Company = model.Company,
                Position = model.Position,
                Birthday = model.Birthday,
                AssigneeId = model.AssigneeId,
                Description = model.Description,
                BlockStatus = (int)CustomerBlockStatus.None,
                IsFavorite = 0,
                CreatedBy = currUser.UserName,
                CreatedDate = DateTime.Now,
                LastModifiedBy = currUser.UserName,
                LastModifiedDate = DateTime.Now
            };
            //Gen fulltextsearch
            string name = StringUtils.UnicodeToKoDauAndSpace(customerEntity.Name.Trim().ToLower());
            string company = !string.IsNullOrEmpty(customerEntity.Company) ? StringUtils.UnicodeToKoDauAndSpace(customerEntity.Company.Trim().ToLower()) : "";
            customerEntity.FullTextSearch = string.Format("{0} {1} {2} {3} {4}", name, customerEntity.PhoneNumber, customerEntity.Email, customerEntity.CustomerCode.ToLower(), company).Trim();
            // Tao moi domain
            var customerDomain = CustomerFactory.Instance.CreateNew(customerEntity);
            // Thêm vào lịch sử KH
            customerDomain.AddCustomerHistory(currUser.UserName, CustomerHistoryStatus.AddNew, "");
            // Thêm lịch sử care KH nếu được gán assignee
            if (model.AssigneeId > 0)
            {
                customerDomain.AddAssignee(model.AssigneeId, model.AssigneeName, currUser.UserName);
            }
            if (customerDomain.IsValid)
            {
                response = this.CustomerRespository.Add(customerDomain);
            }
            else
            {
                response = new Response(SystemCode.Error, "Không thể tạo mới khách hàng", null);
            }
            return response;
        }
        public Response Edit(AuthenticatedUserModel currUser, CustomerUpdateModel model)
        {
            //Check role
            if (!currUser.HasRole(RoleInSystem.Admin, RoleInSystem.CustomerManager))
            {
                return new Response(SystemCode.Warning, "Bạn không có quyền cập nhật thông tin khách hàng", null);
            }
            var domain = this.CustomerRespository.GetById(model.Id);
            if (domain == null)
            {
                return new Response(SystemCode.Error, "Không tìm thấy khách hàng này", null);
            }
            //Check thay đổi để lưu vào lịch sử
            #region Lưu lịch sử
            string oldUserHistory = "";
            string newUserHistory = "";
            if (domain.Name != model.Name)
            {
                oldUserHistory += "  +Tên KH: " + domain.Name + "<br>";
                newUserHistory += "  +Tên KH: " + model.Name + "<br>";
            }
            if (domain.PhoneNumber != model.PhoneNumber)
            {
                oldUserHistory += "  +SĐT: " + domain.PhoneNumber + "<br>";
                newUserHistory += "  +SĐT: " + model.PhoneNumber + "<br>";
            }
            if (domain.Email != model.Email)
            {
                oldUserHistory += "  +Email: " + domain.Email + "<br>";
                newUserHistory += "  +Email: " + model.Email + "<br>";
            }
            if (domain.Scource != model.Scource)
            {
                oldUserHistory += "  +Nguồn KH: " + Utils.GetEnumDescription((CustomerScource)domain.Scource) + "<br>";
                newUserHistory += "  +Nguồn KH: " + Utils.GetEnumDescription((CustomerScource)model.Scource) + "<br>";
            }
            if (domain.Type != model.Type)
            {
                oldUserHistory += "  +Loại KH: " + Utils.GetEnumDescription((CustomerType)domain.Type) + "<br>";
                newUserHistory += "  +Loại KH: " + Utils.GetEnumDescription((CustomerScource)model.Type) + "<br>";
            }
            if (domain.Status != model.Status)
            {
                oldUserHistory += "  +Trạng thái: " + Utils.GetEnumDescription((CustomerStatus)domain.Status) + "<br>";
                newUserHistory += "  +Trạng thái: " + Utils.GetEnumDescription((CustomerStatus)model.Status) + "<br>";
            }
            //Thay đổi link face
            var domainFacebook = domain.FacebookLink != null ? domain.FacebookLink.Trim().ToLower() : "";
            var modelFacebook = model.FacebookLink != null ? model.FacebookLink.Trim().ToLower() : "";
            if (domainFacebook != modelFacebook)
            {
                oldUserHistory += "  +Link Facebook: " + (!string.IsNullOrEmpty(domain.FacebookLink) ? domain.FacebookLink : "") + "<br>";
                newUserHistory += "  +Link Facebook: " + (!string.IsNullOrEmpty(model.FacebookLink) ? model.FacebookLink : "") + "<br>";
            }
            //Thay đổi công ty
            var domainCompany = domain.Company != null ? domain.Company.Trim().ToLower() : "";
            var modelCompany = model.Company != null ? model.Company.Trim().ToLower() : "";
            if (domainCompany != modelCompany)
            {
                oldUserHistory += "  +Công ty: " + (!string.IsNullOrEmpty(domain.Company) ? domain.Company : "") + "<br>";
                newUserHistory += "  +Công ty: " + (!string.IsNullOrEmpty(model.Company) ? model.Company : "") + "<br>";
            }
            //Thay đổi vị trí
            var domainPosition = domain.Position != null ? domain.Position.Trim().ToLower() : "";
            var modelPosition = model.Position != null ? model.Position.Trim().ToLower() : "";
            if (domainPosition != modelPosition)
            {
                oldUserHistory += "  +Vị trí: " + (!string.IsNullOrEmpty(domain.Position) ? domain.Position : "") + "<br>";
                newUserHistory += "  +Vị trí: " + (!string.IsNullOrEmpty(model.Position) ? model.Position : "") + "<br>";
            }
            // Thay đổi birthday
            if (domain.Birthday != model.Birthday)
            {
                oldUserHistory += "  +Ngày sinh: " + (domain.Birthday.HasValue ? string.Format("{0: MM/dd/yyyy}", domain.Birthday) : "") + "<br>";
                newUserHistory += "  +Ngày sinh: " + (model.Birthday.HasValue ? string.Format("{0: MM/dd/yyyy}", model.Birthday) : "") + "<br>";
            }
            // Thay đổi N.V phụ trách
            if (domain.AssigneeId != model.AssigneeId)
            {
                oldUserHistory += "  +N.V phụ trách: " + model.OldAssigneeName + "<br>";
                newUserHistory += "  +N.V phụ trách: " + model.AssigneeName + "<br>";
            }
            //Thay đổi ghi chú
            var domainDescription = domain.Description != null ? domain.Description.Trim().ToLower() : "";
            var modelDescription = model.Description != null ? model.Description.Trim().ToLower() : "";
            if (domain.Description.Trim().ToLower() != model.Description.Trim().ToLower())
            {
                oldUserHistory += "  +Ghi chú: " + domain.Description + "<br>";
                newUserHistory += "  +Ghi chú: " + model.Description + "<br>";
            }
            if (domain.BlockStatus != model.BlockStatus)
            {
                oldUserHistory += "  +Khóa: " + Utils.GetEnumDescription((CustomerBlockStatus)domain.BlockStatus) + "<br>";
                newUserHistory += "  +Khóa: " + Utils.GetEnumDescription((CustomerBlockStatus)model.BlockStatus) + "<br>";
            }
            if (domain.IsFavorite != model.IsFavorite)
            {
                oldUserHistory += "  +Yêu thích: " + (domain.IsFavorite == 0 ? "Không" : "Có") + "<br>";
                newUserHistory += "  +Yêu thích: " + (model.IsFavorite == 0 ? "Không" : "Có") + "<br>";
            }
            if (!string.IsNullOrEmpty(oldUserHistory) && !string.IsNullOrEmpty(newUserHistory))
            {
                string descriptionAction = string.Format("<b>- Old Value</b> <br> {0} <b>- New Value</b> <br> {1}", oldUserHistory.Trim().TrimEnd(','), newUserHistory.Trim().TrimEnd(','));
                domain.AddCustomerHistory(currUser.UserName, CustomerHistoryStatus.UpdateInfor, descriptionAction);
            }
            #endregion
            // End Check thay đổi để lưu vào lịch sử

            // Cập nhật thông tin khách hàng
            var customerEntity = Mapper.Map<CustomerUpdateModel, CustomerEntity>(model);
            //Gen fulltextsearch
            string name = StringUtils.UnicodeToKoDauAndSpace(customerEntity.Name.Trim().ToLower());
            string company = !string.IsNullOrEmpty(customerEntity.Company) ? StringUtils.UnicodeToKoDauAndSpace(customerEntity.Company.Trim().ToLower()) : "";
            customerEntity.FullTextSearch = string.Format("{0} {1} {2} {3} {4}", name, customerEntity.PhoneNumber, customerEntity.Email, customerEntity.CustomerCode.ToLower(), company).Trim();

            if (customerEntity.AssigneeId != model.AssigneeId)
            {
                domain.AddAssignee(model.AssigneeId, model.AssigneeName, currUser.UserName);
            }
            domain.Edit(customerEntity, currUser.UserName);
            if (!domain.IsValid)
            {
                return new Response(SystemCode.Error, "Cập nhật thông tin khách hàng không thành công", "");
            }
            return this.CustomerRespository.Edit(domain);
        }
        public Response Search(AuthenticatedUserModel currUser, CustomerIndexModel model)
        {
            model.CreatedDateFrom = model.CreatedDateFrom ?? new System.DateTime(2019, 1, 1, 0, 0, 0);
            model.CreatedDateTo = model.CreatedDateTo == null ? new System.DateTime(2119, 1, 1, 0, 0, 0) : model.CreatedDateTo.Value.AddDays(1).AddSeconds(-1);

            if (!currUser.HasRole(RoleInSystem.Admin, RoleInSystem.CustomerManager))
            {
                model.AssigneeId = currUser.Id;
            }
            model.FilterKeyword = !string.IsNullOrEmpty(model.FilterKeyword) ? StringUtils.UnicodeToKoDauAndSpace(model.FilterKeyword.Trim().ToLower()) : string.Empty;

            var filterCondition = new CustomerSearchFilterCondition()
            {
                FilterKeyword = model.FilterKeyword,
                Scource = model.Scource,
                Type = model.Type,
                Status = model.Status,
                AssigneeId = model.AssigneeId,
                IsFavorite = model.IsFavorite,
                Order = model.Order,
                CreatedDateFrom = model.CreatedDateFrom,
                CreatedDateTo = model.CreatedDateTo,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize
            };
            // Tính ra tổng số bản ghi
            var searchResult = this.CustomerSearchDtoQueryDal.ListWithTotalRow(filterCondition);
            return new Response(SystemCode.Success, "", searchResult);
        }
        public Response GetById(AuthenticatedUserModel currUser, int id)
        {
            var entity = this.CustomerEntityQuery.List(new IdCondition { Id = id}).ToList().FirstOrDefault();
            if (entity == null)
            {
                return new Response(SystemCode.Warning, "Không tìm thấy khách hàng này", null);
            }
            if ((!currUser.HasRole(RoleInSystem.Admin, RoleInSystem.CustomerManager) && currUser.Id != entity.AssigneeId) || entity.BlockStatus != 0)
            {
                return new Response(SystemCode.NotPermitted, "Bạn không có quyền sửa khách hàng này", null);
            }
            var model = Mapper.Instance.Map<CustomerEntity, CustomerUpdateModel>(entity);
            return new Response(SystemCode.Success, "", model);
        }
        public Response GetDetail(AuthenticatedUserModel currUser, int id)
        {
            var customerDetailDtos = this.CustomerDetailDtoQueryDal.List(new IdCondition() { Id = id }).ToList();
            if (customerDetailDtos.Count == 0)
            {
                return new Response(SystemCode.Warning,"Không tìm thấy khách hàng này", null);
            }
            var firstCustomerDetailDto = customerDetailDtos.First();
            // Kiểm tra xem người dùng hiện thời có quyền xem thông tin tài khoản này không 
            
            var customerDetailView = Mapper.Map<CustomerDetailDto, CustomerDetailModel>(firstCustomerDetailDto);
            return new Response(SystemCode.Success, "", customerDetailView);
        }
        public Response GetListCustomerHistory(CustomerHistoryIndexModel model)
        {
            // Lấy list activity của KH dưới Admin
            var searchResult = this.CustomerHistoryDtoQueryDal.ListWithTotalRow(new HistoryConditon
            {
                CustomerId = model.CustomerId,
               PageIndex = model.PageIndex,
               PageSize = model.PageSize,
            });
            var listModel = Mapper.Instance.Map<List<CustomerHistoryDto>, List<CustomerHistoryModel>>(searchResult.List);
            return new Response(SystemCode.Success, "", new { ListCustomerActivity = listModel, TotalRecord = searchResult.TotalRow });
        }
        public Response GetListCustomerCareHistory(CustomerHistoryIndexModel model)
        {
            // Lấy list activity của KH dưới Admin
            var searchResult = this.CustomerCareHistoryDtoQueryDal.ListWithTotalRow(new HistoryConditon
            {
                CustomerId = model.CustomerId,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
            });
            var listModel = Mapper.Instance.Map<List<CustomerCareHistoryDto>, List<CustomerCareHistoryModel>>(searchResult.List);
            return new Response(SystemCode.Success, "", new { ListCustomerActivity = listModel, TotalRecord = searchResult.TotalRow });
        }
        public Response GetListCustomerNoteHistory(CustomerHistoryIndexModel model)
        {
            // Lấy list activity của KH dưới Admin
            var searchResult = this.CustomerNoteHistoryDtoQueryDal.ListWithTotalRow(new HistoryConditon
            {
                CustomerId = model.CustomerId,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
            });
            var listModel = Mapper.Instance.Map<List<CustomerNoteHistoryDto>, List<CustomerNoteHistoryModel>>(searchResult.List);
            return new Response(SystemCode.Success, "", new { ListCustomerActivity = listModel, TotalRecord = searchResult.TotalRow });
        }
    }
}
