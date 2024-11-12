using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongBaoController : Controller
    {
        private readonly QLMamNonContext _context;

        public ThongBaoController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: ThongBao
        public async Task<IActionResult> Index(string searchQuery, string sortOrder)
        {
            ViewBag.SearchQuery = searchQuery;
            ViewBag.IdTitleSortParm = sortOrder == "idtitle_asc" ? "idtitle_desc" : "idtitle_asc";
            ViewBag.NgayTaoSortParm = sortOrder == "ngaytao_asc" ? "ngaytao_desc" : "ngaytao_asc";

            var thongBaos = from tb in _context.ThongBaos
                            select tb;

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (int.TryParse(searchQuery, out int idTitle))
                {
                    var matchedThongBao = await thongBaos.FirstOrDefaultAsync(tb => tb.IdTitle == idTitle);
                    if (matchedThongBao != null)
                    {
                        return RedirectToAction("Details", new { id = idTitle });
                    }
                }
                thongBaos = thongBaos.Where(tb => tb.Title.Contains(searchQuery) || tb.NoiDung.Contains(searchQuery));
            }

            // Sắp xếp
            thongBaos = sortOrder switch
            {
                "idtitle_desc" => thongBaos.OrderByDescending(tb => tb.IdTitle),
                "idtitle_asc" => thongBaos.OrderBy(tb => tb.IdTitle),
                "ngaytao_desc" => thongBaos.OrderByDescending(tb => tb.NgayTao),
                "ngaytao_asc" => thongBaos.OrderBy(tb => tb.NgayTao),
                _ => thongBaos.OrderBy(tb => tb.IdTitle),
            };

            return View(await thongBaos.ToListAsync());
        }

        // GET: ThongBao/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var thongBao = await _context.ThongBaos
                .FirstOrDefaultAsync(m => m.IdTitle == id);
            if (thongBao == null)
            {
                return NotFound();
            }

            return View(thongBao);
        }

        // GET: ThongBao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ThongBao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTitle,Title,NoiDung,TepDinhKem")] ThongBao thongBao, IFormFile TepDinhKem)
        {
            if (ModelState.IsValid)
            {
                thongBao.NgayTao = DateTime.Now; // Thiết lập thời gian hiện tại cho NgayTao

                if (TepDinhKem != null)
                {
                    // Đường dẫn đến thư mục Content/uploads
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "uploads");
                    var filePath = Path.Combine(folderPath, TepDinhKem.FileName);

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Lưu file vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await TepDinhKem.CopyToAsync(stream);
                    }

                    // Lưu tên file vào TepDinhKem
                    thongBao.TepDinhKem = TepDinhKem.FileName;
                }


                _context.Add(thongBao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                Console.WriteLine("Error: " + error); // Hoặc sử dụng Debug để xem lỗi trong khi debug
            }

            return View(thongBao);
        }

        // GET: ThongBao/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao == null)
            {
                return NotFound();
            }
            return View(thongBao);
        }

        // POST: ThongBao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTitle,Title,NgayTao,NoiDung,TepDinhKem")] ThongBao thongBao, IFormFile TepDinhKem)
        {
            if (id != thongBao.IdTitle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (TepDinhKem != null)
                    {
                        // Đường dẫn đến thư mục Content/uploads
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "uploads");
                        var filePath = Path.Combine(folderPath, TepDinhKem.FileName);

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Lưu file vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await TepDinhKem.CopyToAsync(stream);
                        }

                        // Lưu tên file vào TepDinhKem
                        thongBao.TepDinhKem = TepDinhKem.FileName;
                    }

                    _context.Update(thongBao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongBaoExists(thongBao.IdTitle))
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
            return View(thongBao);
        }

        // GET: ThongBao/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var thongBao = await _context.ThongBaos
                .FirstOrDefaultAsync(m => m.IdTitle == id);
            if (thongBao == null)
            {
                return NotFound();
            }

            return View(thongBao);
        }

        // POST: ThongBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            _context.ThongBaos.Remove(thongBao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThongBaoExists(int id)
        {
            return _context.ThongBaos.Any(e => e.IdTitle == id);
        }
    }
}
