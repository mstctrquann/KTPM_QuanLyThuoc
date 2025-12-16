namespace QLThuocApp.Entities
{
    public class TaiKhoan
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string IdNV { get; set; } = ""; // Mã nhân viên
        public string IdVT { get; set; } = ""; // Mã vai trò (Admin/Staff)
        public string IdTK { get; set; } = "";
    }
}