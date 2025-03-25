using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_csdlnc.Models
{
    public class ThamGiaHoatDong
    {
        [Key]
        public int MaThamGia { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        public int MaSV { get; set; }

        [Required(ErrorMessage = "Tên hoạt động không được để trống")]
        [StringLength(100, ErrorMessage = "Tên hoạt động không được vượt quá 100 ký tự")]
        public string TenHoatDong { get; set; }

        [Required(ErrorMessage = "Ngày tham gia không được để trống")]
        [DataType(DataType.Date)]
        public DateTime NgayThamGia { get; set; }

        [Range(0, 100, ErrorMessage = "Điểm số phải từ 0 đến 100")]
        public int DiemSo { get; set; }

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}
