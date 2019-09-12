using DVG.CRM.XeCung.ApplicationLayer.Cachings;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Expenditure
{
    
    public class ExpenditureAppService : IExpenditureAppService
    {
        IExpenditureCache ExpenditureCache;
        public ExpenditureAppService(IExpenditureCache expenditureCache)
        {
            this.ExpenditureCache = expenditureCache;
        }
        public IEnumerable<KeyValuePair<int, string>> GetAllExpenditureInKeyValue()
        {
            return this.ExpenditureCache.GetAllExpenditureInKeyValue();
        }
    }
}
