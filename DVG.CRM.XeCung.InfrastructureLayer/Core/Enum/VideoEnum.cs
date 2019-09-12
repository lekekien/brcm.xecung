using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public enum VideoType
    {
        [Description("Theo hợp đồng")]
        ContractVideo = 1,

        [Description("Không theo hợp đồng")]
        NonContractVideo = 2,

        [Description("Tự sản xuất")]
        SelfProductionVideo = 3,

    }
}
