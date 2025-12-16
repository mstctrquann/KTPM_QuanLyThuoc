namespace QLThuocApp.Entities
{
    public class XuatXu
    {
        public string IdXX { get; set; } = ""; // ID giả lập
        public string Ten { get; set; } = "";
        
        // Hỗ trợ setter style Java cũ nếu cần
        public void SetIdXX(string id) => IdXX = id;
        public void SetTen(string ten) => Ten = ten;
    }
}