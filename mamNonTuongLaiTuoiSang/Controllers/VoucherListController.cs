using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class VoucherListController : Controller
    {
        public IActionResult VoucherList()
        {
            return View();
        }
    }
}
