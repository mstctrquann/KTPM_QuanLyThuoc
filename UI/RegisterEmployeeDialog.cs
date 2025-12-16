using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;
using QLThuocApp.dao;

namespace QLThuocApp.UI
{
    public class RegisterEmployeeDialog : Form
    {
        private TextBox txtMaNV, txtHoTen, txtSdt, txtUsername, txtPassword, txtConfirmPassword;
        private ComboBox cboGioiTinh, cboVaiTro;
        private NumericUpDown nudNamSinh;
        private DateTimePicker dtpNgayVaoLam;
        private TextBox txtLuong;
        private Button btnRegister, btnCancel;

        public RegisterEmployeeDialog()
        {
            Text = "Đăng ký tài khoản nhân viên";
            Size = new Size(500, 570);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScroll = true;

            InitializeUI();
        }

        private void InitializeUI()
        {
            int yPos = 20;
            int labelX = 20;
            int controlX = 150;
            int controlWidth = 300;

            // Mã nhân viên
            Controls.Add(new Label { Text = "Mã nhân viên:", Location = new Point(labelX, yPos), AutoSize = true });
            txtMaNV = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtMaNV);
            yPos += 35;

            // Họ tên
            Controls.Add(new Label { Text = "Họ tên:", Location = new Point(labelX, yPos), AutoSize = true });
            txtHoTen = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtHoTen);
            yPos += 35;

            // Số điện thoại
            Controls.Add(new Label { Text = "Số điện thoại:", Location = new Point(labelX, yPos), AutoSize = true });
            txtSdt = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtSdt);
            yPos += 35;

            // Giới tính
            Controls.Add(new Label { Text = "Giới tính:", Location = new Point(labelX, yPos), AutoSize = true });
            cboGioiTinh = new ComboBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;
            Controls.Add(cboGioiTinh);
            yPos += 35;

            // Năm sinh
            Controls.Add(new Label { Text = "Năm sinh:", Location = new Point(labelX, yPos), AutoSize = true });
            nudNamSinh = new NumericUpDown { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), Minimum = 1950, Maximum = DateTime.Now.Year - 18, Value = 1990 };
            Controls.Add(nudNamSinh);
            yPos += 35;

            // Ngày vào làm
            Controls.Add(new Label { Text = "Ngày vào làm:", Location = new Point(labelX, yPos), AutoSize = true });
            dtpNgayVaoLam = new DateTimePicker { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), Format = DateTimePickerFormat.Short };
            Controls.Add(dtpNgayVaoLam);
            yPos += 35;

            // Lương
            Controls.Add(new Label { Text = "Lương:", Location = new Point(labelX, yPos), AutoSize = true });
            txtLuong = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), Text = "5000000" };
            Controls.Add(txtLuong);
            yPos += 35;

            // Vai trò
            Controls.Add(new Label { Text = "Vai trò:", Location = new Point(labelX, yPos), AutoSize = true });
            cboVaiTro = new ComboBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboVaiTro.Items.AddRange(new object[] { "Admin", "Manager", "Nhân Viên" });
            cboVaiTro.SelectedIndex = 2;
            Controls.Add(cboVaiTro);
            yPos += 35;

            // Username
            Controls.Add(new Label { Text = "Tên đăng nhập:", Location = new Point(labelX, yPos), AutoSize = true });
            txtUsername = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtUsername);
            yPos += 35;

            // Password
            Controls.Add(new Label { Text = "Mật khẩu:", Location = new Point(labelX, yPos), AutoSize = true });
            txtPassword = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), UseSystemPasswordChar = true };
            Controls.Add(txtPassword);
            yPos += 35;

            // Confirm Password
            Controls.Add(new Label { Text = "Xác nhận MK:", Location = new Point(labelX, yPos), AutoSize = true });
            txtConfirmPassword = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), UseSystemPasswordChar = true };
            Controls.Add(txtConfirmPassword);
            yPos += 40;

            // Buttons
            btnRegister = new Button 
            { 
                Text = "✓ Đăng ký", 
                Location = new Point(controlX, yPos), 
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            Controls.Add(btnRegister);

            btnCancel = new Button 
            { 
                Text = "✖ Hủy", 
                Location = new Point(controlX + 150, yPos), 
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            Controls.Add(btnCancel);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create NhanVien
            var nhanVien = new NhanVien
            {
                IdNV = txtMaNV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                Sdt = txtSdt.Text.Trim(),
                GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                NamSinh = (int)nudNamSinh.Value,
                NgayVaoLam = dtpNgayVaoLam.Value,
                Luong = txtLuong.Text.Trim(),
                TrangThai = "DangLamViec"
            };

            // Determine role_id
            string roleId = cboVaiTro.SelectedIndex switch
            {
                0 => "1", // Admin
                1 => "2", // Manager
                _ => "3"  // Nhân Viên
            };

            try
            {
                // Insert NhanVien
                var nhanVienDAO = new NhanVienDAO();
                if (!nhanVienDAO.Insert(nhanVien))
                {
                    MessageBox.Show("Lỗi khi tạo thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert TaiKhoan
                var taiKhoanDAO = new TaiKhoanDAO();
                if (!taiKhoanDAO.Register(txtUsername.Text.Trim(), txtPassword.Text, nhanVien.IdNV, roleId))
                {
                    MessageBox.Show("Lỗi khi tạo tài khoản đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"✓ Đăng ký thành công!\n\nNhân viên: {nhanVien.HoTen}\nTài khoản: {txtUsername.Text}", 
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
