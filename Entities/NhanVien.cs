using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("nhanvien")]
    public class NhanVien
    {
        [Key]
        [Column("ma_nhan_vien")]
        public string IdNV { get; set; } = "";

        [Column("ho_ten")]
        public string HoTen { get; set; } = "";

        [Column("so_dien_thoai")]
        public string Sdt { get; set; } = "";

        [Column("gioi_tinh")]
        public string GioiTinh { get; set; } = "";

        [Column("nam_sinh")]
        public int NamSinh { get; set; }

        [Column("ngay_vao_lam")]
        public DateTime NgayVaoLam { get; set; }

        [Column("luong")]
        public string Luong { get; set; } = ""; // Để string theo yêu cầu UI, convert sau

        [Column("trang_thai")]
        public string TrangThai { get; set; } = "";

        // Các trường từ bảng TaiKhoan (Join)
        [NotMapped]
        public string Username { get; set; } = "";
        
        [NotMapped]
        public string Password { get; set; } = "";
        
        [NotMapped]
        public string RoleId { get; set; } = "";
        
        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}