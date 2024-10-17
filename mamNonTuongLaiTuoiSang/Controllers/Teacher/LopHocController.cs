using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Controllers.Teacher
{
    public class LopHocController : Controller
    {
        private string url = "http://localhost:5005/api/hocsinhs/";
        private string urlPH = "http://localhost:5005/api/phuhuynhs/";
        private HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            List<HocSinh> hs = new List<HocSinh>(); 
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode) 
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data=JsonConvert.DeserializeObject<List<HocSinh>>(result);
                if (data != null) 
                {
                    hs = data;
                }
            }
            return View(hs);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            HocSinh dd = new HocSinh();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    dd = data;
                }
            }
            ViewBag.IdPh = await GetIdPhSelectListAsync();
            return View(dd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HocSinh dd)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(dd);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url+dd.IdHs, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo điểm danh.");
                }
            }

            // Nếu ModelState không hợp lệ hoặc API trả về lỗi, tái định nghĩa lại ViewBag
            ViewBag.IdHs = await GetIdPhSelectListAsync();
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetIdPhSelectListAsync()
        {
            List<PhuHuynh> ph = new List<PhuHuynh>();
            HttpResponseMessage response = await client.GetAsync(urlPH);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ph = JsonConvert.DeserializeObject<List<PhuHuynh>>(result) ?? new List<PhuHuynh>();
            }

            return ph.Select(l => new SelectListItem
            {
                Value = l.IdPh, // Giá trị bạn muốn gửi về
                Text = $"({l.IdPh}) {l.HoTen}" // Hiển thị tên lớp
            }).ToList();
        }
    }
}
