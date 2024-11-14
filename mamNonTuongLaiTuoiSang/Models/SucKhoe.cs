using System.ComponentModel.DataAnnotations.Schema;

namespace mamNonTuongLaiTuoiSang.Models
{
    public class SucKhoe
    {
        public int IdSK { get; set; } // IDENTITY, nên chỉ cần định nghĩa là int
        public string IdHS { get; set; } // Foreign key đến HocSinh

        public decimal? ChieuCao { get; set; } // Chiều cao tính bằng cm

        public decimal? CanNang { get; set; } // Cân nặng tính bằng kg

        public DateTime? NgayNhap { get; set; } // Ngày nhập dữ liệu

        // Navigation property để kết nối với HocSinh
        public HocSinh? HocSinh { get; set; }
    }


}
