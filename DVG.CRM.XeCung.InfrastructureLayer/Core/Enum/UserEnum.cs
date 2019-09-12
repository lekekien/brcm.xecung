using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public enum UserStatus : int
    {
        NotAvaiable = 0,
        Avaiable = 1
    }
    public enum UserHistoryStatus
    {
        [Description("Block")]
        Block = 1,
        [Description("UnBlock")]
        UnBlock = 2,
        [Description("Change Password")]
        ChangePassword = 3,
        [Description("Change Role")]
        ChangeRole = 4,
        [Description("Add New User")]
        AddNew = 5,
        [Description("Update User Information")]
        UpdateInfor = 6,
        [Description("Get OTP Key")]
        GetOTPKey = 7,
    }
}
