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

        // Trạng thái: 1 = Đang kinh doanh, 0 = Đã ngừng kinh doanh
        [Column("trang_thai")]
        public int TrangThai { get; set; } = 1;
        
        // Cột ID thực trong DB
        [Column("id")]
        public int Id { get; set; }
        
        // Thuộc tính hiển thị trạng thái
        [NotMapped]
        public string TenTrangThai => TrangThai == 1 ? "Đang kinh doanh" : "Đã ngừng kinh doanh";
    }
}