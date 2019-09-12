using DVG.CRM.XeCung.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Interfaces
{
    public interface IExpenditureAppService
    {
        IEnumerable<KeyValuePair<int, string>> GetAllExpenditureInKeyValue();
    }
}
