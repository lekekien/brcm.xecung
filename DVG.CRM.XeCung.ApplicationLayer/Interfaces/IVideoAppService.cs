using DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models;
using DVG.CRM.XeCung.ApplicationLayer.Authentication.Models;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Interfaces
{
    public interface IVideoAppService
    {
        VideoEditModel GetByCode(AuthenticatedUserModel currUser, string VideoCode);
        Response Create(AuthenticatedUserModel currUser, VideoInfoModel model);
        Response Edit(AuthenticatedUserModel currUser, VideoEditModel model);
        Response Search(AuthenticatedUserModel currUser, VideoIndexModel model);
        Response Delete(AuthenticatedUserModel currUser, VideoInfoModel model);
    }
}
