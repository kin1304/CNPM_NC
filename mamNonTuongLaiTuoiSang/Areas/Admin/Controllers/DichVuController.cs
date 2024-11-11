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
    public class DichVuController : Controller
    {
        private readonly QLMamNonContext _context;

        public DichVuController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/DichVu
        public async Task<IActionResult> Index(string sortOrder, string searchQuery)
        {
            ViewBag.IdDvSortParam = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.DonGiaSortParam = sortOrder == "donGia" ? "donGia_desc" : "donGia";
            ViewBag.ThoiHanSortParam = sortOrder == "thoiHan" ? "thoiHan_desc" : "thoiHan";

            var dichVus = from dv in _context.DichVus
                          select dv;

            // Tìm kiếm theo IdDv
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (dichVus.Any(dv => dv.IdDv == searchQuery))
                {
                    return RedirectToAction("Details", new { id = searchQuery });
                }

                dichVus = dichVus.Where(dv => dv.TenDv.Contains(searchQuery) ||
                                              dv.MoTa.Contains(searchQuery) ||
                                              dv.ThoiHan.Contains(searchQuery));
            }

            // Sắp xếp theo cột
            switch (sortOrder)
            {
                case "id_desc":
                    dichVus = dichVus.OrderByDescending(dv => dv.IdDv);
                    break;
                case "donGia":
                    dichVus = dichVus.OrderBy(dv => dv.DonGia);
                    break;
                case "donGia_desc":
                    dichVus = dichVus.OrderByDescending(dv => dv.DonGia);
                    break;
                case "thoiHan":
                    dichVus = dichVus.OrderBy(dv => dv.ThoiHan);
                    break;
                case "thoiHan_desc":
                    dichVus = dichVus.OrderByDescending(dv => dv.ThoiHan);
                    break;
                default:
                    dichVus = dichVus.OrderBy(dv => dv.IdDv);
                    break;
            }

            return View(await dichVus.ToListAsync());
        }


        // GET: Admin/DichVu/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.IdDv == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // GET: Admin/DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DichVu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDv,TenDv,DonGia,MoTa,ThoiHan")] DichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dichVu);
        }

        // GET: Admin/DichVu/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: Admin/DichVu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDv,TenDv,DonGia,MoTa,ThoiHan")] DichVu dichVu)
        {
            if (id != dichVu.IdDv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.IdDv))
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
            return View(dichVu);
        }

        // GET: Admin/DichVu/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.IdDv == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // POST: Admin/DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DichVus == null)
            {
                return Problem("Entity set 'QLMamNonContext.DichVus'  is null.");
            }
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                _context.DichVus.Remove(dichVu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(string id)
        {
          return (_context.DichVus?.Any(e => e.IdDv == id)).GetValueOrDefault();
        }
    }
}
