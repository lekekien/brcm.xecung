using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DTO
{
    public class ListWithTotalRow<T> where T: IDto
    {
        public List<T> List { get; set; }
        public int TotalRow { get; set; }
    }
}
