using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;

namespace mamNonTuongLaiTuoiSang.Controllers.Teacher
{
    public class DiemDanhController : Controller
    {
        private readonly string url = "http://localhost:5005/api/diemdanhs/";
        private readonly string urlHs = "http://localhost:5005/api/hocsinhs/";
        private readonly string urlLop = "http://localhost:5005/api/lops/";
        private readonly HttpClient client;

        private readonly QLMamNonContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public DiemDanhController(QLMamNonContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            client = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id,string sortOrder, string searchQuery)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            // Cài đặt thông số cho việc sắp xếp
            ViewBag.IdDdSortParm = string.IsNullOrEmpty(sortOrder) ? "idd_desc" : "";
            ViewBag.IdHsSortParm = sortOrder == "idh_asc" ? "idh_desc" : "idh_asc";
            ViewBag.IdLopSortParm = sortOrder == "il_asc" ? "il_desc" : "il_asc";
            ViewBag.ThangSortParm = sortOrder == "thang_asc" ? "thang_desc" : "thang_asc";

            List<DiemDanh> diemDanhs = new List<DiemDanh>();
            List<Lop> lops = new List<Lop>();

            try
            {
                // Lấy danh sách lớp theo MaSt
                var lopResponse = await client.GetAsync(urlLop);
                if (lopResponse.IsSuccessStatusCode)
                {
                    var lopResult = await lopResponse.Content.ReadAsStringAsync();
                    lops = JsonConvert.DeserializeObject<List<Lop>>(lopResult) ?? new List<Lop>();
                    lops = lops.Where(l => l.MaSt == id).ToList(); // Lọc lớp có MaSt = id
                }

                // Lấy danh sách điểm danh theo IdLop
                var diemDanhResponse = await client.GetAsync(url);
                if (diemDanhResponse.IsSuccessStatusCode)
                {
                    var diemDanhResult = await diemDanhResponse.Content.ReadAsStringAsync();
                    diemDanhs = JsonConvert.DeserializeObject<List<DiemDanh>>(diemDanhResult) ?? new List<DiemDanh>();
                    var validIdLop = lops.Select(l => l.IdLop).ToList();
                    diemDanhs = diemDanhs.Where(dd => validIdLop.Contains(dd.IdLop)).ToList();
                }

                // Tìm kiếm theo IdDD hoặc các thuộc tính khác
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    HttpResponseMessage responseById = await client.GetAsync(url + Uri.EscapeDataString(searchQuery));
                    if (responseById.IsSuccessStatusCode)
                    {
                        string resultById = await responseById.Content.ReadAsStringAsync();
                        var diemDanhFound = JsonConvert.DeserializeObject<DiemDanh>(resultById);
                        if (diemDanhFound != null)
                        {
                            return RedirectToAction("Details", new { id = diemDanhFound.IdDd });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "IdDd không tồn tại.");
                        }
                    }
                    diemDanhs = diemDanhs
                        .Where(dd => dd.IdDd.Contains(searchQuery) ||
                                     dd.IdHs.Contains(searchQuery) ||
                                     dd.IdLop.Contains(searchQuery))
                        .ToList();

                    if (!diemDanhs.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Không tìm thấy kết quả nào.");
                    }
                }

                // Sắp xếp dữ liệu theo sortOrder
                diemDanhs = sortOrder switch
                {
                    "idd_desc" => diemDanhs.OrderByDescending(dd => dd.IdDd).ToList(),
                    "idh_asc" => diemDanhs.OrderBy(dd => dd.IdHs).ToList(),
                    "idh_desc" => diemDanhs.OrderByDescending(dd => dd.IdHs).ToList(),
                    "il_asc" => diemDanhs.OrderBy(dd => dd.IdLop).ToList(),
                    "il_desc" => diemDanhs.OrderByDescending(dd => dd.IdLop).ToList(),
                    _ => diemDanhs.OrderBy(dd => dd.IdDd).ToList(),
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.");
            }

            return View(diemDanhs);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            ViewBag.IdHs = await GetIdHsSelectListAsync();
            ViewBag.IdLop = await GetIdLopSelectListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiemDanh dd)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(dd);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { id = ViewData["GiaoVien"] });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo điểm danh.");
                }
            }

            // Nếu ModelState không hợp lệ hoặc API trả về lỗi, tái định nghĩa lại ViewBag
            ViewBag.IdHs = await GetIdHsSelectListAsync();
            ViewBag.IdLop = await GetIdLopSelectListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            DiemDanh dd = new DiemDanh();
            HttpResponseMessage response = client.GetAsync(url+id).Result;
            if (response.IsSuccessStatusCode) 
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<DiemDanh>(result);
                if (data != null) 
                {
                    dd=data;
                }
            }
            ViewBag.IdHs = await GetIdHsSelectListAsync();
            ViewBag.IdLop = await GetIdLopSelectListAsync();
            return View(dd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DiemDanh dd)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            if (ModelState.IsValid)
            {
                try
                {
                    string data = JsonConvert.SerializeObject(dd);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"{url}{dd.IdDd}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { id = ViewData["GiaoVien"] });
                    }
                    else
                    {
                        // Đọc nội dung lỗi từ API và thêm vào ModelState
                        string errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi cập nhật điểm danh: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
                }
            }

            // Nếu ModelState không hợp lệ hoặc API trả về lỗi, tái định nghĩa lại ViewBag
            ViewBag.IdHs = await GetIdHsSelectListAsync();
            ViewBag.IdLop = await GetIdLopSelectListAsync();
            return View(dd);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            DiemDanh dd = new DiemDanh();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<DiemDanh>(result);
                if (data != null)
                {
                    dd = data;
                }
            }
            return View(dd);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            DiemDanh dd = new DiemDanh();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<DiemDanh>(result);
                if (data != null)
                {
                    dd = data;
                }
            }
            return View(dd);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = ViewData["GiaoVien"] });
            }
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetIdHsSelectListAsync()
        {
            List<HocSinh> hocsinh = new List<HocSinh>();
            HttpResponseMessage response = await client.GetAsync(urlHs);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                hocsinh = JsonConvert.DeserializeObject<List<HocSinh>>(result) ?? new List<HocSinh>();
            }

            return hocsinh.Select(hs => new SelectListItem
            {
                Value = hs.IdHs, // Giá trị bạn muốn gửi về
                Text = $"({hs.IdHs}) {hs.TenHs}" // Hiển thị tên học sinh
            }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetIdLopSelectListAsync()
        {
            List<Lop> lop = new List<Lop>();
            HttpResponseMessage response = await client.GetAsync(urlLop);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                lop = JsonConvert.DeserializeObject<List<Lop>>(result) ?? new List<Lop>();
            }

            return lop.Select(l => new SelectListItem
            {
                Value = l.IdLop, // Giá trị bạn muốn gửi về
                Text = $"({l.IdLop}) {l.TenLop}" // Hiển thị tên lớp
            }).ToList();
        }
    }
}
