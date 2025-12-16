using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class PhanHoiPanel : UserControl
    {
        private DataGridView dgv;
        private PhanHoiController controller = new PhanHoiController();
        
        public PhanHoiPanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(10);
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // Panel header
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke, Padding = new Padding(5) };
            
            var lblInfo = new Label 
            { 
                Text = "💬 Click vào phản hồi để xem chi tiết", 
                Location = new Point(10, 20), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219)
            };
            pnlTop.Controls.Add(lblInfo);

            // DataGridView - THÊM TRƯỚC
            dgv = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false;
            dgv.CellClick += Dgv_CellClick;
            
            Controls.Add(dgv);

            // Panel khoảng cách
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

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Rows[e.RowIndex].DataBoundItem == null) return;
            
            var phanHoi = dgv.Rows[e.RowIndex].DataBoundItem as PhanHoi;
            if (phanHoi == null) return;

            var detailMsg = $"📋 THÔNG TIN CHI TIẾT PHẢN HỒI\n\n" +
                          $"🆔 ID Phản hồi: {phanHoi.IdPH}\n" +
                          $"👤 Khách hàng: {phanHoi.TenKH ?? "(Chưa có)"} (Mã: {phanHoi.IdKH ?? "N/A"})\n" +
                          $"📧 Email: {phanHoi.Email ?? "(Chưa có)"}\n" +
                          $"📞 Số điện thoại: {phanHoi.Sdt ?? "(Chưa có)"}\n" +
                          $"📅 Ngày tạo: {phanHoi.NgayTao:dd/MM/yyyy HH:mm:ss}\n" +
                          $"⭐ Đánh giá: {phanHoi.DanhGia} sao\n\n" +
                          $"💬 Nội dung:\n{phanHoi.NoiDung ?? "(Không có nội dung)"}";

            MessageBox.Show(detailMsg, "Chi tiết phản hồi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadData() => dgv.DataSource = controller.GetAllPhanHoi();
    }
}