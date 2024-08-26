using DomZdravljaWebApp.Models;
using DomZdravljaWebApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravljaWebApp.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {   
            Osoba korisnik = GetCurrent();
            if (korisnik is null)
                return View(new KorisnikDto { KorisnickoIme = string.Empty, Role = Role.NeprijavljenKorisnik });

            return View(new KorisnikDto { KorisnickoIme = korisnik.KorisnickoIme, Role = korisnik.Role });
        }

        [HttpPost]
        public ActionResult SignIn(LoginDto logger)
        {
            Osoba korisnik = Database.Korisnici.Find(x => x.KorisnickoIme == logger.KorisnickoIme && x.Sifra == logger.Lozinka);
            
            if (korisnik is null)
            {
                ModelState.AddModelError("", "Pogresno korisnicko ime ili lozinka.");
                return View("Index");
            }

            KorisnikDto loggedKorisnik = new KorisnikDto
            {
                Role = korisnik.Role,
                KorisnickoIme = korisnik.KorisnickoIme
            };

            HttpContext.Session["Korisnik"] = korisnik;
            return RedirectToAction("Index", "HomePage");
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            try
            {
                HttpContext.Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }
        public Osoba GetCurrent()
        {
            return HttpContext.Session["Korisnik"] as Osoba;
        }
    }
}