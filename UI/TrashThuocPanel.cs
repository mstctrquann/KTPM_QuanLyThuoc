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
            if (dgv.CurrentRow == null) 
            {
                MessageBox.Show("Vui lòng chọn thuốc cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string id = dgv.CurrentRow.Cells["IdThuoc"].Value.ToString();
            string tenThuoc = dgv.CurrentRow.Cells["TenThuoc"].Value.ToString();
            
            var confirmMsg = $"📦 KHÔI PHỤC THUỐC\n\n" +
                           $"Mã: {id}\n" +
                           $"Tên: {tenThuoc}\n\n" +
                           $"Thuốc sẽ được đưa trở lại tab Thuốc và\n" +
                           $"có thể sử dụng cho đơn hàng mới.\n\n" +
                           $"Bạn có chắc chắn muốn khôi phục?";
            
            if (MessageBox.Show(confirmMsg, "Xác nhận khôi phục", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (controller.Restore(id, out string msg))
                {
                    MessageBox.Show($"✓ {msg}\n\nThuốc '{tenThuoc}' đã được khôi phục!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"Khôi phục thất bại:\n{msg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ActionDeleteForever()
        {
            if (dgv.CurrentRow == null) 
            {
                MessageBox.Show("Vui lòng chọn thuốc cần xóa vĩnh viễn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string id = dgv.CurrentRow.Cells["IdThuoc"].Value.ToString();
            string tenThuoc = dgv.CurrentRow.Cells["TenThuoc"].Value.ToString();
            
            var confirmMsg = $"⚠️ CẢNH BÁO: XÓA VĨnh VIỄN!\n\n" +
                           $"Mã: {id}\n" +
                           $"Tên: {tenThuoc}\n\n" +
                           $"❌ HÀNH ĐỘNG NÀY KHÔNG THỂ HOÀN TÁC!\n\n" +
                           $"Thuốc sẽ bị xóa hoàn toàn khỏi hệ thống.\n" +
                           $"Nếu thuốc đã được sử dụng trong hóa đơn/phiếu nhập,\n" +
                           $"việc xóa sẽ thất bại để bảo toàn dữ liệu.\n\n" +
                           $"Bạn có CHẮC CHẮN muốn xóa vĩnh viễn?";
            
            if(MessageBox.Show(confirmMsg, "⚠️ Cảnh báo nghiêm trọng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (controller.DeleteForever(id, out string msg))
                {
                    MessageBox.Show($"✓ {msg}\n\nThuốc '{tenThuoc}' đã bị xóa vĩnh viễn!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"❌ Xóa vĩnh viễn thất bại!\n\n{msg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}