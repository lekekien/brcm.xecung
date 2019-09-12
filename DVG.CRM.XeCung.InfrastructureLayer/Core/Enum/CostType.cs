using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    //lưu các nội dung chi của chi phí sản xuất
    public enum CostContent
    {
        [Description("Xăng xe")]
        Fuel = 1,

        [Description("Thuê xe")]
        Vehicles = 2,

        [Description("Thuê thiết bị")]
        Devices = 3,

        [Description("Thuê MC")]
        MC = 4,

        [Description("Dự phòng")]
        BackUp = 5,

        [Description("Công tác phí")]
        Expenses = 6,

        [Description("Chi phí khác")]
        Other = 7,

    }
}
