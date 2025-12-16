using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class KhachHangPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnAdd, btnDelete, btnRefresh, btnViewOrders;
        private KhachHangController controller = new KhachHangController();
        
        public KhachHangPanel()
        {
            Dock = DockStyle.Fill;
            AutoScroll = true;
            BackColor = Color.White;
            Padding = new Padding(10);
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // Top panel with buttons
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke, Padding = new Padding(5) };

            btnAdd = new Button
            {
                Text = "➕ Thêm KH",
                Location = new Point(10, 12),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            pnlTop.Controls.Add(btnAdd);

            btnDelete = new Button
            {
                Text = "🗑 Xóa KH",
                Location = new Point(140, 12),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            pnlTop.Controls.Add(btnDelete);

            btnViewOrders = new Button
            {
                Text = "📋 Xem đơn hàng",
                Location = new Point(270, 12),
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewOrders.FlatAppearance.BorderSize = 0;
            btnViewOrders.Click += BtnViewOrders_Click;
            pnlTop.Controls.Add(btnViewOrders);

            btnRefresh = new Button
            {
                Text = "🔄",
                Location = new Point(420, 12),
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
            
            // DataGridView
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
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

            // Add double-click event to view orders
            dgv.CellDoubleClick += Dgv_CellDoubleClick;

            // Add controls in correct order
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
        }

        private void LoadData()
        {
            dgv.DataSource = null;
            dgv.DataSource = controller.GetAllKhachHang();
            
            // Format columns
            if (dgv.Columns.Count > 0)
            {
                if (dgv.Columns.Contains("IdKH"))
                    dgv.Columns["IdKH"].HeaderText = "Mã KH";
                if (dgv.Columns.Contains("HoTen"))
                    dgv.Columns["HoTen"].HeaderText = "Họ tên";
                if (dgv.Columns.Contains("Sdt"))
                    dgv.Columns["Sdt"].HeaderText = "Số điện thoại";
                if (dgv.Columns.Contains("GioiTinh"))
                    dgv.Columns["GioiTinh"].HeaderText = "Giới tính";
                if (dgv.Columns.Contains("NgayThamGia"))
                {
                    dgv.Columns["NgayThamGia"].HeaderText = "Ngày đăng ký";
                    dgv.Columns["NgayThamGia"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgv.Columns.Contains("DiemTichLuy"))
                {
                    dgv.Columns["DiemTichLuy"].HeaderText = "Điểm tích lũy";
                    dgv.Columns["DiemTichLuy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new AddKhachHangDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idKH = dgv.CurrentRow.Cells["IdKH"].Value?.ToString() ?? "";
            string hoTen = dgv.CurrentRow.Cells["HoTen"].Value?.ToString() ?? "";

            var result = MessageBox.Show(
                $"⚠ Bạn muốn xóa khách hàng '{hoTen}'?\n\nKhách hàng sẽ được chuyển vào Thùng rác.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (controller.DeleteKhachHang(idKH, out string msg))
                {
                    MessageBox.Show($"✓ Đã chuyển khách hàng '{hoTen}' vào thùng rác!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"❌ {msg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnViewOrders_Click(object? sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idKH = dgv.CurrentRow.Cells["IdKH"].Value?.ToString() ?? "";
            string hoTen = dgv.CurrentRow.Cells["HoTen"].Value?.ToString() ?? "";

            var dialog = new ViewKhachHangOrdersDialog(idKH, hoTen);
            dialog.ShowDialog();
        }

        private void Dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnViewOrders_Click(sender, e);
            }
        }
    }
}