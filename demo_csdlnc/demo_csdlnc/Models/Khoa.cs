using System.ComponentModel.DataAnnotations;

namespace demo_csdlnc.Models
{
    public class Khoa
    {
        [Key]
        public int MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public ICollection<Lop> Lops { get; set; }
    }
}
