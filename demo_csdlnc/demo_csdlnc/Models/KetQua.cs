using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_csdlnc.Models
{
    public class KetQua
    {
        [Key]
        public int MaKetQua { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        public int MaSV { get; set; }

        [Required(ErrorMessage = "Năm học không được để trống")]
        [StringLength(9, ErrorMessage = "Năm học phải có định dạng hợp lệ (VD: 2023-2024)")]
        public string NamHoc { get; set; }

        [Required(ErrorMessage = "Xếp loại không được để trống")]
        public string XepLoai { get; set; }

        public string GhiChu { get; set; }

        [Required(ErrorMessage = "Mã người xét duyệt không được để trống")]
        public int MaNguoiXetDuyet { get; set; }

        // Khóa ngoại
        [ForeignKey("MaSV")]
        public SinhVien SinhVien { get; set; }

        [ForeignKey("MaNguoiXetDuyet")]
        public NguoiXetDuyet NguoiXetDuyet { get; set; }
    }
}
