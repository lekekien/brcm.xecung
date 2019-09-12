using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public enum SystemCode
    {
        /// <summary>
        /// Lỗi
        /// </summary>
        Error = 0,
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 1,
        /// <summary>
        /// Lỗi báo đối tượng được kiểm tra là đã tồn tại
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Check đăng hết thời hạn, or 1 sự cố nào đó phải đăng nhập lại.
        /// </summary>
        NotLogIn = 3,
        /// <summary>
        /// Không có quyền thực hiện && redirect link
        /// </summary>
        NotPermitted = 4
    }
}
