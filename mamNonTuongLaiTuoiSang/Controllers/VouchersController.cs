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
    public class VouchersController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public VouchersController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
        {
          if (_context.Vouchers == null)
          {
              return BadRequest();
          }
            return await _context.Vouchers.ToListAsync();
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(string id)
        {
          if (_context.Vouchers == null)
          {
              return BadRequest();
          }
            var voucher = await _context.Vouchers.FindAsync(id);

            if (voucher == null)
            {
                return BadRequest();
            }

            return voucher;
        }
        // GET: api/Vouchers/MostUsed
        [HttpGet("MostUsed")]
        public async Task<ActionResult<IEnumerable<object>>> GetMostUsedVouchers()
        {

            var mostUsedVouchers = await _context.Vouchers
                .Where(v => v.Trangthai == "đã sử dụng") //   trạng thái là đã sử dụng
                .GroupBy(v => v.IdVoucher)
                .Select(g => new
                {
                    IdVoucher = g.Key,
                    soluongdangsudung = g.Sum(v => v.SoLuong) // Tính tổng số lượng
                })
                .OrderByDescending(v => v.soluongdangsudung) // Sắp xếp giảm dần theo số lượng
                .ToListAsync();

            return Ok(mostUsedVouchers);
        }
        // GET: api/VoucherCuaPhs/LeastUsed
        [HttpGet("LeastUsed")]
        public async Task<ActionResult<IEnumerable<object>>> GetLeastUsedVouchers()
        {

            var leastUsedVouchers = await _context.Vouchers
                .Where(v => v.Trangthai == "chưa sử dụng")
                .GroupBy(v => v.IdVoucher)
                .Select(g => new
                {
                    IdVoucher = g.Key,
                    Count = g.Sum(v => v.SoLuong)
                })
                .OrderBy(v => v.Count)
                .ToListAsync();

            return Ok(leastUsedVouchers);
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(string id, Voucher voucher)
        {
            if (id != voucher.IdVoucher)
            {
                return BadRequest();
            }

            _context.Entry(voucher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(id))
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

        // POST: api/Vouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voucher>> PostVoucher(Voucher voucher)
        {
          if (_context.Vouchers == null)
          {
              return Problem("Entity set 'QLMamNonContext.Vouchers'  is null.");
          }
            _context.Vouchers.Add(voucher);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoucherExists(voucher.IdVoucher))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVoucher", new { id = voucher.IdVoucher }, voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(string id)
        {
            if (_context.Vouchers == null)
            {
                return BadRequest();
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return BadRequest();
            }

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoucherExists(string id)
        {
            return (_context.Vouchers?.Any(e => e.IdVoucher == id)).GetValueOrDefault();
        }
    }
}
