using System.Collections.Generic;
using QLThuocApp.dao;
using QLThuocApp.Entities;

namespace QLThuocApp.Controllers
{
    public class ThuocController
    {
        private ThuocDAO dao = new ThuocDAO();

        public List<Thuoc> GetAll() => dao.GetAll();

        public List<Thuoc> Search(string keyword) => dao.Search(keyword);

        public bool Add(Thuoc t, out string msg)
        {
            msg = "";
            if (string.IsNullOrWhiteSpace(t.IdThuoc) || string.IsNullOrWhiteSpace(t.TenThuoc))
            {
                msg = "Mã và tên thuốc không được trống";
                return false;
            }
            try 
            {
                if (dao.Insert(t)) return true;
                msg = "Thêm thất bại"; return false;
            }
            catch (System.Exception ex) { msg = ex.Message; return false; }
        }

        public bool Update(Thuoc t, out string msg)
        {
            msg = "";
            try 
            {
                if (dao.Update(t)) return true;
                msg = "Cập nhật thất bại"; return false;
            }
            catch (System.Exception ex) { msg = ex.Message; return false; }
        }

        public bool Delete(string id, out string msg)
        {
            msg = "";
            try 
            {
                if (dao.Delete(id)) return true;
                msg = "Xóa thất bại (Thuốc đang được sử dụng?)"; return false;
            }
            catch (System.Exception ex) { msg = ex.Message; return false; }
        }
        // --- PHẦN BỔ SUNG CHO THÙNG RÁC ---
        public List<Thuoc> GetDeletedList()
        {
            return dao.GetDeleted();
        }

        public bool Restore(string id)
        {
            return dao.Restore(id);
        }

        public bool DeleteForever(string id)
        {
            return dao.DeleteForever(id);
        }
    }
}