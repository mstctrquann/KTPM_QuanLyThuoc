using System.Text.RegularExpressions;

namespace QLThuocApp.ultis
{
    public static class Validator
    {
        // Kiểm tra số điện thoại VN (10 số, bắt đầu bằng 0)
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^0\d{9}$");
        }

        // Kiểm tra email
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try 
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        // Kiểm tra số nguyên dương (cho số lượng)
        public static bool IsPositiveInteger(string input)
        {
            return int.TryParse(input, out int val) && val > 0;
        }

        // Kiểm tra số thực dương (cho giá tiền)
        public static bool IsPositiveDecimal(string input)
        {
            return decimal.TryParse(input, out decimal val) && val >= 0;
        }
    }
}