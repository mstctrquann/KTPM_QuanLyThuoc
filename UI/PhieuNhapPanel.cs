using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class PhieuNhapPanel : UserControl
    {
        private DataGridView dgv = null!;
        private PhieuNhapController controller = new PhieuNhapController();

        public PhieuNhapPanel()
        {
            Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // Panel chứa nút chức năng
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.WhiteSmoke, Padding = new Padding(5) };
            
            var btnAdd = new Button 
            { 
                Text = "NHẬP HÀNG MỚI", 
                Location = new Point(10, 10), 
                Width = 150, 
                Height = 35, 
                BackColor = Color.ForestGreen, 
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnAdd.Click += (s, e) => {
                // Mở form thêm phiếu nhập, nếu thêm thành công (DialogResult.OK) thì tải lại danh sách
                if (new AddPhieuNhapDialog().ShowDialog() == DialogResult.OK) 
                    LoadData();
            };
            pnlTop.Controls.Add(btnAdd);

            var btnRefresh = new Button { Text = "Làm mới", Location = new Point(170, 10), Height = 35 };
            btnRefresh.Click += (s, e) => LoadData();
            pnlTop.Controls.Add(btnRefresh);

            // Grid hiển thị dữ liệu - THÊM TRƯỚC
            dgv = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            
            // Sự kiện xem chi tiết khi click đúp
            dgv.DoubleClick += (s, e) => {
                if (dgv.CurrentRow != null)
                {
                    string idPN = dgv.CurrentRow.Cells["IdPN"].Value?.ToString() ?? "";
                    new ViewPhieuNhapDialog(idPN).ShowDialog();
                }
            };

            // Sự kiện click chuột phải để Sửa/Xóa
            dgv.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                {
                    int currentMouseOverRow = dgv.HitTest(e.X, e.Y).RowIndex;
                    if (currentMouseOverRow >= 0)
                    {
                        dgv.ClearSelection();
                        dgv.Rows[currentMouseOverRow].Selected = true;
                        
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("Xem chi tiết").Click += (ss, ee) => {
                            new ViewPhieuNhapDialog(dgv.Rows[currentMouseOverRow].Cells["IdPN"].Value?.ToString() ?? "").ShowDialog();
                        };
                        m.Items.Add("Sửa phiếu nhập").Click += (ss, ee) => {
                             // Lấy object PhieuNhap từ dòng hiện tại (cần ép kiểu nếu DataSource là List<PhieuNhap>)
                             // Ở đây ta dùng Controller lấy lại từ DB cho chắc chắn
                             string id = dgv.Rows[currentMouseOverRow].Cells["IdPN"].Value?.ToString() ?? "";
                             // TODO: Bạn cần thêm hàm GetById vào PhieuNhapController nếu chưa có
                             // var pn = controller.GetById(id); 
                             // new EditPhieuNhapDialog(pn).ShowDialog();
                             MessageBox.Show("Chức năng sửa đang được hoàn thiện."); 
                        };
                        m.Show(dgv, new Point(e.X, e.Y));
                    }
                }
            };

            Controls.Add(dgv);
        }

        private void LoadData()
        {
            dgv.DataSource = null;
            dgv.DataSource = controller.GetAllPhieuNhap();
            
            // Format cột tiền tệ và ngày tháng
            if (dgv.Columns["TongTien"] != null) 
                dgv.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            if (dgv.Columns["ThoiGian"] != null) 
                dgv.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }
    }
}