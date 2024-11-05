using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class GiaoVienController : Controller
    {
        private readonly QLMamNonContext db = new QLMamNonContext();
        private readonly string url = "https://localhost:5005/api/GiaoViens/";
        private readonly string urlNhanVien = "https://localhost:5005/api/NhanViens/GetNhanVienHd/";
        private readonly HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index(string id)
        {
            TempData["GiaoVien"] = id;
            ViewData["GiaoVien"] = id;
            GiaoVien gv = new GiaoVien();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<GiaoVien>(result);
                if (data != null)
                {
                    gv = data;
                }

            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }

            return View(gv);
        }
        public IActionResult Thunhap(string id)
        {
            TempData["GiaoVien"] = id;
            ViewData["GiaoVien"] = id;
            if (id == null)
            {
                return NotFound();
            }
            NhanVien nv = new NhanVien();
            HttpResponseMessage response = client.GetAsync(urlNhanVien + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<NhanVien>(result);
                if (data != null)
                {
                    nv = data;
                }

            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            return View(nv);
        }

    }
}
