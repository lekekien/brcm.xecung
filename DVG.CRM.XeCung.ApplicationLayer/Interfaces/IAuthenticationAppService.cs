using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.ApplicationLayer.Interfaces
{
    public interface IAuthenticationAppService
    {
        Task<Response> Login(string userName, string password, string otpprivatekey);
        void LogOut();
        void LogOutAndClearToken(long userId, string userToken);
        AuthenticatedUserModel GetCurrentUser();
        bool IsAuthenticated();
    }
}
