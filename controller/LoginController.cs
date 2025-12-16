using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class LoginController
    {
        private TaiKhoanDAO dao = new TaiKhoanDAO();

        public TaiKhoan? Login(string user, string pass)
        {
            // Lấy thông tin tài khoản từ database
            var taiKhoan = dao.GetByUsername(user);
            
            // Kiểm tra tài khoản tồn tại và mật khẩu đúng
            if (taiKhoan != null && taiKhoan.Password == pass)
            {
                return taiKhoan;
            }
            
            return null;
        }
    }
}