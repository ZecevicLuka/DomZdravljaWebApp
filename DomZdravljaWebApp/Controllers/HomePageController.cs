using DomZdravljaWebApp.Models;
using DomZdravljaWebApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DomZdravljaWebApp.Controllers
{
    public class HomePageController : Controller
    {
        public ActionResult Index()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            return View();
        }
    }
}