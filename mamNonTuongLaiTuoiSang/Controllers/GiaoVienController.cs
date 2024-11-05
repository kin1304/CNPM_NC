﻿using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;


namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class GiaoVienController : Controller
    {
        private readonly QLMamNonContext db = new QLMamNonContext();
        public IActionResult Index(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            GiaoVien gv = db.GiaoViens.SingleOrDefault(gv => gv.MaSt == id);
            try
            {
                gv.MaStNavigation = db.NhanViens.SingleOrDefault(nv => nv.MaSt == id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            ViewData["GiaoVien"] = id;
            return View(gv);
        }
    }
}
