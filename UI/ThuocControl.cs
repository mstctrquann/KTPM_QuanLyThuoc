using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class ThuocControl : UserControl
    {
        private DataGridView dgv = null!;
        private TextBox txtMa = null!, txtTen = null!, txtDonViTinh = null!, txtXuatXu = null!;
        private NumericUpDown nudGiaNhap = null!, nudDonGia = null!, nudSoLuong = null!;
        private DateTimePicker dtpHanSuDung = null!;
        private Button btnAdd = null!, btnEdit = null!, btnDelete = null!, btnSearch = null!, btnRefresh = null!, btnImport = null!;
        private ThuocController controller = new ThuocController();

        public ThuocControl()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(10);
            InitializeUI();
            ApplyRolePermissions();
            LoadData();
        }

        private void ApplyRolePermissions()
        {
            var roleId = LoginForm.CurrentUser?.IdVT;
            
            // Chỉ Admin (1) và Manager (2) mới có quyền thêm/sửa/xóa/import
            if (roleId == "3") // Nhân viên
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                btnImport.Visible = false;
                
                // Disable input fields
                txtMa.Enabled = false;
                txtTen.Enabled = false;
                txtDonViTinh.Enabled = false;
                nudGiaNhap.Enabled = false;
                nudDonGia.Enabled = false;
                nudSoLuong.Enabled = false;
                txtXuatXu.Enabled = false;
                dtpHanSuDung.Enabled = false;
            }
        }

        private void InitializeUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 155, BackColor = Color.WhiteSmoke, Padding = new Padding(10) };
            
            // Dòng 1: Mã, Tên
            pnlTop.Controls.Add(new Label { Text = "Mã thuốc:", Location = new Point(10, 15), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            txtMa = new TextBox { Location = new Point(100, 13), Size = new Size(120, 20) };
            pnlTop.Controls.Add(txtMa);

            pnlTop.Controls.Add(new Label { Text = "Tên thuốc:", Location = new Point(240, 15), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            txtTen = new TextBox { Location = new Point(330, 13), Size = new Size(250, 20) };
            pnlTop.Controls.Add(txtTen);

            pnlTop.Controls.Add(new Label { Text = "Đơn vị:", Location = new Point(600, 15), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            txtDonViTinh = new TextBox { Location = new Point(660, 13), Size = new Size(100, 20), Text = "Hộp" };
            pnlTop.Controls.Add(txtDonViTinh);

            // Dòng 2: Giá nhập, Giá bán, Số lượng
            pnlTop.Controls.Add(new Label { Text = "Giá nhập:", Location = new Point(10, 50), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            nudGiaNhap = new NumericUpDown { Location = new Point(100, 48), Size = new Size(120, 20), Maximum = 1000000000, ThousandsSeparator = true };
            pnlTop.Controls.Add(nudGiaNhap);

            pnlTop.Controls.Add(new Label { Text = "Giá bán:", Location = new Point(240, 50), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            nudDonGia = new NumericUpDown { Location = new Point(330, 48), Size = new Size(120, 20), Maximum = 1000000000, ThousandsSeparator = true };
            pnlTop.Controls.Add(nudDonGia);

            pnlTop.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(470, 50), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            nudSoLuong = new NumericUpDown { Location = new Point(550, 48), Size = new Size(100, 20), Maximum = 100000 };
            pnlTop.Controls.Add(nudSoLuong);

            // Dòng 3: Xuất xứ, Hạn sử dụng
            pnlTop.Controls.Add(new Label { Text = "Xuất xứ:", Location = new Point(10, 85), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            txtXuatXu = new TextBox { Location = new Point(100, 83), Size = new Size(150, 20), Text = "Việt Nam" };
            pnlTop.Controls.Add(txtXuatXu);

            pnlTop.Controls.Add(new Label { Text = "Hạn SD:", Location = new Point(270, 85), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            dtpHanSuDung = new DateTimePicker { Location = new Point(330, 83), Size = new Size(150, 20), Format = DateTimePickerFormat.Short };
            dtpHanSuDung.Value = DateTime.Now.AddYears(2);
            pnlTop.Controls.Add(dtpHanSuDung);

            // Buttons
            btnAdd = new Button { Text = "➕ Thêm", Location = new Point(10, 115), Size = new Size(90, 30), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => ActionAdd();
            pnlTop.Controls.Add(btnAdd);

            btnEdit = new Button { Text = "✏ Sửa", Location = new Point(110, 115), Size = new Size(90, 30), BackColor = Color.FromArgb(241, 196, 15), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => ActionUpdate();
            pnlTop.Controls.Add(btnEdit);

            btnDelete = new Button { Text = "🗑 Xóa", Location = new Point(210, 115), Size = new Size(90, 30), BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => ActionDelete();
            pnlTop.Controls.Add(btnDelete);

            btnSearch = new Button { Text = "🔍 Tìm", Location = new Point(310, 115), Size = new Size(90, 30), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => ActionSearch();
            pnlTop.Controls.Add(btnSearch);

            btnRefresh = new Button { Text = "🔄 Tải lại", Location = new Point(410, 115), Size = new Size(90, 30), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => { ClearInputs(); LoadData(); };
            pnlTop.Controls.Add(btnRefresh);

            btnImport = new Button { Text = "📁 Nhập CSV", Location = new Point(510, 115), Size = new Size(100, 30), BackColor = Color.FromArgb(155, 89, 182), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.Click += (s, e) => ActionImport();
            pnlTop.Controls.Add(btnImport);

            // Grid - THÊM TRƯỚC
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
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgv.CellClick += Dgv_CellClick;
            Controls.Add(dgv);

            // Panel khoảng cách (spacer)
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

        private void LoadData() => dgv.DataSource = controller.GetAll();

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgv.Rows[e.RowIndex];
                txtMa.Text = row.Cells["IdThuoc"].Value?.ToString();
                txtTen.Text = row.Cells["TenThuoc"].Value?.ToString();
                txtDonViTinh.Text = row.Cells["DonViTinh"].Value?.ToString();
                txtXuatXu.Text = row.Cells["XuatXu"].Value?.ToString();
                
                if (decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out decimal giaNhap))
                    nudGiaNhap.Value = giaNhap;
                
                if (decimal.TryParse(row.Cells["DonGia"].Value?.ToString(), out decimal donGia))
                    nudDonGia.Value = donGia;
                
                if (int.TryParse(row.Cells["SoLuongTon"].Value?.ToString(), out int sl))
                    nudSoLuong.Value = sl;
                
                if (DateTime.TryParse(row.Cells["HanSuDung"].Value?.ToString(), out DateTime hsd))
                    dtpHanSuDung.Value = hsd;
            }
        }

        private void ActionAdd()
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên thuốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var t = new Thuoc 
            { 
                IdThuoc = txtMa.Text.Trim(), 
                TenThuoc = txtTen.Text.Trim(), 
                DonViTinh = txtDonViTinh.Text.Trim(),
                XuatXu = txtXuatXu.Text.Trim(),
                GiaNhap = (double)nudGiaNhap.Value, 
                DonGia = (double)nudDonGia.Value, 
                SoLuongTon = (int)nudSoLuong.Value, 
                HanSuDung = dtpHanSuDung.Value
            };
            
            if (controller.Add(t, out string msg)) 
            {
                LoadData();
                ClearInputs();
                MessageBox.Show("✓ Thêm thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionUpdate()
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên thuốc để tìm và sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị thông tin hiện tại
            var confirmMsg = $"Bạn có chắc muốn cập nhật thuốc?\n\n" +
                           $"Mã: {txtMa.Text}\n" +
                           $"Tên: {txtTen.Text}\n" +
                           $"Giá nhập: {nudGiaNhap.Value:N0} VNĐ\n" +
                           $"Giá bán: {nudDonGia.Value:N0} VNĐ\n" +
                           $"Số lượng: {nudSoLuong.Value}\n" +
                           $"Hạn SD: {dtpHanSuDung.Value:dd/MM/yyyy}";

            if (MessageBox.Show(confirmMsg, "Xác nhận cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            var t = new Thuoc 
            { 
                IdThuoc = txtMa.Text.Trim(), 
                TenThuoc = txtTen.Text.Trim(),
                DonViTinh = txtDonViTinh.Text.Trim(),
                XuatXu = txtXuatXu.Text.Trim(),
                GiaNhap = (double)nudGiaNhap.Value, 
                DonGia = (double)nudDonGia.Value, 
                SoLuongTon = (int)nudSoLuong.Value, 
                HanSuDung = dtpHanSuDung.Value
            };
            
            if (controller.Update(t, out string msg)) 
            {
                LoadData();
                ClearInputs();
                MessageBox.Show("✓ Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionDelete()
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên thuốc để tìm và xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị thông tin trước khi xóa
            var confirmMsg = $"⚠ BẠN CHẮC CHẮN MUỐN XÓA THUỐC NÀY?\n\n" +
                           $"Mã: {txtMa.Text}\n" +
                           $"Tên: {txtTen.Text}\n" +
                           $"Giá bán: {nudDonGia.Value:N0} VNĐ\n" +
                           $"Số lượng tồn: {nudSoLuong.Value}\n\n" +
                           $"Lưu ý: Nếu thuốc đã được bán hoặc nhập kho,\n" +
                           $"hệ thống sẽ chuyển sang trạng thái 'Đã xóa'\n" +
                           $"thay vì xóa vĩnh viễn (để bảo toàn dữ liệu lịch sử).";

            if (MessageBox.Show(confirmMsg, "⚠ Cảnh báo - Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (controller.Delete(txtMa.Text, out string msg)) 
                {
                    LoadData();
                    ClearInputs();
                    MessageBox.Show("✓ Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Hiển thị lỗi chi tiết hơn
                    if (msg.Contains("foreign key") || msg.Contains("FOREIGN KEY") || msg.Contains("CONSTRAINT"))
                    {
                        MessageBox.Show(
                            "❌ KHÔNG THỂ XÓA THUỐC NÀY!\n\n" +
                            "Lý do: Thuốc này đã được sử dụng trong:\n" +
                            "• Hóa đơn bán hàng, hoặc\n" +
                            "• Phiếu nhập kho\n\n" +
                            "Giải pháp:\n" +
                            "1. Kiểm tra lại các hóa đơn/phiếu nhập có chứa thuốc này\n" +
                            "2. Hoặc đánh dấu 'Ngừng kinh doanh' thay vì xóa\n\n" +
                            "Chi tiết kỹ thuật: " + msg,
                            "Lỗi ràng buộc dữ liệu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi: {msg}", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

private void ActionSearch()
        {
            try
            {
                var dialog = new SearchThuocDialog();
                
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedThuoc != null)
                {
                    // Load thông tin thuốc vào form
                    LoadThuocToForm(dialog.SelectedThuoc);
                    
                    // Hiển thị chi tiết trong MessageBox
                    var thuoc = dialog.SelectedThuoc;
                    var info = $"📋 THÔNG TIN CHI TIẾT THUỐC\n\n" +
                              $"🔖 Mã thuốc: {thuoc.IdThuoc}\n" +
                              $"💊 Tên thuốc: {thuoc.TenThuoc}\n" +
                              $"📦 Đơn vị tính: {thuoc.DonViTinh}\n" +
                              $"🌍 Xuất xứ: {thuoc.XuatXu}\n" +
                              $"💰 Giá nhập: {thuoc.GiaNhap:N0} VNĐ\n" +
                              $"💵 Giá bán: {thuoc.DonGia:N0} VNĐ\n" +
                              $"📊 Số lượng tồn: {thuoc.SoLuongTon}\n" +
                              $"📅 Hạn sử dụng: {thuoc.HanSuDung:dd/MM/yyyy}\n";
                    
                    MessageBox.Show(info, "Thông tin thuốc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThuocToForm(Thuoc thuoc)
        {
            try
            {
                txtMa.Text = thuoc.IdThuoc ?? "";
                txtTen.Text = thuoc.TenThuoc ?? "";
                txtDonViTinh.Text = thuoc.DonViTinh ?? "Hộp";
                txtXuatXu.Text = thuoc.XuatXu ?? "Việt Nam";
                
                // Kiểm tra giá trị hợp lệ trước khi gán
                nudGiaNhap.Value = thuoc.GiaNhap >= 0 ? (decimal)thuoc.GiaNhap : 0;
                nudDonGia.Value = thuoc.DonGia >= 0 ? (decimal)thuoc.DonGia : 0;
                nudSoLuong.Value = thuoc.SoLuongTon >= 0 ? thuoc.SoLuongTon : 0;
                
                if (thuoc.HanSuDung != default(DateTime))
                    dtpHanSuDung.Value = thuoc.HanSuDung;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtDonViTinh.Text = "Hộp";
            txtXuatXu.Text = "Việt Nam";
            nudGiaNhap.Value = 0;
            nudDonGia.Value = 0;
            nudSoLuong.Value = 0;
            dtpHanSuDung.Value = DateTime.Now.AddYears(2);
        }
        
        private void ActionImport()
        {
            using (var dialog = new ImportDrugsDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    MessageBox.Show("Đã nhập thuốc thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}