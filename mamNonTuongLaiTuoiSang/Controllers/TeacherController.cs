using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
