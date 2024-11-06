using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiaoVienController : Controller
    {
        private string url = "http://localhost:5005/api/giaoviens/";
        private string urlNhanVien = "http://localhost:5005/api/nhanviens/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<GiaoVien> gv = new List<GiaoVien>();

            // Lấy danh sách khoa học từ API
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<GiaoVien>>(result);
                if (data != null)
                {
                    gv = data;
                }
            }
            return View(gv);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaSt = GetMaStSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiaoVien hd)
        {
            string data = JsonConvert.SerializeObject(hd);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(errorContent); // In ra lỗi chi tiết từ API

            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
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

            return View(gv);
        }

        [HttpPost]
        public IActionResult Edit(GiaoVien gv)
        {
            string data = JsonConvert.SerializeObject(gv);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + gv.MaSt, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Details(string id)
        {
            GiaoVien hsl = new GiaoVien();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<GiaoVien>(result);
                if (data != null)
                {
                    hsl = data;
                }
            }
            return View(hsl);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            GiaoVien gv = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                gv = JsonConvert.DeserializeObject<GiaoVien>(result);
            }

            if (gv == null)
            {
                return NotFound();
            }

            return View(gv);
        }

        // POST: Admin/NgoaiKhoa/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Delete", new { id = id });
            }
        }


        private IEnumerable<SelectListItem> GetMaStSelectList()
        {
            List<NhanVien> nhanViens = new List<NhanVien>();
            HttpResponseMessage response = client.GetAsync(urlNhanVien).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(result);
            }

            // Chuyển đổi danh sách NhanVien thành danh sách SelectListItem
            var selectListItems = nhanViens.Select(nv => new SelectListItem
            {
                Value = nv.MaSt, // Giá trị bạn muốn gửi về
                Text = $" ({nv.MaSt})" + nv.HoTen// Hiển thị tên nhân viên
            }).ToList();

            return selectListItems;
        }
    }
}
