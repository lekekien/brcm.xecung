using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer.Model
{
    public class CustomerDetailModel
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Scource { get; set; }
        public string ScourceName
        {
            get
            {
                return Utils.GetEnumDescription((CustomerScource)this.Scource);
            }
        }
        public int Type { get; set; }
        public string TypeName
        {
            get
            {
                return Utils.GetEnumDescription((CustomerType)this.Type);
            }
        }
        public int Status { get; set; }
        public string StatusName
        {
            get
            {
                return Utils.GetEnumDescription((CustomerStatus)this.Status);
            }
        }
        public string FacebookLink { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public System.DateTime? Birthday { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public string Description { get; set; }
        public int BlockStatus { get; set; }
        public string BlockStatusName
        {
            get
            {
                return Utils.GetEnumDescription((CustomerBlockStatus)this.BlockStatus);
            }
        }
        public int IsFavorite { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}
