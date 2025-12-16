using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class ChiTietHoaDonDAO
    {
        public List<ChiTietHoaDon> GetByMaHD(string maHD)
        {
            var list = new List<ChiTietHoaDon>();
            string sql = @"SELECT ct.*, t.ma_thuoc, t.ten_thuoc 
                           FROM chitiethoadon ct
                           JOIN hoadon h ON ct.hoa_don_id = h.id
                           JOIN thuoc t ON ct.thuoc_id = t.id
                           WHERE h.ma_hoa_don = @ma";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maHD);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietHoaDon
                            {
                                IdHD = maHD,
                                IdThuoc = reader["ma_thuoc"].ToString() ?? "",
                                TenThuoc = reader["ten_thuoc"].ToString() ?? "", // Map vào thuộc tính [NotMapped]
                                SoLuong = Convert.ToInt32(reader["so_luong"]),
                                DonGia = Convert.ToDouble(reader["don_gia"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}