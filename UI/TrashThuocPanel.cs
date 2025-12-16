using System;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class TrashThuocPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnRestore, btnDeleteForever;
        
        // Gọi qua Controller
        private ThuocController controller = new ThuocController();

        public TrashThuocPanel()
        {
            Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = System.Drawing.Color.White, Padding = new System.Windows.Forms.Padding(5) };
            
            btnRestore = new Button 
            { 
                Text = "🔄 Khôi phục", 
                Location = new System.Drawing.Point(10, 12), 
                Size = new System.Drawing.Size(130, 36),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.Click += (s, e) => ActionRestore();
            
            btnDeleteForever = new Button 
            { 
                Text = "🗑 Xóa vĩnh viễn", 
                Location = new System.Drawing.Point(150, 12), 
                Size = new System.Drawing.Size(150, 36),
                BackColor = System.Drawing.Color.FromArgb(231, 76, 60),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDeleteForever.FlatAppearance.BorderSize = 0;
            btnDeleteForever.Click += (s, e) => ActionDeleteForever();

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new System.Drawing.Point(310, 12),
                Size = new System.Drawing.Size(120, 36),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            pnlTop.Controls.Add(btnRestore);
            pnlTop.Controls.Add(btnDeleteForever);
            pnlTop.Controls.Add(btnRefresh);

            dgv = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, 
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(240, 240, 240) }
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false;

            Controls.Add(dgv);

            var pnlSpacer = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = System.Drawing.Color.White 
            };
            Controls.Add(pnlSpacer);

            var pnlSeparator = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 1, 
                BackColor = System.Drawing.Color.FromArgb(189, 195, 199)
            };
            Controls.Add(pnlSeparator);

            Controls.Add(pnlTop);
        }

        private void LoadData()
        {
            // Gọi Controller thay vì DAO
            dgv.DataSource = controller.GetDeletedList();
        }

        private void ActionRestore()
        {
            if (dgv.CurrentRow == null) return;
            string id = dgv.CurrentRow.Cells["IdThuoc"].Value.ToString();
            
            if (controller.Restore(id))
            {
                MessageBox.Show("Đã khôi phục thuốc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Khôi phục thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionDeleteForever()
        {
            if (dgv.CurrentRow == null) return;
            string id = dgv.CurrentRow.Cells["IdThuoc"].Value.ToString();
            
            if(MessageBox.Show($"Xóa vĩnh viễn thuốc {id}? Hành động này không thể hoàn tác!", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (controller.DeleteForever(id))
                {
                    MessageBox.Show("Đã xóa vĩnh viễn.", "Thông báo");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi");
                }
            }
        }
    }
}