using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class ViewKhachHangOrdersDialog : Form
    {
        private DataGridView dgvOrders;
        private Label lblCustomerInfo;
        private string customerId;
        private string customerName;

        public ViewKhachHangOrdersDialog(string idKH, string tenKH)
        {
            customerId = idKH;
            customerName = tenKH;
            
            Text = $"Lịch sử mua hàng - {tenKH}";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;

            InitializeUI();
            LoadOrders();
        }

        private void InitializeUI()
        {
            // Nút đóng ở dưới cùng - Thêm TRƯỚC
            var pnlBottom = new Panel 
            { 
                Dock = DockStyle.Bottom, 
                Height = 60, 
                BackColor = Color.WhiteSmoke, 
                Padding = new Padding(15) 
            };
            
            var btnClose = new Button
            {
                Text = "✕ Đóng",
                Size = new Size(120, 35),
                Location = new Point(360, 12),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnClose);
            Controls.Add(pnlBottom);

            // Panel khoảng cách phía dưới
            var pnlSpacerBottom = new Panel 
            { 
                Dock = DockStyle.Bottom, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacerBottom);

            // DataGridView for orders - Thêm SAU bottom panels
            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 }
            };
            
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            Controls.Add(dgvOrders);

            // Panel khoảng cách phía trên
            var pnlSpacerTop = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacerTop);

            // Customer info panel - Thêm CUỐI CÙNG
            var pnlInfo = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 70, 
                BackColor = Color.FromArgb(52, 152, 219), 
                Padding = new Padding(15) 
            };
            
            lblCustomerInfo = new Label
            {
                Text = $"👤 Khách hàng: {customerName} (Mã: {customerId})",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 23)
            };
            pnlInfo.Controls.Add(lblCustomerInfo);
            Controls.Add(pnlInfo);
        }

        private void LoadOrders()
        {
            try
            {
                var controller = new HoaDonController();
                var orders = controller.GetHoaDonByKhachHang(customerId);
                
                dgvOrders.DataSource = orders;
                
                // Format columns
                if (dgvOrders.Columns.Count > 0)
                {
                    if (dgvOrders.Columns.Contains("IdHD"))
                        dgvOrders.Columns["IdHD"].HeaderText = "Mã HĐ";
                    if (dgvOrders.Columns.Contains("ThoiGian"))
                    {
                        dgvOrders.Columns["ThoiGian"].HeaderText = "Thời gian";
                        dgvOrders.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                    if (dgvOrders.Columns.Contains("TongTien"))
                    {
                        dgvOrders.Columns["TongTien"].HeaderText = "Tổng tiền";
                        dgvOrders.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                        dgvOrders.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvOrders.Columns.Contains("TrangThaiDonHang"))
                        dgvOrders.Columns["TrangThaiDonHang"].HeaderText = "Trạng thái";
                    if (dgvOrders.Columns.Contains("PhuongThucThanhToan"))
                        dgvOrders.Columns["PhuongThucThanhToan"].HeaderText = "Thanh toán";
                        
                    // Hide unnecessary columns
                    if (dgvOrders.Columns.Contains("IdNV"))
                        dgvOrders.Columns["IdNV"].Visible = false;
                    if (dgvOrders.Columns.Contains("IdKH"))
                        dgvOrders.Columns["IdKH"].Visible = false;
                }
                
                lblCustomerInfo.Text += $" - Tổng {orders.Count} hóa đơn";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
