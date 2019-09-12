using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Conditions.Video
{
    public class VideoCodeCondition : Condition
    {
        public string VideoCode { get; set; }
    }
    public class VideoCodeAndIDCondition : Condition
    {
        public int Id { get; set; }
        public string VideoCode { get; set; }
    }
}
