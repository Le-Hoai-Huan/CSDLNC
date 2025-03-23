using System.ComponentModel.DataAnnotations;

namespace demo_csdlnc.Models
{
    public class TieuChi
    {
        [Key]
        public int MaTieuChi { get; set; }

        [Required(ErrorMessage = "Tên tiêu chí không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tiêu chí không được vượt quá 100 ký tự")]
        public string TenTieuChi { get; set; }

        [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
        public string MoTa { get; set; }
    }
}
