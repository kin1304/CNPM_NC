namespace mamNonTuongLaiTuoiSang.Models
{
    public class SucKhoe
    {
        public int IdSK { get; set; }
        public string? IdHS { get; set; }         
        public decimal? ChieuCao { get; set; }    
        public decimal? CanNang { get; set; }      
        public DateTime? NgayNhap { get; set; }

        public HocSinh HocSinh { get; set; }

    }
}
