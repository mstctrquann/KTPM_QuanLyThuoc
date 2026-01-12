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

        // Soft delete: Đánh dấu ngừng kinh doanh
        public bool Delete(string id, out string msg)
        {
            msg = "";
            try 
            {
                if (dao.Delete(id)) 
                {
                    msg = "Thuốc đã được chuyển vào thùng rác (Đã ngừng kinh doanh)";
                    return true;
                }
                msg = "Ngừng kinh doanh thất bại"; 
                return false;
            }
            catch (System.Exception ex) { msg = ex.Message; return false; }
        }
        
        // --- PHẦN BỔ SUNG CHO THÙNG RÁC ---
        public List<Thuoc> GetDeletedList()
        {
            return dao.GetDeleted();
        }

        public bool Restore(string id, out string msg)
        {
            msg = "";
            try
            {
                if (dao.Restore(id)) 
                {
                    msg = "Khôi phục thành công";
                    return true;
                }
                msg = "Khôi phục thất bại";
                return false;
            }
            catch (System.Exception ex) { msg = ex.Message; return false; }
        }

        public bool DeleteForever(string id, out string msg)
        {
            msg = "";
            try
            {
                if (dao.DeleteForever(id)) 
                {
                    msg = "Đã xóa vĩnh viễn";
                    return true;
                }
                msg = "Xóa vĩnh viễn thất bại (Thuốc đang được sử dụng trong hóa đơn/phiếu nhập)";
                return false;
            }
            catch (System.Exception ex) 
            { 
                if (ex.Message.Contains("foreign key") || ex.Message.Contains("FOREIGN KEY"))
                {
                    msg = "Không thể xóa vĩnh viễn! Thuốc này đã được sử dụng trong hóa đơn hoặc phiếu nhập.";
                }
                else
                {
                    msg = ex.Message;
                }
                return false; 
            }
        }
        
        // Lấy tất cả thuốc bao gồm cả đã ngừng kinh doanh
        public List<Thuoc> GetAllIncludeDeleted() => dao.GetAllIncludeDeleted();
    }
}