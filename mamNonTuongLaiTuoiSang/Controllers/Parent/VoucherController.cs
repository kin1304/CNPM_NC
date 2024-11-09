using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Controllers.Parent
{
    public class VoucherController : Controller
    {
        private readonly string url = "http://localhost:5005/api/Vouchers";
        private readonly string urlPh = "http://localhost:5005/api/VoucherCuaPhs";

        private readonly HttpClient client = new HttpClient();

        public async Task<IActionResult> Index(string id)
        {

            ViewData["PhuHuynh"] = id;
            TempData["PhuHuynh"] = id;
            var currentDate = DateTime.Now;
            var vouchers = new List<Voucher>();

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Voucher>>(result);

                if (data != null)
                {
                    // Lọc voucher có NgayHetHan lớn hơn ngày hiện tại
                    vouchers = data.Where(v => v.NgayHetHan > currentDate).ToList();
                }
            }

            return View(vouchers);
        }

        // Hành động Thu nhập voucher
        [HttpPost]
        public async Task<IActionResult> ThuNhapVoucher(string idVoucher, string idPh)
        {
            ViewData["PhuHuynh"] = idPh;
            TempData["PhuHuynh"] = idPh;

            if (string.IsNullOrEmpty(idPh))
            {
                return BadRequest("Không xác định được IdPh.");
            }

            // Xóa khoảng trắng ở đầu và cuối IdVoucher và IdPh
            string sanitizedIdVoucher = idVoucher.Trim();
            string sanitizedIdPh = idPh.Trim();

            // Kiểm tra xem voucher đã tồn tại trong bảng VoucherCuaPh cho IdPh và IdVoucher này chưa
            HttpResponseMessage checkResponse = await client.GetAsync($"{urlPh}/ByPhuHuynh/{sanitizedIdPh}");

            if (checkResponse.IsSuccessStatusCode)
            {
                var existingVouchers = JsonConvert.DeserializeObject<List<VoucherCuaPh>>(await checkResponse.Content.ReadAsStringAsync());
                if (existingVouchers.Any(v => v.IdVoucher.Trim() == sanitizedIdVoucher))
                {
                    TempData["ErrorMessage"] = "Voucher này bạn đã thu nhập.";
                    return RedirectToAction("Index", new { id = idPh });
                }
            }

            // Tạo dữ liệu mới cho VoucherCuaPh nếu chưa tồn tại
            var newVoucherCuaPh = new VoucherCuaPh
            {
                IdPh = sanitizedIdPh.ToUpper(),
                IdVoucher = sanitizedIdVoucher.ToUpper(),
                Trangthai = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(newVoucherCuaPh), Encoding.UTF8, "application/json");

            // Gọi API để thêm voucher vào bảng VoucherCuaPh
            HttpResponseMessage response = await client.PostAsync(urlPh, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thu thập voucher thành công!";
                return RedirectToAction("Index", new { id = idPh });
            }

            return BadRequest("Không thể thu thập voucher.");
        }


    }


}
