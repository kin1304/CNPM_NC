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
    }
}
