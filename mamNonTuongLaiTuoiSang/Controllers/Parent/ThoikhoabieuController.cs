using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace mamNonTuongLaiTuoiSang.Controllers.Parent
{
    public class ThoikhoabieuController : Controller
    {
        private readonly string urlPh = "http://localhost:5005/api/HocSinhs/ByPhuHuynh/";
        private readonly string urlHs = "http://localhost:5005/api/HocSinhs/";
        private readonly string urlTk = "http://localhost:5005/api/Thoikhoabieus/";
        private HttpClient client = new HttpClient();
        private readonly string urlLop = "http://localhost:5005/api/Lops/";

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewData["PhuHuynh"] = id;
            TempData["PhuHuynh"] = id;
            List<HocSinh> hocSinhs = new List<HocSinh>();
            HttpResponseMessage response = client.GetAsync(urlPh + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinh>>(result);
                if (data != null)
                {
                    hocSinhs = data;
                }

            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            ViewData["IdHocSinh"] = new SelectList(hocSinhs, "IdHs", "TenHs", "Chưa chọn");
            return View();
        }
    }
}
