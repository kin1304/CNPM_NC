using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class NgoaiKhoaGiaoVien
    {
        public string IdNk { get; set; } = null!;
        public string MaSt { get; set; } = null!;
        public virtual NgoaiKhoa IdNKNavigation { get; set; } = null!;
        public virtual GiaoVien MaStNavigation { get; set; } = null!;

        // Phương thức đếm số ngoại khóa mà giáo viên đăng ký
        public static int DemSoNgoaiKhoa(string maSt, QLMamNonContext db)
        {
            return db.NgoaiKhoaGiaoViens.Count(nk => nk.MaSt == maSt);
        }
    }
}
