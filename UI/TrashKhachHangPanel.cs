using System;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class TrashKhachHangPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnRestore, btnDeleteForever;
        private KhachHangController controller = new KhachHangController();

        public TrashKhachHangPanel()
        {
            Dock = DockStyle.Fill;
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            btnRestore = new Button { Text = "Khôi phục", Location = new System.Drawing.Point(10, 10), Width = 100, BackColor = System.Drawing.Color.LightGreen };
            btnRestore.Click += (s, e) => {
                if (dgv?.CurrentRow == null) return;
                string id = dgv.CurrentRow.Cells["IdKH"].Value?.ToString() ?? "";
                if(controller.Restore(id)) { MessageBox.Show("Đã khôi phục."); LoadData(); }
            };
            
            btnDeleteForever = new Button { Text = "Xóa vĩnh viễn", Location = new System.Drawing.Point(120, 10), Width = 100, BackColor = System.Drawing.Color.IndianRed, ForeColor = System.Drawing.Color.White };
            btnDeleteForever.Click += (s, e) => {
                if (dgv?.CurrentRow == null) return;
                string id = dgv.CurrentRow.Cells["IdKH"].Value?.ToString() ?? "";
                if (MessageBox.Show("Xóa vĩnh viễn khách hàng?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(controller.DeleteForever(id)) { MessageBox.Show("Đã xóa."); LoadData(); }
                }
            };

            pnlTop.Controls.Add(btnRestore);
            pnlTop.Controls.Add(btnDeleteForever);
            Controls.Add(pnlTop);

            dgv = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = System.Drawing.Color.White };
            Controls.Add(dgv);
            LoadData();
        }

        private void LoadData() => dgv.DataSource = controller.GetDeletedList();
    }
}