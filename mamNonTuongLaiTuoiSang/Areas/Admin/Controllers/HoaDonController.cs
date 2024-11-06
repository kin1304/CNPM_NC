using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonController : Controller
    {
        private string url = "http://localhost:5005/api/hoadons/";
        private string urlHs = "http://localhost:5005/api/hocsinhs/";
        private string urlNv = "http://localhost:5005/api/nhanviens/";

        private HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            List<HoaDon> hoadon = new List<HoaDon>();

            // Lấy danh sách hóa đơn từ API
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<HoaDon>>(result);
                if (data != null)
                {
                    hoadon = data;
                }
            }

            // Tìm kiếm
            ViewBag.SearchQuery = searchQuery;
            if (!String.IsNullOrEmpty(searchQuery))
            {
                // Tìm kiếm theo IdHd, nếu tìm thấy thì vào trang chi tiết
                var hoaDonById = hoadon.FirstOrDefault(h => h.IdHd.Equals(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (hoaDonById != null)
                {
                    return RedirectToAction("Details", new { id = hoaDonById.IdHd });
                }

                // Tìm kiếm theo NgayLap, NgayHetHan, hoặc TrangThai
                hoadon = hoadon.Where(h => (h.TrangThai != null && h.TrangThai.Contains(searchQuery))
                                        || (h.NgayLap != null && h.NgayLap.Value.ToString("yyyy-MM-dd").Contains(searchQuery))
                                        || (h.NgayHetHan != null && h.NgayHetHan.Value.ToString("yyyy-MM-dd").Contains(searchQuery)))
                               .ToList();
            }

            // Sắp xếp
            ViewBag.IdHdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.SoTienSortParm = sortOrder == "SoTien" ? "soTien_desc" : "SoTien";
            ViewBag.TrangThaiSortParm = sortOrder == "TrangThai" ? "trangThai_desc" : "TrangThai";
            ViewBag.IdHsSortParm = sortOrder == "IdHs" ? "idHs_desc" : "IdHs";
            ViewBag.MaStSortParm = sortOrder == "MaSt" ? "maSt_desc" : "MaSt";

            switch (sortOrder)
            {
                case "id_desc":
                    hoadon = hoadon.OrderByDescending(h => h.IdHd).ToList();
                    break;
                case "SoTien":
                    hoadon = hoadon.OrderBy(h => h.SoTien).ToList();
                    break;
                case "soTien_desc":
                    hoadon = hoadon.OrderByDescending(h => h.SoTien).ToList();
                    break;
                case "TrangThai":
                    hoadon = hoadon.OrderBy(h => h.TrangThai).ToList();
                    break;
                case "trangThai_desc":
                    hoadon = hoadon.OrderByDescending(h => h.TrangThai).ToList();
                    break;
                case "IdHs":
                    hoadon = hoadon.OrderBy(h => h.IdHs).ToList();
                    break;
                case "idHs_desc":
                    hoadon = hoadon.OrderByDescending(h => h.IdHs).ToList();
                    break;
                case "MaSt":
                    hoadon = hoadon.OrderBy(h => h.MaSt).ToList();
                    break;
                case "maSt_desc":
                    hoadon = hoadon.OrderByDescending(h => h.MaSt).ToList();
                    break;
                default:
                    hoadon = hoadon.OrderBy(h => h.IdHd).ToList();
                    break;
            }

            return View(hoadon);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.IdHs = GetIdHsSelectList();
            ViewBag.MaSt = GetMaStSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDon hd)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(hd);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo.");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            HoaDon hd = new HoaDon();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HoaDon>(result);
                if (data != null)
                {
                    hd = data;
                }
            }
            ViewBag.IdHs = GetIdHsSelectList();
            ViewBag.MaSt = GetMaStSelectList();
            return View(hd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HoaDon hd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string data = JsonConvert.SerializeObject(hd);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"{url}{hd.IdHd}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
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
            //ViewBag.IdHs = await GetIdHsSelectListAsync();
            //ViewBag.MaSt = await GetMaStSelectListAsync();
            return View(hd);
        }

        private IEnumerable<SelectListItem> GetIdHsSelectList()
        {
            List<HocSinh> hocsinh = new List<HocSinh>();
            HttpResponseMessage response = client.GetAsync(urlHs).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                hocsinh = JsonConvert.DeserializeObject<List<HocSinh>>(result);
            }

            var selectListItems = hocsinh.Select(hs => new SelectListItem
            {
                Value = hs.IdHs, // Giá trị bạn muốn gửi về
                Text = $" ({hs.IdHs})" + hs.TenHs
            }).ToList();

            return selectListItems;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            HoaDon hd = new HoaDon();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HoaDon>(result);
                if (data != null)
                {
                    hd = data;
                }
            }
            return View(hd);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            HoaDon hd = new HoaDon();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HoaDon>(result);
                if (data != null)
                {
                    hd = data;
                }
            }
            return View(hd);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private IEnumerable<SelectListItem> GetMaStSelectList()
        {
            List<NhanVien> nhanViens = new List<NhanVien>();
            HttpResponseMessage response = client.GetAsync(urlNv).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(result);
            }

            // Chuyển đổi danh sách NhanVien thành danh sách SelectListItem
            var selectListItems = nhanViens.Select(nv => new SelectListItem
            {
                Value = nv.MaSt, // Giá trị bạn muốn gửi về
                Text = $" ({nv.MaSt})" + nv.HoTen// Hiển thị tên nhân viên
            }).ToList();

            return selectListItems;
        }
    }
}
