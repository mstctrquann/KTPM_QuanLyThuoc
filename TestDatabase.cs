using System;
using MySql.Data.MySqlClient;
using QLThuocApp.connectDB;

namespace QLThuocApp
{
    public class TestDatabase
    {
        public static void TestTaiKhoanColumns()
        {
            Console.WriteLine("=== KIỂM TRA CẤU TRÚC BẢNG TAIKHOAN ===\n");
            
            try
            {
                using (var conn = DBConnection.GetConnection())
                {
                    conn.Open();
                    
                    // Test 1: Kiểm tra cấu trúc bảng
                    Console.WriteLine("1. Danh sách columns trong bảng taikhoan:");
                    string sqlColumns = "DESCRIBE taikhoan";
                    using (var cmd = new MySqlCommand(sqlColumns, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string field = reader["Field"].ToString() ?? "";
                            string type = reader["Type"].ToString() ?? "";
                            string nullType = reader["Null"].ToString() ?? "";
                            string key = reader["Key"].ToString() ?? "";
                            Console.WriteLine($"   - {field} ({type}) Null={nullType} Key={key}");
                        }
                    }
                    
                    Console.WriteLine("\n2. Kiểm tra tài khoản admin:");
                    string sqlAdmin = "SELECT * FROM taikhoan WHERE username = 'admin' LIMIT 1";
                    using (var cmd = new MySqlCommand(sqlAdmin, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("   Tài khoản tìm thấy:");
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string colName = reader.GetName(i);
                                object value = reader[i];
                                Console.WriteLine($"   - {colName} = {(value == DBNull.Value ? "NULL" : value.ToString())}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("   KHÔNG TÌM THẤY tài khoản admin!");
                        }
                    }
                }
                
                Console.WriteLine("\n✓ Kiểm tra hoàn tất!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ LỖI: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            Console.WriteLine("\n=== KẾT THÚC KIỂM TRA ===");
        }
    }
}
