using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers.Teacher
{
    public class NgoaiKhoaGiaoVienController : Controller
    {
        private readonly QLMamNonContext _context;
        private string url = "http://localhost:5005/api/NgoaiKhoaGiaoViens/";
        private string urlByGV = "http://localhost:5005/api/NgoaiKhoaGiaoViens/bygiaovien/";
        private HttpClient client = new HttpClient();

        public NgoaiKhoaGiaoVienController(QLMamNonContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string id)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            List<NgoaiKhoaGiaoVien> ngoaikhoagiaoviens = new List<NgoaiKhoaGiaoVien>();
            HttpResponseMessage response = await client.GetAsync(urlByGV+id);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<NgoaiKhoaGiaoVien>>(result);
                if (data != null)
                {
                    ngoaikhoagiaoviens = data;
                }
            }
            else
            {
                string responseResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseResult);
            }
            return View(ngoaikhoagiaoviens);
        }
        public async Task<IActionResult> Delete(string MaSt, string IdNk)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            if (string.IsNullOrEmpty(MaSt) || string.IsNullOrEmpty(IdNk))
            {
                return NotFound();
            }

            NgoaiKhoaGiaoVien nkgv = await FindNKGV(MaSt, IdNk);
            if (nkgv == null)
            {
                return NotFound();
            }
            return View(nkgv);
        }

        // POST: Admin/Tkb/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string MaSt, string IdNk)
        {
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            string apiUrl = $"{url}{MaSt}/{IdNk}";
            HttpResponseMessage response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index",new { id = MaSt } );
            }

            // Nếu có lỗi, thêm thông báo lỗi vào ModelState và trả về View
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa chức vụ.");
            NgoaiKhoaGiaoVien nkgv = await FindNKGV(MaSt, IdNk);
            return View(nkgv);
        }


        private bool NgoaiKhoaGiaoVienExists(string id)
        {
          return (_context.NgoaiKhoaGiaoViens?.Any(e => e.IdNk == id)).GetValueOrDefault();
        }
        public async Task<NgoaiKhoaGiaoVien> FindNKGV(string MaSt, string IdNk)
        {
            using (var NKGV = new HttpClient())
            {
                string path = $"{url}{MaSt}/{IdNk}";
                NKGV.DefaultRequestHeaders.Accept.Clear();
                NKGV.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await NKGV.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<NgoaiKhoaGiaoVien>(data);
                    return response;
                }
                else
                {
                    string responseContent = await getData.Content.ReadAsStringAsync();
                    Console.WriteLine("Content: " + responseContent);
                }
            }
            return null;
        }
    }
}
