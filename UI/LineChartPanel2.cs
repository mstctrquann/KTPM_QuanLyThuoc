using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLThuocApp.UI
{
    public class LineChartPanel2 : UserControl
    {
        private Chart chart;

        public LineChartPanel2()
        {
            Dock = DockStyle.Fill;
            chart = new Chart { Dock = DockStyle.Fill };
            
            var area = new ChartArea("MainArea");
            // Tùy chỉnh giao diện khác đi một chút so với LineChartPanel
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.BackColor = Color.WhiteSmoke;
            chart.ChartAreas.Add(area);

            var series = new Series("DataSeries")
            {
                // Sử dụng biểu đồ Cột (Column) để khác biệt
                ChartType = SeriesChartType.Column, 
                Color = Color.Teal,
                IsValueShownAsLabel = true
            };
            chart.Series.Add(series);

            chart.Titles.Add(new Title("Biểu đồ Thống kê (Dạng cột)", Docking.Top, new Font("Arial", 12, FontStyle.Bold), Color.Black));
            Controls.Add(chart);
        }

        public void LoadData(Dictionary<string, int> data, string titleX, string titleY)
        {
            chart.Series["DataSeries"].Points.Clear();
            chart.ChartAreas[0].AxisX.Title = titleX;
            chart.ChartAreas[0].AxisY.Title = titleY;

            foreach (var item in data)
            {
                chart.Series["DataSeries"].Points.AddXY(item.Key, item.Value);
            }
        }
    }
}