using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class AddPhieuNhapDialog : Form
    {
        // Controller
        private PhieuNhapController pnCtrl = new PhieuNhapController();
        private ThuocController thuocCtrl = new ThuocController();
        private NhaCungCapController nccCtrl = new NhaCungCapController();

        // Controls
        private ComboBox cboNhaCungCap;
        private ComboBox cboThuoc;
        private NumericUpDown nudSoLuong;
        private NumericUpDown nudGiaNhap; // Khác hóa đơn: Nhập giá vốn
        private DataGridView dgvChiTiet;
        private Label lblTongTien;
        private Button btnAdd, btnSave, btnCancel;

        // Giỏ hàng tạm
        private List<ChiTietPhieuNhap> cart = new List<ChiTietPhieuNhap>();

        public AddPhieuNhapDialog()
        {
            Text = "Tạo Phiếu Nhập Kho";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            
            InitializeUI();
        }

        private void InitializeUI()
        {
            // --- Phần Header: Chọn Nhà Cung Cấp ---
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10) };
            
            pnlHeader.Controls.Add(new Label { Text = "Nhà Cung Cấp:", Location = new Point(20, 20), AutoSize = true });
            cboNhaCungCap = new ComboBox { Location = new Point(120, 18), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            // Load NCC
            cboNhaCungCap.DataSource = nccCtrl.GetAll();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "IdNCC";
            pnlHeader.Controls.Add(cboNhaCungCap);

            // Nút tạo mã phiếu tự động (Hiển thị chơi, thực tế Controller tự sinh)
            pnlHeader.Controls.Add(new Label { Text = "Mã Phiếu (Auto):", Location = new Point(400, 20), AutoSize = true });
            var txtMaPN = new TextBox { Text = "PN" + DateTime.Now.ToString("yyMMddHHmm"), ReadOnly = true, Location = new Point(500, 18) };
            pnlHeader.Controls.Add(txtMaPN);

            Controls.Add(pnlHeader);

            // --- Phần Nhập Liệu: Chọn Thuốc ---
            var pnlInput = new GroupBox { Text = "Chi tiết nhập", Dock = DockStyle.Top, Height = 80 };
            
            pnlInput.Controls.Add(new Label { Text = "Thuốc:", Location = new Point(20, 30), AutoSize = true });
            cboThuoc = new ComboBox { Location = new Point(70, 28), Size = new Size(200, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            // Load Thuốc
            cboThuoc.DataSource = thuocCtrl.GetAll();
            cboThuoc.DisplayMember = "TenThuoc";
            cboThuoc.ValueMember = "IdThuoc";
            // Tự động điền giá nhập cũ khi chọn thuốc
            cboThuoc.SelectedIndexChanged += (s, e) => {
                var t = cboThuoc.SelectedItem as Thuoc;
                if (t != null) nudGiaNhap.Value = (decimal)t.GiaNhap;
            };
            pnlInput.Controls.Add(cboThuoc);

            pnlInput.Controls.Add(new Label { Text = "SL:", Location = new Point(290, 30), AutoSize = true });
            nudSoLuong = new NumericUpDown { Location = new Point(320, 28), Size = new Size(80, 20), Minimum = 1, Maximum = 10000 };
            pnlInput.Controls.Add(nudSoLuong);

            pnlInput.Controls.Add(new Label { Text = "Giá Nhập:", Location = new Point(420, 30), AutoSize = true });
            nudGiaNhap = new NumericUpDown { Location = new Point(480, 28), Size = new Size(120, 20), Maximum = 1000000000 }; 
            pnlInput.Controls.Add(nudGiaNhap);

            btnAdd = new Button { Text = "Thêm dòng", Location = new Point(620, 26), Width = 100, BackColor = Color.LightBlue };
            btnAdd.Click += BtnAdd_Click;
            pnlInput.Controls.Add(btnAdd);

            Controls.Add(pnlInput);

            // --- Phần GridView: Danh sách hàng chờ nhập ---
            dgvChiTiet = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            Controls.Add(dgvChiTiet);

            // --- Phần Footer: Tổng tiền & Lưu ---
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            
            lblTongTien = new Label { Text = "Tổng tiền: 0 VNĐ", Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(20, 15), AutoSize = true };
            pnlFooter.Controls.Add(lblTongTien);

            btnSave = new Button { Text = "LƯU PHIẾU NHẬP", Location = new Point(550, 10), Width = 150, Height = 40, BackColor = Color.OrangeRed, ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnSave.Click += BtnSave_Click;
            pnlFooter.Controls.Add(btnSave);

            btnCancel = new Button { Text = "Hủy bỏ", Location = new Point(720, 10), Width = 100, Height = 40 };
            btnCancel.Click += (s, e) => this.Close();
            pnlFooter.Controls.Add(btnCancel);

            Controls.Add(pnlFooter);
            
            // Panel khoảng cách phía dưới
            var pnlSpacerBottom = new Panel 
            { 
                Dock = DockStyle.Bottom, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacerBottom);
            
            // --- Phần GridView: Danh sách hàng chờ nhập ---
            dgvChiTiet = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            Controls.Add(dgvChiTiet);
            
            // Panel khoảng cách giữa input và grid
            var pnlSpacerMiddle = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacerMiddle);
            
            Controls.Add(pnlInput);
            
            // Panel khoảng cách giữa header và input
            var pnlSpacerTop = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacerTop);
            
            Controls.Add(pnlHeader);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var selectedThuoc = cboThuoc.SelectedItem as Thuoc;
            if (selectedThuoc == null) return;

            // Kiểm tra xem thuốc đã có trong list chưa để cộng dồn
            var existingItem = cart.FirstOrDefault(x => x.IdThuoc == selectedThuoc.IdThuoc);
            if (existingItem != null)
            {
                existingItem.SoLuong += (int)nudSoLuong.Value;
                // Cập nhật giá nhập mới nhất nếu muốn
                existingItem.GiaNhap = (double)nudGiaNhap.Value;
            }
            else
            {
                var item = new ChiTietPhieuNhap
                {
                    IdThuoc = selectedThuoc.IdThuoc,
                    TenThuoc = selectedThuoc.TenThuoc, // Hiển thị tên
                    SoLuong = (int)nudSoLuong.Value,
                    GiaNhap = (double)nudGiaNhap.Value
                };
                cart.Add(item);
            }
            
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = cart;
            
            // Ẩn cột ID không cần thiết
            if (dgvChiTiet.Columns["IdPN"] != null) dgvChiTiet.Columns["IdPN"].Visible = false;
            
            // Tính tổng tiền
            double total = cart.Sum(x => x.ThanhTien);
            lblTongTien.Text = "Tổng tiền: " + total.ToString("N0") + " VNĐ";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Danh sách nhập hàng đang trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNhaCungCap.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng PhieuNhap
            var pn = new PhieuNhap
            {
                // IdPN sẽ được Controller/DAO tự sinh hoặc dùng DateTime
                IdPN = "PN" + DateTime.Now.ToString("yyMMddHHmmss"),
                ThoiGian = DateTime.Now,
                IdNV = LoginForm.CurrentUser?.IdNV ?? "NV001", // Lấy nhân viên đang đăng nhập
                IdNCC = cboNhaCungCap.SelectedValue.ToString(),
                TongTien = cart.Sum(x => x.ThanhTien)
            };

            // Gọi Controller lưu
            if (pnCtrl.Add(pn, cart, out string msg))
            {
                MessageBox.Show("Nhập hàng thành công! Tồn kho đã được cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}