using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class TreEmController : Controller
    {
        private readonly string url = "http://localhost:5005/api/HocSinhs/ByPhuHuynh/";
        private readonly string urlDetails = "http://localhost:5005/api/HocSinhs/";
        private readonly string urlPh = "http://localhost:5005/api/HocSinhs/ByPhuHuynh/";
        private readonly string urlChieuCao = "http://localhost:5005/api/HocSinhs/ChieuCao/CanNang/";

        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewData["PhuHuynh"] = id;
            TempData["PhuHuynh"] = id;
            List<HocSinh> hocSinhs = new List<HocSinh>();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
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

            return View(hocSinhs);
        }
        [HttpGet]
        public IActionResult babyinfo(string id)
        {
            ViewData["PhuHuynh"] = TempData["PhuHuynh"] as string;
            HocSinh hocSinh = new HocSinh();
            HttpResponseMessage response = client.GetAsync(urlDetails + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    hocSinh = data;
                }

            }
            CCCN cCCN = new CCCN();
            HttpResponseMessage res = client.GetAsync(urlChieuCao + id).Result;
            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<CCCN>(result);
                if (data != null)
                {
                    cCCN = data;
                }

            }
            if (cCCN != null)
            {

                ViewData["CanNang"] = cCCN.CanNang;
                ViewData["ChieuCao"] = cCCN.ChieuCao;
            }
            else
            {
                ViewData["CanNang"] = 0;
                ViewData["ChieuCao"] = 0;
            }
            TempData["PhuHuynh"] = ViewData["PhuHuynh"] as string;
            return View(hocSinh);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewData["PhuHuynh"] = TempData["PhuHuynh"] as string;
            TempData["PhuHuynh"] = ViewData["PhuHuynh"] as string;
            HocSinh hocSinh = new HocSinh();
            HttpResponseMessage response = client.GetAsync(urlDetails + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    hocSinh = data;
                }

            }
            return View(hocSinh);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(HocSinh hocSinh)
        {
            string data = JsonConvert.SerializeObject(hocSinh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(urlDetails + hocSinh.IdHs, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("babyinfo", new { id = hocSinh.IdHs });
            }
            else
            {
                string errorContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(errorContent); // In ra lỗi chi tiết từ API
                return View(hocSinh);
            }
        }
        [HttpGet]
        public IActionResult BMI(string id)
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
