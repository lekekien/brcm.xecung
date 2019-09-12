using DVG.CRM.XeCung.InfrastructureLayer.Aggregate;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.DomainLayer.Aggregates.Customers.CustomerCareHistorys
{
    public class CustomerCareHistory : Entity<int>
    {
        #region properties
        /// <summary>
        /// Thời điểm bắt đầu chăm sóc khách hàng
        /// </summary>
        public System.DateTime CareStartTime { get; private set; }
        /// <summary>
        /// Thời điểm ngừng chăm sóc khách hàng
        /// </summary>
        public System.DateTime? CareEndTime { get; private set; }
        /// <summary>
        /// Id của người chăm sóc
        /// </summary>
        public int AssigneeId { get; private set; }
        /// <summary>
        /// Tên người chăm sóc
        /// </summary>
        public string AssigneeName { get; private set; }
        /// <summary>
        /// Trạng thái chăm sóc
        /// </summary>
        public int Status { get; private set; }
        #endregion

        #region constructor
        public CustomerCareHistory(int id, System.DateTime careStartTime, System.DateTime? careEndTime, int assigneeId, string assigneeName, int status)
        {
            this.Id = id;
            this.CareStartTime = careStartTime;
            this.CareEndTime = careEndTime;
            this.AssigneeId = assigneeId;
            this.AssigneeName = assigneeName;
            this.Status = status;
        }
        #endregion

        #region behaviors
        public CustomerCareHistory ReturnCustomer(System.DateTime endTime)
        {
            this.CareEndTime = endTime;
            this.Status = (short)CustomerCareStatus.Return;
            return this;
        }
        #endregion
    }
}
