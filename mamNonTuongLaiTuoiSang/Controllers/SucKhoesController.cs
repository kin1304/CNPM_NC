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
    public class SucKhoesController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public SucKhoesController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/SucKhoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucKhoe>>> GetSucKhoes()
        {
          
            return await _context.SucKhoes.ToListAsync();
        }

        // GET: api/SucKhoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SucKhoe>> GetSucKhoe(int id)
        {
            if (_context.SucKhoes == null)
            {
                return BadRequest();
            }
            var sucKhoe = await _context.SucKhoes.FindAsync(id);

            if (sucKhoe == null)
            {
                return BadRequest();
            }

            return sucKhoe;
        }
       
        //api/SucKhoes/HS123
        [HttpGet("last/{id}")]
        public async Task<ActionResult<SucKhoe>> GetHealthRecords(string id)
        {
            
            var currentDate = DateTime.Now;

            
            var healthRecords = await _context.SucKhoes
                .Where(h => h.IdHS == id && h.NgayNhap <= currentDate.Date)
                .OrderByDescending(h => h.NgayNhap) 
                .ToListAsync(); 

            if (healthRecords.Any()) 
            {
                
                var latestHealthRecord = healthRecords.First();

               
                var earlierHealthRecords = healthRecords
                    .Where(h => h.NgayNhap < latestHealthRecord.NgayNhap)
                    .ToList(); 

                
                var result = new
                {
                    LatestRecord = new
                    {
                        IdHS = latestHealthRecord.IdHS,
                        ChieuCao = latestHealthRecord.ChieuCao,
                        CanNang = latestHealthRecord.CanNang,
                        NgayNhap = latestHealthRecord.NgayNhap
                    },
                    EarlierRecords = earlierHealthRecords.Select(h => new
                    {
                        IdHS = h.IdHS,
                        ChieuCao = h.ChieuCao,
                        CanNang = h.CanNang,
                        NgayNhap = h.NgayNhap
                    }).ToList()
                };

                return Ok(result); 
            }

            return NotFound("Không tìm thấy thông tin sức khỏe cho học sinh.");
        }


        // PUT: api/SucKhoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSucKhoe(int id, SucKhoe sucKhoe)
        {
            if (id != sucKhoe.IdSK)
            {
                return BadRequest();
            }

            _context.Entry(sucKhoe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SucKhoeExists(id))
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

        // POST: api/SucKhoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SucKhoe>> PostSucKhoe(SucKhoe sucKhoe)
        {
          if (_context.SucKhoes == null)
          {
              return Problem("Entity set 'QLMamNonContext.SucKhoes'  is null.");
          }
            _context.SucKhoes.Add(sucKhoe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SucKhoeExists(sucKhoe.IdSK))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSucKhoe", new { id = sucKhoe.IdSK }, sucKhoe);
        }

        // DELETE: api/SucKhoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSucKhoe(int  id)
        {
            if (_context.SucKhoes == null)
            {
                return BadRequest();
            }
            var sucKhoe = await _context.SucKhoes.FindAsync(id);
            if (sucKhoe == null)
            {
                return BadRequest();
            }

            _context.SucKhoes.Remove(sucKhoe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SucKhoeExists(int id)
        {
            return (_context.SucKhoes?.Any(e => e.IdSK == id)).GetValueOrDefault();
        }
    }
}
