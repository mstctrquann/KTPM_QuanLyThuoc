using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class ChiTietPhieuNhapDAO
    {
        public List<ChiTietPhieuNhap> GetByMaPhieuNhap(string maPN)
        {
            List<ChiTietPhieuNhap> list = new List<ChiTietPhieuNhap>();
            // Join 3 bảng: chitiet -> phieunhap (lọc ma_pn) -> thuoc (lấy ten_thuoc, ma_thuoc)
            string sql = @"SELECT ctpn.*, t.ma_thuoc, t.ten_thuoc 
                           FROM chitietphieunhap ctpn
                           JOIN phieunhap pn ON ctpn.phieu_nhap_id = pn.id
                           JOIN thuoc t ON ctpn.thuoc_id = t.id
                           WHERE pn.ma_phieu_nhap = @ma";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maPN);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietPhieuNhap
                            {
                                IdPN = maPN,
                                IdThuoc = reader["ma_thuoc"].ToString() ?? "",
                                TenThuoc = reader["ten_thuoc"].ToString() ?? "",
                                SoLuong = Convert.ToInt32(reader["so_luong"]),
                                GiaNhap = (double)Convert.ToDecimal(reader["don_gia_nhap"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}