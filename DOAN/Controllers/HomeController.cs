﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using DOAN.Common;

namespace DOAN.Controllers
{
    public class HomeController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: Home
        public ActionResult Index()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            ViewBag.listTH = db.THUONGHIEUx;


            return View();
        }

        [ChildActionOnly]
        public ActionResult FeaturedBrandsPartial()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            ViewBag.listTH = db.THUONGHIEUx;
            return PartialView(listSP);
        }


        [ChildActionOnly]
        public ActionResult HotItemPartial()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            
            return PartialView(listSP);
        }

        [ChildActionOnly]
        public ActionResult MenuPartial()
        {
            var listSP = db.SANPHAMs;
            ViewBag.listSP =listSP ;
            var listLoai = db.LOAISANPHAMs;
            return PartialView(listLoai);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.ThongBao =0;
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(NGUOIDUNG user)
        {
            ViewBag.ThongBao = 0;
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (user.Mail.Trim() != "" && db.NGUOIDUNGs.SingleOrDefault(x => x.Username == user.Mail.Trim()) == null)
                {
                    user.Username = user.Mail.Trim();
                    user.Password = Encryptor.MD5Hash(user.Password);
                    user.NgayTao = DateTime.Now;
                    user.TT_User = true;
                    user.IdLoaiUser = 1;
                    db.NGUOIDUNGs.Add(user);
                    db.SaveChanges();
                    ViewBag.ThongBao = 1;
                }
                else
                    ViewBag.ThongBao = 2; 
            }
            else
                ViewBag.ThongBao = 3;
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            ViewBag.ThongBao = 0;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            ViewBag.ThongBao = 0;
            string username = f["username"].ToString();
            string password = Encryptor.MD5Hash(f["password"].ToString());
            var user = db.NGUOIDUNGs.SingleOrDefault(x => x.Username == username && x.Password == password);
            if(user!=null)
            {
                Session["TaiKhoan"] = user;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = 1;
            return View();
        }
    }
}