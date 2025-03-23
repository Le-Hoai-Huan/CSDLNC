using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_csdlnc.Models
{
    public class NguoiXetDuyet
    {
        [Key]
        public int MaNguoiXetDuyet { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Phải có mã tài khoản")]
        public int MaAccount { get; set; }

        [ForeignKey("MaAccount")]
        public Account? Account { get; set; }
    }
}
