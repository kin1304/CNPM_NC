using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public NhanViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/NhanViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
          if (_context.NhanViens == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }

            return await _context.NhanViens.ToListAsync();
        }

        // GET: api/NhanViens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
          if (_context.NhanViens == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }

            var nhanVien = await _context.NhanViens.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return nhanVien;
        }
        // GET: api/NhanViens/ByChucVu/{TenCv} ( tìm thông tin thông qua tên chức vụ)
        [HttpGet("ByChucVu/{TenCv}")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanVienbyTenCv(string TenCv)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var NhanViens = await _context.NhanViens
                .Where(Nv => Nv.TenCv == TenCv)
                .ToListAsync();

            if (NhanViens == null || NhanViens.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return NhanViens;
        }

        // GET: api/NhanViens/ByChucVu/{TenCv} ( tìm thông tin  thông qua tên vitri)
        [HttpGet("ByVitri/{vitri}")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanVienbyViTri(string vitri)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var NhanViens = await _context.NhanViens
                .Where(Nv => Nv.ViTri == vitri)
                .ToListAsync();

            if (NhanViens == null || NhanViens.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return NhanViens;
        }

        // GET: api/NhanViens/GetNhanVienHd/{MaSt}
        [HttpGet("GetNhanVienHd/{MaSt}")]
        public async Task<ActionResult<object>> GetNhanVienDetails(string MaSt)
        {
            if (_context.NhanViens == null || _context.ChucVus == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Fetch the NhanVien (Employee) based on MaSt
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.ChucVu)          // Include ChucVu for salary calculation
                .Include(nv => nv.GiaoVien)        // Include GiaoVien if this employee is also a teacher
                .FirstOrDefaultAsync(nv => nv.MaSt == MaSt);

            if (nhanVien == null)
            {
                return BadRequest("Không tìm thấy nhân viên với mã nhân viên cung cấp.");
            }

            // Calculate the salary (LuongCoBan * HeSoLuong)
            var luongCoBan = nhanVien.ChucVu?.LuongCoBan ?? 0;   // Default to 0 if null
            var heSoLuong = nhanVien.ChucVu?.HeSoLuong ?? 0;     // Default to 0 if null
            var tienluong = luongCoBan * heSoLuong;

            // Return the NhanVien (employee) information, GiaoVien (teacher) details if any, and the calculated salary
            var result = new
            {
                NhanVien = new
                {
                    nhanVien.MaSt,
                    nhanVien.HoTen,
                    nhanVien.DiaChi,
                    nhanVien.NamSinh,
                    nhanVien.GioiTinh,
                    nhanVien.Email,
                    nhanVien.Sdt,
                    nhanVien.ViTri,
                    LuongCoBan = luongCoBan,
                    HeSoLuong = heSoLuong,
                    tienluong = tienluong
                },
                GiaoVien = nhanVien.GiaoVien != null ? new
                {
                    nhanVien.GiaoVien.MaSt,
                    nhanVien.GiaoVien.TrinhDoChuyenMon,
                    nhanVien.GiaoVien.SaoDanhGia
                } : null
            };

            return Ok(result);
        }
        // PUT: api/NhanViens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(string id, NhanVien nhanVien)
        {
            _context.Entry(nhanVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(id))
                {
                    return BadRequest("Dữ liệu không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NhanViens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nhanVien)
        {

          if (_context.NhanViens == null)
          {
              return Problem("Entity set 'QLMamNonContext.NhanViens'  is null.");
          }

            _context.NhanViens.Add(nhanVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhanVienExists(nhanVien.MaSt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNhanVien", new { id = nhanVien.MaSt }, nhanVien);
        }

        // DELETE: api/NhanViens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhanVienExists(string id)
        {
            return (_context.NhanViens?.Any(e => e.MaSt == id)).GetValueOrDefault();
        }
        [HttpGet("filter")]
        public async Task<ActionResult<NhanVien>> GetNhanVienByEmailAndPassword(string email, string matKhau)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var nhanvien = await _context.NhanViens
                .FirstOrDefaultAsync(ph => ph.Email == email && ph.MatKhau == matKhau);

            if (nhanvien == null)
            {
                return NotFound("Không tìm thấy phụ huynh với email và mật khẩu này.");
            }

            return nhanvien;
        }
    }
}

