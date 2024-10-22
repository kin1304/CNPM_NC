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
    public class HoaDonsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HoaDonsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HoaDons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDons()
        {
          if (_context.HoaDons == null)
          {
              return BadRequest();
          }
            return await _context.HoaDons.ToListAsync();
        }

        // GET: api/HoaDons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDon(string id)
        {
          if (_context.HoaDons == null)
          {
              return BadRequest();
          }
            var hoaDon = await _context.HoaDons.FindAsync(id);

            if (hoaDon == null)
            {
                return BadRequest();
            }

            return hoaDon;
        }
        // GET: api/HoaDon/PhuHuynh/{idHs}
        [HttpGet("PhuHuynh/{idHs}")]
        public async Task<ActionResult<HoaDon>> GetHoaDonPhuHuynhByHocSinh(string idHs)
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.HoaDons == null || _context.HocSinhs == null || _context.PhuHuynhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm hóa đơn và thông tin phụ huynh dựa trên IdHS
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.IdHsNavigation)    // Bao gồm thông tin học sinh
                .ThenInclude(hs => hs.IdPhNavigation) // Bao gồm thông tin phụ huynh
                .FirstOrDefaultAsync(hd => hd.IdHs == idHs);

            // Nếu không tìm thấy hóa đơn cho mã học sinh đã cho
            if (hoaDon == null)
            {
                return BadRequest("Không tìm thấy hóa đơn hoặc thông tin phụ huynh cho mã học sinh đã cho.");
            }

            // Chuyển đổi thông tin sang DTO để trả về
            var hoaDonPhuHuynhDto = new
            {
                IdHD = hoaDon.IdHd,
                NgayLap = hoaDon.NgayLap,
                NgayHetHan = hoaDon.NgayHetHan,
                SoTien = hoaDon.SoTien,
                TrangThai = hoaDon.TrangThai,
                PhuHuynh = new
                {
                    IdPH = hoaDon.IdHsNavigation.IdPhNavigation.IdPh,
                    HoTen = hoaDon.IdHsNavigation.IdPhNavigation.HoTen,
                    Email = hoaDon.IdHsNavigation.IdPhNavigation.Email
                }
            };

            // Trả về dữ liệu DTO
            return Ok(hoaDonPhuHuynhDto);
        }

        // PUT: api/HoaDons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon(string id, HoaDon hoaDon)
        {
            _context.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HoaDons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostHoaDon(HoaDon hoaDon)
        {
          if (_context.HoaDons == null)
          {
              return Problem("Entity set 'QLMamNonContext.HoaDons'  is null.");
          }
            _context.HoaDons.Add(hoaDon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HoaDonExists(hoaDon.IdHd))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHoaDon", new { id = hoaDon.IdHd }, hoaDon);
        }

        // DELETE: api/HoaDons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon(string id)
        {
            if (_context.HoaDons == null)
            {
                return BadRequest();
            }
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return BadRequest();
            }

            _context.HoaDons.Remove(hoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HoaDonExists(string id)
        {
            return (_context.HoaDons?.Any(e => e.IdHd == id)).GetValueOrDefault();
        }
    }
}
