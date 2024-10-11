using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class PhuhuynhController : Controller
    {
        public IActionResult Phuhuynh()
        {
            return View("PhuHuynh");
        }
    }
}
