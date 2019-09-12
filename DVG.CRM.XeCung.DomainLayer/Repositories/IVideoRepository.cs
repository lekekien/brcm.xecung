using DVG.CRM.XeCung.DomainLayer.Aggregates.Videos;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Repositories
{
    public interface IVideoRepository
    {
        Response Add(Video model);
        Response Delete(Video model);
        Video GetById(int id);
        Response Update(Video model);
    }
}
