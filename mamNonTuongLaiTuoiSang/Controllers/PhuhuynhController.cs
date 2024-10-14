using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class PhuhuynhController : Controller
    {
        [HttpGet]
        public IActionResult PhuHuynh(PhuHuynh ph)
        {
            return View(ph);
        }
    }
}
