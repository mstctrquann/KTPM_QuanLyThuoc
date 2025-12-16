using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("hoadon")]
    public class HoaDon
    {
        [Key]
        [Column("ma_hoa_don")]
        public string IdHD { get; set; } = "";

        [Column("thoi_gian")]
        public DateTime ThoiGian { get; set; }

        // Lưu mã String để hiển thị (NV001), DAO sẽ map sang ID int
        [NotMapped]
        public string IdNV { get; set; } = "";

        [NotMapped]
        public string IdKH { get; set; } = "";

        [Column("tong_tien")]
        public double TongTien { get; set; }

        [Column("phuong_thuc_thanh_toan")]
        public string PhuongThucThanhToan { get; set; } = "";

        [Column("trang_thai_don_hang")]
        public string TrangThaiDonHang { get; set; } = "";
    }
}