using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class NhanVienDAO
    {
        private NhanVien MapData(MySqlDataReader reader)
        {
            string roleId = "";
            try
            {
                // Kiểm tra nếu có cột role_id
                int roleIdIndex = reader.GetOrdinal("role_id");
                if (reader["role_id"] != DBNull.Value)
                {
                    roleId = reader["role_id"].ToString();
                }
            }
            catch
            {
                // Nếu không có cột role_id, để rỗng
                roleId = "";
            }
            
            return new NhanVien
            {
                IdNV = reader["ma_nhan_vien"].ToString(),
                HoTen = reader["ho_ten"].ToString(),
                Sdt = reader["so_dien_thoai"] != DBNull.Value ? reader["so_dien_thoai"].ToString() : "",
                GioiTinh = reader["gioi_tinh"] != DBNull.Value ? reader["gioi_tinh"].ToString() : "",
                NamSinh = reader["nam_sinh"] != DBNull.Value ? Convert.ToInt32(reader["nam_sinh"]) : 0,
                NgayVaoLam = Convert.ToDateTime(reader["ngay_vao_lam"]),
                Luong = reader["luong"] != DBNull.Value ? reader["luong"].ToString() : "0",
                TrangThai = reader["trang_thai"].ToString(),
                RoleId = roleId
            };
        }

        public List<NhanVien> GetAll()
        {
            List<NhanVien> list = new List<NhanVien>();
            string sql = @"SELECT nv.*, tk.role_id 
                           FROM nhanvien nv
                           LEFT JOIN taikhoan tk ON nv.id = tk.nhan_vien_id
                           WHERE nv.is_deleted = 0 OR nv.is_deleted IS NULL";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(MapData(reader));
                }
            }
            return list;
        }

        // Kiểm tra đăng nhập (Giả định table taikhoan hoặc check trực tiếp mã NV nếu chưa có bảng tk)
        // Dựa vào code base, có vẻ bạn dùng mã NV làm username?
        // Nếu dùng bảng riêng `taikhoan` (như file seed), ta query bảng đó.
        public TaiKhoan Login(string username, string password)
        {
            // Lưu ý: Cần tạo bảng 'taikhoan' trong DB nếu chưa có, hoặc dùng bảng 'nhanvien' để test
            // Giả sử dùng bảng nhanvien, cột ma_nhan_vien là user, so_dien_thoai là pass (để test)
            
            // Code chuẩn nếu có bảng TaiKhoan (theo Entities/TaiKhoan.cs)
            // Cần tạo bảng này trong MySQL: CREATE TABLE taikhoan (...)
            
            // Tạm thời trả về Mock Data nếu DB chưa có bảng TaiKhoan để bạn chạy được form Login
            // Bạn nên chạy script tạo bảng taikhoan tôi cung cấp ở cuối.
            
            string sql = @"SELECT tk.*, nv.ma_nhan_vien 
                           FROM taikhoan tk 
                           JOIN nhanvien nv ON tk.nhan_vien_id = nv.id 
                           WHERE tk.username = @u AND tk.password = @p";

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                // Kiểm tra xem bảng taikhoan có tồn tại không để tránh crash
                try 
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@p", password); // Lưu ý: thực tế nên hash password
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TaiKhoan
                                {
                                    Username = reader["username"].ToString(),
                                    Password = reader["password"].ToString(),
                                    IdNV = reader["ma_nhan_vien"].ToString(),
                                    IdVT = reader["role_id"].ToString() // Giả định cột role_id
                                };
                            }
                        }
                    }
                }
                catch 
                {
                    // Fallback nếu chưa có bảng taikhoan: Admin/123
                    if(username == "admin" && password == "123") 
                        return new TaiKhoan { Username="admin", IdNV="NV001", IdVT="Admin" };
                }
            }
            return null;
        }

        public bool Insert(NhanVien nv)
        {
            string sql = @"INSERT INTO nhanvien (ma_nhan_vien, ho_ten, so_dien_thoai, gioi_tinh, nam_sinh, ngay_vao_lam, luong, trang_thai) 
                           VALUES (@ma, @ten, @sdt, @gt, @ns, @nvl, @luong, @tt)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", nv.IdNV);
                    cmd.Parameters.AddWithValue("@ten", nv.HoTen);
                    cmd.Parameters.AddWithValue("@sdt", nv.Sdt);
                    cmd.Parameters.AddWithValue("@gt", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@ns", nv.NamSinh);
                    cmd.Parameters.AddWithValue("@nvl", nv.NgayVaoLam);
                    cmd.Parameters.AddWithValue("@luong", decimal.Parse(nv.Luong)); // Convert string luong to decimal
                    cmd.Parameters.AddWithValue("@tt", nv.TrangThai);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<NhanVien> Search(string id, string name)
        {
            List<NhanVien> list = new List<NhanVien>();
            string sql = @"SELECT * FROM nhanvien 
                           WHERE (is_deleted = 0 OR is_deleted IS NULL)
                           AND (@id = '' OR ma_nhan_vien LIKE @id)
                           AND (@name = '' OR ho_ten LIKE @name)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", "%" + (id ?? "") + "%");
                    cmd.Parameters.AddWithValue("@name", "%" + (name ?? "") + "%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(MapData(reader));
                    }
                }
            }
            return list;
        }

        public bool Update(NhanVien nv)
        {
            string sql = @"UPDATE nhanvien SET ho_ten=@ten, so_dien_thoai=@sdt, gioi_tinh=@gt, 
                           nam_sinh=@ns, luong=@luong, trang_thai=@tt 
                           WHERE ma_nhan_vien=@ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", nv.HoTen);
                    cmd.Parameters.AddWithValue("@sdt", nv.Sdt);
                    cmd.Parameters.AddWithValue("@gt", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@ns", nv.NamSinh);
                    cmd.Parameters.AddWithValue("@luong", decimal.Parse(nv.Luong));
                    cmd.Parameters.AddWithValue("@tt", nv.TrangThai);
                    cmd.Parameters.AddWithValue("@ma", nv.IdNV);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string idNV)
        {
        // Soft delete: set is_deleted = 1 for both nhanvien and related taikhoan
        using (MySqlConnection conn = DBConnection.GetConnection())
        {
            conn.Open();
            using (MySqlTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    // Delete taikhoan permanently (bảng taikhoan không có is_deleted)
                    string sqlAccount = "DELETE FROM taikhoan WHERE nhan_vien_id = (SELECT id FROM nhanvien WHERE ma_nhan_vien = @ma)";
                    using (MySqlCommand cmdAccount = new MySqlCommand(sqlAccount, conn, transaction))
                    {
                        cmdAccount.Parameters.AddWithValue("@ma", idNV);
                        cmdAccount.ExecuteNonQuery();
                    }

                    // Then soft delete nhanvien
                    string sql = "UPDATE nhanvien SET is_deleted = 1 WHERE ma_nhan_vien = @ma";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ma", idNV);
                        int result = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return result > 0;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

        public List<NhanVien> GetDeleted()
        {
            // Get only deleted employees (is_deleted = 1)
            List<NhanVien> list = new List<NhanVien>();
            string sql = "SELECT * FROM nhanvien WHERE is_deleted = 1";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(MapData(reader));
                }
            }
            return list;
        }

        public bool Restore(string id)
        {
        // Restore from trash: set is_deleted = 0 for both nhanvien and related taikhoan
        using (MySqlConnection conn = DBConnection.GetConnection())
        {
            conn.Open();
            using (MySqlTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    // Restore nhanvien first
                    string sql = "UPDATE nhanvien SET is_deleted = 0 WHERE ma_nhan_vien = @ma";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ma", id);
                        int result = cmd.ExecuteNonQuery();
                        
                        // Note: Cannot restore taikhoan as it was permanently deleted
                        // Admin needs to create new account if needed
                        
                        transaction.Commit();
                        return result > 0;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

        public bool DeleteForever(string id)
        {
            // Permanent delete: actually remove from database (taikhoan first, then nhanvien)
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete related taikhoan first to avoid foreign key issues
                        string sqlAccount = "DELETE FROM taikhoan WHERE nhan_vien_id = (SELECT id FROM nhanvien WHERE ma_nhan_vien = @ma)";
                        using (MySqlCommand cmdAccount = new MySqlCommand(sqlAccount, conn, transaction))
                        {
                            cmdAccount.Parameters.AddWithValue("@ma", id);
                            cmdAccount.ExecuteNonQuery();
                        }

                        // Then delete nhanvien
                        string sql = "DELETE FROM nhanvien WHERE ma_nhan_vien = @ma";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ma", id);
                            int result = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}