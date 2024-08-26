using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class LoginDto
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
    }
}