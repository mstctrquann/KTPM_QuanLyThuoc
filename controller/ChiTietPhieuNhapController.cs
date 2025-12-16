using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class ChiTietPhieuNhapController
    {
        private readonly ChiTietPhieuNhapDAO ctDAO;

        public ChiTietPhieuNhapController()
        {
            ctDAO = new ChiTietPhieuNhapDAO();
        }

        public List<ChiTietPhieuNhap> GetByMaPhieuNhap(string idPN)
        {
            // Tên hàm trong DAO là GetByMaPhieuNhap hoặc GetByIdPN
            return ctDAO.GetByMaPhieuNhap(idPN);
        }
    }
}