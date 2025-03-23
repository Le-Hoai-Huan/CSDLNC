using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_csdlnc.Models
{
    public class Lop
    {
        [Key]
        public int MaLop { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        [StringLength(50, ErrorMessage = "Tên lớp không được vượt quá 50 ký tự")]
        public string TenLop { get; set; }

        [Required(ErrorMessage = "Mã khoa không được để trống")]
        public int MaKhoa { get; set; }

        // Khóa ngoại
        [ForeignKey("MaKhoa")]
        public Khoa Khoa { get; set; }

        // Danh sách sinh viên thuộc lớp
        public ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}
