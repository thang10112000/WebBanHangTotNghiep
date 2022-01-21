using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAffiliateMarketing.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { set; get; }

        [Display(Name="Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { set; get; }
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password",ErrorMessage ="Mật khẩu xác nhận sai.")]
        public string ConfirmPassword { set; get; }
        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        public string Name { set; get; }
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string Email { set; get; }
        [Display(Name = "Số điện thoại")]

        public string Phone { set; get; }
     


    }
}