using System;
using System.Windows.Forms;
using System.Drawing;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class EditPhieuNhapDialog : Form
    {
        public EditPhieuNhapDialog(PhieuNhap pn)
        {
            Text = $"Sửa Phiếu Nhập: {pn.IdPN}";
            Size = new Size(400, 200);
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Chức năng sửa phiếu nhập đang được phát triển.", Location = new Point(50, 50), AutoSize = true });
            
            // Bạn có thể thêm các TextBox để sửa IdNCC, IdNV nếu muốn
        }
    }
}