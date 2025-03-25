using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using demo_csdlnc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

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
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (role == "Admin" || role=="NguoiXetDuyet")
            {
                var sinhViens = _context.SinhViens.Include(sv => sv.Lop).ToList();
                return View(sinhViens);
            }
            else if (role == "SinhVien")
            {
                var sinhVien = _context.SinhViens
                    .Include(sv => sv.Lop)
                    .FirstOrDefault(sv => sv.MaAccount == userId.Value); 

                if (sinhVien == null)
                {
                    return NotFound();
                }

                return View(new List<SinhVien> { sinhVien }); 
            }
            else
            {
                return Unauthorized();
            }
        }

        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (role == "Admin" || role == "NguoiXetDuyet")
            {
                var sinhVien = _context.SinhViens
                    .Include(sv => sv.Lop)
                    .FirstOrDefault(sv => sv.MaSV == id);

                if (sinhVien == null)
                {
                    return NotFound();
                }

                return View(sinhVien);
            }
            else if (role == "SinhVien")
            {
                var sinhVien = _context.SinhViens
                    .Include(sv => sv.Lop)
                    .FirstOrDefault(sv => sv.MaSV == id && sv.MaAccount == userId.Value);

                if (sinhVien == null)
                {
                    return Forbid();
                }

                return View(sinhVien);
            }
            else
            {
                return Unauthorized();
            }
        }


        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
            return View();
        }


        [HttpPost]
        public IActionResult Create(SinhVien sinhVien)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
            {
                return Unauthorized();
            }
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
            var userRole = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userRole == "SinhVien")
            {
                var sinhVien = _context.SinhViens.FirstOrDefault(s => s.MaAccount == userId);
                ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
                if (sinhVien == null || sinhVien.MaSV != id)
                {
                    return Unauthorized();
                }
                return View(sinhVien);
            }
            else if (userRole == "Admin" || userRole == "NguoiXetDuyet")
            {
                var sinhVien = _context.SinhViens.Find(id);
                if (sinhVien == null) return NotFound();

                ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
                ViewBag.MaAccount = new SelectList(_context.Accounts, "MaAccount", "Username"); // Thêm danh sách tài khoản

                return View(sinhVien);
            }

            return Unauthorized();
        }


        
        [HttpPost]
        public IActionResult Edit(SinhVien sinhVien)
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userRole == "SinhVien")
            {
                var existingSinhVien = _context.SinhViens.FirstOrDefault(s => s.MaAccount == userId);
                if (existingSinhVien == null || existingSinhVien.MaSV != sinhVien.MaSV)
                {
                    return Unauthorized();
                }
                existingSinhVien.HoTen = sinhVien.HoTen;
                existingSinhVien.NgaySinh = sinhVien.NgaySinh;
                existingSinhVien.GioiTinh = sinhVien.GioiTinh;
                existingSinhVien.Email = sinhVien.Email;
                existingSinhVien.SoDienThoai = sinhVien.SoDienThoai;
                existingSinhVien.MaLop = sinhVien.MaLop;
            }
            else if (userRole == "Admin" || userRole == "NguoiXetDuyet")
            {
                var existingSinhVien = _context.SinhViens.Find(sinhVien.MaSV);
                if (existingSinhVien == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin của sinh viên
                existingSinhVien.HoTen = sinhVien.HoTen;
                existingSinhVien.NgaySinh = sinhVien.NgaySinh;
                existingSinhVien.GioiTinh = sinhVien.GioiTinh;
                existingSinhVien.Email = sinhVien.Email;
                existingSinhVien.SoDienThoai = sinhVien.SoDienThoai;
                existingSinhVien.MaLop = sinhVien.MaLop;
                existingSinhVien.MaAccount = sinhVien.MaAccount; 

                _context.SinhViens.Update(existingSinhVien);
            }
            else
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLop = new SelectList(_context.Lops, "MaLop", "TenLop");
            ViewBag.MaAccount = new SelectList(_context.Accounts, "MaAccount", "Username");

            return View(sinhVien);
        }


        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
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
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "NguoiXetDuyet")
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
