using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class GiaoDienChinhSuaThongTinHocSinhController : Controller
    {
        public IActionResult Index()
        {
            return View("EditBabyLayout");
        }
    }
}
