using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private readonly QLMamNonContext _context;
        private readonly ILogger<NhanVienController> _logger;

        public NhanVienController(QLMamNonContext context, ILogger<NhanVienController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy danh sách nhân viên từ cơ sở dữ liệu
                var nhanViens = await _context.NhanViens
                    .Include(nv => nv.ChucVu) // Nếu ChucVu là navigation property
                    .ToListAsync();

                // Kiểm tra nếu danh sách null, tạo một danh sách rỗng
                if (nhanViens == null)
                {
                    nhanViens = new List<NhanVien>();
                }

                return View(nhanViens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching NhanViens.");
                ViewBag.ErrorMessage = "An error occurred while fetching data.";
                return View(new List<NhanVien>());
            }
        }

        // GET: Admin/NhanVien/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Lấy danh sách ChucVu để hiển thị trong dropdown
                var chucVuList = await _context.ChucVus
                    .Select(cv => new
                    {
                        cv.TenCv,
                        cv.ViTri
                    })
                    .ToListAsync();

                // Tạo danh sách SelectList cho dropdown
                ViewBag.ChucVuList = new SelectList(chucVuList, "TenCv", "TenCv");
                ViewBag.ViTriList = new SelectList(chucVuList, "ViTri", "ViTri");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Create view.");
                ViewBag.ErrorMessage = "An error occurred while preparing the Create view.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/NhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nhanVien);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("NhanVien created successfully.");
                    return RedirectToAction(nameof(Index));
                }

                // Nếu model không hợp lệ, nạp lại danh sách ChucVu
                var chucVuList = await _context.ChucVus
                    .Select(cv => new
                    {
                        cv.TenCv,
                        cv.ViTri
                    })
                    .ToListAsync();

                ViewBag.ChucVuList = new SelectList(chucVuList, "TenCv", "TenCv");
                ViewBag.ViTriList = new SelectList(chucVuList, "ViTri", "ViTri");

                return View(nhanVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating NhanVien.");
                ViewBag.ErrorMessage = "An error occurred while creating the employee.";

                var chucVuList = await _context.ChucVus
                    .Select(cv => new
                    {
                        cv.TenCv,
                        cv.ViTri
                    })
                    .ToListAsync();

                ViewBag.ChucVuList = new SelectList(chucVuList, "TenCv", "TenCv");
                ViewBag.ViTriList = new SelectList(chucVuList, "ViTri", "ViTri");

                return View(nhanVien);
            }
        }

    }
}
