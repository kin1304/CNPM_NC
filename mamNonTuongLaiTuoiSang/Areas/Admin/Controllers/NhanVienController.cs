using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NamLamSortParm = sortOrder == "namlam_asc" ? "namlam_desc" : "namlam_asc";

            try
            {
                var nhanViens = from nv in _context.NhanViens.Include(nv => nv.ChucVu) select nv;

                switch (sortOrder)
                {
                    case "name_desc":
                        nhanViens = nhanViens.OrderByDescending(nv => nv.HoTen);
                        break;
                    case "namlam_asc":
                        nhanViens = nhanViens.OrderBy(nv => nv.NamLam);
                        break;
                    case "namlam_desc":
                        nhanViens = nhanViens.OrderByDescending(nv => nv.NamLam);
                        break;
                    default:
                        nhanViens = nhanViens.OrderBy(nv => nv.HoTen);
                        break;
                }

                return View(await nhanViens.ToListAsync());
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
                    // Check if MaSt already exists
                    bool maStExists = await _context.NhanViens.AnyAsync(nv => nv.MaSt == nhanVien.MaSt);

                    if (maStExists)
                    {
                        // If MaSt already exists, return an error message
                        ModelState.AddModelError("MaSt", "Mã số nhân viên đã tồn tại, vui lòng nhập mã số khác.");

                        // Reload ChucVu dropdowns if validation fails
                        var chucVuList = await _context.ChucVus
                            .Select(cv => new
                            {
                                cv.TenCv,
                                cv.ViTri
                            })
                            .ToListAsync();

                        ViewBag.ChucVuList = new SelectList(chucVuList, "TenCv", "TenCv");
                        ViewBag.ViTriList = new SelectList(chucVuList, "ViTri", "ViTri");

                        return View(nhanVien); // Return to the form with the error message
                    }

                    // If MaSt is unique, proceed to add the employee
                    _context.Add(nhanVien);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("NhanVien created successfully.");
                    return RedirectToAction(nameof(Index));
                }

                // If model is not valid, reload ChucVu dropdowns
                var chucVuListInvalid = await _context.ChucVus
                    .Select(cv => new
                    {
                        cv.TenCv,
                        cv.ViTri
                    })
                    .ToListAsync();

                ViewBag.ChucVuList = new SelectList(chucVuListInvalid, "TenCv", "TenCv");
                ViewBag.ViTriList = new SelectList(chucVuListInvalid, "ViTri", "ViTri");

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
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Find the NhanVien by ID
                var nhanVien = await _context.NhanViens.FindAsync(id);
                if (nhanVien == null)
                {
                    return NotFound();
                }

                // Fetch ChucVu data for dropdowns
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

                return View(nhanVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form for NhanVien ID {NhanVienId}", id);
                ViewBag.ErrorMessage = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/NhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, NhanVien nhanVien)
        {
            if (id != nhanVien.MaSt)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Reload ChucVu dropdowns if validation fails
                var chucVuList = await _context.ChucVus
                    .Select(cv => new
                    {
                        cv.TenCv,
                        cv.ViTri
                    })
                    .ToListAsync();

                ViewBag.ChucVuList = new SelectList(chucVuList, "TenCv", "TenCv", nhanVien.TenCv);
                ViewBag.ViTriList = new SelectList(chucVuList, "ViTri", "ViTri", nhanVien.ViTri);

                return View(nhanVien);
            }

            try
            {
                // Update the entity in the context and save changes
                _context.Update(nhanVien);
                await _context.SaveChangesAsync();
                _logger.LogInformation("NhanVien ID {NhanVienId} updated successfully.", id);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!NhanVienExists(nhanVien.MaSt))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating NhanVien ID {NhanVienId}", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating NhanVien ID {NhanVienId}", id);
                ViewBag.ErrorMessage = "An error occurred while updating the employee.";
                return View(nhanVien);
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(nv => nv.ChucVu) // Nạp ChucVu cùng với NhanVien
                .FirstOrDefaultAsync(nv => nv.MaSt == id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.ChucVu) // Load the ChucVu navigation property
                    .FirstOrDefaultAsync(nv => nv.MaSt == id);

                if (nhanVien == null)
                {
                    return NotFound();
                }

                return View(nhanVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete view.");
                ViewBag.ErrorMessage = "An error occurred while loading the employee details for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var nhanVien = await _context.NhanViens.FindAsync(id);
                if (nhanVien == null)
                {
                    return NotFound();
                }

                _context.NhanViens.Remove(nhanVien);
                await _context.SaveChangesAsync();

                _logger.LogInformation("NhanVien deleted successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting NhanVien.");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa nhân viên. Vui lòng thử lại.";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // Helper method to check if a NhanVien exists by ID
        private bool NhanVienExists(String id)
        {
            return _context.NhanViens.Any(e => e.MaSt == id);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(nv => nv.HoTen.Contains(searchQuery) || nv.Sdt.Contains(searchQuery));

            if (nhanVien == null)
            {
                return NotFound(); // Handle case where no employee is found
            }

            // Redirect to the details page of the found employee
            return RedirectToAction("Details", new { id = nhanVien.MaSt });
        }


    }
}
