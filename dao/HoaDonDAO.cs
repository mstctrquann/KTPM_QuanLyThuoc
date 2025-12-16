using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class HoaDonDAO
    {
        public List<HoaDon> GetAll()
        {
            var list = new List<HoaDon>();
            string sql = @"SELECT hd.*, nv.ma_nhan_vien, kh.ma_khach_hang 
                           FROM hoadon hd
                           LEFT JOIN nhanvien nv ON hd.nhan_vien_id = nv.id
                           LEFT JOIN khachhang kh ON hd.khach_hang_id = kh.id
                           ORDER BY hd.thoi_gian DESC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new HoaDon
                        {
                            IdHD = reader["ma_hoa_don"].ToString() ?? "",
                            ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                            IdNV = reader["ma_nhan_vien"]?.ToString() ?? "",
                            IdKH = reader["ma_khach_hang"]?.ToString() ?? "",
                            TongTien = Convert.ToDouble(reader["tong_tien"]),
                            PhuongThucThanhToan = reader["phuong_thuc_thanh_toan"]?.ToString() ?? "",
                            TrangThaiDonHang = reader["trang_thai_don_hang"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }

        // Helper lấy ID int từ Mã string
        private int GetIdFromCode(MySqlConnection conn, MySqlTransaction trans, string table, string colMa, string valMa)
        {
            string sql = $"SELECT id FROM {table} WHERE {colMa} = @val LIMIT 1";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@val", valMa);
                object res = cmd.ExecuteScalar();
                return res != null ? Convert.ToInt32(res) : 0;
            }
        }

        public bool InsertWithDetails(HoaDon hd, List<ChiTietHoaDon> details)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Lấy ID int
                        int idNV = GetIdFromCode(conn, trans, "nhanvien", "ma_nhan_vien", hd.IdNV);
                        int idKH = GetIdFromCode(conn, trans, "khachhang", "ma_khach_hang", hd.IdKH);

                        // 2. Insert Hóa đơn
                        string sqlHD = @"INSERT INTO hoadon (ma_hoa_don, thoi_gian, nhan_vien_id, khach_hang_id, tong_tien, phuong_thuc_thanh_toan, trang_thai_don_hang)
                                         VALUES (@ma, @time, @nv, @kh, @tong, @pttt, @tt)";
                        using (var cmd = new MySqlCommand(sqlHD, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@ma", hd.IdHD);
                            cmd.Parameters.AddWithValue("@time", hd.ThoiGian);
                            cmd.Parameters.AddWithValue("@nv", idNV == 0 ? (object)DBNull.Value : idNV);
                            cmd.Parameters.AddWithValue("@kh", idKH == 0 ? (object)DBNull.Value : idKH);
                            cmd.Parameters.AddWithValue("@tong", hd.TongTien);
                            cmd.Parameters.AddWithValue("@pttt", hd.PhuongThucThanhToan);
                            cmd.Parameters.AddWithValue("@tt", hd.TrangThaiDonHang);
                            cmd.ExecuteNonQuery();
                        }

                        // Lấy ID hóa đơn vừa tạo
                        long lastId = 0;
                        using (var cmdId = new MySqlCommand("SELECT LAST_INSERT_ID()", conn, trans))
                        {
                            lastId = Convert.ToInt64(cmdId.ExecuteScalar());
                        }

                        // 3. Insert Chi tiết & Trừ kho
                        string sqlCT = "INSERT INTO chitiethoadon (hoa_don_id, thuoc_id, so_luong, don_gia) VALUES (@hd, @t, @sl, @dg)";
                        string sqlKho = "UPDATE thuoc SET so_luong_ton = so_luong_ton - @sl WHERE id = @t";

                        foreach (var item in details)
                        {
                            int idThuoc = GetIdFromCode(conn, trans, "thuoc", "ma_thuoc", item.IdThuoc);
                            
                            // Insert Chi tiết
                            using (var cmdCT = new MySqlCommand(sqlCT, conn, trans))
                            {
                                cmdCT.Parameters.AddWithValue("@hd", lastId);
                                cmdCT.Parameters.AddWithValue("@t", idThuoc);
                                cmdCT.Parameters.AddWithValue("@sl", item.SoLuong);
                                cmdCT.Parameters.AddWithValue("@dg", item.DonGia);
                                cmdCT.ExecuteNonQuery();
                            }

                            // Trừ kho
                            using (var cmdKho = new MySqlCommand(sqlKho, conn, trans))
                            {
                                cmdKho.Parameters.AddWithValue("@sl", item.SoLuong);
                                cmdKho.Parameters.AddWithValue("@t", idThuoc);
                                cmdKho.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        
        // Hàm xóa hóa đơn
        public bool Delete(string idHD)
        {
            string sql = "DELETE FROM hoadon WHERE ma_hoa_don = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idHD);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Exists(string idHD)
        {
            string sql = "SELECT COUNT(*) FROM hoadon WHERE ma_hoa_don = @ma";
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idHD);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public string GetKhachHangIdByHoaDonId(string idHD)
        {
            string sql = @"SELECT kh.ma_khach_hang 
                           FROM hoadon hd
                           JOIN khachhang kh ON hd.khach_hang_id = kh.id
                           WHERE hd.ma_hoa_don = @ma";
            
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", idHD);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
        }

        public List<HoaDon> GetByKhachHang(string idKH)
        {
            var list = new List<HoaDon>();
            string sql = @"SELECT hd.*, nv.ma_nhan_vien, kh.ma_khach_hang 
                           FROM hoadon hd
                           LEFT JOIN nhanvien nv ON hd.nhan_vien_id = nv.id
                           LEFT JOIN khachhang kh ON hd.khach_hang_id = kh.id
                           WHERE kh.ma_khach_hang = @idKH
                           ORDER BY hd.thoi_gian DESC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idKH", idKH);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HoaDon
                            {
                                IdHD = reader["ma_hoa_don"].ToString() ?? "",
                                ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                                IdNV = reader["ma_nhan_vien"]?.ToString() ?? "",
                                IdKH = reader["ma_khach_hang"]?.ToString() ?? "",
                                TongTien = Convert.ToDouble(reader["tong_tien"]),
                                PhuongThucThanhToan = reader["phuong_thuc_thanh_toan"]?.ToString() ?? "",
                                TrangThaiDonHang = reader["trang_thai_don_hang"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<HoaDon> GetDeleted()
        {
            return new List<HoaDon>();
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