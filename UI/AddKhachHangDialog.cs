using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class AddKhachHangDialog : Form
    {
        private TextBox txtMaKH, txtHoTen, txtSdt;
        private ComboBox cboGioiTinh;
        private Button btnSave, btnCancel;

        public AddKhachHangDialog()
        {
            Text = "Đăng ký khách hàng mới";
            Size = new Size(450, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            InitializeUI();
            GenerateMaKH();
        }

        private void InitializeUI()
        {
            int yPos = 20;
            int labelX = 20;
            int controlX = 140;
            int controlWidth = 270;

            // Mã khách hàng
            Controls.Add(new Label { Text = "Mã khách hàng:", Location = new Point(labelX, yPos), AutoSize = true });
            txtMaKH = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), ReadOnly = true, BackColor = Color.LightGray };
            Controls.Add(txtMaKH);
            yPos += 40;

            // Họ tên
            Controls.Add(new Label { Text = "Họ tên:", Location = new Point(labelX, yPos), AutoSize = true });
            txtHoTen = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtHoTen);
            yPos += 40;

            // Số điện thoại
            Controls.Add(new Label { Text = "Số điện thoại:", Location = new Point(labelX, yPos), AutoSize = true });
            txtSdt = new TextBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25) };
            Controls.Add(txtSdt);
            yPos += 40;

            // Giới tính
            Controls.Add(new Label { Text = "Giới tính:", Location = new Point(labelX, yPos), AutoSize = true });
            cboGioiTinh = new ComboBox { Location = new Point(controlX, yPos), Size = new Size(controlWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGioiTinh.SelectedIndex = 0;
            Controls.Add(cboGioiTinh);
            yPos += 50;

            // Buttons
            btnSave = new Button 
            { 
                Text = "💾 Lưu", 
                Location = new Point(controlX, yPos), 
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            btnCancel = new Button 
            { 
                Text = "✖ Hủy", 
                Location = new Point(controlX + 130, yPos), 
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            Controls.Add(btnCancel);
        }

        private void GenerateMaKH()
        {
            // Generate mã khách hàng tự động: KH + timestamp
            txtMaKH.Text = "KH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSdt.Focus();
                return;
            }

            // Create KhachHang
            var khachHang = new KhachHang
            {
                IdKH = txtMaKH.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                Sdt = txtSdt.Text.Trim(),
                GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                NgayThamGia = DateTime.Now
            };

            try
            {
                var controller = new KhachHangController();
                if (controller.AddKhachHang(khachHang, out string msg))
                {
                    MessageBox.Show($"✓ Đăng ký khách hàng thành công!\n\nMã KH: {khachHang.IdKH}\nHọ tên: {khachHang.HoTen}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show($"❌ Lỗi: {msg}", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
