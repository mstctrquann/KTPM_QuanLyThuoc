using System;
using System.Collections.Generic;
using System.Linq;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class HoaDonController
    {
        private HoaDonDAO hdDao = new HoaDonDAO();
        private ChiTietHoaDonDAO ctDao = new ChiTietHoaDonDAO();

        public List<HoaDon> GetAll() => hdDao.GetAll();

        public bool Add(HoaDon hd, List<ChiTietHoaDon> details, out string msg)
        {
            msg = "";
            if (details.Count == 0) { msg = "Chưa chọn thuốc"; return false; }
            
            // Tự sinh mã HD nếu chưa có
            if(string.IsNullOrEmpty(hd.IdHD)) hd.IdHD = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                if (hdDao.InsertWithDetails(hd, details)) return true;
                msg = "Lưu thất bại"; return false;
            }
            catch (Exception ex) { msg = ex.Message; return false; }
        }

        public List<ChiTietHoaDon> GetChiTiet(string idHD)
        {
            return ctDao.GetByMaHD(idHD);
        }

        public bool Delete(string idHD, out string msg)
        {
            msg = "";
            try
            {
                if (hdDao.Delete(idHD)) return true;
                msg = "Xóa thất bại"; return false;
            }
            catch (Exception ex) { msg = ex.Message; return false; }
        }
        public List<HoaDon> GetDeletedList()
        {
            return new HoaDonDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new HoaDonDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new HoaDonDAO().DeleteForever(id);
        }
        
        public List<HoaDon> GetHoaDonByKhachHang(string idKH)
        {
            return hdDao.GetByKhachHang(idKH);
        }
        
    }
}