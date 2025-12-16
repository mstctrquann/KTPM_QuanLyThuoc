using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class NhanVienPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private NhanVienController controller = new NhanVienController();

        public NhanVienPanel()
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
            // Panel Top chứa các nút
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke, Padding = new Padding(5) };

            btnAdd = new Button 
            { 
                Text = "➕ Thêm NV", 
                Location = new Point(10, 12), 
                Size = new Size(110, 36), 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            pnlTop.Controls.Add(btnAdd);

            btnEdit = new Button 
            { 
                Text = "✏ Sửa", 
                Location = new Point(130, 12), 
                Size = new Size(110, 36), 
                BackColor = Color.FromArgb(241, 196, 15), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => MessageBox.Show("Chức năng sửa nhân viên đang phát triển", "Thông báo");
            pnlTop.Controls.Add(btnEdit);

            btnDelete = new Button 
            { 
                Text = "🗑 Xóa (Thùng rác)", 
                Location = new Point(250, 12), 
                Size = new Size(180, 36), 
                BackColor = Color.FromArgb(231, 76, 60), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            pnlTop.Controls.Add(btnDelete);

            btnRefresh = new Button 
            { 
                Text = "🔄", 
                Location = new Point(440, 12), 
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
                MultiSelect = false,
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

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idNV = dgv.CurrentRow.Cells["IdNV"].Value?.ToString() ?? "";
            string hoTen = dgv.CurrentRow.Cells["HoTen"].Value?.ToString() ?? "";

            var confirmMsg = $"⚠ Bạn muốn xóa nhân viên này?\n\n" +
                           $"Mã NV: {idNV}\n" +
                           $"Họ tên: {hoTen}\n\n" +
                           $"Nhân viên sẽ được chuyển vào Thùng rác\n" +
                           $"và có thể khôi phục sau.";

            if (MessageBox.Show(confirmMsg, "Xác nhận xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (controller.DeleteNhanVien(idNV, out string msg))
                {
                    MessageBox.Show($"✓ Đã chuyển nhân viên '{hoTen}' vào thùng rác!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"Lỗi: {msg}", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new RegisterEmployeeDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            dgv.DataSource = controller.GetAllNhanVien();
            
            // Định dạng cột lương với dấu phân cách hàng nghìn
            if (dgv.Columns.Contains("Luong"))
            {
                dgv.Columns["Luong"].HeaderText = "Lương";
                dgv.Columns["Luong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                // Format số thủ công vì Luong là string
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["Luong"].Value != null && decimal.TryParse(row.Cells["Luong"].Value.ToString(), out decimal luong))
                    {
                        row.Cells["Luong"].Value = luong.ToString("N0");
                    }
                }
            }
            
            // Định dạng các cột tiêu đề khác nếu cần
            if (dgv.Columns.Contains("IdNV"))
                dgv.Columns["IdNV"].HeaderText = "Mã NV";
            if (dgv.Columns.Contains("HoTen"))
                dgv.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgv.Columns.Contains("Sdt"))
                dgv.Columns["Sdt"].HeaderText = "Số điện thoại";
            if (dgv.Columns.Contains("GioiTinh"))
                dgv.Columns["GioiTinh"].HeaderText = "Giới tính";
            if (dgv.Columns.Contains("NamSinh"))
                dgv.Columns["NamSinh"].HeaderText = "Năm sinh";
            if (dgv.Columns.Contains("NgayVaoLam"))
            {
                dgv.Columns["NgayVaoLam"].HeaderText = "Ngày vào làm";
                dgv.Columns["NgayVaoLam"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgv.Columns.Contains("TrangThai"))
                dgv.Columns["TrangThai"].HeaderText = "Trạng thái";
        }
    }
}