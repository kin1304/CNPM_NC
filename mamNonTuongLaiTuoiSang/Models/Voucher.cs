using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            VoucherCuaPhs = new HashSet<VoucherCuaPh>();
        }

        public string IdVoucher { get; set; } = null!;
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public decimal? PhanTramGiam { get; set; }
        public int? SoLuong { get; set; }
        public string? Trangthai { get; set; }
        public string? MaSt { get; set; }

        public virtual NhanVien? MaStNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherCuaPh> VoucherCuaPhs { get; set; }
    }
}
