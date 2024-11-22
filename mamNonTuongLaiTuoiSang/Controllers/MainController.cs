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
        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult ChiTietTinTuc(int id)
        {
            var newsDetails = new Dictionary<int, dynamic>
            {
                 { 1, new { title = "Tin tức về Khóa học mới", description = "Khóa học này sẽ tập trung vào phát triển kỹ năng giao tiếp," +
                 " tư duy sáng tạo, và khả năng tự lập cho các bé. Các buổi học sẽ bao gồm các hoạt động đa dạng như hội thảo kỹ năng mềm," +
                 " các trò chơi tư duy và các bài học ngoại khóa.",
                     image = "~/Content/images/KHM.jpg" } },
                 { 2, new { title = "Thông báo Lễ hội Âm nhạc", description = "Lễ hội Âm nhạc sẽ diễn ra vào cuối tháng này tại sân trường, " +
                 "với sự tham gia của các nghệ sĩ khách mời." +
                 " Đây là dịp để các bé khám phá âm nhạc và phát triển tài năng nghệ thuật.", 
                     image = "~/Content/images/lhan.jpg" } },
                 { 3, new { title = "Cập nhật chương trình học", description = "Chương trình học đã được cập nhật với nhiều hoạt động thú vị và bổ ích.", image = "~/Content/images/cncth.jpg" } },
                 { 4, new { title = "Khảo sát sự hài lòng", description = "Chúng tôi mong muốn nhận được ý kiến từ quý phụ huynh để nâng cao chất lượng chương trình giảng dạy.", image = "~/Content/images/ks.jpg" } },
                 { 5, new { title = "Chương trình ngoại khóa sắp tới", description = "Nhằm giúp các bé phát triển toàn diện, trường sẽ tổ chức một chương trình ngoại khóa vào cuối tháng này.", image = "~/Content/images/nk.jpg" } }
            };

            // Kiểm tra xem ID có tồn tại trong từ điển hay không
            if (newsDetails.TryGetValue(id, out var news))
            {
                return View(news); // Trả về view với dữ liệu chi tiết
            }

            // Nếu không tìm thấy ID, trả về trang 404 Not Found
            return NotFound();
        }
    }
}
