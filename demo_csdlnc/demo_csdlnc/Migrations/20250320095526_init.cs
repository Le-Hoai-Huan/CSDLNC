using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo_csdlnc.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    MaAccount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.MaAccount);
                });

            migrationBuilder.CreateTable(
                name: "Khoas",
                columns: table => new
                {
                    MaKhoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoas", x => x.MaKhoa);
                });

            migrationBuilder.CreateTable(
                name: "TieuChis",
                columns: table => new
                {
                    MaTieuChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTieuChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieuChis", x => x.MaTieuChi);
                });

            migrationBuilder.CreateTable(
                name: "NguoiXetDuyets",
                columns: table => new
                {
                    MaNguoiXetDuyet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaAccount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiXetDuyets", x => x.MaNguoiXetDuyet);
                    table.ForeignKey(
                        name: "FK_NguoiXetDuyets_Accounts_MaAccount",
                        column: x => x.MaAccount,
                        principalTable: "Accounts",
                        principalColumn: "MaAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    MaLop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLop = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhoa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_Lops_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSV);
                    table.ForeignKey(
                        name: "FK_SinhViens_Lops_MaLop",
                        column: x => x.MaLop,
                        principalTable: "Lops",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    MaDangKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<int>(type: "int", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNguoiXetDuyet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => x.MaDangKy);
                    table.ForeignKey(
                        name: "FK_DangKys_NguoiXetDuyets_MaNguoiXetDuyet",
                        column: x => x.MaNguoiXetDuyet,
                        principalTable: "NguoiXetDuyets",
                        principalColumn: "MaNguoiXetDuyet");
                    table.ForeignKey(
                        name: "FK_DangKys_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KetQuas",
                columns: table => new
                {
                    MaKetQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<int>(type: "int", nullable: false),
                    NamHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XepLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNguoiXetDuyet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuas", x => x.MaKetQua);
                    table.ForeignKey(
                        name: "FK_KetQuas_NguoiXetDuyets_MaNguoiXetDuyet",
                        column: x => x.MaNguoiXetDuyet,
                        principalTable: "NguoiXetDuyets",
                        principalColumn: "MaNguoiXetDuyet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KetQuas_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThamGiaHoatDongs",
                columns: table => new
                {
                    MaThamGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<int>(type: "int", nullable: false),
                    TenHoatDong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayThamGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiemSo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThamGiaHoatDongs", x => x.MaThamGia);
                    table.ForeignKey(
                        name: "FK_ThamGiaHoatDongs_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TieuChiSinhViens",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSV = table.Column<int>(type: "int", nullable: false),
                    MaTieuChi = table.Column<int>(type: "int", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false),
                    NhanXet = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieuChiSinhViens", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_TieuChiSinhViens_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TieuChiSinhViens_TieuChis_MaTieuChi",
                        column: x => x.MaTieuChi,
                        principalTable: "TieuChis",
                        principalColumn: "MaTieuChi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_MaNguoiXetDuyet",
                table: "DangKys",
                column: "MaNguoiXetDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_MaSV",
                table: "DangKys",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuas_MaNguoiXetDuyet",
                table: "KetQuas",
                column: "MaNguoiXetDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuas_MaSV",
                table: "KetQuas",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_Lops_MaKhoa",
                table: "Lops",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiXetDuyets_MaAccount",
                table: "NguoiXetDuyets",
                column: "MaAccount");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_ThamGiaHoatDongs_MaSV",
                table: "ThamGiaHoatDongs",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_TieuChiSinhViens_MaSV",
                table: "TieuChiSinhViens",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_TieuChiSinhViens_MaTieuChi",
                table: "TieuChiSinhViens",
                column: "MaTieuChi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "KetQuas");

            migrationBuilder.DropTable(
                name: "ThamGiaHoatDongs");

            migrationBuilder.DropTable(
                name: "TieuChiSinhViens");

            migrationBuilder.DropTable(
                name: "NguoiXetDuyets");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "TieuChis");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Lops");

            migrationBuilder.DropTable(
                name: "Khoas");
        }
    }
}
