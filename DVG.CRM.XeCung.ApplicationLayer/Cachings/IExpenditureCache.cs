using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.Cachings
{
    public interface IExpenditureCache
    {
        IEnumerable<KeyValuePair<int, string>> GetAllExpenditureInKeyValue();
    }
}
