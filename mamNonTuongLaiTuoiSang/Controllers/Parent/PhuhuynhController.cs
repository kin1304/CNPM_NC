using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class PhuhuynhController : Controller
    {
        QLMamNonContext db = new QLMamNonContext();
        private readonly string urlPh = "http://localhost:5005/api/VoucherCuaPhs";
        private readonly string url = "http://localhost:5005/api/Vouchers";
        private const string baseURL = "http://localhost:5005/api/PhuHuynhs";
        private const string baseURL2 = "http://localhost:5005/api/KhoaHocs";
        private HttpClient client = new HttpClient();
        [HttpGet]
        public IActionResult PhuHuynh(PhuHuynh ph)
        {
            TempData["PhuHuynh"] = ph.IdPh;
            ViewData["PhuHuynh"] = ph.IdPh;
            List<ThongBao_PhuHuynh> tbph = db.ThongBao_PhuHuynhs.Where(x => x.IdPH == ph.IdPh && x.IsRead == false).ToList();
            ViewData["Nofi"] = tbph.Count;
            HttpContext.Session.SetString("Nofi", tbph.Count.ToString());
            return View(ph);
        }
        [HttpGet]
        public IActionResult Info(string id)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            PhuHuynh ph = new PhuHuynh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            PhuHuynh ph = new PhuHuynh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PhuHuynh ph)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            string data = JsonConvert.SerializeObject(ph);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(baseURL + "/" + ph.IdPh, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Info", new { id = ph.IdPh });
            }
            else
            {
                string errorContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(errorContent); // In ra lỗi chi tiết từ API
            }
            return View(ph);
        }

        [HttpGet]
        public IActionResult KhoaHoc(string id)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            List<KhoaHoc> kh = new List<KhoaHoc>();
            HttpResponseMessage response = client.GetAsync(baseURL2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<KhoaHoc>>(result);
                if (data != null)
                {
                    kh = data;
                }
            }
            return View(kh);
        }

        [HttpGet]
        public async Task<IActionResult> voucherlist(string id)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThuNhapVoucher(string idVoucher, string idPh)
        {
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
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
                    return RedirectToAction("voucherlist", new { id = idPh });
                }
            }

            // Tạo dữ liệu mới cho VoucherCuaPh nếu chưa tồn tại
            VoucherCuaPh voucherCuaPh = new VoucherCuaPh
            {
                IdPh = sanitizedIdPh,
                IdVoucher = sanitizedIdVoucher,
                Trangthai = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(voucherCuaPh), Encoding.UTF8, "application/json");

            // Gọi API để thêm voucher vào bảng VoucherCuaPh
            HttpResponseMessage response = await client.PostAsync(urlPh, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thu thập voucher thành công!";

            }
            else
            {
                TempData["ErrorMessage"] = "Không thể thu thập voucher.";
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }

            return RedirectToAction("voucherlist", new { id = idPh });
        }

        [HttpGet]
        public IActionResult ThongBao(string id)
        {
            List<ThongBao_PhuHuynh> tbph = db.ThongBao_PhuHuynhs.Where(x => x.IdPH == id && x.IsRead == false).ToList();
            ViewData["Nofi"] = tbph.Count;
            HttpContext.Session.SetString("Nofi", tbph.Count.ToString());
            ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            tbph = db.ThongBao_PhuHuynhs.Where(x => x.IdPH == id).ToList();
            for (var i = 0; i < tbph.Count; i++)
            {
                tbph[i].IdTitleNavigation = db.ThongBaos.Where(p => p.IdTitle == tbph[i].IdTitle).FirstOrDefault();
            }
            return View(tbph);
        }

        [HttpGet]
        public IActionResult ChiTietThongBao(int id, string idPh)
        {
            if (HttpContext.Session.GetString("Nofi") == null)
            {

                return RedirectToAction("Login", "Account");

            }
            TempData["PhuHuynh"] = idPh;
            ViewData["PhuHuynh"] = idPh;
            ThongBao_PhuHuynh tbph = db.ThongBao_PhuHuynhs.Where(x => x.IdTitle == id && x.IdPH == idPh).FirstOrDefault();
            if(tbph == null)
            {
                return RedirectToAction("ThongBao", new { id = idPh });
            }
            if(tbph.IsRead == false)
            {
                ViewData["Nofi"] = Math.Max(int.Parse(HttpContext.Session.GetString("Nofi")) - 1, 0);
                HttpContext.Session.SetString("Nofi", ViewData["Nofi"].ToString());
                tbph.IsRead = true;
                db.SaveChanges();
            }
            else
            {
                HttpContext.Session.SetString("Nofi", int.Parse(HttpContext.Session.GetString("Nofi")).ToString());
                ViewData["Nofi"] = HttpContext.Session.GetString("Nofi");
            }
            tbph.IdTitleNavigation = db.ThongBaos.Where(p => p.IdTitle == tbph.IdTitle).FirstOrDefault();
            return View(tbph);
        }

    }
}
