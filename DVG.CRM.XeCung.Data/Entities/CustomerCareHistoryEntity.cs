using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Entities
{
    public class CustomerCareHistoryEntity: DbEntity<int>
    {
        public System.DateTime CareStartTime { get; set; }
        public System.DateTime? CareEndTime { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
    }
}
