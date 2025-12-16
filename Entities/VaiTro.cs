namespace QLThuocApp.Entities
{
    public class VaiTro
    {
        public string IdVT { get; set; } = "";
        public string Ten { get; set; } = "";

        public VaiTro() { }
        public VaiTro(string id, string ten) { IdVT = id; Ten = ten; }
        
        // Hỗ trợ setter style Java cũ
        public void SetIdVT(string id) => IdVT = id;
        public void SetTen(string ten) => Ten = ten;
    }
}