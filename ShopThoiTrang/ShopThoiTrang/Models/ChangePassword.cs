using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAffiliateMarketing.Models
{
  
    public class ChangePassword
    {
        [Key]
        public long ID { set; get; }

        [Display(Name = "Nhập mật khẩu ")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
       
        public string Password { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
       
        public string NewPassword { get; set; }

        [Display(Name = "Nhập lại mật khẩu mới")]
      
        [Compare(otherProperty: "NewPassword", ErrorMessage = "Mật khẩu xác nhận sai.")]
        public string ConfirmNewPassword { set; get; }

    }
}