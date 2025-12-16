using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class NhaCungCapPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnAdd, btnRefresh;
        private NhaCungCapController controller = new NhaCungCapController();
        
        public NhaCungCapPanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(10);
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // Panel top với buttons
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke, Padding = new Padding(5) };
            
            btnAdd = new Button 
            { 
                Text = "➕ Thêm Nhà Cung Cấp", 
                Location = new Point(10, 12), 
                Size = new Size(150, 36), 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            pnlTop.Controls.Add(btnAdd);

            btnRefresh = new Button 
            { 
                Text = "🔄", 
                Location = new Point(170, 12), 
                Size = new Size(60, 36), 
                BackColor = Color.FromArgb(149, 165, 166), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            pnlTop.Controls.Add(btnRefresh);

            // DataGridView - THÊM TRƯỚC
            dgv = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false;

            Controls.Add(dgv);

            // Panel khoảng cách
            var pnlSpacer = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacer);

            // Panel ngăn cách với border
            var pnlSeparator = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 1, 
                BackColor = Color.FromArgb(189, 195, 199)
            };
            Controls.Add(pnlSeparator);

            Controls.Add(pnlTop);
            LoadData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm nhà cung cấp sẽ được phát triển trong phiên bản tiếp theo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // TODO: Tạo AddNhaCungCapDialog sau
        }

        private void LoadData() => dgv.DataSource = controller.GetAllNhaCungCap();
    }
}