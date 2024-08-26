using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models
{
    public class Lekar : Osoba
    {
        public string Email { get; set; }
        public List<Termin> ZakazaniSlobodniTermini { get; set; }
    }
}