using demo_csdlnc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Principal;

namespace demo_csdlnc.Controllers
{
    public class DangKyController : Controller
    {
        private readonly AppDbContext _context;

        public DangKyController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 1. Hiển thị danh sách đăng ký
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (role == "Admin")
            {
                var dangKys = _context.DangKys
                    .Include(dk => dk.SinhVien)  
                    .Include(dk => dk.NguoiXetDuyet) 
                    .ToList();

                return View(dangKys);
            }
            else if (role == "SinhVien" && userId.HasValue)
            {
                // Sinh viên chỉ xem đơn của chính mình
                var sinhVienDangKy = _context.DangKys
                    .Include(dk => dk.SinhVien)
                    .Include(dk => dk.NguoiXetDuyet)
                    .Where(dk => dk.SinhVien.MaAccount == userId.Value)
                    .ToList();

                return View(sinhVienDangKy);
            }
            else
            {
                return Unauthorized(); 
            }
        }

        public IActionResult Details(int id)
        {
            var dangKy = _context.DangKys
                .Include(dk => dk.SinhVien)
                .Include(dk => dk.NguoiXetDuyet)
                .FirstOrDefault(dk => dk.MaDangKy == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userRole == "SinhVien")
            {
                var sinhVien = _context.SinhViens.FirstOrDefault(s => s.MaAccount == userId);
                if (sinhVien == null)
                {
                    return Unauthorized(); 
                }

                ViewBag.MaSV = sinhVien.MaSV; 
            }
            else
            {
                ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen");
            }

            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DangKy dangKy)
        {

            ViewBag.MaSV = new SelectList(_context.SinhViens, "MaSV", "HoTen");
            ViewBag.MaNguoiXetDuyet = new SelectList(_context.NguoiXetDuyets, "MaNguoiXetDuyet", "Account.Username");
           
            dangKy.NgayDangKy = DateTime.Now;
            _context.DangKys.Add(dangKy);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            var dangKy = _context.DangKys
                .Include(d => d.SinhVien)
                .Include(d => d.NguoiXetDuyet)
                .FirstOrDefault(s => s.MaDangKy == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            var sinhViens = _context.SinhViens.ToList();
            var nguoiXetDuyets = _context.NguoiXetDuyets.Include(x => x.Account).ToList();

            ViewBag.MaSV = new SelectList(sinhViens, "MaSV", "HoTen", dangKy.MaSV);
            ViewBag.MaNguoiXetDuyet = new SelectList(nguoiXetDuyets, "MaNguoiXetDuyet", "Account.Username", dangKy.MaNguoiXetDuyet);
            ViewBag.TrangThaiList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Chờ Duyệt", Text = "Chờ Duyệt" },
        new SelectListItem { Value = "Đã Duyệt", Text = "Đã Duyệt" },
        new SelectListItem { Value = "Đã Hủy", Text = "Đã Hủy" }
    };

            return View(dangKy);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DangKy dangKy)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }
            if (id != dangKy.MaDangKy)
            {
                return NotFound();
            }

                var sinhViens = _context.SinhViens.ToList();
                var nguoiXetDuyets = _context.NguoiXetDuyets.Include(x => x.Account).ToList();

                ViewBag.MaSV = new SelectList(sinhViens, "MaSV", "HoTen", dangKy.MaSV);
                ViewBag.MaNguoiXetDuyet = new SelectList(nguoiXetDuyets, "MaNguoiXetDuyet", "Account.Username", dangKy.MaNguoiXetDuyet);
                ViewBag.TrangThaiList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Chờ Duyệt", Text = "Chờ Duyệt" },
                    new SelectListItem { Value = "Đã Duyệt", Text = "Đã Duyệt" },
                    new SelectListItem { Value = "Đã Hủy", Text = "Đã Hủy" }
                };

              
            

            var existingDangKy = _context.DangKys.Find(id);
            if (existingDangKy == null)
            {
                return NotFound();
            }

            existingDangKy.TrangThai = dangKy.TrangThai;
            existingDangKy.MaSV = dangKy.MaSV;
            existingDangKy.MaNguoiXetDuyet = dangKy.MaNguoiXetDuyet;
            existingDangKy.NgayDangKy = DateTime.Now;

            try
            {
                _context.Update(existingDangKy);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Lỗi cập nhật dữ liệu: " + ex.Message);
                return View(dangKy);
            }

            return RedirectToAction("Index");
        }


        // 🔹 5. Xóa đăng ký
        public IActionResult Delete(int id)
        {
            var dangKy = _context.DangKys.Find(id);
            if (dangKy == null)
            {
                return NotFound();
            }
            return View(dangKy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var dangKy = _context.DangKys.Find(id);
            if (dangKy != null)
            {
                _context.DangKys.Remove(dangKy);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
