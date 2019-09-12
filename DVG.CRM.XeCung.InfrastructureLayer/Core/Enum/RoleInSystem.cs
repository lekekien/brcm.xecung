using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public enum RoleInSystem
    {
        [Description("Customer Manager")]
        CustomerManager = 1,
        [Description("Contract Manager")]
        ContractManager = 2,
        [Description("Revenue Manager")]
        RevenueManager = 3,
        [Description("Manager")]
        Manager = 4,
        [Description("Admin")]
        Admin = 5
    }
}
