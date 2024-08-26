using DomZdravljaWebApp.Models;
using DomZdravljaWebApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DomZdravljaWebApp.Controllers
{
    public class TerapijaController : Controller
    {
        public ActionResult NapraviTerapiju()
        {
            var osoba = HttpContext.Session["Korisnik"] as Lekar;
            ViewBag.Osoba = osoba;
            var pacijentiLekara = Database.Pacijenti
                .Where(p => p.ZakazaniTermini.Any(t => t.Lekar.Id == osoba.Id))
                .ToList();

            ViewBag.Pacijenti = pacijentiLekara;

            return View();
        }

        [HttpPost]
        public ActionResult NapraviTerapiju(TerapijaDto dto)
        {
            var osoba = HttpContext.Session["Korisnik"] as Lekar;
            ViewBag.Osoba = osoba;

            var pacijent = Database.Pacijenti.FirstOrDefault(p => p.Id == dto.PacijentId);

            if (pacijent != null)
            {
                var karton = Database.ZdravstveniKartoni.FirstOrDefault(k => k.Pacijent.Id == pacijent.Id);
                if (karton == null)
                {
                    karton = new ZdravstveniKarton
                    {
                        Pacijent = pacijent,
                        ListaTermina = new List<Termin>()
                    };
                    Database.ZdravstveniKartoni.Add(karton);
                }

                var termin = karton.ListaTermina
                    .FirstOrDefault(t => t.Lekar.Id == osoba.Id && t.DatumIVremeTermina == dto.DatumIVremeTermina);

                if (termin != null)
                {
                    termin.OpisTerapije = dto.Terapija;

                    // Update the main database record
                    var terminIzBaze = Database.Termini
                        .FirstOrDefault(t => t.DatumIVremeTermina == termin.DatumIVremeTermina);

                    if (terminIzBaze != null)
                    {
                        terminIzBaze.OpisTerapije = dto.Terapija;
                    }

                    // Update the doctor's record
                    var terminOdLekara = osoba.ZakazaniSlobodniTermini
                        .FirstOrDefault(t => t.DatumIVremeTermina == termin.DatumIVremeTermina);

                    if (terminOdLekara != null)
                    {
                        terminOdLekara.OpisTerapije = dto.Terapija;
                    }

                    Database.Save();
                }
            }

            return RedirectToAction("Index", "HomePage");
        }

        [HttpGet]
        public JsonResult GetTermini(int pacijentId)
        {
            var osoba = HttpContext.Session["Korisnik"] as Lekar;
            var termini = Database.ZdravstveniKartoni
                .FirstOrDefault(k => k.Pacijent.Id == pacijentId)?
                .ListaTermina
                .Where(t => t.Lekar.Id == osoba.Id)
                .Select(t => new { DatumIVremeTermina = t.DatumIVremeTermina.ToString("yyyy-MM-ddTHH:mm") })
                .ToList();

            return Json(termini, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrikaziTerapije()
        {
            var osoba = HttpContext.Session["Korisnik"] as Osoba;
            ViewBag.Osoba = osoba;

            var termini = new Dictionary<Termin, Osoba>();

            if (osoba.Role == Role.Lekar)
            {
                Lekar lekar = (Lekar)osoba;

                var kartoni = Database.ZdravstveniKartoni
                    .Where(k => k.ListaTermina.Any(t => t.Lekar.KorisnickoIme == lekar.KorisnickoIme))
                    .ToList();

                foreach (var karton in kartoni)
                {
                    foreach (var termin in karton.ListaTermina.Where(t => t.Lekar.Id == lekar.Id && !string.IsNullOrEmpty(t.OpisTerapije)))
                    {
                        termini.Add(termin, karton.Pacijent);
                    }
                }
            }
            else if (osoba.Role == Role.Pacijent)
            {
                Pacijent pacijent = (Pacijent)osoba;
                var karton = Database.ZdravstveniKartoni
                    .FirstOrDefault(k => k.Pacijent.KorisnickoIme == pacijent.KorisnickoIme);

                if (karton != null)
                {
                    foreach (var termin in karton.ListaTermina.Where(t => !string.IsNullOrEmpty(t.OpisTerapije) && t.ZakazanTermin))
                    {
                        termini.Add(termin, termin.Lekar);
                    }
                }
            }

            ViewBag.Termini = termini;

            return View();
        }

    }
}
