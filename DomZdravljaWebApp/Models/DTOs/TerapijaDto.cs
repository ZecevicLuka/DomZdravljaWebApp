using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class TerapijaDto
    {
        public int PacijentId { get; set; }
        public string Terapija { get; set; }
        public DateTime DatumIVremeTermina { get; set; }
    }
}