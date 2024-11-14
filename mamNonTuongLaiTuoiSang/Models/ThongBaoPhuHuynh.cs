namespace mamNonTuongLaiTuoiSang.Models
{
    public class ThongBao_PhuHuynh
    {
        
        public int IdTitle { get; set; }  
        public string IdPH { get; set; }  
        public bool? IsRead { get; set; }


        public virtual PhuHuynh? IdPhNavigation { get; set; }
        public virtual ThongBao? IdTitleNavigation { get; set; }


    }
}
