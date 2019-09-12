using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    [Serializable]
    public class ExpenditureEntity : DbEntity<int>
    {
        public string ExpenditureType { get; set; }
    }
}
