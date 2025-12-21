using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.dao;

namespace QLThuocApp.UI
{
    public class RevenueReportForm : Form
    {
        private ComboBox cboPeriod;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Button btnGenerate;
        private DataGridView dgvReport;
        private Label lblTotalRevenue;
        private Label lblTotalCost;
        private Label lblProfit;
        private Label lblOrderCount;

        public RevenueReportForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo Cáo Doanh Thu";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Period selection panel
            Panel pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(10)
            };

            Label lblPeriod = new Label
            {
                Text = "Kỳ báo cáo:",
                Location = new Point(10, 15),
                Size = new Size(100, 25)
            };

            cboPeriod = new ComboBox
            {
                Location = new Point(110, 12),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPeriod.Items.AddRange(new object[] { "1 Tuần", "1 Tháng", "3 Tháng", "1 Năm", "Tùy chỉnh" });
            cboPeriod.SelectedIndex = 1;
            cboPeriod.SelectedIndexChanged += CboPeriod_SelectedIndexChanged;

            Label lblStartDate = new Label
            {
                Text = "Từ ngày:",
                Location = new Point(280, 15),
                Size = new Size(60, 25)
            };

            dtpStartDate = new DateTimePicker
            {
                Location = new Point(345, 12),
                Size = new Size(150, 25),
                Enabled = false
            };

            Label lblEndDate = new Label
            {
                Text = "Đến ngày:",
                Location = new Point(510, 15),
                Size = new Size(70, 25)
            };

            dtpEndDate = new DateTimePicker
            {
                Location = new Point(585, 12),
                Size = new Size(150, 25),
                Enabled = false
            };

            btnGenerate = new Button
            {
                Text = "📊 Tạo Báo Cáo",
                Location = new Point(750, 12),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerate.Click += BtnGenerate_Click;

            pnlTop.Controls.AddRange(new Control[] { lblPeriod, cboPeriod, lblStartDate, dtpStartDate, lblEndDate, dtpEndDate, btnGenerate });

            // Summary panel
            Panel pnlSummary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(236, 240, 241)
            };

            Panel pnlRevenue = CreateSummaryPanel("Tổng Doanh Thu", "0 VNĐ", Color.FromArgb(46, 204, 113), 10);
            lblTotalRevenue = (Label)pnlRevenue.Controls[1];

            Panel pnlCost = CreateSummaryPanel("Tổng Chi Phí", "0 VNĐ", Color.FromArgb(231, 76, 60), 260);
            lblTotalCost = (Label)pnlCost.Controls[1];

            Panel pnlProfitPanel = CreateSummaryPanel("Lợi Nhuận", "0 VNĐ", Color.FromArgb(52, 152, 219), 510);
            lblProfit = (Label)pnlProfitPanel.Controls[1];

            Panel pnlOrderCountPanel = CreateSummaryPanel("Số Đơn Hàng", "0", Color.FromArgb(155, 89, 182), 760);
            lblOrderCount = (Label)pnlOrderCountPanel.Controls[1];

            pnlSummary.Controls.AddRange(new Control[] { pnlRevenue, pnlCost, pnlProfitPanel, pnlOrderCountPanel });

            // DataGridView
            dgvReport = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            dgvReport.Columns.Add("Date", "Ngày");
            dgvReport.Columns.Add("OrderCount", "Số Đơn");
            dgvReport.Columns.Add("Revenue", "Doanh Thu (VNĐ)");
            dgvReport.Columns.Add("Cost", "Chi Phí (VNĐ)");
            dgvReport.Columns.Add("Profit", "Lợi Nhuận (VNĐ)");

            // Format number columns
            dgvReport.Columns["OrderCount"].DefaultCellStyle.Format = "N0";
            dgvReport.Columns["Revenue"].DefaultCellStyle.Format = "N0";
            dgvReport.Columns["Cost"].DefaultCellStyle.Format = "N0";
            dgvReport.Columns["Profit"].DefaultCellStyle.Format = "N0";
            dgvReport.Columns["Revenue"].DefaultCellStyle.ForeColor = Color.Green;
            dgvReport.Columns["Cost"].DefaultCellStyle.ForeColor = Color.Red;
            dgvReport.Columns["Profit"].DefaultCellStyle.ForeColor = Color.Blue;

            this.Controls.Add(dgvReport);
            this.Controls.Add(pnlSummary);
            this.Controls.Add(pnlTop);
        }

        private Panel CreateSummaryPanel(string title, string value, Color color, int x)
        {
            Panel panel = new Panel
            {
                Location = new Point(x, 10),
                Size = new Size(230, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(10, 10),
                Size = new Size(210, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(10, 40),
                Size = new Size(210, 40),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblValue });
            return panel;
        }

        private void CboPeriod_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool customPeriod = cboPeriod.SelectedIndex == 4; // "Tùy chỉnh"
            dtpStartDate.Enabled = customPeriod;
            dtpEndDate.Enabled = customPeriod;

            if (!customPeriod)
            {
                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate;

                switch (cboPeriod.SelectedIndex)
                {
                    case 0: // 1 Tuần
                        startDate = endDate.AddDays(-7);
                        break;
                    case 1: // 1 Tháng
                        startDate = endDate.AddMonths(-1);
                        break;
                    case 2: // 3 Tháng
                        startDate = endDate.AddMonths(-3);
                        break;
                    case 3: // 1 Năm
                        startDate = endDate.AddYears(-1);
                        break;
                }

                dtpStartDate.Value = startDate;
                dtpEndDate.Value = endDate;
            }
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dtpStartDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                // Get report data
                var reportDAO = new ReportDAO();
                var report = reportDAO.GetRevenueReport(startDate, endDate);
                var dailyData = reportDAO.GetDailyRevenue(startDate, endDate);

                // Update summary
                lblTotalRevenue.Text = $"{report.TotalRevenue:N0} VNĐ";
                lblTotalCost.Text = $"{report.TotalCost:N0} VNĐ";
                lblProfit.Text = $"{report.TotalProfit:N0} VNĐ";
                lblOrderCount.Text = $"{report.TotalOrders:N0}";

                // Update profit color
                lblProfit.ForeColor = report.TotalProfit >= 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);

                // Update DataGridView
                dgvReport.Rows.Clear();
                foreach (var day in dailyData)
                {
                    dgvReport.Rows.Add(
                        day.Date.ToString("dd/MM/yyyy"),
                        day.OrderCount,
                        day.Revenue,
                        day.Cost,
                        day.Profit
                    );
                }

                MessageBox.Show("Tạo báo cáo thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
