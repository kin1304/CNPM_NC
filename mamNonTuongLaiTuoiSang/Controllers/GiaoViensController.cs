﻿using System;
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
    public class GiaoViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public GiaoViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/GiaoViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiaoVien>>> GetGiaoViens()
        {
          if (_context.GiaoViens == null)
          {
              return BadRequest();
          }
            return await _context.GiaoViens.ToListAsync();
        }

        /// GET: api/GiaoViens/{maSt} (hãy gán link này api/GiaoViens/St006 vì trong database mới thêm thằng này là tên công việc giáo viên)
        [HttpGet("{maSt}")]
        public async Task<ActionResult<GiaoVien>> GetNhanVienByGiaoVien(string maSt)
        {
            // Kiểm tra xem _context có null không
            if (_context.GiaoViens == null || _context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm giáo viên theo mã số
            GiaoVien giaoVien = await _context.GiaoViens
                .FirstOrDefaultAsync(g => g.MaSt == maSt);
            
            // Nếu không tìm thấy giáo viên
            if (giaoVien == null )
            {
                return BadRequest("Không tìm thấy giáo viên với mã số đã cho hoặc nhân viên không phải là giáo viên.");
            }

            // Chuyển đổi thông tin sang DTO
            if(giaoVien.MaStNavigation == null)
            {
                NhanVien nhanVien = await _context.NhanViens.FirstOrDefaultAsync(g => g.MaSt == maSt);
                giaoVien.MaStNavigation = nhanVien;
            }
            

            return Ok(giaoVien);
        }

        // PUT: api/GiaoViens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGiaoVien(string id, GiaoVien giaoVien)
        {
            _context.Entry(giaoVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiaoVienExists(id))
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

        // POST: api/GiaoViens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GiaoVien>> PostGiaoVien(GiaoVien giaoVien)
        {
            _context.GiaoViens.Add(giaoVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra xem giáo viên có tồn tại không bằng mã MaSt
                if (GiaoVienExists(giaoVien.MaSt))
                {
                    return Conflict("Giáo viên với mã này đã tồn tại.");
                }
                else
                {
                    // Log chi tiết lỗi để dễ dàng kiểm tra
                    Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
                    // Thông báo lỗi chi tiết hơn nếu cần
                    return StatusCode(500, "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }
            }

            return CreatedAtAction("GetGiaoVien", new { id = giaoVien.MaSt }, giaoVien);
        }

        // DELETE: api/GiaoViens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaoVien(string id)
        {
            if (_context.GiaoViens == null)
            {
                return NotFound();
            }
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            if (giaoVien == null)
            {
                return BadRequest();
            }

            _context.GiaoViens.Remove(giaoVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GiaoVienExists(string id)
        {
            return (_context.GiaoViens?.Any(e => e.MaSt == id)).GetValueOrDefault();
        }
    }
}
