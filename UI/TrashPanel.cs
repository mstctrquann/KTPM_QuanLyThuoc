using System.Drawing;
using System.Windows.Forms;

namespace QLThuocApp.UI
{
    public class TrashPanel : UserControl
    {
        public TrashPanel()
        {
            Dock = DockStyle.Fill;
            Padding = new Padding(10);
            BackColor = Color.White;
            var tab = new TabControl { Dock = DockStyle.Fill, Padding = new Point(10, 10) };

            // Thêm đầy đủ tất cả các tab thùng rác
            AddTab(tab, "Thuốc", new TrashThuocPanel());
            AddTab(tab, "Nhân Viên", new TrashNhanVienPanel());
            AddTab(tab, "Khách Hàng", new TrashKhachHangPanel());
            AddTab(tab, "Nhà Cung Cấp", new TrashNhaCungCapPanel()); // Bổ sung
            AddTab(tab, "Hóa Đơn", new TrashHoaDonPanel());
            AddTab(tab, "Phiếu Nhập", new TrashPhieuNhapPanel());
            AddTab(tab, "Hợp Đồng", new TrashHopDongPanel());         // Bổ sung
            AddTab(tab, "Phản Hồi", new TrashPhanHoiPanel());         // Bổ sung

            Controls.Add(tab);
        }

        private void AddTab(TabControl tab, string title, UserControl content)
        {
            var page = new TabPage(title);
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            tab.TabPages.Add(page);
        }
    }
}