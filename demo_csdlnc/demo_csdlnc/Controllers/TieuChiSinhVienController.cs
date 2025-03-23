using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Controllers
{
    public class TieuChiSinhVienController : Controller
    {
        private readonly AppDbContext _context;
        public TieuChiSinhVienController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userIdStr = HttpContext.Session.GetString("MaAccount");

            IQueryable<TieuChiSinhVien> danhGiaList = _context.TieuChiSinhViens
                .Include(t => t.SinhVien)
                .Include(t => t.TieuChi);

            if (userRole == "SinhVien" && int.TryParse(userIdStr, out int userId))
            {
                danhGiaList = danhGiaList.Where(t => t.MaSV == userId);
            }

            return View(danhGiaList.ToList());
        }
        public IActionResult Details(int id)
        {
            var danhGia = _context.TieuChiSinhViens
                .Include(t => t.SinhVien)
                .Include(t => t.TieuChi)
                .FirstOrDefault(t => t.MaDanhGia == id);

            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen");
            ViewBag.MaTieuChi = new SelectList(_context.TieuChis, "MaTieuChi", "TenTieuChi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TieuChiSinhVien danhGia)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }
            if (ModelState.ContainsKey("SinhVien"))
            {
                ModelState["SinhVien"].Errors.Clear();
                ModelState["SinhVien"].ValidationState = ModelValidationState.Valid;
            }
            if (ModelState.ContainsKey("TieuChi"))
            {
                ModelState["TieuChi"].Errors.Clear();
                ModelState["TieuChi"].ValidationState = ModelValidationState.Valid;
            }

            if (ModelState.IsValid)
            {
                _context.TieuChiSinhViens.Add(danhGia);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", danhGia.MaSV);
            ViewBag.MaTieuChi = new SelectList(_context.TieuChis, "MaTieuChi", "TenTieuChi", danhGia.MaTieuChi);
            return View(danhGia);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            var danhGia = _context.TieuChiSinhViens
                .FirstOrDefault(t => t.MaDanhGia == id);

            if (danhGia == null)
            {
                return NotFound();
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", danhGia.MaSV);
            ViewBag.MaTieuChi = new SelectList(_context.TieuChis, "MaTieuChi", "TenTieuChi", danhGia.MaTieuChi);
            return View(danhGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TieuChiSinhVien danhGia)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            if (id != danhGia.MaDanhGia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhGia);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Không thể cập nhật dữ liệu.");
                }
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", danhGia.MaSV);
            ViewBag.MaTieuChi = new SelectList(_context.TieuChis, "MaTieuChi", "TenTieuChi", danhGia.MaTieuChi);
            return View(danhGia);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var danhGia = _context.TieuChiSinhViens
                .Include(t => t.SinhVien)
                .Include(t => t.TieuChi)
                .FirstOrDefault(t => t.MaDanhGia == id);

            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var danhGia = _context.TieuChiSinhViens.Find(id);
            if (danhGia != null)
            {
                _context.TieuChiSinhViens.Remove(danhGia);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
