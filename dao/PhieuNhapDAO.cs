using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class PhieuNhapDAO
    {
        // Helper: Lấy ID int từ Mã string (Dùng chung cho các hàm insert/update)
        private int GetIdFromCode(MySqlConnection conn, MySqlTransaction trans, string table, string colMa, string valMa)
        {
            string sql = $"SELECT id FROM {table} WHERE {colMa} = @val LIMIT 1";
            using (var cmd = new MySqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@val", valMa);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public List<PhieuNhap> GetAll()
        {
            List<PhieuNhap> list = new List<PhieuNhap>();
            // Join để lấy mã hiển thị
            string sql = @"SELECT pn.*, nv.ma_nhan_vien, ncc.ma_nha_cung_cap 
                           FROM phieunhap pn
                           LEFT JOIN nhanvien nv ON pn.nhan_vien_id = nv.id
                           LEFT JOIN nhacungcap ncc ON pn.nha_cung_cap_id = ncc.id
                           ORDER BY pn.thoi_gian DESC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhieuNhap
                        {
                            IdPN = reader["ma_phieu_nhap"].ToString(),
                            ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                            IdNV = reader["ma_nhan_vien"] != DBNull.Value ? reader["ma_nhan_vien"].ToString() : "",
                            IdNCC = reader["ma_nha_cung_cap"] != DBNull.Value ? reader["ma_nha_cung_cap"].ToString() : "",
                            TongTien = Convert.ToDouble(reader["tong_tien"])
                        });
                    }
                }
            }
            return list;
        }

        // Transaction: Thêm Phiếu Nhập + Thêm Chi Tiết + TĂNG Tồn Kho
        public bool InsertPhieuNhapWithDetails(PhieuNhap pn, List<ChiTietPhieuNhap> details)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Map Mã -> ID
                        int idNV = GetIdFromCode(conn, trans, "nhanvien", "ma_nhan_vien", pn.IdNV);
                        int idNCC = GetIdFromCode(conn, trans, "nhacungcap", "ma_nha_cung_cap", pn.IdNCC);

                        // 2. Insert Phiếu Nhập
                        string sqlPN = @"INSERT INTO phieunhap (ma_phieu_nhap, thoi_gian, nhan_vien_id, nha_cung_cap_id, tong_tien)
                                         VALUES (@ma, @time, @nv, @ncc, @tong)";
                        
                        long phieuNhapId;
                        using (var cmd = new MySqlCommand(sqlPN, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@ma", pn.IdPN);
                            cmd.Parameters.AddWithValue("@time", pn.ThoiGian);
                            cmd.Parameters.AddWithValue("@nv", idNV == 0 ? (object)DBNull.Value : idNV);
                            cmd.Parameters.AddWithValue("@ncc", idNCC == 0 ? (object)DBNull.Value : idNCC);
                            cmd.Parameters.AddWithValue("@tong", pn.TongTien);
                            cmd.ExecuteNonQuery();
                            phieuNhapId = cmd.LastInsertedId;
                        }

                        // 3. Insert Chi Tiết & Tăng Kho
                        string sqlCT = @"INSERT INTO chitietphieunhap (phieu_nhap_id, thuoc_id, so_luong, don_gia_nhap) 
                                         VALUES (@pn_id, @t_id, @sl, @dg)";
                        
                        // Quan trọng: Phieu Nhap thi phai CONG (+) so luong ton
                        string sqlUpdateKho = @"UPDATE thuoc SET so_luong_ton = so_luong_ton + @sl, gia_nhap = @dg 
                                                WHERE id = @t_id"; 

                        foreach (var item in details)
                        {
                            int thuocId = GetIdFromCode(conn, trans, "thuoc", "ma_thuoc", item.IdThuoc);
                            if (thuocId == 0) throw new Exception($"Không tìm thấy thuốc mã {item.IdThuoc}");

                            // Insert chi tiết
                            using (var cmdCT = new MySqlCommand(sqlCT, conn, trans))
                            {
                                cmdCT.Parameters.AddWithValue("@pn_id", phieuNhapId);
                                cmdCT.Parameters.AddWithValue("@t_id", thuocId);
                                cmdCT.Parameters.AddWithValue("@sl", item.SoLuong);
                                cmdCT.Parameters.AddWithValue("@dg", item.GiaNhap);
                                cmdCT.ExecuteNonQuery();
                            }

                            // Update Kho (Cộng dồn số lượng & Cập nhật giá nhập mới nhất)
                            using (var cmdKho = new MySqlCommand(sqlUpdateKho, conn, trans))
                            {
                                cmdKho.Parameters.AddWithValue("@sl", item.SoLuong);
                                cmdKho.Parameters.AddWithValue("@dg", item.GiaNhap);
                                cmdKho.Parameters.AddWithValue("@t_id", thuocId);
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
        
        public bool Delete(string maPhieuNhap)
        {
             // Logic xóa phiếu nhập (thường ít khi xóa cứng vì ảnh hưởng kho, nên dùng soft delete hoặc transaction trả kho)
             // Ở đây code xóa đơn giản
             string sql = "DELETE FROM phieunhap WHERE ma_phieu_nhap = @ma";
             using(var conn = DBConnection.GetConnection()){
                 conn.Open();
                 using(var cmd = new MySqlCommand(sql, conn)){
                     cmd.Parameters.AddWithValue("@ma", maPhieuNhap);
                     return cmd.ExecuteNonQuery() > 0;
                 }
             }
        }

        public List<PhieuNhap> Search(string idPN, string idNV, string idNCC)
        {
            List<PhieuNhap> list = new List<PhieuNhap>();
            string sql = @"SELECT pn.*, nv.ma_nhan_vien, ncc.ma_nha_cung_cap 
                           FROM phieunhap pn
                           LEFT JOIN nhanvien nv ON pn.nhan_vien_id = nv.id
                           LEFT JOIN nhacungcap ncc ON pn.nha_cung_cap_id = ncc.id
                           WHERE (@idPN = '' OR pn.ma_phieu_nhap LIKE @idPN)
                           AND (@idNV = '' OR nv.ma_nhan_vien LIKE @idNV)
                           AND (@idNCC = '' OR ncc.ma_nha_cung_cap LIKE @idNCC)
                           ORDER BY pn.thoi_gian DESC";

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idPN", "%" + (idPN ?? "") + "%");
                    cmd.Parameters.AddWithValue("@idNV", "%" + (idNV ?? "") + "%");
                    cmd.Parameters.AddWithValue("@idNCC", "%" + (idNCC ?? "") + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PhieuNhap
                            {
                                IdPN = reader["ma_phieu_nhap"].ToString(),
                                ThoiGian = Convert.ToDateTime(reader["thoi_gian"]),
                                IdNV = reader["ma_nhan_vien"] != DBNull.Value ? reader["ma_nhan_vien"].ToString() : "",
                                IdNCC = reader["ma_nha_cung_cap"] != DBNull.Value ? reader["ma_nha_cung_cap"].ToString() : "",
                                TongTien = Convert.ToDouble(reader["tong_tien"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool DeletePhieuNhap(string idPN)
        {
            return Delete(idPN);
        }

        public bool Update(PhieuNhap pn)
        {
            string sql = @"UPDATE phieunhap SET thoi_gian=@time, tong_tien=@tong,
                           nhan_vien_id=(SELECT id FROM nhanvien WHERE ma_nhan_vien=@mnv LIMIT 1),
                           nha_cung_cap_id=(SELECT id FROM nhacungcap WHERE ma_nha_cung_cap=@mncc LIMIT 1)
                           WHERE ma_phieu_nhap=@ma";
            
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@time", pn.ThoiGian);
                    cmd.Parameters.AddWithValue("@tong", pn.TongTien);
                    cmd.Parameters.AddWithValue("@mnv", pn.IdNV);
                    cmd.Parameters.AddWithValue("@mncc", pn.IdNCC);
                    cmd.Parameters.AddWithValue("@ma", pn.IdPN);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<PhieuNhap> GetDeleted()
        {
            return new List<PhieuNhap>();
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