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
    public class VoucherCuaPhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public VoucherCuaPhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/VoucherCuaPhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherCuaPh>>> GetVoucherCuaPhs()
        {
          if (_context.VoucherCuaPhs == null)
          {
              return NotFound();
          }
            return await _context.VoucherCuaPhs.ToListAsync();
        }

        // GET: api/VoucherCuaPhs/{idPh}/{idVoucher}
        [HttpGet("{idPh}/{idVoucher}")]
        public async Task<ActionResult<VoucherCuaPh>> GetVoucherCuaPh(string idPh, string idVoucher)
        {
            if (_context.VoucherCuaPhs == null)
            {
                return NotFound("Dữ liệu không tồn tại.");
            }

            // Tìm voucher theo hai khóa chính
            var voucherCuaPh = await _context.VoucherCuaPhs
                .FirstOrDefaultAsync(v => v.IdPh == idPh && v.IdVoucher == idVoucher);

            if (voucherCuaPh == null)
            {
                return NotFound("Không tìm thấy voucher với IdPh và IdVoucher đã cho.");
            }

            return Ok(voucherCuaPh);
        }

        // POST: api/VoucherCuaPhs
        [HttpPost]
        public async Task<ActionResult<VoucherCuaPh>> PostVoucherCuaPh(VoucherCuaPh voucherCuaPh)
        {
            if (_context.VoucherCuaPhs == null)
            {
                return Problem("Entity set 'QLMamNonContext.VoucherCuaPhs' is null.");
            }

            // Kiểm tra sự tồn tại của voucher với cùng IdPh và IdVoucher
            if (VoucherCuaPhExists(voucherCuaPh.IdPh, voucherCuaPh.IdVoucher))
            {
                return Conflict("Voucher đã tồn tại với IdPh và IdVoucher này.");
            }

            // Thêm voucher mới vào DbContext
            _context.VoucherCuaPhs.Add(voucherCuaPh);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw; // Xử lý ngoại lệ nếu cần thiết
            }

            // Trả về thông tin voucher đã thêm kèm theo địa chỉ URL của bản ghi mới
            return CreatedAtAction("GetVoucherCuaPh", new { idPh = voucherCuaPh.IdPh, idVoucher = voucherCuaPh.IdVoucher }, voucherCuaPh);
        }

        /// PUT: api/VoucherCuaPhs/{idPh}/{idVoucher}
        [HttpPut("{idPh}/{idVoucher}")]
        public async Task<IActionResult> PutVoucherCuaPh(string idPh, string idVoucher, VoucherCuaPh voucherCuaPh)
        {
            // Kiểm tra xem IdPh và IdVoucher có khớp với đối tượng voucherCuaPh không
            if (idPh != voucherCuaPh.IdPh || idVoucher != voucherCuaPh.IdVoucher)
            {
                return BadRequest("IdPh và IdVoucher không khớp với dữ liệu.");
            }

            // Đánh dấu đối tượng là đã sửa đổi
            _context.Entry(voucherCuaPh).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra xem voucher có tồn tại hay không
                if (!VoucherCuaPhExists(idPh, idVoucher))
                {
                    return NotFound("Voucher không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về 204 No Content nếu thành công
        }   

        // DELETE: api/VoucherCuaPhs/{idPh}/{idVoucher}
        [HttpDelete("{idPh}/{idVoucher}")]
        public async Task<IActionResult> DeleteVoucherCuaPh(string idPh, string idVoucher)
        {
            if (_context.VoucherCuaPhs == null)
            {
                return NotFound();
            }

            // Tìm bản ghi cần xóa
            var voucherCuaPh = await _context.VoucherCuaPhs
                .FirstOrDefaultAsync(v => v.IdPh == idPh && v.IdVoucher == idVoucher);

            // Nếu không tìm thấy bản ghi
            if (voucherCuaPh == null)
            {
                return NotFound("Không tìm thấy voucher với IdPh và IdVoucher đã cho.");
            }

            // Xóa bản ghi
            _context.VoucherCuaPhs.Remove(voucherCuaPh);
            await _context.SaveChangesAsync();

            return NoContent(); // Trả về 204 No Content nếu xóa thành công
        }
        private bool VoucherCuaPhExists(string idPh, string idVoucher)
        {
            return _context.VoucherCuaPhs.Any(e => e.IdPh == idPh && e.IdVoucher == idVoucher);
        }
    }
}
