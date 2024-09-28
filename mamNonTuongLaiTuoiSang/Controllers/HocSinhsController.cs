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
    public class HocSinhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HocSinhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HocSinhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HocSinh>>> GetHocSinhs()
        {
          if (_context.HocSinhs == null)
          {
              return NotFound();
          }
            return await _context.HocSinhs.ToListAsync();
        }

        // GET: api/HocSinhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HocSinh>> GetHocSinh(string id)
        {
          if (_context.HocSinhs == null)
          {
              return NotFound();
          }
            var hocSinh = await _context.HocSinhs.FindAsync(id);

            if (hocSinh == null)
            {
                return NotFound();
            }

            return hocSinh;
        }
        // GET: api/HocSinhs/parent/{parentId}
        [HttpGet("parent/{parentId}")]
        public async Task<ActionResult<IEnumerable<HocSinh>>> GetHocSinhsByParentId(string IdPh)
        {
            if (_context.HocSinhs == null)
            {
                return NotFound();
            }

            // Tìm kiếm danh sách học sinh theo mã phụ huynh
            var hocSinhs = await _context.HocSinhs
                .Where(hs => hs.IdPh == IdPh)
                .ToListAsync();

            if (hocSinhs == null || !hocSinhs.Any())
            {
                return NotFound("Không tìm thấy học sinh nào với mã phụ huynh này.");
            }

            return Ok(hocSinhs);
        }

        // PUT: api/HocSinhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHocSinh(string id, HocSinh hocSinh)
        {
            if (id != hocSinh.IdHs)
            {
                return BadRequest();
            }

            _context.Entry(hocSinh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocSinhExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HocSinhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HocSinh>> PostHocSinh(HocSinh hocSinh)
        {
          if (_context.HocSinhs == null)
          {
              return Problem("Entity set 'QLMamNonContext.HocSinhs'  is null.");
          }
            _context.HocSinhs.Add(hocSinh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HocSinhExists(hocSinh.IdHs))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHocSinh", new { id = hocSinh.IdHs }, hocSinh);
        }

        // DELETE: api/HocSinhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHocSinh(string id)
        {
            if (_context.HocSinhs == null)
            {
                return NotFound();
            }
            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh == null)
            {
                return NotFound();
            }

            _context.HocSinhs.Remove(hocSinh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HocSinhExists(string id)
        {
            return (_context.HocSinhs?.Any(e => e.IdHs == id)).GetValueOrDefault();
        }
    }
}
