using DomZdravljaWebApp.Models;
using DomZdravljaWebApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravljaWebApp.Controllers
{
    public class TerminController : Controller
    {
        public ActionResult Zakazi()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            ViewBag.Lekari = Database.Lekari.ToList();
            ViewBag.Termini = Database.Termini.Where(x => !x.ZakazanTermin).ToList();
            return View(new TerminDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Zakazi(TerminDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Pacijent;
            ViewBag.Osoba = osoba;
            if (!ModelState.IsValid)
            {
                ViewBag.Lekari = Database.Lekari.ToList();
                ViewBag.Termini = Database.Termini.Where(x => !x.ZakazanTermin).ToList();
                return View(dto);
            }

            var termin = Database.Termini.FirstOrDefault(x => x.DatumIVremeTermina == dto.DatumIVremeTermina);

            if (termin == null)
            {
                ModelState.AddModelError("", "Termin does not exist.");
            }
            else if (termin.ZakazanTermin)
            {
                ModelState.AddModelError("", "Termin je vec zakazan.");
            }
            else
            {
                var lekar = Database.Lekari.FirstOrDefault(l => l.Id == termin.Lekar.Id);
                if (lekar != null)
                {
                    termin.Lekar = lekar;
                    termin.ZakazanTermin = true;


                    var existingTermin = lekar.ZakazaniSlobodniTermini
                        .FirstOrDefault(t => t.DatumIVremeTermina == termin.DatumIVremeTermina);

                    if (existingTermin != null)
                    {

                        existingTermin.ZakazanTermin = true;
                    }
                    else
                    {

                        lekar.ZakazaniSlobodniTermini.Add(termin);
                    }

                    osoba.ZakazaniTermini.Add(termin);

                    ZdravstveniKarton karton = Database.ZdravstveniKartoni.FirstOrDefault(x => x.Pacijent == osoba);

                    if (karton == null)
                    {
                        karton = new ZdravstveniKarton
                        {
                            Pacijent = osoba,
                            ListaTermina = new List<Termin>()
                        };
                        Database.ZdravstveniKartoni.Add(karton);
                    }

                    karton.ListaTermina.Add(termin);

                    Database.Save();

                    return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    ModelState.AddModelError("", "Lekar nije pronadjen");
                }
            }

            ViewBag.Lekari = Database.Lekari.ToList();
            ViewBag.Termini = Database.Termini.Where(x => !x.ZakazanTermin).ToList();
            return View(dto);
        }

        [HttpGet]
        public ActionResult Prikazi()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            var termini = Database.Termini.ToList();
            ViewBag.Termini = termini;
            return View(termini);
        }

        [HttpGet]
        public ActionResult Kreiraj()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kreiraj(KreiranjeTerminaDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Lekar;
            ViewBag.Osoba = osoba;
            Termin termin = new Termin { DatumIVremeTermina = dto.DatumIVremeTermina, Lekar = osoba, OpisTerapije = dto.OpisTerapije, ZakazanTermin = false };
            Database.Termini.Add(termin);
            osoba.ZakazaniSlobodniTermini.Add(termin);
            Database.Save();
            return RedirectToAction("Index", "HomePage");
        }
    }
}