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
    public class PhuHuynhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public PhuHuynhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/PhuHuynhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhuHuynh>>> GetPhuHuynhs()
        {
          if (_context.PhuHuynhs == null)
          {
              return NotFound();
          } 
            return await _context.PhuHuynhs.ToListAsync();
        }

        // GET: api/PhuHuynhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhuHuynh>> GetPhuHuynh(string id)
        {
          if (_context.PhuHuynhs == null)
          {
              return NotFound();
          }
            var phuHuynh = await _context.PhuHuynhs.FindAsync(id);

            if (phuHuynh == null)
            {
                return NotFound();
            }

            return phuHuynh;
        }

        // GET: api/PhuHuynhs/{id}/hocsinhs
        [HttpGet("{id}/hocsinhs")]
        public async Task<ActionResult<IEnumerable<HocSinh>>> GetHocSinhsByPhuHuynhId(string id)
        {
            if (_context.PhuHuynhs == null || _context.HocSinhs == null)
            {
                return NotFound("Dữ liệu phụ huynh hoặc học sinh không tồn tại.");
            }

            try
            {
                // Tìm phụ huynh theo mã ID
                var phuHuynh = await _context.PhuHuynhs
                    .Include(ph => ph.HocSinhs)  // Giả sử PhuHuynh có quan hệ với HocSinh
                    .FirstOrDefaultAsync(ph => ph.IdPh == id);

                if (phuHuynh == null)
                {
                    return NotFound($"Không tìm thấy phụ huynh với mã {id}.");
                }

                // Lấy danh sách học sinh liên quan đến phụ huynh
                var hocSinhs = phuHuynh.HocSinhs;

                if (hocSinhs == null || !hocSinhs.Any())
                {
                    return NotFound($"Không tìm thấy học sinh nào liên quan đến phụ huynh có mã {id}.");
                }
                return Ok(hocSinhs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }


        // PUT: api/PhuHuynhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhuHuynh(string id, PhuHuynh phuHuynh)
        {
           
            _context.Entry(phuHuynh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhuHuynhExists(id))
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

        // POST: api/PhuHuynhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhuHuynh>> PostPhuHuynh(PhuHuynh phuHuynh)
        {
          if (_context.PhuHuynhs == null)
          {
              return Problem("Entity set 'QLMamNonContext.PhuHuynhs'  is null.");
          }
            _context.PhuHuynhs.Add(phuHuynh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PhuHuynhExists(phuHuynh.IdPh))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPhuHuynh", new { id = phuHuynh.IdPh }, phuHuynh);
        }

        // DELETE: api/PhuHuynhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhuHuynh(string id)
        {
            if (_context.PhuHuynhs == null)
            {
                return NotFound();
            }
            var phuHuynh = await _context.PhuHuynhs.FindAsync(id);
            if (phuHuynh == null)
            {
                return NotFound();
            }

            _context.PhuHuynhs.Remove(phuHuynh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhuHuynhExists(string id)
        {
            return (_context.PhuHuynhs?.Any(e => e.IdPh == id)).GetValueOrDefault();
        }
    }
}
