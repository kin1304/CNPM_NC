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
    public class ChucVusController : Controller
    {
        private readonly QLMamNonContext _context;

        public ChucVusController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/ChucVus
        public async Task<IActionResult> Index()
        {
              return _context.ChucVus != null ? 
                          View(await _context.ChucVus.ToListAsync()) :
                          Problem("Entity set 'QLMamNonContext.ChucVus'  is null.");
        }

        // GET: Admin/ChucVus/Details/{tenCv}/{viTri}
        public async Task<IActionResult> Details(string tenCv, string viTri)
        {
            if (tenCv == null || viTri == null || _context.ChucVus == null)
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == tenCv && m.ViTri == viTri);
            if (chucVu == null)
            {
                return NotFound();
            }

            return View(chucVu);
        }

        // GET: Admin/ChucVus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChucVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenCv,ViTri,LuongCoBan,HeSoLuong")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chucVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chucVu);
        }

        // GET: Admin/ChucVus/Edit/{tenCv}/{viTri}
        public async Task<IActionResult> Edit(string tenCv, string viTri)
        {
            if (tenCv == null || viTri == null || _context.ChucVus == null)
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus.FindAsync(tenCv, viTri);
            if (chucVu == null)
            {
                return NotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVus/Edit/{tenCv}/{viTri}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string tenCv, string viTri, [Bind("TenCv,ViTri,LuongCoBan,HeSoLuong")] ChucVu chucVu)
        {
            if (tenCv != chucVu.TenCv || viTri != chucVu.ViTri)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chucVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChucVuExists(chucVu.TenCv, chucVu.ViTri))
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
            return View(chucVu);
        }

        // GET: Admin/ChucVus/Delete/{tenCv}/{viTri}
        public async Task<IActionResult> Delete(string tenCv, string viTri)
        {
            if (tenCv == null || viTri == null || _context.ChucVus == null)
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == tenCv && m.ViTri == viTri);
            if (chucVu == null)
            {
                return NotFound();
            }

            return View(chucVu);
        }

        // POST: Admin/ChucVus/Delete/{tenCv}/{viTri}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string tenCv, string viTri)
        {
            if (_context.ChucVus == null)
            {
                return Problem("Entity set 'QLMamNonContext.ChucVus'  is null.");
            }
            var chucVu = await _context.ChucVus.FindAsync(tenCv, viTri);
            if (chucVu != null)
            {
                _context.ChucVus.Remove(chucVu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChucVuExists(string tenCv, string viTri)
        {
            return (_context.ChucVus?.Any(e => e.TenCv == tenCv && e.ViTri == viTri)).GetValueOrDefault();
        }
    }
}
