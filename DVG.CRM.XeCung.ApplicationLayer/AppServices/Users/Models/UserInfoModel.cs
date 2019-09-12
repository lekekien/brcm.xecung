using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {
            this.ListRole = new List<UserRoleModel>();
            this.ListUserRoleId = new List<int>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public short Status { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string RoleName
        {
            get
            {
                var name = "";
                if (this.ListRole.Count > 0)
                {
                    foreach (var item in ListRole)
                    {
                        name = name + Utils.GetEnumDescription((RoleInSystem)item.RoleId) + ", ";
                    }
                }
                return name.Trim().TrimEnd(',');
            }
        }
        public int UserRoleId { get; set; }
        public string OTPPrivateKey { get; set; }
        public string RandomKey { get; set; }
        public System.DateTime ExpiredRandomKey { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public List<UserRoleModel> ListRole { get; set; }
        public List<int> ListUserRoleId { get; set; }
    }
    public class UserInforViewModel : UserInfoModel
    {
        public string Token { get; set; }
    }
    public class ChangePassWord : UserInforViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
