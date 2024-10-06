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
    public class ChucVusController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public ChucVusController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/ChucVus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChucVu>>> GetChucVus()
        {
          if (_context.ChucVus == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            return await _context.ChucVus.ToListAsync();
        }

        // GET: api/ChucVus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChucVu>> GetChucVu(string id)
        {
          if (_context.ChucVus == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var chucVu = await _context.ChucVus.FindAsync(id);

            if (chucVu == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return chucVu;
        }

        // PUT: api/ChucVus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChucVu(string id, ChucVu chucVu)
        {
            if (id != chucVu.TenCv)
            {
                return BadRequest();
            }

            _context.Entry(chucVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChucVuExists(id))
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

        // POST: api/ChucVus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChucVu>> PostChucVu(ChucVu chucVu)
        {
          if (_context.ChucVus == null)
          {
              return Problem("Entity set 'QLMamNonContext.ChucVus'  is null.");
          }
            _context.ChucVus.Add(chucVu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChucVuExists(chucVu.TenCv))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChucVu", new { id = chucVu.TenCv }, chucVu);
        }

        // DELETE: api/ChucVus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChucVu(string id)
        {
            if (_context.ChucVus == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.ChucVus.Remove(chucVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChucVuExists(string id)
        {
            return (_context.ChucVus?.Any(e => e.TenCv == id)).GetValueOrDefault();
        }
    }
}
