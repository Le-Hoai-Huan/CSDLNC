using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace demo_csdlnc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Accounts.AnyAsync(a => a.Username == model.Username))
            {
                ModelState.AddModelError("", "Username đã tồn tại!");
                return View(model);
            }

            // Mã hóa mật khẩu
            string hashedPassword = HashPassword(model.Password);

            var account = new Account
            {
                Username = model.Username,
                Password = hashedPassword,
                Role = model.Role
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == model.Username);
            if (account == null || !VerifyPassword(model.Password, account.Password))
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View(model);
            }

            // Lưu thông tin đăng nhập vào Session
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Role", account.Role);
            HttpContext.Session.SetInt32("UserId", account.MaAccount);

            return RedirectToAction("Index", "Home");
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Hàm băm mật khẩu
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Kiểm tra mật khẩu
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return HashPassword(inputPassword) == storedPassword;
        }
    }
}
