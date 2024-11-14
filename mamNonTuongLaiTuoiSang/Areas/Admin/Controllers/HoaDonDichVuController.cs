using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonDichVuController : Controller
    {
        private string baseURL = "https://localhost:5005/api/hoadondichvus";
        private string urlHd = "https://localhost:5005/api/hoadons";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public HoaDonDichVuController(QLMamNonContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<HoaDonDichVu> hddv = new List<HoaDonDichVu>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HoaDonDichVu>>(result);
                if (data != null)
                {
                    hddv = data;
                }
            }
            return View(hddv);
        }

        // GET: Admin/HoaDonDichVu/Create
        public IActionResult Create()
        {
            ViewBag.IdHd = GetIdHdSelectList();
            ViewBag.IdDv = GetIdDvSelectList();
            return View();
        }

        // POST: Admin/HoaDonDichVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDonDichVu hoaDonDichVu)
        {
            if (await IsHoaDonDichVuExists(hoaDonDichVu.IdHd, hoaDonDichVu.IdDv))
            {
                ModelState.AddModelError(string.Empty, "IdHd và IdDv này đã tồn tại.");
                ViewBag.IdHd = GetIdHdSelectList();
                ViewBag.IdDv = GetIdDvSelectList();
                return View(hoaDonDichVu);
            }

            string jsonData = JsonConvert.SerializeObject(hoaDonDichVu);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(baseURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo hóa đơn dịch vụ.");
                return View(hoaDonDichVu);
            }
        }

        // GET: Admin/HoaDonDichVu/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string IdHd, string IdDv)
        {
            HoaDonDichVu hddv = await FindTKB(IdHd, IdDv);
            if (hddv == null)
            {
                return NotFound();
            }
            ViewBag.IdHd = GetIdHdSelectList();
            ViewBag.IdDv = GetIdDvSelectList();

            return View(hddv);
        }

        // POST: Admin/HoaDonDichVu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string IdHd, string IdDv, HoaDonDichVu hoaDonDichVu)
        {
            string jsonData = JsonConvert.SerializeObject(hoaDonDichVu);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{baseURL}/{IdHd}/{IdDv}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật hóa đơn dịch vụ.");
                ViewBag.IdHd = GetIdHdSelectList();
                ViewBag.IdDv = GetIdDvSelectList();
                return View(hoaDonDichVu);
            }
        }

        // GET: Admin/Tkb/Details/5
        [HttpGet("Admin/HoaDonDichVu/Details/{IdHd}/{IdDv}")]
        public async Task<IActionResult> Details(string IdHd, string IdDv)
        {
            if (string.IsNullOrEmpty(IdHd) || string.IsNullOrEmpty(IdDv))
            {
                return NotFound();
            }

            HoaDonDichVu hddv = await FindTKB(IdHd, IdDv);
            if (hddv == null)
            {
                return NotFound();
            }
            return View(hddv);
        }

        // GET: Admin/HoaDonDichVu/Delete/{IdHd}/{IdDv}
        public async Task<IActionResult> Delete(string IdHd, string IdDv)
        {
            var hddv = await FindTKB(IdHd, IdDv);
            if (hddv == null)
            {
                return NotFound();
            }
            return View(hddv);
        }

        // POST: Admin/HoaDonDichVu/Delete/{IdHd}/{IdDv}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string IdHd, string IdDv)
        {
            var response = await client.DeleteAsync($"{baseURL}/{IdHd}/{IdDv}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private IEnumerable<SelectListItem> GetIdHdSelectList()
        {
            List<HoaDon> hoaDons = new List<HoaDon>();
            HttpResponseMessage response = client.GetAsync(urlHd).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                hoaDons = JsonConvert.DeserializeObject<List<HoaDon>>(result);
            }

            // Chuyển đổi danh sách NhanVien thành danh sách SelectListItem
            var selectListItems = hoaDons.Select(nv => new SelectListItem
            {
                Value = nv.IdHd, // Giá trị bạn muốn gửi về
                Text = $" ({nv.IdHd})"
            }).ToList();

            return selectListItems;
        }

        private IEnumerable<SelectListItem> GetIdDvSelectList()
        {
            var dichVus = _context.DichVus.ToList();

            var selectListItems = dichVus.Select(dv => new SelectListItem
            {
                Value = dv.IdDv,  // Giá trị bạn muốn gửi về
                Text = $"{dv.TenDv} ({dv.IdDv})" // Tên dịch vụ và IdDv để hiển thị trong dropdown
            }).ToList();

            return selectListItems;
        }

        public async Task<HoaDonDichVu> FindTKB(string IdHd, string IdDv)
        {
            using (var hddv = new HttpClient())
            {
                string path = $"{baseURL}/{IdHd}/{IdDv}";
                hddv.DefaultRequestHeaders.Accept.Clear();
                hddv.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await hddv.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<HoaDonDichVu>(data);
                    return response;
                }
                else
                {
                    string responseContent = await getData.Content.ReadAsStringAsync();
                    Console.WriteLine("Content: " + responseContent);
                }
            }
            return null;
        }

        private async Task<bool> IsHoaDonDichVuExists(string IdHd, string IdDv)
        {
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/{IdHd}/{IdDv}");

            return response.IsSuccessStatusCode;
        }

    }
}
