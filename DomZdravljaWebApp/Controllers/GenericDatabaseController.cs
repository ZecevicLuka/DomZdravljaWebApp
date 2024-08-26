using DomZdravljaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomZdravljaWebApp.Controllers
{
    public class GenericDatabaseController : Controller
    {
        protected void Save() => Database.Save();
        protected Lekar GetLekar(Osoba osoba)
        {
            return Database.Lekari.Find(x => x.Id == osoba.Id);
        }
        protected Pacijent GetPacijent(Osoba osoba)
        {
            return Database.Pacijenti.Find(x => x.Id == osoba.Id);
        }

    }
}