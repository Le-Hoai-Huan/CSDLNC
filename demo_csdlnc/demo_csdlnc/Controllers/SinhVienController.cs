using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using demo_csdlnc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Controllers
{
    
    public class SinhVienController : Controller
    {
        private readonly AppDbContext _context;

        public SinhVienController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var sinhViens = _context.SinhViens.Include(sv => sv.Lop).ToList();
         
            return View(sinhViens);
        }

        // Sinh viên có thể xem thông tin cá nhân của mình
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var username = HttpContext.Session.GetString("Username");
            ViewBag.MaLop = _context.Lops.Select(l => new SelectListItem
            {
                Value = l.MaLop.ToString(),
                Text = l.TenLop
            }).ToList();
            if (role == "SinhVien")
            {
                var sinhVien = _context.SinhViens.FirstOrDefault(sv => sv.MaSV == id && sv.Email == username);
                if (sinhVien == null) return Unauthorized();
                return View(sinhVien);
            }

            if (role == "Admin")
            {
                var sinhVien = _context.SinhViens.Find(id);
                return View(sinhVien);
            }

            return Unauthorized();
        }

        // Chỉ Admin được tạo sinh viên
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
            return View();
        }


        [HttpPost]
        public IActionResult Create(SinhVien sinhVien)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            // Kiểm tra xem MaLop có tồn tại trong database không
            var lop = _context.Lops.Find(sinhVien.MaLop);
            if (lop == null)
            {
                ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
                return View(sinhVien); 
            }

            _context.SinhViens.Add(sinhVien);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var sinhVien = _context.SinhViens.Find(id);
            if (sinhVien == null) return NotFound();
            ViewBag.MaLop = _context.Lops.Select(l => new SelectListItem
            {
                Value = l.MaLop.ToString(),
                Text = l.TenLop
            }).ToList();

            return View(sinhVien);
        }

        [HttpPost]
        public IActionResult Edit(SinhVien sinhVien)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.SinhViens.Update(sinhVien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLop = _context.Lops.Select(l => new SelectListItem
            {
                Value = l.MaLop.ToString(),
                Text = l.TenLop
            }).ToList();
            return View(sinhVien);
        }

        // 🔹 Chỉ Admin có thể xóa sinh viên
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var sinhVien = _context.SinhViens.Find(id);
            if (sinhVien == null) return NotFound();

            return View(sinhVien);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var sinhVien = _context.SinhViens.Find(id);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
