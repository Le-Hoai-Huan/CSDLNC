using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Controllers
{
    public class ThamGiaHoatDongController : Controller
    {
        private readonly AppDbContext _context;

        public ThamGiaHoatDongController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userIdStr = HttpContext.Session.GetString("MaAccount");

            IQueryable<ThamGiaHoatDong> thamGiaList = _context.ThamGiaHoatDongs
                .Include(t => t.SinhVien);

            if (userRole == "SinhVien" && int.TryParse(userIdStr, out int userId))
            {
                thamGiaList = thamGiaList.Where(t => t.MaSV == userId);
            }
            return View(thamGiaList.ToList());
        }
        public IActionResult Details(int id)
        {
            var thamGia = _context.ThamGiaHoatDongs
                .Include(t => t.SinhVien)
                .FirstOrDefault(t => t.MaThamGia == id);

            if (thamGia == null)
            {
                return NotFound();
            }

            return View(thamGia);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ThamGiaHoatDong thamGia)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            // Bỏ qua lỗi liên quan đến SinhVien trong ModelState
            if (ModelState.ContainsKey("SinhVien"))
            {
                ModelState["SinhVien"].Errors.Clear();
                ModelState["SinhVien"].ValidationState = ModelValidationState.Valid;
            }

            if (ModelState.IsValid)
            {
                if (userRole == "SinhVien")
                {
                    string maAccount = HttpContext.Session.GetString("MaAccount");
                    if (string.IsNullOrEmpty(maAccount) || !int.TryParse(maAccount, out int userId))
                    {
                        ModelState.AddModelError("", "Không thể xác định mã sinh viên từ session.");
                        ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", thamGia.MaSV);
                        return View(thamGia);
                    }
                    thamGia.MaSV = userId;
                }

                _context.ThamGiaHoatDongs.Add(thamGia);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", thamGia.MaSV);
            return View(thamGia);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            var thamGia = _context.ThamGiaHoatDongs
                .FirstOrDefault(t => t.MaThamGia == id);

            if (thamGia == null)
            {
                return NotFound();
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", thamGia.MaSV);
            return View(thamGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ThamGiaHoatDong thamGia)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            if (id != thamGia.MaThamGia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thamGia);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Không thể cập nhật dữ liệu.");
                }
            }

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen", thamGia.MaSV);
            return View(thamGia);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var thamGia = _context.ThamGiaHoatDongs
                .Include(t => t.SinhVien)
                .FirstOrDefault(t => t.MaThamGia == id);

            if (thamGia == null)
            {
                return NotFound();
            }

            return View(thamGia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var thamGia = _context.ThamGiaHoatDongs.Find(id);
            if (thamGia != null)
            {
                _context.ThamGiaHoatDongs.Remove(thamGia);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
