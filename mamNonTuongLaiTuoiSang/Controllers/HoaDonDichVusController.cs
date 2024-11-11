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
    public class HoaDonDichVusController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HoaDonDichVusController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HoaDonDichVus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonDichVu>>> GetHoaDonDichVus()
        {
            if (_context.HoaDonDichVus == null)
            {
                return BadRequest();
            }
            return await _context.HoaDonDichVus.ToListAsync();
        }

        // GET: api/HoaDonDichVus/{idHd}/{idDv}
        [HttpGet("{idHd}/{idDv}")]
        public async Task<ActionResult<HoaDonDichVu>> GetHoaDonDichVu(string idHd, string idDv)
        {
            var hoaDonDichVu = await _context.HoaDonDichVus
                .FirstOrDefaultAsync(hd => hd.IdHd == idHd && hd.IdDv == idDv);

            if (hoaDonDichVu == null)
            {
                return BadRequest("Không tìm thấy dữ liệu với mã hóa đơn và mã dịch vụ đã cho.");
            }

            return Ok(hoaDonDichVu);
        }
        // GET: api/HoaDonDichVus/MostUsed
        [HttpGet("MostUsed")]
        public async Task<ActionResult<IEnumerable<object>>> GetMostUsedHoaDonDichVu()
        {
            var mostUsedHoaDonDichVu = await _context.HoaDonDichVus
                .GroupBy(hd => hd.IdDv)
                .Select(g => new
                {
                    IdDv = g.Key,
                    TotalSoLuong = g.Sum(hd => hd.SoLuong ?? 0) // Tính tổng số lượng
                })
                .OrderByDescending(hd => hd.TotalSoLuong) // Sắp xếp giảm dần theo tổng số lượng
                .ToListAsync();

            return Ok(mostUsedHoaDonDichVu);
        }

        // GET: api/HoaDonDichVus/LeastUsed
        [HttpGet("LeastUsed")]
        public async Task<ActionResult<IEnumerable<object>>> GetLeastUsedHoaDonDichVu()
        {
            var leastUsedHoaDonDichVu = await _context.HoaDonDichVus
                .GroupBy(hd => hd.IdDv)
                .Select(g => new
                {
                    IdDv = g.Key,
                    TotalSoLuong = g.Sum(hd => hd.SoLuong ?? 0) // Tính tổng số lượng, thay null bằng 0
                })
                .OrderBy(hd => hd.TotalSoLuong) // Sắp xếp tăng dần theo tổng số lượng
                .ToListAsync();

            return Ok(leastUsedHoaDonDichVu);
        }



        // PUT: api/HoaDonDichVu/{idHd}/{idDv}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idHd}/{idDv}")]
        public async Task<IActionResult> PutHoaDonDichVu(string idHd, string idDv, HoaDonDichVu hoaDonDichVu)
        {
            _context.Entry(hoaDonDichVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonDichVuExists(idHd, idDv))
                {
                    return NotFound("Dữ liệu không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/HoaDonDichVu
        [HttpPost]
        public async Task<ActionResult<HoaDonDichVu>> PostHoaDonDichVu(HoaDonDichVu hoaDonDichVu)
        {
            if (HoaDonDichVuExists(hoaDonDichVu.IdHd, hoaDonDichVu.IdDv))
            {
                return BadRequest("Hóa đơn dịch vụ này đã tồn tại.");
            }
            _context.HoaDonDichVus.Add(hoaDonDichVu);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetHoaDonDichVu", new { idHd = hoaDonDichVu.IdHd, idDv = hoaDonDichVu.IdDv }, hoaDonDichVu);
        }

        // DELETE: api/HoaDonDichVu/{idHd}/{idDv}
        [HttpDelete("{idHd}/{idDv}")]
        public async Task<IActionResult> DeleteHoaDonDichVu(string idHd, string idDv)
        {
            var hoaDonDichVu = await _context.HoaDonDichVus
                .FirstOrDefaultAsync(hd => hd.IdHd == idHd && hd.IdDv == idDv);

            if (hoaDonDichVu == null)
            {
                return BadRequest("Hóa đơn dịch vụ không tồn tại.");
            }
            
            _context.HoaDonDichVus.Remove(hoaDonDichVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool HoaDonDichVuExists(string idHd, string idDv)
        {
            return _context.HoaDonDichVus.Any(hd => hd.IdHd == idHd && hd.IdDv == idDv);
        }

    }
}
