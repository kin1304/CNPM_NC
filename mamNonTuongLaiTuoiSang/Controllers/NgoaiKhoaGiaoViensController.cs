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
    public class NgoaiKhoaGiaoViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public NgoaiKhoaGiaoViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/NgoaiKhoaGiaoViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NgoaiKhoaGiaoVien>>> GetNgoaiKhoaGiaoViens()
        {
          
            return await _context.NgoaiKhoaGiaoViens.ToListAsync();
        }

        // GET: api/NgoaiKhoaGiaoViens/{idNk}
        [HttpGet("{idNk}")]
        public async Task<ActionResult> GetGiaoViensByIdNk(string idNk)
        {
           
            // Lấy thông tin giáo viên dựa trên IdNk
            var giaoViens = await _context.NgoaiKhoaGiaoViens
                .Where(nkgv => nkgv.IdNk == idNk)
                .Select(nkgv => new
                {
                    IdNk = nkgv.IdNk,
                    GiaoVien = new
                    {
                        nkgv.MaStNavigation.MaSt,
                        nkgv.MaStNavigation.TrinhDoChuyenMon,
                        nkgv.MaStNavigation.SaoDanhGia
                    }
                })
                .ToListAsync();

            // Nếu không có giáo viên nào
            if (giaoViens == null || !giaoViens.Any())
            {
                return NotFound("Không tìm thấy giáo viên cho IdNk đã cho.");
            }

            return Ok(giaoViens);
        }
    }


}

