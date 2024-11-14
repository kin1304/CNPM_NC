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
        [HttpGet("ByGiaoVien/{MaSt}")]
        public async Task<ActionResult<IEnumerable<NgoaiKhoaGiaoVien>>> GetNgoaiKhoaByGiaoVien(string MaSt)
        {
            if (_context.NgoaiKhoas == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm tất cả học sinh có mã phụ huynh tương ứng
            var ngoaikhoa = await _context.NgoaiKhoaGiaoViens
                .Where(hs => hs.MaSt.Replace(" ", "") == MaSt.Replace(" ", ""))
                .ToListAsync();

            if (ngoaikhoa == null || ngoaikhoa.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return ngoaikhoa;
        }
        [HttpGet("{MaSt}/{idNk}")]
        public async Task<ActionResult<NgoaiKhoaGiaoVien>> GetNKAndGV(string Mast,string idnk)
        {
            var nkgv=  _context.NgoaiKhoaGiaoViens
                .Where(ng=>ng.MaSt==Mast && ng.IdNk==idnk).FirstOrDefault();
            if (nkgv == null)
            {
                return null;
            }
            return nkgv;
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


        [HttpPost]
        public async Task<ActionResult<NgoaiKhoaGiaoVien>> PostNgoaiKhoa(NgoaiKhoaGiaoVien ngoaiKhoa)
        {

            _context.NgoaiKhoaGiaoViens.Add(ngoaiKhoa);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NgoaiKhoaGiaoVienExists(ngoaiKhoa.IdNk, ngoaiKhoa.MaSt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetNgoaiKhoa", new { id = ngoaiKhoa.IdNk, mast = ngoaiKhoa.MaSt }, ngoaiKhoa);
        }
        [HttpDelete("{MaSt}/{idNk}")]
        public async Task<IActionResult> DeleteNgoaiKhoaGiaoVien(string maSt, string idNk)
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

        private bool NgoaiKhoaGiaoVienExists(string id,string mast)
        {
            return (_context.NgoaiKhoaGiaoViens?.Any(e => e.IdNk == id && e.MaSt==mast)).GetValueOrDefault();
        }
    }
}

