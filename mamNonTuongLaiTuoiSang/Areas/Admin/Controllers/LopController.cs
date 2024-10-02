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
    public class LopController : Controller
    {
        private readonly QLMamNonContext _context;

        public LopController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/Lop
        public async Task<IActionResult> Index(string sortOrder, string searchQuery, string siSoFilter)
        {
            // Thiết lập các tham số sắp xếp
            ViewData["TenLopSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tenlop_desc" : "";

            // Lấy danh sách lớp
            var lops = from l in _context.Lops.Include(l => l.MaStNavigation)
                       select l;

            // Kiểm tra nếu có từ khóa tìm kiếm
            if (!String.IsNullOrEmpty(searchQuery))
            {
                lops = lops.Where(l =>
                    l.IdLop.ToLower().Contains(searchQuery.ToLower().Trim()) ||
                    l.TenLop.ToLower().Contains(searchQuery.ToLower().Trim()) ||
                    l.SiSo.ToString().Contains(searchQuery.Trim()) ||
                    l.MaStNavigation.MaSt.ToLower().Contains(searchQuery.ToLower().Trim()));
            }

            // Lọc theo sĩ số nếu không phải "All"
            if (!string.IsNullOrEmpty(siSoFilter))
            {
                string sqlQuery = "SELECT * FROM Lop WHERE 1=1";

                switch (siSoFilter)
                {
                    case "lt10":
                        sqlQuery += " AND SiSo < 10";
                        break;
                    case "10-30":
                        sqlQuery += " AND SiSo >= 10 AND SiSo <= 30";
                        break;
                    case "gt30":
                        sqlQuery += " AND SiSo > 30";
                        break;
                }
                var filteredLops = await _context.Lops.FromSqlRaw(sqlQuery).ToListAsync();

                lops = lops.Where(l => filteredLops.Select(fl => fl.IdLop).Contains(l.IdLop));
            }

            // Sắp xếp theo TenLop
            switch (sortOrder)
            {
                case "tenlop_desc":
                    lops = lops.OrderByDescending(l => l.TenLop);
                    break;
                default:
                    lops = lops.OrderBy(l => l.TenLop);
                    break;
            }

            // Chuẩn bị danh sách các tùy chọn lọc sĩ số
            var siSoOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Sĩ số: All" },
                new SelectListItem { Value = "lt10", Text = "Sĩ số: <10" },
                new SelectListItem { Value = "10-30", Text = "Sĩ số: 10-30" },
                new SelectListItem { Value = "gt30", Text = "Sĩ số: >30" },
            };
            ViewBag.SiSoFilter = new SelectList(siSoOptions, "Value", "Text", siSoFilter);
            return View(await lops.ToListAsync());
        }



        // GET: Admin/Lop/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Lops == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops
                .Include(l => l.MaStNavigation)
                .FirstOrDefaultAsync(m => m.IdLop == id);
            if (lop == null)
            {
                return NotFound();
            }

            return View(lop);
        }

        // GET: Admin/Lop/Create
        public IActionResult Create()
        {
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt");
            return View();
        }

        // POST: Admin/Lop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLop,TenLop,SiSo,MaSt")] Lop lop)
        {
            // Kiểm tra nếu IdLop đã tồn tại
            var existingLop = await _context.Lops
                .FirstOrDefaultAsync(l => l.IdLop == lop.IdLop);

            if (existingLop != null)
            {
                // Thêm thông báo lỗi vào ModelState
                ModelState.AddModelError("IdLop", "Id lớp đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(lop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt", lop.MaSt);
            return View(lop);
        }


        // GET: Admin/Lop/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Lops == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return NotFound();
            }
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt", lop.MaSt);
            return View(lop);
        }

        // POST: Admin/Lop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdLop,TenLop,SiSo,MaSt")] Lop lop)
        {
            if (id != lop.IdLop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LopExists(lop.IdLop))
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
            ViewData["MaSt"] = new SelectList(_context.NhanViens, "MaSt", "MaSt", lop.MaSt);
            return View(lop);
        }

        // GET: Admin/Lop/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Lops == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops
                .Include(l => l.MaStNavigation)
                .FirstOrDefaultAsync(m => m.IdLop == id);
            if (lop == null)
            {
                return NotFound();
            }

            return View(lop);
        }

        // POST: Admin/Lop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return NotFound();
            }

            // Kiểm tra xem IdLop có được sử dụng trong Tkb hoặc HocSinhLop hay không
            bool hasTkb = await _context.Tkbs.AnyAsync(t => t.IdLop == id);
            bool hasHocSinhLop = await _context.HocSinhLops.AnyAsync(h => h.IdLop == id);

            if (hasTkb || hasHocSinhLop)
            {
                // Tạo thông báo lỗi cụ thể
                string errorMessage = "Không thể xóa lớp này vì nó đang tồn tại trong ";
                var tables = new List<string>();
                if (hasTkb) tables.Add("bảng Tkb");
                if (hasHocSinhLop) tables.Add("bảng HocSinhLop");
                errorMessage += string.Join(" hoặc ", tables) + ".";

                // Thêm thông báo lỗi vào ModelState
                ModelState.AddModelError(string.Empty, errorMessage);

                // Bao gồm navigation property để hiển thị thông tin lớp trong view
                lop = await _context.Lops
                    .Include(l => l.MaStNavigation)
                    .FirstOrDefaultAsync(m => m.IdLop == id);

                return View(lop);
            }

            // Nếu không có liên kết, thực hiện xóa
            _context.Lops.Remove(lop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool LopExists(string id)
        {
          return (_context.Lops?.Any(e => e.IdLop == id)).GetValueOrDefault();
        }
    }
}
