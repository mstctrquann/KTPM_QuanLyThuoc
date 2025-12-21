using System;
using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class HopDongController
    {
        private readonly HopDongDAO hopDongDAO;

        public HopDongController()
        {
            hopDongDAO = new HopDongDAO();
        }

        public List<HopDong> GetAllHopDong()
        {
            return hopDongDAO.GetAllHopDong();
        }

        public HopDong? GetHopDongById(string idHD)
        {
            return hopDongDAO.GetHopDongById(idHD);
        }

        public List<HopDong> SearchHopDong(string id, string nv, string ncc)
        {
            return hopDongDAO.SearchHopDong(id, nv, ncc);
        }

        public bool AddHopDong(HopDong hd, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (hd.NgayBatDau > hd.NgayKetThuc)
            {
                errorMsg = "Ngày bắt đầu không thể lớn hơn ngày kết thúc.";
                return false;
            }
            try
            {
                return hopDongDAO.InsertHopDong(hd);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool UpdateHopDong(HopDong hd, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return hopDongDAO.UpdateHopDong(hd);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteHopDong(string id, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                return hopDongDAO.DeleteHopDong(id);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public List<HopDong> GetDeletedList()
        {
            return new HopDongDAO().GetDeleted();
        }

        public bool Restore(string id)
        {
            return new HopDongDAO().Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return new HopDongDAO().DeleteForever(id);
        }
    }
}