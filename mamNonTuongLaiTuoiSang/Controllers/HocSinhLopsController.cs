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
    public class HocSinhLopsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HocSinhLopsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HocSinhLops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HocSinhLop>>> GetHocSinhLops()
        {
          if (_context.HocSinhLops == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }

            return await _context.HocSinhLops.ToListAsync();
        }

        // GET: api/HocSinhLops/
        [HttpGet("/ByHocSinh/{idHs}")]
        public async Task<ActionResult<HocSinhLop>> GetHocSinhLopss(string idHs)
        {
            if (_context.HocSinhLops == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var hocSinhLop = await _context.HocSinhLops
                .Where(hsl => hsl.IdHs == idHs)
                .FirstOrDefaultAsync();

            if (hocSinhLop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return hocSinhLop;
        }
        // GET: api/HocSinhLops/lop/{idLop} ( lấy mã lớp để tìm thông tin còn hàm trên lấy mã học sinh để tìm thông tin)
        [HttpGet("lop/{idLop}")]
        public async Task<ActionResult<List<HocSinhLop>>> GetHocSinhLoplop(string idLop)
        {
            if (_context.HocSinhLops == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var hocSinhLop = await _context.HocSinhLops
                .Where(hsl => hsl.IdLop == idLop)
                .ToListAsync();

            if (hocSinhLop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");

            }

            return hocSinhLop;
        }

        [HttpGet("{IdHs}/{IdLop}")]
        public async Task<ActionResult<HocSinhLop>> GetHocSinhLop(string IdHs, string IdLop)
        {
            if (_context.HocSinhLops == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var hocSinhLop = await _context.HocSinhLops
                .Where(hsl => hsl.IdHs == IdHs && hsl.IdLop == IdLop)
                .FirstOrDefaultAsync();

            if (hocSinhLop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return hocSinhLop;
        }
        // PUT: api/HocSinhLops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{IdHs}/{IdLop}")]
        public async Task<IActionResult> PutHocSinhLop(string IdHs, string IdLop, HocSinhLop hocSinhLop)
        {

            _context.Entry(hocSinhLop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocSinhLopExists(IdHs,IdLop))
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

        // POST: api/HocSinhLops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HocSinhLop>> PostHocSinhLop(HocSinhLop hocSinhLop)
        {
            _context.HocSinhLops.Add(hocSinhLop);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HocSinhLopExists(hocSinhLop.IdHs, hocSinhLop.IdLop))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHocSinhLop", new { IdHs = hocSinhLop.IdHs, IdLop = hocSinhLop.IdLop }, hocSinhLop);
        }

        // DELETE: api/HocSinhLops/5
        [HttpDelete("{IdHs}/{IdLop}")]
        public async Task<IActionResult> DeleteHocSinhLop(string IdHs, string IdLop)
        {

            if (_context.HocSinhLops == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var hocSinhLop = await _context.HocSinhLops.FirstOrDefaultAsync(h => h.IdHs == IdHs && h.IdLop == IdLop);
            if (hocSinhLop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.HocSinhLops.Remove(hocSinhLop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HocSinhLopExists(string IdHs, string IdLop)
        {
            return (_context.HocSinhLops?.Any(e => e.IdHs == IdHs && e.IdLop == IdLop)).GetValueOrDefault();
        }
    }
}

