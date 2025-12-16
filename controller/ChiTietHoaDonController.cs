using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class ChiTietHoaDonController
    {
        private ChiTietHoaDonDAO dao = new ChiTietHoaDonDAO();

        public List<ChiTietHoaDon> GetChiTietByMaHD(string maHD)
        {
            return dao.GetByMaHD(maHD);
        }
    }
}
