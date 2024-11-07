using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class MainController : Controller
    {
        private const string baseURL = "https://localhost:5005/api/PhuHuynhs";
        private const string baseURL2 = "https://localhost:5005/api/KhoaHocs";
        private HttpClient client = new HttpClient();
        [HttpGet]
        public ActionResult trangchu(string id)
        {
            if (id == null)
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
        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult ChiTietTinTuc(int id)
        {
            string title = "";
            string content = "";
            string image = "";
            string date = "01.11.2024";

            switch (id)
            {
                case 1:
                    title = "Tin tức về Khóa học mới";
                    content = "Chúng tôi xin thông báo về các khóa học mới sắp khai giảng dành cho trẻ...";
                    image = "~/Content/images/baotangchientich.jpg";
                    break;
                case 2:
                    title = "Thông báo Lễ hội Âm nhạc";
                    content = "Lễ hội Âm nhạc sẽ diễn ra vào cuối tháng này với nhiều hoạt động thú vị...";
                    image = "link_to_image2.jpg";
                    break;
                case 3:
                    title = "Cập nhật chương trình học";
                    content = "Chương trình học đã được cập nhật với nhiều hoạt động thú vị và bổ ích...";
                    image = "link_to_image3.jpg";
                    break;
                case 4:
                    title = "Khảo sát sự hài lòng";
                    content = "Chúng tôi mong muốn nhận được ý kiến từ quý phụ huynh...";
                    image = "link_to_image4.jpg";
                    break;
                case 5:
                    title = "Chương trình ngoại khóa sắp tới";
                    content = "Để giúp các bé phát triển toàn diện, trường sẽ tổ chức chương trình ngoại khóa...";
                    image = "link_to_image5.jpg";
                    break;
                default:
                    return RedirectToAction("TinTuc");
            }

            ViewData["Title"] = title;
            ViewData["Content"] = content;
            ViewData["Image"] = image;
            ViewData["Date"] = date;

            return View();
        }
    }
}
