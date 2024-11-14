using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class PhuHuynh
    {
        public PhuHuynh()
        {
            HocSinhs = new HashSet<HocSinh>();
            VoucherCuaPhs = new HashSet<VoucherCuaPh>();
        }

        public string IdPh { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public bool? GioiTinh { get; set; }
        public string? NgheNghiep { get; set; }
        public int? NamSinh { get; set; }
        public string? MatKhau { get; set; }
        public string? Email { get; set; }
        public string? Sdt { get; set; }

        public virtual ICollection<HocSinh> HocSinhs { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherCuaPh> VoucherCuaPhs { get; set; }
        [JsonIgnore]
        public virtual ICollection<ThongBao_PhuHuynh>? ThongBaoPhuHuynhs { get; set; }
    }
}
