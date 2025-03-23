using Microsoft.EntityFrameworkCore;

namespace demo_csdlnc.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Khoa> Khoas { get; set; }
        public DbSet<Lop> Lops { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<NguoiXetDuyet> NguoiXetDuyets { get; set; }
        public DbSet<TieuChi> TieuChis { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<ThamGiaHoatDong> ThamGiaHoatDongs { get; set; }
        public DbSet<KetQua> KetQuas { get; set; }
        public DbSet<TieuChiSinhVien> TieuChiSinhViens { get; set; }

    }
}
