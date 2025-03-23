using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Controllers
{
    public class TieuChiController : Controller
    {
        private readonly AppDbContext _context;

        public TieuChiController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 1. Hiển thị danh sách tiêu chí
        public IActionResult Index()
        {
          
            var tieuChis = _context.TieuChis.ToList();
            return View(tieuChis);
        }
        public IActionResult Details(int id)
        {
            var tieuChi = _context.TieuChis.Find(id);
            return View(tieuChi);
        }
        // 🔹 2. Trang tạo tiêu chí mới
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TieuChi tieuChi)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return View(tieuChi);
            }

            _context.TieuChis.Add(tieuChi);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
     
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var tieuChi = _context.TieuChis.FirstOrDefault(i => i.MaTieuChi == id);
            if (tieuChi == null)
            {
                return NotFound();
            }
            return View(tieuChi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TieuChi tieuChi)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            if (id != tieuChi.MaTieuChi)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(tieuChi);
            }

            _context.Update(tieuChi);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 🔹 4. Xóa tiêu chí
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var tieuChi = _context.TieuChis.FirstOrDefault(i => i.MaTieuChi == id);
            if (tieuChi == null)
            {
                return NotFound();
            }

            return View(tieuChi);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            var tieuChi = _context.TieuChis.FirstOrDefault(i => i.MaTieuChi == id);
            if (tieuChi != null)
            {
                _context.TieuChis.Remove(tieuChi);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
