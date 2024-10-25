using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using AngleSharp.Io.Dom;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonKhoaHocController : Controller
    {

        private string baseURL = "http://localhost:5005/api/hoadonkhoahocs";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public HoaDonKhoaHocController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDonKhoaHoc
        public async Task<IActionResult> Index(string sortOrder, int? month, string filterBy)
        {
            List<HoaDonKhoaHoc> hdkh = new List<HoaDonKhoaHoc>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HoaDonKhoaHoc>>(result);
                if (data != null)
                {
                    hdkh = data;
                }
            }

            // Filter by NgayDK or NgayKt by month
            if (month.HasValue)
            {
                if (filterBy == "NgayDK")
                {
                    hdkh = hdkh.Where(x => x.NgayDk.HasValue && x.NgayDk.Value.Month == month).ToList();
                }
                else if (filterBy == "NgayKt")
                {
                    hdkh = hdkh.Where(x => x.NgayKt.HasValue && x.NgayKt.Value.Month == month).ToList();
                }
            }

            ViewData["SoLuongSortParm"] = sortOrder == "SoLuong_Asc" ? "SoLuong_Desc" : "SoLuong_Asc";

            // Sorting by SoLuong
            switch (sortOrder)
            {
                case "SoLuong_Desc":
                    hdkh = hdkh.OrderByDescending(s => s.SoLuong).ToList();
                    break;
                case "SoLuong_Asc":
                default:
                    hdkh = hdkh.OrderBy(s => s.SoLuong).ToList();
                    break;
            }

            return View(hdkh);
        }

        // GET: Admin/HoaDonKhoaHoc/Details/5
        [HttpGet("Admin/HoaDonKhoaHoc/Details/{IdKH}/{IdHD}")]
        public async Task<IActionResult> Details(string IdKH,string IdHD)
        {
            if (string.IsNullOrEmpty(IdKH) || string.IsNullOrEmpty(IdHD))
            {
                return NotFound();
            }

            HoaDonKhoaHoc HDKH = await FindHDKH(IdKH, IdHD);
            if (HDKH== null)
            {
                return NotFound();
            }
            return View(HDKH);
        }

        // GET: Admin/HoaDonKhoaHoc/Create
        public IActionResult Create()
        {
            ViewData["IdHd"] = new SelectList(_context.HoaDons, "IdHd", "IdHd");
            ViewData["IdKh"] = new SelectList(_context.KhoaHocs, "IdKh", "IdKh");
            return View();
        }

        // POST: Admin/HoaDonKhoaHoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKh,IdHd,SoLuong,NgayDk,NgayKt")] HoaDonKhoaHoc hoaDonKhoaHoc)
        {
            string data = JsonConvert.SerializeObject(hoaDonKhoaHoc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL + "/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            ViewData["IdHd"] = new SelectList(_context.HoaDons, "IdHd", "IdHd",hoaDonKhoaHoc.IdHd);
            ViewData["IdKh"] = new SelectList(_context.KhoaHocs, "IdKh", "IdKh",hoaDonKhoaHoc.IdKh);
            return View(hoaDonKhoaHoc);
        }

        // GET: Admin/HoaDonKhoaHoc/Edit/5
        [HttpGet("Admin/HoaDonKhoaHoc/Edit/{IdKH}/{IdHD}")]
        public async Task<IActionResult> Edit(string idkh,string idhd)
        {
            HoaDonKhoaHoc hdkh = await FindHDKH(idkh, idhd);
            if (hdkh == null)
            {
                return NotFound();
            }
            ViewData["IdHd"] = new SelectList(_context.HoaDons, "IdHd", "IdHd", hdkh.IdHd);
            ViewData["IdKh"] = new SelectList(_context.KhoaHocs, "IdKh", "IdKh", hdkh.IdKh);
            return View(hdkh);
        }

        // POST: Admin/HoaDonKhoaHoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/HoaDonKhoaHoc/Edit/{IdKH}/{IdHD}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string IdKH,string IdHD, [Bind("IdKh,IdHd,SoLuong,NgayDk,NgayKt")] HoaDonKhoaHoc hoaDonKhoaHoc)
        {
            hoaDonKhoaHoc.IdKhNavigation = new KhoaHoc { IdKh = IdKH };

            string data = JsonConvert.SerializeObject(hoaDonKhoaHoc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Xây dựng URL đúng cách (loại bỏ dấu +)

            string apiUrl = $"{baseURL}/{IdKH}/{IdHD}";

            // Sử dụng async/await để tránh deadlock
            HttpResponseMessage response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseContent))
                {
                    Console.WriteLine("Content: " + responseContent);
                }
                else
                {
                    Console.WriteLine("No content returned.");
                }
            }
            ViewData["IdHd"] = new SelectList(_context.HoaDons, "IdHd", "IdHd", hoaDonKhoaHoc.IdHd);
            ViewData["IdKh"] = new SelectList(_context.KhoaHocs, "IdKh", "IdKh", hoaDonKhoaHoc.IdKh);
            return View(hoaDonKhoaHoc);
        }

        // GET: Admin/HoaDonKhoaHoc/Delete/5
        [HttpGet("Admin/HoaDonKhoaHoc/Delete/{IdKH}/{IdHD}")]
        public async Task<IActionResult> Delete(string IdKH,string IdHD)
        {
            if (string.IsNullOrEmpty(IdKH) || string.IsNullOrEmpty(IdHD))
            {
                return NotFound();
            }

            HoaDonKhoaHoc HDKH = await FindHDKH(IdKH, IdHD);
            if (HDKH == null)
            {
                return NotFound();
            }
            return View(HDKH);
        }

        // POST: Admin/HoaDonKhoaHoc/Delete/5
        [HttpPost("Admin/HoaDonKhoaHoc/Delete/{IdKH}/{IdHD}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string IdKh,string IdHD)
        {
            string apiUrl = $"{baseURL}/{IdKh}/{IdHD}";

            // Gửi yêu cầu DELETE đến API
            HttpResponseMessage response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, thêm thông báo lỗi vào ModelState và trả về View
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa chức vụ.");
            HoaDonKhoaHoc hdkh = await FindHDKH(IdKh, IdHD);
            return View(hdkh);
        }

        private bool HoaDonKhoaHocExists(string id)
        {
          return (_context.HoaDonKhoaHocs?.Any(e => e.IdKh == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index");
            }

            var hdkh = await _context.HoaDonKhoaHocs
                .Where(hDKH => hDKH.IdKh.Contains(searchQuery))
                .ToListAsync();

            if (!hdkh.Any())
            {
                return NotFound(); // You might want to handle this differently in your UI
            }

            return View("Index", hdkh); // Assuming your Index view can accept a list of HoaDonKhoaHoc
        }
        public async Task<HoaDonKhoaHoc> FindHDKH(string idkh, string idhd)
        {
            using (var tkb = new HttpClient())
            {
                string path = $"{baseURL}/{idkh}/{idhd}";
                tkb.DefaultRequestHeaders.Accept.Clear();
                tkb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await tkb.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<HoaDonKhoaHoc>(data);
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
    }
}
