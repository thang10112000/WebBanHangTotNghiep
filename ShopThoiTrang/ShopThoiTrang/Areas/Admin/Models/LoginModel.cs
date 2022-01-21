using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopThoiTrang.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string UserName { set; get; }

       [Required(ErrorMessage = "Bạn chưa nhập Mật khẩu")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}