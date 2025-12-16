using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class PhieuNhapController
    {
        private readonly PhieuNhapDAO phieuNhapDAO;

        public PhieuNhapController()
        {
            phieuNhapDAO = new PhieuNhapDAO();
        }

        public List<PhieuNhap> GetAllPhieuNhap()
        {
            return phieuNhapDAO.GetAll();
        }

        public List<PhieuNhap> SearchPhieuNhap(string idPN, string idNV, string idNCC)
        {
            return phieuNhapDAO.Search(idPN, idNV, idNCC);
        }

        public bool AddPhieuNhap(PhieuNhap pn, List<ChiTietPhieuNhap> details, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (pn == null || details == null || details.Count == 0)
            {
                errorMsg = "Thông tin phiếu nhập không hợp lệ hoặc không có thuốc.";
                return false;
            }

            try
            {
                // DAO.InsertPhieuNhapWithDetails xử lý Transaction + Tăng tồn kho
                return phieuNhapDAO.InsertPhieuNhapWithDetails(pn, details);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool Add(PhieuNhap pn, List<ChiTietPhieuNhap> details, out string errorMsg)
        {
            return AddPhieuNhap(pn, details, out errorMsg);
        }

        public bool DeletePhieuNhap(string idPN, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return phieuNhapDAO.DeletePhieuNhap(idPN);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        
        // Hàm hỗ trợ Update nếu cần (lưu ý update phiếu nhập phức tạp vì liên quan kho)
        public bool UpdatePhieuNhap(PhieuNhap pn, out string errorMsg)
        {
             errorMsg = string.Empty;
             try {
                 return phieuNhapDAO.Update(pn); // Chỉ update thông tin chung, không update chi tiết kho ở đây
             } catch(Exception ex) {
                 errorMsg = ex.Message;
                 return false;
             }
        }

        public List<PhieuNhap> GetDeletedList()
        {
            return new PhieuNhapDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new PhieuNhapDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new PhieuNhapDAO().DeleteForever(id);
        }
    }
}