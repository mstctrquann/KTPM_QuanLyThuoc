using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class KhachHangDAO
    {
        public List<KhachHang> GetAll()
        {
            List<KhachHang> list = new List<KhachHang>();
            string sql = "SELECT * FROM khachhang WHERE is_deleted = 0 OR is_deleted IS NULL";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new KhachHang
                        {
                            IdKH = reader["ma_khach_hang"].ToString() ?? "",
                            HoTen = reader["ho_ten"].ToString() ?? "",
                            Sdt = reader["so_dien_thoai"]?.ToString() ?? "",
                            GioiTinh = reader["gioi_tinh"]?.ToString() ?? "",
                            NgayThamGia = Convert.ToDateTime(reader["ngay_tham_gia"]),
                            DiemTichLuy = reader["diem"] != DBNull.Value ? Convert.ToInt32(reader["diem"]) : 0
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(KhachHang kh)
        {
            string sql = @"INSERT INTO khachhang (ma_khach_hang, ho_ten, so_dien_thoai, gioi_tinh, ngay_tham_gia) 
                           VALUES (@ma, @ten, @sdt, @gt, @ntg)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", kh.IdKH);
                    cmd.Parameters.AddWithValue("@ten", kh.HoTen);
                    cmd.Parameters.AddWithValue("@sdt", kh.Sdt);
                    cmd.Parameters.AddWithValue("@gt", kh.GioiTinh);
                    cmd.Parameters.AddWithValue("@ntg", kh.NgayThamGia);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<KhachHang> Search(string hoTen, string sdt)
        {
            List<KhachHang> list = new List<KhachHang>();
            string sql = @"SELECT * FROM khachhang 
                           WHERE (@ten = '' OR ho_ten LIKE @ten)
                           AND (@sdt = '' OR so_dien_thoai LIKE @sdt)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", "%" + (hoTen ?? "") + "%");
                    cmd.Parameters.AddWithValue("@sdt", "%" + (sdt ?? "") + "%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new KhachHang
                            {
                                IdKH = reader["ma_khach_hang"].ToString() ?? "",
                                HoTen = reader["ho_ten"].ToString() ?? "",
                                Sdt = reader["so_dien_thoai"]?.ToString() ?? "",
                                GioiTinh = reader["gioi_tinh"]?.ToString() ?? "",
                                NgayThamGia = Convert.ToDateTime(reader["ngay_tham_gia"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool Update(KhachHang kh)
        {
            string sql = @"UPDATE khachhang SET ho_ten=@ten, so_dien_thoai=@sdt, gioi_tinh=@gt 
                           WHERE ma_khach_hang=@ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", kh.HoTen);
                    cmd.Parameters.AddWithValue("@sdt", kh.Sdt);
                    cmd.Parameters.AddWithValue("@gt", kh.GioiTinh);
                    cmd.Parameters.AddWithValue("@ma", kh.IdKH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string idKH)
        {
            // Soft delete
            string sql = "UPDATE khachhang SET is_deleted = 1 WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idKH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public KhachHang GetById(string idKH)
        {
            string sql = "SELECT * FROM khachhang WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idKH);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new KhachHang
                            {
                                IdKH = reader["ma_khach_hang"].ToString() ?? "",
                                HoTen = reader["ho_ten"].ToString() ?? "",
                                Sdt = reader["so_dien_thoai"]?.ToString() ?? "",
                                GioiTinh = reader["gioi_tinh"]?.ToString() ?? "",
                                NgayThamGia = Convert.ToDateTime(reader["ngay_tham_gia"])
                            };
                        }
                    }
                }
            }
            return null!;
        }

        public bool CongDiem(string idKH, int diem)
        {
            string sql = "UPDATE khachhang SET diem = diem + @diem WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@diem", diem);
                    cmd.Parameters.AddWithValue("@ma", idKH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool TruDiem(string idKH, int diem)
        {
            string sql = "UPDATE khachhang SET diem = diem - @diem WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@diem", diem);
                    cmd.Parameters.AddWithValue("@ma", idKH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<KhachHang> GetDeleted()
        {
            List<KhachHang> list = new List<KhachHang>();
            string sql = "SELECT * FROM khachhang WHERE is_deleted = 1";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new KhachHang
                        {
                            IdKH = reader["ma_khach_hang"].ToString() ?? "",
                            HoTen = reader["ho_ten"].ToString() ?? "",
                            Sdt = reader["so_dien_thoai"]?.ToString() ?? "",
                            GioiTinh = reader["gioi_tinh"]?.ToString() ?? "",
                            NgayThamGia = Convert.ToDateTime(reader["ngay_tham_gia"])
                        });
                    }
                }
            }
            return list;
        }

        public bool Restore(string id)
        {
            string sql = "UPDATE khachhang SET is_deleted = 0 WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteForever(string id)
        {
            string sql = "DELETE FROM khachhang WHERE ma_khach_hang = @ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}