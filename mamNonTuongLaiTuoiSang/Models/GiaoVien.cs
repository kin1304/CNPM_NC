﻿using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class GiaoVien
    {
        public GiaoVien()
        {
            NgoaiKhoaGiaoViens = new HashSet<NgoaiKhoaGiaoVien>();
            
        }

        public string MaSt { get; set; } = null!;
        public string? TrinhDoChuyenMon { get; set; }
        public decimal? SaoDanhGia { get; set; }

        public virtual NhanVien? MaStNavigation { get; set; } = null!;

        
        public virtual ICollection<NgoaiKhoaGiaoVien> NgoaiKhoaGiaoViens { get; set; }
    }
}
