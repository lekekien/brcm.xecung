using DVG.CRM.XeCung.Data.Entities;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerCareHistorys;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerHistories;
using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using DVG.CRM.XeCung.InfrastructureLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Customers
{
    public class Customer : AggregateRoot<int>
    {
        #region Property
        public string CustomerCode { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public int Scource { get; private set; }
        public int Type { get; private set; }
        public int Status { get; private set; }
        public string FacebookLink { get; private set; }
        public string Company { get; private set; }
        public string Position { get; private set; }
        public System.DateTime? Birthday { get; private set; }
        public int AssigneeId { get; private set; }
        public string Description { get; private set; }
        public int BlockStatus { get; private set; }
        public int IsFavorite { get; private set; }
        public string CreatedBy { get; private set; }
        public System.DateTime CreatedDate { get; private set; }
        public string LastModifiedBy { get; private set; }
        public System.DateTime LastModifiedDate { get; private set; }
        public string FullTextSearch { get; private set; }
        public List<CustomerCareHistory> LastestCustomerCareHistories { get; private set; }
        public List<CustomerHistory> ListCustomerHistories { get; private set; }
        #endregion
        #region Contructor
        public Customer(int id, string customerCode, string name, string phoneNumber, string email,
            int scource, int type, int status, string facebookLink, string company, string position, System.DateTime? birthday, int assigneeId,
            string description, int blockStatus, int isFavorite, string createdBy, System.DateTime createdDate, string lastModifiedBy, System.DateTime lastModifiedDate, string fullTextSearch)
        {
            this.Id = id;
            this.CustomerCode = customerCode;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Scource = scource;
            this.Type = type;
            this.Status = status;
            this.FacebookLink = facebookLink;
            this.Company = company;
            this.Position = position;
            this.Birthday = birthday;
            this.AssigneeId = assigneeId;
            this.Description = description;
            this.BlockStatus = blockStatus;
            this.IsFavorite = isFavorite;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
            this.LastModifiedBy = lastModifiedBy;
            this.LastModifiedDate = lastModifiedDate;
            this.FullTextSearch = fullTextSearch;
            this.LastestCustomerCareHistories = new List<CustomerCareHistory>();
            this.ListCustomerHistories = new List<CustomerHistory>();
        }
        #endregion
        #region Behavior
        // Add lịch sử thay đổi thông tin KH
        public Customer AddCustomerHistory(string createdBy, CustomerHistoryStatus action, string descriptionAction)
        {
            string actionName = Utils.GetEnumDescription(action);
            if (!string.IsNullOrEmpty(descriptionAction))
            {
                actionName = string.Format("{0} <br> {1}", actionName, descriptionAction);
            }
            this.ListCustomerHistories.Add(new CustomerHistory(0, actionName, createdBy, DateTime.Now));
            return this;
        }
        // Assign KH  cho sale
        public Customer AddAssignee(int assigneeId, string assigneeName, string lastModifiedBy)
        {
            if (this.LastestCustomerCareHistories == null || this.LastestCustomerCareHistories.Count == 0)
            {
                this.LastestCustomerCareHistories = new List<CustomerCareHistory>();
            }
            else
            {
                var lastest = this.LastestCustomerCareHistories.OrderByDescending(o => o.CareStartTime).First();
                lastest = lastest.ReturnCustomer(DateTime.Now.AddSeconds(-1));
            }
            this.LastestCustomerCareHistories.Add(new CustomerCareHistory(0, DateTime.Now, null, assigneeId, assigneeName, (int)CustomerCareStatus.Received));
            this.AssigneeId = assigneeId;
            this.LastModifiedBy = lastModifiedBy;
            this.LastModifiedDate = DateTime.Now;
            return this;
        }
        //Add chăm sóc KH mới nhất vào domain
        public Customer AddCustomerCareHistory(CustomerCareHistoryEntity entity)
        {
            this.LastestCustomerCareHistories.Add(new CustomerCareHistory(entity.Id, entity.CareStartTime, entity.CareEndTime, entity.AssigneeId, entity.AssigneeName, entity.Status));
            return this;
        }
        //Update KH
        public Customer Edit(CustomerEntity entity, string lastModifiedBy) {
            this.Name = entity.Name;
            this.PhoneNumber = entity.PhoneNumber;
            this.Email = entity.Email;
            this.Scource = entity.Scource;
            this.Type = entity.Type;
            this.Status = entity.Status;
            this.FacebookLink = entity.FacebookLink;
            this.Company = entity.Company;
            this.Position = entity.Position;
            this.Birthday = entity.Birthday;
            this.AssigneeId = entity.AssigneeId;
            this.Description = entity.Description;
            this.BlockStatus = entity.BlockStatus;
            this.IsFavorite = entity.IsFavorite;
            this.LastModifiedBy = lastModifiedBy;
            this.LastModifiedDate = DateTime.Now;
            this.FullTextSearch = entity.FullTextSearch;
            return this;
        }
        #endregion
    }
}
