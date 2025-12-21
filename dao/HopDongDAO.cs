using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class HopDongDAO
    {
        private HopDong MapData(MySqlDataReader reader)
        {
            return new HopDong
            {
                IdHD = reader["ma_hop_dong"].ToString(),
                NgayBatDau = Convert.ToDateTime(reader["ngay_bat_dau"]),
                NgayKetThuc = reader["ngay_ket_thuc"] != DBNull.Value ? Convert.ToDateTime(reader["ngay_ket_thuc"]) : (DateTime?)null,
                NoiDung = reader["noi_dung"]?.ToString(),
                IdNV = reader["ma_nhan_vien"] != DBNull.Value ? reader["ma_nhan_vien"].ToString() : "",
                IdNCC = reader["ma_nha_cung_cap"] != DBNull.Value ? reader["ma_nha_cung_cap"].ToString() : "",
                TrangThai = reader["trang_thai"].ToString()
            };
        }

        public List<HopDong> GetAll()
        {
            List<HopDong> list = new List<HopDong>();
            string sql = @"SELECT hd.*, nv.ma_nhan_vien, ncc.ma_nha_cung_cap 
                           FROM hopdong hd
                           LEFT JOIN nhanvien nv ON hd.nhan_vien_id = nv.id
                           LEFT JOIN nhacungcap ncc ON hd.nha_cung_cap_id = ncc.id";

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

        public bool Insert(HopDong hd)
        {
            string sql = @"INSERT INTO hopdong (ma_hop_dong, ngay_bat_dau, ngay_ket_thuc, noi_dung, nhan_vien_id, nha_cung_cap_id, trang_thai)
                           VALUES (@ma, @bd, @kt, @nd, 
                                   (SELECT id FROM nhanvien WHERE ma_nhan_vien = @mnv LIMIT 1), 
                                   (SELECT id FROM nhacungcap WHERE ma_nha_cung_cap = @mncc LIMIT 1), 
                                   @tt)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", hd.IdHD);
                    cmd.Parameters.AddWithValue("@bd", hd.NgayBatDau);
                    cmd.Parameters.AddWithValue("@kt", hd.NgayKetThuc);
                    cmd.Parameters.AddWithValue("@nd", hd.NoiDung ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@mnv", hd.IdNV);
                    cmd.Parameters.AddWithValue("@mncc", hd.IdNCC);
                    cmd.Parameters.AddWithValue("@tt", hd.TrangThai);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(HopDong hd)
        {
            // Update dùng Subquery để tìm ID từ mã
            string sql = @"UPDATE hopdong SET 
                           ngay_bat_dau=@bd, ngay_ket_thuc=@kt, noi_dung=@nd, trang_thai=@tt,
                           nhan_vien_id=(SELECT id FROM nhanvien WHERE ma_nhan_vien = @mnv LIMIT 1),
                           nha_cung_cap_id=(SELECT id FROM nhacungcap WHERE ma_nha_cung_cap = @mncc LIMIT 1)
                           WHERE ma_hop_dong=@ma";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@bd", hd.NgayBatDau);
                    cmd.Parameters.AddWithValue("@kt", hd.NgayKetThuc);
                    cmd.Parameters.AddWithValue("@nd", hd.NoiDung);
                    cmd.Parameters.AddWithValue("@tt", hd.TrangThai);
                    cmd.Parameters.AddWithValue("@mnv", hd.IdNV);
                    cmd.Parameters.AddWithValue("@mncc", hd.IdNCC);
                    cmd.Parameters.AddWithValue("@ma", hd.IdHD);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string maHD)
        {
            string sql = "DELETE FROM hopdong WHERE ma_hop_dong = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maHD);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<HopDong> GetAllHopDong()
        {
            return GetAll();
        }

        public HopDong? GetHopDongById(string idHD)
        {
            string sql = @"SELECT hd.*, nv.ma_nhan_vien, ncc.ma_nha_cung_cap 
                           FROM hopdong hd
                           LEFT JOIN nhanvien nv ON hd.nhan_vien_id = nv.id
                           LEFT JOIN nhacungcap ncc ON hd.nha_cung_cap_id = ncc.id
                           WHERE hd.ma_hop_dong = @ma";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idHD);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapData(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<HopDong> SearchHopDong(string id, string idNV, string idNCC)
        {
            List<HopDong> list = new List<HopDong>();
            string sql = @"SELECT hd.*, nv.ma_nhan_vien, ncc.ma_nha_cung_cap 
                           FROM hopdong hd
                           LEFT JOIN nhanvien nv ON hd.nhan_vien_id = nv.id
                           LEFT JOIN nhacungcap ncc ON hd.nha_cung_cap_id = ncc.id
                           WHERE (@id = '' OR hd.ma_hop_dong LIKE @id)
                           AND (@nv = '' OR nv.ma_nhan_vien LIKE @nv)
                           AND (@ncc = '' OR ncc.ma_nha_cung_cap LIKE @ncc)";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", "%" + (id ?? "") + "%");
                    cmd.Parameters.AddWithValue("@nv", "%" + (idNV ?? "") + "%");
                    cmd.Parameters.AddWithValue("@ncc", "%" + (idNCC ?? "") + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(MapData(reader));
                    }
                }
            }
            return list;
        }

        public bool InsertHopDong(HopDong hd)
        {
            return Insert(hd);
        }

        public bool UpdateHopDong(HopDong hd)
        {
            return Update(hd);
        }

        public bool DeleteHopDong(string id)
        {
            return Delete(id);
        }

        public List<HopDong> GetDeleted()
        {
            // Placeholder - requires soft delete implementation
            return new List<HopDong>();
        }

        public bool Restore(string id)
        {
            // Placeholder - requires soft delete implementation
            return false;
        }

        public bool DeleteForever(string id)
        {
            return Delete(id);
        }
    }
}