using System;
using System.Windows.Forms;
using System.Drawing;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public partial class ViewHopDongDialog : Form
    {
        private HopDongController controller = new HopDongController();
        private NhanVienController nvController = new NhanVienController();
        private NhaCungCapController nccController = new NhaCungCapController();

        public ViewHopDongDialog(string idHD)
        {
            Text = "Chi Tiết Hợp Đồng - " + idHD;
            Size = new Size(700, 650);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            LoadAndDisplay(idHD);
        }

        private void LoadAndDisplay(string idHD)
        {
            var hopDong = controller.GetHopDongById(idHD);
            if (hopDong == null)
            {
                MessageBox.Show("Không tìm thấy hợp đồng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // Lấy thông tin nhân viên và nhà cung cấp
            var nhanVien = nvController.GetAllNhanVien().Find(nv => nv.IdNV == hopDong.IdNV);
            var nhaCungCap = nccController.GetAllNhaCungCap().Find(ncc => ncc.IdNCC == hopDong.IdNCC);

            int y = 20;
            int labelX = 20;
            int valueX = 180;
            int valueWidth = 450;

            // Title
            var lblTitle = new Label
            {
                Text = "THÔNG TIN HỢP ĐỒNG",
                Location = new Point(labelX, y),
                Size = new Size(valueWidth + 180, 35),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblTitle);
            y += 45;

            // Separator
            var separator1 = new Panel
            {
                Location = new Point(labelX, y),
                Size = new Size(valueWidth + 160, 2),
                BackColor = Color.FromArgb(189, 195, 199)
            };
            Controls.Add(separator1);
            y += 15;

            // Mã hợp đồng
            AddInfoRow("Mã Hợp Đồng:", hopDong.IdHD, ref y, labelX, valueX, valueWidth);

            // Ngày bắt đầu
            AddInfoRow("Ngày Bắt Đầu:", hopDong.NgayBatDau.ToString("dd/MM/yyyy HH:mm"), ref y, labelX, valueX, valueWidth);

            // Ngày kết thúc
            var ngayKetThuc = hopDong.NgayKetThuc.HasValue ? hopDong.NgayKetThuc.Value.ToString("dd/MM/yyyy") : "Không xác định";
            AddInfoRow("Ngày Kết Thúc:", ngayKetThuc, ref y, labelX, valueX, valueWidth);

            // Thời hạn còn lại
            if (hopDong.NgayKetThuc.HasValue)
            {
                var daysLeft = (hopDong.NgayKetThuc.Value - DateTime.Now).Days;
                var statusText = daysLeft > 0 ? $"{daysLeft} ngày" : "Đã hết hạn";
                var statusColor = daysLeft > 30 ? Color.Green : (daysLeft > 0 ? Color.Orange : Color.Red);
                AddInfoRow("Thời Hạn Còn:", statusText, ref y, labelX, valueX, valueWidth, statusColor);
            }

            // Người làm hợp đồng
            var tenNhanVien = nhanVien != null ? $"{nhanVien.HoTen} ({nhanVien.IdNV})" : hopDong.IdNV;
            AddInfoRow("Người Làm HĐ:", tenNhanVien, ref y, labelX, valueX, valueWidth);

            // Nhà cung cấp
            var tenNCC = nhaCungCap != null ? $"{nhaCungCap.TenNCC} ({nhaCungCap.IdNCC})" : hopDong.IdNCC;
            AddInfoRow("Nhà Cung Cấp:", tenNCC, ref y, labelX, valueX, valueWidth);

            // Separator
            var separator2 = new Panel
            {
                Location = new Point(labelX, y),
                Size = new Size(valueWidth + 160, 2),
                BackColor = Color.FromArgb(189, 195, 199)
            };
            Controls.Add(separator2);
            y += 15;

            // Điều khoản
            Controls.Add(new Label
            {
                Text = "Điều Khoản Hợp Đồng:",
                Location = new Point(labelX, y),
                Size = new Size(valueWidth + 160, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            });
            y += 30;

            var txtDieuKhoan = new TextBox
            {
                Location = new Point(labelX, y),
                Size = new Size(valueWidth + 160, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9F),
                Text = hopDong.NoiDung ?? "Không có điều khoản",
                BackColor = Color.FromArgb(250, 250, 250)
            };
            Controls.Add(txtDieuKhoan);
            y += 210;

            // Nút đóng
            var btnClose = new Button
            {
                Text = "✖ Đóng",
                Location = new Point((Width - 150) / 2, y),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void AddInfoRow(string label, string value, ref int y, int labelX, int valueX, int valueWidth, Color? valueColor = null)
        {
            Controls.Add(new Label
            {
                Text = label,
                Location = new Point(labelX, y),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            });

            Controls.Add(new Label
            {
                Text = value,
                Location = new Point(valueX, y),
                Size = new Size(valueWidth, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = valueColor ?? Color.FromArgb(44, 62, 80)
            });

            y += 35;
        }
    }
}