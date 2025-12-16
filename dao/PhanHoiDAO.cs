using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class PhanHoiDAO
    {
        public List<PhanHoi> GetAll()
        {
            List<PhanHoi> list = new List<PhanHoi>();
            string sql = @"SELECT ph.*, kh.ma_khach_hang, hd.ma_hoa_don, t.ma_thuoc 
                           FROM phanhoi ph
                           LEFT JOIN khachhang kh ON ph.khach_hang_id = kh.id
                           LEFT JOIN hoadon hd ON ph.hoa_don_id = hd.id
                           LEFT JOIN thuoc t ON ph.thuoc_id = t.id";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhanHoi
                        {
                            IdPH = reader["ma_phan_hoi"].ToString(),
                            IdKH = reader["ma_khach_hang"] != DBNull.Value ? reader["ma_khach_hang"].ToString() : "",
                            IdHD = reader["ma_hoa_don"] != DBNull.Value ? reader["ma_hoa_don"].ToString() : "",
                            // IdThuoc nếu có trong entity PhanHoi
                            NoiDung = reader["noi_dung"].ToString(),
                            ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                            DanhGia = Convert.ToInt32(reader["danh_gia"])
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(PhanHoi ph)
        {
            // Lookup ID cho KH và Hóa đơn
            string sql = @"INSERT INTO phanhoi (ma_phan_hoi, khach_hang_id, hoa_don_id, noi_dung, thoi_gian, danh_gia)
                           VALUES (@ma, 
                                   (SELECT id FROM khachhang WHERE ma_khach_hang = @mkh LIMIT 1),
                                   (SELECT id FROM hoadon WHERE ma_hoa_don = @mhd LIMIT 1),
                                   @nd, @time, @dg)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", ph.IdPH);
                    cmd.Parameters.AddWithValue("@mkh", ph.IdKH);
                    cmd.Parameters.AddWithValue("@mhd", ph.IdHD);
                    cmd.Parameters.AddWithValue("@nd", ph.NoiDung);
                    cmd.Parameters.AddWithValue("@time", ph.ThoiGian);
                    cmd.Parameters.AddWithValue("@dg", ph.DanhGia);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(PhanHoi ph)
        {
            string sql = @"UPDATE phanhoi SET noi_dung=@nd, danh_gia=@dg,
                           khach_hang_id=(SELECT id FROM khachhang WHERE ma_khach_hang=@mkh LIMIT 1) 
                           WHERE ma_phan_hoi=@ma";
            
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nd", ph.NoiDung);
                    cmd.Parameters.AddWithValue("@dg", ph.DanhGia);
                    cmd.Parameters.AddWithValue("@mkh", ph.IdKH);
                    cmd.Parameters.AddWithValue("@ma", ph.IdPH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        
        public bool Delete(string maPH)
        {
            string sql = "DELETE FROM phanhoi WHERE ma_phan_hoi = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maPH);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<PhanHoi> Search(string idPH, string idKH)
        {
            List<PhanHoi> list = new List<PhanHoi>();
            string sql = @"SELECT ph.*, kh.ma_khach_hang, hd.ma_hoa_don, t.ma_thuoc 
                           FROM phanhoi ph
                           LEFT JOIN khachhang kh ON ph.khach_hang_id = kh.id
                           LEFT JOIN hoadon hd ON ph.hoa_don_id = hd.id
                           LEFT JOIN thuoc t ON ph.thuoc_id = t.id
                           WHERE (@idPH = '' OR ph.ma_phan_hoi LIKE @idPH)
                           AND (@idKH = '' OR kh.ma_khach_hang LIKE @idKH)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idPH", "%" + (idPH ?? "") + "%");
                    cmd.Parameters.AddWithValue("@idKH", "%" + (idKH ?? "") + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PhanHoi
                            {
                                IdPH = reader["ma_phan_hoi"].ToString(),
                                IdKH = reader["ma_khach_hang"] != DBNull.Value ? reader["ma_khach_hang"].ToString() : "",
                                IdHD = reader["ma_hoa_don"] != DBNull.Value ? reader["ma_hoa_don"].ToString() : "",
                                NoiDung = reader["noi_dung"].ToString(),
                                ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                                DanhGia = Convert.ToInt32(reader["danh_gia"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<PhanHoi> GetDeleted()
        {
            return new List<PhanHoi>();
        }

        public bool Restore(string id)
        {
            return false;
        }

        public bool DeleteForever(string id)
        {
            return Delete(id);
        }
    }
}