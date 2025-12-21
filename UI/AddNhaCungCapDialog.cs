using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class AddNhaCungCapDialog : Form
    {
        private TextBox txtIdNCC, txtTenNCC, txtSdt, txtDiaChi;
        private Button btnSave, btnCancel;
        
        private NhaCungCapController controller = new NhaCungCapController();
        public string NewNCCId { get; private set; } = "";

        public AddNhaCungCapDialog()
        {
            Text = "Thêm Nhà Cung Cấp Mới";
            Size = new Size(550, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            InitializeUI();
            GenerateNCCId();
        }

        private void InitializeUI()
        {
            int y = 20;
            int labelX = 20;
            int controlX = 180;
            int controlWidth = 320;

            // Tiêu đề
            var lblTitle = new Label
            {
                Text = "📋 THÔNG TIN NHÀ CUNG CẤP",
                Location = new Point(20, y),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219)
            };
            Controls.Add(lblTitle);
            y += 45;

            // Mã NCC
            Controls.Add(new Label 
            { 
                Text = "Mã NCC:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtIdNCC = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10F),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            Controls.Add(txtIdNCC);
            y += 40;

            // Tên NCC
            Controls.Add(new Label 
            { 
                Text = "Tên NCC:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtTenNCC = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập tên nhà cung cấp..."
            };
            Controls.Add(txtTenNCC);
            y += 40;

            // Số điện thoại
            Controls.Add(new Label 
            { 
                Text = "Số Điện Thoại:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtSdt = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập 10 số điện thoại...",
                MaxLength = 10
            };
            Controls.Add(txtSdt);
            y += 40;

            // Địa chỉ
            Controls.Add(new Label 
            { 
                Text = "Địa Chỉ:", 
                Location = new Point(labelX, y), 
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            txtDiaChi = new TextBox 
            { 
                Location = new Point(controlX, y), 
                Size = new Size(controlWidth, 60),
                Font = new Font("Segoe UI", 10F),
                Multiline = true,
                PlaceholderText = "Nhập địa chỉ..."
            };
            Controls.Add(txtDiaChi);
            y += 75;

            // Buttons
            btnSave = new Button 
            { 
                Text = "💾 Lưu", 
                Location = new Point(controlX, y), 
                Size = new Size(140, 40),
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
                Location = new Point(controlX + 160, y), 
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

        private void GenerateNCCId()
        {
            var allNCC = controller.GetAllNhaCungCap();
            int maxNumber = 0;

            foreach (var ncc in allNCC)
            {
                if (ncc.IdNCC.StartsWith("NCC") && ncc.IdNCC.Length > 3)
                {
                    if (int.TryParse(ncc.IdNCC.Substring(3), out int num))
                    {
                        if (num > maxNumber)
                            maxNumber = num;
                    }
                }
            }

            txtIdNCC.Text = "NCC" + (maxNumber + 1).ToString("D3");
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenNCC.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSdt.Focus();
                return;
            }

            if (txtSdt.Text.Length != 10 || !txtSdt.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSdt.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            var ncc = new NhaCungCap
            {
                IdNCC = txtIdNCC.Text,
                TenNCC = txtTenNCC.Text.Trim(),
                Sdt = txtSdt.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            if (controller.AddNhaCungCap(ncc, out string errorMsg))
            {
                NewNCCId = ncc.IdNCC;
                MessageBox.Show("✓ Thêm nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show($"✗ Lỗi: {errorMsg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
