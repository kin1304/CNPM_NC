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

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        private string url = "http://localhost:5005/api/vouchers/";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public VoucherController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/Voucher
        public async Task<IActionResult> Index(string sortOrder, string filterType, string filterValue)
        {
            List<Voucher> vouchers = new List<Voucher>();

            // Lấy danh sách khoa học từ API
            ViewBag.PhanTramGiamSortParm = sortOrder == "phantramgiam_asc" ? "phantramgiam_desc" : "phantramgiam_asc";
            ViewBag.SoLuongSortParm = sortOrder == "soluong_asc" ? "soluong_desc" : "soluong_asc";
            ViewBag.NgayTaoSortParm = sortOrder == "ngaytao_asc" ? "ngaytao_desc" : "ngaytao_asc";
            ViewBag.NgayHetHanSortParm = sortOrder == "ngayhethan_asc" ? "ngayhethan_desc" : "ngayhethan_asc";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Voucher>>(result);
                if (data != null)
                {
                    vouchers = data;
                }
            }
            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                if (filterType == "ngaytao")
                {
                    if (filterValue == "created")
                    {
                        vouchers = vouchers.Where(v => v.NgayTao <= DateTime.Now).ToList();
                    }
                    else if (filterValue == "upcoming")
                    {
                        vouchers = vouchers.Where(v => v.NgayTao > DateTime.Now).ToList();
                    }
                }
                else if (filterType == "ngayhethan")
                {
                    if (filterValue == "valid")
                    {
                        vouchers = vouchers.Where(v => v.NgayHetHan >= DateTime.Now).ToList();
                    }
                    else if (filterValue == "expired")
                    {
                        vouchers = vouchers.Where(v => v.NgayHetHan < DateTime.Now).ToList();
                    }
                }
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "phantramgiam_asc":
                    vouchers = vouchers.OrderBy(v => v.PhanTramGiam).ToList();
                    break;
                case "phantramgiam_desc":
                    vouchers = vouchers.OrderByDescending(v => v.PhanTramGiam).ToList();
                    break;
                case "soluong_asc":
                    vouchers = vouchers.OrderBy(v => v.SoLuong).ToList();
                    break;
                case "soluong_desc":
                    vouchers = vouchers.OrderByDescending(v => v.SoLuong).ToList();
                    break;
                case "ngaytao_asc":
                    vouchers = vouchers.OrderBy(v => v.NgayTao).ToList();
                    break;
                case "ngaytao_desc":
                    vouchers = vouchers.OrderByDescending(v => v.NgayTao).ToList();
                    break;
                case "ngayhethan_asc":
                    vouchers = vouchers.OrderBy(v => v.NgayHetHan).ToList();
                    break;
                case "ngayhethan_desc":
                    vouchers = vouchers.OrderByDescending(v => v.NgayHetHan).ToList();
                    break;
                default:
                    break;
            }
            return View(vouchers);
        }

        // GET: Admin/Voucher/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Voucher voucher = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                voucher = JsonConvert.DeserializeObject<Voucher>(result);
            }

            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Admin/Voucher/Create
        public IActionResult Create()
        {
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt");
            return View();
        }

        // POST: Admin/Voucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVoucher,NgayTao,NgayHetHan,PhanTramGiam,SoLuong,MaSt")] Voucher voucher)
        {
            string data = JsonConvert.SerializeObject(voucher);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt", voucher.MaSt);
            return View();
        }

        // GET: Admin/Voucher/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Voucher voucher = new Voucher();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Voucher>(result);
                if (data != null)
                {
                    voucher = data;
                }
            }
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt");
            return View(voucher);
        }

        // POST: Admin/Voucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdVoucher,NgayTao,NgayHetHan,PhanTramGiam,SoLuong,MaSt")] Voucher voucher)
        {
            string data = JsonConvert.SerializeObject(voucher);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + voucher.IdVoucher, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt", voucher.MaSt);
            return View(voucher);
        }

        // GET: Admin/Voucher/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Voucher voucher = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                voucher = JsonConvert.DeserializeObject<Voucher>(result);
            }

            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Admin/Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
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

        private bool VoucherExists(string id)
        {
          return (_context.Vouchers?.Any(e => e.IdVoucher == id)).GetValueOrDefault();
        }
    }
}
