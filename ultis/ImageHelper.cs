using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace QLThuocApp.ultis
{
    public static class ImageHelper
    {
        // Chuyển Image sang byte[] để lưu DB
        public static byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png); // Mặc định lưu PNG
                return ms.ToArray();
            }
        }

        // Chuyển byte[] từ DB sang Image để hiển thị
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
            using (var ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }
    }
}