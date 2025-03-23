using System.ComponentModel.DataAnnotations;

namespace demo_csdlnc.Models
{
    public class Account
    {
        [Key]
        public int MaAccount { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
