using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class NhanVienController
    {
        private readonly NhanVienDAO nhanVienDAO;

        public NhanVienController()
        {
            nhanVienDAO = new NhanVienDAO();
        }

        public List<NhanVien> GetAllNhanVien()
        {
            return nhanVienDAO.GetAll();
        }

        public List<NhanVien> SearchNhanVien(string id, string name)
        {
            // Nếu DAO có hàm Search(id, name) thì gọi, nếu không thì dùng GetAll + Linq
            // Ở phần DAO trước tôi đã viết hàm Search(id, name)
            return nhanVienDAO.Search(id, name);
        }

        public bool AddNhanVien(NhanVien nv, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(nv.IdNV) || string.IsNullOrWhiteSpace(nv.HoTen))
            {
                errorMsg = "Mã và Tên nhân viên là bắt buộc.";
                return false;
            }
            // Validate tuổi (ví dụ phải > 18)
            if (DateTime.Now.Year - nv.NamSinh < 18)
            {
                errorMsg = "Nhân viên phải đủ 18 tuổi.";
                return false;
            }

            try
            {
                return nhanVienDAO.Insert(nv);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool UpdateNhanVien(NhanVien nv, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return nhanVienDAO.Update(nv);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteNhanVien(string idNV, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return nhanVienDAO.Delete(idNV);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        // --- PHẦN BỔ SUNG CHO THÙNG RÁC ---
        public List<NhanVien> GetDeletedList()
        {
            return new NhanVienDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new NhanVienDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new NhanVienDAO().DeleteForever(id);
        }
    }
}