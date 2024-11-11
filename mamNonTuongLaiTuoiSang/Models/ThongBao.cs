namespace mamNonTuongLaiTuoiSang.Models
{
    public class ThongBao
    {
        public int IdTitle { get; set; }
        public string? Title { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public string? NoiDung { get; set; }
        public string? TepDinhKem { get; set; }
        public virtual ICollection<ThongBao_PhuHuynh>? ThongBaoPhuHuynhs { get; set; }


    }
}
