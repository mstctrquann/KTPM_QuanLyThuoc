using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class ThuocDAO
    {
        // Map dữ liệu an toàn, xử lý Null
        private Thuoc MapData(MySqlDataReader reader)
        {
            return new Thuoc
            {
                Id = Convert.ToInt32(reader["id"]),
                IdThuoc = reader["ma_thuoc"].ToString() ?? "",
                TenThuoc = reader["ten_thuoc"].ToString() ?? "",
                ThanhPhan = reader["thanh_phan"]?.ToString() ?? "",
                DonViTinh = reader["don_vi_tinh"]?.ToString() ?? "",
                DanhMuc = reader["ten_danh_muc"]?.ToString() ?? "", // Lấy từ bảng Join
                XuatXu = reader["xuat_xu"]?.ToString() ?? "",
                SoLuongTon = Convert.ToInt32(reader["so_luong_ton"]),
                GiaNhap = Convert.ToDouble(reader["gia_nhap"]),
                DonGia = Convert.ToDouble(reader["don_gia"]),
                HanSuDung = Convert.ToDateTime(reader["han_su_dung"]),
                TrangThai = reader["trang_thai"] != DBNull.Value ? Convert.ToInt32(reader["trang_thai"]) : 1
            };
        }

        public List<Thuoc> GetAll()
        {
            var list = new List<Thuoc>();
            // Join với danhmuc để lấy tên danh mục hiển thị lên Grid
            // Chỉ lấy thuốc đang kinh doanh (trang_thai = 1)
            string sql = @"SELECT t.*, d.ten_danh_muc 
                           FROM thuoc t 
                           LEFT JOIN danhmuc d ON t.danh_muc_id = d.id 
                           WHERE t.trang_thai = 1
                           ORDER BY t.ten_thuoc ASC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(MapData(reader));
                }
            }
            return list;
        }
        
        // Lấy tất cả thuốc bao gồm cả đã ngừng kinh doanh (dùng cho báo cáo, lịch sử)
        public List<Thuoc> GetAllIncludeDeleted()
        {
            var list = new List<Thuoc>();
            string sql = @"SELECT t.*, d.ten_danh_muc 
                           FROM thuoc t 
                           LEFT JOIN danhmuc d ON t.danh_muc_id = d.id 
                           ORDER BY t.trang_thai DESC, t.ten_thuoc ASC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(MapData(reader));
                }
            }
            return list;
        }

        // Insert: Cần lookup ID danh mục (nếu UI gửi tên). Ở đây giả sử UI gửi ID hoặc ta insert null danh mục nếu ko tìm thấy
        // Để đơn giản và chạy ngay, ta giả định danh mục được chọn từ ComboBox có ValueMember là ID (nếu có bảng) hoặc ta lưu string nếu sửa DB.
        // Theo DB hiện tại: thuoc có danh_muc_id.
        public bool Insert(Thuoc t)
        {
            string sql = @"INSERT INTO thuoc (ma_thuoc, ten_thuoc, thanh_phan, don_vi_tinh, xuat_xu, so_luong_ton, gia_nhap, don_gia, han_su_dung, danh_muc_id, trang_thai) 
                           VALUES (@ma, @ten, @tp, @dvt, @xx, @sl, @gn, @dg, @hsd, 
                                   (SELECT id FROM danhmuc WHERE ten_danh_muc = @dm LIMIT 1), 1)"; // Lookup ID danh mục từ tên, trang_thai = 1 (đang kinh doanh)

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", t.IdThuoc);
                    cmd.Parameters.AddWithValue("@ten", t.TenThuoc);
                    cmd.Parameters.AddWithValue("@tp", t.ThanhPhan);
                    cmd.Parameters.AddWithValue("@dvt", t.DonViTinh);
                    cmd.Parameters.AddWithValue("@xx", t.XuatXu);
                    cmd.Parameters.AddWithValue("@sl", t.SoLuongTon);
                    cmd.Parameters.AddWithValue("@gn", t.GiaNhap);
                    cmd.Parameters.AddWithValue("@dg", t.DonGia);
                    cmd.Parameters.AddWithValue("@hsd", t.HanSuDung);
                    cmd.Parameters.AddWithValue("@dm", t.DanhMuc); // Tên danh mục từ UI
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Thuoc t)
        {
            string sql = @"UPDATE thuoc SET ten_thuoc=@ten, thanh_phan=@tp, don_vi_tinh=@dvt, xuat_xu=@xx, 
                           so_luong_ton=@sl, gia_nhap=@gn, don_gia=@dg, han_su_dung=@hsd,
                           danh_muc_id=(SELECT id FROM danhmuc WHERE ten_danh_muc = @dm LIMIT 1)
                           WHERE ma_thuoc=@ma";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", t.TenThuoc);
                    cmd.Parameters.AddWithValue("@tp", t.ThanhPhan);
                    cmd.Parameters.AddWithValue("@dvt", t.DonViTinh);
                    cmd.Parameters.AddWithValue("@xx", t.XuatXu);
                    cmd.Parameters.AddWithValue("@sl", t.SoLuongTon);
                    cmd.Parameters.AddWithValue("@gn", t.GiaNhap);
                    cmd.Parameters.AddWithValue("@dg", t.DonGia);
                    cmd.Parameters.AddWithValue("@hsd", t.HanSuDung);
                    cmd.Parameters.AddWithValue("@dm", t.DanhMuc);
                    cmd.Parameters.AddWithValue("@ma", t.IdThuoc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Soft delete: Đánh dấu thuốc đã ngừng kinh doanh (trang_thai = 0)
        public bool Delete(string maThuoc)
        {
            string sql = "UPDATE thuoc SET trang_thai = 0 WHERE ma_thuoc = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maThuoc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        
        // Hard delete: Xóa vĩnh viễn khỏi database (dùng trong thùng rác)
        public bool DeletePermanently(string maThuoc)
        {
            string sql = "DELETE FROM thuoc WHERE ma_thuoc = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maThuoc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Thuoc> Search(string keyword)
        {
            var list = new List<Thuoc>();
            string sql = @"SELECT t.*, d.ten_danh_muc 
                           FROM thuoc t 
                           LEFT JOIN danhmuc d ON t.danh_muc_id = d.id 
                           WHERE (t.ma_thuoc LIKE @key OR t.ten_thuoc LIKE @key)
                           AND t.trang_thai = 1";
            
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(MapData(reader));
                    }
                }
            }
            return list;
        }

        // Lấy danh sách thuốc đã ngừng kinh doanh (thùng rác)
        public List<Thuoc> GetDeleted()
        {
            var list = new List<Thuoc>();
            string sql = @"SELECT t.*, d.ten_danh_muc 
                           FROM thuoc t 
                           LEFT JOIN danhmuc d ON t.danh_muc_id = d.id 
                           WHERE t.trang_thai = 0
                           ORDER BY t.ten_thuoc ASC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(MapData(reader));
                }
            }
            return list;
        }

        // Khôi phục thuốc (đưa về trạng thái đang kinh doanh)
        public bool Restore(string maThuoc)
        {
            string sql = "UPDATE thuoc SET trang_thai = 1 WHERE ma_thuoc = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maThuoc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Xóa vĩnh viễn (gọi DeletePermanently)
        public bool DeleteForever(string maThuoc)
        {
            return DeletePermanently(maThuoc);
        }
    }
}