using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("chitiethoadon")]
    public class ChiTietHoaDon
    {
        // Các mã String dùng để hiển thị và liên kết logic
        [NotMapped] public string IdHD { get; set; } = "";
        [NotMapped] public string IdThuoc { get; set; } = "";
        
        [NotMapped] public string TenThuoc { get; set; } = ""; // Hiển thị trên Grid

        [Column("so_luong")]
        public int SoLuong { get; set; }

        [Column("don_gia")]
        public double DonGia { get; set; }

        public double ThanhTien => SoLuong * DonGia;
    }
}