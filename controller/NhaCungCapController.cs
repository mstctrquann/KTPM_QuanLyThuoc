using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class NhaCungCapController
    {
        private readonly NhaCungCapDAO nccDAO;

        public NhaCungCapController()
        {
            nccDAO = new NhaCungCapDAO();
        }

        public List<NhaCungCap> GetAllNhaCungCap()
        {
            return nccDAO.GetAll();
        }

        public List<NhaCungCap> GetAll()
        {
            return nccDAO.GetAll();
        }

        public List<NhaCungCap> SearchNhaCungCap(string id, string ten)
        {
            return nccDAO.Search(id, ten);
        }

        public bool AddNhaCungCap(NhaCungCap ncc, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
            {
                errorMsg = "Tên nhà cung cấp không được để trống.";
                return false;
            }
            try
            {
                return nccDAO.Insert(ncc);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool UpdateNhaCungCap(NhaCungCap ncc, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return nccDAO.Update(ncc);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteNhaCungCap(string id, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return nccDAO.Delete(id);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        public List<NhaCungCap> GetDeletedList()
        {
            return new NhaCungCapDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new NhaCungCapDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new NhaCungCapDAO().DeleteForever(id);
        }
    }
}