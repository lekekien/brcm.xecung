using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos.Models
{
    public class VideoIndexModel
    {
        public string FilterKeyword { get; set; }
        public int VideoType { get; set; }
        public System.DateTime? SpendDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
