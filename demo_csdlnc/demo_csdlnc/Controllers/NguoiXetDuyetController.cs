using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Controllers
{
    public class NguoiXetDuyetController : Controller
    {
        private readonly AppDbContext _context;

        public NguoiXetDuyetController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var nguoiXetDuyets = _context.NguoiXetDuyets.Include(xd => xd.Account).ToList();
            return View(nguoiXetDuyets);
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Create()
        {
            var sinhViens = _context.SinhViens.ToList();
            var nguoiXetDuyets = _context.NguoiXetDuyets.Include(n => n.Account).ToList();

            // Đảm bảo không bị null
            sinhViens = sinhViens ?? new List<SinhVien>();
            nguoiXetDuyets = nguoiXetDuyets ?? new List<NguoiXetDuyet>();

            ViewBag.MaSV = sinhViens.Any() ? new SelectList(sinhViens, "MaSV", "TenSV") : new SelectList(Enumerable.Empty<SinhVien>());
            ViewBag.MaNguoiXetDuyet = nguoiXetDuyets.Any()
                ? new SelectList(nguoiXetDuyets.Where(n => n.Account != null), "MaNguoiXetDuyet", "Account.Username")
                : new SelectList(Enumerable.Empty<NguoiXetDuyet>());

            return View();
        }

        [HttpPost]
        public IActionResult Create(NguoiXetDuyet nguoiXetDuyet)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }          
            if (!ModelState.IsValid)
            {
                var nguoiXetDuyetAccounts = _context.Accounts
                .Where(a => a.Role == "NguoiXetDuyet")
                .Select(a => new { a.MaAccount, a.Username })
                .ToList();

                ViewBag.MaAccount = new SelectList(nguoiXetDuyetAccounts, "MaAccount", "Username");
                return View(nguoiXetDuyet);
            }
            _context.NguoiXetDuyets.Add(nguoiXetDuyet);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
