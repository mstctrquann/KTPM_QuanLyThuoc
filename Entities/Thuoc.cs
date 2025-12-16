using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLThuocApp.Entities
{
    [Table("thuoc")]
    public class Thuoc
    {
        [Key]
        [Column("ma_thuoc")] 
        public string IdThuoc { get; set; } = ""; // Map với cột ma_thuoc

        [Column("ten_thuoc")]
        public string TenThuoc { get; set; } = "";

        [Column("hinh_anh")]
        public byte[]? HinhAnh { get; set; }

        [Column("thanh_phan")]
        public string ThanhPhan { get; set; } = "";

        [Column("don_vi_tinh")]
        public string DonViTinh { get; set; } = "";

        // Lưu ý: Trong DB là danh_muc_id (int), nhưng ở đây ta dùng tên/mã để hiển thị
        // DAO sẽ xử lý việc join bảng để lấy tên
        [Column("ten_danh_muc")] 
        [NotMapped] 
        public string DanhMuc { get; set; } = "";
        
        // Để lưu ID danh mục khi cần update
        [Column("danh_muc_id")]
        public int DanhMucId { get; set; }

        [Column("xuat_xu")]
        public string XuatXu { get; set; } = "";

        [Column("so_luong_ton")]
        public int SoLuongTon { get; set; }

        [Column("gia_nhap")]
        public double GiaNhap { get; set; }

        [Column("don_gia")]
        public double DonGia { get; set; }

        [Column("han_su_dung")]
        public DateTime HanSuDung { get; set; }

        [NotMapped] // Cột này dùng cho logic xóa mềm trên UI nếu cần
        public bool IsDeleted { get; set; }
        
        // Cột ID thực trong DB (nếu cần dùng nội bộ)
        [Column("id")]
        public int Id { get; set; }
    }
}