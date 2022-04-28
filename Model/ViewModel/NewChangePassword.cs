using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class NewChangePassword
    {
        public long Id { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận sai.")]
        public string ConfirmPassword { set; get; }
    }
}