using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class HopDongPanel : UserControl
    {
        private DataGridView dgv;
        private Button btnReminder;
        private HopDongController controller = new HopDongController();
        
        public HopDongPanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(10);
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // Panel top với button
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White, Padding = new Padding(5) };
            
            var btnRenew = new Button 
            { 
                Text = "📝 Gia hạn hợp đồng", 
                Location = new Point(10, 12), 
                Size = new Size(170, 36), 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRenew.FlatAppearance.BorderSize = 0;
            btnRenew.Click += (s, e) => {
                if (new AddHopDongDialog().ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            };
            pnlTop.Controls.Add(btnRenew);

            btnReminder = new Button 
            { 
                Text = "🔔 Nhắc nhở gia hạn", 
                Location = new Point(190, 12), 
                Size = new Size(170, 36), 
                BackColor = Color.FromArgb(230, 126, 34), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReminder.FlatAppearance.BorderSize = 0;
            btnReminder.Click += BtnReminder_Click;
            pnlTop.Controls.Add(btnReminder);
            
            var btnRefresh = new Button 
            { 
                Text = "🔄 Làm mới", 
                Location = new Point(370, 12), 
                Size = new Size(120, 36), 
                BackColor = Color.FromArgb(52, 152, 219), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            pnlTop.Controls.Add(btnRefresh);

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
            
            // Double-click để xem chi tiết
            dgv.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0 && dgv.Rows[e.RowIndex].Cells["IdHD"].Value != null)
                {
                    string idHD = dgv.Rows[e.RowIndex].Cells["IdHD"].Value.ToString() ?? "";
                    new ViewHopDongDialog(idHD).ShowDialog();
                }
            };

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
            LoadData();
        }

        private void BtnReminder_Click(object sender, EventArgs e)
        {
            var allContracts = controller.GetAllHopDong();
            var expiringContracts = allContracts.Where(hd => 
                hd.NgayKetThuc.HasValue && 
                (hd.NgayKetThuc.Value - DateTime.Now).TotalDays <= 30 &&
                (hd.NgayKetThuc.Value - DateTime.Now).TotalDays >= 0
            ).ToList();

            if (expiringContracts.Count == 0)
            {
                MessageBox.Show("✓ Không có hợp đồng nào cần gia hạn trong 30 ngày tới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var msg = $"⚠ Có {expiringContracts.Count} hợp đồng sắp hết hạn trong 30 ngày tới:\n\n";
                foreach (var hd in expiringContracts.Take(5))
                {
                    var daysLeft = (hd.NgayKetThuc.Value - DateTime.Now).Days;
                    msg += $"• {hd.IdHD} - Còn {daysLeft} ngày ({hd.NgayKetThuc:dd/MM/yyyy})\n";
                }
                if (expiringContracts.Count > 5)
                    msg += $"\n...và {expiringContracts.Count - 5} hợp đồng khác";
                    
                MessageBox.Show(msg, "Nhắc nhở gia hạn hợp đồng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadData()
        {
            var data = controller.GetAllHopDong();
            
            // Tạo anonymous type để hiển thị với cột hiệu lực
            var displayData = data.Select(hd => new
            {
                IdHD = hd.IdHD,
                NgayBatDau = hd.NgayBatDau,
                NgayKetThuc = hd.NgayKetThuc,
                IdNV = hd.IdNV,
                IdNCC = hd.IdNCC,
                HieuLuc = GetContractStatus(hd)
            }).ToList();
            
            dgv.DataSource = displayData;
            
            // Tùy chỉnh header
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["IdHD"].HeaderText = "Mã HĐ";
                dgv.Columns["NgayBatDau"].HeaderText = "Ngày Bắt Đầu";
                dgv.Columns["NgayKetThuc"].HeaderText = "Ngày Kết Thúc";
                dgv.Columns["IdNV"].HeaderText = "Mã NV";
                dgv.Columns["IdNCC"].HeaderText = "Mã NCC";
                dgv.Columns["HieuLuc"].HeaderText = "Hiệu Lực";
                
                // Format date columns
                dgv.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgv.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";
                
                // Màu cho cột hiệu lực
                dgv.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == dgv.Columns["HieuLuc"].Index && e.Value != null)
                    {
                        string status = e.Value.ToString() ?? "";
                        if (status.Contains("Hết hạn"))
                            e.CellStyle.ForeColor = Color.Red;
                        else if (status.Contains("Sắp hết"))
                            e.CellStyle.ForeColor = Color.Orange;
                        else if (status.Contains("Còn hiệu lực"))
                            e.CellStyle.ForeColor = Color.Green;
                    }
                };
            }
        }
        
        private string GetContractStatus(HopDong hd)
        {
            if (!hd.NgayKetThuc.HasValue)
                return "Không xác định";
                
            var daysLeft = (hd.NgayKetThuc.Value - DateTime.Now).TotalDays;
            
            if (daysLeft < 0)
                return "Hết hạn";
            else if (daysLeft <= 30)
                return $"Sắp hết hạn ({(int)daysLeft} ngày)";
            else
                return $"Còn hiệu lực ({(int)daysLeft} ngày)";
        }
    }
}