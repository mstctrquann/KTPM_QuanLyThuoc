using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class ViewPhieuNhapDialog : Form
    {
        public ViewPhieuNhapDialog(string idPN)
        {
            Text = $"Chi tiết Phiếu Nhập: {idPN}";
            Size = new System.Drawing.Size(700, 450);
            StartPosition = FormStartPosition.CenterParent;

            var dgv = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Controls.Add(dgv);

            var ctrl = new ChiTietPhieuNhapController();
            dgv.DataSource = ctrl.GetByMaPhieuNhap(idPN);
        }
    }
}