using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class CustomerHistoryDto: IDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Action { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
