using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.CompilerServices;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class PhuhuynhController : Controller
    {
        private const string baseURL = "https://localhost:5005/api/PhuHuynhs";
        private const string baseURL2 = "https://localhost:5005/api/KhoaHocs";
        private HttpClient client = new HttpClient();
        [HttpGet]
        public IActionResult PhuHuynh(PhuHuynh ph)
        {
            TempData["PhuHuynh"] = ph.IdPh;
            ViewData["PhuHuynh"] = ph.IdPh;
            return View(ph);
        }
        [HttpGet]
        public IActionResult Info(string id)
        {
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            PhuHuynh ph = new PhuHuynh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            PhuHuynh ph = new PhuHuynh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PhuHuynh ph)
        {

            string data = JsonConvert.SerializeObject(ph);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(baseURL + "/" + ph.IdPh, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Info", new { id = ph.IdPh });
            }
            else
            {
                string errorContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(errorContent); // In ra lỗi chi tiết từ API
            }
            return View(ph);
        }

        [HttpGet]
        public IActionResult KhoaHoc(string id)
        {
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
            List<KhoaHoc> kh = new List<KhoaHoc>();
            HttpResponseMessage response = client.GetAsync(baseURL2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<KhoaHoc>>(result);
                if (data != null)
                {
                    kh = data;
                }
            }
            return View(kh);
        }
    }
}
