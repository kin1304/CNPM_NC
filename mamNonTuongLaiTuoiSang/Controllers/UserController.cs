using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            return RedirectToAction("Index", "User"); 
        }
        public IActionResult Index()
        {
            return View("User");
        }
    }
}
