using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QLThuocApp.connectDB;

namespace QLThuocApp.dao
{
    public class ReportDAO
    {
        /// <summary>
        /// Lấy dữ liệu báo cáo doanh thu theo khoảng thời gian
        /// </summary>
        public RevenueReport GetRevenueReport(DateTime startDate, DateTime endDate)
        {
            var report = new RevenueReport
            {
                StartDate = startDate,
                EndDate = endDate
            };

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();

                // Lấy tổng doanh thu (tổng tiền các hóa đơn)
                string sqlRevenue = @"SELECT COALESCE(SUM(tong_tien), 0) as total
                                     FROM hoadon 
                                     WHERE thoi_gian BETWEEN @start AND @end
                                     AND trang_thai_don_hang NOT IN ('Đã hủy', 'Hủy')";
                
                using (var cmd = new MySqlCommand(sqlRevenue, conn))
                {
                    cmd.Parameters.AddWithValue("@start", startDate);
                    cmd.Parameters.AddWithValue("@end", endDate);
                    var result = cmd.ExecuteScalar();
                    report.TotalRevenue = Convert.ToDouble(result ?? 0);
                }

                // Lấy tổng chi phí (tính từ giá nhập của thuốc đã bán)
                // Chi phí = tổng (số lượng thuốc bán * giá nhập của thuốc đó)
                string sqlCost = @"SELECT COALESCE(SUM(ct.so_luong * t.gia_nhap), 0) as total
                                  FROM chitiethoadon ct
                                  INNER JOIN hoadon hd ON ct.hoa_don_id = hd.id
                                  INNER JOIN thuoc t ON ct.thuoc_id = t.id
                                  WHERE hd.thoi_gian BETWEEN @start AND @end
                                  AND hd.trang_thai_don_hang NOT IN ('Đã hủy', 'Hủy')";
                
                using (var cmd = new MySqlCommand(sqlCost, conn))
                {
                    cmd.Parameters.AddWithValue("@start", startDate);
                    cmd.Parameters.AddWithValue("@end", endDate);
                    var result = cmd.ExecuteScalar();
                    report.TotalCost = Convert.ToDouble(result ?? 0);
                }

                // Lấy số lượng đơn hàng
                string sqlOrderCount = @"SELECT COUNT(*) as total
                                        FROM hoadon 
                                        WHERE thoi_gian BETWEEN @start AND @end
                                        AND trang_thai_don_hang NOT IN ('Đã hủy', 'Hủy')";
                
                using (var cmd = new MySqlCommand(sqlOrderCount, conn))
                {
                    cmd.Parameters.AddWithValue("@start", startDate);
                    cmd.Parameters.AddWithValue("@end", endDate);
                    report.TotalOrders = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Tính lợi nhuận
                report.TotalProfit = report.TotalRevenue - report.TotalCost;
            }

            return report;
        }

        /// <summary>
        /// Lấy dữ liệu doanh thu theo từng ngày trong khoảng thời gian
        /// </summary>
        public List<DailyRevenue> GetDailyRevenue(DateTime startDate, DateTime endDate)
        {
            var list = new List<DailyRevenue>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT DATE(h.thoi_gian) as date, 
                                     COALESCE(SUM(h.tong_tien), 0) as revenue,
                                     COUNT(DISTINCT h.id) as orders,
                                     COALESCE(SUM(ct.so_luong * t.gia_nhap), 0) as cost
                              FROM hoadon h
                              LEFT JOIN chitiethoadon ct ON h.id = ct.hoa_don_id
                              LEFT JOIN thuoc t ON ct.thuoc_id = t.id
                              WHERE h.thoi_gian BETWEEN @start AND @end
                              AND h.trang_thai_don_hang NOT IN ('Đã hủy', 'Hủy')
                              GROUP BY DATE(h.thoi_gian)
                              ORDER BY DATE(h.thoi_gian)";
                
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@start", startDate);
                    cmd.Parameters.AddWithValue("@end", endDate);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double revenue = Convert.ToDouble(reader["revenue"]);
                            double cost = Convert.ToDouble(reader["cost"]);
                            list.Add(new DailyRevenue
                            {
                                Date = Convert.ToDateTime(reader["date"]),
                                Revenue = revenue,
                                Cost = cost,
                                Profit = revenue - cost,
                                OrderCount = Convert.ToInt32(reader["orders"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy thống kê đánh giá phản hồi
        /// </summary>
        public FeedbackStatistics GetFeedbackStatistics()
        {
            var stats = new FeedbackStatistics();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();

                // Lấy tổng số phản hồi và điểm trung bình
                string sql = @"SELECT 
                                COUNT(*) as total_count,
                                COALESCE(AVG(danh_gia), 0) as avg_rating,
                                SUM(CASE WHEN danh_gia = 5 THEN 1 ELSE 0 END) as rating_5,
                                SUM(CASE WHEN danh_gia = 4 THEN 1 ELSE 0 END) as rating_4,
                                SUM(CASE WHEN danh_gia = 3 THEN 1 ELSE 0 END) as rating_3,
                                SUM(CASE WHEN danh_gia = 2 THEN 1 ELSE 0 END) as rating_2,
                                SUM(CASE WHEN danh_gia = 1 THEN 1 ELSE 0 END) as rating_1
                              FROM phanhoi";
                
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        stats.TotalFeedbacks = Convert.ToInt32(reader["total_count"]);
                        stats.AverageRating = Convert.ToDouble(reader["avg_rating"]);
                        stats.Rating5Count = Convert.ToInt32(reader["rating_5"]);
                        stats.Rating4Count = Convert.ToInt32(reader["rating_4"]);
                        stats.Rating3Count = Convert.ToInt32(reader["rating_3"]);
                        stats.Rating2Count = Convert.ToInt32(reader["rating_2"]);
                        stats.Rating1Count = Convert.ToInt32(reader["rating_1"]);
                    }
                }
            }

            return stats;
        }
    }

    // Classes để lưu dữ liệu báo cáo
    public class RevenueReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalCost { get; set; }
        public double TotalProfit { get; set; }
        public int TotalOrders { get; set; }
    }

    public class DailyRevenue
    {
        public DateTime Date { get; set; }
        public double Revenue { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }
        public int OrderCount { get; set; }
    }

    public class FeedbackStatistics
    {
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public int Rating5Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating1Count { get; set; }
    }
}
