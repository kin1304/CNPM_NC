using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HocSinhLop
    {
        public HocSinhLop()
        {
            DiemDanhs = new HashSet<DiemDanh>();
        }

        public string IdHs { get; set; } = null!;
        public string IdLop { get; set; } = null!;
        public decimal? DiemChuyenCan { get; set; }


        [JsonIgnore]
        public virtual HocSinh? IdHsNavigation { get; set; }
        public virtual Lop? IdLopNavigation { get; set; }
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }
    }
}
