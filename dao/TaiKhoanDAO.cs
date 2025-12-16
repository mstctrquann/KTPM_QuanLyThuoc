using System;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class TaiKhoanDAO
    {
        // Lấy thông tin tài khoản theo Username
        public TaiKhoan GetByUsername(string username)
        {
            // Join với NhanVien để lấy Mã Nhân Viên (thay vì ID int)
            string sql = @"SELECT tk.*, nv.ma_nhan_vien 
                           FROM taikhoan tk
                           LEFT JOIN nhanvien nv ON tk.nhan_vien_id = nv.id
                           WHERE tk.username = @u";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaiKhoan
                            {
                                // Entity TaiKhoan của bạn có thể cần cập nhật để có IdTK (string) thay vì int
                                // IdTK = reader["id"].ToString(), 
                                Username = reader["username"].ToString() ?? "",
                                Password = reader["password"].ToString() ?? "",
                                IdNV = reader["ma_nhan_vien"] != DBNull.Value ? reader["ma_nhan_vien"].ToString() ?? "" : "",
                                IdVT = reader["role_id"] != DBNull.Value ? reader["role_id"].ToString() ?? "" : ""
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Tạo tài khoản cho nhân viên
        public bool Insert(TaiKhoan tk)
        {
            string sql = @"INSERT INTO taikhoan (username, password, nhan_vien_id, role_id)
                           VALUES (@user, @pass, 
                                   (SELECT id FROM nhanvien WHERE ma_nhan_vien = @mnv LIMIT 1), 
                                   @role)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", tk.Username);
                    cmd.Parameters.AddWithValue("@pass", tk.Password);
                    cmd.Parameters.AddWithValue("@mnv", tk.IdNV);
                    cmd.Parameters.AddWithValue("@role", tk.IdVT);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePassword(string username, string newPass)
        {
            string sql = "UPDATE taikhoan SET password = @p WHERE username = @u";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", newPass);
                    cmd.Parameters.AddWithValue("@u", username);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        
        public bool Delete(string username)
        {
            string sql = "DELETE FROM taikhoan WHERE username = @u";
            using(var conn = DBConnection.GetConnection()){
                conn.Open();
                using(var cmd = new MySqlCommand(sql, conn)){
                    cmd.Parameters.AddWithValue("@u", username);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        
        // Register new employee account
        public bool Register(string username, string password, string maNV, string roleId)
        {
            string sql = @"INSERT INTO taikhoan (username, password, nhan_vien_id, role_id)
                           VALUES (@user, @pass, 
                                   (SELECT id FROM nhanvien WHERE ma_nhan_vien = @mnv LIMIT 1), 
                                   @role)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@mnv", maNV);
                    cmd.Parameters.AddWithValue("@role", roleId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}