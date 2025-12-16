using System;
using System.Windows.Forms;
using QLThuocApp.UI; // Gọi các Form trong namespace UI

namespace QLThuocApp // ĐỒNG BỘ NAMESPACE
{
    internal static class Program
    {
        /// <summary>
        /// Điểm vào chính của ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. Cấu hình giao diện chuẩn Windows
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2. Xử lý lỗi toàn cục (Để app không bị crash đột ngột)
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show("Lỗi hệ thống: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            // 3. Quy trình Khởi động: Login -> Main
            try
            {
                // Mở Form Đăng nhập dưới dạng Dialog (Cửa sổ con)
                LoginForm login = new LoginForm();
                DialogResult result = login.ShowDialog();

                // Nếu đăng nhập thành công (Code trong LoginForm trả về DialogResult.OK)
                if (result == DialogResult.OK)
                {
                    // Chạy Form Chính (MainForm sẽ tự lấy User từ LoginForm.CurrentUser)
                    Application.Run(new MainForm());
                }
                else
                {
                    // Nếu tắt form đăng nhập hoặc ấn Thoát
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }
        }
    }
}