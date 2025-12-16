using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("phieunhap")]
    public class PhieuNhap
    {
        [Key]
        [Column("ma_phieu_nhap")]
        public string IdPN { get; set; } = "";

        [Column("thoi_gian")]
        public DateTime ThoiGian { get; set; }

        [NotMapped] public string IdNV { get; set; } = "";
        [NotMapped] public string IdNCC { get; set; } = "";

        [Column("tong_tien")]
        public double TongTien { get; set; }
    }
}