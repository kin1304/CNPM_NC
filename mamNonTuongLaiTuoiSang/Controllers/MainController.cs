using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class MainController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/PhuHuynhs";
        private const string baseURL2 = "http://localhost:5005/api/KhoaHocs";
        private HttpClient client = new HttpClient();
        [HttpGet]
        public ActionResult trangchu(string id)
        { 
            if (id == null || id == "Login")
            {
                return View();
            }
            HttpContext.Session.SetString("PhuHuynh", id);
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
            HttpContext.Session.SetString("TenPh", ph.HoTen);
            ViewData["TenPh"] = ph.HoTen;
            return View();
        }

        public ActionResult NgoaiKhoa()
        {
            string phId = HttpContext.Session.GetString("PhuHuynh");
            if(phId == null)
            {
                return View();
            }
            ViewData["PhuHuynh"] = phId;
            ViewData["TenPh"] = HttpContext.Session.GetString("TenPh");
            return View();
        }
        public ActionResult KhoaHoc()
        {
            string phId = HttpContext.Session.GetString("PhuHuynh");
            if (phId != null)
            {
                ViewData["PhuHuynh"] = phId;
                ViewData["TenPh"] = HttpContext.Session.GetString("TenPh");
            }

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
        public ActionResult ChiTietKhoaHoc(string id)
        {
            string phId = HttpContext.Session.GetString("PhuHuynh");
            if (phId != null)
            {
                ViewData["PhuHuynh"] = phId;
                ViewData["TenPh"] = HttpContext.Session.GetString("TenPh");
            }
            KhoaHoc kh = new KhoaHoc();
            HttpResponseMessage response = client.GetAsync(baseURL2 + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<KhoaHoc>(result);
                if (data != null)
                {
                    kh = data;
                }
            }
            return View(kh);
        }
    }
}
