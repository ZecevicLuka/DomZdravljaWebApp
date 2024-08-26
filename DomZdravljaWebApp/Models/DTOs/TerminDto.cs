using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class TerminDto
    {
        public int LekarId { get; set; }
        public bool ZakazanTermin { get; set; }
        public DateTime DatumIVremeTermina { get; set; }
    }
}
