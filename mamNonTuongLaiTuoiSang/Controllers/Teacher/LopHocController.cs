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
        private string urlLop = "http://localhost:5005/api/lops/getLopByMaSt/";
        private string urldslop = "http://localhost:5005/api/HocSinhLops/lop/";
        private string urlSk = "http://localhost:5005/api/suckhoes/";
        private HttpClient client = new HttpClient();

        private readonly QLMamNonContext _context = new QLMamNonContext();

        public IActionResult Index(string id)
        {
            List<Lop> lops = new List<Lop>();
            HttpResponseMessage response = client.GetAsync(urlLop + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Lop>>(result);
                if (data != null)
                {
                    lops = data;
                }
            }
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            ViewData["Lop"] = HttpContext.Session.GetString("Lop");
            return View(lops);
        }
        //public IActionResult Index()
        //{
        //    List<HocSinh> hs = new List<HocSinh>(); 
        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    if (response.IsSuccessStatusCode) 
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        var data=JsonConvert.DeserializeObject<List<HocSinh>>(result);
        //        if (data != null) 
        //        {
        //            hs = data;
        //        }
        //    }
        //    return View(hs);
        //}

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            ViewData["Lop"] = HttpContext.Session.GetString("Lop");
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            ViewData["HocSinh"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SucKhoe dd)
        {
            ViewData["Lop"] = HttpContext.Session.GetString("Lop");
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            if (ModelState.IsValid)
            {
                dd.NgayNhap= DateTime.Now;
                string data = JsonConvert.SerializeObject(dd);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlSk, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("DanhSachLop", new { id = ViewData["Lop"] });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo điểm danh.");
                }
            }
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
        public IActionResult DanhSachLop(string id)
        {
            List<HocSinhLop> hsl = new List<HocSinhLop>();
            HttpResponseMessage response = client.GetAsync(urldslop + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinhLop>>(result);
                if (data != null)
                {
                    hsl = data;
                }
            }
            List<SucKhoe> hs = new List<SucKhoe>();
            for (int i = 0; i < hsl.Count; i++)
            {
                HttpResponseMessage response1 = client.GetAsync(url + hsl[i].IdHs).Result;
                if (response1.IsSuccessStatusCode)
                {
                    string result = response1.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<HocSinh>(result);
                    if (data != null)
                    {
                        SucKhoe sk = _context.SucKhoes.Where(s => s.IdHS == hsl[i].IdHs).FirstOrDefault();
                        sk.HocSinh = data;
                        hs.Add(sk);
                    }
                }
            }
            if (HttpContext.Session.GetString("Lop") == null)
            {
                HttpContext.Session.SetString("Lop", id);
            }
            ViewData["GiaoVien"] = HttpContext.Session.GetString("GiaoVien");
            ViewData["Lop"] = HttpContext.Session.GetString("Lop");
            return View(hs);
        }
    }
}
