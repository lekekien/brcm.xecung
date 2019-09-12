using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.Data.Dtos
{
    public class CustomerNoteHistoryDto: IDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
