using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models
{
    public class Termin
    {
        public Lekar Lekar { get; set; }
        public bool ZakazanTermin { get; set; }
        public DateTime DatumIVremeTermina { get; set; }
        public string OpisTerapije { get; set; }
    }
}