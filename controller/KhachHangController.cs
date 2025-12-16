using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class KhachHangController
    {
        private readonly KhachHangDAO khachHangDAO;

        public KhachHangController()
        {
            khachHangDAO = new KhachHangDAO();
        }

        public List<KhachHang> GetAllKhachHang()
        {
            return khachHangDAO.GetAll();
        }

        public List<KhachHang> SearchKhachHang(string hoTen, string sdt)
        {
            return khachHangDAO.Search(hoTen, sdt);
        }

        public bool AddKhachHang(KhachHang kh, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(kh.IdKH) || string.IsNullOrWhiteSpace(kh.HoTen))
            {
                errorMsg = "Thông tin khách hàng không đầy đủ.";
                return false;
            }
            try
            {
                return khachHangDAO.Insert(kh);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool UpdateKhachHang(KhachHang kh, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return khachHangDAO.Update(kh);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteKhachHang(string idKH, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return khachHangDAO.Delete(idKH);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        // --- Logic Điểm tích lũy ---
        public int GetDiemHienTai(string idKH)
        {
            var kh = khachHangDAO.GetById(idKH);
            return kh != null ? kh.DiemTichLuy : 0;
        }

        public bool CongDiem(string idKH, int diem)
        {
            return khachHangDAO.CongDiem(idKH, diem);
        }

        public bool TruDiem(string idKH, int diem)
        {
            return khachHangDAO.TruDiem(idKH, diem);
        }
        public List<KhachHang> GetDeletedList()
        {
            return new KhachHangDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new KhachHangDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new KhachHangDAO().DeleteForever(id);
        }
    }
}