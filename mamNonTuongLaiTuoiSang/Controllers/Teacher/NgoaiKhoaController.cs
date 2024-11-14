using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Razor.Language;

namespace mamNonTuongLaiTuoiSang.Controllers.Teacher
{
    public class NgoaiKhoaController : Controller
    {
        private readonly QLMamNonContext _context;
        private string url = "http://localhost:5005/api/ngoaikhoas/";
        private string urlNV = "http://localhost:5005/api/nhanviens/";
        private string urlNKNV = "http://localhost:5005/api/NgoaiKhoaGiaoViens/";
        private HttpClient client = new HttpClient();

        public NgoaiKhoaController(QLMamNonContext context)
        {
            _context = context;
        }
        
        // GET: NgoaiKhoa  
        public async Task<IActionResult> Index(string id)
        {
            TempData["GiaoVien"] = id;
            ViewData["GiaoVien"] = id;
            List<NgoaiKhoa> ngoaikhoas = new List<NgoaiKhoa>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<NgoaiKhoa>>(result);
                if (data != null)
                {
                    ngoaikhoas = data;
                }
            }
            return View(ngoaikhoas);
        }
        public async Task<IActionResult> DangKyNK(string maST,string id)
        {
            TempData["GiaoVien"] = id;
            ViewData["GiaoVien"] = id;
            NgoaiKhoaGiaoVien registration = new NgoaiKhoaGiaoVien
            {
                IdNk = id,
                MaSt = maST

            };

            var jsonContent = JsonConvert.SerializeObject(registration);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(urlNKNV, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = maST });
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            return RedirectToAction("Index", new { id = maST });
        }
        private bool NgoaiKhoaExists(string id)
        {
          return (_context.NgoaiKhoas?.Any(e => e.IdNk == id)).GetValueOrDefault();
        }
    }
}
