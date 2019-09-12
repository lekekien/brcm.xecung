using DVG.CRM.XeCung.DomainLayer.Aggregates.Users.UserPermissions;
using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Users
{
    public class User : AggregateRoot<int>
    {
        #region constructors
        public User(int id, string userName, string password, UserStatus status, string fullName, string createdBy, System.DateTime createdDate, System.DateTime? birthday, string address
            , string note, string email, string phoneNumber, string otpprivatekey, string randomkey, System.DateTime expiredRandomKey, string lastModifiedBy, System.DateTime lastModifiedDate, System.DateTime? lastActivityDate)
        {
            this.Id = id;
            this.LoginInfo = new LoginInfo(userName.ToLower(), password);
            this.Status = status;
            this.FullName = fullName;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
            this.Birthday = birthday;
            this.Address = address;
            this.Note = note;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.OTPPrivateKey = otpprivatekey;
            this.RandomKey = randomkey;
            this.ExpiredRandomKey = expiredRandomKey;
            this.LastModifiedBy = lastModifiedBy;
            this.LastModifiedDate = lastModifiedDate;
            this.LastActivityDate = lastActivityDate;
            this.ListOfPermission = new List<UserRole>();
            this.Validator = new UserValidator();
            this.ListUserRoleHistory = new List<UserRoleHistory>();
            this.ListUserHistory = new List<UserHistory>();
        }
        #endregion

        #region properties
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public System.DateTime? Birthday { get; private set; }
        public string Address { get; private set; }
        public string Note { get; private set; }
        public LoginInfo LoginInfo { get; private set; }
        public UserStatus Status { get; private set; }
        public string CreatedBy { get; private set; }
        public System.DateTime CreatedDate { get; private set; }
        public string OTPPrivateKey { get; private set; }
        public string RandomKey { get; private set; }
        public System.DateTime ExpiredRandomKey { get; private set; }
        public string LastModifiedBy { get; private set; }
        public System.DateTime LastModifiedDate { get; private set; }
        public System.DateTime? LastActivityDate { get; private set; }
        public List<UserRole> ListOfPermission { get; private set; }
        public List<UserRoleHistory> ListUserRoleHistory { get; private set; }
        public List<UserHistory> ListUserHistory { get; private set; }
        #endregion

        #region behaviors
        public User Block()
        {
            this.Status = UserStatus.NotAvaiable;
            return this;
        }
        public User UnBlock()
        {
            this.Status = UserStatus.Avaiable;
            return this;
        }
        public User ChangePassword(string newPassword)
        {
            this.LoginInfo = new LoginInfo(this.LoginInfo.UserName, newPassword);
            return this;
        }
        public User AddRole(int id, int role)
        {
            this.ListOfPermission.Add(new UserRole(id, (RoleInSystem)role));
            return this;
        }

        public User ChangeRole(int userRoleId, RoleInSystem newRole)
        {
            var roleOfUser = this.ListOfPermission.Find(o => o.Id == userRoleId);
            if (roleOfUser == null)
            {
                return this;
            }
            roleOfUser = new UserRole(roleOfUser.Id, newRole);
            return this;
        }

        public User Update(string fullName, string email, string phoneNumber, System.DateTime? birthday, string address, string note, string lastModifiedBy)
        {
            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Birthday = birthday;
            this.Address = address;
            this.Note = note;
            this.LastModifiedBy = lastModifiedBy;
            this.LastModifiedDate = DateTime.Now;
            return this;
        }
        public User UpdateActivityDate()
        {
            this.LastActivityDate = DateTime.Now;
            return this;
        }
        public User UpdateRandomkey(string randomKey, System.DateTime expiredRandomKey)
        {
            RandomKey = randomKey;
            ExpiredRandomKey = expiredRandomKey;
            return this;
        }
        public User UpdateOTPPrivateKey(string otpPrivateKey)
        {
            this.OTPPrivateKey = otpPrivateKey;
            return this;
        }
        public User AddRoleHistory(string changeBy, string oldRole, string currentRole)
        {
            this.ListUserRoleHistory.Add(new UserRoleHistory(0, changeBy, DateTime.Now, oldRole, currentRole));
            return this;
        }
        public User AddUserHistory(string username, string modifiedBy, UserHistoryStatus action, string descriptionAction)
        {
            string actionName = Utils.GetEnumDescription(action);
            if (!string.IsNullOrEmpty(descriptionAction))
            {
                actionName = string.Format("{0} <br> {1}", actionName, descriptionAction);
            }
            this.ListUserHistory.Add(new UserHistory(0, username, DateTime.Now, modifiedBy, actionName));
            return this;
        }
        public User RemoveRole(int id)
        {
            var roleRemove = this.ListOfPermission.Find(o => o.Id == id);
            this.ListOfPermission.Remove(roleRemove);
            return this;
        }
        #endregion
    }
}
