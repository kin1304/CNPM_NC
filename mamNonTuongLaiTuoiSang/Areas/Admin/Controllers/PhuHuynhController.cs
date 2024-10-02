using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhuHuynhController : Controller
    {
        private readonly QLMamNonContext _context;

        public PhuHuynhController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/PhuHuynh
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["HoTenSortParam"] = String.IsNullOrEmpty(sortOrder) ? "hoten_desc" : "";
            ViewData["NamSinhSortParam"] = sortOrder == "namsinh_asc" ? "namsinh_desc" : "namsinh_asc";

            var phuHuynhs = from ph in _context.PhuHuynhs
                            select ph;

            switch (sortOrder)
            {
                case "hoten_desc":
                    phuHuynhs = phuHuynhs.OrderByDescending(ph => ph.HoTen);
                    break;
                case "namsinh_asc":
                    phuHuynhs = phuHuynhs.OrderBy(ph => ph.NamSinh);
                    break;
                case "namsinh_desc":
                    phuHuynhs = phuHuynhs.OrderByDescending(ph => ph.NamSinh);
                    break;
                default:
                    phuHuynhs = phuHuynhs.OrderBy(ph => ph.HoTen);
                    break;
            }

            return View(await phuHuynhs.AsNoTracking().ToListAsync());
        }

        // GET: Admin/PhuHuynh/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.PhuHuynhs == null)
            {
                return NotFound();
            }

            var phuHuynh = await _context.PhuHuynhs
                .FirstOrDefaultAsync(m => m.IdPh == id);
            if (phuHuynh == null)
            {
                return NotFound();
            }

            return View(phuHuynh);
        }

        // GET: Admin/PhuHuynh/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PhuHuynh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPh,HoTen,DiaChi,GioiTinh,NgheNghiep,NamSinh,MatKhau,Email,Sdt")] PhuHuynh phuHuynh)
        {
            // Kiểm tra xem IdPH đã tồn tại hay chưa
            if (_context.PhuHuynhs.Any(ph => ph.IdPh == phuHuynh.IdPh))
            {
                ModelState.AddModelError("IdPh", "ID Phụ Huynh đã tồn tại. Vui lòng chọn ID khác.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(phuHuynh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phuHuynh);
        }

        // GET: Admin/PhuHuynh/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.PhuHuynhs == null)
            {
                return NotFound();
            }

            var phuHuynh = await _context.PhuHuynhs.FindAsync(id);
            if (phuHuynh == null)
            {
                return NotFound();
            }
            return View(phuHuynh);
        }

        // POST: Admin/PhuHuynh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPh,HoTen,DiaChi,GioiTinh,NgheNghiep,NamSinh,MatKhau,Email,Sdt")] PhuHuynh phuHuynh)
        {
            if (id != phuHuynh.IdPh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phuHuynh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhuHuynhExists(phuHuynh.IdPh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(phuHuynh);
        }

        // GET: Admin/PhuHuynh/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.PhuHuynhs == null)
            {
                return NotFound();
            }

            var phuHuynh = await _context.PhuHuynhs
                .FirstOrDefaultAsync(m => m.IdPh == id);
            if (phuHuynh == null)
            {
                return NotFound();
            }

            return View(phuHuynh);
        }

        // POST: Admin/PhuHuynh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                if (_context.PhuHuynhs == null)
                {
                    return Problem("Entity set 'QLMamNonContext.PhuHuynhs' is null.");
                }

                var phuHuynh = await _context.PhuHuynhs.FindAsync(id);
                if (phuHuynh == null)
                {
                    TempData["ErrorMessage"] = "Phụ huynh không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                _context.PhuHuynhs.Remove(phuHuynh);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa phụ huynh. Vui lòng thử lại.";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        private bool PhuHuynhExists(string id)
        {
          return (_context.PhuHuynhs?.Any(e => e.IdPh == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var phuHuynh = await _context.PhuHuynhs
                .FirstOrDefaultAsync(ph => ph.HoTen.Contains(searchQuery) || ph.Sdt.Contains(searchQuery));

            if (phuHuynh == null)
            {
                return NotFound(); // Handle case where no parent is found
            }

            // Redirect to the details page of the found parent
            return RedirectToAction("Details", new { id = phuHuynh.IdPh });
        }

    }
}
