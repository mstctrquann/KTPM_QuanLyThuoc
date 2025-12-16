using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("chitietphieunhap")]
    public class ChiTietPhieuNhap
    {
        [NotMapped] public string IdPN { get; set; } = "";
        [NotMapped] public string IdThuoc { get; set; } = "";
        
        [NotMapped] public string TenThuoc { get; set; } = "";

        [Column("so_luong")]
        public int SoLuong { get; set; }

        [Column("don_gia_nhap")]
        public double GiaNhap { get; set; } // Đổi tên property cho khớp UI

        public double ThanhTien => SoLuong * GiaNhap;
    }
}