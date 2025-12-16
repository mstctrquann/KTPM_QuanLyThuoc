using System;
using System.Globalization;

namespace QLThuocApp.ultis
{
    public static class DateHelper
    {
        // Định dạng chuẩn hiển thị trên UI
        public static string DateFormat = "dd/MM/yyyy";
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public static string ToDateString(DateTime dt)
        {
            return dt.ToString(DateFormat);
        }

        public static string ToDateTimeString(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }

        // Parse chuỗi ngày từ UI để lưu xuống DB hoặc xử lý
        public static DateTime ParseDate(string dateStr)
        {
            if (DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            return DateTime.MinValue; // Trả về min nếu lỗi
        }
    }
}