using MailKit.Search;
using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhoaHocController : Controller
    {
        private string url = "http://localhost:5005/api/khoahocs/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            List<KhoaHoc> khoahoc = new List<KhoaHoc>();

            // Lấy danh sách khoa học từ API
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<KhoaHoc>>(result);
                if (data != null)
                {
                    khoahoc = data;
                }
            }

            // Tìm kiếm
            ViewBag.SearchQuery = searchQuery;
            if (!String.IsNullOrEmpty(searchQuery))
            {
                // Tìm kiếm theo IdKh, nếu tìm thấy thì vào trang chi tiết
                var khoaHocById = khoahoc.FirstOrDefault(k => k.IdKh.Equals(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (khoaHocById != null)
                {
                    return RedirectToAction("Details", new { id = khoaHocById.IdKh });
                }

                // Tìm kiếm theo TenKh hoặc MoTa
                khoahoc = khoahoc.Where(k => k.TenKh.Contains(searchQuery) || k.MoTa.Contains(searchQuery)).ToList();
            }

            // Sắp xếp
            ViewBag.IdKhSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.DonGiaSortParm = sortOrder == "DonGia" ? "donGia_desc" : "DonGia";
            ViewBag.SoLuongSortParm = sortOrder == "SoLuong" ? "soLuong_desc" : "SoLuong";
            ViewBag.ThoiHanSortParm = sortOrder == "ThoiHan" ? "thoiHan_desc" : "ThoiHan";

            switch (sortOrder)
            {
                case "id_desc":
                    khoahoc = khoahoc.OrderByDescending(k => k.IdKh).ToList();
                    break;
                case "DonGia":
                    khoahoc = khoahoc.OrderBy(k => k.DonGia).ToList();
                    break;
                case "donGia_desc":
                    khoahoc = khoahoc.OrderByDescending(k => k.DonGia).ToList();
                    break;
                case "SoLuong":
                    khoahoc = khoahoc.OrderBy(k => k.SoLuong).ToList();
                    break;
                case "soLuong_desc":
                    khoahoc = khoahoc.OrderByDescending(k => k.SoLuong).ToList();
                    break;
                case "ThoiHan":
                    khoahoc = khoahoc.OrderBy(k => k.ThoiHan).ToList();
                    break;
                case "thoiHan_desc":
                    khoahoc = khoahoc.OrderByDescending(k => k.ThoiHan).ToList();
                    break;
                default:
                    khoahoc = khoahoc.OrderBy(k => k.IdKh).ToList();
                    break;
            }

            return View(khoahoc);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KhoaHoc kh)
        {
            string data = JsonConvert.SerializeObject(kh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            KhoaHoc kh = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                kh = JsonConvert.DeserializeObject<KhoaHoc>(result);
            }

            if (kh == null)
            {
                return NotFound();
            }

            return View(kh);
        }

        // GET: Admin/NgoaiKhoa/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            KhoaHoc kh = new KhoaHoc();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<KhoaHoc>(result);
                if (data != null)
                {
                    kh = data;
                }
            }

            return View(kh);
        }

        [HttpPost]
        public IActionResult Edit(KhoaHoc kh)
        {
            string data = JsonConvert.SerializeObject(kh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + kh.IdKh, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/NgoaiKhoa/Delete/{id}
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            KhoaHoc kh = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                kh = JsonConvert.DeserializeObject<KhoaHoc>(result);
            }

            if (kh == null)
            {
                return NotFound();
            }

            return View(kh);
        }

        // POST: Admin/NgoaiKhoa/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}
