using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongBaoPhuHuynhController : Controller
    {
        private readonly QLMamNonContext _context;

        public ThongBaoPhuHuynhController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/ThongBaoPhuHuynh
        public async Task<IActionResult> Index()
        {
            var thongBaoPhuHuynhs = _context.ThongBao_PhuHuynhs
                .Include(tb => tb.IdTitleNavigation)
                .Include(tb => tb.IdPhNavigation);
            return View(await thongBaoPhuHuynhs.ToListAsync());
        }

        // GET: Admin/ThongBaoPhuHuynh/Details/5
        public async Task<IActionResult> Details(int idTitle, string idPH)
        {
            var thongBaoPhuHuynh = await _context.ThongBao_PhuHuynhs
                .Include(tb => tb.IdTitleNavigation)
                .Include(tb => tb.IdPhNavigation)
                .FirstOrDefaultAsync(m => m.IdTitle == idTitle && m.IdPH == idPH);

            if (thongBaoPhuHuynh == null)
            {
                return NotFound();
            }

            return View(thongBaoPhuHuynh);
        }

        // GET: Admin/ThongBaoPhuHuynh/Create
        public IActionResult Create()
        {
            ViewBag.IdTitle = new SelectList(_context.ThongBaos, "IdTitle", "Title");
            ViewBag.IdPH = GetIdPhSelectList();
            return View();
        }

        // POST: Admin/ThongBaoPhuHuynh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThongBao_PhuHuynh thongBaoPhuHuynh)
        {
            // Kiểm tra xem cặp IdTitle và IdPH đã tồn tại trong database chưa
            var exists = await _context.ThongBao_PhuHuynhs
                .AnyAsync(tb => tb.IdTitle == thongBaoPhuHuynh.IdTitle && tb.IdPH == thongBaoPhuHuynh.IdPH);

            if (exists)
            {
                // Nếu tồn tại, thêm thông báo lỗi và trả về view
                ModelState.AddModelError(string.Empty, "Cặp IdTitle và IdPH này đã tồn tại.");
                return View(thongBaoPhuHuynh);
            }

            // Nếu IsRead là null, đặt giá trị mặc định là false
            thongBaoPhuHuynh.IsRead ??= false;

            if (ModelState.IsValid)
            {
                _context.Add(thongBaoPhuHuynh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdTitle = new SelectList(_context.ThongBaos, "IdTitle", "Title", thongBaoPhuHuynh.IdTitle);
            ViewBag.IdPH = GetIdPhSelectList();
            return View(thongBaoPhuHuynh);
        }

        // GET: Admin/ThongBaoPhuHuynh/Edit/5
        public async Task<IActionResult> Edit(int idTitle, string idPH)
        {
            var thongBaoPhuHuynh = await _context.ThongBao_PhuHuynhs
                .FirstOrDefaultAsync(m => m.IdTitle == idTitle && m.IdPH == idPH);

            if (thongBaoPhuHuynh == null)
            {
                return NotFound();
            }

            ViewBag.IdTitle = new SelectList(_context.ThongBaos, "IdTitle", "Title", thongBaoPhuHuynh.IdTitle);
            ViewBag.IdPH = GetIdPhSelectList();
            return View(thongBaoPhuHuynh);
        }

        // POST: Admin/ThongBaoPhuHuynh/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idTitle, string idPH, ThongBao_PhuHuynh thongBaoPhuHuynh)
        {
            if (idTitle != thongBaoPhuHuynh.IdTitle || idPH != thongBaoPhuHuynh.IdPH)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thongBaoPhuHuynh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongBaoPhuHuynhExists(thongBaoPhuHuynh.IdTitle, thongBaoPhuHuynh.IdPH))
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

            ViewBag.IdTitle = new SelectList(_context.ThongBaos, "IdTitle", "Title", thongBaoPhuHuynh.IdTitle);
            ViewBag.IdPH = GetIdPhSelectList();
            return View(thongBaoPhuHuynh);
        }

        // GET: Admin/ThongBaoPhuHuynh/Delete/5
        public async Task<IActionResult> Delete(int idTitle, string idPH)
        {
            var thongBaoPhuHuynh = await _context.ThongBao_PhuHuynhs
                .Include(tb => tb.IdPhNavigation)
                .Include(tb => tb.IdTitleNavigation)
                .FirstOrDefaultAsync(m => m.IdTitle == idTitle && m.IdPH == idPH);

            if (thongBaoPhuHuynh == null)
            {
                return NotFound();
            }

            return View(thongBaoPhuHuynh);
        }

        // POST: Admin/ThongBaoPhuHuynh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idTitle, string idPH)
        {
            var thongBaoPhuHuynh = await _context.ThongBao_PhuHuynhs
                .FirstOrDefaultAsync(m => m.IdTitle == idTitle && m.IdPH == idPH);

            if (thongBaoPhuHuynh != null)
            {
                _context.ThongBao_PhuHuynhs.Remove(thongBaoPhuHuynh);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ThongBaoPhuHuynhExists(int idTitle, string idPH)
        {
            return _context.ThongBao_PhuHuynhs.Any(e => e.IdTitle == idTitle && e.IdPH == idPH);
        }

        private IEnumerable<SelectListItem> GetIdPhSelectList()
        {
            var phuHuynh = _context.PhuHuynhs.ToList();
            var selectListItems = phuHuynh.Select(ph => new SelectListItem
            {
                Value = ph.IdPh,
                Text = $"{ph.HoTen} ({ph.IdPh})"
            }).ToList();

            return selectListItems;
        }

        public IActionResult CreateMany()
        {
            var lopList = _context.Lops.ToList();
            var titleList = _context.ThongBaos.ToList();

            ViewData["LopList"] = lopList;
            ViewData["TitleList"] = titleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMany(string IdLop, int IdTitle, IFormFile excelFile)
        {
            ViewData["LopList"] = _context.Lops.ToList();
            ViewData["TitleList"] = _context.ThongBaos.ToList();

            List<string> idPhList = new List<string>();
            try
            {
                // Kiểm tra nếu người dùng chọn lớp
                if (!string.IsNullOrEmpty(IdLop))
                {
                    // Lấy tất cả các học sinh thuộc lớp được chọn
                    var hocSinhLops = _context.HocSinhLops
                                               .Where(h => h.IdLop == IdLop)
                                               .Select(h => h.IdHs)
                                               .ToList();

                    // Lấy danh sách phụ huynh từ bảng HocSinh dựa trên các IdHs
                    idPhList = _context.HocSinhs
                                       .Where(h => hocSinhLops.Contains(h.IdHs))
                                       .Select(h => h.IdPh)
                                       .Distinct()
                                       .ToList();
                }
                else if (excelFile != null && excelFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await excelFile.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2, bỏ qua tiêu đề
                            {
                                string idPh = worksheet.Cells[row, 1].Text;
                                if (!string.IsNullOrEmpty(idPh))
                                {
                                    idPhList.Add(idPh);
                                }
                            }
                        }
                    }
                }

                if (idPhList.Count == 0)
                {
                    ModelState.AddModelError("", "Không tìm thấy phụ huynh để gửi thông báo.");
                    return View();
                }
                
                var thongBaoPhuHuynhs = idPhList.Select(idPh => new ThongBao_PhuHuynh
                {
                    IdPH = idPh,
                    IdTitle = IdTitle,
                    IsRead = false // Giá trị mặc định IsRead là false
                }).ToList();

                
                _context.ThongBao_PhuHuynhs.AddRange(thongBaoPhuHuynhs);
                await _context.SaveChangesAsync();

                
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View();
            }
            
        }

    }
}
