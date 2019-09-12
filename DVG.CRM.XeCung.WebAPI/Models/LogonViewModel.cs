using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.WebAPI.Models
{
    public class LogonViewModel
    {
        [Required(ErrorMessage = "Bạn vui lòng điền tên đăng nhập!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn vui lòng điền mật khẩu")]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
        public string OtpPrivateKey { get; set; }
    }
}
