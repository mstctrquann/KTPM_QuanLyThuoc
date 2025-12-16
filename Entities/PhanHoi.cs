using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("phanhoi")]
    public class PhanHoi
    {
        [Key]
        [Column("ma_phan_hoi")]
        public string IdPH { get; set; } = "";

        [NotMapped] public string IdKH { get; set; } = "";
        [NotMapped] public string IdHD { get; set; } = "";
        [NotMapped] public string? TenKH { get; set; }
        [NotMapped] public string? Email { get; set; }
        [NotMapped] public string? Sdt { get; set; }

        [Column("noi_dung")]
        public string NoiDung { get; set; } = "";

        [Column("thoi_gian")]
        public DateTime ThoiGian { get; set; }
        
        [NotMapped] public DateTime NgayTao => ThoiGian;

        [Column("danh_gia")]
        public int DanhGia { get; set; }
        
        [NotMapped] public bool IsDeleted { get; set; }
    }
}