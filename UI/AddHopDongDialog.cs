using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class AddHopDongDialog : Form
    {
        private TextBox txtIdHD, txtDieuKhoan;
        private DateTimePicker dtpNgayBatDau, dtpNgayKetThuc;
        private ComboBox cboNhanVien, cboNhaCungCap;
        private Button btnSave, btnCancel;
        
        private HopDongController controller = new HopDongController();
        private NhanVienController nvController = new NhanVienController();
        private NhaCungCapController nccController = new NhaCungCapController();

        public AddHopDongDialog()
        {
            Text = "Gia hạn Hợp Đồng";
            Size = new Size(700, 650);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            int y = 20;
            int labelX = 20;
            int controlX = 180;
            int controlWidth = 450;

            // ID Hợp Đồng
            Controls.Add(new Label 
            { 
                Text = "Mã Hợp Đồng:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtIdHD = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10F),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            txtIdHD.Text = GenerateContractId();
            Controls.Add(txtIdHD);
            y += 40;

            // Ngày bắt đầu
            Controls.Add(new Label 
            { 
                Text = "Ngày Bắt Đầu:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            dtpNgayBatDau = new DateTimePicker 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm:ss",
                Value = DateTime.Now,
                Font = new Font("Segoe UI", 10F)
            };
            Controls.Add(dtpNgayBatDau);
            y += 40;

            // Ngày kết thúc
            Controls.Add(new Label 
            { 
                Text = "Ngày Kết Thúc:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            dtpNgayKetThuc = new DateTimePicker 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Value = DateTime.Now.AddYears(2),
                Font = new Font("Segoe UI", 10F)
            };
            Controls.Add(dtpNgayKetThuc);
            y += 40;

            // Người làm hợp đồng
            Controls.Add(new Label 
            { 
                Text = "Người Làm HĐ:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            cboNhanVien = new ComboBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            Controls.Add(cboNhanVien);
            y += 40;

            // Nhà cung cấp
            Controls.Add(new Label 
            { 
                Text = "Nhà Cung Cấp:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            cboNhaCungCap = new ComboBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth - 50, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            Controls.Add(cboNhaCungCap);
            
            // Nút thêm NCC mới
            var btnAddNCC = new Button
            {
                Text = "➕",
                Location = new Point(controlX + controlWidth - 45, y),
                Size = new Size(40, 25),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnAddNCC.FlatAppearance.BorderSize = 0;
            btnAddNCC.Click += (s, e) => {
                var dialog = new AddNhaCungCapDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadNhaCungCap();
                    // Chọn NCC vừa thêm
                    if (cboNhaCungCap.DataSource is List<NhaCungCap> dataSource)
                    {
                        var newNCC = dataSource.FirstOrDefault(n => n.IdNCC == dialog.NewNCCId);
                        if (newNCC != null)
                        {
                            cboNhaCungCap.SelectedItem = newNCC;
                        }
                    }
                }
            };
            Controls.Add(btnAddNCC);
            y += 40;

            // Điều khoản
            Controls.Add(new Label 
            { 
                Text = "Điều Khoản:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtDieuKhoan = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9F)
            };
            txtDieuKhoan.Text = GetDefaultContractTerms();
            Controls.Add(txtDieuKhoan);
            y += 210;

            // Buttons
            btnSave = new Button 
            { 
                Text = "💾 Lưu Hợp Đồng", 
                Location = new Point(controlX, y), 
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            btnCancel = new Button 
            { 
                Text = "❌ Hủy", 
                Location = new Point(controlX + 220, y), 
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            // Load nhân viên (chỉ Admin và Manager)
            var allNhanVien = nvController.GetAllNhanVien();
            var adminManagers = allNhanVien.Where(nv => 
                nv.TrangThai == "DangLamViec" && 
                (nv.RoleId == "1" || nv.RoleId == "2")
            ).ToList();
            
            cboNhanVien.DataSource = adminManagers;
            cboNhanVien.DisplayMember = "HoTen";
            cboNhanVien.ValueMember = "IdNV";

            // Load nhà cung cấp
            LoadNhaCungCap();
        }

        private void LoadNhaCungCap()
        {
            var nhaCungCaps = nccController.GetAllNhaCungCap();
            cboNhaCungCap.DataSource = nhaCungCaps;
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "IdNCC";
        }

        private string GenerateContractId()
        {
            return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private string GetDefaultContractTerms()
        {
            return @"HỢP ĐỒNG CUNG CẤP DƯỢC PHẨM

I. ĐIỀU KHOẢN CHUNG
1. Bên A (Nhà thuốc) và Bên B (Nhà cung cấp) cam kết thực hiện đúng các điều khoản trong hợp đồng này.
2. Hợp đồng có hiệu lực từ ngày ký đến ngày hết hạn ghi trong hợp đồng.

II. TRÁCH NHIỆM CỦA BÊN B (NHÀ CUNG CẤP)
1. Cung cấp dược phẩm đảm bảo chất lượng, đúng nguồn gốc xuất xứ, có đầy đủ giấy tờ chứng nhận.
2. Giao hàng đúng thời gian, đúng số lượng theo đơn đặt hàng.
3. Bảo hành sản phẩm theo quy định và chịu trách nhiệm thu hồi sản phẩm lỗi.
4. Cung cấp hóa đơn VAT đầy đủ cho mỗi lô hàng.

III. TRÁCH NHIỆM CỦA BÊN A (NHÀ THUỐC)
1. Thanh toán đầy đủ, đúng hạn theo thỏa thuận.
2. Kiểm tra hàng hóa khi nhận và báo ngay nếu có vấn đề.
3. Bảo quản hàng hóa đúng quy cách sau khi nhận.

IV. ĐIỀU KHOẢN THANH TOÁN
1. Hình thức: Chuyển khoản hoặc tiền mặt
2. Thời hạn: Trong vòng 15 ngày sau khi nhận hàng
3. Chiết khấu: Theo thỏa thuận riêng cho từng đơn hàng

V. XỬ LÝ VI PHẠM
1. Phạt 5% giá trị hợp đồng nếu giao hàng chậm quá 7 ngày.
2. Đền bù 100% giá trị nếu hàng hóa không đúng chất lượng.
3. Bên vi phạm chịu mọi chi phí phát sinh do vi phạm.

VI. ĐIỀU KHOẢN KHÁC
1. Hợp đồng được gia hạn tự động nếu không có thông báo hủy trước 30 ngày.
2. Mọi tranh chấp được giải quyết thông qua thương lượng, hòa giải hoặc Tòa án.
3. Hợp đồng có thể được sửa đổi, bổ sung bằng văn bản thỏa thuận giữa hai bên.

Hợp đồng được lập thành 02 bản có giá trị pháp lý như nhau, mỗi bên giữ 01 bản.";
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cboNhanVien.SelectedItem == null || cboNhaCungCap.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpNgayKetThuc.Value <= dtpNgayBatDau.Value)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var hopDong = new HopDong
            {
                IdHD = txtIdHD.Text,
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                NoiDung = txtDieuKhoan.Text,
                IdNV = cboNhanVien.SelectedValue?.ToString() ?? "",
                IdNCC = cboNhaCungCap.SelectedValue?.ToString() ?? "",
                TrangThai = "CoHieuLuc"
            };

            if (controller.AddHopDong(hopDong, out string errorMsg))
            {
                MessageBox.Show("✓ Gia hạn hợp đồng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show($"❌ Lỗi: {errorMsg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
