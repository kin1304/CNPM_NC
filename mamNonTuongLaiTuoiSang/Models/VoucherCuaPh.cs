using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class VoucherCuaPh
    {
        public string IdPh { get; set; } = null!;
        public string IdVoucher { get; set; } = null!;
        public byte? Trangthai { get; set; }

        public virtual PhuHuynh? IdPhNavigation { get; set; }
        public virtual Voucher? IdVoucherNavigation { get; set; }
    }
}

