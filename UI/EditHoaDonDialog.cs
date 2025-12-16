using System;
using System.Windows.Forms;
using System.Drawing;
using QLThuocApp.Entities;
// using QLThuocApp.Controllers; // Nếu cần gọi update

namespace QLThuocApp.UI
{
    public class EditHoaDonDialog : Form
    {
        public HoaDon HoaDonResult { get; private set; }
        private ComboBox cboTrangThai;
        
        public EditHoaDonDialog(HoaDon hd)
        {
            HoaDonResult = hd;
            Text = $"Sửa Hóa Đơn: {hd.IdHD}";
            Size = new Size(400, 250);
            StartPosition = FormStartPosition.CenterParent;

            var lblId = new Label { Text = $"Mã HĐ: {hd.IdHD}", Location = new Point(30, 30), AutoSize = true };
            Controls.Add(lblId);

            var lblTong = new Label { Text = $"Tổng tiền: {hd.TongTien:N0}", Location = new Point(30, 60), AutoSize = true };
            Controls.Add(lblTong);

            Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(30, 100) });
            cboTrangThai = new ComboBox { Location = new Point(120, 98), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Hoàn thành", "Chờ xử lý", "Đã hủy" });
            cboTrangThai.SelectedItem = hd.TrangThaiDonHang;
            Controls.Add(cboTrangThai);

            var btnSave = new Button { Text = "Lưu", Location = new Point(120, 150), DialogResult = DialogResult.OK };
            btnSave.Click += (s, e) => {
                HoaDonResult.TrangThaiDonHang = cboTrangThai.SelectedItem?.ToString() ?? "";
                // Thực tế nên gọi Controller.Update(HoaDonResult) ở đây
                MessageBox.Show("Đã cập nhật trạng thái (Giả lập).");
            };
            Controls.Add(btnSave);
        }
    }
}