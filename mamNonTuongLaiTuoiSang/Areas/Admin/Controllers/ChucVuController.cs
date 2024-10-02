using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChucVuController : Controller
    {
        private readonly QLMamNonContext _context;
        private readonly ILogger<ChucVuController> _logger;

        public ChucVuController(QLMamNonContext context, ILogger<ChucVuController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Admin/ChucVu
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChucVus.ToListAsync());
        }

        // GET: Admin/ChucVu/Details/a/a
        public async Task<IActionResult> Details(string TenCV, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCV) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == TenCV && m.ViTri == ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }

            return View(chucVu);
        }

        // GET: Admin/ChucVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChucVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenCv,ViTri,LuongCoBan,HeSoLuong")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chucVu);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Chức vụ mới đã được tạo.");
                return RedirectToAction(nameof(Index));
            }
            return View(chucVu);
        }

        // GET: Admin/ChucVu/Edit/a/a
        public async Task<IActionResult> Edit(string TenCV, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCV) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == TenCV && m.ViTri == ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVu/Edit/a/a
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string TenCV, string ViTri, [Bind("TenCv,ViTri,LuongCoBan,HeSoLuong")] ChucVu chucVu)
        {
            if (TenCV != chucVu.TenCv || ViTri != chucVu.ViTri)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chucVu);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Chức vụ đã được cập nhật.");
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

        // GET: Admin/ChucVu/Delete/a/a
        [HttpGet("Admin/ChucVu/Delete/{TenCv}/{ViTri}")]
        public async Task<IActionResult> Delete(string TenCV, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCV) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == TenCV && m.ViTri == ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }

            return View(chucVu);
        }

        // POST: Admin/ChucVu/DeleteConfirmed
        [HttpPost("Admin/ChucVu/DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("TenCv,ViTri")] ChucVu chucVu)
        {
            var existingChucVu = await _context.ChucVus
                .FirstOrDefaultAsync(m => m.TenCv == chucVu.TenCv && m.ViTri == chucVu.ViTri);
            if (existingChucVu == null)
            {
                return NotFound();
            }

            _context.ChucVus.Remove(existingChucVu);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Chức vụ đã được xóa.");
            return RedirectToAction(nameof(Index));
        }

        private bool ChucVuExists(string TenCV, string ViTri)
        {
            return _context.ChucVus.Any(e => e.TenCv == TenCV && e.ViTri == ViTri);
        }
    }
}
