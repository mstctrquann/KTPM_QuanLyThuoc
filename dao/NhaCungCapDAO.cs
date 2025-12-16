using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.Entities;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class NhaCungCapDAO
    {
        public List<NhaCungCap> GetAll()
        {
            List<NhaCungCap> list = new List<NhaCungCap>();
            string sql = "SELECT * FROM nhacungcap";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NhaCungCap
                        {
                            IdNCC = reader["ma_nha_cung_cap"].ToString(),
                            TenNCC = reader["ten_nha_cung_cap"].ToString(),
                            Sdt = reader["so_dien_thoai"]?.ToString(),
                            DiaChi = reader["dia_chi"]?.ToString()
                        });
                    }
                }
            }
            return list;
        }

        public bool Insert(NhaCungCap ncc)
        {
            string sql = @"INSERT INTO nhacungcap (ma_nha_cung_cap, ten_nha_cung_cap, so_dien_thoai, dia_chi) 
                           VALUES (@ma, @ten, @sdt, @dc)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", ncc.IdNCC);
                    cmd.Parameters.AddWithValue("@ten", ncc.TenNCC);
                    cmd.Parameters.AddWithValue("@sdt", ncc.Sdt);
                    cmd.Parameters.AddWithValue("@dc", ncc.DiaChi);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<NhaCungCap> Search(string id, string ten)
        {
            List<NhaCungCap> list = new List<NhaCungCap>();
            string sql = @"SELECT * FROM nhacungcap 
                           WHERE (@id = '' OR ma_nha_cung_cap LIKE @id)
                           AND (@ten = '' OR ten_nha_cung_cap LIKE @ten)";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", "%" + (id ?? "") + "%");
                    cmd.Parameters.AddWithValue("@ten", "%" + (ten ?? "") + "%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new NhaCungCap
                            {
                                IdNCC = reader["ma_nha_cung_cap"].ToString(),
                                TenNCC = reader["ten_nha_cung_cap"].ToString(),
                                Sdt = reader["so_dien_thoai"]?.ToString(),
                                DiaChi = reader["dia_chi"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool Update(NhaCungCap ncc)
        {
            string sql = @"UPDATE nhacungcap SET ten_nha_cung_cap=@ten, so_dien_thoai=@sdt, dia_chi=@dc 
                           WHERE ma_nha_cung_cap=@ma";
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ncc.TenNCC);
                    cmd.Parameters.AddWithValue("@sdt", ncc.Sdt);
                    cmd.Parameters.AddWithValue("@dc", ncc.DiaChi);
                    cmd.Parameters.AddWithValue("@ma", ncc.IdNCC);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string id)
        {
            string sql = "DELETE FROM nhacungcap WHERE ma_nha_cung_cap = @ma";
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

        public List<NhaCungCap> GetDeleted()
        {
            return new List<NhaCungCap>();
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