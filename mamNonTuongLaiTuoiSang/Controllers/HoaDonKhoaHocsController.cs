using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using AngleSharp.Io.Dom;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonKhoaHocsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HoaDonKhoaHocsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HoaDonKhoaHocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonKhoaHoc>>> GetHoaDonKhoaHocs()
        {
          if (_context.HoaDonKhoaHocs == null)
          {
              return BadRequest();
          }
            return await _context.HoaDonKhoaHocs.ToListAsync();
        }

        //api/hoadonkhoahocs/MinSoLuong
        [HttpGet("MinSoLuong")]
        public async Task<ActionResult<IEnumerable<HoaDonKhoaHoc>>> GetHoaDonKhoaHocsMinSoLuong()
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Không có dữ liệu hóa đơn khóa học.");
            }

            // Lấy số lượng nhỏ nhất
            var minSoLuong = await _context.HoaDonKhoaHocs
                .MinAsync(hkh => hkh.SoLuong);

            // Lấy tất cả hóa đơn khóa học có số lượng bằng số lượng nhỏ nhất
            var hoaDonKhoaHocsMin = await _context.HoaDonKhoaHocs
                .Where(hkh => hkh.SoLuong == minSoLuong)
                .ToListAsync();

            // Trả về danh sách hóa đơn khóa học
            return hoaDonKhoaHocsMin;
        }


        //api/hoadonkhoahocs/MaxSoLuong
        [HttpGet("MaxSoLuong")]
        public async Task<ActionResult<IEnumerable<HoaDonKhoaHoc>>> GetHoaDonKhoaHocsMaxSoLuong()
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Không có dữ liệu hóa đơn khóa học.");
            }

            // Lấy số lượng nhỏ nhất
            var maxSoLuong = await _context.HoaDonKhoaHocs
                .MaxAsync(hkh => hkh.SoLuong);

            // Lấy tất cả hóa đơn khóa học có số lượng bằng số lượng nhỏ nhất
            var hoaDonKhoaHocsMin = await _context.HoaDonKhoaHocs
                .Where(hkh => hkh.SoLuong == maxSoLuong)
                .ToListAsync();

            // Trả về danh sách hóa đơn khóa học
            return hoaDonKhoaHocsMin;
        }

        // GET: api/hoadonkhoahocs/MaxSoLuong/{date}
        [HttpGet("MaxSoLuong/{date}")]
        public async Task<ActionResult<IEnumerable<object>>> GetHoaDonKhoaHocsMaxSoLuongNgayhethan(DateTime date)
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Không có dữ liệu hóa đơn khóa học.");
            }

            // Lấy số lượng lớn nhất trong ngày hết hạn đã cho
            var maxSoLuong = await _context.HoaDonKhoaHocs
                .Where(hkh => hkh.IdHdNavigation.NgayHetHan.HasValue && hkh.IdHdNavigation.NgayHetHan.Value.Date == date.Date) // So sánh theo Ngày hết hạn
                .MaxAsync(hkh => hkh.SoLuong);

            // Lấy tất cả hóa đơn khóa học có số lượng bằng số lượng lớn nhất trong ngày hết hạn
            var hoaDonKhoaHocsMax = await _context.HoaDonKhoaHocs
                .Include(hkh => hkh.IdHdNavigation) // Bao gồm thông tin hóa đơn
                .Where(hkh => hkh.IdHdNavigation.NgayHetHan.HasValue && hkh.IdHdNavigation.NgayHetHan.Value.Date == date.Date && hkh.SoLuong == maxSoLuong)
                .Select(hkh => new
                {
                    hkh.IdKh,
                    hkh.IdHd,
                    hkh.SoLuong,
                    hkh.NgayDk,
                    hkh.NgayKt,
                    TrangThai = hkh.IdHdNavigation.TrangThai,  // Lấy thông tin trạng thái từ bảng HoaDon
                    NgayHetHan = hkh.IdHdNavigation.NgayHetHan // Bao gồm Ngày hết hạn
                })
                .ToListAsync();

            // Kiểm tra xem có hóa đơn nào không
            if (hoaDonKhoaHocsMax.Count == 0)
            {
                return BadRequest("Không tìm thấy hóa đơn khóa học nào trong ngày hết hạn đã cho.");
            }

            // Trả về danh sách hóa đơn khóa học cùng với trạng thái và ngày hết hạn
            return Ok(hoaDonKhoaHocsMax);
        }

        // GET: api/hoadonkhoahocs/MinSoLuong/{date}
        [HttpGet("MinSoLuong/{date}")]
        public async Task<ActionResult<IEnumerable<object>>> GetHoaDonKhoaHocsMinSoLuongNgayhethan(DateTime date)
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Không có dữ liệu hóa đơn khóa học.");
            }

            // Lấy số lượng lớn nhất trong ngày hết hạn đã cho
            var minSoLuong = await _context.HoaDonKhoaHocs
                .Where(hkh => hkh.IdHdNavigation.NgayHetHan.HasValue && hkh.IdHdNavigation.NgayHetHan.Value.Date == date.Date) // So sánh theo Ngày hết hạn
                .MinAsync(hkh => hkh.SoLuong);

            // Lấy tất cả hóa đơn khóa học có số lượng bằng số lượng lớn nhất trong ngày hết hạn
            var hoaDonKhoaHocsMax = await _context.HoaDonKhoaHocs
                .Include(hkh => hkh.IdHdNavigation) // Bao gồm thông tin hóa đơn
                .Where(hkh => hkh.IdHdNavigation.NgayHetHan.HasValue && hkh.IdHdNavigation.NgayHetHan.Value.Date == date.Date && hkh.SoLuong == minSoLuong)
                .Select(hkh => new
                {
                    hkh.IdKh,
                    hkh.IdHd,
                    hkh.SoLuong,
                    hkh.NgayDk,
                    hkh.NgayKt,
                    TrangThai = hkh.IdHdNavigation.TrangThai,  // Lấy thông tin trạng thái từ bảng HoaDon
                    NgayHetHan = hkh.IdHdNavigation.NgayHetHan // Bao gồm Ngày hết hạn
                })
                .ToListAsync();

            // Kiểm tra xem có hóa đơn nào không
            if (hoaDonKhoaHocsMax.Count == 0)
            {
                return BadRequest("Không tìm thấy hóa đơn khóa học nào trong ngày hết hạn đã cho.");
            }

            // Trả về danh sách hóa đơn khóa học cùng với trạng thái và ngày hết hạn
            return Ok(hoaDonKhoaHocsMax);
        }



        [HttpGet("{IdKH}/{IdHD}")]
        public async Task<ActionResult<object>> GetHoaDonKhoaHocByIdKHIdHD(string IdKH, string IdHD)
        {
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Không có dữ liệu hóa đơn khóa học.");
            }

            // Truy vấn dữ liệu từ bảng HoaDonKhoaHoc theo IdKH và IdHD
            var HDKH = await _context.HoaDonKhoaHocs
                .Include(h => h.IdHdNavigation) // Bao gồm thông tin từ bảng HoaDon
                .FirstOrDefaultAsync(h => h.IdHd == IdHD && h.IdKh == IdKH);

            if (HDKH == null)
            {
                return BadRequest("Không tìm thấy hóa đơn khóa học với IdHD và IdKH được cung cấp.");
            }

            // Chuyển đổi sang DTO để trả về dữ liệu hợp lý
            var HDKHDto = new
            {
                IdHd = HDKH.IdHd,
                IdKh = HDKH.IdKh,
                SoLuong = HDKH.SoLuong,
                NgayDk = HDKH.NgayDk,
                NgayKt = HDKH.NgayKt,
                TrangThai = HDKH.IdHdNavigation.TrangThai,  // Lấy thông tin trạng thái từ bảng HoaDon
                NgayHetHan = HDKH.IdHdNavigation.NgayHetHan // Lấy thông tin Ngày hết hạn từ bảng HoaDon
            };

            return Ok(HDKHDto);
        }




        // PUT: api/HoaDonKhoaHocs/{IdKh}/{IdHd}
        [HttpPut("{IdKh}/{IdHd}")]
        public async Task<IActionResult> PutHoaDonKhoaHoc(string IdKh, string IdHd, HoaDonKhoaHoc hoaDonKhoaHoc)
        {
            // Tìm bản ghi HoaDonKhoaHoc theo IdKh và IdHd
            var existingHoaDonKhoaHoc = await _context.HoaDonKhoaHocs
                .Include(hkh => hkh.IdHdNavigation)  // Bao gồm thông tin từ HoaDon
                .Include(hkh => hkh.IdKhNavigation)  // Bao gồm thông tin từ KhoaHoc
                .FirstOrDefaultAsync(hkh => hkh.IdKh == IdKh && hkh.IdHd == IdHd);

            if (existingHoaDonKhoaHoc == null)
            {
                return BadRequest("Không tìm thấy hóa đơn khóa học với IdKh và IdHd được cung cấp.");
            }

            // Cập nhật thông tin của bản ghi HoaDonKhoaHoc với dữ liệu mới
            existingHoaDonKhoaHoc.SoLuong = hoaDonKhoaHoc.SoLuong;
            existingHoaDonKhoaHoc.NgayDk = hoaDonKhoaHoc.NgayDk;
            existingHoaDonKhoaHoc.NgayKt = hoaDonKhoaHoc.NgayKt;

            // Đánh dấu trạng thái modified cho bản ghi HoaDonKhoaHoc
            _context.Entry(existingHoaDonKhoaHoc).State = EntityState.Modified;
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý lỗi đồng bộ hóa nếu có xung đột
                if (!HoaDonKhoaHocExists(IdKh, IdHd))
                {
                    return BadRequest("Hóa đơn khóa học không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về mã 204 nếu cập nhật thành công
        }
        // POST: api/HoaDonKhoaHocs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HoaDonKhoaHoc>> PostHoaDonKhoaHoc(HoaDonKhoaHoc hoaDonKhoaHoc)
        {
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Entity set 'QLMamNonContext.HoaDonKhoaHocs' is null.");
            }

            // Kiểm tra sự tồn tại của bản ghi với IdHd và IdKh
            if (HoaDonKhoaHocExists(hoaDonKhoaHoc.IdKh, hoaDonKhoaHoc.IdHd))
            {
                return Conflict("Hóa đơn khóa học đã tồn tại với IdKh và IdHd đã cho.");
            }

            // Thêm bản ghi mới vào DbSet
            _context.HoaDonKhoaHocs.Add(hoaDonKhoaHoc);
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Xử lý lỗi nếu có xung đột trong việc thêm bản ghi
                throw;
            }

            // Trả về kết quả 201 Created cùng với thông tin bản ghi vừa thêm
            return CreatedAtAction("GetHoaDonKhoaHoc", new { IdKh = hoaDonKhoaHoc.IdKh, IdHd = hoaDonKhoaHoc.IdHd }, hoaDonKhoaHoc);
        }

        // DELETE: api/HoaDonKhoaHocs/IdKh/IdHd
        [HttpDelete("{IdKh}/{IdHd}")]
        public async Task<IActionResult> DeleteHoaDonKhoaHoc(string IdKh, string IdHd)
        {
            if (_context.HoaDonKhoaHocs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm bản ghi HoaDonKhoaHoc theo IdKh và IdHd
            var hoaDonKhoaHoc = await _context.HoaDonKhoaHocs
                .FirstOrDefaultAsync(hd => hd.IdKh == IdKh && hd.IdHd == IdHd);

            if (hoaDonKhoaHoc == null)
            {
                return BadRequest("Không tìm thấy hóa đơn khóa học với IdKh và IdHd được cung cấp.");
            }

            // Xóa bản ghi HoaDonKhoaHoc
            _context.HoaDonKhoaHocs.Remove(hoaDonKhoaHoc);
            await _context.SaveChangesAsync();

            return NoContent(); // Trả về mã 204 nếu xóa thành công
        }
        private bool HoaDonKhoaHocExists(string idKh, string idHd)
        {
            return _context.HoaDonKhoaHocs.Any(hkh => hkh.IdKh == idKh && hkh.IdHd == idHd);
        }
    }
}
