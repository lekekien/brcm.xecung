using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class CustomerCareHistoryDto: IDto
    {
        public int Id { get; set; }
        public System.DateTime CareStartTime { get; set; }
        public System.DateTime? CareEndTime { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
    }
}
