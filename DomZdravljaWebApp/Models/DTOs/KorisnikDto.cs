using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class KorisnikDto
    {
        public string KorisnickoIme { get; set; }
        public Role Role { get; set; }
    }
}