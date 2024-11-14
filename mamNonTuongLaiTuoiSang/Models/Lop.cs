using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class Lop
    {
        public Lop()
        {
            HocSinhLops = new HashSet<HocSinhLop>();
            Tkbs = new HashSet<Tkb>();
        }

        public string IdLop { get; set; } = null!;
        public string? TenLop { get; set; }   
        public string? TrangThai { get; set; }
        public DateTime? NgayBK { get; set; }
        public DateTime? NgayKT { get; set; }
        public int? SiSo { get; set; }
        public string? MaSt { get; set; }

        public virtual NhanVien? MaStNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<HocSinhLop> HocSinhLops { get; set; }
        [JsonIgnore]
        public virtual ICollection<Tkb> Tkbs { get; set; }

        // Phương thức đếm số lớp của một giáo viên
        public static int DemSoLop(string maSt, QLMamNonContext db)
        {
            return db.Lops.Count(l => l.MaSt == maSt);
        }
    }
}
