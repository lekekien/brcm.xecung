using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions.Customers
{
    public class PhoneNumberCondition : Condition
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}
