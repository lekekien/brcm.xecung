using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Factory;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users
{
    public class UserFactory : Factory
    {
        public static UserFactory Instance => GetInstance<UserFactory>();

        //public User CreateNew(string userName, string password, UserStatus status, string fullName, int groupId, string createdBy, System.DateTime createdDate, System.DateTime? birthday, int cityId, string address, string note, string email, string phoneNumber, string otpprivatekey, string randomkey, System.DateTime expiredRandomKey)
        //{
        //    return new User(DateTime.Now.Ticks, userName, password, status, fullName, groupId, createdBy, createdDate, birthday, cityId, address, note, email, phoneNumber, otpprivatekey, randomkey, expiredRandomKey);
        //}

        public User CreateNew(UsersEntity entity)
        {
            return new User(0, entity.UserName, entity.Password.ToMD5(), UserStatus.Avaiable, entity.FullName, entity.CreatedBy, DateTime.Now, entity.Birthday, entity.Address, entity.Note, entity.Email, entity.PhoneNumber, entity.OTPPrivateKey, entity.RandomKey, entity.ExpiredRandomKey, entity.LastModifiedBy, DateTime.Now, entity.LastActivityDate);
        }

        public User CreateExisting(UsersEntity entity)
        {
            return new User(entity.Id, entity.UserName, entity.Password, (UserStatus)entity.Status, entity.FullName, entity.CreatedBy, entity.CreatedDate, entity.Birthday, entity.Address, entity.Note, entity.Email, entity.PhoneNumber, entity.OTPPrivateKey, entity.RandomKey, entity.ExpiredRandomKey, entity.LastModifiedBy, entity.LastModifiedDate, entity.LastActivityDate);
        }
    }
}
