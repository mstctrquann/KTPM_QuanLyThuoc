using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class PhanHoiController
    {
        private readonly PhanHoiDAO phanHoiDAO;
        private readonly HoaDonDAO hoaDonDAO; // Cần check hóa đơn cho Guest

        public PhanHoiController()
        {
            phanHoiDAO = new PhanHoiDAO();
            hoaDonDAO = new HoaDonDAO();
        }

        public List<PhanHoi> GetAllPhanHoi()
        {
            return phanHoiDAO.GetAll();
        }

        public List<PhanHoi> SearchPhanHoi(string idPH, string idKH)
        {
            return phanHoiDAO.Search(idPH, idKH);
        }

        public bool DeletePhanHoi(string idPH, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return phanHoiDAO.Delete(idPH);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        // Logic thêm phản hồi cho Khách (từ GuestFeedbackForm)
        public bool AddPhanHoiGuest(string idHD, string sdt, string noiDung, int danhGia, out string errorMsg)
        {
            errorMsg = string.Empty;
            
            // 1. Kiểm tra hóa đơn tồn tại
            if (!hoaDonDAO.Exists(idHD))
            {
                errorMsg = "Mã hóa đơn không tồn tại.";
                return false;
            }

            // 2. Lấy ID Khách hàng từ hóa đơn đó
            string idKH = hoaDonDAO.GetKhachHangIdByHoaDonId(idHD);
            if (string.IsNullOrEmpty(idKH))
            {
                errorMsg = "Không tìm thấy thông tin khách hàng của hóa đơn này.";
                return false;
            }

            // 3. Tạo ID Phản hồi tự động (Giả lập: PH + Ticks)
            string idPH = "PH" + DateTime.Now.Ticks.ToString().Substring(12);

            var ph = new PhanHoi
            {
                IdPH = idPH,
                IdKH = idKH,
                IdHD = idHD,
                NoiDung = noiDung,
                ThoiGian = DateTime.Now,
                DanhGia = danhGia,
                IsDeleted = false
            };

            try
            {
                return phanHoiDAO.Insert(ph);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        
        // Add thường (cho Admin/Staff nhập)
        public bool AddPhanHoi(PhanHoi ph, out string errorMsg) {
             errorMsg = "";
             try { return phanHoiDAO.Insert(ph); }
             catch(Exception ex) { errorMsg = ex.Message; return false; }
        }
        
        public bool UpdatePhanHoi(PhanHoi ph, out string errorMsg) {
             errorMsg = "";
             try { return phanHoiDAO.Update(ph); }
             catch(Exception ex) { errorMsg = ex.Message; return false; }
        }

        public List<PhanHoi> GetDeletedList()
        {
            return new PhanHoiDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new PhanHoiDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new PhanHoiDAO().DeleteForever(id);
        }
    }
}