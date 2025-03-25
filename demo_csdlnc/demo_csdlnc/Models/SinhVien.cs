using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demo_csdlnc.Models
{
    public class SinhVien
    {
        [Key]
        public int MaSV { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        public bool GioiTinh { get; set; }

        [Required, EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn lớp")]
       
        public int? MaLop { get; set; }
        [ForeignKey("MaLop")]
        public Lop? Lop { get; set; }

        public int MaAccount { get; set; }
        [ForeignKey("MaAccount")]
        public Account? Account { get; set; }

    }


}
