using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var nguoiXetDuyet = _context.NguoiXetDuyets
           .Include(xd => xd.Account) // Đảm bảo lấy thông tin tài khoản liên kết
           .FirstOrDefault(xd => xd.MaNguoiXetDuyet == id);

            if (nguoiXetDuyet == null)
            {
                return NotFound();
            }

            ViewBag.MaAccount = nguoiXetDuyet.Account != null ? nguoiXetDuyet.Account.Username : "N/A";

            return View(nguoiXetDuyet);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var nguoiXetDuyetAccounts = _context.Accounts
                .Where(a => a.Role == "NguoiXetDuyet")
                .Select(a => new { a.MaAccount, a.Username })
                .ToList();
            ViewBag.MaAccount = new SelectList(nguoiXetDuyetAccounts, "MaAccount", "Username");
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

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var nguoiXetDuyet = _context.NguoiXetDuyets.FirstOrDefault(xd => xd.MaNguoiXetDuyet == id);
            if (nguoiXetDuyet == null)
            {
                return NotFound();
            }
            var nguoiXetDuyetAccounts = _context.Accounts
                .Where(a => a.Role == "NguoiXetDuyet")
                .Select(a => new { a.MaAccount, a.Username })
                .ToList();
            ViewBag.MaAccount = new SelectList(nguoiXetDuyetAccounts, "MaAccount", "Username");
            return View(nguoiXetDuyet);
        }

        [HttpPost]
        public IActionResult Edit(NguoiXetDuyet nguoiXetDuyet)
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
            _context.NguoiXetDuyets.Update(nguoiXetDuyet);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var nguoiXetDuyet = _context.NguoiXetDuyets.FirstOrDefault(xd => xd.MaNguoiXetDuyet == id);
            if (nguoiXetDuyet == null)
            {
                return NotFound();
            }
            _context.NguoiXetDuyets.Remove(nguoiXetDuyet);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
      

    }
}