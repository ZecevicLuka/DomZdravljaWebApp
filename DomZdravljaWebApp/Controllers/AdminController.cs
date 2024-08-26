using DomZdravljaWebApp.Models;
using DomZdravljaWebApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DomZdravljaWebApp.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Kreiraj()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            return View();
        }

        [HttpPost]
        public ActionResult Kreiraj(PacijentDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            if (ModelState.IsValid)
            {
                if (Database.Pacijenti.Any(p => p.JMBG == dto.JMBG))
                {
                    ModelState.AddModelError("JMBG", "JMBG already exists.");
                }
                if (Database.Pacijenti.Any(p => p.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                }
                if (Database.Pacijenti.Any(p => p.KorisnickoIme == dto.KorisnickoIme))
                {
                    ModelState.AddModelError("KorisnickoIme", "Korisnicko Ime already exists.");
                }

                if (ModelState.IsValid)
                {
                    var pacijent = new Pacijent
                    {
                        JMBG = dto.JMBG,
                        Email = dto.Email,
                        Ime = dto.Ime,
                        Prezime = dto.Prezime,
                        DatumRodjenja = dto.DatumRodjenja,
                        KorisnickoIme = dto.KorisnickoIme,
                        Sifra = dto.Sifra,
                        ZakazaniTermini = new List<Termin>(),
                        Role = Role.Pacijent
                    };

                    Database.Pacijenti.Add(pacijent);
                    Database.Save();

                    return RedirectToAction("Index", "HomePage");
                }
            }

            return View(dto);
        }

        [HttpGet]
        public ActionResult Prikazi()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            ViewBag.Pacijenti = Database.Pacijenti.FindAll(p => p.JMBG != null);

            return View();
        }

        public ActionResult Obrisi()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            return View(new ObrisiPacijentaDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(ObrisiPacijentaDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            if (string.IsNullOrEmpty(dto.JMBG))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var pacijentToDelete = Database.Pacijenti.FirstOrDefault(p => p.JMBG == dto.JMBG);
            if (pacijentToDelete == null)
            {
                return HttpNotFound();
            }

            Database.Pacijenti.Remove(pacijentToDelete);
            Database.Save();

            return RedirectToAction("Index", "HomePage");
        }


        public ActionResult SelectPacijentToUpdate()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            var pacijenti = Database.Pacijenti.ToList();
            return View(pacijenti);
        }

        [HttpPost]
        public ActionResult SelectPacijentToUpdate(string jmbg)
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            var pacijent = Database.Pacijenti.FirstOrDefault(p => p.JMBG == jmbg);
            if (pacijent == null)
            {
                ModelState.AddModelError("JMBG", "Pacijent ne postoji");
                return View(Database.Pacijenti.ToList());
            }

            return RedirectToAction("Azuriraj", new { jmbg = jmbg });
        }

        [HttpGet]
        public ActionResult Azuriraj(string jmbg)
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            if (string.IsNullOrEmpty(jmbg))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var pacijent = Database.Pacijenti.FirstOrDefault(p => p.JMBG == jmbg);
            if (pacijent == null)
            {
                return HttpNotFound();
            }

            var dto = new UpdatePacijentDto
            {
                JMBG = pacijent.JMBG,
                Ime = pacijent.Ime,
                Prezime = pacijent.Prezime,
                Email = pacijent.Email,
                DatumRodjenja = pacijent.DatumRodjenja,
                Sifra = pacijent.Sifra
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(UpdatePacijentDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            if (ModelState.IsValid)
            {
                var pacijent = Database.Pacijenti.FirstOrDefault(p => p.JMBG == dto.JMBG);
                if (pacijent == null)
                {
                    return HttpNotFound();
                }

                if (Database.Pacijenti.Any(p => p.Email == dto.Email && p.JMBG != dto.JMBG))
                {
                    ModelState.AddModelError("Email", "Email vec postoji");
                    return View(dto);
                }

                pacijent.Ime = dto.Ime;
                pacijent.Prezime = dto.Prezime;
                pacijent.Email = dto.Email;
                pacijent.DatumRodjenja = dto.DatumRodjenja;
                pacijent.Sifra = dto.Sifra;

                Database.Save();

                return RedirectToAction("Index", "HomePage");
            }

            return View(dto);
        }


    }
}
