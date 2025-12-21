using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class AddHoaDonDialog : Form
    {
        private ComboBox cboThuoc, cboPhuongThucThanhToan, cboKhachHang;
        private NumericUpDown nudSoLuong;
        private DataGridView dgvChiTiet;
        private Label lblTongTien;
        private Button btnAdd, btnSave;
        
        private ThuocController thuocCtrl = new ThuocController();
        private HoaDonController hdCtrl = new HoaDonController();
        private KhachHangController khCtrl = new KhachHangController();
        private List<ChiTietHoaDon> cart = new List<ChiTietHoaDon>();

        public AddHoaDonDialog()
        {
            Text = "Lập Hóa Đơn Bán Lẻ";
            Size = new System.Drawing.Size(900, 650);
            StartPosition = FormStartPosition.CenterParent;
            AutoScroll = true;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 115, BackColor = System.Drawing.Color.FromArgb(245, 245, 245), Padding = new Padding(10) };
            
            // Row 1: Khách hàng
            var lblKhachHang = new Label { Text = "Khách hàng:", Location = new System.Drawing.Point(10, 8), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold) };
            cboKhachHang = new ComboBox { Location = new System.Drawing.Point(10, 28), Size = new System.Drawing.Size(250, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new System.Drawing.Font("Segoe UI", 10F) };
            var khachHangList = khCtrl.GetAllKhachHang();
            // Add default customer
            khachHangList.Insert(0, new KhachHang { IdKH = "KHLE", HoTen = "Khách lẻ (Không tích điểm)" });
            cboKhachHang.DataSource = khachHangList;
            cboKhachHang.DisplayMember = "HoTen";
            cboKhachHang.ValueMember = "IdKH";
            
            // Row 2: Thuốc và số lượng
            var lblThuoc = new Label { Text = "Chọn thuốc:", Location = new System.Drawing.Point(10, 58), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold) };
            cboThuoc = new ComboBox { Location = new System.Drawing.Point(10, 78), Size = new System.Drawing.Size(250, 25), DropDownStyle = ComboBoxStyle.DropDownList, Font = new System.Drawing.Font("Segoe UI", 10F) };
            cboThuoc.DataSource = thuocCtrl.GetAll();
            cboThuoc.DisplayMember = "TenThuoc";
            cboThuoc.ValueMember = "IdThuoc";

            var lblSoLuong = new Label { Text = "Số lượng:", Location = new System.Drawing.Point(270, 58), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold) };
            nudSoLuong = new NumericUpDown { Location = new System.Drawing.Point(270, 78), Size = new System.Drawing.Size(100, 25), Minimum = 1, Maximum = 1000, Font = new System.Drawing.Font("Segoe UI", 10F) };
            
            btnAdd = new Button 
            { 
                Text = "➕ Thêm vào giỏ", 
                Location = new System.Drawing.Point(380, 76), 
                Size = new System.Drawing.Size(130, 30),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += AddToCart;

            pnlTop.Controls.AddRange(new Control[] { lblKhachHang, cboKhachHang, lblThuoc, cboThuoc, lblSoLuong, nudSoLuong, btnAdd });

            dgvChiTiet = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            
            // Add KeyDown event to delete items with Delete key
            dgvChiTiet.KeyDown += (s, ev) => {
                if (ev.KeyCode == Keys.Delete && dgvChiTiet.CurrentRow != null)
                {
                    RemoveFromCart();
                }
            };
            
            // Add context menu for right-click delete
            var contextMenu = new ContextMenuStrip();
            var deleteMenuItem = new ToolStripMenuItem("🗑 Xóa mặt hàng này", null, (s, ev) => RemoveFromCart());
            contextMenu.Items.Add(deleteMenuItem);
            dgvChiTiet.ContextMenuStrip = contextMenu;

            var pnlBot = new Panel { Dock = DockStyle.Bottom, Height = 90, BackColor = System.Drawing.Color.FromArgb(245, 245, 245), Padding = new Padding(10) };
            
            lblTongTien = new Label 
            { 
                Text = "Tổng tiền: 0 VNĐ", 
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold), 
                Location = new System.Drawing.Point(10, 15), 
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(231, 76, 60)
            };
            
            var lblPTTT = new Label 
            { 
                Text = "Phương thức thanh toán:", 
                Location = new System.Drawing.Point(10, 48), 
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
            };
            
            cboPhuongThucThanhToan = new ComboBox 
            { 
                Location = new System.Drawing.Point(220, 46), 
                Width = 200, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            cboPhuongThucThanhToan.Items.AddRange(new string[] { "TienMat", "ChuyenKhoan" });
            cboPhuongThucThanhToan.SelectedIndex = 0;
            
            btnSave = new Button 
            { 
                Text = "💳 THANH TOÁN", 
                Location = new System.Drawing.Point(700, 20), 
                Size = new System.Drawing.Size(160, 50),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += SaveHoaDon;

            pnlBot.Controls.Add(lblTongTien);
            pnlBot.Controls.Add(lblPTTT);
            pnlBot.Controls.Add(cboPhuongThucThanhToan);
            pnlBot.Controls.Add(btnSave);
            
            // Add controls in correct order with spacing
            Controls.Add(pnlBot);
            
            // Panel khoảng cách phía dưới
            var pnlSpacerBottom = new Panel 
            { 
                Dock = DockStyle.Bottom, 
                Height = 10, 
                BackColor = System.Drawing.Color.White 
            };
            Controls.Add(pnlSpacerBottom);
            
            Controls.Add(dgvChiTiet);
            
            // Panel khoảng cách phía trên
            var pnlSpacerTop = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = System.Drawing.Color.White 
            };
            Controls.Add(pnlSpacerTop);
            
            Controls.Add(pnlTop);
        }

        private void AddToCart(object sender, EventArgs e)
        {
            var selectedThuoc = cboThuoc.SelectedItem as Thuoc;
            if (selectedThuoc == null) return;

            int soLuongMua = (int)nudSoLuong.Value;
            
            // Kiểm tra tồn kho
            if (selectedThuoc.SoLuongTon < soLuongMua)
            {
                MessageBox.Show(
                    $"⚠ Không đủ hàng trong kho!\n\n" +
                    $"Thuốc: {selectedThuoc.TenThuoc}\n" +
                    $"Số lượng tồn: {selectedThuoc.SoLuongTon}\n" +
                    $"Số lượng muốn mua: {soLuongMua}\n\n" +
                    $"Vui lòng nhập số lượng nhỏ hơn hoặc bằng {selectedThuoc.SoLuongTon}",
                    "Không đủ hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Kiểm tra tổng số lượng trong giỏ hàng
            var existingItem = cart.FirstOrDefault(x => x.IdThuoc == selectedThuoc.IdThuoc);
            int tongSoLuongTrongGio = existingItem != null ? existingItem.SoLuong : 0;
            
            if (tongSoLuongTrongGio + soLuongMua > selectedThuoc.SoLuongTon)
            {
                MessageBox.Show(
                    $"⚠ Vượt quá số lượng tồn kho!\n\n" +
                    $"Thuốc: {selectedThuoc.TenThuoc}\n" +
                    $"Đã có trong giỏ: {tongSoLuongTrongGio}\n" +
                    $"Muốn thêm: {soLuongMua}\n" +
                    $"Tồn kho: {selectedThuoc.SoLuongTon}\n\n" +
                    $"Chỉ có thể thêm tối đa {selectedThuoc.SoLuongTon - tongSoLuongTrongGio} sản phẩm nữa",
                    "Vượt quá tồn kho",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var ct = new ChiTietHoaDon
            {
                IdThuoc = selectedThuoc.IdThuoc,
                TenThuoc = selectedThuoc.TenThuoc,
                SoLuong = soLuongMua,
                DonGia = selectedThuoc.DonGia
            };
            cart.Add(ct);
            RefreshCart();
        }

        private void RemoveFromCart()
        {
            if (dgvChiTiet.CurrentRow == null || dgvChiTiet.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Bạn muốn xóa mặt hàng này khỏi giỏ hàng?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                int index = dgvChiTiet.CurrentRow.Index;
                if (index >= 0 && index < cart.Count)
                {
                    cart.RemoveAt(index);
                    RefreshCart();
                }
            }
        }

        private void RefreshCart()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = cart;
            
            // Format columns after data binding
            FormatDataGridViewColumns();
            
            lblTongTien.Text = "Tổng tiền: " + cart.Sum(x => x.ThanhTien).ToString("N0") + " VNĐ";
        }

        private void FormatDataGridViewColumns()
        {
            if (dgvChiTiet.Columns.Count > 0)
            {
                // Hide IdThuoc column (first column)
                if (dgvChiTiet.Columns.Contains("IdThuoc"))
                    dgvChiTiet.Columns["IdThuoc"].Visible = false;
                
                // Format column headers
                if (dgvChiTiet.Columns.Contains("TenThuoc"))
                    dgvChiTiet.Columns["TenThuoc"].HeaderText = "Tên Thuốc";
                if (dgvChiTiet.Columns.Contains("SoLuong"))
                    dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                if (dgvChiTiet.Columns.Contains("DonGia"))
                {
                    dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                    dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                }
                if (dgvChiTiet.Columns.Contains("ThanhTien"))
                {
                    dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                    dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }
            }
        }

        private void SaveHoaDon(object sender, EventArgs e)
        {
            if (cart.Count == 0) 
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedKhachHang = cboKhachHang.SelectedItem as KhachHang;
            string idKH = selectedKhachHang?.IdKH ?? "KHLE";
            
            var hd = new HoaDon
            {
                IdNV = LoginForm.CurrentUser?.IdNV ?? "NV001",
                IdKH = idKH,
                ThoiGian = DateTime.Now,
                TongTien = cart.Sum(x => x.ThanhTien),
                TrangThaiDonHang = "HoanThanh",
                PhuongThucThanhToan = cboPhuongThucThanhToan.SelectedItem?.ToString() ?? "TienMat"
            };

            if (hdCtrl.Add(hd, cart, out string msg))
            {
                // Tích điểm cho khách hàng (nếu không phải khách lẻ)
                if (idKH != "KHLE" && selectedKhachHang != null)
                {
                    int diem = CalculateLoyaltyPoints(hd.TongTien);
                    khCtrl.CongDiem(idKH, diem);
                    MessageBox.Show($"✓ Thanh toán thành công!\n\nTổng tiền: {hd.TongTien:N0} VNĐ\nPhương thức: {hd.PhuongThucThanhToan}\n\n🎁 Khách hàng được cộng {diem} điểm!", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"✓ Thanh toán thành công!\n\nTổng tiền: {hd.TongTien:N0} VNĐ\nPhương thức: {hd.PhuongThucThanhToan}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lỗi: " + msg, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Hệ thống tích điểm lũy tiến
        private int CalculateLoyaltyPoints(double tongTien)
        {
            if (tongTien < 10000) return 2;
            else if (tongTien < 50000) return 5;
            else if (tongTien < 100000) return 10;
            else if (tongTien < 500000) return 20;
            else return 50;
        }
    }
}