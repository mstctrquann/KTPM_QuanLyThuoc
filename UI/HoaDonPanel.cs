using System;
using System.Windows.Forms;
using System.Drawing;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class HoaDonPanel : UserControl
    {
        private DataGridView dgv;
        private HoaDonController controller = new HoaDonController();

        public HoaDonPanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(10);
            
            // DataGridView - Thêm TRƯỚC để nó nằm dưới
            dgv = new DataGridView 
            { 
                Dock = DockStyle.Fill, 
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9F),
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgv.DoubleClick += (s, e) => {
                if(dgv.CurrentRow != null)
                {
                    string id = dgv.CurrentRow.Cells["IdHD"].Value.ToString();
                    new ViewHoaDonDialog(id).ShowDialog();
                }
            };
            Controls.Add(dgv);

            // Panel ngăn cách với border
            var pnlSeparator = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 1, 
                BackColor = Color.FromArgb(189, 195, 199)
            };
            Controls.Add(pnlSeparator);

            // Panel khoảng cách
            var pnlSpacer = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 10, 
                BackColor = Color.White 
            };
            Controls.Add(pnlSpacer);

            // Panel chứa các nút - Thêm SAU để nó nằm trên
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White, Padding = new Padding(0, 10, 0, 10) };
            
            var btnAdd = new Button 
            { 
                Text = "➕ Tạo Hóa Đơn Mới", 
                Location = new Point(0, 10),
                Size = new Size(180, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => { 
                if(new AddHoaDonDialog().ShowDialog() == DialogResult.OK) LoadData(); 
            };
            panelTop.Controls.Add(btnAdd);
            
            var btnRefresh = new Button 
            { 
                Text = "🔄 Làm mới", 
                Location = new Point(190, 10),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            panelTop.Controls.Add(btnRefresh);

            // Nút Báo Cáo - Chỉ hiển thị cho Admin và Manager
            var roleId = LoginForm.CurrentUser?.IdVT;
            if (roleId == "1" || roleId == "2") // Admin hoặc Manager
            {
                var btnReport = new Button 
                { 
                    Text = "📊 Báo Cáo Doanh Thu", 
                    Location = new Point(320, 10),
                    Size = new Size(180, 40),
                    BackColor = Color.FromArgb(155, 89, 182),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnReport.FlatAppearance.BorderSize = 0;
                btnReport.Click += (s, e) => { 
                    var revenueForm = new RevenueReportForm();
                    revenueForm.ShowDialog();
                };
                panelTop.Controls.Add(btnReport);
            }
            
            Controls.Add(panelTop);

            LoadData();
        }
        private void LoadData() => dgv.DataSource = controller.GetAll();
    }
}