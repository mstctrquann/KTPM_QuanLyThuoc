using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class SearchThuocDialog : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvResults = null!;
        private Button btnOK = null!, btnCancel = null!;
        private ThuocController controller = new ThuocController();
        private System.Collections.Generic.List<Thuoc> allThuoc = new System.Collections.Generic.List<Thuoc>();
        
        public Thuoc? SelectedThuoc { get; private set; }

        public SearchThuocDialog()
        {
            InitializeComponent();
            LoadAllThuoc();
        }

        private void InitializeComponent()
        {
            this.Text = "🔍 Tìm kiếm thuốc";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Panel phía dưới (nút) - Thêm trước
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.WhiteSmoke, Padding = new Padding(15) };
            this.Controls.Add(pnlBottom);

            btnOK = new Button 
            { 
                Text = "✓ Chọn thuốc này", 
                Size = new Size(150, 35), 
                Location = new Point(380, 12),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += BtnOK_Click;
            pnlBottom.Controls.Add(btnOK);

            btnCancel = new Button 
            { 
                Text = "✕ Hủy", 
                Size = new Size(100, 35), 
                Location = new Point(540, 12),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            pnlBottom.Controls.Add(btnCancel);

            // Panel phía trên
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.WhiteSmoke, Padding = new Padding(15) };
            this.Controls.Add(pnlTop);

            var lblInfo = new Label 
            { 
                Text = "💡 Nhập mã thuốc hoặc tên thuốc để tìm kiếm:", 
                Location = new Point(15, 12), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219)
            };
            pnlTop.Controls.Add(lblInfo);

            txtSearch = new TextBox 
            { 
                Location = new Point(15, 40), 
                Size = new Size(640, 25),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Ví dụ: TH001 hoặc Paracetamol"
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            pnlTop.Controls.Add(txtSearch);

            // DataGridView kết quả
            dgvResults = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgvResults.CellDoubleClick += DgvResults_CellDoubleClick;
            this.Controls.Add(dgvResults);
        }

        private void LoadAllThuoc()
        {
            try
            {
                allThuoc = controller.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var searchText = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                dgvResults.DataSource = null;
                return;
            }

            // Tìm kiếm theo mã hoặc tên (không phân biệt hoa thường)
            var results = allThuoc.Where(t => 
                (t.IdThuoc != null && t.IdThuoc.ToLower().Contains(searchText)) ||
                (t.TenThuoc != null && t.TenThuoc.ToLower().Contains(searchText))
            ).Select(t => new
            {
                MaThuoc = t.IdThuoc,
                TenThuoc = t.TenThuoc,
                DonVi = t.DonViTinh,
                GiaBan = $"{t.DonGia:N0} VNĐ",
                SoLuongTon = t.SoLuongTon,
                XuatXu = t.XuatXu,
                HanDung = t.HanSuDung.ToString("dd/MM/yyyy")
            }).ToList();

            dgvResults.DataSource = results;
            
            if (dgvResults.Columns.Count > 0)
            {
                dgvResults.Columns["MaThuoc"]!.HeaderText = "Mã thuốc";
                dgvResults.Columns["MaThuoc"]!.Width = 100;
                dgvResults.Columns["TenThuoc"]!.HeaderText = "Tên thuốc";
                dgvResults.Columns["TenThuoc"]!.Width = 200;
                dgvResults.Columns["DonVi"]!.HeaderText = "Đơn vị";
                dgvResults.Columns["DonVi"]!.Width = 80;
                dgvResults.Columns["GiaBan"]!.HeaderText = "Giá bán";
                dgvResults.Columns["GiaBan"]!.Width = 120;
                dgvResults.Columns["SoLuongTon"]!.HeaderText = "Tồn kho";
                dgvResults.Columns["SoLuongTon"]!.Width = 80;
                dgvResults.Columns["XuatXu"]!.HeaderText = "Xuất xứ";
                dgvResults.Columns["XuatXu"]!.Width = 100;
                dgvResults.Columns["HanDung"]!.HeaderText = "Hạn dùng";
                dgvResults.Columns["HanDung"]!.Width = 100;
            }
        }

        private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng beep
                
                // Nếu có kết quả, chọn dòng đầu tiên
                if (dgvResults.Rows.Count > 0)
                {
                    dgvResults.Rows[0].Selected = true;
                    BtnOK_Click(null, EventArgs.Empty);
                }
            }
            else if (e.KeyCode == Keys.Down && dgvResults.Rows.Count > 0)
            {
                dgvResults.Focus();
                dgvResults.Rows[0].Selected = true;
            }
        }

        private void DgvResults_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnOK_Click(null, EventArgs.Empty);
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (dgvResults.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một thuốc từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var maThuoc = dgvResults.CurrentRow.Cells["MaThuoc"].Value?.ToString();
            
            if (!string.IsNullOrEmpty(maThuoc))
            {
                SelectedThuoc = allThuoc.FirstOrDefault(t => t.IdThuoc == maThuoc);
                
                if (SelectedThuoc != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
