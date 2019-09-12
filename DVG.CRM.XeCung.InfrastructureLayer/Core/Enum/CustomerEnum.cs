using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public enum CustomerScource
    {
        [Description("Facebook")]
        Facebook = 1,
        [Description("Fanpage")]
        Fanpage = 2,
        [Description("Youtube")]
        Youtube = 3,
        [Description("Hotline")]
        Hotline = 4,
        [Description("Người quen giới thiệu")]
        AcquaintancesRecommend = 5,
        [Description("Khác")]
        Other = 6
    }
    public enum CustomerStatus
    {
        [Description("Chưa liên hệ")]
        NotContacted = 1,
        [Description("Đang trao đổi")]
        Discussing = 2,
        [Description("Đang làm hợp đồng")]
        MakingContract = 3,
        [Description("Hoàn thành")]
        Finish = 4,
        [Description("Liên hệ lại sau")]
        ContactLater= 5,
    }
    public enum CustomerType
    {
        [Description("Agency")]
        Agency = 1,
        [Description("Hãng Oto")]
        OtoBrand = 2,
        [Description("Đại lý Oto")]
        OtoDearlership = 3,
        [Description("Showroom Oto")]
        OtoShowroom = 4,
        [Description("Sale Oto")]
        OtoSale = 5,
        [Description("Khác")]
        Other = 6
    }
    public enum CustomerTypeShortName
    {
        [Description("AG")]
        Agency = 1,
        [Description("OBR")]
        OtoBrand = 2,
        [Description("ODL")]
        OtoDearlership = 3,
        [Description("OSR")]
        OtoShowroom = 4,
        [Description("OSA")]
        OtoSale = 5,
        [Description("OT")]
        Other = 6
    }
    public enum CustomerBlockStatus
    {
        [Description("")]
        None = 0,
        [Description("Khóa khách hàng")]
        Block = 1,
    }
    public enum CustomerCareStatus
    {
        /// <summary>
        /// Receive
        /// </summary>
        [Description("Nhận khách hàng")]
        Received = 1,
        /// <summary>
        /// Return
        /// </summary>
        [Description("Trả khách hàng")]
        Return = 2
    }
    public enum CustomerHistoryStatus
    {
        [Description("Khóa khách hàng")]
        //[LocalizedDescription("Msg_Admin", typeof(AppValidateMessage))]
        Block = 1,
        [Description("Mở khóa khách hàng")]
        UnBlock = 2,
        [Description("Tạo khách hàng mới")]
        AddNew = 5,
        [Description("Thay đổi thông tin khách hàng")]
        UpdateInfor = 6,
    }
}
