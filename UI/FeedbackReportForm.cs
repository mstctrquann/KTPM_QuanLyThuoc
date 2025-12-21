using System;
using System.Drawing;
using System.Windows.Forms;
using QLThuocApp.dao;

namespace QLThuocApp.UI
{
    public class FeedbackReportForm : Form
    {
        private Label lblAverageRating;
        private Label lblTotalFeedbacks;
        private DataGridView dgvFeedbacks;
        private DataGridView dgvRatingStats;

        public FeedbackReportForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo Cáo Đánh Giá Phản Hồi";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Summary panel
            Panel pnlSummary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(236, 240, 241)
            };

            Panel pnlAvgRating = CreateSummaryPanel("Đánh Giá Trung Bình", "0.0 ⭐", Color.FromArgb(241, 196, 15), 150);
            lblAverageRating = (Label)pnlAvgRating.Controls[1];

            Panel pnlTotal = CreateSummaryPanel("Tổng Phản Hồi", "0", Color.FromArgb(52, 152, 219), 500);
            lblTotalFeedbacks = (Label)pnlTotal.Controls[1];

            pnlSummary.Controls.AddRange(new Control[] { pnlAvgRating, pnlTotal });

            // Rating statistics panel
            GroupBox grpStats = new GroupBox
            {
                Text = "📊 Thống Kê Theo Số Sao",
                Dock = DockStyle.Top,
                Height = 250,
                Padding = new Padding(10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvRatingStats = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 35 }
            };

            dgvRatingStats.Columns.Add("Rating", "Đánh Giá");
            dgvRatingStats.Columns.Add("Count", "Số Lượng");
            dgvRatingStats.Columns.Add("Percentage", "Tỷ Lệ (%)");

            dgvRatingStats.Columns["Count"].DefaultCellStyle.Format = "N0";
            dgvRatingStats.Columns["Percentage"].DefaultCellStyle.Format = "N2";

            grpStats.Controls.Add(dgvRatingStats);

            // Recent feedbacks
            GroupBox grpFeedbacks = new GroupBox
            {
                Text = "💬 Phản Hồi Gần Đây (20 mới nhất)",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvFeedbacks = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            dgvFeedbacks.Columns.Add("Date", "Ngày");
            dgvFeedbacks.Columns.Add("Customer", "Khách Hàng");
            dgvFeedbacks.Columns.Add("Rating", "Đánh Giá");
            dgvFeedbacks.Columns.Add("Comment", "Nội Dung");

            dgvFeedbacks.Columns["Date"].Width = 120;
            dgvFeedbacks.Columns["Customer"].Width = 150;
            dgvFeedbacks.Columns["Rating"].Width = 100;
            dgvFeedbacks.Columns["Comment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grpFeedbacks.Controls.Add(dgvFeedbacks);

            this.Controls.Add(grpFeedbacks);
            this.Controls.Add(grpStats);
            this.Controls.Add(pnlSummary);
        }

        private Panel CreateSummaryPanel(string title, string value, Color color, int x)
        {
            Panel panel = new Panel
            {
                Location = new Point(x, 10),
                Size = new Size(280, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(10, 10),
                Size = new Size(260, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(10, 40),
                Size = new Size(260, 40),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblValue });
            return panel;
        }

        private void LoadData()
        {
            try
            {
                var reportDAO = new ReportDAO();
                var stats = reportDAO.GetFeedbackStatistics();

                // Update summary from stats object
                lblAverageRating.Text = $"{stats.AverageRating:F1} ⭐";
                lblTotalFeedbacks.Text = $"{stats.TotalFeedbacks:N0}";

                // Create array of rating data
                var ratingData = new[]
                {
                    new { Rating = 5, Count = stats.Rating5Count },
                    new { Rating = 4, Count = stats.Rating4Count },
                    new { Rating = 3, Count = stats.Rating3Count },
                    new { Rating = 2, Count = stats.Rating2Count },
                    new { Rating = 1, Count = stats.Rating1Count }
                };

                // Update rating statistics with colors
                dgvRatingStats.Rows.Clear();
                foreach (var item in ratingData)
                {
                    double percentage = stats.TotalFeedbacks > 0 ? (double)item.Count / stats.TotalFeedbacks * 100 : 0;
                    int rowIndex = dgvRatingStats.Rows.Add(
                        $"{item.Rating} ⭐",
                        item.Count,
                        percentage
                    );

                    // Color code the rows
                    Color rowColor = item.Rating switch
                    {
                        5 => Color.FromArgb(46, 204, 113),  // Green
                        4 => Color.FromArgb(52, 152, 219),  // Blue
                        3 => Color.FromArgb(241, 196, 15),  // Yellow
                        2 => Color.FromArgb(230, 126, 34),  // Orange
                        1 => Color.FromArgb(231, 76, 60),   // Red
                        _ => Color.Gray
                    };

                    dgvRatingStats.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, rowColor.R, rowColor.G, rowColor.B);
                    dgvRatingStats.Rows[rowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }

                // Load recent feedbacks
                var phanHoiDAO = new PhanHoiDAO();
                var feedbacks = phanHoiDAO.GetAll();
                feedbacks.Sort((a, b) => b.ThoiGian.CompareTo(a.ThoiGian));

                dgvFeedbacks.Rows.Clear();
                int count = 0;
                foreach (var feedback in feedbacks)
                {
                    if (count >= 20) break;
                    
                    dgvFeedbacks.Rows.Add(
                        feedback.ThoiGian.ToString("dd/MM/yyyy HH:mm"),
                        feedback.TenKH ?? "N/A",
                        $"{feedback.DanhGia} ⭐",
                        feedback.NoiDung ?? ""
                    );
                    count++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
