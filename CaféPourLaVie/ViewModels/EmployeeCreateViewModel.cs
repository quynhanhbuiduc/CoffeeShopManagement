using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Ngày vào làm không được để trống")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        public string Role { get; set; }
    }
}