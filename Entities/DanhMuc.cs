using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("danhmuc")]
    public class DanhMuc
    {
        [Key]
        [Column("id")]
        public string IdDM { get; set; } = ""; // Convert ID int sang string để đồng bộ UI

        [Column("ten_danh_muc")]
        public string Ten { get; set; } = "";

        [Column("mo_ta")]
        public string MoTa { get; set; } = "";
    }
}