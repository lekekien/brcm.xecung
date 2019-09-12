using AutoMapper;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.Data.Conditions;
using DVG.CRM.XeCung.Data.Conditions.User;
using DVG.CRM.XeCung.Data.Dtos;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IDtoQueryDal<UsersInAllDto> UserDtoQueryDal;
        private readonly IEntityQueryDal<UsersEntity, int> UserQueryDal;
        private readonly IUserRespository UserRespository;
        private readonly IEntityQueryDal<UserRoleEntity> UserRoleQueryDal;
        private readonly IEntityQueryDal<UserRoleHistoryEntity, int> UserRoleHistoryQueryDal;
        private readonly IEntityQueryDal<UserHistoryEntity, int> UserHistoryQueryDal;
        public UserAppService(
                            IDtoQueryDal<UsersInAllDto> userDtoQueryDal,
                            IEntityQueryDal<UsersEntity, int> userQueryDal,
                            IUserRespository userRespository,
                            IEntityQueryDal<UserRoleEntity> userRoleQueryDal,
                            IEntityQueryDal<UserRoleHistoryEntity, int> userRoleHistoryQueryDal,
                            IEntityQueryDal<UserHistoryEntity, int> userHistoryQueryDal
            )
        {
            this.UserDtoQueryDal = userDtoQueryDal;
            this.UserQueryDal = userQueryDal;
            this.UserRespository = userRespository;
            this.UserRoleQueryDal = userRoleQueryDal;
            this.UserRoleHistoryQueryDal = userRoleHistoryQueryDal;
            this.UserHistoryQueryDal = userHistoryQueryDal;
        }
        public bool Create(AuthenticatedUserModel currUser, UserInfoModel model)
        {       
            model.CreatedBy = currUser.UserName;
            model.LastModifiedBy = currUser.UserName;
            var userEntity = Mapper.Map<UserInfoModel, UsersEntity>(model);
            var newUser = UserFactory.Instance.CreateNew(userEntity);
            string currentRole = "";
            foreach (var item in model.ListUserRoleId)
            {
                newUser = newUser.AddRole(0, item);
                currentRole += Utils.GetEnumDescription((RoleInSystem)item) + ", ";
            }
            //Tạo random key cho cho user
            var randomKey = StringUtils.RandomString(AppSettings.Instance.GetInt32("RandomStringLength"));
            var expiredRandomKey = DateTime.Now.AddDays(AppSettings.Instance.GetInt32("ExpiredRandomKey"));
            newUser.UpdateRandomkey(randomKey, expiredRandomKey);
            //Thêm vào RoleHistory và UserHistory
            newUser.AddRoleHistory(currUser.UserName, "", currentRole.Trim().TrimEnd(','));
            newUser.AddUserHistory(model.UserName, currUser.UserName, UserHistoryStatus.AddNew, "");
            if (newUser.IsValid)
            {
                return this.UserRespository.Add(newUser);
            }
            return false;
        }
        public bool Update(AuthenticatedUserModel currUser, UserInfoModel model)
        {
            var oldDomainUser = this.UserRespository.GetById(model.Id);
            var oldRole = "";
            var currentRole = "";
            foreach (var item in oldDomainUser.ListOfPermission)
            {
                oldRole += Utils.GetEnumDescription(item.Role) + ", ";
            }
            //var birday = model.Birthday.HasValue ? model.Birthday.Value : System.DateTime.MinValue;
            var birday = model.Birthday;
            //Check thay đổi để lưu vào lịch sử
            string oldUserHistory = "";
            string newUserHistory = "";
            if (oldDomainUser.FullName != model.FullName)
            {
                oldUserHistory += "  +FullName: " + oldDomainUser.FullName + "<br>";
                newUserHistory += "  +FullName: " + model.FullName + "<br>";
            }
            if (oldDomainUser.Email != model.Email)
            {
                oldUserHistory += "  +Email: " + oldDomainUser.Email + "<br>";
                newUserHistory += "  +Email: " + model.Email + "<br>";
            }
            if (oldDomainUser.PhoneNumber != model.PhoneNumber)
            {
                oldUserHistory += "  +PhoneNumber: " + oldDomainUser.PhoneNumber + "<br>";
                newUserHistory += "  +PhoneNumber: " + model.PhoneNumber + "<br>";
            }

            if (oldDomainUser.Birthday != birday)
            {
                if (oldDomainUser.Birthday == null || oldDomainUser.Birthday == DateTime.MinValue)
                {
                    oldUserHistory += "  +Birthday: " + "<br>";
                }
                else
                {
                    oldUserHistory += "  +Birthday: " + string.Format("{0: MM/dd/yyyy}", oldDomainUser.Birthday) + "<br>";
                }

                if (birday == DateTime.MinValue)
                {
                    newUserHistory += "  +Birthday: " + "<br>";
                }
                else
                {
                    newUserHistory += "  +Birthday: " + string.Format("{0: MM/dd/yyyy}", birday) + "<br>";
                }


            }
            if (model.Address != null && oldDomainUser.Address != model.Address)
            {
                oldUserHistory += "  +Address: " + oldDomainUser.Address + "<br>";
                newUserHistory += "  +Address: " + model.Address + "<br>";
            }
            if (model.Note != null && oldDomainUser.Note != model.Note)
            {
                oldUserHistory += "  +Note :" + oldDomainUser.Note + "<br>";
                newUserHistory += "  +Note :" + model.Note + "<br>";
            }
            //End Check thay đổi để lưu vào lịch sử.
            var newDomainUser = oldDomainUser.Update(model.FullName, model.Email, model.PhoneNumber, birday, model.Address, model.Note, currUser.UserName);
            if (!string.IsNullOrEmpty(oldUserHistory) && !string.IsNullOrEmpty(newUserHistory))
            {
                string descriptionAction = string.Format("<b>- Old Value</b> <br> {0} <b>- New Value</b> <br> {1}", oldUserHistory.Trim().TrimEnd(','), newUserHistory.Trim().TrimEnd(','));
                newDomainUser.AddUserHistory(newDomainUser.LoginInfo.UserName, currUser.UserName, UserHistoryStatus.UpdateInfor, descriptionAction);
            }
            //Tạo random key cho cho user
            if (DateTime.Now > newDomainUser.ExpiredRandomKey || string.IsNullOrEmpty(model.RandomKey))
            {
                var randomKey = StringUtils.RandomString(AppSettings.Instance.GetInt32("RandomStringLength"));
                var expiredRandomKey = DateTime.Now.AddDays(AppSettings.Instance.GetInt32("ExpiredRandomKey"));
                newDomainUser.UpdateRandomkey(randomKey, expiredRandomKey);
            }
            //var lstOldRole = oldDomainUser.ListOfPermission.Select(role => (int)role.Role).ToList();
            var lstRemoveRoleUser = new List<int>();
            if (currUser.HasRole(RoleInSystem.Admin))
            {
                foreach (var item in newDomainUser.ListOfPermission)
                {
                    var roleId = model.ListUserRoleId.Find(role => role == (int)item.Role);
                    if (roleId > 0)
                    {
                        //Nếu role đã có và giữ nguyên, thì Remove Role ra khỏi list role mới, để chỉ add những role chưa có vào list.
                        model.ListUserRoleId.Remove(roleId);
                    }
                    else
                    {
                        //Nếu role đã có và không giữ nguyên role này nữa thì add vào list remove để remove role trong Domain.
                        lstRemoveRoleUser.Add(item.Id);
                    }
                }
                if (lstRemoveRoleUser.Count > 0)
                {
                    foreach (var id in lstRemoveRoleUser)
                    {
                        //Remove RoleId cũ không được chọn.
                        newDomainUser = oldDomainUser.RemoveRole(id);
                        var res = this.UserRespository.RemoveUserRole(id);
                        if (res.Code != SystemCode.Success)
                        {
                            return false;
                        }
                    }
                }
                //Sau khi Remove, add thêm những RoleId mới.
                if (model.ListUserRoleId.Count > 0)
                {
                    foreach (var roleId in model.ListUserRoleId)
                    {
                        newDomainUser = oldDomainUser.AddRole(0, roleId);
                    }
                }
                //Nếu có role bị xóa, OR role được giữ nguyên + thêm mới role, thì chứng tỏ có thay đổi Role
                if (lstRemoveRoleUser.Count > 0 || model.ListUserRoleId.Count > 0)
                {
                    foreach (var item in newDomainUser.ListOfPermission)
                    {
                        currentRole += Utils.GetEnumDescription(item.Role) + ", ";
                    }
                    newDomainUser.AddRoleHistory(currUser.UserName, oldRole.Trim().TrimEnd(','), currentRole.Trim().TrimEnd(','));
                    newDomainUser.AddUserHistory(newDomainUser.LoginInfo.UserName, currUser.UserName, UserHistoryStatus.ChangeRole, "");
                }
            }
            if (newDomainUser.IsValid)
            {
                return this.UserRespository.Update(newDomainUser);
            }
            else
            {
                return false;
            }
        }
        //public IEnumerable<UserSearchModel> GetList()
        //{
        //    var allUser = this.UserDtoQueryDal.List(new NonCondition());
        //    var lstSearch = Mapper.Instance.Map<IEnumerable<UsersInAllDto>, IEnumerable<UserSearchModel>>(allUser);
        //    return lstSearch;
        //}
        public bool Block(int id)
        {
            var oldDomainUser = this.UserRespository.GetById(id);
            oldDomainUser = oldDomainUser.Block();
            if (oldDomainUser.IsValid)
            {
                return this.UserRespository.Update(oldDomainUser);
            }
            else
            {
                return false;
            }
        }
        public bool Unblock(int id)
        {
            var oldDomainUser = this.UserRespository.GetById(id);
            oldDomainUser = oldDomainUser.UnBlock();
            if (oldDomainUser.IsValid)
            {
                this.UserRespository.Update(oldDomainUser);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChangePassword(int id, string newPassword)
        {
            var oldDomainUser = this.UserRespository.GetById(id);
            oldDomainUser = oldDomainUser.ChangePassword(newPassword.ToMD5());
            if (oldDomainUser.IsValid)
            {
                return this.UserRespository.Update(oldDomainUser);
            }
            else
            {
                return false;
            }
        }
        public UserInfoModel GetById(int id)
        {
            var userEntity = this.UserQueryDal.GetById(id);
            if (userEntity == null)
            {
                return null;
            }
            var userInfo = Mapper.Map<UsersEntity, UserInfoModel>(userEntity);
            //Get list Role
            var lstRole = this.UserRoleQueryDal.List(new UserIdCondition() { UserId = id }).ToList();
            userInfo.ListRole = Mapper.Map<List<UserRoleEntity>, List<UserRoleModel>>(lstRole);
            //Get list Role Id
            userInfo.ListUserRoleId = lstRole.Select(item => item.RoleId).ToList();
            return userInfo;
        }
        public List<UserInfoModel> GetByConditon(int id, string email = null, string phoneNumber = null, string userName = null)
        {
            IEnumerable<UsersEntity> userEntities = null;
            if (!string.IsNullOrEmpty(userName))
            {
                userEntities = this.UserQueryDal.List(new EmailPhoneNumberUsername() { Id = id, UserName = userName.ToLower() });
            }
            else if (!string.IsNullOrEmpty(email))
            {
                userEntities = this.UserQueryDal.List(new EmailPhoneNumberUsername() { Id = id, Email = email.ToLower() });
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                userEntities = this.UserQueryDal.List(new EmailPhoneNumberUsername() { Id = id, PhoneNumber = phoneNumber.ToLower() });
            }

            if (userEntities == null)
            {
                return null;
            }
            var userInfo = Mapper.Map<IEnumerable<UsersEntity>, List<UserInfoModel>>(userEntities);
            return userInfo;
        }
        public bool UpdateOTPPrivateKey(int userId, string key)
        {
            var userOldDomain = this.UserRespository.GetById(userId);
            userOldDomain = userOldDomain.UpdateOTPPrivateKey(key);
            if (userOldDomain.IsValid)
            {
                return this.UserRespository.Update(userOldDomain);
            }
            else
            {
                return false;
            }
        }
        public bool UpdateActivityDate(int userId)
        {
            var oldDomainUser = this.UserRespository.GetById(userId);
            oldDomainUser = oldDomainUser.UpdateActivityDate();
            if (oldDomainUser.IsValid)
            {
                return this.UserRespository.Update(oldDomainUser);
            }
            else
            {
                return false;
            }
        }
        public List<UserRoleHistoryModel> ListUserRoleHistory(UserRoleHistoryIndexModel roleHistory, out int totalRecord)
        {
            throw new NotImplementedException();
            ////Get list RoleHistory
            //var lstRoleHistoryEntity = this.UserRoleHistoryQueryDal.List(new UserIdPagingCondition() { UserId = roleHistory.UserId, PageIndex = roleHistory.PageIndex, PageSize = roleHistory.PageSize }).ToList();
            //totalRecord = this.UserRoleHistoryQueryDal.CountTotalRecord(new UserIdCondition() { UserId = roleHistory.UserId });
            //var lstRoleHistory = new List<UserRoleHistoryModel>();
            //if (totalRecord > 0)
            //{
            //    lstRoleHistory = Mapper.Map<List<UserRoleHistoryEntity>, List<UserRoleHistoryModel>>(lstRoleHistoryEntity);
            //}
            //return lstRoleHistory;
        }
        public List<UserHistoryModel> ListUserHistory(UserHistoryIndexModel userHistory, out int totalRecord)
        {
            throw new NotImplementedException();
            ////Get list UserHistory
            //var lstUserHistoryEntity = this.UserHistoryQueryDal.List(new UserIdPagingCondition() { UserId = userHistory.UserId, PageIndex = userHistory.PageIndex, PageSize = userHistory.PageSize }).ToList();
            //totalRecord = this.UserHistoryQueryDal.CountTotalRecord(new UserIdCondition() { UserId = userHistory.UserId });
            //var lstUserHistory = new List<UserHistoryModel>();
            //if (totalRecord > 0)
            //{
            //    lstUserHistory = Mapper.Map<List<UserHistoryEntity>, List<UserHistoryModel>>(lstUserHistoryEntity);
            //}
            //return lstUserHistory;
        }

        public List<UserSearchModel> GetListPaging(UserIndexModel model, AuthenticatedUserModel currUser, out int totalRecord)
        {
            var condition = new CountUserGetlistPaging()
            {
                FilterKeyWord = !string.IsNullOrEmpty(model.FilterKeyWord) ? model.FilterKeyWord.ToLower() : "",
            };
            var allUserPaging = this.UserDtoQueryDal.ListWithTotalRow(new UserGetlistPagingCondition(condition, model.PageIndex, model.PageSize));
            totalRecord = allUserPaging.TotalRow;
            var lstSearch = Mapper.Instance.Map<IEnumerable<UsersInAllDto>, IEnumerable<UserSearchModel>>(allUserPaging.List).ToList();
            if (totalRecord > 0)
            {
                foreach (var item in lstSearch)
                {
                    item.Position = "";
                    var lstRole = this.UserRoleQueryDal.List(new UserIdCondition() { UserId = item.Id }).ToList();
                    foreach (var role in lstRole)
                    {
                        item.Position += Utils.GetEnumDescription((RoleInSystem)role.RoleId) + ", ";
                    }
                    item.Position = item.Position.Trim().TrimEnd(',');
                }
            }
            return lstSearch;
        }
    }
}
