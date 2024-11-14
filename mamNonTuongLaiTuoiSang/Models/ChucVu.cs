using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class ChucVu
    {
        public ChucVu()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public string TenCv { get; set; } = null!;
        public string ViTri { get; set; } = null!;
        public int? LuongCoBan { get; set; }
        public decimal? HeSoLuong { get; set; }
        [JsonIgnore]
        public virtual ICollection<NhanVien> NhanViens { get; set; }


        public decimal? TinhLuong()
        {
            if (LuongCoBan == null || HeSoLuong == null)
            {
                return null;
            }
            return LuongCoBan * (decimal)HeSoLuong;
        }

        public decimal? TongLuong(int soLopDay, int soNgoaiKhoaDangKy)
        {
            decimal? luong = TinhLuong();

            if (soLopDay >= 3 && soNgoaiKhoaDangKy >= 5)
            {
                return luong;
            }
            else
            {
                return luong * 0.9m;
            }
        }
    }
}
