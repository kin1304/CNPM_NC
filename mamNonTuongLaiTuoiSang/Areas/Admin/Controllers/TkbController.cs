using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using Ganss.XSS;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TkbController : Controller
    {
        private string baseURL = "http://localhost:5005/api/Tkbs";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public TkbController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/Tkb
        public async Task<IActionResult> Index()
        {
            List<Tkb> tkbs = new List<Tkb>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Tkb>>(result);
                if (data != null)
                {
                    tkbs = data;
                }
            }
            return View(tkbs);
        }

        // GET: Admin/Tkb/Details/5
        [HttpGet("Admin/Tkb/Details/{Ngay}/{IdLop}")]
        public async Task<IActionResult> Details(string id, string ngay)
        {
            Tkb tkb = new Tkb();
            HttpResponseMessage response = client.GetAsync(baseURL  + "/" + ngay+ "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Tkb>(result);
                if (data != null)
                {
                    tkb = data;
                }
            }
            return View(tkb);
        }


        // GET: Admin/Tkb/Create
        public IActionResult Create()
        {
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop");
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh");
            return View();
        }

        // POST: Admin/Tkb/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLop,Ngay,CaHoc,IdMh")] Tkb tkb)
        {
            string data = JsonConvert.SerializeObject(tkb);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View();
        }

        // GET: Admin/Tkb/Edit/5
        [HttpGet("Admin/Tkb/Edit/{IdLop}/{Ngay}")]
        public async Task<IActionResult> Edit(string id, string ngay)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(ngay))
            {
                return NotFound();
            }

            Tkb tkb = await FindTKB(id, ngay);
            if (tkb == null)
            {
                return NotFound();
            }
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View(tkb);
        }

        // POST: Admin/Tkb/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/Tkb/Edit/{IdLop}/{Ngay}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string ngay, [Bind("IdLop,Ngay,CaHoc,IdMh")] Tkb tkb)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(tkb);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Xây dựng URL đúng cách (loại bỏ dấu +)
                string apiUrl = $"{baseURL}{tkb.IdLop}/{tkb.Ngay}";

                // Sử dụng async/await để tránh deadlock
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Thêm thông báo lỗi vào ModelState để hiển thị cho người dùng
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi cập nhật chức vụ.");
                }
            }

            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View(tkb);
        }

        // GET: Admin/Tkb/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Tkb tkb = new Tkb();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Tkb>(result);
                if (data != null)
                {
                    tkb = data;
                }
            }
            return View(tkb);
        }

        // POST: Admin/Tkb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = "Deleted...";
                return RedirectToAction("Index");
            }
            return View();
        }

        private bool TkbExists(string id)
        {
            return (_context.Tkbs?.Any(e => e.IdLop == id)).GetValueOrDefault();
        }
        public async Task<Tkb> FindTKB(string id, string ngay)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(ngay))
            {
                return null;
            }

            using (var tkb = new HttpClient())
            {
                string path = $"{baseURL}/{id}/{ngay}";
                tkb.DefaultRequestHeaders.Accept.Clear();
                tkb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await tkb.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Tkb>(data);
                    return response;
                }
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index");
            }

            var tkb = await _context.Tkbs
            .FirstOrDefaultAsync(tKB => tKB.IdLop.Contains(searchQuery));

            if (tkb == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = tkb.IdLop });
        }
    }
}
