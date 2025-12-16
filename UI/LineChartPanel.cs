using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLThuocApp.UI
{
    public class LineChartPanel : UserControl
    {
        private Chart chart;

        public LineChartPanel()
        {
            Dock = DockStyle.Fill;
            chart = new Chart { Dock = DockStyle.Fill };
            
            // Tạo ChartArea
            var area = new ChartArea("MainArea");
            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            chart.ChartAreas.Add(area);

            // Tạo Series
            var series = new Series("DoanhThu")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.Blue,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                IsValueShownAsLabel = true
            };
            chart.Series.Add(series);

            // Title
            chart.Titles.Add(new Title("Biểu đồ Doanh Thu", Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.Black));

            Controls.Add(chart);
        }

        public void LoadData(Dictionary<string, int> data, string titleX, string titleY)
        {
            chart.Series["DoanhThu"].Points.Clear();
            chart.ChartAreas[0].AxisX.Title = titleX;
            chart.ChartAreas[0].AxisY.Title = titleY;

            foreach (var item in data)
            {
                chart.Series["DoanhThu"].Points.AddXY(item.Key, item.Value);
            }
        }
    }
}