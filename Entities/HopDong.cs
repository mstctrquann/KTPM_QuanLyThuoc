using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("hopdong")]
    public class HopDong
    {
        [Key]
        [Column("ma_hop_dong")]
        public string IdHD { get; set; } = "";

        [Column("ngay_bat_dau")]
        public DateTime NgayBatDau { get; set; }

        [Column("ngay_ket_thuc")]
        public DateTime? NgayKetThuc { get; set; }

        [Column("noi_dung")]
        public string NoiDung { get; set; } = "";

        [NotMapped] public string IdNV { get; set; } = "";
        [NotMapped] public string IdNCC { get; set; } = "";

        [Column("trang_thai")]
        public string TrangThai { get; set; } = "";
    }
}