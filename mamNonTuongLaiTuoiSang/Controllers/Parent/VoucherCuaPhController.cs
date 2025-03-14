﻿using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers.Parent
{
    public class VoucherCuaPhController : Controller
    {
        private readonly string url = "http://localhost:5005/api/VoucherCuaPhs/ByPhuHuynh/";
        private readonly string url1 = "http://localhost:5005/api/VoucherCuaPhs/";

        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            ViewData["PhuHuynh"] = id;
            TempData["PhuHuynh"] = id;
            List<VoucherCuaPh> vouchers = new List<VoucherCuaPh>();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<VoucherCuaPh>>(result);
                if (data != null)
                {
                    vouchers = data;
                }

            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            return View(vouchers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string idPh, string idVoucher)
        {
            ViewData["PhuHuynh"] = idPh;
            TempData["PhuHuynh"] = idPh;

            // Đường dẫn đến API Delete
            string urlt = url1 + $"{idPh}/{idVoucher}";

            // Gửi yêu cầu xóa đến API
            HttpResponseMessage response = await client.DeleteAsync(urlt);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xóa voucher thành công.";
            }
            else
            {
                string responeContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Content: " + responeContent);
                TempData["ErrorMessage"] = "Không thể xóa voucher.";
            }

            return RedirectToAction("Index", new { id = idPh });
        }

    }
}
