using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users.Models;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Interfaces
{
    public interface IUserAppService
    {
        bool Create(AuthenticatedUserModel currUser, UserInfoModel model);
        bool Update(AuthenticatedUserModel currUser, UserInfoModel model);
        //IEnumerable<UserSearchModel> GetList();
        List<UserSearchModel> GetListPaging(UserIndexModel model, AuthenticatedUserModel currUser, out int totalRecord);
        bool Block(int id);
        bool Unblock(int id);
        bool ChangePassword(int id, string newPassword);
        UserInfoModel GetById(int id);
        List<UserInfoModel> GetByConditon(int id, string email = null, string phoneNumber = null, string userName = null);
        bool UpdateOTPPrivateKey(int userId, string Key);
        bool UpdateActivityDate(int userId);
    }
}
