using AutoMapper;
using DVG.CRM.XeCung.Data.Conditions;
using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users.UserPermissions;
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
    public class UserRespository : IUserRespository
    {
        private readonly ICommandDal<UserRoleEntity> UserPermissionEntityCommandDal;
        private readonly ICommandDal<UsersEntity, int> UsersCommandDal;
        private readonly IEntityQueryDal<UsersEntity, int> UserEntityQuery;
        private readonly IEntityQueryDal<UserRoleEntity, int> UserRoleEntityQuery;
        private readonly ICommandDal<UserRoleHistoryEntity, int> UserRoleHistoryEntityCommandDal;
        private readonly ICommandDal<UserHistoryEntity, int> UserHistoryEntityCommandDal;
        public UserRespository(ICommandDal<UserRoleEntity> userPermissionEntityCommandDal
                             , ICommandDal<UsersEntity, int> usersEntityDal
                             , IEntityQueryDal<UsersEntity, int> userEntityQuery
                             , IEntityQueryDal<UserRoleEntity, int> userRoleEntityQuery
                             , ICommandDal<UserRoleHistoryEntity, int> userRoleHistoryEntityCommandDal
                             , ICommandDal<UserHistoryEntity, int> userHistoryEntityCommandDal)
        {
            this.UserPermissionEntityCommandDal = userPermissionEntityCommandDal;
            this.UsersCommandDal = usersEntityDal;
            this.UserEntityQuery = userEntityQuery;
            this.UserRoleEntityQuery = userRoleEntityQuery;
            this.UserRoleHistoryEntityCommandDal = userRoleHistoryEntityCommandDal;
            this.UserHistoryEntityCommandDal = userHistoryEntityCommandDal;
        }

        public bool Add(User model)
        {
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.UsersCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserPermissionEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserRoleHistoryEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserHistoryEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    var id = UsersCommandDal.AddGetId(Mapper.Map<User, UsersEntity>(model));
                    foreach (var item in model.ListOfPermission)
                    {
                        var entityItem = Mapper.Map<UserRole, UserRoleEntity>(item);
                        entityItem.UserId = id;
                        this.UserPermissionEntityCommandDal.Add(entityItem);
                    }
                    //Add Role History
                    var userRoleHistory = model.ListUserRoleHistory.FirstOrDefault();
                    var roleHistoryEntity = Mapper.Map<UserRoleHistory, UserRoleHistoryEntity>(userRoleHistory);
                    roleHistoryEntity.UserId = id;
                    this.UserRoleHistoryEntityCommandDal.Add(roleHistoryEntity);
                    //Add User History
                    var userHistory = model.ListUserHistory.FirstOrDefault();
                    var userHistoryEntity = Mapper.Map<UserHistory, UserHistoryEntity>(userHistory);
                    userHistoryEntity.UserId = id;
                    this.UserHistoryEntityCommandDal.Add(userHistoryEntity);
                    unitOfWork.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    unitOfWork.Rollback();
                    return false;
                }
            }
        }

        public User GetById(int id)
        {
            var userEntity = this.UserEntityQuery.GetById(id);
            if (userEntity == null)
            {
                return null;
            }
            var userDomain = UserFactory.Instance.CreateExisting(userEntity);

            var lstUserRoleEntity = this.UserRoleEntityQuery.List(new UserIdCondition() { UserId = id });
            foreach (var roleItem in lstUserRoleEntity)
            {
                userDomain.AddRole(roleItem.Id, roleItem.RoleId);
            }
            return userDomain;
        }

        public bool Update(User model)
        {
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                unitOfWork.BeginTransaction();
                try
                {
                    this.UsersCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserPermissionEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserRoleHistoryEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserHistoryEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UsersCommandDal.Update(Mapper.Map<User, UsersEntity>(model));

                    //Add Role
                    foreach (var item in model.ListOfPermission)
                    {
                        var entityItem = Mapper.Map<UserRole, UserRoleEntity>(item);
                        entityItem.UserId = model.Id;
                        if (entityItem.Id > 0)
                        {
                            this.UserPermissionEntityCommandDal.Update(entityItem);
                        }
                        else
                        {
                            this.UserPermissionEntityCommandDal.Add(entityItem);
                        }
                    }
                    //Add Role History
                    var userRoleHistory = model.ListUserRoleHistory.FirstOrDefault();
                    if (userRoleHistory != null)
                    {
                        var roleHistoryEntity = Mapper.Map<UserRoleHistory, UserRoleHistoryEntity>(userRoleHistory);
                        roleHistoryEntity.UserId = model.Id;
                        this.UserRoleHistoryEntityCommandDal.Add(roleHistoryEntity);
                    }
                    //Add User History
                    var userHistory = model.ListUserHistory.FirstOrDefault();
                    if (userHistory != null)
                    {
                        var userHistoryEntity = Mapper.Map<UserHistory, UserHistoryEntity>(userHistory);
                        userHistoryEntity.UserId = model.Id;
                        this.UserHistoryEntityCommandDal.Add(userHistoryEntity);
                    }
                    unitOfWork.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return false;
                }
            }
        }
        public Response RemoveUserRole(int id)
        {
            using (var unitOfWork = DVGServiceLocator.Current.GetInstance<IUnitOfWork>())
            {
                try
                {
                    this.UserPermissionEntityCommandDal.SetWriteDbContext(unitOfWork.GetDbContext());
                    this.UserPermissionEntityCommandDal.DeleteById(id);
                    return new Response(SystemCode.Success, "", null);
                }
                catch (Exception ex)
                {
                    return new Response(SystemCode.Error, ex.Message, null);
                }
            }
        }
    }
}
