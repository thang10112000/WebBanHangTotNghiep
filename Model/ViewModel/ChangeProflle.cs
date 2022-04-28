using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ChangeProflle
    {
        [Key]
        public long ID { set; get; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { set; get; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { set; get; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail không hợp lệ")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { set; get; }
    }
}