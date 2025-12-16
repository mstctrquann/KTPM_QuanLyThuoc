using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class MainForm : Form
    {
        private TabControl tabControl;

        public MainForm()
        {
            // Hiển thị vai trò người dùng
            string roleName = GetRoleName(LoginForm.CurrentUser?.IdVT);
            Text = $"Hệ thống Quản lý Nhà thuốc - {LoginForm.CurrentUser?.Username ?? "N/A"} ({roleName})";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.WhiteSmoke;
            
            // Tab Control - PHẢI THÊM TRƯỚC để nằm dưới cùng
            tabControl = new TabControl 
            { 
                Dock = DockStyle.Fill,
                ItemSize = new Size(130, 35),
                SizeMode = TabSizeMode.Fixed,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Padding = new Point(10, 5),
                Appearance = TabAppearance.Normal
            };
            Controls.Add(tabControl);

            // Menu Strip - THÊM SAU để nằm trên cùng
            var menu = new MenuStrip();
            menu.Dock = DockStyle.Top;
            menu.BackColor = Color.FromArgb(41, 128, 185); // Màu xanh dương chuyên nghiệp
            menu.ForeColor = Color.White;
            menu.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            menu.Padding = new Padding(10, 5, 0, 5);
            
            var itemHeThong = new ToolStripMenuItem("⚙ Hệ thống");
            itemHeThong.ForeColor = Color.White;
            itemHeThong.DropDownItems.Add("↪ Đăng xuất", null, (s, e) => {
                this.Hide(); new LoginForm().Show();
            });
            itemHeThong.DropDownItems.Add("✖ Thoát", null, (s, e) => Application.Exit());
            menu.Items.Add(itemHeThong);
            this.MainMenuStrip = menu;
            Controls.Add(menu);

            InitializeTabs();
        }

        private string GetRoleName(string? roleId)
        {
            return roleId switch
            {
                "1" => "Admin",
                "2" => "Manager",
                "3" => "Nhân Viên",
                _ => "Không xác định"
            };
        }

        private void InitializeTabs()
        {
            var roleId = LoginForm.CurrentUser?.IdVT;

            // ==============================================================
            // PHÂN QUYỀN THEO ROLE_ID TỪ DATABASE
            // ==============================================================
            // ID = 1: Admin - Toàn quyền hệ thống
            // ID = 2: Manager - Quản lý vận hành, xem báo cáo
            // ID = 3: Nhân Viên - Chỉ bán hàng, quản lý khách hàng
            // ==============================================================

            if (roleId == "1") // ADMIN - Full access
            {
                AddTab("Bán Hàng", new HoaDonPanel());
                AddTab("Thuốc", new ThuocControl());
                AddTab("Khách Hàng", new KhachHangPanel());
                AddTab("Nhập Hàng", new PhieuNhapPanel());
                AddTab("Nhân Viên", new NhanVienPanel());
                AddTab("Nhà Cung Cấp", new NhaCungCapPanel());
                AddTab("Hợp Đồng", new HopDongPanel());
                AddTab("Phản Hồi", new PhanHoiPanel());
                AddTab("Thùng Rác", new TrashPanel());
            }
            else if (roleId == "2") // MANAGER - Operations + Reports
            {
                AddTab("Bán Hàng", new HoaDonPanel());
                AddTab("Thuốc", new ThuocControl());
                AddTab("Khách Hàng", new KhachHangPanel());
                AddTab("Nhập Hàng", new PhieuNhapPanel());
                AddTab("Nhà Cung Cấp", new NhaCungCapPanel());
                AddTab("Hợp Đồng", new HopDongPanel());
                AddTab("Phản Hồi", new PhanHoiPanel());
            }
            else if (roleId == "3") // NHÂN VIÊN - Sales only
            {
                AddTab("Bán Hàng", new HoaDonPanel());
                AddTab("Thuốc", new ThuocControl());
                AddTab("Khách Hàng", new KhachHangPanel());
            }
            else
            {
                // Trường hợp không xác định vai trò - chỉ cho phép xem thuốc
                AddTab("Thuốc", new ThuocControl());
                MessageBox.Show("Vai trò không hợp lệ. Vui lòng liên hệ quản trị viên.", 
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddTab(string title, UserControl content)
        {
            TabPage page = new TabPage(title);
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            tabControl.TabPages.Add(page);
        }
    }
}