using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("khachhang")]
    public class KhachHang
    {
        [Key]
        [Column("ma_khach_hang")]
        public string IdKH { get; set; } = "";

        [Column("ho_ten")]
        public string HoTen { get; set; } = "";

        [Column("so_dien_thoai")]
        public string? Sdt { get; set; }

        [Column("gioi_tinh")]
        public string? GioiTinh { get; set; }

        [Column("ngay_tham_gia")]
        public DateTime NgayThamGia { get; set; }
        
        // Điểm tích lũy từ database
        [Column("diem")] 
        public int DiemTichLuy { get; set; }
    }
}