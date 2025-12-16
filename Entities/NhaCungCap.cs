using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("nhacungcap")]
    public class NhaCungCap
    {
        [Key]
        [Column("ma_nha_cung_cap")]
        public string IdNCC { get; set; } = "";

        [Column("ten_nha_cung_cap")]
        public string TenNCC { get; set; } = "";

        [Column("so_dien_thoai")]
        public string Sdt { get; set; } = "";

        [Column("dia_chi")]
        public string DiaChi { get; set; } = "";
    }
}