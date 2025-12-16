using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class ViewHoaDonDialog : Form
    {
        public ViewHoaDonDialog(string idHD)
        {
            Text = $"Chi tiết hóa đơn: {idHD}";
            Size = new System.Drawing.Size(600, 400);
            StartPosition = FormStartPosition.CenterParent;

            var controller = new ChiTietHoaDonController();
            var grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            grid.DataSource = controller.GetChiTietByMaHD(idHD);
            Controls.Add(grid);
        }
    }
}