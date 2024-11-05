using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.IO;
using AngleSharp.Io;


namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AccountController> logger;
        private readonly HttpClient httpClient;

        private readonly string urlPh = "https://localhost:5005/api/PhuHuynhs";
        private readonly string urlNv = "https://localhost:5005/api/NhanViens";

        public AccountController(IHttpContextAccessor httpContextAccessor, ILogger<AccountController> logger, HttpClient httpClient)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.httpClient = httpClient;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            try
            {
                var session = httpContextAccessor.HttpContext.Session;
                // Kiểm tra thông tin người dùng PhuHuynh qua API
                var phuHuynhResponse = await httpClient.GetAsync($"{urlPh}/filter?email={email}&matKhau={password}");
                if (phuHuynhResponse.IsSuccessStatusCode)
                {
                    var parent = await phuHuynhResponse.Content.ReadFromJsonAsync<PhuHuynh>();
                    if (parent != null)
                    {
                        session.SetString("UserEmail", parent.Email);
                        session.SetString("Password", parent.MatKhau);
                        ViewData["PhuHuynh"] = parent.IdPh;
                        return RedirectToAction("PhuHuynh", "PhuHuynh", parent);
                    }
                }

                // Kiểm tra thông tin người dùng NhanVien qua API
                var nhanVienResponse = await httpClient.GetAsync($"{urlNv}/filter?email={email}&matKhau={password}");
                if (nhanVienResponse.IsSuccessStatusCode)
                {
                    var user = await nhanVienResponse.Content.ReadFromJsonAsync<NhanVien>();
                    if (user != null)
                    {
                        session.SetString("UserEmail", user.Email);
                        session.SetString("Password", user.MatKhau);

                        if (user.TenCv == "Admin")
                        {
                            return RedirectToAction("Index", "NhanVien", new { area = "Admin" });
                        }
                        else if (user.TenCv == "GiaoVien")
                        {
                            ViewData["GiaoVien"] = user.MaSt;
                            ViewBag.GiaoVien = user.MaSt;
                            return RedirectToAction("Index", "GiaoVien", new { id = user.MaSt });
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    string responseContent = await nhanVienResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Content: " + responseContent);
                    ViewBag.ErrorMessage = "Invalid email or password";
                    return View();
                }

                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while logging in.");
                ViewBag.ErrorMessage = "An error occurred while accessing the database.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var session = httpContextAccessor.HttpContext.Session;
            session.Clear();

            return Ok();
        }
    }
}
