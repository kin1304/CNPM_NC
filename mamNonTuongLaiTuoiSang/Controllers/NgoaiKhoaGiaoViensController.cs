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
    public class NgoaiKhoaGiaoViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public NgoaiKhoaGiaoViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/NgoaiKhoaGiaoViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NgoaiKhoaGiaoVien>>> GetNgoaiKhoaGiaoViens()
        {
          
            return await _context.NgoaiKhoaGiaoViens.ToListAsync();
        }

        // GET: api/NgoaiKhoaGiaoViens/{idNk}
        [HttpGet("{idNk}")]
        public async Task<ActionResult> GetGiaoViensByIdNk(string idNk)
        {
           
            // Lấy thông tin giáo viên dựa trên IdNk
            var giaoViens = await _context.NgoaiKhoaGiaoViens
                .Where(nkgv => nkgv.IdNk == idNk)
                .Select(nkgv => new
                {
                    IdNk = nkgv.IdNk,
                    GiaoVien = new
                    {
                        nkgv.MaStNavigation.MaSt,
                        nkgv.MaStNavigation.TrinhDoChuyenMon,
                        nkgv.MaStNavigation.SaoDanhGia
                    }
                })
                .ToListAsync();

            // Nếu không có giáo viên nào
            if (giaoViens == null || !giaoViens.Any())
            {
                return BadRequest("Không tìm thấy giáo viên cho IdNk đã cho.");
            }

            return Ok(giaoViens);
        }
        // PUT: api/NgoaiKhoaGiaoViens/{idNk}/{maSt}
        [HttpPut("{idNk}/{maSt}")]
        public async Task<IActionResult> PutNgoaiKhoaGiaoVien(string idNk, string maSt, NgoaiKhoaGiaoVien ngoaiKhoaGiaoVien)
        {
           
            // Đánh dấu thực thể này sẽ được sửa đổi
            _context.Entry(ngoaiKhoaGiaoVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NgoaiKhoaGiaoVienExists(idNk, maSt))
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

        // POST: api/NgoaiKhoaGiaoViens
        [HttpPost]
        public async Task<ActionResult<NgoaiKhoaGiaoVien>> PostNgoaiKhoaGiaoVien(NgoaiKhoaGiaoVien ngoaiKhoaGiaoVien)
        {
            if (_context.NgoaiKhoaGiaoViens == null)
            {
                return Problem("Entity set 'QLMamNonContext.NgoaiKhoaGiaoViens' is null.");
            }

            _context.NgoaiKhoaGiaoViens.Add(ngoaiKhoaGiaoVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NgoaiKhoaGiaoVienExists(ngoaiKhoaGiaoVien.IdNk, ngoaiKhoaGiaoVien.MaSt))
                {
                    return Conflict("NgoaiKhoaGiaoVien với IdNk và MaSt này đã tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNgoaiKhoaGiaoVien", new { idNk = ngoaiKhoaGiaoVien.IdNk, maSt = ngoaiKhoaGiaoVien.MaSt }, ngoaiKhoaGiaoVien);
        }

        // DELETE: api/NgoaiKhoaGiaoViens/{idNk}/{maSt}
        [HttpDelete("{idNk}/{maSt}")]
        public async Task<IActionResult> DeleteNgoaiKhoaGiaoVien(string idNk, string maSt)
        {
            if (_context.NgoaiKhoaGiaoViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var ngoaiKhoaGiaoVien = await _context.NgoaiKhoaGiaoViens
                .FirstOrDefaultAsync(n => n.IdNk == idNk && n.MaSt == maSt);
            if (ngoaiKhoaGiaoVien == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.NgoaiKhoaGiaoViens.Remove(ngoaiKhoaGiaoVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NgoaiKhoaGiaoVienExists(string idNk, string maSt)
        {
            return _context.NgoaiKhoaGiaoViens.Any(e => e.IdNk == idNk && e.MaSt == maSt);
        }

    }


}

