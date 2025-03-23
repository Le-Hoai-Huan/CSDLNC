using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demo_csdlnc.Models
{
    public class DangKy
    {
        [Key]
        public int MaDangKy { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        public int MaSV { get; set; }

        [Required(ErrorMessage = "Ngày đăng ký không được để trống")]
        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; } = "Chờ Duyệt";

        public int? MaNguoiXetDuyet { get; set; }

        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        [ForeignKey("MaNguoiXetDuyet")]
        public NguoiXetDuyet NguoiXetDuyet { get; set; }
    }
}
