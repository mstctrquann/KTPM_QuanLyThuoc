using System;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class TrashHopDongPanel : UserControl
    {
        private DataGridView dgv;
        private HopDongController controller = new HopDongController();

        public TrashHopDongPanel()
        {
            Dock = DockStyle.Fill;
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            var btnRestore = new Button { Text = "Khôi phục", Location = new System.Drawing.Point(10, 10), Width = 100, BackColor = System.Drawing.Color.LightGreen };
            btnRestore.Click += (s, e) => {
                if (dgv?.CurrentRow == null) return;
                string id = dgv.CurrentRow.Cells["IdHDong"].Value?.ToString() ?? "";
                if(controller.Restore(id)) { MessageBox.Show("Khôi phục thành công!"); LoadData(); }
            };

            var btnDel = new Button { Text = "Xóa vĩnh viễn", Location = new System.Drawing.Point(120, 10), Width = 100, BackColor = System.Drawing.Color.IndianRed, ForeColor = System.Drawing.Color.White };
            btnDel.Click += (s, e) => {
                if (dgv?.CurrentRow == null) return;
                string id = dgv.CurrentRow.Cells["IdHDong"].Value?.ToString() ?? "";
                if(MessageBox.Show("Xóa vĩnh viễn?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(controller.DeleteForever(id)) { MessageBox.Show("Đã xóa."); LoadData(); }
                }
            };

            pnlTop.Controls.Add(btnRestore);
            pnlTop.Controls.Add(btnDel);
            Controls.Add(pnlTop);

            dgv = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = System.Drawing.Color.White };
            Controls.Add(dgv);
            LoadData();
        }
        private void LoadData() => dgv.DataSource = controller.GetDeletedList();
    }
}