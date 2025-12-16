using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace QLThuocApp.connectDB
{
    public static class DBConnection
    {
        // Thông tin cấu hình MySQL Local
        private static string Server = "127.0.0.1";
        private static string Port = "3306";
        private static string Database = "quanlythuocdb";
        private static string Uid = "root";
        private static string Pwd = ""; 

        // Chuỗi kết nối MySQL local (không cần SSL)
        private static string connectionString = 
            $"Server={Server};Port={Port};Database={Database};Uid={Uid};Pwd={Pwd};SslMode=None;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                // Không mở kết nối ở đây, để DAO tự quản lý việc Open/Close bằng khối 'using'
                return conn; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấu hình kết nối: " + ex.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}